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
            this.label1 = new MetroFramework.Controls.MetroLabel();
            this.currentModBox = new System.Windows.Forms.GroupBox();
            this.modSettingsButton = new MetroFramework.Controls.MetroButton();
            this.modToggleButton = new MetroFramework.Controls.MetroButton();
            this.label3 = new MetroFramework.Controls.MetroLabel();
            this.label2 = new MetroFramework.Controls.MetroLabel();
            this.modDesc = new MetroFramework.Controls.MetroTextBox();
            this.modHelp = new MetroFramework.Controls.MetroTextBox();
            this.modVersion = new MetroFramework.Controls.MetroLabel();
            this.modCreator = new MetroFramework.Controls.MetroLabel();
            this.listBox1 = new VisualModHandler.MetroListBox();
            this.currentModBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "Mods: ";
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
            this.currentModBox.Location = new System.Drawing.Point(138, 57);
            this.currentModBox.Name = "currentModBox";
            this.currentModBox.Size = new System.Drawing.Size(242, 250);
            this.currentModBox.TabIndex = 2;
            this.currentModBox.TabStop = false;
            this.currentModBox.Text = "null";
            // 
            // modSettingsButton
            // 
            this.modSettingsButton.Location = new System.Drawing.Point(64, 221);
            this.modSettingsButton.Name = "modSettingsButton";
            this.modSettingsButton.Size = new System.Drawing.Size(75, 23);
            this.modSettingsButton.TabIndex = 7;
            this.modSettingsButton.Text = "Settings";
            this.modSettingsButton.UseSelectable = true;
            this.modSettingsButton.UseStyleColors = true;
            // 
            // modToggleButton
            // 
            this.modToggleButton.Location = new System.Drawing.Point(145, 221);
            this.modToggleButton.Name = "modToggleButton";
            this.modToggleButton.Size = new System.Drawing.Size(88, 23);
            this.modToggleButton.TabIndex = 6;
            this.modToggleButton.Text = "Enable/Disable";
            this.modToggleButton.UseSelectable = true;
            this.modToggleButton.UseStyleColors = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(108, 19);
            this.label3.TabIndex = 5;
            this.label3.Text = "Mod description:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 141);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 19);
            this.label2.TabIndex = 4;
            this.label2.Text = "Help:";
            // 
            // modDesc
            // 
            this.modDesc.Lines = new string[0];
            this.modDesc.Location = new System.Drawing.Point(9, 83);
            this.modDesc.MaxLength = 32767;
            this.modDesc.Multiline = true;
            this.modDesc.Name = "modDesc";
            this.modDesc.PasswordChar = '\0';
            this.modDesc.ReadOnly = true;
            this.modDesc.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.modDesc.SelectedText = "";
            this.modDesc.Size = new System.Drawing.Size(227, 55);
            this.modDesc.TabIndex = 3;
            this.modDesc.TabStop = false;
            this.modDesc.UseSelectable = true;
            this.modDesc.UseStyleColors = true;
            // 
            // modHelp
            // 
            this.modHelp.Lines = new string[0];
            this.modHelp.Location = new System.Drawing.Point(9, 162);
            this.modHelp.MaxLength = 32767;
            this.modHelp.Multiline = true;
            this.modHelp.Name = "modHelp";
            this.modHelp.PasswordChar = '\0';
            this.modHelp.ReadOnly = true;
            this.modHelp.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.modHelp.SelectedText = "";
            this.modHelp.Size = new System.Drawing.Size(227, 55);
            this.modHelp.TabIndex = 2;
            this.modHelp.TabStop = false;
            this.modHelp.UseSelectable = true;
            this.modHelp.UseStyleColors = true;
            // 
            // modVersion
            // 
            this.modVersion.AutoSize = true;
            this.modVersion.Location = new System.Drawing.Point(6, 43);
            this.modVersion.Name = "modVersion";
            this.modVersion.Size = new System.Drawing.Size(44, 19);
            this.modVersion.TabIndex = 1;
            this.modVersion.Text = "label2";
            // 
            // modCreator
            // 
            this.modCreator.AutoSize = true;
            this.modCreator.Location = new System.Drawing.Point(6, 21);
            this.modCreator.Name = "modCreator";
            this.modCreator.Size = new System.Drawing.Size(44, 19);
            this.modCreator.TabIndex = 0;
            this.modCreator.Text = "label2";
            // 
            // listBox1
            // 
            this.listBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.IntegralHeight = true;
            this.listBox1.ItemHeight = 13;
            this.listBox1.Location = new System.Drawing.Point(12, 82);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(120, 225);
            this.listBox1.TabIndex = 1;
            this.listBox1.TabStop = false;
            this.listBox1.UseSelectable = true;
            this.listBox1.UseStyleColors = true;
            // 
            // MainForm
            // 
            this.AcceptButton = this.modToggleButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 316);
            this.Controls.Add(this.currentModBox);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Resizable = false;
            this.Text = "Visual Mod Handler";
            this.TransparencyKey = System.Drawing.Color.Empty;
            this.currentModBox.ResumeLayout(false);
            this.currentModBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroLabel label1;
        private VisualModHandler.MetroListBox listBox1;
        private System.Windows.Forms.GroupBox currentModBox;
        private MetroFramework.Controls.MetroLabel modCreator;
        private MetroFramework.Controls.MetroLabel modVersion;
        private MetroFramework.Controls.MetroLabel label3;
        private MetroFramework.Controls.MetroLabel label2;
        private MetroFramework.Controls.MetroTextBox modDesc;
        private MetroFramework.Controls.MetroTextBox modHelp;
        private MetroFramework.Controls.MetroButton modToggleButton;
        private MetroFramework.Controls.MetroButton modSettingsButton;
    }
}