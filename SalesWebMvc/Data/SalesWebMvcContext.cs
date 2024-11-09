using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Models;

namespace SalesWebMvc.Data
{
    public class SalesWebMvcContext : DbContext
    {
        public SalesWebMvcContext (DbContextOptions<SalesWebMvcContext> options)
            : base(options)
        {
        }

        public DbSet<SalesWebMvc.Models.Department> Department { get; set; } = default!;
        public DbSet<SalesWebMvc.Models.Seller> Seller { get; set; }
        public DbSet<SalesWebMvc.Models.SalesRecord> SalesRecord { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Seller>()
                .HasOne(s => s.Department)                // Propriedade de navegação
                .WithMany(d => d.Sellers)                 // Relacionamento inverso
                .HasForeignKey(s => s.DepartmentId)       // Chave estrangeira
                .OnDelete(DeleteBehavior.Restrict);        // Define comportamento de exclusão, se desejado

            // Configuração para SalesRecord (exemplo de relacionamento com Seller)
            modelBuilder.Entity<SalesRecord>()
                .HasOne(sr => sr.Seller)                // Supondo que SalesRecord tenha um Seller associado
                .WithMany(s => s.Sales)          // Supondo que Seller tenha uma coleção de SalesRecords
                .HasForeignKey(sr => sr.SellerId)       // Chave estrangeira de SalesRecord para Seller
                .OnDelete(DeleteBehavior.Restrict);     // Escolha o comportamento de exclusão apropriado
        }
    }
}
