using GestaoDesignerDeMemorias.Data;
using GestaoDesignerDeMemorias.Models;
using Microsoft.EntityFrameworkCore;

namespace GestaoDesignerDeMemorias.Services
{
    public class WhatsAppService
    {
        private readonly AppDbContext _context;

        public WhatsAppService(AppDbContext context)
        {
            _context = context;
        }

        public async Task ProcessarMensagemAsync(string whatsapp, string mensagemTexto)
        {
            mensagemTexto = mensagemTexto.ToLower();

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(c => c.WhatsApp == whatsapp);

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

            var projeto = new Projeto
            {
                ClienteId = cliente.Id,
                NomeEvento = $"Pedido via WhatsApp - {DateTime.Now:dd/MM/yyyy}",
                TipoProjeto = mensagemTexto.Contains("empresa") || mensagemTexto.Contains("logo") ? "Empreendedor" : "Evento",
                Status = "Novo",
                DataCriacao = DateTime.UtcNow
            };

            _context.Projetos.Add(projeto);
            await _context.SaveChangesAsync();

            Console.WriteLine($"📨 Novo lead processado! Tipo: {projeto.TipoProjeto} | Projeto ID: {projeto.Id}");

            // Aqui vamos adicionar respostas automáticas no próximo passo
        }
    }
}