<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="sms-reports_sysadmin_DLR_new.aspx.cs" Inherits="eMIMPanel.sms_reports_sysadmin_DLR_new" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .ajax__combobox_itemlist {
            top: 39px !important;
            left: 0px !important;
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
                    <li class="breadcrumb-item active" aria-current="page">Download Delivery Report</li>
                </ol>
            </nav>
            <div class="row">
                <div class="col-12">
                    <div class="card card-body mb-4 bg-primary border-light shadow-soft">

                        <div class="form-row">
                            <asp:RadioButton ID="rbTdy" runat="server" AutoPostBack="true" Text="Today" GroupName="Filter" Checked="true" Style="margin-left: 20px" OnCheckedChanged="rbTdy_CheckedChanged" />
                            <asp:RadioButton ID="rbHis" runat="server" AutoPostBack="true" OnCheckedChanged="rbHis_CheckedChanged" Text="Old" Style="margin-left: 20px; margin-right: 50px" GroupName="Filter" />

                        </div>
                        <div class="form-row">
                            <asp:RadioButton ID="rbSbmtd" runat="server" AutoPostBack="true" Text="Submitted" GroupName="Filter1" Checked="true" Style="margin-left: 20px" />
                            <asp:RadioButton ID="rbDlvr" runat="server" AutoPostBack="true" Text="Delivery" Style="margin-left: 20px" GroupName="Filter1" />
                            <asp:RadioButton ID="rbFailed" runat="server" AutoPostBack="true" Text="Failed" Style="margin-left: 20px; margin-right: 50px" GroupName="Filter1" />
                            <div class="form-group col-lg-2  col-xl-2 mt-auto ">
                                <asp:LinkButton ID="btnsearch" runat="server" OnClick="btnsearch_Click" class="btn btn-primary text-success btn-block"><i class="fas fa-search fa-sm text-success"></i>Search</asp:LinkButton>
                            </div>
                        </div>
                        <div id="divOld" runat="server" class="form-row mt-2 d-none">
                            <div class="form-row" style="margin-left: 20px">

                                <div class="form-group col-lg-4 col-xl-4">
                                    <label for="#">Download Delivery Report</label>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtFrm1" runat="server" onchange="javascript:text_changed_from1();" class="form-control datepicker" placeholder="From Date"  autocomplete="off"></asp:TextBox>

                                            <asp:HiddenField ID="hdntxtFrm1" runat="server" />
                                        </div>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtTo1" runat="server" onchange="javascript:text_changed_to1();" class="form-control datepicker" placeholder="To Date"  autocomplete="off"></asp:TextBox>
                                            <asp:HiddenField ID="hdntxtTo1" runat="server" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-3" style="margin-top: -0.5rem">
                                    <label for="#">User Wise</label>
                                    <asp:TextBox ID="txtUserWise" runat="server" class="form-control" placeholder="USER ID" MaxLength="10"></asp:TextBox>
                                </div>
                                <div class="form-group col-lg-3 col-xl-3">
                                    SenderID wise
                                <div class="input-group">
                                    <div class="input-group-prepend">
                                        <div class="input-group-text">
                                            <%--<asp:RadioButton ID="rdbSender" GroupName="rdb" Checked="true" runat="server" aria-label="Radiobutton for following text input" />--%>
                                        </div>
                                    </div>
                                    <asp:DropDownList ID="ddlSender" runat="server" class="custom-select">
                                    </asp:DropDownList>
                                </div>
                                </div>
                                <div class="form-group col-lg-3 col-xl-3">
                                    Campaign wise
                                <div class="input-group">
                                    <div class="input-group-prepend">
                                        <div class="input-group-text">
                                            <%--<asp:RadioButton ID="rdbCamp" GroupName="rdb" runat="server" aria-label="Radiobutton for following text input" />--%>
                                        </div>
                                    </div>
                                    <asp:DropDownList ID="ddlCamp" runat="server" class="custom-select"></asp:DropDownList>
                                </div>
                                </div>
                                <div class="form-group col-lg-3 col-xl-3">
                                    Template ID(Name)
                                <%--<div class="input-group">--%>
                                    <%--<div class="input-group-prepend">--%>
                                    <%--<div class="input-group-text">--%>
                                    <%--<asp:DropDownList runat="server" ID="ddltemplate" class="custom-select">
                                    </asp:DropDownList>--%>
                                    <div class="input-group" style="width: max-content;">

                                        <asp:ComboBox runat="server" ID="ddltemplate" Style="font-family: 'Courier New';" placeholder="Enter Template id" AutoPostBack="true" AutoCompleteMode="SuggestAppend">
                                            <asp:ListItem Value="-1" Text="--Select--" Selected="True"></asp:ListItem>
                                        </asp:ComboBox>


                                        <%--</div>--%>
                                        <%--</div>--%>

                                        <%--</div>--%>
                                        <%-- <asp:LinkButton runat="server" ID="Button3" OnClick="Button3_Click">
                                                            <i  class="fa fa-search mt-3 px-2"></i>
                                        </asp:LinkButton>--%>
                                    </div>
                                </div>



                                <div class="form-group col-lg-3 col-xl-3" style="margin-top: -0.5rem">
                                    <label for="#">Search mobile number :</label>
                                    <asp:TextBox ID="txtmob" runat="server" class="form-control" placeholder="XX-XXX-XXXX" MaxLength="10"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterMode="ValidChars"
                                        TargetControlID="txtmob" ValidChars="0123456789">
                                    </cc1:FilteredTextBoxExtender>
                                </div>

                            </div>

                            <div class="form-row">
                                <div class="col-md-6 mb-3">
                                    <asp:RadioButtonList runat="server" RepeatDirection="Horizontal" ID="rdblistselect" Style="margin-left: 20px">
                                        <asp:ListItem Value="D" Selected="True">Date Wise</asp:ListItem>
                                        <asp:ListItem Value="S">Single</asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xl-12 col-lg-12">
                            <!-- Start Card -->
                            <div class="card bg-primary border-light shadow-soft mb-4">
                                <div class="card-header py-3 bg-primary d-flex justify-content-between flex-wrap align-content-center border-bottom">
                                    <h6 class="m-0 font-weight-bold my-auto">SMS Reports</h6>
                                </div>
                                <div class="card-body">
                                    <div class="table-responsive">
                                        <asp:GridView ID="grv" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
                                            runat="server" Width="100%" CellPadding="10" BorderColor="#ede8e8" Class="table table-striped table-bordered dt-responsive nowrap dataTable-view">
                                            <Columns>
                                                <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Sr No">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Action">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="LinkButton1" runat="server" class="mx-1 btn btn-primary text-success"
                                                            OnClick="btnView_Click" data-toggle="tooltip" data-placement="top" title=""
                                                            data-original-title="View"> 
                                                <span class="text-success"> <i class="fas fa-file-csv"></i> </span></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="User ID">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUserId" runat="server" Text='<%#Eval("username")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Sender ID">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblsender" runat="server" Text='<%#Eval("senderid")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Submitted">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl1" runat="server" Text='<%#Eval("submitted")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Delivered">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblname" runat="server" Text='<%#Eval("delivered")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Failed">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl3" runat="server" Text='<%#Eval("failed")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Unknown">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl5" runat="server" Text='<%#Eval("unknown")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                            <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                                        </asp:GridView>

                                    </div>
                                </div>
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
            $('[id*=txtTo1]').attr('min', date1);

        });
    </script>

    <%--    <script>
            $(function () {
               $('.datepicker').datepicker({ defaultDate: -1, minDate:'-1d' });
            });
        </script>--%>

    <%--<script type="text/javascript">
        $(document).ready(function () {
            $('.datepicker').datepicker({ defaultDate: -1, minDate:'-1d' });
        });
    </script>--%>

    <%--<script type="text/javascript">
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

            function EndRequestHandler(sender, args) {
                $('.datepicker').datepicker({ dateFormat: 'dd-mm-yy' });

            }

        });
    </script>--%>
    <script type="text/javascript">  
        function view(senderid, fileid) {
            console.log(senderid);
            console.log(fileid);


            var today = new Date();
            var dd = String(today.getDate()).padStart(2, '0');
            var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
            var yyyy = today.getFullYear();

            var user = '<% =Session["User"].ToString() %>';
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

    <%--<script type="text/javascript">
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

            function EndRequestHandler(sender, args) {
                $('.datepicker').datepicker({ dateFormat: 'dd-mm-yy' });
            }

        });
    </script>--%>

    <script type="text/javascript"> 

        function text_changed_from1() {
            var d = document.getElementById("ContentPlaceHolder1_txtFrm1").value
            console.log(d);
            document.getElementById("ContentPlaceHolder1_hdntxtFrm1").value = d;
        }
        function text_changed_to1() {
            var d = document.getElementById("ContentPlaceHolder1_txtTo1").value
            console.log(d);
            document.getElementById("ContentPlaceHolder1_hdntxtTo1").value = d;

            var fromDate = $("#<%= txtFrm1.ClientID %>").datepicker("getDate");
            var toDate = $("#<%= txtTo1.ClientID %>").datepicker("getDate");
            // Check if From Date is greater than To Date
            if (fromDate > toDate) {
                alert("To Date cannot be lesser than From Date !");
                document.getElementById("ContentPlaceHolder1_txtTo1").value = '';
                return false;
            }
            return true;
        }
    </script>
   <script>
        var mindate = new Date();
       var today = new Date();
       var edate = new Date();
       edate.setDate(today.getDate() - 1);
        mindate.setDate(today.getDate() - 45);
            $('.datepicker').datepicker({
                endDate: edate,
                disableTouchKeyboard: false,
                todayHighlight: true,
                autoclose: true,
                startDate: mindate,
                format: 'yyyy-mm-dd',
            });
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
</asp:Content>
