using System.Reflection;
using System.Text.Json;
using Microsoft.Web.WebView2.WinForms;

namespace WinformsMonaco;

public class Monaco : Control
{
    private readonly WebView2 _webView;
    private readonly MonacoBridge _bridge;

    private bool _ready;
    private bool _navigationCompleted;
    private string? _text;
    private string? _language;

    public Monaco()
    {
        _webView = new WebView2
        {
            Dock = DockStyle.Fill
        };
        Controls.Add(_webView);

        _bridge = new MonacoBridge(this);

        _webView.CoreWebView2InitializationCompleted += (s, e) =>
        {
            if (!e.IsSuccess)
                throw e.InitializationException;

            if (_ready)
                return;

            _ready = true;

            _webView.CoreWebView2.AddHostObjectToScript("monacoBridge", _bridge);

            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("WinformsMonaco.Resources.monaco.html"))
            using (var reader = new StreamReader(stream))
            {
                var html = reader.ReadToEnd();
                html = html.Replace("#backcolor", $"#{BackColor.R:X2}{BackColor.G:X2}{BackColor.B:X2}");
                html = html.Replace("#loadingfont", $"'{Font.Name}'");
                html = html.Replace("#loadingfontsize", $"{Font.Size}pt");
                _webView.NavigateToString(html);
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
}
