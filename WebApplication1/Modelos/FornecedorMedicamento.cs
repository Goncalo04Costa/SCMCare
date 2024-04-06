using System;
using System.ComponentModel.DataAnnotations;

namespace Modelos
{
    public class FornecedorMedicamento
    {
        public int MedicamentosId { get; set; }
        public int FornecedoresId { get; set; }
    }
}