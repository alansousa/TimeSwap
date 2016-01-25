using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSwap.Models
{
    public class Context : DbContext
    {
        public Context()
            : base("name=TimeSwapEntities")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }

        public DbSet<CARGO> CARGO { get; set; }
        public DbSet<FASE> FASE { get; set; }
        public DbSet<HABILIDADE> HABILIDADE { get; set; }
        public DbSet<PROJETO> PROJETO { get; set; }
        public DbSet<RECURSO> RECURSO { get; set; }
        public DbSet<RECURSOHABILIDADE> RECURSOHABILIDADE { get; set; }
        public DbSet<RECURSOTAREFA> RECURSOTAREFA { get; set; }
        public DbSet<TAREFA> TAREFA { get; set; }
    }
}
