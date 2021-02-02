namespace MediBook.Data.DataAccess
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediBook.Core.Models;
    using Microsoft.EntityFrameworkCore;

    public class MediBookDatabaseContext : DbContext, IDatabaseContext
    {
        /// <summary>
        /// Initializes an instance of the <see cref="MediBookDatabaseContext"/> class
        /// </summary>
        /// <param name="options"></param>
        public MediBookDatabaseContext(DbContextOptions<MediBookDatabaseContext> options) 
            : base(options)
        {

        }

        /// <summary>
        /// The DbSet of Appointments table in the Database
        /// </summary>
        public DbSet<Appointment> Appointments { get; set; }

        /// <summary>
        /// The DbSet of AppointmentSessions table in the Database
        /// </summary>
        public DbSet<AppointmentSession> AppointmentSessions { get; set; }

        /// <summary>
        /// The DbSet of JobDescriptions table in the Database
        /// </summary>
        public DbSet<JobDescription> JobDescriptions { get; set; }

        /// <summary>
        /// The DbSet of Patients table in the Database
        /// </summary>
        public DbSet<Patient> Patients { get; set; }

        /// <summary>
        /// The DbSet of PatientNotes table in the Database
        /// </summary>
        public DbSet<PatientNote> PatientNotes { get; set; }

        /// <summary>
        /// The DbSet of PatientsMedicalPractitioners table in the Database
        /// </summary>
        public DbSet<PatientsMedicalPractitioner> PatientsMedicalPractitioners { get; set; }

        /// <summary>
        /// The DbSet of Employees table in the Database
        /// </summary>
        public DbSet<Employee> Employees { get; set; }

        /// <summary>
        /// The DbSet of Users table in the Database
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        ///     Creates a <see cref="DbSet{TEntity}" /> that can be used to query and save instances of <typeparamref name="TEntity" />.
        /// </summary>
        /// <typeparam name="TEntity"> The type of entity for which a set should be returned. </typeparam>
        /// <returns> A set for the given entity type. </returns>
        public new DbSet<TEntity> Set<TEntity>() where TEntity : class, IDbEntity => base.Set<TEntity>();

        /// <summary>
        /// The save changes async.
        /// </summary>
        /// <param name="cancellationToken">
        /// The cancellation Token.
        /// </param>
        /// <exception cref="T:Microsoft.EntityFrameworkCore.DbUpdateException">
        /// An error is encountered while saving to the database.
        /// </exception>
        /// <exception cref="T:Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException">
        /// A concurrency violation is encountered while saving to the database.
        ///                 A concurrency violation occurs when an unexpected number of rows are affected during save.
        ///                 This is usually because the data in the database has been modified since it was loaded into memory.
        /// </exception>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public new Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => base.SaveChangesAsync(cancellationToken);

        // Configure the context to use database. 
        /// <exception cref="T:System.ArgumentNullException"><paramref name="optionsBuilder"/> is <see langword="null"/></exception>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder == null)
            {
                throw new ArgumentNullException(nameof(optionsBuilder));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Set up relationships between tables
            modelBuilder.Entity<Appointment>()
                .HasOne(pt => pt.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(pt => pt.PatientId);

            modelBuilder.Entity<Appointment>()
                .HasOne(pt => pt.MedicalPractitioner)
                .WithMany(p => p.Appointments)
                .HasForeignKey(pt => pt.MedicalPractitionerId)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<Appointment>()
                .HasOne(pt => pt.AppointmentSession)
                .WithMany(p => p.Appointments)
                .HasForeignKey(pt => pt.AppointmentSessionId);

            modelBuilder.Entity<AppointmentSession>()
                .HasOne(pt => pt.MedicalPractitioner)
                .WithMany(p => p.AppointmentSessions)
                .HasForeignKey(pt => pt.MedicalPractitionerId);

            modelBuilder.Entity<PatientsMedicalPractitioner>()
                .HasOne(pt => pt.MedicalPractitioner)
                .WithMany(p => p.PatientsMedicalPractitioners)
                .HasForeignKey(pt => pt.MedicalPractitionerId);

            modelBuilder.Entity<PatientsMedicalPractitioner>()
                .HasOne(pt => pt.Patient)
                .WithMany(p => p.PatientsMedicalPractitioners)
                .HasForeignKey(pt => pt.PatientId);
        }
    }
}