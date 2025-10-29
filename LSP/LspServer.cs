using LSP.Model;
using StreamJsonRpc;

namespace LSP
{
    public class LspServer
    {
        private readonly JsonRpc _jsonRpc;

        public LspServer(Stream serverStream)
        {
            _jsonRpc = new JsonRpc(serverStream, serverStream);
            _jsonRpc.AddLocalRpcTarget(this);
            _jsonRpc.StartListening();

            _jsonRpc.TraceSource.Switch.Level = System.Diagnostics.SourceLevels.All;
            _jsonRpc.TraceSource.Listeners.Add(new System.Diagnostics.ConsoleTraceListener());
        }

        [JsonRpcMethod("initialize", UseSingleObjectParameterDeserialization = true)]
        public Task<InitializeResult> InitializeAsync(InitializeParams @params)
        {
            return Task.FromResult(new InitializeResult
            {
                Capabilities = new ServerCapabilities
                {
                    TextDocumentSync = 1,
                    CompletionProvider = new CompletionOptions { TriggerCharacters = new[] { ".", "\"" } },
                    HoverProvider = true
                }
            });
        }

        [JsonRpcMethod("textDocument/completion", UseSingleObjectParameterDeserialization = true)]
        public Task<CompletionList> CompletionAsync(CompletionParams @params)
        {
            var items = new [] {
                new CompletionItem {
                    Label = "console",
                    Kind = 14, // Keyword
                    InsertText = "console"
                },
                new CompletionItem {
                    Label = "log",
                    Kind = 3, // Function
                    InsertText = "log()"
                }
            };

            SendDiagnostics(@params.TextDocument.Uri);

            return Task.FromResult(new CompletionList { Items = items });
        }

        [JsonRpcMethod("textDocument/hover", UseSingleObjectParameterDeserialization = true)]
        public Task<HoverResult> HoverAsync(HoverParams @params)
        {
            return Task.FromResult(new HoverResult
            {
                Contents = new[] {
                    new MarkedString { Language = "javascript", Value = "console.log()" },
                }
            });
        }

        public void SendDiagnostics(string uri)
        {
            _jsonRpc.NotifyAsync("textDocument/publishDiagnostics", new PublishDiagnosticsParams
            {
                Uri = uri,
                Diagnostics = new[] {
                    new Diagnostic {
                        Severity = DiagnosticSeverity.Error,
                        Message = "Unexpected token",
                        Range = new Model.Range {
                            Start = new Position { Line = 0, Character = 5 },
                            End = new Position { Line = 0, Character = 10 }
                        }
                    }
                }
            });
        }
    }
}
