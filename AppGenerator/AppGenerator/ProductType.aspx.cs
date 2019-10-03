using MyGeneratedApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyGeneratedApp.Models
{
	public partial class ProductTypeModel 
	{
		public string InsertProductType(ProductType producttype)
		{
			try
			{
				GarageEntities db = new GarageEntities();
				db.ProductTypes.Add(producttype);
				db.SaveChanges();

				return producttype.ID + " was succesufully inserted.";
			}
			catch (Exception e)
			{
			    return "Error: " + e;
			}
		}
	}
}
