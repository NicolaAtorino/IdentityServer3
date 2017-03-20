using IdentityServer3.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Procredito.IdentityServer3
{
    public static class Scopes
    {
        public static List<Scope> Get()
        {
            var scopes = new List<Scope>
            {
                new Scope
                {
                    Name = "Api2",
                    Enabled = true,
                    DisplayName = "Api2",
                    Description = "Access to Api2",
                    Type = ScopeType.Resource,
                    //Claims = new List<ScopeClaim>
                    //{
                    //    new ScopeClaim("role")
                    //},
                    IncludeAllClaimsForUser = true,
                },
                new Scope
                {
                    Enabled = true,
                    Name = "roles",
                    Type = ScopeType.Identity,
                    
                    Claims = new List<ScopeClaim>
                    {
                        new ScopeClaim("role",true)
                    }
                }
            };

            scopes.AddRange(StandardScopes.AllAlwaysInclude);
            return scopes;
        }
    }
}
