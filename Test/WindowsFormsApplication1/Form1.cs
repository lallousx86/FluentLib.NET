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
    public partial class Form1 : Form
    {
        enum Z
        {
            V1,
            V2,
            V21,
            V22,
            V23,
            V3,
            Hi_Whats_Up
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(
            object sender, 
            EventArgs e)
        {
            var Names = typeof(Z).GetEnumNames();
            var StartColors = new Color[Names.Length];
            StartColors[0] = Color.Red;
            StartColors[1] = Color.Gray;
            StartColors[2] = Color.Orange;

            var f = new lallouslab.FluentLib.WinForms.Dialogs.NamedItemsColorChooser(
                Names,
                StartColors)
            {
                Text = "Choose color"
            };
            
            f.ShowDialog();
        }
    }
}
