using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.ElkhateebDynamicLinq;
using System.Globalization;
using System.Data;
public partial class Admin_ManageSales : System.Web.UI.Page
{


    #region Objects
    SalesMaster objSalesMaster = new SalesMaster();
    spSalesMasterResult objspSalesMasterResult = new spSalesMasterResult();

    SalesDetail objSalesDetail = new SalesDetail();
    spSalesDetailsResult objspSalesDetailsResult = new spSalesDetailsResult();

    Customer objCustomer = new Customer();
    Employee objEmployee = new Employee();
    Product objProduct = new Product();
    CultureInfo culture = new CultureInfo("en-US");
    Unit objunit = new Unit();

   
    #endregion

    #region Methods

   

    void Clear()
    {
        txtID.Text = txtNotes.Text = txtSalesDateM.Text = txtSalesDateH.Text = string.Empty;
        hfOldRest.Value = txtPayment.Text = lblRest.Text = lblCustomerBalance.Text = lblNewCustomerBalance.Text = "0";
        ddlCustomer.SelectedIndex = ddlEmployee.SelectedIndex = 0;
        cboxIsCash.Checked = false;
        gvSales.SelectedIndex = -1;
        IntialgvProducts();
      

    }
    void FillddlCustomer()
    {
        ddlCustomer.DataSource = objCustomer.Get().ToList();
        ddlCustomer.DataTextField = "CustomerName";
        ddlCustomer.DataValueField = "CustomerID";
        ddlCustomer.DataBind();
    }
    void FillddlEmployee()
    {
        ddlEmployee.DataSource = objEmployee.Get();
        ddlEmployee.DataTextField = "EmployeeName";
        ddlEmployee.DataValueField = "EmployeeID";
        ddlEmployee.DataBind();
    }
    void FillgvSales()
    {
        object[] parameter = null;
        gvSales.DataSource = objspSalesMasterResult.Get(parameter).ToList();
        gvSales.DataBind();
    }
    void FillddlProduct(GridViewRow gvRow)
    {
        DropDownList ddlProduct = (DropDownList)gvRow.FindControl("ddlProduct");
        ddlProduct.DataSource = objProduct.Get();
        ddlProduct.DataTextField = "ProductName";
        ddlProduct.DataValueField = "ProductID";
        ddlProduct.DataBind();
        ddlProduct.Items.Insert(0, new ListItem("Select", "0"));
        string productid = (gvProducts.DataKeys[gvRow.RowIndex]["ProductID"] != null) ? gvProducts.DataKeys[gvRow.RowIndex]["ProductID"].ToString() : "0";
        ddlProduct.SelectedValue = productid;
    }
    void FillddlUnits(GridViewRow gvRow)
    {
        DropDownList ddlUnits = (DropDownList)gvRow.FindControl("ddlUnits");
        ddlUnits.DataSource = objunit.Get().ToList();
        ddlUnits.DataTextField = (Session["culture"] == "en-US") ? "UnitName_En" : "UnitName_Ar";
        ddlUnits.DataValueField = "UnitID";
        ddlUnits.DataBind();
        ddlUnits.Items.Insert(0, new ListItem("Select", "0"));
        string UnitID = (gvProducts.DataKeys[gvRow.RowIndex]["UnitID"] != null) ? gvProducts.DataKeys[gvRow.RowIndex]["UnitID"].ToString() : "0";
        ddlUnits.SelectedValue = UnitID;
    }
    void FillgvProducts(List<SalesDetail> list)
    {
        gvProducts.DataSource = list;
        gvProducts.DataBind();
    }
    void FillgvProducts(int SalesID)
    {
        var list = objSalesDetail.Filter(p => p.SalesID == SalesID).ToList();
        list.ForEach(p => p.oldQty = p.Quantity);
        gvProducts.DataSource = list;
        gvProducts.DataBind();
    }
    void Totalcala()
    {
        lblRest.Text = (Convert.ToDouble(lblTotalSalesPrice.Text) - Convert.ToDouble(!string.IsNullOrEmpty(txtPayment.Text) ? txtPayment.Text : "0.0")).ToString();
        if (ddlCustomer.SelectedIndex > 0)
        {
           // lblRest.Text = (Convert.ToDouble(lblTotalSalesPrice.Text) - Convert.ToDouble(!string.IsNullOrEmpty(txtPayment.Text) ? txtPayment.Text : "0.0")).ToString();

            int customerid = Convert.ToInt32(ddlCustomer.SelectedValue);
            var entity = objCustomer.Single(p => p.CustomerID == customerid);

            lblCustomerBalance.Text = entity.Balance.ToString();

            lblNewCustomerBalance.Text = ((Convert.ToDouble(lblRest.Text) + entity.Balance) - Convert.ToDouble(hfOldRest.Value)).ToString();


        }
        else
        {
            lblCustomerBalance.Text = lblNewCustomerBalance.Text = "0";
        }
    }
    void CalculateTotalPrice(GridViewRow gvrow)
    {
        TextBox txtSQTY = (TextBox)gvrow.FindControl("txtSQTY");
        //TextBox txtQuantity = (TextBox)gvrow.FindControl("txtQuantity");
        TextBox txtSUnitPrice = (TextBox)gvrow.FindControl("txtSUnitPrice");
       // TextBox txtPrice = (TextBox)gvrow.FindControl("txtPrice");
        TextBox txtTotalPrice = (TextBox)gvrow.FindControl("txtTotalPrice");

        //if (string.IsNullOrEmpty(txtQuantity.Text))
        //    txtQuantity.Text = "0";
        //if (string.IsNullOrEmpty(txtPrice.Text))
        //    txtPrice.Text = "0";

        txtTotalPrice.Text = Convert.ToString(Convert.ToDouble(!string.IsNullOrEmpty(txtSQTY .Text.Trim()) ? txtSQTY.Text.Trim() : "0.0") * Convert.ToDouble(!string.IsNullOrEmpty(txtSUnitPrice .Text.Trim()) ? txtSUnitPrice.Text.Trim() : "0.0"));

     //   txtTotalPrice.Text = Convert.ToString(Convert.ToDouble(txtQuantity.Text.Trim()) * Convert.ToDouble(txtPrice.Text.Trim()));
        Totalcala();

    }
    void CalculateTotalSales()
    {
        
         lblTotalSalesPrice.Text = "0";
        foreach (GridViewRow gvrow in gvProducts.Rows)
            lblTotalSalesPrice.Text = Convert.ToString(Convert.ToDouble(lblTotalSalesPrice.Text) + Convert.ToDouble(((TextBox)gvrow.FindControl("txtTotalPrice")).Text));
        Totalcala();
    }

