using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace SMS_SCHEDULER
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            


#if DEBUG
            Service1 ob = new Service1();
            //ob.ExecuteSQL("EXEC PROC_PROCESS_RFID_RAW_DATA");
            //ob.ExecuteSQLwithLOCK("EXEC TESTPR", "PROC_PROCESS_RFID_RAW_DATA"); 
            ob.Debug();
#else   
           ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Service1()
            };
            ServiceBase.Run(ServicesToRun);
#endif

        }
    }
}
