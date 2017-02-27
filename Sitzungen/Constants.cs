// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Constants.cs" company="NEXPLORE AG">
//   Copyright (c) 2010 NEXPLORE AG
// </copyright>
// <summary>
//   Defines the Constants type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SuisseID.Wineshop.UI.Web
{
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>
    /// Defines the Constants type.
    /// </summary>
    public struct Constants
    {
        /// <summary>
        /// The session key of the current cashback controller action
        /// </summary>
        public const string CASHBACK_CURRENT_ACTION_SESSION_KEY = "CASHBACK_CURRENT_ACTION_KEY";

        /// <summary>
        /// The session key of the person info view model
        /// </summary>
        public const string CASHBACK_REFUNDREQUEST_SESSION_KEY = "CASHBACK_PERSON_INFO_KEY";

        /// <summary>
        /// The session key of the current certification hash
        /// </summary>
        public const string CASHBACK_CERT_HASH_SESSION_KEY = "CASHBACK_CERTHASH_KEY";

        /// <summary>
        /// Gets the valid lang.
        /// </summary>
        /// <value>The valid lang.</value>
        public static IEnumerable<CultureInfo> ValidCultures
        {
            get
            {
                return new List<CultureInfo>
                    {
                        new CultureInfo("de-CH"),
                        new CultureInfo("fr-CH"),
                        new CultureInfo("it-CH")
                    };
            }
        }
    }
}