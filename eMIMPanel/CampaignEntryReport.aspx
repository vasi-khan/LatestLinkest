<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="CampaignEntryReport.aspx.cs" Inherits="eMIMPanel.CampaignEntryReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <cc1:ToolkitScriptManager ID="scrmg" runat="server" EnablePageMethods="true">
    </cc1:ToolkitScriptManager>

    <link rel="stylesheet" href="https://unpkg.com/bootstrap-table@1.15.4/dist/bootstrap-table.min.css" />
    <link rel="stylesheet" href="https://unpkg.com/bootstrap-table@1.15.4/dist/bootstrap-table.min.js" />
    <link rel="stylesheet" href="https://unpkg.com/bootstrap-table@1.15.4/dist/bootstrap-table-locale-all.min.js" />
    <script src="vendor/jquery/jquery-3.5.1.min.js"></script>

    <style>
        .table thead th {
            vertical-align: middle;
            border-bottom: 0.125rem solid #6e6e6f;
            text-align: center;
        }

        .table th, .table td {
            padding: 8px;
            font-size: 12px;
            border: 1px solid #9a9a9a;
            text-align: center;
        }

        #ContentPlaceHolder1_divResult thead {
            background: #d2d2d2;
        }

        #ContentPlaceHolder1_divResult tbody tr {
            background: #f1f1f1;
        }
            /* #ContentPlaceHolder1_divResult tbody tr.collapse.show {
            background: #ffffff !important;
        } */
            #ContentPlaceHolder1_divResult tbody tr:nth-child(2n) {
                background: #ffffff;
            }

            #ContentPlaceHolder1_divResult tbody tr.collapse.show table tbody tr:nth-child(odd) {
                background: #f1f1f1;
            }

            #ContentPlaceHolder1_divResult tbody tr.collapse.show table tbody tr:nth-child(even) {
                background: #e8e8e8;
            }
    </style>
    <main>
        <div class="container-fluid">
            <nav aria-label="breadcrumb" class="my-3">
                <ol class="breadcrumb breadcrumb-info">
                    <li class="breadcrumb-item"><a href="#">Home</a></li>
                    <li class="breadcrumb-item"><a href="#">Campaign Entry Reports</a></li>
                    <li class="breadcrumb-item active" aria-current="page">Campaign Report</li>
                </ol>
            </nav>
            <div class="row">
                <div class="col-12">

                    <div class="card card-body mb-4 bg-primary border-light shadow-soft">

                        <div>
                            <div class="right-view">

                                <div class="form-row">

                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtFrm" runat="server" onchange="javascript:text_changed_from();" class="form-control datepicker" placeholder="From Date" autocomplete="off"></asp:TextBox>
                                        <asp:HiddenField ID="hdntxtFrm" runat="server" />
                                    </div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtTo" runat="server" onchange="javascript:text_changed_to();" class="form-control datepicker mt-3 my-lg-0" placeholder="To Date" autocomplete="off"></asp:TextBox>
                                        <asp:HiddenField ID="hdntxtTo" runat="server" />
                                    </div>


                                    <div class="col-md-3">
                                        <asp:RadioButtonList ID="rbUser" runat="server" class="form-control" OnSelectedIndexChanged="rbUser_SelectedIndexChanged" AutoPostBack="true" RepeatDirection="Horizontal">
                                            <asp:ListItem Value="0" Selected="True">All User</asp:ListItem>
                                            <asp:ListItem Value="1">User</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                    <div class="col-md-3" id="dUser" runat="server">
                                        <asp:TextBox ID="txtUser" runat="server" class="form-control" placeholder="Enter User Name"></asp:TextBox>

                                    </div>
                                    <%-- <div class="col-md-3" id="dGo" runat="server">
                                        <asp:Button ID="btnGo" runat="server" class="form-control" Text="GO" OnClick="btnGo_Click" />
                                    </div>--%>
                                    <div class="col-md-3" id="dCamp" runat="server">
                                        <asp:DropDownList ID="ddlCamp" runat="server" class="custom-select my-3 my-lg-0">
                                        </asp:DropDownList>
                                        <%--<asp:TextBox ID="txtCamp" runat="server" placeholder="Campaign" autocomplete="off" CssClass="form-control my-3 my-lg-0"></asp:TextBox>--%>
                                    </div>

                                    <div class="col-md-3">
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>

                                                <asp:LinkButton runat="server" ID="lnkShow" OnClick="lnkShow_Click" class="btn btn-block">
                                                    Show <i class="fas fa-eye" aria-hidden="true"></i>
                                                </asp:LinkButton>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="lnkShow" />
                                            </Triggers>
                                        </asp:UpdatePanel>

                                    </div>

                                    <asp:HiddenField ID="h1" runat="server" />
                                    <asp:HiddenField ID="h2" runat="server" />
                                </div>


                            </div>
                        </div>
                    </div>
                    <!-- Card End -->

                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <!--  -->
                            <div class="card mb-4 bg-primary border-light shadow-soft">
                                <div class="card-header pt-3 pb-0 bg-primary d-flex justify-content-between flex-wrap align-items-center">
                                    <div class="flex-fill">
                                        <h6 class="font-weight-bold mb-0"><i class="fas fa-chart-line"></i>Campaign Report</h6>
                                    </div>
                                    <div class="downB">
                                        <asp:LinkButton runat="server" ID="lnkDownload" OnClick="lnkDownload_Click" class="btn btn-block">
                                                    Download <i class="fas fa-download" aria-hidden="true"></i>
                                        </asp:LinkButton>
                                    </div>
                                </div>

                                <div class="card-body">
                                    <div class="table-responsive">
                                        <div id="divResult" runat="server">
                                            <!-- <asp:Label ID="lblResult" Style="overflow-y: scroll" runat="server"></asp:Label> -->
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!--  -->

                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lnkDownload" />
                            <asp:PostBackTrigger ControlID="txtFrm" />
                        </Triggers>
                    </asp:UpdatePanel>

                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel2" DisplayAfter="0">
                        <ProgressTemplate>
                            <div align="center" style="position: absolute; left: 40%; top: 50%; vertical-align: middle;">
                                <img src="Img/LOADING.GIF" />
                            </div>
                            <div align="center" style="position: absolute; left: 0%; top: 0%; vertical-align: middle;">
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </div>

            </div>

            <!-- Bootstrap core JavaScript-->
            <script src="vendor/jquery/jquery-3.5.1.min.js"></script>
            <script src="vendor/bootstrap/js/bootstrap.bundle.min.js"></script>

            <!-- Core plugin JavaScript-->
            <script src="vendor/jquery-easing/jquery.easing.min.js"></script>

            <!--  Date-->
            <script src="vendor/datepicker/moment.min.js"></script>
            <script src="vendor/datepicker/daterangepicker.min.js"></script>

            <!-- Custom scripts for all pages-->
            <script src="js/sb-admin-2.min.js"></script>

            <script type="text/javascript"> 
                function text_changed_from() {
                    var d = document.getElementById("ContentPlaceHolder1_txtFrm").value
                    console.log(d);
                    document.getElementById("ContentPlaceHolder1_hdntxtFrm").value = d;
                }
                function text_changed_to() {
                    var d = document.getElementById("ContentPlaceHolder1_txtTo").value
                    console.log(d);
                    document.getElementById("ContentPlaceHolder1_hdntxtTo").value = d;
                    var fromDate = $("#<%= txtFrm.ClientID %>").datepicker("getDate");
                    var toDate = $("#<%= txtTo.ClientID %>").datepicker("getDate");
                    // Check if From Date is greater than To Date
                    if (fromDate > toDate) {
                        alert("To Date cannot be lesser than From Date !");
                        document.getElementById("ContentPlaceHolder1_txtTo").value = '';
                        return false;
                    }
                    return true;
                }

            </script>
            <script type="text/javascript">
                $(function () {
                    // popover
                    $('.notePopover').popover();

                    var $table = $('#table');

                    $(".collapse td.inside-table").siblings().remove();

                    $("table tr [data-toggle=collapse]").on('click', function (e) {
                        var rowIndex = ($(this).closest('td').parent()[0].sectionRowIndex);
                        if ($($(this).closest('td').parent()[0]).hasClass('active')) {
                            $('table tr').removeClass('active');
                            $('.collapse').removeClass('show');
                        } else {
                            $($(this).closest('td').parent()[0]).addClass('active');
                        }
                        console.log(rowIndex);
                    });

                });
            </script>



            <script type="text/javascript">
                $(function () {
                    debugger;
                    var pDate = new Date();
                    pDate.setDate(pDate.getDate() - 365);
                    var month1 = ('0' + (pDate.getMonth() + 1)).slice(-2);
                    var day1 = ('0' + pDate.getDate()).slice(-2);
                    var year1 = pDate.getFullYear();
                    var date1 = year1 + '-' + month1 + '-' + day1;


                    var today = new Date();
                    today.setDate(today.getDate() - 1);
                    var month = ('0' + (today.getMonth() + 1)).slice(-2);
                    var day = ('0' + today.getDate()).slice(-2);
                    var year = today.getFullYear();
                    var date = year + '-' + month + '-' + day;

                    $('[id*=txtFrm]').attr('max', date);
                    $('[id*=txtFrm]').attr('min', date1);

                    $('[id*=txtTo]').attr('max', date);
                    $('[id*=txtTo]').attr('min', date1);

                });
            </script>
        </div>
    </main>
</asp:Content>
