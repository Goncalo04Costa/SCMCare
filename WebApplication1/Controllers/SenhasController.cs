using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;
using RegrasNegocio;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SenhasController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly RegrasSenhas _regrasSenhas;

        public SenhasController(AppDbContext context, RegrasSenhas regrasSenhas)
        {
            _context = context;
            _regrasSenhas = regrasSenhas;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Senha>>> ObterTodasSenhas()
        {
            return await _context.Senhas.ToListAsync();
        }

        [HttpGet("{funcionariosId}/{menuId}")]
        public async Task<ActionResult<Senha>> ObterSenha(int funcionariosId, int menuId)
        {
            var senha = await _context.Senhas.FindAsync(funcionariosId, menuId);

            if (senha == null)
            {
                return NotFound();
            }

            return senha;
        }

        [HttpPost]
        public async Task<ActionResult<Senha>> InserirSenha([FromBody] Senha senha)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Senhas.Add(senha);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ObterSenha), new { funcionariosId = senha.FuncionariosId, menuId = senha.MenuId }, senha);
        }

        [HttpPut("{funcionariosId}/{menuId}")]
        public async Task<IActionResult> AtualizarSenha(int funcionariosId, int menuId, [FromBody] Senha senha)
        {
            if (funcionariosId != senha.FuncionariosId || menuId != senha.MenuId)
            {
                return BadRequest();
            }

            _context.Entry(senha).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SenhaExists(funcionariosId, menuId))
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

        [HttpDelete("{funcionariosId}/{menuId}")]
        public async Task<IActionResult> RemoverSenha(int funcionariosId, int menuId)
        {
            var senha = await _context.Senhas.FindAsync(funcionariosId, menuId);
            if (senha == null)
            {
                return NotFound();
            }

            _context.Senhas.Remove(senha);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("reservar/{funcionarioId}/{menuId}")]
        public async Task<ActionResult<Senha>> ReservarSenha(int funcionarioId, int menuId)
        {
            var funcionario = await _context.Funcionarios.FindAsync(funcionarioId);
            var menu = await _context.Menu.FindAsync(menuId);

            if (funcionario == null || menu == null)
            {
                return NotFound("Funcionário ou menu não encontrado.");
            }

            var senhaReservada = await _regrasSenhas.VerificarReservaExistente(funcionarioId, menuId);
            if (senhaReservada)
            {
                return Conflict("A senha já está reservada para este funcionário.");
            }

            var senha = new Senha
            {
                FuncionariosId = funcionarioId,
                MenuId = menuId,
                Estado = 1 
            };

            _context.Senhas.Add(senha);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ObterSenha), new { funcionariosId = senha.FuncionariosId, menuId = senha.MenuId }, senha);
        }

        private bool SenhaExists(int funcionariosId, int menuId)
        {
            return _context.Senhas.Any(e => e.FuncionariosId == funcionariosId && e.MenuId == menuId);
        }
    }
}
