using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly TokenService _tokenService;
    private readonly AppDbContext _context;

    public AuthController(TokenService tokenService, AppDbContext appDbContext)
    {
        _tokenService = tokenService;
        _context = appDbContext;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(string userUserName, string password)
    {
        var data = await DetermineUserRoleAsync(userUserName, password);
        string token;

        if (!string.IsNullOrEmpty(data.Item1) && data.Item1 == "Funcionario")
        {
            string kid = DetermineKidBasedOnLogic(data.Item1);
            token = _tokenService.GenerateToken(userUserName, data.Item1, kid);
            return Ok(new { Token = token });
        }
        else if (!string.IsNullOrEmpty(data.Item1) && data.Item1 == "Responsavel")
        {
            string kid = DetermineKidBasedOnLogic(data.Item1);
            token = _tokenService.GenerateToken(userUserName, data.Item1, kid);
            return Ok(new { Token = token });
        }

        return BadRequest("Invalid username or password.");
    }

    [HttpGet("funcionario/{UserName}")]
    public async Task<ActionResult<UtilizadorF>> GetUserFuncionario(string UserName, string Password)
    {
        var userF = await _context.utilizadorF.FirstOrDefaultAsync(f => f.UserName == UserName && f.Password == Password);

        if (userF == null)
        {
            return NotFound($"User with UserName {UserName} not found.");
        }

        return Ok(userF);
    }

    [HttpGet("responsavel/{UserName}")]
    public async Task<ActionResult<UtilizadorR>> GetUserResponsavel(string UserName, string Password)
    {
        var userR = await _context.utilizadorR.FirstOrDefaultAsync(r => r.UserName == UserName && r.Password == Password);

        if (userR == null)
        {
            return NotFound($"User with UserName {UserName} not found.");
        }

        return Ok(userR);
    }

    private async Task<(string, string, string)> DetermineUserRoleAsync(string userUserName, string password)
    {
        var userF = await GetUserFuncionario(userUserName, password);
        if (userF.Result is OkObjectResult okFuncionario)
        {
            var funcionarioResult = okFuncionario.Value as UtilizadorF;
            return ("Funcionario", funcionarioResult.UserName, funcionarioResult.Password);
        }

        var userR = await GetUserResponsavel(userUserName, password);
        if (userR.Result is OkObjectResult okResponsavel)
        {
            var responsavelResult = okResponsavel.Value as UtilizadorR;
            return ("Responsavel", responsavelResult.UserName, responsavelResult.Password);
        }

        return ("", "", "");
    }

    [HttpGet("validate-token")]
    public IActionResult ValidateToken(string token)
    {
        var isValid = _tokenService.ValidateToken(token);
        if (isValid)
        {
            return Ok("Token is valid.");
        }
        return Unauthorized("Token is invalid or expired.");
    }

    private string DetermineKidBasedOnLogic(string role)
    {
        // Example logic to determine kid based on role or environment
        if (role == "Funcionario")
        {
            return "kid_for_funcionario";
        }
        else if (role == "Responsavel")
        {
            return "kid_for_responsavel";
        }
        else
        {
            return "default_kid";
        }
    }
}
