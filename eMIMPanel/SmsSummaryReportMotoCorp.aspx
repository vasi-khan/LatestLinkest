<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="SmsSummaryReportMotoCorp.aspx.cs" Inherits="eMIMPanel.SmsSummaryReportMotoCorp" %>

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
        <nav aria-label="breadcrumb" class="my-3">
            <ol class="breadcrumb breadcrumb-info">
                <li class="breadcrumb-item"><a href="#">Home</a></li>
                <li class="breadcrumb-item"><a href="#">Reports</a></li>
                <li class="breadcrumb-item active" aria-current="page">Summary Reports</li>
            </ol>
        </nav>
        <!-- Start Row -->
        <div class="row">
            <div class="col-12">
                <!--  -->
                <div class="card card-body mb-4 bg-primary border-light shadow-soft">
                    <div class="form-row justify-content-between">
                        <div class="form-group col-lg-4 col-xl-5 d-none">
                            <label for="#">Download Delivery Report</label>
                            <div class="row d-none">
                                <div class="col-md-6">
                                    <asp:TextBox ID="txtFrm1" runat="server" onchange="javascript:text_changed_from1();" class="form-control datepicker" placeholder="From Date" autocomplete="off"></asp:TextBox>
                                    <asp:HiddenField ID="hdntxtFrm1" runat="server" />
                                </div>
                                <div class="col-md-6">
                                    <asp:TextBox ID="txtTo1" runat="server" onchange="javascript:text_changed_to1();" class="form-control datepicker" placeholder="To Date" autocomplete="off"></asp:TextBox>
                                    <asp:HiddenField ID="hdntxtTo1" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="form-group col-lg-3 col-xl-3" style="display: none;">
                            <label for="#">Search mobile number :</label>
                            <asp:TextBox ID="txtmob" runat="server" class="form-control" placeholder="XX-XXX-XXXX" MaxLength="10"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterMode="ValidChars"
                                TargetControlID="txtmob" ValidChars="0123456789">
                            </cc1:FilteredTextBoxExtender>
                        </div>
                        <div class="form-group col-lg-6 col-xl-4 m-0">
                        </div>
                        <div class="form-group col-lg-6 col-xl-4 m-0">
                            <asp:LinkButton ID="btnSumm" runat="server" OnClick="btnSumm_Click" class="btn btn-primary text-success btn-block"><i class="fas fa-sm text-success"></i> Day Summary Report</asp:LinkButton>
                        </div>
                    </div>
                </div>
                <!--  -->
            </div>
        </div>
        <!-- End Row -->

        <!-- Start Row -->
        <div class="row">
            <div class="col-xl-12 col-lg-12">
                <!-- Basic Card Example -->
                <div class="card bg-primary border-light shadow-soft mb-4">
                    <div class="card-header py-3 bg-primary d-flex flex-column flex-lg-row justify-content-lg-between flex-wrap align-content-lg-center">
                        <h6 class="m-0 font-weight-bold my-auto">Summary Reports</h6>
                    </div>
                    <div class="row">
                        <div class="col-md-3">
                            <div class="flex-fill mr-lg-3">From Date</div>
                            <div class="flex-fill">
                                <asp:TextBox ID="txtFrm" runat="server" onchange="javascript:text_changed_from();" class="form-control datepicker" placeholder="From Date" autocomplete="off"></asp:TextBox>
                                <asp:HiddenField ID="hdntxtFrm" runat="server" />
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="flex-fill mr-lg-3">To Date</div>
                            <div class="flex-fill">
                                <asp:TextBox ID="txtTo" runat="server" onchange="javascript:text_changed_to();" class="form-control datepicker" placeholder="To Date" autocomplete="off"></asp:TextBox>
                                <asp:HiddenField ID="hdntxtTo" runat="server" />
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="flex-fill mr-lg-3">Campaign</div>
                            <div class="flex-fill">
                                <asp:DropDownList ID="ddlCamp" runat="server" class="custom-select " ClientIDMode="Static"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="flex-fill mr-lg-3">Template ID(Name)</div>
                            <div class="flex-fill">
                                <asp:DropDownList ID="ddlTempIdAndName" runat="server" class="custom-select" ClientIDMode="Static"></asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-3">
                            <div class="flex-fill mr-lg-3">Group Location</div>
                            <div class="flex-fill">
                                <asp:DropDownList ID="ddlCategory" runat="server" class="custom-select" AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="flex-fill mr-lg-3">Location</div>
                            <div class="flex-fill">
                                <asp:DropDownList ID="ddlLocation" runat="server" class="custom-select" AutoPostBack="true" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged">
                                    <asp:ListItem Value="0">--All--</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="flex-fill mr-lg-3">SubLocation</div>
                            <div class="flex-fill">
                                <asp:DropDownList ID="ddlSubLocation" runat="server" class="custom-select" AutoPostBack="true" OnSelectedIndexChanged="ddlSubLocation_SelectedIndexChanged">
                                    <asp:ListItem Value="0">--All--</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="flex-fill mr-lg-3">Dealer</div>
                            <div class="flex-fill">
                                <asp:DropDownList ID="ddlDealerCode" runat="server" class="custom-select" ClientIDMode="Static">
                                    <asp:ListItem Value="0">--All--</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-3">
                            <div class="flex-fill mr-lg-3">Mobile No</div>
                            <div class="flex-fill">
                                <asp:TextBox ID="txtMobileNo" runat="server" class="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="flex-fill mr-lg-3"><br /></div>
                            <div class="right-view" style="text-align: right;">
                                <asp:LinkButton runat="server" ID="LinkButton1" OnClick="btnUpdate_Click" class="btn btn-block" Width="30%"> Show <i class="fas fa-eye" aria-hidden="true"></i></asp:LinkButton>
                            </div>
                        </div>
                        <div class ="col-md-3">
                            <div class="flex-fill mr-lg-3"><br /></div>
                            <asp:Button ID="btnDownload" runat="server" CssClass="btn btn-block fa fa-download" Text="Download" OnClick="btnDownload_Click1" />
                        </div>
                    </div>
                    <br />
                </div>
                <div class="accordion" id="accordionExample">
                    <div class="card">
                        <div class="card-header d-none" id="headingOne">
                            <h2 class="mb-0">
                                <button class="btn btn-link btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseOne" aria-expanded="true" aria-controls="collapseOne">
                                    SMS Summary
                                </button>
                            </h2>
                        </div>
                        <div id="collapseOne" class="collapse show" aria-labelledby="headingOne" data-parent="#accordionExample" runat="server">
                            <div class="card-body">
                                <div class="row">
                                    <!-- Area Chart -->
                                    <div class="col-xl-12 col-lg-12">
                                        <div id="divFileLoader" runat="server" style="display: none; text-align: center" class="form-group row">
                                            <h3>File uploading. Please wait...</h3>
                                            <img src="img/loading.gif" />
                                        </div>
                                        <div class="table-smsReport">
                                            <asp:GridView UseAccessibleHeader="true" ID="grv" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false" ShowFooter="true"
                                                runat="server" Width="100%" CellPadding="10" BorderColor="#ede8e8" Class="table table-striped table-bordered dt-responsive nowrap dataTable-view">
                                                <Columns>
                                                    <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Sr No">
                                                        <ItemTemplate>
                                                            <%#Container.DataItemIndex+1 %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Action">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="return Confirm();" OnClick="btnXL_Click" class="btn btn-datatable btn-icon btn-transparent-dark px-2 py-0" title="CSV UTF-8"><i class="fa fa-file-csv"></i></asp:LinkButton>
                                                            <asp:LinkButton ID="LinkButton2" OnClientClick=<%# "view('" + Eval("sender") + "','" + Eval("fileid") + "','" + Eval("reqsrc") + "'); return false;" %> runat="server" class="btn btn-datatable btn-icon btn-transparent-dark px-2 py-0" data-toggle="tooltip" data-placement="top" title="Details" Visible="false"><i class="far fa-file-alt"></i></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Submit Date Time" HeaderStyle-CssClass="text-wrap">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSubmitDate" runat="server" CssClass="text-wrap" Text='<%#Eval("submitdate")%>'></asp:Label>
                                                            <asp:HiddenField ID="hdnFileId" runat="server" Value='<%#Eval("fileid")%>'></asp:HiddenField>
                                                            <asp:HiddenField ID="hdnUserId" runat="server" Value='<%#Eval("userid")%>'></asp:HiddenField>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Group Location">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCategory" runat="server" Text='<%#Eval("CategoryName")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Loction">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLocation" runat="server" Text='<%#Eval("LocationName")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Sub Location">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSubLocation" runat="server" Text='<%#Eval("SubLocationName")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Dealer Code">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDealerCode" runat="server" Text='<%#Eval("DLRCODE")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Dealer Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDealerName" runat="server" Text='<%#Eval("DLRName")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Source">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSource" runat="server" Text='<%#Eval("reqsrc")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Sender ID">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSenderID" runat="server" Text='<%#Eval("sender")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Submitted">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSubmitted" runat="server" Text='<%#Eval("submitted")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Delivered">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDelivered" runat="server" Text='<%#Eval("delivered")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Failed">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFailed" runat="server" Text='<%#Eval("failed")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Unknown">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblUnknown" runat="server" Text='<%#Eval("unknown")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="File Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFileName" runat="server" Text='<%#Eval("filenm")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Message">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblMessage" runat="server" Text='<%#Eval("msg")%>'></asp:Label>
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
                    <div class="card d-none">
                        <div class="card-header" id="headingTwo">
                            <h2 class="mb-0">
                                <button class="btn btn-link btn-block text-left collapsed" type="button" data-toggle="collapse" data-target="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo">
                                    SMS Detail
                                </button>
                            </h2>
                        </div>
                        <div id="collapseTwo" class="collapse" aria-labelledby="headingTwo" data-parent="#accordionExample" runat="server">
                            <div class="card-body">
                                <div style="text-align: right;">
                                    <asp:LinkButton runat="server" ID="LinkButton3" OnClick="btnCloseDetail_Click" class="btn btn-mini">Close Detail</asp:LinkButton>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- End Row -->
    </main>

    <%--lnkDetail Link Button for ModalPopup as TargetControlID--%>
    <asp:LinkButton ID="lnkDetail" runat="server"></asp:LinkButton>

    <%--pnlPopUp_Detail Panel With Design--%>
    <asp:Panel ID="pnlPopUp_Detail" runat="server" CssClass="modal modalPopup" Style="display: none;">
        <div class="modal-dialog modal-xl modal-dialog-centered modal-dialog-scrollable">
            <div class="modal-content">
                <div class="modal-header">
                    <asp:Label ID="lblHeading" runat="server" CssClass="modal-title" Text="SMS Report Details"></asp:Label>
                </div>
                <div class="modal-body">
                    <div class="">
                        <div class="table-responsive">
                            <table class="table table-striped table-bordered dt-responsive nowrap" id="rpttable" width="100%" cellspacing="0">
                                <thead>
                                    <tr>
                                        <th>Sr. No</th>
                                        <th>MSG ID</th>
                                        <th>Mobile No</th>
                                        <th>Sender Id</th>
                                        <th>Submit Date Time</th>
                                        <th>Status Date Time</th>
                                        <th>Message</th>
                                        <th>Status</th>
                                        <th>Server Response</th>
                                    </tr>
                                </thead>
                                <tbody>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button id="btnCancel" runat="server" class="btn btn-primary" onclick="btnUpdate_Click">Close</button>
                    <button id="btnCancel2" runat="server" visible="false"></button>
                </div>
            </div>
        </div>
    </asp:Panel>

    <%--pnlPopUp_Detail Modal Popup Extender For pnlPopUp_Detail--%>
    <cc1:ModalPopupExtender ID="pnlPopUp_Detail_ModalPopupExtender" runat="server" PopupControlID="pnlPopUp_Detail"
        TargetControlID="lnkDetail" BehaviorID="mpeSMSDetailReport" CancelControlID="btnCancel"
        BackgroundCssClass="modalBackground">
    </cc1:ModalPopupExtender>

    <!-- Select2 CSS -->
    <link href="css/select2.min.css" rel="stylesheet" />
    <!-- Select2 -->
    <script src="js/select2.min.js"></script>
    <script>
        $("#ddlTempIdAndName").select2({
            allowClear: true
        });

        $("#ddlTempIdAndName").select2({
            allowClear: true
        });

        $("#ddlCamp").select2({
            allowClear: true
        });

        $("#ContentPlaceHolder1_ddlCategory").select2({
            allowClear: true
        });

        $("#ContentPlaceHolder1_ddlLocation").select2({
            allowClear: true
        });

        $("#ContentPlaceHolder1_ddlSubLocation").select2({
            allowClear: true
        });

        $("#ddlDealerCode").select2({
            allowClear: true
        });
    </script>

    <!-- Bootstrap core JavaScript-->
    <script src="vendor/jquery/jquery-3.5.1.min.js"></script>

    <script>
        $('#rpttable').dataTable({
            destroy: true,
            aaData: response.data
        });
    </script>

    <script type="text/javascript">  
        function view(senderid, fileid, reqsrc) {
            console.log(senderid);
            console.log(fileid);
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
            window.open("../ViewDetail.aspx?A=" + startDate + '$' + endDate + '$' + user + '$' + fileid + '$' + senderid + '$' + reqsrc + '$' + mob, '_blank');
            console.log("done");
            console.log("done222");
        }
    </script>

    <script type="text/javascript">
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

            function EndRequestHandler(sender, args) {
                $('.datepicker').datepicker({ dateFormat: 'dd-mm-yy' });
            }

        });
    </script>

    <script type="text/javascript"> 
        function text_changed_from() {
            var d = document.getElementById("ContentPlaceHolder1_txtFrm").value
            document.getElementById("ContentPlaceHolder1_hdntxtFrm").value = d;
        }
        function text_changed_to() {
            var d = document.getElementById("ContentPlaceHolder1_txtTo").value
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
</asp:Content>
