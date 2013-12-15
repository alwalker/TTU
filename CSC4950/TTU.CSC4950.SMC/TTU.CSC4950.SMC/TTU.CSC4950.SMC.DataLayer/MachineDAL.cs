using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using TTU.CSC4950.SMC.FrameWork;

namespace TTU.CSC4950.SMC.DataLayer
{
    public class MachineDAL
    {
         #region Fields

        private static ConnectionStringSettings _conn;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Initializes the <see cref="MachineDAL"/> class.
        /// </summary>
        static MachineDAL()
        {
            _conn = ConfigurationManager.ConnectionStrings["LocalDB"];
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Gets the machine IDs.
        /// </summary>
        /// <returns></returns>
        public static List<string> GetMachineIDs()
        {
            List<string> ids = new List<string>();

            SqlConnection con = new SqlConnection(_conn.ConnectionString);
            SqlDataReader reader = null;
            SqlCommand getMachineIDs = new SqlCommand("usp_GetMachineIDs");
            getMachineIDs.Connection = con;
            getMachineIDs.CommandType = CommandType.StoredProcedure;

            try
            {
                con.Open();
                reader = getMachineIDs.ExecuteReader();

                if (!reader.HasRows)
                {
                    con.Close();
                    reader.Close();

                    return null;
                }

                while (reader.Read())
                {
                    ids.Add(reader["MachineID"].ToString());
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

            return ids;
        }

        #endregion Methods
    }
}
