using GestaoDesignerDeMemorias.Data;
using GestaoDesignerDeMemorias.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoDesignerDeMemorias.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjetosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProjetosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/projetos (lista todos os projetos da Jana)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Projeto>>> GetProjetos()
        {
            return await _context.Projetos
                .Include(p => p.Cliente)
                .Include(p => p.Itens)
                .ToListAsync();
        }

        // POST: api/projetos (cria um novo pedido)
        [HttpPost]
        public async Task<ActionResult<Projeto>> PostProjeto(Projeto projeto)
        {
            _context.Projetos.Add(projeto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProjetos), new { id = projeto.Id }, projeto);
        }
    }
}