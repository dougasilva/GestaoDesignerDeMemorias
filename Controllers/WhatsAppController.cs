using GestaoDesignerDeMemorias.Data;
using GestaoDesignerDeMemorias.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoDesignerDeMemorias.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WhatsAppController : ControllerBase
    {
        private readonly AppDbContext _context;

        public WhatsAppController(AppDbContext context)
        {
            _context = context;
        }

        // Webhook - Recebe mensagens do WhatsApp
        [HttpPost("webhook")]
        public async Task<IActionResult> Webhook([FromBody] dynamic payload)
        {
            // Log da mensagem recebida (para debug)
            Console.WriteLine($"Mensagem WhatsApp recebida: {payload}");

            // TODO: Extrair número do cliente e texto da mensagem
            string whatsapp = "11999999999"; // placeholder - vamos melhorar depois
            string mensagem = payload.ToString(); // placeholder

            // Verifica se o cliente já existe
            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(c => c.WhatsApp == whatsapp);

            if (cliente == null)
            {
                cliente = new Cliente
                {
                    Nome = "Cliente WhatsApp",
                    WhatsApp = whatsapp,
                    DataCadastro = DateTime.UtcNow
                };
                _context.Clientes.Add(cliente);
                await _context.SaveChangesAsync();
            }

            // Cria um novo projeto automaticamente
            var projeto = new Projeto
            {
                ClienteId = cliente.Id,
                NomeEvento = "Novo Pedido via WhatsApp",
                TipoProjeto = "Evento",
                Status = "Novo",
                DataCriacao = DateTime.UtcNow
            };

            _context.Projetos.Add(projeto);
            await _context.SaveChangesAsync();

            return Ok(new { status = "Mensagem recebida", projetoId = projeto.Id });
        }

        // Endpoint de verificação (necessário para alguns provedores)
        [HttpGet("webhook")]
        public IActionResult VerifyWebhook([FromQuery] string hub_mode, [FromQuery] string hub_challenge)
        {
            if (hub_mode == "subscribe")
                return Ok(hub_challenge);

            return Ok();
        }
    }
}