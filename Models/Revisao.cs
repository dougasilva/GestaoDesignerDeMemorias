namespace GestaoDesignerDeMemorias.Models
{
    public class Revisao
    {
        public int Id { get; set; }

        public int ProjetoId { get; set; }
        public Projeto Projeto { get; set; } = null!;

        public string Descricao { get; set; } = string.Empty;
        public string Status { get; set; } = "Pendente";
        public DateTime DataSolicitacao { get; set; } = DateTime.UtcNow;
    }
}