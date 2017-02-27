using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialog.Behoerdenloesung.Sitzungen.UI.Web.Models
{
    public interface IRepository
    {
        IEnumerable<TbGESSitzung> GetSitzungen(int benutzerId);
        TbGESSitzung GetSitzung(int sitzungId);

        IEnumerable<TbGESTraktanden> GetSitzungTraktanden(int sitzungId);
        SitzungTraktandModel GetTraktand(int traktandId);

        void UpdateComment(int traktandBenutzerId, string comment);
    }
}
