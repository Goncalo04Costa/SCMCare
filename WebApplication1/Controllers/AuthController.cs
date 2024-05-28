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

    public class LoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        var data = await DetermineUserRoleAsync(loginRequest.UserName, loginRequest.Password);
        if (!string.IsNullOrEmpty(data.Role))
        {
            string kid = DetermineKidBasedOnLogic(data.Role);
            string token = _tokenService.GenerateToken(loginRequest.UserName, data.Role, kid);
            return Ok(new { Token = token });
        }

        return BadRequest("Invalid username or password.");
    }

    private async Task<(string Role, string UserName, string Password)> DetermineUserRoleAsync(string userName, string password)
    {
        var userF = await _context.utilizadorF.FirstOrDefaultAsync(f => f.UserName == userName && f.Password == password);
        if (userF != null)
        {
            return ("Funcionario", userF.UserName, userF.Password);
        }

        var userR = await _context.utilizadorR.FirstOrDefaultAsync(r => r.UserName == userName && r.Password == password);
        if (userR != null)
        {
            return ("Responsavel", userR.UserName, userR.Password);
        }

        return ("", "", "");
    }

    [HttpGet("funcionario/{userName}")]
    public async Task<ActionResult<UtilizadorF>> GetUserFuncionario(string userName, string password)
    {
        var userF = await _context.utilizadorF.FirstOrDefaultAsync(f => f.UserName == userName && f.Password == password);
        if (userF == null)
        {
            return NotFound($"User with UserName {userName} not found.");
        }

        return Ok(userF);
    }

    [HttpGet("responsavel/{userName}")]
    public async Task<ActionResult<UtilizadorR>> GetUserResponsavel(string userName, string password)
    {
        var userR = await _context.utilizadorR.FirstOrDefaultAsync(r => r.UserName == userName && r.Password == password);
        if (userR == null)
        {
            return NotFound($"User with UserName {userName} not found.");
        }

        return Ok(userR);
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
        return role switch
        {
            "Funcionario" => "kid_for_funcionario",
            "Responsavel" => "kid_for_responsavel",
            _ => "default_kid",
        };
    }
}
