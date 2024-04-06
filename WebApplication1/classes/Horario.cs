using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Horario
    {
        public int FuncionariosId { get; set; }
        public int TurnosId { get; set; }
        public DateTime Dia { get; set; }
    }
}