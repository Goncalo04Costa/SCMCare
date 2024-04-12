using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;


namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContaCorrenteMedicamentoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ContaCorrenteMedicamentoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContaCorrenteMedicamento>>> obterTodasContasCorrentes(
            int? idMin = null, int? idMax = null,
            int? medicamentosId = null,
            int? pedidosMedicamentoId = null,
            int? utentesId = null,
            int? funcionariosId = null,
            DateTime? dataMin = null, DateTime? dataMax = null,
            bool tipo = false,
            int? quantidadeMovimentoMin = null, int? quantidadeMovimentoMax = null,
            string observacoesMin = null, string observacoesMax = null)
        {
            IQueryable<ContaCorrenteMedicamento> query = _context.ContaCorrenteMedicamento;

            if (idMin.HasValue)
            {
                query = query.Where(d => d.Id >= idMin.Value);
            }

            if (idMax.HasValue)
            {
                query = query.Where(d => d.Id <= idMax.Value);
            }

            if (medicamentosId.HasValue)
            {
                query = query.Where(d => d.MedicamentosId == medicamentosId.Value);
            }

            if (pedidosMedicamentoId.HasValue)
            {
                query = query.Where(d => d.PedidosMedicamentoId == pedidosMedicamentoId.Value);
            }

            if (funcionariosId.HasValue)
            {
                query = query.Where(d => d.FuncionariosId == funcionariosId.Value);
            }

            if (utentesId.HasValue)
            {
                query = query.Where(d => d.UtentesId == utentesId.Value);
            }

            if (dataMin.HasValue)
            {
                query = query.Where(d => d.Data <= dataMin.Value);
            }

            if (dataMax.HasValue)
            {
                query = query.Where(d => d.Data >= dataMax.Value);
            }

            query = query.Where(d => d.Tipo == tipo);

            if (quantidadeMovimentoMin.HasValue)
            {
                query = query.Where(d => d.QuantidadeMovimento >= quantidadeMovimentoMin);
            }

            if (quantidadeMovimentoMax.HasValue)
            {
                query = query.Where(d => d.QuantidadeMovimento <= quantidadeMovimentoMax.Value);
            }

            if (!string.IsNullOrEmpty(observacoesMin))
            {
                query = query.Where(d => d.Observacoes.CompareTo(observacoesMin) >= 0);
            }

            if (!string.IsNullOrEmpty(observacoesMax))
            {
                query = query.Where(d => d.Observacoes.CompareTo(observacoesMax + "ZZZ") <= 0);
            }

            var dados = await query.ToListAsync();
            return dados;
        }

        [HttpGet("{id")]
        public async Task<ActionResult<ContaCorrenteMedicamento>> obterContaCorrente(int id)
        {
            var dado = await _context.ContaCorrenteMedicamento.FirstOrDefaultAsync(dado => dado.Id == id);

            if (dado == null)
            {
                return NotFound();
            }
            return Ok(dado);
        }

        [HttpPost]
        public async Task<ActionResult<ContaCorrenteMedicamento>> InserirContaCorrente([FromBody] ContaCorrenteMedicamento contaCorrente)
        {
            if (contaCorrente == null)
            {
                return BadRequest("Objeto inválido");
            }

            _context.ContaCorrenteMedicamento.Add(contaCorrente);
            await _context.SaveChangesAsync();

            return Ok("Conta corrente adicionada com sucesso");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizaContaCorrente(int id, [FromBody] ContaCorrenteMedicamento novaContaCorrente)
        {
            var contaCorrente = await _context.ContaCorrenteMedicamento.FirstOrDefaultAsync(d => d.Id == id);
            if (contaCorrente == null)
            {
                return NotFound($"Não foi possível encontrar a conta corrente com o ID {id}");
            }

            contaCorrente.Fatura = novaContaCorrente.Fatura;
            contaCorrente.MedicamentosId = novaContaCorrente.MedicamentosId;
            contaCorrente.PedidosMedicamentoId = novaContaCorrente.PedidosMedicamentoId;
            contaCorrente.FuncionariosId = novaContaCorrente.FuncionariosId;
            contaCorrente.UtentesId = novaContaCorrente.UtentesId;
            contaCorrente.Data = novaContaCorrente.Data;
            contaCorrente.Tipo = novaContaCorrente.Tipo;
            contaCorrente.QuantidadeMovimento = novaContaCorrente.QuantidadeMovimento;
            contaCorrente.Observacoes = novaContaCorrente.Observacoes;

            try
            {
                await _context.SaveChangesAsync();

                return Ok($"Foi atualizada a conta com Id {id}");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveContaCorrente(int id)
        {
            var contaCorrente = await _context.ContaCorrenteMedicamento.FirstOrDefaultAsync(d => d.Id == id);
            if (contaCorrente == null)
            {
                return NotFound($"Não foi possível encontrar a conta corrente com o ID {id}");
            }

            _context.ContaCorrenteMedicamento.Remove(contaCorrente);
            await _context.SaveChangesAsync();

            return Ok($"Foi removida a conta corrente com o ID {id}");
        }


        [HttpPost("adicionar-medicamento")]
        public async Task<ActionResult> AdicionarMedicamento(int medicamentosId, int quantidadeAdicionada, string observacoes)
        {
            try
            {
                
                var medicamentoExistente = await _context.Medicamentos.FindAsync(medicamentosId);

                if (medicamentoExistente == null)
                {
                    return NotFound("Medicamento não encontrado");
                }

               
                var novaContaCorrente = new ContaCorrenteMedicamento
                {
                    MedicamentosId = medicamentosId,
                    Data = DateTime.Now, 
                    Tipo = false, 
                    QuantidadeMovimento = quantidadeAdicionada,
                    Observacoes = observacoes
                };

             
                _context.ContaCorrenteMedicamento.Add(novaContaCorrente);

                medicamentoExistente.Stock += quantidadeAdicionada;

                await _context.SaveChangesAsync();

                return Ok($"Adição de {quantidadeAdicionada} unidades ao stock do medicamento registrada com sucesso");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocorreu um erro ao adicionar o medicamento ao stock: {ex.Message}");
            }
        }



        [HttpPost("registrar-utilizacao")]
        public async Task<ActionResult> RegistrarUtilizacaoMedicamento(int medicamentosId, int funcionariosId, int utentesId, int quantidadeUtilizada, string observacoes)
        {
            try
            {
               
                var medicamentoExistente = await _context.Medicamentos.FindAsync(medicamentosId);
                var funcionarioExistente = await _context.Funcionarios.FindAsync(funcionariosId);
                var utenteExistente = await _context.Utentes.FindAsync(utentesId);

                if (medicamentoExistente == null || funcionarioExistente == null || utenteExistente == null)
                {
                    return NotFound("Medicamento, funcionário ou utente não encontrado");
                }

              
                if (medicamentoExistente.Stock < quantidadeUtilizada)
                {
                    return BadRequest("Stock insuficiente para realizar a utilização do medicamento");
                }

                
                var novaContaCorrente = new ContaCorrenteMedicamento
                {
                    MedicamentosId = medicamentosId,
                    FuncionariosId = funcionariosId,
                    UtentesId = utentesId,
                    Data = DateTime.Now, 
                    Tipo = true, 
                    QuantidadeMovimento = quantidadeUtilizada,
                    Observacoes = observacoes
                };

               
                _context.ContaCorrenteMedicamento.Add(novaContaCorrente);

               
                medicamentoExistente.Stock -= quantidadeUtilizada;

               
                await _context.SaveChangesAsync();

                return Ok("Utilização de medicamento registrada com sucesso e stock atualizado");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocorreu um erro ao registrar a utilização de medicamento: {ex.Message}");
            }
        }


    }
}
