<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false" Debug="true" CodeBehind="TemplateAddition.aspx.cs" Inherits="ParkPlusTemplateAddition.TemplateAddition" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>
 <link href="css/neumorphism.css" rel="stylesheet" />
        <link rel="stylesheet" href="vendor/fontawesome-free/css/all.min.css" />
        <link href="vendor/datatables/dataTables.bootstrap4.min.css" rel="stylesheet" />
<link rel="stylesheet" href="css/sidebar-new.css" />
<style type="text/css">
    /*CSS Classes For Design Modal*/
    .modal.modalPopup {
        top: 0 !important;
        left: 0 !important;
        display: block;
        width: 600px !important;
    }

    .modalBackground {
        background-color: #000;
        opacity: 0.5;
    }
</style>
<style type="text/css">
    .display1 {
        display: none !important;
    }

    .display2 {
        display: none !important;
    }

    .display3 {
        display: none !important;
    }

    .display4 {
        display: none !important;
    }

    .display5 {
        display: none !important;
    }

    .displaySem1 {
        display: none !important;
    }

    .displaySem2 {
        display: none !important;
    }

    .displaySem3 {
        display: none !important;
    }

    .displaySem4 {
        display: none !important;
    }

    .displaySem5 {
        display: none !important;
    }
</style>

<html xmlns="http://www.w3.org/1999/xhtml">

