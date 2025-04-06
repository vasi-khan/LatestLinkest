<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="Click_Report_u1ShortURL.aspx.cs" Inherits="eMIMPanel.Click_Report_u1ShortURL" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .select2-container--default .select2-selection--single {
            background-color: #e6e7ee !important;
            border-radius: 4px;
            border: 1px solid #aaa !important;
            box-shadow: inset 2px 2px 5px #b8b9be, inset -3px -3px 7px #ffffff !important;
        }

        .select2-search--dropdown .select2-search__field {
            padding: 4px;
            width: 100%;
            box-sizing: border-box;
            background: #e6e7ee !important;
            border: 1px solid red;
        }
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

        input[type="checkbox"] {
            box-sizing: border-box;
            padding: 8px;
            width: 17px;
            height: 17px;
        }
    </style>
    <script>
        function Confirm() {
            if (confirm(' Are you sure to download.')) {
                return true;
            }
            else {
                return false;
            }
        }
    </script>
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
                    <li class="breadcrumb-item active" aria-current="page">Click Reports</li>
                </ol>
            </nav>

            <div class="row">
                <div class="col-xl-12 col-lg-12">
                    <div class="card card-body bg-primary border-light shadow-soft mb-4">
                        <div>
                            <div class="form-row">
                                <asp:RadioButton ID="rbOther" runat="server" AutoPostBack="true" Text="Date Wise" GroupName="Filter" Checked="true" Style="margin-left: 20px; margin-right: 50px" OnCheckedChanged="rbOther_CheckedChanged" />
                                <asp:RadioButton ID="rbTempWise" runat="server" AutoPostBack="true" Text="Template Id Wise" GroupName="Filter" Style="margin-left: 20px" OnCheckedChanged="rbTempWise_CheckedChanged" />
                            </div>
                            <div class="row" id="ditempwise" runat="server" visible="false">
                                <div class="col-md-2">
                                    <asp:Label runat="server"> Template ID</asp:Label>
                                </div>
                                <div class="col-md-3">
                                    <asp:DropDownList ID="ddlTempIdName" runat="server" class="custom-select" ClientIDMode="Static"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="col md-12"></div>
                            <div id="didatewise" runat="server">
                                <div class="row">
                                    <div class="col-md-2">
                                        <asp:Label runat="server"> URL Creation Date</asp:Label>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtFrm" runat="server" onchange="javascript:text_changed_from();" 
                                            class="form-control datepicker" placeholder="From Date" autocomplete="off" ClientIDMode="Static"></asp:TextBox>
                                        <asp:HiddenField ID="hdntxtFrm" runat="server" ClientIDMode="Static" />
                                    </div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtTo" runat="server" onchange="javascript:text_changed_to();" class="form-control datepicker" 
                                            placeholder="To Date" autocomplete="off" ClientIDMode="Static"></asp:TextBox>
                                        <asp:HiddenField ID="hdntxtTo" runat="server" ClientIDMode="Static" />
                                    </div>
                                    <div class="col-md-1">
                                        <asp:Button runat="server" OnClientClick="return CheckDates();" Text="GO" class="btn btn-success" ID="btnGO" OnClick="btnGO_Click" />
                                    </div>
                                    <div class="col-md-3">
                                        <asp:DropDownList ID="ddlShortUrl" runat="server" ClientIDMode="Static" class="custom-select">
                                            <asp:ListItem Text="--ALL--" Value="-1"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="row p-1">
                                <div class="col-md-2">
                                    <asp:Label runat="server"> Click Date</asp:Label>
                                </div>
                                <div class="col-md-3">
                                    <asp:TextBox ID="txtdatefromCLickDate" runat="server" onchange="javascript:text_changed_from2();"
                                        class="form-control datepicker" placeholder="From Date" autocomplete="off" ClientIDMode="Static"></asp:TextBox>
                                    <asp:HiddenField ID="hdntxtFrm2" runat="server" ClientIDMode="Static" />
                                </div>
                                <div class="col-md-3">
                                    <asp:TextBox ID="txtdatetoToCLickDate" runat="server" onchange="javascript:text_changed_to2();" 
                                        class="form-control datepicker" placeholder="To Date" autocomplete="off" ClientIDMode="Static"></asp:TextBox>
                                    <asp:HiddenField ID="hdntxtTo2" runat="server" ClientIDMode="Static" />
                                </div>
                                <div class="col-md-2">
                                    <div class="d-flex py-2" style="align-items: center; padding-left: 2rem;">
                                        <label class="form-check-label">Date Wise</label>
                                        <asp:CheckBox ID="Chkdatewise" CssClass="form-check-input" runat="server" Checked="true" />
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <asp:LinkButton runat="server" OnClientClick="return CheckDates1();" ID="btnUpdate" OnClick="btnUpdate_Click" class="btn btn-block">
                                            Show <i class="fas fa-eye" aria-hidden="true"></i>
                                    </asp:LinkButton>
                                </div>
                            </div>
                            <asp:HiddenField ID="h1" runat="server" />
                            <asp:HiddenField ID="h2" runat="server" />
                        </div>
                    </div>
                </div>
            </div>

            <!-- Content Row -->
            <div class="row">
                <div class="col-xl-12 col-lg-12">
                    <!-- Start Card-->
                    <div class="card bg-primary border-light shadow-soft mb-4">
                        <div class="card-header py-3 bg-primary d-flex justify-content-between flex-wrap align-content-center">
                            <h6 class="m-0 font-weight-bold my-auto"><i class="fas fa-hand-pointer"></i>URL Click</h6>
                        </div>
                        <div class="card-body">
                            <div class="table-responsive">
                                <asp:GridView ID="grv" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
                                    runat="server" Width="100%" CellPadding="10" BorderColor="#ede8e8"
                                    Class="table table-striped table-bordered dt-responsive nowrap dataTable-view">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Sr No">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                                <asp:HiddenField ID="hdnFrom" Value='<%#Eval("From")%>' runat="server" />
                                                <asp:HiddenField ID="hdnTo" Value='<%#Eval("To")%>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Action">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnXL" runat="server" OnClick="btnXL_Click" OnClientClick="return Confirm();" class="btn btn-datatable btn-icon btn-transparent-dark px-2 py-0" title="CSV UTF-8"><i class="fa fa-file-csv"></i></asp:LinkButton>
                                                <asp:LinkButton ID="LinkButton2" OnClientClick=<%# "view('" + Eval("URLID") + "','" + Eval("clkdate") + "','" + Eval("Country") + "','" + Eval("From") + "','" + Eval("To") + "'); return false;" %> runat="server" class="btn btn-datatable btn-icon btn-transparent-dark px-2 py-0" data-toggle="tooltip" data-placement="top" title="Details"><i class="far fa-file-alt"></i></asp:LinkButton>
                                                <asp:LinkButton ID="btnReTarget" runat="server" OnClick="btnReTarget_Click" class="btn btn-datatable btn-icon btn-transparent-dark px-2 py-0" data-toggle="tooltip" data-placement="top" title="ReTarget"><i class="fas fa-retweet"></i></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="URL Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblurlname" runat="server" Text='<%#Eval("URLName")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Long URL">
                                            <ItemTemplate>
                                                <asp:Label ID="lbllongurl" runat="server" Text='<%#Eval("LongURL")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Small URL">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl1" runat="server" Text='<%#Eval("smallURL")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Creation Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl2" runat="server" Text='<%#Eval("CreationDate")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Tot URL Sent">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl3" runat="server" Text='<%#Eval("No_of_url_sent")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="No of Hits">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNoOfHits" runat="server" Text='<%#Eval("No_Of_Hits")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="URL ID">
                                            <ItemTemplate>
                                                <asp:Label ID="lblurlid" runat="server" Text='<%#Eval("URLID")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Country Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCountry" runat="server" Text='<%#Eval("Country")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Country Code">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCountryCode" runat="server" Text='<%#Eval("CountryCode")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ClickDate">
                                            <ItemTemplate>
                                                <asp:Label ID="lblClickDate" runat="server" Text='<%#Eval("clkdate")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                    <!-- End Card -->
                </div>
            </div>
            <!-- End Row -->
        </div>
    </main>

    <%-- Modal popup for View -------->>> --%>
    <%--lnkDetail Link Button for ModalPopup as TargetControlID--%>
    <asp:LinkButton ID="lnkDetail" runat="server"></asp:LinkButton>

    <%--pnlPopUp_Detail Panel With Design--%>
    <asp:Panel ID="pnlPopUp_Detail" runat="server" CssClass="modal modalPopup" Style="display: none;">
        <div class="modal-dialog modal-xl modal-dialog-centered modal-dialog-scrollable">
            <div class="modal-content">
                <div class="modal-header">
                    <asp:Label ID="lblHeading" runat="server" CssClass="modal-title" Text="Click Report Details"></asp:Label>
                </div>
                <div class="modal-body">
                    <div class="">
                        <asp:GridView ID="grvDtl" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
                            runat="server" Width="100%" CellPadding="10"
                            BorderColor="#ede8e8" Class="table table-striped table-bordered dt-responsive nowrap dataTable-view">
                            <Columns>
                                <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Sr No">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Mobile">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMobile" runat="server" Text='<%#Eval("mobile")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="SMS Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblsmsdt" runat="server" Text='<%#Eval("smsDate")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Click Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblhitdate" runat="server" Text='<%#Eval("ClickDate")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Operator">
                                    <ItemTemplate>
                                        <asp:Label ID="lblip" runat="server" Text='<%#Eval("operator")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Circle">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl1" runat="server" Text='<%#Eval("Circle")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Platform">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl2" runat="server" Text='<%#Eval("Platform")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Browser">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl3" runat="server" Text='<%#Eval("Browser")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="From Mobile Device">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl4" runat="server" Text='<%#Eval("IsMobileDevice")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Mobile Device Manufacturer">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl5" runat="server" Text='<%#Eval("MobileDeviceManufacturer")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Mobile Device Model">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl6" runat="server" Text='<%#Eval("MobileDeviceModel")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <div class="modal-footer">
                    <button id="btnCancel" runat="server" class="btn btn-primary">Close</button>
                </div>
            </div>
        </div>
    </asp:Panel>

    <%--pnlPopUp_Detail Modal Popup Extender For pnlPopUp_Detail--%>
    <cc1:ModalPopupExtender ID="pnlPopUp_Detail_ModalPopupExtender" runat="server" PopupControlID="pnlPopUp_Detail"
        TargetControlID="lnkDetail" BehaviorID="mpeAddUpdateEmployee" CancelControlID="btnCancel"
        BackgroundCssClass="modalBackground">
    </cc1:ModalPopupExtender>

    <%-- Modal popup for View <<<-------- --%>

    <%-- Modal popup for ReTarget -------->>> --%>
    <%--lnkDetail Link Button for ModalPopup as TargetControlID--%>
    <asp:LinkButton ID="lnkDetail1" runat="server"></asp:LinkButton>

    <%--pnlPopUp_Detail Panel With Design--%>
    <asp:Panel ID="pnlPopUp_Detail1" runat="server" CssClass="modal modalPopup" Style="display: none;">
        <div class="modal-dialog modal-xl modal-dialog-centered modal-dialog-scrollable">
            <div class="modal-content">
                <div class="modal-header">
                    <asp:Label ID="Label1" runat="server" CssClass="modal-title" Text="ReTarget"></asp:Label>
                </div>
                <div class="modal-body">
                    <div class="form-group row">
                        <label for="exampleFormControlTextarea12" class="col-sm-2 col-form-label font-weight-bold">Sender ID</label>
                        <div class="col-md-6">
                            <label for="#" class="sr-only">Sender ID</label>
                            <asp:DropDownList ID="ddlSender" runat="server" class="custom-select"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label for="exampleFormControlTextarea22" class="col-sm-2 col-form-label font-weight-bold">Short URL</label>
                        <div class="col-sm-6">
                            <asp:DropDownList ID="ddlURL" runat="server" class="custom-select"></asp:DropDownList>
                        </div>
                        <div class="col-sm-2">
                            <asp:Button ID="btnInsertURL" runat="server" class="btn btn-primary" Text="Insert"></asp:Button>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">SMS Text</label>
                        <div class="col-sm-10">
                            <div id="divMsg" runat="server" style="pointer-events: all;">
                                <asp:TextBox ID="txtMsg" runat="server" TextMode="MultiLine" class="form-control" Rows="7" onkeyup="smscnt(); return true;"></asp:TextBox>
                            </div>
                            <div class="d-flex justify-content-start">
                                <p class="mb-1 mr-5">
                                </p>
                                <p class="mb-1 mr-5">
                                    <span class="font-weight-bold">SMS Count :</span>
                                    <asp:Label ID="lblsmscnt" runat="server" Text="0"></asp:Label>
                                </p>
                                <p class="mb-1">
                                    <asp:Label ID="lblUniCode" runat="server" Text="" Style="color: red; font-weight: bolder;"></asp:Label>
                                </p>
                            </div>
                        </div>
                    </div>
                    <div id="divTempsms" runat="server">
                        <div class="form-group row">
                            <label for="exampleFormControlTextarea23" class="col-sm-2 col-form-label font-weight-bold">Template ID</label>
                            <div class="col-sm-10">
                                <div id="div8" runat="server" style="pointer-events: all;">
                                    <asp:DropDownList ID="ddlTempID" runat="server" class="custom-select"></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">Template SMS:</label>
                            <div class="col-sm-10">
                                <div id="div9" runat="server" style="pointer-events: all;">
                                    <asp:TextBox ID="txtTempSMS" runat="server" ReadOnly="true" class="form-control" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnSend" runat="server" class="btn btn-primary" Text="Send" OnClientClick="return ConfirmSubmit();"></asp:Button>
                    <button id="btnCancel1" runat="server" class="btn btn-primary">Close</button>
                </div>
            </div>
        </div>
    </asp:Panel>

    <%--pnlPopUp_Detail Modal Popup Extender For pnlPopUp_Detail--%>
    <cc1:ModalPopupExtender ID="pnlPopUp_Detail1_ModalPopupExtender" runat="server" PopupControlID="pnlPopUp_Detail1"
        TargetControlID="lnkDetail1" BehaviorID="mpeAddUpdateEmployee1" CancelControlID="btnCancel1"
        BackgroundCssClass="modalBackground">
    </cc1:ModalPopupExtender>

    <%-- Modal popup for ReTarget <<<-------- --%>

    <!-- Select2 CSS -->
    <link href="css/select2.min.css" rel="stylesheet" />
    <!-- Select2 -->
    <script src="js/select2.min.js"></script>
    <script>
        $("#ddlTempIdName").select2({
            allowClear: true
        });

        $("#ddlTempIdName").select2({
            allowClear: true
        });
    </script>

    <script type="text/javascript">  
        function view(UrlId, ClkDate, CountryCode, From, To) {
            console.log(UrlId);
            console.log(ClkDate);
            console.log(From);
            console.log(To);
            $('#rpttable').dataTable().fnDestroy();   //Change on 05 Aug 2022

            var today = new Date();
            var dd = String(today.getDate()).padStart(2, '0');
            var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
            var yyyy = today.getFullYear();

            var user = '<% =Session["UserID"].ToString() %>';
            var startDate = yyyy + '-' + mm + '-' + dd;
            var endDate = startDate + ' 23:59:59';
            var mob = $('#ContentPlaceHolder1_txtMobileNo').val();
            console.log(startDate);
            console.log(endDate);
            console.log(user);
            var Checked = $("#Chkdatewise").is(":Checked");
            window.open("../ViewDetailsClickReport.aspx?A=" + UrlId + '$' + ClkDate + '$' + CountryCode + '$' + From + '$' + To + '$' + Checked, '_blank');
        }
    </script>

    <script type="text/javascript"> 
        function text_changed_from() {
            var d = document.getElementById("txtFrm").value
            console.log(d);
            document.getElementById("hdntxtFrm").value = d;
        }
        function text_changed_to() {
            var d = document.getElementById("txtTo").value
            console.log(d);
            document.getElementById("hdntxtTo").value = d;
        }


        function text_changed_from2() {
            var d = document.getElementById("txtdatefromCLickDate").value
            console.log(d);
            document.getElementById("hdntxtFrm2").value = d;
        }

        function text_changed_to2() {
            var d = document.getElementById("txtdatetoToCLickDate").value
            console.log(d);
            document.getElementById("hdntxtTo2").value = d;
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
            var fromDate = $("#<%= txtFrm.ClientID %>").datepicker("getDate");
            var toDate = $("#<%= txtTo.ClientID %>").datepicker("getDate");

            if ((fromDate == null || toDate == null) || (fromDate === "" || toDate === "")) {
                alert("From date and To Date cannot be empty");
                return false;
            }

            if (fromDate > toDate) {
                alert("From Date cannot be greater than To Date");
                return false;
            }

            return true;
        }

        function CheckDates1() {
            var fromDate = $("#<%= txtdatefromCLickDate.ClientID %>").datepicker("getDate");
            var toDate = $("#<%= txtdatetoToCLickDate.ClientID %>").datepicker("getDate");

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
    <script>

        $(function () {
            // Get the current date
            var currentDate = new Date();

            var minDate = new Date();
            minDate.setMonth(currentDate.getMonth() - 6);

            var formattedMinDate = minDate.toISOString().split('T')[0];

            // Get all elements with class "date_group"
            var dateInputs = document.getElementsByClassName("date_group");

            // Iterate through the collection and set attributes for each element
            for (var i = 0; i < dateInputs.length; i++) {
                dateInputs[i].setAttribute("min", formattedMinDate);
                dateInputs[i].setAttribute("max", currentDate.toISOString().split('T')[0]);
            }
        });
    </script>
    <script>
        function ConfirmSubmit() {
            var s = document.getElementById("<%=txtMsg.ClientID%>").value;
            if (s != '') {
                var i = 0;
                var ln = s.length;
                for (var k = 0, n = s.length; k < n; k++) {
                    if (s.charAt(k) == "~" || s.charAt(k) == "|" || s.charAt(k) == "{" || s.charAt(k) == "}" || s.charAt(k) == "[" || s.charAt(k) == "]" || s.charAt(k) == "^" || s.charAt(k) == "\\") {
                        ln = ln + 1;
                    }
                }

                if (document.getElementById('<%= ddlURL.ClientID %>').value != "0") ln = ln + 2;
                i = getLength(ln);

                var y = 0;
                for (var k = 0, n = s.length; k < n; k++) {
                    if (s.charCodeAt(k) > 126) { y = 1; }
                }
                if (y == 1) {
                    ln = s.length;
                    i = getUniCodeLength(ln);
                }
                if (i > 1) {
                    if (confirm('This message will cost \n\n' + i + ' SMS per message. \n\nDo you want to continue ? ')) {
                        console.log('ok clicked.');
                        if (confirm('This message will cost \n\n' + i + ' SMS per message. \n\n Are you sure you want to continue ? ')) {
                            return true;
                        }
                        else {
                            return false;
                        }
                    }
                    else {
                        return false;
                    }
                }
                else
                    return true;
            }
            else
                return true;
        }

        function getLength(ln) {
            var i = 0;
            if (ln >= 1) i = 1;
            if (ln > 160) i = 2;
            if (ln > 306) i = 3;
            if (ln > 459) i = 4;
            if (ln > 612) i = 5;
            if (ln > 765) i = 6;
            if (ln > 918) i = 7;
            if (ln > 1071) i = 8;
            if (ln > 1224) i = 9;
            if (ln > 1377) i = 10;
            if (ln > 1530) i = 11;
            if (ln > 1683) i = 12;
            return i;
        }

        function getUniCodeLength(ln) {
            var i = 0;
            if (ln >= 1) i = 1;
            if (ln > 70) i = 2;
            if (ln > 134) i = 3;
            if (ln > 201) i = 4;
            if (ln > 268) i = 5;
            if (ln > 335) i = 6;
            if (ln > 402) i = 7;
            if (ln > 469) i = 8;
            if (ln > 536) i = 9;
            if (ln > 603) i = 10;
            return i;
        }

        function smscnt() {
            var s = document.getElementById("<%=txtMsg.ClientID%>").value;
            if (s != '') {
                var i = 0;
                var ln = s.length;
                for (var k = 0, n = s.length; k < n; k++) {
                    if (s.charAt(k) == "~" || s.charAt(k) == "|" || s.charAt(k) == "{" || s.charAt(k) == "}" || s.charAt(k) == "[" || s.charAt(k) == "]" || s.charAt(k) == "^") {
                        ln = ln + 1;
                    }
                }
                if (document.getElementById('<%= ddlURL.ClientID %>').value != "0") ln = ln + 2;
                i = getLength(ln);
                document.getElementById('<%= lblUniCode.ClientID %>').innerHTML = "";

                document.getElementById('<%= lblsmscnt.ClientID %>').innerHTML = "No. of Char: " + ln + ". <br />No. of SMS: " + i.toString();
                var y = 0;
                for (var k = 0, n = s.length; k < n; k++) {
                    if (s.charCodeAt(k) > 126) { y = 1; }
                }
                if (y == 1) {
                    ln = s.length;
                    i = getUniCodeLength(ln);
                    document.getElementById('<%= lblUniCode.ClientID %>').innerHTML = "UNICODE : YES";

                    document.getElementById('<%= lblsmscnt.ClientID %>').innerHTML = "No. of Char: " + ln + ". <br />No. of SMS: " + i.toString();
                }
            }
            else
                document.getElementById('<%= lblsmscnt.ClientID %>').innerHTML = "";
        }
    </script>
</asp:Content>