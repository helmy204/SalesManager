<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ManagePurchases.aspx.cs" Inherits="Admin_ManagePurchases" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="javascript" type="text/javascript">
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
    </script>
    <asp:Panel ID="pnlControl" runat="server" GroupingText="Purchase Control" DefaultButton="imgbtnSave">
        <asp:UpdatePanel ID="upnlControl" runat="server">
            <ContentTemplate>
                <table width="100%">
                    <tr>
                        <td style="width: 147px" class="tableCell">
                            <asp:Label ID="lblPurchaseID" runat="server" Text="Purchase Number : "></asp:Label>
                        </td>
                        <td colspan="4">
                            <asp:TextBox ID="txtID" runat="server" Enabled="False" ReadOnly="True" 
                                Width="50px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 147px" class="tableCell">
                            <asp:Label ID="lblSupplier" runat="server" Text="Supplier : "></asp:Label>
                        </td>
                        <td colspan="4">
                            <asp:DropDownList ID="ddlSupplier" runat="server" AppendDataBoundItems="True" 
                                AutoPostBack="True" onselectedindexchanged="ddlSupplier_SelectedIndexChanged">
                                <asp:ListItem Value="0">Select</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlSupplier"
                                Display="Dynamic" ErrorMessage="Enter Supplier ." InitialValue="0" ValidationGroup="i">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="tableCell" style="width: 147px">
                            <asp:Label ID="lblEmployee" runat="server" Text="Employee : "></asp:Label>
                        </td>
                        <td colspan="4">
                            <asp:DropDownList ID="ddlEmployee" runat="server" AppendDataBoundItems="True">
                                <asp:ListItem Value="0">Select</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tableCell" style="width: 147px">
                            <asp:Label ID="lblPurchaseDateM" runat="server" Text="PurchaseDate M : "></asp:Label>
                        </td>
                        <td colspan="4">
                            <asp:TextBox ID="txtPurchaseDateM" runat="server" AutoPostBack="True" OnTextChanged="txtPurchaseDateM_TextChanged"></asp:TextBox>
                            <cc1:CalendarExtender ID="txtPurchaseDateM_CalendarExtender" runat="server" Enabled="True"
                                TargetControlID="txtPurchaseDateM">
                            </cc1:CalendarExtender>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPurchaseDateM"
                                Display="Dynamic" ErrorMessage="Enter PurchaseDate ." ValidationGroup="i">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="tableCell" style="width: 147px">
                            <asp:Label ID="lblPurchaseDateH" runat="server" Text="PurchaseDate H : "></asp:Label>
                        </td>
                        <td colspan="4">
                            <asp:TextBox ID="txtPurchaseDateH" runat="server" Enabled="False"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tableCell" style="width: 147px">
                            <asp:Label ID="lblNotes" runat="server" Text="Notes  : "></asp:Label>
                        </td>
                        <td colspan="4">
                            <asp:TextBox ID="txtNotes" runat="server" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <asp:Panel ID="pnlProduct" runat="server" GroupingText="Products" DefaultButton="imgbtnAdd">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:GridView ID="gvProducts" runat="server" DataKeyNames="PurchasesDetailID,PurchaseID,ProductID,UnitID"
                                                AutoGenerateColumns="False" OnRowCommand="gvProducts_RowCommand" OnRowDataBound="gvProducts_RowDataBound"
                                                Width="100%" OnDataBound="gvProducts_DataBound" 
                                                EnableModelValidation="True">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="PurchasesDetailID" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPurchasesDetailID" Text='<%# Eval("PurchasesDetailID") %>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="PurchaseID" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPurchaseID" Text='<%# Eval("PurchaseID") %>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="S.N.">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblS" Text='<%# Container.DataItemIndex + 1 %>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Product">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="ddlProduct" runat="server" Width="100px">
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator33" runat="server" ControlToValidate="ddlProduct"
                                                                Display="Dynamic" ErrorMessage="Enter Product" InitialValue="0" ValidationGroup="i">*</asp:RequiredFieldValidator>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="P.Qty.">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtPQTY" runat="server" AutoPostBack="True" 
                                                                Text='<%# Eval("PQTY") %>' Width="70px" ontextchanged="txtPQTY_TextChanged" ></asp:TextBox>
                                                                          <asp:RequiredFieldValidator ID="RequiredFieldValidator44" runat="server" ControlToValidate="txtPQTY"
                                                                Display="Dynamic" ErrorMessage="Enter P.QTY." ValidationGroup="i">*</asp:RequiredFieldValidator>

                                                                 <asp:RegularExpressionValidator ID="RegularExpressionValidator111" runat="server" ControlToValidate="txtPQTY"
                                                                Display="Dynamic" ErrorMessage="Enter Valid P.QTY." ValidationExpression="[-+]?[0-9]*\.?[0-9]+">*</asp:RegularExpressionValidator>
                                                                <asp:HiddenField ID="hfoldQty" runat="server" Value='<%# Eval("oldQty") %>'  />

                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Unit">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="ddlUnits" runat="server" AutoPostBack="True" 
                                                                onselectedindexchanged="ddlUnits_SelectedIndexChanged">
                                                            </asp:DropDownList>

                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlUnits"
                                                                Display="Dynamic" ErrorMessage="Enter Unit" InitialValue="0" ValidationGroup="i">*</asp:RequiredFieldValidator>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:TemplateField HeaderText="Quantity" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtQuantity" Text='<%# Eval("Quantity") %>' runat="server" AutoPostBack="True" ReadOnly="true"
                                                                OnTextChanged="txtQuantity_TextChanged" Width="90px"></asp:TextBox>
                                                           
                                                           
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtQuantity"
                                                                Display="Dynamic" ErrorMessage="Enter Quantity." ValidationGroup="i">*</asp:RequiredFieldValidator>

                                                                 <asp:RegularExpressionValidator ID="RegularExpressionValidator11" runat="server" ControlToValidate="txtQuantity"
                                                                Display="Dynamic" ErrorMessage="Enter Valid Quantity." ValidationExpression="[-+]?[0-9]*\.?[0-9]+">*</asp:RegularExpressionValidator>

                                                                   
                                                        </ItemTemplate>
                                                    </asp:TemplateField>--%>
                                                    <asp:TemplateField HeaderText="Unit Price">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtPUnitPrice" Text='<%# Eval("PUnitPrice") %>' runat="server" AutoPostBack="True"
                                                                OnTextChanged="txtPUnitPrice_TextChanged" Width="90px"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtPUnitPrice"
                                                                Display="Dynamic" ErrorMessage="Enter Unit Price." ValidationGroup="i">*</asp:RequiredFieldValidator>
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtPUnitPrice"
                                                                Display="Dynamic" ErrorMessage="Enter Valid Price." ValidationExpression="[-+]?[0-9]*\.?[0-9]+">*</asp:RegularExpressionValidator>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total Price">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtTotalPrice" runat="server" ReadOnly="true" Width="90px" Text='<%# Eval("TotalPrice") %>'></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="imgbtnDelete" runat="server" AlternateText="Delete" CausesValidation="false"
                                                                CommandArgument='<%#Eval("PurchasesDetailID") %>' CommandName="del" Height="30px"
                                                                ImageAlign="Middle" ImageUrl="~/img/delete.png" OnClientClick="return confirm('Are you sure ?');"
                                                                ToolTip="Delete" Width="35px" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tableCell">
                                            <table style="background-color: #5D7B9D">
                                                <tr>
                                                    <td class="tableCell">
                                                        <asp:ImageButton ID="imgbtnAdd" runat="server" AlternateText="Add" ValidationGroup="i"
                                                            CommandName="Add" Height="30px" ImageAlign="Middle" ImageUrl="~/img/add.png"
                                                            ToolTip="Add" OnClick="imgbtnAdd_Click" />
                                                    </td>
                                                    <td class="tableCell" style="">
                                                        &nbsp;</td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 147px">
                            <asp:Label ID="lblIsCash1" runat="server" Text="Is Cash ? "></asp:Label>
                        </td>
                        <td>
                            <asp:CheckBox ID="cboxIsCash" runat="server" AutoPostBack="True" 
                                oncheckedchanged="cboxIsCash_CheckedChanged" />
                        </td>
                        <td colspan="2">
                            <asp:Label ID="lblpurchase" runat="server" Text="Total Purchase : "></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblTotalPurchasePrice" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 147px">
                            <asp:Label ID="Label25" runat="server" Text="Supplier Balance:"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblSupplierBalance" runat="server">0</asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:Label ID="Label21" runat="server" Text="Payment:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPayment" runat="server" Width="50px" AutoPostBack="True" 
                                ontextchanged="txtPayment_TextChanged">0</asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvPayments" runat="server" 
                                ControlToValidate="txtPayment" Display="Dynamic" ErrorMessage="Enter Payments." 
                                ValidationGroup="i">*</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" 
                                ControlToValidate="txtPayment" Display="Dynamic" 
                                ErrorMessage="Enter Valid Payment." ValidationExpression="[-+]?[0-9]*\.?[0-9]+" 
                                ValidationGroup="i">*</asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 147px">
                            <asp:Label ID="Label28" runat="server" Text="new Supplier Balance:"></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:Label ID="lblNewSupplierBalance" runat="server" Text="0"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label27" runat="server" Text="Rest:"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblRest" runat="server" Text="0"></asp:Label>
                            <asp:HiddenField ID="hfOldRest" runat="server" Value="0" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 147px">
                            &nbsp;</td>
                        <td colspan="4">
                            <asp:ImageButton ID="imgbtnSave" runat="server" AlternateText="Save" 
                                Height="35px" ImageAlign="Middle" ImageUrl="~/img/1270384072_filesave.png" 
                                OnClick="imgbtnSave_Click" OnClientClick="return ShowModalDialog('i')" 
                                ToolTip="Save" />
                            <asp:ImageButton ID="imgbtnClear" runat="server" AlternateText="Clear" 
                                CausesValidation="false" Height="35px" ImageAlign="Middle" 
                                ImageUrl="~/img/Reset.png" OnClick="imgbtnClear_Click" ToolTip="Clear" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <asp:Panel ID="pnlShow" runat="server" GroupingText="Purchases Show">
        <asp:UpdatePanel ID="upnlShow" runat="server">
            <ContentTemplate>
                <asp:TextBox ID="txtSearchValue" runat="server"></asp:TextBox>
                <cc1:AutoCompleteExtender ID="txtSearchValue_AutoCompleteExtender" runat="server"
                    DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionList"
                    ServicePath="" TargetControlID="txtSearchValue" UseContextKey="True">
                </cc1:AutoCompleteExtender>
                <cc1:TextBoxWatermarkExtender ID="txtSearchPurchase_TextBoxWatermarkExtender" runat="server"
                    Enabled="True" TargetControlID="txtSearchValue" WatermarkCssClass="WaterMark"
                    WatermarkText="Find Purchase">
                </cc1:TextBoxWatermarkExtender>
                <asp:ImageButton ID="imgbtnSearch" runat="server" Height="27px" ImageAlign="Middle"
                    ImageUrl="~/img/Search.png" AlternateText="Search" CausesValidation="False"
                    ToolTip="Search" OnClick="imgbtnSearch_Click" />

                <asp:GridView ID="gvPurchases" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                    DataKeyNames="PurchaseID" Width="100%" OnPageIndexChanging="gvPurchases_PageIndexChanging"
                    OnRowCommand="gvPurchases_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="PurchaseID" HeaderText="ID" ReadOnly="True" SortExpression="PurchaseID" />
                        <asp:BoundField DataField="SupplierName" HeaderText="Supplier" ReadOnly="True" SortExpression="SupplierName" />
                        <asp:TemplateField HeaderText="PurchaseDate M">
                            <ItemTemplate>
                                <asp:Label ID="lblPurchaseDateM" Text='<%# Convert.ToDateTime(Eval("PurchaseDateM")).ToString("MM/dd/yyyy") %>'
                                    runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="PurchaseDateH" HeaderText="PurchaseDate H" ReadOnly="True"
                            SortExpression="PurchaseDateH" />
                        <asp:BoundField DataField="Notes" HeaderText="Notes" ReadOnly="True" SortExpression="Notes" />
                        <asp:CheckBoxField DataField="IsCash" HeaderText="Is Cash" ReadOnly="True" SortExpression="IsCash" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="imgbtnEdit" runat="server" Height="35px" ImageAlign="Middle"
                                    ImageUrl="~/img/1270383845_edit.png" CommandName="select" CausesValidation="false"
                                    CommandArgument='<%#Eval("PurchaseID") %>' AlternateText="Edit" ToolTip="Edit" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="imgbtnDelete" runat="server" AlternateText="Delete" CausesValidation="false"
                                    CommandArgument='<%#Eval("PurchaseID") %>' CommandName="del" Height="35px" ImageAlign="Middle"
                                    ImageUrl="~/img/delete.png" OnClientClick="return confirm('Are you sure ?');"
                                    ToolTip="Delete" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
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
