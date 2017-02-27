using System;
using System.Collections.Generic;
using System.Text;
using System.DirectoryServices;
using System.Security.Principal;
using System.Threading;

namespace pem.pemTime.Services
{
    internal enum GrpType : uint
    {
        UnivGrp = 0x08,
        DomLocalGrp = 0x04,
        GlobalGrp = 0x02,
        SecurityGrp = 0x80000000
    }

    public class ADHelper
    {
        public static void CreateGroup(string GroupName, string Description)
        {
            SearchResult result = null;
            DirectoryEntry ent = new DirectoryEntry("LDAP://RootDSE");
            String str = (String)ent.Properties["defaultNamingContext"][0];
            DirectoryEntry deAD = new DirectoryEntry("LDAP://" + str);
            GrpType gt = GrpType.GlobalGrp | GrpType.SecurityGrp;
            int typeNum = (int)gt;
            DirectorySearcher mySearcher = new DirectorySearcher(deAD);

            // Find group
            mySearcher.Filter = "(&(objectClass=group)(sAMAccountName=" + GroupName + "))";
            result = mySearcher.FindOne();
            if (result == null)
            {
                // Group not exists -> create it!
                DirectoryEntry cn = deAD.Children.Find("cn=Users");
                DirectoryEntry group = cn.Children.Add("cn=" + GroupName, "group");
                group.Properties["sAMAccountName"].Add(GroupName);
                group.Properties["description"].Add(Description);
                group.Properties["groupType"].Add(typeNum);
                group.CommitChanges();
                group.Close();
            }
        }

        public static void AddCurrentUserToGroup(string GroupName)
        {
            SearchResult result = null;
            DirectoryEntry ent = new DirectoryEntry("LDAP://RootDSE");
            String str = (String)ent.Properties["defaultNamingContext"][0];
            DirectoryEntry deAD = new DirectoryEntry("LDAP://" + str);
            DirectorySearcher mySearcher = new DirectorySearcher(deAD);

            // Find group
            mySearcher.Filter = "(&(objectClass=group)(sAMAccountName=" + GroupName + "))";
            result = mySearcher.FindOne();
            if (result == null)
            {
                throw new NullReferenceException
                ("unable to locate the distinguishedName for the object group in the domain");
            }
            DirectoryEntry group = result.GetDirectoryEntry();

            AppDomain myDomain = Thread.GetDomain();
            myDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
            WindowsPrincipal prin = Thread.CurrentPrincipal as WindowsPrincipal;
            string userName = prin.Identity.Name.Substring(prin.Identity.Name.LastIndexOf('\\') + 1);
            string domain = prin.Identity.Name.Substring(0, prin.Identity.Name.LastIndexOf('\\'));

            // Find user
            mySearcher.Filter = "(&(objectClass=user)(sAMAccountName=" + userName + "))";
            result = mySearcher.FindOne();
            if (result == null)
            {
                throw new NullReferenceException
                ("unable to locate the distinguishedName for the object user in the domain");
            }
            DirectoryEntry user = result.GetDirectoryEntry();

            // Add user to group
            string cnUser = user.Path.Substring(user.Path.ToUpper().IndexOf("CN="));
            if (!group.Properties["Member"].Contains(cnUser))
            {
                group.Properties["member"].Add(cnUser);
                group.CommitChanges();
                group.Close();
            }
        }
    }
}
