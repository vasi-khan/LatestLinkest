<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="campReportU_New.aspx.cs" Inherits="eMIMPanel.campReportU_New" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <cc1:ToolkitScriptManager ID="scrmg" runat="server" EnablePageMethods="true">
    </cc1:ToolkitScriptManager>

  <%--  <link rel="stylesheet" href="https://unpkg.com/bootstrap-table@1.15.4/dist/bootstrap-table.min.css" />
    <link rel="stylesheet" href="https://unpkg.com/bootstrap-table@1.15.4/dist/bootstrap-table.min.js" />
    <link rel="stylesheet" href="https://unpkg.com/bootstrap-table@1.15.4/dist/bootstrap-table-locale-all.min.js" />--%>

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
                    <li class="breadcrumb-item"><a href="#">Reports</a></li>
                    <li class="breadcrumb-item active" aria-current="page">Campaign Report</li>
                </ol>
            </nav>
            <div class="row">
                <div class="col-12">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="card card-body mb-4 bg-primary border-light shadow-soft">
                                <div class="form-row">

                                    <asp:RadioButton ID="rbTdy" runat="server" AutoPostBack="true" Text="Today" GroupName="Filter" Checked="true" Style="margin-left: 20px" OnCheckedChanged="rbTdy_CheckedChanged" />
                                    <asp:RadioButton ID="rbHis" runat="server" AutoPostBack="true" OnCheckedChanged="rbHis_CheckedChanged" Text="Old" Style="margin-left: 20px; margin-right: 50px" GroupName="Filter" />

                                    <div class="col-md-3">
                                        <asp:LinkButton runat="server" ID="lnkShow" OnClick="lnkShow_Click" class="btn btn-block">
                                                    Show <i class="fas fa-eye" aria-hidden="true"></i>
                                        </asp:LinkButton>
                                    </div>

                                </div>
                                <div id="divOld" runat="server" class="form-row mb-2 d-none">
                                    <div class="form-row mt-2">
                                        <div class="row">
                                            <div class="col-md-3">
                                                <asp:TextBox ID="txtFrm" runat="server" onchange="javascript:text_changed_from();" class="form-control" placeholder="From Date" autocomplete="off" TextMode="Date"></asp:TextBox>
                                                <asp:HiddenField ID="hdntxtFrm" runat="server" />
                                            </div>
                                            <div class="col-md-3">
                                                <asp:TextBox ID="txtTo" runat="server" onchange="javascript:text_changed_to();" class="form-control" placeholder="To Date" autocomplete="off" TextMode="Date"></asp:TextBox>
                                                <asp:HiddenField ID="hdntxtTo" runat="server" />
                                            </div>
                                            <div class="col-md-3">
                                                <asp:DropDownList ID="ddlCamp" runat="server" class="custom-select my-3 my-lg-0">
                                                </asp:DropDownList>
                                                <%--<asp:TextBox ID="txtCamp" runat="server" placeholder="Campaign" autocomplete="off" CssClass="form-control my-3 my-lg-0"></asp:TextBox>--%>
                                            </div>
                                            <asp:HiddenField ID="h1" runat="server" />
                                            <asp:HiddenField ID="h2" runat="server" />
                                        </div>
                                    </div>
                                </div>

                            </div>
                            <!-- Card End -->


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
                        </Triggers>
                    </asp:UpdatePanel>

                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="0">
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
                }

            </script>
            <!-- Bootstrap core JavaScript-->
            <script src="vendor/jquery/jquery-3.5.1.min.js"></script>
            <script src="vendor/bootstrap/js/bootstrap.bundle.min.js"></script>
            <script src="vendor/datepicker/bootstrap-datepicker.js"></script>


            <!-- Core plugin JavaScript-->
            <script src="vendor/jquery-easing/jquery.easing.min.js"></script>

            <!--  Date-->
            <script src="vendor/datepicker/moment.min.js"></script>
            <script src="vendor/datepicker/daterangepicker.min.js"></script>

            <!-- Custom scripts for all pages-->
            <script src="js/sb-admin-2.min.js"></script>

            <!-- Page level plugins -->
            <!-- <script src="vendor/chart.js/Chart.min.js"></script>  -->

            <!-- Page level custom scripts -->
            <!-- <script src="js/demo/chart-pie-demo.js"></script> 
                                            <script src="js/demo/chart-bar-demo.js"></script>  -->

            <!-- Page level plugins -->
            <script src="vendor/datatables/jquery.dataTables.min.js"></script>
            <script src="vendor/datatables/dataTables.bootstrap4.min.js"></script>
            <script src="vendor/datatables/dataTables.responsive.min.js"></script>
            <script src="vendor/datatables/responsive.bootstrap4.min.js"></script>

            <!--  Select-->
            <script src="vendor/select/bootstrap-select.min.js"></script>

            <!-- Page level custom scripts -->
            <script src="js/demo/datatables-demo.js"></script>
            <script src="js/demo/date-range-picker-demo.js"></script>

            <script type="text/javascript">
                $(function () {

                    var pDate = new Date();
                    pDate.setDate(pDate.getDate() - 45);
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
                    console.log(date);
                    console.log(date1);
                    $('[id*=txtFrm]').attr('max', date);
                    $('[id*=txtFrm]').attr('min', date1);

                    $('[id*=txtTo]').attr('max', date);
                    $('[id*=txtTo]').attr('min', date1);

                });
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
    </main>
    <script type="text/javascript">
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

            function EndRequestHandler(sender, args) {
                $('.datepicker').datepicker({ dateFormat: 'dd-mm-yy' });
            }

        });
        function loadscrq() {
            $('.datepicker').datepicker({ dateFormat: 'dd-mm-yy' });
        }
    </script>
</asp:Content>
