using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BugTracker.BusinessLayer;
using BugTracker.Entities;

namespace BugTracker.GUILayer.Clientes
{
    public partial class frmABMCliente : Form
    {
        private FormMode formMode = FormMode.insert;
        private ClienteService oClienteService;
        private BarrioService oBarrioService;
        private BugTracker.Entities.Clientes oClienteSelected; 

        public frmABMCliente()
        {
            InitializeComponent();
            oClienteService = new ClienteService();
            oBarrioService = new BarrioService();
        }

        public enum FormMode
        {
            insert,
            update,
            delete = 99
        }

        public void InicializarFormulario(FormMode op, BugTracker.Entities.Clientes clienteSelected)
        {
            formMode = op;
            oClienteSelected = clienteSelected;
        }
        private void MostrarDatos()
        {
            if (oClienteSelected != null)
            {
                txtRazonSocial.Text = oClienteSelected.RazonSocial;
                txtCuit.Text = oClienteSelected.Cuit.ToString();
                txtCalle.Text = oClienteSelected.Calle;
                txtFechaAlta.Text = oClienteSelected.FechaAlta.ToString();
                txtNumeroCalle.Text = oClienteSelected.NumeroCalle.ToString();
                cboBarrio.Text = oClienteSelected.Barrio.Nombre;
            }
        }

        private void LlenarCombo(ComboBox cbo, Object source, string display, String value)
        {
            cbo.DataSource = source;
            cbo.DisplayMember = display;
            cbo.ValueMember = value;
            cbo.SelectedIndex = -1;
        }
         
        private void frmABMCliente_Load_1(object sender, EventArgs e)
        {
            LlenarCombo(cboBarrio, oBarrioService.ObtenerTodos(), "Nombre", "IDBarrio");
            switch (formMode)
            {
                case FormMode.insert:
                    {
                        this.Text = "Nuevo Cliente";
                        break;
                    }
                case FormMode.update:
                    {
                        this.Text = "Actualizar Cliente";
                        MostrarDatos();
                        txtRazonSocial.Enabled = true;
                        txtNumeroCalle.Enabled = true;
                        txtFechaAlta.Enabled = true;
                        txtCuit.Enabled = true;
                        txtCalle.Enabled = true;
                        cboBarrio.Enabled = true;
                        break;

                    }
                case FormMode.delete:
                    {
                        MostrarDatos();
                        this.Text = "Eliminar Usuario";
                        txtRazonSocial.Enabled = false;
                        txtNumeroCalle.Enabled = false;
                        txtFechaAlta.Enabled = false;
                        txtCuit.Enabled = false;
                        txtCalle.Enabled = false;
                        cboBarrio.Enabled = false;
                        break;
                    }

            }

        }

        

        private bool ExisteCliente()
        {
            return oClienteService.ObtenerCliente(txtRazonSocial.Text) != null;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            switch (formMode)
            {
                case FormMode.insert:
                    {
                        if (ExisteCliente() == false)
                        {
                            if (ValidarCampos())
                            {
                                var oClientes = new BugTracker.Entities.Clientes();
                                oClientes.RazonSocial = txtRazonSocial.Text;
                                oClientes.Cuit = (int)Convert.ToUInt32(txtCuit.Text);
                                oClientes.Calle = txtCalle.Text;
                                oClientes.Barrio = new Barrio();
                                oClientes.Barrio.IDBarrio = (int)cboBarrio.SelectedValue;
                                oClientes.FechaAlta = Convert.ToDateTime(txtFechaAlta.Text);
                                oClientes.NumeroCalle = Convert.ToInt32(txtNumeroCalle.Text);
                                oClientes.Borrado = 0;

                                if (oClienteService.CrearCliente(oClientes))
                                {
                                    MessageBox.Show("Cliente Insertado", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    this.Close();
                                }


                            }
                        }
                        else
                            MessageBox.Show("Cliente encontrado! Ingrese un cliente distinto", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    }
                case FormMode.update:
                    {
                        if (ValidarCampos())
                        {
                            oClienteSelected.RazonSocial = txtRazonSocial.Text;
                            oClienteSelected.Cuit = Convert.ToInt32(txtCuit.Text);
                            oClienteSelected.FechaAlta = Convert.ToDateTime(txtFechaAlta.Text);
                            oClienteSelected.Calle = txtCalle.Text;
                            oClienteSelected.NumeroCalle = Convert.ToInt32(txtNumeroCalle.Text);
                            oClienteSelected.Barrio = new Barrio();
                            oClienteSelected.Barrio.IDBarrio = (int)cboBarrio.SelectedValue;

                            if (oClienteService.ActualizarCliente(oClienteSelected))
                            {
                                MessageBox.Show("Cliente actualizado!", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.Dispose();
                            }
                            else MessageBox.Show("Error al actualizar el usuario!", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        }
                        break;
                    }
                case FormMode.delete:
                    {
                        if (MessageBox.Show("¿Seguro que desea eliminar el cliente seleccionado?", "Aviso", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                        {
                            if (oClienteService.EliminarCliente(oClienteSelected))
                            {
                                MessageBox.Show("Cliente eliminado", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.Close();
                            }
                            else
                                MessageBox.Show("Error al eliminar el cliente!", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        break;
                    }
            }                            
        }

        private bool ValidarCampos()
        {
            if (txtRazonSocial.Text == string.Empty)
            {
                txtRazonSocial.BackColor = Color.Red;
                txtRazonSocial.Focus();
                return false;
            }
            else txtRazonSocial.BackColor = Color.White;

            if (txtCuit.Text == string.Empty)
            {
                txtCuit.BackColor = Color.Red;
                txtCuit.Focus();
                return false;
            }
            else txtCuit.BackColor = Color.White;

            if (txtFechaAlta.Text == string.Empty)
            {
                txtFechaAlta.BackColor = Color.Red;
                txtFechaAlta.Focus();
                return false;
            }
            else txtFechaAlta.BackColor = Color.White;

            if (txtCalle.Text == string.Empty)
            {
                txtCalle.BackColor = Color.Red;
                txtCalle.Focus();
                return false;
            }
            else txtCalle.BackColor = Color.White;

            if (txtNumeroCalle.Text == string.Empty)
            {
                txtNumeroCalle.BackColor = Color.Red;
                txtNumeroCalle.Focus();
                return false;
            }
            else txtNumeroCalle.BackColor = Color.White;

            if (cboBarrio.Text == string.Empty)
            {
                cboBarrio.BackColor = Color.Red;
                cboBarrio.Focus();
                return false;
            }
            else cboBarrio.BackColor = Color.White;

            return true;
        }
    }
}