    bool Insert()
    {
        objSalesMaster = new SalesMaster();

        objSalesMaster.CustomerID = (ddlCustomer.SelectedIndex==0)?null:(Nullable<int>)Convert.ToInt32 (ddlCustomer.SelectedValue);
        objSalesMaster.CustomerName = txtCustomerName.Text;

        objSalesMaster.EmployeeID = (ddlEmployee.SelectedIndex > 0) ? (Nullable<int>)int.Parse(ddlEmployee.SelectedValue) : null;
        objSalesMaster.Notes = txtNotes.Text.Trim();
        objSalesMaster.SalesDateM = Convert.ToDateTime(txtSalesDateM.Text.Trim(), culture);
        objSalesMaster.SalesDateH = txtSalesDateH.Text.Trim();
        objSalesMaster.IsCash = cboxIsCash.Checked;

        // new-------------------
        objSalesMaster.TotalSales  = Convert.ToDouble(lblTotalSalesPrice .Text);
        objSalesMaster.Rest = Convert.ToDouble(lblRest.Text);

        double Payments = Convert.ToDouble(!string.IsNullOrEmpty(txtPayment.Text) ? txtPayment.Text : "0.0");
        objSalesMaster.Payments = Payments;

        //-----------
        
        

       
        List<SalesDetail> saleslist = new List<SalesDetail>();
        saleslist = CurrentgvProducts(gvProducts);
        saleslist.ForEach(p =>
        {
            p.Quantity = SalesManager.UnitConvert(p.UnitID, p.SQTY);
            p.UnitPrice = SalesManager.UnitPriceConvert(p.UnitID, p.SUnitPrice);
            p.BasicUnitID = SalesManager.getBasicUnitID;

        });
        //var q = saleslist.Select(p => new { SalesID = p["SalesID"], ProductID = p["ProductID"] }).GroupBy(p => p.ProductID).Select(z => new { ProductID = z.Key, Count = z.Count() }).Where(p => p.Count > 1);
        //if (q.Count() > 0)
        //{
        //    MessageBox.Show(this.Page, "Cant Insert same Product more than once in Sales.");
        //    return false ;
        //}


     //  double TotalSalesPrice = saleslist.Sum(p=> (p.Quantity*p.Price));

        objSalesMaster.SalesDetails.AddRange(saleslist);

        objSalesMaster.BeginTransaction();
        
        
            // update qty product in store(table product)
       saleslist.ForEach(p=> SalesManager.UpdateproductQTY(QTYOperation.Decrement ,p.ProductID,p.Quantity));

       if (Payments  > 0)
       {
           CustomersPayment CustomersPayment = new CustomersPayment();
           CustomersPayment.CustomerID = objSalesMaster.CustomerID;
           CustomersPayment.PaymentDateM = Convert.ToDateTime(txtSalesDateM .Text.Trim(), culture);
           CustomersPayment.PaymentDateH = txtSalesDateH .Text.Trim();
           CustomersPayment.Payment = Payments;
           objSalesMaster.CustomerPaymentID  = Convert.ToInt32(CustomersPayment.Insert());
       }
       objSalesMaster.Insert();

        if (cboxIsCash.Checked != true)
        {
            if (ddlCustomer.SelectedIndex > 0)
            {
                objCustomer = objCustomer.Single(p => p.CustomerID == int.Parse(ddlCustomer.SelectedValue));
                objCustomer.Balance += objSalesMaster.Rest; ;
                objCustomer.Update(p => p.CustomerID == objCustomer.CustomerID);
            }
        }


        objSalesMaster.EndTransaction();

        
        
        return true;
    }
   
