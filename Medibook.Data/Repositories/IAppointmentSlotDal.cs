﻿namespace MediBook.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MediBook.Core.Models;

    public interface IAppointmentSlotDal
    {
        /// <summary>
        /// Method to Add an Entity to the database after confirming it doesn't already exist
        /// </summary>
        /// <param name="entity">The entity parameter</param>
        /// <returns></returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="entity"/> is <see langword="null"/></exception>
        Task<AppointmentSlot> AddAsync(AppointmentSlot entity);

        /// <summary>
        /// Method to delete an entity from the database if it exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Method to delete an entity from the database if it exists
        /// </summary>
        /// <param name="slot"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(AppointmentSlot slot);

        /// <summary>
        /// Method to Update an Entity to the database after confirming it exists
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="entity"/> is <see langword="null"/></exception>
        Task<AppointmentSlot> UpdateAsync(AppointmentSlot entity);

        /// <summary>
        /// Method to Update a list of AppointmentSlots after confirming they already exist
        /// </summary>
        /// <param name="appointmentSlots"></param>
        /// <returns></returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="appointmentSlots"/> is <see langword="null"/></exception>
        Task Update(List<AppointmentSlot> appointmentSlots);

        /// <summary>
        /// Returns all the entities in a particular DbSet
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<AppointmentSlot>> GetAllAsync();

        /// <summary>
        /// Returns an entity of a given type using its Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<AppointmentSlot> GetEntityAsync(int id);

        /// <summary>
        /// Check if entity exists in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="entity"/> is <see langword="null"/></exception>
        Task<bool> CheckEntityExistsAsync(AppointmentSlot entity);

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
        IEnumerable<AppointmentSlot> Filter(Func<AppointmentSlot, bool> predicate);

        /// <summary>
        /// Filter entities in a DbSet based on predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="predicate"/> is <see langword="null"/></exception>
        /// <exception cref="T:System.InvalidOperationException">Condition.</exception>
        Task<IEnumerable<AppointmentSlot>> FilterAsync(Func<AppointmentSlot, bool> predicate);

        /// <summary>
        /// GetPatientsAppointmentSlots
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns></returns>
        Task<List<AppointmentSlot>> GetPatientsAppointmentSlots(int patientId);
    }
}