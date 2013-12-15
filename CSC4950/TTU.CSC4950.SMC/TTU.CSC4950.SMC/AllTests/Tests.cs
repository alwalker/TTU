using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Collections;

namespace AllTests
{
    public class Tests
    {
        [Suite]
        public static IEnumerable Suite
        {
            get 
            {
                ArrayList suite = new ArrayList();

                suite.Add(new TTU.CSC4950.SMC.DataLayer_Test.BatchDAL_Test());
                suite.Add(new TTU.CSC4950.SMC.DataLayer_Test.BatchTypeDAL_Test());
                suite.Add(new TTU.CSC4950.SMC.DataLayer_Test.MachineDAL_Test());

                suite.Add(new TTU.CSC4950.SMC.LogicLayer_Test.BatchController_Test());
                suite.Add(new TTU.CSC4950.SMC.LogicLayer_Test.BatchTypeController_Test());
                suite.Add(new TTU.CSC4950.SMC.LogicLayer_Test.MachineController_Test());
                suite.Add(new TTU.CSC4950.SMC.LogicLayer_Test.PLCController_Test());

                suite.Add(new TTU.CSC4950.SMC.FrameWork_Test.Batch_Test());

                return suite;
            }
        }
    }
}
