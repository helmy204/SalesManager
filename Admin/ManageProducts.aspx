<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ManageProducts.aspx.cs" Inherits="Admin_ManageProducts" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<%@ Register src="../ucBasicUnit.ascx" tagname="ucBasicUnit" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <script language="javascript" type="text/javascript" >
    //--- To Show Modal Popup of ValidationSummary
    function ShowModalDialog(group) {
    var g='';
    g=group ;
    var vs = document.getElementById ('<%=ValidationSummary3.ClientID %>');
         vs.validationGroup=g ;
        var x = $find('<%=ModalExtnd1.ClientID %>');
        
        Page_ClientValidate(g );
        if (!Page_IsValid)
            x.show();


    }

    function CallPrint(strid) {
        var dir = ('<%= Session["culture"] %>' == "en-US") ? 'ltr' : 'rtl';
        var prtContent = document.getElementById(strid);
        var WinPrint = window.open('', '', 'letf=0,top=0,width=400,height=400,toolbar=0,scrollbars=1,status=0');
        WinPrint.document.write("<html dir=" +dir+ "><head><link href='../CSS/TableStyle.css' rel='stylesheet' type='text/css' /></head><body><table dir="+dir +">" + prtContent.innerHTML + "</table></body></html>");
        WinPrint.dir = dir;
        WinPrint.document.close();
        WinPrint.focus();

        WinPrint.print();
        // WinPrint.close();
    }

    function btnprint_onclick() {
        CallPrint('<%= divprint.ClientID %>')
    }

    </script>

    <asp:Panel ID="pnlControl" runat="server" GroupingText="Products Control" 
        DefaultButton="imgbtnSave">
        <asp:UpdatePanel ID="upnlControl" runat="server">
        <ContentTemplate>
        <table width="100%">
        <tr>
        <td style="width: 147px" class="tableCell">
            <asp:Label ID="lblProductID" runat="server" Text="Product ID : "></asp:Label>
            </td>
        <td>
            <asp:TextBox ID="txtID" runat="server" Enabled="False" ReadOnly="True"></asp:TextBox>
            </td>
        </tr>
        <tr>
        <td style="width: 147px" class="tableCell">
            <asp:Label ID="lblProductName" runat="server" Text="Product Name : "></asp:Label>
            </td>
        <td>
            <asp:TextBox ID="txtProductName" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvProductName" runat="server" 
                ControlToValidate="txtProductName" Display="Dynamic" 
                ErrorMessage="Enter Product Name ." ValidationGroup="i">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
        <td style="width: 147px" class="tableCell">
            <asp:Label ID="lblCategory" runat="server" Text="Category : "></asp:Label>
            </td>
        <td>
            <asp:DropDownList ID="ddlCategory" runat="server" 
                AppendDataBoundItems="True">
                <asp:ListItem Value="0">Select</asp:ListItem>
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="rfvCategory" runat="server" 
                ControlToValidate="ddlCategory" Display="Dynamic" 
                ErrorMessage="Enter Category ." InitialValue="0" ValidationGroup="i">*</asp:RequiredFieldValidator>
            </td>
        </tr>
            <tr>
                <td class="tableCell" style="width: 147px">
                    <asp:Label ID="Label24" runat="server" Text="SQTY :"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtSQTY" runat="server" AutoPostBack="True" 
                        ontextchanged="txtSQTY_TextChanged"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvSQTY" runat="server" 
                        ControlToValidate="txtSQTY" Display="Dynamic" ErrorMessage="Enter SQTY." 
                        ValidationGroup="i">*</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" 
                        ControlToValidate="txtSQTY" Display="Dynamic" ErrorMessage="Enter Valid SQTY." 
                        ValidationExpression="[-+]?[0-9]*\.?[0-9]+" ValidationGroup="i">*</asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td class="tableCell" style="width: 147px">
                    <asp:Label ID="Label23" runat="server" Text="Unit :"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlUnites" runat="server" AutoPostBack="True" 
                        onselectedindexchanged="ddlUnites_SelectedIndexChanged" 
                        AppendDataBoundItems="True" >
                        <asp:ListItem Value="0">Select</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvUnites" runat="server" 
                        ControlToValidate="ddlUnites" Display="Dynamic" ErrorMessage="Enter Unite ." 
                        InitialValue="0" ValidationGroup="i">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="tableCell" style="width: 147px">
                    <asp:Label ID="Label21" runat="server" Text="Initial Quantity :"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtStartQuantity" runat="server" ReadOnly="True"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvPrice0" runat="server" 
                        ControlToValidate="txtStartQuantity" Display="Dynamic" 
                        ErrorMessage="Enter Initial Quantity." ValidationGroup="i">*</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                        ControlToValidate="txtStartQuantity" Display="Dynamic" 
                        ErrorMessage="Enter Valid Initial Quantity." 
                        ValidationExpression="[-+]?[0-9]*\.?[0-9]+" ValidationGroup="i">*</asp:RegularExpressionValidator>
                    
                    <uc1:ucBasicUnit ID="ucBasicUnit1" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="tableCell" style="width: 147px">
                    <asp:Label ID="Label22" runat="server" Text="Quantity :"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtQuantity" runat="server" ReadOnly="True"></asp:TextBox>
                    <asp:HiddenField ID="hfOldStartQuantity" runat="server" />
                  
                    <uc1:ucBasicUnit ID="ucBasicUnit2" runat="server" />
                  
                </td>
            </tr>
            <tr>
                <td class="tableCell" style="width: 147px">
                    <asp:Label ID="lblDescription" runat="server" Text="Description : "></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
        <tr>
        <td style="width: 147px"></td>
        <td>
       
    <asp:ImageButton ID="imgbtnSave" runat="server" Height="35px" 
        ImageAlign="Middle" ImageUrl="~/img/1270384072_filesave.png"   
                OnClientClick ="ShowModalDialog('i')" AlternateText="Save" ToolTip="Save" 
                onclick="imgbtnSave_Click"/>
  
    <asp:ImageButton ID="imgbtnClear" runat="server" Height="35px" 
        ImageAlign="Middle" ImageUrl="~/img/Reset.png" 
                CausesValidation="false" AlternateText="Clear" ToolTip="Clear" 
                onclick="imgbtnClear_Click"  />
        </td>
        </tr>
        </table>
        </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <asp:Panel ID="pnlShow" runat="server" GroupingText="Products Show">
    <asp:UpdatePanel ID="upnlShow" runat="server">
    
        <ContentTemplate>
      
            <asp:TextBox ID="txtSearchValue" runat="server"></asp:TextBox>
            <cc1:AutoCompleteExtender ID="txtSearchValue_AutoCompleteExtender" 
                runat="server" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" 
                ServiceMethod="GetCompletionList" ServicePath="" 
                TargetControlID="txtSearchValue" UseContextKey="True">
            </cc1:AutoCompleteExtender>
            <cc1:TextBoxWatermarkExtender ID="txtSearchProduct_TextBoxWatermarkExtender" 
                runat="server" Enabled="True" TargetControlID="txtSearchValue" 
                WatermarkCssClass="WaterMark" WatermarkText="Find Product ">
            </cc1:TextBoxWatermarkExtender>
            <asp:ImageButton ID="imgbtnSearch" runat="server" Height="27px" 
                ImageAlign="Middle" ImageUrl="~/img/Search.png" 
                AlternateText="Search" CausesValidation="False" ToolTip="Search" 
                onclick="imgbtnSearch_Click" />
              
            <input id="btnprint" type="button" 
    value="Print" onclick="return btnprint_onclick()" />
              
            <asp:GridView ID="gvProducts" runat="server" AllowPaging="True"  
                AutoGenerateColumns="False"  DataKeyNames="ProductID" Width="100%" 
                onpageindexchanging="gvProducts_PageIndexChanging" 
                onrowcommand="gvProducts_RowCommand">
                
                <Columns>
                    <asp:BoundField DataField="ProductID" HeaderText="No" ReadOnly="True" 
                        SortExpression="ProductID" />
                    <asp:BoundField DataField="ProductName" HeaderText="Product" 
                        ReadOnly="True" SortExpression="ProductName" />
                    <asp:BoundField DataField="CategoryName" HeaderText="Category" 
                        ReadOnly="True" SortExpression="CategoryName" />
                       
                        <asp:BoundField DataField="Description" HeaderText="Description" 
                        ReadOnly="True" SortExpression="Description" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="imgbtnEdit" runat="server" Height="35px" ImageAlign="Middle" ImageUrl="~/img/1270383845_edit.png" CommandName="select" CausesValidation="false" CommandArgument='<%#Eval("ProductID") %>' AlternateText="Edit" ToolTip="Edit"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField>
                        <ItemTemplate>
                           <asp:ImageButton ID="imgbtnDelete" runat="server" Height="35px" ImageAlign="Middle" ImageUrl="~/img/delete.png" CommandArgument='<%#Eval("ProductID") %>' CommandName="del" CausesValidation="false" AlternateText="Delete" ToolTip="Delete" OnClientClick="return confirm('Are you sure ?');"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
               
            </asp:GridView>
         
        </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <div id="divprint" runat="server" style="display:none;">
    
    </div>
