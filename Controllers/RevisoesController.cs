using GestaoDesignerDeMemorias.Data;
using GestaoDesignerDeMemorias.Models;
using Microsoft.AspNetCore.Mvc;

namespace GestaoDesignerDeMemorias.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RevisoesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RevisoesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Revisao>> PostRevisao(Revisao revisao)
        {
            _context.Revisoes.Add(revisao);
            await _context.SaveChangesAsync();

            // Atualiza status do projeto
            var projeto = await _context.Projetos.FindAsync(revisao.ProjetoId);
            if (projeto != null) projeto.Status = "Revisao";

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRevisoesByProjeto", new { projetoId = revisao.ProjetoId }, revisao);
        }
    }
}