using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebApplication1.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class MateriaisPlanoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MateriaisPlanoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MaterialPlano>>> ObterTodosMaterialPlano(
            int? planoId = null,
            int? materialId = null)
        {
            IQueryable<MaterialPlano> query = _context.MateriaisPlano;

            if (planoId.HasValue)
            {
                query = query.Where(d => d.PlanosId == planoId.Value);
            }

            if (materialId.HasValue)
            {
                query = query.Where(d => d.MateriaisId == materialId.Value);
            }


            var materialPlanoDetalhes = await (
                from matPla in query
                join material in _context.Materiais on matPla.MateriaisId equals material.Id into mG
                from material in mG.DefaultIfEmpty()
                select new
                {
                    PlanoId = matPla.PlanosId,
                    MaterialId = matPla.MateriaisId,
                    Material = material.Nome,
                    QuantidadePI = matPla.QuantidadePIntervalo,
                    Intervalo = matPla.IntervaloHoras,
                    Intrucoes = matPla.Instrucoes
                }
            ).ToListAsync();

            return Ok(materialPlanoDetalhes);
        }

        [HttpGet("{PlanosId}/{MateriaisId}")]
        public async Task<ActionResult<MaterialPlano>> ObterMaterialPlano(int PlanosId, int MateriaisId)
        {
            IQueryable<MaterialPlano> query = _context.MateriaisPlano;
            query = query.Where(d => d.PlanosId == PlanosId && d.MateriaisId == MateriaisId);

            var materialPlanoDetalhes = await (
                from matPla in query
                join material in _context.Materiais on matPla.MateriaisId equals material.Id into mG
                from material in mG.DefaultIfEmpty()
                select new
                {
                    PlanoId = matPla.PlanosId,
                    MaterialId = matPla.MateriaisId,
                    Material = material.Nome,
                    QuantidadePI = matPla.QuantidadePIntervalo,
                    Intervalo = matPla.IntervaloHoras,
                    Intrucoes = matPla.Instrucoes
                }
            ).ToListAsync();

            return Ok(materialPlanoDetalhes);
        }

        [HttpPost]
        public async Task<ActionResult<MaterialPlano>> InserirMaterialPlano([FromBody] MaterialPlano materialPlano)
        {
            if (materialPlano == null)
            {
                return BadRequest("Objeto inválido");
            }

            _context.MateriaisPlano.Add(materialPlano);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ObterMaterialPlano), new { PlanosId = materialPlano.PlanosId, MateriaisId = materialPlano.MateriaisId }, materialPlano);
        }

        [HttpPut("{PlanosId}/{MateriaisId}")]
        public async Task<IActionResult> AtualizaMaterialPlano(int PlanosId, int MateriaisId, [FromBody] MaterialPlano novoMaterialPlano)
        {
            var materialPlano = await _context.MateriaisPlano.FirstOrDefaultAsync(a => a.PlanosId == PlanosId && a.MateriaisId == MateriaisId);

            if (materialPlano == null)
            {
                return NotFound($"Não foi possível encontrar o materialPlano com o plano ID {PlanosId} e material ID {MateriaisId}");
            }

            materialPlano.QuantidadePIntervalo = novoMaterialPlano.QuantidadePIntervalo;
            materialPlano.IntervaloHoras = novoMaterialPlano.IntervaloHoras;
            materialPlano.Instrucoes = novoMaterialPlano.Instrucoes;

            await _context.SaveChangesAsync();

            return Ok($"Foi atualizada o materialPlano com o plano ID {PlanosId} e material ID {MateriaisId}");
        }

        [HttpDelete("{PlanosId}/{MateriaisId}")]
        public async Task<IActionResult> RemoveMaterialPlano(int PlanosId, int MateriaisId)
        {
            var materialPlano = await _context.MateriaisPlano.FirstOrDefaultAsync(a => a.PlanosId == PlanosId && a.MateriaisId == MateriaisId);

            if (materialPlano == null)
            {
                return NotFound($"Não foi possível encontrar o materialPlano com o plano ID {PlanosId} e material ID {MateriaisId}");
            }

            _context.MateriaisPlano.Remove(materialPlano);
            await _context.SaveChangesAsync();

            return Ok($"Foi removida o materialPlano com o plano ID {PlanosId} e material ID {MateriaisId}");
        }
    }
}
