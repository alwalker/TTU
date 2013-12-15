using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PLCSimulator
{
    public class Rule
    {
        #region Properties

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public bool Value1 { get; set; }
        public bool Value2 { get; set; }

        #endregion Properties

        #region Constructor

        public Rule(string addr1, bool val1, string addr2, bool val2)
        {
            Address1 = addr1;
            Address2 = addr2;
            Value1 = val1;
            Value2 = val2;
        }

        #endregion Constructor
    }
}
