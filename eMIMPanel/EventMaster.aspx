<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="EventMaster.aspx.cs" Inherits="eMIMPanel.EventMaster" %>

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
                        <li class="breadcrumb-item active" aria-current="page">Event Master</li>
                    </ol>
                </nav>

                <div class="row">
                    <div class="col-xl-12 col-lg-12">
                        <div class="card bg-primary border-light shadow-soft mb-4">
                            <div class="card-body px-4">
                                <asp:Panel ID="pnlMain" runat="server">
                                    <div class="row">
                                        <div class="col-md-3">
                                            <label>Event Code</label>
                                            <asp:TextBox ID="txtEvntCode" runat="server" CssClass="form-control" onkeypress="return /^[a-zA-Z0-9-_\s]{0,20}$/.test(this.value+event.key);" MaxLength="20"></asp:TextBox>
                                        </div>
                                        <div class="col-md-6">
                                            <label>Event Name</label>
                                            <asp:TextBox ID="txtEvntName" runat="server" CssClass="form-control" onkeypress="return /^[a-zA-Z0-9-_.\s]{0,100}$/.test(this.value+event.key);" MaxLength="100"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3 py-4">
                                            <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-success" Text="SUBMIT" OnClick="btnSubmit_Click" />
                                            <asp:Button ID="btnReset" runat="server" CssClass="btn btn-danger" Text="RESET" OnClick="btnReset_Click" />
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>

                            <div class="mt-5 px-4" runat="server" id="divgrdtran">
                                <asp:GridView ID="grd" border="0" runat="server" CellPadding="10" 
                                    CssClass="table table-striped table-bordered dt-responsive wrap dataTable-view" AutoGenerateColumns="false" Style="width: 100%">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="15%" HeaderText="Sr No">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Event Code" HeaderStyle-Width="20%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEventID" runat="server" Text='<%#Eval("EventID")%>' Width="100%"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Event Name" HeaderStyle-Width="55%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEventName" runat="server" Text='<%#Eval("EventName")%>' Width="100%"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Edit" HeaderStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:HiddenField Value='<%#Eval("EventID") %>' ID="HD_EventID" runat="server" />
                                                <asp:LinkButton ID="lnkbtnEdit" class="fa fa-edit  edit" runat="server" OnClick="lnkbtnEdit_Click"></asp:LinkButton>
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
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
     <script src="vendor/jquery/jquery-3.5.1.min.js"></script>
</asp:Content>
