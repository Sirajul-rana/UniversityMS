using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace UniversityMS.Gateway
{
    public class Gateway
    {
        public SqlConnection Connection { get; set; }
        public SqlCommand SqlCommand { get; set; }

        public Gateway(string query)
        {
            Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["UniversityMS_DB"].ConnectionString);
            
            Connection.Open();
            SqlCommand = new SqlCommand(query,Connection);
        }
    }
}