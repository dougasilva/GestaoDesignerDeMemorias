namespace GestaoDesignerDeMemorias.Models
{
    public class Proposta
    {
        public int Id { get; set; }

        public int ProjetoId { get; set; }
        public Projeto Projeto { get; set; } = null!;

        public string UrlPdf { get; set; } = string.Empty;
        public DateTime DataGeracao { get; set; } = DateTime.UtcNow;
        public bool Assinada { get; set; } = false;
    }
}