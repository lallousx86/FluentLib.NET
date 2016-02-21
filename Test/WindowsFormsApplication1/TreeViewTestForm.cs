using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class TreeViewTestForm : Form
    {
        public TreeViewTestForm()
        {
            InitializeComponent();
        }

        private void TreeViewTestForm_Load(
            object sender, 
            EventArgs e)
        {
            PopulateTree(treeView1);
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
