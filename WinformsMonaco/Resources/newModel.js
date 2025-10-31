import { sendLspNotification } from "./lsp/core.js";

export const modelInitializers = [];

export const newModel = async function (editor, uri, language, content) {
    const newModel = monaco.editor.createModel(content, language, monaco.Uri.parse(uri));
    editor.setModel(newModel);

    for (let i = 0; i < modelInitializers.length; i++)
        modelInitializers[i](newModel);

    await sendLspNotification("textDocument/didOpen", {
        textDocument: {
            uri: uri,
            languageId: language,
            version: newModel.getVersionId(),
            text: content
        }
    });
}