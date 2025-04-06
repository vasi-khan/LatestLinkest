using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WhatsApp
{
    static class Program
    {
       
        private static string appGuid = "ab9823dc2983e-f89273ac8723d-ef2893b32423"; // FOR WABA CHAT BOT
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
                        MessageBox.Show("WABA Chat Bot Application is already running.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Stop); 
                        Application.Exit();
                    }
                    else
                    {
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        Application.Run(new WhatsApp.Form1()); 
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
