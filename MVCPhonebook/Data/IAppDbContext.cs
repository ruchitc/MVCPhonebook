using Microsoft.EntityFrameworkCore;
using MVCPhonebook.Models;

namespace MVCPhonebook.Data
{
    public interface IAppDbContext : IDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<City> Cities { get; set; }
    }
}
