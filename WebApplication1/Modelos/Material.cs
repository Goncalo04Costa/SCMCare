﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Modelos
{
    public class Material
    {
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        public int? Limite { get; set; }

        [Required]
        public int TiposMaterialId { get; set; }

        public bool Ativo { get; set; }
    }

}