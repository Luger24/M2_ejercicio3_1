using System;
using System.Collections.Generic;
using System.Text;

using SQLite;

namespace ejercicio3_1.Models
{
    public class Empleado
    {
        [PrimaryKey ,AutoIncrement]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public int Edad { get; set; }
        public string Direccion { get; set; }
        public string Puesto { get; set; }
        public byte[] Foto { get; set; }
    }
}
