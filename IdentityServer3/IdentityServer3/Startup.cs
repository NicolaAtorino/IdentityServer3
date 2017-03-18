using Owin;
using System.Collections.Generic;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services.InMemory;
using System;
using System.Security.Cryptography.X509Certificates;
using IdentityServer3.Core;

namespace Procredito.IdentityServer3
{
    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var options = new IdentityServerOptions
            {
                SigningCertificate = LoadCertificate(),
                Factory = new IdentityServerServiceFactory()
                            .UseInMemoryClients(Clients.Get())
                            .UseInMemoryScopes(Scopes.Get())
                            .UseInMemoryUsers(Users.Get()),

                RequireSsl = false
            };



            app.UseIdentityServer(options);
        }

        private X509Certificate2 LoadCertificate()
        {
            return new X509Certificate2(
            string.Format(@"{0}\idsrv3test.pfx", AppDomain.CurrentDomain.BaseDirectory), "idsrv3test");
        }
    }
}