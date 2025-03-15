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
    
    public ResponseVM<List<SFCartVM>> List(SFCartVM sfCartVm)
    {
        ResponseVM<List<SFCartVM>> res = new ResponseVM<List<SFCartVM>>()
        {
            Data = new List<SFCartVM>()
        };

        try
        {
            var command = new SqlCommand("SPCart", Open()) { CommandType = CommandType.StoredProcedure };
            command.Parameters.AddRange(_parameters(sfCartVm,  "SELECT_ALL"));

            using (var dr = command.ExecuteReader())
            {
                res.HasRow(dr);
                while (dr.Read())
                {
                    res.Data.Add(new SFCartVM
                    {
                        id_carrito = dr.GetInt64(dr.GetOrdinal("id_carrito")),
                        id_producto = dr.GetInt32(dr.GetOrdinal("id_producto")),
                        id_marca = dr.GetInt64(dr.GetOrdinal("id_marca")),
                        cantidad = dr.GetInt32(dr.GetOrdinal("cantidad")),
                        subtotal = dr.GetDecimal(dr.GetOrdinal("subtotal")),
                        precio = dr.GetDecimal(dr.GetOrdinal("precio")),
                        nombre = dr.GetString(dr.GetOrdinal("nombre")),  
                        nombre_marca = dr.GetString(dr.GetOrdinal("nombre_marca")), 
                        tipo_fragancia = dr.GetString(dr.GetOrdinal("tipo_fragancia")), 
                        genero = dr.GetString(dr.GetOrdinal("genero")),
                        tamanio_ml = dr.GetInt32(dr.GetOrdinal("tamanio_ml")),
                        descripcion = dr.GetString(dr.GetOrdinal("descripcion")),
                        imagen = dr.GetString(dr.GetOrdinal("imagen")),
                        email_usuario = dr.GetString(dr.GetOrdinal("email_usuario"))
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
    
    public ResponseVM Add(SFCartVM sfCartVM)
        {
            ResponseVM res = new ResponseVM();

            try
            {
                var command = new SqlCommand("SPCart", Open()) { CommandType = CommandType.StoredProcedure };
                command.Parameters.AddRange(_parameters(sfCartVM, "ADD"));

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
    
        public ResponseVM ResCant(SFCartVM sfCartVM)
        {
            ResponseVM res = new ResponseVM();

            try
            {
                var command = new SqlCommand("SPCart", Open()) { CommandType = CommandType.StoredProcedure };
                command.Parameters.AddRange(_parameters(sfCartVM, "RES"));

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
        
        public ResponseVM DeleteCart(SFCartVM sfCartVM)
        {
            ResponseVM res = new ResponseVM();

            try
            {
                var command = new SqlCommand("SPCart", Open()) { CommandType = CommandType.StoredProcedure };
                command.Parameters.AddRange(_parameters(sfCartVM, "DELETE"));

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
        
        public ResponseVM AddCant(SFCartVM sfCartVM)
        {
            ResponseVM res = new ResponseVM();

            try
            {
                var command = new SqlCommand("SPCart", Open()) { CommandType = CommandType.StoredProcedure };
                command.Parameters.AddRange(_parameters(sfCartVM, "SUM"));

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
        
        private SqlParameter[] _parameters(SFCartVM sfCartVM, string action)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@id_carrito", sfCartVM.id_carrito),
                new SqlParameter("@id_marca", sfCartVM.id_marca),
                new SqlParameter("@id_producto", sfCartVM.id_producto),
                new SqlParameter("@cantidad", sfCartVM.cantidad),
                new SqlParameter("@subtotal", sfCartVM.subtotal),
                new SqlParameter("@precio", sfCartVM.precio),
                new SqlParameter("@nombre", sfCartVM.nombre),
                new SqlParameter("@nombre_marca", sfCartVM.nombre_marca),
                new SqlParameter("@tipo_fragancia", sfCartVM.tipo_fragancia),
                new SqlParameter("@genero", sfCartVM.genero),
                new SqlParameter("@tamanio", sfCartVM.tamanio_ml),
                new SqlParameter("@descripcion", sfCartVM.descripcion),
                new SqlParameter("@imagen", sfCartVM.imagen),
                new SqlParameter("@email_usuario", sfCartVM.email_usuario),
                new SqlParameter("@Action", action)
            };
        }
    }