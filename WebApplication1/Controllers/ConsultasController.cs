using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;


namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ConsultasController(AppDbContext context)
        {
            _context = context;
        }


        //Metodo para obter todas as consultas com filtros especificos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Consulta>>> ObterTodasConsultas(
            int? idMin = null, int? idMax = null,
            int? hospitaisId = null,
            int? utentesId = null,
            int? funcionariosId = null,
            DateTime? dataMin = null, DateTime? dataMax = null)
        {
            IQueryable<Consulta> query = _context.Consultas;

            if (idMin.HasValue)
            {
                query = query.Where(d => d.Id >= idMin.Value);
            }

            if (idMax.HasValue)
            {
                query = query.Where(d => d.Id <= idMax.Value);
            }

            if (dataMin.HasValue)
            {
                query = query.Where(d => d.Data >= dataMin.Value);
            }

            if (dataMax.HasValue)
            {
                query = query.Where(d => d.Data <= dataMax.Value);
            }

            if (hospitaisId.HasValue)
            {
                query = query.Where(d => d.HospitaisId == hospitaisId.Value);
            }

            if (utentesId.HasValue)
            {
                query = query.Where(d => d.UtentesId == utentesId.Value);
            }

            if (funcionariosId.HasValue)
            {
                query = query.Where(d => d.FuncionariosId == funcionariosId.Value);
            }


            var consultasDetalhes = await (
                from consulta in query
                join hospital in _context.Hospitais on consulta.HospitaisId equals hospital.Id into hG
                from hospital in hG.DefaultIfEmpty()
                join utente in _context.Utentes on consulta.UtentesId equals utente.Id into uG
                from utente in uG.DefaultIfEmpty()
                join funcionario in _context.Funcionarios on consulta.FuncionariosId equals funcionario.FuncionarioID into fG
                from funcionario in fG.DefaultIfEmpty()
                join responsavel in _context.Responsaveis on consulta.ResponsaveisId equals responsavel.Id into rG
                from responsavel in rG.DefaultIfEmpty()
                select new
                {
                    Id = consulta.Id,
                    HospitalId = consulta.HospitaisId,
                    Hospital = hospital.Nome,
                    FuncionarioId = consulta.FuncionariosId,
                    Funcionario = funcionario.Nome,
                    UtenteId = consulta.UtentesId,
                    Utente = utente.Nome,
                    ResponsavelId = consulta.ResponsaveisId,
                    Responsavel = responsavel.Nome,
                    Data = consulta.Data,
                    Descricao = consulta.Descricao
                }
            ).ToListAsync();

            return Ok(consultasDetalhes);
        }


        //Obter consulta pelo ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Consulta>> ObterConsulta(int id)
        {
            IQueryable<Consulta> query = _context.Consultas;
            query = query.Where(d => d.Id == id);


            var consultaDetalhes = await (
                from consulta in query
                join hospital in _context.Hospitais on consulta.HospitaisId equals hospital.Id into hG
                from hospital in hG.DefaultIfEmpty()
                join utente in _context.Utentes on consulta.UtentesId equals utente.Id into uG
                from utente in uG.DefaultIfEmpty()
                join funcionario in _context.Funcionarios on consulta.FuncionariosId equals funcionario.FuncionarioID into fG
                from funcionario in fG.DefaultIfEmpty()
                join responsavel in _context.Responsaveis on consulta.ResponsaveisId equals responsavel.Id into rG
                from responsavel in rG.DefaultIfEmpty()
                select new
                {
                    Id = consulta.Id,
                    HospitalId = consulta.HospitaisId,
                    Hospital = hospital.Nome,
                    FuncionarioId = consulta.FuncionariosId,
                    Funcionario = funcionario.Nome,
                    UtenteId = consulta.UtentesId,
                    Utente = utente.Nome,
                    ResponsavelId = consulta.ResponsaveisId,
                    Responsavel = responsavel.Nome,
                    Data = consulta.Data,
                    Descricao = consulta.Descricao
                }
            ).ToListAsync();

            return Ok(consultaDetalhes);
        }


        // Metodo para registar uma nova consulta, e seja com funcionario ou responsavel a acompanhar
        [HttpPost]
        public async Task<IActionResult> RegistarConsulta([FromBody] Consulta consulta)
        {
            if (consulta.ResponsaveisId != null)
            {
                var responsavel = await _context.Responsaveis.FindAsync(consulta.ResponsaveisId);
                if (responsavel == null)
                {
                    return NotFound($"Responsável com o ID {consulta.ResponsaveisId} não encontrado");
                }
                var canAttendResult = await CanAttendConsulta(consulta.ResponsaveisId, consulta.Data);
                if (canAttendResult is OkObjectResult)
                {
                    _context.Consultas.Add(consulta);
                    await _context.SaveChangesAsync();
                    return Ok("O responsável pode comparecer à consulta. Não é necessário nomear um funcionário.");
                }
                else
                {
                    return Ok("O responsável não pode comparecer à consulta. É necessário nomear um funcionário.");
                }
            }
            else if (consulta.FuncionariosId != null)
            {
                var funcionario = await _context.Funcionarios.FindAsync(consulta.FuncionariosId);
                if (funcionario == null)
                {
                    return NotFound($"Funcionário com o ID {consulta.FuncionariosId} não encontrado");
                }
                _context.Consultas.Add(consulta);
                await _context.SaveChangesAsync();
                return Ok("Consulta criada com um funcionário nomeado como acompanhante.");
            }
            else
            {
                _context.Consultas.Add(consulta);
                await _context.SaveChangesAsync();
                return Ok("Consulta criada sem acompanhante.");
            }
        }



        // Metodo para atualizar os dados de uma consulta
        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarConsulta(int id, [FromBody] Consulta novaConsulta)
        {
            var consulta = await _context.Consultas.FindAsync(id);

            if (consulta == null)
            {
                return NotFound($"Não foi possível encontrar a consulta com o ID {id}");
            }

            consulta.Data = novaConsulta.Data;
            consulta.Descricao = novaConsulta.Descricao;
            consulta.HospitaisId = novaConsulta.HospitaisId;
            consulta.UtentesId = novaConsulta.UtentesId;
            consulta.FuncionariosId = novaConsulta.FuncionariosId;
            consulta.ResponsaveisId = novaConsulta.ResponsaveisId;

            try
            {
                await _context.SaveChangesAsync();
                return Ok($"Consulta atualizada com sucesso para o ID {id}");
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        // Metodo para remover uma consulta
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoverConsulta(int id)
        {
            var consulta = await _context.Consultas.FindAsync(id);

            if (consulta == null)
            {
                return NotFound($"Consulta com o ID {id} não encontrada");
            }

            _context.Consultas.Remove(consulta);
            await _context.SaveChangesAsync();

            return Ok($"Consulta com o ID {id} removida com sucesso");
        }



        //Metodo para acompanhante
        [HttpGet("CanAttendConsulta")]
        public async Task<IActionResult> CanAttendConsulta(int? responsavelId, DateTime? consultaData)
        {          
            var outraConsulta = await _context.Consultas
                .FirstOrDefaultAsync(c => c.ResponsaveisId == responsavelId && c.Data == consultaData);

            if (outraConsulta != null)
            {
                return Ok("O responsável não pode comparecer à consulta.");
            }
            else
            {
                return Ok("O responsável pode comparecer à consulta");
            }
        }


      

    }
}
