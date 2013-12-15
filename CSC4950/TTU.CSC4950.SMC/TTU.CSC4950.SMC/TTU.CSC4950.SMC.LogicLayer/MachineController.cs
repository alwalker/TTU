using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using TTU.CSC4950.SMC.FrameWork;
using TTU.CSC4950.SMC.DataLayer;

namespace TTU.CSC4950.SMC.LogicLayer
{
    public class MachineController
    {
        /// <summary>
        /// Initializes the machine.
        /// </summary>
        /// <returns></returns>
        public static Machine InitializeMachine()
        {
            Machine mac = Machine.Instance(ConfigurationManager.AppSettings["MachineID"].ToString());

            return mac;
        }

        /// <summary>
        /// Gets all unique machine Ids.
        /// </summary>
        /// <returns></returns>
        public static List<string> GetMachineIDs()
        {
            try
            {
                return MachineDAL.GetMachineIDs();
            }
            catch
            {
                throw;
            }
        }
    }    
}
