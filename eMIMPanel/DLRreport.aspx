<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="DLRreport.aspx.cs" Inherits="eMIMPanel.DLRreport" %>

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
    <main>
        <div class="container-fluid">
            <!--  -->
            <nav aria-label="breadcrumb" class="my-3">
                <ol class="breadcrumb breadcrumb-info">
                    <li class="breadcrumb-item"><a href="#">Home</a></li>
                    <li class="breadcrumb-item"><a href="#">Reports</a></li>
                    <li class="breadcrumb-item active" aria-current="page">DLR Report</li>
                </ol>
            </nav>
            <!--  -->

            <div class="row">
                <div class="col-12">
                    <!--  -->
                    <div class="card card-body mb-4 bg-primary border-light shadow-soft">
                        <%--<div class="row">
                            <div class="col-md-4">
                                <asp:TextBox ID="txtFrm" runat="server" onchange="javascript:text_changed_from();" class="form-control datepicker" placeholder="From Date" autocomplete="off"></asp:TextBox>
                                <asp:HiddenField ID="hdntxtFrm" runat="server" />
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtTo" runat="server" onchange="javascript:text_changed_to();" class="form-control datepicker" placeholder="To Date" autocomplete="off"></asp:TextBox>
                                <asp:HiddenField ID="hdntxtTo" runat="server" />
                            </div>
                            <div class="col-md-4">
                                <asp:LinkButton runat="server" ID="btnshow" OnClick="btnshow_Click" class="btn btn-block">
                                    <i class="fas fa-paper-plane"></i> Send Request  
                                </asp:LinkButton>

                            </div>
                            <asp:HiddenField ID="h1" runat="server" />
                            <asp:HiddenField ID="h2" runat="server" />
                        </div>--%>


                        <div class="form-row">
                            <asp:RadioButton ID="rbTdy" runat="server" AutoPostBack="true" Text="Today" GroupName="Filter" Checked="true" Style="margin-left: 20px" OnCheckedChanged="rbTdy_CheckedChanged" />
                            <asp:RadioButton ID="rbHis" runat="server" AutoPostBack="true" OnCheckedChanged="rbHis_CheckedChanged" Text="Old" Style="margin-left: 20px; margin-right: 50px" GroupName="Filter" />
                            <div class="form-group col-lg-2  col-xl-2">
                                <asp:RadioButtonList ID="rdbselectlist" RepeatDirection="Horizontal" runat="server">
                                    <asp:ListItem Value="D" Selected="True">Date Wise</asp:ListItem>
                                    <asp:ListItem Value="S">Single</asp:ListItem>
                                </asp:RadioButtonList>
                            </div>


                        </div>
                        <div class="form-row">
                            <asp:RadioButton ID="rbSbmtd" runat="server" AutoPostBack="true" Text="Submitted" GroupName="Filter1" Checked="true" Style="margin-left: 20px" />
                            <asp:RadioButton ID="rbDlvr" runat="server" AutoPostBack="true" Text="Delivery" Style="margin-left: 20px" GroupName="Filter1" />
                            <asp:RadioButton ID="rbFailed" runat="server" AutoPostBack="true" Text="Failed" Style="margin-left: 20px; margin-right: 50px" GroupName="Filter1" />
                            <div class="form-group col-lg-2  col-xl-2 mt-auto ">
                                <asp:LinkButton  OnClientClick="return CheckDates();"  ID="btnsearch" runat="server" OnClick="btnshow_Click" class="btn btn-primary text-success btn-block"><i class="fas fa-search fa-sm text-success"></i>Send Request </asp:LinkButton>
                            </div>
                        </div>

                        <div id="divOld" runat="server" class="form-row mt-2 d-none">
                            <div class="row">
                                <div class="col-md-4" Style="margin-left: 20px;">
                                    <asp:TextBox ID="txtFrm" runat="server" onchange="javascript:text_changed_from();" class="form-control datepicker"  placeholder="From Date" autocomplete="off"></asp:TextBox>
                                    <asp:HiddenField ID="hdntxtFrm" runat="server" />
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtTo" runat="server" onchange="javascript:text_changed_to();" class="form-control datepicker"  placeholder="To Date" autocomplete="off"></asp:TextBox>
                                    <asp:HiddenField ID="hdntxtTo" runat="server" />
                                </div>

                                <asp:HiddenField ID="h1" runat="server" />
                                <asp:HiddenField ID="h2" runat="server" />
                            </div>
                        </div>
                    </div>
                    <!-- Card End -->
                    <!--  -->
                </div>
            </div>

            <!--  -->
            <div class="row">
                <div class="col-12">
                    <div class="accordion shadow-soft rounded" id="accordionExample">
                        <div class="card card-sm card-body bg-primary border-light mb-0">

                            <a href="#collapseOne" id="headingOne" data-target="#collapseOne" class="accordion-panel-header" data-toggle="collapse" role="button" aria-expanded="true" aria-controls="collapseOne">
                                <span class="icon-title h6 mb-0 font-weight-bold"><span class="fas fa-chart-line"></span>Pending DLR Request</span>
                                <span class="icon"><span class="fas fa-plus"></span></span>
                            </a>



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
                                                        <asp:TemplateField HeaderText="RequestDateTime">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblreqdate" runat="server" Text='<%#Eval("ReqDate")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="DLRFrom">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbldlrfrom" runat="server" Text='<%#Eval("DLRFrom")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="DLRTo">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbldlrto" runat="server" Text='<%#Eval("DLRTo")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        
                                                        <asp:TemplateField HeaderText="Request Type">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbldRequestType" runat="server" Text='<%#Eval("RequestType")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Download">
                                                            <ItemTemplate>
                                                                <asp:HiddenField ID="hdnid" Value='<%#Eval("generatedpath")%>' runat="server" />
                                                                <asp:HiddenField ID="hdnactive" Value='<%#Eval("Active")%>' runat="server" />
                                                                <asp:LinkButton ID="btnlink" runat="server" OnClick="btnlink_Click"
                                                                    Visible='<%#Convert.ToBoolean(Eval("Active"))%>'> <i class="fa fa-download"></i>
                                                                    
                                                                </asp:LinkButton>
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


    <script type="text/javascript">
      /*  $(function () {
            var pDate = new Date();
            pDate.setDate(pDate.getDate() - 365);
            var month1 = ('0' + (pDate.getMonth() + 1)).slice(-2);
            var day1 = ('0' + pDate.getDate()).slice(-2);
            var year1 = pDate.getFullYear();
            var date1 = year1 + '-' + month1 + '-' + day1;


            var today = new Date();
            today.setDate(today.getDate() - 1);
            var month = ('0' + (today.getMonth() + 1)).slice(-2);
            var day = ('0' + today.getDate()).slice(-2);
            var year = today.getFullYear();
            var date = year + '-' + month + '-' + day;

            $('[id*=txtFrm]').attr('max', date);
            $('[id*=txtFrm]').attr('min', date1);

            $('[id*=txtTo]').attr('max', date);
            $('[id*=txtTo]').attr('min', date1);

        });*/


         $(function () {
                var oneyear = new Date();
                oneyear.setFullYear(oneyear.getFullYear() - 1);
                $(".datepicker").datepicker({
                    endDate: new Date(),
                    todayHighlight: true,
                    autoclose: true,
                    startDate: oneyear,
                    format: 'yyyy-mm-dd',
                    //autoUpdateInput: false
                });
            });




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


</asp:Content>
