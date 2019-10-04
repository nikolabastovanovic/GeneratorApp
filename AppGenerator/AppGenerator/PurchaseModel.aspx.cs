 
using MyGeneratedApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyGeneratedApp.Models
{
	public partial class PurchaseModel 
	{
		public string InsertPurchase(Purchase purchase)
		{
			try
			{
				GarageEntities db = new GarageEntities();
				db.Purchases.Add(purchase);
				db.SaveChanges();

				return purchase.ID + " was succesufully inserted.";
			}
			catch (Exception e)
			{
			    return "Error: " + e;
			}
		}

		public string UpdatePurchase(int id, Purchase purchase)
		{
		    try
		    {
				GarageEntities db = new GarageEntities();
		        Purchase tmp = db.Purchases.Find(id);

								tmp.CustomerID = purchase.CustomerID;
								tmp.ProductID = purchase.ProductID;
								tmp.Amount = purchase.Amount;
								tmp.Date = purchase.Date;
								tmp.IsInCart = purchase.IsInCart;
				
				db.SaveChanges();
		        return purchase.ID + " was succesufully updated.";
		    }
		    catch (Exception e)
		    {
		        return "Error: " + e;
			}
		}

		public string DeletePurchase(int id)
		{
		    try
		    {
		        GarageEntities db = new GarageEntities();
		        Purchase purchase = db.Purchases.Find(id);

		        db.Purchases.Attach(purchase);
		        db.Purchases.Remove(purchase);
		        db.SaveChanges();

		        return purchase.ID + " was succesufully deleted.";
		    }
		    catch (Exception e)
		    {
		        return "Error: " + e;
		    }
		}

		public Purchase GetPurchase(int id)
		{
		    try
		    {
		        using (GarageEntities db = new GarageEntities())
		        {
		            Purchase purchase = db.Purchases.Find(id);
		            return purchase;
		        }
		    }
		    catch (Exception)
		    {
		        return null;
		    }
		}

		public List<Purchase> GetAllPurchases()
		{
		    try
		    {
		        using(GarageEntities db = new GarageEntities())
		        {
		            List<Purchase> purchases = (from x in db.Purchases select x).ToList();
		            return purchases;
		        }
		    }
		    catch (Exception)
		    {
		        return null;
		    }
		}
	}
}
