﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Modelos
{
    public class UserFuncionario
    {
        [Required]
        public int FuncionariosId { get; set; }

        [Required]
        public string User { get; set; }

        [Required]
        public string Passe { get; set; }
    }
}