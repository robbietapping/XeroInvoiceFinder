using System;

namespace XeroInvoiceFinder
{
    class Program
    {
        public static void Main(string[] args)
        {

            GetData();
            var x = Console.ReadLine();
        }


        public static async void GetData()
        {
            XeroInvoiceFinder.Common.XeroClient client = new Common.XeroClient("04B394EAEE844D588260B2E569F2B6A5", "6iDgnnVlyOCtEmqLqmTawcCZIS7i9BqBx7Y5kCUks5z7n4ff");
           
            var authContext =  await client.Authenticate();

            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(authContext));

            var connection = await client.GetConnection(authContext.access_token);

            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(connection));

            var invoice = await client.GetInvoice(connection[0].TenantId.ToString(), "ORC0999", authContext.access_token);
            
            Console.WriteLine(invoice._Invoices[0].AmountDue);

        }
    }
}
