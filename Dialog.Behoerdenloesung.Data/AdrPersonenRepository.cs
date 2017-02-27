using System.Data.Entity;
using System.Linq;
using Dialog.Behoerdenloesung.Data.Contracts;
using Dialog.Behoerdenloesung.Model;

namespace Dialog.Behoerdenloesung.Data
{
    public class AdrPersonenRepository : EFRepository<TBADRPERSON>, IAdrPersonenRepository
    {
        public AdrPersonenRepository(DbContext context) : base(context) { }

        //public IQueryable<Speaker> GetSpeakers()
        //{
        //    return DbContext
        //        .Set<Session>()
        //        .Select(session => session.Speaker)
        //        .Distinct().Select(s =>
        //            new Speaker
        //                {    
        //                        Id = s.Id,
        //                        FirstName = s.FirstName,
        //                        LastName = s.LastName,
        //                        ImageSource = s.ImageSource,
        //                });
        //}
    }
}
