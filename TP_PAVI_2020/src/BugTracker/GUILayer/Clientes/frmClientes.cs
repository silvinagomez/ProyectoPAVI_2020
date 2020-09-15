using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BugTracker.BusinessLayer;
using BugTracker.Entities;

namespace BugTracker.GUILayer.Clientes
{
    public partial class frmClientes : Form
    {
        private ClienteService oClienteService;
        public frmClientes()
        {
            InitializeComponent();
            InitializeDataGridView();
            oClienteService = new ClienteService();

        }
        private void InitializeDataGridView()
        {
            // Cree un DataGridView no vinculado declarando un recuento de columnas.
            dgvClientes.ColumnCount = 6;
            dgvClientes.ColumnHeadersVisible = true;

            // Configuramos la AutoGenerateColumns en false para que no se autogeneren las columnas
            dgvClientes.AutoGenerateColumns = false;

            // Cambia el estilo de la cabecera de la grilla.
            DataGridViewCellStyle columnHeaderStyle = new DataGridViewCellStyle();

            columnHeaderStyle.BackColor = Color.Beige;
            columnHeaderStyle.Font = new Font("Verdana", 8, FontStyle.Bold);
            dgvClientes.ColumnHeadersDefaultCellStyle = columnHeaderStyle;

            // Definimos el nombre de la columnas y el DataPropertyName que se asocia a DataSource
            // Definimos el ancho de la columna.

            dgvClientes.Columns[0].Name = "Razon Social";
            dgvClientes.Columns[0].DataPropertyName = "RazonSocial";

            dgvClientes.Columns[1].Name = "CUIT";
            dgvClientes.Columns[1].DataPropertyName = "Cuit";

            dgvClientes.Columns[2].Name = "Fecha Alta";
            dgvClientes.Columns[2].DataPropertyName = "FechaAlta";

            dgvClientes.Columns[3].Name = "Calle";
            dgvClientes.Columns[3].DataPropertyName = "Calle";

            dgvClientes.Columns[4].Name = "Altura";
            dgvClientes.Columns[4].DataPropertyName = "NumeroCalle";

            dgvClientes.Columns[5].Name = "Barrio";
            dgvClientes.Columns[5].DataPropertyName = "Barrio";

            

            // Cambia el tamaño de la altura de los encabezados de columna.
            dgvClientes.AutoResizeColumnHeadersHeight();

            // Cambia el tamaño de todas las alturas de fila para ajustar el contenido de todas las celdas que no sean de encabezado.
            dgvClientes.AutoResizeRows(
                DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders);
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            frmABMCliente formulario = new frmABMCliente();
            formulario.ShowDialog();
            btnConsultar_Click(sender, e);
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            var filters = new Dictionary<string, object>();
            
            if (txtRazonSocial.Text != string.Empty)
            {
                filters.Add("RazonSocial", txtRazonSocial.Text);
            }
            if (txtCuit.Text != string.Empty)
            {
                filters.Add("Cuit", txtCuit.Text);
            }

            if (filters.Count > 0)
            {
                dgvClientes.DataSource = oClienteService.ConsultarConFiltro(filters);
            }
            else
            {
                MessageBox.Show("Debe ingresar un criterio de búsqueda", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            frmABMCliente formulario = new frmABMCliente();
            var cliente = (BugTracker.Entities.Clientes)dgvClientes.CurrentRow.DataBoundItem;
            formulario.InicializarFormulario(frmABMCliente.FormMode.update, cliente);
            formulario.ShowDialog();
            btnConsultar_Click(sender, e);
        }

        private void dgvClientes_CellClick(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            btnEditar.Enabled = true;
            btnQuitar.Enabled = true;
        }

        private void btnQuitar_Click(object sender, EventArgs e)
        {
            frmABMCliente formulario = new frmABMCliente();
            var cliente = (BugTracker.Entities.Clientes)dgvClientes.CurrentRow.DataBoundItem;
            formulario.InicializarFormulario(frmABMCliente.FormMode.delete, cliente);
            formulario.ShowDialog();
            btnConsultar_Click(sender, e);
        }
    }
}
