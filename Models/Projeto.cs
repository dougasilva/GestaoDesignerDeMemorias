using System.ComponentModel.DataAnnotations;

namespace GestaoDesignerDeMemorias.Models
{
    public class Projeto
    {
        public int Id { get; set; }

        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; } = null!;

        public string NomeEvento { get; set; } = string.Empty;     // "XV Ana Júlia" ou "Logo - Loja da Maria"

        public DateTime? DataEvento { get; set; }

        [Required]
        public string TipoProjeto { get; set; } = "Evento";        // "Evento" ou "Empreendedor"

        public string Status { get; set; } = "Novo";               // Novo, Briefing, EmProducao, Revisao, Aprovado, Finalizado, Cancelado

        public decimal ValorTotal { get; set; } = 0;

        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        public List<ItemProjeto> Itens { get; set; } = new();
        public List<Briefing> Briefings { get; set; } = new();
        public List<Revisao> Revisoes { get; set; } = new();
        public List<Pagamento> Pagamentos { get; set; } = new();
        public List<ArquivoEntrega> Arquivos { get; set; } = new();
        public List<Proposta> Propostas { get; set; } = new();
    }
}