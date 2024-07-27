using Microsoft.EntityFrameworkCore;

namespace APIPhonebook.Models
{
    public interface IAppDbContext : IDbContext
    {
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<SecurityQuestion> SecurityQuestions { get; set; }
    }
}
