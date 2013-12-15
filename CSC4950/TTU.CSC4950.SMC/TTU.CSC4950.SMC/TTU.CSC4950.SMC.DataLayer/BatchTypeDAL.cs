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
    public class BatchTypeDAL
    {
        #region Fields

        private static ConnectionStringSettings _conn;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Initializes the <see cref="BatchTypeDAL"/> class.
        /// </summary>
        static BatchTypeDAL()
        {
            _conn = ConfigurationManager.ConnectionStrings["LocalDB"];
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Adds a batch type to the database.
        /// </summary>
        /// <param name="type">The batch type.</param>
        /// <returns></returns>
        public static bool AddBatchType(BatchType type)
        {
            SqlConnection con = new SqlConnection(_conn.ConnectionString);
            SqlParameter name, variableA, variableB, temperatureReq, viscosityReq, speedReq, timeReq;
            name = new SqlParameter("@Name", SqlDbType.VarChar, type.Name.Length);
            name.Value = type.Name;
            variableA = new SqlParameter("@VariableA", SqlDbType.Float);
            variableA.Value = type.VariableA;
            variableB = new SqlParameter("@VariableB", SqlDbType.Float);
            variableB.Value = type.VariableB;
            temperatureReq = new SqlParameter("@TemperatureReq", SqlDbType.Float);
            if (!type.TemperatureRequirement.HasValue)
            {
                temperatureReq.Value = DBNull.Value;
            }
            else
            {
                temperatureReq.Value = type.TemperatureRequirement.Value;
            }
            viscosityReq = new SqlParameter("@ViscosityReq", SqlDbType.Float);
            if (!type.ViscosityRequirement.HasValue)
            {
                viscosityReq.Value = DBNull.Value;
            }
            else
            {
                viscosityReq.Value = type.ViscosityRequirement.Value;
            }
            speedReq = new SqlParameter("@SpeedReq", SqlDbType.Float);
            if (!type.SpeedRequirement.HasValue)
            {
                speedReq.Value = DBNull.Value;
            }
            else
            {
                speedReq.Value = type.SpeedRequirement.Value;
            }
            timeReq = new SqlParameter("@TimeReq", SqlDbType.Float);
            if (!type.TimeRequirement.HasValue)
            {
                timeReq.Value = DBNull.Value;
            }
            else
            {
                timeReq.Value = type.TimeRequirement.Value;
            }

            SqlCommand addBatchType = new SqlCommand("usp_AddBatchType");
            addBatchType.CommandType = CommandType.StoredProcedure;
            addBatchType.Connection = con;
            addBatchType.Parameters.Add(name);
            addBatchType.Parameters.Add(variableA);
            addBatchType.Parameters.Add(variableB);
            addBatchType.Parameters.Add(temperatureReq);
            addBatchType.Parameters.Add(viscosityReq);
            addBatchType.Parameters.Add(speedReq);
            addBatchType.Parameters.Add(timeReq);

            try
            {
                con.Open();
                addBatchType.ExecuteNonQuery();
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
        /// Deletes a batch type from the database.
        /// </summary>
        /// <param name="strName">Name of the batch type.</param>
        /// <returns></returns>
        public static bool DeleteBatchType(string strName)
        {
            SqlConnection con = new SqlConnection(_conn.ConnectionString);
            SqlParameter name;
            name = new SqlParameter("@Name", SqlDbType.VarChar, strName.Length);
            name.Value = strName;

            SqlCommand deleteBatchType = new SqlCommand("usp_DeleteBatchType");
            deleteBatchType.CommandType = CommandType.StoredProcedure;
            deleteBatchType.Connection = con;
            deleteBatchType.Parameters.Add(name);

            try
            {
                con.Open();
                deleteBatchType.ExecuteNonQuery();
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
        /// Edits a batch type in the database.
        /// </summary>
        /// <param name="originalName">Name of the original.</param>
        /// <param name="newVariableA">The new variable A.</param>
        /// <param name="newVariableB">The new variable B.</param>
        /// <param name="newTempReq">The new temp req.</param>
        /// <param name="newViscosityReq">The new viscosity req.</param>
        /// <param name="newSpeedReq">The new speed req.</param>
        /// <param name="newTimeReq">The new time req.</param>
        /// <returns></returns>
        public static bool EditBatchType(string originalName, double newVariableA, double newVariableB, double? newTempReq, double? newViscosityReq, double? newSpeedReq, double? newTimeReq)
        {
            SqlConnection con = new SqlConnection(_conn.ConnectionString);
            SqlParameter name, variableA, variableB, temperatureReq, viscosityReq, speedReq, timeReq;
            name = new SqlParameter("@Name", SqlDbType.VarChar, originalName.Length);
            name.Value = originalName;
            variableA = new SqlParameter("@VariableA", SqlDbType.Float);
            variableA.Value = newVariableA;
            variableB = new SqlParameter("@VariableB", SqlDbType.Float);
            variableB.Value = newVariableB;
            temperatureReq = new SqlParameter("@TemperatureReq", SqlDbType.Float);
            if (!newTempReq.HasValue)
            {
                temperatureReq.Value = DBNull.Value;
            }
            else
            {
                temperatureReq.Value = newTempReq.Value;
            }
            viscosityReq = new SqlParameter("@ViscosityReq", SqlDbType.Float);
            if (!newViscosityReq.HasValue)
            {
                viscosityReq.Value = DBNull.Value;
            }
            else
            {
                viscosityReq.Value = newViscosityReq.Value;
            }
            speedReq = new SqlParameter("@SpeedReq", SqlDbType.Float);
            if (!newSpeedReq.HasValue)
            {
                speedReq.Value = DBNull.Value;
            }
            else
            {
                speedReq.Value = newSpeedReq.Value;
            }
            timeReq = new SqlParameter("@TimeReq", SqlDbType.Float);
            if (!newTimeReq.HasValue)
            {
                timeReq.Value = DBNull.Value;
            }
            else
            {
                timeReq.Value = newTimeReq.Value;
            }

            SqlCommand editBatchType = new SqlCommand("usp_EditBatchType");
            editBatchType.CommandType = CommandType.StoredProcedure;
            editBatchType.Connection = con;
            editBatchType.Parameters.Add(name);
            editBatchType.Parameters.Add(variableA);
            editBatchType.Parameters.Add(variableB);
            editBatchType.Parameters.Add(temperatureReq);
            editBatchType.Parameters.Add(viscosityReq);
            editBatchType.Parameters.Add(speedReq);
            editBatchType.Parameters.Add(timeReq);

            try
            {
                con.Open();
                editBatchType.ExecuteNonQuery();
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
        /// Gets the type of the batch.
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <returns></returns>
        public static BatchType GetBatchType(string typeName)
        {
            BatchType type = null;

            SqlConnection con = new SqlConnection(_conn.ConnectionString);
            SqlDataReader reader = null;
            SqlParameter name = new SqlParameter("@Name", SqlDbType.VarChar, typeName.Length);
            name.Value = typeName;
            SqlCommand getBatchType = new SqlCommand("usp_GetBatchType");
            getBatchType.Connection = con;
            getBatchType.CommandType = CommandType.StoredProcedure;
            getBatchType.Parameters.Add(name);

            try
            {
                con.Open();
                reader = getBatchType.ExecuteReader();

                if (!reader.HasRows)
                {
                    reader.Close();
                    con.Close();

                    return null;
                }

                reader.Read();
                type = new BatchType();

                type.Name = typeName;
                type.VariableA = Convert.ToDouble(reader["VariableA"].ToString());
                type.VariableB = Convert.ToDouble(reader["VariableB"].ToString());

                if (reader["TemperatureReq"].Equals(DBNull.Value))
                {
                    type.TemperatureRequirement = null;
                }
                else
                {
                    type.TemperatureRequirement = Convert.ToDouble(reader["TemperatureReq"].ToString());
                }
                if (reader["ViscosityReq"].Equals(DBNull.Value))
                {
                    type.ViscosityRequirement = null;
                }
                else
                {
                    type.ViscosityRequirement = Convert.ToDouble(reader["ViscosityReq"].ToString());
                }
                if (reader["SpeedReq"].Equals(DBNull.Value))
                {
                    type.SpeedRequirement = null;
                }
                else
                {
                    type.SpeedRequirement = Convert.ToDouble(reader["SpeedReq"].ToString());
                }
                if (reader["TimeReq"].Equals(DBNull.Value))
                {
                    type.TimeRequirement = null;
                }
                else
                {
                    type.TimeRequirement = Convert.ToDouble(reader["TimeReq"].ToString());
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

            return type;
        }

        /// <summary>
        /// Gets all batch types.
        /// </summary>
        /// <returns></returns>
        public static List<BatchType> GetAllBatchTypes()
        {
            BatchType type = null;
            List<BatchType> batchTypes = new List<BatchType>();
            SqlConnection con = new SqlConnection(_conn.ConnectionString);
            SqlDataReader reader = null;
            SqlCommand getAllBatchTypes = new SqlCommand("usp_GetAllBatchTypes");
            getAllBatchTypes.CommandType = CommandType.StoredProcedure;
            getAllBatchTypes.Connection = con;

            try
            {
                con.Open();
                reader = getAllBatchTypes.ExecuteReader();

                if (!reader.HasRows)
                {
                    reader.Close();
                    con.Close();
                    return null;
                }

                while (reader.Read())
                {
                    type = new BatchType();

                    type.Name = reader["Name"].ToString();
                    type.VariableA = Convert.ToDouble(reader["VariableA"].ToString());
                    type.VariableB = Convert.ToDouble(reader["VariableB"].ToString());

                    if (reader["TemperatureReq"].Equals(DBNull.Value))
                    {
                        type.TemperatureRequirement = null;
                    }
                    else
                    {
                        type.TemperatureRequirement = Convert.ToDouble(reader["TemperatureReq"].ToString());
                    }
                    if (reader["ViscosityReq"].Equals(DBNull.Value))
                    {
                        type.ViscosityRequirement = null;
                    }
                    else
                    {
                        type.ViscosityRequirement = Convert.ToDouble(reader["ViscosityReq"].ToString());
                    }
                    if (reader["SpeedReq"].Equals(DBNull.Value))
                    {
                        type.SpeedRequirement = null;
                    }
                    else
                    {
                        type.SpeedRequirement = Convert.ToDouble(reader["SpeedReq"].ToString());
                    }
                    if (reader["TimeReq"].Equals(DBNull.Value))
                    {
                        type.TimeRequirement = null;
                    }
                    else
                    {
                        type.TimeRequirement = Convert.ToDouble(reader["TimeReq"].ToString());
                    }

                    batchTypes.Add(type);
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

            return batchTypes;
        }

        #endregion Methods
    }
}
