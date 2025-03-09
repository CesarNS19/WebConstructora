using System.Data;
using Microsoft.Data.SqlClient;
using ConstructoraWeb.Models.ViewModels;

namespace ConstructoraWeb.Models.Services;

public class SFBrandsSrv: DBAccess 
{
    private readonly DBAccess cnx;

        public SFBrandsSrv(string connectionString) : base(connectionString)
        {
            this.cnx = new DBAccess(connectionString);
        }

        public ResponseVM<List<SFBrandsVM>> List(SFBrandsVM sfBrandsVm)
        {
            ResponseVM<List<SFBrandsVM>> res = new ResponseVM<List<SFBrandsVM>>()
            {
                Data = new List<SFBrandsVM>()
            };

            try
            {
                var command = new SqlCommand("SPBrands", Open()) { CommandType = CommandType.StoredProcedure };
                command.Parameters.AddRange(_parameters(sfBrandsVm, sfBrandsVm.id_marca == 0 ? "SELECT_ALL" : "SELECT_BY_ID"));

                using (var dr = command.ExecuteReader())
                {
                    res.HasRow(dr);
                    while (dr.Read())
                    {
                        res.Data.Add(new SFBrandsVM
                        {
                            id_marca = dr.GetInt64(dr.GetOrdinal("id_marca")),
                            nombre_marca = dr.GetString(dr.GetOrdinal("nombre_marca")),
                            estatus = dr.GetString(dr.GetOrdinal("estatus"))
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

        public ResponseVM AddUpdate(SFBrandsVM sfBrandsVm)
        {
            ResponseVM res = new ResponseVM();

            try
            {
                var command = new SqlCommand("SPBrands", Open()) { CommandType = CommandType.StoredProcedure };
                command.Parameters.AddRange(_parameters(sfBrandsVm, sfBrandsVm.id_marca == 0 ? "ADD" : "UPDATE"));

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
        public ResponseVM HandleStatus(SFBrandsVM sfBrandsVm, string action)
        {
            ResponseVM res = new ResponseVM();

            try
            {
                var command = new SqlCommand("SPBrands", Open()) { CommandType = CommandType.StoredProcedure };

                string caseType = (action == "Activate") ? "ACTIVATE" :
                                  (action == "Deactivate") ? "DEACTIVATE" : "";

                command.Parameters.AddRange(_parameters(sfBrandsVm, caseType));

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

        private SqlParameter[] _parameters(SFBrandsVM sfBrandsVm, string action)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@id_marca", sfBrandsVm.id_marca),
                new SqlParameter("@nombre_marca" , sfBrandsVm.nombre_marca),
                new SqlParameter("@estatus" , sfBrandsVm.estatus),
                new SqlParameter("@Action", action)
            };
        }
}