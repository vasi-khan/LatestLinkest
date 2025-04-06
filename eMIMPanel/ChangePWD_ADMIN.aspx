<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="ChangePWD_ADMIN.aspx.cs" Inherits="eMIMPanel.ChangePWD_ADMIN" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <cc1:ToolkitScriptManager ID="scrmg" runat="server" EnablePageMethods="true">
    </cc1:ToolkitScriptManager>
    <asp:UpdatePanel ID="updFormArea" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <main>
                <asp:Timer ID="Timer1" runat="server" OnTick="Timer1_Tick" Enabled="false"></asp:Timer>
                <div class="container-fluid">
                    <nav aria-label="breadcrumb" class="my-3">
                        <ol class="breadcrumb breadcrumb-info">
                            <li class="breadcrumb-item"><a href="#">Home</a></li>
                            <li class="breadcrumb-item active" aria-current="page">Setting</li>
                        </ol>
                    </nav>

                    <div class="row">
                        <div class="col-12">
                            <div class="accordion shadow-soft rounded">
                                <div id="divVerify" runat="server" style="display: none;" class="card card-sm card-body bg-primary border-light mb-0">
                                    <a href="#panel-5" data-target="#panel-5" class="accordion-panel-header" data-toggle="collapse" role="button" aria-expanded="false" aria-controls="panel-1">
                                        <span class="icon-title h6 mb-0 font-weight-bold"><span class="fas fa-user-check"></span>Verification for Change Password</span>
                                        <span class="icon"><span class="fas fa-plus"></span></span>
                                    </a>
                                    <div class="collapse show" id="panel-5">
                                        <div class="pt-3">

                                            <fieldset class="form-group">
                                                <div class="row">
                                                    <legend class="col-form-label col-sm-12 pt-0 font-weight-bold">Click Verify to get a SMS containg link for verification for changing the account password.</legend>
                                                    <%--<div class="col-sm-10">
                                                <input type="text" class="form-control" id="exampleInputText2" aria-describedby="genHelp" placeholder="OTP Enter">
                                                <small id="genHelp" class="form-text text-muted">Please Check Phone SMS</small>
                                            </div>--%>
                                                </div>
                                            </fieldset>
                                            <div class="form-group row justify-content-end">
                                                <div class="col-sm-10 ">
                                                    <asp:LinkButton ID="btnVerify" runat="server" OnClick="btnVerify_Click" class="btn btn-primary text-success my-2 my-lg-0"><i class="fas fa-thumbs-up"></i>Verify</asp:LinkButton>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                                <div id="divProcess" runat="server" style="display: none;" class="card card-sm card-body bg-primary border-light mb-0">
                                    <fieldset class="form-group">
                                        <div class="row">
                                            <legend class="col-form-label col-sm-12 pt-0 font-weight-bold">SMS containg Link to verify the account for changing the password has been sent to your registered mobile number. Please click on the link</legend>
                                        </div>
                                    </fieldset>
                                    <div class="form-group row justify-content-end">
                                        <div class="col-sm-10 ">
                                            <div style="text-align: center;">
                                                <asp:Image ID="img" runat="server" ImageUrl="~/img/loading.gif" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div id="divPwd" runat="server" class="card card-sm card-body bg-primary border-light mb-0" style="display: none;">
                                    <a href="#panel-4" data-target="#panel-4" class="accordion-panel-header" data-toggle="collapse" role="button" aria-expanded="false" aria-controls="panel-1">
                                        <span class="icon-title h6 mb-0 font-weight-bold"><span class="fas fa-unlock-alt"></span>Password</span>
                                        <span class="icon"><span class="fas fa-plus"></span></span>
                                    </a>
                                    <div class="collapse show" id="panel-4">
                                        <div class="pt-3">
                                            <p class="mb-0">

                                                <%--<fieldset class="form-group">
                                            <div class="row">
                                                <legend class="col-form-label col-sm-2 pt-0 font-weight-bold">Old Password:</legend>
                                                <div class="col-sm-10">
                                                    <input type="password" class="form-control" id="exampleInputText1" aria-describedby="textHelp1" placeholder="************">
                                                    <small id="textHelp1" class="form-text text-muted">Please Fill Old Passowrd</small>
                                                </div>
                                            </div>
                                        </fieldset>--%>
                                                <fieldset class="form-group">
                                                    <div class="row">
                                                        <legend class="col-form-label col-sm-2 pt-0 font-weight-bold">New Password:</legend>
                                                        <div class="col-sm-10">
                                                            <%--<input type="text" class="form-control" id="exampleInputText1" aria-describedby="textHelp1" placeholder="">--%>
                                                            <asp:TextBox ID="txtPwd" runat="server" TextMode="Password" MaxLength="20" class="form-control" aria-describedby="textHelp1"></asp:TextBox>
                                                            <small id="textHelp1" class="form-text text-muted">Please Fill New Passowrd</small>
                                                        </div>
                                                    </div>
                                                </fieldset>
                                                <fieldset class="form-group">
                                                    <div class="row">
                                                        <legend class="col-form-label col-sm-2 pt-0 font-weight-bold">Confirm Password:</legend>
                                                        <div class="col-sm-10">
                                                            <asp:TextBox ID="txtConfPwd" runat="server" TextMode="Password" MaxLength="20" class="form-control" aria-describedby="textHelp1"></asp:TextBox>
                                                            <small id="textHelp1" class="form-text text-muted">Please Fill New Passowrd</small>
                                                        </div>
                                                    </div>
                                                </fieldset>
                                                <div class="form-group row justify-content-end">
                                                    <div class="col-sm-10 ">
                                                        <asp:LinkButton ID="btnOk" runat="server" OnClick="btnOk_Click" class="btn btn-primary text-dark my-2 my-lg-0"><i class="fab fa-telegram-plane"></i>Submit</asp:LinkButton>
                                                    </div>
                                                </div>

                                            </p>
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>

                </div>
            </main>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
