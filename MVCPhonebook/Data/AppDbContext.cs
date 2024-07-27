using Microsoft.EntityFrameworkCore;
using MVCPhonebook.Models;

namespace MVCPhonebook.Data
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<City> Cities {  get; set; }

        public EntityState GetEntryState<TEntity>(TEntity entity) where TEntity : class
        {
            return Entry(entity).State;
        }

        public void SetEntryState<TEntity>(TEntity entity, EntityState entityState) where TEntity : class
        {
            Entry(entity).State = entityState;
        }

    }
}
