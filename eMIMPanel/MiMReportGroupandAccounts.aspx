<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="MiMReportGroupandAccounts.aspx.cs" Inherits="eMIMPanel.MiMReportGroupandAccounts" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="scrmg" runat="server" EnablePageMethods="true">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="container-fluid">
                <nav aria-label="breadcrumb" class="my-3">
                    <ol class="breadcrumb breadcrumb-info">
                        <li class="breadcrumb-item"><a href="index2.aspx">Home</a></li>
                        <li class="breadcrumb-item active" aria-current="page">MiM Report Group & Accounts</li>
                    </ol>
                </nav>

                <div class="row">
                    <div class="col-xl-12 col-lg-12">
                        <div class="card bg-primary border-light shadow-soft mb-4">
                            <div class="card-body px-4">
                                <asp:Panel ID="pnlMain" runat="server">
                                    <div class="row">
                                        <div class="col-md-3">
                                            <label>Group Name</label>
                                            <asp:TextBox ID="txtGroupName" runat="server" CssClass="form-control" PlaceHolder="GROUP NAME" MaxLength="20"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3">
                                            <label>Server</label>
                                            <asp:DropDownList ID="ddlServer" runat="server" CssClass="form-control">
                                                <asp:ListItem Selected="True" Value="" Text="Others"></asp:ListItem>
                                                <asp:ListItem Value="10.10.33.252" Text="Main"></asp:ListItem>
                                                <asp:ListItem Value="10.10.31.17" Text="SMPP"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-1 py-4">
                                            <asp:Button ID="btnSubmit" runat="server" Text="SUBMIT" CssClass="btn btn-success btn-sm" OnClick="btnSubmit_Click" />
                                        </div>
                                        <div class="col-md-1 py-4">
                                            <asp:Button ID="btnReset" runat="server" Text="RESET" CssClass="btn btn-danger btn-sm" OnClick="btnReset_Click" />
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="mt-5" runat="server" id="divgrdtran">
                    <asp:GridView ID="grd" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
                        runat="server" Width="100%" CellPadding="10"
                        BorderColor="#ede8e8" Class="table table-striped table-bordered dt-responsive dataTable-view">
                        <Columns>
                            <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Sr No">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Group Name" HeaderStyle-Width="30%">
                                <ItemTemplate>
                                    <asp:Label ID="lblgpName" runat="server" Text='<%#Eval("Client")%>' Width="100%"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Server" HeaderStyle-Width="30%">
                                <ItemTemplate>
                                    <asp:Label ID="lblServer" runat="server" Text='<%#Eval("Server")%>' Width="100%"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="No of Accounts" HeaderStyle-Width="30%">
                                <ItemTemplate>
                                    <asp:Label ID="lblnoofacc" runat="server"  Text='<%#Eval("NoAcc")%>' Width="100%"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <table style="width: 100%;">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lblEmpty" Text="No Data Found !!" Style="color: Red;" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script src="OffLineCDN/JS%20File/jquery-3.6.0.min.js"></script>
    <script>
        // Datatable Script 
        $(document).ready(function () {
            $('.dataTable-view').DataTable();
            $('.dataTable-view1').DataTable({ paging: false });

        });
    </script>
</asp:Content>
