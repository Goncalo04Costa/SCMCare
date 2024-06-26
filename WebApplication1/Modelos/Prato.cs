﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Modelos
{
    public class Prato
    {
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        public string Descricao { get; set; }

        [Required]
        public bool Tipo { get; set; }

        public bool Ativo { get; set; }
    }

}