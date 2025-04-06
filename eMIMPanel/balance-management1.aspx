<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="balance-management1.aspx.cs" Inherits="eMIMPanel.balance_management1" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>MIM Balance Management</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
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
                        <div class="card-header py-3 bg-primary">
                            <h6 class="m-0 font-weight-bold">Balance List</h6>
                        </div>
                        <div class="card-body">
                            <div class="table-responsive">
                                <asp:HiddenField ID="hdnUserName" runat="server" />
                                <asp:HiddenField ID="hidden" runat="server" />
                                <asp:HiddenField ID="hidden2" runat="server" />
                                <asp:HiddenField ID="hs1" runat="server" />
                                <asp:HiddenField ID="hs2" runat="server" />
                                <asp:HiddenField ID="hs3" runat="server" />
                                <asp:HiddenField ID="hs4" runat="server" />
                                <asp:HiddenField ID="hdnUrlRate" runat="server" />
                                <asp:HiddenField ID="hdnDltRate" runat="server" />
                                <input type="hidden" id="hiddenfieldid" runat="server" />
                                <table class="table table-striped table-bordered dt-responsive wrap display table" id="balanceTable" cellspacing="0">
                                    <thead>
                                        <tr>
                                            <th>Sr. No</th>
                                            <th>Action</th>
                                            <th>Company Name</th>
                                            <th>Full Name</th>
                                            <th>User Id</th>
                                            <th>Sender Id</th>
                                            <th>Mobile No</th>
                                            <th>Email ID</th>
                                            <th>Balance</th>
                                        </tr>
                                    </thead>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </main>
       
   <%-- <form class="needs-validation" novalidate>--%>
        <!-- SMS Rate Management Modal -->
        <div class="modal fade" id="modal-default" tabindex="-1" role="dialog" aria-labelledby="modal-default" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h2 class="h6 modal-title mb-0" id="modal-title-default">SMS Rate Management</h2>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">×</span>
                        </button>
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
                    <div class="modal-footer">
                        <%--<button type="button" class="btn btn-sm btn-primary" runat="server" onclick="" onserverclick="btnUpdateRate_Click"><i class="fas fa-user-edit"></i>Update</button>--%>
                        <asp:LinkButton id="btnUpdateRate" runat="server" class="btn btn-sm btn-primary" OnClientClick="return UpdateRate();" onclick="btnUpdateRate_Click"><i class="fas fa-user-edit"></i>Update</asp:LinkButton>
                        <button type="button" class="btn btn-primary text-danger ml-auto" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
        <!-- End of SMS Rate Management Modal Content -->

        <!--  Balance Management Modal -->
        <div class="modal fade" id="modal-balance" tabindex="-1" role="dialog" aria-labelledby="modal-default" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h2 class="h6 modal-title mb-0" id="modal-title-default">Balance Management</h2>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">×</span>
                        </button>
                    </div>
                    <div class="modal-body">

                        <div class="form-row">
                            <div class="col-md-12">
                                User Name :
                                <asp:Label ID="lbluser" runat="server"></asp:Label>
                                <%--<asp:TextBox ID="txtUser" runat="server"></asp:TextBox>--%>
                            </div>
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
                    <div class="modal-footer py-3">
                        <button type="button" class="btn btn-sm btn-primary" runat="server" onserverclick="btnUpdate_Click"><i class="fas fa-user-edit"></i>Update</button>
                        <button type="button" class="btn btn-primary text-danger ml-auto" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
        <!-- End of  Balance Management Content -->
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
        function UpdateRate() {
            var s1 = document.getElementById('<%= txtns1.ClientID %>').value;
            var s2 = document.getElementById('<%= txtns2.ClientID %>').value;
            var s3 = document.getElementById('<%= txtns3.ClientID %>').value;
            var s4 = document.getElementById('<%= txtns4.ClientID %>').value;
            var s5 = document.getElementById('<%= txtUrlRateN.ClientID %>').value;
            var s6 = document.getElementById('<%= txtnd1.ClientID %>').value;
            if (s1 == "" && s2 == "" && s3 == "" && s4 == "" && s5 == "" && s6 == "") {
                alert('Please enter new price.');
                return false;
            }
            else
                return true;           
        }

        function show() {
            var hv = $('#hdnUserName').val();
            alert(hv);
            return true;
        }

        $(document).ready(function () {

            if ($.fn.DataTable.isDataTable('#balanceTable')) {
                $('#balanceTable').DataTable().destroy();
            }
            $('#balanceTable tbody').empty();


            var user = '<% =Session["User"].ToString() %>';
            var dlt = '<% =Session["DLT"].ToString() %>';

            $.ajax({
                type: "POST",
                dataType: "json",
                url: "WebService.asmx/GetCustomersWithBalance",
                data: { dater: user + '$' + dlt },
                success: function (data) {
                    var datatableVariable = $('#balanceTable').DataTable({
                        data: data,
                        columns: [
                            { 'data': 'Sln' },
                            {
                                'data': null, 'bSortable': false, 'mRender': function (data, type, full) {
                                    return '<a href="#" class="mx-1 btn btn-primary text-secondary" data-toggle="modal" data-id="' + full.UserName + '" data-target="#modal-balance"> <i class="far fa-credit-card"></i></a>';
                                }
                            },                           
                            { 'data': 'CompName' },
                            { 'data': 'Name' },
                            { 'data': 'UserName' },
                            { 'data': 'SenderID' },
                            { 'data': 'Mobile1' },
                            { 'data': 'Email' },
                            { 'data': 'Balance' },
                        ]
                    });

                }
            });

            $("#modal-balance").on('show.bs.modal', function (e) {
                var triggerLink = $(e.relatedTarget);
                var id = triggerLink.data("id");
                document.getElementById('ContentPlaceHolder1_lbluser').innerHTML = id;
                document.getElementById('<%= txtbal.ClientID %>').value="";
                document.getElementById('<%= hidden.ClientID %>').value = id;
                //var fieldname = triggerLink.data("fieldname");
                //$(this).find(".modal-body").html("<h5>id: "+id+"</h5>");
            });

            $("#modal-default").on('show.bs.modal', function (e) {
                
                    var triggerLink = $(e.relatedTarget);
                    var id = triggerLink.data("id");
                    var fields = id.split('$');
                    var u = fields[0];
                    var s1 = fields[1];
                    var s2 = fields[2];
                    var s3 = fields[3];
                    var s4 = fields[4];
                    var s5 = fields[5];
                    var s6 = fields[6];
                    //document.getElementById('ContentPlaceHolder1_lblUser1').innerHTML = u;
                    document.getElementById('<%= hidden2.ClientID %>').value = u;
                    document.getElementById('<%= hs1.ClientID %>').value = s1;
                    document.getElementById('<%= hs2.ClientID %>').value = s2;
                    document.getElementById('<%= hs3.ClientID %>').value = s3;
                    document.getElementById('<%= hs4.ClientID %>').value = s4;
                    document.getElementById('<%= hdnUrlRate.ClientID %>').value = s5;
                    document.getElementById('<%= hdnDltRate.ClientID %>').value = s6;

                    document.getElementById('<%= txts1.ClientID %>').value = s1;
                    document.getElementById('<%= txts2.ClientID %>').value = s2;
                    document.getElementById('<%= txts3.ClientID %>').value = s3;
                    document.getElementById('<%= txts4.ClientID %>').value = s4;
                    document.getElementById('<%= txtUrlRate.ClientID %>').value = s5;
                    document.getElementById('<%= txtd1.ClientID %>').value = s6;

                    document.getElementById('<%= txtns1.ClientID %>').value = "";
                    document.getElementById('<%= txtns2.ClientID %>').value = "";
                    document.getElementById('<%= txtns3.ClientID %>').value = "";
                    document.getElementById('<%= txtns4.ClientID %>').value = "";
                    document.getElementById('<%= txtnd1.ClientID %>').value = "";
                    document.getElementById('<%= txtUrlRateN.ClientID %>').value = "";
                
                //var fieldname = triggerLink.data("fieldname");
                //$(this).find(".modal-body").html("<h5>id: "+id+"</h5>");
            });

        });
    </script>
</asp:Content>
