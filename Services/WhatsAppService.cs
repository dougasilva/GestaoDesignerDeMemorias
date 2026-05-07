using GestaoDesignerDeMemorias.Data;
using GestaoDesignerDeMemorias.Models;
using Microsoft.EntityFrameworkCore;

namespace GestaoDesignerDeMemorias.Services
{
    public class WhatsAppService
    {
        private readonly AppDbContext _context;
        private readonly WhatsAppSenderService _senderService;

        public WhatsAppService(AppDbContext context, WhatsAppSenderService senderService)
        {
            _context = context;
            _senderService = senderService;
        }

        public async Task<string> ProcessarMensagemAsync(string whatsapp, string mensagemTexto)
        {
            var cliente = await GetOrCreateClienteAsync(whatsapp);
            var projeto = await GetOrCreateProjetoAsync(cliente.Id);

            // TODO: No futuro vamos salvar o estado da conversa por cliente
            string resposta = GerarRespostaGuiada(mensagemTexto, projeto, cliente);

            await _senderService.EnviarMensagemAsync(whatsapp, resposta);

            return resposta;
        }

        private async Task<Cliente> GetOrCreateClienteAsync(string whatsapp)
        {
            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.WhatsApp == whatsapp);
            if (cliente == null)
            {
                cliente = new Cliente 
                { 
                    Nome = "Novo Cliente", 
                    WhatsApp = whatsapp, 
                    DataCadastro = DateTime.UtcNow 
                };
                _context.Clientes.Add(cliente);
                await _context.SaveChangesAsync();
            }
            return cliente;
        }

        private async Task<Projeto> GetOrCreateProjetoAsync(int clienteId)
        {
            var projeto = await _context.Projetos
                .FirstOrDefaultAsync(p => p.ClienteId == clienteId && p.Status == "Novo");

            if (projeto == null)
            {
                projeto = new Projeto
                {
                    ClienteId = clienteId,
                    NomeEvento = "Pedido em andamento",
                    TipoProjeto = "Evento",
                    Status = "Briefing",
                    DataCriacao = DateTime.UtcNow
                };
                _context.Projetos.Add(projeto);
                await _context.SaveChangesAsync();
            }
            return projeto;
        }

        private string GerarRespostaGuiada(string msg, Projeto projeto, Cliente cliente)
        {
            msg = msg.ToLower();

            if (msg.Contains("15") || msg.Contains("quinze") || msg.Contains("debutante") || msg.Contains("xv"))
            {
                return "Perfeito! Vamos criar algo incrível para os 15 anos 💜\n\n" +
                       "Vou te fazer algumas perguntas para montar a proposta:\n\n" +
                       "1️⃣ Qual o nome completo da debutante?";
            }

            if (string.IsNullOrWhiteSpace(projeto.NomeEvento) || projeto.NomeEvento == "Pedido em andamento")
            {
                // Simplesmente salva o nome e pergunta a próxima
                return "Ótimo! Qual o nome completo da debutante?";
            }

            // Resposta padrão / fallback
            return "Entendi! Para te enviar uma proposta completa, preciso de mais alguns detalhes.\n\n" +
                   "Me fala:\n" +
                   "• Nome do evento / pessoa\n" +
                   "• Data aproximada\n" +
                   "• Tema ou cores preferidas?";
        }
    }
}