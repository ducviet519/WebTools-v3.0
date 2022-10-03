using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Models;
using WebTools.Services.Interface;

namespace WebTools.Services
{
    public class RolesServices : IRolesServices
    {
        private readonly IConfiguration _configuration;

        public RolesServices(IConfiguration configuration)
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

        public string AddRoles(Roles roles)
        {
            string result = "";
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    var data = dbConnection.Query<Roles>("sp_Roles",
                        new
                        {
                            ID = "",
                            Name = roles.RoleName,
                            Description = roles.Description,
                            Status = roles.Status,
                            User = roles.CreatedBy,
                            Action = "Add"
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

        public string DeleteRoles(int? id)
        {
            string result = "";
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    var data = dbConnection.Query<Roles>("sp_Roles",
                        new
                        {
                            ID = id,
                            Name = "",
                            Description = "",
                            Status = "",
                            User = "",
                            Action = "Delete"
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

        public string EditRoles(Roles roles)
        {
            string result = "";
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    var data = dbConnection.Query<Roles>("sp_Roles",
                        new
                        {
                            ID = roles.RoleID,
                            Name = roles.RoleName,
                            Description = roles.Description,
                            Status = roles.Status,
                            User = roles.CreatedBy,
                            Action = "Edit"
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

        public List<Roles> GetAllRoles()
        {
            List<Roles> roles = new List<Roles>();
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    roles = dbConnection.Query<Roles>("sp_Roles"
                        , new
                        {
                            ID = "",
                            Name = "",
                            Description = "",
                            Status = "",
                            User = "",
                            Action = "GetAll"

                        }
                        , commandType: CommandType.StoredProcedure).ToList();
                    dbConnection.Close();
                }
                return roles;
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                return roles;
            }
        }

        public Roles GetRolesByID(int? id)
        {
            Roles roles = new Roles();
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    roles = dbConnection.Query<Roles>("sp_Roles", new
                    {
                        ID = id,
                        Name = "",
                        Description = "",
                        Status = "",
                        User = "",
                        Action = "GetByID"
                    }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    dbConnection.Close();
                }
                return roles;
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                return roles;
            }
        }


    }
}
