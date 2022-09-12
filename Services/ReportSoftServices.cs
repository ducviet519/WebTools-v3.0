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
        //private readonly DatabaseContext _context;
        private readonly IConfiguration _configuration;

        //public ReportListServices(DatabaseContext context)
        //{
        //    _context = context;
        //}
        public ReportSoftServices(IConfiguration configuration)
        {
            _configuration = configuration;
            ConnectionString = _configuration.GetConnectionString("DbConn");
            provideName = "System.Data.SqlClient";
        }
        public string ConnectionString { get; }
        public string provideName { get; }
        public IDbConnection Connection
        {
            get { return new SqlConnection(ConnectionString); }
        }

        public string DeleteReportSoft(string IdBieuMau)
        {
            throw new NotImplementedException();
        }


        public List<ReportSoft> GetReportSoft(string IdBieuMau)
        {
            List<ReportSoft> reportLists = new List<ReportSoft>();

            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    reportLists = dbConnection.Query<ReportSoft>("sp_Report_Soft_View", new { IDBieuMau = IdBieuMau }, commandType: CommandType.StoredProcedure).ToList();
                    dbConnection.Close();
                    return reportLists;
                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                return reportLists;
            }
        }

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
                        result = "Inserted";
                    }
                    dbConnection.Close();
                    return result;
                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                return result;
            }
        }

        public string UpdateReportSoft(ReportSoft reportSoft)
        {
            throw new NotImplementedException();
        }
    }
}
