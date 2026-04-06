namespace GestaoDesignerDeMemorias.Models
{
    public class Pagamento
    {
        public int Id { get; set; }

        public int ProjetoId { get; set; }
        public Projeto Projeto { get; set; } = null!;

        public decimal Valor { get; set; }
        public string Tipo { get; set; } = "Sinal";                // Sinal ou Final
        public string Status { get; set; } = "Pendente";
        public string PixId { get; set; } = string.Empty;
        public DateTime? DataPagamento { get; set; }
    }
}