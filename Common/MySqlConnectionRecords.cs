using DKSHApplication.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Text;

namespace DKSHApplication.Common
{
    public class MySqlConnectionRecords
    {
        public DataTable GetRecordingIssabel(Records46Dto param)
        {
            string connstring = @"server=10.210.148.36;userid=readdata;password=readdatacallcenter2019;database=asteriskcdrdb";
            MySqlConnection conn = null;
            DataTable dt = new DataTable();
            try
            {               
                conn = new MySqlConnection(connstring);
                conn.Open();
                StringBuilder query = new StringBuilder();
                query.Append(@" SELECT * 
                                    FROM asteriskcdrdb.cdr
                                WHERE calldate BETWEEN '");
                query.Append(Utils.StringDateFormat(param.calldate) + " 00:00:00'");
                query.Append(" AND '");
                query.Append(Utils.StringDateFormat(param.calldate_to) + " 23:59:59'");
                if (!string.IsNullOrEmpty(param.cnum))
                {
                    query.Append(" AND cnum = '" + param.cnum + "' ");
                }                
                //MySqlCommand paras = new MySqlCommand(query.ToString());
                //paras.Parameters.AddWithValue("@calldate", param.calldate + " 00:00:00");
                //paras.Parameters.AddWithValue("@calldate_to", param.calldate_to + " 23:59:59");
                MySqlDataAdapter da = new MySqlDataAdapter(query.ToString(), conn);
                DataSet ds = new DataSet();
                da.Fill(ds, "cdr");
                dt = ds.Tables["cdr"];
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return dt;
        }

        public DataTable GetRecordingIssabelGigamed(Records46Dto param)
        {
            string connstring = @"server=119.82.130.112;userid=datareader;password=datareader@2021;database=asteriskcdrdb";
            MySqlConnection conn = null;
            DataTable dt = new DataTable();
            try
            {
                conn = new MySqlConnection(connstring);
                conn.Open();
                StringBuilder query = new StringBuilder();
                query.Append(@" SELECT * 
                                    FROM asteriskcdrdb.cdr
                                WHERE calldate BETWEEN '");
                query.Append(Utils.StringDateFormat(param.calldate) + " 00:00:00'");
                query.Append(" AND '");
                query.Append(Utils.StringDateFormat(param.calldate_to) + " 23:59:59'");
                if (!string.IsNullOrEmpty(param.cnum))
                {
                    query.Append(string.Format(" AND cnum = ", param.cnum));
                }
                //MySqlCommand paras = new MySqlCommand(query.ToString());
                //paras.Parameters.AddWithValue("@calldate", param.calldate + " 00:00:00");
                //paras.Parameters.AddWithValue("@calldate_to", param.calldate_to + " 23:59:59");
                MySqlDataAdapter da = new MySqlDataAdapter(query.ToString(), conn);
                DataSet ds = new DataSet();
                da.Fill(ds, "cdr");
                dt = ds.Tables["cdr"];
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return dt;
        }
    }
}