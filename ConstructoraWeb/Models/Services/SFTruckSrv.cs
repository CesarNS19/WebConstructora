using System.Data;
using Microsoft.Data.SqlClient;
using ConstructoraWeb.Models.ViewModels;

namespace ConstructoraWeb.Models.Services
{
    public class SFTruckSrv : DBAccess
    {
        public SFTruckSrv(string connectionString) : base(connectionString)
        {
        }

        public ResponseVM<List<SFTruckVM>> List(SFTruckVM sfTruckVM)
        {
            var res = new ResponseVM<List<SFTruckVM>>()
            {
                Data = new List<SFTruckVM>()
            };

            try
            {
                using (var connection = Open())
                {
                    var command = new SqlCommand("SPTruck", connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    command.Parameters.AddRange(_parameters(sfTruckVM, sfTruckVM.TruckID == 0 ? "SELECT_ALL" : "SELECT_BY_ID"));

                    using (var dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            res.Data.Add(new SFTruckVM
                            {
                                TruckID = dr.GetInt64(dr.GetOrdinal("TruckID")),
                                TypeID = dr.GetInt64(dr.GetOrdinal("TypeID")),
                                TruckName = dr.GetString(dr.GetOrdinal("TruckName")),
                                TruckSupportWeight = (float)dr.GetDouble(dr.GetOrdinal("TruckSupportWeight")),
                                TruckX = (float)dr.GetDouble(dr.GetOrdinal("TruckX")),
                                TruckY = (float)dr.GetDouble(dr.GetOrdinal("TruckY")),
                                TruckZ = (float)dr.GetDouble(dr.GetOrdinal("TruckZ")),
                                TruckSTS = dr.GetString(dr.GetOrdinal("TruckSTS")),
                                TruckImage = dr.GetString(dr.GetOrdinal("TruckImage")),
                            });
                        }

                        res.HasRow(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                res.Error(ex);
            }
            
            return res;
        }

        public ResponseVM AddUpdate(SFTruckVM sfTruckVM)
    {
        ResponseVM res = new ResponseVM();

        try
        {
            var command = new SqlCommand("SPTruck", Open()) { CommandType = CommandType.StoredProcedure };
            command.Parameters.AddRange(_parameters(sfTruckVM, sfTruckVM.TruckID == 0 ? "ADD" : "UPDATE"));

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

        public ResponseVM HandleStatus(SFTruckVM sfTruckVM, string action)
        {
            ResponseVM res = new ResponseVM();

            try
            {
                var command = new SqlCommand("SPTruck", Open()) { CommandType = CommandType.StoredProcedure };

                string caseType = (action == "Activate") ? "ACTIVATE" :
                                  (action == "Deactivate") ? "DEACTIVATE" : "";

                command.Parameters.AddRange(_parameters(sfTruckVM, caseType));

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

        public ResponseVM DeleteTruck(SFTruckVM sfTruckVM)
        {
            ResponseVM res = new ResponseVM();

            try
            {
                var command = new SqlCommand("SPTruck", Open()) { CommandType = CommandType.StoredProcedure };
                
                command.Parameters.AddRange(_parameters(sfTruckVM, "DELETE"));

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

        private SqlParameter[] _parameters(SFTruckVM sfTruckVM, string action)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@TruckID", SqlDbType.BigInt) { Value = sfTruckVM.TruckID },
                new SqlParameter("@TypeID", SqlDbType.BigInt) { Value = sfTruckVM.TypeID },
                new SqlParameter("@TruckName", SqlDbType.NVarChar, 250) { Value = sfTruckVM.TruckName },
                new SqlParameter("@TruckSupportWeight", SqlDbType.Float) { Value = sfTruckVM.TruckSupportWeight },
                new SqlParameter("@TruckX", SqlDbType.Float) { Value = sfTruckVM.TruckX },
                new SqlParameter("@TruckY", SqlDbType.Float) { Value = sfTruckVM.TruckY },
                new SqlParameter("@TruckZ", SqlDbType.Float) { Value = sfTruckVM.TruckZ },
                new SqlParameter("@TruckSTS", SqlDbType.NVarChar, 20) { Value = sfTruckVM.TruckSTS },
                new SqlParameter("@TruckImage", SqlDbType.NVarChar, 250) { Value = sfTruckVM.TruckImage },
                new SqlParameter("@Action", SqlDbType.NVarChar, 20) { Value = action }
            };
        }
    }
}
