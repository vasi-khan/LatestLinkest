<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="ScheduleLogsOLD.aspx.cs" Inherits="eMIMPanel.ScheduleLogsOLD" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <cc1:ToolkitScriptManager ID="scrmg" runat="server" EnablePageMethods="true">
    </cc1:ToolkitScriptManager>
    <main>
        <div class="container-fluid">
            <nav aria-label="breadcrumb" class="my-3">
                <ol class="breadcrumb breadcrumb-info">
                    <li class="breadcrumb-item"><a href="#">Home</a></li>
                    <li class="breadcrumb-item"><a href="#">Linkext</a></li>
                    <li class="breadcrumb-item active" aria-current="page">Schedule Logs</li>
                </ol>
            </nav>
            <div class="row">
                <div class="col-12">
                    <div class="card card-body mb-4 bg-primary border-light shadow-soft">

                        <div class="form-row">
                            <div class="right-view">
                                <div class="row">

                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtFrm" runat="server" onchange="javascript:text_changed_from();" class="form-control datepicker" placeholder="From Date" autocomplete="off"></asp:TextBox>
                                        <asp:HiddenField ID="hdntxtFrm" runat="server" />
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtTo" runat="server" onchange="javascript:text_changed_to();" class="form-control datepicker" placeholder="To Date" autocomplete="off"></asp:TextBox>
                                        <asp:HiddenField ID="hdntxtTo" runat="server" />
                                    </div>
                                    <%--<a class="btn btn-primary text-dark btn-block" id="reportrange" role="button" aria-pressed="true">
                                                <i class="fas fa-calendar mr-2 text-dark"></i>
                                                <span class="text-dark"></span><i class="ml-1 fas fa-chevron-down" data-feather="chevron-down"></i>
                                            </a>--%>
                                    <div class="col-md-4">
                                        <asp:LinkButton runat="server" ID="aaaa" OnClick="btnUpdate_Click" class="btn btn-mini">
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
                    </div>
                    <!-- Card End -->

                    <div class="accordion shadow-soft rounded" id="accordionExample">
                        <div class="card card-sm card-body bg-primary border-light mb-0">

                            <a href="#collapseOne" id="headingOne" data-target="#collapseOne" class="accordion-panel-header" data-toggle="collapse" role="button" aria-expanded="true" aria-controls="collapseOne">
                                <span class="icon-title h6 mb-0 font-weight-bold"><span class="fas fa-chart-line"></span> Scheduled SMS</span>
                                <span class="icon"><span class="fas fa-plus"></span></span>
                            </a>

                            <!-- <div class="card-header" id="headingOne">
                                <h2 class="mb-0">
                                    <button class="btn btn-link btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseOne" aria-expanded="true" aria-controls="collapseOne">
                                        Analytics
                                    </button>
                                </h2>
                            </div> -->

                            <div id="collapseOne" class="collapse show" aria-labelledby="headingOne" data-parent="#accordionExample" runat="server">
                                <div class="card-body px-0">
                                    <div class="row">
                                        <!-- Area Chart -->
                                        <div class="col-xl-12 col-lg-12">
                                            <!-- Loader -->
                                            <div class="o-page-loader">
                                                <div class="o-page-loader--content">
                                                    <div class="o-page-loader--spinner"></div>
                                                    <div class="o-page-loader--message">
                                                        <span>Loading...</span>
                                                    </div>
                                                </div>
                                            </div>
                                            <!--End Loader -->

                                            <div class="table-responsive">
                                                <asp:GridView ID="grv" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
                                                    runat="server" Width="100%" CellPadding="10" BorderColor="#ede8e8" Class="table table-striped table-bordered dt-responsive wrap dataTable-view">
                                                    <Columns>
                                                        <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Sr No">
                                                            <ItemTemplate>
                                                                <%#Container.DataItemIndex+1 %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Action">
                                                            <ItemTemplate>
                                                                <%--<asp:LinkButton ID="LinkDownload2" runat="server" OnClick="Down_Click" class="btn btn-datatable btn-icon btn-transparent-dark px-2 py-0" data-toggle="tooltip" data-placement="top" title="Download"><i class="fas fa-file-download"></i></asp:LinkButton>--%>
                                                                <%-- <asp:LinkButton ID="LinkButton1" runat="server" OnClick="QR_Click" class="btn btn-datatable btn-icon btn-transparent-dark px-2 py-0" data-toggle="tooltip" data-placement="top" title="QR Code"><i class="fas fa-qrcode"></i></asp:LinkButton> --%>
                                                                <asp:LinkButton ID="LinkButton2" runat="server" OnClick="btnView_Click" class="btn btn-datatable btn-icon btn-transparent-dark px-2 py-0" data-toggle="tooltip" data-placement="top" title="Details"><i class="far fa-file-alt"></i></asp:LinkButton>
                                                                <%--<asp:LinkButton ID="LinkButton3" runat="server" class="btn btn-datatable btn-icon btn-transparent-dark px-2 py-0" data-toggle="tooltip" data-placement="top" title="Trash"><i class="far fa-trash-alt"></i></asp:LinkButton></td>--%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="User ID">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblUserId" runat="server" Text='<%#Eval("userid")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Long URL">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbllongurl" runat="server" Text='<%#Eval("LongURL")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Small URL">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl1" runat="server" Text='<%#Eval("smallURL")%>'></asp:Label>
                                                                <asp:HiddenField ID="hdnSegment" runat="server" Value='<%#Eval("segment")%>'></asp:HiddenField>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Creation Date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl2" runat="server" Text='<%#Eval("CreationDate")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="No of Hits">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl3" runat="server" Text='<%#Eval("No_Of_Hits")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="URL ID">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblurlid" runat="server" Text='<%#Eval("URLID")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                    </Columns>
                                                </asp:GridView>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!--Start Accordian 2   -->
                        <div class="card card-sm card-body bg-primary border-light mb-0">
                            
                            <a href="#collapseTwo" id="headingTwo" data-target="#collapseTwo" class="accordion-panel-header" data-toggle="collapse" role="button" aria-expanded="true" aria-controls="collapseTwo">
                                <span class="icon-title h6 mb-0 font-weight-bold"><span class="fas fa-chart-bar"></span> Scheduled SMS Detail</span>
                                <span class="icon"><span class="fas fa-plus"></span></span>
                            </a>

                            <!-- <div class="card-header" id="headingTwo">
                                <h2 class="mb-0">
                                    <button class="btn btn-link btn-block text-left collapsed" type="button" data-toggle="collapse" data-target="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo">
                                        Analytics Detail
                                    </button>
                                </h2>
                            </div> -->

                            <div id="collapseTwo" class="collapse" aria-labelledby="headingTwo" data-parent="#accordionExample" runat="server">
                                <div class="card-body px-0">
                                    <div style="text-align: right;">
                                        <asp:LinkButton runat="server" ID="LinkButton1" OnClick="btnCloseDetail_Click" class="btn btn-mini">Close Detail</asp:LinkButton>
                                    </div>
                                    <asp:GridView ID="grv2" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
                                        runat="server" Width="100%" CellPadding="10"
                                        BorderColor="#ede8e8" Class="table table-striped table-bordered dt-responsive nowrap dataTable-view">
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Sr No">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Hit Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUserId" runat="server" Text='<%#Eval("ClickDate")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="IP Address">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbllongurl" runat="server" Text='<%#Eval("ip")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Location">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl1" runat="server" Text='<%#Eval("referer")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Platform">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl2" runat="server" Text='<%#Eval("Platform")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Browser">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl3" runat="server" Text='<%#Eval("Browser")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="From Mobile Device">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl4" runat="server" Text='<%#Eval("IsMobileDevice")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Mobile Device Manufacturer">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl5" runat="server" Text='<%#Eval("MobileDeviceManufacturer")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Mobile Device Model">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl6" runat="server" Text='<%#Eval("MobileDeviceModel")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>


                                </div>
                            </div>
                        </div>
                      <!-- End Accordian 2 -->
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
        (function (loader) {

            window.addEventListener('beforeunload', function (e) {
                activateLoader();
            });

            window.addEventListener('load', function (e) {
                deactivateLoader();
            });

            function activateLoader() {
                loader.style.display = 'block';
                loader.style.opacity = 1;
            }

            function deactivateLoader() {
                /**
                 * ensures that the loading animation plays for at least a second to give the
                 * appearance of seamless loading on pages that execute and load extremely
                 * quickly (i.e., intranet pages)
                 */
                setTimeout(function () {
                    deactivate();
                }, 1000);

                function deactivate() {
                    loader.style.opacity = 0;
                    loader.addEventListener('transitionend', function () {
                        loader.style.display = 'none';
                    }, false);
                }
            }

        })(document.querySelector('.o-page-loader'));
</script>
</asp:Content>
