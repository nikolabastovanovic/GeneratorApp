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
	}

	public string UpdatePurchase(int id, Purchase purchase)
    {
        try
        {
            GarageEntities db = new GarageEntities();
            Purchase tmp = db.Purchases.Find(id);

			
			tmp.CustomerID = purchase.CustomerID
			
			tmp.ProductID = purchase.ProductID
			
			tmp.Amount = purchase.Amount
			
			tmp.Date = purchase.Date
			
			tmp.IsInCart = purchase.IsInCart
			

			db.SaveChanges();
            return product.ID + " was succesufully updated.";
         }
         catch (Exception e)
         {
             return "Error: " + e;
         }
	}
}
