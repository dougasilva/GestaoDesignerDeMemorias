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
            // Busca ou cria o cliente
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

            // Cria um novo projeto
            var projeto = new Projeto
            {
                ClienteId = cliente.Id,
                NomeEvento = $"Pedido via WhatsApp - {DateTime.Now:dd/MM}",
                TipoProjeto = "Evento",
                Status = "Novo",
                DataCriacao = DateTime.UtcNow
            };

            _context.Projetos.Add(projeto);
            await _context.SaveChangesAsync();

            Console.WriteLine($"✅ Novo lead processado! Projeto ID: {projeto.Id} | Cliente: {whatsapp}");
            // Aqui vamos adicionar depois o envio de resposta automática
        }
    }
}