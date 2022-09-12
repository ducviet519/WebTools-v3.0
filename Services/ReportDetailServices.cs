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
    public class ReportDetailServices : IReportDetailServices
    {
        //private readonly DatabaseContext _context;
        private readonly IConfiguration _configuration;

        //public ReportListServices(DatabaseContext context)
        //{
        //    _context = context;
        //}
        public ReportDetailServices(IConfiguration configuration)
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

        public string DeleteReportDetail(string IdBieuMau)
        {
            throw new NotImplementedException();
        }

        public List<ReportDetail> GetReportDetail(string IdBieuMau)
        {
            List<ReportDetail> reportLists = new List<ReportDetail>();

            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    reportLists = dbConnection.Query<ReportDetail>("sp_Report_Detail_View", new { IDBieuMau = IdBieuMau }, commandType: CommandType.StoredProcedure).ToList();
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

        public string InsertReportDetail(ReportDetail reportDetail)
        {
            string result = "";
            var detailTable = new List<ReportDetail>
            {
                new ReportDetail
                {
                    IDBieuMau = reportDetail.IDBieuMau,
                    IDPhienBan = reportDetail.IDPhienBan,
                    KhoaPhong = reportDetail.KhoaPhong,
                    TrangThai = reportDetail.TrangThai,
                }
            };
            try
            {
                using (IDbConnection dbConnection = Connection)
                {

                    dbConnection.Open();
                    var data = dbConnection.Query<ReportDetail>("sp_Report_Detail_Add",
                        new
                        {
                            ReportDetail = detailTable.AsTableValuedParameter("dbo.ReportDetail",
                            new[] { "IDBieuMau", "IDPhienBan", "KhoaPhong", "TrangThai" }),
                            User = reportDetail.User
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

        public string UpdateReportDetail(ReportDetail reportDetail)
        {
            throw new NotImplementedException();
        }
    }
}
