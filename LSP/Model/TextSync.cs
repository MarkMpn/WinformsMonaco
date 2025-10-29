using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LSP.Model
{
    public class DidChangeTextDocumentParams
    {
        [JsonProperty("textDocument")]
        public TextDocumentIdentifier TextDocument { get; set; }

        [JsonProperty("contentChanges")]
        public TextDocumentContentChangeEvent[] ContentChanges { get; set; }
    }

    public class TextDocumentContentChangeEvent
    {
        [JsonProperty("range")]
        public Range Range { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public class DidSaveTextDocumentParams
    {
        [JsonProperty("textDocument")]
        public TextDocumentIdentifier TextDocument { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public class DidCloseTextDocumentParams
    {
        [JsonProperty("textDocument")]
        public TextDocumentIdentifier TextDocument { get; set; }
    }
}
