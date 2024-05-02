using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;
//using RegrasNegocio;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SenhasController : ControllerBase
    {
        private readonly AppDbContext _context;

        // !!! Rever o funcionamento das regras
        //private readonly RegrasSenhas _regrasSenhas;

        //public SenhasController(AppDbContext context, RegrasSenhas regrasSenhas)
        public SenhasController(AppDbContext context)
        {
            _context = context;
            //_regrasSenhas = regrasSenhas;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Senha>>> ObterTodasSenhas(
            int? funcionarioId = null, int? menuId = null, int? estado = null)
        {
            IQueryable<Senha> query = _context.Senhas;

            if (funcionarioId.HasValue)
            {
                query = query.Where(d => d.FuncionariosId == funcionarioId.Value);
            }

            if (menuId.HasValue)
            {
                query = query.Where(d => d.MenuId == menuId.Value);
            }

            if (estado.HasValue)
            {
                query = query.Where(d => d.Estado == estado.Value);
            }


            var senhasDetalhes = await (
                from senha in query
                join funcionario in _context.Funcionarios on senha.FuncionariosId equals funcionario.FuncionarioID into fG
                from funcionario in fG.DefaultIfEmpty()
                join menu in _context.Menu on senha.MenuId equals menu.Id into mG
                from menu in mG.DefaultIfEmpty()
                select new
                {
                    FuncionarioId = senha.FuncionariosId,
                    Funcionario = funcionario.Nome,
                    MenuId = senha.MenuId,
                    MenuDia = menu.Dia,
                    MenuHorario = menu.Horario,
                    MenuTipo = menu.Tipo,
                    Estado = senha.Estado
                }
            ).ToListAsync();

            return Ok(senhasDetalhes);
        }

        [HttpGet("{funcionariosId}/{menuId}")]
        public async Task<ActionResult<Senha>> ObterSenha(int funcionariosId, int menuId)
        {
            IQueryable<Senha> query = _context.Senhas;
            query = query.Where(d => d.FuncionariosId == funcionariosId && d.MenuId == menuId);

            var senhaDetalhes = await (
                from senha in query
                join funcionario in _context.Funcionarios on senha.FuncionariosId equals funcionario.FuncionarioID into fG
                from funcionario in fG.DefaultIfEmpty()
                join menu in _context.Menu on senha.MenuId equals menu.Id into mG
                from menu in mG.DefaultIfEmpty()
                select new
                {
                    FuncionarioId = senha.FuncionariosId,
                    Funcionario = funcionario.Nome,
                    MenuId = senha.MenuId,
                    MenuDia = menu.Dia,
                    MenuHorario = menu.Horario,
                    MenuTipo = menu.Tipo,
                    Estado = senha.Estado
                }
            ).ToListAsync();

            return Ok(senhaDetalhes);
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

            //var senhaReservada = await _regrasSenhas.VerificarReservaExistente(funcionarioId, menuId);
            //if (senhaReservada)
            //{
            //    return Conflict("A senha já está reservada para este funcionário.");
            //}

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
