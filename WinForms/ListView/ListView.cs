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
using System.Windows.Forms;

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
            All = 0xff,
            AllNoDelete = All & ~Delete,
        }

        public const string FILTER_TEXT_Filter = "Text Files (*.txt)|*.txt|All files (*.*)|*.*";
        public const string FILTER_TEXT_DefaultExt = "txt";

        public class ContextMenuTag
        {
            internal string FindString;
            internal int SearchPos = 0;
            internal ToolStripItem[] GenColItemMenu = new ToolStripItem[2] { null, null };

            public ContextMenuStrip menu;
            public ListView lv;

            public Options options;

            public ToolStripMenuItem FindFirstMenu;
            public ToolStripMenuItem FindNextMenu;
            public ToolStripMenuItem CopyItemMenu;
            public ToolStripMenuItem SelectAllMenu;
            public ToolStripMenuItem DeleteMenu;
        }

        public delegate void delOnGenColItemMenuItem(
            string ColText,
            out object Tag,
            out string Caption);

        public delegate string delUserFindDialog(string FindStr);

        public class Options
        {
            public MenuFlags MFlags = MenuFlags.All;
            public string CopyItemsSeparator = " ";
            public string ExportDefaultExtensions = FILTER_TEXT_DefaultExt;
            public string ExportDefaultFilter = FILTER_TEXT_Filter;
            public string ExportDefaultDirectory;
            public bool WantColSorting = true;

            public delOnGenColItemMenuItem OnGenColItemMenuItem = null;
            public EventHandler OnColItemMenuClick = null;
            public delUserFindDialog UserFindDialog = null;
        }

        #region Extension methods
        public static string GetItemsString(
            this ListViewItem lvi,
            string SurroundL = "\"",
            string SurroundR = "\"",
            string Join = "\t")
        {
            List<string> s = new List<string>();
            foreach (System.Windows.Forms.ListViewItem.ListViewSubItem CurSub in lvi.SubItems)
                s.Add(string.Format("{0}{1}{2}", SurroundL, CurSub.Text, SurroundR));

            return string.Join(Join, s);
        }
        public static void SelectAll(
            this System.Windows.Forms.ListView lv)
        {
            lv.BeginUpdate();
            foreach (System.Windows.Forms.ListViewItem lvi in lv.Items)
                lvi.Selected = true;
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
            foreach (ListViewItem lvi in lvCtx.lv.SelectedItems)
                sb.AppendLine(lvi.GetItemsString(Join: " "));

            string str = sb.ToString();
            if (string.IsNullOrEmpty(str))
                return;

            Clipboard.Clear();
            Clipboard.SetText(str);
        }

        private static void menuCommonLVDeleteItem_Click(
            object sender,
            EventArgs e)
        {
            var lvCtx = GetCommonLVContext(sender);
            if (lvCtx == null)
                return;

            lvCtx.lv.BeginUpdate();
            foreach (ListViewItem lvi in lvCtx.lv.SelectedItems)
                lvCtx.lv.Items.Remove(lvi);

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
                lvCtx.lv.SelectAll();
        }

        private static void menuCommonLVExportToTextFile_Click(
            object sender,
            EventArgs e)
        {
            var lvCtx = GetCommonLVContext(sender);
            if (lvCtx == null)
                return;

            var lv = lvCtx.lv;

            string fn = FileSysDialogs.BrowseFile(
                lvCtx.options.ExportDefaultDirectory?? Path.GetDirectoryName((new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location)).FullName),
                "",
                lvCtx.options.ExportDefaultExtensions,
                lvCtx.options.ExportDefaultFilter,
                true);
            if (string.IsNullOrEmpty(fn))
                return;

            using (TextWriter Out = new System.IO.StreamWriter(fn, false))
            {
                // Write column header
                foreach (ColumnHeader Cur in lv.Columns)
                {
                    Out.Write("\"" + Cur.Text + "\"");
                    Out.Write(lvCtx.options.CopyItemsSeparator);
                }
                Out.WriteLine();

                foreach (ListViewItem Item in lv.Items)
                    Out.WriteLine(Item.GetItemsString());

                Out.Close();
            }
        }

        private static void menuCommonLVFindNext_Click(
            object sender,
            EventArgs e)
        {
            var lvCtx = GetCommonLVContext(sender);
            if (lvCtx == null)
                return;

            if (string.IsNullOrEmpty(lvCtx.FindString))
                return;

            for (int i = lvCtx.SearchPos, c = lvCtx.lv.Items.Count; i < c; i++)
            {
                ListViewItem lvi = lvCtx.lv.Items[i];
                foreach (ListViewItem.ListViewSubItem lvsi in lvi.SubItems)
                {
                    lvi.Selected = false;
                    if (lvsi.Text.IndexOf(lvCtx.FindString, StringComparison.OrdinalIgnoreCase) != -1)
                    {
                        lvi.Selected = true;
                        lvi.EnsureVisible();
                        lvCtx.lv.FocusedItem = lvi;

                        lvCtx.SearchPos = i + 1;
                        return;
                    }
                }
            }

            // Nothing found, reset search position
            lvCtx.SearchPos = 0;
        }


        private static void menuCommonLV_Closed(
            object sender,
            ToolStripDropDownClosedEventArgs e)
        {
            // Get context
            var lvCtx = GetCommonLVContext(sender);
            if (lvCtx == null || lvCtx.GenColItemMenu == null)
                return;

            foreach (var mi in lvCtx.GenColItemMenu)
                lvCtx.menu.Items.Remove(mi);
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
            if (lv.SelectedItems.Count <= 0)
                return;

            // Get the first selected item
            var lvi = lv.SelectedItems[0];

            // Determine the column index where the click occured
            Point mousePosition = lv.PointToClient(Control.MousePosition);
            ListViewHitTestInfo hit = lv.HitTest(mousePosition);
            if (hit.SubItem == null)
                return;

            int idx = hit.Item.SubItems.IndexOf(hit.SubItem);

            // Call user callback to get some contextual info
            string MenuText;
            object MenuTag;

            lvCtx.options.OnGenColItemMenuItem(
                lvi.SubItems[idx].Text,
                out MenuTag,
                out MenuText);

            // Generate a dynamic menu item
            lvCtx.GenColItemMenu[0] = new ToolStripSeparator();
            lvCtx.GenColItemMenu[1] = new ToolStripMenuItem()
            {
                Text = MenuText,
                Tag = MenuTag,
            };
            lvCtx.GenColItemMenu[1].Click += lvCtx.options.OnColItemMenuClick;

            lvCtx.menu.Items.AddRange(lvCtx.GenColItemMenu);
        }

        #endregion

        #region LV handlers
        private static void lvCommon_ColumnClick(
           object sender,
           ColumnClickEventArgs e)
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
            if (sender is ToolStripMenuItem)
                Tag = (sender as ToolStripMenuItem).Owner.Tag;
            else if (sender is ListView)
                Tag = (sender as ListView).Tag;
            else if (sender is ContextMenuStrip)
                Tag = (sender as ContextMenuStrip).Tag;

            return Tag as ContextMenuTag;
        }
        #endregion

        #region Public methods
        public static ContextMenuTag CreateCommonMenuItems(
            ContextMenuStrip Menu,
            ListView LV,
            Options options = null)
        {
            if (options == null)
                options = new Options();

            var Context = new ContextMenuTag()
            {
                lv = LV,
                options = options,
                menu = Menu
            };

            // FindFirst
            Context.FindFirstMenu = new ToolStripMenuItem()
            {
                ShortcutKeys = (Keys.Control | System.Windows.Forms.Keys.F),
                Text = "Find",
            };
            Context.FindFirstMenu.Click += new EventHandler(menuCommonLVFindFirst_Click);

            // FindNext
            Context.FindNextMenu = new ToolStripMenuItem()
            {
                ShortcutKeys = Keys.F3,
                Text = "Find Next",
            };
            Context.FindNextMenu.Click += new EventHandler(menuCommonLVFindNext_Click);

            // Copy
            Context.CopyItemMenu = new ToolStripMenuItem()
            {
                ShortcutKeys = (Keys.Control | Keys.C),
                Text = "Copy"
            };
            Context.CopyItemMenu.Click += new EventHandler(menuCommonLVCopyItem_Click);

            // Select All
            Context.SelectAllMenu = new ToolStripMenuItem()
            {
                ShortcutKeys = (Keys.Control | Keys.A),
                Text = "Select all"
            };
            Context.SelectAllMenu.Click += new EventHandler(menuCommonLVSelectAll_Click);

            var ExportToTextFile = new ToolStripMenuItem()
            {
                ShortcutKeys = (Keys.Control | Keys.S),
                Text = "Export to text file"
            };
            ExportToTextFile.Click += new EventHandler(menuCommonLVExportToTextFile_Click);

            Context.DeleteMenu = new ToolStripMenuItem()
            {
                ShortcutKeys = Keys.Delete,
                Text = "Delete"
            };
            Context.DeleteMenu.Click += new EventHandler(menuCommonLVDeleteItem_Click);

            if (options.MFlags.HasFlag(MenuFlags.Find))
            {
                Menu.Items.AddRange(new ToolStripItem[]
                {
                    new ToolStripSeparator(),
                    Context.FindFirstMenu,
                    Context.FindNextMenu,
                });
            }

            if (options.MFlags.HasFlag(MenuFlags.Delete))
            {
                Menu.Items.AddRange(new ToolStripItem[]
                {
                    new ToolStripSeparator(),
                    Context.DeleteMenu
                });
            }

            if (options.MFlags.HasFlag(MenuFlags.Export))
            {
                Menu.Items.AddRange(new ToolStripItem[]
                {
                    new ToolStripSeparator(),
                    ExportToTextFile
                });
            }

            Menu.Items.AddRange(new ToolStripItem[]
            {
                new ToolStripSeparator(),
                Context.CopyItemMenu,
                Context.SelectAllMenu,
            });

            // Dynamic menu to be created based on column item click?
            if (options.OnGenColItemMenuItem != null && options.OnColItemMenuClick != null)
            {
                Menu.Opening += new CancelEventHandler(menuCommonLV_Opening);
                Menu.Closed += new ToolStripDropDownClosedEventHandler(menuCommonLV_Closed);
            }

            // Install column sorter
            if (options.WantColSorting)
                LV.ColumnClick += new ColumnClickEventHandler(lvCommon_ColumnClick);

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