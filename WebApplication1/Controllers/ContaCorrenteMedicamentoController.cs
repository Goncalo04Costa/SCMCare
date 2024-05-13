using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


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
            bool tipo0 = false, bool tipo1 = false,
            int? quantidadeMovimentoMin = null, int? quantidadeMovimentoMax = null,
            string? observacoesMin = null, string? observacoesMax = null)
        {
            IQueryable<ContaCorrenteMedicamento> query = _context.ContaCorrenteMedicamentos;

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
                query = query.Where(d => d.Data >= dataMin.Value);
            }

            if (dataMax.HasValue)
            {
                query = query.Where(d => d.Data <= dataMax.Value);
            }

            if (tipo0 && !tipo1)
            {
                query = query.Where(d => !d.Tipo);
            }

            else if (!tipo0 && tipo1)
            {
                query = query.Where(d => d.Tipo);
            }

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


            var contacorrenteDetalhes = await (
                from contacorrente in query
                join medicamento in _context.Materiais on contacorrente.MedicamentosId equals medicamento.Id into mG
                from medicamento in mG.DefaultIfEmpty()
                join funcionario in _context.Funcionarios on contacorrente.FuncionariosId equals funcionario.FuncionarioID into fG
                from funcionario in fG.DefaultIfEmpty()
                join utente in _context.Utentes on contacorrente.UtentesId equals utente.Id into uG
                from utente in uG.DefaultIfEmpty()
                select new
                {
                    Id = contacorrente.Id,
                    Fatura = contacorrente.Fatura,
                    MedicamentosId = contacorrente.MedicamentosId,
                    Medicamento = medicamento.Nome,
                    PedidosId = contacorrente.PedidosMedicamentoId,
                    FuncionariosId = contacorrente.FuncionariosId,
                    Funcionario = funcionario.Nome,
                    UtentesId = contacorrente.UtentesId,
                    Utente = utente.Nome,
                    Data = contacorrente.Data,
                    Tipo = contacorrente.Tipo,
                    Quantidade = contacorrente.QuantidadeMovimento,
                    Obs = contacorrente.Observacoes
                }
            ).ToListAsync();

            return Ok(contacorrenteDetalhes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ContaCorrenteMedicamento>> obterContaCorrente(int id)
        {
            IQueryable<ContaCorrenteMedicamento> query = _context.ContaCorrenteMedicamentos;
            query = query.Where(d => d.Id == id);

            var contacorrenteDetalhes = await (
                from contacorrente in query
                join medicamento in _context.Materiais on contacorrente.MedicamentosId equals medicamento.Id into mG
                from medicamento in mG.DefaultIfEmpty()
                join funcionario in _context.Funcionarios on contacorrente.FuncionariosId equals funcionario.FuncionarioID into fG
                from funcionario in fG.DefaultIfEmpty()
                join utente in _context.Utentes on contacorrente.UtentesId equals utente.Id into uG
                from utente in uG.DefaultIfEmpty()
                select new
                {
                    Id = contacorrente.Id,
                    Fatura = contacorrente.Fatura,
                    MedicamentosId = contacorrente.MedicamentosId,
                    Medicamento = medicamento.Nome,
                    PedidosId = contacorrente.PedidosMedicamentoId,
                    FuncionariosId = contacorrente.FuncionariosId,
                    Funcionario = funcionario.Nome,
                    UtentesId = contacorrente.UtentesId,
                    Utente = utente.Nome,
                    Data = contacorrente.Data,
                    Tipo = contacorrente.Tipo,
                    Quantidade = contacorrente.QuantidadeMovimento,
                    Obs = contacorrente.Observacoes
                }
            ).ToListAsync();

            return Ok(contacorrenteDetalhes);
        }

        [HttpPost]
        public async Task<ActionResult<ContaCorrenteMedicamento>> InserirContaCorrente([FromBody] ContaCorrenteMedicamento contaCorrente)
        {
            if (contaCorrente == null)
            {
                return BadRequest("Objeto inválido");
            }

            _context.ContaCorrenteMedicamentos.Add(contaCorrente);
            await _context.SaveChangesAsync();

            return Ok("Adicionado novo registo na conta corrente");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizaContaCorrente(int id, [FromBody] ContaCorrenteMedicamento novaContaCorrente)
        {
            var contaCorrente = await _context.ContaCorrenteMedicamentos.FirstOrDefaultAsync(d => d.Id == id);
            if (contaCorrente == null)
            {
                return NotFound($"Não foi possível encontrar o registo com o Id {id}");
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

                return Ok($"Foi atualizado o registo com Id {id}");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveContaCorrente(int id)
        {
            var contaCorrente = await _context.ContaCorrenteMedicamentos.FirstOrDefaultAsync(d => d.Id == id);
            if (contaCorrente == null)
            {
                return NotFound($"Não foi possível encontrar o registo com o ID {id}");
            }

            _context.ContaCorrenteMedicamentos.Remove(contaCorrente);
            await _context.SaveChangesAsync();

            return Ok($"Foi removido o registo com o Id {id}");
        }



        // !!! Porquê adicionar isto? v v v
        // Já existe o InserirContaCorrente, mas podemos aproveitar a verificação de quantidade para as regras

        //[HttpPost("adicionar-medicamento")]
        //public async Task<ActionResult> AdicionarMedicamento(int medicamentosId, int quantidadeAdicionada, string observacoes)
        //{
        //    try
        //    {

        //        var medicamentoExistente = await _context.Medicamentos.FindAsync(medicamentosId);

        //        if (medicamentoExistente == null)
        //        {
        //            return NotFound("Medicamento não encontrado");
        //        }


        //        var novaContaCorrente = new ContaCorrenteMedicamento
        //        {
        //            MedicamentosId = medicamentosId,
        //            Data = DateTime.Now, 
        //            Tipo = false, 
        //            QuantidadeMovimento = quantidadeAdicionada,
        //            Observacoes = observacoes
        //        };


        //        _context.ContaCorrenteMedicamentos.Add(novaContaCorrente);

        //        //medicamentoExistente.Stock += quantidadeAdicionada;

        //        await _context.SaveChangesAsync();

        //        return Ok($"Adição de {quantidadeAdicionada} unidades ao stock do medicamento registrada com sucesso");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Ocorreu um erro ao adicionar o medicamento ao stock: {ex.Message}");
        //    }
        //}



        //[HttpPost("registrar-utilizacao")]
        //public async Task<ActionResult> RegistrarUtilizacaoMedicamento(int medicamentosId, int funcionariosId, int utentesId, int quantidadeUtilizada, string observacoes)
        //{
        //    try
        //    {

        //        var medicamentoExistente = await _context.Medicamentos.FindAsync(medicamentosId);
        //        var funcionarioExistente = await _context.Funcionarios.FindAsync(funcionariosId);
        //        var utenteExistente = await _context.Utentes.FindAsync(utentesId);

        //        if (medicamentoExistente == null || funcionarioExistente == null || utenteExistente == null)
        //        {
        //            return NotFound("Medicamento, funcionário ou utente não encontrado");
        //        }


        //        /*if (medicamentoExistente.Stock < quantidadeUtilizada)
        //        {
        //            return BadRequest("Stock insuficiente para realizar a utilização do medicamento");
        //        }*/


        //        var novaContaCorrente = new ContaCorrenteMedicamento
        //        {
        //            MedicamentosId = medicamentosId,
        //            FuncionariosId = funcionariosId,
        //            UtentesId = utentesId,
        //            Data = DateTime.Now, 
        //            Tipo = true, 
        //            QuantidadeMovimento = quantidadeUtilizada,
        //            Observacoes = observacoes
        //        };


        //        _context.ContaCorrenteMedicamento.Add(novaContaCorrente);


        //        //medicamentoExistente.Stock -= quantidadeUtilizada;


        //        await _context.SaveChangesAsync();

        //        return Ok("Utilização de medicamento registrada com sucesso e stock atualizado");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Ocorreu um erro ao registrar a utilização de medicamento: {ex.Message}");
        //    }
        //}


    }
}
