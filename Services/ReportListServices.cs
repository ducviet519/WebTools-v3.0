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
        private readonly IConfiguration _configuration;
        public ReportListServices(IConfiguration configuration)
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
                }
                return reportLists;
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
            DateTime? NgayBanHanh = null;
            if (!String.IsNullOrEmpty(reportList.NgayBanHanh))
            {
                NgayBanHanh = DateTime.ParseExact(reportList.NgayBanHanh, "dd/MM/yyyy", null);
            }
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
                            NgayBH = NgayBanHanh,
                            FileLink = reportList.FileLink,
                            GhiChu = reportList.GhiChu,
                            KhoaPhongSD = reportList.KhoaPhong,
                            PhienBan = reportList.PhienBan,
                            TheLoai = reportList.postTheLoai,
                            User = reportList.CreatedUser
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
                string errorMsg = ex.Message;
                return result;
            }
        }
    }
}