    bool Update()
    {

        objSalesMaster = new SalesMaster();
        objSalesMaster = objSalesMaster.Single(p => p.SalesID == Convert.ToInt32(txtID.Text.Trim()));

        objSalesMaster.CustomerName = txtCustomerName.Text;

        objSalesMaster.EmployeeID = (ddlEmployee.SelectedIndex > 0) ? (Nullable<int>)int.Parse(ddlEmployee.SelectedValue) : null;

        //objSalesMaster.CustomerID  = int.Parse(ddlCustomer .SelectedValue);//?
        objSalesMaster.Notes = txtNotes.Text.Trim();

        objSalesMaster.SalesDateM  = Convert.ToDateTime(txtSalesDateM .Text.Trim(), culture);

        objSalesMaster.SalesDateH  = txtSalesDateH .Text.Trim();

        objSalesMaster.IsCash  = cboxIsCash.Checked;

        double oldrest = objSalesMaster.Rest;

        bool oldPaymentType = objSalesMaster.IsCash ;
        int? paymentid = objSalesMaster.CustomerPaymentID ;

        objSalesMaster.TotalSales  = Convert.ToDouble(lblTotalSalesPrice.Text);
        objSalesMaster.Rest = Convert.ToDouble(lblRest.Text);

        double Payments = Convert.ToDouble(!string.IsNullOrEmpty(txtPayment.Text) ? txtPayment.Text : "0.0");
        objSalesMaster.Payments = Payments;





        objSalesMaster.BeginTransaction();


        CustomersPayment CustomersPayment = new CustomersPayment();
        if (paymentid == null)
        {
            if (Payments > 0)
            {

                CustomersPayment.CustomerID = objSalesMaster.CustomerID; ;
                CustomersPayment.PaymentDateM = Convert.ToDateTime(txtSalesDateM.Text.Trim(), culture);
                CustomersPayment.PaymentDateH = txtSalesDateH.Text.Trim();
                CustomersPayment.Payment = Payments;
                CustomersPayment.CustomerPaymentID = Convert.ToInt32(CustomersPayment.Insert());
            }
        }
        else if (paymentid != null)
        {
            CustomersPayment = objSalesMaster.CustomersPayment;
            CustomersPayment.PaymentDateM = Convert.ToDateTime(txtSalesDateM.Text.Trim(), culture);
            CustomersPayment.PaymentDateH = txtSalesDateH.Text.Trim();
            CustomersPayment.Payment = Payments;
            CustomersPayment.Update(p => p.CustomerPaymentID == CustomersPayment.CustomerPaymentID);
        }

        objSalesMaster.Update(p => p.SalesID  == objSalesMaster.SalesID );

        //var q=objPurchasesDetail.Filter(p => p.PurchaseID == objPurchasesMaster.PurchaseID).Select(p => (p.TotalPrice ));
        //double oldTotalPurchasePrice =(q!=null )?q.Sum():0.0; 


        List<SalesDetail> SalesList = CurrentgvProducts(gvProducts);
        SalesList.ForEach(p =>
        {
            p.Quantity = SalesManager.UnitConvert(p.UnitID, p.SQTY);
            p.UnitPrice = SalesManager.UnitPriceConvert(p.UnitID, p.SUnitPrice);
            p.BasicUnitID = SalesManager.getBasicUnitID;

        });

        //double TotalPurchasePrice = Purchaseslist.Sum(p => p.TotalPrice );
        List<SalesDetail> newSalesList = SalesList.Where(p => p.SalesDetailID == 0).ToList();
        List<SalesDetail> updateSalesList = SalesList.Where(p => p.SalesDetailID > 0).ToList();



        if (newSalesList.Count > 0)
        {
            newSalesList.ForEach(p => p.SalesID = objSalesMaster.SalesID );
            newSalesList.InsertAll();

            // update qty product in store(table product)
            newSalesList.ForEach(p => SalesManager.UpdateproductQTY(QTYOperation.Decrement, p.ProductID, p.Quantity));


        }
        if (updateSalesList.Count > 0)
        {

            updateSalesList.ForEach(p =>
            {


                p.Update(z => z.SalesDetailID == p.SalesDetailID );
                SalesManager.UpdateproductQTY(QTYOperation.Decrement, p.ProductID, (p.Quantity - p.oldQty));
            });
        }






        //========= composite calcualtion balance --- compare between old value and new  value 
        if (oldPaymentType != true)// not cash
        {
            if (ddlCustomer.SelectedIndex > 0)
            {
                // any way( - balance) form old supplier if user change supplier 
                objCustomer = objCustomer.Single(p => p.CustomerID == objSalesMaster.CustomerID);
                objCustomer.Balance -= oldrest;
                // objSupplier.Update(p => p.SupplierID == objSupplier.SupplierID);

                if (cboxIsCash.Checked != true)// if (not cash) add new price to balance 
                {

                    //objSupplier = objSupplier.Single(p => p.SupplierID == int.Parse(ddlSupplier.SelectedValue));
                    objCustomer.Balance += objSalesMaster.Rest;

                }
                objCustomer.Update(p => p.CustomerID == objCustomer.CustomerID);
            }

        }
        else if (oldPaymentType == true)//  cash
        {

            if (cboxIsCash.Checked != true)  // if user change to (not cash) will add new price to supplier balance
            {
                if (ddlCustomer.SelectedIndex > 0)
                {
                    objCustomer = objCustomer.Single(p => p.CustomerID == objSalesMaster.CustomerID );
                    objCustomer.Balance += objSalesMaster.Rest;
                    objCustomer.Update(p => p.CustomerID == objCustomer.CustomerID);
                }
            }
        }

        objSalesMaster .EndTransaction();

        return true;


    }
    void Dir(SalesMaster entity)
    {
        txtID.Text = entity.SalesID.ToString();

        ddlCustomer.SelectedValue = (entity.CustomerID==null )?"0":entity.CustomerID.Value.ToString();
        txtCustomerName.Text = entity.CustomerName;

        ddlEmployee.SelectedValue = (entity.EmployeeID != null) ? entity.EmployeeID.Value.ToString() : "0";
        txtNotes.Text = entity.Notes;
        txtSalesDateM.Text = entity.SalesDateM.ToString("MM/dd/yyyy" , culture);
        txtSalesDateH.Text = entity.SalesDateH;
        cboxIsCash.Checked = entity.IsCash;

        //oldCustomerid = entity.CustomerID;
        //oldPaymentType = entity.IsCash;

        FillgvProducts(entity.SalesID);

        lblTotalSalesPrice .Text = entity.TotalSales .ToString();
        txtPayment.Text = entity.Payments.ToString();
        hfOldRest.Value = lblRest.Text = entity.Rest.ToString();
        lblCustomerBalance .Text = lblNewCustomerBalance.Text =(entity.CustomerID==null )?"0": entity.Customer .Balance.ToString();
        ddlCustomer.Enabled = false;
    }

