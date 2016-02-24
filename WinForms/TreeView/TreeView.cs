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
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lallouslab.FluentLib.WinForms.TreeView
{
    public enum TVNMA_NodeIconIndex : int
    {
        Node = 0,
        Closed = 1,
        Opened = 2
    }

    [Flags]
    public enum TVNMA_Actions
    {
        None = 0x00,

        Add_Above           = 0x01,
        Add_Below           = 0x02,
        Add_Both            = Add_Above | Add_Below,

        Move_Above          = 0x04,
        Move_Below          = 0x08,
        Move_Both           = Move_Above | Move_Below,

        Move_Into_Top       = 0x10,
        Move_Into_Bottom    = 0x20,
        Move_Into_Both      = Move_Into_Top | Move_Into_Bottom,

        Indent              = 0x40,
        UnIndent            = 0x80,
        Indent_Both         = Indent | UnIndent,

        Rename              = 0x100,
        Delete              = 0x200,

        RenOrDel            = Rename | Delete,

        // Internal flag
        Internal            = 0x400,

        // All actions
        All                 = Add_Both       | Move_Both |
                              Move_Into_Both | Indent_Both |
                              Rename         | Delete,
    }

    public class TVNMA_ActionArgs
    {
        public TVNMA_Actions Action = TVNMA_Actions.None;
        public object Tag;
        public TreeNode SrcNode;
        public TreeNode DestNode;

        public string TextNew;
        public string TextOld;

        public TVNMA_ActionArgs Clone()
        {
            return new TVNMA_ActionArgs()
            {
                Action = Action,
                Tag = Tag,
                SrcNode = SrcNode,
                DestNode = DestNode,
                TextNew = TextNew,
                TextOld = TextOld
            };
        }
    }

    public class TVNMA_ActionOptions
    {
        public Keys Shortcut = Keys.None;
        public string MenuText;
        public TVNMA_Actions Action;
        public ToolStripMenuItem MenuItem;

        public static TVNMA_ActionOptions Indent
        {
            get
            {
                return new TVNMA_ActionOptions()
                {
                    MenuText = "&Indent",
                    Shortcut = Keys.Alt | Keys.Right,
                    Action = TVNMA_Actions.Indent
                };
            }
        }

        public static TVNMA_ActionOptions UnIndent
        {
            get
            {
                return new TVNMA_ActionOptions()
                {
                    MenuText = "U&nIndent",
                    Shortcut = Keys.Alt | Keys.Left,
                    Action = TVNMA_Actions.UnIndent
                };
            }
        }

        public static TVNMA_ActionOptions AddAbove
        {
            get
            {
                return new TVNMA_ActionOptions()
                {
                    MenuText = "Add A&bove",
                    Shortcut = Keys.Alt | Keys.Shift | Keys.Insert,
                    Action = TVNMA_Actions.Add_Above
                };
            }
        }

        public static TVNMA_ActionOptions AddBelow
        {
            get
            {
                return new TVNMA_ActionOptions()
                {
                    MenuText = "Add Belo&w",
                    Shortcut = Keys.Alt | Keys.Insert,
                    Action = TVNMA_Actions.Add_Below
                };
            }
        }

        public static TVNMA_ActionOptions MoveBelow
        {
            get
            {
                return new TVNMA_ActionOptions()
                {
                    MenuText = "Move Be&low",
                    Shortcut = Keys.Control | Keys.Shift | Keys.Down,
                    Action = TVNMA_Actions.Move_Below
                };
            }
        }

        public static TVNMA_ActionOptions MoveAbove
        {
            get
            {
                return new TVNMA_ActionOptions()
                {
                    MenuText = "Move Abo&ve",
                    Shortcut = Keys.Control | Keys.Shift | Keys.Up,
                    Action = TVNMA_Actions.Move_Above
                };
            }
        }

        public static TVNMA_ActionOptions MoveIntoTop
        {
            get
            {
                return new TVNMA_ActionOptions()
                {
                    MenuText = "Move In&to top",
                    Shortcut = Keys.Control | Keys.Shift | Keys.M,
                    Action = TVNMA_Actions.Move_Into_Top
                };
            }
        }

        public static TVNMA_ActionOptions MoveIntoBottom
        {
            get
            {
                return new TVNMA_ActionOptions()
                {
                    MenuText = "Move Into botto&m",
                    Shortcut = Keys.Control | Keys.Shift | Keys.B,
                    Action = TVNMA_Actions.Move_Into_Bottom
                };
            }
        }

        public static TVNMA_ActionOptions Rename
        {
            get
            {
                return new TVNMA_ActionOptions()
                {
                    MenuText = "&Rename",
                    Shortcut = Keys.F2,
                    Action = TVNMA_Actions.Rename
                };
            }
        }

        public static TVNMA_ActionOptions Delete
        {
            get
            {
                return new TVNMA_ActionOptions()
                {
                    MenuText = "&Delete",
                    Shortcut = Keys.Delete,
                    Action = TVNMA_Actions.Delete
                };
            }
        }
    }

    public delegate bool TVNMA_OnAction(TVNMA_ActionArgs Args);

    public class TreeViewNodesManipulationAdditions
    {
        private readonly Type TV_NODE = typeof(TreeNode);
        #region User arguments
        public TVNMA_OnAction OnAfterAction, OnBeforeAction;
        public System.Windows.Forms.TreeView tv;
        public ContextMenuStrip Menu;
        public long nAutoExpandMS = 0;
        public bool bWantMenu;
        public bool bWantDragDrop;
        public List<TVNMA_ActionOptions> WantedActions;
        #endregion

        private Stopwatch m_DragOverAutoExpandStopWatch = new Stopwatch();
        private TreeNode m_LastDragOverDestTreeNode = null;
        private bool m_bDragInto;
        private bool m_bDragAbove;

        private ImageList ImgList = new ImageList();
        private TVNMA_Actions m_AllActions;
        private List<ToolStripItem> m_AllMenuItems;

        private void SetupImgList()
        {
            // Assign image list.
            // (Must be parallel to TVNMA_NodeIconIndex)
            ImgList.Images.AddRange(new Image[] 
            {
                TreeViewVaultNodes.node,
                TreeViewVaultNodes.node_closed,
                TreeViewVaultNodes.node_opened
            });
            ImgList.TransparentColor = Color.Blue;
            tv.ImageList = ImgList;
        }

        public void Attach()
        {
            SetupImgList();
            CreateMenuItems();
            SetupDragDropHandlers();
        }

        private void SetupDragDropHandlers()
        {
            if (!bWantDragDrop)
                return;

            tv.AllowDrop = true;
            //
            // ItemDrag
            //
            tv.ItemDrag += new ItemDragEventHandler(delegate (object sender, ItemDragEventArgs e)
            {
                tv.DoDragDrop(
                    e.Item,
                    DragDropEffects.Copy | DragDropEffects.Move | DragDropEffects.Link);
            });

            //
            // DragEnter
            //
            tv.DragEnter += new DragEventHandler(delegate (object sender, DragEventArgs e)
            {
                if (e.Data.GetDataPresent(TV_NODE))
                {
                    e.Effect = GetDragEffectFromKeyState(e.KeyState);
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }
            });

            //
            // DragOver
            //
            tv.DragOver += new DragEventHandler(delegate (object sender, DragEventArgs e)
            {
                if (!e.Data.GetDataPresent(TV_NODE))
                    return;

                var treeView = sender as System.Windows.Forms.TreeView;
                Point pt = treeView.PointToClient(new Point(e.X, e.Y));

                TreeNode DestTreeNode = treeView.GetNodeAt(pt);

                if (DestTreeNode == null)
                    return;

                TreeNode SrcTreeNode = e.Data.GetData(TV_NODE) as TreeNode;

                // Select the destination node
                treeView.SelectedNode = DestTreeNode;

                // Make sure we are not going to drop the src node into any of its parent nodes
                var parent = DestTreeNode;
                while (parent != null)
                {
                    if (parent == SrcTreeNode)
                    {
                        e.Effect = DragDropEffects.None;
                        return;
                    }

                    // Walk up the tree to figure out that the destination node is not inside its parent
                    parent = parent.Parent;
                }

                if (nAutoExpandMS > 0)
                {
                    // If the destination node has changed, then reset the stopwatch
                    if (m_LastDragOverDestTreeNode != DestTreeNode)
                    {
                        m_DragOverAutoExpandStopWatch.Restart();
                    }
                    else
                    {
                        // Enough time elapsed? User's intent to open this node
                        if (m_DragOverAutoExpandStopWatch.ElapsedMilliseconds > nAutoExpandMS)
                            DestTreeNode.Expand();
                    }

                    // Remember the last node
                    m_LastDragOverDestTreeNode = DestTreeNode;
                }
                e.Effect = GetDragEffectFromKeyState(e.KeyState);
            });

            //
            // DragDrop
            //
            tv.DragDrop += new DragEventHandler(delegate (object sender, DragEventArgs e)
            {
                if (!e.Data.GetDataPresent(TV_NODE))
                {
                    e.Effect = DragDropEffects.None;
                    return;
                }

                var tv = sender as System.Windows.Forms.TreeView;
                Point pt = tv.PointToClient(new Point(e.X, e.Y));

                TreeNode DestTreeNode = tv.GetNodeAt(pt);
                TreeNode SrcTreeNode = e.Data.GetData(TV_NODE) as TreeNode;

                // Remember the source node's parent (before the action takes place).
                // We need this so we can properly update its image indexes.
                // For example, if we move out its last child, then it becomes a regular node.
                var SrcParent = SrcTreeNode.Parent;

                // Create a TreeNode Action
                var Args = new TVNMA_ActionArgs()
                {
                    SrcNode = SrcTreeNode,
                    DestNode = DestTreeNode
                };

                // Convert KeyStates into an elaborate action
                if (m_bDragInto && m_bDragAbove)
                {
                    Args.Action = TVNMA_Actions.Move_Into_Top;
                }
                else if (m_bDragInto && !m_bDragAbove)
                {
                    Args.Action = TVNMA_Actions.Move_Into_Bottom;
                }
                else if (m_bDragAbove)
                {
                    Args.Action = TVNMA_Actions.Move_Above;
                }
                else if (m_bDragInto)
                {
                    Args.Action = TVNMA_Actions.Move_Into_Bottom;
                }

                // No action intended yet, let's try to guess user's intent
                if (Args.Action == TVNMA_Actions.None)
                {
                    // Must be moving to a parent node
                    if (DestTreeNode.IsExpanded)
                    {
                        Args.Action = TVNMA_Actions.Move_Into_Bottom;
                    }
                    // Just a regular node, let's just move above
                    else
                    {
                        Args.Action = TVNMA_Actions.Move_Above;
                    }
                }
                CarryTVNodeAction(Args);

                DestTreeNode.SetAutoImageIndex();
                if (SrcParent != null)
                    SrcParent.SetAutoImageIndex();
            });

            //
            // BeforeCollapse
            //
            tv.BeforeCollapse += new TreeViewCancelEventHandler(delegate (object sender, TreeViewCancelEventArgs e)
            {
                e.Node.SetAllImageIndex(TVNMA_NodeIconIndex.Closed);
            });

            //
            // BeforeExpand
            //
            tv.BeforeExpand += new TreeViewCancelEventHandler(delegate (object sender, TreeViewCancelEventArgs e)
            {
                e.Node.SetAllImageIndex(TVNMA_NodeIconIndex.Opened);
            });
        }

        private void CreateMenuItems()
        {
            m_AllActions = TVNMA_Actions.None;
            if (WantedActions == null)
                return;

            // Create all menu items
            var _acts = new Dictionary<TVNMA_Actions, TVNMA_ActionOptions>();
            foreach (var act in WantedActions)
            {
                if (bWantMenu)
                {
                    _acts[act.Action] = act;
                    act.MenuItem = new ToolStripMenuItem()
                    {
                        ShortcutKeys = act.Shortcut,
                        Text = act.MenuText
                    };
                }
                m_AllActions |= act.Action;
            }

            if (!bWantMenu)
                return;

            if (Menu == null)
                Menu = new ContextMenuStrip();

            m_AllMenuItems = new List<ToolStripItem>();
            var AddSep = new Action(() =>
            {
                m_AllMenuItems.Add(new ToolStripSeparator());
            });

            //
            // Assign Ident menu item actions
            //
            TVNMA_ActionOptions Opts;
            if (_acts.TryGetValue(TVNMA_Actions.Indent, out Opts))
            {
                Opts.MenuItem.Click += new EventHandler(delegate (object o, EventArgs ev)
                {
                    CarryTVNodeAction(new TVNMA_ActionArgs()
                    {
                        Action = TVNMA_Actions.Indent,
                        SrcNode = tv.SelectedNode
                    });
                });
                m_AllMenuItems.Add(Opts.MenuItem);
            }

            if (_acts.TryGetValue(TVNMA_Actions.UnIndent, out Opts))
            {
                Opts.MenuItem.Click += new EventHandler(delegate (object o, EventArgs ev)
                {
                    CarryTVNodeAction(new TVNMA_ActionArgs()
                    {
                        Action = TVNMA_Actions.UnIndent,
                        SrcNode = tv.SelectedNode
                    });
                });
                m_AllMenuItems.Add(Opts.MenuItem);
            }

            if (m_AllActions.HasFlag(TVNMA_Actions.Indent_Both))
                AddSep();

            //
            // Assign Add menu item actions
            //
            if (_acts.TryGetValue(TVNMA_Actions.Add_Above, out Opts))
            {
                Opts.MenuItem.Click += new EventHandler(delegate (object o, EventArgs ev)
                {
                    CarryTVNodeAction(new TVNMA_ActionArgs()
                    {
                        Action = TVNMA_Actions.Add_Above,
                        SrcNode = tv.SelectedNode
                    });
                });
                m_AllMenuItems.Add(Opts.MenuItem);
            }

            if (_acts.TryGetValue(TVNMA_Actions.Add_Below, out Opts))
            {
                Opts.MenuItem.Click += new EventHandler(delegate (object o, EventArgs ev)
                {
                    CarryTVNodeAction(new TVNMA_ActionArgs()
                    {
                        Action = TVNMA_Actions.Add_Below,
                        SrcNode = tv.SelectedNode
                    });
                });
                m_AllMenuItems.Add(Opts.MenuItem);
            }

            if (m_AllActions.HasFlag(TVNMA_Actions.Add_Both))
                AddSep();

            //
            // Assign Move menu item actions
            //
            if (_acts.TryGetValue(TVNMA_Actions.Move_Above, out Opts))
            {
                Opts.MenuItem.Click += new EventHandler(delegate (object o, EventArgs ev)
                {
                    var Args = new TVNMA_ActionArgs()
                    {
                        Action = TVNMA_Actions.Move_Above,
                        SrcNode = tv.SelectedNode,
                        DestNode = tv.SelectedNode.PrevNode
                    };
                    if (Args.DestNode != null)
                        CarryTVNodeAction(Args);
                });
                m_AllMenuItems.Add(Opts.MenuItem);
            }

            if (_acts.TryGetValue(TVNMA_Actions.Move_Below, out Opts))
            {
                Opts.MenuItem.Click += new EventHandler(delegate (object o, EventArgs ev)
                {
                    var Args = new TVNMA_ActionArgs()
                    {
                        Action = TVNMA_Actions.Move_Below,
                        SrcNode = tv.SelectedNode,
                        DestNode = tv.SelectedNode.NextNode
                    };
                    if (Args.DestNode != null)
                        CarryTVNodeAction(Args);
                });
                m_AllMenuItems.Add(Opts.MenuItem);
            }

            if (m_AllActions.HasFlag(TVNMA_Actions.Move_Both))
                AddSep();

            //
            // Add Rename/Delete
            //
            if (_acts.TryGetValue(TVNMA_Actions.Rename, out Opts))
            {
                Opts.MenuItem.Click += new EventHandler(delegate (object o, EventArgs ev)
                {
                    CarryTVNodeAction(new TVNMA_ActionArgs()
                    {
                        Action = TVNMA_Actions.Rename,
                        SrcNode = tv.SelectedNode
                    });
                });
                m_AllMenuItems.Add(Opts.MenuItem);
            }

            if (_acts.TryGetValue(TVNMA_Actions.Delete, out Opts))
            {
                Opts.MenuItem.Click += new EventHandler(delegate (object o, EventArgs ev)
                {
                    CarryTVNodeAction(new TVNMA_ActionArgs()
                    {
                        Action = TVNMA_Actions.Delete,
                        SrcNode = tv.SelectedNode
                    });
                });
                m_AllMenuItems.Add(Opts.MenuItem);
            }

            if (m_AllActions.HasFlag(TVNMA_Actions.RenOrDel))
                AddSep();

            //
            // Add all the menu items
            //
            if (m_AllMenuItems[m_AllMenuItems.Count - 1] is ToolStripSeparator)
                m_AllMenuItems.RemoveAt(m_AllMenuItems.Count - 1);

            foreach (var mi in m_AllMenuItems)
                Menu.Items.Add(mi);

            tv.ContextMenuStrip = Menu;
        }

        private DragDropEffects GetDragEffectFromKeyState(int KeyState)
        {
            /*
            1 (bit 0)   The left mouse button. 
            2 (bit 1)   The right mouse button. 
            16 (bit 4)  The middle mouse button. 
            32 (bit 5)  The ALT key. 
            */

            // 8 (bit 3)   The CTRL key. 
            m_bDragInto = (KeyState & 8) != 0;

            // 4 (bit 2)   The SHIFT key. 
            m_bDragAbove = (KeyState & 4) != 0;

            return m_bDragAbove ? DragDropEffects.Link :
                   (m_bDragInto ? DragDropEffects.Copy : DragDropEffects.Move);
        }

        private bool CarryTVNodeAction(
            TVNMA_ActionArgs Args)
        {
            // SrcNode should always be not null
            if (Args.SrcNode == null)
                return false;

            // Capture old title before calling the callback
            Args.TextOld = Args.SrcNode.Text;

            // Internal flag allows us to call this function recursively if needed.
            bool bInternal = Args.Action.HasFlag(TVNMA_Actions.Internal);
            if (!bInternal && !DoOnBeforeAction(Args))
                return false;

            var tv = Args.SrcNode.TreeView;

            TreeNode PrevParent = Args.SrcNode.Parent == null ? tv.TopNode : Args.SrcNode.Parent;

            // Clear the internal flag
            Args.Action &= ~TVNMA_Actions.Internal;
            switch (Args.Action)
            {
                case TVNMA_Actions.Rename:
                    Args.SrcNode.Text = Args.TextNew;
                    break;

                case TVNMA_Actions.Add_Both:
                {
                    // Create new node with the desired title.
                    // (No need to insert the node into the tree since it will be moved.)
                    Args.DestNode = new TreeNode(Args.TextNew);

                    // Move the node above
                    Args.DestNode.MoveAboveOrBelow(
                        Args.SrcNode,
                        bAbove: Args.Action == TVNMA_Actions.Add_Above);
                    break;
                }

                case TVNMA_Actions.Add_Above:
                case TVNMA_Actions.Add_Below:
                    goto case TVNMA_Actions.Add_Both;

                case TVNMA_Actions.Move_Above:
                    Args.SrcNode.MoveAboveOrBelow(
                        Args.DestNode,
                        bAbove: true);
                    break;

                case TVNMA_Actions.Move_Below:
                    Args.SrcNode.MoveAboveOrBelow(
                        Args.DestNode,
                        bAbove: false);
                    break;

                case TVNMA_Actions.Move_Into_Top:
                    Args.SrcNode.MakeChildOf(
                        Args.DestNode,
                        bAppend: false);
                    break;

                case TVNMA_Actions.Move_Into_Bottom:
                    Args.SrcNode.MakeChildOf(
                        Args.DestNode,
                        bAppend: true);
                    break;

                case TVNMA_Actions.Indent:
                    Args.SrcNode.Indent();
                    break;

                case TVNMA_Actions.UnIndent:
                    Args.SrcNode.UnIndent();
                    break;

                case TVNMA_Actions.Delete:
                {
                    // Elect the new selected node after deleltion
                    
                    // NextNode?
                    Args.DestNode = Args.SrcNode.NextNode;
                    
                    // PrevNode?
                    if (Args.DestNode == null)
                        Args.DestNode = Args.SrcNode.PrevNode;

                    // Parent?
                    if (Args.DestNode == null)
                        Args.DestNode = Args.SrcNode.Parent;

                    // Delete the node
                    Args.SrcNode.Remove();

                    // Also clear the node reference so the caller knows
                    Args.SrcNode = null;

                    break;
                }

                default:
                    return false;
            }

            TreeNode VisSrcNode = (Args.Action == TVNMA_Actions.Delete) ? Args.DestNode : Args.SrcNode;

            if (VisSrcNode != null)
            {
                tv.SelectedNode = VisSrcNode;

                // Set new parent
                if (VisSrcNode.Parent == null && tv.TopNode != null)
                    tv.TopNode.SetAutoImageIndex();
                else
                    VisSrcNode.Parent.SetAutoImageIndex();
            }

            if (PrevParent != null)
                PrevParent.SetAutoImageIndex();

            return bInternal ? true : DoOnAfterAction(Args);
        }

        private bool DoOnAfterAction(TVNMA_ActionArgs Args)
        {
            return OnAfterAction == null ? true : OnAfterAction(Args);
        }

        private bool DoOnBeforeAction(TVNMA_ActionArgs Args)
        {
            return OnBeforeAction == null ? true : OnBeforeAction(Args);
        }
    }
}
