<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="BlackListNo.aspx.cs" Inherits="eMIMPanel.BlackListNo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="ms-Dropdown-master/css/msdropdown/dd.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="scrmg" runat="server" EnablePageMethods="true">
    </asp:ToolkitScriptManager>
    <div class="container-fluid">
        <div class="col-12 col-xl-12 col-lg-6 col-md-12 col-sm-6 mb-6">
            <div class="card bg-primary shadow-soft border-light">
                <div class="row no-gutters align-items-center">
                    <div class="col-sm-12">
                        <!-- Header -->
                        <div class="card-header text-center pb-0">
                            <h4 class="mb-0">Add Blacklist Mobile No</h4>
                            <div id="divFileLoader" runat="server" style="display: none; text-align: center" class="form-group row">
                                <h3>File uploading. Please wait...</h3>
                                <img src="img/loading.gif" />
                            </div>
                            </div>
                        <div class="card-body pt-0">
                            <div class="row">
                                <div class="col-md-2">
                                        <asp:RadioButton class="mr-2" ID="rdbUserIdBase" runat="server" Checked="true" GroupName="TypeGU" AutoPostBack="true" OnCheckedChanged="rdbUserIdBase_CheckedChanged"/>
                                        <label for="rdbUpload" class="col-form-label font-weight-bold">User Id based</label>
                                </div>
                                <div class="col-md-10">
                                        <asp:RadioButton class="mr-2" ID="rdbGlobal" runat="server" GroupName="TypeGU" AutoPostBack="true" OnCheckedChanged="rdbGlobal_CheckedChanged" />
                                        <label for="rdbEntry" class="col-form-label font-weight-bold">Global</label>
                                 </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3" id="userdiv" runat="server">
                                    <asp:TextBox ID="txtUser" runat="server" class="form-control" placeholder="User Id" ToolTip="User Id"></asp:TextBox>
                                </div>
                                <div class="col-md-6">
                                    <label for="exampleFormControlTextarea1" class="col-sm-2 col-form-label font-weight-bold">Option</label>
                                    <div class="custom-control custom-radio custom-control-inline pl-2">
                                        <asp:RadioButton class="mr-2" ID="rdbUpload" runat="server" GroupName="mobile" AutoPostBack="true" OnCheckedChanged="rdbUpload_CheckedChanged" />
                                        <label for="rdbUpload">
                                            Upload Files <i class="fas fa-file-csv text-dark"></i>
                                            <small>(TXT/XLS)</small></label>
                                    </div>
                                    <div class="custom-control custom-radio custom-control-inline pl-2">
                                        <asp:RadioButton class="mr-2" ID="rdbEntry" runat="server" Checked="true" GroupName="mobile" AutoPostBack="true" OnCheckedChanged="rdbEntry_CheckedChanged" />
                                        <label for="rdbEntry">Enter Number <i class="far fa-keyboard text-dark"></i></label>
                                    </div>
                                </div>
                                <div class="col-md-3" style="display:none">
                                    <label for="ddlOperator">Operator</label>
                                    <asp:DropDownList ID="ddlOperator" runat="server" class="custom-select">
                                        <asp:ListItem Text="JIO" Value="JIO"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-3" style="display:none">
                                    <label for="txtDate">Date</label>
                                    <asp:TextBox ID="txtDate" runat="server" onchange="javascript:text_changed_from();" class="form-control datepicker" placeholder="Date" autocomplete="off"></asp:TextBox>
                                    <asp:HiddenField ID="hdntxtDate" runat="server" />
                                    <%--<asp:CalendarExtender TargetControlID="txtDate" Format="dd/MM/yyy" runat="server" ID="cldate"></asp:CalendarExtender>--%>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div id="divFileUpload" runat="server" class="form-group row d-none" visible="false">
                                        <label for="inputEmail3" class="col-sm-6 col-form-label font-italic" style="margin-top: -10px;">Mobile Numbers File&nbsp;</label>
                                        <div class="col-md-12">
                                            <div class="custom-file">
                                                <%-- this.form.submit(); --%>
                                                <asp:FileUpload ID="FileUpload1" runat="server" class="custom-file-input" ClientIDMode="Static" />
                                                <label class="custom-file-label" for="customFile">Choose file</label>
                                                <p id="ptextnum" runat="server" class="text-danger small mt-2 mb-0">Maximum File Size 6 MB <b>(Mobile Numbers without Country Code)</b></p>
                                                <p id="ptextglobal" runat="server" class="text-danger small mt-2 mb-0" visible="false">Maximum File Size 6 MB <b>(Mobile Numbers with Country Code)</b></p>
                                            </div>
                                        </div>
                                        <div class="4">
                                            <asp:Label ID="lblUploading" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <div id="divNum" runat="server" style="pointer-events: all;">
                                        <div class="d-flex">
                                            <span id="spantextnum" runat="server" class="text-danger small mt-2 mb-0" style="font-size: smaller;"><b>Enter Mobile Numbers without Country Code</b></span>
                                            <span id="spantextglobal" runat="server" class="text-danger small mt-2 mb-0" style="font-size: smaller;" visible="false"><b>Enter Mobile Numbers with Country Code</b></span>
                                        </div>
                                        <asp:TextBox ID="txtMobNum" runat="server" class="form-control" TextMode="MultiLine" Rows="5" MaxLength="2147483647" onkeyup="integersOnly(this); mobnumbcnt(); return true;"></asp:TextBox>
                                        <%--<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterMode="ValidChars"
                                                    TargetControlID="txtMobNum" ValidChars="0123456789, ">
                                                </asp:FilteredTextBoxExtender>--%>
                                        <asp:Label ID="lblMobileCnt" runat="server" Style="font-size: smaller; display: none"></asp:Label>
                                    </div>
                                </div>
                                <div class="col-md-3" style="display:none">
                                    <asp:CheckBox ID="chkAsperUser" onclick="showhideuser(this)" runat="server" />
                                    <label class="form-check-label" for="chkAsperUser">As Per User </label>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="col-md-12" style="text-align:center">
                                    <asp:LinkButton ID="lnkSave" runat="server" class="mx-0 btn btn-primary text-secondary small" OnClientClick="if( SMSfileUpload() ) { console.log('formsubmit'); }" OnClick="lnkSave_Click">
                                     <span class="text-secondary" style="font-size:smaller;"> <i class="fas fa-save text-secondary"></i> Save </span>
                                    </asp:LinkButton>
                                </div>
                                <%--     <div class="col-md-4">
                            <asp:LinkButton ID="lnkUser" runat="server" class="mx-0 btn btn-primary text-secondary small" OnClick="lnkUser_Click">
                                  <span class="text-secondary" style="font-size:smaller;"> <i class="fas fa-save text-secondary"></i> As Per User </span>
                            </asp:LinkButton>
                        </div>--%>
                            </div>
                        </div>
                </div>
            </div>
            </div>
        </div>
    </div>
    <script>
        function text_changed_from() {
            var d = document.getElementById("ContentPlaceHolder1_txtDate").value
            console.log(d);
            document.getElementById("ContentPlaceHolder1_hdntxtDate").value = d;
        }
        function integersOnly(obj) {
            obj.value = obj.value.replace(/[^0-9,\r\n]/g, '');
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
                } else
                    document.getElementById('<%= lblMobileCnt.ClientID %>').innerHTML = "";
                if (s != '' && x == 0)
                    document.getElementById('<%= lblMobileCnt.ClientID %>').innerHTML = "1";
            } else {
                document.getElementById('<%= lblMobileCnt.ClientID %>').innerHTML = "";
            }
        }
        function SMSfileUpload() {
            var uploadControl = document.getElementById('<%= FileUpload1.ClientID %>');
            var myfile = uploadControl.value;
            console.log(myfile);
            var Extension = myfile.substring(myfile.lastIndexOf('.') + 1).toLowerCase();
            console.log(Extension);
            if (Extension == "txt") {
                if (uploadControl.files[0].size > 2 * 6291456) {
                    alert("Upload text file of size upto 6 MB only.");
                    return false;
                } else {
                    showLoading();
                    console.log("ret true");
                    return true;
                }
            }
            if (Extension == "xls" || Extension == "xlsx") {
                if (uploadControl.files[0].size > 5242880) {
                    alert("Upload Excel file of size upto 5 MB only.");
                    return false;
                } else {
                    showLoading();
                    console.log("ret true");
                    return true;
                }
            }
        }
        function showLoading() {
            document.getElementById('<%= divFileLoader.ClientID %>').style.display = 'block';
            document.getElementById('<%= lblUploading.ClientID %>').innerHTML = "Uploading. Please Wait ... ";

        }
        function showhideuser(obj) {
            debugger;
            if (obj.checked) {
                document.getElementById('<%= userdiv.ClientID %>').style.display = 'block';
            }
            else {
                document.getElementById('<%= userdiv.ClientID %>').style.display = 'none';
            }
        }
    </script>
</asp:Content>
