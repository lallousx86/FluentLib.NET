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
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using MSWinForms = global::System.Windows.Forms;

namespace lallouslab.FluentLib.WinForms
{
    public static class ListViewExtensions
    {
        [Flags]
        public enum MenuFlags : int
        {
            Delete = 0x01,
            Find = 0x02,
            Export = 0x04,
            CopyAndSelect = 0x08,
            All = 0xff,
            AllNoDelete = All & ~Delete,
        }

        public const string FILTER_TEXT_Filter = "Text Files (*.txt)|*.txt|All files (*.*)|*.*";
        public const string FILTER_TEXT_DefaultExt = "txt";

        public class ContextMenuTag
        {
            internal string FindString;
            internal int SearchPos;
            internal List<MSWinForms.ToolStripItem> GenColItemMenu = new List<MSWinForms.ToolStripItem>();

            public MSWinForms.ContextMenuStrip menu;
            public MSWinForms.ListView lv;

            public Options options;

            public MSWinForms.ToolStripMenuItem FindFirstMenu;
            public MSWinForms.ToolStripMenuItem FindNextMenu;
            public MSWinForms.ToolStripMenuItem CopyItemMenu;
            public MSWinForms.ToolStripMenuItem SelectAllMenu;
            public MSWinForms.ToolStripMenuItem DeSelectAllMenu;
            public MSWinForms.ToolStripMenuItem DeleteMenu;
        }

        public class UserMenuItem
        {
            public object Tag;
            public string Text;
        }

        public delegate bool delOnGenColItemMenuItem(
            int ColIdx,
            string ColText,
            out UserMenuItem [] Items);

        public delegate string delUserFindDialog(string FindStr);

        public class Options
        {
            public MenuFlags MFlags = MenuFlags.All;
            public string CopyItemsSeparator = " ";
            public string ExportDefaultExtensions = FILTER_TEXT_DefaultExt;
            public string ExportDefaultFilter = FILTER_TEXT_Filter;
            public string ExportDefaultDirectory;
            public bool WantColSorting = true;
            public bool FindNextMultiSelect = false;

            public delOnGenColItemMenuItem OnGenColItemMenuItem = null;
            public EventHandler OnColItemMenuClick = null;
            public EventHandler OnAfterDelete = null;
            public delUserFindDialog UserFindDialog = null;
        }

        #region Extension methods
        public static string GetItemsString(
            this MSWinForms.ListViewItem lvi,
            string SurroundL = "\"",
            string SurroundR = "\"",
            string Join = "\t")
        {
            List<string> s = new List<string>();
            foreach (MSWinForms.ListViewItem.ListViewSubItem CurSub in lvi.SubItems)
                s.Add(string.Format("{0}{1}{2}", SurroundL, CurSub.Text, SurroundR));

            return string.Join(Join, s);
        }

        public static void SelectDeselectAll(
            this MSWinForms.ListView lv,
            bool bSelected = true)
        {
            lv.BeginUpdate();

            bool bChk = lv.CheckBoxes;
            foreach (MSWinForms.ListViewItem lvi in lv.Items)
            {
                if (bChk)
                    lvi.Checked = bSelected;
                else
                    lvi.Selected = bSelected;
            }

            lv.EndUpdate();
        }

        #endregion

        #region Menu handlers
        private static void menuCommonLVCopyItem_Click(
            object sender,
            EventArgs e)
        {
            var lvCtx = GetCommonLVContext(sender);
            if (lvCtx == null)
                return;

            var sb = new StringBuilder();
            foreach (MSWinForms.ListViewItem lvi in lvCtx.lv.SelectedItems)
                sb.AppendLine(lvi.GetItemsString(Join: " "));

            string str = sb.ToString();
            if (string.IsNullOrEmpty(str))
                return;

            MSWinForms.Clipboard.Clear();
            MSWinForms.Clipboard.SetText(str);
        }

        private static void menuCommonLVDeleteItem_Click(
            object sender,
            EventArgs e)
        {
            var lvCtx = GetCommonLVContext(sender);
            if (lvCtx == null)
                return;

            lvCtx.lv.BeginUpdate();
            foreach (MSWinForms.ListViewItem lvi in lvCtx.lv.SelectedItems)
            {
                if (lvCtx.options.OnAfterDelete != null)
                    lvCtx.options.OnAfterDelete(lvi, e);
                lvCtx.lv.Items.Remove(lvi);
            }

            lvCtx.lv.EndUpdate();
        }

