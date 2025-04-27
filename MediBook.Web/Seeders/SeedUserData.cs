namespace MediBook.Web.Seeders;

using System;
using System.Linq;
using System.Threading.Tasks;
using MediBook.Core.DTOs;
using MediBook.Core.Enums;
using MediBook.Data.DataAccess;
using MediBook.Services.UserAdministration;
using Microsoft.Extensions.DependencyInjection;

public class SeedUserData
{
    public static async Task Seed(IServiceProvider services, MediBookDatabaseContext dbContext)
    {
        var userAdministrationService = services.GetRequiredService<IUserAdministrationService>();

        //Seed initial data if the database is empty.
        if (!dbContext.Users.Any())
        {
            await userAdministrationService.CreateUserAsync(new UserForRegistrationDto
            {
                Username = "jsmith",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                Title = Title.Dr,
                FirstName = "John",
                LastName = "Smith",
                JobDescription = "GP",
                Role = UserRole.MedicalPractitioner,
                State = AccountState.Active
            }).ConfigureAwait(false);

            await userAdministrationService.CreateUserAsync(new UserForRegistrationDto
            {
                Username = "ajones",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                Title = Title.Dr,
                FirstName = "Amy",
                LastName = "Jones",
                JobDescription = "Physio",
                Role = UserRole.MedicalPractitioner,
                State = AccountState.Active
            }).ConfigureAwait(false);

            await userAdministrationService.CreateUserAsync(new UserForRegistrationDto
            {
                Username = "rblack",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                Title = Title.Mr,
                FirstName = "Robert",
                LastName = "Black",
                JobDescription = "Dentist",
                Role = UserRole.MedicalPractitioner,
                State = AccountState.Active
            }).ConfigureAwait(false);

            await userAdministrationService.CreateUserAsync(new UserForRegistrationDto
            {
                Username = "fwilliams",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                Title = Title.Dr,
                FirstName = "Frank",
                LastName = "Williams",
                JobDescription = "GP",
                Role = UserRole.MedicalPractitioner,
                State = AccountState.Deleted
            }).ConfigureAwait(false);

            await userAdministrationService.CreateUserAsync(new UserForRegistrationDto
            {
                Username = "probinson",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                Title = Title.Mrs,
                FirstName = "Paula",
                LastName = "Robinson",
                JobDescription = "PracticeAdmin",
                Role = UserRole.PracticeAdmin,
                State = AccountState.Active
            }).ConfigureAwait(false);

            await userAdministrationService.CreateUserAsync(new UserForRegistrationDto
            {
                Username = "mprice",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                Title = Title.Mr,
                FirstName = "Mark",
                LastName = "Price",
                JobDescription = "PracticeAdmin",
                Role = UserRole.PracticeAdmin,
                State = AccountState.Deleted
            }).ConfigureAwait(false);

            await userAdministrationService.CreateUserAsync(new UserForRegistrationDto
            {
                Username = "tcorey",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                Title = Title.Mr,
                FirstName = "Trevor",
                LastName = "Corey",
                JobDescription = "PracticeAdmin",
                Role = UserRole.PracticeAdmin,
                State = AccountState.Inactive
            }).ConfigureAwait(false);

            await userAdministrationService.CreateUserAsync(new UserForRegistrationDto
            {
                Username = "tjames",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                Title = Title.Ms,
                FirstName = "Tracey",
                LastName = "James",
                JobDescription = "Receptionist",
                Role = UserRole.Reception,
                State = AccountState.Active
            }).ConfigureAwait(false);

            await userAdministrationService.CreateUserAsync(new UserForRegistrationDto
            {
                Username = "ewhite",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                Title = Title.Miss,
                FirstName = "Emily",
                LastName = "White",
                JobDescription = "Receptionist",
                Role = UserRole.Reception,
                State = AccountState.Deleted
            }).ConfigureAwait(false);

            await userAdministrationService.CreateUserAsync(new UserForRegistrationDto
            {
                Username = "ebrown",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                Title = Title.Mrs,
                FirstName = "Erica",
                LastName = "Brown",
                JobDescription = "Receptionist",
                Role = UserRole.Reception,
                State = AccountState.Inactive
            }).ConfigureAwait(false);

            await userAdministrationService.CreateUserAsync(new UserForRegistrationDto
            {
                Username = "joneil",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                Title = Title.Mr,
                FirstName = "Jim",
                LastName = "O'Neil",
                JobDescription = "Receptionist",
                Role = UserRole.Reception,
                State = AccountState.Inactive
            }).ConfigureAwait(false);
        }
    }
}