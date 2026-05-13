using HealthcareEPR.Dossier.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HealthcareEPR.Dossier.Infrastructure.Persistence;

public class DossierDbContext : DbContext
{
    public DossierDbContext(DbContextOptions<DossierDbContext> options) : base(options) { }

    public DbSet<PatientDossier> Dossiers => Set<PatientDossier>();
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<SessionNote> SessionNotes => Set<SessionNote>();
    public DbSet<TreatmentPlan> TreatmentPlans => Set<TreatmentPlan>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PatientDossier>().HasKey(d => d.Id);
        modelBuilder.Entity<Patient>().HasKey(p => p.Id);
        modelBuilder.Entity<SessionNote>().HasKey(s => s.Id);
        modelBuilder.Entity<TreatmentPlan>().HasKey(t => t.Id);
    }
}
