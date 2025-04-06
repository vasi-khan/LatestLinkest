<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="CampaignEntry.aspx.cs" Inherits="eMIMPanel.CampaignEntry" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
 th,  td {
    padding: 0.3rem !important;
    vertical-align: top;
    border-top: 0.0625rem solid #D1D9E6;
}
        .table thead th {
            vertical-align: middle;
            border-bottom: 0.125rem solid #6e6e6f;
            text-align: center;
        }

        .table th, .table td {
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="scrmg" runat="server" EnablePageMethods="true">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="updFormArea" runat="server" UpdateMode="Conditional">
        <%--<Triggers>
            <asp:PostBackTrigger ControlID="btnUpload" />
        </Triggers>--%>
        <ContentTemplate>
            <main>
                <div class="container-fluid">
                    <nav aria-label="breadcrumb" class="my-3">
                        <ol class="breadcrumb breadcrumb-info">
                            <li class="breadcrumb-item"><a href="#">Home</a></li>
                            <li class="breadcrumb-item"><a href="#">Campaign Entry Reports</a></li>
                            <li class="breadcrumb-item active" aria-current="page">Campaign Entry </li>
                        </ol>
                    </nav>

                    <!-- Content Row -->
                    <div class="row">
                        <div class="col-12">
                            <div class="card mb-4 bg-primary border-light shadow-soft">

                                <div id="divFileLoader" runat="server" style="display: none; text-align: center" class="form-group row">
                                    <h3>File uploading. Please wait...</h3>
                                    <img src="img/loading.gif" />
                                </div>


                                <div class="card-header py-3 bg-primary d-flex justify-content-between flex-wrap align-content-center">
                                    <h6 class="m-0 font-weight-bold font-weight-bold my-auto"><i class="far fa-id-card"></i>Campaign Entry</h6>
                                </div>
                                <div class="card-body">
                                    <div class="form-group row">
                                        <label for="inputEmail3" class="col-sm-2 col-form-label font-weight-bold">User ID :</label>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtUserid" runat="server" MaxLength="15" class="form-control" placeholder=""></asp:TextBox>
                                        </div>
                                        <label for="inputEmail3" class="col-sm-2 col-form-label font-weight-bold">Request Date :</label>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtFrm" runat="server" onchange="javascript:text_changed_from();" class="form-control datepicker" placeholder="From Date" autocomplete="off"></asp:TextBox>
                                            <asp:HiddenField ID="hdntxtFrm" runat="server" />
                                        </div>

                                    </div>

                                    <div class="form-group row">
                                        <label for="inputEmail3" class="col-sm-2 col-form-label font-weight-bold">Campaign Name :</label>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtcampname" runat="server" MaxLength="50" class="form-control" placeholder=""></asp:TextBox>
                                        </div>
                                        <label for="inputEmail33" class="col-sm-2 col-form-label font-weight-bold">File Name :</label>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtfname" runat="server" MaxLength="100" class="form-control" placeholder=""></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group row">
                                        <label for="inputEmail3" class="col-sm-2 col-form-label font-weight-bold">SMS Credit :</label>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtsmscredit" runat="server" MaxLength="100" class="form-control" placeholder="" onkeypress="return isNumberKeyWithDecimal(event)"></asp:TextBox>
                                        </div>
                                        <label for="inputEmail33" class="col-sm-2 col-form-label font-weight-bold">Submitted :</label>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtsubmitted" runat="server" MaxLength="100" class="form-control" placeholder="" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group row">
                                        <label for="inputEmail3" class="col-sm-2 col-form-label font-weight-bold">Delivered :</label>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtdelivered" runat="server" MaxLength="100" class="form-control" placeholder="" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                        </div>
                                        <label for="inputEmail33" class="col-sm-2 col-form-label font-weight-bold">Delivered (%) :</label>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtdlvrper" runat="server" MaxLength="19" class="form-control" placeholder="" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <label for="inputEmail3" class="col-sm-2 col-form-label font-weight-bold">Failed :</label>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtfailed" runat="server" MaxLength="100" class="form-control" placeholder="" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                        </div>
                                        <label for="inputEmail33" class="col-sm-2 col-form-label font-weight-bold">Failed (%) :</label>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtfldper" runat="server" MaxLength="100" class="form-control" placeholder="" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group row">
                                        <label for="inputEmail3" class="col-sm-2 col-form-label font-weight-bold">Awaited :</label>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtawaited" runat="server" MaxLength="100" class="form-control" placeholder="" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                        </div>
                                        <label for="inputEmail33" class="col-sm-2 col-form-label font-weight-bold">Awaited (%) :</label>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtawtdper" runat="server" MaxLength="100" class="form-control" placeholder="" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group row">
                                        <label for="inputEmail3" class="col-sm-2 col-form-label font-weight-bold">Hit Count :</label>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txthitcount" runat="server" MaxLength="100" class="form-control" placeholder="" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                        </div>
                                        <label for="inputEmail33" class="col-sm-2 col-form-label font-weight-bold">Hit Count (%) :</label>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txthitcntper" runat="server" MaxLength="100" class="form-control" placeholder="" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group row">
                                        <label for="inputEmail3" class="col-sm-2 col-form-label font-weight-bold">Sender ID :</label>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtsndrid" runat="server" MaxLength="50" class="form-control" placeholder=""></asp:TextBox>
                                        </div>

                                        <%--<label for="inputEmail33" class="col-sm-2 col-form-label font-weight-bold">File Name :</label>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtfname" runat="server" MaxLength="100" class="form-control" placeholder=""></asp:TextBox>
                                        </div>--%>
                                    </div>
                                    <div class="form-group row">
                                        <label for="exampleFormControlTextarea1" class="col-sm-2 col-form-label font-weight-bold">SMS Text:</label>
                                        <div class="col-sm-10">
                                            <asp:TextBox ID="txtmsg" runat="server" TextMode="MultiLine" Rows="5" MaxLength="500" class="form-control"></asp:TextBox>
                                            <small>(UpTo 500 char)</small>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <label for="inputEmail3" class="col-sm-2 col-form-label font-weight-bold">Upload File</label>
                                        <div class="col-sm-6">

                                            <div class="custom-file">
                                                <asp:FileUpload ID="FileUpload1" runat="server" accept=".csv" class="custom-file-input" ClientIDMode="Static" onchange="if( SMSfileUpload() ) { console.log('formsubmit'); this.form.submit(); }" />
                                                <label class="custom-file-label" for="customFile">Choose file</label>
                                                <asp:Label ID="lblfn" runat="server"></asp:Label>
                                            </div>
                                            <small>(Upload Only csv format)</small>
                                        </div>
                                    </div>
                                    <div class="form-group row justify-content-end">
                                        <div class="col-sm-10 ">
                                            <div class="row">
                                                <div class="col-6 col-lg-2">
                                                    <asp:LinkButton ID="btnSubmit" runat="server" class="btn btn-primary text-success font-weight-bold btn-block" OnClick="btnSubmit_Click"><i class="far fa-save text-success"></i> Submit</asp:LinkButton>
                                                </div>

                                                <div class="col-6 col-lg-2">
                                                    <asp:LinkButton ID="Lnkbtnreset" runat="server" class="btn btn-primary text-success font-weight-bold btn-block" OnClick="Lnkbtnreset_Click"><i class="far fa-save text-success"></i> Reset</asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>

                                        <%-- Template List POPUP --%>
                                    </div>
                                   <%-- <div class="row">
                                        <div class="col-md-12">--%>
                                            <div class="card">
                                                <div class="table-responsive">
                                                    <div id="divResult" runat="server">
                                                        <!-- <asp:Label ID="lblResult" Style="overflow-y: scroll" runat="server"></asp:Label> -->
                                                    </div>
                                                </div>
                                            </div>
                                       <%-- </div>
                                    </div>--%>
                                </div>
                            </div>
                        </div>

                    </div>

                    <style type="text/css">
                        /*CSS Classes For Design Modal*/
                        .modalPopup {
                            min-height: 75px;
                            position: fixed;
                            z-index: 2000;
                            padding: 0;
                            background-color: #fff;
                            border-radius: 6px;
                            background-clip: padding-box;
                            border: 1px solid rgba(0, 0, 0, 0.2);
                            min-width: 290px;
                            box-shadow: 0 5px 10px rgba(0, 0, 0, 0);
                        }

                        .modalBackground {
                            position: fixed;
                            top: 0;
                            left: 0;
                            background-color: #000;
                            opacity: 0.5;
                            z-index: 1800;
                            min-height: 100%;
                            width: 100%;
                            overflow: hidden;
                            filter: alpha(opacity=50);
                            display: inline-block;
                            z-index: 1000;
                        }
                    </style>
            </main>
        </ContentTemplate>
    </asp:UpdatePanel>
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
    <script type="text/javascript"> 
        function text_changed_from() {
            var d = document.getElementById("ContentPlaceHolder1_txtFrm").value
            console.log(d);
            document.getElementById("ContentPlaceHolder1_hdntxtFrm").value = d;
        }


        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }

        function isNumberKeyWithDecimal(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode != 46 && charCode > 31
                && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }






        function SMSfileUpload() {
            var uploadControl = document.getElementById('<%= FileUpload1.ClientID %>');
            var myfile = uploadControl.value;

            var fake_path = document.getElementById('FileUpload1').value
            var arr = fake_path.split("\\").pop().split(".");
            var fname = arr[0];
            if (fname.length > 50) {
                alert("Filename should be atleast 50 character !!");
                return false;
            }
            console.log(myfile);
            var Extension = myfile.substring(myfile.lastIndexOf('.') + 1).toLowerCase();
            console.log(Extension);

            //if (Extension == "txt") {
            //    if (uploadControl.files[0].size > 2 * 6291456) {
            //        //alert("Upload text file of size upto 6 MB only.");
            //        return true; 
            //    } else {
            //        showLoading();
            //        console.log("ret true");
            //        return true;
            //    }
            //}
            if (Extension == "csv") {
                if (uploadControl.files[0].size > 20971520) {
                    //alert("Upload csv file of size upto 20 MB only."); /*3 * 6291456*/
                    return true;
                } else {
                    showLoading();
                    console.log("ret true");
                    return true;
                }
            }

            //if (Extension == "xls" || Extension == "xlsx") {
            //    if (uploadControl.files[0].size > 5242880) {
            //        //alert("Upload Excel file of size upto 5 MB only.");
            //        return true;
            //    } else {
            //        showLoading();
            //        console.log("ret true");
            //        return true;
            //    }
            //}
        }

        function showLoading() {
            document.getElementById('<%= divFileLoader.ClientID %>').style.display = 'block';
            //document.getElementById('<%= lblfn.ClientID %>').innerText = "Uploading. Please Wait ... ";

        }
    </script>
</asp:Content>
