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
    public class ReportListServices : IReportListServices
    {
        //private readonly DatabaseContext _context;
        private readonly IConfiguration _configuration;

        //public ReportListServices(DatabaseContext context)
        //{
        //    _context = context;
        //}
        public ReportListServices(IConfiguration configuration)
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



        public string DeleteReportList(string IdBieuMau)
        {
            string result = "";
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    var data = dbConnection.Query<ReportList>("sp_Report_Delete",
                        new
                        {
                            IDBieuMau = IdBieuMau
                        },
                        commandType: CommandType.StoredProcedure);
                    if (data != null)
                    {
                        result = "Deleted";
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

        //Thực thi StoredProcedure sp_Report_List lấy về danh sách Biểu mẫu
        public List<ReportList> GetReportList()
        {
            List<ReportList> reportLists = new List<ReportList>();

            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    reportLists = dbConnection.Query<ReportList>("sp_Report_List", commandType: CommandType.StoredProcedure).ToList();
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

        //Thực thi StoredProcedure sp_Report_New thêm mới Biểu mẫu
        public string InsertReportList(ReportList reportList)
        {
            string result = "";
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    var data = dbConnection.Query<ReportList>("sp_Report_New",
                        new 
                        {
                            TenBM = reportList.TenBM,
                            MaBM = reportList.MaBM,
                            NgayBH = reportList.NgayBanHanh.ToString("yyyyMMdd"),
                            FileLink = reportList.FileLink,
                            GhiChu = reportList.GhiChu,
                            KhoaPhongSD = reportList.KhoaPhong,
                            PhienBan = reportList.PhienBan,
                            TheLoai = reportList.postTheLoai,
                            User = reportList.CreatedUser
                        },
                        commandType: CommandType.StoredProcedure);
                    if( data != null)
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

        public string UpdateReportList(ReportList reportList)
        {
            string result = "";
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    var data = dbConnection.Query<ReportList>("sp_Report_Update",
                        new
                        {
                            TenBM = reportList.TenBM,
                            MaBM = reportList.MaBM,
                            NgayBH = reportList.NgayBanHanh.ToString("yyyyMMdd"),
                            FileLink = reportList.FileLink,
                            GhiChu = reportList.GhiChu,
                            KhoaPhongSD = reportList.KhoaPhong,
                            PhienBan = reportList.PhienBan,
                            TheLoai = reportList.postTheLoai,
                            User = reportList.CreatedUser,
                            IDBieuMau = reportList.IDBieuMau
                        },
                        commandType: CommandType.StoredProcedure);
                    if (data != null)
                    {
                        result = "Updated";
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
    }
}
