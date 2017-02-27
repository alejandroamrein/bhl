using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace DialogPortal.Controllers
{
    public class DocsController : ApiController
    {
        public async Task<string> Post()
        {
            CleanDocs();
            var bytes = await Request.Content.ReadAsByteArrayAsync();
            if (bytes.Length == 0)
            {
                return "";
            }
            var docsDir = HttpContext.Current.Server.MapPath("~/docs");
            var guid = System.Guid.NewGuid();
            var name = string.Format("TMP{0}.docx", guid);
            var fs = System.IO.File.Create(System.IO.Path.Combine(docsDir, name));
            using (fs)
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Url.Content("~/docs/" + name);
        }

        private void CleanDocs()
        {
            var docsDir = HttpContext.Current.Server.MapPath("~/docs");
            var datum = DateTime.Today.AddDays(-2);
            Task.Run(() => {
                var dir = new DirectoryInfo(docsDir);
                foreach (var fi in dir.GetFiles("*.docx"))
                {
                    if (fi.CreationTime < datum)
                    {
                        fi.Delete();
                    }
                }
            });
        }
    }
}
