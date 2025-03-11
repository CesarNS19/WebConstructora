using System.Data;
using Microsoft.Data.SqlClient;
using ConstructoraWeb.Models.ViewModels;

namespace ConstructoraWeb.Models.Services;

public class SFCartSrv: DBAccess 
{
    private readonly DBAccess cnx;

    public SFCartSrv(string connectionString) : base(connectionString)
    {
        this.cnx = new DBAccess(connectionString);
    }
    
        public ResponseVM Add(SFCartVM _sfCart)
        {
            ResponseVM res = new ResponseVM();

            try
            {
                var command = new SqlCommand("SPCart", Open()) { CommandType = CommandType.StoredProcedure };
                command.Parameters.AddRange(_parameters(_sfCart, "ADD"));

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
        
        private SqlParameter[] _parameters(SFCartVM _sfCart, string action)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@id_carrito", _sfCart.id_carrito),
                new SqlParameter("@id_marca", _sfCart.id_marca),
                new SqlParameter("@id_producto", _sfCart.id_producto),
                new SqlParameter("@cantidad", _sfCart.cantidad),
                new SqlParameter("@subtotal", _sfCart.subtotal),
                new SqlParameter("@email_usuario", _sfCart.email_usuario),
                new SqlParameter("@Action", action)
            };
        }
    }