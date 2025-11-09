namespace NetCore
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            topPanel = new Panel();
            authButton = new Button();
            showMinimapCheckBox = new CheckBox();
            addLspButton = new Button();
            setTextButton = new Button();
            getTextButton = new Button();
            setLangButton = new Button();
            statusStrip1 = new StatusStrip();
            statusLabel = new ToolStripStatusLabel();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            bodyPanel = new Panel();
            tabPage2 = new TabPage();
            lspLogTextBox = new TextBox();
            topPanel.SuspendLayout();
            statusStrip1.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            SuspendLayout();
            // 
            // topPanel
            // 
            topPanel.Controls.Add(authButton);
            topPanel.Controls.Add(showMinimapCheckBox);
            topPanel.Controls.Add(addLspButton);
            topPanel.Controls.Add(setTextButton);
            topPanel.Controls.Add(getTextButton);
            topPanel.Controls.Add(setLangButton);
            topPanel.Dock = DockStyle.Top;
            topPanel.Location = new Point(0, 0);
            topPanel.Name = "topPanel";
            topPanel.Size = new Size(800, 73);
            topPanel.TabIndex = 0;
            // 
            // authButton
            // 
            authButton.Location = new Point(336, 12);
            authButton.Name = "authButton";
            authButton.Size = new Size(75, 23);
            authButton.TabIndex = 2;
            authButton.Text = "Auth";
            authButton.UseVisualStyleBackColor = true;
            authButton.Click += authButton_Click;
            // 
            // showMinimapCheckBox
            // 
            showMinimapCheckBox.AutoSize = true;
            showMinimapCheckBox.Location = new Point(12, 41);
            showMinimapCheckBox.Name = "showMinimapCheckBox";
            showMinimapCheckBox.Size = new Size(106, 19);
            showMinimapCheckBox.TabIndex = 4;
            showMinimapCheckBox.Text = "Show Minimap";
            showMinimapCheckBox.UseVisualStyleBackColor = true;
            showMinimapCheckBox.CheckedChanged += showMinimapCheckBox_CheckedChanged;
            // 
            // addLspButton
            // 
            addLspButton.Location = new Point(255, 12);
            addLspButton.Name = "addLspButton";
            addLspButton.Size = new Size(75, 23);
            addLspButton.TabIndex = 3;
            addLspButton.Text = "Add LSP";
            addLspButton.UseVisualStyleBackColor = true;
            addLspButton.Click += addLspButton_Click;
            // 
            // setTextButton
            // 
            setTextButton.Location = new Point(12, 12);
            setTextButton.Name = "setTextButton";
            setTextButton.Size = new Size(75, 23);
            setTextButton.TabIndex = 0;
            setTextButton.Text = "Set Text";
            setTextButton.UseVisualStyleBackColor = true;
            setTextButton.Click += setTextButton_Click;
            // 
            // getTextButton
            // 
            getTextButton.Location = new Point(93, 12);
            getTextButton.Name = "getTextButton";
            getTextButton.Size = new Size(75, 23);
            getTextButton.TabIndex = 1;
            getTextButton.Text = "Get Text";
            getTextButton.UseVisualStyleBackColor = true;
            getTextButton.Click += getTextButton_Click;
            // 
            // setLangButton
            // 
            setLangButton.Location = new Point(174, 12);
            setLangButton.Name = "setLangButton";
            setLangButton.Size = new Size(75, 23);
            setLangButton.TabIndex = 2;
            setLangButton.Text = "Set Lang";
            setLangButton.UseVisualStyleBackColor = true;
            setLangButton.Click += setLangButton_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { statusLabel });
            statusStrip1.Location = new Point(0, 428);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(800, 22);
            statusStrip1.TabIndex = 1;
            statusStrip1.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new Size(66, 17);
            statusLabel.Text = "statusLabel";
            statusLabel.Visible = false;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 73);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(800, 355);
            tabControl1.TabIndex = 6;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(bodyPanel);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(792, 327);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Monaco";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // bodyPanel
            // 
            bodyPanel.Dock = DockStyle.Fill;
            bodyPanel.Location = new Point(3, 3);
            bodyPanel.Name = "bodyPanel";
            bodyPanel.Size = new Size(786, 321);
            bodyPanel.TabIndex = 1;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(lspLogTextBox);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(792, 327);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "LSP Log";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // lspLogTextBox
            // 
            lspLogTextBox.Dock = DockStyle.Fill;
            lspLogTextBox.Location = new Point(3, 3);
            lspLogTextBox.Multiline = true;
            lspLogTextBox.Name = "lspLogTextBox";
            lspLogTextBox.ReadOnly = true;
            lspLogTextBox.ScrollBars = ScrollBars.Vertical;
            lspLogTextBox.Size = new Size(786, 321);
            lspLogTextBox.TabIndex = 0;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(tabControl1);
            Controls.Add(statusStrip1);
            Controls.Add(topPanel);
            Name = "Form1";
            Text = "Form1";
            topPanel.ResumeLayout(false);
            topPanel.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel topPanel;
        private Button setTextButton;
        private Button getTextButton;
        private Button setLangButton;
        private Button addLspButton;
        private CheckBox showMinimapCheckBox;
        private StatusStrip statusStrip1;
        private Button authButton;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private Panel bodyPanel;
        private TabPage tabPage2;
        private TextBox lspLogTextBox;
        private ToolStripStatusLabel statusLabel;
    }
}
