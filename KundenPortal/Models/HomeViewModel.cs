// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HomeViewModel.cs" company="NEXPLORE AG">
//    Copyright (c) 2010 NEXPLORE AG
// </copyright>
// <license>
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//      http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using System.Linq;
using DevExpress.Office.Utils;
using System.Collections.Generic;

namespace Dialog.Behoerdenloesung.Sitzungen.UI.Web.Models
{
    /// <summary>
    /// Home view model class
    /// </summary>
    public class HomeViewModel
    {
        public int SysUsrId { get; set; }
        public int PersonId { get; set; }
        public List<TbBHDGremium> GremiumListe { get; set; }
        public string Shortname { get; set; }
        public string Fullname { get; set; }
        public string Module { get; set; }

        public HomeViewModel()
        {
            this.SysUsrId = 0;
            this.PersonId = 0;
            this.GremiumListe = new List<TbBHDGremium>();
            this.Shortname = "";
            this.Fullname = "";
            this.Module = "";
        }

        public override string ToString()
        {
            string tmp = "{";
            for (var i = 0; i < GremiumListe.Count; i++)
            {
                if (tmp != "{")
                {
                    tmp += ",";
                }
                tmp += GremiumListe[i].TbBHDGremium_id.ToString();
            }
            tmp += "}";
            return string.Format("Shortname={0} SysUsrId={1} PersonId={2} GremiumListe={3}", Shortname, SysUsrId, PersonId, tmp);
        }

        public bool HasModule(string moduleName)
        {
            return Module.Contains(moduleName);
        }
    }
}
