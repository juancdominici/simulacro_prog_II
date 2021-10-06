using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcialApp.Acceso_a_datos
{
    class DaoFactory : AbstractDaoFactory
    {
        public override IFacturaDao CrearFacturaDao()
        {
            return new FacturaDao();
        }
    }
}
