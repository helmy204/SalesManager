using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.ElkhateebDynamicLinq;
using System.Globalization;
public partial class Admin_ManagePaymentsForSuppliers : System.Web.UI.Page
{
    #region Objects
    PaymentsForSupplier objPaymentsForSupplier = new PaymentsForSupplier();
    spPaymentsForSuppliersResult objspPaymentsForSuppliersResult = new spPaymentsForSuppliersResult();
    Supplier objSupplier = new Supplier();
    CultureInfo culture = new CultureInfo("en-US");
    static double oldPayment;
    #endregion

    #region Methods
 
    void Clear()
    {
        txtID.Text =txtPayment.Text=txtPaymentDateM.Text=txtPaymentDateH.Text= string.Empty;
        ddlSupplier.SelectedIndex = 0;
        gvPaymentsForSuppliers.SelectedIndex = -1;
        oldPayment=0;
    }
    void FillddlSupplier()
    {
        ddlSupplier.DataSource = objSupplier.Get();
        ddlSupplier.DataTextField = "SupplierName";
        ddlSupplier.DataValueField = "SupplierID";
        ddlSupplier.DataBind();
    }
    void FillgvPaymentsForSuppliers()
    {
        object[] parameter = null;
        gvPaymentsForSuppliers.DataSource = objspPaymentsForSuppliersResult.Get(parameter).ToList();
        gvPaymentsForSuppliers.DataBind();
    }
    bool Insert()
    {
        objPaymentsForSupplier = new PaymentsForSupplier();
        objPaymentsForSupplier.BeginTransaction();
        objPaymentsForSupplier.SupplierID = int.Parse(ddlSupplier.SelectedValue);
        objPaymentsForSupplier.Payment  = Convert.ToDouble(txtPayment.Text.Trim());
        objPaymentsForSupplier.PaymentDateM = Convert.ToDateTime(txtPaymentDateM.Text.Trim(),culture );
        objPaymentsForSupplier.PaymentDateH = txtPaymentDateH.Text.Trim();
        objPaymentsForSupplier.Insert();

        objSupplier = objSupplier.Single(p => p.SupplierID == int.Parse(ddlSupplier.SelectedValue));
        objSupplier.Balance -= Convert.ToDouble(txtPayment.Text.Trim());
        objSupplier.Update(p => p.SupplierID == objSupplier.SupplierID);
        objSupplier.EndTransaction();
        return true;
    }
    bool Update()
    {
        objPaymentsForSupplier = new PaymentsForSupplier();
        objPaymentsForSupplier.BeginTransaction();
        objPaymentsForSupplier.SupplierPaymentID = Convert.ToInt32(txtID.Text.Trim());
        objPaymentsForSupplier.SupplierID = int.Parse(ddlSupplier.SelectedValue);
        objPaymentsForSupplier.Payment = Convert.ToDouble(txtPayment.Text.Trim());
        objPaymentsForSupplier.PaymentDateM = Convert.ToDateTime(txtPaymentDateM.Text.Trim(),culture );
        objPaymentsForSupplier.PaymentDateH = txtPaymentDateH.Text.Trim();
        objPaymentsForSupplier.Update(p => p.SupplierPaymentID == objPaymentsForSupplier.SupplierPaymentID);

        objSupplier = objSupplier.Single(p => p.SupplierID == int.Parse(ddlSupplier.SelectedValue));
        objSupplier.Balance -= (oldPayment - Convert.ToDouble(txtPayment.Text.Trim()));
        objSupplier.Update(p => p.SupplierID == objSupplier.SupplierID);
        objSupplier.EndTransaction();
        return true;
    }
    void Dir(PaymentsForSupplier entity)
    {
        txtID.Text = entity.SupplierPaymentID.ToString();
        
        ddlSupplier.SelectedValue = entity.SupplierID.ToString();
        txtPayment.Text = entity.Payment.ToString();
        txtPaymentDateM.Text = entity.PaymentDateM.ToString("MM/dd/yyyy", culture);
        txtPaymentDateH.Text = entity.PaymentDateH;
        oldPayment = entity.Payment;
    }

    bool Delete(int SupplierPaymentID)
    {
        try
        {
            objPaymentsForSupplier.BeginTransaction();
            objPaymentsForSupplier = objPaymentsForSupplier.Single(z => z.SupplierPaymentID == SupplierPaymentID);


            objSupplier = objSupplier.Single(p => p.SupplierID == objPaymentsForSupplier.SupplierID);
            objSupplier.Balance += objPaymentsForSupplier.Payment;
            objSupplier.Update(p => p.SupplierID == objSupplier.SupplierID);

            objPaymentsForSupplier.Delete(p => p.SupplierPaymentID == objPaymentsForSupplier.SupplierPaymentID);
            objPaymentsForSupplier.EndTransaction();

        }
        catch (Exception ex)
        {
            return false;
        }
        return true;

    }



    void GetSupplierByName(string SupplierName)
    {
        object[] parameter = null;
        gvPaymentsForSuppliers.DataSource = objspPaymentsForSuppliersResult.Filter(parameter, p => p.SupplierName.ToLower() == SupplierName.ToLower()).ToList();
      gvPaymentsForSuppliers.DataBind();
    }
    #endregion

    #region Events
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.Title = "Manage Payments For Suppliers";
            FillddlSupplier();
            FillgvPaymentsForSuppliers();
        }

    }
    protected void imgbtnSave_Click(object sender, ImageClickEventArgs e)
    {
        System.Threading.Thread.Sleep(2000);
        if (gvPaymentsForSuppliers.SelectedIndex == -1)
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
        else if (gvPaymentsForSuppliers.SelectedIndex > -1)
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
        FillgvPaymentsForSuppliers();
        //FillddlSupplier();
        Clear();
    }
    protected void imgbtnClear_Click(object sender, ImageClickEventArgs e)
    {
        FillgvPaymentsForSuppliers();
        Clear();

    }
    protected void gvPaymentsForSuppliers_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int SupplierPaymentID = Convert.ToInt32(e.CommandArgument);
        if (e.CommandName == "select")
        {
            Dir(objPaymentsForSupplier.Single(p => p.SupplierPaymentID == SupplierPaymentID));
        }
        else if (e.CommandName == "del")
        {
            try
            {
                System.Threading.Thread.Sleep(2000);
                if (Delete(SupplierPaymentID))
                {
                    mdl_Sucess.Show();
                    FillgvPaymentsForSuppliers();
                    //FillddlSupplier();
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
    protected void gvPaymentsForSuppliers_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvPaymentsForSuppliers.PageIndex = e.NewPageIndex;
        FillgvPaymentsForSuppliers();
    }
    #endregion


    protected void imgbtnSearch_Click(object sender, ImageClickEventArgs e)
    {
        if(!string.IsNullOrEmpty(txtSearchValue.Text))
        {
            GetSupplierByName(txtSearchValue.Text.Trim());
        }
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList(string prefixText, int count, string contextKey)
    {
        spPaymentsForSuppliersResult objspPaymentsForSuppliersResult = new spPaymentsForSuppliersResult();
        return objspPaymentsForSuppliersResult.Get(null,p => p.SupplierName.ToLower().StartsWith(prefixText.ToLower()), p => p.SupplierName).Distinct().ToArray();
    }
    protected void txtPaymentDateM_TextChanged(object sender, EventArgs e)
    {
        if (txtPaymentDateM.Text != string.Empty)
            txtPaymentDateH.Text = CustomsConverter.GetHijriDate(txtPaymentDateM.Text);
    }
}
