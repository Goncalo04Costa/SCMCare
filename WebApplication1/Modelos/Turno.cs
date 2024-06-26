﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Modelos
{
    public class Turno
    {
        public int Id { get; set; }

        [Required]
        public TimeOnly HoraInicio { get; set; }

        [Required]
        public TimeOnly HoraFim { get; set; }

        [Required]
        public bool Ativo { get; set; }
    }
}