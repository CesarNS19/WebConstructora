using System.Data;
using Microsoft.Data.SqlClient;
using ConstructoraWeb.Models.ViewModels;

namespace ConstructoraWeb.Models.Services;

public class SFMaterialTypeSrv: DBAccess 
{
    private readonly DBAccess cnx;

        public SFMaterialTypeSrv(string connectionString) : base(connectionString)
        {
            this.cnx = new DBAccess(connectionString);
        }

        public ResponseVM<List<SFMaterialTypeVM>> List(SFMaterialTypeVM sfMaterialTypeVM)
        {
            ResponseVM<List<SFMaterialTypeVM>> res = new ResponseVM<List<SFMaterialTypeVM>>()
            {
                Data = new List<SFMaterialTypeVM>()
            };

            try
            {
                var command = new SqlCommand("SPMaterialType", Open()) { CommandType = CommandType.StoredProcedure };
                command.Parameters.AddRange(_parameters(sfMaterialTypeVM, sfMaterialTypeVM.TypeMeterialID == 0 ? "SELECT_ALL" : "SELECT_BY_ID"));

                using (var dr = command.ExecuteReader())
                {
                    res.HasRow(dr);
                    while (dr.Read())
                    {
                        res.Data.Add(new SFMaterialTypeVM
                        {
                            TypeMeterialID = dr.GetInt64(dr.GetOrdinal("TypeMeterialID")),
                            TypeMeterialName = dr.GetString(dr.GetOrdinal("TypeMeterialName")),
                            TypeMeterialSTS = dr.GetString(dr.GetOrdinal("TypeMeterialSTS")),
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

        public ResponseVM AddUpdate(SFMaterialTypeVM sfMaterialTypeVM)
        {
            ResponseVM res = new ResponseVM();

            try
            {
                var command = new SqlCommand("SPMaterialType", Open()) { CommandType = CommandType.StoredProcedure };
                command.Parameters.AddRange(_parameters(sfMaterialTypeVM, sfMaterialTypeVM.TypeMeterialID == 0 ? "ADD" : "UPDATE"));

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
        public ResponseVM HandleStatus(SFMaterialTypeVM sfMaterialTypeVM, string action)
        {
            ResponseVM res = new ResponseVM();

            try
            {
                var command = new SqlCommand("SPMaterialType", Open()) { CommandType = CommandType.StoredProcedure };

                // Determinar el caso según la acción (Activate o Deactivate)
                string caseType = (action == "Activate") ? "ACTIVATE" :
                                  (action == "Deactivate") ? "DEACTIVATE" : "";

                command.Parameters.AddRange(_parameters(sfMaterialTypeVM, caseType));

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

        private SqlParameter[] _parameters(SFMaterialTypeVM sfMaterialTypeVM, string action)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@TypeMeterialID", sfMaterialTypeVM.TypeMeterialID),
                new SqlParameter("@TypeMeterialName" , sfMaterialTypeVM.TypeMeterialName),
                new SqlParameter("@TypeMeterialSTS" , sfMaterialTypeVM.TypeMeterialSTS),
                new SqlParameter("@Action", action)
            };
        }
}