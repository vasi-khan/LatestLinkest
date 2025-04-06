<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="BulkUrlShortner.aspx.cs" Inherits="eMIMPanel.BulkUrlShortner" %>

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
                    <li class="breadcrumb-item"><a href="#">Bulk URL Shortner</a></li>
                </ol>
            </nav>
            <!-- Content Row -->
            <div class="row">
                <div class="col-12">
                    <div class="card mb-4 bg-primary border-light shadow-soft">
                        <div class="card-header py-3 bg-primary d-flex justify-content-between flex-wrap align-content-center border-bottom">
                            <h6 class="m-0 font-weight-bold font-weight-bold my-auto"><i class="far fa-address-book"></i>Bulk Url Shortner</h6>
                        </div>
                        <div class="card-body pt-0 pt-lg-4">
                            <form>
                                <div id="divFileLoader" runat="server" style="display: none; text-align: center" class="form-group row">
                                    <h3>File uploading. Please wait...</h3>
                                    <img src="img/loading.gif" />
                                </div>

                                <div class="form-group row">
                                    <label for="inputEmail3" class="col-sm-2 col-form-label font-weight-bold">User ID :</label>
                                    <div class="col-sm-5">
                                        <asp:TextBox ID="txtUserId" runat="server" class="form-control" MaxLength='12' placeholder="User ID"></asp:TextBox>
                                    </div>
                                </div>

                                <div id="divFileUpload" runat="server" class="form-group row">
                                    <label for="inputEmail3" class="col-sm-2 col-form-label font-weight-bold" style="margin-top: -10px;">Long URL File&nbsp;</label>
                                    <div class="col-md-6">
                                        <div class="custom-file">
                                            <asp:FileUpload ID="FileUpload1" runat="server" class="custom-file-input" ClientIDMode="Static" onchange="if( SMSfileUpload() ) { console.log('formsubmit'); this.form.submit(); }" />
                                            <label class="custom-file-label" for="customFile">Choose file</label>
                                            <p class="text-danger small mt-2 mb-0">Maximum File Size 6 MB <span class="text-danger">(CSV)</span>&nbsp;&nbsp;
                                                 <asp:LinkButton ID="lnkbtn" runat="server" OnClick="lnkbtn_Click"><i class="fas fa-info text-secondary bg-light rounded-circle small " ></i>
                                                                             <span class="tooltiptext01">Download File Format</span>
                                        </asp:LinkButton>
                                            </p>
                                           
                                        </div>
                                    </div>
                                    <div class="4">
                                        <asp:Label ID="lblUploading" runat="server"></asp:Label>
                                    </div>
                                </div>

                                <div class="form-group row justify-content-end">
                                    <div class="col-sm-10 ">
                                        <div class="row">
                                            <div class="col-6 col-lg-6">
                                                <%--<asp:LinkButton ID="btnSave" runat="server" class="btn btn-primary text-secondary font-weight-bold btn-block"
                                                OnClick="btnSave_Click"> 
                                                        <span><i class="fas fa-save text-secondary"></i> Save</span>
                                                </asp:LinkButton>--%>
                                                <asp:LinkButton ID="btnShortUrl" runat="server" class="btn btn-primary text-secondary font-weight-bold btn-block"
                                                    OnClick="btnShortUrl_Click"> 
                                                        <span><i class="fas fa-save text-secondary"></i> Make Short URL And Download</span>
                                                </asp:LinkButton>
                                            </div>
                                            <%--<div class="col-6 col-lg-3">--%>
                                            <%--<asp:LinkButton ID="btnClear" runat="server" class="btn btn-primary text-danger font-weight-bold btn-block"
                                                OnClick="btnClear_Click"> 
                                                        <span><i class="fas fa-times"></i> Clear</span>
                                                </asp:LinkButton>--%>
                                            <%--</div>--%>
                                        </div>
                                    </div>
                                </div>
                            </form>
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
        function showLoading() {
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
        }

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
