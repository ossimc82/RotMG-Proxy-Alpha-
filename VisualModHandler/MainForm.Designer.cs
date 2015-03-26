namespace VisualModHandler
{
    partial class MainForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.currentModBox = new System.Windows.Forms.GroupBox();
            this.modToggleButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.modDesc = new System.Windows.Forms.TextBox();
            this.modHelp = new System.Windows.Forms.TextBox();
            this.modVersion = new System.Windows.Forms.Label();
            this.modCreator = new System.Windows.Forms.Label();
            this.modSettingsButton = new System.Windows.Forms.Button();
            this.currentModBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Mods: ";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(12, 38);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(120, 225);
            this.listBox1.TabIndex = 1;
            // 
            // currentModBox
            // 
            this.currentModBox.Controls.Add(this.modSettingsButton);
            this.currentModBox.Controls.Add(this.modToggleButton);
            this.currentModBox.Controls.Add(this.label3);
            this.currentModBox.Controls.Add(this.label2);
            this.currentModBox.Controls.Add(this.modDesc);
            this.currentModBox.Controls.Add(this.modHelp);
            this.currentModBox.Controls.Add(this.modVersion);
            this.currentModBox.Controls.Add(this.modCreator);
            this.currentModBox.Location = new System.Drawing.Point(138, 13);
            this.currentModBox.Name = "currentModBox";
            this.currentModBox.Size = new System.Drawing.Size(242, 250);
            this.currentModBox.TabIndex = 2;
            this.currentModBox.TabStop = false;
            this.currentModBox.Text = "null";
            // 
            // modToggleButton
            // 
            this.modToggleButton.Location = new System.Drawing.Point(145, 221);
            this.modToggleButton.Name = "modToggleButton";
            this.modToggleButton.Size = new System.Drawing.Size(88, 23);
            this.modToggleButton.TabIndex = 6;
            this.modToggleButton.Text = "Enable/Disable";
            this.modToggleButton.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Mod description:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 141);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Help:";
            // 
            // modDesc
            // 
            this.modDesc.Location = new System.Drawing.Point(6, 82);
            this.modDesc.Multiline = true;
            this.modDesc.Name = "modDesc";
            this.modDesc.ReadOnly = true;
            this.modDesc.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.modDesc.Size = new System.Drawing.Size(227, 55);
            this.modDesc.TabIndex = 3;
            this.modDesc.TabStop = false;
            // 
            // modHelp
            // 
            this.modHelp.Location = new System.Drawing.Point(9, 160);
            this.modHelp.Multiline = true;
            this.modHelp.Name = "modHelp";
            this.modHelp.ReadOnly = true;
            this.modHelp.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.modHelp.Size = new System.Drawing.Size(227, 55);
            this.modHelp.TabIndex = 2;
            this.modHelp.TabStop = false;
            // 
            // modVersion
            // 
            this.modVersion.AutoSize = true;
            this.modVersion.Location = new System.Drawing.Point(6, 43);
            this.modVersion.Name = "modVersion";
            this.modVersion.Size = new System.Drawing.Size(35, 13);
            this.modVersion.TabIndex = 1;
            this.modVersion.Text = "label2";
            // 
            // modCreator
            // 
            this.modCreator.AutoSize = true;
            this.modCreator.Location = new System.Drawing.Point(6, 21);
            this.modCreator.Name = "modCreator";
            this.modCreator.Size = new System.Drawing.Size(35, 13);
            this.modCreator.TabIndex = 0;
            this.modCreator.Text = "label2";
            // 
            // modSettingsButton
            // 
            this.modSettingsButton.Location = new System.Drawing.Point(64, 221);
            this.modSettingsButton.Name = "modSettingsButton";
            this.modSettingsButton.Size = new System.Drawing.Size(75, 23);
            this.modSettingsButton.TabIndex = 7;
            this.modSettingsButton.Text = "Settings";
            this.modSettingsButton.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AcceptButton = this.modToggleButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 274);
            this.Controls.Add(this.currentModBox);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Visual Mod Handler";
            this.currentModBox.ResumeLayout(false);
            this.currentModBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.GroupBox currentModBox;
        private System.Windows.Forms.Label modCreator;
        private System.Windows.Forms.Label modVersion;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox modDesc;
        private System.Windows.Forms.TextBox modHelp;
        private System.Windows.Forms.Button modToggleButton;
        private System.Windows.Forms.Button modSettingsButton;
    }
}