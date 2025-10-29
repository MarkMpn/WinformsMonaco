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
                    TextDocumentSync = new TextDocumentSyncOptions
                    {
                        OpenClose = true,
                        Change = 1,
                        Save = new SaveOptions { IncludeText = true }
                    },
                    CompletionProvider = new CompletionOptions { TriggerCharacters = new[] { ".", "\"" } },
                    HoverProvider = true,
                    DocumentFormattingProvider = true,
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

        [JsonRpcMethod("textDocument/formatting", UseSingleObjectParameterDeserialization = true)]
        public Task<TextEdit[]> FormatAsync(DocumentFormattingParams @params)
        {
            var edits = new[]
            {
                new TextEdit
                {
                    Range = new Model.Range
                    {
                        Start = new Position { Line = 0, Character = 0 },
                        End = new Position { Line = 0, Character = 1000 }
                    },
                    NewText = "// formatted output\n" + "// " + DateTime.UtcNow
                }
            };

            return Task.FromResult(edits);
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

        [JsonRpcMethod("textDocument/didChange", UseSingleObjectParameterDeserialization = true)]
        public Task DidChangeAsync(DidChangeTextDocumentParams @params)
        {
            SendDiagnostics(@params.TextDocument.Uri);

            // Update internal state, re-run diagnostics, etc.
            return Task.CompletedTask;
        }

        [JsonRpcMethod("textDocument/didSave", UseSingleObjectParameterDeserialization = true)]
        public Task DidSaveAsync(DidSaveTextDocumentParams @params)
        {
            // Trigger formatting, linting, etc.
            return Task.CompletedTask;
        }

        [JsonRpcMethod("textDocument/didClose", UseSingleObjectParameterDeserialization = true)]
        public Task DidCloseAsync(DidCloseTextDocumentParams @params)
        {
            // Clean up resources or diagnostics
            return Task.CompletedTask;
        }
    }
}
