using DietetykAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;


public class AppDbContext : DbContext
{
 
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Employee> Employees { get; set; }

    public DbSet<Visit> Visits { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<MedicalRecomendations> MedicalRecommendations { get; set; }
    public DbSet<MedicalResult> MedicalResults
    {
        get; set;
    }

    public DbSet<Config> Config { get; set; }
    public DbSet<Diet> Diets { get; set; }
    

    public AppDbContext() { }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Visit>(entity =>
        {
            entity.HasIndex(v => new { v.EmployeeId, v.Date }, "ix_visits_employeeid_date");
        });

        modelBuilder.Entity<Employee>()
        .HasIndex(e => e.email)
        .IsUnique();


        modelBuilder.Entity<Visit>()
            .HasOne(v => v.Notification)
            .WithOne(n => n.Visit)
            .HasForeignKey<Notification>(n => n.NotificationId);

        modelBuilder.Entity<Visit>()
            .HasOne(v => v.Recomendation)
            .WithOne(n => n.Visit)
            .HasForeignKey<MedicalRecomendations>(n => n.MedicalRecomendationsId);

        modelBuilder.Entity<Visit>()
            .HasOne(v => v.Result)
            .WithOne(n => n.visit)
            .HasForeignKey<MedicalResult>(n => n.MedicalResultId);
    }

}