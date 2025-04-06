<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="SMSReport2.aspx.cs" Inherits="eMIMPanel.SMSReport2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
       <cc1:ToolkitScriptManager ID="scrmg" runat="server" EnablePageMethods="true">
    </cc1:ToolkitScriptManager>
       <style>
        .table thead th {
            vertical-align: middle;
            border-bottom: 0.125rem solid #6e6e6f;
            text-align: center;
        }

        .table th, .table td {
            padding: 8px;
            font-size: 12px;
            border: 1px solid #9a9a9a;
            text-align: center;
        }

        #ContentPlaceHolder1_divResult thead {
            background: #d2d2d2;
        }

        #ContentPlaceHolder1_divResult tbody tr {
            background: #f1f1f1;
        }
            /* #ContentPlaceHolder1_divResult tbody tr.collapse.show {
            background: #ffffff !important;
        } */
            #ContentPlaceHolder1_divResult tbody tr:nth-child(2n) {
                background: #ffffff;
            }

            #ContentPlaceHolder1_divResult tbody tr.collapse.show table tbody tr:nth-child(odd) {
                background: #f1f1f1;
            }

            #ContentPlaceHolder1_divResult tbody tr.collapse.show table tbody tr:nth-child(even) {
                background: #e8e8e8;
            }
    </style>
     <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
         <ContentTemplate>
               <main>
         <div class="container-fluid">
             <nav aria-label="breadcrumb" class="my-3">
                 <ol class="breadcrumb breadcrumb-info">
                     <li class="breadcrumb-item"><a href="#">Home</a></li>
                     <li class="breadcrumb-item"><a href="#">Reports</a></li>
                     <li class="breadcrumb-item active" aria-current="page">SMS Reports2</li>
                 </ol>
             </nav>


              <div class="row">
                 <div class="col-xl-12 col-lg-12">
                     <div class="card card-body bg-primary border-light shadow-soft mb-4 py-3 px-4">
                         
                                <div class="row align-items-center">
                             <div class="col-md-3">
                                 <label class="col-form-label font-weight-bold">From Date</label>
                                 <asp:TextBox ID="txtFrm" runat="server" onchange="javascript:text_changed_from();" class="form-control datepicker" placeholder="From Date" autocomplete="off"></asp:TextBox>
                                 <asp:HiddenField ID="hdntxtFrm" runat="server" />
                                  
                             </div>
                             <div class="col-md-3">
                                     <label class="col-form-label font-weight-bold">To Date</label>
                                 <asp:TextBox ID="txtTo" runat="server" onchange="javascript:text_changed_to();" class="form-control datepicker mt-3 my-lg-0" placeholder="To Date" autocomplete="off"></asp:TextBox>
                                 <asp:HiddenField ID="hdntxtTo" runat="server" />
                             </div>
                              
   
                           <div class="col-md-3">
                             <label class="col-form-label font-weight-bold">Mobile No</label>
                                 <asp:TextBox ID="txtMobileNo" onkeypress="return onlyNumberKey(event);" MaxLength="15" runat="server" class="form-control" autocomplete="off"></asp:TextBox>
                             </div>

         
        
                             <div class="col-md-2">
                                 <label>&nbsp;</label>
                                 <asp:LinkButton runat="server" ID="btnShow" class="btn btn-block mt-3 my-lg-0" OnClick="btnShow_Click">
                                             Show <i class="fas fa-eye" aria-hidden="true"></i>
                                 </asp:LinkButton>
                             </div>
    
                             <asp:HiddenField ID="h1" runat="server" />
                             <asp:HiddenField ID="h2" runat="server" />
                        

         </div>
                     </div>
                 </div>
             </div>


               <div class="card card-body mb-4 bg-primary border-light shadow-soft">
                        <div class="table-responsive ttd">


                         <asp:GridView UseAccessibleHeader="true" OnDataBound="OnDataBound" AllowPaging="true" PageSize="10" OnPageIndexChanging="grv_PageIndexChanging" ID="grv" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
       runat="server" Width="100%" CellPadding="10" BorderColor="#ede8e8" Class="table table-striped table-bordered dt-responsive nowrap">
       <PagerSettings FirstPageText="First Page" LastPageText="Last Page" Mode="NumericFirstLast" />
       <Columns>
             <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Sno">
                                                                <ItemTemplate>
                                                                    <%#Container.DataItemIndex+1 %>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
           <asp:BoundField DataField="MessageId" HeaderText="MSG ID" />
           <asp:BoundField DataField="MobileNo" HeaderText="MobileNo" />
           <asp:BoundField DataField="Sender" HeaderText="Sender ID" />
           <asp:BoundField DataField="SentDate" HeaderText="Submit Date Time" />
           <asp:BoundField DataField="DeliveredDate" HeaderText="Status Date Time" />
           <asp:BoundField DataField="Message" HeaderText="Message" />
           <asp:BoundField DataField="MessageState" HeaderText="MessageState" />
           <asp:BoundField DataField="RESPONSE" HeaderText="Server Response" />

           </Columns>
       </asp:GridView>
    
                          
                            <asp:Label ID="lblTotal" Font-Size="Small" Font-Bold="true" runat="server"></asp:Label>
                 </div>
          </div>

             </div>
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
               function onlyNumberKey(evt) {              
        var ASCIICode = (evt.which) ? evt.which : evt.keyCode
        if (ASCIICode > 31 && (ASCIICode < 48 || ASCIICode > 57))
            return false;
        return true;
        }
     </script>
       </main>
               <script type="text/javascript">
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

            function EndRequestHandler(sender, args) {
                $('.datepicker').datepicker({ dateFormat: 'dd-mm-yy' });
            }

        });
         function loadscrq() {
             $('.datepicker').datepicker({ dateFormat: 'dd-mm-yy' });
         }
     </script>
         </ContentTemplate>
  </asp:UpdatePanel>
     <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="0">
                        <ProgressTemplate>
                            <div align="center" style="position: absolute; left: 40%; top: 50%; vertical-align: middle;">
                                <img src="Img/LOADING.GIF" />
                            </div>
                            <div align="center" style="position: absolute; left: 0%; top: 0%; vertical-align: middle;">
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
        <link rel="stylesheet" href="https://unpkg.com/bootstrap-table@1.15.4/dist/bootstrap-table.min.css" />
    <link rel="stylesheet" href="https://unpkg.com/bootstrap-table@1.15.4/dist/bootstrap-table.min.js" />
    <link rel="stylesheet" href="https://unpkg.com/bootstrap-table@1.15.4/dist/bootstrap-table-locale-all.min.js" />
     <script type="text/javascript">
                $(function () {
                    // popover
                    $('.notePopover').popover();

                    var $table = $('#table');

                    $(".collapse td.inside-table").siblings().remove();

                    $("table tr [data-toggle=collapse]").on('click', function (e) {
                        var rowIndex = ($(this).closest('td').parent()[0].sectionRowIndex);
                        if ($($(this).closest('td').parent()[0]).hasClass('active')) {
                            $('table tr').removeClass('active');
                            $('.collapse').removeClass('show');
                        } else {
                            $($(this).closest('td').parent()[0]).addClass('active');
                        }
                        console.log(rowIndex);
                    });

                });
            </script>
</asp:Content>
