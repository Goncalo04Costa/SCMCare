using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class FornecedorMedicamento
    {
        public int MedicamentosId { get; set; }
        public int FornecedoresId { get; set; }
    }
}