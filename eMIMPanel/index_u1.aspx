<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="index_u1.aspx.cs" Inherits="eMIMPanel.index_u1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="scrmg" runat="server" EnablePageMethods="true">
    </asp:ToolkitScriptManager>

    <asp:UpdatePanel ID="updFormArea" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <main>
                <div class="container-fluid">

                    <div class="card card-body py-2 mb-4 bg-primary border-light shadow-inset">
                        <div class="d-flex justify-content-end align-items-center">
                            <asp:LinkButton ID="btnRefresh" runat="server" Visible="false" class="mx-0 btn btn-sm btn-primary text-secondary small mr-3" OnClick="btnSubmit_Click"><i class="fas fa-sync"></i></asp:LinkButton>
                            Last updated on &nbsp;&nbsp;
                                    <asp:Label ID="lblLastUpd" runat="server" Text=""></asp:Label>
                        </div>
                    </div>

                    <!-- Content Row -->
                    <div class="row">
                        <div class="col-12">
                            <div class="card bg-primary border-light shadow-soft mb-4">
                                <div class="card-header py-3 bg-primary d-flex justify-content-between">
                                    <div>
                                        <h6 class="m-0 font-weight-bold"><i class="fas fa-chart-area"></i>SMS Delivery
                                        </h6>
                                    </div>
                                    <div class="">
                                        <asp:Label ID="lblLastDays" runat="server" Text="Last 10 Days"></asp:Label>
                                        <%--<select name="" id="" class="form-control">
                                                    <option value="" selected>Last 15 Min</option>
                                                    <option value="">Last 6 Hour</option>
                                                    <option value="">Last 24 Hour</option>
                                                    <option value="">Last 7 Days</option>
                                                    <option value="">Last 30 Days</option>
                                                    <option value="">This Week</option>
                                                    <option value="">Last Week</option>
                                                    <option value="">This Month</option>
                                                    <option value="">Last Month</option>
                                                </select>--%>
                                    </div>
                                </div>
                                <div class="card-body py-2">
                                    <div class="row">
                                        <div class="col-12">

                                            <div class="chart">
                                                <!-- Chart wrapper -->
                                                <canvas id="chart-sales-dark" class="chart-canvas"></canvas>
                                            </div>
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
                                    <h6 class="m-0 font-weight-bold"><i class="fas fa-comment-alt"></i>SMS Performance (Today's)</h6>
                                </div>
                                <div class="card-body py-2">
                                    <div class="row">
                                        <!-- Earnings (Monthly) Card Example -->
                                        <div class="col-12 col-md-4 col-sm-4 col-lg-4 col-xl-4 mb-2">
                                            <a href="sms-reports_usr.aspx">
                                                <div class="card bg-primary shadow-inset border-light h-100 py-2">
                                                    <div class="card-body py-2">
                                                        <div class="d-flex justify-content-between align-items-center">
                                                            <div class="icon icon-shape shadow-soft rounded-circle">
                                                                <span class="fas fa-paper-plane fa-2x bg-primary"></span>
                                                            </div>
                                                            <div class="flex-right-height text-right">
                                                                <p class="font-weight-bold mb-1">Submitted</p>
                                                                <h2 class="m-0">
                                                                    <asp:Label ID="lblSubmit" runat="server" Text=""></asp:Label></h2>
                                                                <p class="font-weight-medium p-0 m-0"><span class="text-success mr-2"></span></p>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </a>
                                        </div>

                                        <!-- Earnings (Monthly) Card Example -->
                                        <div class="col-12 col-md-4 col-sm-4 col-lg-4 col-xl-4 mb-2">
                                            <a href="sms-reports_usr.aspx">
                                                <div class="card bg-primary shadow-inset border-light h-100 py-2">
                                                    <div class="card-body py-2">
                                                        <div class="d-flex justify-content-between align-items-center">
                                                            <div class="icon icon-shape shadow-soft rounded-circle">
                                                                <span class="fas fa-check-double fa-2x text-success"></span>
                                                            </div>
                                                            <div class="flex-right-height text-right">
                                                                <p class="font-weight-bold mb-1">Deliverd</p>
                                                                <h2 class="m-0">
                                                                    <asp:Label ID="lblDlr" runat="server" Text=""></asp:Label></h2>
                                                                <p class="font-weight-medium p-0 m-0">
                                                                    <span class="text-success mr-2">
                                                                        <asp:Label ID="lblDlrPer" runat="server" Text=""></asp:Label>%</span>
                                                                </p>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </a>
                                        </div>
                                        <!-- Earnings (Monthly) Card Example -->
                                        <div class="col-12 col-md-4 col-sm-4 col-lg-4 col-xl-4 mb-2">
                                            <a href="sms-reports_usr.aspx">
                                                <div class="card bg-primary shadow-inset border-light h-100 py-2">
                                                    <div class="card-body py-2">
                                                        <div class="d-flex justify-content-between align-items-center">
                                                            <div class="icon icon-shape shadow-soft rounded-circle">
                                                                <span class="fas fa-thumbs-down fa-2x text-danger"></span>
                                                            </div>
                                                            <div class="flex-right-height text-right">
                                                                <p class="font-weight-bold mb-1">Failed</p>
                                                                <h2 class="m-0">
                                                                    <asp:Label ID="lblFail" runat="server" Text=""></asp:Label></h2>
                                                                <p class="font-weight-medium p-0 m-0">
                                                                    <span class="text-success mr-2">
                                                                        <asp:Label ID="lblFailPer" runat="server" Text=""></asp:Label>%</span>
                                                                </p>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </a>
                                        </div>
                                        <!--  -->
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
                                    <h6 class="m-0 font-weight-bold"><i class="fas fa-comment-slash"></i>SMS Failure Breakup</h6>
                                </div>
                                <div class="card-body py-2" style="font-size:small;" >

                                    <div class="row">
                                        <!-- Earnings (Monthly) Card Example -->
                                        <div class="col-12 col-md-6 col-sm-6 col-lg-3 col-xl-6 mb-2">
                                            <a href="analytics_u.aspx">
                                                <div class="card bg-primary shadow-inset border-light h-100 py-2">
                                                    <div class="card-body py-2">
                                                        <div class="d-flex justify-content-between align-items-center">
                                                            <div class="icon icon-shape shadow-soft rounded-circle">
                                                                <span class="fas fa-exclamation-triangle fa-2x text-warning"></span>
                                                            </div>
                                                            <div class="flex-right-height text-right">
                                                                <p class="font-weight-bold mb-1">'<%= Session["Desc1"] %>'</p>
                                                                <h6 class="m-0 h6">
                                                                    <asp:Label ID="lblFailPer1" runat="server" Text=""></asp:Label></h6>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </a>
                                        </div>

                                        <!-- Earnings (Monthly) Card Example -->
                                        <div class="col-12 col-md-6 col-sm-6 col-lg-3 col-xl-6 mb-2">
                                            <a href="analytics_u.aspx">
                                                <div class="card bg-primary shadow-inset border-light h-100 py-2">
                                                    <div class="card-body py-2">
                                                        <div class="d-flex justify-content-between align-items-center">
                                                            <div class="icon icon-shape shadow-soft rounded-circle">
                                                                <span class="fas fa-ban fa-2x text-danger"></span>
                                                            </div>
                                                            <div class="flex-right-height text-right">
                                                                <p class="font-weight-bold mb-1">'<%= Session["Desc2"] %>'</p>
                                                                <h6 class="m-0 h6">
                                                                    <asp:Label ID="lblFailPer2" runat="server" Text=""></asp:Label></h6>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </a>
                                        </div>
                                        <!-- Earnings (Monthly) Card Example -->
                                        <div class="col-12 col-md-6 col-sm-6 col-lg-3 col-xl-6 mb-2">
                                            <a href="click-reports_u.aspx">
                                                <div class="card bg-primary shadow-inset border-light h-100 py-2">
                                                    <div class="card-body py-2">
                                                        <div class="d-flex justify-content-between align-items-center">
                                                            <div class="icon icon-shape shadow-soft rounded-circle">
                                                                <span class="fas fa-not-equal fa-2x text-danger"></span>
                                                            </div>
                                                            <div class="flex-right-height text-right">
                                                                <p class="font-weight-bold mb-1">'<%= Session["Desc3"] %>'</p>
                                                                <h6 class="m-0 h6">
                                                                    <asp:Label ID="lblFailPer3" runat="server" Text=""></asp:Label></h6>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </a>
                                        </div>
                                        <!--  -->
                                        <div class="col-12 col-md-6 col-sm-6 col-lg-3 col-xl-6 mb-2">
                                            <a href="click-reports_u.aspx">
                                                <div class="card bg-primary shadow-inset border-light h-100 py-2">
                                                    <div class="card-body py-2">
                                                        <div class="d-flex justify-content-between align-items-center">
                                                            <div class="icon icon-shape shadow-soft rounded-circle">
                                                                <span class="fas fa-star-of-life fa-2x text-danger"></span>
                                                            </div>
                                                            <div class="flex-right-height text-right">
                                                                <p class="font-weight-bold mb-1">'<%= Session["Desc4"] %>'</p>
                                                                <h6 class="m-0 h6">
                                                                    <asp:Label ID="lblFailPer4" runat="server" Text=""></asp:Label></h6>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </a>
                                        </div>
                                        <!--  -->
                                    </div>

                                    <!--  -->
                                    <div class="d-xl-flex justify-content-between">

                                        <!-- <div>
                                                    <p class="text-muted m-0">Invalid/ Not Reachable </p>
                                                    <h3>80.9%</h3>
                                                </div>
                                                <div>
                                                    <p class="text-muted m-0">Blocked By DLT</p>
                                                    <h3>0.004%</h3>
                                                </div>
                                                <div>
                                                    <p class="text-muted m-0">Template Mismatch</p>
                                                    <h3>0%</h3>
                                                </div>
                                                <div>
                                                    <p class="text-muted m-0">Others</p>
                                                    <h3>16.66%</h3>
                                                </div> -->
                                    </div>
                                    <!--  -->
                                </div>

                                <div class="card-body py-2">
                                    <div class="row">
                                        <div class="col-12 col-md-6 col-sm-6 col-lg-3 col-xl-6 mb-2">
                                            <a href="analytics_u.aspx">
                                                <div class="card bg-primary shadow-inset border-light h-100 py-2">
                                                    <div class="card-body py-2">
                                                        <div class="d-flex justify-content-between align-items-center">
                                                            <div class="icon icon-shape shadow-soft rounded-circle">
                                                                <span class="fas fa-exclamation-triangle fa-2x text-warning"></span>
                                                            </div>
                                                            <div class="flex-right-height text-right">
                                                                <p class="font-weight-bold mb-1">'<%= Session["Desc5"] %>'</p>
                                                                <h6 class="m-0 h6">
                                                                    <asp:Label ID="lblFailPer5" runat="server" Text=""></asp:Label></h6>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </a>
                                        </div>
                                         <div class="col-12 col-md-6 col-sm-6 col-lg-3 col-xl-6 mb-2">
                                            <a href="analytics_u.aspx">
                                                <div class="card bg-primary shadow-inset border-light h-100 py-2">
                                                    <div class="card-body py-2">
                                                        <div class="d-flex justify-content-between align-items-center">
                                                            <div class="icon icon-shape shadow-soft rounded-circle">
                                                                <span class="fas fa-exclamation-triangle fa-2x text-warning"></span>
                                                            </div>
                                                            <div class="flex-right-height text-right">
                                                                <p class="font-weight-bold mb-1">'<%= Session["Desc6"] %>'</p>
                                                                <h6 class="m-0 h6">
                                                                    <asp:Label ID="lblFailPer6" runat="server" Text=""></asp:Label></h6>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </a>
                                        </div>
                                         <div class="col-12 col-md-6 col-sm-6 col-lg-3 col-xl-6 mb-2">
                                            <a href="analytics_u.aspx">
                                                <div class="card bg-primary shadow-inset border-light h-100 py-2">
                                                    <div class="card-body py-2">
                                                        <div class="d-flex justify-content-between align-items-center">
                                                            <div class="icon icon-shape shadow-soft rounded-circle">
                                                                <span class="fas fa-exclamation-triangle fa-1x text-warning"></span>
                                                            </div>
                                                            <div class="flex-right-height text-right">
                                                                <p class="font-weight-bold mb-1 text-wrap">'<%= Session["Desc7"] %>'</p>
                                                                <h6 class="m-0 h6">
                                                                    <asp:Label ID="lblFailPer7" runat="server" Text=""></asp:Label></h6>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </a>
                                        </div>
                                         <div class="col-12 col-md-6 col-sm-6 col-lg-3 col-xl-6 mb-2">
                                            <a href="analytics_u.aspx">
                                                <div class="card bg-primary shadow-inset border-light h-100 py-2">
                                                    <div class="card-body py-2">
                                                        <div class="d-flex justify-content-between align-items-center">
                                                            <div class="icon icon-shape shadow-soft rounded-circle">
                                                                <span class="fas fa-exclamation-triangle fa-2x text-warning"></span>
                                                            </div>
                                                            <div class="flex-right-height text-right">
                                                                <p class="font-weight-bold mb-1 text-wrap">'<%= Session["Desc8"] %>'</p>
                                                                <h6 class="m-0 h6">
                                                                    <asp:Label ID="lblFailPer8" runat="server" Text=""></asp:Label></h6>
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
                    <!--  -->
                </div>
            </main>
        </ContentTemplate>
    </asp:UpdatePanel>

    <!-- Bootstrap core JavaScript-->
    <script src="vendor/jquery/jquery-3.5.1.min.js"></script>

    <!-- Optional JS -->
    <script src="../assets/vendor/chart.js/dist/Chart.min.js"></script>
    <script src="../assets/vendor/chart.js/dist/Chart.extension.js"></script>
    <script>
        Date1 =  '<%= Session["Date1"] %>';
        Date2 =  '<%= Session["Date2"] %>';
        Date3 =  '<%= Session["Date3"] %>';
        Date4 =  '<%= Session["Date4"] %>';
        Date5 =  '<%= Session["Date5"] %>';
        Date6 =  '<%= Session["Date6"] %>';
        Date7 =  '<%= Session["Date7"] %>';
        Date8 =  '<%= Session["Date8"] %>';
        Date9 =  '<%= Session["Date9"] %>';
        Date10 =  '<%= Session["Date10"] %>';

        Value1 =  '<%= Session["Value1"] %>';
        Value2 =  '<%= Session["Value2"] %>';
        Value3 =  '<%= Session["Value3"] %>';
        Value4 =  '<%= Session["Value4"] %>';
        Value5 =  '<%= Session["Value5"] %>';
        Value6 =  '<%= Session["Value6"] %>';
        Value7 =  '<%= Session["Value7"] %>';
        Value8 =  '<%= Session["Value8"] %>';
        Value9 =  '<%= Session["Value9"] %>';
        Value10 =  '<%= Session["Value10"] %>';

    </script>
    <script src="js/chart-custom.js"></script>

</asp:Content>
