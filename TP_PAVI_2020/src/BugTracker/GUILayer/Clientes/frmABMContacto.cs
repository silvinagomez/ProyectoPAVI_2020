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
    public partial class frmABMContacto : Form
    {
        private FormMode formMode = FormMode.insert;
        private Entities.Clientes oClienteSelected;
        private ClienteService oClienteService;


        public frmABMContacto()
        {
            InitializeComponent();
            oClienteService = new ClienteService();
        }

        public enum FormMode
        {
            insert,
            update,
            delete = 99
        }

        public void InicializarFormulario(FormMode op, Entities.Clientes clienteSelected)
        {
            formMode = op;
            oClienteSelected = clienteSelected;
        }
        private void MostrarDatos()
        {
            if (oClienteSelected != null)
            {
                if (oClienteSelected.Contacto != null)
                {
                    txtNombre.Text = oClienteSelected.Contacto.Nombre;
                    txtApellido.Text = oClienteSelected.Contacto.Apellido;
                    txtEmail.Text = oClienteSelected.Contacto.Email;
                    txtTelefono.Text = oClienteSelected.Contacto.Telefono;
                }
                else
                {
                    MessageBox.Show("El cliente seleccionado no tiene un contacto, Por favor cree uno", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                    
                
            }
        }

        private void frmABMContacto_Load(object sender, EventArgs e)
        {
            switch (formMode)
            {
                case FormMode.insert:
                    {
                        this.Text = "Nuevo Contacto";
                        break;
                    }
                case FormMode.update:
                    {
                        this.Text = "Actualizar Contacto";
                        MostrarDatos();
                        txtNombre.Enabled = true;
                        txtApellido.Enabled = true;
                        txtEmail.Enabled = true;
                        txtTelefono.Enabled = true;
                        break;
                    }
                case FormMode.delete:
                    {
                        this.Text = "Eliminar Contacto";
                        MostrarDatos();
                        txtNombre.Enabled = false;
                        txtApellido.Enabled = false;
                        txtTelefono.Enabled = false;
                        txtEmail.Enabled = false;
                        break;
                    }
            }
        }
        private bool ExisteContacto()
        {
            return oClienteService.ObtenerContacto(oClienteSelected) != null;
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
                    if (ExisteContacto() == false)
                    {
                        if (ValidarCampos())
                        {
                            Contacto oContacto = new Contacto();

                            oContacto.Nombre = txtNombre.Text;
                            oContacto.Apellido = txtApellido.Text;
                            oContacto.Email = txtEmail.Text;
                            oContacto.Telefono = txtTelefono.Text;
                            oContacto.Borrado = 0;

                            oClienteSelected.Contacto = oContacto;

                            if (oClienteService.CrearContacto(oClienteSelected))
                            {
                                MessageBox.Show("Contacto Insertado", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.Close();
                            }
                            
                        }
                    }
                    else
                        MessageBox.Show("Contacto encontrado! Ingrese un contacto distinto", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                
                case FormMode.delete:
                    {
                        if(MessageBox.Show("¿Seguro que desea eliminar el contacto seleccionado?", "Aviso", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                        {
                            if (oClienteService.EliminarContacto(oClienteSelected))
                            {
                                MessageBox.Show("Contacto Eliminado", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.Close();
                            }
                            else
                                MessageBox.Show("Error al eliminar el contacto seleccionado!", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }
                        break;
                    }
                
                case FormMode.update:
                    {
                        if(ValidarCampos())
                        {
                            oClienteSelected.Contacto.Nombre = txtNombre.Text;
                            oClienteSelected.Contacto.Apellido = txtApellido.Text;
                            oClienteSelected.Contacto.Email = txtEmail.Text;
                            oClienteSelected.Contacto.Telefono = txtTelefono.Text;

                            if (MessageBox.Show("¿Seguro que desea actualizar el contacto seleccionado?", "Aviso", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                            {

                                if (oClienteService.ActualizarContacto(oClienteSelected))
                                {
                                    MessageBox.Show("Contacto actualizado!", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    this.Dispose();
                                }
                                else
                                    MessageBox.Show("Error al actualizar el contacto!", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        break;
                    }
            }

        }

        private bool ValidarCampos()
        {
            if (txtNombre.Text == string.Empty)
            {
                txtNombre.BackColor = Color.Red;
                txtNombre.Focus();
                return false;
            }
            else txtNombre.BackColor = Color.White;
            
            if (txtApellido.Text == string.Empty)
            {
                txtApellido.BackColor = Color.Red;
                txtApellido.Focus();
                return false;
            }
            else txtApellido.BackColor = Color.White;
            
            if (txtEmail.Text == string.Empty)
            {
                txtEmail.BackColor = Color.Red;
                txtEmail.Focus();
                return false;
            }
            else txtEmail.BackColor = Color.White;
            
            if (txtTelefono.Text == string.Empty)
            {
                txtTelefono.BackColor = Color.Red;
                txtTelefono.Focus();
                return false;
            }
            else txtTelefono.BackColor = Color.White;

            return true;
        }
    }

}
