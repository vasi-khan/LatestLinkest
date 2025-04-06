<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="add-phone-number.aspx.cs" Inherits="eMIMPanel.add_phone_number" %>

<%@ MasterType VirtualPath="~/Site2.Master" %>
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
                    <li class="breadcrumb-item"><a href="#">Phone Book</a></li>
                    <li class="breadcrumb-item active" aria-current="page">Add Phone Number </li>
                </ol>
            </nav>
            <!-- Content Row -->
            <div class="row">
                <div class="col-12">
                    <div class="card mb-4 bg-primary border-light shadow-soft">
                        <div class="card-header py-3 bg-primary d-flex justify-content-between flex-wrap align-content-center border-bottom">
                            <h6 class="m-0 font-weight-bold font-weight-bold my-auto"><i class="far fa-address-book"></i> Add Phone Number</h6>
                        </div>
                        <div class="card-body pt-0 pt-lg-4">
                            <form>
                                <div id="divFileLoader" runat="server" style="display: none; text-align: center" class="form-group row">
                                    <h3>File uploading. Please wait...</h3>
                                    <img src="img/loading.gif" />
                                </div>
                                <div class="form-group row">
                                    <label for="inputEmail3" class="col-sm-2 col-form-label font-weight-bold">Group :</label>
                                    <div class="col-sm-5">
                                        <asp:DropDownList ID="ddlGrp" runat="server" class="custom-select">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group row">
                                    <label for="inputEmail3" class="col-sm-2 col-form-label font-weight-bold">Country :</label>
                                    <div class="col-sm-5">
                                        <asp:DropDownList ID="ddlCountry" runat="server" class="custom-select"></asp:DropDownList>
                                    </div>
                                </div>

                                <fieldset class="form-group">
                                    <div class="row">
                                        <legend class="col-form-label col-sm-2 pt-0 font-weight-bold">Mobile Numbers</legend>
                                        <div class="col-sm-10">
                                            <div class="custom-control custom-radio custom-control-inline pl-2">
                                                <asp:RadioButton class="mr-2" ID="rdbEntry" runat="server" Checked="true" GroupName="mobile" AutoPostBack="true" OnCheckedChanged="rdbUpload_CheckedChanged" />
                                                <label>Enter Number <i class="far fa-keyboard text-dark"></i></label>
                                            </div>
                                            <div class="custom-control custom-radio custom-control-inline pl-2">
                                                <asp:RadioButton class="mr-2" ID="rdbUpload" runat="server" GroupName="mobile" AutoPostBack="true" OnCheckedChanged="rdbUpload_CheckedChanged" />
                                                <label>
                                                    Upload Files <i class="fas fa-file-csv text-primary"></i>
                                                    <br>
                                                     <small>(XLS/CSV/TXT)</small></label>
                                            </div>
                                            
                                        </div>
                                    </div>
                                </fieldset>

                                <div id="divFileUpload" runat="server" class="form-group row d-none">
                                    <label for="inputEmail3" class="col-sm-2 col-form-label font-weight-bold" style="margin-top: -10px;">Mobile Numbers File&nbsp;</label>
                                    <div class="col-md-6">
                                        <div class="custom-file">
                                            <asp:FileUpload ID="FileUpload1" runat="server" class="custom-file-input" ClientIDMode="Static" onchange="if( SMSfileUpload() ) { console.log('formsubmit'); this.form.submit(); }" />
                                            <label class="custom-file-label" for="customFile">Choose file</label>
                                            <p class="text-danger small mt-2 mb-0">Maximum File Size 6 MB</p>
                                        </div>
                                    </div>
                                    <div class="4">
                                        <asp:Label ID="lblUploading" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="form-group row">
                                    <label for="exampleFormControlTextarea1" class="col-sm-2 col-form-label font-weight-bold">Add Numbers</label>
                                    <div class="col-sm-10">

                                        <asp:TextBox ID="txtMobNum" runat="server" class="form-control" MaxLength='50' onkeyDown="checkTextAreaMaxLength(this,event,'10');" TextMode="MultiLine" Rows="5" placeholder="Enter Mobile No." onkeyup="integersOnly(this); mobnumbcnt(); return true;" ></asp:TextBox>
                                        <%--<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterMode="ValidChars"
                                            TargetControlID="txtMobNum" ValidChars="0123456789, ">
                                        </asp:FilteredTextBoxExtender>--%>
                                        <small>Separate Number using Comma<,> or Enter</small> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <span class="font-weight-bold" style="font-size: smaller;">Number Count:</span>
                                            <asp:Label ID="lblMobileCnt" runat="server" Style="font-size: smaller;"></asp:Label>
                                    </div>
                                    <%-- mobile count here label--%>
                                    <div class="d-flex">
                                        <p class="my-2">
                                            
                                        </p>
                                    </div>

                                </div>
                                <div class="form-group row justify-content-end">
                                    <div class="col-sm-10 ">
                                        <div class="row">
                                            <div class="col-6 col-lg-3">
                                                <asp:LinkButton ID="btnSave" runat="server" class="btn btn-primary text-secondary font-weight-bold btn-block"
                                                OnClick="btnSave_Click"> 
                                                        <span><i class="fas fa-save text-secondary"></i> Save</span>
                                                </asp:LinkButton>
                                            </div>
                                            <div class="col-6 col-lg-3">
                                                <asp:LinkButton ID="btnClear" runat="server" class="btn btn-primary text-danger font-weight-bold btn-block"
                                                OnClick="btnClear_Click"> 
                                                        <span><i class="fas fa-times"></i> Clear</span>
                                                </asp:LinkButton>
                                            </div>
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
            obj.value = obj.value.replace(/[^0-9,\r\n]/g,'');
        }
        function mobnumbcnt() {
            var s = document.getElementById("<%=txtMobNum.ClientID%>").value;
            if (s != '') {
                var str = s;
                var x = 0;
                for (var i = 0; i < str.length; i++) {
                    if (str.charAt(i) == ',') x++;
                }
                for (var i = 0; i < str.length; i++) {
                    if (str.charAt(i) == '\n') x++;
                }

                if (x > 0) {
                    document.getElementById('<%= lblMobileCnt.ClientID %>').innerHTML = (x + 1).toString();
                }
                else
                    document.getElementById('<%= lblMobileCnt.ClientID %>').innerHTML = "";
                if (s != '' && x == 0)
                    document.getElementById('<%= lblMobileCnt.ClientID %>').innerHTML = "1";
            }
            else {
                document.getElementById('<%= lblMobileCnt.ClientID %>').innerHTML = "";
            }
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
            if (Extension == "txt") {
                if (uploadControl.files[0].size > 6291456) {
                    alert("Upload text file of size upto 6 MB only.");
                    return false;
                }
                else {
                    showLoading();
                    console.log("ret true");
                    return true;
                }
            }
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
            if (Extension == "xls" || Extension == "xlsx") {
                if (uploadControl.files[0].size > 5242880) {
                    alert("Upload Excel file of size upto 5 MB only.");
                    return false;
                }
                else {
                    showLoading();
                    console.log("ret true");
                    return true;
                }
            }

        }

        function checkTextAreaMaxLength(textBox,e, length)
        {

        var mLen = textBox["MaxLength"];
        if(null==mLen)
            mLen=length;

        var maxLength = parseInt(mLen);
        if(!integersOnly(e))
        {
         if(textBox.value.length > maxLength-1)
         {
            if(window.event)//IE
              e.returnValue = false;
            else//Firefox
                e.preventDefault();
         }
        }   
        }

    </script>
</asp:Content>
