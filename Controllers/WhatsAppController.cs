using GestaoDesignerDeMemorias.Services;
using Microsoft.AspNetCore.Mvc;

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

        // Webhook principal - recebe mensagens
        [HttpPost("webhook")]
        public async Task<IActionResult> Webhook([FromBody] dynamic payload)
        {
            try
            {
                Console.WriteLine($"📨 Payload completo recebido: {payload}");

                // Tenta extrair número e mensagem (funciona com vários provedores)
                string whatsapp = ExtrairNumeroWhatsApp(payload);
                string mensagem = ExtrairMensagem(payload);

                if (string.IsNullOrEmpty(whatsapp) || string.IsNullOrEmpty(mensagem))
                {
                    return BadRequest("Não foi possível extrair número ou mensagem");
                }

                Console.WriteLine($"✅ Mensagem recebida de {whatsapp}: {mensagem}");

                var (resposta, projetoId) = await _whatsAppService.ProcessarMensagemAsync(whatsapp, mensagem);

                // TODO: Aqui vamos chamar o serviço de envio de mensagem de volta
                Console.WriteLine($"🤖 Resposta gerada: {resposta}");

                return Ok(new { status = "success", resposta, projetoId });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro no webhook: {ex.Message}");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // Método auxiliar para extrair número (funciona com Meta e Evolution API)
        private string ExtrairNumeroWhatsApp(dynamic payload)
        {
            try
            {
                // Meta WhatsApp Business API
                if (payload.entry != null)
                {
                    var changes = payload.entry[0].changes[0].value.messages[0];
                    string from = changes.from.ToString();
                    return from;
                }

                // Evolution API ou outros
                if (payload.from != null) return payload.from.ToString();
                if (payload.key?.remoteJid != null) return payload.key.remoteJid.ToString().Replace("@s.whatsapp.net", "");

                return "";
            }
            catch
            {
                return "";
            }
        }

        // Método auxiliar para extrair texto da mensagem
        private string ExtrairMensagem(dynamic payload)
        {
            try
            {
                if (payload.entry != null)
                {
                    var message = payload.entry[0].changes[0].value.messages[0];
                    return message.text?.body?.ToString() ?? "";
                }

                if (payload.text != null) return payload.text.ToString();
                if (payload.message?.conversation != null) return payload.message.conversation.ToString();

                return payload.ToString();
            }
            catch
            {
                return "";
            }
        }

        // Verificação do webhook (obrigatório para Meta/Evolution)
        [HttpGet("webhook")]
        public IActionResult VerifyWebhook([FromQuery] string hub_mode, [FromQuery] string hub_challenge, [FromQuery] string hub_verify_token)
        {
            // Coloque aqui seu token de verificação se usar Meta
            if (hub_mode == "subscribe")
                return Ok(hub_challenge);

            return Ok("Webhook configurado com sucesso!");
        }
    }
}