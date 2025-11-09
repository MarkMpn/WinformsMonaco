using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using Newtonsoft.Json.Linq;
using StreamJsonRpc;

namespace MarkMpn.WinformsMonaco;

/// <summary>
/// Hosts the Monaco text editor control in a Windows Forms control
/// </summary>
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
    private bool _minimapVisible = true;

    /// <summary>
    /// Creates a new Monaco text editor control
    /// </summary>
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

        var resourceName = e.Request.Uri.Replace("monaco://monaco/", "MarkMpn.WinformsMonaco.Resources.").Replace('/', '.');

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

    /// <summary>
    /// Returns or sets the current text in the editor
    /// </summary>
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

    /// <summary>
    /// Returns the currently selected text in the editor
    /// </summary>
    public string SelectedText
    {
        get
        {
            return JsonSerializer.Deserialize<string>(_webView.ExecuteScriptAsync("getSelectedText()").ExecuteSync()) ?? string.Empty;
        }
    }

    /// <summary>
    /// Returns or sets the language the editor uses for features such as syntax highlighting
    /// </summary>
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

    /// <summary>
    /// Returns or sets the URI that identifies the document in the editor
    /// </summary>
    /// <remarks>
    /// This value is used by language servers to identify the document.
    /// </remarks>
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

    /// <summary>
    /// Shows or hides the minimap view on the right hand side of the editor
    /// </summary>
    public bool MinimapVisible
    {
        get => _minimapVisible;
        set
        {
            _minimapVisible = value;

            if (_ready)
                _webView.ExecuteScriptAsync($"showMinimap({JsonSerializer.Serialize(value)});").ExecuteSync();
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

    /// <summary>
    /// Adds a Language Server Protocol client to provide additional language features to the editor.
    /// </summary>
    /// <param name="language">The language that the LSP client should be used for</param>
    /// <param name="sendingStream">The stream used to transmit messages to the LSP server</param>
    /// <param name="receivingStream">The stream used to receive messages from the LSP server</param>
    public void RegisterLSPClient(string language, Stream sendingStream, Stream receivingStream)
    {
        var rpcClient = new JsonRpc(sendingStream, receivingStream);

        // Forward any notifications on to the javascript
        rpcClient.AddLocalRpcMethod("textDocument/publishDiagnostics", (Action<JObject>)((parameters) =>
        {
            InvokeIfRequired(() =>
            {
                _webView.ExecuteScriptAsync($"lspClient.handleNotification('textDocument/publishDiagnostics', {parameters.ToString()})");
            });
        }));

        rpcClient.StartListening();

        _lspTransport.RegisterLSPClient(language, rpcClient);
        _webView.ExecuteScriptAsync($"registerLsp({JsonSerializer.Serialize(language)});").ExecuteSync();
        _webView.GotFocus += (s, e) =>
        {
            _lspTransport.SendNotificationInternal("textDocument/didFocus", new { textDocument = new { uri = Uri } }).ExecuteSync();
        };
    }

    protected override void Dispose(bool disposing)
    {
        _lspTransport.SendNotificationInternal("textDocument/didClose", new { textDocument = new { uri = Uri } }).ExecuteSync();
        base.Dispose(disposing);
    }

    private void InvokeIfRequired(Action action)
    {
        if (InvokeRequired)
            Invoke(action);
        else
            action();
    }
}
