<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="WA_RCS_Report.aspx.cs" Inherits="eMIMPanel.WA_RCS_Report" %>

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
    <%--   <asp:UpdatePanel runat="server" id="up1">
                        <ContentTemplate>--%>
    <main>
        <div class="container-fluid">

            <nav aria-label="breadcrumb" class="my-3">
                <ol class="breadcrumb breadcrumb-info">
                    <li class="breadcrumb-item"><a href="#">Home</a></li>
                    <li class="breadcrumb-item"><a href="#">Reports</a></li>
                    <li class="breadcrumb-item active" aria-current="page">Whatsapp / RCS Reports</li>
                </ol>
            </nav>

            <!-- Start Row -->
            <div class="row- d-none">
                <div class="col-12">
                    <!--  -->
                    <div class="card card-body mb-4 bg-primary border-light shadow-soft">
                        <div class="form-row justify-content-between">
                            <div class="form-group col-lg-4 col-xl-5 d-none">
                                <label for="#">Download Delivery Report</label>
                                
                            </div>
                            <div class="form-group col-lg-3 col-xl-3" style="display: none;">
                                <label for="#">Search mobile number :</label>
                                <asp:TextBox ID="txtmob" runat="server" class="form-control" placeholder="XX-XXX-XXXX" MaxLength="10"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterMode="ValidChars"
                                    TargetControlID="txtmob" ValidChars="0123456789">
                                </cc1:FilteredTextBoxExtender>
                            </div>
                            <div class="form-group col-lg-6 col-xl-4 mb-2 mb-lg-0">
                                <asp:LinkButton ID="btnsearch" runat="server" OnClick="btnsearch_Click" class="btn btn-primary text-success btn-block"><i class="fas fa-download fa-sm text-success"></i> Download Delivery Report</asp:LinkButton>
                            </div>
                            <div class="form-group col-lg-6 col-xl-4 m-0">
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
                                            <asp:LinkButton ID="LinkButton1" runat="server" OnClick="btnDownLoad_Click" class="btn btn-datatable btn-icon btn-transparent-dark px-2 py-0" data-toggle="tooltip" data-placement="top" title="XLS"><i class="fas fa-download"></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Submit Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblsubmitdt" runat="server" Text='<%#Eval("submitdate")%>'></asp:Label>
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
                            <%--<h6 class="m-0 font-weight-bold my-auto">WA/RCS Reports</h6>--%>

                            <div class="d-flex align-items-center my-3 my-lg-0 d-none" runat="server" visible="false">
                                <div class="flex-fill mr-lg-3"> Campaign</div>  
                                <div class="flex-fill">
                                    <asp:DropDownList ID="ddlCamp" runat="server" class="custom-select">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>


                            <div class="row">
  <div class="col-sm-4">
                                        <div class="btn-group btn-group-toggle btn-block mb-3 mb-lg-0" data-toggle="buttons">
                                            <label class="btn text-secondary active">
                                                  <img src="assets/icon/whatsapp-business.svg" alt="whatsapp-business" width="28px">
                                                <%--<input type="radio" name="videoUp1" value="videoUp1" checked>--%>
                                                <asp:RadioButton ID="rdbWhatsapp" OnCheckedChanged="rdbWhatsapp_CheckedChanged" runat="server" AutoPostBack="true" GroupName="vi" Checked="true"/>
                                                Whatsapp
                                            </label>
                                       <label class="btn text-secondary"><%--<i class="fab fa-google"></i>--%><img src="assets/icon/google_icon.svg" alt="google_icon.svg" width="20px">
                                                <%--<input type="radio" name="ImgUp1" value="ImgUp1">--%>
                                                <asp:RadioButton ID="rdbRCS" runat="server" OnCheckedChanged="rdbWhatsapp_CheckedChanged" AutoPostBack="true" GroupName="vi"/>
                                                RCS
                                            </label>
                                       </div>
                                    </div>

                               <%--  <div class="row d-none">--%>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtFrm1" runat="server" onchange="javascript:text_changed_from1();" class="form-control datepicker" placeholder="From Date" autocomplete="off"></asp:TextBox>
                                        <asp:HiddenField ID="hdntxtFrm1" runat="server" />
                                    </div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtTo1" runat="server" onchange="javascript:text_changed_to1();" class="form-control datepicker" placeholder="To Date" autocomplete="off"></asp:TextBox>
                                        <asp:HiddenField ID="hdntxtTo1" runat="server" />
                                    </div>
                                <%--</div>--%>

                            <div class="right-view">
                                <asp:LinkButton runat="server" ID="LinkButton1" OnClick="btnUpdate_Click" class="btn btn-block"> Show <i class="fas fa-eye" aria-hidden="true"></i></asp:LinkButton>
                                <asp:HiddenField ID="h1" runat="server" />
                                <asp:HiddenField ID="h2" runat="server" />
                            </div>
                            </div>
                            
                        </div>
                        
                    </div>
                      
                    

                       <div class="card bg-primary border-light shadow-soft mb-4">
                        <div class="card-header py-3 bg-primary d-flex flex-column flex-lg-row justify-content-lg-between flex-wrap align-content-lg-center">
                            <div>
