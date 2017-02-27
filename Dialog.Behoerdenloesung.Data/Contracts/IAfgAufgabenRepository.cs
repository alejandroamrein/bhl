using System.Collections.Generic;
using System.Linq;
using Dialog.Behoerdenloesung.Data.Contracts;
using Dialog.Behoerdenloesung.Model;

namespace Dialog.Behoerdenloesung.Data.Contracts
{
    public interface IAfgAufgabenRepository : IRepository<TbAFGAufgabe>
    {
        //IQueryable<SessionBrief> GetSessionBriefs();
        //IEnumerable<TagGroup> GetTagGroups();
    }
}
