using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lallouslab.FluentLib.WinForms.TreeView
{
    public static class TreeNodeExtensions
    {
        /// <summary>
        // Make previous sibling's parent the parent of the selected node
        /// </summary>
        public static void Indent(this TreeNode node)
        {
            TreeNode prev = node.PrevNode;
            if (prev == null)
                return;

            TreeNode parent = node.Parent;
            node.Remove();
            prev.Nodes.Add(node);
        }

        /// <summary>
        // Make previous sibling's parent the parent of the selected node
        /// </summary>
        public static void UnIndent(this TreeNode node)
        {
            if (node == null || node.Parent == null)
                return;

            var NextSiblings = new List<TreeNode>();
            for (int i = node.Index + 1, c = node.Parent.Nodes.Count;
                 i < c;
                 i++)
            {
                var sibling = node.Parent.Nodes[i];
                NextSiblings.Add(sibling);
            }

            var grandpa = node.Parent.Parent;
            var parent = node.Parent;
            var tv = node.TreeView;

            // Remove self
            node.Remove();

            // Grandpa not the tree's root
            if (grandpa != null)
            {
                grandpa.Nodes.Insert(parent.Index + 1, node);
            }
            else
            {
                tv.Nodes.Insert(parent.Index + 1, node);
            }

            foreach (var sibling in NextSiblings)
            {
                sibling.Remove();
                node.Nodes.Add(sibling);
            }
            node.Expand();
        }

        /// <summary>
        /// Make as a child of another node
        /// </summary>
        public static void MakeChildOf(
            this TreeNode node,
            TreeNode NewParent,
            bool bAppend = false)
        {
            // Remove the original node
            node.Remove();

            if (bAppend)
            {
                // Add to the end of the parent
                NewParent.Nodes.Add(node);
            }
            else
            {
                // Insert at the top
                NewParent.Nodes.Insert(0, node);
            }
        }

        public static void SetAllImageIndex(
            this TreeNode node,
            TVNMA_NodeIconIndex Idx)
        {
            node.SelectedImageIndex = node.ImageIndex = (int)Idx;
        }

        public static void SetAutoImageIndex(this TreeNode node)
        {
            node.SetAllImageIndex(GetNodeImageIndex(node));
        }

        private static TVNMA_NodeIconIndex GetNodeImageIndex(this TreeNode Node)
        {
            return (Node.Nodes.Count == 0 ? TVNMA_NodeIconIndex.Node
                                       : (Node.IsExpanded ? TVNMA_NodeIconIndex.Opened : TVNMA_NodeIconIndex.Closed));
        }

        /// <summary>
        /// Move node above or below in the same parent
        /// </summary>
        public static void MoveAboveOrBelow(
            this TreeNode node,
            TreeNode RelativeNode,
            bool bAbove = true)
        {
            node.Remove();

            int idx = RelativeNode.Index;
            if (!bAbove)
                ++idx;

            var parent = RelativeNode.Parent;
            if (parent == null)
                RelativeNode.TreeView.Nodes.Insert(idx, node);
            else
                parent.Nodes.Insert(idx, node);
        }
    }
}
