using iText.Kernel.Pdf.Canvas.Wmf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;

namespace WebApplication1.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class FeriasFuncionarioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FeriasFuncionarioController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<FeriasFuncionario>>> ObterTodasFeriasFuncionario(
            int? idFuncionario = null, int? idFuncionarioAprv = null,
            int? estado = null)
        {
            IQueryable<FeriasFuncionario> query = _context.FeriasFuncionario;

            if (idFuncionario.HasValue)
            {
                query = query.Where(d => d.FuncionariosId == idFuncionario.Value);
            }

            if (idFuncionarioAprv.HasValue)
            {
                query = query.Where(d => d.FuncionariosIdValida == idFuncionarioAprv.Value);
            }

            if (estado.HasValue)
            {
                query = query.Where(d => d.Estado == estado.Value);
            }

            var feriasDetalhes = await (
                from ferias in query
                join funcionario in _context.Funcionarios on ferias.FuncionariosId equals funcionario.FuncionarioID into fG
                from funcionario in fG.DefaultIfEmpty()
                join funcionarioV in _context.Funcionarios on ferias.FuncionariosIdValida equals funcionarioV.FuncionarioID into fvG
                from funcionarioV in fvG.DefaultIfEmpty()
                select new
                {
                    Id = ferias.Id,
                    FuncionariosId = ferias.FuncionariosId,
                    Funcionario = funcionario.Nome,
                    FuncionariosIdValida = ferias.FuncionariosIdValida,
                    FuncionarioValida = funcionarioV.Nome,
                    Dia = ferias.Dia,
                    Estado = ferias.Estado,
                }
            ).ToListAsync();

            return Ok(feriasDetalhes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FeriasFuncionario>> ObterFeriaFuncionario(int id)
        {
            IQueryable<FeriasFuncionario> query = _context.FeriasFuncionario;
            query = query.Where(d => d.Id == id);
            

            var feriaDetalhes = await (
                from ferias in query
                join funcionario in _context.Funcionarios on ferias.FuncionariosId equals funcionario.FuncionarioID into fG
                from funcionario in fG.DefaultIfEmpty()
                join funcionarioV in _context.Funcionarios on ferias.FuncionariosIdValida equals funcionarioV.FuncionarioID into fvG
                from funcionarioV in fvG.DefaultIfEmpty()
                select new
                {
                    Id = ferias.Id,
                    FuncionariosId = ferias.FuncionariosId,
                    Funcionario = funcionario.Nome,
                    FuncionariosIdValida = ferias.FuncionariosIdValida,
                    FuncionarioValida = funcionarioV.Nome,
                    Dia = ferias.Dia,
                    Estado = ferias.Estado,
                }
            ).ToListAsync();

            return Ok(feriaDetalhes);
        }

        [HttpPost]
        public async Task<ActionResult<FeriasFuncionario>> InserirFeriasFuncionario([FromBody] FeriasFuncionario feriaFuncionario)
        {
            if (feriaFuncionario == null)
            {
                return BadRequest("Objeto inválido");
            }

            _context.FeriasFuncionario.Add(feriaFuncionario);
            await _context.SaveChangesAsync();

            return Ok("FeriasFuncionario adicionada com sucesso");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizaFeriasFuncionario(int id, [FromBody] FeriasFuncionario novaFeriaFuncionario)
        {
            var feriaFuncionario = await _context.FeriasFuncionario.FindAsync(id);

            if (feriaFuncionario == null)
            {
                return NotFound($"Não foi possível encontrar a feriaFuncionario com o ID {id}");
            }

            feriaFuncionario.FuncionariosId = novaFeriaFuncionario.FuncionariosId;
            feriaFuncionario.FuncionariosIdValida = novaFeriaFuncionario.FuncionariosIdValida;
            feriaFuncionario.Dia = novaFeriaFuncionario.Dia;
            feriaFuncionario.Estado = novaFeriaFuncionario.Estado;

            try
            {
                await _context.SaveChangesAsync();

                return Ok($"Foi atualizada a feriaFuncionario com o ID {id}");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveFeriasFuncionario(int id)
        {
            var feriaFuncionario = await _context.FeriasFuncionario.FindAsync(id);

            if (feriaFuncionario == null)
            {
                return NotFound($"Não foi possível encontrar a feriaFuncionario com o ID {id}");
            }

            _context.FeriasFuncionario.Remove(feriaFuncionario);
            await _context.SaveChangesAsync();

            return Ok($"Foi removida a feriaFuncionario com o ID {id}");
        }

    }
}