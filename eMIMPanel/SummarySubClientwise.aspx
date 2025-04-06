<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="SummarySubClientwise.aspx.cs" Inherits="eMIMPanel.SummarySubClientwise" %>
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
         <script>
 function Confirm()
 {
     if(confirm(' Are you sure to download.'))
        {
            return true;
        }
        else
        {
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
                    <li class="breadcrumb-item active" aria-current="page">Insurance company wise Report</li>
                </ol>
            </nav>

            <!-- Start Row -->
            <div class="row">
                <div class="col-12">
                    <!--  -->
                    <div class="card card-body mb-4 bg-primary border-light shadow-soft">
                        <div class="form-row justify-content-between px-2">

                          <%--<h6 class="m-0 font-weight-bold my-auto">Insurance company wise Report</h6>--%>




                            <div class="row">
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtFrm" runat="server" onchange="javascript:text_changed_from();" class="form-control datepicker" placeholder="From Date" autocomplete="off"></asp:TextBox>
                                        <asp:HiddenField ID="hdntxtFrm" runat="server" />
                                    </div>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtTo" runat="server" onchange="javascript:text_changed_to();" class="form-control datepicker" placeholder="To Date" autocomplete="off"></asp:TextBox>
                                        <asp:HiddenField ID="hdntxtTo" runat="server" />
                                    </div>
                                </div>






                            <div class="d-flex align-items-center my-3 my-lg-0" style="display:none;">
                                <div class="flex-fill mr-lg-3"> </div>  
                                <div class="flex-fill">
                                    <asp:DropDownList ID="ddlCamp" visible="false" runat="server" class="custom-select"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="d-flex align-items-center my-3 my-lg-0">
                                <div class="flex-fill mr-lg-3"> Mobile No</div>  
                                <div class="flex-fill">
                                    <asp:TextBox ID="txtMobileNo" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="right-view">
                                  
                                <asp:LinkButton runat="server" ID="LinkButton1" OnClick="btnUpdate_Click" class="btn btn-block"> Show <i class="fas fa-eye" aria-hidden="true"></i></asp:LinkButton>
                                <asp:HiddenField ID="h1" runat="server" />
                                <asp:HiddenField ID="h2" runat="server" />
                            </div>

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
                            <div class="form-group col-lg-6 col-xl-4 mb-2 mb-lg-0" runat="server" visible="false">
                                <asp:LinkButton ID="btnsearch" runat="server" OnClick="btnsearch_Click" class="btn btn-primary text-success btn-block"><i class="fas fa-download fa-sm text-success"></i> Download Delivery Report</asp:LinkButton>
                            </div>
                            <div class="form-group col-lg-6 col-xl-4 m-0" runat="server" visible="false">
                                <asp:LinkButton ID="btnSumm" runat="server" OnClick="btnSumm_Click" class="btn btn-primary text-success btn-block"><i class="fas fa-sm text-success"></i> Show Summary Report</asp:LinkButton>
                            </div>
                            <div class="form-group col-lg-2  col-xl-2 mt-lg-auto d-none">
                                <%--<asp:LinkButton ID="btnXLdw" runat="server" OnClick="btnXLdw_Click"><i class="fas fa-file-excel fa-2x text-success"></i></asp:LinkButton>--%>
                                <%--<a href="#"><i class="fas fa-file-csv fa-2x text-danger"></i></a>
                                <a href="#"><i class="far fa-file-alt fa-2x text-secondary"></i></a>--%>
                            </div>
                        </div>
                    </div>
                    <!--  -->

                    <div class="card card-body mb-4 bg-primary border-light shadow-soft d-none">
                        <div class="table-responsive ttd">
                            <asp:GridView ID="grv2" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false" 
                                runat="server" Width="100%" CellPadding="10" BorderColor="#ede8e8" Class="table table-striped table-bordered dt-responsive nowrap dataTable-view">
                                <Columns>
                                    <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Sr No">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton1" runat="server" OnClick="btnDownLoad_Click" class="btn btn-datatable btn-icon btn-transparent-dark px-2 py-0" data-toggle="tooltip" data-placement="top" title="CSV"><i class="far fa-file-csv"></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Submit Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblsubmitdt" runat="server" Text='<%#Eval("submitdate")%>'></asp:Label>
                                            <%--<asp:HiddenField ID="hdnfrom" runat="server" Value='<%#Eval("fileid")%>'></asp:HiddenField>--%>
                                            <asp:HiddenField ID="hdnto" runat="server" Value='<%#Eval("userid")%>'></asp:HiddenField>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sender ID">
                                        <ItemTemplate>
                                            <asp:Label ID="lblsender" runat="server" Text='<%#Eval("sender")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Submitted">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl3" runat="server" Text='<%#Eval("submitted")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Delivered">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl4" runat="server" Text='<%#Eval("delivered")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Failed">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl5" runat="server" Text='<%#Eval("failed")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Unknown">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl6" runat="server" Text='<%#Eval("unknown")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>                                
                            </asp:GridView>
                        </div>
                     </div>

                 </div>
            </div>
            <!-- End Row -->

            <!-- Start Row -->
            <div class="row justify-content-center d-none">
                <div class="col-12">
                    <!--Accordion-->
                    <div class="accordion shadow-soft rounded mb-4">
                        <div class="card card-sm card-body bg-primary border-light mb-0">
                            <a href="#panel-4" data-target="#panel-4" class="accordion-panel-header" data-toggle="collapse" role="button" aria-expanded="false" aria-controls="panel-1">
                                <span class="icon-title h6 mb-0 font-weight-bold"><span class="fas fa-chevron-down"></span>Todays sent count info</span>
                                <span class="icon"><span class="fas fa-plus"></span></span>
                            </a>
                            <div class="collapse" id="panel-4">
                                <div class="pt-3">
                                    <!-- Card-1 -->
                                    <div class="card bg-primary shadow-inset border-light h-100 py-2">
                                        <div class="card-body py-2">
                                           <p><strong>Delivery Status :</strong></p>
                                            <ul class="mb-0">
                                                <li>Delivered : Delivered to Handset </li>
                                                <li>Undelivered : Not Delivered </li>
                                                <li>Unknown : SMS may be delivered but report not received from telecom operator(Du/Etisalat) </li>
                                            </ul>
                                        </div>
                                    </div>
                                    <!-- Card-2 -->
                                    <div class="card mb-0 border-left-danger">
                                        <div class="card-body py-2">
                                            <p><span class="font-weight-bold text-danger">Note :</span> We show exactly those reports which are being provided by different operators. At times the SMS gets delivered but the report shows unknown. These are those SMS whose delivery reports are not provided by the operator but they may have been actually delivered.</p>
                                            <ul class="mb-0">
                                                <li>System will send DELIVERED/UNDELIVERED/UNKNOWN status only. Failure details is not supported as per system design. Single customer complain should be raised by customer to support. </li>
                                                <li>NO DLR received: Confirm that full packet is sent or reaching server or not. </li>
                                                <li>Confirm Validity period is 12 hours. </li>
                                                <li>Verify schedule delivery time and other configuration. </li>
                                                <li>In case successful DLR but no content showing in handset: It’s the handset which sent ACK to the network regarding SMS receipt. </li>
                                                <li>Ensure to remove inactive/invalid MSISDNs from recipients list. </li>
                                            </ul>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <!--End of Accordion-->
                </div>
            </div>
            <!-- End Row -->

            <!-- Start Row -->
            <div class="row">
                <div class="col-xl-12 col-lg-12">
                    <!-- Basic Card Example -->
                    <div class="card bg-primary border-light shadow-soft mb-4">
                        <div class="card-header py-3 bg-primary d-flex flex-column flex-lg-row justify-content-lg-between flex-wrap align-content-lg-center">

                                <%--<div class="col-md-11"></div>--%>
                            <div class="col-md-2">From Date :</div>
                            <div class="col-md-2">
                                 <asp:Label ID="lblfromdate" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="col-md-2">To Date :</div>
                            <div class="col-md-2">
                                 <asp:Label ID="lbltodate" runat="server" Text=""></asp:Label>
                            </div>
                            <%--<div class="col-md-5"></div>--%>
                                <div class="col-md-1 text-end float-right">
                                       <asp:LinkButton ID="lnkalldownload" runat="server" OnClientClick="return Confirm();" OnClick="lnkalldownload_Click" class="btn btn-datatable btn-icon btn-transparent-dark px-3" title="CSV UTF-8"><i class="fa fa-file-csv"></i></asp:LinkButton>
                                </div>
                           
                         
                       

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
                                                                    <asp:LinkButton ID="LinkButton2" OnClientClick=<%# "view('" + Eval("sender") + "','" + Eval("fileid") + "','" + Eval("reqsrc") + "','"+Eval("SubClientID")+"'); return false;" %> runat="server" class="btn btn-datatable btn-icon btn-transparent-dark px-2 py-0" data-toggle="tooltip" data-placement="top" title="Details"><i class="far fa-file-alt"></i></asp:LinkButton>
                                                                    <asp:HiddenField ID="hdnFileId" runat="server" Value='<%#Eval("fileid")%>'></asp:HiddenField>
                                                                    <asp:HiddenField ID="hdnUserId" runat="server" Value='<%#Eval("userid")%>'></asp:HiddenField>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Submit Date Time" HeaderStyle-CssClass="text-wrap" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblmobile" runat="server" CssClass="text-wrap" Text='<%#Eval("submitdate")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Source">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl0" runat="server" Text='<%#Eval("reqsrc")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Sender ID">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblsender" runat="server" Text='<%#Eval("sender")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="SubClient ID">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSubClientID" runat="server" Text='<%#Eval("SubClientCode")%>'></asp:Label>
                                            <asp:HiddenField ID="hfSubClientID" runat="server" Value='<%#Eval("SubClientID")%>'></asp:HiddenField>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Submitted">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl3" runat="server" Text='<%#Eval("submitted")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Delivered">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl4" runat="server" Text='<%#Eval("delivered")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Failed">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl5" runat="server" Text='<%#Eval("failed")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Unknown">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl6" runat="server" Text='<%#Eval("unknown")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="File Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl1" runat="server" Text='<%#Eval("filenm")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Message">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl7" runat="server" Text='<%#Eval("msg")%>'></asp:Label>
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

         </div>
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

    <!-- Bootstrap core JavaScript-->
    <script src="vendor/jquery/jquery-3.5.1.min.js"></script>

    <script>
        $('#rpttable').dataTable({
            destroy: true,
            aaData: response.data
        });
    </script>

    <script type="text/javascript">  
        function view(senderid, fileid,reqsrc,SubClientCode) {
            console.log(senderid);
            console.log(fileid);
               $('#rpttable').dataTable().fnDestroy();   //Change on 05 Aug 2022

            var today = new Date();
            var dd = String(today.getDate()).padStart(2, '0');
            var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
            var yyyy = today.getFullYear();

            var user = '<% =Session["UserID"].ToString() %>';

            //var startDate = yyyy + '-' + mm + '-' + dd;
            //var endDate = startDate + ' 23:59:59';

            var startDate = '<%= Session["startdate"] %>';
            var endDate = '<%= Session["enddate"] %>';


var mob = $('#ContentPlaceHolder1_txtMobileNo').val();
            console.log(startDate);
            console.log(endDate);
            console.log(user);

            //$.ajax({
            //    type: "POST",
            //    dataType: "json",
            //    url: "WebService.asmx/GetSMSReportDetailUserNew",
            //    data: { dater: startDate + '$' + endDate + '$' + user + '$' + fileid + '$' + senderid + '$' + reqsrc},
            //    success: function (data) {
                  
            //        var datatableVariable = $('#rpttable').DataTable({
            //            data: data,
            //            bRetrieve: true,
            //            columns: [
            //                { 'data': 'sln' },
            //                { 'data': 'msgid' },
            //                { 'data': 'mobile' },
            //                { 'data': 'senderid' },
            //                { 'data': 'senttime' },
            //                { 'data': 'dlrtime' },
            //                { 'data': 'msgtext' },
            //                { 'data': 'dlrstat' },
            //                { 'data': 'dlrresp' }
            //            ]
            //        });
            //    }
            //});
           window.open("../ViewDetailSubClient.aspx?A="+startDate + '$' + endDate + '$' + user + '$' + fileid + '$' + senderid + '$' + reqsrc+'$'+mob+'$'+SubClientCode,'_blank');
            console.log("done");
         
           // $find('mpeSMSDetailReport').show();
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

<!-- <script>
    $(document).ready( function() {
  $( '#rpttable' ).dataTable( {
   "fnRowCallback": function( nRow, aData, iDisplayIndex, iDisplayIndexFull ) {
     // Bold the grade for all 'A' grade browsers
     if ( aData[4] == "A" )
     {
       $('td:eq(4)', nRow).html( '<b>A</b>' );
     }
   }
 } );
 } );
</script> -->
</asp:Content>
