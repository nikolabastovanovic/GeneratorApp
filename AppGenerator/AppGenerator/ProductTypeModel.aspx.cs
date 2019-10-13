 
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
				BeforeInsert(producttype);
				db.SaveChanges();

				return producttype.ID + " was succesufully inserted.";
			}
			catch (Exception e)
			{
			    return "Error: " + e;
			}
		}

		public string UpdateProductType(int id, ProductType producttype)
		{
		    try
		    {
				GarageEntities db = new GarageEntities();
		        ProductType tmp = db.ProductTypes.Find(id);

								tmp.Name = producttype.Name;
				
				BeforeUpdate(producttype);
				db.SaveChanges();
		        return producttype.ID + " was succesufully updated.";
		    }
		    catch (Exception e)
		    {
		        return "Error: " + e;
			}
		}

		public string DeleteProductType(int id)
		{
		    try
		    {
		        GarageEntities db = new GarageEntities();
		        ProductType producttype = db.ProductTypes.Find(id);

		        db.ProductTypes.Attach(producttype);
		        db.ProductTypes.Remove(producttype);
				BeforeDelete(id);
		        db.SaveChanges();

		        return producttype.ID + " was succesufully deleted.";
		    }
		    catch (Exception e)
		    {
		        return "Error: " + e;
		    }
		}

		public ProductType GetProductType(int id)
		{
		    try
		    {
		        using (GarageEntities db = new GarageEntities())
		        {
		            ProductType producttype = db.ProductTypes.Find(id);
					BeforeGetDetails(producttype);
		            return producttype;
		        }
		    }
		    catch (Exception)
		    {
		        return null;
		    }
		}

		public List<ProductType> GetAllProductTypes()
		{
		    try
		    {
		        using(GarageEntities db = new GarageEntities())
		        {
		            List<ProductType> producttypes = (from x in db.ProductTypes select x).ToList();
					BeforeGetList(producttypes);
		            return producttypes;
		        }
		    }
		    catch (Exception)
		    {
		        return null;
		    }
		}

		partial void BeforeInsert(ProductType producttype);
		partial void BeforeUpdate(ProductType producttype);
		partial void BeforeDelete(int id);
		partial void BeforeGetDetails(ProductType producttype);
		partial void BeforeGetList(List<ProductType> producttypes);
	}
}
