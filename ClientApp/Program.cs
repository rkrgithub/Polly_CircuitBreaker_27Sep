using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace ClientApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter '1 <delay in milli second>' for Delayed products \r\n Enter '2' for Get Products immediately \r\n Enter '3' for Getting Exception \r\n");
            var s = Console.ReadLine();
            string[] input = s.Split(' ');
            switch (input[0])
            {
                case "1":
                    ServiceRequest("http://localhost:52892/api/products/Getdelayedproducts?delay=" + input[1]);
                    break;
                case "2":
                    ServiceRequest("http://localhost:52892/api/products/GetProducts");
                    break;
                case "3":
                    ServiceRequest("http://localhost:52892/api/products/GetException");
                    break;
                default:
                    break;
            }

        }

        static async void ServiceRequest(string url)
        {
            string result = await Get(url);
            Console.WriteLine(result);
            Console.Read();
        }

        static async Task<string> Get(string url)
        {
            using (var client = new HttpClient().GetAsync(new Uri(url)))
            {
                string content = await client.Result.Content.ReadAsStringAsync();
                //var yourOjbect = new JavaScriptSerializer().Deserialize<string[]>(content);
                return content;
            }
        }
    }
}
