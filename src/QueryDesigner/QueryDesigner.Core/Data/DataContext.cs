using System.Data.Entity;

namespace QueryDesigner.Core
{
    public class DataContext : DbContext
    {
        public DbSet<Connection> Connections { get; set; }
        public DbSet<User> Users { get; set; }


        public override int SaveChanges()
        {
            var changes = base.SaveChanges();
            return changes;
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}
