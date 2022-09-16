using System.Collections.Generic;
using WebTools.Models;
using WebTools.Context;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using Dapper;
using System.Linq;

namespace WebTools.Services
{
    public class ReportURDServices:IReportURDServices
    {
        private readonly IConfiguration _configuration;
        public ReportURDServices(IConfiguration configuration)
        {
            _configuration = configuration;
            ConnectionString = _configuration.GetConnectionString("ToolsDB");
            provideName = "System.Data.SqlClient";
        }
        public string ConnectionString { get; }
        public string provideName { get; }
        public IDbConnection Connection
        {
            get { return new SqlConnection(ConnectionString); }
        }

        public List<ReportURD> GetAll_URD()
        {
            List<ReportURD> reporURDs = new List<ReportURD>();
            var sql = "SELECT * FROM Report_URD";
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    reporURDs = dbConnection.Query<ReportURD>(sql).ToList();
                    dbConnection.Close();
                }
                return reporURDs;
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                return reporURDs;
            }
        }
    }
}
