using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LSP.Model
{
    public class HoverParams
    {
        [JsonProperty("textDocument")]
        public TextDocumentIdentifier TextDocument { get; set; }

        [JsonProperty("position")]
        public Position Position { get; set; }
    }

    public class HoverResult
    {
        [JsonProperty("contents")]
        public MarkedString[] Contents { get; set; }

        [JsonProperty("range")]
        public Range Range { get; set; }
    }

    public class MarkedString
    {
        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
