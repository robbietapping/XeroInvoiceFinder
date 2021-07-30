using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xero.NetStandard.OAuth2.Api;
using Xero.NetStandard.OAuth2.Model.Accounting;
using Xero.NetStandard.OAuth2.Model.Identity;
using System.Linq;

namespace XeroInvoiceFinder.Common
{
    public class XeroClient
    {

        private readonly string _clientid;
        private readonly string _clientSecret;
        public static AuthResponse AuthContext { get; set; }

        public XeroClient(string clientId, string clientSecret)
        {

            _clientid = clientId;
            _clientSecret = clientSecret;
        }


        public async Task<AuthResponse> Authenticate()
        {

            string code = $"{_clientid}:{_clientSecret}";
            string authorizationCode = System.Convert.ToBase64String(System.Text.ASCIIEncoding.UTF8.GetBytes(code));

            RestSharp.RestClient client = new RestSharp.RestClient("https://identity.xero.com/");

            var request = new RestSharp.RestRequest("/connect/token?", RestSharp.Method.POST);

            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

            request.AddObject(new { grant_type = "client_credentials", scope = "accounting.transactions" });

            request.AddHeader("Authorization", $"Basic {authorizationCode}");

            var response = await client.ExecuteAsync<AuthResponse>(request);


            AuthContext = response.Data;
            return AuthContext;


        }



        public async Task<List<Connection>> GetConnection(string accessToken = null)
        {
            if (accessToken == null)
            {
                if (AuthContext == null)
                {
                    await Authenticate();
                }
                accessToken = AuthContext.access_token;
            }
            var apiInstance = new IdentityApi();

            try
            {
                var result = await apiInstance.GetConnectionsAsync(accessToken);
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception when calling GetConnection: " + e.Message);
                return null;
            }
        }

        public async Task<Invoices> GetInvoices(string xeroTenantId, List<string> invoiceNos = null, string accessToken = null)
        {
            if (accessToken == null)
            {
                if (AuthContext == null)
                {
                    await Authenticate();
                }
                accessToken = AuthContext.access_token;
            }
            var apiInstance = new AccountingApi();

            try
            {
                var result = await apiInstance.GetInvoicesAsync(accessToken, xeroTenantId, invoiceNumbers: invoiceNos);
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception when calling GetInvoice: " + e.Message);
                return null;
            }
        }

        public async Task<Invoice> GetInvoice(string xeroTenantId, string invoiceNo, string accessToken = null)
        {
            if (accessToken == null)
            {
                if (AuthContext == null)
                {
                    await Authenticate();
                }
                accessToken = AuthContext.access_token;
            }
            var apiInstance = new AccountingApi();

            try
            {
                var result = await apiInstance.GetInvoicesAsync(accessToken, xeroTenantId, invoiceNumbers: new List<string>() { invoiceNo });
                return result._Invoices.FirstOrDefault();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception when calling GetInvoice: " + e.Message);
                return null;
            }
        }



    }



    public class AuthResponse
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string token_type { get; set; }
        public string scope { get; set; }
    }

}
