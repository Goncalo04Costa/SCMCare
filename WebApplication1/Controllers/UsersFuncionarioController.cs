using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Dtos;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserFuncionarioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserFuncionarioController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserFuncionario>>> ObterTodosUsersFuncionario(
            int? idMin = null, int? idMax = null,
            string nomeMin = null, string nomeMax = null)
        {
            IQueryable<UserFuncionario> query = _context.UserFuncionario;

            if (idMin.HasValue)
            {
                query = query.Where(d => d.FuncionarioId >= idMin.Value);
            }

            if (idMax.HasValue)
            {
                query = query.Where(d => d.FuncionarioId <= idMax.Value);
            }

            if (!string.IsNullOrEmpty(nomeMin) && !string.IsNullOrEmpty(nomeMax))
            {
                query = query.Where(d => string.Compare(d.User, nomeMin, StringComparison.OrdinalIgnoreCase) >= 0 &&
                                          string.Compare(d.User, nomeMax, StringComparison.OrdinalIgnoreCase) <= 0);
            }
            else if (!string.IsNullOrEmpty(nomeMin))
            {
                query = query.Where(d => string.Compare(d.User, nomeMin, StringComparison.OrdinalIgnoreCase) >= 0);
            }
            else if (!string.IsNullOrEmpty(nomeMax))
            {
                query = query.Where(d => string.Compare(d.User, nomeMax, StringComparison.OrdinalIgnoreCase) <= 0);
            }

            var dados = await query.ToListAsync();
            return Ok(dados);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserFuncionario>> ObterUserFuncionario(int id)
        {
            var userFuncionario = await _context.UserFuncionario.FirstOrDefaultAsync(d => d.FuncionarioId == id);
            if (userFuncionario == null)
            {
                return NotFound();
            }
            return Ok(userFuncionario);
        }

        [HttpPost]
        public async Task<ActionResult<UserFuncionario>> InserirUserFuncionario([FromBody] UserFuncionario user)
        {
            if (user == null || !ModelState.IsValid)
            {
                return BadRequest("Objeto inválido");
            }

            _context.UserFuncionario.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ObterUserFuncionario), new { id = user.FuncionarioId }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarUserFuncionario(int id, [FromBody] UserFuncionario novoUser)
        {
            var user = await _context.UserFuncionario.FirstOrDefaultAsync(d => d.FuncionarioId == id);
            if (user == null)
            {
                return NotFound($"Não foi possível encontrar o funcionário com o ID {id}");
            }

            user.User = novoUser.User;
            user.Passe = novoUser.Passe; // Suponho que também precise atualizar a senha

            try
            {
                await _context.SaveChangesAsync();

                return Ok($"Foi atualizado o funcionário com o ID {id}");
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Erro ao atualizar o funcionário: {e.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoverUserFuncionario(int id)
        {
            var user = await _context.UserFuncionario.FirstOrDefaultAsync(d => d.FuncionarioId == id);
            if (user == null)
            {
                return NotFound($"Não foi possível encontrar o funcionário com o ID {id}");
            }

            _context.UserFuncionario.Remove(user);
            await _context.SaveChangesAsync();

            return Ok($"Foi removido o funcionário com o ID {id}");
        }


        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarUserFuncionario([FromBody] UserFuncionarioRegistroDto userDto)
        {
            if (userDto == null || !ModelState.IsValid)
            {
                return BadRequest("Dados de registro inválidos");
            }

            // Verifique se o usuário já existe
            var existingUser = await _context.UserFuncionario.FirstOrDefaultAsync(u => u.User == userDto.User);
            if (existingUser != null)
            {
                return Conflict("Usuário já existe");
            }

            // Crie um novo objeto UserFuncionario com base nos dados recebidos
            var newUser = new UserFuncionario
            {
                User = userDto.User,
                Passe = userDto.Passe,
                // Outras propriedades, se houver
            };

            // Adicione o novo usuário ao contexto do banco de dados e salve as alterações
            _context.UserFuncionario.Add(newUser);
            await _context.SaveChangesAsync();

            // Retorne uma resposta de sucesso com os detalhes do novo usuário
            return CreatedAtAction(nameof(ObterUserFuncionario), new { id = newUser.FuncionarioId }, newUser);
        }



    }
}
