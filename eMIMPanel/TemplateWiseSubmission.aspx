<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="TemplateWiseSubmission.aspx.cs" Inherits="eMIMPanel.TemplateWiseSubmission" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        /*CSS Classes For Design Modal*/
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

        .select2-dropdown {
            background-color: #e6e7ee !important;
            border: 1px solid #aaa;
        }

        .select2-container .select2-selection--single .select2-selection__rendered {
            padding-left: 0;
            padding-right: 0;
            height: auto;
            margin-top: -3px
        }

        .select2-container--default .select2-selection--single,
        .select2-selection .select2-selection--single {
            border: 1px solid #d2d6de;
            border-radius: 5px !important;
            padding: 6px 12px;
            height: 40px !important
        }

            .select2-container--default .select2-selection--single .select2-selection__arrow {
                height: 26px;
                position: absolute;
                top: 6px !important;
                right: 1px;
                width: 20px
            }


        #info {
            position: relative;
            left: -10px;
        }
    </style>
    <link href="ms-Dropdown-master/css/msdropdown/dd.css" rel="stylesheet" />
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>
    <script type="text/javascript" src="ms-Dropdown-master/js/jquery/jquery-1.9.0.min.js"></script>
    <script type="text/javascript" src="ms-Dropdown-master/js/msdropdown/jquery.dd.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main>
        <div class="container-fluid">
            <nav aria-label="breadcrumb" class="my-3">
                <ol class="breadcrumb breadcrumb-info">
                    <li class="breadcrumb-item"><a href="#">Home</a></li>
                    <li class="breadcrumb-item"><a href="#">Reports</a></li>
                    <li class="breadcrumb-item active" aria-current="page">Template Wise Submissions</li>
                </ol>
            </nav>

            <div class="row">
                <div class="card card-body mb-4 bg-primary border-light shadow-soft">
                    <div class="form-row">
                        <div class="col-md-2 pt-2">
                            <asp:Label runat="server" Text="From Date:"></asp:Label>
                        </div>
                        <div class="col-md-3 ">
                            <asp:TextBox ID="txtFrm" runat="server" onchange="javascript:text_changed_from();" class="form-control datepicker ml-3" autocomplete="off"></asp:TextBox>
                            <asp:HiddenField ID="hdntxtFrm" runat="server" />
                        </div>
                        <div class="col-md-2 pt-2 ml-4">
                            <asp:Label runat="server" Text="To Date:"></asp:Label>
                        </div>
                        <div class="col-md-3">
                            <asp:TextBox ID="txtTo" runat="server" onchange="javascript:text_changed_to();" class="form-control datepicker" autocomplete="off"></asp:TextBox>
                            <asp:HiddenField ID="hdntxtTo" runat="server" />
                        </div>
                    </div>
                    <div class="form-row mt-4" id="divTempId" runat="server">
                        <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">Template ID</label>
                        <div class="col-md-6">
                            <div id="div8" runat="server" style="pointer-events: all;">
                                <div class="col-lg-12">
                                    <asp:DropDownList ID="ddlTempID" runat="server" ClientIDMode="Static" class="custom-select" AutoPostBack="true" Width="97%">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <asp:LinkButton runat="server" OnClientClick="return CheckDates();" ID="lnkShow" class="btn btn-block" OnClick="lnkShow_Click">
                            Show <i class="fas fa-eye" aria-hidden="true"></i>
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>


            <!-- Grid begins-->
            <div id="collapseOne" class="collapse show" aria-labelledby="headingOne" data-parent="#accordionExample" runat="server">
                <div class="card-body px-0">
                    <div class="row">
                        <!-- Area Chart -->
                        <div class="col-xl-12 col-lg-12">
                            <div class="table-responsive">
                                <asp:GridView ID="grv" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false" ShowFooter="true"
                                    runat="server" Width="100%" CellPadding="10" BorderColor="#ede8e8" Class="table table-striped table-bordered dt-responsive wrap dataTable-view">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Sr No">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Template Id">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("templateId")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Submitted">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("submitted")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Delivered">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("Delivered")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Failed">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("Failed")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Unknown">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("Unknown")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Template Text">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("msgtext")%>'></asp:Label>
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
    </main>

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
    </script>

    <script>
        function CheckDates() {
            var fromDate = $("#<%= txtFrm.ClientID %>").datepicker("getDate");
            var toDate = $("#<%= txtTo.ClientID %>").datepicker("getDate");

            // Check if either From Date or To Date is empty or null
            if ((fromDate == null || toDate == null) || (fromDate === "" || toDate === "")) {
                alert("From date and To Date cannot be empty");
                return false;
            }

            // Check if From Date is greater than To Date
            if (fromDate > toDate) {
                alert("From Date cannot be greater than To Date");
                return false;
            }
            return true;
        }
    </script>

    <script>
        $(function () {
            var today = new Date();
            var fortyFiveDaysAgo = new Date();
            fortyFiveDaysAgo.setDate(today.getDate() - 45);
            var todayString = today.toISOString().split('T')[0];

            $(".datepicker").datepicker({
                endDate: today,
                todayHighlight: true,
                autoclose: true,
                startDate: fortyFiveDaysAgo,
                format: 'yyyy-mm-dd'
            });

            $('#<%= txtFrm.ClientID %>').datepicker('setDate', todayString);
            $('#<%= txtTo.ClientID %>').datepicker('setDate', todayString);
        });

    </script>

    <!-- Select2 CSS -->
    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/css/select2.min.css" rel="stylesheet" />
    <!-- jQuery -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
    <!-- Select2 -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/js/select2.min.js"></script>
    <script>
        $("#ddlTempID").select2({
            allowClear: true
        });

        $("#ddlTemplate").select2({
            allowClear: true
        });
    </script>
</asp:Content>