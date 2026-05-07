using GestaoDesignerDeMemorias.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace GestaoDesignerDeMemorias.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WhatsAppController : ControllerBase
    {
        private readonly WhatsAppService _whatsAppService;

        public WhatsAppController(WhatsAppService whatsAppService)
        {
            _whatsAppService = whatsAppService;
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> Webhook([FromBody] JsonElement payload)
        {
            try
            {
                Console.WriteLine($"📨 Payload recebido: {payload}");

                string whatsapp = payload.TryGetProperty("from", out var fromEl) 
                    ? fromEl.GetString() ?? "11995108729" 
                    : "11995108729";

                string mensagem = payload.TryGetProperty("text", out var textEl) 
                    ? textEl.GetString() ?? "" 
                    : payload.ToString();

                Console.WriteLine($"✅ Extraído → WhatsApp: {whatsapp} | Mensagem: {mensagem}");

                // Processa e recebe a resposta gerada
                string respostaGerada = await _whatsAppService.ProcessarMensagemAsync(whatsapp, mensagem);

                return Ok(new 
                { 
                    status = "success", 
                    whatsapp,
                    mensagem_recebida = mensagem,
                    resposta_enviada = respostaGerada
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro: {ex.Message}");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("webhook")]
        public IActionResult Verify()
        {
            return Ok("Webhook configurado com sucesso!");
        }
    }
}