    bool Delete(int SalesID)
    {
      
        //return     objSalesMaster.Delete(p => p.SalesID == SalesID);

        objSalesMaster = objSalesMaster.Single(p => p.SalesID == SalesID);
        // totalpurchase-payments
        Double rest = objSalesMaster.Rest;//محصله الفاتوره النهائيه الا هى عباره عن اجمالى الفاتوره ناقص مدفوع العميل
        List<SalesDetail> itemslist = objSalesMaster.SalesDetails.ToList();
        var payments = objSalesMaster.CustomersPayment;
        objCustomer = objSalesMaster.Customer;


        try
        {
            objSalesMaster.BeginTransaction();

            //update Customer balance
            objCustomer.Balance -= rest;
            objCustomer.Update(p => p.CustomerID == objCustomer.CustomerID);
            //update products (store)
            itemslist.ForEach(p =>
            {
                //update qty
                SalesManager.UpdateproductQTY(QTYOperation.Increment, p.ProductID, p.Quantity);
            }
            );




            // delete purchaseitems(PurchasesDetails)
            itemslist.DeleteAll();



            //delete masetr(PurchasesMaster)

            objSalesMaster.Delete(p => p.SalesID == SalesID);
            //delete payments
            if (payments != null)
                payments.Delete(p => p.CustomerPaymentID == payments.CustomerPaymentID);
            objSalesMaster.EndTransaction();
        }
        catch (Exception ex)
        {
            ExtenssionClass.Rollback();
            Page.Show(ex.Message);
            return false;
        }
        return true;
        

    }
    bool deleteSalesDetail(int SalesDetailID)
    {
       // objSalesDetail.BeginTransaction();
       // objSalesDetail = objSalesDetail.Single(p => p.SalesDetailID == SalesDetailID);
        

       //// double Totalproductprice = objSalesDetail.Price * objSalesDetail.Quantity;

       // // update balance
       // objCustomer = objCustomer.Single(p => p.CustomerID == Convert.ToInt32(ddlCustomer.SelectedValue));
       //// objCustomer.Balance =(objCustomer.Balance)- (Totalproductprice);
       // objCustomer.Update(p => p.CustomerID == objCustomer.CustomerID);

       // //update qty
       // SalesManager.UpdateproductQTY(QTYOperation.Increment, objSalesDetail.ProductID, objSalesDetail.Quantity);
       // // delete row
       // bool status = objSalesDetail.Delete(p => p.SalesDetailID == SalesDetailID);
       // objSalesDetail.EndTransaction();
       // return status;

        objSalesDetail.BeginTransaction();
        objSalesDetail = objSalesDetail.Single(p => p.SalesDetailID == SalesDetailID);
        //var payment = objPurchasesDetail.PurchasesMaster.PaymentsForSupplier;
        //payment.Delete(p => p.SupplierPaymentID == payment.SupplierPaymentID);

        objSalesMaster = objSalesDetail.SalesMaster;
        objSalesMaster.TotalSales -= objSalesDetail.TotalPrice;
        objSalesMaster.Rest -= objSalesDetail.TotalPrice;

        objSalesMaster.Update(p => p.SalesID == objSalesMaster.SalesID);
        hfOldRest.Value = objSalesMaster.Rest.ToString();
        // update balance
        if (ddlCustomer.SelectedIndex > 0)
        {
            objCustomer = objCustomer.Single(p => p.CustomerID == objSalesDetail.SalesMaster.CustomerID);
            objCustomer.Balance = (objCustomer.Balance) - (objSalesDetail.TotalPrice);
            objCustomer.Update(p => p.CustomerID == objCustomer.CustomerID);
        }

        //update qty
        SalesManager.UpdateproductQTY(QTYOperation.Increment , objSalesDetail.ProductID, objSalesDetail.Quantity);

        bool status = objSalesDetail.Delete(p => p.SalesDetailID == SalesDetailID);
        objSalesDetail.EndTransaction();
        return status;


    }


