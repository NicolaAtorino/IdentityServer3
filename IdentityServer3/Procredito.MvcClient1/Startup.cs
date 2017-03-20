using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Web.Helpers;
using IdentityServer3.Core;
using System.Security.Claims;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using Procredito.MvcClient1;
using Microsoft.IdentityModel.Protocols;

[assembly: OwinStartup(typeof(Procredito.Api1.Startup))]
namespace Procredito.Api1
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "Cookies",
            });

            AntiForgeryConfig.UniqueClaimTypeIdentifier = Constants.ClaimTypes.Subject;
            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                Authority = "http://localhost:5000",
                ClientId = "mvc",
                Scope = "openid profile roles Api2",
                
                RedirectUri = "http://localhost:5002/",
                ResponseType = "id_token token",
                
                SignInAsAuthenticationType = "Cookies",

                UseTokenLifetime = false,

                Notifications = new OpenIdConnectAuthenticationNotifications
                {

                    SecurityTokenValidated = n =>
                    {
                        var id = n.AuthenticationTicket.Identity;

                        // we want to keep first name, last name, subject and roles
                        var givenName = id.FindFirst(Constants.ClaimTypes.GivenName);
                        var familyName = id.FindFirst(Constants.ClaimTypes.FamilyName);
                        var sub = id.FindFirst(Constants.ClaimTypes.Subject);
                        var roles = id.FindAll(Constants.ClaimTypes.Role);

                        // create new identity and set name and role claim type
                        var nid = new ClaimsIdentity(
                            id.AuthenticationType,
                            Constants.ClaimTypes.GivenName,
                            Constants.ClaimTypes.Role);

                        nid.AddClaim(givenName);
                        nid.AddClaim(familyName);
                        nid.AddClaim(sub);
                        nid.AddClaims(roles);
                        nid.AddClaim(new Claim("id_token", n.ProtocolMessage.IdToken));
                        nid.AddClaim(new Claim("access_token", n.ProtocolMessage.AccessToken));


                        // add some other app specific claim
                        nid.AddClaim(new Claim("app_specific", "some data"));

                        n.AuthenticationTicket = new AuthenticationTicket(
                            nid,
                            n.AuthenticationTicket.Properties);

                        return Task.FromResult(0);
                    },

                    RedirectToIdentityProvider = n =>
                    {
                        if (n.ProtocolMessage.RequestType == OpenIdConnectRequestType.LogoutRequest)
                        {
                            var idTokenHint = n.OwinContext.Authentication.User.FindFirst("id_token");

                            if (idTokenHint != null)
                            {
                                n.ProtocolMessage.IdTokenHint = idTokenHint.Value;
                            }
                        }

                        return Task.FromResult(0);
                    }
                }
            });

            app.UseResourceAuthorization(new AuthorizationManager());

        }
    }
}