using HealthcareEPR.Dossier.Domain.Entities;
using HealthcareEPR.Dossier.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HealthcareEPR.Dossier.IntegrationTests;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    public Guid TestDossierId { get; } = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6");

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove the existing DbContext registration
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<DossierDbContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Add an In-Memory database for testing
            services.AddDbContext<DossierDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDbForTesting");
            });

            // Seed the database with a test Patient and Dossier
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<DossierDbContext>();
            var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory<TProgram>>>();

            db.Database.EnsureCreated();

            try
            {
                SeedData(db);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding the database with test messages. Error: {Message}", ex.Message);
            }
        });
    }

    private void SeedData(DossierDbContext db)
    {
        if (!db.Dossiers.Any(d => d.Id == TestDossierId))
        {
            var patient = new Patient(Guid.NewGuid(), "Test", "Patient", new DateTime(1990, 1, 1));
            var dossier = new PatientDossier(TestDossierId, patient.Id);

            db.Patients.Add(patient);
            db.Dossiers.Add(dossier);
            db.SaveChanges();
        }
    }
}
