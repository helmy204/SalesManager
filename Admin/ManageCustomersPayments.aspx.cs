using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.ElkhateebDynamicLinq;
using System.Globalization;
public partial class Admin_ManageCustomersPayments : System.Web.UI.Page
{
    #region Objects
    CustomersPayment objCustomersPayment = new CustomersPayment();
    spCustomersPaymentsResult objspCustomersPaymentsResult = new spCustomersPaymentsResult();
    Customer objCustomer = new Customer();
    CultureInfo culture = new CultureInfo("en-US");
    static double oldPayment;
    #endregion

    #region Methods
 
    void Clear()
    {
        txtID.Text =txtPayment.Text=txtPaymentDateM.Text=txtPaymentDateH.Text= string.Empty;
        ddlCustomer.SelectedIndex = 0;
        gvCustomersPayments.SelectedIndex = -1;
        oldPayment = 0;
    }
    void FillddlCustomer()
    {
        ddlCustomer.DataSource = objCustomer.Get();
        ddlCustomer.DataTextField = "CustomerName";
        ddlCustomer.DataValueField = "CustomerID";
        ddlCustomer.DataBind();
    }
    void FillgvCustomersPayments()
    {
        object[] parameter = null;
        gvCustomersPayments.DataSource = objspCustomersPaymentsResult.Get(parameter).ToList();
        gvCustomersPayments.DataBind();
    }
    bool Insert()
    {
        objCustomersPayment = new CustomersPayment();
        objCustomersPayment.BeginTransaction();
        objCustomersPayment.CustomerID = int.Parse(ddlCustomer.SelectedValue);
        objCustomersPayment.Payment  = Convert.ToDouble(txtPayment.Text.Trim());
        objCustomersPayment.PaymentDateM = Convert.ToDateTime(txtPaymentDateM.Text.Trim(),culture );
        objCustomersPayment.PaymentDateH = txtPaymentDateH.Text.Trim();
       objCustomersPayment.Insert();
       objCustomer = objCustomer.Single(p => p.CustomerID == int.Parse(ddlCustomer.SelectedValue));
       objCustomer.Balance -= Convert.ToDouble(txtPayment.Text.Trim());
       objCustomer.Update(p => p.CustomerID == objCustomer.CustomerID);
       objCustomer.EndTransaction();
       return true;
    }
    bool Update()
    {
        objCustomersPayment = new CustomersPayment();
        objCustomersPayment.BeginTransaction();
        objCustomersPayment.CustomerPaymentID = Convert.ToInt32(txtID.Text.Trim());
        objCustomersPayment.CustomerID = int.Parse(ddlCustomer.SelectedValue);
        objCustomersPayment.Payment = Convert.ToDouble(txtPayment.Text.Trim());
        objCustomersPayment.PaymentDateM = Convert.ToDateTime(txtPaymentDateM.Text.Trim(),culture );
        objCustomersPayment.PaymentDateH = txtPaymentDateH.Text.Trim();
        objCustomersPayment.Update(p => p.CustomerPaymentID == objCustomersPayment.CustomerPaymentID);

        objCustomer = objCustomer.Single(p => p.CustomerID == int.Parse(ddlCustomer.SelectedValue));
        objCustomer.Balance -=(oldPayment - Convert.ToDouble(txtPayment.Text.Trim()));
        objCustomer.Update(p => p.CustomerID == objCustomer.CustomerID);
        objCustomer.EndTransaction();
        return true;
    }
    bool Delete(int CustomerPaymentID)
    {
        try
        {
            objCustomersPayment.BeginTransaction();
            objCustomersPayment = objCustomersPayment.Single(z => z.CustomerPaymentID == CustomerPaymentID);


            objCustomer = objCustomer.Single(p => p.CustomerID == objCustomersPayment.CustomerID);
            objCustomer.Balance += objCustomersPayment.Payment;
            objCustomer.Update(p => p.CustomerID == objCustomer.CustomerID);

            objCustomersPayment.Delete(p => p.CustomerPaymentID == objCustomersPayment.CustomerPaymentID);
            objCustomersPayment.EndTransaction();
          
        }
        catch (Exception ex)
        {
            return false;
        }
        return true;

    }
    void Dir(CustomersPayment entity)
    {
        txtID.Text = entity.CustomerPaymentID.ToString();
        
        ddlCustomer.SelectedValue = entity.CustomerID.ToString();
        txtPayment.Text = entity.Payment.ToString();
        txtPaymentDateM.Text = entity.PaymentDateM.ToString("MM/dd/yyyy", culture);
        txtPaymentDateH.Text = entity.PaymentDateH;
        oldPayment = entity.Payment;
    }
    void GetCustomerByName(string CustomerName)
    {
        object[] parameter = null;
        gvCustomersPayments.DataSource = objspCustomersPaymentsResult.Filter(parameter, p => p.CustomerName.ToLower() == CustomerName.ToLower()).ToList();
      gvCustomersPayments.DataBind();
    }
    #endregion

    #region Events
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.Title = "Manage Customers Payments";
            FillddlCustomer();
            FillgvCustomersPayments();
        }

    }
    protected void imgbtnSave_Click(object sender, ImageClickEventArgs e)
    {
        System.Threading.Thread.Sleep(2000);
        if (gvCustomersPayments.SelectedIndex == -1)
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
        else if (gvCustomersPayments.SelectedIndex > -1)
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
        FillgvCustomersPayments();
        //FillddlCustomer();
        Clear();
    }
    protected void imgbtnClear_Click(object sender, ImageClickEventArgs e)
    {
        FillgvCustomersPayments();
        Clear();

    }
    protected void gvCustomersPayments_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int CustomerPaymentID = Convert.ToInt32(e.CommandArgument);
        if (e.CommandName == "select")
        {
            Dir(objCustomersPayment.Single(p => p.CustomerPaymentID == CustomerPaymentID));
        }
        else if (e.CommandName == "del")
        {
            try
            {
                System.Threading.Thread.Sleep(2000);
               

           
                if (Delete(CustomerPaymentID))
                {


                    mdl_Sucess.Show();
                    FillgvCustomersPayments();
                    //FillddlCustomer();
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
    protected void gvCustomersPayments_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvCustomersPayments.PageIndex = e.NewPageIndex;
        FillgvCustomersPayments();
    }
    #endregion


    protected void imgbtnSearch_Click(object sender, ImageClickEventArgs e)
    {
        if(!string.IsNullOrEmpty(txtSearchValue.Text))
        {
            GetCustomerByName(txtSearchValue.Text.Trim());
        }
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList(string prefixText, int count, string contextKey)
    {
        spCustomersPaymentsResult objspCustomersPaymentsResult = new spCustomersPaymentsResult();
        return objspCustomersPaymentsResult.Get(null,p => p.CustomerName.ToLower().StartsWith(prefixText.ToLower()), p => p.CustomerName).Distinct().ToArray();
    }
    protected void txtPaymentDateM_TextChanged(object sender, EventArgs e)
    {
        if (txtPaymentDateM.Text != string.Empty)
            txtPaymentDateH.Text = CustomsConverter.GetHijriDate(txtPaymentDateM.Text);
    }
}
