﻿/* ----------------------------------------------------------------------------- 
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
    public partial class StaticItemsPicker : Form
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

        /// <summary>
        /// Filter options
        /// </summary>
        class FilterOptions
        {
            public string Text;
            public MatchingFlags MatchType = MatchingFlags.Contains;
            public Regex re;

            public string[] ItemsStringCache;
            public bool[] ItemsSelCache;
        }

        private FilterOptions m_Filter = new FilterOptions();

        private object[] m_Items;

        /// <summary>
        /// Instantly apply the typed text filter.
        /// This option can be updated on runtime.
        /// </summary>
        public bool InstantFilter;

        public StaticItemsPicker(
            object [] Items,
            bool MultiSelect = false,
            string Title = null,
            bool InstantFilter = true,
            MatchingFlags MatchFlags = MatchingFlags.Basic,
            MatchingFlags DefaultMatchFlag = 0)
        {
            InitializeComponent();

            // Multiselect option means: enable checkboxes
            lvItems.MultiSelect = false;
            lvItems.CheckBoxes = MultiSelect;

            // Take items reference
            m_Items = Items;

            // Set instant filter
            this.InstantFilter = InstantFilter;

            // Override the title if given
            if (!string.IsNullOrEmpty(Title))
                Text = Title;

            // Create match options menu
            var FirstFlag = CreateMatchTypeMenu(MatchFlags);
            if (!SetMatchMenu(DefaultMatchFlag))
                SetMatchMenu(FirstFlag);
        }

        private void StaticItemsPicker_Load(
            object sender, 
            EventArgs e)
        {
            PopulateItems();
        }

        public object [] GetSelection()
        {
            var L = new List<object>();
            if (lvItems.CheckBoxes)
            {
                foreach (ListViewItem lvi in lvItems.Items)
                {
                    if (lvi.Checked)
                        L.Add(m_Items[(int)lvi.Tag]);
                }
            }
            else if (lvItems.SelectedItems.Count > 0)
            {
                L.Add(m_Items[(int) lvItems.SelectedItems[0].Tag]);
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
            int count = m_Items.Length;

            //
            // Cache the items once
            //
            if (m_Filter.ItemsStringCache == null)
            {
                m_Filter.ItemsStringCache = new string[count];
                m_Filter.ItemsSelCache = new bool[count];
                for (int i = 0; i < count; i++)
                {
                    m_Filter.ItemsStringCache[i] = m_Items[i].ToString();
                    m_Filter.ItemsSelCache[i] = false;
                }
            }

            // Do not repopulate if RegEx could not be set
            if (m_Filter.MatchType == MatchingFlags.RegEx && !UpdateFilterRegEx())
                return;

            //
            // Start populating
            //
            lvItems.Items.Clear();
            lvItems.BeginUpdate();

            var FilterText = m_Filter.Text;
            for (int i = 0; i < count; i++)
            {
                var str = m_Filter.ItemsStringCache[i];
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
                    Checked = m_Filter.ItemsSelCache[i]
                });
            }

            lvItems.EndUpdate();
        }

        #region Event handlers
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
            m_Filter.ItemsSelCache[(int)e.Item.Tag] = e.Item.Checked;
        }
        #endregion

    }
}