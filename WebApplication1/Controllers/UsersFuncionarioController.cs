using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Modelos;
using Microsoft.Extensions.Options;
using WebApplication1.Dtos;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserFuncionarioController : ControllerBase
    {
        private readonly UserManager<UserFuncionario> _userManager;
        private readonly SignInManager<UserFuncionario> _signInManager;
        private readonly AppSettings _appSettings;

        public UserFuncionarioController(UserManager<UserFuncionario> userManager, SignInManager<UserFuncionario> signInManager, IOptions<AppSettings> appSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appSettings = appSettings.Value;
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

            // Crie um novo objeto UserFuncionario sem definir explicitamente a coluna de identidade
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


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserFuncionarioLoginDto loginDto)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest("Dados de login inválidos");
            }

            var user = await _userManager.FindByNameAsync(loginDto.UserName);

            
            if (user == null)
            {
                return NotFound("Usuário não encontrado");
            }

            
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, lockoutOnFailure: false);

            
            if (result.Succeeded)
            {
                var token = GenerateJwtToken(user); 
                return Ok(new { Token = token });
            }

            
            return Unauthorized("Credenciais inválidas");
        }

        private string GenerateJwtToken(UserFuncionario user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret); 

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.Name, user.UserName),
                   
                }),
                Expires = DateTime.UtcNow.AddHours(1), 
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
