using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugTracker.Entities;
using BugTracker.DataAccessLayer;
using System.Data;

namespace BugTracker.DataAccessLayer
{
    class ClientesDao
    {
        public IList<Clientes> GetByFilters(Dictionary<string, object> parametros)
        {
            List<Clientes> listadoClientes = new List<Clientes>();

            var strSql = "SELECT id_cliente, razon_social, cuit, calle, numero, b.id_barrio, b.nombre, fecha_alta  " +
                         "FROM Clientes c " +
                         "INNER JOIN Barrios b ON c.id_barrio = b.id_barrio " +
                         "WHERE c.borrado = 0";
                          

            if (parametros.ContainsKey("RazonSocial"))
            {
                strSql += " AND (razon_social  LIKE '%' + @RazonSocial + '%') ";
            }

            if (parametros.ContainsKey("Cuit"))
            {
                strSql += " AND (cuit = @Cuit)";
            }

            var resultadoConsulta = DataManager.GetInstance().ConsultaSQL(strSql, parametros);
            foreach (DataRow row in resultadoConsulta.Rows)
            {
                listadoClientes.Add(ObjectMapping(row));
            }

            return listadoClientes;

        }

        public Clientes GetClientes(string razonSocial)
        {
            var strSql = string.Concat("SELECT id_cliente, " +
                                       "       razon_social, " +
                                       "       cuit, " +
                                       "       calle " +
                                       "       numero, " +
                                       "       fecha_alta, " +
                                       "       b.id_barrio " +
                                       "FROM   Clientes c " +
                                       "INNER JOIN Barrios b on c.id_barrio = b.id_barrio " +
                                       "WHERE razon_social = @razonSocial");
            var parametros = new Dictionary<string, object>();
            parametros.Add("razonSocial", razonSocial);

            var resultadoConsulta = DataManager.GetInstance().ConsultaSQL(strSql, parametros);

            if (resultadoConsulta.Rows.Count > 0)
            {
                return ObjectMapping(resultadoConsulta.Rows[0]);
            }
            return null;
        }
        internal bool Create(Clientes oClientes)
        {
            var strSql = "INSERT INTO Clientes (cuit, razon_social, borrado, calle, numero, fecha_alta, id_barrio)" +
                         "VALUES               (@Cuit, @RazonSocial, 0, @Calle, @NumeroCalle, @FechaAlta, @Idbarrio)";

            var parametros = new Dictionary<string, object>();
            parametros.Add("Cuit", oClientes.Cuit);
            parametros.Add("RazonSocial", oClientes.RazonSocial);
            parametros.Add("Calle", oClientes.Calle);
            parametros.Add("NumeroCalle", oClientes.NumeroCalle);
            parametros.Add("FechaAlta", oClientes.FechaAlta);
            parametros.Add("IDbarrio", oClientes.Barrio.IDBarrio);

            return (DataManager.GetInstance().EjecutarSQL(strSql, parametros) == 1);
        }

        internal bool Update(Clientes oCliente)
        {
            string strSql = "UPDATE Clientes " +
                            "SET razon_social = @RazonSocial, " +
                            "    cuit = @Cuit, " +
                            "    fecha_alta = @FechaAlta, " +
                            "    calle = @Calle, " +
                            "    numero = @NumeroCalle, " +
                            "    id_barrio = @IDbarrio " +
                            "WHERE id_cliente = @id_cliente";
            var parametros = new Dictionary<string, object>();
            parametros.Add("RazonSocial", oCliente.RazonSocial);
            parametros.Add("Cuit", oCliente.Cuit);
            parametros.Add("FechaAlta", oCliente.FechaAlta);
            parametros.Add("Calle", oCliente.Calle);
            parametros.Add("NumeroCalle", oCliente.NumeroCalle);
            parametros.Add("IDbarrio", oCliente.Barrio.IDBarrio);
            parametros.Add("id_cliente", oCliente.IDCLiente);

            return (DataManager.GetInstance().EjecutarSQL(strSql, parametros) == 1);
        }

        internal bool Delete(Clientes oCliente)
        {
            string strSql = "UPDATE Clientes " +
                            "SET borrado = 1 " +
                            "WHERE id_cliente = @id_cliente";
            var parametros = new Dictionary<string, object>();
            parametros.Add("id_cliente", oCliente.IDCLiente);
            return (DataManager.GetInstance().EjecutarSQL(strSql, parametros) == 1);
        }
        private Clientes ObjectMapping(DataRow row)
        {
            Clientes oClientes = new Clientes();
            oClientes.IDCLiente = Convert.ToInt32(row["id_cliente"].ToString());
            oClientes.RazonSocial = row["razon_social"].ToString();
            oClientes.Cuit = Convert.ToInt32(row["cuit"].ToString());
            oClientes.Calle = row["calle"].ToString();
            oClientes.NumeroCalle = Convert.ToInt32(row["numero"].ToString());
            oClientes.Barrio = new Barrio();
            oClientes.Barrio.IDBarrio = Convert.ToInt32(row["id_barrio"].ToString());
            oClientes.Barrio.Nombre = row["nombre"].ToString();
            oClientes.FechaAlta = Convert.ToDateTime(row["fecha_alta"].ToString());
            
            

            return oClientes;
        }

        
    }
}
