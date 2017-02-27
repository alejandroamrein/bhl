using System;
using System.Collections.Generic;
using System.Data.Entity;
using Dialog.Behoerdenloesung.Data.Contracts;

namespace Dialog.Behoerdenloesung.Data.Helpers
{
    public class RepositoryFactories
    {
        private IDictionary<Type, Func<DbContext, object>> GetDialogFactories()
        {
            return new Dictionary<Type, Func<DbContext, object>>
                {
                   {typeof(IAdrPersonenRepository), dbContext => new AfgAufgabenRepository(dbContext)},
                   {typeof(IAfgAufgabenRepository), dbContext => new AfgAufgabenRepository(dbContext)},
                   {typeof(IAfgAufgabeInternBeschreibungenRepository), dbContext => new AfgAufgabeInternBeschreibungenRepository(dbContext)}
                };
        }

        public RepositoryFactories()  
        {
            _repositoryFactories = GetDialogFactories();
        }

        public RepositoryFactories(IDictionary<Type, Func<DbContext, object>> factories )
        {
            _repositoryFactories = factories;
        }

        public Func<DbContext, object> GetRepositoryFactory<T>()
        {       
            Func<DbContext, object> factory;
            _repositoryFactories.TryGetValue(typeof(T), out factory);
            return factory;
        }

        public Func<DbContext, object> GetRepositoryFactoryForEntityType<T>() where T : class
        {
            return GetRepositoryFactory<T>() ?? DefaultEntityRepositoryFactory<T>();
        }

        protected virtual Func<DbContext, object> DefaultEntityRepositoryFactory<T>() where T : class
        {
            return dbContext => new EFRepository<T>(dbContext);
        }

        private readonly IDictionary<Type, Func<DbContext, object>> _repositoryFactories;
    }
}
