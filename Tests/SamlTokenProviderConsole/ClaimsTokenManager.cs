using System;
using System.Collections.Generic;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Linq;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;

namespace SamlTokenProviderConsole
{
    public class ClaimsTokenManager : ServiceCredentialsSecurityTokenManager
    {
        ClaimsProviderServiceCredentials claimsProviderServiceCredentials;

        public ClaimsTokenManager(ClaimsProviderServiceCredentials claimsProviderServiceCredentials)
            : base(claimsProviderServiceCredentials)
        {
            this.claimsProviderServiceCredentials = claimsProviderServiceCredentials;
        }

        public override SecurityTokenAuthenticator CreateSecurityTokenAuthenticator(SecurityTokenRequirement tokenRequirement, out SecurityTokenResolver outOfBandTokenResolver)
        {
            if (tokenRequirement.TokenType == SecurityTokenTypes.UserName)
            {
                outOfBandTokenResolver = null;
                return new ClaimsTokenAuthenticator();
            }
            else
            {
                return base.CreateSecurityTokenAuthenticator(tokenRequirement, out outOfBandTokenResolver);
            }
        }
    }
}
