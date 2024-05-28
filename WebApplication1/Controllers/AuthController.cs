using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1;
using WebApplication1.Controllers;

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


    // mudar isso de id para username 
    // funcao para verificar se token passou validade

    [HttpPost("login")]
    public async Task<IActionResult> Login(string userUserName, string password)
    {
        var data = await DetermineUserRoleAsync(userUserName, password);
        string token;


        if (!string.IsNullOrEmpty(data.Item1 )&& data.Item1=="Funcionario"  )
        {
            token = _tokenService.GenerateToken(userUserName,  data.Item1);
            return Ok(new { Token = token });
        }

       else  if (!string.IsNullOrEmpty(data.Item1) && data.Item1 == "Responsavel")
        {
            token = _tokenService.GenerateToken(userUserName, data.Item1);
            return Ok(new { Token = token });
        }

        return BadRequest();

    }

    [HttpGet("{UserName}")]
    public async Task<ActionResult<UtilizadorF>> GetUserFuncionario(string UserName, string Password)
    {
        var userR = await _context.utilizadorF.FirstOrDefaultAsync(f => f.UserName == UserName && f.Password == Password);

        if (userR == null)
        {
            return NotFound($"User com o UserName {UserName} não encontrado");
        }

        return Ok(userR);
    }

    // GET: api/UserR/5
    [HttpGet("{UsernameR}")]
    public async Task<ActionResult<UtilizadorR>> GetUserResponsavel(string UserName, string Password)
    {
        var userR = await _context.utilizadorF.FirstOrDefaultAsync(f => f.UserName == UserName && f.Password == Password);

        if (userR == null)
        {
            return NotFound($"User com o UserName {UserName} não encontrado");
        }

        return Ok(userR);
    }

    private async Task<(string,string,string)> DetermineUserRoleAsync(string userUserName, string password)
    {
        var funcionario = await GetUserFuncionario(userUserName, password);

        if (funcionario.Result is OkObjectResult okFuncionario)
        {
            var funcionarioResult = okFuncionario.Value as UtilizadorF;
            return ("Funcionario", funcionarioResult.UserName, funcionarioResult.Password);
        }

        var responsavel = await GetUserResponsavel(userUserName, password);
        if(responsavel.Result is OkObjectResult okResponsavel)
        {
            var responsavelresult = okResponsavel.Value as UtilizadorR;
            return  ("Responsavel", responsavelresult.UserName, responsavelresult.Password);
        }


        return ("","","");
    }

    
}
