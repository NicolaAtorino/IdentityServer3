using Microsoft.Owin;
using Owin;
using System.Web.Http;
using IdentityServer3.AccessTokenValidation;
using System.Collections.Generic;
using System.IdentityModel.Tokens;

[assembly: OwinStartup(typeof(Procredito.Api2.Startup))]
namespace Procredito.Api2
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //AntiForgeryConfig.UniqueClaimTypeIdentifier = Constants.ClaimTypes.Subject;
            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();

            // accept access tokens from identityserver and require a scope of 'api1'   
            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                Authority = "http://localhost:5000",
                ValidationMode = ValidationMode.Local,
                RequiredScopes = new List<string> { "Api2" }
            });

            // configure web api
            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();

            // require authentication for all controllers
            config.Filters.Add(new AuthorizeAttribute());

            app.UseWebApi(config);
        }
    }
}