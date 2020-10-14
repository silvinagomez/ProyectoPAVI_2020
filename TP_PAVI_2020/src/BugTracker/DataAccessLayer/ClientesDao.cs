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

            var strSql = "SELECT razon_social, cuit, calle, numero, b.id_barrio, b.nombre, fecha_alta  " +
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
        public Contacto GetContacto(Clientes oCliente)
        {
            var strSql = string.Concat("SELECT Co.id_contacto, " +
                                               "nombre, " +
                                               "apellido, " +
                                               "email, " +
                                               "telefono " +
                                       "FROM    Contactos Co INNER JOIN Clientes C " +
                                       "ON      C.id_contacto = Co.id_contacto " +
                                       "WHERE   C.id_cliente = @IDCliente " +
                                       "AND     Co.borrado <> 1");
            var parametros = new Dictionary<string, object>();
            parametros.Add("IDCliente", oCliente.IDCLiente);

            var resultadoConsulta = DataManager.GetInstance().ConsultaSQL(strSql, parametros);

            if(resultadoConsulta.Rows.Count > 0)
            {
                return ObjectContactoMapping(resultadoConsulta.Rows[0]);
            }
            return null;
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

        internal bool CreateContact(Clientes oCliente)
        {
            var resultado = false;
            DataManager dm = new DataManager();
            try
            {
                dm.Open();
                dm.BeginTransaction();

                string strSql = "INSERT INTO Contactos (nombre, apellido, email, telefono, borrado) " +
                                "VALUES                (@Nombre, @Apellido, @Email, @Telefono, @Borrado) ";
                var parametros = new Dictionary<string, object>();
                parametros.Add("Nombre", oCliente.Contacto.Nombre);
                parametros.Add("Apellido", oCliente.Contacto.Apellido);
                parametros.Add("Email", oCliente.Contacto.Email);
                parametros.Add("Telefono", oCliente.Contacto.Telefono);
                parametros.Add("Borrado", oCliente.Contacto.Borrado);

                dm.EjecutarSQL(strSql, parametros);

                var nuevoId = dm.ConsultaSQLScalar(" SELECT @@IDENTITY ");
                oCliente.Contacto.IdContacto = Convert.ToInt32(nuevoId);

                string strSqlC = "UPDATE CLientes (id_contacto) " +
                                 "VALUES          (@id_contacto) ";

                var parametrosC = new Dictionary<string, Object>();
                parametrosC.Add("id_contacto", oCliente.Contacto.IdContacto);

                resultado = (dm.EjecutarSQL(strSqlC, parametrosC) == 1);

                dm.Commit();
            }

            catch (Exception ex)
            {
                dm.Rollback();
                throw ex;
            }

            finally
            {
                dm.Close();
            }

            return resultado;            
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




        internal bool UpdateContact( Clientes oCliente)
        {
            string strSql = "Update Contactos " +
                            "SET nombre = @Nombre," +
                            "    apellido = @Apellido, " +
                            "    email = @Email, " +
                            "    telefono = @Telefono " +
                            "WHERE id_contacto=@IDContacto";
            var parametros = new Dictionary<string, object>();
            parametros.Add("Nombre", oCliente.Contacto.Nombre);
            parametros.Add("Apellido", oCliente.Contacto.Apellido);
            parametros.Add("Email", oCliente.Contacto.Email);
            parametros.Add("Telefono", oCliente.Contacto.Telefono);
            parametros.Add("IDContacto", oCliente.Contacto.IdContacto);

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
        
        
        
        internal bool DeleteContact(Clientes oCliente)
        {
            var resultado = false;
            DataManager dm = new DataManager();
            try
            {
                dm.Open();
                dm.BeginTransaction();

                string strSql = "UPDATE Contactos " +
                                "SET borrado = 1 " +
                                "WHERE id_contacto = (SELECT id_contacto " +
                                "                     FROM Clientes " +
                                "                     WHERE id_cliente=@id_cliente";
                var parametros = new Dictionary<string, object>();
                parametros.Add("id_cliente", oCliente.IDCLiente);
                dm.EjecutarSQL(strSql, parametros);

                string strSqlC = "UPDATE Clientes " +
                                 "SET id_contacto = NULL" +
                                 "WHERE id_cliente = @id_cliente ";
                var parametrosC = new Dictionary<string, object>();
                parametrosC.Add("id_cliente", oCliente.IDCLiente);
                
                resultado = (dm.EjecutarSQL(strSqlC, parametrosC) == 1);

                dm.Commit();


            }
            catch (Exception ex)
            {
                dm.Rollback();
                throw ex;
            }

            finally
            {
                dm.Close();
            }

            return resultado;
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

        private Contacto ObjectContactoMapping(DataRow row)
        {
            Contacto oContacto = new Contacto();
            oContacto.IdContacto = Convert.ToInt32(row["id_contacto"].ToString());
            oContacto.Nombre = row["nombre"].ToString();
            oContacto.Apellido = row["apellido"].ToString();
            oContacto.Email = row["email"].ToString();
            oContacto.Telefono = row["telefono"].ToString();

            return oContacto;
        }
        
    }
}
