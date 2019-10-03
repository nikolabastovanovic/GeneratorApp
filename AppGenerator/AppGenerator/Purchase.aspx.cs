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
}
