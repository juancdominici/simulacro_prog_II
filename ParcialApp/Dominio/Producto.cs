using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcialApp.Dominio
{
    class Producto
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public double Precio { get; set; }
        public bool Activo { get; set; }
        public Producto(int idProducto, string nombre, double precio)
        {
            IdProducto = idProducto;
            Nombre = nombre;
            Precio = precio;
            Activo = true;
        }
        public override string ToString()
        {
            return Nombre;
        }
    }
}
