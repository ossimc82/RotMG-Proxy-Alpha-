﻿//The MIT License (MIT)
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
    public partial class MainForm : Form
    {
        private IProxyMod selectedMod;
        private Dictionary<string, IProxyMod> mods;

        public MainForm()
        {
            mods = new Dictionary<string, IProxyMod>();
            InitializeComponent();
            label1.Text = "Mods: " + ModHandler.Mods.Count;
            listBox1.SelectionMode = SelectionMode.One;
            foreach(var mod in ModHandler.Mods)
            {
                listBox1.Items.Add(mod.Value.Information.Name);
                mods.Add(mod.Value.Information.Name, mod.Key);
                if (mod.Key.Name == Singleton<Mod>.Instance.Name) continue;
                if (!mod.Value.Enabled && Singleton<Settings>.Instance.GetValue<bool>("Enabled_" + mod.Key.Name.Replace(" ", String.Empty), "true"))
                    mod.Value.Enable();

                if (mod.Value.Enabled && !Singleton<Settings>.Instance.GetValue<bool>("Enabled_" + mod.Key.Name.Replace(" ", String.Empty), "true"))
                    mod.Value.Disable();
            }
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            modToggleButton.Click += modToggleButton_Click;
            modSettingsButton.Click += modSettingsButton_Click;
            listBox1.SelectedItem = listBox1.Items[0];
        }

        private void modSettingsButton_Click(object sender, EventArgs e)
        {
            var settingsForm = new ModSettingsForm(ModHandler.Mods[selectedMod].Settings, selectedMod);
            settingsForm.StartPosition = FormStartPosition.CenterParent;
            settingsForm.ShowDialog(this);
        }

        private void modToggleButton_Click(object sender, EventArgs e)
        {
            if (modToggleButton.Text == "Enable Mod")
            {
                ModHandler.Mods[selectedMod].Enable();
                Singleton<Settings>.Instance.SetValue("Enabled_" + selectedMod.Name.Replace(" ", String.Empty), "true");
            }
            else
            {
                ModHandler.Mods[selectedMod].Disable();
                Singleton<Settings>.Instance.SetValue("Enabled_" + selectedMod.Name.Replace(" ", String.Empty), "false");
            }

            updateInfo();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectItem((string)listBox1.SelectedItem);
        }

        private void selectItem(string mod)
        {
            selectedMod = mods[mod];
            updateInfo();
        }

        private void updateInfo()
        {
            currentModBox.Text = selectedMod.Name + (!ModHandler.Mods[selectedMod].Enabled ? " [Disabled]" : String.Empty);
            modCreator.Text = "Made by: " + selectedMod.Creator;
            modVersion.Text = "Version " + selectedMod.ModVersion;
            modDesc.Text = selectedMod.Description;
            modHelp.Text = String.IsNullOrWhiteSpace(selectedMod.Help) ? "No help available" : selectedMod.Help.Replace("\n", Environment.NewLine);
            modToggleButton.Text = ModHandler.Mods[selectedMod].Enabled ? "Disable Mod" : "Enable Mod";
            modToggleButton.Enabled = ModHandler.Mods[selectedMod].Information.Name != Singleton<Mod>.Instance.Name;

            if (ModHandler.Mods[selectedMod].Settings != null && ModHandler.Mods[selectedMod].Information.Name != Singleton<Mod>.Instance.Name)
                modSettingsButton.Enabled = true;
            else 
                modSettingsButton.Enabled = false;
        }
    }
}
