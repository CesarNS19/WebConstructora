using System.Data;
using Microsoft.Data.SqlClient;
using ConstructoraWeb.Models.ViewModels;

namespace ConstructoraWeb.Models.Services
{
    public class AccountSrv : DBAccess
    {
        private readonly DBAccess cnx;

        public AccountSrv(string connectionString) : base(connectionString)
        {
            this.cnx = new DBAccess(connectionString);
        }

        public ResponseVM<List<AccountVM>> List(AccountVM accountVM)
        {
            ResponseVM<List<AccountVM>> res = new ResponseVM<List<AccountVM>>()
            {
                Data = new List<AccountVM>()
            };

            try
            {
                var command = new SqlCommand("SPAccountNew", Open()) { CommandType = CommandType.StoredProcedure };
                command.Parameters.AddRange(_parameters(accountVM, accountVM.AccountID == 0 ? "SELECT_ALL" : "SELECT_BY_ID"));

                using (var dr = command.ExecuteReader())
                {
                    res.HasRow(dr);
                    while (dr.Read())
                    {
                        res.Data.Add(new AccountVM
                        {
                            AccountID = GetInt(dr, "AccountID"),
                            AcUser = GetString(dr, "AcUser"),
                            AcEmailAddress = GetString(dr, "AcEmailAddress"),
                            AcPhoneNumber = GetString(dr, "AcPhoneNumber"),
                            AcRDate = GetString(dr, "AcRDate"),
                            AcStatus = GetString(dr, "AcStatus"),
                            PeopleID = GetInt(dr, "PeopleID"),
                            PeFirstName = GetString(dr, "PeFirstName"),
                            PeLastName = GetString(dr, "PeLastName"),
                            PeMiddleInitial = GetString(dr, "PeMiddleInitial"),
                            ProfileID = GetInt(dr, "ProfileID"),
                            ProDescription = GetString(dr, "ProDescription")
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

        public ResponseVM<string> Login(string email, string password)
        {
            var response = new ResponseVM<string>();

            try
            {
                using (var connection = Open())
                using (var command = new SqlCommand("SPAccount", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@Email", email));
                    command.Parameters.Add(new SqlParameter("@Password", password));

                    using (var dr = command.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            string message = dr.GetString(0);

                            if (message == "Acceso permitido")
                            {
                                response.Success(message);
                                response.Data = dr["PeFirstName"] + " " + dr["PeMiddleInitial"] + " " + dr["PeLastName"];
                            }
                            else
                            {
                                response.Error(message);
                            }
                        }
                        else
                        {
                            response.NotFind();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.Error(ex);
            }
            finally
            {
                Close();
            }

            return response;
        }
        

        public ResponseVM AddUpdate(AccountVM accountVM)
        {
            ResponseVM res = new ResponseVM();

            try
            {
                var command = new SqlCommand("SPAccountNew", Open()) { CommandType = CommandType.StoredProcedure };
                command.Parameters.AddRange(_parameters(accountVM, accountVM.AccountID == 0 ? "ADD" : "UPDATE"));

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


        public ResponseVM HandleStatus(AccountVM accountVM, string action)
        {
            ResponseVM res = new ResponseVM();

            try
            {
                var command = new SqlCommand("SPAccountNew", Open()) { CommandType = CommandType.StoredProcedure };

                // Determinar el caso según la acción (Activate o Deactivate)
                string caseType = (action == "Activate") ? "ACTIVATE" :
                                  (action == "Deactivate") ? "DEACTIVATE" : "";

                command.Parameters.AddRange(_parameters(accountVM, caseType));

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

        public ResponseVM ChangePass(AccountVM accountVM)
        {
            ResponseVM res = new ResponseVM();

            try
            {
                var command = new SqlCommand("SPAccountNew", Open()) { CommandType = CommandType.StoredProcedure };
                command.Parameters.AddRange(_parameters(accountVM, accountVM.AccountID > 0 ? "CHANGE_PASS" : ""));

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

        private SqlParameter[] _parameters(AccountVM accountVM, string Action)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@Id", accountVM.AccountID),
                new SqlParameter("@User", accountVM.AcUser),
                new SqlParameter("@Password", accountVM.AcPassword),
                new SqlParameter("@Email", accountVM.AcEmailAddress),
                new SqlParameter("@PhoneNumber", accountVM.AcPhoneNumber),
                new SqlParameter("@Status", accountVM.AcStatus),
                new SqlParameter("@Fecha", accountVM.AcRDate),
                new SqlParameter("@PeopleID", accountVM.PeopleID),
                new SqlParameter("@FirstName", accountVM.PeFirstName),
                new SqlParameter("@MiddleInitial", accountVM.PeMiddleInitial),
                new SqlParameter("@LastName", accountVM.PeLastName),
                new SqlParameter("@Pestatus", accountVM.PeStatus),
                new SqlParameter("@PeFecha", accountVM.PeRDate),
                new SqlParameter("@ProfileID", accountVM.ProfileID),
                new SqlParameter("@Descripcion", accountVM.ProDescription),
                new SqlParameter("@Prostatus", accountVM.ProStatus),
                new SqlParameter("@ProFecha", accountVM.ProRDate),
                new SqlParameter("@PeStreet", accountVM.PeStreet),
                new SqlParameter("@PeOutsideCode", accountVM.PeOutsideCode),
                new SqlParameter("@PeInsideCode", accountVM.PeInsideCode),
                new SqlParameter("@SettlementID", accountVM.SettlementID),
                new SqlParameter("@Action", Action)
            };
        }
    }
}
