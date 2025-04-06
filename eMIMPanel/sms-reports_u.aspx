<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="sms-reports_u.aspx.cs" Inherits="eMIMPanel.sms_reports_u" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
                            <li class="breadcrumb-item active" aria-current="page">SMS Reports</li>
                        </ol>
                    </nav>
                    <!-- Content Row -->
                    <div class="row">
                        <div class="col-12">
                            <div class="card bg-primary border-light shadow-soft mb-4">
                                <div class="card-header py-3 bg-primary">
                                    <h6 class="m-0 font-weight-bold"><i class="fas fa-info-circle"></i>Info</h6>
                                </div>
                                <div class="card-body py-2">
                                    <div class="row">
                                        <!-- Card-->
                                        <div class="col-12 col-md-6 col-sm-6 col-lg-6 col-xl-4 mb-2 count-view">
                                            <a href="sms-reports.html">
                                                <div class="card bg-primary shadow-inset border-light h-100 py-2">
                                                    <div class="card-body">
                                                        <div class="row no-gutters align-items-center">
                                                            <div class="col mr-2">
                                                                <div class="text-xs font-weight-bold text-warning text-uppercase mb-1">Submitted</div>
                                                                <div class="h5 mb-0 font-weight-bold text-gray-800">
                                                                    <asp:Label ID="lblTodaySubmitted" runat="server"></asp:Label>
                                                                </div>
                                                            </div>
                                                            <div class="col-auto">
                                                                <div class="icon icon-shape shadow-soft rounded-circle">
                                                                    <span class="fas fa-check-circle fa-2x text-warning"></span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </a>
                                        </div>
                                        <!-- End Card -->
                                        <!-- Card-->
                                        <div class="col-12 col-md-6 col-sm-6 col-lg-6 col-xl-4 mb-2 count-view">
                                            <a href="sms-reports.html">
                                                <div class="card bg-primary shadow-inset border-light h-100 py-2">
                                                    <div class="card-body">
                                                        <div class="row no-gutters align-items-center">
                                                            <div class="col mr-2">
                                                                <div class="text-xs font-weight-bold text-success text-uppercase mb-1">Delivered</div>
                                                                <div class="row no-gutters align-items-center">
                                                                    <div class="col-auto">
                                                                        <div class="h5 mb-0 mr-3 font-weight-bold text-gray-900">
                                                                            <asp:Label ID="lblTodayDelivered" runat="server"></asp:Label>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-auto">
                                                                <div class="icon icon-shape shadow-soft rounded-circle">
                                                                    <span class="fas fa-check-double fa-2x text-success"></span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </a>
                                        </div>
                                        <!-- End Card -->

                                        <!-- Card-->
                                        <div class="col-12 col-md-6 col-sm-6 col-lg-6 col-xl-4 mb-4 count-view">
                                            <a href="sms-reports.html">
                                                <div class="card bg-primary shadow-inset border-light h-100 py-2">
                                                    <div class="card-body">
                                                        <div class="row no-gutters align-items-center">
                                                            <div class="col mr-2">
                                                                <div class="text-xs font-weight-bold text-danger text-uppercase mb-1">Failed</div>
                                                                <div class="h5 mb-0 font-weight-bold text-danger-800">
                                                                    <asp:Label ID="lblTodayFailed" runat="server"></asp:Label>
                                                                </div>
                                                            </div>
                                                            <div class="col-auto">
                                                                <div class="icon icon-shape shadow-soft rounded-circle">
                                                                    <span class="fas fa-thumbs-down fa-2x text-danger"></span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </a>
                                        </div>
                                        <!-- End Card -->
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-12">
                            <div class="card card-body mb-4 bg-primary border-light shadow-soft">
                                
                                    <div class="form-row">
                                        <div class="form-group col-lg-4 col-xl-5">
                                            <label for="#">Date Select</label>
                                            <div class="row">
                                                <div class="col-md-6">
                                                   <asp:TextBox ID="txtFrm1"  runat="server"  onchange="javascript:text_changed_from1();" class="form-control datepicker" placeholder="From Date" autocomplete="off"></asp:TextBox>
                                                <asp:HiddenField ID="hdntxtFrm1" runat="server" />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:TextBox ID="txtTo1" runat="server" onchange="javascript:text_changed_to1();" class="form-control datepicker" placeholder="To Date" autocomplete="off"></asp:TextBox>
                                                    <asp:HiddenField ID="hdntxtTo1" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group col-lg-3 col-xl-3">
                                            <label for="#">Search mobile number :</label>
                                            <asp:TextBox id="txtmob" runat="server"  class="form-control" placeholder="XX-XXX-XXXX" MaxLength="10"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterMode="ValidChars"
                                                                        TargetControlID="txtmob" ValidChars="0123456789">
                                                                    </cc1:FilteredTextBoxExtender>
                                        </div>
                                        <div class="form-group col-lg-3  col-xl-2 mt-auto">
                                            <asp:LinkButton ID="btnsearch" runat="server" OnClick="btnsearch_Click" class="btn btn-primary text-success btn-block" ><i class="fas fa-search fa-sm text-success"></i>Search</asp:LinkButton>
                                        </div>
                                        <div class="form-group col-lg-2  col-xl-2 mt-lg-auto">
                                            <asp:LinkButton ID="btnXLdw" runat="server" OnClick="btnXLdw_Click"><i class="fas fa-file-excel fa-2x text-success"></i></asp:LinkButton>
                                            <%--<a href="#"><i class="fas fa-file-csv fa-2x text-danger"></i></a>
                                            <a href="#"><i class="far fa-file-alt fa-2x text-secondary"></i></a>--%>
                                        </div>
                                    </div>
                                
                            </div>
                        </div>
                    </div>

                    <div class="row justify-content-center">
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

                    <!-- Content Row -->
                    <div class="row">
                        <div class="col-xl-12 col-lg-12">
                            <!-- Basic Card Example -->
                            <div class="card bg-primary border-light shadow-soft mb-4">
                                <div class="card-header py-3 bg-primary d-flex justify-content-between flex-wrap align-content-center">
                                    <h6 class="m-0 font-weight-bold my-auto">SMS Reports</h6>
                                    <div class="right-view">
                                        <div class="row">

                                            <div class="col-md-4">
                                               <asp:TextBox ID="txtFrm"  runat="server" onchange="javascript:text_changed_from();"  class="form-control datepicker" placeholder="From Date"  autocomplete="off"></asp:TextBox>
                                                <asp:HiddenField ID="hdntxtFrm" runat="server" />
                                            </div>
                                            <div class="col-md-4">
                                                 <asp:TextBox ID="txtTo" runat="server"  onchange="javascript:text_changed_to();" class="form-control datepicker" placeholder="To Date"  autocomplete="off"></asp:TextBox>
                                                <asp:HiddenField ID="hdntxtTo" runat="server" />
                                            </div>
                                            <%--<a class="btn btn-primary text-dark btn-block" id="reportrange" role="button" aria-pressed="true">
                                                <i class="fas fa-calendar mr-2 text-dark"></i>
                                                <span class="text-dark"></span><i class="ml-1 fas fa-chevron-down" data-feather="chevron-down"></i>
                                            </a>--%>
                                            <div class="col-md-4">
                                                <asp:LinkButton runat="server" ID="LinkButton1" OnClick="btnUpdate_Click" class="btn btn-mini">
                                                    Show <i class="fas fa-eye" aria-hidden="true"></i>
                                                </asp:LinkButton>
                                                <%--<a href="#" class="btn text-success mr-3">
                                                    <span class="text-success">
                                                        <i class="fas fa-sync"></i>
                                                    </span>
                                                    <asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnUpdate_Click" />

                                                </a>--%>
                                            </div>

                                            <asp:HiddenField ID="h1" runat="server" />
                                            <asp:HiddenField ID="h2" runat="server" />
                                        </div>
                                    </div>
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
                                                <asp:TemplateField HeaderText="Message ID">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUserId" runat="server" Text='<%#Eval("msgid")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Mobile">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblmobile" runat="server" Text='<%#Eval("mobile")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Sender ID">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblsender" runat="server" Text='<%#Eval("senderid")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Message">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl1" runat="server" Text='<%#Eval("msgtext")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Submit Time">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblname" runat="server" Text='<%#Eval("senttime")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="DLR Status">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl3" runat="server" Text='<%#Eval("dlrstat")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>

                                            <%--<EmptyDataTemplate>
                                                <table style="width: 100%;">
                                                    <tr>
                                                        <td align="center">
                                                            <asp:Label ID="lblEmpty" Text="No Data Found!!!" Style="color: Red;" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </EmptyDataTemplate>--%>
                                        </asp:GridView>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
    </main>
    <script type="text/javascript">
        $(document).ready(function() {
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
</asp:Content>
