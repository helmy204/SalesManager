using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.ElkhateebDynamicLinq;
using System.Globalization;
using System.Threading;

public partial class Admin_ManagePurchaseDiscards : System.Web.UI.Page
{

    // what if more than one discard for the same purchase ???

    #region Objects

    PurchasesMaster objPurchasesMaster = new PurchasesMaster();
    spPurchasesMasterResult objspPurchasesMasterResult = new spPurchasesMasterResult();

    PurchasesDetail objPurchasesDetail = new PurchasesDetail();

    PurchaseDiscardsMaster objPurchaseDiscardsMaster = new PurchaseDiscardsMaster();
    spPurchaseDiscardsMasterResult objspPurchaseDiscardsMasterResult = new spPurchaseDiscardsMasterResult();

    PurchaseDiscardsDetail objPurchaseDiscardsDetail = new PurchaseDiscardsDetail();

    Supplier objSupplier = new Supplier();

    CultureInfo culture = new CultureInfo("en-US");

    static int index;

    #endregion

    #region Methods

    void FillgvProducts(object list)
    {
        gvProducts.DataSource = list;
        gvProducts.DataBind();

    }

    void FillgvProducts(int purchaseID)
    {
        var list = objPurchasesDetail.Filter(p => p.PurchaseID == purchaseID).ToList().
                   Select( p=>new
                   { 
                       ProductName = p.Product.ProductName,
                                ProductID=p.ProductID,
                       PQTY = p.PQTY - GetReturnProductQTY(p.PurchaseID,p.ProductID)
                                , 
                                UnitName = p.Product.Unit.UnitName_En,
                                UnitID=p.UnitID,
                                PUnitPrice = p.PUnitPrice,
                                TotalPrice = 0.0,
                                PDQTY=0.0 
                   }
                                ).ToList()
                                ;

        FillgvProducts(list);
        //(p.PurchasesMaster.PurchaseDiscardsMasters==null && p.PurchasesMaster.PurchaseDiscardsMasters.Count ==0 )? (p.PQTY):
        //                        (
        //                            p.PQTY -
        //                            (
        //                            p.PurchasesMaster.PurchaseDiscardsMasters.Select
        //                            (
                                 
        //                         z => new { PDQTY=z.PurchaseDiscardsDetails.Where(f=>f.ProductID==p.ProductID).Sum(a=>a.PDQTY) }
                                     
        //                          ).First().PDQTY)
        //                          )
    }


     double  GetReturnProductQTY(int PurchaseID,int ProductID)
    {
        var q = (new PurchaseDiscardsDetail()).Filter(p => p.PurchaseDiscardsMaster!=null && p.PurchaseDiscardsMaster.PurchaseID == PurchaseID && p.ProductID == ProductID).ToList();
        if (q.Count > 0)
        {
            var q1 = q.Select(p => p.PDQTY).Sum();
            return q1;
        }
        return 0.0;
    }


    /// <summary>
    /// ///////////////////
    /// </summary>
    /// <param name="discardID"></param>
    void FillgvProductsDiscarded(int discardID)
    {
        var list = from p in objPurchaseDiscardsDetail.Filter(p => p.PurchaseDiscardsID == discardID).ToList()
                   select new
                   {
                       ProductName=p.Product.ProductName,
                       ProductID=p.ProductID,
                       PQTY="",
                       UnitName=p.Unit.UnitName_En,
                       UnitID=p.UnitID,
                       PUnitPrice=p.PUnitPrice,
                       TotalPrice=p.TotalPrice,
                       PDQTY=p.PDQTY
                   };

        FillgvProducts(list);
    }

