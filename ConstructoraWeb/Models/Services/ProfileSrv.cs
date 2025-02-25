using System.Data;
using Microsoft.Data.SqlClient;
using ConstructoraWeb.Models.ViewModels;

namespace ConstructoraWeb.Models.Services
{
    public class ProfileSrv : DBAccess
    {
        public ProfileSrv(string connectionString) : base(connectionString) { }

        public ResponseVM<List<ProfileVM>> List(ProfileVM profileVM)
        {
            ResponseVM<List<ProfileVM>> res = new ResponseVM<List<ProfileVM>>()
            {
                Data = new List<ProfileVM>()
            };
            try
            {
                var command = new SqlCommand("SPProfiles", Open()) { CommandType = System.Data.CommandType.StoredProcedure };
                command.Parameters.AddRange(_parameters(profileVM, profileVM.ProfileID == 0 ? "SELECT_ALL" : "SELECT_BY_ID"));
                using (var dr = command.ExecuteReader())
                {
                    res.HasRow(dr);
                    while (dr.Read())
                    {
                        res.Data.Add(new ProfileVM()
                        {
                            ProfileID = GetInt(dr, "ProfileID"),
                            ProDescription = GetString(dr, "ProDescription"),
                            ProStatus = GetString(dr, "ProStatus"),
                            ProRDate = dr.IsDBNull(dr.GetOrdinal("ProRDate")) ? (DateTime?)null : dr.GetDateTime(dr.GetOrdinal("ProRDate")),
                        });
                    }
                }
                Close();

            }
            catch (Exception e)
            {
                Close();
                res.Error(e);
            }
            return res;
        }

        public ResponseVM AddUpdate(ProfileVM profileVM)
        {
            ResponseVM res = new ResponseVM();

            try
            {
                var command = new SqlCommand("SPProfiles", Open()) { CommandType = CommandType.StoredProcedure };
                if (!profileVM.ProRDate.HasValue)
                {
                    profileVM.ProRDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                }
                command.Parameters.AddRange(_parameters(profileVM, profileVM.ProfileID == 0 ? "ADD" : "UPDATE"));

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

        public ResponseVM HandleStatus(ProfileVM profileVM, string action)
        {
            ResponseVM res = new ResponseVM();

            try
            {
                var command = new SqlCommand("SPProfiles", Open()) { CommandType = CommandType.StoredProcedure };

                string caseType = (action == "Activate") ? "ACTIVATE" :
                                  (action == "Deactivate") ? "DEACTIVATE" : "";

                command.Parameters.AddRange(_parameters(profileVM, caseType));

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

        public ResponseVM DeleteProfile(ProfileVM profileVM)
        {
            ResponseVM res = new ResponseVM();

            try
            {
                var command = new SqlCommand("SPProfiles", Open()) { CommandType = CommandType.StoredProcedure };
                
                command.Parameters.AddRange(_parameters(profileVM, "DELETE"));

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


        private SqlParameter[] _parameters(ProfileVM profileVM, String action)
        {
            return new SqlParameter[] {
                new SqlParameter("@Id", profileVM.ProfileID),
                new SqlParameter("@Descripcion", profileVM.ProDescription),
                new SqlParameter("@Estatus", profileVM.ProStatus),
                new SqlParameter("@Fecha", profileVM.ProRDate.HasValue ? (object)profileVM.ProRDate.Value : DBNull.Value),
                new SqlParameter("@Action", action)
            };
        }
    }
}