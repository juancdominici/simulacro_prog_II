using System;
using System.Collections.Generic;

namespace ParcialApp.Dominio
{
     class Factura
    {
        public int Numero { get; set; }
        public DateTime Fecha { get; set; }
        public string Cliente { get; set; }
        public int FormaPago { get; set; }
        public double Total { get; set; }
        public DateTime FechaBaja { get; set; }
        public List<DetalleFactura> Detalles { get; set; }
        public Factura()
        {
            Detalles = new List<DetalleFactura>();
        }
        public void AgregarDetalle(DetalleFactura detalle)
        {
            Detalles.Add(detalle);
        }

        public void QuitarDetalle(int i)
        {
            Detalles.RemoveAt(i);
        }

        public double CalcularTotal()
        {
            double total = 0;
            foreach (DetalleFactura detalle in Detalles)
            {
                total += detalle.CalcularSubtotal();
            }
            return total;
        }
    }
}