    /// <summary>
    /// ///////////////////
    /// </summary>
    /// <param name="purchaseID"></param>
    void EditgvProducts(int purchaseID)
    {
        var q = objPurchasesDetail.Filter(p => p.PurchaseID == purchaseID).ToList();

        int index = 0;

        foreach (GridViewRow gvrow in gvProducts.Rows)
        {
            if (index < gvProducts.Rows.Count)
            {
                TextBox txtPQTY = gvrow.FindControl("txtPQTY") as TextBox;
                //txtPQTY.Text= ( q[index].PQTY).ToString();

                txtPQTY.Text = GetReturnProductQTY(purchaseID, q[0].ProductID).ToString();

            //    Label lblTotalPrice = gvrow.FindControl("lblTotalPrice") as Label;
            //    lblTotalPrice.Text = (q[index].TotalPrice).ToString();

                TextBox txtPDQTY = gvrow.FindControl("txtPDQTY") as TextBox;
             //   txtPDQTY.ReadOnly = true;
                if (txtPDQTY.Text != string.Empty)
                {
                    CheckBox chkDiscard = gvrow.FindControl("chkDiscard") as CheckBox;
                    chkDiscard.Checked = true;
                    chkDiscard.Enabled = false;
                }

                index++;
            }

        }
    }

    void FillgvPurchaseDiscards()
    {
        object[] parameter = null;
        gvPurchaseDiscards.DataSource = objspPurchaseDiscardsMasterResult.Get(parameter).ToList();
        gvPurchaseDiscards.DataBind();
    }

    bool Insert()
    {
        objPurchaseDiscardsMaster = new PurchaseDiscardsMaster();

        objPurchaseDiscardsMaster.PurchaseID = Convert.ToInt32( hfPID.Value);
        objPurchaseDiscardsMaster.PurchaseDiscardsDateM = Convert.ToDateTime( txtPurchaseDiscardDateM.Text.Trim(),culture );
        objPurchaseDiscardsMaster.PurchaseDiscardsDateH = txtPurchaseDiscardDateH.Text.Trim();
        objPurchaseDiscardsMaster.Notes = txtNotes.Text.Trim();

        objPurchaseDiscardsMaster.TotalReturn = Convert.ToDouble(lblTotalPurchaseDiscardsPrice.Text);

        List<PurchaseDiscardsDetail> discardsList = new List<PurchaseDiscardsDetail>();
        discardsList = CurrentgvProducts(gvProducts);

        discardsList.ForEach(p =>
        {
            p.Quantity = SalesManager.UnitConvert(p.UnitID, p.PDQTY);
            p.UnitPrice = SalesManager.UnitPriceConvert(p.UnitID, p.PUnitPrice);
            p.BasicUnitID = SalesManager.getBasicUnitID;
        });

        objPurchaseDiscardsMaster.PurchaseDiscardsDetails.AddRange(discardsList);

        objPurchaseDiscardsMaster.BeginTransaction();

        objPurchaseDiscardsMaster.Insert();

        // update product qty in store ( table product )
        discardsList.ForEach(p => SalesManager.UpdateproductQTY(QTYOperation.Decrement, p.ProductID, p.Quantity));

        // update supplier balance
        objSupplier = objSupplier.Single(p => p.SupplierID == int.Parse(hfSID.Value));
        objSupplier.Balance = Convert.ToDouble(lblNewSupplierBalance.Text);
        objSupplier.Update(p => p.SupplierID == objSupplier.SupplierID);

        objPurchaseDiscardsMaster.EndTransaction();

        return true;
    }

    void Dir(PurchaseDiscardsMaster entity)
    {
        txtID.Text = entity.PurchaseDiscardsID.ToString();

        txtPurchaseNo.Text = entity.PurchaseID.ToString();
        txtSupplierName.Text = entity.PurchasesMaster.Supplier.SupplierName;
        txtNotes.Text = entity.Notes;

        txtPurchaseDiscardDateM.Text = entity.PurchaseDiscardsDateM.ToString("MM/dd/yyyy", culture);
        txtPurchaseDiscardDateH.Text = entity.PurchaseDiscardsDateH;

        FillgvProductsDiscarded(entity.PurchaseDiscardsID);
        EditgvProducts(entity.PurchaseID);

        lblTotalPurchaseDiscardsPrice.Text = entity.TotalReturn.ToString();
        // ???
        lblSupplierBalance.Text = lblNewSupplierBalance.Text = (entity.PurchasesMaster.Supplier.Balance).ToString();
        //lblSupplierBalance.Text = lblNewSupplierBalance.Text = "0.0";
        txtPurchaseNo.ReadOnly = txtNotes.ReadOnly = true;
         imgbtnSave.Enabled = txtPurchaseDiscardDateM.Enabled = false;
    }

