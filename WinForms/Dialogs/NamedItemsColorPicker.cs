using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lallouslab.FluentLib.WinForms.Dialogs
{
    public class NamedItemsColorChooser : Form
    {
        string[] m_Names;
        Color[] m_SelColors = null;
        private ColorDialog clrDlg;

        public Color [] SelColors
        {
            get { return m_SelColors; }
        }

        public NamedItemsColorChooser(
            string[] Names,
            Color[] StartColors = null)
        {
            m_Names = Names;
            m_SelColors = StartColors == null || StartColors.Length != Names.Length ? new Color[Names.Length] : StartColors;

            SuspendLayout();

            ShowIcon = false;
            StartPosition = FormStartPosition.CenterParent;
            ShowInTaskbar = false;
            FormBorderStyle = FormBorderStyle.FixedToolWindow;

            BuildForm(Font);
            ResumeLayout(false);
            PerformLayout();
        }

        private void BuildForm(Font font)
        {
            int x = 10;
            int y = 10;

            clrDlg = new ColorDialog();

            var Btns = new List<Button>();
            int MaxWidth = 0;
            int idx = -1;
            foreach (string name in m_Names)
            {
                ++idx;

                var L = new Label()
                {
                    Text = string.Format("{0}:", name),
                    Top = y,
                    Left = x,
                    AutoSize = true,
                };

                var ts = TextRenderer.MeasureText(L.Text, font);
                if (ts.Width > MaxWidth)
                    MaxWidth = ts.Width;

                var B = new Button()
                {
                    Top = y,
                    Height = ts.Height * 2,
                    Width = L.Height * 3,
                    BackColor = m_SelColors[idx],
                    Tag = idx
                };

                B.Click += OnColorButtonClick;
                Btns.Add(B);

                Controls.Add(L);

                y += ts.Height + 20;
            }

            // Adjust color buttons left positions.
            // (We can do that now because we know the largest label width)
            foreach (var btn in Btns)
            {
                btn.Left = x + MaxWidth;
                Controls.Add(btn);
            }

            // 
            // OK button
            var BtnOk = new Button()
            {
                Text = "&OK",
                Left = x,
                Top = y,
                DialogResult = DialogResult.OK
            };
            Controls.Add(BtnOk);

            // 
            // Cancel Button
            Controls.Add(new Button()
            {
                Text = "&CANCEL",
                Left = x + BtnOk.Width + 20,
                Top = y,
                DialogResult = DialogResult.Cancel
            });

            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            AutoSize = true;
        }

        private void OnColorButtonClick(
            object sender, 
            EventArgs e)
        {
            var btn = sender as Button;
            int iBtn = (int)btn.Tag;
            clrDlg.Color = m_SelColors[iBtn];
            if (clrDlg.ShowDialog() == DialogResult.OK)
                btn.BackColor = m_SelColors[iBtn] = clrDlg.Color;
        }
    }
}
