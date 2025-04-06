<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="BlacklistNoEntry_A.aspx.cs" Inherits="eMIMPanel.BlacklistNoEntry_A" %>

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
                    <%--<li class="breadcrumb-item"><a href="#">Reports</a></li>--%>
                    <li class="breadcrumb-item active" aria-current="page">Blacklist Number Entry</li>
                </ol>
            </nav>
            <div class="row">
                <div class="col-12">
                    <div class="card card-body mb-4 bg-primary border-light shadow-soft">
                        <div class="form-row">
                            <div class="form-group col-lg-12 col-xl-12">
                                <asp:Label ID="Label1" runat="server" Style="font-weight: bold;" Text="Blacklist numbers are applicable only for campaign run through the Send SMS page."></asp:Label>
                            </div>
                        </div>
                        <div class="form-row">
                            <div class="form-group col-lg-2 col-xl-2">
                                <asp:Label ID="lbl1" runat="server" Text="Black List No Entry : "></asp:Label>
                            </div>
                            <div class="form-group col-lg-6 col-xl-6">
                                <asp:TextBox ID="txtmob" runat="server" class="form-control" MaxLength='50' onkeyDown="checkTextAreaMaxLength(this,event,'10');" TextMode="MultiLine" Rows="5" placeholder="Enter Mobile No." onkeyup="integersOnly(this); mobnumbcnt(); return true;"></asp:TextBox>
                                <small>Separate Number using Comma<,> or Enter</small> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <span class="font-weight-bold" style="font-size: smaller;">Number Count:</span>
                                <asp:Label ID="lblMobileCnt" runat="server" Style="font-size: smaller;"></asp:Label>
                            </div>
                        </div>
                        <div class="form-row">
                            <div class="form-group col-lg-2  col-xl-2"></div>
                            <div class="form-group col-lg-2  col-xl-2 mt-auto">
                                <asp:LinkButton OnClientClick=" return CheckDates();" ID="LnkbtnInsert" runat="server" OnClick="LnkbtnInsert_Click" class="btn btn-primary text-success btn-block"><i class="fas fa-search fa-sm text-success"></i>Add</asp:LinkButton>
                            </div>
                            <div class="form-group col-lg-2  col-xl-2 mt-auto">
                                <asp:LinkButton ID="btnClear" runat="server" class="btn btn-primary text-danger font-weight-bold btn-block" OnClick="btnClear_Click"> 
                                      <span><i class="fas fa-times"></i> Clear</span>
                                </asp:LinkButton>
                            </div>
                        </div>
                    </div>

                    <div class="card card-body mb-4 bg-primary border-light shadow-soft">
                        <!--End Loader -->
                        <div class="flex-fill text-right d-none">
                            <asp:LinkButton OnClientClick="CheckDates();" runat="server" ID="lnkDownload" OnClick="lnkDownload_Click" class="btn btn-mini">
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
                                    <asp:TemplateField HeaderText="Mobile Number">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMobileNo" runat="server" Text='<%#Eval("MobileNo")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="User ID">
                                        <ItemTemplate>
                                            <asp:Label ID="lbluserid" runat="server" Text='<%#Eval("userid")%>'></asp:Label>
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
    <script src="vendor/datatables/jquery.dataTables.min.js"></script>
    <script src="vendor/datatables/dataTables.bootstrap4.min.js"></script>
    <script src="vendor/datatables/dataTables.responsive.min.js"></script>
    <script src="vendor/datatables/responsive.bootstrap4.min.js"></script>

    <!--  Select-->
    <script src="vendor/select/bootstrap-select.min.js"></script>

    <!-- Page level custom scripts -->
    <script src="js/demo/datatables-demo.js"></script>
    <script src="js/demo/date-range-picker-demo.js"></script>
    <script>

        function checkTextAreaMaxLength(textBox, e, length) {
            var mLen = textBox["MaxLength"];
            if (null == mLen)
                mLen = length;
            var maxLength = parseInt(mLen);
            if (!integersOnly(e)) {
                if (textBox.value.length > maxLength - 1) {
                    if (window.event)//IE
                        e.returnValue = false;
                    else//Firefox
                        e.preventDefault();
                }
            }
        }

        function integersOnly(obj) {
            obj.value = obj.value.replace(/[^0-9,\r\n]/g, '');
        }
        function mobnumbcnt() {
            var s = document.getElementById("<%=txtmob.ClientID%>").value;
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
                }
                else
                    document.getElementById('<%= lblMobileCnt.ClientID %>').innerHTML = "";
                if (s != '' && x == 0)
                    document.getElementById('<%= lblMobileCnt.ClientID %>').innerHTML = "1";
            }
            else {
                document.getElementById('<%= lblMobileCnt.ClientID %>').innerHTML = "";
            }
        }

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
