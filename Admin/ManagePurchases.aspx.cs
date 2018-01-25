using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.ElkhateebDynamicLinq;
using System.Globalization;
using System.Data;
public partial class Admin_ManagePurchases : System.Web.UI.Page
{


    #region Objects
    PurchasesMaster objPurchasesMaster = new PurchasesMaster();
    spPurchasesMasterResult objspPurchasesMasterResult = new spPurchasesMasterResult();

    PurchasesDetail objPurchasesDetail = new PurchasesDetail();
    spPurchasesDetailsResult objspPurchasesDetailsResult = new spPurchasesDetailsResult();

    Supplier objSupplier = new Supplier();
    Employee objEmployee = new Employee();
    Product objProduct = new Product();
    Unit objunit = new Unit();
    CultureInfo culture = new CultureInfo("en-US");


    
   
   
    #endregion

    #region Methods

    void Clear()
    {
        txtID.Text = txtNotes.Text = txtPurchaseDateM.Text = txtPurchaseDateH.Text = string.Empty;
      hfOldRest.Value=  txtPayment.Text = lblRest.Text = lblSupplierBalance.Text = lblNewSupplierBalance.Text = "0";
        ddlSupplier.SelectedIndex = ddlEmployee.SelectedIndex = 0;
        cboxIsCash.Checked = false;
        gvPurchases.SelectedIndex = -1;
        

    }
    void FillddlSupplier()
    {
        var q = objSupplier.Get().ToList();
        ddlSupplier.DataSource = q;
        ddlSupplier.DataTextField = "SupplierName";
        ddlSupplier.DataValueField = "SupplierID";
        ddlSupplier.DataBind();
    }
    void FillddlEmployee()
    {
        var q = objEmployee.Get().ToList();
        ddlEmployee.DataSource = q;
        ddlEmployee.DataTextField = "EmployeeName";
        ddlEmployee.DataValueField = "EmployeeID";
        ddlEmployee.DataBind();
    }
    void FillgvPurchases()
    {
        object[] parameter = null;
        gvPurchases.DataSource = objspPurchasesMasterResult.Get(parameter).ToList();
        gvPurchases.DataBind();
    }
    void FillddlProduct(GridViewRow gvRow)
    {
        DropDownList ddlProduct = (DropDownList)gvRow.FindControl("ddlProduct");
        ddlProduct.DataSource = objProduct.Get().ToList();
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
    void FillgvProducts(List<PurchasesDetail> list)
    {
        gvProducts.DataSource = list;
        gvProducts.DataBind();
    }
    void FillgvProducts(int PurchaseID)
    {
        var list = objPurchasesDetail.Filter(p => p.PurchaseID == PurchaseID).ToList();
             list.ForEach(p => p.oldQty = p.Quantity);
             FillgvProducts(list);
    }
    void CalculateTotalPrice(GridViewRow gvrow)
    {

        TextBox txtPQTY = (TextBox)gvrow.FindControl("txtPQTY");
        TextBox txtPUnitPrice = (TextBox)gvrow.FindControl("txtPUnitPrice");
        TextBox txtTotalPrice = (TextBox)gvrow.FindControl("txtTotalPrice");

        //if (string.IsNullOrEmpty(txtPQTY.Text))
        //    txtPQTY.Text = "0";
        //if (string.IsNullOrEmpty(txtPUnitPrice.Text))
        //    txtPUnitPrice.Text = "0";


        txtTotalPrice.Text = Convert.ToString(Convert.ToDouble(!string.IsNullOrEmpty(txtPQTY.Text.Trim()) ? txtPQTY.Text.Trim() : "0.0") * Convert.ToDouble(!string.IsNullOrEmpty(txtPUnitPrice.Text.Trim()) ? txtPUnitPrice.Text.Trim() : "0.0"));
        

    }
    void CalculateTotalPurchase()
    {
        
            lblTotalPurchasePrice.Text = "0";
        foreach (GridViewRow gvrow in gvProducts.Rows)
        lblTotalPurchasePrice.Text = Convert.ToString(Convert.ToDouble(lblTotalPurchasePrice.Text) +Convert.ToDouble(((TextBox )gvrow.FindControl("txtTotalPrice")).Text  ));


        Totalcala();
    }
    bool Insert()
    {
        objPurchasesMaster = new PurchasesMaster();
       
        objPurchasesMaster.SupplierID = int.Parse(ddlSupplier.SelectedValue);
        objPurchasesMaster.EmployeeID = (ddlEmployee.SelectedIndex > 0) ? (Nullable<int>)int.Parse(ddlEmployee.SelectedValue) : null;
        objPurchasesMaster.Notes = txtNotes.Text.Trim();
        objPurchasesMaster.PurchaseDateM = Convert.ToDateTime(txtPurchaseDateM.Text.Trim(), culture);
        objPurchasesMaster.PurchaseDateH = txtPurchaseDateH.Text.Trim();

        objPurchasesMaster.TotalPurchase = Convert.ToDouble(lblTotalPurchasePrice.Text);
        objPurchasesMaster.Rest = Convert.ToDouble(lblRest.Text);

        double Payments = Convert.ToDouble(!string.IsNullOrEmpty(txtPayment.Text) ? txtPayment.Text : "0.0");
        objPurchasesMaster.Payments = Payments;
       

        objPurchasesMaster.ISCash = cboxIsCash.Checked;




        List<PurchasesDetail> Purchaseslist = new List<PurchasesDetail>();
        Purchaseslist = CurrentgvProducts(gvProducts);
        Purchaseslist.ForEach(p => {
            p.Quantity = SalesManager.UnitConvert(p.UnitID, p.PQTY);
            p.UnitPrice = SalesManager.UnitPriceConvert(p.UnitID, p.PUnitPrice);
            p.BasicUnitID = SalesManager.getBasicUnitID;
        
        });



        //double TotalPurchasePrice = Purchaseslist.Sum(p => p.TotalPrice);
        objPurchasesMaster.PurchasesDetails.AddRange(Purchaseslist);


        objPurchasesMaster.BeginTransaction();
       
        if (Payments > 0)
        {
            PaymentsForSupplier paymentforsupplier = new PaymentsForSupplier();
            paymentforsupplier.SupplierID = Convert.ToInt32(ddlSupplier.SelectedValue);
            paymentforsupplier.PaymentDateM = Convert.ToDateTime(txtPurchaseDateM.Text.Trim(), culture);
            paymentforsupplier.PaymentDateH = txtPurchaseDateH.Text.Trim();
            paymentforsupplier.Payment = Payments;
            objPurchasesMaster.SupplierPaymentID = Convert.ToInt32(paymentforsupplier.Insert());
        }

        objPurchasesMaster.Insert();

        // update qty product in store(table product)
        Purchaseslist.ForEach(p => SalesManager.UpdateproductQTY(QTYOperation.Increment, p.ProductID, p.Quantity));

        if (cboxIsCash.Checked != true)
        {
            objSupplier = objSupplier.Single(p => p.SupplierID == int.Parse(ddlSupplier.SelectedValue));
            objSupplier.Balance += objPurchasesMaster.Rest ;
            objSupplier.Update(p => p.SupplierID == objSupplier.SupplierID);
        }


        objPurchasesMaster.EndTransaction();

        
        
        return true;
    }
    bool Update()
    {
        objPurchasesMaster = new PurchasesMaster();
      
       
       objPurchasesMaster= objPurchasesMaster.Single(p => p.PurchaseID == Convert.ToInt32 (txtID.Text.Trim()));

        //objPurchasesMaster.SupplierID = int.Parse(ddlSupplier.SelectedValue);
        objPurchasesMaster.EmployeeID = (ddlEmployee.SelectedIndex > 0) ? (Nullable<int>)int.Parse(ddlEmployee.SelectedValue) : null;
        objPurchasesMaster.Notes = txtNotes.Text.Trim();
        objPurchasesMaster.PurchaseDateM = Convert.ToDateTime(txtPurchaseDateM.Text.Trim(), culture);
        objPurchasesMaster.PurchaseDateH = txtPurchaseDateH.Text.Trim();
        objPurchasesMaster.ISCash = cboxIsCash.Checked;

        double oldrest = objPurchasesMaster.Rest;
        bool oldPaymentType = objPurchasesMaster.ISCash;
        int?  paymentid = objPurchasesMaster.SupplierPaymentID;

        objPurchasesMaster.TotalPurchase = Convert.ToDouble(lblTotalPurchasePrice.Text);
        objPurchasesMaster.Rest = Convert.ToDouble(lblRest.Text);

        double Payments = Convert.ToDouble(!string.IsNullOrEmpty(txtPayment.Text) ? txtPayment.Text : "0.0");
        objPurchasesMaster.Payments = Payments;

        


        objPurchasesMaster.BeginTransaction();
        PaymentsForSupplier paymentforsupplier = new PaymentsForSupplier();
        if (paymentid == null)
        {
            if (Payments > 0)
            {
                
                paymentforsupplier.SupplierID = Convert.ToInt32(ddlSupplier.SelectedValue);
                paymentforsupplier.PaymentDateM = Convert.ToDateTime(txtPurchaseDateM.Text.Trim(), culture);
                paymentforsupplier.PaymentDateH = txtPurchaseDateH.Text.Trim();
                paymentforsupplier.Payment = Payments;
                objPurchasesMaster.SupplierPaymentID = Convert.ToInt32(paymentforsupplier.Insert());
            }
        }
        else if (paymentid != null)
        {
            paymentforsupplier = objPurchasesMaster.PaymentsForSupplier;
            paymentforsupplier.PaymentDateM = Convert.ToDateTime(txtPurchaseDateM.Text.Trim(), culture);
            paymentforsupplier.PaymentDateH = txtPurchaseDateH.Text.Trim();
            paymentforsupplier.Payment = Payments;
            paymentforsupplier.Update(p => p.SupplierPaymentID == paymentforsupplier.SupplierPaymentID);
        }

        objPurchasesMaster.Update(p => p.PurchaseID == objPurchasesMaster.PurchaseID);

        //var q=objPurchasesDetail.Filter(p => p.PurchaseID == objPurchasesMaster.PurchaseID).Select(p => (p.TotalPrice ));
        //double oldTotalPurchasePrice =(q!=null )?q.Sum():0.0; 
        
       
        List<PurchasesDetail> Purchaseslist = CurrentgvProducts(gvProducts);
        Purchaseslist.ForEach(p =>
        {
            p.Quantity = SalesManager.UnitConvert(p.UnitID, p.PQTY);
            p.UnitPrice = SalesManager.UnitPriceConvert(p.UnitID, p.PUnitPrice);
            p.BasicUnitID = SalesManager.getBasicUnitID;

        });

        //double TotalPurchasePrice = Purchaseslist.Sum(p => p.TotalPrice );
        List<PurchasesDetail> newPurchaseslist = Purchaseslist.Where(p => p.PurchasesDetailID == 0).ToList();
        List<PurchasesDetail> updatePurchaseslist = Purchaseslist.Where(p => p.PurchasesDetailID > 0).ToList();



        if (newPurchaseslist.Count > 0)
        {
            newPurchaseslist.ForEach(p => p.PurchaseID = objPurchasesMaster.PurchaseID);
            newPurchaseslist.InsertAll();

            // update qty product in store(table product)
            newPurchaseslist.ForEach(p => SalesManager.UpdateproductQTY(QTYOperation.Increment, p.ProductID, p.Quantity));


        }
        if (updatePurchaseslist.Count > 0)
        {

            updatePurchaseslist.ForEach(p =>
            {


                p.Update(z => z.PurchasesDetailID == p.PurchasesDetailID);
                SalesManager.UpdateproductQTY(QTYOperation.Increment, p.ProductID, (p.Quantity - p.oldQty));
            });
        }






        //========= composite calcualtion balance --- compare between old value and new  value 
        if (oldPaymentType != true)// not cash
        {

            // any way( - balance) form old supplier if user change supplier 
            objSupplier = objSupplier.Single(p => p.SupplierID == objPurchasesMaster.SupplierID );
            objSupplier.Balance -= oldrest;
           // objSupplier.Update(p => p.SupplierID == objSupplier.SupplierID);

            if (cboxIsCash.Checked != true)// if (not cash) add new price to balance 
            {      

                //objSupplier = objSupplier.Single(p => p.SupplierID == int.Parse(ddlSupplier.SelectedValue));
                objSupplier.Balance += objPurchasesMaster.Rest ;
                
            }
            objSupplier.Update(p => p.SupplierID == objSupplier.SupplierID);
        }
        else if (oldPaymentType == true)//  cash
        {

            if (cboxIsCash.Checked != true)  // if user change to (not cash) will add new price to supplier balance
            {

                objSupplier = objSupplier.Single(p => p.SupplierID == int.Parse(ddlSupplier.SelectedValue));
                objSupplier.Balance += objPurchasesMaster.Rest ;
                objSupplier.Update(p => p.SupplierID == objSupplier.SupplierID);
            }
        }

        objPurchasesMaster.EndTransaction();
      
        return true;
    }
    void Dir(PurchasesMaster entity)
    {
        txtID.Text = entity.PurchaseID.ToString();

        ddlSupplier.SelectedValue = entity.SupplierID.ToString();

        ddlSupplier.Enabled = false;

        ddlEmployee.SelectedValue = (entity.EmployeeID != null) ? entity.EmployeeID.Value.ToString() : "0";
        txtNotes.Text = entity.Notes;
        txtPurchaseDateM.Text = entity.PurchaseDateM.Value.ToString("MM/dd/yyyy", culture);
        txtPurchaseDateH.Text = entity.PurchaseDateH;
        cboxIsCash.Checked = entity.ISCash;

        //oldsupplierid = entity.SupplierID;
        //oldPaymentType = entity.ISCash;



        FillgvProducts(entity.PurchaseID);

        lblTotalPurchasePrice.Text = entity.TotalPurchase.ToString();
        txtPayment.Text = entity.Payments.ToString();
       hfOldRest.Value= lblRest.Text = entity.Rest.ToString();
        lblSupplierBalance.Text = lblNewSupplierBalance.Text = entity.Supplier.Balance.ToString();
       
    }

    bool Delete(int PurchaseID)
    {
        objPurchasesMaster = objPurchasesMaster.Single(p => p.PurchaseID == PurchaseID);
        // totalpurchase-payments
        Double rest = objPurchasesMaster.Rest;//محصله الفاتوره النهائيه الا هى عباره عن اجمالى الفاتوره ناقص مدفوع العميل
        List<PurchasesDetail> itemslist = objPurchasesMaster.PurchasesDetails.ToList();
        var payments = objPurchasesMaster.PaymentsForSupplier;
        objSupplier = objPurchasesMaster.Supplier;


        try
        {
            objPurchasesMaster.BeginTransaction();

            //update supplier balance
            objSupplier.Balance -= rest;
            objSupplier.Update(p => p.SupplierID == objSupplier.SupplierID);
            //update products (store)
            itemslist.ForEach(p =>
                {
                    //update qty
                    SalesManager.UpdateproductQTY(QTYOperation.Decrement, p.ProductID, p.Quantity);
                }
            );


           

            // delete purchaseitems(PurchasesDetails)
            itemslist.DeleteAll();



            //delete masetr(PurchasesMaster)

            objPurchasesMaster.Delete(p => p.PurchaseID == PurchaseID);
            //delete payments
            if (payments != null)
                payments.Delete(p => p.SupplierPaymentID == payments.SupplierPaymentID);
            objPurchasesMaster.EndTransaction();
        }
        catch (Exception ex)
        {
            ExtenssionClass.Rollback();
            Page.Show(ex.Message);
            return false;
        }
        return true;

    }
    bool deletePurchasesDetail(int PurchasesDetailID)
    {
        objPurchasesDetail.BeginTransaction();
        objPurchasesDetail = objPurchasesDetail.Single(p => p.PurchasesDetailID == PurchasesDetailID);
        //var payment = objPurchasesDetail.PurchasesMaster.PaymentsForSupplier;
        //payment.Delete(p => p.SupplierPaymentID == payment.SupplierPaymentID);

        objPurchasesMaster = objPurchasesDetail.PurchasesMaster;
        objPurchasesMaster.TotalPurchase -= objPurchasesDetail.TotalPrice;
        objPurchasesMaster.Rest -= objPurchasesDetail.TotalPrice;
        objPurchasesMaster.Update(p => p.PurchaseID == objPurchasesMaster.PurchaseID);
        hfOldRest.Value = objPurchasesMaster.Rest.ToString();
        // update balance
        objSupplier = objSupplier.Single(p => p.SupplierID == objPurchasesDetail.PurchasesMaster.SupplierID);
        objSupplier.Balance = (objSupplier.Balance) - (objPurchasesDetail.TotalPrice );
        objSupplier.Update(p => p.SupplierID == objSupplier.SupplierID);


        //update qty
        SalesManager.UpdateproductQTY(QTYOperation.Decrement, objPurchasesDetail.ProductID, objPurchasesDetail.Quantity);

        bool status = objPurchasesDetail.Delete(p => p.PurchasesDetailID == PurchasesDetailID);
        objPurchasesDetail.EndTransaction();
        return status;
    }


    void GetPurchase(int PurchaseID)
    {
        object[] parameter = null;
        gvPurchases.DataSource = objspPurchasesMasterResult.Filter(parameter, p => p.PurchaseID == PurchaseID).ToList();
        gvPurchases.DataBind();
    }
    #endregion


    #region  DataTable

    List<PurchasesDetail> IntialgvProducts()
    {
       
        List<PurchasesDetail> list = new List<PurchasesDetail>();
        PurchasesDetail objPurchasesDetail = new PurchasesDetail();

        list.Add(objPurchasesDetail);

        return list;
    }
    List<PurchasesDetail> CurrentgvProducts(GridView gv)
    {

        List<PurchasesDetail> list = new List<PurchasesDetail>();
        foreach (GridViewRow gvrow in gv.Rows)
        {

            PurchasesDetail objPurchasesDetail = new PurchasesDetail();
            string value = "";
            value = (gvrow.FindControl("lblPurchasesDetailID") as Label).Text;
            objPurchasesDetail.PurchasesDetailID = Convert.ToInt32(((string.IsNullOrEmpty(value)) ? "0" : value));

            value = (gvrow.FindControl("lblPurchaseID") as Label).Text;
            objPurchasesDetail.PurchaseID = Convert.ToInt32(((string.IsNullOrEmpty(value)) ? "0" : value));

            value = (gvrow.FindControl("ddlProduct") as DropDownList).SelectedValue;
            objPurchasesDetail.ProductID = Convert.ToInt32(((string.IsNullOrEmpty(value)) ? "0" : value));
            value = (gvrow.FindControl("ddlUnits") as DropDownList).SelectedValue;
            objPurchasesDetail.UnitID = Convert.ToInt32(((string.IsNullOrEmpty(value)) ? "0" : value));

            objPurchasesDetail.PQTY = Convert.ToDouble((gvrow.FindControl("txtPQTY") as TextBox).Text);

            objPurchasesDetail.PUnitPrice = Convert.ToDouble((gvrow.FindControl("txtPUnitPrice") as TextBox).Text);

            objPurchasesDetail.TotalPrice = Convert.ToDouble((gvrow.FindControl("txtTotalPrice") as TextBox).Text);

            value = (gvrow.FindControl("hfoldQty") as HiddenField).Value;

            objPurchasesDetail.oldQty = (string.IsNullOrEmpty(value)) ? 0.0 : Convert.ToDouble(value); ;//hfQuantity

            list.Add(objPurchasesDetail);

        }

        return list;


    }
    List<PurchasesDetail> AddRow(GridView gv)
    {

        List<PurchasesDetail> list = CurrentgvProducts(gv);
        //var q = list.Select(p => new { PurchaseID = p.PurchaseID, ProductID = p.ProductID }).GroupBy(p => p.ProductID)
        //.Select(e => new { ProductID = e.Key, Count = e.Count() }).Where(p => p.Count > 1);
        //if (q.Count() > 0)
        //{
        //    MessageBox.Show(this.Page, "Cant Insert same Product more than once in Purchases.");
        //    return list;
        //}

        PurchasesDetail objPurchasesDetail = new PurchasesDetail();

        list.Add(objPurchasesDetail);
        return list;
    }
    List<PurchasesDetail> RemoveRow(GridView gv, int RowIndex)
    {

        List<PurchasesDetail> list = CurrentgvProducts(gv);
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
            this.Title = "Manage Purchases ";
            FillddlSupplier();
            FillddlEmployee();
            FillgvPurchases();
            FillgvProducts(IntialgvProducts());
        }
       
    }
    protected void imgbtnSave_Click(object sender, ImageClickEventArgs e)
    {
        System.Threading.Thread.Sleep(2000);
       
        if (gvPurchases.SelectedIndex == -1)
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
        else if (gvPurchases.SelectedIndex > -1)
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

        FillgvPurchases();
        FillgvProducts(IntialgvProducts());
        Clear();
    }
    protected void imgbtnClear_Click(object sender, ImageClickEventArgs e)
    {
        FillgvProducts(IntialgvProducts());
        FillgvPurchases();
        Clear();

    }
    protected void gvPurchases_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int PurchaseID = Convert.ToInt32(e.CommandArgument);
        if (e.CommandName == "select")
        {
            Dir(objPurchasesMaster.Single(p => p.PurchaseID == PurchaseID));
        }
        else if (e.CommandName == "del")
        {
            try
            {
                System.Threading.Thread.Sleep(2000);
                if (Delete(PurchaseID))
                {
                    mdl_Sucess.Show();
                    FillgvPurchases();
                   
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
    protected void gvPurchases_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvPurchases.PageIndex = e.NewPageIndex;
        FillgvPurchases();
    }
    protected void txtPurchaseDateM_TextChanged(object sender, EventArgs e)
    {
        if (txtPurchaseDateM.Text != string.Empty)
            txtPurchaseDateH.Text = CustomsConverter.GetHijriDate(txtPurchaseDateM.Text);
    }


    #region Products
    protected void gvProducts_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        int PurchasesDetailID = Convert.ToInt32(string.IsNullOrEmpty(e.CommandArgument.ToString()) ? "0" : e.CommandArgument);

        if (e.CommandName == "del")
        {
            int index = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
            if (PurchasesDetailID == 0)
            {

                FillgvProducts(RemoveRow(gvProducts, index));
            }
            else if (PurchasesDetailID > 0)
            {
                // here you must update balance of supplier 
                //............??????????????...........
                if (deletePurchasesDetail( PurchasesDetailID))
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
    //    CalculateTotalPurchase();
    //}


    protected void txtPUnitPrice_TextChanged(object sender, EventArgs e)
    {

        GridViewRow gvrow = (GridViewRow)((TextBox)sender).NamingContainer;
        CalculateTotalPrice(gvrow);
        CalculateTotalPurchase();
    }
    protected void gvProducts_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            FillddlProduct(e.Row);
            FillddlUnits(e.Row);
            //CalculateTotalPrice(e.Row);
            
        }
    }
    protected void gvProducts_DataBound(object sender, EventArgs e)
    {
        CalculateTotalPurchase();
    }
    #endregion
    #endregion


    #region Search
    protected void imgbtnSearch_Click(object sender, ImageClickEventArgs e)
    {
        if (!string.IsNullOrEmpty(txtSearchValue.Text))
        {
            GetPurchase(int.Parse(txtSearchValue.Text.Trim()));
        }
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList(string prefixText, int count, string contextKey)
    {
        PurchasesMaster objPurchasesMaster = new PurchasesMaster();
        return objPurchasesMaster.Get(p => p.PurchaseID.ToString().ToLower().StartsWith(prefixText.ToLower()), p => p.PurchaseID.ToString()).ToArray();
    }

    #endregion


    protected void cboxIsCash_CheckedChanged(object sender, EventArgs e)
    {
       
            txtPayment.Enabled =(cboxIsCash.Checked == true)? false:true ;
            txtPayment.Text = (cboxIsCash.Checked == true)?lblTotalPurchasePrice.Text:"0";

            Totalcala();
    }
    protected void ddlSupplier_SelectedIndexChanged(object sender, EventArgs e)
    {
        Totalcala();
    }
    protected void txtPQTY_TextChanged(object sender, EventArgs e)
    {
        GridViewRow gvrow = (GridViewRow)((TextBox)sender).NamingContainer;
        CalculateTotalPrice(gvrow);
        CalculateTotalPurchase();
    }
    protected void ddlUnits_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void txtPayment_TextChanged(object sender, EventArgs e)
    {
        Totalcala();
    }

    void Totalcala()
    {
        if (ddlSupplier.SelectedIndex > 0)
        {
            lblRest.Text = (Convert.ToDouble(lblTotalPurchasePrice.Text) - Convert.ToDouble(!string.IsNullOrEmpty(txtPayment.Text) ? txtPayment.Text : "0.0")).ToString();

            int supplierid = Convert.ToInt32(ddlSupplier.SelectedValue);
            var entity = objSupplier.Single(p => p.SupplierID == supplierid);

            lblSupplierBalance.Text = entity.Balance.ToString();

            lblNewSupplierBalance.Text = ((Convert.ToDouble(lblRest.Text) + entity.Balance) - Convert.ToDouble(hfOldRest.Value )).ToString();

          
        }
        else
        {
            lblSupplierBalance.Text = lblNewSupplierBalance.Text = "0";
        }
    }
}