    bool Delete(int discardsID)
    {
        objPurchaseDiscardsMaster = objPurchaseDiscardsMaster.Single(p => p.PurchaseDiscardsID == discardsID);

        List<PurchaseDiscardsDetail> itemsList = objPurchaseDiscardsMaster.PurchaseDiscardsDetails.ToList();

        objSupplier = objPurchaseDiscardsMaster.PurchasesMaster.Supplier;

        try
        {
            objPurchaseDiscardsMaster.BeginTransaction();


            //update Supplier balance
            // هيزيد
            objSupplier.Balance += objPurchaseDiscardsMaster.TotalReturn;
            objSupplier.Update(p => p.SupplierID == objSupplier.SupplierID);


            //update products (store)
            itemsList.ForEach(p =>
            {
            //update qty
            //هيزيد
                SalesManager.UpdateproductQTY(QTYOperation.Increment, p.ProductID,p.Quantity);
            }
            );

            // delete purchaseitems(PurchasesDetails)
            itemsList.DeleteAll();

            //delete masetr(PurchasesMaster)
            objPurchaseDiscardsMaster.Delete(p => p.PurchaseDiscardsID == discardsID);

            objPurchaseDiscardsMaster.EndTransaction();
        }
        catch (Exception ex)
        {
            ExtenssionClass.Rollback();
            Page.Show(ex.Message);
            return false;
        }
        return true;

    }

    void CalculateTotalPrice(GridViewRow gvrow)
    {
        CheckBox chkDiscard = gvrow.FindControl("chkDiscard") as CheckBox;

        if (chkDiscard.Checked == true)
        {
            TextBox txtPDQTY = gvrow.FindControl("txtPDQTY") as TextBox;
            double pdQTY = Convert.ToDouble((string.IsNullOrEmpty( txtPDQTY.Text))?"0.0":txtPDQTY.Text);
            //double pQTY = Convert.ToDouble((gvrow.FindControl("lblPQty") as Label).Text);
            //double newQTY = pQTY - pdQTY;

            Label lblPUnitPrice = gvrow.FindControl("lblPUnitPrice") as Label;
            Label lblTotalPrice = gvrow.FindControl("lblTotalPrice") as Label;

            //if (newQTY > 0)
            //{
            lblTotalPrice.Text = Convert.ToString(pdQTY * Convert.ToDouble(lblPUnitPrice.Text));
            //}
        }
    }

    void CalculateTotalPurchase()
    {
        lblTotalPurchaseDiscardsPrice.Text = "0";

        foreach (GridViewRow gvrow in gvProducts.Rows)
        {
            CheckBox chkDiscard = gvrow.FindControl("chkDiscard") as CheckBox;

            if (chkDiscard.Checked == true)
            {
                TextBox txtPDQTY = gvrow.FindControl("txtPDQTY") as TextBox;
                double pdQTY = Convert.ToDouble((string.IsNullOrEmpty(txtPDQTY.Text)) ? "0" : txtPDQTY.Text);
                double unitPrice = Convert.ToDouble(((gvrow.FindControl("lblPUnitPrice") as Label).Text));

                double totalRow = pdQTY * unitPrice;

                lblTotalPurchaseDiscardsPrice.Text = Convert.ToString( Convert.ToDouble( lblTotalPurchaseDiscardsPrice.Text) + totalRow);
            }
        }

        TotalCalc();
    }

    void TotalCalc()
    {
        int sID = Convert.ToInt32( hfSID.Value);

        var entity = objSupplier.Single(p => p.SupplierID == sID);

        lblSupplierBalance.Text = entity.Balance.ToString();

        lblNewSupplierBalance.Text = Convert.ToString( entity.Balance - Convert.ToDouble( lblTotalPurchaseDiscardsPrice.Text));
    }

