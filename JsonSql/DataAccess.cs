using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DataContext
{
    public class DataAccess
    {

        public string connectionString { get; set; }
        public SqlConnection SqlConnection { get; set; }


        public SqlParameter[] plist { get; set; }

        public void ExecuteQuery(string spName)
        {
            SqlConnection cnn = new SqlConnection();
            cnn.ConnectionString = this.connectionString;

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            cmd.CommandText = spName;
            cmd.CommandType = CommandType.StoredProcedure;
            if (plist != null)
            {
                cmd.Parameters.Clear();
                cmd.Parameters.AddRange(plist);
            }
            cnn.Open();

            cmd.ExecuteNonQuery();

            cnn.Close();
        }

        public DataTable GetData(string spName, CommandType type)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter adap = new SqlDataAdapter();

            SqlConnection cnn = new SqlConnection();
            if (this.connectionString != null)
                cnn.ConnectionString = this.connectionString;
            
            
            if (this.SqlConnection != null)
                cnn = SqlConnection;
            

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            cmd.CommandText = spName;
            cmd.CommandType = type;

            if (plist != null)
            {
                cmd.Parameters.Clear();
                cmd.Parameters.AddRange(plist);
            }


            try
            {
                cnn.Open();

                adap.SelectCommand = cmd;
                adap.Fill(ds);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cnn.Close();
            }

            return ds.Tables[0];
        }
    }
}
