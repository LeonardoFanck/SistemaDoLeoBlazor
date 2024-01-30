using Microsoft.EntityFrameworkCore;
using SistemaDoLeoBlazor.API.Entities;

namespace SistemaDoLeoBlazor.API.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Operador>? Operador { get; set; }
        public DbSet<OperadorTela>? OperadorTela { get; set; }
        public DbSet<Tela>? Tela { get; set; }
        public DbSet<Categoria>? Categoria { get; set; }
        public DbSet<Cliente>? Cliente { get; set;}
        public DbSet<FormaPgto>? FormaPgto { get; set; }
        public DbSet<Pedido>? Pedido { get; set;}
        public DbSet<PedidoItem>? PedidoItem { get; set; }
        public DbSet<Produto>? Produto { get; set; }
        public DbSet<ProximoRegistro>? ProximoRegistro { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // OPERADOR
            modelBuilder.Entity<Operador>().HasData(new Operador{
                id = 1,
                nome = "Operador Padrão - Administrador",
                senha = "5555",
                admin = true,
                inativo = false
            });

            // TELAS
            modelBuilder.Entity<Tela>().HasData(new Tela
            {
                id = 1,
                nome = "Operador"
            });
            modelBuilder.Entity<Tela>().HasData(new Tela
            {
                id = 2,
                nome = "Categoria"
            });
            modelBuilder.Entity<Tela>().HasData(new Tela
            {
                id = 3,
                nome = "Cliente"
            });
            modelBuilder.Entity<Tela>().HasData(new Tela
            {
                id = 4,
                nome = "Forma Pagamento"
            });
            modelBuilder.Entity<Tela>().HasData(new Tela
            {
                id = 5,
                nome = "Pedido"
            });
            modelBuilder.Entity<Tela>().HasData(new Tela
            {
                id = 6,
                nome = "Produto"
            });

            // telas operador

            modelBuilder.Entity<OperadorTela>().HasData(new OperadorTela
            {
                id = 1,
                operadorId = 1,
                ativo = true,
                editar = true,
                excluir = true,
                novo = true,
                telaId = 1
            });

            modelBuilder.Entity<OperadorTela>().HasData(new OperadorTela
            {
                id = 2,
                operadorId = 1,
                ativo = true,
                editar = true,
                excluir = true,
                novo = true,
                telaId = 2
            });

            modelBuilder.Entity<OperadorTela>().HasData(new OperadorTela
            {
                id = 3,
                operadorId = 1,
                ativo = true,
                editar = true,
                excluir = true,
                novo = true,
                telaId = 3
            });

            modelBuilder.Entity<OperadorTela>().HasData(new OperadorTela
            {
                id = 4,
                operadorId = 1,
                ativo = true,
                editar = true,
                excluir = true,
                novo = true,
                telaId = 4
            });

            modelBuilder.Entity<OperadorTela>().HasData(new OperadorTela
            {
                id = 5,
                operadorId = 1,
                ativo = true,
                editar = true,
                excluir = true,
                novo = true,
                telaId = 5
            });

            modelBuilder.Entity<OperadorTela>().HasData(new OperadorTela
            {
                id = 6,
                operadorId = 1,
                ativo = true,
                editar = true,
                excluir = true,
                novo = true,
                telaId = 6
            });

            // PROXIMO REGISTRO
            modelBuilder.Entity<ProximoRegistro>().HasData(new ProximoRegistro
            {
                id = 1,
                categoria = 0,
                cliente = 0,
                formaPgto = 0,
                operador = 1,
                pedido = 0,
                produto = 0,
            });
        }
    }
}
