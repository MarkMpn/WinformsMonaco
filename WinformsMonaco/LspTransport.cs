using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StreamJsonRpc;
using StreamJsonRpc.Protocol;

namespace MarkMpn.WinformsMonaco;

[ClassInterface(ClassInterfaceType.AutoDual)]
[ComVisible(true)]
[Browsable(false)]
public class LspTransport
{
    private readonly Monaco _monaco;
    private readonly Dictionary<string, JsonRpc> _languageClients;

    public LspTransport(Monaco monaco)
    {
        _monaco = monaco;
        _languageClients = new Dictionary<string, JsonRpc>();
    }

    internal void RegisterLSPClient(string language, JsonRpc rpcClient)
    {
        _languageClients[language] = rpcClient;
    }

    public async Task<string> SendRequest(string jsonRpcMessage)
    {
        var language = _monaco.Language;
        if (!_languageClients.TryGetValue(language, out var rpcClient))
        {
            throw new InvalidOperationException($"No LSP server registered for language '{language}'.");
        }

        var message = JsonConvert.DeserializeObject<JsonRpcRequest>(jsonRpcMessage);
        var response = await rpcClient.InvokeWithParameterObjectAsync<object>(message.Method, message.Arguments).ConfigureAwait(false);
        return JsonConvert.SerializeObject(response);
    }

    public async Task SendNotification(string jsonRpcMessage)
    {
        var language = _monaco.Language;
        if (!_languageClients.TryGetValue(language, out var rpcClient))
        {
            throw new InvalidOperationException($"No LSP server registered for language '{language}'.");
        }

        var message = JsonConvert.DeserializeObject<JsonRpcRequest>(jsonRpcMessage);
        await rpcClient.NotifyWithParameterObjectAsync(message.Method, message.Arguments).ConfigureAwait(false);
    }
}
