using System;
using System.Linq;

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

            var clientId = SettingsReader.Instance().GetSettings().clientId;

            var clientSecret = SettingsReader.Instance().GetSettings().clientSecret;

            XeroInvoiceFinder.Common.XeroClient client = new Common.XeroClient(clientId, clientSecret);
           
            var authContext =  await client.Authenticate();

            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(authContext));


            Console.WriteLine("**********************************************************************************************");
            Console.WriteLine("*******************RETRIEVING TENANTS*****************************");

            var connection = await client.GetConnection(authContext.access_token);

            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(connection));


            Console.WriteLine("**********************************************************************************************");
            Console.WriteLine("*******************RETRIEVING INVOICE for ORC0999*****************************");

            var invoice = await client.GetInvoice(connection.FirstOrDefault().TenantId.ToString(), "ORC0999", authContext.access_token);
            
            Console.WriteLine(invoice.AmountDue);

            Console.WriteLine("**********************************************************************************************");
            Console.WriteLine("*******************RETRIEVING ALL INVOICES*****************************");

            var invoices = await client.GetInvoices(connection.FirstOrDefault().TenantId.ToString(),null, authContext.access_token);

            invoices._Invoices.ForEach(inv =>
            {
                Console.WriteLine($"{inv.InvoiceID} - {inv.InvoiceNumber} - Amount Due: $ {inv.AmountDue}");
            });

            Console.WriteLine("**********************************************************************************************");
            Console.WriteLine($"***Total Amount Due ${invoices._Invoices.Sum(a=> a.AmountDue)}***\n");
            Console.WriteLine($"***Total Number of Invoices - {invoices._Invoices.Count()}***\n");
            Console.WriteLine($"***Earliest Due Date - {invoices._Invoices.Min(a=> a.DueDate).Value.ToString("dd/MM/yyyy")}***\n");
            Console.WriteLine("*******************PRESS ANY KEY TO CLOSE*****************************");

        }
    }
}