        private static void menuCommonLVFindFirst_Click(
            object sender,
            EventArgs e)
        {
            var lvCtx = GetCommonLVContext(sender);
            if (lvCtx == null)
                return;

            string str;
            
            if (lvCtx.options.UserFindDialog != null)
            {
                str = lvCtx.options.UserFindDialog(lvCtx.FindString);
            }
            else
            {
                str = UIUtils.InputString(
                        lvCtx.FindString,
                        "Find");
            }

            if (string.IsNullOrEmpty(str))
                return;

            lvCtx.SearchPos = 0;
            lvCtx.FindString = str;
            menuCommonLVFindNext_Click(sender, e);
        }

        private static void menuCommonLVSelectAll_Click(
            object sender,
            EventArgs e)
        {
            var lvCtx = GetCommonLVContext(sender);
            if (lvCtx != null)
                lvCtx.lv.SelectDeselectAll();
        }

        private static void menuCommonLVDeSelectAll_Click(
            object sender,
            EventArgs e)
        {
            var lvCtx = GetCommonLVContext(sender);
            if (lvCtx != null)
                lvCtx.lv.SelectDeselectAll(false);
        }

        private static void menuCommonLVExportToTextFile_Click(
            object sender,
            EventArgs e)
        {
            var lvCtx = GetCommonLVContext(sender);
            if (lvCtx == null)
                return;

            var lv = lvCtx.lv;

            string fn = Dialogs.FileSysDialogs.BrowseFile(
                lvCtx.options.ExportDefaultDirectory?? Path.GetDirectoryName((new FileInfo(global::System.Reflection.Assembly.GetExecutingAssembly().Location)).FullName),
                "",
                lvCtx.options.ExportDefaultExtensions,
                lvCtx.options.ExportDefaultFilter,
                true);
            if (string.IsNullOrEmpty(fn))
                return;

            using (TextWriter Out = new global::System.IO.StreamWriter(fn, false))
            {
                // Write column header
                foreach (MSWinForms.ColumnHeader Cur in lv.Columns)
                {
                    Out.Write("\"" + Cur.Text + "\"");
                    Out.Write(lvCtx.options.CopyItemsSeparator);
                }
                Out.WriteLine();

                foreach (MSWinForms.ListViewItem Item in lv.Items)
                    Out.WriteLine(Item.GetItemsString());

                Out.Close();
            }
        }

        private static bool DeselectPreviousSearchPos(ContextMenuTag lvCtx)
        {
            bool bDeSelected =
                   !lvCtx.options.FindNextMultiSelect
                && lvCtx.SearchPos != 0
                && (lvCtx.SearchPos - 1) < lvCtx.lv.Items.Count;

            // Deselect previous finding
            if (bDeSelected)
                lvCtx.lv.Items[lvCtx.SearchPos - 1].Selected = false;

            return bDeSelected;
        }

        private static void menuCommonLVFindNext_Click(
            object sender,
            EventArgs e)
        {
            var lvCtx = GetCommonLVContext(sender);
            if (lvCtx == null)
                return;

            // No string to lookup?
            if (string.IsNullOrEmpty(lvCtx.FindString))
                return;

            for (int i = lvCtx.SearchPos, c = lvCtx.lv.Items.Count; i < c; i++)
            {
                MSWinForms.ListViewItem lvi = lvCtx.lv.Items[i];
                foreach (MSWinForms.ListViewItem.ListViewSubItem lvsi in lvi.SubItems)
                {
                    // Deselect current search items (in case it was previously selected)
                    lvi.Selected = false;

                    if (lvsi.Text.IndexOf(lvCtx.FindString, StringComparison.OrdinalIgnoreCase) != -1)
                    {
                        DeselectPreviousSearchPos(lvCtx);

                        lvi.Selected = true;
                        lvi.EnsureVisible();
                        lvCtx.lv.FocusedItem = lvi;

                        // Remember the search position so search next works properly
                        lvCtx.SearchPos = i + 1;
                        return;
                    }
                }
            }

            // Nothing found, reset search position
            DeselectPreviousSearchPos(lvCtx);

            lvCtx.SearchPos = 0;
        }

