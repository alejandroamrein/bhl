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

using DevExpress.Office.Utils;
using System.Collections.Generic;

namespace Dialog.Behoerdenloesung.Sitzungen.UI.Web.Models
{
    /// <summary>
    /// Home view model class
    /// </summary>
    public class HomeViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HomeViewModel"/> class.
        /// </summary>
        public HomeViewModel()
        {
            this.BenutzerId = 0;
            this.GremiumListe = new List<int>();
        }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        public int BenutzerId { get; set; }

        /// <summary>
        /// Gets or sets the suisse id nummer.
        /// </summary>
        /// <value>The suisse id nummer.</value>
        public List<int> GremiumListe { get; set; }
    }
}
