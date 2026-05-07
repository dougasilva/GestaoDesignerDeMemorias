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

        public async Task ProcessarMensagemAsync(string whatsapp, string mensagemTexto)
        {
            mensagemTexto = mensagemTexto.ToLower().Trim();

            var cliente = await GetOrCreateClienteAsync(whatsapp);
            var projeto = await GetOrCreateProjetoAsync(cliente.Id);

            string resposta = await GerarRespostaAsync(mensagemTexto, projeto, cliente);

            await _senderService.EnviarMensagemAsync(whatsapp, resposta);
        }

        private async Task<Cliente> GetOrCreateClienteAsync(string whatsapp)
        {
            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.WhatsApp == whatsapp);
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
            return cliente;
        }

        private async Task<Projeto> GetOrCreateProjetoAsync(int clienteId)
        {
            var projeto = await _context.Projetos
                .Where(p => p.ClienteId == clienteId && p.Status == "Novo")
                .FirstOrDefaultAsync();

            if (projeto == null)
            {
                projeto = new Projeto
                {
                    ClienteId = clienteId,
                    NomeEvento = $"Pedido WhatsApp - {DateTime.Now:dd/MM}",
                    TipoProjeto = "Evento",
                    Status = "Novo",
                    DataCriacao = DateTime.UtcNow
                };
                _context.Projetos.Add(projeto);
                await _context.SaveChangesAsync();
            }
            return projeto;
        }

        private async Task<string> GerarRespostaAsync(string msg, Projeto projeto, Cliente cliente)
        {
            if (msg.Contains("15") || msg.Contains("quinze") || msg.Contains("debutante") || msg.Contains("xv"))
            {
                return "Ótimo! Vamos fazer algo lindo para os 15 anos 💜\n\n" +
                       "Para eu começar a montar a proposta, me responde:\n\n" +
                       "1️⃣ Nome completo da debutante?\n" +
                       "2️⃣ Data do evento?\n" +
                       "3️⃣ Tema ou cores preferidas? (ou envie fotos de referência)\n\n" +
                       "Assim que você responder, já gero a proposta completa!";
            }

            // Resposta padrão por enquanto
            return "Olá! 🫶 Agradeço o contato.\n\n" +
                   "Me conta mais sobre o que você precisa (convite de 15 anos, aniversário, logo, etc.) que já te ajudo com tudo.";
        }
    }
}