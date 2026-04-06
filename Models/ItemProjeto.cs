namespace GestaoDesignerDeMemorias.Models
{
    public class ItemProjeto
    {
        public int Id { get; set; }

        public int ProjetoId { get; set; }
        public Projeto Projeto { get; set; } = null!;

        public string Nome { get; set; } = string.Empty;           // "Convite Infinity Plus", "Brasão", etc.
        public string Descricao { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public bool Entregue { get; set; } = false;
    }
}