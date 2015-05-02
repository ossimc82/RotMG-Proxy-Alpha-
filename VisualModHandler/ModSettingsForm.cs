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
using MetroFramework.Controls;
using MetroFramework.Forms;
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
    public partial class ModSettingsForm : MetroForm
    {
        private ISettingsProvider settingsProvider;
        private IProxyMod mod;

        public ModSettingsForm(ISettingsProvider settingsProvider, IProxyMod mod)
        {
            InitializeComponent();
            this.settingsProvider = settingsProvider;
            this.mod = mod;

            Text = String.Format("{0} - Settings", mod.Name);

            var nextHeight = 60;

            foreach (var setting in settingsProvider.GetValues())
            {
                Controls.Add(new SettingsControl(setting.Key, setting.Value));
                Controls[Controls.Count - 1].Location = new Point(12, nextHeight);
                nextHeight += 30;
            }
            this.saveButton.Location = new Point(376, nextHeight);

            this.Size = new Size(474, nextHeight + 30);
            this.DialogResult = System.Windows.Forms.DialogResult.No;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            foreach (var s in Controls.Cast<Control>().OfType<SettingsControl>())
                settingsProvider.SetValue(s.Key, s.Value);
            settingsProvider.Save();
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
    }

    class SettingsControl : Control
    {
        private MetroTextBox key;
        private MetroTextBox textValue;
        private MetroToggle boolValue;
        private MetroLabel equalsSign;

        public SettingsControl(string key, string value = null)
        {
            this.key = new MetroTextBox();
            this.key.UseStyleColors = true;
            this.equalsSign = new MetroLabel();
            this.Size = new System.Drawing.Size(450, 25);
            this.key.Width = 200;
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
                this.boolValue = new MetroToggle();
                this.boolValue.UseStyleColors = true;
                this.boolValue.Width = 100;
                this.boolValue.Height = 20;
                this.boolValue.Location = new Point(230, 0);
                this.boolValue.Checked = value.ToLower() == "true";
                Controls.Add(this.boolValue);
            }
            else
            {
                this.textValue = new MetroTextBox();
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
