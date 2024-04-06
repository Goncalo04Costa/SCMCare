using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class UtenteAlergia
    {
        public int UtentesId { get; set; }
        public int TiposAlergiaId { get; set; }
    }

}