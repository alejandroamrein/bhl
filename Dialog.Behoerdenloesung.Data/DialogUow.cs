using System;
using Dialog.Behoerdenloesung.Data.Contracts;
using Dialog.Behoerdenloesung.Data.Helpers;
using Dialog.Behoerdenloesung.Model;

namespace Dialog.Behoerdenloesung.Data
{
    public class DialogUow : IDialogUow, IDisposable
    {
        public DialogUow(IRepositoryProvider repositoryProvider)
        {
            CreateDbContext();

            repositoryProvider.DbContext = DbContext;
            RepositoryProvider = repositoryProvider;       
        }

        // Code Camper repositories

        public IRepository<TbAFGAufgabeMutationen> AfgAufgabeMutationen { get { return GetStandardRepo<TbAFGAufgabeMutationen>(); } }
        public IAdrPersonenRepository AdrPersonen { get { return GetRepo<IAdrPersonenRepository>(); } }
        public IAfgAufgabenRepository AfgAufgaben { get { return GetRepo<IAfgAufgabenRepository>(); } }
        public IAfgAufgabeInternBeschreibungenRepository AfgAufgabeInternBeschreibungen { get { return GetRepo<IAfgAufgabeInternBeschreibungenRepository>(); } }

        public void Commit()
        {
            //System.Diagnostics.Debug.WriteLine("Committed");
            DbContext.SaveChanges();
        }

        protected void CreateDbContext()
        {
            DbContext = new BehoerdenloesungEntities();

            // Do NOT enable proxied entities, else serialization fails
            DbContext.Configuration.ProxyCreationEnabled = false;

            // Load navigation properties explicitly (avoid serialization trouble)
            DbContext.Configuration.LazyLoadingEnabled = false;

            // Because Web API will perform validation, we don't need/want EF to do so
            DbContext.Configuration.ValidateOnSaveEnabled = false;

            //DbContext.Configuration.AutoDetectChangesEnabled = false;
            // We won't use this performance tweak because we don't need 
            // the extra performance and, when autodetect is false,
            // we'd have to be careful. We're not being that careful.
        }

        protected IRepositoryProvider RepositoryProvider { get; set; }

        private IRepository<T> GetStandardRepo<T>() where T : class
        {
            return RepositoryProvider.GetRepositoryForEntityType<T>();
        }
        private T GetRepo<T>() where T : class
        {
            return RepositoryProvider.GetRepository<T>();
        }

        private BehoerdenloesungEntities DbContext { get; set; }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (DbContext != null)
                {
                    DbContext.Dispose();
                }
            }
        }

        #endregion
    }
}