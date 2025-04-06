<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="index_u.aspx.cs" Inherits="eMIMPanel.index_u" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:toolkitscriptmanager id="scrmg" runat="server" enablepagemethods="true">
    </asp:toolkitscriptmanager>
    <asp:updatepanel id="updFormArea" runat="server" updatemode="Conditional">
        <ContentTemplate>
            <main>
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
                                    <div>
                                    <h6 class="m-0 font-weight-bold"><i class="fas fa-calendar-plus"></i>Todays Count &nbsp;&nbsp;&nbsp;&nbsp; </h6>  
                                    <%--<asp:LinkButton ID="btnRefresh" runat="server" OnClick="btnSubmit_Click"><i class="fas fa-sync"></i></asp:LinkButton>--%>
                                    </div>
                                </div>
                                <div class="card-body py-2">
                                    <div class="row">
                                        <!-- Earnings (Monthly) Card Example -->
                                        <div class="col-12 col-md-6 col-sm-6 col-lg-6 col-xl-4 mb-2 count-view">
                                            <a href="sms-reports_usr.aspx">
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
                                            <a href="sms-reports_usr.aspx">
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
                                                </div>
                                            </a>
                                        </div>
                                        <!-- Earnings (Monthly) Card Example -->

                                        <!-- Earnings (Monthly) Card Example -->
                                        <div class="col-12 col-md-6 col-sm-6 col-lg-6 col-xl-4 mb-4 count-view">
                                            <a href="sms-reports_usr.aspx">
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
                                            <a href="analytics_u.aspx">
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
                                            <a href="analytics_u.aspx">
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
                                            <a href="click-reports_u.aspx">
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
                    <!-- Content Row -->
                </div>
            </main>
        </ContentTemplate>
    </asp:updatepanel>
</asp:Content>
