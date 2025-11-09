using LSP;
using Nerdbank.Streams;
using MarkMpn.WinformsMonaco;
using StreamJsonRpc;

namespace NetCore
{
    public partial class Form1 : Form
    {
        private readonly Monaco _monaco;

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
            var (clientStream, serverStream) = FullDuplexStream.CreatePair();
            var rpcServer = new LspServer(serverStream);
            var rpcClient = new JsonRpc(clientStream);
            _monaco.RegisterLSPClient(_monaco.Language, rpcClient);
        }

        private void showMinimapCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _monaco.MinimapVisible = showMinimapCheckBox.Checked;
        }
    }
}