<i class="fas fa-file-alt"></i> Report
                            </div>
        
                          </div>
                           <div class="card-body">
                                <!-- Loader -->
                                                <div class="o-page-loader">
                                                    <div class="o-page-loader--content">
                                                        <div class="o-page-loader--spinner"></div>
                                                        <div class="o-page-loader--message">
                                                            <span>Loading...</span>
                                                        </div>
                                                    </div>
                                                </div>
                                                <!--End Loader -->
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
                                                                    <asp:LinkButton ID="LinkButton1" runat="server" OnClick="btnXL_Click" class="btn btn-datatable btn-icon btn-transparent-dark px-2 py-0" data-toggle="tooltip" data-placement="top" title="XLS"><i class="fas fa-download"></i></asp:LinkButton>
                                                                    <asp:LinkButton ID="LinkButton2" OnClientClick=<%# "view('" + Eval("inserttime") + "'); return false;" %> runat="server" class="btn btn-datatable btn-icon btn-transparent-dark px-2 py-0" data-toggle="tooltip" data-placement="top" title="Details"><i class="far fa-file-alt"></i></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Submit Date" HeaderStyle-CssClass="text-wrap">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblinserttime" runat="server" style="display:none" Text='<%#Eval("inserttime")%>'></asp:Label>
                                                                    <asp:Label ID="lblmobile" runat="server" CssClass="text-wrap" Text='<%#Eval("submitdate")%>'></asp:Label>
                                                                    <%--<asp:HiddenField ID="hdnFileId" runat="server" Value='<%#Eval("fileid")%>'></asp:HiddenField>
                                                                    <asp:HiddenField ID="hdnUserId" runat="server" Value='<%#Eval("userid")%>'></asp:HiddenField>--%>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                           <%-- <asp:TemplateField HeaderText="Source">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl0" runat="server" Text='<%#Eval("reqsrc")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Sender ID">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblsender" runat="server" Text='<%#Eval("sender")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>--%>
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
                                                           <%-- <asp:TemplateField HeaderText="File Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl1" runat="server" Text='<%#Eval("filenm")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Message">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl7" runat="server" Text='<%#Eval("msg")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>--%>
                                                        </Columns>
                                                        <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                                                    </asp:GridView>
                                                
                                                    <asp:GridView UseAccessibleHeader="true" ID="grv_rcs" Visible="false" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false" ShowFooter="true"
                                                        runat="server" Width="100%" CellPadding="10" BorderColor="#ede8e8" Class="table table-striped table-bordered dt-responsive nowrap dataTable-view">
                                                        <Columns>
                                                            <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Sr No">
                                                                <ItemTemplate>
                                                                    <%#Container.DataItemIndex+1 %>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="LinkButton1" runat="server" OnClick="btnXL_Click" class="btn btn-datatable btn-icon btn-transparent-dark px-2 py-0" data-toggle="tooltip" data-placement="top" title="XLS"><i class="fas fa-download"></i></asp:LinkButton>
                                                                    <asp:LinkButton ID="LinkButton2" OnClientClick=<%# "view('" + Eval("inserttime") + "'); return false;" %> runat="server" class="btn btn-datatable btn-icon btn-transparent-dark px-2 py-0" data-toggle="tooltip" data-placement="top" title="Details"><i class="far fa-file-alt"></i></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Submit Date" HeaderStyle-CssClass="text-wrap">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblinserttime" runat="server" style="display:none" Text='<%#Eval("inserttime")%>'></asp:Label>
                                                                    <asp:Label ID="lblmobile" runat="server" CssClass="text-wrap" Text='<%#Eval("submitdate")%>'></asp:Label>
                                                                    
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
                                                        <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                                                    </asp:GridView>
                                                </div>
                           </div>
                       </div>
                        
            </div>
            <!-- End Row -->

         </div>
            </div>
    </main>

    <%--lnkDetail Link Button for ModalPopup as TargetControlID--%>
    <asp:LinkButton ID="lnkDetail" runat="server"></asp:LinkButton>

    <%--pnlPopUp_Detail Panel With Design--%>
    <asp:Panel ID="pnlPopUp_Detail" runat="server" CssClass="modal modalPopup" Style="display: none;">
        <div class="modal-dialog modal-xl modal-dialog-centered modal-dialog-scrollable">
            <div class="modal-content">
                <div class="modal-header">

                    <asp:Label ID="lblHeading" runat="server" CssClass="modal-title" Text="WA/RCS Report Details"></asp:Label>

                    <asp:LinkButton runat="server" style="text-align:right;" ID="lnkbtncancel">X</asp:LinkButton>
                </div>
                <div class="modal-body">
                    <div class="">
                        <div class="table-responsive">
                            <table class="table table-striped table-bordered dt-responsive nowrap" id="rpttable" width="100%" cellspacing="0" style="display:none">
                                <thead>
                                    <tr>
                                        <th>Sr. No</th>
                                        <th>Mobile No</th>
                                        <th>Submit Date Time</th>
                                        <th>Status</th>

                                        <th>Status Date Time</th>
                                        <th>Server Response</th>

                                        <th>Message</th>
                                        <th>MSG ID</th>

                                    </tr>
                                </thead>
                                <tbody>
                                </tbody>
                            </table>

                             <table class="table table-striped table-bordered dt-responsive nowrap" id="rcs_rpttable" style="display:none" width="100%" cellspacing="0">
                                <thead>
                                    <tr>
                                          <th>Sr. No</th>
                                        <th>Mobile No</th>
                                        <th>Submit Date Time</th>
                                        <th>Status</th>

                                        <th>Status Date Time</th>
                                        <th>Server Response</th>

                                        <th>Message</th>
                                        <th>MSG ID</th>
                                    </tr>
                                </thead>
                                <tbody>
                                   
                                </tbody>
                            </table>

                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnclosepop" ClientIDMode="Static" runat="server" class="btn btn-primary" Text="Close" />
                    <%--<button id="btnCancel" runat="server" class="btn btn-primary"  >Close</button>--%>
                    <button id="btnCancel2" runat="server" visible="false"></button>
                </div>
            </div>
        </div>
    </asp:Panel>

    <%--pnlPopUp_Detail Modal Popup Extender For pnlPopUp_Detail--%>
    <cc1:ModalPopupExtender ID="pnlPopUp_Detail_ModalPopupExtender" runat="server" PopupControlID="pnlPopUp_Detail"
        TargetControlID="lnkDetail" BehaviorID="mpeSMSDetailReport" 
        BackgroundCssClass="modalBackground">
    </cc1:ModalPopupExtender>
 <%-- </ContentTemplate>
                    </asp:UpdatePanel>--%>
    <!-- Bootstrap core JavaScript-->
    <script src="vendor/jquery/jquery-3.5.1.min.js"></script>

  <%--  <script>
        $('#rpttable').dataTable({
            destroy: true,
            aaData: response.data
        });
    </script>--%>

    <script type="text/javascript"> 
         
        function view(submitdate) {
            //console.log(submitdate);
             var user = '<% =Session["UserID"].ToString() %>';
          
            if ($('input[name="ctl00$ContentPlaceHolder1$vi"]:checked').val() == "rdbWhatsapp") {

                
                $('#ContentPlaceHolder1_lblHeading').text("Whatsapp Report Detail");
                $('#rcs_rpttable').hide();
                $('#rpttable').show();
                $.ajax({
                type: "POST",
                dataType: "json",
                url: "WebService.asmx/GetWAReport",
                    data: { dater: submitdate + '$' + user },
                
                success: function (data) {
                    var datatableVariable = $('#rpttable').DataTable({
                        data: data,
                        bRetrieve: true,
                        columns: [
                            { 'data': 'sln' },
                            
                            { 'data': 'mobile' },
                            { 'data': 'senttime' },
                            { 'data': 'dlrstat' },

                            { 'data': 'dlrtime' },
                            { 'data': 'dlrresp' },
                            { 'data': 'msgtext' },
                             { 'data': 'msgid' }
                        ]
                    });
                    }
                
            });
            }
            else {
                 $('#ContentPlaceHolder1_lblHeading').text("RCS Report Detail");
                 $('#rcs_rpttable').show();
                 $('#rpttable').hide();

                $.ajax({
                type: "POST",
                dataType: "json",
                url: "WebService.asmx/GetRCSReport",
                data: { dater: submitdate + '$' + user},
                success: function (data) {
                    var datatableVariable = $('#rcs_rpttable').DataTable({
                        data: data,
                        bRetrieve: true,
                        columns: [
                            { 'data': 'sln' },
                            { 'data': 'mobile' },
                            { 'data': 'senttime' },
                            { 'data': 'dlrstat' },
                            { 'data': 'dlrtime' },
                            { 'data': 'dlrresp' },
                            { 'data': 'msgtext' },
                            { 'data': 'msgid' }
                        ]
                    });
                }
            });
            }


            console.log("done");
          
            $find('mpeSMSDetailReport').show();
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
         function loadtblscrpt() {
            //alert('eafbsuyhasb');
             //$('.dataTable').DataTable( {
             //       responsive: true
             //   } );
        }
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
   <%-- </div>--%>
</asp:Content>

