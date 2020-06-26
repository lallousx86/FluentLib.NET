/* ----------------------------------------------------------------------------- 
* .NET FluentLib - Copyright (c) Elias Bachaalany <elias.bachaalany@gmail.com>
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
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace lallouslab.FluentLib.WinForms.Controls.RichTextBox
{
    public class FastRichTextBox
    {
        private class FormatInfo
        {
            public int begin, end;
            public Color? color;
            public FontStyle? style;
        }
        private List<FormatInfo> Formats = new List<FormatInfo>();
        private StringBuilder Text = new StringBuilder();
        private System.Windows.Forms.RichTextBox richtext;

        public FastRichTextBox(System.Windows.Forms.RichTextBox richtext)
        {
            this.richtext = richtext;
        }

        public void ApplyFormatting()
        {
            richtext.Text = Text.ToString();
            foreach (var format in Formats)
            {
                richtext.Select(format.begin, format.end);

                if (format.color != null)
                    richtext.SelectionColor = format.color.Value;

                if (format.style != null)
                    richtext.SelectionFont = new Font(richtext.SelectionFont, format.style.Value);
            }
        }

        public void AddLine(
            string text,
            Color? color = null,
            FontStyle? style = null)
        {
            var len = text.Length;
            Text.Append(text);

            if (color == null && style == null)
                return;

            FormatInfo format = new FormatInfo()
            {
                begin = Text.Length - len,
                end = len
            };

            if (color != null)
                format.color = color.Value;

            if (style != null)
                format.style = style.Value;

            Formats.Add(format);
        }
    }
}
