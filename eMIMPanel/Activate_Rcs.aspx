<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Activate_Rcs.aspx.cs" Inherits="eMIMPanel.Activate_Rcs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>MIM Activate RCS</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <cc1:ToolkitScriptManager ID="scrmg" runat="server" EnablePageMethods="true">
    </cc1:ToolkitScriptManager>
    <div class="container-fluid">
         <nav aria-label="breadcrumb" class="my-3">
                        <ol class="breadcrumb breadcrumb-info">
                            <li class="breadcrumb-item"><a href="#">Home</a></li>
                            <li class="breadcrumb-item active" aria-current="page">Activate RCS</li>
                        </ol>
                    </nav>

        <div class="row">
                        
                        <!-- Area Chart -->
                        <div class="col-xl-12 col-lg-12">
                            <!-- Basic Card Example -->
                            <div class="card bg-primary border-light shadow-soft mb-4">
                                <div class="card-header py-3 bg-primary">
                                    <h6 class="m-0 font-weight-bold"><i class="far fa-user-circle"></i>Activate RCS</h6>
                                </div>
                                <div class="col-md-6" style="text-align: right; margin-left:50%;">
                                            <asp:LinkButton class="btn btn-primary btn-icon-split" ID="lnkEditAC" runat="server" OnClick="lnkEditAC_Click">
                                            <span class="text-success"><i class="fas fa-user-edit"></i></span>
                                            <span class="text-success font-weight-bold">Edit Account</span>
                                            </asp:LinkButton>
                                        </div>
                                <div class="card-body">
                                    <asp:Panel ID="pnledit" runat="server" Visible="false">
                                            <div class="form-row">
                                                <div class="col-md-6 mb-3">
                                                    <asp:TextBox class="form-control" ID="txtAccId" runat="server" placeholder="Enter User ID to Edit" />
                                                </div>
                                                <div class="col-md-6 mb-3">
                                                    <asp:LinkButton class="btn btn-primary btn-icon-split" ID="lnkShowAC" runat="server" OnClick="lnkShowAC_Click">
                                                    <span class="text-success"><i class="fas fa-eye"></i></span>
                                                    <span class="text-success font-weight-bold">Show Details</span>
                                                    </asp:LinkButton>
                                                </div>
                                            </div>
                                            <div class="form-row">
                                            <div class="col-md-6 mb-3">
                                                    <asp:TextBox class="form-control" ID="txtrcsaccid" runat="server" placeholder="Enter Sender ID" />
                                                <div class="invalid-feedback">
                                                    Please Enter Sender ID.
                             
                                                </div>
                                            </div>
                                                <div class="col-md-6 mb-3">
                                                <asp:TextBox class="form-control" ID="txtrcsapikey" runat="server" placeholder="Enter API Key" />
                                                <div class="invalid-feedback">
                                                    Please Enter Api Key.
                                                </div>
                                            </div>
                                        </div>
                                            <div class="form-row">
                                            <div class="col-md-6 mb-3">
                                                <asp:TextBox class="form-control" ID="txtrcsapiurl" runat="server" placeholder="Enter API URL" ToolTip="API URL" />
                                                <div class="invalid-feedback">
                                                    Please provide a API URL.                             
                                                </div>
                                            </div>
                                        </div>
                                            <asp:LinkButton class="btn btn-primary btn-icon-split" id="lnkbtntoedit" runat="server" OnClick="lnkbtntoedit_Click">
                                            <span class="text-success"><i class="fas fa-user"></i></span>
                                            <span class="text-success font-weight-bold">Update</span>
                                        </asp:LinkButton>
                                        </asp:Panel>
                                    <asp:Panel id="pnlMain" runat="server" >
                                        <div class="form-row">
                                            <div class="col-md-6 mb-3">
                                                    <asp:TextBox class="form-control" ID="txtuser" runat="server" placeholder="Enter User ID" />
                                                <div class="invalid-feedback">
                                                    Please Enter User ID.
                             
                                                </div>
                                            </div>
                                            <div class="col-md-6 mb-3">
                                                    <asp:TextBox class="form-control" ID="txtAccountID" runat="server" placeholder="Enter Account ID" />
                                                <div class="invalid-feedback">
                                                    Please Enter Account ID.
                             
                                                </div>
                                            </div>
                                        </div>


                                        <div class="form-row">
                                            
                                            <div class="col-md-6 mb-3">
                                                <asp:TextBox class="form-control" ID="txtapikey" runat="server" placeholder="Enter API Key" />
                                                <div class="invalid-feedback">
                                                    Please Enter Api Key.
                                                </div>
                                            </div>
                                            <div class="col-md-6 mb-3">
                                                <!-- <label for="validationCustom05" class="font-weight-bold">Mobile No. 01</label> -->
                                                <asp:TextBox class="form-control" ID="txtapiurl" runat="server" placeholder="Enter API URL" ToolTip="API URL" />
                                                <%--<input type="text" class="form-control" id="validationCustom05" placeholder="Mobile No. 01">--%>
                                                <div class="invalid-feedback">
                                                    Please provide a API URL.                             
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-row">
                                            
                                            <div class="col-md-6 mb-3">
                                                <!-- <label for="validationCustom05" class="font-weight-bold">Mobile No. 01</label> -->
                                                <asp:TextBox class="form-control" ID="txtmobile" runat="server" placeholder="Enter Mobile Number" ToolTip="Mobile No" />
                                                <%--<input type="text" class="form-control" id="validationCustom05" placeholder="Mobile No. 01">--%>
                                                <div class="invalid-feedback">
                                                    Please Enter Mobile Number.                             
                                                </div>
                                            </div>
                                            <%--<div class="col-md-6 mb-3">
                                                <asp:CheckBox ID="chkVerify" runat="server" Text="Verify" />
                                            </div>--%>
                                        </div>
                                        <asp:LinkButton class="btn btn-primary btn-icon-split" id="lnkverify" runat="server" OnClick="lnkverify_Click">
                                            <span class="text-success"><i class="fas fa-user"></i></span>
                                            <span class="text-success font-weight-bold">Verify</span>
                                        </asp:LinkButton>
                                        <asp:LinkButton class="btn btn-primary btn-icon-split" id="btnActivate" runat="server" OnClick="btnActivate_Click">
                                            <span class="text-success"><i class="fas fa-check-circle text-success"></i></span>
                                            <span class="text-success font-weight-bold">Activate</span>
                                        </asp:LinkButton>
                                       <%-- <asp:Label ID="lblmsg" runat="server" ForeColor="Red" Font-Size="Medium"></asp:Label>--%>
                                        </asp:Panel>
                                    
                                </div>
                            </div>
                        </div>
                        
                        <!-- Pie Chart -->
                        <div class="col-xl-4 col-lg-5"></div>
                    </div>
        </div>
</asp:Content>
