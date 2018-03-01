using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiTesting.ActionFilter;
using WebApiTesting.Models;

namespace WebApiTesting.Controllers
{    
    [RoutePrefix("api/bind")]
    [Route("{action=index}")]
    public class BindingController : ApiController
    {
        private string _connString = ConfigurationManager.ConnectionStrings["Northwind"].ConnectionString;

        [HttpGet]
        [TokenCheck]
        public IHttpActionResult Index()
        {
            return Ok("hello world");            
        }

        [HttpPost]        
        public Products QueryProducts(QueryDto query)
        {
            using (var conn = new SqlConnection(_connString))
            {
                var builder = new SqlBuilder();
                var productBuilder = builder.AddTemplate("select * from products /**where**/");                
                if (query != null)
                {
                    builder.Where("ProductId = @productId", new { ProductId = query.ProductId });
                }

                var product = conn.QueryFirstOrDefault<Products>(productBuilder.RawSql, 
                                                                 productBuilder.Parameters);

                return product;
            }            
        }

        [HttpPost]
        public HttpResponseMessage QueryProductsHttp(QueryDto query)
        {
            using (var conn = new SqlConnection(_connString))
            {
                var product = conn.QueryFirstOrDefault<Products>("select * from products where productId = @productId", new { productId = query.ProductId });

                product.ProductName = query.ProductName;

                return Request.CreateResponse(HttpStatusCode.OK, product);
            }
        }

        [HttpPost]
        public IEnumerable<QueryDto> PassListParam(QueryCollection collection)
        {
            return collection.Queries;
        }
    }

    public class QueryDto
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }
    }

    public class QueryCollection
    {
        public List<QueryDto> Queries { get; set; }
    }
}
