using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Context;
using WebTools.Models;

namespace WebTools.Services
{
    public class ReportSoftServices : IReportSoftServices
    {
        private readonly IConfiguration _configuration;
        public ReportSoftServices(IConfiguration configuration)
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

        //Truyền dữ liệu thực thi sp_Report_Soft_View
        public List<ReportSoft> GetReportSoft(string IdBieuMau)
        {
            List<ReportSoft> reportLists = new List<ReportSoft>();

            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    if (dbConnection.State == ConnectionState.Closed)
                        dbConnection.Open();
                    reportLists = dbConnection.Query<ReportSoft>("sp_Report_Soft_View", new { IDBieuMau = IdBieuMau }, commandType: CommandType.StoredProcedure).ToList();
                    dbConnection.Close();                  
                }
                return reportLists;
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                return reportLists;
            }           
        }

        //Truyền dữ liệu thực thi sp_Report_Soft_Add
        public string InsertReportSoft(ReportSoft reportSoft)
        {
            string result = "";
            var softTable = new List<ReportSoft>
            {
                new ReportSoft 
                {
                    IDBieuMau = reportSoft.IDBieuMau,
                    IDPhienBan = reportSoft.IDPhienBan,
                    PhanMem = reportSoft.PhanMem,
                    URD = reportSoft.URD,
                    ViTriIn = reportSoft.ViTriIn,
                    CachIn = reportSoft.CachIn,
                    TrangThaiPM = reportSoft.TrangThaiPM
                }
            };
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    if (dbConnection.State == ConnectionState.Closed)
                        dbConnection.Open();
                    var data = dbConnection.Query<ReportSoft>("sp_Report_Soft_Add",
                        new
                        {
                            ReportSoft = softTable.AsTableValuedParameter("dbo.ReportSoft",
                            new[] { "IDBieuMau", "IDPhienBan", "PhanMem", "URD", "ViTriIn", "CachIn", "TrangThaiPM" }),
                            User = reportSoft.User
                        },
                        commandType: CommandType.StoredProcedure);
                    if (data != null)
                    {
                        result = "OK";
                    }
                    dbConnection.Close();              
                }
                return result;
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return result;
            }
        }
    }
}
