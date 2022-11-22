using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Models.Entities;
using WebTools.Services.Interface;

namespace WebTools.Services
{
    public class BangKeChiPhiServices:IBangKeChiPhiSevices
    {
        #region Connection Database

        private readonly IConfiguration _configuration;
        public BangKeChiPhiServices(IConfiguration configuration)
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

        public async Task<List<BangKeChiPhi>> GetBangKeChiPhi(string id, string loai)
        {
            List<BangKeChiPhi> list = new List<BangKeChiPhi>();
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    list = (await dbConnection.QueryAsync<BangKeChiPhi>("sp_BHTN_Detail", new
                    {
                        id = id,
                        Loai = loai,
                    }, commandType: CommandType.StoredProcedure)).ToList();
                    dbConnection.Close();
                }
                return list;
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                return list;
            }
        }

    }
}
