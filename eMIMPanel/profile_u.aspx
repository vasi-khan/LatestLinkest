<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="profile_u.aspx.cs" Inherits="eMIMPanel.profile_u" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="scrmg" runat="server" EnablePageMethods="true">
    </asp:ToolkitScriptManager>
    <main>
        <div class="container-fluid">
            <nav aria-label="breadcrumb" class="my-3">
                <ol class="breadcrumb breadcrumb-info">
                    <li class="breadcrumb-item"><a href="#">Home</a></li>
                    <li class="breadcrumb-item active" aria-current="page">Profile Details</li>
                </ol>
            </nav>
            <div class="row">

                <!-- Area Chart -->
                <div class="col-xl-12 col-lg-12">
                    <!-- Basic Card Example -->
                    <div class="card bg-primary border-light shadow-soft mb-4">
                        <div class="card-header py-3 bg-primary">
                            <h6 class="m-0 font-weight-bold"><i class="far fa-user-circle"></i>Profile</h6>
                        </div>
                        <div class="card-body">
                            <form class="needs-validation" novalidate>
                                <div class="form-row">
                                    <div class="col-md-6 mb-3">
                                        <label for="validationCustom05" class="font-weight-bold">Account Id :</label>
                                        <asp:TextBox ID="txtUserId" runat="server" Text="test" class="form-control" disabled="disabled"></asp:TextBox>

                                    </div>
                                    <div class="col-md-6 mb-3">
                                        <label for="validationCustom05" class="font-weight-bold">Alpha Sender Id * :</label>
                                        <asp:DropDownList ID="ddlSender" runat="server" class="custom-select">
                                        </asp:DropDownList>

                                    </div>
                                    <%--<div class="col-md-4 mb-3">
                                            <label for="validationCustom05" class="font-weight-bold">Default SMS Type :</label>
                                            <select class="custom-select">
                                                  <option value="4">Promotional</option>
                                                  <option value="0">Transactional-1</option>
                                                  <option value="3">Transactional-OptIn</option>
                                                  <option selected="selected" value="6">Transactional-3</option>
                                                  <option value="13">Transactional-OTP</option>
                                                  <option value="9">Transactional-2</option>  
                                            </select>
                                            <div class="invalid-feedback">
                                              Please select a valid state.
                                            </div>
                                          </div>--%>
                                </div>
                                <div class="form-row">
                                    <div class="col-md-6 mb-3">
                                        <label for="validationCustom01" class="font-weight-bold">Full Name</label>
                                        <asp:TextBox ID="txtName" runat="server" Text="" class="form-control" disabled="disabled"></asp:TextBox>
                                    </div>
                                    <div class="col-md-6 mb-3">
                                        <label for="validationCustom05" class="font-weight-bold">DND :</label>
                                        <div class="">
                                            <div>
                                                <asp:RadioButton ID="rdbDnd" runat="server" Text="DND Allow" GroupName="grpdnd" /> &nbsp;&nbsp;
                                                <asp:RadioButton ID="rdbNonDnd" runat="server" Text="DND Block" GroupName="grpdnd" />
                                            </div>

                                        </div>
                                    </div>
                                </div>
                                <div class="form-row">
                                    <div class="col-md-6 mb-3">
                                        <label for="validationCustom03" class="font-weight-bold">Company Name</label>
                                        <asp:TextBox ID="txtComp" runat="server" Text="" class="form-control" disabled="disabled"></asp:TextBox>
                                    </div>
                                    <div class="col-md-6 mb-3">
                                        <label for="validationCustom03" class="font-weight-bold">Website : www.</label>
                                        <asp:TextBox ID="txtWebsite" runat="server" Text="" class="form-control"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-row">
                                    <div class="col-md-6 mb-3">
                                        <label for="validationCustom05" class="font-weight-bold">Mobile No.</label>
                                        <asp:TextBox ID="txtMobile" runat="server" Text="" class="form-control" disabled="disabled"></asp:TextBox>
                                        
                                    </div>
                                    <div class="col-md-6 mb-3">
                                        <label for="validationCustom05" class="font-weight-bold">DLT No</label>
                                        <asp:TextBox ID="txtDLT" runat="server" Text="" class="form-control" disabled="disabled"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-row">
                                    <div class="col-md-6 mb-3">
                                        <label for="validationCustom06" class="font-weight-bold">Email Id</label>
                                        <asp:TextBox ID="txtMail" runat="server" Text="" class="form-control" disabled="disabled"></asp:TextBox>
                                    </div>
                                    <div class="col-md-6 mb-3">
                                        <asp:Label ID="lblPEID" runat="server" class="font-weight-bold" Text="PE Id"></asp:Label>
                                        <asp:TextBox ID="txtPEID" runat="server" Text="" class="form-control" disabled="disabled"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-row">
                                    <div class="col-md-6 mb-3">
                                        <label for="validationCustom06" class="font-weight-bold">Account Creation Date :</label>
                                        <asp:TextBox ID="txtACcreateDt" runat="server" Text="" class="form-control" disabled="disabled"></asp:TextBox>
                                    </div>
                                    <div class="col-md-6 mb-3">
                                        <label for="validationCustom05" class="font-weight-bold">Expiry : </label>
                                        <asp:TextBox ID="txtExpDt" runat="server" Text="" class="form-control" disabled="disabled"></asp:TextBox>
                                    </div>
                                </div>
                                <asp:LinkButton class="btn btn-primary btn-icon-split" id="btnUpdate" runat="server" OnClick="btnUpdate_Click">
                                            <span class="text-success"><i class="fas fa-user-edit"></i></span>
                                            <span class="text-success font-weight-bold">Update</span>
                                        </asp:LinkButton>
                            </form>
                        </div>
                    </div>
                </div>

            </div>
        </div>
    </main>
</asp:Content>
