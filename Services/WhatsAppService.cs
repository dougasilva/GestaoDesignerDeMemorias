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

        public async Task<(string Resposta, int? ProjetoId)> ProcessarMensagemAsync(string whatsapp, string mensagemTexto)
        {
            mensagemTexto = mensagemTexto.ToLower().Trim();

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
                NomeEvento = $"Pedido via WhatsApp - {DateTime.Now:dd/MM}",
                TipoProjeto = DetectarTipoProjeto(mensagemTexto),
                Status = "Novo",
                DataCriacao = DateTime.UtcNow
            };

            _context.Projetos.Add(projeto);
            await _context.SaveChangesAsync();

            string respostaAutomatica = GerarRespostaAutomatica(mensagemTexto, projeto.Id, cliente.Nome);

            // Após criar o projeto...
            var sender = new WhatsAppSenderService(); // vamos injetar depois
            await sender.EnviarMensagemAsync(whatsapp, respostaAutomatica);

            Console.WriteLine($"📨 Mensagem processada → Resposta enviada para {whatsapp}");

            return (respostaAutomatica, projeto.Id);
        }

        private string DetectarTipoProjeto(string msg)
        {
            if (msg.Contains("logo") || msg.Contains("marca") || msg.Contains("empresa") || msg.Contains("identidade"))
                return "Empreendedor";
            return "Evento";
        }

        private string GerarRespostaAutomatica(string msg, int projetoId, string nomeCliente)
        {
            if (msg.Contains("15") || msg.Contains("quinze") || msg.Contains("debutante") || msg.Contains("xv"))
            {
                return $"Olá! Que ótimo que você quer fazer o convite de 15 anos 💜\n\nPara eu já montar uma proposta linda pra você, me responde rapidinho:\n1. Nome da debutante?\n2. Data do evento?\n3. Tema ou cores preferidas?\n\nEnquanto isso, já criei um projeto (ID: {projetoId}) pra organizar tudo.";
            }

            if (msg.Contains("aniversario") || msg.Contains("aniversário"))
            {
                return $"Olá! 🎉 Adorei que você quer convite de aniversário!\nMe conta mais:\n- Nome da pessoa?\n- Idade / Data?\n- Tema?\n\nJá iniciei seu projeto (ID: {projetoId}).";
            }

            if (msg.Contains("logo") || msg.Contains("marca"))
            {
                return $"Olá! 💼 Entendi que você quer identidade visual / logo.\n\nMe fala:\n- Nome da empresa?\n- Segmento/área de atuação?\n- Alguma referência de estilo?\n\nJá criei o projeto (ID: {projetoId}) pra gente trabalhar.";
            }

            // Resposta padrão
            return $"Olá {nomeCliente}! 🫶 Agradeço o contato.\n\nEntendi que você quer artes digitais. Para eu te enviar uma proposta personalizada, me fala um pouco mais sobre o que precisa (convite, 15 anos, logo, etc.).\n\nEnquanto isso, criei um projeto (ID: {projetoId}) pra organizar seu atendimento.";
        }
    }
}