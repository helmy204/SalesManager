using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.ElkhateebDynamicLinq;
public partial class Admin_ManageCategories : System.Web.UI.Page
{
    #region Objects
    Category objCategory = new Category();
    spCategoriesResult objspCategoriesResult = new spCategoriesResult();
    #endregion

    #region Methods
    bool CategoryNameExist(string CategoryName)
    {
        var q = objCategory.Filter(p => p.CategoryName.ToLower() == CategoryName.ToLower()).ToList();
        if (q.Count > 0)//exist
            return true;
        else
            return false;//not exist
    }
    bool CategoryNameExist(int CategoryID, string CategoryName)
    {
        var q = objCategory.Filter(p => p.CategoryID != CategoryID && p.CategoryName == CategoryName).ToList();
        if (q.Count > 0)//exist
            return true;
        else
            return false;//not exist
    }
    void Clear()
    {
        txtID.Text = txtCategoryName.Text = string.Empty;
        ddlParentCategory.SelectedIndex = 0;
        gvCategories.SelectedIndex = -1;
    }
    void FillddlParentCategory()
    {
        var q = objCategory.Get().ToList();
        ddlParentCategory.DataSource = q;
        ddlParentCategory.DataTextField = "CategoryName";
        ddlParentCategory.DataValueField = "categoryID";
        ddlParentCategory.DataBind();
    }
    void Fillgvcategories()
    {
        object[] parameter = null;
        gvCategories.DataSource = objspCategoriesResult.Get(parameter).ToList();
        gvCategories.DataBind();
    }
    bool Insert()
    {
        objCategory = new Category();
        objCategory.CategoryName = txtCategoryName.Text.Trim();
        objCategory.ParentCategoryID = (ddlParentCategory.SelectedIndex > 0) ? (Nullable<int>)int.Parse(ddlParentCategory.SelectedValue) : null;
        return (Convert.ToInt32(objCategory.Insert()) > 0);
    }
    bool Update()
    {
        objCategory = new Category();
        objCategory.CategoryID = Convert.ToInt32(txtID.Text.Trim());
        objCategory.CategoryName = txtCategoryName.Text.Trim();
        objCategory.ParentCategoryID = (ddlParentCategory.SelectedIndex > 0) ? (Nullable<int>)int.Parse(ddlParentCategory.SelectedValue) : null;
        return objCategory.Update(p => p.CategoryID == objCategory.CategoryID);
    }
    void Dir(Category entity)
    {
        txtID.Text = entity.CategoryID.ToString();
        txtCategoryName.Text = entity.CategoryName;
        ddlParentCategory.SelectedValue = (entity.ParentCategoryID != null) ? entity.ParentCategoryID.ToString() : "0";
    }
    #endregion

    #region Events
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.Title = "Manage Categories";
            FillddlParentCategory();
            Fillgvcategories();
        }

    }
    protected void imgbtnSave_Click(object sender, ImageClickEventArgs e)
    {
        System.Threading.Thread.Sleep(2000);
        if (gvCategories.SelectedIndex == -1)
        {
            if (CategoryNameExist(txtCategoryName.Text.Trim()))
            {
                MessageBox.Show(this.Page, "Category Name exist before");
                return;
            }
            if (Insert())
            {
                mdl_Sucess.Show();
            }
            else
            {
                mdl_Fail.Show();
            }
        }
        else if (gvCategories.SelectedIndex > -1)
        {
            if (CategoryNameExist(Convert.ToInt32(txtID.Text.Trim()), txtCategoryName.Text.Trim()))
            {
                MessageBox.Show(this.Page, "Category Name exist before");
                return;
            }
            if (Update())
            {
                mdl_Sucess.Show();
            }
            else
            {
                mdl_Fail.Show();
            }
        }
        Fillgvcategories();
        FillddlParentCategory();
        Clear();
    }
    protected void imgbtnClear_Click(object sender, ImageClickEventArgs e)
    {
        Fillgvcategories();
        Clear();

    }
    protected void gvCategories_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int CategoryID = Convert.ToInt32(e.CommandArgument);
        if (e.CommandName == "select")
        {
            Dir(objCategory.Single(p => p.CategoryID == CategoryID));
        }
        else if (e.CommandName == "del")
        {
            try
            {
                System.Threading.Thread.Sleep(2000);
                if (objCategory.Delete(p => p.CategoryID == CategoryID))
                {
                    mdl_Sucess.Show();
                    Fillgvcategories();
                    FillddlParentCategory();
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
    protected void gvCategories_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvCategories.PageIndex = e.NewPageIndex;
        Fillgvcategories();
    }
    #endregion


    protected void imgbtnSearch_Click(object sender, ImageClickEventArgs e)
    {
        if (!string.IsNullOrEmpty(txtSearchValue.Text))
        {
            GetCategoryByName(txtSearchValue.Text.Trim());
        }
    }

    private void GetCategoryByName(string CategoryName)
    {
        object[] parameter = null;
        gvCategories.DataSource = objspCategoriesResult.Filter(parameter, p => p.CategoryName.ToLower() == CategoryName.ToLower()).ToList();
        gvCategories.DataBind();
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList(string prefixText, int count, string contextKey)
    {
        Category objCategory = new Category();
        return objCategory.Get(p => p.CategoryName.ToLower().StartsWith(prefixText.ToLower()), p => p.CategoryName).ToArray();
    }


    public static void SetDefaultButton(TextBox control, Button btButton)
    {
        control.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btButton.UniqueID + "').click();return false;}} else {return true}; ");
    }
    protected void ddlParentCategory_DataBound(object sender, EventArgs e)
    {
        ddlParentCategory.Items.Insert(0, new ListItem("Select", "0"));
    }
}
