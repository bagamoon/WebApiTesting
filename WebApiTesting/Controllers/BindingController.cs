using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiTesting.Models;

namespace WebApiTesting.Controllers
{    
    [RoutePrefix("api/bind")]
    [Route("{action=index}")]
    public class BindingController : ApiController
    {
        private string _connString = ConfigurationManager.ConnectionStrings["Northwind"].ConnectionString;

        [HttpGet]
        public HttpResponseMessage Index()
        {
            var msg = new HttpResponseMessage();
            msg.StatusCode = HttpStatusCode.OK;
            msg.Content = new StringContent("hello world");

            return msg;
        }

        [HttpPost]
        public Products GetProducts(QueryDto query)
        {
            using (var conn = new SqlConnection(_connString))
            {
                var product = conn.QueryFirstOrDefault<Products>("select * from products where productId = @productId", new { productId = query.ProductId });

                product.ProductName = query.ProductName;

                return product;
            }
        }
    }

    public class QueryDto
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }
    }

}
