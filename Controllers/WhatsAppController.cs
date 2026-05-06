using GestaoDesignerDeMemorias.Data;
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

        [HttpPost("webhook")]
        public async Task<IActionResult> Webhook([FromBody] dynamic payload)
        {
            // Extrair dados reais (melhorar depois com provedor)
            string whatsapp = "11995108729"; // placeholder
            string mensagem = payload.ToString();

            var (resposta, projetoId) = await _whatsAppService.ProcessarMensagemAsync(whatsapp, mensagem);

            // Aqui vamos chamar o envio da mensagem de volta pro cliente
            Console.WriteLine($"🤖 Resposta automática: {resposta}");

            return Ok(new { status = "ok", resposta });
        }

        [HttpGet("webhook")]
        public IActionResult Verify([FromQuery] string hub_challenge = "")
        {
            return Ok(hub_challenge);
        }
    }
}