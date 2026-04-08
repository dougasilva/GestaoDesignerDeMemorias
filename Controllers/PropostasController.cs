using GestaoDesignerDeMemorias.Data;
using GestaoDesignerDeMemorias.Models;
using Microsoft.AspNetCore.Mvc;

namespace GestaoDesignerDeMemorias.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropostasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PropostasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Proposta>> PostProposta(Proposta proposta)
        {
            _context.Propostas.Add(proposta);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPropostasByProjeto", new { projetoId = proposta.ProjetoId }, proposta);
        }
    }
}