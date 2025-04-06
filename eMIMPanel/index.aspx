<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="eMIMPanel.index" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <cc1:ToolkitScriptManager ID="scrmg" runat="server" EnablePageMethods="true">
    </cc1:ToolkitScriptManager>
    <main>
        <asp:UpdatePanel ID="updFormArea" runat="server" UpdateMode="Conditional">
            <ContentTemplate>

                <div class="container-fluid">
                    <nav aria-label="breadcrumb" class="my-3">
                        <ol class="breadcrumb breadcrumb-info">
                            <li class="breadcrumb-item"><a href="#">Home</a></li>
                            <li class="breadcrumb-item active" aria-current="page">Dashboard</li>
                        </ol>
                    </nav>
                    <!-- Content Row -->
                    <div class="row">
                        <div class="col-12">
                            <div class="card bg-primary border-light shadow-soft mb-4">
                                <div class="card-header py-3 bg-primary">
                                    <h6 class="m-0 font-weight-bold"><i class="fas fa-calendar-plus"></i>Todays Count</h6>
                                </div>
                                <div class="card-body py-2">
                                    <div class="row">
                                        <!-- Earnings (Monthly) Card Example -->
                                        <div class="col-12 col-md-6 col-sm-6 col-lg-6 col-xl-4 mb-2 count-view">
                                            <a href="sms-reports.aspx">
                                                <div class="card bg-primary shadow-inset border-light h-100 py-2">
                                                    <div class="card-body">
                                                        <div class="row no-gutters align-items-center">
                                                            <div class="col mr-2">
                                                                <div class="text-xs font-weight-bold text-warning text-uppercase mb-1">Submitted</div>
                                                                <div class="h5 mb-0 font-weight-bold text-gray-800"><asp:Label ID="lblTodaySubmitted" runat="server" Text=""></asp:Label></div>
                                                            </div>
                                                            <div class="col-auto">
                                                                <div class="icon icon-shape shadow-soft rounded-circle">
                                                                    <span class="fas fa-check-circle fa-2x text-warning"></span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </a>
                                        </div>

                                        <!-- Earnings (Monthly) Card Example -->
                                        <div class="col-12 col-md-6 col-sm-6 col-lg-6 col-xl-4 mb-2 count-view">
                                            <a href="sms-reports.aspx">
                                                <div class="card bg-primary shadow-inset border-light h-100 py-2">
                                                    <div class="card-body">
                                                        <div class="row no-gutters align-items-center">
                                                            <div class="col mr-2">
                                                                <div class="text-xs font-weight-bold text-success text-uppercase mb-1">Delivered</div>
                                                                <div class="row no-gutters align-items-center">
                                                                    <div class="col-auto">
                                                                        <div class="h5 mb-0 mr-3 font-weight-bold text-gray-900"><asp:Label ID="lblTodayDelivered" runat="server" Text=""></asp:Label></div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-auto">
                                                                <div class="icon icon-shape shadow-soft rounded-circle">
                                                                    <span class="fas fa-check-double fa-2x text-success"></span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                            </a>
                                        </div>
                                    </div>
                                    <!-- Earnings (Monthly) Card Example -->
                                    <div class="col-12 col-md-6 col-sm-6 col-lg-6 col-xl-4 mb-4 count-view">
                                        <a href="sms-reports.aspx">
                                            <div class="card bg-primary shadow-inset border-light h-100 py-2">
                                                <div class="card-body">
                                                    <div class="row no-gutters align-items-center">
                                                        <div class="col mr-2">
                                                            <div class="text-xs font-weight-bold text-danger text-uppercase mb-1">Failed</div>
                                                            <div class="h5 mb-0 font-weight-bold text-danger-800"><asp:Label ID="lblTodayFailed" runat="server" Text=""></asp:Label></div>
                                                        </div>
                                                        <div class="col-auto">
                                                            <div class="icon icon-shape shadow-soft rounded-circle">
                                                                <span class="fas fa-thumbs-down fa-2x text-danger"></span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </a>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Content Row -->
                <div class="row">
                    <div class="col-12">
                        <div class="card bg-primary border-light shadow-soft mb-4">
                            <div class="card-header py-3 bg-primary">
                                <h6 class="m-0 font-weight-bold"><i class="fas fa-calendar-day"></i>Month Count</h6>
                            </div>
                            <div class="card-body py-2">
                                <div class="row">
                                    <!-- Earnings (Monthly) Card Example -->
                                    <div class="col-12 col-md-6 col-sm-6 col-lg-6 col-xl-4 mb-2 count-view">
                                        <a href="analytics.aspx">
                                            <div class="card bg-primary shadow-inset border-light h-100 py-2">
                                                <div class="card-body">
                                                    <div class="row no-gutters align-items-center">
                                                        <div class="col mr-2">
                                                            <div class="text-xs font-weight-bold text-warning text-uppercase mb-1">Links Created</div>
                                                            <div class="h5 mb-0 font-weight-bold text-gray-800"><asp:Label ID="lblMonthLinkCreated" runat="server" Text=""></asp:Label></div>
                                                        </div>
                                                        <div class="col-auto">
                                                            <div class="icon icon-shape shadow-soft rounded-circle">
                                                                <span class="fas fa-link fa-2x text-warning"></span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </a>
                                    </div>

                                    <!-- Earnings (Monthly) Card Example -->
                                    <div class="col-12 col-md-6 col-sm-6 col-lg-6 col-xl-4 mb-2 count-view">
                                        <a href="analytics.aspx">
                                            <div class="card bg-primary shadow-inset border-light h-100 py-2">
                                                <div class="card-body">
                                                    <div class="row no-gutters align-items-center">
                                                        <div class="col mr-2">
                                                            <div class="text-xs font-weight-bold text-success text-uppercase mb-1">Clicks</div>
                                                            <div class="row no-gutters align-items-center">
                                                                <div class="col-auto">
                                                                    <div class="h5 mb-0 mr-3 font-weight-bold text-gray-900"><asp:Label ID="lblMonthClick" runat="server" Text=""></asp:Label></div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-auto">
                                                            <div class="icon icon-shape shadow-soft rounded-circle">
                                                                <span class="fas fa-mouse-pointer fa-2x text-success"></span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </a>
                                    </div>
                                    <!-- Earnings (Monthly) Card Example -->
                                    <div class="col-12 col-md-6 col-sm-6 col-lg-6 col-xl-4 mb-2 count-view">
                                        <a href="click-reports.aspx">
                                            <div class="card bg-primary shadow-inset border-light h-100 py-2">
                                                <div class="card-body">
                                                    <div class="row no-gutters align-items-center">
                                                        <div class="col mr-2">
                                                            <div class="text-xs font-weight-bold text-dark text-uppercase mb-1">SMS Clicks</div>
                                                            <div class="h5 mb-0 font-weight-bold text-gray"><asp:Label ID="lblMonthSmsClick" runat="server" Text="0"></asp:Label></div>
                                                        </div>
                                                        <div class="col-auto">
                                                            <div class="icon icon-shape shadow-soft rounded-circle">
                                                                <span class="fas fa-hand-pointer fa-2x text-dark"></span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </a>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-12 col-xl-6 col-lg-6 col-md-12 col-sm-6 mb-4">
                        <div class="card bg-primary shadow-soft border-light">
                            <div class="row no-gutters align-items-center">
                                <div class="col-sm-5">
                                    <!-- Header -->
                                    <div class="card-header text-center pb-0">
                                        <h3 class="mb-0">Current Month</h3>
                                        <span class="d-block my-3">
                                            <span class="display-3 font-weight-bold spent-money"><span class="align-baseline font-medium"></span><i class="fas fa-rupee-sign"></i>
                                                <asp:Label ID="lblCurMonthRs" runat="server" Text="0"></asp:Label>
                        </span>
                                        </span>
                                        <!-- <button type="button" class="btn btn-sm btn-primary btn-block">Add to Cart</button> -->
                                    </div>
                                </div>
                                <div class="col-sm-7">
                                    <!-- Content -->
                                    <div class="card-body pb-0">
                                        <ul class="list-group list-group-flush price-list mb-4">
                                            <li class="list-group-item border-primary pb-1"><span class="fas fa-user-plus"></span><a href="account-list.aspx">A/C Created : <asp:Label ID="lblAccountCreated" runat="server" Text="0"></asp:Label></a></li>
                                            <li class="list-group-item border-primary pb-1"><span class="fas fa-wallet"></span><a href="balance-management.aspx">Credit Alloted : <asp:Label ID="lblCreditAllotted" runat="server" Text="0"></asp:Label></a></li>
                                            <li class="list-group-item border-primary pb-1"><span class="fas fa-check-circle"></span><a href="account-list.aspx">Active User : <asp:Label ID="lblActiveUsers" runat="server" Text="0"></asp:Label></a></li>
                                            <li class="list-group-item border-primary pb-1"><span class="fas fa-times-circle"></span><a href="account-list.aspx">Inactive User: <asp:Label ID="lblInactiveUsers" runat="server" Text="0"></asp:Label></a></li>
                                        </ul>
                                    </div>
                                    <!-- End Content -->
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-12 col-xl-6 col-lg-6 col-md-12 col-sm-6  mb-4">
                        <div class="card bg-primary shadow-soft border-light">
                            <div class="row no-gutters align-items-center">
                                <div class="col-sm-5">
                                    <!-- Header -->
                                    <div class="card-header text-center pb-0">
                                        <h3 class="mb-0">Last Month</h3>
                                        <span class="d-block my-3">
                                            <span class="display-3 font-weight-bold spent-money"><span class="align-baseline font-medium"></span><i class="fas fa-rupee-sign"></i><asp:Label ID="lblLastMonthRs" runat="server" Text="0"></asp:Label>
                        </span>
                                        </span>
                                        <!-- <button type="button" class="btn btn-sm btn-primary btn-block">Add to Cart</button> -->
                                    </div>
                                </div>
                                <div class="col-sm-7">
                                    <!-- Content -->
                                    <div class="card-body pb-0">
                                        <ul class="list-group list-group-flush price-list mb-4">
                                            <li class="list-group-item border-primary pb-1"><span class="fas fa-user-plus"></span><a href="account-list.aspx">A/C Created : <asp:Label ID="lblLastMonthAccountCreated" runat="server" Text="0"></asp:Label></a></li>
                                            <li class="list-group-item border-primary pb-1"><span class="fas fa-wallet"></span><a href="balance-management.aspx">Credit Alloted : <asp:Label ID="lblLastMonthCreditAlloted" runat="server" Text="0"></asp:Label></a></li>
                                            <li class="list-group-item border-primary pb-1"><span class="fas fa-check-circle"></span><a href="account-list.aspx">Active User : <asp:Label ID="lblLastMonthActiveUsers" runat="server" Text="0"></asp:Label></a></li>
                                            <li class="list-group-item border-primary pb-1"><span class="fas fa-times-circle"></span><a href="account-list.aspx">Inactive User: <asp:Label ID="lblLastMonthInactiveUsers" runat="server" Text="0"></asp:Label></a></li>
                                        </ul>
                                    </div>
                                    <!-- End Content -->
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Content Row -->

                </div>
                

            </ContentTemplate>
        </asp:UpdatePanel>
    </main>


</asp:Content>
