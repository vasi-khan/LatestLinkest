<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Hyundai_InsuranceSubClientMaster.aspx.cs" Inherits="eMIMPanel.Hyundai_InsuranceSubClientMaster" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <nav aria-label="breadcrumb" class="my-3">
                        <ol class="breadcrumb breadcrumb-info">
                            <li class="breadcrumb-item"><a href="#">Home</a></li>
                            <li class="breadcrumb-item active" aria-current="page">Hundai Insurance SubClient Master</li>
                        </ol>
                    </nav>

        <div class="row">
            <div class="col-xl-12 col-lg-12">
                <div class="card bg-primary border-light shadow-soft mb-4">
                                <div class="card-header py-3 bg-primary">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <h6 class="m-0 font-weight-bold"><i class="far fa-user-circle"></i>Hyundai Insurance Sub Client Master</h6>
                                        </div>
                                        </div>

                                    </div>
                    <div class="card-body">
                        <div class="row">
                            <div class="col-sm-6">
                            <asp:Label runat="server" class="col-form-label"> Insurance Company Code</asp:Label>
                                <asp:TextBox runat="server" class="form-control" ID="txtInsuranceCompCode" MaxLength="20"></asp:TextBox>
                                </div>
                            <div class="col-sm-6">
                            <asp:Label runat="server" class="col-form-label"> Insurance Company Name</asp:Label>
                                <asp:TextBox runat="server" ID="txtInsuranceCompName" class="form-control"></asp:TextBox>
                                </div>

                        </div>
                        <div class="row">
                            <div class="col-sm-4"></div>
                            <div class="col-sm-4 p-2 mx-auto" style="align-self:center">

                                <asp:Button runat="server" CssClass="btn btn-success" Text="Submit" ID="btnSubmit"  OnClick="btnSubmit_Click"/>
                                <asp:Button runat="server" CssClass="btn btn-danger" Text="Reset" ID="btnReset" OnClick="btnReset_Click" />

                            </div>
                            <div class="col-sm-4"></div>
                        </div>

                        <div class="row">
                            <div class="col-sm-12">
                                <asp:GridView runat="server" ID="grd" Class="table table-striped table-bordered dt-responsive nowrap dataTable-view" AutoGenerateColumns="false">
                                    <Columns>
                                         <asp:TemplateField HeaderText="S No">
                                            <ItemTemplate>
                                               <%#Container.DataItemIndex + 1%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Insurance Company Code">
                                            <ItemTemplate>
                                                <asp:Label   ID="lblCompCode" runat="server" Text='<%#Eval("SMS_IC_Code")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Insurance Company Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCompName" runat="server" Text='<%#Eval("SMS_IC_Name")%>'></asp:Label>
                                            </ItemTemplate>

                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Edit">
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" Text="Edit" ID="btn_Edit" OnClick="btn_Edit_Click"> </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                        </div>
                    </div>
               
                </div>
            </div>
        </div>
</asp:Content>
