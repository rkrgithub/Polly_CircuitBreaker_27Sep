using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ClientApp
{
    class Program
    {

        private static HttpClient _client = new HttpClient() {
            Timeout = TimeSpan.FromSeconds(2)
        };
        static void Main(string[] args)
        {
            Console.WriteLine("API Timeout set to 2 seconds \r\n------------------\r\n> Enter '1 <number of calls> <delay in milli second>' for Delayed products \r\n> Enter '2 <number of calls>' for Get Products immediately \r\n> Enter '3 <number of calls>' for Getting Exception \r\n");
            while (true)
            {
                var s = Console.ReadLine();
                string[] input = s.Split(' ');
                var numberofCalls = input.Length > 1 ? Convert.ToInt32(input[1]) : 1;
                switch (input[0])
                {
                    case "1":
                        ServiceRequest($"http://localhost:52892/api/products/Getdelayedproducts?delay={(input.Length > 2 ? input[2] : "1000")}", numberofCalls);
                        break;
                    case "2":
                        ServiceRequest("http://localhost:52892/api/products/GetProducts", numberofCalls);
                        break;
                    case "3":
                        ServiceRequest("http://localhost:52892/api/products/GetException", numberofCalls);
                        break;
                    default:
                        break;
                }
            }
        }

        static void ServiceRequest(string url, int number_of_calls = 1)
        {
            try
            {
                Console.WriteLine("'" + url + "' api is calling " + number_of_calls + " times");
                for (int i = 0; i < Convert.ToInt32(number_of_calls); i++)
                {
                    var res = GetAsync(url).Result;
                    Console.WriteLine("\r\n" + res);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        static async Task<string> GetAsync(string url)
        {
            string result = "";
            try
            {
               
                HttpResponseMessage response = await _client.GetAsync(new Uri(url));
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            return result;
        }
    }
}
