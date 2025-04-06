<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="sms_reports_usr_mobileno_wise.aspx.cs" Inherits="eMIMPanel.sms_reports_usr_mobileno_wise" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <style type="text/css">
        /*CSS Classes For Design Modal*/
        .modal.modalPopup {
            top: 0 !important;
            left: 0 !important;
            display: block;
        }

        .modalBackground {
            background-color: #000;
            opacity: 0.5;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <cc1:ToolkitScriptManager ID="scrmg" runat="server" EnablePageMethods="true">
    </cc1:ToolkitScriptManager>
    <main>
        <div class="container-fluid">
            <nav aria-label="breadcrumb" class="my-3">
                <ol class="breadcrumb breadcrumb-info">
                    <li class="breadcrumb-item"><a href="#">Home</a></li>
                    <li class="breadcrumb-item"><a href="#">Reports</a></li>
                    <li class="breadcrumb-item active" aria-current="page">Mobile No Report</li>
                </ol>
            </nav>
            <div class="row">
                <div class="col-12">
                    <div class="card card-body mb-4 bg-primary border-light shadow-soft">

                        <div class="form-row">
                            <asp:RadioButton ID="rbTdy" runat="server" AutoPostBack="true" Text="Today" GroupName="Filter" Checked="true" Style="margin-left: 20px" OnCheckedChanged="rbTdy_CheckedChanged" />
                            <asp:RadioButton ID="rbHis" runat="server" AutoPostBack="true" OnCheckedChanged="rbHis_CheckedChanged" Text="Old" Style="margin-left: 20px; margin-right:50px" GroupName="Filter" />
                            <div class="form-group col-lg-2 col-xl-2">
                                <asp:TextBox ID="txtmob" runat="server" class="form-control" placeholder="Enter Mobile No" MaxLength="15"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterMode="ValidChars"
                                        TargetControlID="txtmob" ValidChars="0123456789">
                                </cc1:FilteredTextBoxExtender>
                            </div>
                            <div class="form-group col-lg-2 col-xl-2" id="DivDateForm" runat="server" visible="false">
                               <asp:TextBox ID="txtFrm1" runat="server" onchange="javascript:text_changed_from1();" class="form-control datepicker" placeholder="From Date"    autocomplete="off" ></asp:TextBox>
                               <asp:HiddenField ID="hdntxtFrm1" runat="server" />
                            </div>
                            <div class="form-group col-lg-2 col-xl-2" id="DivDateTo" runat="server" visible="false">
                               <asp:TextBox ID="txtTo1" runat="server" onchange="javascript:text_changed_to1();" class="form-control datepicker" placeholder="To Date"   autocomplete="off"></asp:TextBox>
                               <asp:HiddenField ID="hdntxtTo1" runat="server" />
                             </div>
                            <div class="form-group col-lg-2  col-xl-2 mt-auto">
                                <asp:LinkButton OnClientClick=" return CheckDates();" ID="btnsearch" runat="server" OnClick="btnsearch_Click" class="btn btn-primary text-success btn-block"><i class="fas fa-search fa-sm text-success"></i>Search</asp:LinkButton>
                            </div>
                        </div>

                    <div class="card card-body mb-4 bg-primary border-light shadow-soft">
                        <!-- Loader -->
                        <%--<div class="o-page-loader">
                            <div class="o-page-loader--content">
                                <div class="o-page-loader--spinner"></div>
                                <div class="o-page-loader--message">
                                    <span>Loading...</span>
                                </div>
                            </div>
                        </div>--%>
                        <!--End Loader -->
                         <div class="flex-fill text-right">
                                <asp:LinkButton OnClientClick="CheckDates();"  runat="server" ID="lnkDownload" OnClick="lnkDownload_Click" class="btn btn-mini">
                                    Download <i class="fas fa-download" aria-hidden="true"></i>
                                 </asp:LinkButton>
                         </div>
                        <div class="table-responsive">
                            <asp:GridView ID="grv" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
                            runat="server" Width="100%" CellPadding="10"
                            BorderColor="#ede8e8" Class="table table-striped table-bordered dt-responsive nowrap dataTable-view">
                            <Columns>
                                <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Sr No">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDate" runat="server" Text='<%#Eval("SMSDATE")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Message Id">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUserId" runat="server" Text='<%#Eval("Messageid")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Mobile No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMobileNo" runat="server" Text='<%#Eval("MobileNo")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Sender ID">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSenderId" runat="server" Text='<%#Eval("Sender")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Sent Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblsentDate" runat="server" Text='<%#Eval("SentDate")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Delivered Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDeliveredDate" runat="server" Text='<%#Eval("DeliveredDate")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Message">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMessage" runat="server" Text='<%#Eval("Message")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Message State">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMessageState" runat="server" Text='<%#Eval("MessageState")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Response">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMessage" runat="server" Text='<%#Eval("RESPONSE")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>

                        </div>
                    </div>
                </div>
            </div>

        </div>
    </main>
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

            /*
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

            $('[id*=txtFrm1]').attr('max', date);
            $('[id*=txtFrm1]').attr('min', date1);

            $('[id*=txtTo1]').attr('max', date);
            $('[id*=txtTo1]').attr('min', date1);*/

                var currentDate = new Date();

            var fortyfivedaysago = new Date();
                     fortyfivedaysago.setDate(currentDate.getDate() - 45);
                $(".datepicker").datepicker({
                    endDate: new Date(),
                    todayHighlight: true,
                    autoclose: true,
                    startDate: fortyfivedaysago,
                    format: 'yyyy-mm-dd'
                });



    });
