<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="balance-management.aspx.cs" Inherits="eMIMPanel.balance_management" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        /*CSS Classes For Design Modal*/
        .modalPopup {
            min-height: 75px;
            position: fixed;
            z-index: 2000;
            padding: 0;
            background-color: #fff;
            border-radius: 6px;
            background-clip: padding-box;
            border: 1px solid rgba(0, 0, 0, 0.2);
            min-width: 290px;
            box-shadow: 0 5px 10px rgba(0, 0, 0, 0);
        }

        .modalBackground {
            position: fixed;
            top: 0;
            left: 0;
            background-color: #000;
            opacity: 0.5;
            z-index: 1800;
            min-height: 100%;
            width: 100%;
            overflow: hidden;
            filter: alpha(opacity=50);
            display: inline-block;
            z-index: 1000;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%-- <form runat="server">--%>
    <cc1:ToolkitScriptManager ID="scrmg" runat="server" EnablePageMethods="true">
    </cc1:ToolkitScriptManager>
    <main>
        <div class="container-fluid">
            <nav aria-label="breadcrumb" class="my-3">
                <ol class="breadcrumb breadcrumb-info">
                    <li class="breadcrumb-item"><a href="#">Home</a></li>
                    <li class="breadcrumb-item active" aria-current="page">Balance Management</li>
                </ol>
            </nav>

            <!-- Content Row -->
            <div class="row">

                <!-- Area Chart -->
                <div class="col-xl-12 col-lg-12">
                    <!-- Basic Card Example -->
                    <div class="card bg-primary border-light shadow-soft mb-4">
                        <div class="card-header py-3 bg-primary d-flex justify-content-between flex-wrap align-content-center">
                            <h6 class="m-0 font-weight-bold my-auto">Balance Management</h6>
                            <div class="right-view">
                                <div class="row">

                                    <%--<div class="col-md-4">
                                                    <asp:TextBox ID="txtFrm" runat="server" class="form-control datepicker"
                                                        placeholder="From Date"></asp:TextBox>
                                                </div>--%>
                                    <%--<div class="col-md-4">
                                                    <asp:TextBox ID="txtTo" runat="server" class="form-control datepicker"
                                                        placeholder="To Date"></asp:TextBox>
                                                </div>--%>
                                    <%--<a class="btn btn-primary text-dark btn-block" id="reportrange" role="button" aria-pressed="true">
                                                <i class="fas fa-calendar mr-2 text-dark"></i>
                                                <span class="text-dark"></span><i class="ml-1 fas fa-chevron-down" data-feather="chevron-down"></i>
                                            </a>--%>
                                    <div class="col-md-12">
                                        <asp:LinkButton runat="server" ID="LinkButton2" OnClick="btnUpdate_Click" class="btn btn-mini">
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
                                            <asp:LinkButton ID="LinkButton1" runat="server" class="mx-1 btn btn-primary text-secondary"
                                                OnClick="btnView_Click"
                                                data-toggle="tooltip" data-placement="top" title="" data-original-title="Balance"> 
                                                        <span class="text-secondary"> <i class="fas fa-credit-card"></i> </span></asp:LinkButton>

                                            <asp:LinkButton ID="LinkButton2" runat="server" class="mx-1 btn btn-primary text-secondary"
                                                OnClick="btnSMSRate_Click"
                                                data-toggle="tooltip" data-placement="top" title="" data-original-title="SMS Rate"> 
                                                        <span class="text-secondary"> <i class="fas fa-sliders-h"></i> </span></asp:LinkButton>

                                            <%--<a href="#" class="mx-1 btn btn-primary text-secondary">
                                                            <span class="text-secondary"><i class="fas fa-credit-card"></i></span>
                                                            <asp:Button ID="btnView" runat="server" Text="" OnClick="btnView_Click" data-toggle="tooltip" data-placement="top" title="" data-original-title="View" />
                                                        </a>--%>
                                            <%--<a href="#" class="mx-1 btn btn-primary text-dark">
                                                            <span class="text-secondary"><i class="fas fa-sliders-h"></i></span>
                                                            <asp:Button ID="btnSMSRate" runat="server" Text="" OnClick="btnSMSRate_Click" data-toggle="tooltip" data-placement="top" title="" data-original-title="View" />
                                                        </a>--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Company Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl1" runat="server" Text='<%#Eval("compname")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Full Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblname" runat="server" Text='<%#Eval("fullname")%>'></asp:Label>
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
                                    <asp:TemplateField HeaderText="Mobile No">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl3" runat="server" Text='<%#Eval("mobile")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Email ID">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl4" runat="server" Text='<%#Eval("Email")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Balance">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl5" runat="server" Text='<%#Eval("balance")%>'></asp:Label>
                                            <asp:HiddenField ID="hdnrate_normalsms" runat="server" Value='<%#Eval("rate_normalsms")%>' />
                                            <asp:HiddenField ID="hdnrate_smartsms" runat="server" Value='<%#Eval("rate_smartsms")%>' />
                                            <asp:HiddenField ID="hdnrate_campaign" runat="server" Value='<%#Eval("rate_campaign")%>' />
                                            <asp:HiddenField ID="hdnrate_otp" runat="server" Value='<%#Eval("rate_otp")%>' />
                                            <asp:HiddenField ID="hdnUrlrate" runat="server" Value='<%#Eval("urlrate")%>' />
                                            <asp:HiddenField ID="hdn_dlt" runat="server" Value='<%#Eval("dltcharge")%>' />

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

        <asp:Panel ID="pnlpopupCRDR" runat="server" CssClass="modalPopup" Style="display: none;">
            <div style="overflow-y: auto; overflow-x: hidden; max-height: 550px;">
                <div class="modal-header">
                    <asp:Label ID="Label1" runat="server" CssClass="modal-title" Text="Balance Management"></asp:Label>
                </div>
                <div class="modal-body">
                    <div class="form-row">

                        <div class="col-md-12">
                            <%--<div class="custom-control custom-radio custom-control-inline">--%>

                            <asp:RadioButton ID="rdbCredit" runat="server" Text="Credits" Checked="true" GroupName="crdr" />
                            <%--<input type="radio" id="customRadioInline01" name="customRadioInline2" class="custom-control-input">
                                    <label class="custom-control-label" for="customRadioInline01">Credits</label>--%>
                            <%--</div>
                                <div class="custom-control custom-radio custom-control-inline">--%>
                            <asp:RadioButton ID="rdbDebt" runat="server" Text="Debit" GroupName="crdr" />
                            <%--<input type="radio" id="customRadioInline02" name="customRadioInline2" class="custom-control-input">
                                    <label class="custom-control-label" for="customRadioInline02">Debit</label>--%>
                            <%--</div>--%>
                        </div>
                        <div class="col-md-12">
                            <label for="validationCustom02" class="font-weight-bold">Quantity</label>
                            <%--<input type="number" class="form-control" id="validationCustom01" value="">--%>
                            <asp:TextBox ID="txtbal" runat="server" class="form-control" Text="" />
                            <div class="valid-feedback">Looks good! </div>
                        </div>
                    </div>
                </div>
                <div align="center" class="modal-footer">
                    <button type="button" class="btn btn-sm btn-primary" runat="server" onserverclick="btnUpdateCRDR_Click"><i class="fas fa-user-edit"></i>Update</button>
                    <button type="button" id="btnCancel2" runat="server" onserverclick="btnUpdate_Click" class="btn btn-primary text-danger ml-auto" data-dismiss="modal">Close</button>
                </div>
            </div>
        </asp:Panel>

        <asp:LinkButton ID="LinkButton3" runat="server"></asp:LinkButton>
        <cc1:ModalPopupExtender ID="modalpopupCRDR" runat="server" BackgroundCssClass="modalBackground"
            PopupControlID="pnlpopupCRDR" TargetControlID="LinkButton3" CancelControlID="btnCancel2">
        </cc1:ModalPopupExtender>


        <asp:Panel ID="pnlpopupRATE" runat="server" CssClass="modalPopup" Style="display: none;">
            <div style="overflow-y: auto; overflow-x: hidden; max-height: 600px;">
                <div class="modal-header">
                    <asp:Label ID="Label2" runat="server" CssClass="modal-title" Text="Rate Management"></asp:Label>
                </div>
                <div class="modal-body">
                    <ul class="nav nav-tabs" id="myTab" role="tablist">
                        <li class="nav-item" role="presentation">
                            <a class="nav-link active" id="home-tab" data-toggle="tab" href="#home" role="tab" aria-controls="home" aria-selected="true">SMS</a>
                        </li>
                        <li class="nav-item" role="presentation">
                            <a class="nav-link" id="profile-tab" data-toggle="tab" href="#profile" role="tab" aria-controls="profile" aria-selected="false">Link</a>
                        </li>
                    </ul>
                    <div class="tab-content" id="myTabContent">
                        <div class="tab-pane fade show active" id="home" role="tabpanel" aria-labelledby="home-tab">
                            <div class="form-group row">
                                <label for="exampleFormControlTextarea2" class="col-sm-4 col-form-label font-weight-bold">Premium</label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txts1" runat="server" class="form-control" placeholder="Old Price" disabled="true" />
                                </div>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtns1" runat="server" class="form-control" placeholder="Price" />
                                </div>
                            </div>
                            <div class="form-group row">
                                <label for="exampleFormControlTextarea2" class="col-sm-4 col-form-label font-weight-bold">Link Text</label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txts2" runat="server" class="form-control" placeholder="Old Price" disabled="true" />
                                </div>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtns2" runat="server" class="form-control" placeholder="Price" />
                                </div>
                            </div>
                            <div class="form-group row">
                                <label for="exampleFormControlTextarea2" class="col-sm-4 col-form-label font-weight-bold">Campaign</label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txts3" runat="server" class="form-control" placeholder="Old Price" disabled="true" />
                                </div>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtns3" runat="server" class="form-control" placeholder="Price" />
                                </div>
                            </div>
                            <div class="form-group row">
                                <label for="exampleFormControlTextarea2" class="col-sm-4 col-form-label font-weight-bold">OTP</label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txts4" runat="server" class="form-control" placeholder="Old Price" disabled="true" />
                                </div>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtns4" runat="server" class="form-control" placeholder="Price" />
                                </div>
                            </div>
                            <div class="form-group row">

                                <label for="exampleFormControlTextarea2" class="col-sm-4 col-form-label"><b>DLT Charges</b><span style="font-size: x-small;"> (included in SMS Rate)</span></label>

                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtd1" runat="server" class="form-control" placeholder="Old Price" disabled="true" />
                                </div>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtnd1" runat="server" class="form-control" placeholder="Price" />
                                </div>
                            </div>


                        </div>
                        <div class="tab-pane fade" id="profile" role="tabpanel" aria-labelledby="profile-tab">

                            <div class="form-group row">
                                <label for="exampleFormControlTextarea2" class="col-sm-4 col-form-label font-weight-bold">URL</label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtUrlRate" runat="server" class="form-control" placeholder="Old Price" disabled="true" />
                                </div>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtUrlRateN" runat="server" class="form-control" placeholder="Price" />
                                </div>
                            </div>
                            <div class="form-group row">
                                <label for="exampleFormControlTextarea2" class="col-sm-4 col-form-label font-weight-bold">Form</label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtFormRate" runat="server" class="form-control" placeholder="1000" disabled="true" />
                                </div>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtFormRateN" runat="server" class="form-control" placeholder="Price" disabled="true" />
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
                <div align="center" class="modal-footer">
                    <div class="row">
                        <div class="col-md-12">
                            <button type="button" class="btn btn-sm btn-primary" runat="server" onserverclick="btnUpdateRate_Click"><i class="fas fa-user-edit"></i>Update</button>
                            <button id="btnCancel1" type="button" runat="server" onserverclick="btnUpdate_Click" class="btn btn-primary text-danger ml-auto" data-dismiss="modal">Close</button>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
        <asp:LinkButton ID="lnkSCH" runat="server"></asp:LinkButton>
        <cc1:ModalPopupExtender ID="modalpopupRATE" runat="server" BackgroundCssClass="modalBackground"
            PopupControlID="pnlpopupRATE" TargetControlID="lnkSCH" CancelControlID="btnCancel1">
        </cc1:ModalPopupExtender>

    </main>
    <%--</form>--%>

    <!-- Bootstrap core JavaScript-->
    <script src="vendor/jquery/jquery-3.5.1.min.js"></script>
    <script src="vendor/bootstrap/js/bootstrap.bundle.min.js"></script>

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



</script>
</asp:Content>
