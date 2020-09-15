using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using BugTracker.Entities;
using System.Data;


namespace BugTracker.DataAccessLayer
{
    public class BarrioDao
    {
        public IList<Barrio> GetAll()
        {
            List<Barrio> listadoBarrio = new List<Barrio>();
            var strSql = "Select id_barrio, nombre from Barrios";
            var resultadoConsulta = DataManager.GetInstance().ConsultaSQL(strSql);

            foreach (DataRow row in resultadoConsulta.Rows)
            {
                listadoBarrio.Add(ObjectMapping(row));
            }

            return listadoBarrio;
        }

        private Barrio ObjectMapping(DataRow row)
        {
            Barrio oBarrio = new Barrio();

            oBarrio.IDBarrio = Convert.ToInt32(row["id_barrio"].ToString());
            oBarrio.Nombre = row["nombre"].ToString();
            return oBarrio;
        }
    }
}
