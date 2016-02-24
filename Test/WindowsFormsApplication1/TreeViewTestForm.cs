using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using lallouslab.FluentLib.WinForms.TreeView;

namespace WindowsFormsApplication1
{
    public partial class TreeViewTestForm : Form
    {
        private TreeViewNodesManipulationAdditions tvExt;

        public TreeViewTestForm()
        {
            InitializeComponent();
        }

        private void TreeViewTestForm_Load(
            object sender, 
            EventArgs e)
        {
            tvExt = new TreeViewNodesManipulationAdditions()
            {
                tv = treeView1,
                bWantMenu = true,
                bWantDragDrop = true,
                WantedActions = new List<TVNMA_ActionOptions>()
                {
                    TVNMA_ActionOptions.Indent,
                    TVNMA_ActionOptions.UnIndent,
                    TVNMA_ActionOptions.MoveAbove,
                    TVNMA_ActionOptions.MoveBelow,
                    TVNMA_ActionOptions.AddAbove,
                    TVNMA_ActionOptions.AddBelow,
                    TVNMA_ActionOptions.Rename,
                    TVNMA_ActionOptions.Delete,
                },
                OnBeforeAction = BeforeTVNMA,
                OnAfterAction = AfterTVNMA
            };
            tvExt.Attach();

            PopulateTree(treeView1);
        }

        private bool AfterTVNMA(TVNMA_ActionArgs Args)
        {
            Text = ArgsToText(Args, false);
            return true;
        }

        private bool BeforeTVNMA(TVNMA_ActionArgs Args)
        {
            switch (Args.Action)
            {
                case TVNMA_Actions.Add_Above:
                    Args.TextNew = "New node above - " + Args.TextOld;
                    break;

                case TVNMA_Actions.Add_Below:
                    Args.TextNew = "New node below - " + Args.TextOld;
                    break;
                case TVNMA_Actions.Rename:
                    Args.TextNew = Args.TextOld + "*";
                    break;
            }

            Text = ArgsToText(Args, true);
            Args.Tag = Args.SrcNode.FullPath;
            return true;
        }

        private string ArgsToText(
            TVNMA_ActionArgs Args, 
            bool bBefore)
        {
            // Deleted node?
            if (!bBefore && Args.Action == TVNMA_Actions.Delete)
            {
                if (Args.DestNode != null)
                {
                    return string.Format("Action={0}, OldPath=[{1}] CurSelPath=[{2}]",
                        Args.Action.ToString(),
                        Args.Tag as string,
                        Args.DestNode.FullPath);
                }
                else
                {
                    return "Empty!";
                }
            }
            else
            {
                return string.Format("@{0},Action={1},OldPath=[{2}] NewPath[{3}]",
                    Args.SrcNode.Index,
                    Args.Action.ToString(),
                    Args.Tag as string,
                    Args.SrcNode.FullPath);
            }
        }

        private void PopulateTree(TreeView tv1)
        {
            var rn = tv1.Nodes.Add("*");
            for (int i = 1; i <= 4; i++)
            {
                var t = rn.Nodes.Add("hello #" + i);
                for (int j = 1; j <= 5; j++)
                {
                    var u = t.Nodes.Add("item #" + i + "." + j);
                    for (int k = 1; k <= 5; k++)
                        u.Nodes.Add("item #" + i + "." + j + "." + k);
                }
            }

            rn.Nodes.Add("Root 2");

            rn.ExpandAll();
        }
    }
}
