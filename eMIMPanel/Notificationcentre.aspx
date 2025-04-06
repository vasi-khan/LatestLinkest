<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="Notificationcentre.aspx.cs" MaintainScrollPositionOnPostback="true" Inherits="eMIMPanel.Notificationcentre" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <main>
        <div class="container-fluid">
            <nav aria-label="breadcrumb" class="my-3">
                <ol class="breadcrumb breadcrumb-info">
                    <li class="breadcrumb-item"><a href="#">Home</a></li>
                    <li class="breadcrumb-item active" aria-current="page">Notifications</li>
                </ol>
            </nav>

            <!-- Content Row -->
            <div class="row">
                <div class="col-xl-12 col-lg-12">
                    <!-- Basic Card Example -->
                    <div class="card bg-primary border-light shadow-soft mb-4">
                        <div class="card-header py-3 bg-primary d-flex justify-content-between flex-wrap align-content-center">
                            <h6 class="font-weight-bold my-lg-auto mb-3"><i class="fas fa-bell"></i>Schedule Notification</h6>
                        </div>
                        <div class="card-body"> 

                            <fieldset class="form-group mb-3">
                                <div class="row">
                                    <legend class="col-form-label col-sm-2 pt-0 font-weight-bold">Balance</legend>
                                    <div class="col-sm-3">
                                        <asp:Label ID="Label1" runat="server" Text="Minimum balance"></asp:Label>
                                        <!--  -->
                                        <%-- <asp:DropDownList ID="ddlbalance" runat="server" class="custom-select" OnSelectedIndexChanged="ddlbalance_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Text="Minimun balance" Value="1" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Set % of last recharge" Value="2"></asp:ListItem>
                                        </asp:DropDownList>--%>
                                        <!--  -->
                                    </div>
                                    <div class="col-sm-3 right-7">
                                        <div class="input-group">
                                            <asp:TextBox ID="txtbalance" runat="server" TextMode="Number" MaxLength="12" autocomplete="Off" class="form-control" placeholder="" aria-label="six alphabet" aria-describedby="button-addon2"></asp:TextBox>

                                        </div>
                                    </div>
                                    <!--  -->
                                    <div class="col-sm-3 right-7">
                                        <%-- <button type="submit" class="btn btn-primary text-secondary font-weight-bold mr-3"><i class="fas fa-plus text-secondary"></i>Add</button>--%>
                                        <asp:LinkButton ID="LinkButton5" runat="server" class="btn btn-outline-secondary" OnClick="button5_Click"><i class="fas fa-plus-circle"></i></asp:LinkButton>

                                        <!-- <button type="submit" class="btn btn-primary text-danger font-weight-bold"><i class="fas fa-minus text-danger"></i> Remove</button> -->
                                    </div>

                                    <!--  -->
                                </div>
                            </fieldset>
                            <fieldset class="form-group mb-4">
                                <div class="row" id="Sen6" runat="server" style="display: none">
                                    <legend class="col-form-label col-sm-2 pt-0 font-weight-bold">Balance</legend>

                                    <div class="col-sm-3">
                                        <asp:Label ID="Label2" runat="server" Text="Set % of last recharge"></asp:Label>

                                        <%--<asp:DropDownList ID="ddlbalance1" runat="server" class="custom-select" AutoPostBack="true">
                                            <asp:ListItem Text="Minimun balance" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Set % of last recharge" Value="2" Selected="True"></asp:ListItem>
                                        </asp:DropDownList>--%>
                                    </div>
                                    <div class="col-sm-3  right-7">
                                        <div class="input-group">
                                            <asp:TextBox ID="txtper" runat="server" MaxLength="3" autocomplete="Off" class="form-control" aria-label="six alphabet" aria-describedby="button-addon2" onchange="handleChange(this);"></asp:TextBox>
                                        </div>
                                        <%-- <asp:RangeValidator runat="server" ControlToValidate="txtper" ErrorMessage="Enter number between 1 to 100"
                                            Type="Integer" MinimumValue="1" MaximumValue="100" ForeColor="Red"></asp:RangeValidator>--%>
                                    </div>
                                    <div class="col-sm-3 right-7">
                                        <asp:LinkButton ID="LinkButton6" runat="server" class="btn btn-outline-secondary" OnClick="button6_Click"><i class="fas fa-minus-circle"></i></asp:LinkButton>
                                    </div>
                                    <div class="col-sm-12  left-9">
                                        <asp:Label ID="Label3" class="col-form-label col-sm-2 pt-0 font-weight-bold"
                                            runat="server" Text="Higher balance of the above two would be considered for Notification alert"></asp:Label>
                                        <%--  <legend class="col-form-label col-sm-2 pt-0 font-weight-bold">Higher of the above two</legend>--%>
                                    </div>
                                </div>

                            </fieldset>
                            <fieldset class="form-group mb-4">
                                <div class="row">
                                    <legend class="col-form-label col-sm-2 pt-0 font-weight-bold">Notification Type</legend>
                                    <div class="col-sm-1">
                                        <a href="#" class="btn text-dark btn-icon-split mr-3 p-2 btn-block text-left">
                                            <span class="text-dark">
                                                <asp:CheckBox ID="chkall" runat="server" OnCheckedChanged="chkall_CheckedChanged" AutoPostBack="true" />
                                            </span>
                                            <span class="text font-weight-bold">All</span>
                                        </a>
                                    </div>
                                    <div class="col-sm-2">
                                        <a href="#" class="btn text-dark btn-icon-split mr-3 btn-block text-left">
                                            <span class="text-dark">
                                                <asp:CheckBox ID="chksms" runat="server" />
                                            </span>
                                            <span class="text font-weight-bold">SMS</span>
                                        </a>
                                    </div>
                                    <div class="col-sm-2">
                                        <a href="#" class="btn text-dark btn-icon-split mr-3 btn-block text-left">
                                            <span class="text-dark">
                                                <asp:CheckBox ID="chkemail" runat="server" />
                                            </span>
                                            <span class="text font-weight-bold">Email</span>
                                        </a>
                                    </div>
                                    <div class="col-sm-2">
                                        <a href="#" class="btn text-dark btn-icon-split mr-3 btn-block text-left">
                                            <span class="text-dark">
                                                <asp:CheckBox ID="chkvoice" runat="server" />
                                            </span>
                                            <span class="text font-weight-bold">Voice</span>
                                        </a>
                                    </div>
                                    <!--  -->
                                    <div class="col-sm-2">
                                        <a href="#" class="btn text-dark btn-icon-split mr-3 btn-block text-left">
                                            <span class="text-dark">
                                                <asp:CheckBox ID="chkwhatsapp" runat="server" />
                                            </span>
                                            <span class="text font-weight-bold">WhatsApp</span>
                                        </a>
                                    </div>
                                    <!--  -->
                                </div>
                            </fieldset>
                        </div>
                    </div>
                </div>
            </div>
            <!-- End Content Row -->

            <!-- 2 Content Start -->
            <div class="row">
                <div class="col-12">
                    <div class="card bg-primary border-light shadow-soft mb-4">
                        <div class="card-body">
                            <!--  -->
                            <!--  -->
                            <fieldset class="form-group mb-4">
                                <div class="row"> 
                                    <div class="col-md-12 card-header py-3 bg-primary d-flex justify-content-between flex-wrap align-content-center">
                                        <h6 class="font-weight-bold my-lg-auto mb-3"><i class="fas fa-plus"></i> Add Members</h6>
                                    </div> 

                                    <div class="col-sm-3 col-md-3">
                                        <!--  -->
                                        <div class="input-group mb-3">
                                            <div class="input-group-prepend">
                                                <span class="input-group-text" id="basic-addon1"><i class="fas fa-user"></i>&nbsp; Name</span>
                                            </div>
                                            <asp:TextBox ID="txtname" runat="server" autocomplete="Off" class="form-control" placeholder="" aria-label="six alphabet" aria-describedby="button-addon2"></asp:TextBox>

                                        </div>
                                        <!--  -->
                                    </div>
                                    <div class="col-sm-3 col-md-3">
                                        <!--  -->
                                        <div class="input-group mb-3">
                                            <div class="input-group-prepend">
                                                <span class="input-group-text" id="basic-addon1"><i class="fas fa-phone-alt"></i>&nbsp; Mobile No</span>
                                            </div>
                                            <asp:TextBox ID="txtmobileno" runat="server" MaxLength="12" onkeypress="return numeric(event);" autocomplete="Off" class="form-control" placeholder="" aria-label="six alphabet" aria-describedby="button-addon2"></asp:TextBox>
                                        </div>
                                        <!--  -->
                                    </div>
                                    <div class="col-sm-4 col-md-5">
                                        <!--  -->
                                        <div class="input-group mb-3">
                                            <div class="input-group-prepend">
                                                <span class="input-group-text" id="basic-addon1"><i class="fas fa-envelope"></i>&nbsp; Email Id</span>
                                            </div>
                                            <asp:TextBox ID="txtemail" runat="server" autocomplete="Off" class="form-control" aria-label="six alphabet" aria-describedby="button-addon2"></asp:TextBox>
                                             
                                        </div>
                                        <asp:RegularExpressionValidator ID="validateEmail"
                                                runat="server" ErrorMessage="Please provide a valid Email Address !!."
                                                ControlToValidate="txtemail" ForeColor="Red"
                                                ValidationExpression="^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$" />
                                    </div>
                                    <div class="col-sm-1  col-md-1">
                                        <asp:Button ID="btnsubmit" Text="Submit" runat="server" OnClick="btnsubmit_Click" CssClass="btn btn-primary" />
                                    </div>

                                    <!--  -->


                                </div>
                                <div class="row">
                                </div>
                            </fieldset>
                            <!--  -->
                            <fieldset class="form-group mb-4">
                                <div class="row">
                                    <legend class="col-form-label col-sm-2 pt-0 font-weight-bold">Members List</legend>
                                    <div class="col-sm-10">
                                        <!--  -->

                                        <div id="exportToExcel" runat="server" class="table-responsive">
                                            <asp:GridView ID="grv" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
                                                runat="server" Width="100%" CellPadding="10"
                                                BorderColor="#ede8e8">
                                                <Columns>
                                                    <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Sr No">
                                                        <ItemTemplate>

                                                            <asp:Label ID="lblSeq" runat="server" Text='<%#Eval("Rownumber")%>'></asp:Label>

                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblName" runat="server" Text='<%#Eval("column1")%>'></asp:Label>

                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Mobile">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblmobile" runat="server" Text='<%#Eval("column2")%>'></asp:Label>

                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Email">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEmail" runat="server" Text='<%#Eval("column3")%>'></asp:Label>

                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Edit">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click"><i class="fas fa-edit"></i></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Remove">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="LinkButton2" runat="server" OnClick="LinkButton2_Click"
                                                                OnClientClick="return confirm('Are you sure you wish to remove this Record ?');"><i class="fas fa-times"></i></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <EditRowStyle BackColor="#7C6F57" />
                                                <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                                                <HeaderStyle BackColor="#dbd7d7" Font-Bold="True" ForeColor="Black" Height="50px" />
                                                <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                                                <RowStyle />
                                                <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                                <SortedAscendingCellStyle BackColor="#F8FAFA" />
                                                <SortedAscendingHeaderStyle BackColor="#246B61" />
                                                <SortedDescendingCellStyle BackColor="#D4DFE1" />
                                                <SortedDescendingHeaderStyle BackColor="#15524A" />
                                                <EmptyDataTemplate>
                                                    <table style="width: 100%;">
                                                        <tr>
                                                            <%--<td align="center">
                                                                <asp:Label ID="Label1" Text="No Data Found!!!" Style="color: Red;" runat="server"></asp:Label>
                                                            </td>--%>
                                                        </tr>
                                                    </table>
                                                </EmptyDataTemplate>
                                            </asp:GridView>
                                            <table class="table table-bordered" id="dataTable" runat="server" width="100%" cellspacing="0">
                                            </table>
                                        </div>

                                        <!--  -->
                                    </div>

                                </div>
                                <div class="row">
                                    <div class="col-sm-2">
                                        <asp:Button ID="Save" Text="Save" runat="server" OnClick="Save_Click" CssClass="btn btn-primary" />
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:Button ID="btnReset" Text="Reset" runat="server" OnClick="btnReset_Click" CssClass="btn btn-primary" />
                                    </div>
                                </div>
                            </fieldset>
                            <div class="row" style="color: red">
                                <div class="col-sm-3">
                                    SMS -
                                    <asp:Label ID="lblSMS1" runat="server"></asp:Label>
                                </div>
                                <div class="col-sm-3">
                                    EMAIL -
                                     <asp:Label ID="lblEMAIL1" runat="server"></asp:Label>
                                </div>
                                <div class="col-sm-3">
                                    VOICE -
                                     <asp:Label ID="lblVOICE1" runat="server"></asp:Label>
                                </div>
                                <div class="col-sm-3">
                                    WHATSAPP -
                                     <asp:Label ID="lblWHATSAPP1" runat="server"></asp:Label>
                                </div>
                            </div>

                            <!--  -->
                            <!--  -->
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </main>

    <!-- Bootstrap core JavaScript-->
    <script src="vendor/jquery/jquery-3.5.1.min.js"></script>
    <script src="vendor/bootstrap/js/bootstrap.bundle.min.js"></script>

    <!-- Core plugin JavaScript-->
    <script src="vendor/jquery-easing/jquery.easing.min.js"></script>

    <!--  Date-->
    <script src="vendor/datepicker/moment.min.js"></script>
    <script src="vendor/datepicker/daterangepicker.min.js"></script>



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


    <script>
        // Left Sidebar
        (function ($) {
            "use strict";

            // Add active state to sidbar nav links
            var path = window.location.href; // because the 'href' property of the DOM element is the absolute path
            $("#layoutSidenav_nav .sb-sidenav a.nav-link").each(function () {
                if (this.href === path) {
                    $(this).addClass("active");
                }
            });

            // Toggle the side navigation
            $("#sidebarToggle").on("click", function (e) {
                e.preventDefault();
                $("body").toggleClass("sb-sidenav-toggled");
            });
        })(jQuery);
    </script>
    <script>
        //Datatable Search Filter
        $(document).ready(function () {
            // Setup - add a text input to each footer cell
            $('#dataTable3 thead tr').clone(true).appendTo('#dataTable3 thead');
            $('#dataTable3 thead tr:eq(1) th').each(function (i) {
                var title = $(this).text();
                $(this).html('<input type="text" placeholder="Search ' + title + '" />');

                $('input', this).on('keyup change', function () {
                    if (table.column(i).search() !== this.value) {
                        table
                            .column(i)
                            .search(this.value)
                            .draw();
                    }
                });
            });

            var table = $('#dataTable3').DataTable({
                orderCellsTop: true,
                fixedHeader: true
            });
        });
    </script>
    <script>  
        //$(document).ready(function () {
        //    $("#btnsubmit").click(function () {
        //        var email = document.getElementById('txtemail');
        //        if (email.value != "") {
        //            var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        //            if (!filter.test(email.value)) {
        //                alert('Please provide a valid email address');
        //                email.focus;
        //                return false;
        //            }
        //        }
        //    });
        //});


            //function checkEmail() {
            //    var email = document.getElementById('txtEmail');
            //    if (email.value != "") {
            //        alert('Enter Email Address !!');
            //        return false;
            //    }
            //    else {
            //        var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
            //        if (!filter.test(email.value)) {
            //            alert('Please provide a valid email address');
            //            email.focus;
            //            return false;
            //        }
            //    }

            //}
        </script>
    <script>
        //Select Convert ul li
        $('.drop-select').selectpicker();
    </script>

    <script>
        //Tool Tip
        $(function () {
            $('[data-toggle="tooltip"]').tooltip()
        })
    </script>

    <script>
        //File Name Show
        $(".custom-file-input").on("change", function () {
            var fileName = $(this).val().split("\\").pop();
            $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
        });
    </script>

    <script>
        //   Current Date Time
        $(function ($) {
            setInterval(function () {
                // param sets timezone 
                $('.user').timeUser();
            }, 1000);
        });

        $.fn.extend({
            timeTarget: function (offSet) {
                var hiDate,
                    hiTime,
                    hiHours;

                tgtTime = new Date();
                tgtHours = tgtTime.getUTCHours() - offSet;
                tgtTime.setHours(tgtHours);
                tgtDate = tgtTime.toDateString();
                tgtTime = tgtTime.toTimeString();
                this.html(tgtDate + ' ' + tgtTime.substring(0, 8));
            },

            timeUser: function () {
                var userTime,
                    userDate;

                userTime = new Date();
                userDate = userTime.toDateString();
                userTime = userTime.toTimeString();
                this.html(userDate + ' ' + userTime.substring(0, 8));
            }
        });
    </script>

    <script>

        // Scroll to top button appear
        $(document).on('scroll', function () {
            var scrollDistance = $(this).scrollTop();
            if (scrollDistance > 100) {
                $('.scroll-to-top').fadeIn();
            } else {
                $('.scroll-to-top').fadeOut();
            }
        });

        // Smooth scrolling using jQuery easing
        $(document).on('click', 'a.scroll-to-top', function (e) {
            var $anchor = $(this);
            $('html, body').stop().animate({
                scrollTop: ($($anchor.attr('href')).offset().top)
            }, 1000, 'easeInOutExpo');
            e.preventDefault();
        });
        function integersOnly(obj) {
            obj.value = obj.value.replace(/[^0-9,\r\n]/g, '');
        }

        function numeric(e) {
            var unicode = e.charCode ? e.charCode : e.keyCode;
            if (unicode == 8 || unicode == 9 || unicode == 46 || (unicode >= 48 && unicode <= 57)) {
                return true;
            }
            else {
                return false;
            }
        }
// End of use strict
    </script>
    <script>
        function handleChange(input) {
            if (input.value < 0) input.value = 0;
            if (input.value > 100) input.value = 100;
        }
    </script>
</asp:Content>
