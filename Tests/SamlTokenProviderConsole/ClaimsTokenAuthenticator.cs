using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;
using System.IdentityModel.Selectors;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SamlTokenProviderConsole
{
    public class ClaimsTokenAuthenticator : WindowsUserNameSecurityTokenAuthenticator
    {
        protected override ReadOnlyCollection<IAuthorizationPolicy> ValidateUserNamePasswordCore(string userName, string password)
        {
            if (IsAuthenticated(userName, password))
            {
                var name = userName.Split('\\')[1];
                var policies = new List<IAuthorizationPolicy>(1);
                var claimSet = new WindowsClaimSet(new WindowsIdentity(name), true);
                policies.Add(new SecurityTokenAuthorizationPolicy(claimSet));
                return policies.AsReadOnly();
            }
            return null;
        }
        private bool IsAuthenticated(string userName, string password)
        {
            //TODO: Call the provider to validate the credentials
            return (userName.Length == 3 && userName == password);
        }
    }
}
