<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="campReportU.aspx.cs" Inherits="eMIMPanel.campReportU" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <cc1:ToolkitScriptManager ID="scrmg" runat="server" EnablePageMethods="true">
    </cc1:ToolkitScriptManager>
    <link rel="stylesheet" href="https://unpkg.com/bootstrap-table@1.15.4/dist/bootstrap-table.min.css" />
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
                                    <div class="right-view">
                                        <div class="row">
                                            <div class="col-md-3">
                                                <asp:TextBox ID="txtFrm" runat="server" onchange="javascript:text_changed_from();" class="form-control datepicker" placeholder="From Date" autocomplete="off"></asp:TextBox>
                                                <asp:HiddenField ID="hdntxtFrm" runat="server" />
                                            </div>
                                            <div class="col-md-3">
                                                <asp:TextBox ID="txtTo" runat="server" onchange="javascript:text_changed_to();" class="form-control datepicker" placeholder="To Date" autocomplete="off"></asp:TextBox>
                                                <asp:HiddenField ID="hdntxtTo" runat="server" />
                                            </div>
                                            <div class="col-md-3">
                                                <asp:DropDownList ID="ddlCamp" runat="server" class="custom-select my-3 my-lg-0">
                                                </asp:DropDownList>
                                            </div>

                                            <div class="col-md-3">
                                                <asp:LinkButton runat="server" OnClientClick=" return CheckDates();" ID="lnkShow" OnClick="lnkShow_Click" class="btn btn-block">
                                                    Show <i class="fas fa-eye" aria-hidden="true"></i>
                                                </asp:LinkButton>
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
            <%--<script src="https://unpkg.com/bootstrap-table@1.15.4/dist/bootstrap-table.min.js"></script>
            <script src="https://unpkg.com/bootstrap-table@1.15.4/dist/bootstrap-table-locale-all.min.js"></script>--%>
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
        </div>
    </main>
    <script type="text/javascript">
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
            function EndRequestHandler(sender, args) {
                $('.datepicker').datepicker({ format: 'yyyy-mm-dd' });
            }
        });
        function loadscrq() {
            $('.datepicker').datepicker({ format: 'yyyy-mm-dd' });
        }
    </script>
    <script>
        $(function () {
            var threeYearsAgo = new Date();
            threeYearsAgo.setFullYear(threeYearsAgo.getFullYear() - 3);

            $(".datepicker").datepicker({
                endDate: new Date(),
                todayHighlight: true,
                autoclose: true,
                startDate: threeYearsAgo,
                format: 'yyyy-mm-dd'
            }).datepicker("setDate", "0");  
        });
    </script>
    <script type="text/javascript">
        function ResendSmsProcess(fileid) {
            window.location.href = "send-sms-u-B4Send.aspx?Id=" + fileid;
        }
    </script>
    <script type="text/javascript">
        function ResendSmsDuplicateProcess(fileid) {
            window.location.href = "send-sms-u-B4Send.aspx?FId=" + fileid;
        }
    </script>
    <script>
        function CheckDates() {
            var fromDate = $("#<%= txtFrm.ClientID %>").datepicker("getDate");
            var toDate = $("#<%= txtTo.ClientID %>").datepicker("getDate");

            // Check if either From Date or To Date is empty or null
            if ((fromDate == null || toDate == null) || (fromDate === "" || toDate === "")) {
                alert("From date and To Date cannot be empty");
                return false;
            }

            // Check if From Date is greater than To Date
            if (fromDate > toDate) {
                alert("From Date cannot be greater than To Date");
                return false;
            }
            return true;
        }
    </script>
    <script type="text/javascript">
        function DownloadCSV(FileProcessId, TYPE) {
            $.ajax({
                type: 'POST',
                url: '<%=ResolveUrl("campReportU.aspx/UploadedDownloadCSV") %>',
                data: JSON.stringify({ FileProcessId: FileProcessId, TYPE: TYPE }),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (response) {
                    var d = response.d; // Accessing the nested 'd' object from the response
                    if (d.hasOwnProperty('error')) {
                        // Handle error
                    } else {
                        console.log('Response:', d); // Check the 'd' object

                        // Access fileName and csvData from the 'd' object
                        var fileName = d.fileName;
                        var csvData = d.csvData;

                        console.log('FileName:', fileName); // Check the value of fileName
                        console.log('CsvData:', csvData); // Check the value of csvData

                        // Check if the properties are accessible directly
                        console.log('Direct Access - FileName:', fileName);
                        console.log('Direct Access - CsvData:', csvData);

                        // Trigger file download using Blob and anchor link
                        var blob = new Blob([csvData], { type: 'text/csv' });
                        var url = window.URL.createObjectURL(blob);
                        var a = document.createElement('a');
                        a.href = url;
                        a.download = fileName + ".csv";
                        document.body.appendChild(a);
                        a.click();
                        window.URL.revokeObjectURL(url);
                    }
                },
                error: function (xhr, status, error) {
                    // Handle error
                }
            });
        }

    </script>
</asp:Content>
