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

                // Extração segura
                string whatsapp = "11995108729"; // fallback
                string mensagem = "";

                // Tenta pegar "from" ou "phone"
                if (payload.TryGetProperty("from", out var fromElement))
                    whatsapp = fromElement.GetString() ?? whatsapp;

                if (payload.TryGetProperty("phone", out var phoneElement))
                    whatsapp = phoneElement.GetString() ?? whatsapp;

                // Tenta pegar a mensagem
                if (payload.TryGetProperty("text", out var textElement))
                    mensagem = textElement.GetString() ?? "";

                if (string.IsNullOrEmpty(mensagem) && payload.TryGetProperty("message", out var msgElement))
                    mensagem = msgElement.GetString() ?? payload.ToString();

                if (string.IsNullOrEmpty(mensagem))
                    mensagem = payload.ToString();

                Console.WriteLine($"✅ Extraído → WhatsApp: {whatsapp} | Mensagem: {mensagem}");

                var (resposta, projetoId) = await _whatsAppService.ProcessarMensagemAsync(whatsapp, mensagem);

                return Ok(new 
                { 
                    status = "success", 
                    whatsapp,
                    mensagem_recebida = mensagem,
                    resposta_enviada = resposta,
                    projetoId 
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