<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="BDwiseindex.aspx.cs" Inherits="eMIMPanel.BDwiseindex" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      <style>
        canvas#myPieChart,
        canvas#myPieChart1 {
            height: 230px;
        }

        .count_text {
            color: Black;
            float: right;
            text-align: center;
            padding-right: 40px;
        }

        body {
            font-family: Arial;
            font-size: 10pt;
        }

        .Grid td {
            background-color: #A1DCF2;
            color: black;
            font-size: 10pt;
            line-height: 200%
        }

        .Grid th {
            background-color: #3AC0F2;
            color: White;
            font-size: 10pt;
            line-height: 200%
        }

        .ChildGrid td {
            background-color: #eee !important;
            color: black;
            font-size: 10pt;
            line-height: 200%
        }

        .ChildGrid th {
            background-color: #6C6C6C !important;
            color: White;
            font-size: 10pt;
            line-height: 200%
        }
    </style>

    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript">
        $("[src*=plus]").live("click", function () {
            $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
            $(this).attr("src", "img/minus.png");
        });
        $("[src*=minus]").live("click", function () {
            $(this).attr("src", "img/plus.png");
            $(this).closest("tr").next().remove();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <cc1:ToolkitScriptManager ID="scrmg" runat="server" EnablePageMethods="true">
    </cc1:ToolkitScriptManager>
    <main>
        <asp:UpdatePanel ID="updFormArea" runat="server" UpdateMode="Conditional">
            <ContentTemplate>

                <div class="container-fluid">

                    <div class="card card-body py-2 mb-4 bg-primary border-light shadow-inset">
                        <div class="d-flex justify-content-between justify-content-lg-end align-items-center text-center">
                            <asp:LinkButton ID="btnRefresh" runat="server" class="btn btn-sm btn-primary text-secondary small mr-lg-3 fw-bold" OnClick="btnSubmit_Click"><i class="fas fa-sync"></i></asp:LinkButton>
                            <p class="font-weight-bold m-0" style="font-size: 14px;">
                                Last updated on -
                                <asp:Label ID="lblLastUpd" runat="server" Text="21-Dec-2020 10:00" class="ml-2"></asp:Label>
                            </p>
                        </div>
                    </div>

                    <!-- Start Conternt  -->
                    <div class="row">
                        <!--Start Half -->
                        <div class="col-lg-6 order-2 order-lg-1">
                            <div class="card bg-primary border-light shadow-soft mb-4">
                                <div class="card-header py-3 bg-primary">
                                    <h6 class="m-0 font-weight-bold"><i class="fas fa-calendar-day"></i> Month Count</h6>
                                </div>
                                <div class="card-body py-2">
                                    <div class="row">

                                        <!-- Pie Chart -->
                                        <div class="col-xl-12 col-lg-12 mb-4">
                                            <div class="card bg-primary shadow-inset border-light h-100 py-2">
                                                <!-- Card Body -->
                                                <div class="card-body p-3">
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
                                            <a href="DLRDayData.aspx">
                                                <div class="card bg-primary shadow-inset border-light h-100 py-2">
                                                    <div class="card-body">
                                                        <div class="row no-gutters align-items-center">
                                                            <div class="col mr-2">
                                                                <div class="text-xs font-weight-bold text-warning text-uppercase mb-1">
                                                                 <%--   Links Created
                                                                     <asp:Label ID="lblMonthLinkCreated" class="count_text" runat="server" Text=""></asp:Label>--%>

                                                                    Submitted
                                                                     <asp:Label ID="lblMonthSubmitted" class="count_text" runat="server" Text=""></asp:Label>

                                                                </div>

                                                            </div>
                                                            <div class="col-auto">
                                                                <div class="icon icon-shape shadow-soft rounded-circle">
                                                                    <span class="fas fa-check fa-2x text-warning"></span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </a>
                                        </div>

                                        <!-- Earnings (Monthly) Card Example -->
                                        <div class="col-12 mb-4 ">
                                            <a href="DLRDayData.aspx">
                                                <div class="card bg-primary shadow-inset border-light h-100 py-2">
                                                    <div class="card-body">
                                                        <div class="row no-gutters align-items-center">
                                                            <div class="col mr-2">
                                                                <div class="text-xs font-weight-bold text-success text-uppercase mb-1">
                                                                 <%--   Clicks
                                                                      <asp:Label ID="lblMonthClick" class="count_text" runat="server" Text=""></asp:Label>--%>
                                                                     Delivered
                                                                      <asp:Label ID="lblMonthdelivered" class="count_text" runat="server" Text=""></asp:Label>
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
                                        <div class="col-12 mb-4 ">
                                            <a href="DLRDayData.aspx">
                                                <div class="card bg-primary shadow-inset border-light h-100 py-2">
                                                    <div class="card-body">
                                                        <div class="row no-gutters align-items-center">
                                                            <div class="col mr-2">
                                                                <div class="text-xs font-weight-bold text-dark text-uppercase mb-1">
                                                                   <%-- SMS Clicks
                                                                     <asp:Label ID="lblMonthSmsClick" class="count_text" runat="server" Text="0"></asp:Label>--%>
                                                                    Failed
                                                                     <asp:Label ID="lblMonthFailed" class="count_text" runat="server" Text="0"></asp:Label>
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
                        <!--End  Half -->

                        <!--Start Half -->
                        <div class="col-lg-6 order-1 order-lg-2">
                            <div class="card bg-primary border-light shadow-soft mb-4">
                                <div class="card-header py-3 bg-primary">
                                    <h6 class="m-0 font-weight-bold"><i class="fas fa-calendar-plus"></i> Todays Count</h6>
                                </div>
                                <div class="card-body py-2">
                                    <div class="row">

                                        <!-- Pie Chart -->
                                        <div class="col-xl-12 col-lg-12 mb-4">
                                            <div class="card bg-primary shadow-inset border-light h-100 py-2">
                                                <!-- Card Body -->
                                                <div class="card-body p-3">
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
                                            <a href="sms-reports.aspx">
                                                <div class="card bg-primary shadow-inset border-light h-100 py-2">
                                                    <div class="card-body">
                                                        <div class="row no-gutters align-items-center">
                                                            <div class="col mr-2">
                                                                <div class="text-xs font-weight-bold text-warning text-uppercase mb-1">
                                                                    Submitted
                                                                      <asp:Label ID="lblTodaySubmitted" class="count_text" runat="server" Text=""></asp:Label>
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
                                            <a href="sms-reports.aspx"></a>
                                            <div class="card bg-primary shadow-inset border-light h-100 py-2">
                                                <a href="sms-reports.aspx">
                                                    <div class="card-body">
                                                        <div class="row no-gutters align-items-center">
                                                            <div class="col mr-2">
                                                                <div class="text-xs font-weight-bold text-success text-uppercase mb-1">
                                                                    Delivered
                                                                     <asp:Label ID="lblTodayDelivered" class="count_text" runat="server" Text=""></asp:Label>
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
                                            <a href="sms-reports.aspx">
                                                <div class="card bg-primary shadow-inset border-light h-100 py-2">
                                                    <div class="card-body">
                                                        <div class="row no-gutters align-items-center">
                                                            <div class="col mr-2">
                                                                <div class="text-xs font-weight-bold text-danger text-uppercase mb-1">
                                                                    Failed
                                                                      <asp:Label ID="lblTodayFailed" class="count_text" runat="server" Text=""></asp:Label>
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
                        <!--End Half -->
                    </div>
                    <!-- End Conternt  -->

                    <!-- Content Row -->
                    <%--<div class="row">
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
                                </div>--%>

                    <!-- Content Row -->
                    <%-- <div class="row">
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
                                    </div>--%>


                    <%-- Top 5 --%>


                    <div class="card bg-primary shadow-soft border-light">
                        
                            <asp:UpdatePanel ID="updatepnl" runat="server">
                                <ContentTemplate>

                                    <div class="container">
									<div class="row">
                                        <div class="col-12 col-xl-12 col-lg-12 col-md-12 col-sm-12 mb-12">
                                            <div class="form-row pt-3 pb-3">
                                                <div class="col-md-4 col-4">
                                                    <h4>Today's Top Performer</h4>
                                                </div>
                                                <div class="col-md-1 col-3">
                                                    <asp:DropDownList ID="ddlRecords" runat="server" class="form-control">
                                                        <asp:ListItem Text="5" Value="5" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                        <asp:ListItem Text="25" Value="25"></asp:ListItem>
                                                        <asp:ListItem Text="50" Value="50"></asp:ListItem>
                                                        <asp:ListItem Text="100" Value="100"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-md-7 col-5">
                                                    <asp:LinkButton runat="server" ID="lnkShow" OnClick="lnkShow_Click" class="btn btn-mini">
                                Show <i class="fas fa-eye" aria-hidden="true"></i>
                                                    </asp:LinkButton>
                                                </div>

                                                <div class="col-sm-12 mt-2">




                                                    <div class="table-responsive">
                                                        <asp:GridView ID="grv" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
                                                            runat="server" Width="100%" CellPadding="10" BorderColor="#ede8e8" Class="table table-striped table-bordered dt-responsive wrap dataTable-view1" DataKeyNames="groupname" OnRowDataBound="grv_RowDataBound">
                                                            <Columns>






                                                                <asp:TemplateField HeaderStyle-Width="5%" HeaderText="Sr No">
                                                                    <ItemTemplate>
                                                                        <%#Container.DataItemIndex+1 %>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                
                                                                <asp:TemplateField HeaderText="User ID">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblUserId" runat="server" Text='<%#Eval("userid")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                 <asp:TemplateField HeaderText="Name">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCompa" runat="server" Text='<%#Eval("COMPNAME")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Group Name" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblgrpname" runat="server" Text='<%#Eval("groupname")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Submitted">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbl1" runat="server" Text='<%#Eval("Submitted")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Submitted (%)">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblPer" runat="server" Text='<%#Eval("SubmittedPer")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Delivered">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbl2" runat="server" Text='<%#Eval("Delivered")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Failed">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbl3" runat="server" Text='<%#Eval("Failed")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Unknown">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblurlid" runat="server" Text='<%#Eval("Unknown")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>

                                                        </asp:GridView>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>




                     <div class="card bg-primary shadow-soft border-light" id="divDBTraffic" runat="server">
                        
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>

                                    <div class="container">
									<div class="row">
                                        <div class="col-12 col-xl-12 col-lg-12 col-md-12 col-sm-12 mb-12">
                                            <div class="form-row pt-3 pb-3">
                                                <div class="col-md-4 col-4">
                                                    <h4>BD Wise Traffic</h4>
                                                </div>
                                                <div class="col-md-1 col-3">
                                                    <asp:DropDownList ID="ddlBDRecords" runat="server" class="form-control">
                                                        <asp:ListItem Text="5" Value="5" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                        <asp:ListItem Text="25" Value="25"></asp:ListItem>
                                                        <asp:ListItem Text="50" Value="50"></asp:ListItem>
                                                        <asp:ListItem Text="100" Value="100"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-md-7 col-5">
                                                    <asp:LinkButton runat="server" ID="lnkBDShow" OnClick="lnkBDShow_Click" class="btn btn-mini">
                                Show <i class="fas fa-eye" aria-hidden="true"></i>
                                                    </asp:LinkButton>
                                                </div>

                                                <div class="col-sm-12 mt-2">




                                   <div class="table-responsive">
                                       <asp:GridView ID="GvBDWise" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
                                           runat="server" OnRowDataBound="GvBDWise_RowDataBound" Width="100%" CellPadding="10" BorderColor="#ede8e8" Class="table table-striped table-bordered dt-responsive wrap dataTable-view1">
                                           <Columns> 
                                               
                                               <asp:TemplateField>
                                                   <ItemTemplate>
                                                       <img alt="" style="cursor: pointer" src="img/plus.png" />
                                                       <asp:Panel ID="pnlOrders" runat="server" Style="display: none">
                                                           <asp:GridView ID="Grouptedgv" runat="server" AutoGenerateColumns="false" CssClass="ChildGrid" OnRowDataBound="Grouptedgv_RowDataBound">
                                                               <Columns>

                                                                    
                                               <asp:TemplateField>
                                                   <ItemTemplate>
                                                       <img alt="" style="cursor: pointer;max-width:fit-content;" src="img/plus.png" />
                                                       <asp:Panel ID="pnladminOrders" runat="server" Style="display: none">
                                                           <asp:GridView ID="Adminnestedgv" OnRowDataBound="Adminnestedgv_RowDataBound" runat="server" AutoGenerateColumns="false" CssClass="ChildGrid">
                                                               <Columns>

                                                                    <asp:TemplateField>
                                                   <ItemTemplate>
                                                       <img alt="" style="cursor: pointer;max-width:fit-content;" src="img/plus.png" />
                                                       <asp:Panel ID="pnluserOrders" runat="server" Style="display: none">
                                                           <asp:GridView ID="Usernestedgv" runat="server" AutoGenerateColumns="false" CssClass="ChildGrid">
                                                               <Columns>
                                                                   <asp:TemplateField HeaderStyle-Width="5%" HeaderText="Sr No">
                                                                       <ItemTemplate>
                                                                           <%#Container.DataItemIndex+1 %>
                                                                       </ItemTemplate>
                                                                   </asp:TemplateField>
                                                                   <asp:BoundField ItemStyle-Width="150px" DataField="userid" HeaderText="User Id" />
                                                                   <asp:BoundField ItemStyle-Width="150px" DataField="COMPNAME" HeaderText="User Name" />
                                                                   <asp:BoundField ItemStyle-Width="150px" DataField="Submitted" HeaderText="Submitted" />
                                                                   <asp:BoundField ItemStyle-Width="150px" DataField="SubmittedPer" Visible="false" HeaderText="Submitted (%)" />
                                                                   <asp:BoundField ItemStyle-Width="150px" DataField="Delivered" HeaderText="Delivered" />
                                                                   <asp:BoundField ItemStyle-Width="150px" DataField="Failed" HeaderText="Failed" />
                                                                   <asp:BoundField ItemStyle-Width="150px" DataField="Unknown" HeaderText="Unknown" />
                                                               </Columns>
                                                           </asp:GridView>
                                                       </asp:Panel>
                                                   </ItemTemplate>
                                               </asp:TemplateField>



                                                                   <asp:TemplateField HeaderStyle-Width="5%" HeaderText="Sr No">
                                                                       <ItemTemplate>
                                                                           <%#Container.DataItemIndex+1 %>
                                                                       </ItemTemplate>
                                                                   </asp:TemplateField>
                                                                   <asp:BoundField ItemStyle-Width="150px" DataField="userid" HeaderText="User Id" />
                                                                   <asp:BoundField ItemStyle-Width="150px" DataField="DLTNO" HeaderText="DLTNO" />
                                                                   <asp:BoundField ItemStyle-Width="150px" DataField="Submitted" HeaderText="Submitted" />
                                                                   <asp:BoundField ItemStyle-Width="150px" DataField="SubmittedPer" Visible="false" HeaderText="Submitted (%)" />
                                                                   <asp:BoundField ItemStyle-Width="150px" DataField="Delivered" HeaderText="Delivered" />
                                                                   <asp:BoundField ItemStyle-Width="150px" DataField="Failed" HeaderText="Failed" />
                                                                   <asp:BoundField ItemStyle-Width="150px" DataField="Unknown" HeaderText="Unknown" />
                                                               </Columns>
                                                           </asp:GridView>
                                                       </asp:Panel>
                                                   </ItemTemplate>
                                               </asp:TemplateField>


                                                                   <asp:TemplateField HeaderStyle-Width="5%" HeaderText="Sr No">
                                                                       <ItemTemplate>
                                                                           <%#Container.DataItemIndex+1 %>
                                                                       </ItemTemplate>
                                                                   </asp:TemplateField>
                                                                   <asp:BoundField ItemStyle-Width="150px" DataField="GROUPNAME" HeaderText="Group Name" />
                                                                   <asp:BoundField ItemStyle-Width="150px" DataField="Submitted" HeaderText="Submitted" />
                                                                   <asp:BoundField ItemStyle-Width="150px" DataField="SubmittedPer" Visible="false" HeaderText="Submitted (%)" />
                                                                   <asp:BoundField ItemStyle-Width="150px" DataField="Delivered" HeaderText="Delivered" />
                                                                   <asp:BoundField ItemStyle-Width="150px" DataField="Failed" HeaderText="Failed" />
                                                                   <asp:BoundField ItemStyle-Width="150px" DataField="Unknown" HeaderText="Unknown" />
                                                               </Columns>
                                                           </asp:GridView>
                                                       </asp:Panel>
                                                   </ItemTemplate>
                                               </asp:TemplateField>

                                               <asp:TemplateField HeaderStyle-Width="5%" HeaderText="Sr No">
                                                   <ItemTemplate>
                                                       <%#Container.DataItemIndex+1 %>
                                                   </ItemTemplate>
                                               </asp:TemplateField>
                                               
                                               <asp:TemplateField HeaderText="User ID">
                                                   <ItemTemplate>
                                                       <asp:Label ID="lblUserId" runat="server" Text='<%#Eval("userid")%>'></asp:Label>
                                                   </ItemTemplate>
                                               </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Name">
                                                   <ItemTemplate>
                                                       <asp:Label ID="lblCompa" runat="server" Text='<%#Eval("COMPNAME")%>'></asp:Label>
                                                   </ItemTemplate>
                                               </asp:TemplateField> 
                                               <asp:TemplateField HeaderText="Submitted">
                                                   <ItemTemplate>
                                                       <asp:Label ID="lbl1" runat="server" Text='<%#Eval("Submitted")%>'></asp:Label>
                                                   </ItemTemplate>
                                               </asp:TemplateField>
                                               <asp:TemplateField HeaderText="Submitted (%)" Visible="false">
                                                   <ItemTemplate>
                                                       <asp:Label ID="lblPer" runat="server" Text='<%#Eval("SubmittedPer")%>'></asp:Label>
                                                   </ItemTemplate>
                                               </asp:TemplateField>
                                               <asp:TemplateField HeaderText="Delivered">
                                                   <ItemTemplate>
                                                       <asp:Label ID="lbl2" runat="server" Text='<%#Eval("Delivered")%>'></asp:Label>
                                                   </ItemTemplate>
                                               </asp:TemplateField>
                                               <asp:TemplateField HeaderText="Failed">
                                                   <ItemTemplate>
                                                       <asp:Label ID="lbl3" runat="server" Text='<%#Eval("Failed")%>'></asp:Label>
                                                   </ItemTemplate>
                                               </asp:TemplateField>
                                               <asp:TemplateField HeaderText="Unknown">
                                                   <ItemTemplate>
                                                       <asp:Label ID="lblurlid" runat="server" Text='<%#Eval("Unknown")%>'></asp:Label>
                                                       <asp:HiddenField ID="hfEmpID" runat="server" Value='<%#Eval("EmpCode")%>' />
                                                   </ItemTemplate>
                                               </asp:TemplateField>
                                           </Columns>

                                       </asp:GridView>
                                   </div>

                               </div>
                           </div>
                                        </div>
                                    </div>

                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>

                    </div>


                    <%-- END Top 5 --%>

                    <div class="row mt-4">
                        <div class="col-12 col-xl-6 col-lg-6 col-md-12 col-sm-6 mb-4">
                            <div class="card bg-primary shadow-soft border-light">
                                <div class="row no-gutters align-items-center">
                                    <div class="col-sm-5">
                                        <!-- Header -->
                                        <div class="card-header text-center pb-0">
                                            <h3 class="mb-0">Current Month</h3>
                                            <span class="d-block my-3">
                                                <span class="display-3 font-weight-bold spent-money"><span class="align-baseline font-medium"></span>
                                                    <asp:Label ID="lblCurMonthRs" runat="server" Text=""></asp:Label>
                                                </span>
                                            </span>
                                        </div>
                                    </div>
                                    <div class="col-sm-7">
                                        <!-- Content -->
                                        <div class="card-body pb-0">
                                            <ul class="list-group list-group-flush price-list mb-4">
                                                <li class="list-group-item border-primary pb-1"><span class="fas fa-user-plus"></span><a href="account-list.aspx">A/C Created :
                                                    <asp:Label ID="lblAccountCreated" runat="server" Text="0"></asp:Label></a></li>
                                                <li class="list-group-item border-primary pb-1"><span class="fas fa-wallet"></span><a href="balance-management.aspx">Credit Alloted :
                                                    <asp:Label ID="lblCreditAllotted" runat="server" Text="0"></asp:Label></a></li>
                                                <li class="list-group-item border-primary pb-1"><span class="fas fa-check-circle"></span><a href="account-list.aspx">Active User :
                                                    <asp:Label ID="lblActiveUsers" runat="server" Text="0"></asp:Label></a></li>
                                                <li class="list-group-item border-primary pb-1"><span class="fas fa-times-circle"></span><a href="account-list.aspx">Inactive User:
                                                    <asp:Label ID="lblInactiveUsers" runat="server" Text="0"></asp:Label></a></li>
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
                                                <span class="display-3 font-weight-bold spent-money"><span class="align-baseline font-medium"></span>
                                                    <asp:Label ID="lblLastMonthRs" runat="server" Text=""></asp:Label>
                                                </span>
                                            </span>
                                            <!-- <button type="button" class="btn btn-sm btn-primary btn-block">Add to Cart</button> -->
                                        </div>
                                    </div>
                                    <div class="col-sm-7">
                                        <!-- Content -->
                                        <div class="card-body pb-0">
                                            <ul class="list-group list-group-flush price-list mb-4">
                                                <li class="list-group-item border-primary pb-1"><span class="fas fa-user-plus"></span><a href="account-list.aspx">A/C Created :
                                                    <asp:Label ID="lblLastMonthAccountCreated" runat="server" Text="0"></asp:Label></a></li>
                                                <li class="list-group-item border-primary pb-1"><span class="fas fa-wallet"></span><a href="balance-management.aspx">Credit Alloted :
                                                    <asp:Label ID="lblLastMonthCreditAlloted" runat="server" Text="0"></asp:Label></a></li>
                                                <li class="list-group-item border-primary pb-1"><span class="fas fa-check-circle"></span><a href="account-list.aspx">Active User :
                                                    <asp:Label ID="lblLastMonthActiveUsers" runat="server" Text="0"></asp:Label></a></li>
                                                <li class="list-group-item border-primary pb-1"><span class="fas fa-times-circle"></span><a href="account-list.aspx">Inactive User:
                                                    <asp:Label ID="lblLastMonthInactiveUsers" runat="server" Text="0"></asp:Label></a></li>
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

    <script src="https://cdnjs.cloudflare.com/ajax/libs/c3/0.3.0/c3.min.js"></script>

    <script src="https://cdnjs.cloudflare.com/ajax/libs/d3/3.4.12/d3.min.js"></script>



<%--    <script>
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
    </script>--%>

      <script>
        var chart = c3.generate({
            data: {
                // iris data from R
                columns: [
                    ['Delivered', <%= Session["lblMonthDelivered"] %>],

                    ['Failed', <%= Session["lblMonthFailed"] %>]

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
                  Submitted: '#ff7f0e',
                Delivered: '#2ca02c',
                Failed: '#DB3A1B'
          
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
                Delivered: '#2ca02c',
                Failed: '#DB3A1B'
            });
        }, 100);
    </script>
    <script>
            // Datatable Script 
          
        </script>
</asp:Content>
