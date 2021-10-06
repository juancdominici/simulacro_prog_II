using ParcialApp.Dominio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcialApp.Acceso_a_datos
{
    interface IFacturaDao
    {
        int ProximoNro();
        bool Crear(Factura factura);
        DataTable ListarProductos();
    }
}
