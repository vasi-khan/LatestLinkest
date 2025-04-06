<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="SendWABAOnLinkClick.aspx.cs" Inherits="eMIMPanel.SendWABAOnLinkClick" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="scrmg" runat="server" EnablePageMethods="true">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="updFormArea" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <main>
                <div class="container-fluid">
                    <!-- Content Row -->
                    <div class="row">
                        <div class="col-lg-10 col-xl-11">
                            <!-- Start Card -->
                            <div class="card bg-primary border-light shadow-soft mb-4">
                                <div class="card-header py-3 bg-primary d-flex justify-content-between flex-wrap align-content-center">
                                    <h6 class="m-0 font-weight-bold font-weight-bold my-auto"><i class="fas fa-link"></i>&nbsp;&nbsp;Send WABA On Link CLick</h6>
                                </div>
                                <div class="card-body pt-0">
                                    <!-- Start Card -->
                                    <div class="form-group row">
                                        <label for="inputEmail3" class="col-sm-3 col-form-label font-weight-bold">Linkext User ID :</label>
                                        <div class="col-sm-5">
                                            <asp:TextBox ID="txtLinkUserId" runat="server" class="form-control" placeholder="Enter Linkext User ID"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-2">
                                            <asp:Button ID="btnLinkID" runat="server" class="btn btn-primary text-secondary btn-block" Text="GO" OnClick="btnLinkID_Click" />
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <label for="inputEmail3" class="col-sm-3 col-form-label font-weight-bold">Short URL</label>
                                        <div class="col-sm-5">
                                            <asp:DropDownList ID="ddlShortUrl" runat="server" class="custom-select">
                                                <asp:ListItem Value="0" Selected="True">Select</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <label for="inputEmail3" class="col-sm-3 col-form-label font-weight-bold">WABA User ID :</label>
                                        <div class="col-sm-5">
                                            <asp:TextBox ID="txtWABAUserID" runat="server" class="form-control" placeholder="Enter WABA User ID"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-2">
                                            <asp:Button ID="btnWABA" runat="server" class="btn btn-primary text-secondary btn-block" OnClick="btnWABA_Click" Text="GO" />
                                        </div>
                                    </div>
                                    <div id="div4" runat="server" class="form-group row">
                                        <label for="inputEmail3" class="col-sm-3 col-form-label font-weight-bold">WABA Registered Template</label>
                                        <div class="col-sm-5">
                                            <asp:DropDownList ID="ddlTempID" runat="server" class="custom-select" AutoPostBack="true" OnSelectedIndexChanged="ddlTempID_SelectedIndexChanged1">
                                                <asp:ListItem Value="0" Selected="True">Select</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <label for="exampleFormControlTextarea2" class="col-sm-3 col-form-label font-weight-bold">Whatsapp Text: </label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtWhatsappText" runat="server" TextMode="MultiLine" class="form-control" Rows="3" disabled="disabled"></asp:TextBox>
                                        </div>
                                    </div>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <div class="form-group row justify-content-end">
                                                <div class="col-sm-10">
                                                    <div class="row">
                                                        <div class="col-lg-3">
                                                        </div>
                                                        <div class="col-lg-3">
                                                            <asp:LinkButton ID="lnkUpdateDate" runat="server" class="btn btn-primary text-secondary btn-block" OnClick="lnkUpdateDate_Click">
                                                        <span class="text-secondary"> <i class="fas fa-save"></i>Submit</span>
                                                            </asp:LinkButton>
                                                        </div>
                                                        <div class="col-lg-3">
                                                            <asp:LinkButton ID="LinkButton3" runat="server" class="btn btn-primary text-danger btn-block my-3 my-lg-0" OnClick="btnCancel_Click" data-toggle="tooltip" data-placement="top">
                                                        <span class="text-danger"> <i class="fas fa-times"></i>Reset</span>
                                                            </asp:LinkButton>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="0">
                                        <ProgressTemplate>
                                            <div align="center" style="position: absolute; left: 40%; top: 50%; vertical-align: middle;">
                                                <img src="Img/LOADING.GIF" />
                                            </div>
                                            <div align="center" style="position: absolute; left: 0%; top: 0%; vertical-align: middle;">
                                            </div>
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                </div>
                            </div>
                            <!-- End Card -->
                            <div id="collapseOne" class="collapse show" aria-labelledby="headingOne" data-parent="#accordionExample" runat="server">
                                <div class="card-body">
                                    <div class="row">
                                        <!-- Area Chart -->
                                        <div class="col-xl-12 col-lg-12">
                                            <div class="table-smsReport">

                                                <asp:GridView UseAccessibleHeader="true" ID="grv" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false" ShowFooter="true"
                                                    runat="server" Width="100%" CellPadding="10" BorderColor="#ede8e8" Class="table table-striped table-bordered dt-responsive nowrap dataTable-view">
                                                    <Columns>
                                                        <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Sr No">
                                                            <ItemTemplate>
                                                                <%#Container.DataItemIndex+1 %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Linkext User ID" HeaderStyle-CssClass="text-wrap">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblLinkextUserid" runat="server" CssClass="text-wrap" Text='<%#Eval("LinkextUserId")%>'></asp:Label>
                                                                <asp:Label ID="lblid" runat="server" CssClass="text-wrap" Visible="false" Text='<%#Eval("ID")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Short URl ID">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblShortUrlID" runat="server" Text='<%#Eval("ShortUrlID")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Short URl">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblShortUrl" runat="server" Text='<%#Eval("ShortUrl")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="WABA User ID">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblWabaUserID" runat="server" Text='<%#Eval("WABAUserID")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Templete Name">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblWabaTempID" runat="server" Text='<%#Eval("WABATemplateName")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Active">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="CheckBox1" runat="server"  Checked='<%# Eval("Active").ToString() == "True" ? true:false %>'  OnCheckedChanged="chkAction_CheckedChanged" AutoPostBack="true"/>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- End Row -->
                </div>
            </main>
        </ContentTemplate>
    </asp:UpdatePanel>
      <script>
          function Confirm() {
              alert();
              debugger;
              if (confirm(' Are you sure to Active.')) {
                  return true;
              }
              else {
                  return false;
              }
          }
      </script>
</asp:Content>
