<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="AddDistributionList.aspx.cs" Inherits="eMIMPanel.AddDistributionList" %>

<%@ MasterType VirtualPath="~/Site2.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .select2-container--default .select2-selection--single {
            background-color: #e6e7ee !important;
            border-radius: 4px;
            border: 1px solid #aaa !important;
            box-shadow: inset 2px 2px 5px #b8b9be, inset -3px -3px 7px #ffffff !important;
        }

        .select2-search--dropdown .select2-search__field {
            padding: 4px;
            width: 100%;
            box-sizing: border-box;
            background: #e6e7ee !important;
            border: 1px solid red;
        }
        /*CSS Classes For Design Modal*/
        .modal.modalPopup {
            top: 0 !important;
            left: 0 !important;
            display: block;
        }

        .modalBackground {
            background-color: #000;
            opacity: 0.5;
        }
    </style>
    <script>
        function Confirm() {
            if (confirm(' Are you sure to delete this group \n Data can not be restore after delete. ')) {
                return true;
            }
            else {
                return false;
            }
        }
    </script>
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
                    <li class="breadcrumb-item active" aria-current="page">Add Distribution List</li>
                </ol>
            </nav>
            <!-- Content Row -->
            <div class="row">
                <div class="col-12">
                    <div class="card card-body mb-4 bg-primary border-light shadow-soft">
                        <div class="form-row">
                            <div class="form-group col-md-2 mt-auto">
                                <label for="#" class="font-weight-bold">Add Distribution List</label>
                            </div>
                            <div class="form-group col-md-4">
                                <asp:TextBox ID="txtGroupName" runat="server" class="form-control" MaxLength="120" placeholder="Enter Distribution Name"></asp:TextBox>
                            </div>
                            <div class="form-group col-md-2 mt-auto">
                                <asp:LinkButton ID="btnCreate" runat="server" class="btn btn-primary text-secondary btn-block" OnClick="btnCreate_Click">ADD</asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-12">
                    <div class="card card-body mb-4 bg-primary border-light shadow-soft">
                        <div class="form-row">
                            <div class="form-group col-md-2 mt-auto">
                                <label for="#" class="font-weight-bold">Search By</label>
                            </div>
                            <div class="form-group col-md-3">
                                <asp:DropDownList ID="ddlSerachType" runat="server" class="custom-select">
                                    <asp:ListItem Value="Distribution">Distribution</asp:ListItem>
                                    <asp:ListItem Value="Mobile">Mobile</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="form-group col-md-3">
                                <asp:TextBox ID="txtSearchText" runat="server" class="form-control" MaxLength="50" placeholder="Enter Search Text"></asp:TextBox>
                            </div>
                            <div class="form-group col-md-2 mt-auto">
                                <asp:LinkButton ID="lnkSearch" runat="server" class="btn btn-primary text-secondary btn-block" OnClick="lnkSearch_Click">Search</asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Content Row -->
            <div class="row">
                <div class="col-xl-12 col-lg-12">
                    <!-- Basic Card Example -->
                    <div class="card bg-primary border-light shadow-soft mb-4">
                        <div class="card-header py-3 bg-primary d-flex justify-content-between flex-wrap align-content-center">
                            <h6 class="font-weight-bold my-lg-auto mb-3"><i class="far fa-clock"></i>Distribution List</h6>
                        </div>
                        <div class="card-body">
                            <div class="table-responsive">
                                <asp:GridView ID="grv" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
                                    runat="server" Width="100%" CellPadding="10" BorderColor="#ede8e8" Class="table table-striped table-bordered dt-responsive nowrap dataTable-view">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Sr No">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                                <asp:Label ID="lblGroupID" runat="server" Text='<%#Eval("GroupID")%>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblGroupName" runat="server" Text='<%#Eval("GroupName")%>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Group Name">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkGroupNameWithCount" runat="server" class="text-secondary" OnClick="lnkGroupNameWithCount_Click"
                                                    data-toggle="tooltip" data-placement="top" title="Show Distribution Details" Text='<%#Eval("GroupNameWithCount")%>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Created">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCreatedDateTime" runat="server" Text='<%#Eval("InsertDateTime")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="SMS">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnSMSSend" runat="server" OnClick="btnSMSSend_Click" class="btn btn-primary text-secondary"
                                                    data-toggle="tooltip" data-placement="top" title="Send SMS"><i class="fas fa-paper-plane"></i></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Contacts">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnADD" runat="server" OnClick="btnADD_Click" class="btn btn-primary text-secondary" CommandName="Add"
                                                    data-toggle="tooltip" data-placement="top" title="ADD Mobile Number"><i class="fas fa-plus"></i></asp:LinkButton>
                                                <asp:LinkButton ID="btnText" runat="server" OnClick="btnADD_Click" class="btn btn-primary text-secondary" CommandName="Text"
                                                    data-toggle="tooltip" data-placement="top" title="Upload Text File"><i class="fas fa-file"></i></asp:LinkButton>
                                                <asp:LinkButton ID="btnExcel" runat="server" OnClick="btnADD_Click" class="btn btn-primary text-secondary" CommandName="Excel"
                                                    data-toggle="tooltip" data-placement="top" title="Upload CSV File"><i class="fas fa-file-excel"></i></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Actions">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnEdit" runat="server" OnClick="btnEdit_Click" class="btn btn-primary text-secondary"
                                                    data-toggle="tooltip" data-placement="top" title="Edit Distribution"><i class="fas fa-edit"></i></asp:LinkButton>
                                                <asp:LinkButton ID="btnDelete" runat="server" OnClick="btnDelete_Click" class="btn btn-primary text-secondary" OnClientClick="return confirmDelete();"
                                                    data-toggle="tooltip" data-placement="top" title="Delete Distribution"><i class="fas fa-trash"></i></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="5%">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkAll" runat="server" onclick="ToggleCheckboxes(this)" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkItem" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <div class="text-center">
                                    <asp:LinkButton ID="btnSendAll" runat="server" CssClass="btn btn-primary text-secondary"
                                        data-toggle="tooltip" data-placement="top" title="Send" OnClick="btnSendAll_Click">
                                        <i class="fas fa-paper-plane"></i> Send
                                     </asp:LinkButton>
                                    &nbsp;
                                    <asp:LinkButton ID="btnDeleteAll" runat="server" OnClientClick="return confirmDelete();" CssClass="btn btn-primary text-secondary"
                                        data-toggle="tooltip" data-placement="top" title="Delete" OnClick="btnDeleteAll_Click">
                                       <i class="fas fa-trash"></i> Delete
                                     </asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </main>

    <%--Modal Start--%>

    <%--lnkUploadNumberModal Link Button for ModalPopup as TargetControlID--%>
    <asp:LinkButton ID="lnkUploadNumberModal" runat="server"></asp:LinkButton>

    <%--pnlPopUp_Detail Panel With Design--%>
    <asp:Panel ID="pnlPopUp_UploadNumber" runat="server" CssClass="modal modalPopup" Style="display: none;">
        <div class="modal-dialog modal-xl modal-dialog-centered modal-dialog-scrollable">
            <div class="modal-content">
                <div class="modal-header">
                    <asp:Label ID="lblHeading" runat="server" CssClass="modal-title" Text="Distribution Details"></asp:Label>
                </div>
                <div class="modal-body">
                    <div class="">
                        <div id="divFileLoader" runat="server" style="display: none; text-align: center" class="form-group row">
                            <h3>File uploading. Please wait...</h3>
                            <img src="img/loading.gif" />
                        </div>
                        <div class="form-group row">
                            <label for="inputEmail3" class="col-sm-2 col-form-label font-weight-bold">Country :</label>
                            <div class="col-sm-5">
                                <asp:DropDownList ID="ddlCountry" runat="server" class="custom-select"></asp:DropDownList>
                            </div>
                        </div>
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
                        <div id="divMannuel" runat="server" class="form-group row">
                            <label for="exampleFormControlTextarea1" class="col-sm-2 col-form-label font-weight-bold">Add Numbers</label>
                            <div class="col-sm-10">
                                <asp:TextBox ID="txtMobNum" runat="server" class="form-control" MaxLength='50' onkeyDown="checkTextAreaMaxLength(this,event,'10');"
                                    TextMode="MultiLine" Rows="5" placeholder="Enter Mobile No." onkeyup="integersOnly(this); mobnumbcnt(); return true;"></asp:TextBox>
                                <small>Separate Number using Comma<,> or Enter</small>
                            </div>
                        </div>
                        <div class="form-group row" style="margin-left: 90%;">
                            <span class="font-weight-bold" style="font-size: smaller;">Number Count:</span><asp:Label ID="lblMobileCnt" runat="server" Style="font-size: smaller;"></asp:Label>
                        </div>
                        <div class="form-group row justify-content-end">
                            <div class="col-sm-10 ">
                                <div class="row">
                                    <div class="col-6 col-lg-3">
                                        <asp:LinkButton ID="btnUploadMobileNo" runat="server" class="btn btn-primary text-secondary font-weight-bold btn-block" OnClick="btnUploadMobileNo_Click">
                                                        <span><i class="fas fa-save text-secondary"></i> Save</span>
                                        </asp:LinkButton>
                                    </div>
                                    <div class="col-6 col-lg-3">
                                        <asp:LinkButton ID="btnClear" runat="server" class="btn btn-primary text-danger font-weight-bold btn-block">
                                                        <span><i class="fas fa-times"></i> Clear</span>
                                        </asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button id="btnCancel" runat="server" class="btn btn-primary">Close</button>
                </div>
            </div>
        </div>
    </asp:Panel>

    <%--pnlPopUp_Detail Modal Popup Extender For pnlPopUp_Detail--%>
    <asp:ModalPopupExtender ID="pnlPopUp_UploadNumber_Modal" runat="server" PopupControlID="pnlPopUp_UploadNumber"
        TargetControlID="lnkUploadNumberModal" CancelControlID="btnCancel" BackgroundCssClass="modalBackground">
    </asp:ModalPopupExtender>

    <%--Modal End--%>

    <%--Modal Start--%>

    <%--lnkUploadNumberModal Link Button for ModalPopup as TargetControlID--%>
    <asp:LinkButton ID="lnkShowDetailsGroup" runat="server"></asp:LinkButton>

    <%--pnlPopUp_Detail Panel With Design--%>
    <asp:Panel ID="PanelShowDetailsGroup" runat="server" CssClass="modal modalPopup" Style="display: none;">
        <div class="modal-dialog modal-xl modal-dialog-centered modal-dialog-scrollable">
            <div class="modal-content">
                <div class="modal-header">
                    <asp:Label ID="Label1" runat="server" CssClass="modal-title" Text="Distribution Details"></asp:Label>
                </div>
                <div class="modal-body">
                    <div class="">
                        <asp:GridView ID="grvDistributionDetails" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
                            runat="server" Width="100%" CellPadding="10"
                            BorderColor="#ede8e8" Class="table table-striped table-bordered dt-responsive nowrap dataTable-view">
                            <Columns>
                                <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Sr No">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>
                                        <asp:Label ID="lblGroupID" runat="server" Text='<%#Eval("Group_ID")%>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Mobile">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMobile" runat="server" Text='<%#Eval("Mobile")%>'></asp:Label>
                                        <asp:TextBox ID="txtMobile" runat="server" Text='<%#Eval("Mobile")%>' Visible="false"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Created">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCreatedDateTime" runat="server" Text='<%#Eval("InsertDateTime")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="SMS">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnSMSSendManuel" runat="server" OnClick="btnSMSSendManuel_Click" class="btn btn-primary text-secondary" data-toggle="tooltip" data-placement="top" title="Send SMS"><i class="fas fa-paper-plane"></i></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Actions">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnEditManuel" runat="server" OnClick="btnEditManuel_Click" class="btn btn-primary text-secondary"
                                            data-toggle="tooltip" data-placement="top" title="Edit Distribution"><i class="fas fa-edit"></i></asp:LinkButton>
                                        <asp:LinkButton ID="lnkUpdateManuel" runat="server" OnClick="lnkUpdateManuel_Click" class="btn btn-primary text-secondary"
                                            data-toggle="tooltip" data-placement="top" title="Edit Distribution" Visible="false"><i class="fas fa-upload"></i></asp:LinkButton>
                                        <asp:LinkButton ID="btnDeleteManuel" runat="server" OnClick="btnDeleteManuel_Click" class="btn btn-primary text-secondary"
                                            data-toggle="tooltip" data-placement="top" title="Delete Distribution" OnClientClick="return confirmDelete();">
                                            <i class="fas fa-trash"></i></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="5%">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkAll" runat="server" onclick="ToggleCheckboxesDetails(this)" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkItem" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <div class="text-center">
                    <asp:LinkButton ID="lnlSendAllDetails" runat="server" CssClass="btn btn-primary text-secondary"
                        data-toggle="tooltip" data-placement="top" title="Send" OnClick="lnlSendAllDetails_Click">
                                        <i class="fas fa-paper-plane"></i> Send
                                     </asp:LinkButton>
                    &nbsp;
                                    <asp:LinkButton ID="lnkSendAllDelete" runat="server" OnClientClick="return confirmDelete();" CssClass="btn btn-primary text-secondary"
                                        data-toggle="tooltip" data-placement="top" title="Delete" OnClick="lnkSendAllDelete_Click">
                                       <i class="fas fa-trash"></i> Delete
                                     </asp:LinkButton>
                </div>
                <div class="modal-footer">
                    <button id="btnCancelDetails" runat="server" class="btn btn-primary">Close</button>
                </div>
            </div>
        </div>
    </asp:Panel>

    <%--pnlPopUp_Detail Modal Popup Extender For pnlPopUp_Detail--%>
    <asp:ModalPopupExtender ID="ModalPopupExtenderShowDetails" runat="server" PopupControlID="PanelShowDetailsGroup"
        TargetControlID="lnkShowDetailsGroup" CancelControlID="btnCancelDetails" BackgroundCssClass="modalBackground">
    </asp:ModalPopupExtender>

    <%--Modal End--%>
    <script type="text/javascript">
        function ToggleCheckboxes(headerCheckbox) {
            var checkboxes = document.querySelectorAll('#<%= grv.ClientID %> input[type="checkbox"]');
            for (var i = 0; i < checkboxes.length; i++) {
                checkboxes[i].checked = headerCheckbox.checked;
            }
        }
        function ToggleCheckboxesDetails(headerCheckbox) {
            var checkboxes = document.querySelectorAll('#<%= grvDistributionDetails.ClientID %> input[type="checkbox"]');
            for (var i = 0; i < checkboxes.length; i++) {
                checkboxes[i].checked = headerCheckbox.checked;
            }
        }
   </script>
    <script type="text/javascript">
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

        function SMSfileUpload() {
            var uploadControl = document.getElementById('<%= FileUpload1.ClientID %>');
            var myfile = uploadControl.value;
            var Extension = myfile.substring(myfile.lastIndexOf('.') + 1).toLowerCase();
            var fileSize = uploadControl.files[0].size;
            switch (Extension) {
                case "txt":
                case "csv":
                    if (fileSize > 6291456) {
                        alert("Upload " + Extension.toUpperCase() + " file of size up to 6 MB only.");
                        return false;
                    }
                    break;
                case "xls":
                case "xlsx":
                    if (fileSize > 5242880) {
                        alert("Upload " + Extension.toUpperCase() + " file of size up to 5 MB only.");
                        return false;
                    }
                    break;
                default:
                    alert("Please upload a file with one of the following extensions: txt, csv, xls, xlsx.");
                    return false;
            }
            showLoading();
            console.log("File upload validation successful.");
            return true;
        }

        function showLoading() {
            document.getElementById('<%= divFileLoader.ClientID %>').style.display = 'block';
            document.getElementById('<%= lblUploading.ClientID %>').innerHTML = "Uploading. Please Wait ... ";

        }
    </script>
    <script type="text/javascript">
        function checkTextAreaMaxLength(textBox, e, length) {
            var mLen = textBox["MaxLength"];
            if (null == mLen)
                mLen = length;

            var maxLength = parseInt(mLen);
            if (!integersOnly(e)) {
                if (textBox.value.length > maxLength - 1) {
                    if (window.event)
                        e.returnValue = false;
                    else
                        e.preventDefault();
                }
            }
        }
    </script>
    <script type="text/javascript">
        function confirmDelete() {
            return confirm('Are you sure you want to delete this item?');
        }
    </script>
</asp:Content>
