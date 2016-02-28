/* ----------------------------------------------------------------------------- 
* .NET FluentLib - Copyright (c) Elias Bachaalany <lallousz-x86@yahoo.com>
* All rights reserved.
* 
* Redistribution and use in source and binary forms, with or without
* modification, are permitted provided that the following conditions
* are met:
* 1. Redistributions of source code must retain the above copyright
*    notice, this list of conditions and the following disclaimer.
* 2. Redistributions in binary form must reproduce the above copyright
*    notice, this list of conditions and the following disclaimer in the
*    documentation and/or other materials provided with the distribution.
* 
* THIS SOFTWARE IS PROVIDED BY THE AUTHOR AND CONTRIBUTORS ``AS IS'' AND
* ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
* IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
* ARE DISCLAIMED.  IN NO EVENT SHALL THE AUTHOR OR CONTRIBUTORS BE LIABLE
* FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
* DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS
* OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
* HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
* LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY
* OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF
* SUCH DAMAGE.
* ----------------------------------------------------------------------------- 
*/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace lallouslab.FluentLib.WinForms.Dialogs
{
    public partial class StringsPicker : Form
    {
        /// <summary>
        /// Match types
        /// </summary>
        [Flags]
        public enum MatchingFlags: int
        {
            Contains     = 0x01,
            Exact        = 0x02,
            StartsWith   = 0x04,
            EndsWith     = 0x08,
            RegEx        = 0x10,
            Fuzzy        = 0x20,

            Basic        = Contains | Exact,
            Intermediate = Basic | StartsWith | EndsWith
        }

        class ItemsCache
        {
            public string[] Strs;
            public bool[] ChkState;
            public ItemsCache(int size)
            {
                Strs = new string[size];
                ChkState = new bool[size];
            }

            public ItemsCache(string [] Items): this(Items.Length)
            {
                for (int i=0, c = Length; i < c; i++)
                {
                    Strs[i] = Items[i];
                    ChkState[i] = false;
                }
            }

            public int Length
            {
                get
                {
                    return Strs.Length;
                }
            }

            internal void CopyFrom(ItemsCache Src)
            {
                for (int i=0, c = Src.Length; i < c; i++)
                {
                    Strs[i] = Src.Strs[i];
                    ChkState[i] = Src.ChkState[i];
                }
            }

            public void Add(string str)
            {
                ItemsCache NewItems = new ItemsCache(Length + 1);
                NewItems.CopyFrom(this);

                NewItems.Strs[Length] = str;
                NewItems.ChkState[Length] = true;

                Strs = NewItems.Strs;
                ChkState = NewItems.ChkState;
            }
        }

        public ListView GetListView()
        {
            return lvItems;
        }

        /// <summary>
        /// Filter options
        /// </summary>
        class FilterOptions
        {
            public string Text;
            public MatchingFlags MatchType = MatchingFlags.Contains;
            public Regex re;
            public ItemsCache Items;
        }

        private FilterOptions m_Filter = new FilterOptions();

        /// <summary>
        /// Instantly apply the typed text filter.
        /// This option can be updated on runtime.
        /// </summary>
        public bool InstantFilter;

        public StringsPicker(
            string [] Items,
            bool MultiSelect = false,
            string Title = null,
            bool AllowAddItems = false,
            bool InstantFilter = true,
            bool UseBasicLVExtensions = true,
            MatchingFlags MatchFlags = MatchingFlags.Basic,
            MatchingFlags DefaultMatchFlag = 0)
        {
            InitializeComponent();

            // Multiselect option means: enable checkboxes
            lvItems.MultiSelect = false;
            lvItems.CheckBoxes = MultiSelect;

            // Set instant filter
            this.InstantFilter = InstantFilter;

            // Override the title if given
            if (!string.IsNullOrEmpty(Title))
                Text = Title;

            // Create match options menu
            var FirstFlag = CreateMatchTypeMenu(MatchFlags);
            if (!SetMatchMenu(DefaultMatchFlag))
                SetMatchMenu(FirstFlag);

            LayoutControls(AllowAddItems);

            m_Filter.Items = new ItemsCache(Items);

            if (UseBasicLVExtensions)
            {
                ListViewExtensions.CreateCommonMenuItems(
                    ctxmenuLV,
                    lvItems,
                    new ListViewExtensions.Options()
                    {
                        MFlags = ListViewExtensions.MenuFlags.Find | ListViewExtensions.MenuFlags.CopyAndSelect
                    }
                );
            }
        }

        private void LayoutControls(bool AllowAddItems)
        {
            var flow = new TableLayoutPanel();
            Panel[] panels = new Panel[]
            {
                pnlAdd,
                pnlFilter,
                pnlLV,
                pnlOkCancel
            };
            flow.AutoSize = true;
            flow.AutoSizeMode = AutoSizeMode.GrowOnly;
            flow.ColumnCount = 1;
            flow.ColumnStyles.Add(new ColumnStyle()
            {
                SizeType = SizeType.Percent,
                Width = 100
            });

            int tabidx = 1;
            foreach (var panel in panels)
            {
                Controls.Remove(panel);
                if (!AllowAddItems && panel == pnlAdd)
                    continue;

                panel.TabIndex = tabidx++;
                foreach (Control control in panel.Controls)
                    control.TabIndex = tabidx++;

                flow.Controls.Add(panel);
            }
            Controls.Add(flow);

            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            AutoSize = true;
        }

        public string [] GetSelection()
        {
            var L = new List<string>();
            if (lvItems.CheckBoxes)
            {
                foreach (ListViewItem lvi in lvItems.Items)
                {
                    if (lvi.Checked)
                        L.Add(m_Filter.Items.Strs[(int)lvi.Tag]);
                }
            }
            else if (lvItems.SelectedItems.Count > 0)
            {
                L.Add(m_Filter.Items.Strs[(int)lvItems.SelectedItems[0].Tag]);
            }
            return L.ToArray();
        }

        private MatchingFlags CreateMatchTypeMenu(
            MatchingFlags MatchFlags)
        {
            MatchingFlags FirstFlag = 0;

            // Count the single bit matching options and dynamically create the menus
            foreach (MatchingFlags fl in Enum.GetValues(typeof(MatchingFlags)))
            {
                if (!MatchFlags.HasFlag(fl))
                    continue;

                // Skip multiflag options
                if (Sys.Utils.CountSetBits((long)fl) > 1)
                    continue;

                // Create the menu item
                var menu = new ToolStripMenuItem()
                {
                    Text = fl.ToString(),
                    Tag = fl,
                };

                // Menu handler is generic
                menu.Click += ctxmenuMatchTypeClick;
                ctxmenuMatchOptions.Items.Add(menu);

                // Remember the first default option
                if (FirstFlag == 0)
                    FirstFlag = fl;
            }
            return FirstFlag;
        }

        private bool SetMatchMenu(MatchingFlags CurMatch)
        {
            bool bFound = false;

            // Uncheck all other items but the current one.
            foreach (ToolStripMenuItem mi in ctxmenuMatchOptions.Items)
            {
                if ((MatchingFlags)mi.Tag == CurMatch)
                {
                    mi.Checked = true;
                    bFound = true;
                }
                else
                {
                    mi.Checked = false;
                }
            }

            m_Filter.MatchType = CurMatch;
            return bFound;
        }

        private bool UpdateFilterRegEx()
        {
            try
            {
                // Build the regex
                m_Filter.re = new Regex(m_Filter.Text, RegexOptions.IgnoreCase);

                SetFilterTextError(false);
                return true;
            }
            catch
            {
                SetFilterTextError(true);

                // Clear the previous expression
                m_Filter.re = null;
                return false;
            }
        }

        private void SetFilterTextError(bool bError)
        {
            txtFilter.ForeColor = bError ? Color.Red : Color.Empty;
        }

        private void PopulateItems()
        {
            // Do not repopulate if RegEx could not be set
            if (m_Filter.MatchType == MatchingFlags.RegEx && !UpdateFilterRegEx())
                return;

            //
            // Start populating
            //
            lvItems.Items.Clear();
            lvItems.BeginUpdate();

            var FilterText = m_Filter.Text;

            for (int i = 0, count = m_Filter.Items.Length; i < count; i++)
            {
                var str = m_Filter.Items.Strs[i];
                if (!string.IsNullOrEmpty(FilterText))
                {
                    bool bIncl = true;
                    switch (m_Filter.MatchType)
                    {
                        case MatchingFlags.Exact:
                            bIncl = FilterText.Equals(str, StringComparison.OrdinalIgnoreCase);
                            break;
                        case MatchingFlags.Contains:
                            bIncl = str.Contains(FilterText);
                            break;
                        case MatchingFlags.EndsWith:
                            bIncl = str.EndsWith(FilterText, StringComparison.OrdinalIgnoreCase);
                            break;
                        case MatchingFlags.StartsWith:
                            bIncl = str.StartsWith(FilterText, StringComparison.OrdinalIgnoreCase);
                            break;
                        case MatchingFlags.RegEx:
                            bIncl = m_Filter.re.IsMatch(str);
                            break;
                    }
                    // Skip item if no match.
                    if (!bIncl)
                        continue;
                }

                // Add item
                lvItems.Items.Add(new ListViewItem(str)
                {
                    Tag = i,
                    Checked = m_Filter.Items.ChkState[i]
                });
            }

            lvItems.EndUpdate();
        }

        private void AddItem(string str)
        {
            m_Filter.Items.Add(str);
            PopulateItems();
       }
        #region Event handlers
        private void StaticItemsPicker_Load(
            object sender, 
            EventArgs e)
        {
            PopulateItems();
            ActiveControl = txtFilter;
        }

        private void ctxmenuMatchTypeClick(
            object sender, 
            EventArgs e)
        {
            var CurMatch = (MatchingFlags)(sender as ToolStripMenuItem).Tag;

            SetMatchMenu(CurMatch);

            SetFilterTextError(CurMatch == MatchingFlags.RegEx ? !UpdateFilterRegEx() : false);

            PopulateItems();
        }

        private void txtFilter_TextChanged(
            object sender, 
            EventArgs e)
        {
            m_Filter.Text = txtFilter.Text;

            if (InstantFilter)
            {
                BeginInvoke(new MethodInvoker(
                    delegate
                    {
                        PopulateItems();
                    }));
            }
        }

        private void btnTextFilterOption_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            ctxmenuMatchOptions.Show(btn, new Point(0, btn.Height));
        }
        /// <summary>
        /// Handle key presses. ENTER to apply filter.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtFilter_KeyPress(
            object sender, 
            KeyPressEventArgs e)
        {
            if (e.KeyChar == '\t' || e.KeyChar == '\r')
            {
                e.Handled = true;
                PopulateItems();
            }
        }

        private void lvItems_ItemChecked(
            object sender, 
            ItemCheckedEventArgs e)
        {
            m_Filter.Items.ChkState[(int)e.Item.Tag] = e.Item.Checked;
        }
        private void btnInsertText_Click(object sender, EventArgs e)
        {
            var str = txtInsertText.Text;
            if (!string.IsNullOrEmpty(str))
                AddItem(str);
        }

        #endregion

    }
}