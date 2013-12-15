using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TTU.CSC4950.SMC.FrameWork;
using TTU.CSC4950.SMC.DataLayer;

namespace TTU.CSC4950.SMC.LogicLayer
{
    public class BatchTypeController
    {
        /// <summary>
        /// Gets all batch types.
        /// </summary>
        /// <returns></returns>
        public static List<BatchType> GetAllBatchTypes()
        {
            try
            {
                return BatchTypeDAL.GetAllBatchTypes();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Adds the type of the batch.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static bool AddBatchType(BatchType type)
        {
            try
            {
                return BatchTypeDAL.AddBatchType(type);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Edits the batch type.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="variableA">The variable A.</param>
        /// <param name="variableB">The variable B.</param>
        /// <param name="temperatureReq">The temperature requirement.</param>
        /// <param name="viscosityReq">The viscosity requirement.</param>
        /// <param name="speedReq">The speed requirement.</param>
        /// <param name="timeReq">The time requirement.</param>
        /// <returns></returns>
        public static bool EditBatchType(string name, double variableA, double variableB, double? temperatureReq, double? viscosityReq, double? speedReq, double? timeReq)
        {
            try
            {
                return BatchTypeDAL.EditBatchType(name, variableA, variableB, temperatureReq, viscosityReq, speedReq, timeReq);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the batch type.
        /// </summary>
        /// <param name="name">The name of the batch type.</param>
        /// <returns></returns>
        public static BatchType GetBatchType(string name)
        {
            try
            {
                return BatchTypeDAL.GetBatchType(name);
            }
            catch
            {
                throw;
            }
        }
    }
}
