<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="CustomReportDownload.aspx.cs" Inherits="eMIMPanel.CustomReportDownload" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        body {
            background-color: mediumaquamarine;
            font-family: Arial;
            font-size: 10pt;
            color: #444;
        }

        .ParentMenu, .ParentMenu:hover {
            width: 100px;
            background-color: #fff;
            color: #333;
            text-align: center;
            height: 30px;
            line-height: 30px;
            margin-right: 5px;
        }

            .ParentMenu:hover {
                background-color: #ccc;
            }

        .ChildMenu, .ChildMenu:hover {
            width: 110px;
            background-color: #fff;
            color: #333;
            text-align: center;
            height: 30px;
            line-height: 30px;
            margin-top: 5px;
        }

            .ChildMenu:hover {
                background-color: #ccc;
            }

        .selected, .selected:hover {
            background-color: #A6A6A6 !important;
            color: #fff;
        }

        .level2 {
            background-color: #fff;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" ID="sm1"></asp:ScriptManager>
    <asp:UpdatePanel ID="updFormArea" runat="server">
        <ContentTemplate>
            <main>
                <nav aria-label="breadcrumb" class="my-3">
                    <ol class="breadcrumb breadcrumb-info">
                        <li class="breadcrumb-item"><a href="#">Home</a></li>
                        <li class="breadcrumb-item active" aria-current="page">Report Download</li>
                    </ol>
                </nav>
                <div class="row">


                    <div class="col-sm-12 col-lg-12">
                        <div class="card nav-pills2">

                            <div class="card-body">
                                <form class="bg-white ">
                                    <div class="row">
                                        <div class="col-md-4 sidebartwo border rounded">
                                            <div class="row">
                                                <div class="col-md-12 purchase-top">
                                                    <h5><b>Report</b></h5>
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:DropDownList ID="ddlreport" runat="server" class="custom-select" AutoPostBack="true" OnSelectedIndexChanged="ddlreport_SelectedIndexChanged">
                                                        <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                                                    </asp:DropDownList>

                                                    <%--                                 <div class="row  p-1">
                                     <div class="col-sm-12">
                                 <asp:RadioButton ID="rdoshow_Grid" Text="Show On Grid" runat="server"  GroupName="A"  Checked="true" />
                                         </div>
                                     <div class="col-sm-12">
                                 <asp:RadioButton ID="rdoDownloadCSV" Text="Download CSV" runat="server" GroupName="A" />
                                         </div>
                                     </div>--%>
                                                    <div class="row p-1">
                                                        <div class="col-sm-12">
                                                            <asp:RadioButtonList runat="server" ID="rdoshow_Grid" Visible="false">
                                                                <asp:ListItem Value="Grid" Text="Show On Grid" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Value="CSV" Text="Download- CSV"></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <br />



                                            </div>
                                            <div class="card-body">
                                                <div class="row" style="margin-right: 20px; margin-bottom: 4px">
                                                    <div id="reportsidediv" runat="server">
                                                        <asp:Panel ID="pnldynamiccontrol" runat="server">
                                                            <div class="row mt-1">
                                                                <div class="col-sm-4">
                                                                    <%-- <asp:Label runat="server" ID="ldl1" Visible="false" CssClass="form-label" ></asp:Label>--%>
                                                                    <label runat="server" id="ldl1" visible="false" class="form-label"></label>
                                                                </div>
                                                                <div class="col-sm-8">
                                                                    <asp:TextBox runat="server" ID="txt1" CssClass="form-control" Visible="false"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="row mt-1">
                                                                <div class="col-sm-4">
                                                                    <%--<asp:Label runat="server" ID="lbl2" Visible="false" CssClass="form-label"></asp:Label>--%>
                                                                    <label runat="server" id="lbl2" visible="false" class="form-label"></label>
                                                                </div>
                                                                <div class="col-sm-8">
                                                                    <asp:TextBox runat="server" ID="txt2" CssClass="form-control" Visible="false"></asp:TextBox>
                                                                </div>
                                                            </div>

                                                            <div class="row mt-1">
                                                                <div class="col-sm-4">
                                                                    <%--  <asp:Label runat="server" ID="lbl3" Visible="false" CssClass="form-label"></asp:Label>--%>
                                                                    <label runat="server" id="lbl3" visible="false" class="form-label"></label>

                                                                </div>
                                                                <div class="col-sm-8">
                                                                    <asp:TextBox runat="server" ID="txt3" CssClass="form-control" Visible="false"></asp:TextBox>
                                                                </div>
                                                            </div>

                                                            <div class="row mt-1">
                                                                <div class="col-sm-4">
                                                                    <label runat="server" id="lbl4" visible="false" class="form-label"></label>
                                                                    <%-- <asp:Label runat="server" ID="lbl4" Visible="false" CssClass="form-label"></asp:Label>--%>
                                                                </div>
                                                                <div class="col-sm-8">
                                                                    <asp:TextBox runat="server" ID="txt4" CssClass="form-control" Visible="false"></asp:TextBox>
                                                                </div>
                                                            </div>

                                                            <div class="row mt-1">
                                                                <div class="col-sm-4">
                                                                    <label runat="server" id="lbl5" visible="false" class="form-label"></label>
                                                                    <%--<asp:Label runat="server" ID="lbl5" Visible="false"></asp:Label>--%>
                                                                </div>
                                                                <div class="col-sm-8">
                                                                    <asp:TextBox runat="server" ID="txt5" CssClass="form-control" Visible="false"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="row mt-1">
                                                                <div class="col-sm-4">
                                                                    <label runat="server" id="lbl6" visible="false" class="form-label"></label>
                                                                    <%-- <asp:Label runat="server" ID="lbl6" Visible="false"></asp:Label>--%>
                                                                </div>
                                                                <div class="col-sm-8">
                                                                    <asp:TextBox runat="server" ID="txt6" CssClass="form-control" Visible="false"></asp:TextBox>
                                                                </div>
                                                            </div>

                                                            <div class="row mt-1">
                                                                <div class="col-sm-4">
                                                                    <label runat="server" id="lbl7" visible="false" class="form-label"></label>
                                                                    <%--    <asp:Label runat="server" ID="lbl7" Visible="false"></asp:Label>--%>
                                                                </div>
                                                                <div class="col-sm-8">

                                                                    <asp:TextBox runat="server" ID="txt7" CssClass="form-control" Visible="false"></asp:TextBox>
                                                                </div>
                                                            </div>

                                                            <div class="row mt-1">
                                                                <div class="col-sm-4">
                                                                    <label runat="server" id="lbl8" visible="false" class="form-label"></label>
                                                                    <%-- <asp:Label runat="server" ID="lbl8" Visible="false"></asp:Label>--%>
                                                                </div>
                                                                <div class="col-sm-8">

                                                                    <asp:TextBox runat="server" ID="txt8" CssClass="form-control" Visible="false"></asp:TextBox>
                                                                </div>
                                                            </div>

                                                            <div class="row mt-1">

                                                                <div class="col-sm-4">

                                                                    <label runat="server" id="lbl9" visible="false" class="form-label"></label>
                                                                    <%-- <asp:Label runat="server" ID="lbl9" Visible="false"></asp:Label>--%>
                                                                </div>
                                                                <div class="col-sm-8">

                                                                    <asp:TextBox runat="server" ID="txt9" CssClass="form-control" Visible="false"></asp:TextBox>
                                                                </div>
                                                            </div>

                                                            <div class="row mt-1">
                                                                <div class="col-sm-4">
                                                                    <%-- <asp:Label runat="server" ID="lbl10" Visible="false"></asp:Label>--%>
                                                                    <label runat="server" id="lbl10" visible="false" class="form-label"></label>


                                                                </div>
                                                                <div class="col-sm-8">

                                                                    <asp:TextBox runat="server" ID="txt10" CssClass="form-control" Visible="false"></asp:TextBox>
                                                                </div>
                                                            </div>


                                                        </asp:Panel>
                                                        <asp:Button runat="server" Text="Submit" ID="btnsubmit" class="btn btn-success mt-2 " Visible="false" OnClick="btnsubmit_Click" />
                                                        <asp:Button runat="server" Text="Reset" ID="btnreset" Visible="false" class="btn btn-success mt-2  " OnClick="btnreset_Click" />

                                                    </div>
                                                </div>
                                            </div>

                                        </div>




                                        <div class="col-md-8">
                                            <div class="row">
                                                <div class="col-md-12 purchase-top px-2 mt-2">
                                                    <h5><b>Custom Report</b></h5>
                                                </div>

                                                <div class="row ">
                                                    <div class="container">
                                                        <div class="col-md-12 mt-4 text-end">
                                                            <asp:Button ID="btndownload" class="btn btn-warning btn-sm" Text="Download" runat="server" OnClick="btndownload_Click" Visible="false" />
                                                            <div class="table-responsive" id="tpurchase" style="max-height: 200px">
                                                                <asp:GridView ID="grd_report" runat="server" HeaderStyle-BackColor="#EAB012" class="table  text-center table-sm  table-bordered mt-2 ">
                                                                </asp:GridView>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>


                                                <%--</div>--%>
                                            </div>
                                        </div>
                                    </div>
                                </form>
                            </div>
                        </div>
                    </div>
                </div>

            </main>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btndownload" />
            <asp:PostBackTrigger ControlID="btnsubmit" />
            <asp:PostBackTrigger ControlID="txt1" />
            <asp:PostBackTrigger ControlID="txt2" />
            <asp:PostBackTrigger ControlID="txt3" />

            <asp:PostBackTrigger ControlID="txt4" />
            <asp:PostBackTrigger ControlID="txt5" />
            <asp:PostBackTrigger ControlID="txt6" />
            <asp:PostBackTrigger ControlID="txt7" />
            <asp:PostBackTrigger ControlID="txt8" />
            <asp:PostBackTrigger ControlID="txt9" />
            <asp:PostBackTrigger ControlID="txt10" />
        </Triggers>
    </asp:UpdatePanel>

    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="updFormArea" DisplayAfter="0">
        <ProgressTemplate>
            <div align="center" style="position: absolute; left: 40%; top: 50%; vertical-align: middle;">
                <img src="Img/LOADING.GIF" />
            </div>
            <div align="center" style="position: absolute; left: 0%; top: 0%; vertical-align: middle;">
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

</asp:Content>
