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
using System.Windows.Forms;

namespace lallouslab.FluentLib.WinForms.Dialogs
{
    public static class FileSysDialogs
    {
        public static string BrowseFile(
            string DefaultDir,
            string DefaultFile,
            string DefExt,
            string Filter,
            bool bSaveDialog = false)
        {
            FileDialog FD;
            if (bSaveDialog)
                FD = new SaveFileDialog();
            else
                FD = new OpenFileDialog();

            FD.DefaultExt = DefExt;
            FD.Filter = Filter;
            FD.FileName = DefaultFile;
            if (!string.IsNullOrEmpty(DefaultDir))
                FD.InitialDirectory = DefaultDir;

            return (FD.ShowDialog() == DialogResult.OK) ? FD.FileName : null;
        }

        public static string BrowseFolder(string DefaultDir)
        {
            FolderBrowserDialog FBD = new FolderBrowserDialog();
            FBD.SelectedPath = DefaultDir;

            return (FBD.ShowDialog() == DialogResult.OK) ? FBD.SelectedPath : null;
        }
    }
}
