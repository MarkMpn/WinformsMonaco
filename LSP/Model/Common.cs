using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LSP.Model
{
    public class Position
    {
        [JsonProperty("line")]
        public int Line { get; set; }

        [JsonProperty("character")]
        public int Character { get; set; }
    }

    public class Range
    {
        [JsonProperty("start")]
        public Position Start { get; set; }

        [JsonProperty("end")]
        public Position End { get; set; }
    }

    public class TextDocumentIdentifier
    {
        [JsonProperty("uri")]
        public string Uri { get; set; }
    }
}
