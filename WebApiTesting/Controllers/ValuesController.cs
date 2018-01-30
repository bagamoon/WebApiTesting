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
	public class ValuesController : ApiController
	{
		private string _connString = ConfigurationManager.ConnectionStrings["Northwind"].ConnectionString;

		// GET api/values
		public IEnumerable<Products> Get()
		{
			using (var conn = new SqlConnection(_connString))
			{
				var products = conn.Query<Products>("select top 5 * from Products");
				return products;
			}				
		}
	}
}
