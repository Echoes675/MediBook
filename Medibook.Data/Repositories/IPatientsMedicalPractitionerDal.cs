﻿namespace MediBook.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MediBook.Core.Models;

    public interface IPatientsMedicalPractitionerDal
    {
        /// <summary>
        /// Method to Add an Entity to the database after confirming it doesn't already exist
        /// </summary>
        /// <param name="entity">The entity parameter</param>
        /// <returns></returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="entity"/> is <see langword="null"/></exception>
        Task<PatientsMedicalPractitioner> AddAsync(PatientsMedicalPractitioner entity);

        /// <summary>
        /// Method to delete an entity from the database if it exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Method to Update an Entity to the database after confirming it doesn't already exist
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="entity"/> is <see langword="null"/></exception>
        Task<PatientsMedicalPractitioner> UpdateAsync(PatientsMedicalPractitioner entity);

        /// <summary>
        /// Returns all the entities in a particular DbSet
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<PatientsMedicalPractitioner>> GetAllAsync();

        /// <summary>
        /// Returns an entity of a given type using its Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<PatientsMedicalPractitioner> GetEntityAsync(int id);

        /// <summary>
        /// Check if entity exists in the database
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="medicalPractitionerId"></param>
        /// <returns></returns>
        bool CheckEntityExistsAsync(int patientId, int medicalPractitionerId);

        /// <summary>
        /// Check if entity exists in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="entity"/> is <see langword="null"/></exception>
        bool CheckEntityExistsAsync(PatientsMedicalPractitioner entity);

        /// <summary>
        /// Check if entity exists in the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> CheckEntityExistsAsync(int id);

        /// <summary>
        /// Filter entities in a DbSet based on predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="predicate"/> is <see langword="null"/></exception>
        /// <exception cref="T:System.InvalidOperationException">Condition.</exception>
        IEnumerable<PatientsMedicalPractitioner> Filter(Func<PatientsMedicalPractitioner, bool> predicate);

        /// <summary>
        /// Filter entities in a DbSet Asynchronously based on predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="predicate"/> is <see langword="null"/></exception>
        /// <exception cref="T:System.InvalidOperationException">Condition.</exception>
        Task<IEnumerable<PatientsMedicalPractitioner>> FilterAsync(Func<PatientsMedicalPractitioner, bool> predicate);
    }
}