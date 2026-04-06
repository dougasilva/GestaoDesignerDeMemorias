namespace GestaoDesignerDeMemorias.Models
{
    public class ArquivoEntrega
    {
        public int Id { get; set; }

        public int ProjetoId { get; set; }
        public Projeto Projeto { get; set; } = null!;

        public string NomeArquivo { get; set; } = string.Empty;
        public string UrlGoogleDrive { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;           // PNG, PDF, MP4...
        public DateTime DataEntrega { get; set; } = DateTime.UtcNow;
    }
}