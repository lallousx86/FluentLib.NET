using lallouslab.FluentLib.WinForms;
using lallouslab.FluentLib.WinForms.Dialogs;
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
        public string AutoClickButton;

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

        private void btnStringsPicker1_Click(
            object sender,
            EventArgs e)
        {
            var items = (new List<string>
            {
                "hello",
                "world",
                "tree hugging",
                "human hugging",
                "piece",
                "peice",
            }).ToArray();

            var f = new StringsPicker(
                Items: items,
                Title: "Pick an item",
                MultiSelect: true,
                InstantFilter: true,
                AllowAddItems: true,
                MatchFlags: StringsPicker.MatchingFlags.Basic | StringsPicker.MatchingFlags.StartsWith | StringsPicker.MatchingFlags.RegEx,
                DefaultMatchFlag: StringsPicker.MatchingFlags.StartsWith);

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
            if (string.IsNullOrEmpty(AutoClickButton))
                return;

            BeginInvoke(new MethodInvoker(() =>
            {
                switch (AutoClickButton)
                {
                    case "click-stringpicker1":
                        btnStringsPicker1_Click(btnStringsPicker1, e);
                        break;

                    case "click-stringpicker2":
                        btnStringsPicker2_Click(btnStringsPicker2, e);
                        break;

                    default:
                        return;
                }
                Close();
            }));
        }

        private void btnStringsPicker2_Click(
            object sender, 
            EventArgs e)
        {
            var items = (new List<string>
            {
                "hello",
                "world",
                "tree hugging",
                "human hugging",
                "piece",
                "peice",
            }).ToArray();

            var f = new StringsPicker(
                Items: items,
                Title: "Pick an item",
                MultiSelect: true,
                InstantFilter: true,
                AllowAddItems: true,
                UseBasicLVExtensions: true,
                MatchFlags: StringsPicker.MatchingFlags.Basic | StringsPicker.MatchingFlags.StartsWith | StringsPicker.MatchingFlags.RegEx,
                DefaultMatchFlag: StringsPicker.MatchingFlags.StartsWith);

            if (f.ShowDialog() == DialogResult.OK)
            {
                object[] res = f.GetSelection();
                foreach (string s in res)
                    Debug.WriteLine(s);
            }
        }
    }
}
