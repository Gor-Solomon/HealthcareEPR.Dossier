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

// Relaxed CORS for debugging
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
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

// Seed Data for Testing
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DossierDbContext>();
    var dossierId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6");
    
    if (!context.Dossiers.Any())
    {
        var patient = new Patient(Guid.NewGuid(), "John", "Doe", new DateTime(1980, 5, 15));
        var dossier = new PatientDossier(dossierId, patient.Id);
        
        context.Patients.Add(patient);
        context.Dossiers.Add(dossier);
        context.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// REMOVED UseHttpsRedirection for local debugging to avoid certificate/redirect issues
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();
