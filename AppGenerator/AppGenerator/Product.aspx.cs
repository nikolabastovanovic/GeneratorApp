using MyGeneratedApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyGeneratedApp.Models
{
	public partial class ProductModel 
	{
		public string InsertProduct(Product product)
		{
			try
			{
				GarageEntities db = new GarageEntities();
				db.Products.Add(product);
				db.SaveChanges();

				return product.ID + " was succesufully inserted.";
			}
			catch (Exception e)
			{
			    return "Error: " + e;
			}
		}
	}
}
