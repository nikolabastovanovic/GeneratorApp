 
using GeneratedDinamicWebSite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace GeneratedDinamicWebSite.Models
{
	public partial class ProductModel 
	{
		public string InsertProduct(Product product)
		{
			try
			{
				GarageEntities db = new GarageEntities();
				db.Products.Add(product);
				BeforeInsert(product);
				db.SaveChanges();

				return product.ID + " was succesufully inserted.";
			}
			catch (Exception e)
			{
			    return "Error: " + e;
			}
		}

		public string UpdateProduct(int id, Product product)
		{
		    try
		    {
				GarageEntities db = new GarageEntities();
		        Product tmp = db.Products.Find(id);

								tmp.TypeID = product.TypeID;
								tmp.Name = product.Name;
								tmp.Price = product.Price;
								tmp.Description = product.Description;
								tmp.Image = product.Image;
				
				BeforeUpdate(product);
				db.SaveChanges();
		        return product.ID + " was succesufully updated.";
		    }
		    catch (Exception e)
		    {
		        return "Error: " + e;
			}
		}

		public string DeleteProduct(int id)
		{
		    try
		    {
		        GarageEntities db = new GarageEntities();
		        Product product = db.Products.Find(id);

		        db.Products.Attach(product);
		        db.Products.Remove(product);
				BeforeDelete(id);
		        db.SaveChanges();

		        return product.ID + " was succesufully deleted.";
		    }
		    catch (Exception e)
		    {
		        return "Error: " + e;
		    }
		}

		public Product GetProduct(int id)
		{
		    try
		    {
		        using (GarageEntities db = new GarageEntities())
		        {
		            Product product = db.Products.Find(id);
					BeforeGetDetails(product);
		            return product;
		        }
		    }
		    catch (Exception)
		    {
		        return null;
		    }
		}

		public List<Product> GetAllProducts()
		{
		    try
		    {
		        using(GarageEntities db = new GarageEntities())
		        {
		            List<Product> products = (from x in db.Products select x).ToList();
					BeforeGetList(products);
		            return products;
		        }
		    }
		    catch (Exception)
		    {
		        return null;
		    }
		}

		partial void BeforeInsert(Product product);
		partial void BeforeUpdate(Product product);
		partial void BeforeDelete(int id);
		partial void BeforeGetDetails(Product product);
		partial void BeforeGetList(List<Product> products);
	}
}
