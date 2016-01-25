using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using TimeSwap.Model;

namespace TimeSwap.Models
{
    public class Context : DbContext
    {
        public Context()
            : base("name=timeswap")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Properties().Where(p => p.Name == p.DeclaringType.Name + "ID")
                .Configure(p => p.IsKey())                ;
        }

        public DbSet<CARGO> CARGO { get; set; }
        public DbSet<FASE> FASE { get; set; }
        public DbSet<HABILIDADE> HABILIDADE { get; set; }
        public DbSet<PROJETO> PROJETO { get; set; }
        public DbSet<RECURSO> RECURSO { get; set; }
        public DbSet<RECURSOHABILIDADE> RECURSOHABILIDADE { get; set; }
        public DbSet<RECURSOTAREFA> RECURSOTAREFA { get; set; }
        public DbSet<TAREFA> TAREFA { get; set; }

        public DbSet<RecursoTarefaExecutado> RecursoTarefaExecutado { get; set; }
    }
}
