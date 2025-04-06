<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="allot-sender-id.aspx.cs" Inherits="eMIMPanel.allot_sender_id" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="ms-Dropdown-master/css/msdropdown/dd.css" rel="stylesheet" />
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>
    <script type="text/javascript" src="ms-Dropdown-master/js/jquery/jquery-1.9.0.min.js"></script>
    <script type="text/javascript" src="ms-Dropdown-master/js/msdropdown/jquery.dd.js"></script>
   <%-- <script type="text/javascript" type="text/javascript">
        $(document).ready(function () {
            $("#ddlCCode").msDropDown();
        });
    </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<form runat="server">--%>
    <cc1:ToolkitScriptManager ID="scrmg" runat="server" EnablePageMethods="true">
    </cc1:ToolkitScriptManager>

    <main>
        <div class="container-fluid">
            <nav aria-label="breadcrumb" class="my-3">
                <ol class="breadcrumb breadcrumb-info">
                    <li class="breadcrumb-item"><a href="#">Home</a></li>
                    <li class="breadcrumb-item"><a href="#">Sender ID</a></li>
                    <li class="breadcrumb-item active" aria-current="page">Allot Sender ID</li>
                </ol>
            </nav>
            <!-- Content Row -->
            <div class="row">

                <!-- Area Chart -->
                <div class="col-xl-12 col-lg-12">
                    <!-- Basic Card Example -->
                    <div class="card bg-primary border-light shadow-soft mb-4">
                        <div class="card-header py-3 bg-primary d-flex justify-content-between flex-wrap align-content-center">
                            <h6 class="m-0 font-weight-bold font-weight-bold my-auto"><i class="far fa-id-card"></i>Allot Sender ID</h6>
                            <%--<div class="dateRangeView">--%>
                            <div class="row">
                                <div class="col-md-auto">
                                    <asp:Button ID="btnConfirm" runat="server" class="btn btn-mini" Text="SenderId List"
                                        PostBackUrl="~/SenderListA.aspx" />
                                </div>
                              <%--  <div class="col-md-3">
                                    <asp:TextBox ID="txtFrm" runat="server" onchange="javascript:text_changed_from();" class="form-control datepicker" placeholder="From Date" autocomplete="off"></asp:TextBox>
                                    <asp:HiddenField ID="hdntxtFrm" runat="server" />
                                </div>
                                <div class="col-md-3">
                                    <asp:TextBox ID="txtTo" runat="server" onchange="javascript:text_changed_to();" class="form-control datepicker" placeholder="To Date" autocomplete="off"></asp:TextBox>
                                    <asp:HiddenField ID="hdntxtTo" runat="server" />
                                </div>--%>
                                
                                    <Label class="p-2">User Id</Label>
                                <div class="col-md-3 form-group">
                                    <asp:TextBox ID="txtUser" class="form-control" runat="server" MaxLength="20"></asp:TextBox>
                                </div>
                                <Label class="p-2">Company Name</Label>
                                <div class="col-md-3">
                                    
                                    <asp:TextBox ID="txtcompnm" class="form-control" runat="server" MaxLength="300"></asp:TextBox>
                                </div>
                                <%--<a class="btn btn-primary text-dark btn-block" id="reportrange" role="button" aria-pressed="true">
                                                <i class="fas fa-calendar mr-2 text-dark"></i>
                                                <span class="text-dark"></span><i class="ml-1 fas fa-chevron-down" data-feather="chevron-down"></i>
                                            </a>--%>
                                <div class="col-md-auto">
                                    <asp:LinkButton runat="server" ID="LinkButton1" OnClick="btnUpdate_Click" class="btn btn-mini">
                                                    Show <i class="fas fa-eye" aria-hidden="true"></i>
                                    </asp:LinkButton>

                                </div>

                                <asp:HiddenField ID="h1" runat="server" />
                                <asp:HiddenField ID="h2" runat="server" />
                            </div>
                            <%--</div>--%>
                        </div>
                        <div class="card-body">
                            <asp:GridView ID="grv" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
                                runat="server" Width="100%" CellPadding="10" BorderColor="#ede8e8" Class="table table-striped table-bordered dt-responsive nowrap dataTable-view">
                                <Columns>
                                    <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Sr No">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnAllot1" runat="server" class="mx-1 btn btn-primary text-dark" OnClick="btnAllot_Click" data-toggle="tooltip" data-placement="top" title="Allot" data-original-title="View"> <span class="text-secondary"> <i class="fas fa-user-plus"></i> </span></asp:LinkButton>
                                            <asp:LinkButton ID="lnkbtnRemove" runat="server" class="mx-1 btn btn-primary text-dark d-none" OnClick="lnkbtnRemove_Click" data-toggle="tooltip" data-placement="top" title="Remove" data-original-title="View"> <span class="text-secondary"> <i class="fas fa-user-minus"></i> </span></asp:LinkButton>

                                            <%--<asp:Button ID="btnAllot" runat="server" class="mx-1 btn btn-primary text-dark" Text="Allot" OnClick="btnAllot_Click" />--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="User Id">
                                        <ItemTemplate>
                                            <asp:Label ID="lbluserid" runat="server" Text='<%#Eval("username")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Company Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl1" runat="server" Text='<%#Eval("compname")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Full Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl2" runat="server" Text='<%#Eval("fullname")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Mobile No">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl3" runat="server" Text='<%#Eval("mobile")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Email ID">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl4" runat="server" Text='<%#Eval("Email")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sender ID">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl5" runat="server" Text='<%#Eval("sender")%>'></asp:Label>
                                            <asp:HiddenField ID="hdnUserId" runat="server" Value='<%#Eval("username")%>' />
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
                </div>

            </div>
        </div>

        <!-- Modal Popup for Sender ID Creation  --------------->
        <asp:HiddenField ID="hdn1q" runat="server" EnableViewState="False" ViewStateMode="Disabled" />
        <cc1:ModalPopupExtender ID="modalpopupallot" runat="server" TargetControlID="hdn1q"
            PopupControlID="pnlpopupallot" BackgroundCssClass="modelpopupback" CancelControlID="Button12">
        </cc1:ModalPopupExtender>
        <div class="col-md-12">
            <asp:Panel ID="pnlpopupallot" runat="server" Style="width: 50%; min-height: 175px; overflow: auto; display: none;" align="center">
                <div class="col-md-12 no-spacing">
                    <table id="tblpopup" runat="server" style="width: 100%; background: white;">
                        <tr>
                            <td>
                                <div class="col-md-12">
                                    <asp:Label ID="lblheadpopup" runat="server" Text="Allot Sender ID"></asp:Label>
                                    <asp:Button ID="Button12" runat="server" Text="X" Style="float: right;" CssClass="popclosebutton" />
                                </div>
                                <%-- <div class="modal-header">
                                    <h2 class="h6 modal-title mb-0" id="modal-title-default">Allot Sender Id</h2>
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"  CssClass="popclosebutton" >
                                        <span aria-hidden="true">×</span>
                                    </button>
                                </div>--%>
                                <div class="modal-body" style="overflow-y: auto; height: 70vh;">
                                    <div class="form-row">
                                        <label for="inputEmail33" class="col-sm-3 col-form-label font-weight-bold" style="margin-top: -10px;">Country Code</label>
                                        <div class="col-md-6">
                                            <div id="divmobile" runat="server" class="form-label-group">
                                                <asp:DropDownList ID="ddlCCode" runat="server" class="custom-select"></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-row mt-2">
                                        <asp:HiddenField ID="hdnAllotCount" runat="server" ClientIDMode="Static" />
                                        <div class="col-md-12">
                                            <div class="input-group mb-3">
                                                <asp:TextBox ID="txtSender" runat="server" class="form-control" placeholder="Enter Sender Id" aria-label="Add Sender Id" aria-describedby="button-addon2" />
                                                <cc1:FilteredTextBoxExtender ID="ftrtxtSender" runat="server" FilterMode="ValidChars"
                                                    TargetControlID="txtSender" ValidChars=" ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-">
                                                </cc1:FilteredTextBoxExtender>
                                                <div class="input-group-append">
                                                    <button class="btn btn-primary" type="button" onclick="SetAllot1()" id="btnAllot"><i class="fas fa-user-plus"></i></button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-row" id="divAllot1" runat="server" style="display: none" clientidmode="Static">
                                        <asp:HiddenField ID="hdnAllot1" runat="server" ClientIDMode="Static" />
                                        <div class="col-md-12">
                                            <div class="input-group mb-3">
                                                <asp:TextBox ID="txtSender1" runat="server" class="form-control"  placeholder="Enter Sender Id" aria-label="Add Sender Id" aria-describedby="button-addon2" />
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterMode="ValidChars"
                                                    TargetControlID="txtSender1" ValidChars=" ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-">
                                                </cc1:FilteredTextBoxExtender>
                                                <div class="input-group-append">
                                                    <button class="btn btn-primary" type="button" onclick="SetAllot2()" id="btnAllot1"><i class="fas fa-user-plus"></i></button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-row" id="divAllot2" runat="server" style="display: none" clientidmode="Static">
                                        <asp:HiddenField ID="hdnAllot2" runat="server" ClientIDMode="Static" />

                                        <div class="col-md-12">
                                            <div class="input-group mb-3">
                                                <asp:TextBox ID="txtSender2" runat="server" class="form-control" placeholder="Enter Sender Id" aria-label="Add Sender Id" aria-describedby="button-addon2" />
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterMode="ValidChars"
                                                    TargetControlID="txtSender2" ValidChars=" ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-">
                                                </cc1:FilteredTextBoxExtender>

                                                <div class="input-group-append">
                                                    <button class="btn btn-primary" type="button" onclick="SetAllot3()" id="btnAllot2"><i class="fas fa-user-plus"></i></button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-row" id="divAllot3" runat="server" style="display: none" clientidmode="Static">
                                        <asp:HiddenField ID="hdnAllot3" runat="server" ClientIDMode="Static" />

                                        <div class="col-md-12">
                                            <div class="input-group mb-3">
                                                <asp:TextBox ID="txtSender3" runat="server" class="form-control" placeholder="Enter Sender Id" aria-label="Add Sender Id" aria-describedby="button-addon2" />
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterMode="ValidChars"
                                                    TargetControlID="txtSender3" ValidChars=" ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-">
                                                </cc1:FilteredTextBoxExtender>
                                                <div class="input-group-append">
                                                    <button class="btn btn-primary" type="button" onclick="SetAllot4()" id="btnAllot3"><i class="fas fa-user-plus"></i></button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-row" id="divAllot4" runat="server" style="display: none" clientidmode="Static">
                                        <asp:HiddenField ID="hdnAllot4" runat="server" ClientIDMode="Static" />

                                        <div class="col-md-12">
                                            <div class="input-group mb-3">
                                                <asp:TextBox ID="txtSender4" runat="server" class="form-control" placeholder="Enter Sender Id" aria-label="Add Sender Id" aria-describedby="button-addon2" />
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" FilterMode="ValidChars"
                                                    TargetControlID="txtSender4" ValidChars=" ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-">
                                                </cc1:FilteredTextBoxExtender>
                                                <div class="input-group-append">
                                                    <button class="btn btn-primary" type="button" onclick="SetAllot5()" id="btnAllot4"><i class="fas fa-user-plus"></i></button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-row" id="divAllot5" runat="server" style="display: none" clientidmode="Static">
                                        <asp:HiddenField ID="hdnAllot5" runat="server" ClientIDMode="Static" />

                                        <div class="col-md-12">
                                            <div class="input-group mb-3">
                                                <asp:TextBox ID="txtSender5" runat="server" class="form-control" placeholder="Enter Sender Id" aria-label="Add Sender Id" aria-describedby="button-addon2" />
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" FilterMode="ValidChars"
                                                    TargetControlID="txtSender5" ValidChars=" ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-">
                                                </cc1:FilteredTextBoxExtender>
                                                <div class="input-group-append">
                                                    <button class="btn btn-primary" type="button" onclick="SetAllot6()" id="btnAllot5"><i class="fas fa-user-plus"></i></button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="form-row" id="divAllot6" runat="server" style="display: none" clientidmode="Static">
                                        <asp:HiddenField ID="hdnAllot6" runat="server" ClientIDMode="Static" />

                                        <div class="col-md-12">
                                            <div class="input-group mb-3">
                                                <asp:TextBox ID="txtSender6" runat="server" class="form-control" placeholder="Enter Sender Id" aria-label="Add Sender Id" aria-describedby="button-addon2" />
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" FilterMode="ValidChars"
                                                    TargetControlID="txtSender6" ValidChars=" ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-">
                                                </cc1:FilteredTextBoxExtender>
                                                <div class="input-group-append">
                                                    <button class="btn btn-primary" type="button" onclick="SetAllot7()" id="btnAllot6"><i class="fas fa-user-plus"></i></button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-row" id="divAllot7" runat="server" style="display: none" clientidmode="Static">
                                        <asp:HiddenField ID="hdnAllot7" runat="server" ClientIDMode="Static" />

                                        <div class="col-md-12">
                                            <div class="input-group mb-3">
                                                <asp:TextBox ID="txtSender7" runat="server" class="form-control" placeholder="Enter Sender Id" aria-label="Add Sender Id" aria-describedby="button-addon2" />
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" FilterMode="ValidChars"
                                                    TargetControlID="txtSender7" ValidChars=" ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-">
                                                </cc1:FilteredTextBoxExtender>
                                                <div class="input-group-append">
                                                    <button class="btn btn-primary" type="button" onclick="SetAllot8()" id="btnAllot7"><i class="fas fa-user-plus"></i></button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-row" id="divAllot8" runat="server" style="display: none" clientidmode="Static">
                                        <asp:HiddenField ID="hdnAllot8" runat="server" ClientIDMode="Static" />

                                        <div class="col-md-12">
                                            <div class="input-group mb-3">
                                                <asp:TextBox ID="txtSender8" runat="server" class="form-control" placeholder="Enter Sender Id" aria-label="Add Sender Id" aria-describedby="button-addon2" />
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" FilterMode="ValidChars"
                                                    TargetControlID="txtSender8" ValidChars=" ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-">
                                                </cc1:FilteredTextBoxExtender>
                                                <div class="input-group-append">
                                                    <button class="btn btn-primary" type="button" onclick="SetAllot9()" id="btnAllot8"><i class="fas fa-user-plus"></i></button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-row" id="divAllot9" runat="server" style="display: none" clientidmode="Static">
                                        <asp:HiddenField ID="hdnAllot9" runat="server" ClientIDMode="Static" />

                                        <div class="col-md-12">
                                            <div class="input-group mb-3">
                                                <asp:TextBox ID="txtSender9" runat="server" class="form-control" placeholder="Enter Sender Id" aria-label="Add Sender Id" aria-describedby="button-addon2" />
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" FilterMode="ValidChars"
                                                    TargetControlID="txtSender9" ValidChars=" ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-">
                                                </cc1:FilteredTextBoxExtender>

                                            </div>
                                        </div>
                                    </div>

                                </div>
                                <div class="modal-footer">
                                    <asp:Button type="button" runat="server" class="btn btn-sm btn-primary" OnClick="btnAllotUpdate_Click" Text="Update" />
                                    <asp:Button type="button" class="btn btn-primary text-danger ml-auto" data-dismiss="modal" runat="server" OnClick="btnClosePopup_Click" Text="Close" />
                                </div>

                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
        </div>

        <!-- <<<<-----------   Modal Popup for Sender ID Remove    -->

        <asp:HiddenField ID="hdn1qrm" runat="server" EnableViewState="False" ViewStateMode="Disabled" />
        <cc1:ModalPopupExtender ID="ModalPopuprm" runat="server" TargetControlID="hdn1qrm"
            PopupControlID="pnlpopupallotrm" BackgroundCssClass="modelpopupback" CancelControlID="Button12rm">
        </cc1:ModalPopupExtender>
        <div class="col-md-12">
            <asp:Panel ID="pnlpopupallotrm" runat="server" Style="width: 50%; height: 280px; overflow: auto; display: none;" align="center">
                <div class="col-md-12 no-spacing">
                    <table id="Table1" runat="server" style="width: 100%; background: white;">
                        <tr>
                            <td>
                                <div class="col-md-12">
                                    <asp:Label ID="lblheadpopuprm" runat="server" Text="Remove Sender ID"></asp:Label>
                                    <asp:Button ID="Button12rm" runat="server" Text="X" Style="float: right;" CssClass="popclosebutton" />
                                </div>
                                <%-- <div class="modal-header">
                                    <h2 class="h6 modal-title mb-0" id="modal-title-default">Allot Sender Id</h2>
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"  CssClass="popclosebutton" >
                                        <span aria-hidden="true">×</span>
                                    </button>
                                </div>--%>
                                <div class="modal-body">
                                    <div class="form-row">
                                        <div class="col-md-12">
                                            <div class="input-group mb-3">
                                                <%--<input type="text" class="form-control" placeholder="Enter Sender Id" aria-label="Add Sender Id" aria-describedby="button-addon2">--%>
                                                <asp:TextBox ID="txtSenderrm" runat="server" class="form-control" placeholder="Enter Sender Id" aria-label="Remove Sender Id" aria-describedby="button-addon2" />
                                                <cc1:FilteredTextBoxExtender ID="ftrtxtSenderrm" runat="server" FilterMode="ValidChars"
                                                    TargetControlID="txtSenderrm" ValidChars="ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-">
                                                </cc1:FilteredTextBoxExtender>

                                                <div class="input-group-append">
                                                    <button class="btn btn-primary" type="button" id="button-addon2rm"><i class="fas fa-user-plus"></i></button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <asp:Button type="button" ID="btnRemove" runat="server" class="btn btn-sm btn-primary" OnClick="btnRemove_Click" Text="Remove" />
                                    <asp:Button type="button" class="btn btn-primary text-danger ml-auto" data-dismiss="modal" runat="server" OnClick="btnClosePopup_Click" Text="Close" />
                                </div>

                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
        </div>

    </main>
    <%-- </form>--%>
    <!--   popup -->
    <div id="DivPopUp" style="position: absolute; display: none; background: #FAFBDB;">
        <div class="modal-dialog modal-dialog-centered" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h2 class="h6 modal-title mb-0" id="modal-title-default2">Allot Sender Id</h2>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">×</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="needs-validation">
                        <div class="form-row">
                            <div class="col-md-12">
                                <div class="input-group mb-3">
                                    <input type="text" class="form-control" placeholder="Enter Sender Id" aria-label="Add Sender Id" aria-describedby="button-addon2">
                                    <div class="input-group-append">
                                        <button class="btn btn-primary" type="button" id="button-addon2"><i class="fas fa-user-plus"></i></button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-sm btn-primary">Update</button>
                    <button type="button" class="btn btn-primary text-danger ml-auto" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
    <!-- SMS Rate Management Modal -->
    <div class="modal fade" id="modal-default" tabindex="-1" role="dialog" aria-labelledby="modal-default" aria-hidden="true">
    </div>
    <!-- End of SMS Rate Management Modal Content -->

    <!-- Bootstrap core JavaScript-->
    <script src="vendor/jquery/jquery-3.5.1.min.js"></script>
    <script src="vendor/bootstrap/js/bootstrap.bundle.min.js"></script>

    <!-- Core plugin JavaScript-->
    <script src="vendor/jquery-easing/jquery.easing.min.js"></script>

    <!--  Date-->
    <script src="vendor/datepicker/moment.min.js"></script>
    <script src="vendor/datepicker/daterangepicker.min.js"></script>

    <!-- Custom scripts for all pages-->
    <script src="js/sb-admin-2.min.js"></script>

    <!-- Page level plugins -->
    <!-- <script src="vendor/chart.js/Chart.min.js"></script>  -->

    <!-- Page level custom scripts -->
    <!-- <script src="js/demo/chart-pie-demo.js"></script> 
        <script src="js/demo/chart-bar-demo.js"></script>  -->

    <!-- Page level plugins -->
    <script src="vendor/datatables/jquery.dataTables.min.js"></script>
    <script src="vendor/datatables/dataTables.bootstrap4.min.js"></script>
    <script src="vendor/datatables/dataTables.responsive.min.js"></script>
    <script src="vendor/datatables/responsive.bootstrap4.min.js"></script>

    <!--  Select-->
    <script src="vendor/select/bootstrap-select.min.js"></script>

    <!-- Page level custom scripts -->
    <script src="js/demo/datatables-demo.js"></script>
    <script src="js/demo/date-range-picker-demo.js"></script>

    <script type="text/javascript">
        function SetAllot1() {
            document.getElementById('divAllot1').style.display = "";
            document.getElementById('hdnAllot1').value = 1;
            document.getElementById('hdnAllotCount').value = 2;
        }
        function SetAllot2() {
            document.getElementById('divAllot2').style.display = "";
            document.getElementById('hdnAllot2').value = 1;
            document.getElementById('hdnAllotCount').value = 3;
        }
        function SetAllot3() {
            document.getElementById('divAllot3').style.display = "";
            document.getElementById('hdnAllot3').value = 1;
            document.getElementById('hdnAllotCount').value = 4;
        }
        function SetAllot4() {
            document.getElementById('divAllot4').style.display = "";
            document.getElementById('hdnAllot4').value = 1;
            document.getElementById('hdnAllotCount').value = 5;
        }
        function SetAllot5() {
            document.getElementById('divAllot5').style.display = "";
            document.getElementById('hdnAllot5').value = 1;
            document.getElementById('hdnAllotCount').value = 6;
        }
        function SetAllot6() {
            document.getElementById('divAllot6').style.display = "";
            document.getElementById('hdnAllot6').value = 1;
            document.getElementById('hdnAllotCount').value = 7;
        }
        function SetAllot7() {
            document.getElementById('divAllot7').style.display = "";
            document.getElementById('hdnAllot7').value = 1;
            document.getElementById('hdnAllotCount').value = 8;
        }
        function SetAllot8() {
            document.getElementById('divAllot8').style.display = "";
            document.getElementById('hdnAllot8').value = 1;
            document.getElementById('hdnAllotCount').value = 9;
        }
        function SetAllot9() {
            document.getElementById('divAllot9').style.display = "";
            document.getElementById('hdnAllot9').value = 1;
            document.getElementById('hdnAllotCount').value = 10;
        }
    </script>


    <script type="text/javascript">  
       <%-- $(document).ready(function () {
            //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

            //function EndRequestHandler(sender, args) {
            //    $('.classTarget').daterangepicker({ dateFormat: 'dd-mm-yy' });
            //}

            getdt();
        });



        function getdt() {
            var s1 = $('#reportrange').data('daterangepicker').startDate.format('YYYY-MM-DD');
            var s2 = $('#reportrange').data('daterangepicker').endDate.format('YYYY-MM-DD');

            document.getElementById('<%= h1.ClientID %>').value = s1;
            document.getElementById('<%= h2.ClientID %>').value = s2;
        }

        $('#reportrange').daterangepicker();
        $('#reportrange').on('apply.daterangepicker', function (ev, picker) {
            console.log(picker.startDate.format('YYYY-MM-DD'));
            console.log(picker.endDate.format('YYYY-MM-DD'));
        });--%>
</script>
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
