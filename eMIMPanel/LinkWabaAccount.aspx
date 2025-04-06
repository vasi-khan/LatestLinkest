<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="LinkWabaAccount.aspx.cs" Inherits="eMIMPanel.LinkWabaAccount" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="scrmg" runat="server" EnablePageMethods="true">
    </asp:ToolkitScriptManager>

    <main>
        <div class="container-fluid">
            <nav aria-label="breadcrumb" class="my-3">
                <ol class="breadcrumb breadcrumb-info">
                    <li class="breadcrumb-item"><a href="#">Home</a></li>
                    <li class="breadcrumb-item"><a href="#">Link WABA Account</a></li>
                </ol>
            </nav>
            <!-- Content Row -->
            <div class="row">
                <div class="col-12">
                    <div class="card mb-4 bg-primary border-light shadow-soft">
                        <div class="card-body pt-0 pt-lg-4">
                            <form>
                                <div class="form-group row">
                                    <div class="col-sm-2">
                                        <asp:CheckBox ID="ChklinkWabaAC" runat="server" />
                                    </div>
                                    <div class="col-sm-5">
                                        <label class="form-check-label" for="gridCheck2">Link WABA Account </label>
                                    </div>
                                </div>
                                <div class="form-group row">
                                    <label for="inputEmail3" class="col-sm-2 col-form-label font-weight-bold">Linkext Account ID :</label>
                                    <div class="col-sm-5">
                                        <asp:TextBox ID="txtUserId" runat="server" class="form-control" MaxLength='12' placeholder="Account ID"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:LinkButton ID="btnVerifyAC" runat="server" OnClick="btnVerifyAC_Click" class="btn btn-primary text-secondary font-weight-bold btn-block"> 
                                                        <span>Go</span>
                                        </asp:LinkButton>
                                    </div>
                                </div>

                                <div runat="server" id="divVerify" visible="false">
                                    <div class="form-group row">
                                        <div class="col-sm-2"></div>
                                        <div class="col-sm-5">
                                            <label class="form-check-label font-weight-bold" for="gridCheck2">Verify Your WABA Account</label>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <label for="inputEmail3" class="col-sm-2 col-form-label font-weight-bold">WABA Account ID :</label>
                                        <div class="col-sm-5">
                                            <asp:TextBox ID="txtWabaAccountID" runat="server" class="form-control" MaxLength='12' placeholder="WABA Account ID"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-2">
                                            <asp:LinkButton ID="LnkbtnSentOTP" runat="server" OnClick="LnkbtnSentOTP_Click" class="btn btn-primary text-secondary font-weight-bold btn-block"> 
                                                        <span>Verify</span>
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                            </form>
                        </div>
                    </div>


                    <div class="card mb-4 bg-primary border-light shadow-soft" runat="server" id="Maindiv" visible="false">
                        <div class="card-body pt-0 pt-lg-4">
                            <div runat="server" visible="false" id="divVerifyOTP">
                                <div class="form-group row">
                                    <div class="col-sm-2"></div>
                                    <div class="col-sm-5">
                                        <label class="form-check-label font-weight-bold" for="gridCheck2">We have sent an OTP on the customer's mobile number of the WABA Account.</label>
                                    </div>
                                </div>
                                <div class="form-group row">
                                    <label for="inputEmail3" class="col-sm-2 col-form-label font-weight-bold">OTP :</label>
                                    <div class="col-sm-5">
                                        <asp:TextBox ID="txtOTP" runat="server" class="form-control" MaxLength='6' placeholder="Enter OTP"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:LinkButton ID="LnkbtnSubmit" runat="server" OnClick="LnkbtnSubmit_Click" class="btn btn-primary text-secondary font-weight-bold btn-block"> 
                                                        <span>Submit</span>
                                        </asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                            <div runat="server" id="divVerified" visible="false">
                                <div class="form-group row">
                                    <div class="col-sm-5">
                                        <h4><i class="fas fa-certificate" style="color: #63E6BE;"></i> Verified</h4>
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
        function integersOnly(obj) {
            obj.value = obj.value.replace(/[^0-9,\r\n]/g, '');
        }
        <%--function showLoading() {
            document.getElementById('<%= divFileLoader.ClientID %>').style.display = 'block';
            document.getElementById('<%= lblUploading.ClientID %>').innerHTML = "Uploading. Please Wait ... ";

        }

        function SMSfileUpload() {

            var uploadControl = document.getElementById('<%= FileUpload1.ClientID %>');
            var myfile = uploadControl.value;
            console.log(myfile);
            var Extension = myfile.substring(myfile.lastIndexOf('.') + 1).toLowerCase();
            console.log(Extension);
            if (Extension == "csv") {
                if (uploadControl.files[0].size > 6291456) {
                    alert("Upload csv file of size upto 6 MB only.");
                    return false;
                }
                else {
                    showLoading();
                    console.log("ret true");
                    return true;
                }
            }
            else {
                alert("Please Upload CSV File !!");
            }
        }--%>

        function checkTextAreaMaxLength(textBox, e, length) {

            var mLen = textBox["MaxLength"];
            if (null == mLen)
                mLen = length;

            var maxLength = parseInt(mLen);
            if (!integersOnly(e)) {
                if (textBox.value.length > maxLength - 1) {
                    if (window.event)//IE
                        e.returnValue = false;
                    else//Firefox
                        e.preventDefault();
                }
            }
        }

    </script>
</asp:Content>
