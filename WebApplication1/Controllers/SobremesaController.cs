using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Conecta;

namespace WebApplication1.Controllers
{
    public class SobremesaController : Controller
    {

        private readonly SCMDbContext _context;

        public SobremesaController(SCMDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Sobremesas.FirstOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                return NotFound();
            }

            return Json(person); // Return JSON data
        }
    }
}