    void Clear()
    {
        txtID.Text = txtNotes.Text = txtPurchaseDiscardDateM.Text = txtPurchaseDiscardDateH.Text = txtPurchaseNo.Text = txtSupplierName.Text = string.Empty;
        hfPID.Value = hfSID.Value = lblTotalPurchaseDiscardsPrice.Text = lblSupplierBalance.Text = lblNewSupplierBalance.Text = lblTotalPurchaseDiscardsPrice.Text = "0";
        ddlSearch.SelectedIndex = 0;
        gvPurchaseDiscards.SelectedIndex = -1;
        gvProducts.DataBind();
        txtPurchaseNo.ReadOnly = txtNotes.ReadOnly = false;
        imgbtnSave.Enabled = txtPurchaseDiscardDateM.Enabled = true;
    }

    //void GetTotalQuantityDiscarded(int purchaseID, int productID)
    //{
    //    //var list = objPurchaseDiscardsDetail.Filter(p => p.PurchaseDiscardsMaster.PurchaseID == purchaseID).Where(p=>p.ProductID==productID).ToList();

    //    var list = objPurchaseDiscardsDetail.Filter(p => p.PurchaseDiscardsMaster.PurchaseID == purchaseID).ToList()
            
    //        .Select(p=> new { ProductName =  p.Product.ProductName,
    //        ProductID=p.ProductID,
    //        PQTY=p.PDQTY,
    //        UnitName=p.Product.Unit.UnitName_En,
    //        UnitID=p.Product.UnitID,
    //        PUnitPrice=p.PUnitPrice,
    //        TotalPrice="0.0",
    //        PDQTY=""
    //        })
    //        ;

        
    //}

    #endregion

    #region  DataTable

    List<PurchaseDiscardsDetail> CurrentgvProducts(GridView gv)
    {
        List<PurchaseDiscardsDetail> list = new List<PurchaseDiscardsDetail>();

        foreach (GridViewRow gvrow in gv.Rows)
        {
            CheckBox chk = gvrow.FindControl("chkDiscard") as CheckBox;
            PurchaseDiscardsDetail objDiscardDetail = new PurchaseDiscardsDetail();
            string value = "";

            if (chk.Checked == true)
            {
                value = (gvrow.FindControl("lblProductID") as Label).Text;
                objDiscardDetail.ProductID = Convert.ToInt32(value);

                value = (gvrow.FindControl("txtPDQTY") as TextBox).Text;
                objDiscardDetail.PDQTY = Convert.ToDouble(value);

                value = (gvrow.FindControl("lblUnitID") as Label).Text;
                objDiscardDetail.UnitID = Convert.ToInt32(value);

                objDiscardDetail.PUnitPrice = Convert.ToDouble((gvrow.FindControl("lblPUnitPrice") as Label).Text);


                double pdQTY = Convert.ToDouble((gvrow.FindControl("txtPDQTY") as TextBox).Text);
                double unitPrice = Convert.ToDouble(((gvrow.FindControl("lblPUnitPrice") as Label).Text));
                double totalRow = pdQTY * unitPrice;
                objDiscardDetail.TotalPrice = totalRow;

                list.Add(objDiscardDetail);
            }
        }
        return list;
    }

    #endregion

    #region Events

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.Title = "Manage Purchase Discards";

