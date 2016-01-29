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
using lallouslab.FluentLib.WinForms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listView1.BeginUpdate();
            for (int i=0;i<10;i++)
            {
                var lvi = new ListViewItem(string.Format("COL0 #{0}", i + 1));
                lvi.SubItems.AddRange(new string[]
                    {
                    string.Format("COL1 #{0}", i + 1),
                    string.Format("COL2 #{0}", i + 1)
                    });

                listView1.Items.Add(lvi);
            }
            listView1.EndUpdate();

            var opt = new ListViewExtensions.Options()
            {
                CopyItemsSeparator = ",",
                WantColSorting = true
            };

            opt.OnGenColItemMenuItem += delegate (
                    string ColText,
                    out object Tag,
                    out string Caption)
            {
                Caption = "Exclude - " + ColText;
                Tag = null;
            };

            opt.OnColItemMenuClick += delegate (object s, EventArgs ev)
            {
                var m = s as ToolStripMenuItem;
                if (m != null)
                    UIUtils.MsgBoxInfo("Hello" + m.Text);
            };

            ListViewExtensions.CreateCommonMenuItems(
                contextMenuStrip1,
                listView1,
                opt);
        }
    }
}
