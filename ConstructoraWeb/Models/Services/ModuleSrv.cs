using System.Data;
using Microsoft.Data.SqlClient;
using ConstructoraWeb.Models.ViewModels;

namespace ConstructoraWeb.Models.Services
{
    public class ModuleSrv : DBAccess
    {
        private readonly DBAccess cnx;

        public ModuleSrv(string connectionString) : base(connectionString)
        {
            this.cnx = new DBAccess(connectionString);
        }

        public ResponseVM<List<ModuleVM>> List(ModuleVM moduleVM)
        {
            ResponseVM<List<ModuleVM>> res = new ResponseVM<List<ModuleVM>>()
            {
                Data = new List<ModuleVM>()
            };

            try
            {
                var command = new SqlCommand("SPModule", Open()) { CommandType = CommandType.StoredProcedure };
                command.Parameters.AddRange(_parameters(moduleVM, moduleVM.ModuleID == 0 ? "SELECT_ALL" : "SELECT_BY_ID"));

                using (var dr = command.ExecuteReader())
                {
                    res.HasRow(dr);
                    while (dr.Read())
                    {
                        res.Data.Add(new ModuleVM
                        {
                            ModuleID = dr.GetInt64(dr.GetOrdinal("ModuleID")),
                            MoDescription = dr.GetString(dr.GetOrdinal("MoDescription")),
                            MoController = dr.GetString(dr.GetOrdinal("MoController")),
                            MoIcon = dr.GetString(dr.GetOrdinal("MoIcon")),
                            MoStatus = dr.GetString(dr.GetOrdinal("MoStatus")),
                            MoPosition = dr.GetInt32(dr.GetOrdinal("MoPosition")),
                            MoRDate = dr.IsDBNull(dr.GetOrdinal("MoRDate")) ? (DateTime?)null : dr.GetDateTime(dr.GetOrdinal("MoRDate")),
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

        public ResponseVM AddUpdate(ModuleVM moduleVM)
        {
            ResponseVM res = new ResponseVM();

            try
            {
                var command = new SqlCommand("SPModule", Open()) { CommandType = CommandType.StoredProcedure };
                if (!moduleVM.MoRDate.HasValue)
                {
                    moduleVM.MoRDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                }
                command.Parameters.AddRange(_parameters(moduleVM, moduleVM.ModuleID == 0 ? "INSERT" : "UPDATE"));

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

        public ResponseVM ActivateStatus(ModuleVM moduleVM)
        {
            ResponseVM res = new ResponseVM();

            try
            {
                var command = new SqlCommand("SPModule", Open()) { CommandType = CommandType.StoredProcedure };

                command.Parameters.AddRange(_parameters(moduleVM, "ACTIVATE"));

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

        public ResponseVM UpModule(ModuleVM moduleVM)
        {
            ResponseVM res = new ResponseVM();

            try
            {
                var command = new SqlCommand("SPModule", Open()) { CommandType = CommandType.StoredProcedure };

                command.Parameters.AddRange(_parameters(moduleVM, "UP"));

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

        public ResponseVM DownModule(ModuleVM moduleVM)
        {
            ResponseVM res = new ResponseVM();

            try
            {
                var command = new SqlCommand("SPModule", Open()) { CommandType = CommandType.StoredProcedure };

                command.Parameters.AddRange(_parameters(moduleVM, "DOWN"));

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

        public ResponseVM DeactivateStatus(ModuleVM moduleVM)
        {
            ResponseVM res = new ResponseVM();

            try
            {
                var command = new SqlCommand("SPModule", Open()) { CommandType = CommandType.StoredProcedure };
                
                command.Parameters.AddRange(_parameters(moduleVM, "DEACTIVATE"));

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
        
        public ResponseVM DeleteModule(ModuleVM moduleVM)
        {
            ResponseVM res = new ResponseVM();

            try
            {
                var command = new SqlCommand("SPModule", Open()) { CommandType = CommandType.StoredProcedure };
                
                command.Parameters.AddRange(_parameters(moduleVM, "DELETE"));

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

        private SqlParameter[] _parameters(ModuleVM moduleVM, string action)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@ModuleID", moduleVM.ModuleID),
                new SqlParameter("@MoDescription", moduleVM.MoDescription),
                new SqlParameter("@MoController", moduleVM.MoController),
                new SqlParameter("@MoIcon", moduleVM.MoIcon),
                new SqlParameter("@MoStatus", moduleVM.MoStatus),
                new SqlParameter("@MoPosition", moduleVM.MoPosition),
                new SqlParameter("@MoRDate", moduleVM.MoRDate.HasValue ? (object)moduleVM.MoRDate.Value : DBNull.Value),
                new SqlParameter("@Case", action)
            };
        }
    }
}
