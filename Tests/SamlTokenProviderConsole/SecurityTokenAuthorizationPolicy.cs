using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamlTokenProviderConsole
{
    public class SecurityTokenAuthorizationPolicy : IAuthorizationPolicy
    {
        WindowsClaimSet claims;
        public SecurityTokenAuthorizationPolicy(WindowsClaimSet claims)
        {
            this.claims = claims;
        }

        public string Id
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public ClaimSet Issuer
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool Evaluate(EvaluationContext evaluationContext, ref object state)
        {
            evaluationContext.AddClaimSet(this, claims);
            evaluationContext.RecordExpirationTime(DateTime.Now.AddHours(10));
            return true;
        }
    }
}
