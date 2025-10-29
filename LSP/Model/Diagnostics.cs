using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LSP.Model
{
    public class PublishDiagnosticsParams
    {
        [JsonProperty("uri")]
        public string Uri { get; set; }

        [JsonProperty("diagnostics")]
        public Diagnostic[] Diagnostics { get; set; }
    }

    public class Diagnostic
    {
        [JsonProperty("range")]
        public Range Range { get; set; }

        [JsonProperty("severity")]
        public DiagnosticSeverity Severity { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("tags")]
        public DiagnosticTag[] Tags { get; set; }
    }

    public enum DiagnosticSeverity
    {
        Error = 1,
        Warning = 2,
        Information = 3,
        Hint = 4
    }

    public enum DiagnosticTag
    {
        Unnecessary = 1,
        Deprecated = 2
    }
}
