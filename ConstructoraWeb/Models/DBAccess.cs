using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace ConstructoraWeb.Models
{
    public class DBAccess
    {
        private SqlConnection cnx;

        public DBAccess(string connectionString)
        {
            this.cnx = new SqlConnection(connectionString);
        }

        public SqlConnection Open()
        {
            if (cnx != null)
            {
                if (cnx.State == ConnectionState.Closed)
                {
                    cnx.Open();
                }
            }
            else
            {
                throw new Exception("Parameters [cnx] is null.");
            }
            return cnx;
        }

        public void Close()
        {
            if (cnx != null)
            {
                if (cnx.State == ConnectionState.Open)
                {
                    cnx.Close();
                }
            }
            else
            {
                throw new Exception("Parameters [cnx] is null.");
            }
        }

        public string GetString(SqlDataReader dr, string name) => dr[name].ToString();
        public int GetInt(SqlDataReader dr, string name) => Convert.ToInt32(dr[name]);
        public long GetLong(SqlDataReader dr, string name) => Convert.ToInt64(dr[name]);
        public decimal GetDecimal(SqlDataReader dr, string name) => Convert.ToDecimal(dr[name]);
        public double GetDouble(SqlDataReader dr, string name) => Convert.ToDouble(dr[name]);
        public bool GetBool(SqlDataReader dr, string name) => Convert.ToBoolean(dr[name]);
        public DateTime GetDateTime(SqlDataReader dr, string name) => Convert.ToDateTime(dr[name]);

        protected string PrmStr(string str)
        {
            return str ?? string.Empty;
        }
    }
}
