namespace GestaoDesignerDeMemorias.Models
{
    public class Briefing
    {
        public int Id { get; set; }

        public int ProjetoId { get; set; }
        public Projeto Projeto { get; set; } = null!;

        public string Pergunta { get; set; } = string.Empty;
        public string Resposta { get; set; } = string.Empty;
        public DateTime DataResposta { get; set; } = DateTime.UtcNow;
        public string TipoEvento { get; set; } = string.Empty;
    }
}