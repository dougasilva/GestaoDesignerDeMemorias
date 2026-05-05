using System.ComponentModel.DataAnnotations;

namespace GestaoDesignerDeMemorias.Models
{
    public class Projeto
    {
        public int Id { get; set; }

        [Required]
        public int ClienteId { get; set; }
        
        public Cliente? Cliente { get; set; }   // ← Mudado para nullable

        public string NomeEvento { get; set; } = string.Empty;

        public DateTime? DataEvento { get; set; }

        [Required]
        public string TipoProjeto { get; set; } = "Evento";

        public string Status { get; set; } = "Novo";

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