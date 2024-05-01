using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;


namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MateriaisController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MateriaisController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Material>>> ObterTodosMateriais(
            int? idMin = null, int? idMax = null,
            string? nomeMin = null, string? nomeMax = null,
            int? tipoMaterial = null,
            bool ativo0 = false, bool ativo1 = false)
        {
            IQueryable<Material> query = _context.Materiais;

            if (idMin.HasValue)
            {
                query = query.Where(d => d.Id >= idMin.Value);
            }

            if (idMax.HasValue)
            {
                query = query.Where(d => d.Id <= idMax.Value);
            }

            if (!string.IsNullOrEmpty(nomeMin))
            {
                query = query.Where(d => d.Nome.CompareTo(nomeMin) >= 0);
            }

            if (!string.IsNullOrEmpty(nomeMax))
            {
                query = query.Where(d => d.Nome.CompareTo(nomeMax + "ZZZ") <= 0);
            }

            if (tipoMaterial.HasValue)
            {
                query = query.Where(d => d.TiposMaterialId == tipoMaterial.Value);
            }

            if (ativo0 && !ativo1)
            {
                query = query.Where(d => !d.Ativo);
            }

            else if (!ativo0 && ativo1)
            {
                query = query.Where(d => d.Ativo);
            }

            var materiaisDetalhes = await (
                from materiais in query
                join tipomaterial in _context.TiposMaterial on materiais.TiposMaterialId equals tipomaterial.Id into tG
                from tipomaterial in tG.DefaultIfEmpty()
                select new
                {
                    Id = materiais.Id,
                    Nome = materiais.Nome,
                    Quantidade = _context.ContaCorrenteMateriais
                        .Where(m => m.MateriaisId == materiais.Id)
                        .Sum(m => m.Tipo ? m.QuantidadeMovimento : -m.QuantidadeMovimento),
                    Limite = materiais.Limite,
                    TipoMaterialId = materiais.TiposMaterialId,
                    TipoMaterial = tipomaterial.Descricao,
                    Ativo = materiais.Ativo
                }
            ).ToListAsync();

            return Ok(materiaisDetalhes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Material>> ObterMaterial(int id)
        {
            IQueryable<Material> query = _context.Materiais;
            query = query.Where(d => d.Id == id);

            var materiaisDetalhes = await (
                from materiais in query
                join tipomaterial in _context.TiposMaterial on materiais.TiposMaterialId equals tipomaterial.Id into tG
                from tipomaterial in tG.DefaultIfEmpty()
                select new
                {
                    Id = materiais.Id,
                    Nome = materiais.Nome,
                    Quantidade = _context.ContaCorrenteMateriais
                        .Where(m => m.MateriaisId == materiais.Id)
                        .Sum(m => m.Tipo ? m.QuantidadeMovimento : -m.QuantidadeMovimento),
                    Limite = materiais.Limite,
                    TipoMaterialId = materiais.TiposMaterialId,
                    TipoMaterial = tipomaterial.Descricao,
                    Ativo = materiais.Ativo
                }
            ).ToListAsync();

            return Ok(materiaisDetalhes);
        }

        [HttpPost]
        public async Task<ActionResult<Material>> InserirMaterial([FromBody] Material material)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Materiais.Add(material);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ObterMaterial), new { id = material.Id }, material);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarMaterial(int id, [FromBody] Material material)
        {
            if (id != material.Id)
            {
                return BadRequest();
            }

            _context.Entry(material).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MaterialExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoverMaterial(int id)
        {
            var material = await _context.Materiais.FindAsync(id);
            if (material == null)
            {
                return NotFound();
            }

            _context.Materiais.Remove(material);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MaterialExists(int id)
        {
            return _context.Materiais.Any(e => e.Id == id);
        }


        // !!! Rever com o calcular quantidades a partir de conta corrente

        //[HttpGet("emrisco")]
        //public async Task<ActionResult<IEnumerable<Material>>> ObterMateriaisRisco(int limite)
        //{
        //    try
        //    {
        //        var materiaisRisco = await _context.Materiais
        //            .Where(m => m.QuantidadeAtual < m.Limite)
        //            .ToListAsync();

        //        return Ok(materiaisRisco);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Erro interno ao obter materiais em risco: {ex.Message}");
        //    }
        //}

    }
}
