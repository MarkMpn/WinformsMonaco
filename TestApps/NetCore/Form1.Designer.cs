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
            showMinimapCheckBox = new CheckBox();
            addLspButton = new Button();
            setTextButton = new Button();
            getTextButton = new Button();
            setLangButton = new Button();
            bodyPanel = new Panel();
            topPanel.SuspendLayout();
            SuspendLayout();
            // 
            // topPanel
            // 
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
            // bodyPanel
            // 
            bodyPanel.Dock = DockStyle.Fill;
            bodyPanel.Location = new Point(0, 73);
            bodyPanel.Name = "bodyPanel";
            bodyPanel.Size = new Size(800, 377);
            bodyPanel.TabIndex = 0;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(bodyPanel);
            Controls.Add(topPanel);
            Name = "Form1";
            Text = "Form1";
            topPanel.ResumeLayout(false);
            topPanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel topPanel;
        private Button setTextButton;
        private Button getTextButton;
        private Button setLangButton;
        private Panel bodyPanel;
        private Button addLspButton;
        private CheckBox showMinimapCheckBox;
    }
}
