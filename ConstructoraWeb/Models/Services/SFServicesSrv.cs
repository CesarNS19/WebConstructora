using System.Data;
using Microsoft.Data.SqlClient;
using ConstructoraWeb.Models.ViewModels;

namespace ConstructoraWeb.Models.Services;

public class SFServicesSrv : DBAccess
{
    private readonly DBAccess cnx;

    public SFServicesSrv(string connectionString) : base(connectionString)
    {
        this.cnx = new DBAccess(connectionString);
    }

    public ResponseVM<List<SFservicesVM>> List(SFservicesVM sFservicesVm)
    {
        ResponseVM<List<SFservicesVM>> res = new ResponseVM<List<SFservicesVM>>()
        {
            Data = new List<SFservicesVM>()
        };

        try
        {
            var command = new SqlCommand("SPMaterial", Open()) { CommandType = CommandType.StoredProcedure };
            command.Parameters.AddRange(_parameters(sFservicesVm, sFservicesVm.MeterialID == 0 ? "SELECT_ALL" : "SELECT_BY_ID"));

            using (var dr = command.ExecuteReader())
            {
                res.HasRow(dr);
                while (dr.Read())
                {
                    res.Data.Add(new SFservicesVM
                    {
                        MeterialID = dr.GetInt64(dr.GetOrdinal("MeterialID")),
                        TypeMeterialID = dr.GetInt64(dr.GetOrdinal("TypeMeterialID")),
                        MeterialName = dr.GetString(dr.GetOrdinal("MeterialName")),
                        MeterialWeight = (float)dr.GetDouble(dr.GetOrdinal("MeterialWeight")),
                        MeterialX = (float)dr.GetDouble(dr.GetOrdinal("MeterialX")),
                        MeterialY = (float)dr.GetDouble(dr.GetOrdinal("MeterialY")),
                        MeterialZ = (float)dr.GetDouble(dr.GetOrdinal("MeterialZ")),
                        MeterialSTS = dr.GetString(dr.GetOrdinal("MeterialSTS")),
                        MeterialImage = dr.GetString(dr.GetOrdinal("MeterialImage")),
                        TypeMeterialName = dr.GetString(dr.GetOrdinal("TypeMeterialName")),
                        TypeMeterialSTS = dr.GetString(dr.GetOrdinal("TypeMeterialSTS"))

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

    public ResponseVM AddUpdate(SFservicesVM sFservicesVm)
    {
        ResponseVM res = new ResponseVM();

        try
        {
            var command = new SqlCommand("SPMaterial", Open()) { CommandType = CommandType.StoredProcedure };
            command.Parameters.AddRange(_parameters(sFservicesVm, sFservicesVm.MeterialID == 0 ? "ADD" : "UPDATE"));

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
     public ResponseVM HandleStatus(SFservicesVM sFservicesVm, string action)
        {
            ResponseVM res = new ResponseVM();

            try
            {
                var command = new SqlCommand("SPMaterial", Open()) { CommandType = CommandType.StoredProcedure };

                // Determinar el caso según la acción (Activate o Deactivate)
                string caseType = (action == "Activate") ? "ACTIVATE" :
                                  (action == "Deactivate") ? "DEACTIVATE" : "";

                command.Parameters.AddRange(_parameters(sFservicesVm, caseType));

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

    private SqlParameter[] _parameters(SFservicesVM sFservicesVm, string action)
    {
        return new SqlParameter[]
        {
                new SqlParameter("@MeterialID", sFservicesVm.MeterialID),
                new SqlParameter("@TypeMeterialID", sFservicesVm.TypeMeterialID),
                new SqlParameter("@MeterialName", sFservicesVm.MeterialName),
                new SqlParameter("@MeterialWeight", sFservicesVm.MeterialWeight),
                new SqlParameter("@MeterialX", sFservicesVm.MeterialX),
                new SqlParameter("@MeterialY", sFservicesVm.MeterialY),
                new SqlParameter("@MeterialZ", sFservicesVm.MeterialZ),
                new SqlParameter("@MeterialSTS" , sFservicesVm.MeterialSTS),
                new SqlParameter("@MeterialImage" , sFservicesVm.MeterialImage),
                new SqlParameter("TypeMeterialSTS", sFservicesVm.TypeMeterialSTS),
                new SqlParameter("TypeMeterialName", sFservicesVm.TypeMeterialName),
                new SqlParameter("@Action", action)
        };
    }
}