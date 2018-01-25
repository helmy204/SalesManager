using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.ElkhateebDynamicLinq;
/// <summary>
/// Summary description for SalesManager
/// </summary>
public class SalesManager
{
	public SalesManager()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public static string getBasicUnitName
    {
        get { return BasicUnitName(); }
    }
    public static int getBasicUnitID
    {
        get { return BasicUnitID(); }
    }
  public static   void UpdateproductQTY(QTYOperation operation ,int ProductID, Double Quantity)
    {
        Product objProduct = new Product();
        objProduct = objProduct.Single(p => p.ProductID == ProductID);
      if(operation == QTYOperation.Increment)
        objProduct.Quantity += Quantity;
      else if(operation== QTYOperation.Decrement)
          objProduct.Quantity -=Quantity;
        objProduct.Update(p => p.ProductID == objProduct.ProductID);


    }
   static string BasicUnitName()
  {
      Unit objunit = new Unit();
      var q = objunit.Filter(p => p.Basic == true);
      if (q.Count() == 0) return string.Empty;
      else
          return (HttpContext.Current.Session["culture"] == "en-US") ? q.First().UnitName_En : q.First().UnitName_Ar;
  }
   static int BasicUnitID()
  {
      Unit objunit = new Unit();
      var q = objunit.Filter(p => p.Basic == true);
      if (q.Count() == 0) throw new Exception("Basic unit dos not Set.");
      else
          return q.First().UnitID;
  }
  public static double UnitConvert(int UnitID, double Quantity)
  {
      Unit objunit = new Unit();
      objunit = objunit.Single(p => p.UnitID == UnitID);
      //if (objunit == null)
      //    return 0;
      //if ( objunit.Basic == true)
      //    return Quantity;
            
      //    return Quantity * objunit.UnitConversions.First().Factor;
      return  ((objunit.Basic == true) ? Quantity : Quantity * objunit.UnitConversions.First().Factor);
  }
  public static double UnitPriceConvert(int UnitID, double price)
  {
      Unit objunit = new Unit();
      objunit = objunit.Single(p => p.UnitID == UnitID);
      
      return ((objunit.Basic == true) ? price : price / objunit.UnitConversions.First().Factor);
  }



}
public enum QTYOperation
{
    Increment,Decrement
}