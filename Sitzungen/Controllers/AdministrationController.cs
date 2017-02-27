using Dialog.Behoerdenloesung.Sitzungen.UI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Dialog.Behoerdenloesung.Sitzungen.UI.Web.Controllers
{
    public class AdministrationController : ApiController
    {
        private IRepository _Repository;

        public AdministrationController(IRepository repository)
        {
            _Repository = repository;
        }

        public AdministrationController()
        {
            _Repository = new EFRepository();
        }

        // GET api/Administration
        public IEnumerable<TbGESTraktanden> Get()
        {
            return _Repository.GetSitzungTraktanden(1);
        }

        // GET api/Administration
        public SitzungTraktandModel Get(int id)
        {
            return _Repository.GetTraktand(id);
        }

        // PUT api/Administration
        public void Put(int id, [FromBody] string stellungnahme)
        {
            _Repository.UpdateComment(id, stellungnahme);
        }

    }
}
