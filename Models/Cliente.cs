using System.ComponentModel.DataAnnotations;

namespace GestaoDesignerDeMemorias.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; } = string.Empty;

        [Required]
        public string WhatsApp { get; set; } = string.Empty; // chave única

        public string Email { get; set; } = string.Empty;

        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;

        public List<Projeto> Projetos { get; set; } = new();
    }
}