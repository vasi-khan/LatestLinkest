<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="smsSummary.aspx.cs" Inherits="smsSummary.smsSummary" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html>
<link href="css/neumorphism.css" rel="stylesheet" />
<link rel="stylesheet" href="vendor/fontawesome-free/css/all.min.css" />
<link href="vendor/datatables/dataTables.bootstrap4.min.css" rel="stylesheet" />
<link href="vendor/datatables/dataTables.bootstrap4.css" rel="stylesheet" />
<link rel="stylesheet" href="css/sidebar-new.css" />
<link href="css/select2.min.css" rel="stylesheet" />

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
<html xmlns="http://www.w3.org/1999/xhtml">

<body>
    <form id="form1" runat="server">
        <asp:ToolkitScriptManager ID="scrmg" runat="server" EnablePageMethods="true">
        </asp:ToolkitScriptManager>

        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="container-fluid">
                    <nav aria-label="breadcrumb" class="my-3 ">
                        <ol class="breadcrumb breadcrumb-info justify-content-between align-items-center">
                            <li class="breadcrumb-item active font-weight-bold" aria-current="page">SMS SUMMARY</li>
                            <div>
                                <asp:Label ID="lblUserid" runat="server"></asp:Label>
                                <asp:Label class="breadcrumb-item active font-weight-bold mr-2" ID="lblAcc" runat="server"></asp:Label>
                                <asp:LinkButton class="btn btn-danger" ID="btnLogout" runat="server" Text="Logout" OnClick="btnLogout_Click"><i class="fa fa-sign-out-alt"></i></asp:LinkButton>
                            </div>
                        </ol>
                    </nav>

                    <div class="row">
                        <!-- Area Chart -->
                        <div class="col-xl-12 col-lg-12">
                            <!-- Basic Card Example -->
                            <div class="card bg-primary border-light shadow-soft mb-4">
                                <div class="card-body px-4">
                                    <asp:Panel ID="pnlMain" runat="server">
                                        <div class="form-row">
                                            <div class="col-md-4" id="divtemp" runat="server">
                                                <Label> Template</Label>
                                                <asp:DropDownList ID="ddlTempIdAndName" runat="server" class="custom-select" ClientIDMode="Static"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-2">
                                                <Label>From Date</Label>
                                                <asp:TextBox ID="txtFrm" Type="date" runat="server" class="form-control" placeholder="From Date" autocomplete="off"></asp:TextBox>
                                                <%--<asp:HiddenField ID="hdntxtFrm" runat="server" />--%>
                                            </div>
                                            <div class="col-md-2">
                                                <Label>To Date</Label>
                                                <asp:TextBox ID="txtTo" Type="date" runat="server" class="form-control " placeholder="To Date" autocomplete="off"></asp:TextBox>
                                                <asp:HiddenField ID="hdntxtTo" runat="server" />
                                            </div>
                                            <div class="col-md-2 p-4">
                                                <asp:LinkButton runat="server" OnClientClick=" return CheckDates();" ID="btnShow" OnClick="btnShow_Click" class="btn btn-block">
                                                    Show <i class="fas fa-eye" aria-hidden="true"></i>
                                                </asp:LinkButton>
                                            </div>
                                            <div class="col-md-2 p-4">
                                                <asp:Button ID="btnReset" class="btn btn-danger" runat="server" Text="RESET" OnClick="btnReset_Click" />
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </div>

                            </div>
                            <div class="card mb-4 bg-primary border-light shadow-soft">
                                <div class="card-header d-flex justify-content-between align-items-center border-bottom">
                                    <div class="flex-fill m-0 font-weight-bold">SMS Summary</div>
                                    <div class="flex-fill text-right">
                                        <asp:LinkButton runat="server" ID="lnkDownload" class="btn btn-mini" OnClick="lnkDownload_Click" >
                                Download <i class="fas fa-download" aria-hidden="true"></i>
                                        </asp:LinkButton>
                                    </div>
                                </div>

                                <div class="card-body">
                                    <div id="collapseOne" class="collapse show" aria-labelledby="headingOne" data-parent="#accordionExample" runat="server">
                                        <!--  -->
                                        <div class="table-responsive">
                                            <asp:GridView ID="grv" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false" ShowFooter="true"
                                                runat="server" Width="100%" CellPadding="10" BorderColor="#ede8e8" Class="table table-striped table-bordered dt-responsive wrap dataTable-view">
                                                <Columns>
                                                    <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Sr No">
                                                        <ItemTemplate>
                                                            <%#Container.DataItemIndex+1 %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="SMS Content">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSMSContent" runat="server" Text='<%#Eval("MSGTEXT")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="TemplateID Wise" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTemplateId" runat="server" Text='<%#Eval("senderid")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDate" runat="server" Text='<%#Eval("SMSDATE")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="User ID">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblUserId" runat="server" Text='<%#Eval("userid")%>'></asp:Label>
                                                            <asp:HiddenField ID="hdndate" runat="server" Value='<%#Eval("smsDate1")%>'></asp:HiddenField>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Submitted">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl1" runat="server" Text='<%#Eval("Submitted")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Delivered">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl2" runat="server" Text='<%#Eval("Delivered")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Failed">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl3" runat="server" Text='<%#Eval("Failed")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Unknown">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblurlid" runat="server" Text='<%#Eval("Unknown")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                                            </asp:GridView>
                                        </div>
                                        <!--  -->
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnShow" />
                <asp:PostBackTrigger ControlID="lnkDownload" />
            </Triggers>
        </asp:UpdatePanel>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel2" DisplayAfter="0">
            <ProgressTemplate>
                <div align="center" style="position: absolute; left: 40%; top: 50%; vertical-align: middle;">
                    <img src="Img/loading.gif" />
                </div>
                <div align="center" style="position: absolute; left: 0%; top: 0%; vertical-align: middle;">
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>

        
        <!-- Select2 -->
        <script src="js/select2.min.js"></script>
        <script>
            $("#ddlTempIdAndName").select2({
                allowClear: true
            });

            $("#ddlTempIdAndName").select2({
                allowClear: true
            });
        </script>

        <script type="text/javascript">
            function text_changed_from() {
                var d = document.getElementById("ContentPlaceHolder1_txtFrm").value;
                console.log(d);
                document.getElementById("ContentPlaceHolder1_hdntxtFrm").value = d;
            }
            function text_changed_to() {
                var d = document.getElementById("ContentPlaceHolder1_txtTo").value;
                console.log(d);
                document.getElementById("ContentPlaceHolder1_hdntxtTo").value = d;
            }
        </script>


        <script src="vendor/jquery-easing/jquery.easing.min.js"></script>

        <!--  Select-->
        <script src="vendor/select/bootstrap-select.min.js"></script>

        <!-- Page level custom scripts -->
        <script src="js/demo/datatables-demo.js"></script>

        <%--<script src="vendor/datepicker/bootstrap-datepicker.js"></script>--%>

        <script src="vendor/bootstrap/js/bootstrap.bundle.min.js"></script>
        <%--<script src="js/sb-admin-2.min.js"></script>--%>


        <script src="vendor/jquery/jquery-3.5.1.min.js"></script>
        <%--<script src="js/jquery.min.js"></script>--%>

        <!-- Page level custom scripts -->

        <script src="js/datatables-demo.js"></script>
        <%--<script src="vendor/datepicker/bootstrap-datepicker.js"></script>--%>


        <script src="vendor/bootstrap/js/bootstrap.bundle.min.js"></script>
        <script src="js/sb-admin-2.min.js"></script>

        <!-- Page level plugins -->
        <script src="vendor/datatables/jquery.dataTables.min.js"></script>
        <script src="vendor/datatables/dataTables.bootstrap4.min.js"></script>
        <script src="vendor/datatables/dataTables.responsive.min.js"></script>
        <script src="vendor/datatables/responsive.bootstrap4.min.js"></script>

        <!--  Select-->
        <script src="vendor/select/bootstrap-select.min.js"></script>

        <!-- Page level custom scripts -->
        <%--<script src="js/demo/datatables-demo.js"></script>--%>

        <script>
            $(function () {
                var sixMonthAgo = new Date();
                sixMonthAgo.setMonth(sixMonthAgo.getMonth() - 6);
                $(".datepicker").datepicker({
                    endDate: new Date(),
                    todayHighlight: true,
                    autoclose: true,
                    startDate: sixMonthAgo,
                    format: 'yyyy-mm-dd'
                    //autoUpdateInput: false
                });
            });
        </script>

        <script>
            // Datatable Script 
            $(document).ready(function () {
                $('.dataTable-view').DataTable();
            });
        </script>
    </form>
</body>
</html>
