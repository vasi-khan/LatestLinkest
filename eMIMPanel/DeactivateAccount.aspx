<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="DeactivateAccount.aspx.cs" Inherits="eMIMPanel.DeactivateAccount" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <cc1:ToolkitScriptManager ID="scrmg" runat="server" EnablePageMethods="true">
    </cc1:ToolkitScriptManager>
    <%--  <asp:UpdatePanel ID="updt" runat="server">
        <ContentTemplate>--%>
    <div class="container-fluid">
        <nav aria-label="breadcrumb" class="my-3">
            <ol class="breadcrumb breadcrumb-info">
                <li class="breadcrumb-item"><a href="index2.aspx">Home</a></li>
                <li class="breadcrumb-item active" aria-current="page">Inactive Accounts</li>
            </ol>
        </nav>
        <div class="card-body">
            <div class="table-responsive">
                <asp:GridView ID="grv" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
                    runat="server" Width="100%" CellPadding="10" BorderColor="#ede8e8" Class="table table-striped table-bordered dt-responsive nowrap dataTable-view">
                    <Columns>
                        <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Sr.No.">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="User ID">
                            <ItemTemplate>
                                <asp:Label ID="lblUserId" runat="server" Text='<%#Eval("UserID")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                         <asp:TemplateField HeaderText="Email">
                            <ItemTemplate>
                                <asp:Label ID="lblEmail" runat="server" Text='<%#Eval("Email")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Last Active Date">
                            <ItemTemplate>
                                <asp:Label ID="lblaccdis" runat="server" Text='<%#Eval("LastActiveDate")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Last Used Date">
                            <ItemTemplate>
                                <asp:Label ID="lblLLd" runat="server" Text='<%#Eval("LastUsedDate")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Last Submitted">
                            <ItemTemplate>
                                <asp:Label ID="lbl1" runat="server" Text='<%#Eval("LastSubmitted")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Last 3 Month Avg">
                            <ItemTemplate>
                                <asp:Label ID="lblname" runat="server" Text='<%#Eval("Last3MonthAvg")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:HiddenField ID="hdnuid" runat="server" Value='<%#Eval("UserID") %>' />
                                <asp:LinkButton ID="lblActivate" runat="server" OnClick="lblActivate_Click" OnClientClick="return confirm('Are you sure you want to Edit the Record?');"><i class="fa fa-recycle"></i></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <%--<FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />--%>
                </asp:GridView>
            </div>
        </div>
    </div>
    <%--  </ContentTemplate>
    </asp:UpdatePanel>--%>
    <!-- Bootstrap core JavaScript-->
    <script src="vendor/jquery/jquery-3.5.1.min.js"></script>
</asp:Content>
