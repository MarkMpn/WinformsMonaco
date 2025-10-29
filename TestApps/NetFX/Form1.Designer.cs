namespace NetFX
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.topPanel = new System.Windows.Forms.Panel();
            this.bodyPanel = new System.Windows.Forms.Panel();
            this.setTextButton = new System.Windows.Forms.Button();
            this.getTextButton = new System.Windows.Forms.Button();
            this.setLangButton = new System.Windows.Forms.Button();
            this.topPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // topPanel
            // 
            this.topPanel.Controls.Add(this.setTextButton);
            this.topPanel.Controls.Add(this.getTextButton);
            this.topPanel.Controls.Add(this.setLangButton);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(0, 0);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(800, 49);
            this.topPanel.TabIndex = 0;
            // 
            // bodyPanel
            // 
            this.bodyPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bodyPanel.Location = new System.Drawing.Point(0, 49);
            this.bodyPanel.Name = "bodyPanel";
            this.bodyPanel.Size = new System.Drawing.Size(800, 401);
            this.bodyPanel.TabIndex = 0;
            // 
            // setTextButton
            // 
            this.setTextButton.Location = new System.Drawing.Point(12, 12);
            this.setTextButton.Name = "setTextButton";
            this.setTextButton.Size = new System.Drawing.Size(75, 23);
            this.setTextButton.TabIndex = 0;
            this.setTextButton.Text = "Set Text";
            this.setTextButton.UseVisualStyleBackColor = true;
            this.setTextButton.Click += new System.EventHandler(this.setTextButton_Click);
            // 
            // getTextButton
            // 
            this.getTextButton.Location = new System.Drawing.Point(93, 12);
            this.getTextButton.Name = "getTextButton";
            this.getTextButton.Size = new System.Drawing.Size(75, 23);
            this.getTextButton.TabIndex = 1;
            this.getTextButton.Text = "Get Text";
            this.getTextButton.UseVisualStyleBackColor = true;
            this.getTextButton.Click += new System.EventHandler(this.getTextButton_Click);
            // 
            // setLangButton
            // 
            this.setLangButton.Location = new System.Drawing.Point(174, 12);
            this.setLangButton.Name = "setLangButton";
            this.setLangButton.Size = new System.Drawing.Size(75, 23);
            this.setLangButton.TabIndex = 2;
            this.setLangButton.Text = "Set Lang";
            this.setLangButton.UseVisualStyleBackColor = true;
            this.setLangButton.Click += new System.EventHandler(this.setLangButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.bodyPanel);
            this.Controls.Add(this.topPanel);
            this.Name = "Form1";
            this.Text = "Form1";
            this.topPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.Button setTextButton;
        private System.Windows.Forms.Button getTextButton;
        private System.Windows.Forms.Button setLangButton;
        private System.Windows.Forms.Panel bodyPanel;
    }
}