    void GetSales(int SalesID)
    {
        object[] parameter = null;
        gvSales.DataSource = objspSalesMasterResult.Filter(parameter, p => p.SalesID == SalesID).ToList();
        gvSales.DataBind();
    }
    #endregion


    #region  DataTable
     List<SalesDetail> IntialgvProducts()
    {
        List<SalesDetail> list = new List<SalesDetail>();
        SalesDetail objSalesDetail = new SalesDetail();
        
        list.Add(objSalesDetail);
       
        return list;
    }
     List<SalesDetail> CurrentgvProducts(GridView gv)
    {

        List<SalesDetail> list = new List<SalesDetail>();
        foreach (GridViewRow gvrow in gv.Rows)
        {

            SalesDetail objSalesDetail = new SalesDetail();
            string value = "";
            value = (gvrow.FindControl("lblSalesDetailID") as Label).Text;
            objSalesDetail.SalesDetailID = Convert.ToInt32(((string.IsNullOrEmpty(value)) ? "0" : value));

            value = (gvrow.FindControl("lblSalesID") as Label).Text;
            objSalesDetail.SalesID = Convert.ToInt32(((string.IsNullOrEmpty(value)) ? "0" : value));

            value = (gvrow.FindControl("ddlProduct") as DropDownList).SelectedValue;
            objSalesDetail.ProductID = Convert.ToInt32(((string.IsNullOrEmpty(value)) ? "0" : value));

            value = (gvrow.FindControl("ddlUnits") as DropDownList).SelectedValue;
            objSalesDetail.UnitID = Convert.ToInt32(((string.IsNullOrEmpty(value)) ? "0" : value));

           
           // objSalesDetail.Quantity =Convert.ToDouble( (gvrow.FindControl("txtQuantity") as TextBox).Text);
           // objSalesDetail.Price = Convert.ToDouble((gvrow.FindControl("txtPrice") as TextBox).Text);

           // value = (gvrow.FindControl("hfoldQty") as HiddenField ).Value;
           //objSalesDetail.oldQty = (string.IsNullOrEmpty(value)) ? 0.0 : Convert.ToDouble(value); ;//hfQuantity
           //list.Add(objSalesDetail);



           //objSalesDetail.ProductID = Convert.ToInt32(((string.IsNullOrEmpty(value)) ? "0" : value));
 

           objSalesDetail.SQTY = Convert.ToDouble((gvrow.FindControl("txtSQTY") as TextBox).Text);

           objSalesDetail.SUnitPrice = Convert.ToDouble((gvrow.FindControl("txtSUnitPrice") as TextBox).Text);

           objSalesDetail.TotalPrice = Convert.ToDouble((gvrow.FindControl("txtTotalPrice") as TextBox).Text);

           value = (gvrow.FindControl("hfoldQty") as HiddenField).Value;

           objSalesDetail.oldQty = (string.IsNullOrEmpty(value)) ? 0.0 : Convert.ToDouble(value); ;//hfQuantity

           list.Add(objSalesDetail);
           
        }

        return list ;
    }
     List<SalesDetail> AddRow(GridView gv)
    {

         List<SalesDetail> list = CurrentgvProducts(gv);
        // var q = list.Select(p => new { SalesID = p.SalesID, ProductID = p.ProductID }).GroupBy(p => p.ProductID)
        // .Select(e => new { ProductID = e.Key, Count = e.Count() }).Where(p => p.Count > 1);
        //if (q.Count() > 0)
        //{
        //    MessageBox.Show(this.Page, "Cant Insert same Product more than once in Sales.");
        //    return list ;
        //}

        SalesDetail objSalesDetail = new SalesDetail();

        list.Add(objSalesDetail);
        return list ;
    }
     List<SalesDetail> RemoveRow(GridView gv, int RowIndex)
    {

        List<SalesDetail> list = CurrentgvProducts(gv);
        list.RemoveAt(RowIndex);
        if (list.Count == 0)
             return IntialgvProducts();
        return list;
    }
    
