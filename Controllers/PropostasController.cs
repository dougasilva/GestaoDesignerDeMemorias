using GestaoDesignerDeMemorias.Data;
using GestaoDesignerDeMemorias.Models;
using GestaoDesignerDeMemorias.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoDesignerDeMemorias.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropostasController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly PropostaPdfService _pdfService;

        public PropostasController(AppDbContext context, PropostaPdfService pdfService)
        {
            _context = context;
            _pdfService = pdfService;
        }

        // POST: api/propostas/gerar/{projetoId}
        [HttpPost("gerar/{projetoId}")]
        public async Task<IActionResult> GerarProposta(int projetoId)
        {
            var projeto = await _context.Projetos
                .Include(p => p.Cliente)
                .Include(p => p.Itens)
                .FirstOrDefaultAsync(p => p.Id == projetoId);

            if (projeto == null || projeto.Cliente == null)
                return NotFound("Projeto ou cliente não encontrado.");

            // Gera o PDF
            byte[] pdfBytes = _pdfService.GerarProposta(projeto, projeto.Cliente);

            // Salva referência no banco
            var proposta = new Proposta
            {
                ProjetoId = projetoId,
                UrlPdf = $"propostas/projeto-{projetoId}.pdf", // futuro: salvar em pasta ou cloud
                DataGeracao = DateTime.UtcNow,
                Assinada = false
            };

            _context.Propostas.Add(proposta);
            await _context.SaveChangesAsync();

            // Retorna PDF para download
            return File(pdfBytes, "application/pdf", $"Proposta_{projeto.NomeEvento}.pdf");
        }
    }
}