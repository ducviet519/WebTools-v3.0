using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System;
using WebTools.Services.Interface;
using WebTools.Models.Entities;
using System.Data.SqlClient;
using Dapper;
using System.Linq;

namespace WebTools.Services
{
    public class DanhMucBHTNServices:IDanhMucBHTNServices
    {
        #region Connection Database

        private readonly IConfiguration _configuration;
        public DanhMucBHTNServices(IConfiguration configuration)
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

        #endregion
        public async Task<List<DanhMucBHTN>> GetDanhMucBHTN(string loai)
        {
            List<DanhMucBHTN> data = new List<DanhMucBHTN>();
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    data = (await dbConnection.QueryAsync<DanhMucBHTN>("sp_BHTN_DM", new { Loai = loai }, commandType: CommandType.StoredProcedure)).ToList();
                    dbConnection.Close();
                }
                return data;
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                return null;
            }
        }


    }
}
