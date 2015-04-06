//The MIT License (MIT)
//
//Copyright (c) 2015 Fabian Fischer
//
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.
using IProxy.Mod;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisualModHandler
{
    public partial class ModSettingsForm : Form
    {
        private ISettingsProvider settingsProvider;
        private IProxyMod mod;
        private Button saveButton;

        public ModSettingsForm(ISettingsProvider settingsProvider, IProxyMod mod)
        {
            InitializeComponent();
            this.settingsProvider = settingsProvider;
            this.mod = mod;

            SuspendLayout();

            Text = String.Format("{0} - Settings", mod.Name);

            var nextHeight = 12;

            foreach (var setting in settingsProvider.GetValues())
            {
                Controls.Add(new SettingsControl(setting.Key, setting.Value));
                Controls[Controls.Count - 1].Location = new Point(12, nextHeight);
                nextHeight += 30;
            }

            saveButton = new Button();
            saveButton.Text = "Save";
            saveButton.Click += saveButton_Click;
            saveButton.Location = new Point(350, nextHeight);
            saveButton.Size = new Size(100, 25);
            Controls.Add(saveButton);

            this.Size = new Size(474, nextHeight + 70);
            ResumeLayout(false);
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            foreach (var s in Controls.Cast<Control>().OfType<SettingsControl>())
                settingsProvider.SetValue(s.Key, s.Value);
            settingsProvider.Save();
        }
    }

    class SettingsControl : Control
    {
        private TextBox key;
        private TextBox textValue;
        private CheckBox boolValue;
        private Label equalsSign;

        public SettingsControl(string key, string value = null)
        {
            this.key = new TextBox();
            this.equalsSign = new Label();
            this.Size = new System.Drawing.Size(450, 20);
            this.key.Size = new Size(200, 30);
            this.equalsSign.AutoSize = true;
            this.equalsSign.Size = new Size(10, 20);
            this.key.Width = 200;
            this.key.Location = new Point(0, 0);
            this.equalsSign.Location = new Point(210, 0);
            this.equalsSign.Text = "=";
            this.key.ReadOnly = true;

            Controls.Add(this.equalsSign);
            Controls.Add(this.key);

            if (value.ToLower() == "true" || value.ToLower() == "false")
            {
                this.boolValue = new CheckBox();
                this.boolValue.Width = 200;
                this.boolValue.Location = new Point(230, 0);
                this.boolValue.Checked = value.ToLower() == "true";
                Controls.Add(this.boolValue);
            }
            else
            {
                this.textValue = new TextBox();
                this.textValue.Width = 200;
                this.textValue.Location = new Point(230, 0);
                this.textValue.Text = value;
                Controls.Add(this.textValue);
            }
            this.key.Text = key;
        }

        public string Key { get { return this.key.Text; } }
        public string Value { get { return this.textValue == null ? this.boolValue.Checked.ToString() : this.textValue.Text; } }
    }
}
