using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialog.Archivplan.UI.Web.Models
{
    public interface IRepository
    {
        IEnumerable<BegriffItem> GetDataByBegriff();
        IEnumerable<RegistraturItem> GetDataByRegistratur();
    }
}
