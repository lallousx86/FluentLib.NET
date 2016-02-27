﻿using lallouslab.FluentLib.WinForms.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class SelectorForm : Form
    {
        enum Z
        {
            V1,
            V2,
            V21,
            V22,
            V23,
            V3,
            Hi_Whats_Up
        }

        public SelectorForm()
        {
            InitializeComponent();
        }

        private void button1_Click(
            object sender, 
            EventArgs e)
        {
            var Names = typeof(Z).GetEnumNames();
            var StartColors = new Color[Names.Length];
            StartColors[0] = Color.Red;
            StartColors[1] = Color.Gray;
            StartColors[2] = Color.Orange;

            var f = new NamedItemsColorChooser(
                Names,
                StartColors)
            {
                Text = "Choose color"
            };
            
            f.ShowDialog();
        }

        private void button2_Click(
            object sender,
            EventArgs e)
        {
            object[] items = (new List<string>
            {
                "hello",
                "world",
                "tree hugging",
                "human hugging",
                "piece",
                "peice",
            }).ToArray();

            var f = new StaticItemsPicker(
                Items: items,
                Title: "Asdf",
                bMultiSelect: false,
                MatchFlags: StaticItemsPicker.MatchingFlags.Basic | StaticItemsPicker.MatchingFlags.StartsWith | StaticItemsPicker.MatchingFlags.RegEx,
                DefaultMatchFlag: StaticItemsPicker.MatchingFlags.StartsWith);

            if (f.ShowDialog() == DialogResult.OK)
            {
                object[] res = f.GetSelection();
                foreach (string s in res)
                    Debug.WriteLine(s);
            }
        }

        private void SelectorForm_Load(
            object sender,
            EventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(() =>
            {
                button2_Click(button2, e);
                Close();
            }));
        }
    }
}
