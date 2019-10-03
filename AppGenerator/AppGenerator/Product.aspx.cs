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

	public string UpdateProduct(int id, Product product)
    {
        try
        {
            GarageEntities db = new GarageEntities();
            Product tmp = db.Products.Find(id);

			
			tmp.TypeID = product.TypeID
			
			tmp.Name = product.Name
			
			tmp.Price = product.Price
			
			tmp.Description = product.Description
			
			tmp.Image = product.Image
			

			db.SaveChanges();
            return product.ID + " was succesufully updated.";
         }
         catch (Exception e)
         {
             return "Error: " + e;
         }
	}
}
