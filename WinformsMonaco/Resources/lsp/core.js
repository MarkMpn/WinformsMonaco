export const sendLspRequest = async function (method, params) {
    const request = {
        "jsonrpc": "2.0",
        "method": method,
        "params": params
    };

    const resultJson = await window.chrome.webview.hostObjects.lspTransport.SendRequest(JSON.stringify(request));
    return JSON.parse(resultJson);
};

export const sendLspNotification = async function (method, params) {
    const request = {
        "jsonrpc": "2.0",
        "method": method,
        "params": params
    };

    await window.chrome.webview.hostObjects.lspTransport.SendNotification(JSON.stringify(request));
};