</script>
    <script type="text/javascript">  
        function view(senderid, fileid) {
            console.log(senderid);
            console.log(fileid);


            var today = new Date();
            var dd = String(today.getDate()).padStart(2, '0');
            var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
            var yyyy = today.getFullYear();

            var user = '<% =Session["UserID"].ToString() %>';
            var startDate = yyyy + '-' + mm + '-' + dd;
            var endDate = startDate + ' 23:59:59';

            console.log(startDate);
            console.log(endDate);
            console.log(user);

            $.ajax({
                type: "POST",
                dataType: "json",
                url: "WebService.asmx/GetSMSReportDetailUserNew",
                data: { dater: startDate + '$' + endDate + '$' + user + '$' + fileid + '$' + senderid },
                success: function (data) {
                    var datatableVariable = $('#rpttable').DataTable({
                        data: data,
                        columns: [
                            { 'data': 'sln' },
                            { 'data': 'msgid' },
                            { 'data': 'mobile' },
                            { 'data': 'senderid' },
                            { 'data': 'senttime' },
                            { 'data': 'dlrtime' },
                            { 'data': 'msgtext' },
                            { 'data': 'dlrstat' },
                            { 'data': 'dlrresp' }
                        ]
                    });
                }
            });
            console.log("done");
            document.getElementById('ContentPlaceHolder1_collapseOne').className = 'collapse';
            document.getElementById('ContentPlaceHolder1_collapseTwo').className = 'collapse show';
            console.log("done222");
        }

    </script>

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
        function text_changed_from1() {
            var d = document.getElementById("ContentPlaceHolder1_txtFrm1").value
            console.log(d);
            document.getElementById("ContentPlaceHolder1_hdntxtFrm1").value = d;
        }
        function text_changed_to1() {
            var d = document.getElementById("ContentPlaceHolder1_txtTo1").value
            console.log(d);
            document.getElementById("ContentPlaceHolder1_hdntxtTo1").value = d;
        }

    </script>

    <script>
        (function (loader) {

            window.addEventListener('beforeunload', function (e) {
                activateLoader();
            });

            window.addEventListener('load', function (e) {
                deactivateLoader();
            });

            function activateLoader() {
                loader.style.display = 'block';
                loader.style.opacity = 1;
            }

            function deactivateLoader() {
                /**
                 * ensures that the loading animation plays for at least a second to give the
                 * appearance of seamless loading on pages that execute and load extremely
                 * quickly (i.e., intranet pages)
                 */
                setTimeout(function () {
                    deactivate();
                }, 1000);

                function deactivate() {
                    loader.style.opacity = 0;
                    loader.addEventListener('transitionend', function () {
                        loader.style.display = 'none';
                    }, false);
                }
            }

        })(document.querySelector('.o-page-loader'));
    </script>
   <script>
       
              function CheckDates() {
            var fromDate = $("#<%= txtFrm1.ClientID %>").datepicker("getDate");
           var toDate = $("#<%= txtTo1.ClientID %>").datepicker("getDate");

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


</asp:Content>
