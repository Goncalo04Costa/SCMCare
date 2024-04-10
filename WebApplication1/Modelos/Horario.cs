using System;
using System.ComponentModel.DataAnnotations;

namespace Modelos
{
    public class Horario
    {
        public int TiposFuncionarioId { get; set; }
        public int FuncionariosId { get; set; }
        public int TurnosId { get; set; }
        public DateTime Dia { get; set; }
    }
}