using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LSP.Model
{
    public class InitializeParams
    {
        [JsonProperty("processId")]
        public int? ProcessId { get; set; }

        [JsonProperty("clientInfo")]
        public ClientInfo ClientInfo { get; set; }

        [JsonProperty("rootUri")]
        public string RootUri { get; set; }

        [JsonProperty("capabilities")]
        public ClientCapabilities Capabilities { get; set; }

        [JsonProperty("workspaceFolders")]
        public object WorkspaceFolders { get; set; }
    }

    public class ClientInfo
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }
    }

    public class ClientCapabilities
    {
        [JsonProperty("textDocument")]
        public TextDocumentClientCapabilities TextDocument { get; set; }

        [JsonProperty("workspace")]
        public WorkspaceClientCapabilities Workspace { get; set; }
    }

    public class TextDocumentClientCapabilities
    {
        [JsonProperty("completion")]
        public CompletionClientCapabilities Completion { get; set; }

        [JsonProperty("hover")]
        public HoverClientCapabilities Hover { get; set; }
    }

    public class CompletionClientCapabilities
    {
        [JsonProperty("completionItem")]
        public CompletionItemCapabilities CompletionItem { get; set; }
    }

    public class CompletionItemCapabilities
    {
        [JsonProperty("snippetSupport")]
        public bool SnippetSupport { get; set; }
    }

    public class HoverClientCapabilities { }

    public class WorkspaceClientCapabilities { }

    public class InitializeResult
    {
        [JsonProperty("capabilities")]
        public ServerCapabilities Capabilities { get; set; }
    }

    public class ServerCapabilities
    {
        [JsonProperty("textDocumentSync")]
        public int? TextDocumentSync { get; set; }

        [JsonProperty("completionProvider")]
        public CompletionOptions CompletionProvider { get; set; }

        [JsonProperty("hoverProvider")]
        public bool? HoverProvider { get; set; }

        [JsonProperty("documentFormattingProvider")]
        public bool DocumentFormattingProvider { get; set; }
    }

    public class CompletionOptions
    {
        [JsonProperty("triggerCharacters")]
        public string[] TriggerCharacters { get; set; }
    }
}
