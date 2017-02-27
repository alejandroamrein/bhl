using System;
using System.Collections.Generic;
using System.IdentityModel.Selectors;
using System.Linq;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace SamlTokenProviderConsole
{
    public class ClaimsProviderServiceCredentials : ServiceCredentials
    {
        public ClaimsProviderServiceCredentials()
        : base()
        { }
        protected override ServiceCredentials CloneCore()
        {
            return new ClaimsProviderServiceCredentials();
        }
        public override SecurityTokenManager CreateSecurityTokenManager()
        {
            return new ClaimsTokenManager(this);
        }
    }
}
