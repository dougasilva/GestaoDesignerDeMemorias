using GestaoDesignerDeMemorias.Data;
using GestaoDesignerDeMemorias.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoDesignerDeMemorias.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BriefingsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BriefingsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("projeto/{projetoId}")]
        public async Task<ActionResult<IEnumerable<Briefing>>> GetBriefingsByProjeto(int projetoId)
        {
            return await _context.Briefings
                .Where(b => b.ProjetoId == projetoId)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Briefing>> PostBriefing(Briefing briefing)
        {
            _context.Briefings.Add(briefing);
            await _context.SaveChangesAsync();

            // Atualiza status do projeto automaticamente
            var projeto = await _context.Projetos.FindAsync(briefing.ProjetoId);
            if (projeto != null && projeto.Status == "Novo")
                projeto.Status = "Briefing";

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBriefingsByProjeto), new { projetoId = briefing.ProjetoId }, briefing);
        }
    }
}