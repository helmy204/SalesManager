<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ManagePurchaseDiscards.aspx.cs" Inherits="Admin_ManagePurchaseDiscards" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<%-- <script language="javascript" type="text/javascript">
     //--- To Show Modal Popup of ValidationSummary
     function ShowModalDialog(group) {
         var g = '';
         g = group;
         var vs = document.getElementById('<%=ValidationSummary3.ClientID %>');
         vs.validationGroup = g;
         var x = $find('<%=ModalExtnd1.ClientID %>');

         Page_ClientValidate(g);
         if (!Page_IsValid) {
             x.show();
             return false;
         }
         return true;

     }
    </script>--%>

    <asp:Panel ID="pnlControl" runat="server" 
        GroupingText="PurchaseDiscards Control" DefaultButton="imgbtnSave">
        <asp:UpdatePanel ID="upnlControl" runat="server">
            <ContentTemplate>
                <table width="100%">
                    <tr>
                        <td class="tableCell" colspan="2" style="height: 26px">
                            <asp:Label ID="lblPurchaseDiscardesID" runat="server" 
                                Text="Purchase Discards Number : "></asp:Label>
                        </td>
                        <td style="width: 213px; height: 26px;" class="style3" colspan="2">
                            <asp:TextBox ID="txtID" runat="server" Enabled="False" ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tableCell" colspan="2" style="height: 35px">
                            <asp:Label ID="lblPurchaseID" runat="server" Text="Purchase Number : "></asp:Label>
                        </td>
                        <td class="style3" colspan="2" style="width: 213px; height: 35px;">
                            <asp:TextBox ID="txtPurchaseNo" runat="server"></asp:TextBox>
                            <cc1:AutoCompleteExtender ID="txtPurchaseNo_AutoCompleteExtender" 
                                runat="server" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" 
                                ServiceMethod="GetPurchaseCompletionList" ServicePath="" 
                                TargetControlID="txtPurchaseNo">
                            </cc1:AutoCompleteExtender>
                            <cc1:TextBoxWatermarkExtender ID="txtPurchaseNo_TextBoxWatermarkExtender" 
                                runat="server" Enabled="True" TargetControlID="txtPurchaseNo" 
                                WatermarkCssClass="WaterMark" WatermarkText="Find Purchase">
                            </cc1:TextBoxWatermarkExtender>
                            <asp:ImageButton ID="imgbtnSearchPurchase" runat="server" 
                                ImageUrl="~/img/1270384342_search_button_green.png" 
                                onclick="imgbtnSearchPurchase_Click" ToolTip="Search Purchase" Width="25px" />
                            <asp:HiddenField ID="hfPID" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tableCell" colspan="2">
                            <asp:Label ID="lblSupplierName" runat="server" Text="Supplier Name : "></asp:Label>
                        </td>
                        <td class="style3" colspan="2" style="width: 213px">
                            <asp:TextBox ID="txtSupplierName" runat="server" 
                                ReadOnly="True"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator46" runat="server" 
                                ControlToValidate="txtSupplierName" Display="Dynamic" 
                                ErrorMessage="Enter PurchaseDate ." ValidationGroup="i">*</asp:RequiredFieldValidator>
                            <asp:HiddenField ID="hfSID" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tableCell" colspan="2">
                            <asp:Label ID="lblPurchaseDiscardDateM" runat="server" 
                                Text="PurchaseDiscardDate M : "></asp:Label>
                        </td>
                        <td class="style3" colspan="2" style="width: 213px">
                            <asp:TextBox ID="txtPurchaseDiscardDateM" runat="server" AutoPostBack="True" ontextchanged="txtPurchaseDiscardDateM_TextChanged" 
                                ></asp:TextBox>
                            <cc1:CalendarExtender ID="txtPurchaseDiscardDateM_CalendarExtender" 
                                runat="server" Enabled="True" TargetControlID="txtPurchaseDiscardDateM">
                            </cc1:CalendarExtender>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                ControlToValidate="txtPurchaseDiscardDateM" Display="Dynamic" 
                                ErrorMessage="Enter PurchaseDate ." ValidationGroup="i">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="tableCell" colspan="2">
                            <asp:Label ID="lblPurchaseDiscardDateH" runat="server" 
                                Text="PurchaseDiscardDate H : "></asp:Label>
                        </td>
                        <td class="style3" colspan="2" style="width: 213px">
                            <asp:TextBox ID="txtPurchaseDiscardDateH" runat="server" Enabled="False"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tableCell" colspan="2">
                            <asp:Label ID="lblNotes" runat="server" Text="Notes : "></asp:Label>
                        </td>
                        <td class="style3" colspan="2" style="width: 213px">
                            <asp:TextBox ID="txtNotes" runat="server" Height="41px" 
                                TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Panel ID="pnlProduct" runat="server" GroupingText="Products">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:GridView ID="gvProducts" runat="server"
                                                AutoGenerateColumns="false"
                                                Width="100%" onrowcommand="gvProducts_RowCommand" EnableModelValidation="true">
                                                <Columns>
                                                <asp:TemplateField HeaderText="Select">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkDiscard" runat="server"  CommandName="select" 
                                                        CausesValidation="false" AutoPostBack="True" 
                                                        oncheckedchanged="chkDiscard_CheckedChanged" />
                                                </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="S.N.">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblS" Text='<%# Container.DataItemIndex + 1 %>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Product">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblProduct" Text='<%# Eval("ProductName") %>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField  Visible="false">
                                                    <ItemTemplate>
                                                    <asp:Label ID="lblProductID" Text='<%# Eval("ProductID") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="PD.Qty">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtPQty" Text='<%# Eval("PQTY") %>' runat="server" 
                                                                ReadOnly="True" Width="70px" ValidationGroup="i"></asp:TextBox>
                                                            &nbsp;&nbsp;&nbsp;&nbsp;
                                                            <asp:TextBox ID="txtPDQTY" runat="server" AutoPostBack="True" 
                                                                Text='<%# Eval("PDQTY") %>' Width="70px" 
                                                                ontextchanged="txtPDQTY_TextChanged" ReadOnly="True" ValidationGroup="i"></asp:TextBox>


                                                            <asp:CompareValidator ID="CompareValidator1" runat="server" 
                                                                ErrorMessage="Value of PDQTY must be less than PQTY" 
                                                                ControlToCompare="txtPQty" ControlToValidate="txtPDQTY" Type="Double" 
                                                                Operator="LessThanEqual" ValidationGroup="i">*</asp:CompareValidator>


                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Unit">
                                                        <ItemTemplate>
                                                        <asp:Label ID="lblUnit" Text='<%# Eval("UnitName") %>' runat="server"></asp:Label>
                                                       
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                    <asp:Label ID="lblUnitID" Text='<%# Eval("UnitID") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Unit Price">
                                                        <ItemTemplate>
                                                          <asp:Label ID="lblPUnitPrice" Text='<%# Eval("PUnitPrice") %>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total Price">
                                                        <ItemTemplate>
                                                        <asp:Label ID="lblTotalPrice" Text='<%# Eval("TotalPrice") %>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <EmptyDataTemplate>
                                                    <asp:Label ID="lblNoData" runat="server" Text="No Data"></asp:Label>
                                                </EmptyDataTemplate>
                                            </asp:GridView>
                                        </td>
                                    </tr>
          
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 170px"  >
                            <asp:Label ID="lblPurchaseDiscards" runat="server" 
                                Text="Total Purchase Discards : "></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:Label ID="lblTotalPurchaseDiscardsPrice" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 130px">
                            <asp:Label ID="lblSuppBalance" runat="server" Text="Supplier Balance:"></asp:Label>
                        </td>
                        <td style="width: 97px">
                            <asp:Label ID="lblSupplierBalance" runat="server">0</asp:Label>
                        </td>
                        <td class="style3" style="width: 166px">
                            &nbsp;</td>
                        <td class="style3" style="width: 213px">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 130px">
                            <asp:Label ID="lblnewSupplBalance" runat="server" Text="New Supplier Balance:"></asp:Label>
                        </td>
                        <td style="width: 97px">
                            <asp:Label ID="lblNewSupplierBalance" runat="server" Text="0"></asp:Label>
                        </td>
                        <td class="style3" style="width: 166px">
                            &nbsp;</td>
                        <td class="style3" style="width: 213px">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="2">
                        </td>
                        <td class="style3" style="width: 213px" colspan="2">
                            <asp:ImageButton ID="imgbtnSave" runat="server" AlternateText="Save" 
                                Height="35px" ImageAlign="Middle" ImageUrl="~/img/1270384072_filesave.png" 
                                
                                ToolTip="Save" ValidationGroup="i" onclick="imgbtnSave_Click" />
                            <asp:ImageButton ID="imgbtnClear" runat="server" AlternateText="Clear" 
                                CausesValidation="false" Height="35px" ImageAlign="Middle" 
                                ImageUrl="~/img/Reset.png" ToolTip="Clear" onclick="imgbtnClear_Click" />
                            </td>
                    </tr>
                </table>
                
                </ContentTemplate>
                </asp:UpdatePanel>

                </asp:Panel>
                <asp:UpdatePanel ID="upnlShow" runat="server">
                <ContentTemplate>
                
                
                

                <asp:Panel ID="pblPurchaseDiscards" runat="server" GroupingText="Purchase Discards" DefaultButton="imgbtnSearch">


                <table width="100%">
    <tr>
        <td style="width: 77px; height: 33px;">
            <asp:DropDownList ID="ddlSearch" runat="server" AutoPostBack="True" 
                onselectedindexchanged="ddlSearch_SelectedIndexChanged">
                <asp:ListItem Selected="True" Value="0">-- Search by --</asp:ListItem>
                <asp:ListItem Value="1">Discard No.</asp:ListItem>
                <asp:ListItem Value="2">Purchase No.</asp:ListItem>
                <asp:ListItem Value="3">Supplier Name</asp:ListItem>
            </asp:DropDownList>
        </td>
        <td style="height: 33px">
            <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox> 
            <cc1:TextBoxWatermarkExtender ID="txtSearch_TextBoxWatermarkExtender" 
                runat="server" Enabled="True" TargetControlID="txtSearch" 
                WatermarkCssClass="WaterMark" WatermarkText="Find Discards">
            </cc1:TextBoxWatermarkExtender>
            <cc1:AutoCompleteExtender ID="txtSearch_AutoCompleteExtender" runat="server" 
                DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" 
                ServiceMethod="GetCompletionList" ServicePath="" TargetControlID="txtSearch">
            </cc1:AutoCompleteExtender>
            <asp:ImageButton ID="imgbtnSearch" runat="server" 
                ImageUrl="~/img/1270384342_search_button_green.png" Width="25px" 
                onclick="imgbtnSearch_Click" />
        </td>
    </tr>
    <tr>
        <td colspan="2">
           
            <asp:GridView ID="gvPurchaseDiscards" runat="server" 
                AutoGenerateColumns="false" 
                             
                 Width="100%" onpageindexchanging="gvPurchaseDiscards_PageIndexChanging" 
                onrowcommand="gvPurchaseDiscards_RowCommand">
                <Columns>                 
                <asp:TemplateField HeaderText="PurchaseDiscardsID" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="PurchaseDiscardsID1" runat="server" 
                                Text='<%# Eval("PurchaseDiscardsID") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="S.N.">
                        <ItemTemplate>
                            <asp:Label ID="lblS0" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Discards Date">
                        <ItemTemplate>
                            <asp:Label ID="lblDiscardsDate" runat="server" Text='<%# Eval("PurchaseDiscardsDateM") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Supplier">
                        <ItemTemplate>
                            <asp:Label ID="lblSupplier" runat="server" Text='<%# Eval("SupplierName") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>                  
                    <asp:TemplateField HeaderText="Total Discards">
                        <ItemTemplate>
                            <asp:Label ID="lblTotalPrice" runat="server" Text='<%# Eval("TotalReturn") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="imgbtnEdit" runat="server" AlternateText="Edit" 
                                CausesValidation="false" CommandArgument='<%#Eval("PurchaseDiscardsID") %>' 
                                CommandName="select" Height="30px" ImageAlign="Middle" ImageUrl="~/img/1270383845_edit.png" 
                                 ToolTip="select" 
                                Width="35px" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="imgbtnDelete" runat="server" AlternateText="Delete" 
                                CausesValidation="false" CommandArgument='<%#Eval("PurchaseDiscardsID") %>' 
                                CommandName="del" Height="30px" ImageAlign="Middle" ImageUrl="~/img/delete.png" 
                                OnClientClick="return confirm('Are you sure ?');" ToolTip="Delete" 
                                Width="35px" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <asp:Label ID="lblNoData" runat="server" Text="No Data"></asp:Label>
                </EmptyDataTemplate>
            </asp:GridView>
        </td>
    </tr>
