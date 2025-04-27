namespace MediBook.Web.Seeders;

using System;
using System.Linq;
using System.Threading.Tasks;
using MediBook.Core.DTOs;
using MediBook.Core.Enums;
using MediBook.Data.DataAccess;
using MediBook.Services.PatientAdministration;
using Microsoft.Extensions.DependencyInjection;

public class SeedPatientsData
{
    public static async Task Seed(IServiceProvider services, MediBookDatabaseContext dbContext)
    {
        var patientAdministrationService = services.GetRequiredService<IPatientAdministrationService>();

        //Seed initial data if the database is empty.
        if (!dbContext.Patients.Any())
        {
            for (var i = 1; i <= 50; i++)
            {
                var firstNames = new[] { "John", "Jane", "Michael", "Emily", "David", "Sophia", "Daniel", "Olivia", "James", "Emma" };
                var lastNames = new[] { "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez" };
                var firstName = firstNames[i % 10];
                var lastName = lastNames[i % 10];
                var patient = new PatientForRegistration
                {
                    Title = (Title)(i % Enum.GetValues(typeof(Title)).Length),
                    Firstname = firstName,
                    Lastname = lastName,
                    DateOfBirth = DateTime.UtcNow.AddYears(-(i > 30 ? 30 % i : i % 30)).AddMonths(-(i > 12 ? 12 % i : i % 12)).AddDays(i),
                    HealthAndCare = 1000000000L + i,
                    Address1 = $"Address Line 1 - {i}",
                    Address2 = $"Address Line 2 - {i}",
                    City = $"City{i}",
                    County = $"County{i}",
                    PostCode = $"PC{i:D4}",
                    PhoneNumber = $"123456789{i % 10}",
                    MobilePhone = $"987654321{i % 10}",
                    Email = $"{firstName.ToLower()}.{lastName.ToLower()}@medibook.com",
                    Status = (PatientStatus)(i % Enum.GetValues(typeof(PatientStatus)).Length)
                };

                await patientAdministrationService.RegisterPatientAsync(patient).ConfigureAwait(false);

            }
        }
    }
}
