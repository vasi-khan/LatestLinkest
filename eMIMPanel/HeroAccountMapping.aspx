<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="HeroAccountMapping.aspx.cs" Inherits="eMIMPanel.HeroAccountMapping" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class=" container-fluid">
        <nav aria-label="breadcrumb" class="my-3">
            <ol class="breadcrumb breadcrumb-info  ">
                <li class="breadcrumb-item active font-weight-bold " aria-current="page">Account Group Location Mapping</li>
            </ol>
        </nav>
        <div class="row">
            <div class="col-md-2">
                <label>User Id</label>
            </div>
            <div class="col-md-3">
                <asp:TextBox ID="txtUserID" runat="server" class="form-control" placeholder="Enter User Id"></asp:TextBox>
            </div>
            <div class="col-md-3">
                <asp:Button ID="btnShow" runat="server" Text="SHOW" CssClass="btn btn-primary" OnClick="btnShow_Click" />
            </div>
        </div>

        <div class="row">
            <div class="col-xl-12 col-lg-12 p-3">
                <div class="card bg-primary border-light shadow-soft mb-4">
                    <div class="card-body">
                        <div class="form-group mb-3">
                            <div class="row">
                                <div class="col-md-3">
                                    <label class="font-weight-bold">Mapped Group Location</label>
                                </div>
                                <div class="col-md-12"></div>
                                <div class="col-md-2">
                                    <label>Group Location</label>
                                </div>
                                <div class="col-md-3">
                                    <asp:DropDownList ID="ddlgroup" runat="server" class="custom-select" AutoPostBack="true">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-3">
                                    <asp:Button ID="btnsubmit" runat="server" Text="Submit" OnClick="btnsubmit_Click" CssClass="btn btn-primary" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="card-body">
            <asp:GridView ID="grv" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
                runat="server" Width="100%" CellPadding="10"  Class="table table-striped table-bordered dt-responsive nowrap dataTable-view" 
                BorderColor="#ede8e8">
                <Columns>
                       <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Sr No">
                        <ItemTemplate>
                            <%#Container.DataItemIndex+1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-Width="10%" HeaderText="GroupLocationCode">
                        <ItemTemplate>

                            <asp:Label ID="lblSeq" runat="server" Text='<%#Eval("GroupLocationCode")%>'></asp:Label>

                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="GroupLocationName">
                        <ItemTemplate>
                            <asp:Label ID="lblName" runat="server" Text='<%#Eval("CategoryName")%>'></asp:Label>

                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
                </div>
        </div>
    </div>

</asp:Content>