        private static void menuCommonLV_Closed(
            object sender,
            MSWinForms.ToolStripDropDownClosedEventArgs e)
        {
            // Get context
            var lvCtx = GetCommonLVContext(sender);
            if (lvCtx == null || lvCtx.GenColItemMenu == null)
                return;

            // Remove the user menus
            foreach (var mi in lvCtx.GenColItemMenu)
                lvCtx.menu.Items.Remove(mi);

            // Clear the list
            lvCtx.GenColItemMenu.Clear();
        }

        private static void menuCommonLV_Opening(
            object sender,
            CancelEventArgs e)
        {
            // Get context
            var lvCtx = GetCommonLVContext(sender);
            if (lvCtx == null)
                return;

            // Bail out if no items selected
            var lv = lvCtx.lv;
            if (lv.SelectedItems.Count == 0)
                return;

            // Get the first selected item
            var lvi = lv.SelectedItems[0];

            //
            // Determine the column index where the click occured
            Point mousePosition = lv.PointToClient(MSWinForms.Control.MousePosition);
            MSWinForms.ListViewHitTestInfo hit = lv.HitTest(mousePosition);
            if (hit.SubItem == null)
                return;

            int ColIdx = hit.Item.SubItems.IndexOf(hit.SubItem);

            //
            // Call user callback to get some contextual info
            UserMenuItem[] UserMenus;
            bool bProceed = lvCtx.options.OnGenColItemMenuItem(
                                ColIdx,
                                lvi.SubItems[ColIdx].Text,
                                out UserMenus);
            if (!bProceed)
            {
                e.Cancel = true;
                return;
            }

            // Generate a dynamic menu item
            lvCtx.GenColItemMenu.Add(new MSWinForms.ToolStripSeparator());
            foreach (var m in UserMenus)
            {
                var menu = new MSWinForms.ToolStripMenuItem()
                {
                    Text = m.Text,
                    Tag = m.Tag,
                };
                menu.Click += lvCtx.options.OnColItemMenuClick;
                lvCtx.GenColItemMenu.Add(menu);
            }

            // Insert the user menus
            foreach (var mi in lvCtx.GenColItemMenu)
                lvCtx.menu.Items.Add(mi);
        }

        #endregion

        #region LV handlers
        private static void lvCommon_ColumnClick(
           object sender,
           MSWinForms.ColumnClickEventArgs e)
        {
            var lvCtx = GetCommonLVContext(sender);
            if (lvCtx == null)
                return;

            var lv = lvCtx.lv;

            SimpleListViewItemComparer sorter = lv.ListViewItemSorter as SimpleListViewItemComparer;

            if (sorter == null)
            {
                sorter = new SimpleListViewItemComparer(e.Column);
                lv.ListViewItemSorter = sorter;
            }
            else
            {
                sorter.Column = e.Column;
            }

            sorter.Ascending = !sorter.Ascending;

            lv.Sort();
        }

        private static ContextMenuTag GetCommonLVContext(object sender)
        {
            object Tag = null;
            if (sender is MSWinForms.ToolStripMenuItem)
                Tag = (sender as MSWinForms.ToolStripMenuItem).Owner.Tag;
            else if (sender is MSWinForms.ListView)
                Tag = (sender as MSWinForms.ListView).Tag;
            else if (sender is MSWinForms.ContextMenuStrip)
                Tag = (sender as MSWinForms.ContextMenuStrip).Tag;

            return Tag as ContextMenuTag;
        }
        #endregion

