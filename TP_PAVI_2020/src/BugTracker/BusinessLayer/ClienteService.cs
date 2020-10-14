using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugTracker.Entities;
using BugTracker.DataAccessLayer;

namespace BugTracker.BusinessLayer
{
    class ClienteService
    {
        private ClientesDao oClienteDao;
        public ClienteService()
        {
            oClienteDao = new ClientesDao();
        }


        internal IList<Clientes> ConsultarConFiltro(Dictionary<string, object> filtros)
        {
            return oClienteDao.GetByFilters(filtros);
        }

        internal Object ObtenerCliente(string cliente)
        {
            return oClienteDao.GetClientes(cliente);
        }

        internal bool CrearCliente(Clientes oCliente)
        {
            return oClienteDao.Create(oCliente);
        }

        internal bool ActualizarCliente(Clientes oClienteSelected)
        {
            return oClienteDao.Update(oClienteSelected);
        }

        internal bool EliminarCliente(Clientes oClienteSelected)
        {
            return oClienteDao.Delete(oClienteSelected);
        }

        
        internal Object ObtenerContacto(Clientes oClienteSelected)
        {
            return oClienteDao.GetContacto(oClienteSelected);
        }
        internal bool CrearContacto(Clientes oClienteSelected)
        {
            return oClienteDao.CreateContact(oClienteSelected);
        }
        internal bool ActualizarContacto(Clientes oClienteSelected)
        {
            return oClienteDao.UpdateContact(oClienteSelected);
        }
        internal bool EliminarContacto(Clientes oClienteSelected)
        {
            return oClienteDao.DeleteContact(oClienteSelected);
        }
    }
}
