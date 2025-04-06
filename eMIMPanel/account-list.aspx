<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="account-list.aspx.cs" Inherits="eMIMPanel.account_list" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<form runat="server">--%>
    <cc1:ToolkitScriptManager ID="scrmg" runat="server" EnablePageMethods="true">
    </cc1:ToolkitScriptManager>
    <main>
        <div class="container-fluid">
            <nav aria-label="breadcrumb" class="my-3">
                <ol class="breadcrumb breadcrumb-info">
                    <li class="breadcrumb-item"><a href="#">Home</a></li>
                    <li class="breadcrumb-item active" aria-current="page">Account List</li>
                </ol>
            </nav>

            <!-- Start Row -->
            <div class="row">
                <div class="col">
                    <div class="card card-body bg-primary border-light shadow-soft mb-4">
                        <!--  -->
                        <div class="row align-items-center">
                            <div class="col-12 col-md-2">
                                <asp:RadioButtonList ID="rblFilter" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem style="margin-right: 25px" Text="Admin" Value="admin"></asp:ListItem>
                                    <asp:ListItem Text="User" Value="user" Selected="True"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                            <div class="col-12 col-md-3">
                                <asp:TextBox ID="txtFrm" runat="server" onchange="javascript:text_changed_from();" class="form-control datepicker" placeholder="From Date" autocomplete="off"></asp:TextBox>
                                <asp:HiddenField ID="hdntxtFrm" runat="server" />
                            </div>
                            <div class="col-12 col-md-3">
                                <asp:TextBox ID="txtTo" runat="server" onchange="javascript:text_changed_to();" class="form-control datepicker my-3 my-lg-0" placeholder="To Date" autocomplete="off"></asp:TextBox>
                                <asp:HiddenField ID="hdntxtTo" runat="server" />
                            </div>
                                <div class="col-12 col-md-3">
                                <asp:TextBox ID="txtname" runat="server"  class="form-control" placeholder="Name" autocomplete="off"></asp:TextBox>
                                
                            </div>
                            <div class="col-12 col-md-2"></div>
                            <div class="col-12 col-md-3">
                                <asp:TextBox ID="txtmobile" runat="server" class="form-control" placeholder="Mobile" autocomplete="off"></asp:TextBox>
                               
                            </div>
                            <div class="col-12 col-md-3">
                                <asp:TextBox ID="txtemailid" runat="server"  class="form-control" placeholder= "Email" autocomplete="off"></asp:TextBox>
                                
                            </div>
                            <div class="col-12 col-md-3">
                                <asp:TextBox ID="TemplateID" runat="server"  class="form-control" placeholder=" Template ID" autocomplete="off"></asp:TextBox>
                                
                            </div>

                            <div class="col-6 col-md-2">
                                <asp:LinkButton runat="server" ID="LinkButton2" OnClick="btnUpdate_Click" class="btn btn-block">
                                            Show <i class="fas fa-eye" aria-hidden="true"></i>
                                </asp:LinkButton>

                            </div>
                            <div class="col-6 col-md-2">
                                <asp:LinkButton runat="server" ID="lnkDownload" OnClick="lnkDownload_Click" class="btn btn-block">
                                     <i class="fas fa-download" aria-hidden="true"></i> Download
                                </asp:LinkButton>
                            </div>
                            <asp:HiddenField ID="h1" runat="server" />
                            <asp:HiddenField ID="h2" runat="server" />
                        </div>
                        <!--  -->
                    </div>
                </div>
            </div>
            <!-- End Row -->

            <!-- Start Row -->
            <div class="row">
                <div class="col-xl-12 col-lg-12">
                    <!-- Card Start -->
                    <div class="card bg-primary border-light shadow-soft mb-4">
                        <div class="card-header py-3 bg-primary d-flex justify-content-between flex-wrap align-content-center">
                            <h6 class="m-0 font-weight-bold my-auto"> Account List</h6>
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
                                  <%--  <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton1" runat="server" class="mx-1 btn btn-primary text-success"
                                                OnClick="btnView_Click" data-toggle="tooltip" data-placement="top" title="" data-original-title="View"> 
                                                        <span class="text-success"> <i class="fas fa-key"></i> </span></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="Company Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl1" runat="server" Text='<%#Eval("compname")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="User ID">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hdnpwd" runat="server" Value='<%#Eval("pwd")%>' />
                                            <asp:Label ID="lblUserId" runat="server" Text='<%#Eval("username")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblname" runat="server" Text='<%#Eval("fullname")%>'></asp:Label>
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
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="A/C Status">
                                        <ItemTemplate>
                                            <asp:Label ID="lblstat" runat="server" Text='<%#Eval("status")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Created By">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl6" runat="server" Text='<%#Eval("createdby")%>'></asp:Label>
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
            <!-- End Row -->

        </div>

        <asp:HiddenField ID="hdn1q" runat="server" EnableViewState="False" ViewStateMode="Disabled" />
        <cc1:ModalPopupExtender ID="modalpopuppwd" runat="server" TargetControlID="hdn1q"
            PopupControlID="pnlpopuppwd" BackgroundCssClass="modelpopupback" CancelControlID="Button12">
        </cc1:ModalPopupExtender>

        <div class="col-md-12">
            <asp:Panel ID="pnlpopuppwd" runat="server" Style="width: 50%; height: 370px; overflow: auto; display: none;"
                align="center">
                <div class="col-md-12 no-spacing">
                    <table id="tblpopup" runat="server" style="width: 100%; background: white;">
                        <tr>
                            <td>
                                <div class="col-md-12">
                                    <asp:Label ID="lblheadpopup" runat="server" Text="User ID & Password"></asp:Label>
                                    <asp:Button ID="Button12" runat="server" Text="X" Style="float: right;" CssClass="popclosebutton" />
                                </div>

                                <div class="modal-body">
                                    <form class="needs-validation" novalidate>
                                        <div class="form-group">
                                            <label for="formGroupExampleInput">User Id</label>
                                            <!-- <input type="text" class="form-control" id="formGroupExampleInput" placeholder="User Id"> -->
                                            <div class="input-group mb-3">
                                                <asp:TextBox ID="txtusername" runat="server" type="text" class="form-control" placeholder="User Id" aria-label="User Id" aria-describedby="button-addon2"></asp:TextBox>
                                                <div class="input-group-append">
                                                    <button class="btn btn-primary text-success" type="button" id="button-addon2"><i class="fas fa-eye"></i></button>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="formGroupExampleInput2">Password</label>
                                            <!-- <input type="text" class="form-control" id="formGroupExampleInput2" placeholder="***********"> -->
                                            <div class="input-group mb-3">
                                                <asp:TextBox ID="txtpwd" runat="server" type="text" class="form-control" placeholder="***********" aria-label="Password" aria-describedby="button-addon2"></asp:TextBox>
                                                <div class="input-group-append">
                                                    <button class="btn btn-primary text-success" type="button" id="button-addon2"><i class="fas fa-eye"></i></button>
                                                </div>
                                            </div>
                                        </div>
                                        <asp:TextBox ID="myinput" runat="server" class="form-control"></asp:TextBox>
                                    </form>
                                </div>
                                <div class="modal-footer py-3">
                                    <asp:LinkButton ID="lnkCopy" runat="server" OnClientClick="copyToClipboard('myinput')" class="btn btn-sm btn-primary text-success"><i class="fas fa-copy"></i> Copy</asp:LinkButton>
                                    <%--<a href="#" onClick="copyfieldvalue(event, 'myinput');return false" class="btn btn-sm btn-primary text-success"><i class="fas fa-copy"></i>Copy</a>--%>
                                    <%--<a href="#" onClick="copyToClipboard('myinput')" class="btn btn-sm btn-primary text-success"><i class="fas fa-copy"></i>Copy</a>--%>
                                    <%--<button type="button" class="btn btn-sm btn-primary text-success" onclick="funcopy()"><i class="fas fa-copy"></i>Copy</button>--%>
                                    <asp:Button type="button" class="btn btn-primary text-danger ml-auto" data-dismiss="modal" runat="server" OnClick="btnClosePopup_Click" Text="Close" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
        </div>
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

        function copyToClipboard(element) {
            var $temp = $("<input>");
            $("body").append($temp);
            $temp.val($(element).text()).select();
            document.execCommand("copy");
            $temp.remove();
        }

        function copyfield() {
            var copysuccess // var to check whether execCommand successfully executed
            try {
                copysuccess = document.execCommand("copy") // run command to copy selected text to clipboard
            } catch (e) {
                copysuccess = false
            }
            return copysuccess
        }

        function funcopy() {
            /* Get the text field */
            var copyText = document.getElementById('myinput');

            /* Select the text field */
            //copyText.focus();
            //copyText.select();
            copyText.setSelectionRange(0, copyText.value.length); /*For mobile devices*/

            /* Copy the text inside the text field */
            document.execCommand('copy');

            /* Alert the copied text */
            //alert("Copied the text: " + copyText.value);
        }
        function copyfieldvalue(e, id) {
            var field = document.getElementById("myinput")
            field.focus()
            field.setSelectionRange(0, field.value.length)
            var copysuccess = copySelectionText()
            if (copysuccess) {
                showtooltip(e)
            }

            window.setTimeout(function () {
                document.getElementById('myinput').focus();
            }, 0);
        }
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
            var fromDate = $("#<%= txtFrm.ClientID %>").datepicker("getDate");
            var toDate = $("#<%= txtTo.ClientID %>").datepicker("getDate");
            // Check if From Date is greater than To Date
            if (fromDate > toDate) {
                alert("To Date cannot be lesser than From Date !");
                document.getElementById("ContentPlaceHolder1_txtTo").value = '';
                document.getElementById("ContentPlaceHolder1_txtTo").value = '';
                return false;
            }
            return true;
        }

    </script>
</asp:Content>
