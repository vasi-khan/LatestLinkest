<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="add-sender-id_u.aspx.cs" Inherits="eMIMPanel.add_sender_id_u" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
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
    <asp:ToolkitScriptManager ID="scrmg" runat="server" EnablePageMethods="true">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="updFormArea" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnUpload" />
        </Triggers>
        <ContentTemplate>
            <main>
                <div class="container-fluid">
                    <nav aria-label="breadcrumb" class="my-3">
                        <ol class="breadcrumb breadcrumb-info">
                            <li class="breadcrumb-item"><a href="#">Home</a></li>
                            <li class="breadcrumb-item"><a href="#">Requests</a></li>
                            <li class="breadcrumb-item active" aria-current="page">Add Sender ID </li>
                        </ol>
                    </nav>

                    <div class="alert alert-success alert-dismissible shadow-soft fade show" role="alert">
                        <span class="alert-inner--icon"><span class="fas fa-info-circle"></span></span>
                        <span class="alert-inner--text">Sample File <a href="#" class="alert-link" data-toggle="modal" data-target="#exampleModal">Documents for Sender-ID's</a>. Give it a click if you like.</span>
                        <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                            <span aria-hidden="true">×</span>
                        </button>
                    </div>

                    <!-- Content Row -->
                    <div class="row">
                        <div class="col-12">
                            <div class="card mb-4 bg-primary border-light shadow-soft">
                                <div class="card-header py-3 bg-primary d-flex justify-content-between flex-wrap align-items-center border-bottom">
                                    <h6 class="m-0 font-weight-bold font-weight-bold my-auto"><i class="far fa-id-card"></i>Add Sender Id</h6>
                                </div>
                                <div class="card-body pt-0 pt-lg-4">
                                    <div id="div10" runat="server" class="form-group row">
                                        <label for="inputEmail33" class="col-sm-2 col-form-label font-weight-bold" style="margin-top: -10px;">Country Code</label>
                                        <div class="col-md-5">
                                            <div id="divmobile" runat="server" class="form-label-group">
                                                <asp:DropDownList ID="ddlCCode" runat="server" class="custom-select" AutoPostBack="true" OnSelectedIndexChanged="ddlCCode_SelectedIndexChanged"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="5">
                                        </div>
                                    </div>

                                    <div class="form-group row">
                                        <label for="inputEmail3" class="col-sm-2 col-form-label font-weight-bold">Add Sender ID</label>
                                        <div class="col-sm-5">
                                            <div class="input-group">
                                                <asp:TextBox ID="txtSender1" runat="server" autocomplete="Off" class="form-control" placeholder="" aria-label="six alphabet" aria-describedby="button-addon2"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="ftrtxtSender" runat="server" FilterMode="ValidChars"
                                                    TargetControlID="txtSender1" ValidChars=" ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890-">
                                                </asp:FilteredTextBoxExtender>
                                                <div class="input-group-append">
                                                    <asp:LinkButton ID="LinkButton1" runat="server" class="btn btn-outline-secondary" OnClick="button1_Click"><i class="fas fa-plus-circle"></i></asp:LinkButton>
                                                    <%--  <button class="btn btn-outline-secondary" runat="server" type="button" id="buttonsen" OnClick="btnLogoutt_Click"><i class="fas fa-plus-circle"></i></button>--%>
                                                </div>
                                            </div>
                                            <small id="genHelp1" class="form-text text-muted"></small>
                                        </div>
                                    </div>

                                    <div id="Sen2" runat="server" style="display: none">
                                        <div class="form-group row">
                                            <label for="inputEmail3" class="col-sm-2 col-form-label font-weight-bold">Add Sender ID</label>
                                            <div class="col-md-5">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtSender2" runat="server" autocomplete="Off" class="form-control" placeholder="" aria-label="six alphabet" aria-describedby="button-addon2"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterMode="ValidChars"
                                                        TargetControlID="txtSender2" ValidChars="ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890-">
                                                    </asp:FilteredTextBoxExtender>
                                                    <div class="input-group-append">
                                                        <asp:LinkButton ID="LinkButton2" runat="server" class="btn btn-outline-secondary" OnClick="button2_Click"><i class="fas fa-plus-circle"></i></asp:LinkButton>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>


                                    <div id="Sen3" runat="server" style="display: none">
                                        <div class="form-group row">
                                            <label for="inputEmail3" class="col-sm-2 col-form-label font-weight-bold">Add Sender ID</label>
                                            <div class="col-sm-5">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtSender3" runat="server" autocomplete="Off" class="form-control" placeholder="" aria-label="six alphabet" aria-describedby="button-addon2"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterMode="ValidChars"
                                                        TargetControlID="txtSender3" ValidChars="ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890-">
                                                    </asp:FilteredTextBoxExtender>
                                                    <div class="input-group-append">
                                                        <asp:LinkButton ID="LinkButton3" runat="server" class="btn btn-outline-secondary" OnClick="button3_Click"><i class="fas fa-plus-circle"></i></asp:LinkButton>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div id="Sen4" runat="server" style="display: none">
                                        <div class="form-group row">
                                            <label for="inputEmail3" class="col-sm-2 col-form-label font-weight-bold">Add Sender ID</label>
                                            <div class="col-sm-5">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtSender4" runat="server" autocomplete="Off" class="form-control" placeholder="" aria-label="six alphabet" aria-describedby="button-addon2"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterMode="ValidChars"
                                                        TargetControlID="txtSender4" ValidChars="ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890-">
                                                    </asp:FilteredTextBoxExtender>
                                                    <div class="input-group-append">
                                                        <asp:LinkButton ID="LinkButton4" runat="server" class="btn btn-outline-secondary" OnClick="button4_Click"><i class="fas fa-plus-circle"></i></asp:LinkButton>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>


                                    <div id="Sen5" runat="server" style="display: none">
                                        <div class="form-group row">
                                            <label for="inputEmail3" class="col-sm-2 col-form-label font-weight-bold">Add Sender ID</label>
                                            <div class="col-sm-5">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtSender5" runat="server" autocomplete="Off" class="form-control" placeholder="" aria-label="six alphabet" aria-describedby="button-addon2"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" FilterMode="ValidChars"
                                                        TargetControlID="txtSender5" ValidChars="ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890-">
                                                    </asp:FilteredTextBoxExtender>
                                                    <div class="input-group-append">
                                                        <asp:LinkButton ID="LinkButton5" runat="server" class="btn btn-outline-secondary" OnClick="button5_Click"><i class="fas fa-plus-circle"></i></asp:LinkButton>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>


                                    <div id="Sen6" runat="server" style="display: none">
                                        <div class="form-group row">
                                            <label for="inputEmail3" class="col-sm-2 col-form-label font-weight-bold">Add Sender ID</label>
                                            <div class="col-sm-5">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtSender6" runat="server" autocomplete="Off" class="form-control" placeholder="" aria-label="six alphabet" aria-describedby="button-addon2"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" FilterMode="ValidChars"
                                                        TargetControlID="txtSender6" ValidChars="ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890-">
                                                    </asp:FilteredTextBoxExtender>
                                                    <div class="input-group-append">
                                                        <asp:LinkButton ID="LinkButton6" runat="server" class="btn btn-outline-secondary" OnClick="button6_Click"><i class="fas fa-plus-circle"></i></asp:LinkButton>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>


                                    <div id="Sen7" runat="server" style="display: none">
                                        <div class="form-group row">
                                            <label for="inputEmail3" class="col-sm-2 col-form-label font-weight-bold">Add Sender ID</label>
                                            <div class="col-sm-5">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtSender7" runat="server" autocomplete="Off" class="form-control" placeholder="" aria-label="six alphabet" aria-describedby="button-addon2"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" FilterMode="ValidChars"
                                                        TargetControlID="txtSender7" ValidChars="ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890-">
                                                    </asp:FilteredTextBoxExtender>
                                                    <div class="input-group-append">
                                                        <asp:LinkButton ID="LinkButton7" runat="server" class="btn btn-outline-secondary" OnClick="button7_Click"><i class="fas fa-plus-circle"></i></asp:LinkButton>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>


                                    <div id="Sen8" runat="server" style="display: none">
                                        <div class="form-group row">
                                            <label for="inputEmail3" class="col-sm-2 col-form-label font-weight-bold">Add Sender ID</label>
                                            <div class="col-sm-5">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtSender8" runat="server" autocomplete="Off" class="form-control" placeholder="" aria-label="six alphabet" aria-describedby="button-addon2"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" FilterMode="ValidChars"
                                                        TargetControlID="txtSender8" ValidChars="ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890-">
                                                    </asp:FilteredTextBoxExtender>
                                                    <div class="input-group-append">
                                                        <asp:LinkButton ID="LinkButton8" runat="server" class="btn btn-outline-secondary" OnClick="button8_Click"><i class="fas fa-plus-circle"></i></asp:LinkButton>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>


                                    <div id="Sen9" runat="server" style="display: none">
                                        <div class="form-group row">
                                            <label for="inputEmail3" class="col-sm-2 col-form-label font-weight-bold">Add Sender ID</label>
                                            <div class="col-sm-5">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtSender9" runat="server" autocomplete="Off" class="form-control" placeholder="" aria-label="six alphabet" aria-describedby="button-addon2"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" FilterMode="ValidChars"
                                                        TargetControlID="txtSender9" ValidChars="ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890-">
                                                    </asp:FilteredTextBoxExtender>
                                                    <div class="input-group-append">
                                                        <asp:LinkButton ID="LinkButton9" runat="server" class="btn btn-outline-secondary" OnClick="button9_Click"><i class="fas fa-plus-circle"></i></asp:LinkButton>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>


                                    <div id="Sen10" runat="server" style="display: none">
                                        <div class="form-group row">
                                            <label for="inputEmail3" class="col-sm-2 col-form-label font-weight-bold">Add Sender ID</label>
                                            <div class="col-sm-5">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtSender10" runat="server" autocomplete="Off" class="form-control" placeholder="" aria-label="six alphabet" aria-describedby="button-addon2"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" FilterMode="ValidChars"
                                                        TargetControlID="txtSender10" ValidChars="ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890-">
                                                    </asp:FilteredTextBoxExtender>
                                                    <div class="input-group-append">
                                                        <asp:LinkButton ID="LinkButton10" runat="server" class="btn btn-outline-secondary" OnClick="button10_Click"><i class="fas fa-plus-circle"></i></asp:LinkButton>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>


                                    <div id="Sen11" runat="server" style="display: none">
                                        <div class="form-group row">
                                            <label for="inputEmail3" class="col-sm-2 col-form-label font-weight-bold">Add Sender ID</label>
                                            <div class="col-sm-5">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtSender11" runat="server" autocomplete="Off" class="form-control" placeholder="" aria-label="six alphabet" aria-describedby="button-addon2"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" FilterMode="ValidChars"
                                                        TargetControlID="txtSender11" ValidChars="ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890-">
                                                    </asp:FilteredTextBoxExtender>
                                                    <div class="input-group-append">
                                                        <asp:LinkButton ID="LinkButton11" runat="server" class="btn btn-outline-secondary" OnClick="button11_Click"><i class="fas fa-plus-circle"></i></asp:LinkButton>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div id="Sen12" runat="server" style="display: none">
                                        <div class="form-group row">
                                            <label for="inputEmail3" class="col-sm-2 col-form-label font-weight-bold">Add Sender ID</label>
                                            <div class="col-sm-5">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtSender12" runat="server" autocomplete="Off" class="form-control" placeholder="" aria-label="six alphabet" aria-describedby="button-addon2"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" FilterMode="ValidChars"
                                                        TargetControlID="txtSender12" ValidChars="ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890-">
                                                    </asp:FilteredTextBoxExtender>
                                                    <div class="input-group-append">
                                                        <asp:LinkButton ID="LinkButton12" runat="server" class="btn btn-outline-secondary" OnClick="button12_Click"><i class="fas fa-plus-circle"></i></asp:LinkButton>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>


                                    <div id="Sen13" runat="server" style="display: none">
                                        <div class="form-group row">
                                            <label for="inputEmail3" class="col-sm-2 col-form-label font-weight-bold">Add Sender ID</label>
                                            <div class="col-sm-5">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtSender13" runat="server" autocomplete="Off" class="form-control" placeholder="" aria-label="six alphabet" aria-describedby="button-addon2"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" FilterMode="ValidChars"
                                                        TargetControlID="txtSender13" ValidChars="ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890-">
                                                    </asp:FilteredTextBoxExtender>
                                                    <div class="input-group-append">
                                                        <asp:LinkButton ID="LinkButton13" runat="server" class="btn btn-outline-secondary" OnClick="button13_Click"><i class="fas fa-plus-circle"></i></asp:LinkButton>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>


                                    <div id="Sen14" runat="server" style="display: none">
                                        <div class="form-group row">
                                            <label for="inputEmail3" class="col-sm-2 col-form-label font-weight-bold">Add Sender ID</label>
                                            <div class="col-sm-5">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtSender14" runat="server" autocomplete="Off" class="form-control" placeholder="" aria-label="six alphabet" aria-describedby="button-addon2"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" runat="server" FilterMode="ValidChars"
                                                        TargetControlID="txtSender14" ValidChars="ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890-">
                                                    </asp:FilteredTextBoxExtender>
                                                    <div class="input-group-append">
                                                        <asp:LinkButton ID="LinkButton14" runat="server" class="btn btn-outline-secondary" OnClick="button14_Click"><i class="fas fa-plus-circle"></i></asp:LinkButton>

                                                    </div>
                                                </div>

                                            </div>
                                        </div>
                                    </div>

                                    <div id="Sen15" runat="server" style="display: none">
                                        <div class="form-group row">
                                            <label for="inputEmail3" class="col-sm-2 col-form-label font-weight-bold">Add Sender ID</label>
                                            <div class="col-sm-5">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtSender15" runat="server" autocomplete="Off" class="form-control" placeholder="" aria-label="six alphabet" aria-describedby="button-addon2"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" runat="server" FilterMode="ValidChars"
                                                        TargetControlID="txtSender15" ValidChars="ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890-">
                                                    </asp:FilteredTextBoxExtender>
                                                    <div class="input-group-append">
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div id="divTempsms" runat="server">
                                        <div class="form-group row">
                                            <label for="inputEmail3" class="col-sm-2 col-form-label font-weight-bold">DLT Registration Number</label>
                                            <div class="col-sm-5" id="divdlt">
                                                <asp:Label ID="txtDLT" runat="server"></asp:Label>

                                            </div>
                                        </div>
                                        <div class="form-group row">
                                            <label for="inputEmail3" class="col-sm-2 col-form-label font-weight-bold">DLT Screenshot</label>
                                            <div class="col-sm-8">
                                                <div class="custom-file">
                                                    <asp:FileUpload ID="FileUpload1" runat="server" />
                                                    <!-- <label class="custom-file-label" for="customFile">Choose file</label> -->
                                                    <asp:Button ID="btnUpload" runat="server" Text="Upload" class="btn btn-medium btn-theme btn-rounded" Style="margin-top: 5px;" OnClick="btnUpload_Click" />
                                                    <asp:Label ID="lblfn" runat="server"></asp:Label>
                                                </div>
                                                <small>(Upload Only image/pdf/winrar format)</small>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group row justify-content-end">
                                        <div class="col-sm-10 ">
                                            <div class="row">
                                                <div class="col-6 col-md-2">
                                                    <asp:LinkButton ID="btnSubmit" runat="server" class="btn btn-primary text-success font-weight-bold btn-block" OnClick="btnSubmit_Click"><i class="fas fa-paper-plane"></i> Submit</asp:LinkButton>
                                                </div>
                                                <div class="col-6 col-md-2">
                                                    <asp:LinkButton ID="btnCancel" runat="server" class="btn btn-primary text-danger font-weight-bold btn-block" OnClick="btnCancel_Click"><i class="fas fa-times"></i> Cancel</asp:LinkButton>
                                                </div>
                                                <div class="col-12 col-md-3">
                                                    <asp:Button ID="btnConfirm" runat="server" class="btn btn-primary font-weight-bold btn-block mt-3 mt-lg-0" Text="SenderId List"
                                                        PostBackUrl="~/SenderListU.aspx" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </main>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script src="vendor/jquery/jquery-3.5.1.min.js"></script>

    <script type="text/javascript"> 
        //$("#divdlt *").attr("disabled", "disabled").off('click');

        //$(document).ready(function () {

        //});


    </script>

</asp:Content>
