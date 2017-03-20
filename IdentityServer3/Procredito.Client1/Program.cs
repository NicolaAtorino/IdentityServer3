using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityModel.Client;
using System.Net.Http;

namespace Procredito.Client1
{
    class Program
    {
        static void Main(string[] args)
        {

            CallApi(GetClientToken());
            CallApi(GetUserToken());
        }
        
        static TokenResponse GetClientToken()
        {
            var client = new TokenClient(
                "http://localhost:5000/connect/token",
                "silicon",
                "F621F470-9731-4A25-80EF-67A6F7C5F4B8");

            return client.RequestClientCredentialsAsync("Api2").Result;
        }

        static TokenResponse GetUserToken()
        {
            var client = new TokenClient(
                "http://localhost:5000/connect/token",
                "carbon",
                "21B5F798-BE55-42BC-8AA8-0025B903DC3B");

            return client.RequestResourceOwnerPasswordAsync("bob", "secret", "Api2").Result;
        }

        static void CallApi(TokenResponse response)
        {
            var client = new HttpClient();
            client.SetBearerToken(response.AccessToken);

            Console.WriteLine(client.GetStringAsync("http://localhost:5001/test").Result);
        }
    }
}
