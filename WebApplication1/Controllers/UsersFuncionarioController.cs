using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<UserFuncionario> _userManager;

        public UserFuncionarioController(UserManager<UserFuncionario> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserFuncionario>>> ObterTodosUsersFuncionario(
            int? idMin = null, int? idMax = null,
            string nomeMin = null, string nomeMax = null)
        {
            IQueryable<UserFuncionario> query = _userManager.Users;

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
                query = query.Where(d => string.Compare(d.UserName, nomeMin, StringComparison.OrdinalIgnoreCase) >= 0 &&
                                          string.Compare(d.UserName, nomeMax, StringComparison.OrdinalIgnoreCase) <= 0);
            }
            else if (!string.IsNullOrEmpty(nomeMin))
            {
                query = query.Where(d => string.Compare(d.UserName, nomeMin, StringComparison.OrdinalIgnoreCase) >= 0);
            }
            else if (!string.IsNullOrEmpty(nomeMax))
            {
                query = query.Where(d => string.Compare(d.UserName, nomeMax, StringComparison.OrdinalIgnoreCase) <= 0);
            }

            var dados = await query.ToListAsync();
            return Ok(dados);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserFuncionario>> ObterUserFuncionario(string id)
        {
            var userFuncionario = await _userManager.FindByIdAsync(id);
            if (userFuncionario == null)
            {
                return NotFound();
            }
            return Ok(userFuncionario);
        }
        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarUserFuncionario([FromBody] UserFuncionarioRegistroDto userDto)
        {
            if (userDto == null || !ModelState.IsValid)
            {
                return BadRequest("Dados de registro inválidos");
            }

            // Verifique se o usuário já existe
            var existingUser = await _userManager.FindByNameAsync(userDto.User);
            if (existingUser != null)
            {
                return Conflict("Usuário já existe");
            }

            // Crie um novo objeto UserFuncionario com base nos dados recebidos
            var newUser = new UserFuncionario { UserName = userDto.User };

            // Adicione o novo usuário ao contexto do banco de dados e salve as alterações
            var result = await _userManager.CreateAsync(newUser, userDto.Passe);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            // Retorne uma resposta de sucesso com os detalhes do novo usuário
            return CreatedAtAction(nameof(ObterUserFuncionario), new { id = newUser.Id }, newUser);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarUserFuncionario(string id, [FromBody] UserFuncionarioRegistroDto userDto)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Não foi possível encontrar o funcionário com o ID {id}");
            }

            user.UserName = userDto.User;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok($"Foi atualizado o funcionário com o ID {id}");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoverUserFuncionario(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Não foi possível encontrar o funcionário com o ID {id}");
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok($"Foi removido o funcionário com o ID {id}");
        }
    }
}
