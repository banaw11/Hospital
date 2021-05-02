using HospitalAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace HospitalAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Employee>()
                .HasKey(e => e.PersonalId);
            builder.Entity<Employee>()
                .Property(e => e.PersonalId)
                .IsRequired();
            builder.Entity<Employee>()
                .Property(e => e.Login)
                .IsRequired();
            builder.Entity<Employee>()
                .Property(e => e.PasswordHash)
                .IsRequired();
            builder.Entity<Employee>()
                .Property(e => e.Profession)
                .IsRequired();


        }

    }
}
