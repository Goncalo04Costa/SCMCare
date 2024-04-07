using System;
using System.ComponentModel.DataAnnotations;

namespace Modelos
{
    public class Horario
    {
        public int FuncionariosId { get; set; }
        public int TurnosId { get; set; }
        public DateTime Dia { get; set; }
    }
}