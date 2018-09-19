using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Webapi_delay.Models;

namespace Webapi_delay.Controllers
{
    public class ProductsController : ApiController
    {
        private Product[] Products()
        {
            return new Product[] { new Product() { ProductId = 1111, Title = "Fitness Nutrition Specialization (FNS)", Price = 499.00, ReleaseDate = DateTime.Now },
                new Product() { ProductId = 2222, Title = "Weight Loss Specialization (WLS)", Price = 299.00, ReleaseDate = DateTime.Now },
                new Product() { ProductId = 3333, Title = "Behavior Change Specialization (BCS)", Price = 699.00, ReleaseDate = DateTime.Now },
                new Product() { ProductId = 4444, Title = "Group Personal Training Specialization (GPTS)", Price = 549.00, ReleaseDate = DateTime.Now }
            };
        }

        /// <summary>
        /// Get products with given delay
        /// </summary>
        /// <param name="delay"></param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Product> GetdelayedProducts(int delay = 5000)
        {
            System.Threading.Thread.Sleep(delay);
            return Products();
        }

        /// <summary>
        /// Get all products
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Product> GetProducts()
        {
            return Products();
        }

        /// <summary>
        /// Get an exception
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Product GetException()
        {
            //throw new HttpResponseException(HttpStatusCode.NotFound);
            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Throwing exception"));
        }
    }
}
