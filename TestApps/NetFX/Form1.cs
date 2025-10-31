using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MarkMpn.WinformsMonaco;

namespace NetFX
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
    }
}
