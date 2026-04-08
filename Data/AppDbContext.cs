using GestaoDesignerDeMemorias.Models;
using Microsoft.EntityFrameworkCore;

namespace GestaoDesignerDeMemorias.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // Todas as tabelas do nosso sistema
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Projeto> Projetos { get; set; }
        public DbSet<ItemProjeto> ItensProjeto { get; set; }
        public DbSet<Briefing> Briefings { get; set; }
        public DbSet<Revisao> Revisoes { get; set; }
        public DbSet<Pagamento> Pagamentos { get; set; }
        public DbSet<ArquivoEntrega> ArquivosEntrega { get; set; }
        public DbSet<Proposta> Propostas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurações de relacionamento (1:N)
            modelBuilder.Entity<Projeto>()
                .HasOne(p => p.Cliente)
                .WithMany(c => c.Projetos)
                .HasForeignKey(p => p.ClienteId);

            modelBuilder.Entity<ItemProjeto>()
                .HasOne(i => i.Projeto)
                .WithMany(p => p.Itens)
                .HasForeignKey(i => i.ProjetoId);

            modelBuilder.Entity<Briefing>()
                .HasOne(b => b.Projeto)
                .WithMany(p => p.Briefings)
                .HasForeignKey(b => b.ProjetoId);

            modelBuilder.Entity<Revisao>()
                .HasOne(r => r.Projeto)
                .WithMany(p => p.Revisoes)
                .HasForeignKey(r => r.ProjetoId);

            modelBuilder.Entity<Pagamento>()
                .HasOne(p => p.Projeto)
                .WithMany(p => p.Pagamentos)
                .HasForeignKey(p => p.ProjetoId);

            modelBuilder.Entity<ArquivoEntrega>()
                .HasOne(a => a.Projeto)
                .WithMany(p => p.Arquivos)
                .HasForeignKey(a => a.ProjetoId);

            modelBuilder.Entity<Proposta>()
                .HasOne(p => p.Projeto)
                .WithMany(p => p.Propostas)
                .HasForeignKey(p => p.ProjetoId);
        }
    }
}