import { sendLspRequest, sendLspNotification } from "./core.js";
import { modelInitializers } from "../newModel.js";

export const registerLspHandlers = async function (editor, language) {

    // Initialize the LSP
    const initializeResult = await sendLspRequest("initialize", {
        processId: null,
        clientInfo: { name: "MarkMpn.WinformsMonaco", version: "1.0" },
        rootUri: null,
        capabilities: {
            textDocument: {
                completion: {
                    completionItem: {
                        snippetSupport: true
                    }
                },
                hover: {},
            },
            workspace: {}
        },
        workspaceFolders: null
    });

    // Register Monaco event handlers for each of the supported capabilities
    const triggerChars = initializeResult?.capabilities?.completionProvider?.triggerCharacters || [];

    if (triggerChars) {
        monaco.languages.registerCompletionItemProvider(language, {
            triggerCharacters: triggerChars,
            provideCompletionItems: async function (model, position, context, token) {
                const result = await sendLspRequest("textDocument/completion", {
                    textDocument: { uri: model.uri.toString() },
                    position: {
                        line: position.lineNumber - 1,
                        character: position.column - 1
                    },
                    context: {
                        triggerKind: context.triggerKind,
                        triggerCharacter: context.triggerCharacter
                    }
                });

                const items = result.items || [];

                return {
                    suggestions: items.map(item => {
                        return {
                            label: item.label,
                            kind: monaco.languages.CompletionItemKind[item.kind] || monaco.languages.CompletionItemKind.Text,
                            insertText: item.insertText || item.label,
                            documentation: item.documentation,
                            detail: item.detail,
                            sortText: item.sortText,
                            filterText: item.filterText,
                            range: undefined
                        };
                    })
                };
            }
        });
    }

    if (initializeResult?.capabilities?.hoverProvider) {
        monaco.languages.registerHoverProvider(language, {
            provideHover: async function (model, position, token) {
                const result = sendLspRequest("textDocument/hover", {
                    textDocument: { uri: model.uri.toString() },
                    position: {
                        line: position.lineNumber - 1,
                        character: position.column - 1
                    }
                });

                if (!result || !result.contents)
                    return null;

                return {
                    contents: Array.isArray(result.contents)
                        ? result.contents.map(c => ({ value: typeof c === 'string' ? c : c.value }))
                        : [{ value: typeof result.contents === 'string' ? result.contents : result.contents.value }]
                };
            }
        });
    }

    if (initializeResult?.capabilities?.documentFormattingProvider) {
        monaco.languages.registerDocumentFormattingEditProvider(language, {
            provideDocumentFormattingEdits: async function (model, options, token) {
                const result = await sendLspRequest("textDocument/formatting", {
                    textDocument: { uri: model.uri.toString() },
                    options: {
                        tabSize: options.tabSize,
                        insertSpaces: options.insertSpaces
                    }
                });

                return result.map(edit => {
                    return {
                        range: new monaco.Range(
                            edit.range.start.line + 1,
                            edit.range.start.character + 1,
                            edit.range.end.line + 1,
                            edit.range.end.character + 1),
                        text: edit.newText
                    };
                });
            }
        });
    }

    if (initializeResult?.capabilities?.textDocumentSync) {
        const contentChangeHandler = async (e) => {
            if (initializeResult?.capabilities?.textDocumentSync?.change === 1) {
                // Full sync
                await sendLspNotification("textDocument/didChange", {
                    textDocument: {
                        uri: editor.getModel().uri.toString(),
                        version: editor.getModel().getVersionId()
                    },
                    contentChanges: [
                        {
                            text: editor.getModel().getValue()
                        }
                    ]
                });
            } else if (initializeResult?.capabilities?.textDocumentSync?.change === 2) {
                // Incremental sync
                await sendLspNotification("textDocument/didChange", {
                    textDocument: {
                        uri: editor.getModel().uri.toString(),
                        version: editor.getModel().getVersionId()
                    },
                    contentChanges: e.changes.map(change => ({
                        range: {
                            start: { line: change.range.startLineNumber - 1, character: change.range.startColumn - 1 },
                            end: { line: change.range.endLineNumber - 1, character: change.range.endColumn - 1 }
                        },
                        text: change.text
                    }))
                });
            }
        };

        // Add this handler to the current model, and register for future models
        editor.getModel().onDidChangeContent(contentChangeHandler);
        modelInitializers.push(model => {
            model.onDidChangeContent(contentChangeHandler);
        });

        if (initializeResult?.capabilities?.textDocumentSync?.openClose) {
            await sendLspNotification("textDocument/didOpen", {
                textDocument: {
                    uri: editor.getModel().uri.toString(),
                    languageId: language,
                    version: editor.getModel().getVersionId(),
                    text: editor.getModel().getValue()
                }
            });
        }

        // Send the initial open notification
        await sendLspNotification("textDocument/didOpen", {
            textDocument: {
                uri: editor.getModel().uri.toString(),
                languageId: language,
                version: editor.getModel().getVersionId(),
                text: editor.getModel().getValue()
            }
        });
    }

    await sendLspNotification("initialized", {});
    await sendLspNotification("workspace/didChangeConfiguration", { settings: {} });
};