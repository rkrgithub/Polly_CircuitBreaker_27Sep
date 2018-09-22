using System;
using System.Collections.Generic;
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
        [HttpGet]
        public async Task<HttpResponseMessage> Get()
        {
            return await ServiceCall(new Uri($"http://localhost:52892/api/products/GetProducts"));
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetwithDelay(int delay = 0)
        {
            return await ServiceCall(new Uri($"http://localhost:52892/api/products/GetdelayedProducts?delay={delay}"));
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
                using (var client = new HttpClient() { Timeout = TimeSpan.FromSeconds(5) })
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
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

        
    }
}
