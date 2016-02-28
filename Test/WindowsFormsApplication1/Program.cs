using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string [] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Dictionary<string, Type> TestForms = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase)
            {
                { "listviewtest", typeof(ListViewTestForm) },
                { "treeviewtest", typeof(TreeViewTestForm) }
            };
            Type formType = null;
            if (args.Length <= 0 || !TestForms.TryGetValue(args[0], out formType))
                formType = typeof(SelectorForm);

            // Did we selected a auto-click event from the selector form
            Form frm;
            if (formType == typeof(SelectorForm) && args.Length > 0)
            {
                var selform = new SelectorForm();

                // Set the auto click name
                selform.AutoClickButton = args[0];
                frm = selform;
            }
            else
            {
                // Select a test form
                frm = (Form)Activator.CreateInstance(formType);
            }

            Application.Run(frm);
        }
    }
}
