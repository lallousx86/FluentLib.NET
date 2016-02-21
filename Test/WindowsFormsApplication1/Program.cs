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
            Form f = null;
            if (args.Length > 0)
            {
                string formName = args[0];
                if (formName.Equals("listviewtest", StringComparison.OrdinalIgnoreCase))
                    f = new ListViewTestForm();
            }
            if (f == null)
                f = new SelectorForm();
            Application.Run(f);
        }
    }
}
