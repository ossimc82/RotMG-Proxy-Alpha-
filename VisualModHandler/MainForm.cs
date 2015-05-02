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
using IProxy;
using IProxy.Mod;
using IProxy.Mod.WinForm;
using MetroFramework.Forms;
using Proxy;
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
    public partial class MainForm : MetroForm
    {
        private IProxyMod selectedMod;
        private Dictionary<string, IProxyMod> mods;

        public MainForm()
        {
            mods = new Dictionary<string, IProxyMod>();
            InitializeComponent();
            label1.Text = "Mods: " + Singleton<ModHandler>.Instance.ModCount;
            listBox1.ListBox.SelectionMode = SelectionMode.One;
            foreach(var mod in Singleton<ModHandler>.Instance)
            {
                listBox1.ListBox.Items.Add(mod.Information.Name);
                mods.Add(mod.Information.Name, mod.Information);
                if (mod.Information.Name == Singleton<Mod>.Instance.Name) continue;
                if (!mod.Enabled && Singleton<Settings>.Instance.GetValue<bool>("Enabled_" + mod.Information.Name.Replace(" ", String.Empty), "true"))
                    mod.Enable();

                if (mod.Enabled && !Singleton<Settings>.Instance.GetValue<bool>("Enabled_" + mod.Information.Name.Replace(" ", String.Empty), "true"))
                    mod.Disable();
            }
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            modToggleButton.Click += modToggleButton_Click;
            modSettingsButton.Click += modSettingsButton_Click;
            listBox1.ListBox.SelectedItem = listBox1.ListBox.Items[0];
        }

        private void modSettingsButton_Click(object sender, EventArgs e)
        {
            var settingsForm = new ModSettingsForm(Singleton<ModHandler>.Instance[selectedMod].Settings, selectedMod);
            settingsForm.StartPosition = FormStartPosition.CenterParent;
            if (settingsForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                MetroFramework.MetroMessageBox.Show(this, "Settings saved", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void modToggleButton_Click(object sender, EventArgs e)
        {
            if (modToggleButton.Text == "Enable Mod")
            {
                Singleton<ModHandler>.Instance[selectedMod].Enable();
                Singleton<Settings>.Instance.SetValue("Enabled_" + selectedMod.Name.Replace(" ", String.Empty), "true");
            }
            else
            {
                Singleton<ModHandler>.Instance[selectedMod].Disable();
                Singleton<Settings>.Instance.SetValue("Enabled_" + selectedMod.Name.Replace(" ", String.Empty), "false");
            }

            updateInfo();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectItem((string)listBox1.ListBox.SelectedItem);
        }

        private void selectItem(string mod)
        {
            selectedMod = mods[mod];
            updateInfo();
        }

        private void updateInfo()
        {
            currentModBox.Text = selectedMod.Name + (!Singleton<ModHandler>.Instance[selectedMod].Enabled ? " [Disabled]" : String.Empty);
            modCreator.Text = "Made by: " + selectedMod.Creator;
            modVersion.Text = "Version: " + selectedMod.ModVersion;
            modDesc.Text = selectedMod.Description;
            modHelp.Text = String.IsNullOrWhiteSpace(selectedMod.Help) ? "No help available" : selectedMod.Help.Replace("\n", Environment.NewLine);
            modToggleButton.Text = Singleton<ModHandler>.Instance[selectedMod].Enabled ? "Disable Mod" : "Enable Mod";
            modToggleButton.Enabled = Singleton<ModHandler>.Instance[selectedMod].Information.Name != Singleton<Mod>.Instance.Name;

            if (Singleton<ModHandler>.Instance[selectedMod].Settings != null && Singleton<ModHandler>.Instance[selectedMod].Information.Name != Singleton<Mod>.Instance.Name)
                modSettingsButton.Enabled = true;
            else 
                modSettingsButton.Enabled = false;
        }

        public IProxyMod ModHandler { get; set; }
    }
}
