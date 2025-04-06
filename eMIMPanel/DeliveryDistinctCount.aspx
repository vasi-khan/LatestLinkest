<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="DeliveryDistinctCount.aspx.cs" Inherits="eMIMPanel.DeliveryDistinctCount" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        label {
            font-weight: 600;
            margin-bottom: 0.5rem;
        }

        .container-fluid {
            width: 70%;
        }

        input[type="checkbox"] {
            margin-right: 12px;
        }

        @media(max-width:600px) {
            .container-fluid {
                width: 100%;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="sc" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>


            <div class="container-fluid">
                <div class="card mt-1 p-2 ">
                    <div class="card-header">
                    </div>
                    <div class="row py-2 pl-2">
                        <div class="col-md-4">
                            <label style="color: deepskyblue">User Id</label>
                            <asp:TextBox CssClass="form-control" ID="txtuserid" runat="server" MaxLength="10" onkeypress="return /^[a-zA-Z0-9\s]*$/.test(event.key)&& this.value.length<10;" placeholder="User ID"></asp:TextBox>

                        </div>

                        <div class="col-md-3">
                            <label style="color: deepskyblue">Distinct count</label>
                            <asp:TextBox CssClass="form-control" ID="txtdistinct" runat="server" MaxLength="7" onkeypress="return /^[0-9\s]*$/.test(event.key)&& this.value.length<10;" placeholder="Distinct count"></asp:TextBox>
                        </div>

                        <%--<div class="col-md-3">
                        <label style="color: deepskyblue">From Date</label>
                        <asp:TextBox CssClass="form-control" type="date" runat="server" ID="txtFromdate"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        <label style="color: deepskyblue">To Date</label><br />
                        <asp:TextBox CssClass="form-control" type="date" runat="server" ID="txttodate"></asp:TextBox>
                    </div>--%>
                        <div class="col-md-2">
                            <label style="color: deepskyblue">From Date </label>
                            <asp:TextBox ID="txtFrm" runat="server" onchange="javascript:text_changed_from();" class="form-control datepicker my-3 my-lg-0" placeholder="From Date" autocomplete="off"></asp:TextBox>
                            <asp:HiddenField ID="hdntxtFrm" runat="server" />
                        </div>
                        <div class="col-md-2">
                            <label style="color: deepskyblue">To Date </label>
                            <asp:TextBox ID="txtTo" runat="server" onchange="javascript:text_changed_to();" class="form-control datepicker my-3 my-lg-0" placeholder="To Date" autocomplete="off"></asp:TextBox>
                            <asp:HiddenField ID="hdntxtTo" runat="server" />
                        </div>
                        <asp:HiddenField ID="h1" runat="server" />
                        <asp:HiddenField ID="h2" runat="server" />

                    </div>
                    <div class="row pl-2">
                        <div class="col-md-8">
                            <label style="color: deepskyblue">Distinct count :</label>
                            <asp:Label ID="lbldistinct" runat="server"></asp:Label>
                        </div>
                        <div class="col-md-4 text-end">
                            <asp:Button ID="btnsearch" CssClass="btn btn-success btn-sm" runat="server" Text="SEARCH" OnClick="btnsearch_Click" />
                            <asp:Button ID="btnkdownload" CssClass="btn btn-info btn-sm text-white" runat="server" Text="DOWNLOAD" OnClick="btnkdownload_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnkdownload" />
        </Triggers>
    </asp:UpdatePanel>
    <!-- Bootstrap core JavaScript-->
    <script src="vendor/jquery/jquery-3.5.1.min.js"></script>
    <script src="vendor/bootstrap/js/bootstrap.bundle.min.js"></script>

    <!-- Custom scripts for all pages-->
    <script src="js/sb-admin-2.min.js"></script>

    <!--  Date-->
    <script src="vendor/datepicker/moment.min.js"></script>
    <script src="vendor/datepicker/daterangepicker.min.js"></script>

    <script src="js/demo/date-range-picker-demo.js"></script>
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

</asp:Content>
