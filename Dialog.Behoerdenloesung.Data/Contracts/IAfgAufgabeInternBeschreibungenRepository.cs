using System.Linq;
using Dialog.Behoerdenloesung.Data.Contracts;
using Dialog.Behoerdenloesung.Model;

namespace Dialog.Behoerdenloesung.Data.Contracts
{
    public interface IAfgAufgabeInternBeschreibungenRepository : IRepository<TbAFGAufgabeInternBeschreibung>
    {
        //IQueryable<Attendance> GetByPersonId(int id);
        //IQueryable<Attendance> GetBySessionId(int id);
        //Attendance GetByIds(int personId, int sessionId);
        //void Delete(int personId, int sessionId);
    }
}
