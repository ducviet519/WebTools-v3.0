﻿using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using WebTools.Models;
using Dapper;
using System;
using System.Linq;

namespace WebTools.Services
{
    public class ReportVersionServices : IReportVersionServices
    {
        //private readonly DatabaseContext _context;
        private readonly IConfiguration _configuration;

        //public ReportListServices(DatabaseContext context)
        //{
        //    _context = context;
        //}
        public ReportVersionServices(IConfiguration configuration)
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


        public string DeleteReportVersion(string IdBieuMau)
        {
            throw new NotImplementedException();
        }

        //Lấy về danh sách Phiên bản sp_Report_Version_View
        public List<ReportVersion> GetReportVersion(string IdBieuMau)
        {
            List<ReportVersion> reportLists = new List<ReportVersion>();

            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    reportLists = dbConnection.Query<ReportVersion>("sp_Report_Version_View",new{ IDBieuMau = IdBieuMau }, commandType: CommandType.StoredProcedure).ToList();
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

        //Thêm mới phiên bản sp_Report_Version_Add
        public string InsertReportVersion(ReportVersion reportVersion)
        {
            string result = "";
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    var data = dbConnection.Query<ReportVersion>("sp_Report_Version_Add",
                        new
                        {
                            IDBieuMau = reportVersion.IDBieuMau,
                            NgayBH = reportVersion.NgayBanHanh,
                            FileLink = reportVersion.FileLink,
                            GhiChu = reportVersion.GhiChu,
                            PhienBan = reportVersion.PhienBan,
                            User = reportVersion.CreatedUser
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

        public string UpdateReportVersion(ReportVersion reportVersion)
        {
            throw new NotImplementedException();
        }
    }
}
