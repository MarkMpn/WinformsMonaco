export const publishDiagnostics = function (params) {
    const { uri, diagnostics } = params;

    const model = monaco.editor.getModel(monaco.Uri.parse(uri));
    if (!model) return;

    monaco.editor.setModelMarkers(
        model,
        'lsp',
        diagnostics.map(d => ({
            startLineNumber: d.range.start.line + 1,
            startColumn: d.range.start.character + 1,
            endLineNumber: d.range.end.line + 1,
            endColumn: d.range.end.character + 1,
            message: d.message,
            severity: monaco.MarkerSeverity[d.severity] || monaco.MarkerSeverity.Info
        }))
    );
}