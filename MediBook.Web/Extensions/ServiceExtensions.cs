namespace MediBook.Web.Extensions
{
    using MediBook.Data.Repositories;
    using MediBook.Services.AppointmentBook;
    using MediBook.Services.Cryptography;
    using MediBook.Services.PatientAdministration;
    using MediBook.Services.PatientRecord;
    using MediBook.Services.PatientRecord.Processors;
    using MediBook.Services.UserAdministration;
    using MediBook.Services.UserAuthentication;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Service Collection Extension methods
    /// </summary>
    public static class ServiceExtensions
    {
        public static void ConfigureRepositoryManager(this IServiceCollection services) =>
            services.AddScoped<IRepositoryManager, RepositoryManager>();

        /// <summary>
        /// Adds the Data Accessors to the Service Collection
        /// </summary>
        /// <param name="services"></param>
        public static void AddDataAccessors(this IServiceCollection services)
        {
            services.AddScoped<IJobDescriptionDal, JobDescriptionDal>();
            services.AddScoped<IAppointmentDal, AppointmentDal>();
            services.AddScoped<IAppointmentSlotDal, AppointmentSlotDal>();
            services.AddScoped<IAppointmentSessionDal, AppointmentSessionDal>();
            services.AddScoped<IPatientDal, PatientDal>();
            services.AddScoped<IPatientNoteDal, PatientNoteDal>();
            services.AddScoped<IUserDal, UserDal>();
            services.AddScoped<IEmployeeDal, EmployeeDal>();
        }

        /// <summary>
        /// Adds the Services to the Service Collection
        /// </summary>
        /// <param name="services"></param>
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<ICryptographyProcessorFactory, CryptographyProcessorFactory>();
            services.AddScoped<ICryptographyService, CryptographyService>();
            services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();
            services.AddScoped<IUserAdministrationService, UserAdministrationService>();
            services.AddScoped<IPatientAdministrationService, PatientAdministrationService>();
            services.AddScoped<IPatientNoteProcessor, PatientNoteProcessor>();
            services.AddScoped<IPatientRecordProcessorFactory, PatientRecordProcessorFactory>();
            services.AddScoped<IPatientRecordService, PatientRecordService>();
            //services.AddScoped<IAppointmentBookService, AppointmentBookService>();
        }
    }
}