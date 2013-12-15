using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TTU.CSC4950.SMC.FrameWork
{
    public class Machine
    {
        #region Fields

        private static Machine _machine;

        #endregion 

        #region Properties

        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        /// <value>The ID.</value>
        public string ID { get; set; }
        
        #endregion

        #region Constructor

        protected Machine(string id)
        {
            ID = id;
        }

        /// <summary>
        /// Returns the single instance of the Machine class
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public static Machine Instance(string id)
        {
            if (_machine == null)
            {
                _machine = new Machine(id);
            }
            else
            {
                _machine.ID = id;
            }

            return _machine;
        }

        #endregion
    }
}