    #endregion


    #region Events
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.Title = "Manage Sales ";
            FillddlCustomer();
            FillddlEmployee();
            FillgvSales();
            FillgvProducts(IntialgvProducts());
        }
       
    }
    protected void imgbtnSave_Click(object sender, ImageClickEventArgs e)
    {
        System.Threading.Thread.Sleep(2000);

       

        if (gvSales.SelectedIndex == -1)
        {

            if (Insert())
            {
                mdl_Sucess.Show();
            }
            else
            {
                mdl_Fail.Show();
            }
        }
        else if (gvSales.SelectedIndex > -1)
        {

            if (Update())
            {
                mdl_Sucess.Show();
            }
            else
            {
                mdl_Fail.Show();
            }
        }

        FillgvSales();
        FillgvProducts(IntialgvProducts());
        Clear();
    }
    protected void imgbtnClear_Click(object sender, ImageClickEventArgs e)
    {
        FillgvProducts(IntialgvProducts());
        FillgvSales();
        Clear();

    }
    protected void gvSales_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int SalesID = Convert.ToInt32(e.CommandArgument);
        if (e.CommandName == "select")
        {
            Dir(objSalesMaster.Single(p => p.SalesID == SalesID));
        }
        else if (e.CommandName == "del")
        {
            try
            {
                System.Threading.Thread.Sleep(2000);
                if (Delete(SalesID))
                {
                    mdl_Sucess.Show();
                    FillgvSales();
                   
                    Clear();
                }
                else
                {
                    mdl_Fail.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.Page, "this filed linked With other data");
            }

        }
    }
    protected void gvSales_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvSales.PageIndex = e.NewPageIndex;
        FillgvSales();
    }
    protected void txtSalesDateM_TextChanged(object sender, EventArgs e)
    {
        if (txtSalesDateM.Text != string.Empty)
            txtSalesDateH.Text = CustomsConverter.GetHijriDate(txtSalesDateM.Text);
    }
    //protected void txtPQTY_TextChanged(object sender, EventArgs e)
    //{
    //    GridViewRow gvrow = (GridViewRow)((TextBox)sender).NamingContainer;
    //    CalculateTotalPrice(gvrow);
    //    CalculateTotalSales ();
    //}

    #region Products
    protected void gvProducts_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        int SalesDetailID = Convert.ToInt32(string.IsNullOrEmpty(e.CommandArgument.ToString()) ? "0" : e.CommandArgument);

        if (e.CommandName == "del")
        {
            int index = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
            if (SalesDetailID == 0)
            {

                FillgvProducts(RemoveRow(gvProducts, index));
            }
            else if (SalesDetailID > 0)
            {
                // here you must update balance of Customer 
                //............??????????????...........
                if (deleteSalesDetail( SalesDetailID))
                {
                    FillgvProducts(Convert.ToInt32(txtID.Text.Trim()));
                    if (gvProducts.Rows.Count == 0)
                        FillgvProducts(IntialgvProducts());
                }
            }
        }
    }


    protected void imgbtnAdd_Click(object sender, ImageClickEventArgs e)
    {
        FillgvProducts(AddRow(gvProducts));
    }
    //protected void txtQuantity_TextChanged(object sender, EventArgs e)
    //{
    //    GridViewRow gvrow = (GridViewRow)((TextBox)sender).NamingContainer;
    //    CalculateTotalPrice(gvrow);
    //    CalculateTotalSales();
    //}


    protected void txtPrice_TextChanged(object sender, EventArgs e)
    {

        GridViewRow gvrow = (GridViewRow)((TextBox)sender).NamingContainer;
        CalculateTotalPrice(gvrow);
        CalculateTotalSales();
    }
    protected void gvProducts_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            FillddlProduct(e.Row);
            FillddlUnits(e.Row);
           // CalculateTotalPrice(e.Row);
            
        }
    }
    protected void gvProducts_DataBound(object sender, EventArgs e)
    {
        CalculateTotalSales();
    }
    #endregion
    #endregion



    #region Search
    protected void imgbtnSearch_Click(object sender, ImageClickEventArgs e)
    {
        if (!string.IsNullOrEmpty(txtSearchValue.Text))
        {
            GetSales(int.Parse(txtSearchValue.Text.Trim()));
        }
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList(string prefixText, int count, string contextKey)
    {
        SalesMaster objSalesMaster = new SalesMaster();
        return objSalesMaster.Get(p => p.SalesID.ToString().ToLower().StartsWith(prefixText.ToLower()), p => p.SalesID.ToString()).ToArray();
    }

    #endregion



    protected void cboxIsCash_CheckedChanged(object sender, EventArgs e)
    {
        txtPayment.Enabled = (cboxIsCash.Checked == true) ? false : true;
        txtPayment.Text = (cboxIsCash.Checked == true) ? lblTotalSalesPrice.Text : "0";

        Totalcala();
    }
    protected void txtPayment_TextChanged(object sender, EventArgs e)
    {
        Totalcala();
    }
    protected void txtSUnitPrice_TextChanged(object sender, EventArgs e)
    {
        GridViewRow gvrow = (GridViewRow)((TextBox)sender).NamingContainer;
        CalculateTotalPrice(gvrow);
        CalculateTotalSales();
    }
    protected void txtSQTY_TextChanged(object sender, EventArgs e)
    {
        GridViewRow gvrow = (GridViewRow)((TextBox)sender).NamingContainer;
        CalculateTotalPrice(gvrow);
        CalculateTotalSales();

    }
    protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCustomer.SelectedIndex > 0)
        {
            RequiredFieldValidator1.ValidationGroup = "i";
            RequiredFieldValidator45.ValidationGroup = "none";
            txtCustomerName.ReadOnly = true;
            cboxIsCash.Checked = false;
            cboxIsCash.Enabled = true;
        }
        else if (ddlCustomer.SelectedIndex == 0)
        {
            RequiredFieldValidator1.ValidationGroup = "none";
            RequiredFieldValidator45.ValidationGroup = "i";
            txtCustomerName.ReadOnly = false;
            cboxIsCash.Checked = true;
            cboxIsCash.Enabled = false;
        }
        Totalcala();
    }
    protected void ddlUnits_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    
}
