 <%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="index_u2.aspx.cs" Inherits="eMIMPanel.index_u2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        canvas#myPieChart,
        canvas#myPieChart1 {
            height: 230px;
        }

        .modal.modalPopup {
            top: 0 !important;
            left: 0 !important;
            display: block;
        }

        .modalBackground {
            background-color: #000;
            opacity: 0.5;
        }

        .modalPopup1 {
            min-height: 75px;
            position: fixed;
            z-index: 2000;
            padding: 0;
            background-color: #fff;
            border-radius: 6px;
            background-clip: padding-box;
            border: 1px solid rgba(0, 0, 0, 0.2);
            min-width: 800px;
            box-shadow: 0 5px 10px rgba(0, 0, 0, 0);
        }

        .modalBackground {
            position: fixed;
            top: 0;
            left: 0;
            background-color: #000;
            opacity: 0.5;
            z-index: 1800;
            min-height: 100%;
            width: 100%;
            overflow: hidden;
            filter: alpha(opacity=50);
            display: inline-block;
            z-index: 1000;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="scrmg" runat="server" EnablePageMethods="true">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="updFormArea" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <main>
                <div class="container-fluid">
                    <div class="card card-body py-2 mb-4 bg-primary border-light shadow-inset">
                        <div class="d-flex justify-content-between align-items-center text-center">
                            <div class="text-danger">
                                <strong>
                                    <asp:Label ID="lblNotice" runat="server"></asp:Label></strong>
                            </div>
                            <div class="d-flex align-items-center">
                                <asp:LinkButton ID="btnRefresh" runat="server" class="btn btn-sm btn-primary text-secondary small mr-lg-3 fw-bold" OnClick="btnSubmit_Click"><i class="fas fa-sync"></i></asp:LinkButton>
                                <p class="font-weight-bold m-0" style="font-size: 14px;">
                                    Last updated on -
                                    <asp:Label ID="lblLastUpd" runat="server" Text="21-Dec-2020 10:00" class="ml-2"></asp:Label>
                                </p>
                            </div>
                        </div>
                    </div>

                    

                    <!-- Start Conternt  -->
                    <div class="row">
                        <!--Start Half -->
                        <div class="col-lg-6 order-2 order-lg-1">
                            <div class="card bg-primary border-light shadow-soft mb-4">
                                <div class="card-header py-3 bg-primary">
                                    <h6 class="m-0 font-weight-bold"><i class="fas fa-calendar-day"></i>Month Count</h6>
                                </div>
                                <div class="card-body py-2">
                                    <div class="row">

                                        <!-- Pie Chart -->
                                        <div class="col-xl-12 col-lg-12 mb-4">
                                            <div class="card bg-primary shadow-inset border-light h-100 py-2">
                                                <!-- Card Body -->
                                                <div class="card-body p-3">
                                                    <!-- <div class="chart-pie">
                                                                <canvas id="myPieChart1"></canvas>
                                                            </div> -->
                                                    <div class="shadow-soft h-100 py-3">
                                                        <div id="chart"></div>
                                                    </div>
                                                    <!--  -->
                                                </div>
                                            </div>
                                        </div>
                                        <!--  -->

                                        <!-- Earnings (Monthly) Card Example -->
                                        <div class="col-12 mb-4">
                                            <a href="analytics_u.aspx">
                                                <div class="card bg-primary shadow-inset border-light h-100 py-2">
                                                    <div class="card-body">
                                                        <div class="row no-gutters align-items-center">
                                                            <div class="col mr-2">
                                                                <div class="text-xs font-weight-bold text-warning text-uppercase mb-1">
                                                                    Links Created
                                                                </div>
                                                                <div class="h5 mb-0 font-weight-bold text-gray-800">
                                                                    <asp:Label ID="lblMonthLinkCreated" runat="server" Text=""></asp:Label>
                                                                </div>
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
                                        <div class="col-12 mb-4 ">
                                            <a href="analytics_u.aspx">
                                                <div class="card bg-primary shadow-inset border-light h-100 py-2">
                                                    <div class="card-body">
                                                        <div class="row no-gutters align-items-center">
                                                            <div class="col mr-2">
                                                                <div class="text-xs font-weight-bold text-success text-uppercase mb-1">Clicks</div>
                                                                <div class="row no-gutters align-items-center">
                                                                    <div class="col-auto">
                                                                        <div class="h5 mb-0 mr-3 font-weight-bold text-gray-900">
                                                                            <asp:Label ID="lblMonthClick" runat="server" Text=""></asp:Label>
                                                                        </div>
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
                                        <div class="col-12 mb-4 ">
                                            <a href="click-reports_u.aspx">
                                                <div class="card bg-primary shadow-inset border-light h-100 py-2">
                                                    <div class="card-body">
                                                        <div class="row no-gutters align-items-center">
                                                            <div class="col mr-2">
                                                                <div class="text-xs font-weight-bold text-dark text-uppercase mb-1">SMS Clicks</div>
                                                                <div class="h5 mb-0 font-weight-bold text-gray">
                                                                    <asp:Label ID="lblMonthSmsClick" runat="server" Text="0"></asp:Label>
                                                                </div>
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
                        <!--End  Half -->

                        <!--Todays Count -->
                        <div class="col-lg-6 order-1 order-lg-2">
                            <div class="card bg-primary border-light shadow-soft mb-4">
                                <div class="card-header py-3 bg-primary">
                                    <h6 class="m-0 font-weight-bold"><i class="fas fa-calendar-plus"></i>Todays Count</h6>
                                </div>
                                <div class="card-body py-2">
                                    <div class="row">

                                        <!-- Pie Chart -->
                                        <div class="col-xl-12 col-lg-12 mb-4">
                                            <div class="card bg-primary shadow-inset border-light h-100 py-2">
                                                <!-- Card Body -->
                                                <div class="card-body p-3">
                                                    <!-- <div class="chart-pie">
                                <canvas id="myPieChart"></canvas>
                            </div> -->
                                                    <div class="shadow-soft h-100 py-3">
                                                        <div id="chart2"></div>
                                                    </div>
                                                    <!--  -->
                                                </div>
                                            </div>
                                        </div>
                                        <!--  -->

                                        <!-- Earnings (Monthly) Card Example -->
                                        <div class="col-12 mb-4 ">
                                            <a href="sms-reports_usr_new.aspx">
                                                <div class="card bg-primary shadow-inset border-light h-100 py-2">
                                                    <div class="card-body">
                                                        <div class="row no-gutters align-items-center">
                                                            <div class="col mr-2">
                                                                <div class="text-xs font-weight-bold text-warning text-uppercase mb-1">Submitted</div>
                                                                <div class="h5 mb-0 font-weight-bold text-gray-800">
                                                                    <asp:Label ID="lblTodaySubmitted" runat="server" Text=""></asp:Label>
                                                                </div>
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
                                        <div class="col-12 mb-4 ">
                                            <a href="sms-reports.html"></a>
                                            <div class="card bg-primary shadow-inset border-light h-100 py-2">
                                                <a href="sms-reports_usr_new.aspx">
                                                    <div class="card-body">
                                                        <div class="row no-gutters align-items-center">
                                                            <div class="col mr-2">
                                                                <div class="text-xs font-weight-bold text-success text-uppercase mb-1">Delivered</div>
                                                                <div class="row no-gutters align-items-center">
                                                                    <div class="col-auto">
                                                                        <div class="h5 mb-0 mr-3 font-weight-bold text-gray-900">
                                                                            <asp:Label ID="lblTodayDelivered" runat="server" Text=""></asp:Label>
                                                                        </div>
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
                                        <div class="col-12 mb-4 ">
                                            <a href="sms-reports_usr_new.aspx">
                                                <div class="card bg-primary shadow-inset border-light h-100 py-2">
                                                    <div class="card-body">
                                                        <div class="row no-gutters align-items-center">
                                                            <div class="col mr-2">
                                                                <div class="text-xs font-weight-bold text-danger text-uppercase mb-1">Failed</div>
                                                                <div class="h5 mb-0 font-weight-bold text-danger-800">
                                                                    <asp:Label ID="lblTodayFailed" runat="server" Text=""></asp:Label>
                                                                </div>
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
                            <!--End Half -->

                        </div>
                        <!-- End Todays Count  -->
                    </div>


                    <!-- Content Row -->
                    <%-- <div class="row">
                        <div class="col-12">
                            <div class="card bg-primary border-light shadow-soft mb-4">
                                <div class="card-header py-3 bg-primary">
                                    <div >
                                        <h6 class="m-0 font-weight-bold"><i class="fas fa-calendar-plus"></i>Todays Count  
                                    </h6>
                                    </div>
                                </div>
                                <div class="card-body py-2">
                                    <div class="row">
                                        <!-- Earnings (Monthly) Card Example -->
                                        <div class="col-12 col-md-6 col-sm-6 col-lg-6 col-xl-4 mb-2 count-view">
                                            <a href="sms-reports_usr_new.aspx">
                                                <div class="card bg-primary shadow-inset border-light h-100 py-2">
                                                    <div class="card-body">
                                                        <div class="row no-gutters align-items-center">
                                                            <div class="col mr-2">
                                                                <div class="text-xs font-weight-bold text-warning text-uppercase mb-1">Submitted</div>
                                                                <div class="h5 mb-0 font-weight-bold text-gray-800">
                                                                    </div>
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
                                            <a href="sms-reports_usr_new.aspx">
                                                <div class="card bg-primary shadow-inset border-light h-100 py-2">
                                                    <div class="card-body">
                                                        <div class="row no-gutters align-items-center">
                                                            <div class="col mr-2">
                                                                <div class="text-xs font-weight-bold text-success text-uppercase mb-1">Delivered</div>
                                                                <div class="row no-gutters align-items-center">
                                                                    <div class="col-auto">
                                                                        <div class="h5 mb-0 mr-3 font-weight-bold text-gray-900">
                                                                            </div>
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
                                            <a href="sms-reports_usr_new.aspx">
                                                <div class="card bg-primary shadow-inset border-light h-100 py-2">
                                                    <div class="card-body">
                                                        <div class="row no-gutters align-items-center">
                                                            <div class="col mr-2">
                                                                <div class="text-xs font-weight-bold text-danger text-uppercase mb-1">Failed</div>
                                                                <div class="h5 mb-0 font-weight-bold text-danger-800">
                                                                    </div>
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
                    </div>--%>

                    <!-- Content Row -->
                    <%--<div class="row">
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
                                                                <div class="h5 mb-0 font-weight-bold text-gray-800">
                                                                    </div>
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
                                                                        <div class="h5 mb-0 mr-3 font-weight-bold text-gray-900">
                                                                            </div>
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
                                                                <div class="h5 mb-0 font-weight-bold text-gray">
                                                                    </div>
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
                    </div>--%>
                    <!-- Content Row -->

                    <%-- Expire url in 5 days--%>
                    <!--Start Full -->
                    <div class="col-lg-12URL order-2 order-lg-1">
                        <div class="card bg-primary border-light shadow-soft mb-4">
                            <div class="card-header py-3 bg-primary">
                                <h6 class="m-0 font-weight-bold"><i class="fas fa-calendar-day"></i>URL Expires in 5 days</h6>
                            </div>
                            <div class="card-body py-1">
                                <div class="row">
                                    <!-- Earnings (Monthly) Card Example -->
                                    <div class="col-12 mb-4 ">
                                        <div class="card bg-primary shadow-inset border-light h-100 py-2">
                                            <div class="card-body">
                                                <div class="row no-gutters align-items-center">
                                                    <div class="col mr-2">
                                                        <div class="h5 mb-0 text-gray">
                                                            <div class="table-responsive">
                                                                <asp:GridView ID="grv2" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
                                                                    runat="server" CellPadding="10" BorderColor="#ede8e8" Class="table table-striped table-bordered dt-responsive nowrap dataTable-view">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderStyle-Width="5%" HeaderText="#">
                                                                            <ItemTemplate>
                                                                                <%#Container.DataItemIndex+1 %>
                                                                                <asp:HiddenField ID="hdnId" runat="server" Value='<%# Eval("ID") %>' />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Long URL">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbl1" runat="server" Text='<%#Eval("long_url")%>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Short URL">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbl2" runat="server" Text='<%#Eval("shortURL")%>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Created On">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbl3" runat="server" Text='<%#Eval("createdDate")%>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Expiry Date">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbl4" runat="server" Text='<%#Eval("expiry")%>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Action" HeaderStyle-Width="10%">
                                                                            <ItemTemplate>
                                                                                <%--<asp:Label ID="lbl4" runat="server" Text='<%#Eval("Expiry")%>'></asp:Label>--%>
                                                                                <asp:LinkButton ID="btnRenew" runat="server" class="btn btn-sm btn-primary text-secondary small mr-lg-3 fw-bold" OnClick="btnRenew_Click" OnClientClick="return ConfirmBal();"><i class="fa fa-expand"></i>Renew</asp:LinkButton>
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
                                </div>
                            </div>
                        </div>
                    </div>
                    <!--End  Full-->


                    <div class="col-lg-12URL order-2 order-lg-1">
                        <div class="card bg-primary border-light shadow-soft mb-4">
                            <div class="card-header py-3 bg-primary">
                                <h6 class="m-0 font-weight-bold" id="HeadingCheckerMaker" runat="server">
                                    <%--Maker Campaign Lists--%>
                                </h6>
                            </div>
                            <div class="card-body py-1">
                                <div class="row">
                                    <!-- Checker Card  -->
                                    <div class="col-12 mb-4 ">
                                        <div class="card bg-primary shadow-inset border-light h-100 py-2">
                                            <div class="card-body">
                                                <div class="row no-gutters align-items-center">
                                                    <div class="col mr-2">
                                                        <div class="h6 mb-0 text-gray">
                                                            <div class="table-responsive">
                                                                <asp:GridView ID="grdMakerCampaign" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
                                                                    runat="server" CellPadding="10" BorderColor="#ede8e8" Class="table table-striped table-bordered dt-responsive nowrap dataTable-view">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderStyle-Width="5%" HeaderText="Sl. No.">
                                                                            <ItemTemplate>
                                                                                <%#Container.DataItemIndex+1 %>
                                                                                <asp:HiddenField ID="hdnMId" runat="server" Value='<%# Eval("ID") %>' />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Campaign Name">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbl1" runat="server" Text='<%#Eval("campname")%>'></asp:Label>

                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Creation Date">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbl2" runat="server" Text='<%#Eval("InsertTime")%>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="File Name">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbl3" runat="server" Text='<%#Eval("fileName")%>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Mobile Count">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbl4" runat="server" Text='<%#Eval("noofrecord")%>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="SenderID">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbl5" runat="server" Text='<%#Eval("sender")%>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="TemplateID">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbl6" runat="server" Text='<%#Eval("templateid")%>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Template Text">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbl7" runat="server" Text='<%#Eval("msg")%>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Test SMS">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="lnkTest" runat="server" class="btn btn-sm btn-primary text-secondary small mr-lg-3 fw-bold" title="Send SMS"
                                                                                    OnClick="lnkTest_Click"><i class="fas fa-paper-plane"></i></asp:LinkButton>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Approval Status">
                                                                            <ItemTemplate>
                                                                                <%--<asp:Label ID="lbl8" runat="server" Text='<%#Eval("Status")%>'></asp:Label>--%>
                                                                                <asp:Label ID="Label1" runat="server" Text='<%# string.IsNullOrEmpty(Eval("Status").ToString()) ? "Pending" : Eval("Status").ToString() %>'></asp:Label>

                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Execute" HeaderStyle-Width="10%">
                                                                            <ItemTemplate>
                                                                                <%--<asp:LinkButton ID="lnkbtnExecute" runat="server" class="btn btn-sm btn-primary text-secondary small mr-lg-3 fw-bold" OnClick="lnkbtnExecute_Click"><i class="fa fa-expand"></i></asp:LinkButton>--%>
                                                                                <asp:LinkButton ID="lnkbtnExecute" Visible='<%# Eval("Status").ToString().ToLower() == "approved" %>' runat="server" class="btn btn-sm btn-primary text-secondary small mr-lg-3 fw-bold" OnClick="lnkbtnExecute_Click"><i class="fas fa-arrow-alt-circle-right"></i></asp:LinkButton>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Approve" HeaderStyle-Width="10%">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="lnkbtnApprovePopup" runat="server" class="btn btn-sm btn-primary text-secondary small mr-lg-3 fw-bold" OnClick="lnkbtnApprovePopup_Click"><i class="fas fa-check-square" style="color: #30ca12;"></i></asp:LinkButton>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Reject" HeaderStyle-Width="10%">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="lnkbtnRejectPopup" runat="server" class="btn btn-sm btn-primary text-secondary small mr-lg-3 fw-bold" OnClick="lnkbtnRejectPopup_Click">
                                                                                    <i class="fas fa-times-circle" style="color: #fd2c08;"></i></asp:LinkButton>
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
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </main>

            <%--  Schedule POPUP --%>
            <asp:Panel ID="pnlPopUp_SCHEDULE" runat="server" CssClass="modalPopup1" Style="display: none;">
                <div style="overflow-y: auto; overflow-x: hidden; max-height: 80%;">
                    <div class="modal-header">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="custom-control custom-radio custom-control-inline pl-2">
                                    <asp:RadioButton class="mr-2" ID="rdbScheduleSMS" runat="server" Checked="true" GroupName="mobile" AutoPostBack="true" OnCheckedChanged="rdbScheduleSMS_CheckedChanged" />
                                    <label>Schedule SMS</label>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="custom-control custom-radio custom-control-inline pl-2">
                                    <asp:RadioButton class="mr-2" ID="rdbSendInstantly" runat="server" GroupName="mobile" AutoPostBack="true" OnCheckedChanged="rdbScheduleSMS_CheckedChanged" />
                                    <label>Send Instantly</label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-body" style="overflow-y: auto; height: 70vh;">
                        <div>
                            <asp:HiddenField ID="hdnScheduleCount" runat="server" Value="1" ClientIDMode="Static" />
                            <div class="row">
                                <div class="col-md-5">
                                    Schedule Date : 
                            <asp:Label ID="lblScheduleDate" Visible="false" runat="server" ClientIDMode="Static"></asp:Label>
                                    <asp:TextBox ID="txtScheduleDate" runat="server" TextMode="Date" onchange="javascript:text_changed_from();" class="form-control datepicker " placeholder="Scheduled Date"></asp:TextBox>
                                    <asp:HiddenField ID="hdnScheduleDate" runat="server" />
                                </div>
                                <div class="col-md-4">
                                    Time (HH:MM) :
                                            <asp:TextBox ID="txtTime" ToolTip="Enter time in HH:MM format. Entered time will not be deleted. You can overwrite the time." onkeypress="return false" onkeyup="return ValTime();" OnPaste="return false" runat="server" MaxLength="6" Width="35%" Enabled="true"
                                                class="form-control"></asp:TextBox>
                                    <asp:MaskedEditExtender ID="MaskedEditExtendertxtTime" runat="server" TargetControlID="txtTime" Mask="99:99" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" MaskType="Time" AcceptAMPM="false" ErrorTooltipEnabled="True" />
                                </div>
                                <%--<div class="col-md-2">
                                    <button type="button" onclick="SetSchedule1()" class="btn btn-primary btn-icon-split"><span class="text-success"><i class="fas fa-plus"></i></span></button>
                                </div>--%>
                                <div id="divSchedule" runat="server" style="display: none; text-align: center" class="form-group row">
                                    <div>Scheduling SMS. Please wait...</div>
                                    <div>
                                        <img src="img/loading.gif" width="125px" height="100px" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div align="center" class="modal-footer">
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:Button ID="btnScheduleSMS" runat="server" Text="Schedule SMS" class="btn btn-primary" OnClientClick="showLoading1(); return confirm('Are you sure you want to schedule?');" OnClick="btnScheduleSMS_Click" />
                                    <asp:Button ID="lnkbtnSendInstantly" runat="server" Text="Send" class="btn btn-primary" OnClientClick="showLoading1(); return confirm('Are you sure you want to send instantly?');" OnClick="lnkbtnSendInstantly_Click" Visible="false" />
                                    <asp:Button ID="btnCancelSch" runat="server" Text="Cancel" class="btn btn-primary" OnClick="btnCancelSch_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <asp:LinkButton ID="lnkSCH" runat="server"></asp:LinkButton>
            <asp:ModalPopupExtender ID="pnlPopUp_SCHEDULE_ModalPopupExtender" runat="server" BackgroundCssClass="modalBackground" PopupControlID="pnlPopUp_SCHEDULE" TargetControlID="lnkSCH">
            </asp:ModalPopupExtender>
            <%--  Schedule POPUP --%>



            <asp:LinkButton ID="lnkDetail" runat="server"></asp:LinkButton>
            <asp:Panel ID="pnlPopUp_Detail" runat="server" CssClass="modal modalPopup" Style="display: none;">
                <div class="modal-dialog modal-xl modal-dialog-centered modal-dialog-scrollable">
                    <div class="modal-content">
                        <div class="modal-header">
                            <asp:Label ID="lblHeading" runat="server" CssClass="modal-title font-weight-bold" Text="TEST CAMPAIGN"></asp:Label>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-md-2">
                                    <asp:Label ID="lbl1" runat="server" Text="Campaign Name :"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:Label ID="lblCampaignName" runat="server"></asp:Label>
                                </div>

                                <div class="col-md-2">
                                    <asp:Label ID="lbl2" runat="server" Text="SenderID :"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:Label ID="lblSenderID" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row mt-2">
                                <div class="col-md-2">
                                    <asp:Label ID="lbl3" runat="server" Text="File Name :"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:Label ID="lblFileName" runat="server"></asp:Label>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="lbl4" runat="server" Text="TemplateID :"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:Label ID="lblTemplateID" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row mt-2">
                                <div class="col-md-2">
                                    <asp:Label ID="lbl5" runat="server" Text="Mobile Count :"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:Label ID="lblMobileCount" runat="server"></asp:Label>
                                </div>

                                <div class="col-md-2">
                                    <asp:Label ID="lbl6" runat="server" Text="Template Text :"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:Label ID="lblTemplateText" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row mt-2">
                                <div class="col-md-3">
                                    <asp:Label ID="lbl7" runat="server" Text="Enter Mobile Numbers :"></asp:Label>
                                </div>
                                <div class="col-md-5">
                                    <asp:TextBox ID="txtMobNum" runat="server" class="form-control" TextMode="MultiLine" Rows="5"
                                        MaxLength="2147483647" onkeyup="integersOnly(this); mobnumbcnt(); return true;" placeholder="Enter Mobile Numbers"></asp:TextBox>
                                    <p class="my-2">
                                        <span class="font-weight-bold small">SMS to be Sent On:</span>
                                        <asp:Label ID="lblMobileCnt" runat="server" class="small" ClientIDMode="Static" Font-Size="Large"></asp:Label>
                                    </p>
                                </div>
                            </div>
                            <div class="row mt-2">
                                <div class="col-md-3">
                                </div>
                                <div class="col-md-2">
                                    <asp:LinkButton runat="server" ID="lnkbtnTest" class="btn btn-block" OnClick="lnkbtnTest_Click">
                                                   <i class="fas fa-paper-plane"></i> Test
                                    </asp:LinkButton>
                                </div>
                                <div class="col-md-2">
                                    <button id="btnCancel" runat="server" class="btn btn-primary">Close</button>
                                </div>
                            </div>
                        </div>
                        <%--<div class="modal-footer">
                            <button id="btnCancel" runat="server" class="btn btn-primary">Close</button>
                        </div>--%>
                    </div>
                </div>
            </asp:Panel>

            <%--pnlPopUp_Detail Modal Popup Extender For pnlPopUp_Detail--%>
            <cc1:ModalPopupExtender ID="pnlPopUp_Detail_ModalPopupExtender" runat="server" PopupControlID="pnlPopUp_Detail"
                TargetControlID="lnkDetail" BehaviorID="mpeAddUpdateEmployee" CancelControlID="btnCancel"
                BackgroundCssClass="modalBackground">
            </cc1:ModalPopupExtender>

            <asp:Panel ID="pnlPopUp_AppRej" runat="server" CssClass="modalPopup1" Style="display: none;">
                <div style="overflow-y: auto; overflow-x: hidden; max-height: 80%;">
                    <div class="modal-header">
                        <asp:Label ID="lblHeadingReason" runat="server" CssClass="font-weight-bold"></asp:Label>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-md-2">
                                <asp:Label ID="Label" runat="server">Reason : </asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtreason" runat="server" class="form-control" placeholder="Reason"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row mt-3">
                            <div class="col-md-2"></div>
                            <div class="col-md-2">
                                <asp:Button ID="lnkbtnAppRej" runat="server" Text="Submit" class="btn btn-primary" OnClientClick="return confirm('Are you sure you want to update status?');" OnClick="lnkbtnAppRej_Click" />
                            </div>
                            <div class="col-md-2">
                                <asp:Button ID="btnAppRejCancel" runat="server" Text="Cancel" class="btn btn-primary" OnClick="btnCancel_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <asp:LinkButton ID="lnkAppRej" runat="server"></asp:LinkButton>
            <asp:ModalPopupExtender ID="pnlPopUp_AppRej_ModalPopupExtender" runat="server" BackgroundCssClass="modalBackground" PopupControlID="pnlPopUp_AppRej" TargetControlID="lnkAppRej">
            </asp:ModalPopupExtender>

        </ContentTemplate>
    </asp:UpdatePanel>

    <script src="https://cdnjs.cloudflare.com/ajax/libs/c3/0.3.0/c3.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/d3/3.4.12/d3.min.js"></script>
    <script>
        function integersOnly(obj) {
            obj.value = obj.value.replace(/[^0-9,\r\n]/g, '');
        }

        function ValTime() {
            var flag = true;
            var MM = $("<%=txtTime.ClientID%>").val();
            HH = MM.split(':')[0];
            MM = MM.split(':')[1];

            if (HH > 23) {
                alert('Please Enter HH less than 24')
                flag = false;
            }

            if (MM > 59) {
                alert('Please Enter MM less than 60')
                flag = false;
            }
        }

        function text_changed_from() {
            var d = document.getElementById("ContentPlaceHolder1_txtScheduleDate").value
            console.log(d);
            document.getElementById("ContentPlaceHolder1_hdnScheduleDate").value = d;
        }

        //function SetSchedule1() {
        //    document.getElementById('divSchedule1').style.display = "";
        //    document.getElementById('hdnSchedule1').value = 1;
        //    document.getElementById('hdnScheduleCount').value = 2;
        //}

        function mobnumbcnt() {
            var s = document.getElementById("<%=txtMobNum.ClientID%>").value;
            if (s != '') {
                var str = s;
                var x = 0;
                for (var i = 0; i < str.length; i++) {
                    if (str.charAt(i) == ',') x++;
                }
                for (var i = 0; i < str.length; i++) {
                    if (str.charAt(i) == '\n') x++;
                }

                if (x > 0) {
                    document.getElementById('<%= lblMobileCnt.ClientID %>').innerHTML = (x + 1).toString();
                } else
                    document.getElementById('<%= lblMobileCnt.ClientID %>').innerHTML = "8";
                if (s != '' && x == 0)
                    document.getElementById('<%= lblMobileCnt.ClientID %>').innerHTML = "1";
            } else {
                document.getElementById('<%= lblMobileCnt.ClientID %>').innerHTML = "";
            }
        }

        function showLoading1() {
            document.getElementById('<%= divSchedule.ClientID %>').style.display = 'block';
        }

        var chart = c3.generate({
            data: {
                // iris data from R
                columns: [

                    ['Clicks', <%= Session["lblMonthClick"] %>],
                    ['SMSClicks', <%= Session["lblMonthSmsClick"] %>]
                ],
                type: 'pie',
                onclick: function (d, i) {
                    console.log("onclick", d, i);
                },
                onmouseover: function (d, i) {
                    console.log("onmouseover", d, i);
                },
                onmouseout: function (d, i) {
                    console.log("onmouseout", d, i);
                }
            },
            bindto: '#chart'
        });

        setTimeout(function () {
            chart.data.colors({
                LinkCreated: '#DB3A1B',
                Clicks: '#2ca02c',
                SMSClicks: '#777777'
            });
        }, 100);
    </script>

    <script>
        var chart = c3.generate({
            data: {
                // iris data from R
                columns: [

                    ['Delivered', <%= Session["lblTodayDelivered"] %>],
                    ['Failed', <%= Session["lblTodayFailed"] %>]
                ],
                type: 'pie',
                onclick: function (d, i) {
                    console.log("onclick", d, i);
                },
                onmouseover: function (d, i) {
                    console.log("onmouseover", d, i);
                },
                onmouseout: function (d, i) {
                    console.log("onmouseout", d, i);
                }
            },
            bindto: '#chart2'
        });

        setTimeout(function () {
            chart.data.colors({
                Submitted: '#ff7f0e',
                Delivered: '#4CAF50',
                Failed: '#f44336'
            });
        }, 100);
    </script>

</asp:Content>
