using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenusController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MenusController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Menu>>> ObterTodosMenus(
            int? idMin = null, int? idMax = null,
            DateTime? diaMin = null, DateTime? diaMax = null,
            bool horario0 = false, bool horario1 = false,
            bool tipo0 = false, bool tipo1 = false)
        {
            IQueryable<Menu> query = _context.Menu;

            if (idMin.HasValue)
            {
                query = query.Where(d => d.Id >= idMin.Value);
            }

            if (idMax.HasValue)
            {
                query = query.Where(d => d.Id <= idMax.Value);
            }

            if (diaMin.HasValue)
            {
                query = query.Where(d => d.Dia <= diaMin.Value);
            }

            if (diaMax.HasValue)
            {
                query = query.Where(d => d.Dia >= diaMax.Value);
            }

            if (horario0 && !horario1)
            {
                query = query.Where(d => !d.Horario);
            }
            else if (!horario0 && horario1)
            {
                query = query.Where(d => d.Horario);
            }

            if (tipo0 && !tipo1)
            {
                query = query.Where(d => !d.Tipo);
            }
            else if (!tipo0 && tipo1)
            {
                query = query.Where(d => d.Tipo);
            }


            var menusDetalhes = await (
                from menu in query
                join sobremesa in _context.Sobremesas on menu.SobremesasId equals sobremesa.Id into sG
                from sobremesa in sG.DefaultIfEmpty()
                join prato in _context.Pratos on menu.PratosId equals prato.Id into pG
                from prato in pG.DefaultIfEmpty()
                join sopa in _context.Sopas on menu.SopasId equals sopa.Id into soG
                from sopa in soG.DefaultIfEmpty()
                select new
                {
                    Id = menu.Id,
                    Dia = menu.Dia,
                    Horario = menu.Horario,
                    Tipo = menu.Tipo,
                    SopasId = menu.SopasId,
                    Sopa = sopa.Nome,
                    PratosId = menu.PratosId,
                    Prato = prato.Nome,
                    SobremesasId = menu.SobremesasId,
                    Sobremesa = sobremesa.Nome
                }
            ).ToListAsync();

            return Ok(menusDetalhes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Menu>> ObterMenu(int id)
        {
            IQueryable<Menu> query = _context.Menu;
            query = query.Where(d => d.Id == id);

            var menusDetalhes = await (
                from menu in query
                join sobremesa in _context.Sobremesas on menu.SobremesasId equals sobremesa.Id into sG
                from sobremesa in sG.DefaultIfEmpty()
                join prato in _context.Pratos on menu.PratosId equals prato.Id into pG
                from prato in pG.DefaultIfEmpty()
                join sopa in _context.Sopas on menu.SopasId equals sopa.Id into soG
                from sopa in soG.DefaultIfEmpty()
                select new
                {
                    Id = menu.Id,
                    Dia = menu.Dia,
                    Horario = menu.Horario,
                    Tipo = menu.Tipo,
                    SopasId = menu.SopasId,
                    Sopa = sopa.Nome,
                    PratosId = menu.PratosId,
                    Prato = prato.Nome,
                    SobremesasId = menu.SobremesasId,
                    Sobremesa = sobremesa.Nome
                }
            ).ToListAsync();

            return Ok(menusDetalhes);
        }

        [HttpPost]
        public async Task<ActionResult<Menu>> InserirMenu([FromBody] Menu menu)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Menu.Add(menu);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ObterMenu), new { id = menu.Id }, menu);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarMenu(int id, [FromBody] Menu menu)
        {
            if (id != menu.Id)
            {
                return BadRequest();
            }

            _context.Entry(menu).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MenuExists(id))
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
        public async Task<IActionResult> RemoverMenu(int id)
        {
            var menu = await _context.Menu.FindAsync(id);
            if (menu == null)
            {
                return NotFound();
            }

            _context.Menu.Remove(menu);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MenuExists(int id)
        {
            return _context.Menu.Any(e => e.Id == id);
        }
    }
}
