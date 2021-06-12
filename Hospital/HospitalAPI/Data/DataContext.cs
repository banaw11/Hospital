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
        public DbSet<Schedule> Schedules { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Employee>()
                .HasKey(e => e.Login);
          
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

            builder.Entity<Schedule>()
                .Property(s => s.Date)
                .IsRequired();

            builder.Entity<Schedule>()
                .Property(s => s.EmployeeLogin)
                .IsRequired();

            builder.Entity<Schedule>()
                .Property(s => s.Month)
                .IsRequired();

            builder.Entity<Schedule>()
                .HasOne(s => s.Employee)
                .WithMany(e => e.Schedules)
                .HasForeignKey(s => s.EmployeeLogin);




        }

    }
}