</table>

</asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>


         <%--End of Sucess ModalPopup --%><%--End of Sucess ModalPopup --%>
    <cc1:ModalPopupExtender ID="ModalExtnd1" runat="server" TargetControlID="lblHidden"
        PopupControlID="SummaryDiv" BackgroundCssClass="modalBackground" DynamicServicePath=""
        Enabled="True" CancelControlID="btnclose">
    </cc1:ModalPopupExtender>
    <div id="SummaryDiv" runat="server" align="center" class="confirm" style="display: none">
        <table align="center" class="confirmtable">
            <tr>
                <td valign="top">
                    <img align="middle" src="../img/warning.gif" />
                    <asp:Label ID="LabelMsg" runat="server" Text="PLZ,Enter The following :-"></asp:Label>
                    &nbsp;&nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="2" align="left">
                    <asp:ValidationSummary ID="ValidationSummary3" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="btnclose" runat="server" Text="Close" />
                    <%--End of Sucess ModalPopup --%>
                </td>
            </tr>
        </table>
    </div>
    <asp:Label ID="lblHidden" runat="server" Text="hidden" CssClass="hidelbl"></asp:Label>
    <%--End of Sucess ModalPopup --%><%--End of Sucess ModalPopup --%><%--End of Sucess ModalPopup --%>
    <cc1:ModalPopupExtender ID="mdl_Fail" runat="server" TargetControlID="lbl_Fail" PopupControlID="FailDiv"
        BackgroundCssClass="modalBackground" DynamicServicePath="" Enabled="True" CancelControlID="btnclose_f">
    </cc1:ModalPopupExtender>
    <div id="FailDiv" runat="server" align="center" class="confirm" style="display: none">
        <table align="center" class="confirmtable" id="faliur" runat="server">
            <tr>
                <td valign="top">
                    <img align="middle" src="../img/achtung.gif" />
                    <asp:Label ID="Label20" runat="server" Font-Bold="True" ForeColor="Red" Text="Error,plz try again. "></asp:Label>
                    &nbsp;&nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="btnclose_f" runat="server" Text="Close" />
                </td>
            </tr>
        </table>
    </div>
    <asp:Label ID="lbl_Fail" runat="server" Text="hidden" CssClass="hidelbl"></asp:Label>
    <%--End of Sucess ModalPopup --%>
    <%--End of Sucess ModalPopup --%>
    <cc1:ModalPopupExtender ID="mdl_Sucess" runat="server" TargetControlID="lbl_Sucess"
        PopupControlID="SucessDiv" BackgroundCssClass="modalBackground" DynamicServicePath=""
        Enabled="True" CancelControlID="btnclose_s">
    </cc1:ModalPopupExtender>
    <div id="SucessDiv" runat="server" align="center" class="confirm" style="display: none">
        <table align="center" class="confirmtable" id="Sucess" runat="server">
            <tr>
                <td valign="top">
                    <img align="middle" src="../img/accept.png" />
                    <asp:Label ID="Label5" runat="server" Font-Bold="True" ForeColor="Green" Text="Process Complete Successfully. "></asp:Label>
                    &nbsp;&nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="btnclose_s" Text="Close" runat="server" />
                </td>
            </tr>
        </table>
    </div>
    <asp:Label ID="lbl_Sucess" runat="server" Text="hidden" CssClass="hidelbl"></asp:Label>
    
</asp:Content>


