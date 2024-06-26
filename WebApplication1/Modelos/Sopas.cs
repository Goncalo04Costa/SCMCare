﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace Modelos
{
    public class Sopa
    {
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        public string Descricao { get; set; }

        public bool Tipo { get; set; }

        public bool Ativo { get; set; }
    }
}