        #region Public methods
        public static ContextMenuTag CreateCommonMenuItems(
            MSWinForms.ContextMenuStrip Menu,
            MSWinForms.ListView LV,
            Options options = null)
        {
            // Use default options if none were passed
            if (options == null)
                options = new Options();

            // If no menu passed, use the menu associated with the LV
            if (Menu == null)
                Menu = LV.ContextMenuStrip;

            //
            // Create the context
            var Context = new ContextMenuTag()
            {
                lv = LV,
                options = options,
                menu = Menu
            };

            var DynMenus = new List<MSWinForms.ToolStripItem>();

            //
            // Copy/Select All
            if (options.MFlags.HasFlag(MenuFlags.CopyAndSelect))
            {
                // Copy
                Context.CopyItemMenu = new MSWinForms.ToolStripMenuItem()
                {
                    ShortcutKeys = (MSWinForms.Keys.Control | MSWinForms.Keys.C),
                    Text = "Copy"
                };
                Context.CopyItemMenu.Click += new EventHandler(menuCommonLVCopyItem_Click);

                // Select All
                Context.SelectAllMenu = new MSWinForms.ToolStripMenuItem()
                {
                    ShortcutKeys = (MSWinForms.Keys.Control | MSWinForms.Keys.A),
                    Text = "Select all"
                };
                Context.SelectAllMenu.Click += new EventHandler(menuCommonLVSelectAll_Click);

                // DeSelect All
                Context.DeSelectAllMenu = new MSWinForms.ToolStripMenuItem()
                {
                    ShortcutKeys = (MSWinForms.Keys.Control | MSWinForms.Keys.D),
                    Text = "De-Select all"
                };
                Context.DeSelectAllMenu.Click += new EventHandler(menuCommonLVDeSelectAll_Click);

                DynMenus.AddRange(new MSWinForms.ToolStripItem[]
                {
                    Context.CopyItemMenu,
                    Context.SelectAllMenu,
                    Context.DeSelectAllMenu,
                    new MSWinForms.ToolStripSeparator(),
                });
            }

            //
            // Find
            if (options.MFlags.HasFlag(MenuFlags.Find))
            {
                // FindFirst
                Context.FindFirstMenu = new MSWinForms.ToolStripMenuItem()
                {
                    ShortcutKeys = (MSWinForms.Keys.Control | MSWinForms.Keys.F),
                    Text = "Find",
                };
                Context.FindFirstMenu.Click += new EventHandler(menuCommonLVFindFirst_Click);

                // FindNext
                Context.FindNextMenu = new MSWinForms.ToolStripMenuItem()
                {
                    ShortcutKeys = MSWinForms.Keys.F3,
                    Text = "Find Next",
                };
                Context.FindNextMenu.Click += new EventHandler(menuCommonLVFindNext_Click);

                DynMenus.AddRange(new MSWinForms.ToolStripItem[]
                {
                    Context.FindFirstMenu,
                    Context.FindNextMenu,
                    new MSWinForms.ToolStripSeparator(),
                });
            }

            //
            // Delete
            if (options.MFlags.HasFlag(MenuFlags.Delete))
            {
                // Delete
                Context.DeleteMenu = new MSWinForms.ToolStripMenuItem()
                {
                    ShortcutKeys = MSWinForms.Keys.Delete,
                    Text = "Delete"
                };
                Context.DeleteMenu.Click += new EventHandler(menuCommonLVDeleteItem_Click);

                DynMenus.AddRange(new MSWinForms.ToolStripItem[]
                {
                    Context.DeleteMenu,
                    new MSWinForms.ToolStripSeparator()
                });
            }

            //
            // Export
            if (options.MFlags.HasFlag(MenuFlags.Export))
            {
                var ExportToTextFile = new MSWinForms.ToolStripMenuItem()
                {
                    ShortcutKeys = (MSWinForms.Keys.Control | MSWinForms.Keys.S),
                    Text = "Export to text file"
                };
                ExportToTextFile.Click += new EventHandler(menuCommonLVExportToTextFile_Click);

                DynMenus.AddRange(new MSWinForms.ToolStripItem[]
                {
                    ExportToTextFile,
                    new MSWinForms.ToolStripSeparator()
                });
            }

            //
            // Add all the dynamic menu items now
            if (DynMenus.Count > 0)
            {
                if (DynMenus[DynMenus.Count - 1] is MSWinForms.ToolStripSeparator)
                    DynMenus.RemoveAt(DynMenus.Count - 1);

                foreach (var m in DynMenus)
                    Menu.Items.Add(m);
            }

            //
            // Dynamic menu to be created based on column item click?
            if (options.OnGenColItemMenuItem != null && options.OnColItemMenuClick != null)
            {
                Menu.Opening += new CancelEventHandler(menuCommonLV_Opening);
                Menu.Closed  += new MSWinForms.ToolStripDropDownClosedEventHandler(menuCommonLV_Closed);
            }

            //
            // Install column sorter
            if (options.WantColSorting)
                LV.ColumnClick += new MSWinForms.ColumnClickEventHandler(lvCommon_ColumnClick);

            //
            // Associate the context with the tags of the listview and the menu
            Menu.Tag = Context;
            LV.Tag = Context;

            // Associate the menu with the LV
            LV.ContextMenuStrip = Menu;

            return Context;
        }
        #endregion
    }
}