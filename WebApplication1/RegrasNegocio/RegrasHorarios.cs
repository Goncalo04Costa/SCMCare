using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Modelos;
using System.Threading.Tasks;

namespace WebApplication1.RegrasNegocio
{
	public class RegrasHorario : ActionFilterAttribute
	{
		private readonly AppDbContext _context;

		public RegrasHorario(AppDbContext context)
		{
			_context = context;
		}

		public async Task<bool> HorarioEValido(Horario horario)
		{
			var horarioExistente = await _context.Horarios.FirstOrDefaultAsync(h =>
				h.FuncionariosId == horario.FuncionariosId &&
				h.Dia.Date == horario.Dia.Date &&
				h.TurnosId == horario.TurnosId);

			return horarioExistente == null;
		}

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			var tipoFuncionarioUsuario = 1;

			if (tipoFuncionarioUsuario != 1 && tipoFuncionarioUsuario != 2 && tipoFuncionarioUsuario != 3)
			{
				context.Result = new ForbidResult();
				return;
			}

			base.OnActionExecuting(context);
		}
	}
}
