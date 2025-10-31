using System.Reflection;
using System.Text;
using System.Text.Json;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using Newtonsoft.Json.Linq;
using StreamJsonRpc;

namespace WinformsMonaco;

public class Monaco : Control
{
    private readonly WebView2 _webView;
    private readonly MonacoBridge _bridge;
    private readonly LspTransport _lspTransport;

    private bool _ready;
    private bool _navigationCompleted;
    private string? _text;
    private string? _language;
    private string? _uri;

    public Monaco()
    {
        _webView = new WebView2
        {
            Dock = DockStyle.Fill,
        };
        Controls.Add(_webView);

        _bridge = new MonacoBridge(this);
        _lspTransport = new LspTransport(this);

        _webView.CoreWebView2InitializationCompleted += (s, e) =>
        {
            if (!e.IsSuccess)
                throw e.InitializationException;

            if (_ready)
                return;

            _ready = true;

            _webView.CoreWebView2.AddHostObjectToScript("monacoBridge", _bridge);
            _webView.CoreWebView2.AddHostObjectToScript("lspTransport", _lspTransport);

            _webView.CoreWebView2.AddWebResourceRequestedFilter("monaco://monaco/*", CoreWebView2WebResourceContext.All, CoreWebView2WebResourceRequestSourceKinds.All);
            _webView.CoreWebView2.WebResourceRequested += LoadResourceFile;

            _webView.CoreWebView2.Navigate("monaco://monaco/index.html");
        };

        _webView.NavigationCompleted += (s, e) =>
        {
            _navigationCompleted = true;
        };

        var customSchemeRegistrations = new List<CoreWebView2CustomSchemeRegistration>
        {
            new CoreWebView2CustomSchemeRegistration("monaco")
            {
                AllowedOrigins = { "*" },
                TreatAsSecure = true,
                HasAuthorityComponent = true,
            }
        };
        var envOptions = new CoreWebView2EnvironmentOptions(customSchemeRegistrations: customSchemeRegistrations);
        var env = CoreWebView2Environment.CreateAsync(null, null, envOptions).ConfigureAwait(false).GetAwaiter().GetResult();

        _ = _webView.EnsureCoreWebView2Async(env).ConfigureAwait(false);
    }

    private void LoadResourceFile(object? sender, CoreWebView2WebResourceRequestedEventArgs e)
    {
        if (!e.Request.Uri.StartsWith("monaco://monaco/"))
            return;

        var resourceName = e.Request.Uri.Replace("monaco://monaco/", "WinformsMonaco.Resources.").Replace('/', '.');

        // Don't dispose of the stream - it needs to be open after this method returns so the data
        // is available to the WebView2 control to return the response.
        var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
        
        if (stream == null)
            return;

        if (e.Request.Uri == "monaco://monaco/index.html")
        {
            using (var reader = new StreamReader(stream))
            {
                var content = reader.ReadToEnd();

                content = content.Replace("#backcolor", $"#{BackColor.R:X2}{BackColor.G:X2}{BackColor.B:X2}");
                content = content.Replace("#loadingfontsize", $"{Font.Size}pt");
                content = content.Replace("#loadingfont", $"'{Font.Name}'");

                var response = _webView.CoreWebView2.Environment.CreateWebResourceResponse(
                    new MemoryStream(Encoding.UTF8.GetBytes(content)),
                    200,
                    "OK",
                    "Content-Type: text/html");
                e.Response = response;
            }
        }
        else
        {
            var response = _webView.CoreWebView2.Environment.CreateWebResourceResponse(
                stream,
                200,
                "OK",
                "Content-Type: application/javascript");
            e.Response = response;
        }
    }

    public override string Text
    {
        get
        {
            return _text ?? JsonSerializer.Deserialize<string>(_webView.ExecuteScriptAsync("getValue()").ExecuteSync()) ?? string.Empty;
        }
        set
        {
            if (_ready)
                _webView.ExecuteScriptAsync($"setValue({JsonSerializer.Serialize(value)});").ExecuteSync();
            else
                _text = value;
        }
    }

    internal string GetInitValue()
    {
        var value = _text;
        _text = null;
        return value;
    }

    public string SelectedText
    {
        get
        {
            return JsonSerializer.Deserialize<string>(_webView.ExecuteScriptAsync("getSelectedText()").ExecuteSync()) ?? string.Empty;
        }
    }

    public string Language
    {
        get => _language ?? string.Empty;
        set
        {
            _language = value;

            if (_ready)
                _webView.ExecuteScriptAsync($"setLanguage({JsonSerializer.Serialize(value)});").ExecuteSync();
        }
    }

    public string Uri
    {
        get => _uri ?? $"inmemory://model/{GetHashCode()}.{Language}";
        set
        {
            _uri = value;

            if (_ready)
                _webView.ExecuteScriptAsync($"newModel({JsonSerializer.Serialize(_uri)}, {JsonSerializer.Serialize(Language)}, {JsonSerializer.Serialize(Text)});").ExecuteSync();
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        if (_ready && _navigationCompleted)
        {
            base.OnPaint(e);
            return;
        }

        using var brush = new SolidBrush(BackColor);
        e.Graphics.FillRectangle(brush, e.ClipRectangle);
        var sf = new StringFormat
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };
        e.Graphics.DrawString("Loading Monaco Editor...", Font, SystemBrushes.ControlText, e.ClipRectangle, sf);
    }

    public void RegisterLanguageServerProvider(string language, Stream clientStream)
    {
        var rpcClient = new JsonRpc(clientStream, clientStream);

        // Forward any notifications on to the javascript
        rpcClient.AddLocalRpcMethod("textDocument/publishDiagnostics", (Action<JObject>)((parameters) =>
        {
            InvokeIfRequired(() =>
            {
                _webView.ExecuteScriptAsync($"lspClient.handleNotification('textDocument/publishDiagnostics', {parameters.ToString()})");
            });
        }));

        rpcClient.StartListening();

        rpcClient.TraceSource.Switch.Level = System.Diagnostics.SourceLevels.All;
        rpcClient.TraceSource.Listeners.Add(new System.Diagnostics.ConsoleTraceListener());

        _lspTransport.RegisterLanguageServerProvider(language, rpcClient);
        _webView.ExecuteScriptAsync($"registerLsp({JsonSerializer.Serialize(language)});").ExecuteSync();
    }

    private void InvokeIfRequired(Action action)
    {
        if (InvokeRequired)
            Invoke(action);
        else
            action();
    }
}
