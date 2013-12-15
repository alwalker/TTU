using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TTU.CSC4950.SMC.FrameWork
{
    public class Batch : System.Object
    {
        #region Properties

        /// <summary>
        /// Gets or sets the machine ID.
        /// </summary>
        /// <value>The machine ID.</value>
        public string MachineID { get; set; }

        /// <summary>
        /// Gets or sets the batch date.
        /// </summary>
        /// <value>The batch date.</value>
        public DateTime BatchDate { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public BatchType Type { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Batch"/> is success.
        /// </summary>
        /// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
        public bool Success { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Batch"/> class.
        /// </summary>
        public Batch()
        {
            MachineID = string.Empty;
            BatchDate = DateTime.MinValue;
            Type = null;
            Success = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Batch"/> class.
        /// </summary>
        /// <param name="machineID">The machine ID.</param>
        /// <param name="batchDate">The batch date.</param>
        /// <param name="type">The type.</param>
        /// <param name="success">if set to <c>true</c> [success].</param>
        public Batch(string machineID, DateTime batchDate, BatchType type, bool success)
        {
            MachineID = machineID;
            BatchDate = batchDate;
            Type = type;
            Success = success;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            Batch other = obj as Batch;

            if(other == null)
            {
                return false;
            }

            if (MachineID == other.MachineID && BatchDate == other.BatchDate && Type.Equals(Type, other.Type) && Success == other.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion Methods
    }
}
