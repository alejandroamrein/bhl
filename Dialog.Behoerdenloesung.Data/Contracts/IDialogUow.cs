using Dialog.Behoerdenloesung.Model;

namespace Dialog.Behoerdenloesung.Data.Contracts
{
    /// <summary>
    /// Interface for the Code Camper "Unit of Work"
    /// </summary>
    public interface IDialogUow
    {
        // Save pending changes to the data store.
        void Commit();

        // Repositories
        IAdrPersonenRepository AdrPersonen { get; }
        IRepository<TbAFGAufgabeMutationen> AfgAufgabeMutationen { get; }
        IAfgAufgabenRepository AfgAufgaben { get; }
        IAfgAufgabeInternBeschreibungenRepository AfgAufgabeInternBeschreibungen { get; }
    }
}