<body>
    <form id="form1" runat="server">
        <asp:ToolkitScriptManager ID="scrmg" runat="server" EnablePageMethods="true">
        </asp:ToolkitScriptManager>

        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="container-fluid">
                    <nav aria-label="breadcrumb" class="my-3 ">
                        <ol class="breadcrumb breadcrumb-info justify-content-between align-items-center">
                            <li class="breadcrumb-item active font-weight-bold" aria-current="page">Add Template</li>
                            <div>
                                  <asp:Label class="breadcrumb-item active font-weight-bold mr-2" ID="lblAcc" runat="server" ></asp:Label>
                                <asp:LinkButton class="btn btn-danger" ID="btnLogout" runat="server" Text="Logout" OnClick="btnLogout_Click"><i class="fa fa-sign-out-alt"></i></asp:LinkButton>
                            </div>
                          
                            
                        </ol>
                    </nav>

                    <div class="row">
                        <!-- Area Chart -->
                        <div class="col-xl-12 col-lg-12">
                            <!-- Basic Card Example -->
                            <div class="card bg-primary border-light shadow-soft mb-4">
                                <div class="card-header py-3 bg-primary">
                                    <div class="row">
                                        <div class="col-md-10">
                                            <h6 class="m-0 font-weight-bold"><i class="far fa-user-circle"></i>Add New Template</h6>
                                        </div>
                                    </div>
                                </div>
                                <div class="card-body px-4">
                                    <asp:Panel ID="pnlMain" runat="server">
                                        <div class="form-row">


                                            <div class="col-md-6 mb-3">
                                                <asp:TextBox class="form-control" ID="txtTempId" runat="server" MaxLength="19" placeholder="Template Id" ToolTip="Template Id" OnKeypress="return /^[0-9]{0,19}$/.test(this.value+event.key);" />
                                                <div class="valid-feedback">
                                                    Looks good!                             
                                                </div>
                                            </div>
                                            <div class="col-md-6 mb-3">
                                                <asp:TextBox class="form-control" ID="txtTempName" runat="server" MaxLength="100" placeholder="Template Name" ToolTip="Template Name" />
                                                <div class="valid-feedback">
                                                    Looks good!                             
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-row">
                                            <div class="col-md-12 mb-12">
                                                <asp:TextBox class="form-control" ID="txtTemplateContent" runat="server" placeholder="Template Text" TextMode="MultiLine" Rows="5" ToolTip="Template Content" />
                                                <div class="valid-feedback">
                                                    Looks good!                             
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6" id="divuser" runat="server">
                                                <div class="row  mt-3 gx-0">
                                                    <div class="col-md-2 mt-2 p-0 pl-3">
                                                        <asp:Label ID="lblusr1" runat="server" Text="UserId"></asp:Label>
                                                    </div>
                                                    <div class="col-4 p-0">
                                                        <%--<input type="text" class="form-control">--%>
                                                        <asp:TextBox ID="txtuserid1" MaxLength="20" runat="server" class="form-control"></asp:TextBox>
                                                    </div>
                                                    <div class="col-2 text-center">
                                                        <button class="input-group-text" for="inputGroupFile02" onclick="myFunction()">+</button>
                                                    </div>
                                                </div>

                                                <div class=" row mt-2 display1" id="UserId1">
                                                    <div class="col-2 mt-2 p-0 pl-3">
                                                        <asp:Label ID="lblusr2" runat="server" Text="UserId"></asp:Label>
                                                    </div>
                                                    <div class="col-4 p-0">
                                                        <asp:TextBox ID="txtuserid2" MaxLength="20" runat="server" class="form-control"></asp:TextBox>
                                                    </div>
                                                    <div class="col-1 text-center ">
                                                        <button class="input-group-text" for="inputGroupFile02" onclick="myFunction1()">+</button>

                                                    </div>
                                                </div>

                                                <div class=" row mt-2 display2" id="UserId2">
                                                    <div class="col-2 mt-2 p-0 pl-3">
                                                        <asp:Label ID="lblusr3" runat="server" Text="UserId"></asp:Label>
                                                    </div>
                                                    <div class="col-4 p-0">
                                                        <asp:TextBox ID="txtuserid3" MaxLength="20" runat="server" class="form-control"></asp:TextBox>
                                                    </div>
                                                    <div class="col-1 text-center">
                                                        <button class="input-group-text" for="inputGroupFile02" onclick="myFunction2()">+</button>

                                                    </div>
                                                </div>

                                                <div class=" row mt-2 display3" id="UserId3">
                                                    <div class="col-2 mt-2 p-0 pl-3">
                                                        <asp:Label ID="lblusr4" runat="server" Text="UserId"></asp:Label>
                                                    </div>
                                                    <div class="col-4 p-0">
                                                        <asp:TextBox ID="txtuserid4" MaxLength="20" runat="server" class="form-control"></asp:TextBox>
                                                    </div>
                                                    <div class="col-1 text-center">
                                                        <button class="input-group-text" for="inputGroupFile02" onclick="myFunction3()">+</button>

                                                    </div>
                                                </div>

                                                <div class=" row mt-2 display4" id="UserId4">
                                                    <div class="col-2 mt-2 p-0 pl-3">
                                                        <asp:Label ID="lblusr5" runat="server" Text="UserId"></asp:Label>
                                                    </div>
                                                    <div class="col-4 p-0">
                                                        <asp:TextBox ID="txtuserid5" MaxLength="20" runat="server" class="form-control"></asp:TextBox>
                                                    </div>
                                                    <div class="col-4 text-center">
                                                        <button class="input-group-text" for="inputGroupFile02" onclick="myFunction4()">+</button>

                                                    </div>
                                                </div>

                                                <div class=" row mt-2 display5" id="UserId5">
                                                    <div class="col-2 mt-2 p-0 pl-3">
                                                        <asp:Label ID="lblusr10" runat="server" Text="UserId"></asp:Label>
                                                    </div>
                                                    <div class="col-4 p-0">
                                                        <asp:TextBox ID="txtuserid6" MaxLength="20" runat="server" class="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-6" runat="server" id="divSender">

                                                <div class="row  mt-3 gx-0">
                                                    <div class="col-2 mt-2 p-0 pl-3">
                                                        <asp:Label ID="lblSender1" runat="server" Text="SenderId"></asp:Label>
                                                    </div>
                                                    <div class="col-4 p-0">
                                                        <%--<input type="text" class="form-control">--%>
                                                        <asp:TextBox ID="txtSenderId1" MaxLength="6" runat="server" class="form-control"></asp:TextBox>
                                                    </div>
                                                    <div class="col-1 text-center">
                                                        <button class="input-group-text" for="inputGroupFile02" onclick="myFunctionSen()">+</button>
                                                    </div>
                                                </div>

                                                <div class=" row mt-2 displaySem1" id="SenderId1">
                                                    <div class="col-2 mt-2 p-0 pl-3">
                                                        <asp:Label ID="lblSender2" runat="server" Text="SenderId"></asp:Label>
                                                    </div>
                                                    <div class="col-4 p-0">
                                                        <asp:TextBox ID="txtSenderId2" MaxLength="6" runat="server" class="form-control"></asp:TextBox>
                                                    </div>
                                                    <div class="col-1 text-center ">
                                                        <button class="input-group-text" for="inputGroupFile02" onclick="myFunctionSen1()">+</button>

                                                    </div>
                                                </div>

                                                <div class=" row mt-2 displaySem2" id="SenderId2">
                                                    <div class="col-2 mt-2 p-0 pl-3">
                                                        <asp:Label ID="lblSender3" runat="server" Text="SenderId"></asp:Label>
                                                    </div>
                                                    <div class="col-4 p-0">
                                                        <asp:TextBox ID="txtSenderId3" MaxLength="6" runat="server" class="form-control"></asp:TextBox>
                                                    </div>
                                                    <div class="col-1 text-center">
                                                        <button class="input-group-text" for="inputGroupFile02" onclick="myFunctionSen2()">+</button>

                                                    </div>
                                                </div>

                                                <div class=" row mt-2 displaySem3" id="SenderId3">
                                                    <div class="col-2 mt-2 p-0 pl-3">
                                                        <asp:Label ID="lblSender4" runat="server" Text="SenderId"></asp:Label>
                                                    </div>
                                                    <div class="col-4 p-0">
                                                        <asp:TextBox ID="txtSenderId4" MaxLength="6" runat="server" class="form-control"></asp:TextBox>
                                                    </div>
                                                    <div class="col-1 text-center">
                                                        <button class="input-group-text" for="inputGroupFile02" onclick="myFunctionSen3()">+</button>

                                                    </div>
                                                </div>

                                                <div class=" row mt-2 displaySem4" id="SenderId4">
                                                    <div class="col-2 mt-2 p-0 pl-3">
                                                        <asp:Label ID="lblSender5" runat="server" Text="SenderId"></asp:Label>
                                                    </div>
                                                    <div class="col-4 p-0">
                                                        <asp:TextBox ID="txtSenderId5" MaxLength="6" runat="server" class="form-control"></asp:TextBox>
                                                    </div>
                                                    <div class="col-1 text-center">
                                                        <button class="input-group-text" for="inputGroupFile02" onclick="myFunctionSen4()">+</button>

                                                    </div>
                                                </div>

                                                <div class=" row mt-2 displaySem5" id="SenderId5">
                                                    <div class="col-2 mt-2 p-0 pl-3">
                                                        <asp:Label ID="lblSender6" runat="server" Text="SenderId"></asp:Label>
                                                    </div>
                                                    <div class="col-4 p-0">
                                                        <asp:TextBox ID="txtSenderId6" MaxLength="6" runat="server" class="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-1 mt-4">
                                                <asp:Button class="btn btn-primary btn-icon-split" ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
                                            </div>
                                            <div class="col-md-1 mt-4">
                                                <asp:Button class="btn btn-danger btn-icon-split" ID="BtnReset" runat="server" Text="Reset" OnClick="BtnReset_Click" />
                                            </div>
                                        </div>

                                    </asp:Panel>
                                </div>





                                <div class="modal fade" runat="server" id="mt1" tabindex="-1" role="dialog" aria-labelledby="exmt1" aria-hidden="true">
                                    <div class="modal-dialog modal-lg" role="document">
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <asp:Label runat="server" ID="lblHeader" CssClass="h5"></asp:Label>
                                                <%--<button class="btn btn-danger" type="button" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span> </button>--%>
                                                <asp:Button class="btn btn-danger" ID="btnClose" runat="server" Text="X" OnClick="btnClose_Click" />
                                            </div>
                                            <div class="modal-body">
                                                <div class="card">
                                                    <div class="row ">
                                                        <div class="col-md-6">
                                                            <label class="font-weight-bold">Template ID :</label>
                                                            <asp:Label ID="lbltempid"  runat="server"></asp:Label>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <label class="font-weight-bold">Template Name :</label>
                                                            <asp:Label ID="lbltempName"  runat="server"></asp:Label>
                                                        </div>
                                                        <div class="col-md-12">
                                                            <label class="font-weight-bold">Template Text :</label>
                                                            <asp:Label ID="lbltempText"  runat="server"></asp:Label>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <label class="font-weight-bold">Mob No :</label>
                                                            <asp:TextBox ID="txtMobNo" runat="server" CssClass="form-control" MaxLength="10" onkeypress="return /^[0-9,]+$/.test(event.key) "></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-6 mb-2">
                                                            <label class="font-weight-bold">User Id :</label>
                                                            <asp:TextBox ID="txtUserId" runat="server" CssClass="form-control" MaxLength="30"></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-6 mb-2">
                                                            <label class="font-weight-bold">Sender Id :</label>
                                                            <asp:DropDownList ID="ddlSenderId" CssClass="form-control" runat="server"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="modal-footer">
                                                <div class="card-footer">
                                                    <asp:LinkButton runat="server" ID="btnSend" Text="Send" CssClass="btn btn-Success" OnClick="btnSend_Click" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <%--<asp:RadioButton ID="rbPanel" runat="server" Text="PANEL" GroupName="radio"  OnCheckedChanged="rbAPI_CheckedChanged"/>--%>
                                
                            </div>
                           <%-- <div class="card bg-primary border-light shadow-soft mb-4">
                                    
                                </div>--%>

                                <div class="card bg-primary border-light shadow-soft mb-4 py-3 pl-3" runat="server" id="divAPI">

                                    <div class="row py-3 pl-3">
                                        <div class="col-md-6 p-2 input-group">
                                            <label class="font-weight-bold col-md-3">Template Types : </label>
                                            <asp:RadioButtonList ID="rblType" runat="server" AutoPostBack="true" RepeatDirection="Horizontal" OnSelectedIndexChanged="rblType_SelectedIndexChanged">
                                                <asp:ListItem Text="API" Value="API"></asp:ListItem>
                                                <asp:ListItem Text="SMS PANEL" Value="PANEL"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                    </div>

                                    <asp:GridView ID="grvAPI" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
                                        runat="server" Width="100%" CellPadding="10" BorderColor="#ede8e8" Class="table table-striped table-bordered dt-responsive wrap dataTable-view">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Template Id" HeaderStyle-Width="15%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTemplateId" runat="server" Width="100%" Text='<%#Eval("templateID")%>'></asp:Label>
                                                    <asp:HiddenField ID="hfTemplateId" Value='<%# Eval("templateID") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Template Name" HeaderStyle-Width="25%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTemplateName" runat="server" Text='<%#Eval("templateName")%>' Width="100%"></asp:Label>
                                                    <asp:HiddenField ID="hftemplatename" Value='<%# Eval("templateName") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Template text" HeaderStyle-Width="60%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTemplateText" runat="server" Text='<%#Eval("msgtext")%>' Width="100%"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Text" HeaderStyle-Width="20%">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkTest" runat="server" class="mx-1 btn btn-primary table-success" OnClick="lnkTest_Click"
                                                        data-toggle="tooltip" data-placement="top" title="" data-original-title="Send SMS"> 
                                                         <span class="text-success"> <i class="fas fa-paper-plane"></i> </span></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td align="center">
                                                        <asp:Label ID="lblEmpty" Text="No Data Found!!!" Style="color: Red;" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </div>

                        </div>
                    </div>
                </div>

                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="rblType" />
            </Triggers>
        </asp:UpdatePanel>
        <script>
            function myFunction() {
                document.getElementById("UserId1").classList.remove('display1');
                event.preventDefault();

            }
            function myFunction1() {
                document.getElementById("UserId2").classList.remove('display2');
                event.preventDefault();
            }
            function myFunction2() {
                document.getElementById("UserId3").classList.remove('display3');
                event.preventDefault();

            }
            function myFunction3() {
                document.getElementById("UserId4").classList.remove('display4');
                event.preventDefault();
            }
            function myFunction4() {
                document.getElementById("UserId5").classList.remove('display5');
                event.preventDefault();

            }

            //FOR SENDER
            function myFunctionSen() {
                document.getElementById("SenderId1").classList.remove('displaySem1');
                event.preventDefault();

            }
            function myFunctionSen1() {
                document.getElementById("SenderId2").classList.remove('displaySem2');
                event.preventDefault();
            }
            function myFunctionSen2() {
                document.getElementById("SenderId3").classList.remove('displaySem3');
                event.preventDefault();

            }
            function myFunctionSen3() {
                document.getElementById("SenderId4").classList.remove('displaySem4');
                event.preventDefault();
            }
            function myFunctionSen4() {
                document.getElementById("SenderId5").classList.remove('displaySem5');
                event.preventDefault();

            }

            function MyFunction() {
                var b = document.getElementById("txtuserid2").value;
                var c = document.getElementById("txtuserid3").value;
                var d = document.getElementById("txtuserid4").value;
                var e = document.getElementById("txtuserid5").value;
                var f = document.getElementById("txtuserid6").value;

                var g = document.getElementById("txtSenderId2").value;
                var h = document.getElementById("txtSenderId3").value;
                var i = document.getElementById("txtSenderId4").value;
                var j = document.getElementById("txtSenderId5").value;
                var k = document.getElementById("txtSenderId6").value;

                if (b != "") {
                    document.getElementById("UserId1").classList.remove('display1');
                    event.preventDefault();
                }
                if (c != "") {
                    document.getElementById("UserId2").classList.remove('display2');
                    event.preventDefault();
                }
                if (d != "") {
                    document.getElementById("UserId3").classList.remove('display3');
                    event.preventDefault();
                }
                if (e != "") {
                    document.getElementById("UserId4").classList.remove('display4');
                    event.preventDefault();
                }
                if (f != "") {
                    document.getElementById("UserId5").classList.remove('display5');
                    event.preventDefault();
                }


                if (g != "") {
                    document.getElementById("SenderId1").classList.remove('displaySem1');
                    event.preventDefault();
                }
                if (h != "") {
                    document.getElementById("SenderId2").classList.remove('displaySem2');
                    event.preventDefault();
                }
                if (i != "") {
                    document.getElementById("SenderId3").classList.remove('displaySem3');
                    event.preventDefault();
                }
                if (j != "") {
                    document.getElementById("SenderId4").classList.remove('displaySem4');
                    event.preventDefault();
                }
                if (k != "") {
                    document.getElementById("SenderId5").classList.remove('displaySem5');
                    event.preventDefault();
                }



            }
        </script>

        <script type="text/javascript">
            function Confirm1() {
                $('#mt1').modal('show');
            }
            function HideModal1() {
                $('#mt1').modal('hide');
            }
        </script>

        <script src="vendor/jquery-easing/jquery.easing.min.js"></script>

        <!--  Select-->
        <script src="vendor/select/bootstrap-select.min.js"></script>

        <!-- Page level custom scripts -->
        <script src="js/demo/datatables-demo.js"></script>

        <script src="vendor/datepicker/bootstrap-datepicker.js"></script>

        <script src="vendor/bootstrap/js/bootstrap.bundle.min.js"></script>
        <script src="js/sb-admin-2.min.js"></script>
        

        <script src="vendor/jquery/jquery-3.5.1.min.js"></script>

        <!-- Page level custom scripts -->
        <script src="js/demo/datatables-demo.js"></script>

        <script src="vendor/datepicker/bootstrap-datepicker.js"></script>

       
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
        <script src="js/demo/datatables-demo.js"></script>


    </form>
</body>
</html>