            gvProducts.DataBind();
            FillgvPurchaseDiscards();
        }
    }

    protected void txtPurchaseDiscardDateM_TextChanged(object sender, EventArgs e)
    {
        if (txtPurchaseDiscardDateM.Text != string.Empty)
            txtPurchaseDiscardDateH.Text = CustomsConverter.GetHijriDate(txtPurchaseDiscardDateM.Text);
    }

    protected void imgbtnSave_Click(object sender, ImageClickEventArgs e)
    {
        Thread.Sleep(2000);

        if (gvPurchaseDiscards.SelectedIndex == -1)
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
        FillgvPurchaseDiscards();
        Clear();
    }

    protected void imgbtnClear_Click(object sender, ImageClickEventArgs e)
    {
        FillgvPurchaseDiscards();

        Clear();
    }

    protected void gvPurchaseDiscards_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int discardsID = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "select")
            {         
                Dir(objPurchaseDiscardsMaster.Single(p=>p.PurchaseDiscardsID==discardsID));
            }
            else if (e.CommandName == "del")
            {
                try
                {
                    Thread.Sleep(2000);
                    if (Delete(discardsID))
                    {
                        mdl_Sucess.Show();
                        FillgvPurchaseDiscards();

                        Clear();
                    }
                    else
                    {
                        mdl_Fail.Show();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this.Page, "this fieled linked With other data");
                }

            }
    }

    protected void gvPurchaseDiscards_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvPurchaseDiscards.PageIndex = e.NewPageIndex;
            FillgvPurchaseDiscards();
    }

    #region Products

    protected void gvProducts_RowCommand(object sender, GridViewCommandEventArgs e)
    {
    }

    protected void txtPDQTY_TextChanged(object sender, EventArgs e)
    {
        GridViewRow gvrow = (GridViewRow)((TextBox)sender).NamingContainer;

        TextBox txtPDQTY = gvrow.FindControl("txtPDQTY") as TextBox;
        //CheckBox chkDiscard = gvrow.FindControl("chkDiscard") as CheckBox;


        CalculateTotalPrice(gvrow);
        CalculateTotalPurchase();

        //if (txtPDQTY.Text != string.Empty)
        //{
            
        //    chkDiscard.Checked = true;
        //    gvrow.RowState = DataControlRowState.Selected;

            
        //}
        //else if (txtPDQTY.Text == string.Empty)
        //{
        //    chkDiscard.Checked = false;
        //    gvrow.RowState = DataControlRowState.Normal;
        //}

    }

    protected void chkDiscard_CheckedChanged(object sender, EventArgs e)
    {
        GridViewRow gvrow = (GridViewRow)((CheckBox)sender).NamingContainer;
        CheckBox chkDiscard = gvrow.FindControl("chkDiscard") as CheckBox;

        TextBox txtPDQTY = gvrow.FindControl("txtPDQTY") as TextBox;

        

        if (chkDiscard.Checked == true)
        {
            gvrow.RowState = DataControlRowState.Selected;
            txtPDQTY.ReadOnly = false;
        }
        else if (chkDiscard.Checked == false)
        {
            //TextBox txtPDQTY = gvrow.FindControl("txtPDQTY") as TextBox;
            txtPDQTY.Text = string.Empty;
            gvrow.RowState = DataControlRowState.Normal;

            
            txtPDQTY.ReadOnly = true;
        }

    }

    #endregion

    #endregion

    #region Search

    // Search For Purchase Number
    protected void imgbtnSearchPurchase_Click(object sender, ImageClickEventArgs e)
    {
        if (!string.IsNullOrEmpty(txtPurchaseNo.Text))
        {
            int purchaseID = int.Parse(txtPurchaseNo.Text.Trim());
            //if (!IsExist(purchaseID))
            //{
                GetPurchaseNo(purchaseID);
            //}
        }
    }

    bool GetPurchaseNo(int PurchaseID)
    {
        var q = objPurchasesMaster.Filter(p => p.PurchaseID == PurchaseID).ToList();

        //if (q.Count > 0)
        //{
        txtSupplierName.Text = q[0].Supplier.SupplierName;
        hfSID.Value = Convert.ToString(q[0].Supplier.SupplierID);

        int purchaseID = Convert.ToInt32(q[0].PurchaseID);
        hfPID.Value = purchaseID.ToString();

        lblTotalPurchaseDiscardsPrice.Text = "0.0";
        lblSupplierBalance.Text = lblNewSupplierBalance.Text = Convert.ToString(q[0].Supplier.Balance);
           
            FillgvProducts(PurchaseID);

            return true;
        //}
        //else
        //{
        //    txtSupplierName.Text = hfPID.Value = string.Empty;
        //    return false;
        //}

    }

    bool IsExist(int purchaseID)
    {
        var q = objPurchaseDiscardsMaster.Filter(p => p.PurchaseID == purchaseID).ToList();

        if (q.Count > 0)
        {
            //Dir(q[0]);

            FillgvProducts(purchaseID);


            return true;
        }
        else
        {
            return false;
        }

    }

      void FillgvProductsExist(int purchaseID)
    {
        var list = from p in objPurchaseDiscardsDetail.Filter(p=>p.PurchaseDiscardsMaster.PurchaseID==purchaseID).ToList()
                   select new
                   {
                      ProductName = p.Product.ProductName,
                      ProductID=p.ProductID,
                      PQTY = p.PDQTY, 
                      UnitName = p.Product.Unit.UnitName_En, 
                      UnitID=p.UnitID,
                      PUnitPrice = p.PUnitPrice,
                      TotalPrice = "0.0",
                      PDQTY="" 
                   };
        FillgvProducts(list);          
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetPurchaseCompletionList(string prefixText)
    {
        PurchasesMaster objPurchasesMaster = new PurchasesMaster();
        return objPurchasesMaster.Get(p => p.PurchaseID.ToString().ToLower().StartsWith(prefixText.ToLower()), p => p.PurchaseID.ToString()).ToArray();
    }

    // Search For Purchase Discards
    protected void imgbtnSearch_Click(object sender, ImageClickEventArgs e)
    {
        if (!string.IsNullOrEmpty(txtSearch.Text))
        {
            GetPurchaseDiscards(txtSearch.Text);
        }
    }

    void GetPurchaseDiscards(string key)
    {
        object [] parameter = null;
        
        if (index == 1)
        {
            var q = objspPurchaseDiscardsMasterResult.Filter(parameter, p => p.PurchaseDiscardsID == int.Parse(key)).ToList();
            if (q.Count > 0)
            {
                gvPurchaseDiscards.DataSource = q;
                gvPurchaseDiscards.DataBind();
            }
        }
        else if (index == 2)
        {
            var q = objspPurchaseDiscardsMasterResult.Filter(parameter, p => p.PurchaseID == int.Parse(key)).ToList();
            if (q.Count > 0)
            {
                gvPurchaseDiscards.DataSource = q;
                gvPurchaseDiscards.DataBind();
            }
        }
        else if (index == 3)
        {
            var q = objspPurchaseDiscardsMasterResult.Filter(parameter, p => p.SupplierName.ToLower() == key.ToLower()).ToList();
            if (q.Count > 0)
            {
                gvPurchaseDiscards.DataSource = q;
                gvPurchaseDiscards.DataBind();
            }
        }
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList(string prefixText)
    {
        string[] list = default(string[]);
        string[] noData = { "No Data!" };

        // stored procedures can't be used

        PurchaseDiscardsMaster objPurchaseDiscardsMaster = new PurchaseDiscardsMaster();

        if (index == 1)
        {
            list = objPurchaseDiscardsMaster.Get(p => p.PurchaseDiscardsID.ToString().ToLower().StartsWith(prefixText.ToLower()), p => p.PurchaseDiscardsID.ToString()).ToArray();
            return list;
        }
        else if (index == 2)
        {
            list = objPurchaseDiscardsMaster.Get(p => p.PurchaseID.ToString().ToLower().StartsWith(prefixText.ToLower()), p => p.PurchaseID.ToString()).ToArray();
            return list;
        }
        else if (index == 3)
        {
            list = objPurchaseDiscardsMaster.Get(p => p.PurchasesMaster.Supplier.SupplierName.ToLower().StartsWith(prefixText.ToLower()), p => p.PurchasesMaster.Supplier.SupplierName).ToArray();
            return list;
        }
        else
        {
            return noData;
        }
        
    }

    protected void ddlSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        index = ddlSearch.SelectedIndex;

        txtSearch.Text = string.Empty;
    }

    #endregion   


}