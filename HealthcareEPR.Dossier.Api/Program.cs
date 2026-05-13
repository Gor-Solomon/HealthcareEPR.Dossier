using HealthcareEPR.Dossier.Application.Common.Interfaces;
using HealthcareEPR.Dossier.Application.Dossiers.Commands.AddSessionNote;
using HealthcareEPR.Dossier.Domain.Entities;
using HealthcareEPR.Dossier.Infrastructure.Persistence;
using HealthcareEPR.Dossier.Infrastructure.Persistence.Repositories;
using HealthcareEPR.Dossier.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// S5122: Restricted CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("FmscaPolicy",
        policy => policy.WithOrigins("https://fmsca-solution.vercel.app")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

// Infrastructure
builder.Services.AddDbContext<DossierDbContext>(options =>
    options.UseInMemoryDatabase("DossierDb"));

builder.Services.AddScoped<IDossierRepository, DossierRepository>();
builder.Services.AddScoped<IAiSummarizationService, AiSummarizationService>();

// Application
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<AddSessionNoteCommand>());

var app = builder.Build();

// S6966: Await async calls in top-level statements
// Seed Data for Testing
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DossierDbContext>();
    var dossierId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6");

    // S6966: Use AnyAsync and await
    if (!await context.Dossiers.AnyAsync())
    {
        // S6562: Provide DateTimeKind.Utc
        var patient = new Patient(Guid.NewGuid(), "John", "Doe", new DateTime(1980, 5, 15, 0, 0, 0, DateTimeKind.Utc));
        var dossier = new PatientDossier(dossierId, patient.Id);

        context.Patients.Add(patient);
        context.Dossiers.Add(dossier);

        // S6966: Use SaveChangesAsync and await
        await context.SaveChangesAsync();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("FmscaPolicy");
app.UseAuthorization();
app.MapControllers();

// S6966: Use RunAsync and await
await app.RunAsync();

// Make the implicit Program class public so test projects can access it
public partial class Program
{
    // S1118: Add a protected constructor to satisfy SonarCloud
    protected Program() { }
}