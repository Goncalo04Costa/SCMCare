﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Avaliacao
    {
        public int UtentesId { get; set; }
        public int FuncionariosId { get; set; }
        public DateTime Data { get; set; }

        public string Analise { get; set; }

        [Required]
        public DateTime Data { get; set; }

        [Required]
        public int TipoAvaliacaoId { get; set; }

        [Required]
        public string AuscultacaoPulmonar { get; set; }

        [Required]
        public string AuscultacaoCardiaca { get; set; }
    }
}