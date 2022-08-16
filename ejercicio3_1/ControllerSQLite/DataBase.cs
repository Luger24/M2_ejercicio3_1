using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

using ejercicio3_1.Models;
using System.Threading.Tasks;

namespace ejercicio3_1.ControllerSQLite
{
    public class DataBase
    {

        readonly SQLiteAsyncConnection dbase;

        public DataBase(string dbpath)
        {
            dbase = new SQLiteAsyncConnection(dbpath);

            dbase.CreateTableAsync<Empleado>();
        }



        #region Empleado
        
        public Task<int> EmpleadoSave(Empleado empleado)
        {
            if (empleado.Id != 0)
            {
                return dbase.UpdateAsync(empleado); // Update
            }
            else
            {
                return dbase.InsertAsync(empleado);
            }
        }



        public Task<List<Empleado>> obtenerListaEmpleado()
        {
            return dbase.Table<Empleado>().ToListAsync();
        }



        public Task<Empleado> obtenerEmpleado(int eid)
        {
            return dbase.Table<Empleado>()
                    .Where(i => i.Id == eid)
                    .FirstOrDefaultAsync();
        }



        public Task<int> EmpleadoDelete(Empleado empleado)
        {
            return dbase.DeleteAsync(empleado);
        }



        public Task<int> EmpleadoDeleteAll()
        {
            return dbase.DropTableAsync<Empleado>();
        }

        #endregion


        

    }
}
