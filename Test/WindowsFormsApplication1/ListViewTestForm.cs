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
    public partial class ListViewTestForm : Form
    {
        public ListViewTestForm()
        {
            InitializeComponent();
        }

        private void ListViewTest_Load(object sender, EventArgs e)
        {
            //Export/ExportToTextFile (by default)
            //Screen width check before positioning the window
            //get an icon / icons for the buttons
            listView1.BeginUpdate();

            // Repeat the same items to test the FindNext functionality
            for (int iRepeat=1;iRepeat<=2;iRepeat++)
            {
                for (int i = 0; i < 10; i++)
                {
                    var lvi = new ListViewItem(string.Format("COL0 #{0}", i + 1));
                    lvi.SubItems.AddRange(new string[]
                        {
                    string.Format("COL1 #{0}", i + 1),
                    string.Format("COL2 #{0}", i + 1)
                        });
                    listView1.Items.Add(lvi);
                }
            }
            listView1.EndUpdate();

            var opt = new ListViewExtensions.Options()
            {
                CopyItemsSeparator = ",",
                WantColSorting = true
            };

            opt.OnGenColItemMenuItem += LVColUserMenuClick;

            opt.OnColItemMenuClick += delegate (object s, EventArgs ev)
            {
                var m = s as ToolStripMenuItem;
                if (m != null)
                    UIUtils.MsgBoxInfo("Hello " + m.Text);
            };

            ListViewExtensions.CreateCommonMenuItems(
                contextMenuStrip1,
                listView1,
                opt);
        }

        private bool LVColUserMenuClick(
            int ColIdx, 
            string ColText, 
            out ListViewExtensions.UserMenuItem[] Items)
        {
            Items = null;
            if (ColIdx == 0)
                return false;

            Items = new ListViewExtensions.UserMenuItem[]
            {
                new ListViewExtensions.UserMenuItem() { Text = "Exclude - " + ColText },
                new ListViewExtensions.UserMenuItem() { Text = "Include - " + ColText }
            };

            return true;
        }
    }
}