using GestaoDesignerDeMemorias.Data;
using GestaoDesignerDeMemorias.Models;
using Microsoft.AspNetCore.Mvc;

namespace GestaoDesignerDeMemorias.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagamentosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PagamentosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Pagamento>> PostPagamento(Pagamento pagamento)
        {
            _context.Pagamentos.Add(pagamento);
            await _context.SaveChangesAsync();

            // Atualiza status do projeto
            var projeto = await _context.Projetos.FindAsync(pagamento.ProjetoId);
            if (projeto != null)
            {
                if (pagamento.Tipo == "Sinal") projeto.Status = "EmProducao";
                if (pagamento.Tipo == "Final") projeto.Status = "Finalizado";
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPagamentosByProjeto", new { projetoId = pagamento.ProjetoId }, pagamento);
        }
    }
}