<%--End of Sucess ModalPopup --%><%--End of Sucess ModalPopup --%>
<cc1:ModalPopupExtender ID="ModalExtnd1" runat="server" 
    TargetControlID="lblHidden"
    PopupControlID="SummaryDiv" 
    BackgroundCssClass="modalBackground" DynamicServicePath="" Enabled="True" CancelControlID="btnclose" >
</cc1:ModalPopupExtender>
   
<div id="SummaryDiv" runat="server" align="center" class="confirm" style="display:none" >
    <table align="center" class="confirmtable" >
        <tr>
        <td valign="top">
            <img align="middle" src="../img/warning.gif" />
            <asp:Label ID="LabelMsg" runat="server" Text="PLZ,Enter The following :-" 
                ></asp:Label>
            &nbsp;&nbsp;</td>
        </tr>
        <tr>
        <td colspan="2" align="left" >
             <asp:ValidationSummary ID="ValidationSummary3" runat="server" 
                   />
            </td>
        </tr>
        <tr>
        <td colspan="2">
        <asp:Button ID="btnclose" runat="server" Text="Close" 
                 />
        
            <%--End of Sucess ModalPopup --%>
         </td>
        </tr>
    </table>      
</div>
<asp:Label ID="lblHidden" runat="server" Text="hidden" CssClass="hidelbl"      ></asp:Label>
        
    <%--End of Sucess ModalPopup --%><%--End of Sucess ModalPopup --%><%--End of Sucess ModalPopup --%>
    <cc1:ModalPopupExtender ID="mdl_Fail" runat="server" 
        TargetControlID="lbl_Fail"
        PopupControlID="FailDiv" 
        BackgroundCssClass="modalBackground" DynamicServicePath="" Enabled="True"  CancelControlID ="btnclose_f">
    </cc1:ModalPopupExtender>
    <div id="FailDiv" runat="server" align="center" class="confirm" style="display:none">
        <table align="center" class="confirmtable" id="faliur" runat="server" >
            <tr>
            <td valign="top">
                <img align="middle" src="../img/achtung.gif" />
                <asp:Label ID="Label20" runat="server" Font-Bold="True" ForeColor="Red" 
                    Text="Error,plz try again. "   ></asp:Label>
                &nbsp;&nbsp;</td>
            </tr>
            
            <tr>
            <td colspan="2">
            <asp:Button ID="btnclose_f" runat="server" Text="Close" 
                     />
            </td> 
            </tr>
        </table>      
    </div>
    <asp:Label ID="lbl_Fail" runat="server" Text="hidden" CssClass="hidelbl"         ></asp:Label>
    
    <%--End of Sucess ModalPopup --%>

<%--End of Sucess ModalPopup --%>
    <cc1:ModalPopupExtender ID="mdl_Sucess" runat="server" 
        TargetControlID="lbl_Sucess"
        PopupControlID="SucessDiv" 
        BackgroundCssClass="modalBackground" DynamicServicePath="" Enabled="True" CancelControlID ="btnclose_s" >
    </cc1:ModalPopupExtender>
    <div id="SucessDiv" runat="server" align="center" class="confirm" style="display:none">
        <table align="center" class="confirmtable" id="Sucess" runat="server" >
            <tr>
            <td valign="top">
                <img align="middle" src="../img/accept.png" />
                <asp:Label ID="Label5" runat="server" Font-Bold="True" ForeColor="Green" 
                    Text="Process Complete Successfully. "            ></asp:Label>
                &nbsp;&nbsp;</td>
            </tr>
            
            <tr>
            <td colspan="2">
            <asp:Button ID="btnclose_s" Text="Close" runat="server" 
                     />
                
                    
                    </td>
            </tr>
        </table>      
    </div>
    <asp:Label ID="lbl_Sucess" runat="server" Text="hidden" CssClass="hidelbl"  ></asp:Label>




</asp:Content>


