using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LSP.Model
{
    public class DocumentFormattingParams
    {
        [JsonProperty("textDocument")]
        public TextDocumentIdentifier TextDocument { get; set; }

        [JsonProperty("options")]
        public FormattingOptions Options { get; set; }
    }

    public class FormattingOptions
    {
        [JsonProperty("tabSize")]
        public int TabSize { get; set; }

        [JsonProperty("insertSpaces")]
        public bool InsertSpaces { get; set; }
    }

    public class TextEdit
    {
        [JsonProperty("range")]
        public Range Range { get; set; }

        [JsonProperty("newText")]
        public string NewText { get; set; }
    }
}
