using System.ComponentModel;
using System.Runtime.InteropServices;

namespace MarkMpn.WinformsMonaco;

[ClassInterface(ClassInterfaceType.AutoDual)]
[ComVisible(true)]
[Browsable(false)]
public class MonacoBridge
{
    private readonly Monaco _monaco;

    public MonacoBridge(Monaco monaco)
    {
        _monaco = monaco;
    }

    public string GetInitValue()
    {
        return _monaco.GetInitValue();
    }

    public string GetLanguage()
    {
        return _monaco.Language;
    }

    public string GetUri()
    {
        return _monaco.Uri;
    }

    public bool GetMinimapVisible()
    {
        return _monaco.MinimapVisible;
    }
}
