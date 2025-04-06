using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TechXureWinApp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        //[STAThread]
        //static void Main()
        //{
        //    Application.EnableVisualStyles();
        //    Application.SetCompatibleTextRenderingDefault(false);
        //    //Application.Run(new Form3());
        //    Application.Run(new Form1());
        //    //Application.Run(new Form2());
        //}

        //private static string appGuid = "ab9823dc2983e-f89273ac8723d-ef2893bc92-4"; // FOR WABA RCS
        private static string appGuid = "ab9823dc2983e-f89273ac8723d-ef2893bc92-534e"; // FOR WABA CHAT
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                using (Mutex mutex = new Mutex(false, "Global\\" + appGuid))
                {
                    if (!mutex.WaitOne(0, false))
                    {
                        MessageBox.Show("WABA / RCS Application is already running.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Stop); // FOR WABA RCS
                        //MessageBox.Show("WABA Chat Application is already running.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Stop); // FOR WABA CHAT
                        Application.Exit();
                    }
                    else
                    {
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        Application.Run(new TechXureWinApp.WabaChat()); // FOR WABA CHAT
                       // Application.Run(new Form1()); // FOR WABA RCS
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }

        }
    }
}
