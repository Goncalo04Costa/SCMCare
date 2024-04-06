﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Funcionario
    {
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public int TiposFuncionarioId { get; set; }

        [Required]
        public bool Historico { get; set; }
    }
}