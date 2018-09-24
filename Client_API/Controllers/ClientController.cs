using Polly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Client_API.Controllers
{
    public class ClientController : ApiController
    {
        protected ClientController()
        {
            //if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "My Log File.txt"))
            //    File.Delete(AppDomain.CurrentDomain.BaseDirectory + "My Log File.txt");
        }

        [HttpGet]
        public async Task<string> Get()
        {
            LogMessageToFile("-------Calling Get API");
            Action<Exception, TimeSpan> onBreak = (exception, timespan) =>
            {
                LogMessageToFile("On Break, time span : " + timespan.ToString());
            };
            Action onReset = () =>
            {
                LogMessageToFile("On Reset");
            };
            Action onHalfOpen = () =>
            {
                LogMessageToFile("on HalfOpen");
            };
            var circuitBreaker = Policy
                                .Handle<Exception>()
                                .CircuitBreakerAsync(
                                    exceptionsAllowedBeforeBreaking: 1,
                                    durationOfBreak: TimeSpan.FromSeconds(20000),
                                    onBreak: onBreak,
                                    onReset: onReset,
                                    onHalfOpen: onHalfOpen
                                );

            HttpResponseMessage res = null;
            await circuitBreaker.ExecuteAsync(async () => res = await ServiceCall(new Uri($"http://localhost:52892/api/products/GetProducts")));
            return await res.Content.ReadAsStringAsync();
        }

        [HttpGet]
        public async Task<string> GetwithDelay(int delay = 0)
        {
            LogMessageToFile("-------Calling GetwithDelay API with dalay " + delay + " -----");
            Action<Exception, TimeSpan> onBreak = (exception, timespan) =>
            {
                LogMessageToFile("On Break, time span : " + timespan.ToString());
            };
            Action onReset = () =>
            {
                LogMessageToFile("On Reset");
            };
            Action onHalfOpen = () =>
            {
                LogMessageToFile("on HalfOpen");
            };
            var circuitBreaker = Policy
                                .Handle<Exception>()
                                .CircuitBreakerAsync(
                                    exceptionsAllowedBeforeBreaking: 1,
                                    durationOfBreak: TimeSpan.FromMinutes(1),
                                    onBreak: onBreak,
                                    onReset: onReset,
                                    onHalfOpen: onHalfOpen
                                );

            HttpResponseMessage res = null;
            await circuitBreaker.ExecuteAsync(async () => res = await ServiceCall(new Uri($"http://localhost:52892/api/products/GetdelayedProducts?delay={delay}")));
            return await res.Content.ReadAsStringAsync();
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetException()
        {
            return await ServiceCall(new Uri($"http://localhost:52892/api/products/GetException"));
        }

        private static async Task<HttpResponseMessage> ServiceCall(Uri uri)
        {
            HttpResponseMessage response = null;
            try
            {
                using (var client = new HttpClient()
                {
                    Timeout = TimeSpan.FromSeconds(5)
                })
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    LogMessageToFile("Calling API");
                    response = await client.GetAsync(uri);
                    if (!response.IsSuccessStatusCode)
                    {
                        return new HttpResponseMessage(response.StatusCode);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public static void LogMessageToFile(string msg)
        {
            System.IO.StreamWriter sw = System.IO.File.AppendText(
                 AppDomain.CurrentDomain.BaseDirectory + "My Log File.txt");
            try
            {
                string logLine = System.String.Format(
                    "{0:G}: {1}.", System.DateTime.Now, msg);
                sw.WriteLine(logLine);
            }
            finally
            {
                sw.Close();
            }
        }


    }
}
