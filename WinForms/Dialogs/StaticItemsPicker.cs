using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lallouslab.FluentLib.WinForms.Dialogs
{
    public partial class StaticItemsPicker : Form
    {
        object[] Items;

        /// <summary>
        /// Match types
        /// </summary>
        [Flags]
        enum MatchingFlags: int
        {
            Contains   = 0x01,
            Matches    = 0x02,
            StartsWith = 0x04,
            EndsWith   = 0x08,
            REMatch    = 0x10,
            Fuzzy      = 0x20,

            Basic      = Contains | Matches,
        }

        /// <summary>
        /// Filter options
        /// </summary>
        class FilterOptions
        {
            public string Text;
            public MatchingFlags type;
            public Regex re;
        }

        public StaticItemsPicker(
            object [] Items,
            bool bMultiSelect = false,
            string Title = null,
            MatchingFlags MatchFlags = MatchingFlags.Basic)
        {
            InitializeComponent();

            lvItems.MultiSelect = bMultiSelect;
            this.Items = Items;
            if (!string.IsNullOrEmpty(Title))
                Text = Title;
        }

        private void StaticItemsPicker_Load(
            object sender, 
            EventArgs e)
        {
            Populate();
        }

        private void Populate()
        {
            lvItems.Items.Clear();
            lvItems.BeginUpdate();
            foreach (var obj in Items)
            {
                lvItems.Items.Add(
                    new ListViewItem(obj.ToString())
                    {
                        Tag = obj
                    }
                );
            }

            lvItems.EndUpdate();
        }

        private void textBox1_TextChanged(
            object sender, 
            EventArgs e)
        {

        }

        private void btnTextFilterOption_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            ctxmenuFilterOptions.Show(btn, new Point(0, btn.Height));
        }
    }
}
