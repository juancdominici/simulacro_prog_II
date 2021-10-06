using ParcialApp.Acceso_a_datos;
using ParcialApp.Dominio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcialApp.Servicios
{
    class GestorFactura
    {
        private IFacturaDao _facturaDao;

        public GestorFactura(AbstractDaoFactory factory)
        {
            _facturaDao = factory.CrearFacturaDao();
        }

        public int ProximoNro()
        {
            return _facturaDao.ProximoNro();
        }

        public DataTable ListarProductos()
        {
            return _facturaDao.ListarProductos();
        }

        public bool Crear(Factura factura)
        {
            return _facturaDao.Crear(factura);
        }
    }
}
