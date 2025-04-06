<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="add-template_u.aspx.cs" Inherits="eMIMPanel.add_template_u" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
 
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
                            <li class="breadcrumb-item"><a href="#">Requests</a></li>
                            <li class="breadcrumb-item active" aria-current="page">Add Template </li>
                        </ol>
                    </nav>

                    <!-- Content Row -->
                    <div class="row">
                        <div class="col-12">
                            <div class="card mb-4 bg-primary border-light shadow-soft">
                                <div class="card-header py-3 bg-primary d-flex justify-content-between flex-wrap align-content-center">
                                    <h6 class="m-0 font-weight-bold font-weight-bold my-auto"><i class="far fa-id-card"></i>Add Template</h6>
                                </div>
                                <div class="card-body">
                                    <div class="form-group row" id="idsenderidALL" runat="server" visible="false">
                                        <label for="inputEmail3" class="col-sm-2 col-form-label font-weight-bold">SenderId:</label>
                                        <div class="col-sm-10">
                                            <asp:TextBox ID="txtSenderIdall" runat="server" MaxLength="15" class="form-control" placeholder="" ></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group row" id="idsenderidIND" runat="server" visible="false">
                                        <label for="inputEmail3" class="col-sm-2 col-form-label font-weight-bold">SenderId:</label>
                                        <div class="col-sm-10">
                                            <asp:TextBox ID="tstsenderind" runat="server" MaxLength="6" class="form-control" placeholder="" onkeypress="if(event.keyCode==44 || event.keyCode>31 &&event.keyCode<48 ||event.keyCode>57 && event.keyCode<64  || event.keyCode>90 && event.keyCode<97 || event.keyCode>122 && event.keyCode<128)event.returnValue=false;" ></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <label for="inputEmail3" class="col-sm-2 col-form-label font-weight-bold">Template Name :</label>
                                        <div class="col-sm-10">
                                            <asp:TextBox ID="txtName" runat="server" MaxLength="100" class="form-control" placeholder=""></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <label for="inputEmail33" class="col-sm-2 col-form-label font-weight-bold">Template ID :</label>
                                        <div class="col-sm-10">
                                            <asp:TextBox ID="txtTempId" runat="server" class="form-control" placeholder="" onkeypress="return onlyNumbersWithColon(event);"></asp:TextBox>

                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <label for="exampleFormControlTextarea1" class="col-sm-2 col-form-label font-weight-bold">Message:</label>
                                        <div class="col-sm-10">
                                            <asp:TextBox ID="txtmsg" runat="server" TextMode="MultiLine" Rows="5" MaxLength="500" class="form-control"></asp:TextBox>
                                            <small>(UpTo 500 char)</small>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <label for="inputEmail3" class="col-sm-2 col-form-label font-weight-bold">DLT Screenshot</label>
                                        <div class="col-sm-6">

                                            <div class="custom-file">
                                                <asp:FileUpload ID="FileUpload1" runat="server" class="custom-file-input" ClientIDMode="Static" onchange="this.form.submit()" />
                                                <label class="custom-file-label" for="customFile">Choose file</label>
                                                <%--<asp:Button ID="btnUpload" runat="server" Text="Upload" class="btn btn-medium btn-theme btn-rounded" Style="margin-top: 5px;" OnClick="btnUpload_Click" />--%>
                                                <asp:Label ID="lblfn" runat="server"></asp:Label>
                                            </div>
                                            <small>(Upload Only image/pdf/winrar format)</small>
                                        </div>
                                    </div>
                                    <div class="form-group row justify-content-end">
                                        <div class="col-sm-10 ">
                                            <div class="row">
                                                <div class="col-6 col-lg-2">
                                                    <asp:LinkButton ID="btnSubmit" runat="server" class="btn btn-primary text-success font-weight-bold btn-block" OnClick="btnSubmit_Click"><i class="far fa-save text-success"></i> Add New</asp:LinkButton>
                                                </div>
                                                <div class="col-6 col-lg-2">
                                                    <asp:LinkButton ID="btnList" runat="server" class="btn btn-primary text-info font-weight-bold btn-block" OnClick="btnList_Click"><i class="fas fa-list text-info"></i> List</asp:LinkButton>
                                                </div>
                                                <div class="col-12 col-lg-2">
                                                    <asp:LinkButton ID="btnCancel" runat="server" class="btn btn-primary text-danger font-weight-bold btn-block mt-3 mt-lg-0" OnClick="btnCancel_Click"><i class="fas fa-times text-danger"></i> Close</asp:LinkButton>
                                                </div>
                                                <div class="col-12 col-lg-3">
                                                    <asp:LinkButton ID="btnDownload" runat="server" class="btn btn-primary text-success font-weight-bold btn-block" OnClick="btnDownload_Click"><i class="fas fa-download"></i> Download</asp:LinkButton>
                                                </div>
                                            </div>

                                            <%--<asp:LinkButton ID="btnSubmit" runat="server" class="btn btn-primary text-success font-weight-bold" OnClick="btnSubmit_Click"><i class="fas fa-paper-plane"></i> Submit</asp:LinkButton>--%>
                                            <%--<asp:LinkButton ID="btnCancel" runat="server"  class="btn btn-primary text-danger font-weight-bold" OnClick="btnCancel_Click"><i class="fas fa-times"></i> Cancel</asp:LinkButton>--%>
                                        </div>
                                    </div>

                                    <%-- Template List POPUP --%>
                                    <asp:Panel ID="pnlPopUp_NUMBER" runat="server" CssClass="modalPopup" Style="display: none;">
                                        <div style="overflow-y: auto; overflow-x: hidden; max-height: 450px;">
                                            <div class="modal-header">
                                                <asp:Label ID="Label132" runat="server" CssClass="modal-title" Text="Template List"></asp:Label>
                                            </div>
                                            <div class="modal-body">
                                                <div>
                                                    <asp:GridView ID="grvTemplate" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
                                                        runat="server" Width="100%" CellPadding="10"
                                                        BorderColor="#ede8e8">
                                                        <Columns>
                                                            <asp:TemplateField HeaderStyle-Width="5%" HeaderText="Sr No">
                                                                <ItemTemplate>
                                                                    <%#Container.DataItemIndex+1 %>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Template Id" HeaderStyle-Width="10%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTemplateId" runat="server" Width="100%" Text='<%#Eval("TemplateId")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Template Name" HeaderStyle-Width="20%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTemplateName" runat="server" Text='<%#Eval("tempname")%>' Width="100%"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Template" HeaderStyle-Width="65%">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtTemplate" runat="server" TextMode="MultiLine" ReadOnly="true" Text='<%#Eval("Template")%>' Rows="3" Width="100%"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            <table style="width: 100%;">
                                                                <tr>
                                                                    <td align="center">
                                                                        <asp:Label ID="lblEmpty" Text="No Data Found!!!" Style="color: Red;" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </EmptyDataTemplate>
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                            <div align="center" class="modal-footer">
                                                <div class="row">
                                                    <div class="col-md-6">
                                                        <button id="btnCancel2" runat="server" class="btn btn-primary">Cancel</button>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>

                                    <asp:LinkButton ID="lnkNumber" runat="server"></asp:LinkButton>

                                    <asp:ModalPopupExtender ID="pnlPopUp_NUMBER_ModalPopupExtender" runat="server" BackgroundCssClass="modalBackground"
                                        PopupControlID="pnlPopUp_NUMBER" TargetControlID="lnkNumber" CancelControlID="btnCancel2">
                                    </asp:ModalPopupExtender>

                                </div>
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
            <script type="text/javascript">
                function onlyNumbersWithColon(e) {
                    var CountyCode = '<%= Session["DEFAULTCOUNTRYCODE"].ToString() %>';
                    debugger;
                    if (CountyCode == "91") {
                        var charCode;
                        if (e.keyCode > 0) {
                            charCode = e.which || e.keyCode;
                        }
                        else if (typeof (e.charCode) != "undefined") {
                            charCode = e.which || e.keyCode;
                        }
                        if (charCode == 58)
                            return true
                        if (charCode > 31 && (charCode < 48 || charCode > 57))
                            return false;
                        return true;
                    }
                    else {
                        return true;
                    }
                }
    </script>
        </ContentTemplate>
      
    </asp:UpdatePanel>

</asp:Content>
