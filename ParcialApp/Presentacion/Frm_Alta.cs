
using ParcialApp.Acceso_a_datos;
using ParcialApp.Dominio;
using ParcialApp.Servicios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParcialApp.Presentacion
{
    public partial class Frm_Alta : Form
    {
        private Factura factura;
        private GestorFactura _gestorFactura;
        public Frm_Alta()
        {
            InitializeComponent();
            _gestorFactura = new GestorFactura(new DaoFactory());
        }
        private void Frm_Alta_Presupuesto_Load(object sender, EventArgs e)
        {
            lblNro.Text += _gestorFactura.ProximoNro();
            dtpFecha.Text = DateTime.Today.ToString("dd/MM/yyyy");
            txtCliente.Text = "CONSUMIDOR FINAL";
            nudCantidad.Text = "1";
            cboProducto.DataSource = _gestorFactura.ListarProductos();
            cboProducto.DisplayMember = "n_producto";
            cboProducto.ValueMember = "id_producto";
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (cboProducto.SelectedIndex == -1)
            {
                MessageBox.Show("Debe seleccionar un producto...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else if (cboForma.SelectedIndex == -1)
            {
                MessageBox.Show("Debe seleccionar una forma de pago...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else if (!int.TryParse(nudCantidad.Text, out _))
            {
                MessageBox.Show("Debe ingresar una cantidad valida...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else if (ExisteProductoEnGrilla(dgvDetalles.Text))
            {
                        MessageBox.Show("El producto ya fue ingresado...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
            }
            else { 

                DataRowView fila = (DataRowView)cboProducto.SelectedItem;
                int producto = (int)fila.Row.ItemArray[0];
                string nom = fila.Row.ItemArray[1].ToString();
                double precio = Convert.ToDouble(fila.Row.ItemArray[2]);
                Producto p = new Producto(producto, nom, precio);

                int cant = Convert.ToInt32(nudCantidad.Text);
                DetalleFactura d = new DetalleFactura(p, cant);
                factura = new Factura();
                factura.AgregarDetalle(d);
                dgvDetalles.Rows.Add(new object[] { producto, nom, precio, cant });

                CalcularTotales();
            }
        }
        private void CalcularTotales()
        {
            lblTotal.Text = "Total:" + factura.CalcularTotal().ToString();
        }

        private bool ExisteProductoEnGrilla(string text)
        {
            foreach (DataGridViewRow fila in dgvDetalles.Rows)
            {
                if (fila.Cells["producto"].Value.Equals(text))
                    return true;
            }
            return false;
        }

        private void dgvDetalles_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvDetalles.CurrentCell.ColumnIndex == 5)
            {
                factura.QuitarDetalle(dgvDetalles.CurrentRow.Index);
                dgvDetalles.Rows.Remove(dgvDetalles.CurrentRow);
                CalcularTotales();
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCliente.Text))
            {
                MessageBox.Show("Debe ingresar un cliente valido...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtCliente.Focus();
                return;
            }
            if (dgvDetalles.Rows.Count == 0)
            {
                MessageBox.Show("Debe insertar al menos un detalle...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            GuardarFactura();
        }

        private void GuardarFactura()
        {
            factura.Fecha = Convert.ToDateTime(dtpFecha.Text);
            factura.Cliente = txtCliente.Text;
            factura.Total = factura.CalcularTotal();
            if (_gestorFactura.Crear(factura))
            {
                MessageBox.Show("La factura se grabó correctamente!", "Notificación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Dispose();
            }
            else
            {
                MessageBox.Show("La factura no se grabó.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            };
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("¿Está seguro que desea cancelar?", "Salir", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Dispose();
            }
            else
            {
                return;
            }
        }
    }
}
