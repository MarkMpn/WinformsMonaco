using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LSP.Model
{
    public class CompletionParams
    {
        [JsonProperty("textDocument")]
        public TextDocumentIdentifier TextDocument { get; set; }

        [JsonProperty("position")]
        public Position Position { get; set; }

        [JsonProperty("context")]
        public CompletionContext Context { get; set; }
    }

    public class CompletionContext
    {
        [JsonProperty("triggerKind")]
        public int TriggerKind { get; set; }

        [JsonProperty("triggerCharacter")]
        public string TriggerCharacter { get; set; }
    }

    public class CompletionList
    {
        [JsonProperty("items")]
        public CompletionItem[] Items { get; set; }

        [JsonProperty("isIncomplete")]
        public bool IsIncomplete { get; set; } = false;
    }

    public class CompletionItem
    {
        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("kind")]
        public int? Kind { get; set; }

        [JsonProperty("detail")]
        public string Detail { get; set; }

        [JsonProperty("documentation")]
        public string Documentation { get; set; }

        [JsonProperty("insertText")]
        public string InsertText { get; set; }

        [JsonProperty("sortText")]
        public string SortText { get; set; }

        [JsonProperty("filterText")]
        public string FilterText { get; set; }
    }
}
