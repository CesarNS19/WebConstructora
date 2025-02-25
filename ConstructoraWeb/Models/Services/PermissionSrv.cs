using System.Data;
using Microsoft.Data.SqlClient;
using ConstructoraWeb.Models.ViewModels;

namespace ConstructoraWeb.Models.Services
{
    public class PermissionSrv : DBAccess
    {
        private readonly DBAccess cnx;

        public PermissionSrv(string connectionString) : base(connectionString)
        {
            this.cnx = new DBAccess(connectionString);
        }

        public ResponseVM<List<PermissionVM>> List(PermissionVM permissionVM)
        {
            ResponseVM<List<PermissionVM>> res = new ResponseVM<List<PermissionVM>>()
            {
                Data = new List<PermissionVM>()
            };

            try
            {
                var command = new SqlCommand("SPPermission", Open()) { CommandType = CommandType.StoredProcedure };
                command.Parameters.AddRange(_parameters(permissionVM, permissionVM.PermissionID == 0 ? "SELECT_ALL" : "SELECT_BY_ID"));

                using (var dr = command.ExecuteReader())
                {
                    res.HasRow(dr);
                    while (dr.Read())
                    {
                        res.Data.Add(new PermissionVM
                        {
                            PermissionID = dr.GetInt64(dr.GetOrdinal("PermissionID")),
                            PerStatus = dr.GetString(dr.GetOrdinal("PerStatus")),
                            SubRDate = dr.IsDBNull(dr.GetOrdinal("SubRDate")) ? (DateTime?)null : dr.GetDateTime(dr.GetOrdinal("SubRDate")),
                            SubModuleID = dr.GetInt64(dr.GetOrdinal("SubModuleID")),
                            ProfileID = dr.GetInt64(dr.GetOrdinal("ProfileID"))
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

        public ResponseVM AddUpdate(PermissionVM permissionVM)
        {
            ResponseVM res = new ResponseVM();

            try
            {
                var command = new SqlCommand("SPPermission", Open()) { CommandType = CommandType.StoredProcedure };
                if (!permissionVM.SubRDate.HasValue)
                {
                    permissionVM.SubRDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                }
                command.Parameters.AddRange(_parameters(permissionVM, permissionVM.PermissionID == 0 ? "INSERT" : "UPDATE"));

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

        public ResponseVM AssignPermissions(long profileID, List<long> permissionIDs, List<long> submodulesIDs)
        {
            var res = new ResponseVM();

            try
            {
                using (var command = new SqlCommand("SPPermission", Open()))
                {
                    command.CommandType = CommandType.StoredProcedure;

                   
                    var permissionVM = new PermissionVM
                    {
                        ProfileID = profileID
                    };

                   
                    string permissionIDsString = string.Join(",", permissionIDs);
                    string submodulesIDsString = string.Join(",", submodulesIDs);
                   
                    command.Parameters.Add(new SqlParameter("@ProfileID", profileID));
                    command.Parameters.Add(new SqlParameter("@PermissionIDs", permissionIDsString));
                    command.Parameters.Add(new SqlParameter("@SubmodulesIDs", submodulesIDsString));
                    command.Parameters.Add(new SqlParameter("@Case", "ASSIGN_PERMISSIONS"));

                    // Ejecuta el procedimiento almacenado y procesa el resultado
                    using (var dr = command.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            res.DBCatchResponseInOneLine(dr);
                        }
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



        private SqlParameter[] _parameters(PermissionVM permissionVM, string action)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@PermissionID", permissionVM.PermissionID),
                new SqlParameter("@PerStatus", permissionVM.PerStatus),
                new SqlParameter("@SubRDate", permissionVM.SubRDate.HasValue ? (object)permissionVM.SubRDate.Value : DBNull.Value),
                new SqlParameter("@SubModuleID", permissionVM.SubModuleID),
                new SqlParameter("@ProfileID", permissionVM.ProfileID),
                new SqlParameter("@Case", action)
            };
        }
    }
}
