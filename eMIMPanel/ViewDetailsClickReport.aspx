<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="ViewDetailsClickReport.aspx.cs" Inherits="eMIMPanel.ViewDetailsClickReport" %>

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
            <asp:Button runat="server" CssClass="d-none" ID="btnShow" OnClick="btnShow_Click" />
            <main>
                <div class="container-fluid">
                    <!-- Content Row -->
                    <div class="row">
                        <div class="col-lg-12">
                            <!-- Start Card -->
                            <div class="card bg-primary border-light shadow-soft mb-4">
                                <div class="card-header py-3 bg-primary d-flex justify-content-between flex-wrap align-content-center">
                                    <h6 class="m-0 font-weight-bold font-weight-bold my-auto"><i class="fas fa-comment-alt"></i>View Details </h6>
                                </div>
                                <div class="card card-body mb-4 bg-primary border-light shadow-soft">
                                    <div class="table-responsive ttd">


                                        <asp:GridView UseAccessibleHeader="true" OnDataBound="OnDataBound" AllowPaging="true" PageSize="100" OnPageIndexChanging="grv_PageIndexChanging" ID="grv" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
                                            runat="server" Width="100%" CellPadding="10" BorderColor="#ede8e8" Class="table table-striped table-bordered dt-responsive nowrap">
                                            <PagerSettings FirstPageText="First Page" LastPageText="Last Page" Mode="NumericFirstLast" />
                                            <Columns>
                                                <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Sr.no">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="mobile" HeaderText="Mobile" />
                                                <asp:BoundField DataField="smsDate" HeaderText="SMS Date" />
                                                <asp:BoundField DataField="ClickDate" HeaderText="Click Date" />
                                                <asp:BoundField DataField="operator" HeaderText="Operator" />
                                                <asp:BoundField DataField="Circle" HeaderText="Circle" />
                                                <asp:BoundField DataField="Platform" HeaderText="Platform" />
                                                <asp:BoundField DataField="Browser" HeaderText="Browser" />
                                                <asp:BoundField DataField="IsMobileDevice" HeaderText="From Mobile Device" />
                                                <asp:BoundField DataField="MobileDeviceManufacturer" HeaderText="Mobile Device Manufacturer" />
                                                <asp:BoundField DataField="MobileDeviceModel" HeaderText="Mobile Device Model" />

                                            </Columns>
                                        </asp:GridView>


                                        <asp:Label ID="lblTotal" Font-Size="Small" Font-Bold="true" runat="server"></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <!-- End Card -->
                        </div>
                    </div>
                    <!-- End Row -->
                </div>
            </main>
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
