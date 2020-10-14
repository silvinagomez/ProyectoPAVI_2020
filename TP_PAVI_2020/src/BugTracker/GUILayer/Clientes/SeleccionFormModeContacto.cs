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
    public partial class SeleccionFormModeContacto : Form
    {
        private BugTracker.Entities.Clientes oClienteSelected;
        public SeleccionFormModeContacto()
        {
            InitializeComponent();
        }
        public void InicializarFormulario(BugTracker.Entities.Clientes clienteSelected)
        {
            oClienteSelected = clienteSelected;
        }
        private void btnCrear_Click(object sender, EventArgs e)
        {
            frmABMContacto formulario = new frmABMContacto();
            formulario.InicializarFormulario(frmABMContacto.FormMode.insert, oClienteSelected);
            formulario.ShowDialog();
        }

        private void lblConsult_update_Click(object sender, EventArgs e)
        {
            frmABMContacto formulario = new frmABMContacto();
            formulario.InicializarFormulario(frmABMContacto.FormMode.update, oClienteSelected);
            formulario.ShowDialog();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            frmABMContacto formulario = new frmABMContacto();
            formulario.InicializarFormulario(frmABMContacto.FormMode.delete, oClienteSelected);
            formulario.ShowDialog();
        }
    }
}
