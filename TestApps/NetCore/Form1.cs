using LSP;
using Nerdbank.Streams;
using MarkMpn.WinformsMonaco;
using StreamJsonRpc;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace NetCore
{
    public partial class Form1 : Form
    {
        private readonly Monaco _monaco;
        private JsonRpc _rpcClient;

        public Form1()
        {
            InitializeComponent();

            _monaco = new Monaco
            {
                Dock = DockStyle.Fill,
                Text = "// Write your code here...\nfunction hello() {\n    console.log(\"Hello, Monaco Editor!\");\n}\n",
                Language = "javascript"
            };
            bodyPanel.Controls.Add(_monaco);

            showMinimapCheckBox.Checked = _monaco.MinimapVisible;
        }

        private void setTextButton_Click(object sender, EventArgs e)
        {
            _monaco.Text = "SELECT * FROM account";
        }

        private void getTextButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show(_monaco.Text + "\r\n\r\nSelected: " + _monaco.SelectedText);
        }

        private void setLangButton_Click(object sender, EventArgs e)
        {
            _monaco.Language = "sql";
        }

        private void addLspButton_Click(object sender, EventArgs e)
        {
            var psi = new ProcessStartInfo(@"C:\Users\mark_\Downloads\copilot-language-server.exe");
            psi.ArgumentList.Add("--stdio");
            psi.RedirectStandardInput = true;
            psi.RedirectStandardOutput = true;
            var process = Process.Start(psi);
            /*var (clientStream, serverStream) = FullDuplexStream.CreatePair();
            var rpcServer = new LspServer(serverStream);
            var rpcClient = new JsonRpc(clientStream);*/
            _rpcClient = new JsonRpc(process.StandardInput.BaseStream, process.StandardOutput.BaseStream);
            _rpcClient.TraceSource.Switch.Level = SourceLevels.All;
            _rpcClient.TraceSource.Listeners.Add(new ConsoleTraceListener());

            _rpcClient.AddLocalRpcTarget(new LspHandlers(this));

            _monaco.RegisterLSPClient(_monaco.Language, _rpcClient);
        }

        private void showMinimapCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _monaco.MinimapVisible = showMinimapCheckBox.Checked;
        }

        private void InvokeIfRequired(Action action)
        {
            if (InvokeRequired)
                Invoke(action);
            else
                action();
        }

        private async void authButton_Click(object sender, EventArgs e)
        {
            var response = await _rpcClient.InvokeAsync<AuthResponse>("signIn", new { });
        }

        class AuthResponse
        {
            [JsonPropertyName("userCode")]
            public string UserCode { get; set; }
            [JsonPropertyName("command")]
            public AuthCommand Command { get; set; }
        }

        class AuthCommand
        {
            [JsonPropertyName("command")]
            public string Command { get; set; }
            [JsonPropertyName("arguments")]
            public string[] Arguments { get; set; }
            [JsonPropertyName("title")]
            public string Title { get; set; }
        }

        public class LspHandlers
        {
            private readonly Form1 _form;

            public LspHandlers(Form1 form)
            {
                _form = form;
            }

            [JsonRpcMethod("window/logMessage", UseSingleObjectParameterDeserialization = true)]
            public void HandleLogMessage(LogMessageParams param)
            {
                _form.lspLogTextBox.Text += $"[{param.Type}] {param.Message}{Environment.NewLine}";
            }

            [JsonRpcMethod("didChangeStatus", UseSingleObjectParameterDeserialization = true)]
            public void ChangeStatus(ChangeStatusParams param)
            {
                _form.statusLabel.Text = $"[{param.Kind}] {param.Message}";
                _form.statusLabel.Visible = true;
            }

            public class LogMessageParams
            {
                public MessageType Type { get; set; }
                public string Message { get; set; }
            }

            public enum MessageType
            {
                Error = 1,
                Warning = 2,
                Info = 3,
                Log = 4,
                Debug = 5,
            }

            public class ChangeStatusParams
            {
                public string Kind { get; set; }
                public string Message { get; set; }
            }
        }
    }
}
