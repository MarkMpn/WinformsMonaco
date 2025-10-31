import { initMonaco } from "./init.js";
import { newModel } from "./newModel.js";
import { registerLspHandlers } from "./lsp/registerLsp.js";
import { publishDiagnostics } from "./lsp/textDocument/publishDiagnostics.js";

require(['vs/editor/editor.main'], async function () {
    const editor = await initMonaco();

    window.createNewModel = async function (uri, language, content) {
        await newModel(editor, uri, language, content);
    };

    window.setLanguage = function (language) {
        monaco.editor.setModelLanguage(editor.getModel(), language);
    }

    window.getSelectedText = function () {
        return editor.getModel().getValueInRange(editor.getSelection());
    }

    window.getValue = function () {
        return editor.getValue();
    }

    window.setValue = function (value) {
        editor.setValue(value);
    }

    window.registerLsp = async function (language) {
        await registerLspHandlers(editor, language);
    };

    window.showMinimap = function(show) {
        editor.updateOptions({ minimap: { enabled: show } });
    };

    window.lspClient = {
        handleNotification: function (method, params) {
            if (method === "textDocument/publishDiagnostics") {
                publishDiagnostics(params);
            }
        }
    };
});