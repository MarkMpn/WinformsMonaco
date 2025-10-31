export async function initMonaco() {
    // Get the value and language that have been set in .NET before the editor has loaded
    const value = await window.chrome.webview.hostObjects.monacoBridge.GetInitValue();
    const language = await window.chrome.webview.hostObjects.monacoBridge.GetLanguage();
    const uri = await window.chrome.webview.hostObjects.monacoBridge.GetUri();
    const minimapVisible = await window.chrome.webview.hostObjects.monacoBridge.GetMinimapVisible();

    const model = monaco.editor.createModel(value, language, monaco.Uri.parse(uri));

    const editor = monaco.editor.create(document.getElementById('container'), {
        model: model,
        minimap: {
            enabled: minimapVisible
        }
    });

    editor.onDidChangeModelLanguageConfiguration(() => {
        // Editor has loaded so remove loading notification
        document.getElementById('container').classList.remove('loading');
        const loading = document.getElementById('loading');
        loading.parentElement.removeChild(loading);
    });

    return editor;
}