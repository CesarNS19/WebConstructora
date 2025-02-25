using System.Data;
using System.Reflection;
using Microsoft.Data.SqlClient;
using ConstructoraWeb.Models.ViewModels;

namespace ConstructoraWeb.Models.Services
{
    public class SubModuleSrv : DBAccess
    {
        private readonly DBAccess cnx;

        public SubModuleSrv(string connectionString) : base(connectionString)
        {
            this.cnx = new DBAccess(connectionString);
        }
        public ResponseVM<List<SubModuleVM>> ListPermission(long auxID)
        {
            ResponseVM<List<SubModuleVM>> res = new ResponseVM<List<SubModuleVM>>()
            {
                Data = new List<SubModuleVM>()
            };

            try
            {
                var command = new SqlCommand("SPSubmodule", Open()) { CommandType = CommandType.StoredProcedure };
                command.Parameters.AddWithValue("@auxID", auxID);
                command.Parameters.AddWithValue("@Case", "SELECT_PERMISSIONS");

                using (var dr = command.ExecuteReader())
                {
                    res.HasRow(dr);
                    while (dr.Read())
                    {
                        res.Data.Add(new SubModuleVM
                        {
                            ModuleID = GetInt(dr, "ModuleID"),
                            MoDescription = GetString(dr, "MoDescription"),
                            MoController = GetString(dr, "MoController"),
                            SubModuleID = GetInt(dr, "SubmoduleID"),
                            SubDescription = GetString(dr, "SubDescription"),
                            SubAction = GetString(dr, "SubAction"),
                            SubStatus = GetString(dr, "SubStatus"),
                            SubPosition = GetInt(dr, "SubPosition"),
                            SubRDate = GetDateTime(dr, "SubRDate"),
                            PerStatus = GetString(dr, "PerStatus"),
                            auxID = auxID,
                            PermissionID = GetInt(dr, "PermissionID")
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                res.Error(ex);
            }
            finally
            {
                Close();
            }

            return res;
        }

        public ResponseVM<List<SubModuleVM>> List(SubModuleVM subModuleVM)
        {
            ResponseVM<List<SubModuleVM>> res = new ResponseVM<List<SubModuleVM>>()
            {
                Data = new List<SubModuleVM>()
            };

            try
            {
                var command = new SqlCommand("SPSubmodule", Open()) { CommandType = CommandType.StoredProcedure };
                command.Parameters.AddRange(_parameters(subModuleVM, subModuleVM.SubModuleID == 0 ? "SELECT_ALL" : "SELECT_BY_ID"));

                using (var dr = command.ExecuteReader())
                {
                    res.HasRow(dr);
                    while (dr.Read())
                    {
                        res.Data.Add(new SubModuleVM
                        {
                            SubModuleID = dr.GetInt64(dr.GetOrdinal("SubModuleID")),
                            SubDescription = dr.GetString(dr.GetOrdinal("SubDescription")),
                            SubAction = dr.GetString(dr.GetOrdinal("SubAction")),
                            SubStatus = dr.GetString(dr.GetOrdinal("SubStatus")),
                            SubPosition = dr.GetInt32(dr.GetOrdinal("SubPosition")),
                            SubRDate = dr.IsDBNull(dr.GetOrdinal("SubRDate")) ? (DateTime?)null : dr.GetDateTime(dr.GetOrdinal("SubRDate")),
                            ModuleID = dr.GetInt64(dr.GetOrdinal("ModuleID")),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                res.Error(ex);
            }
            finally
            {
                Close();
            }
            return res;
        }

        public ResponseVM AddUpdate(SubModuleVM submoduleVM)
        {
            ResponseVM res = new ResponseVM();

            try
            {
                var command = new SqlCommand("SPSubmodule", Open()) { CommandType = CommandType.StoredProcedure };
                if (!submoduleVM.SubRDate.HasValue)
                {
                    submoduleVM.SubRDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                }
                command.Parameters.AddRange(_parameters(submoduleVM, submoduleVM.SubModuleID == 0 ? "INSERT" : "UPDATE"));

                using (var dr = command.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        res.DBCatchResponseInOneLine(dr);
                    }
                }
            }
            catch (Exception e)
            {
                res.Error(e);
            }
            finally
            {
                Close();
            }
            return res;
        }
        
        public ResponseVM UpSubModule(SubModuleVM submoduleVM)
        {
            ResponseVM res = new ResponseVM();

            try
            {
                var command = new SqlCommand("SPSubmodule", Open()) { CommandType = CommandType.StoredProcedure };

                command.Parameters.AddRange(_parameters(submoduleVM, "UP"));

                using (var dr = command.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        res.DBCatchResponseInOneLine(dr);
                    }
                }
            }
            catch (Exception e)
            {
                res.Error(e);
            }
            finally
            {
                Close();
            }

            return res;
        }

        public ResponseVM DownSubModule(SubModuleVM submoduleVM)
        {
            ResponseVM res = new ResponseVM();

            try
            {
                var command = new SqlCommand("SPSubmodule", Open()) { CommandType = CommandType.StoredProcedure };

                command.Parameters.AddRange(_parameters(submoduleVM, "DOWN"));

                using (var dr = command.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        res.DBCatchResponseInOneLine(dr);
                    }
                }
            }
            catch (Exception e)
            {
                res.Error(e);
            }
            finally
            {
                Close();
            }

            return res;
        }

         public ResponseVM ActivateStatus(SubModuleVM submoduleVM)
        {
            ResponseVM res = new ResponseVM();

            try
            {
                var command = new SqlCommand("SPSubmodule", Open()) { CommandType = CommandType.StoredProcedure };

                command.Parameters.AddRange(_parameters(submoduleVM, "ACTIVATE"));

                using (var dr = command.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        res.DBCatchResponseInOneLine(dr);
                    }
                }
            }
            catch (Exception e)
            {
                res.Error(e);
            }
            finally
            {
                Close();
            }

            return res;
        }

        public ResponseVM DeactivateStatus(SubModuleVM submoduleVM)
        {
            ResponseVM res = new ResponseVM();

            try
            {
                var command = new SqlCommand("SPSubmodule", Open()) { CommandType = CommandType.StoredProcedure };
                
                command.Parameters.AddRange(_parameters(submoduleVM, "DEACTIVATE"));

                using (var dr = command.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        res.DBCatchResponseInOneLine(dr);
                    }
                }
            }
            catch (Exception e)
            {
                res.Error(e);
            }
            finally
            {
                Close();
            }

            return res;
        }

        public ResponseVM DeleteSubModule(SubModuleVM submoduleVM)
        {
            ResponseVM res = new ResponseVM();

            try
            {
                var command = new SqlCommand("SPSubmodule", Open()) { CommandType = CommandType.StoredProcedure };
                
                command.Parameters.AddRange(_parameters(submoduleVM, "DELETE"));

                using (var dr = command.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        res.DBCatchResponseInOneLine(dr);
                    }
                }
            }
            catch (Exception e)
            {
                res.Error(e);
            }
            finally
            {
                Close();
            }

            return res;
        }

        public bool CheckPermissionOnView(string emailAddress, string moController, string subAction)
        {
            bool hasPermission = false;

            try
            {
                using (var connection = Open())
                {
                    using (var command = new SqlCommand("SPCheckPermissionOnView", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@AcEmailAddress", emailAddress);
                        command.Parameters.AddWithValue("@MoController", moController);
                        command.Parameters.AddWithValue("@SubAction", subAction);


                        SqlParameter permissionOnViewParam = new SqlParameter("@PermissionOnView", SqlDbType.Bit);
                        permissionOnViewParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(permissionOnViewParam);

                        command.ExecuteNonQuery();


                        if (permissionOnViewParam.Value != DBNull.Value)
                        {
                            hasPermission = Convert.ToBoolean(permissionOnViewParam.Value);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                Close();
            }

            return hasPermission;
        }
        public ResponseVM<List<SubModuleVM>> ListModuleOnProfile(SubModuleVM subModuleVM)
        {
            ResponseVM<List<SubModuleVM>> res = new ResponseVM<List<SubModuleVM>>()
            {
                Data = new List<SubModuleVM>()
            };

            try
            {
                string caseType = subModuleVM.SubModuleID != 0 ? "SELECT_BY_ID" : (subModuleVM.AcEmailAddress != "" ? "SELECT_ON_PROFILE" : "");

                var command = new SqlCommand("SPSubmodule", Open()) { CommandType = CommandType.StoredProcedure };
                command.Parameters.AddRange(_parameters(subModuleVM, caseType));

                using (var dr = command.ExecuteReader())
                {
                    res.HasRow(dr);
                    while (dr.Read())
                    {
                        res.Data.Add(new SubModuleVM
                        {
                            SubModuleID = dr.GetInt64(dr.GetOrdinal("SubModuleID")),
                            SubDescription = dr.GetString(dr.GetOrdinal("SubDescription")),
                            SubAction = dr.GetString(dr.GetOrdinal("SubAction")),
                            SubStatus = dr.GetString(dr.GetOrdinal("SubStatus")),
                            SubPosition = dr.GetInt32(dr.GetOrdinal("SubPosition")),
                            SubRDate = dr.IsDBNull(dr.GetOrdinal("SubRDate")) ? (DateTime?)null : dr.GetDateTime(dr.GetOrdinal("SubRDate")),
                            ModuleID = dr.GetInt64(dr.GetOrdinal("ModuleID")),
                            MoDescription = dr.GetString(dr.GetOrdinal("MoDescription")),
                            MoController = dr.GetString(dr.GetOrdinal("MoController")),
                            MoIcon = dr.GetString(dr.GetOrdinal("MoIcon")),
                            MoStatus = dr.GetString(dr.GetOrdinal("MoStatus")),
                            MoPosition = dr.GetInt32(dr.GetOrdinal("MoPosition")),
                            MoRDate = dr.IsDBNull(dr.GetOrdinal("MoRDate")) ? (DateTime?)null : dr.GetDateTime(dr.GetOrdinal("MoRDate")),
                            PerStatus = dr.GetString(dr.GetOrdinal("PerStatus")),

                            
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                res.Error(ex);
            }
            finally
            {
                Close();
            }
            return res;
        }

        private SqlParameter[] _parameters(SubModuleVM submoduleVM, string action)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@SubmoduleID", submoduleVM.SubModuleID),
                new SqlParameter("@SubDescription", submoduleVM.SubDescription),
                new SqlParameter("@SubAction", submoduleVM.SubAction),
                new SqlParameter("@SubStatus", submoduleVM.SubStatus),
                new SqlParameter("@SubPosition", submoduleVM.SubPosition),
                new SqlParameter("@SubRDate", submoduleVM.SubRDate.HasValue ? (object)submoduleVM.SubRDate.Value : DBNull.Value),
                new SqlParameter("@ModuleID", submoduleVM.ModuleID),
                new SqlParameter("@MoDescription", submoduleVM.MoDescription),
                new SqlParameter("@MoController", submoduleVM.MoController),
                new SqlParameter("@MoIcon", submoduleVM.MoIcon),
                new SqlParameter("@MoStatus", submoduleVM.MoStatus),
                new SqlParameter("@MoPosition", submoduleVM.MoPosition),
                new SqlParameter("@PermissionID", submoduleVM.PermissionID),
                new SqlParameter("@AcEmailAddress", submoduleVM.AcEmailAddress),
                new SqlParameter("@auxID", submoduleVM.auxID),
                new SqlParameter("@PerStatus", submoduleVM.PerStatus),
                
                new SqlParameter("@Case", action)
            };
        }
    }
}
