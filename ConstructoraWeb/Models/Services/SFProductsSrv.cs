using System.Data;
using Microsoft.Data.SqlClient;
using ConstructoraWeb.Models.ViewModels;

namespace ConstructoraWeb.Models.Services;

public class SFProductsSrv : DBAccess
{
    private readonly DBAccess cnx;

    public SFProductsSrv(string connectionString) : base(connectionString)
    {
        this.cnx = new DBAccess(connectionString);
    }

    public ResponseVM<List<SFProductsVM>> List(SFProductsVM sfProductsVm)
    {
        ResponseVM<List<SFProductsVM>> res = new ResponseVM<List<SFProductsVM>>()
        {
            Data = new List<SFProductsVM>()
        };

        try
        {
            var command = new SqlCommand("SPParfum", Open()) { CommandType = CommandType.StoredProcedure };
            command.Parameters.AddRange(_parameters(sfProductsVm, sfProductsVm.id_producto == 0 ? "SELECT_ALL" : "SELECT_BY_ID"));

            using (var dr = command.ExecuteReader())
            {
                res.HasRow(dr);
                while (dr.Read())
                {
                    res.Data.Add(new SFProductsVM
                    {
                        id_producto = dr.GetInt32(dr.GetOrdinal("id_producto")),
                        id_marca = dr.GetInt64(dr.GetOrdinal("id_marca")),
                        nombre = dr.GetString(dr.GetOrdinal("nombre")),
                        tipo_fragancia = dr.GetString(dr.GetOrdinal("tipo_fragancia")),
                        genero = dr.GetString(dr.GetOrdinal("genero")),
                        tamanio_ml = dr.GetInt32(dr.GetOrdinal("tamanio_ml")),
                        precio = dr.GetDecimal(dr.GetOrdinal("precio")),
                        stock = dr.GetInt32(dr.GetOrdinal("stock")),
                        descripcion = dr.GetString(dr.GetOrdinal("descripcion")),
                        estatus = dr.GetString(dr.GetOrdinal("estatus")),
                        imagen = dr.GetString(dr.GetOrdinal("imagen")),

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

    public ResponseVM AddUpdate(SFProductsVM sfProductsVm)
    {
        ResponseVM res = new ResponseVM();

        try
        {
            var command = new SqlCommand("SPParfum", Open()) { CommandType = CommandType.StoredProcedure };
            command.Parameters.AddRange(_parameters(sfProductsVm, sfProductsVm.id_producto == 0 ? "ADD" : "UPDATE"));

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
     public ResponseVM HandleStatus(SFProductsVM sfProductsVm, string action)
        {
            ResponseVM res = new ResponseVM();

            try
            {
                var command = new SqlCommand("SPParfum", Open()) { CommandType = CommandType.StoredProcedure };

                string caseType = (action == "Activate") ? "ACTIVATE" :
                                  (action == "Deactivate") ? "DEACTIVATE" : "";

                command.Parameters.AddRange(_parameters(sfProductsVm, caseType));

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
     
        public ResponseVM DeleteParfum(SFProductsVM sFProductsVM)
        {
            ResponseVM res = new ResponseVM();

            try
            {
                var command = new SqlCommand("SPParfum", Open()) { CommandType = CommandType.StoredProcedure };
                
                command.Parameters.AddRange(_parameters(sFProductsVM, "DELETE"));

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

    private SqlParameter[] _parameters(SFProductsVM sfProductsVm, string action)
    {
        return new SqlParameter[]
        {
                new SqlParameter("@id_producto", sfProductsVm.id_producto),
                new SqlParameter("@id_marca", sfProductsVm.id_marca),
                new SqlParameter("@nombre", sfProductsVm.nombre),
                new SqlParameter("@tipo_fragancia", sfProductsVm.tipo_fragancia),
                new SqlParameter("@genero", sfProductsVm.genero),
                new SqlParameter("@tamanio", sfProductsVm.tamanio_ml),
                new SqlParameter("@precio", sfProductsVm.precio),
                new SqlParameter("@stock" , sfProductsVm.stock),
                new SqlParameter("@descripcion" , sfProductsVm.descripcion),
                new SqlParameter("estatus", sfProductsVm.estatus),
                new SqlParameter("imagen", sfProductsVm.imagen),
                new SqlParameter("@Action", action)
        };
    }
}