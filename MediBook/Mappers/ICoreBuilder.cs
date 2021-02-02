//namespace MediBook.Core.Mappers
//{
//    using MediBook.Core.Models;

//    /// <summary>
//    /// Used to build classes of IDbEntity and relevant DTO and vice versa
//    /// </summary>
//    public interface ICoreBuilder
//    {
//        /// <summary>
//        /// Maps from DTO to class derived from IDbEntity
//        /// </summary>
//        /// <typeparam name="TInEntity"></typeparam>
//        /// <param name="entity"></param>
//        /// <returns></returns>
//        IDbEntity Map<TInEntity>(TInEntity entity) where TInEntity : class;

//        /// <summary>
//        /// Maps from class derived from IDbEntity to the appropriate DTO
//        /// </summary>
//        /// <typeparam name="TOutEntity"></typeparam>
//        /// <typeparam name="TInEntity"></typeparam>
//        /// <param name="entity"></param>
//        /// <returns></returns>
//        TOutEntity Map<TInEntity, TOutEntity>(TInEntity entity)
//            where TInEntity : class, IDbEntity
//            where TOutEntity : class;
//    }

//    /// <summary>
//    /// The Core Mapper Factory
//    /// </summary>
//    public class CoreBuilder : ICoreBuilder
//    {
//        public IDbEntity Map<TInEntity>(TInEntity entity) where TInEntity : class
//        {
//            throw new System.NotImplementedException();
//        }

//        public TOutEntity Map<TInEntity, TOutEntity>(TInEntity entity) where TInEntity : class, IDbEntity where TOutEntity : class
//        {
//            throw new System.NotImplementedException();
//        }
//    }

//    /// <summary>
//    /// The Core Builder Factory
//    /// </summary>
//    public interface ICoreBuilderFactory
//    {
//        /// <summary>
//        /// Gets the relevant builder for the specified types
//        /// </summary>
//        /// <typeparam name="TInEntity"></typeparam>
//        /// <typeparam name="TOutEntity"></typeparam>
//        /// <returns></returns>
//        ICoreBuilder GetBuilder<TInEntity, TOutEntity>() 
//            where TInEntity : class
//            where TOutEntity : class;
//    }

//    /// <summary>
//    /// The Core Builder Factory
//    /// </summary>
//    public class CoreBuilderFactory : ICoreBuilderFactory
//    {
//        /// <summary>
//        /// Gets the relevant builder for the specified types
//        /// </summary>
//        /// <typeparam name="TInEntity"></typeparam>
//        /// <typeparam name="TOutEntity"></typeparam>
//        /// <returns></returns>
//        public ICoreBuilder GetBuilder<TInEntity, TOutEntity>()
//        {
//            throw new System.NotImplementedException();
//        }
//    }
//}