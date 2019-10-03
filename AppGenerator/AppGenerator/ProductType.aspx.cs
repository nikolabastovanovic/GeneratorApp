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

	public string UpdateProductType(int id, ProductType producttype)
    {
        try
        {
            GarageEntities db = new GarageEntities();
            ProductType tmp = db.ProductTypes.Find(id);

			
			tmp.Name = producttype.Name
			

			db.SaveChanges();
            return product.ID + " was succesufully updated.";
         }
         catch (Exception e)
         {
             return "Error: " + e;
         }
	}
}
