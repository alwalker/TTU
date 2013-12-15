using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using TTU.CSC4950.SMC.FrameWork;

namespace TTU.CSC4950.SMC.DataLayer
{
    public class BatchDAL
    {
        #region Fields

        private static ConnectionStringSettings _conn;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Initializes the <see cref="BatchDAL"/> class.
        /// </summary>
        static BatchDAL()
        {
            _conn = ConfigurationManager.ConnectionStrings["LocalDB"];
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Adds the batch.
        /// </summary>
        /// <param name="batch">The batch.</param>
        /// <returns></returns>
        public static bool AddBatch(Batch batch)
        {
            SqlConnection con = new SqlConnection(_conn.ConnectionString);
            SqlParameter machineID, batchDate, batchType, success;
            machineID = new SqlParameter("@MachineID", SqlDbType.VarChar, batch.MachineID.Length);
            machineID.Value = batch.MachineID;
            batchDate = new SqlParameter("@BatchDate", SqlDbType.DateTime);
            batchDate.Value = batch.BatchDate;
            batchType = new SqlParameter("@BatchType", SqlDbType.VarChar, batch.Type.Name.Length);
            batchType.Value = batch.Type.Name;
            success = new SqlParameter("@Success", SqlDbType.Bit);
            success.Value = batch.Success;
            SqlCommand addBatch = new SqlCommand("usp_AddBatch");
            addBatch.CommandType = CommandType.StoredProcedure;
            addBatch.Connection = con;
            addBatch.Parameters.Add(machineID);
            addBatch.Parameters.Add(batchDate);
            addBatch.Parameters.Add(batchType);
            addBatch.Parameters.Add(success);

            try
            {
                con.Open();
                addBatch.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
            finally
            {
                con.Close();
            }

            return true;
        }

        /// <summary>
        /// Deletes the batch.
        /// </summary>
        /// <param name="batch">The batch.</param>
        /// <returns></returns>
        public static bool DeleteBatch(Batch batch)
        {
            SqlConnection con = new SqlConnection(_conn.ConnectionString);

            SqlParameter machineID, batchDate;
            machineID = new SqlParameter("@MachineID", SqlDbType.VarChar, batch.MachineID.Length);
            machineID.Value = batch.MachineID;
            batchDate = new SqlParameter("@BatchDate", SqlDbType.DateTime);
            batchDate.Value = batch.BatchDate;

            SqlCommand deleteBatch = new SqlCommand("usp_DeleteBatch");
            deleteBatch.CommandType = CommandType.StoredProcedure;
            deleteBatch.Connection = con;
            deleteBatch.Parameters.Add(machineID);
            deleteBatch.Parameters.Add(batchDate);

            try
            {
                con.Open();
                deleteBatch.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
            finally
            {
                con.Close();
            }

            return true;
        }

        /// <summary>
        /// Gets all batches.
        /// </summary>
        /// <returns></returns>
        public static List<Batch> GetAllBatches()
        {
            Batch newBatch;
            List<Batch> batches = new List<Batch>();

            SqlConnection con = new SqlConnection(_conn.ConnectionString);
            SqlDataReader reader = null;
            SqlCommand getAllBatches = new SqlCommand("usp_GetAllBatches");
            getAllBatches.Connection = con;
            getAllBatches.CommandType = CommandType.StoredProcedure;

            try
            {
                con.Open();
                reader = getAllBatches.ExecuteReader();

                if (!reader.HasRows)
                {
                    con.Close();
                    reader.Close();

                    return null;
                }

                while (reader.Read())
                {
                    newBatch = new Batch();

                    newBatch.MachineID = reader["MachineID"].ToString();
                    newBatch.BatchDate = Convert.ToDateTime(reader["BatchDate"].ToString());
                    newBatch.Type = BatchTypeDAL.GetBatchType(reader["BatchType"].ToString());
                    newBatch.Success = Convert.ToBoolean(reader["Success"].ToString());

                    batches.Add(newBatch);
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                con.Close();
            }

            return batches;
        }

        #endregion Methods
    }
}
