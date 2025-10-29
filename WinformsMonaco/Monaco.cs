using System.Reflection;
using System.Text.Json;
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
            Dock = DockStyle.Fill
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

            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("WinformsMonaco.Resources.monaco.html"))
            using (var reader = new StreamReader(stream))
            {
                var html = reader.ReadToEnd();
                html = html.Replace("#backcolor", $"#{BackColor.R:X2}{BackColor.G:X2}{BackColor.B:X2}");
                html = html.Replace("#loadingfontsize", $"{Font.Size}pt");
                html = html.Replace("#loadingfont", $"'{Font.Name}'");

#if DEBUG
                var url = Path.ChangeExtension(Path.GetTempFileName(), ".html");
                File.WriteAllText(url, html);
                _webView.CoreWebView2.Navigate(url);
#else
                _webView.NavigateToString(html);
#endif
            }
        };

        _webView.NavigationCompleted += (s, e) =>
        {
            _navigationCompleted = true;
        };

        _ = _webView.EnsureCoreWebView2Async();
    }

    public override string Text
    {
        get
        {
            return _text ?? JsonSerializer.Deserialize<string>(_webView.ExecuteScriptAsync("editor.getValue()").ExecuteSync()) ?? string.Empty;
        }
        set
        {
            if (_ready)
                _webView.ExecuteScriptAsync($"editor.setValue({JsonSerializer.Serialize(value)});").ExecuteSync();
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
            return JsonSerializer.Deserialize<string>(_webView.ExecuteScriptAsync("editor.getModel().getValueInRange(editor.getSelection())").ExecuteSync()) ?? string.Empty;
        }
    }

    public string Language
    {
        get => _language ?? string.Empty;
        set
        {
            _language = value;

            if (_ready)
                _webView.ExecuteScriptAsync($"monaco.editor.setModelLanguage(editor.getModel(), {JsonSerializer.Serialize(value)});").ExecuteSync();
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
