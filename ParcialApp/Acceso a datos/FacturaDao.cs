using ParcialApp.Dominio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcialApp.Acceso_a_datos
{
    class FacturaDao : IFacturaDao
    {
        public int ProximoNro()
        {
            
            try
            {
                SqlConnection cnn = new SqlConnection();
                cnn.ConnectionString = @"Data Source=localhost;Initial Catalog=db_facturas;Integrated Security=True";
                cnn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_PROXIMO_ID";

                SqlParameter param = new SqlParameter("@next", SqlDbType.Int);
                param.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();
                cnn.Close();

                return Convert.ToInt32(param.Value);
            }
            catch (Exception)
            {
                return 1;
                throw;
            }
        }
        public bool Crear(Factura factura)
        {
            bool estado = true;
            SqlConnection cnn = new SqlConnection();
            SqlTransaction t = null;
            try
            {
                cnn.ConnectionString = @"Data Source=localhost;Initial Catalog=db_facturas;Integrated Security=True";
                cnn.Open();

                t = cnn.BeginTransaction();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.Transaction = t;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_INSERTAR_FACTURA";

                cmd.Parameters.AddWithValue("@cliente", factura.Cliente);
                cmd.Parameters.AddWithValue("@forma", factura.FormaPago);
                cmd.Parameters.AddWithValue("@total", factura.Total);
                cmd.Parameters.AddWithValue("@nro", factura.Numero);

                cmd.ExecuteNonQuery();

                int contador = 1;
                foreach (DetalleFactura item in factura.Detalles)
                {
                    SqlCommand cmdDet = new SqlCommand();
                    cmdDet.Connection = cnn;
                    cmdDet.Transaction = t;
                    cmdDet.CommandType = CommandType.StoredProcedure;
                    cmdDet.CommandText = "SP_INSERTAR_DETALLES";

                    cmdDet.Parameters.AddWithValue("@nro", factura.Numero);
                    cmdDet.Parameters.AddWithValue("@detalle", contador);
                    cmdDet.Parameters.AddWithValue("@id_producto", item.Producto.IdProducto);
                    cmdDet.Parameters.AddWithValue("@cantidad", item.Cantidad);
                    cmdDet.ExecuteNonQuery();
                    contador++;
                }

                t.Commit();
            }
            catch (Exception ex)
            {
                t.Rollback();
                estado = false;
            }
            finally
            {
                if (cnn.State == ConnectionState.Open)
                {
                    cnn.Close();
                }
            }
            return estado;
        }

        public DataTable ListarProductos()
        {
            SqlConnection cnn = new SqlConnection();
            cnn.ConnectionString = @"Data Source=localhost;Initial Catalog=db_facturas;Integrated Security=True";
            cnn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_CONSULTAR_PRODUCTOS";

            DataTable table = new DataTable();
            table.Load(cmd.ExecuteReader());
            cnn.Close();
            return table;
        }
    }
}
