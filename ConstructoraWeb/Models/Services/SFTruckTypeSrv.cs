using System.Data;
using Microsoft.Data.SqlClient;
using ConstructoraWeb.Models.ViewModels;

namespace ConstructoraWeb.Models.Services;

public class SFTruckTypeSrv : DBAccess 
{
    private readonly DBAccess cnx;

        public SFTruckTypeSrv(string connectionString) : base(connectionString)
        {
            this.cnx = new DBAccess(connectionString);
        }

        public ResponseVM<List<SFTruckTypeVM>> List(SFTruckTypeVM sfTruckTypeVM)
        {
            ResponseVM<List<SFTruckTypeVM>> res = new ResponseVM<List<SFTruckTypeVM>>()
            {
                Data = new List<SFTruckTypeVM>()
            };

            try
            {
                var command = new SqlCommand("SPTruckType", Open()) { CommandType = CommandType.StoredProcedure };
                command.Parameters.AddRange(_parameters(sfTruckTypeVM, sfTruckTypeVM.TypeID == 0 ? "SELECT_ALL" : "SELECT_BY_ID"));

                using (var dr = command.ExecuteReader())
                {
                    res.HasRow(dr);
                    while (dr.Read())
                    {
                        res.Data.Add(new SFTruckTypeVM
                        {
                            TypeID = dr.GetInt64(dr.GetOrdinal("TypeID")),
                            TypeName = dr.GetString(dr.GetOrdinal("TypeName")),
                            TypeSTS = dr.GetString(dr.GetOrdinal("TypeSTS")),
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

        public ResponseVM AddUpdate(SFTruckTypeVM sfTruckTypeVM)
        {
            ResponseVM res = new ResponseVM();

            try
            {
                var command = new SqlCommand("SPTruckType", Open()) { CommandType = CommandType.StoredProcedure };
                command.Parameters.AddRange(_parameters(sfTruckTypeVM, sfTruckTypeVM.TypeID == 0 ? "ADD" : "UPDATE"));

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

        public ResponseVM HandleStatus(SFTruckTypeVM sfTruckTypeVM, string action)
        {
            ResponseVM res = new ResponseVM();

            try
            {
                var command = new SqlCommand("SPTruckType", Open()) { CommandType = CommandType.StoredProcedure };

                string caseType = (action == "Activate") ? "ACTIVATE" :
                                  (action == "Deactivate") ? "DEACTIVATE" : "";

                command.Parameters.AddRange(_parameters(sfTruckTypeVM, caseType));

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

        public ResponseVM DeleteTruckType(SFTruckTypeVM sfTruckTypeVM)
        {
            ResponseVM res = new ResponseVM();

            try
            {
                var command = new SqlCommand("SPTruckType", Open()) { CommandType = CommandType.StoredProcedure };
                
                command.Parameters.AddRange(_parameters(sfTruckTypeVM, "DELETE"));

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

        private SqlParameter[] _parameters(SFTruckTypeVM sfTruckTypeVM, string action)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@TypeID", sfTruckTypeVM.TypeID),
                new SqlParameter("@TypeName" , sfTruckTypeVM.TypeName),
                new SqlParameter("@TypeSTS" , sfTruckTypeVM.TypeSTS),
                new SqlParameter("@Action", action)
            };
        }
}