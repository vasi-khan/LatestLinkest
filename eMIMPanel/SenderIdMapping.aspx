<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SenderIdMapping.aspx.cs" Inherits="eMIMPanel.SenderIdMapping" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <!-- Required meta tags -->
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1">

    <!-- Bootstrap CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css" rel="stylesheet"
        integrity="sha384-1BmE4kWBq78iYhFldvKuhfTAU6auU8tT94WrHftjDbrCEXSU1oBoqyl2QvZ6jIW3" crossorigin="anonymous">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
    <link rel="stylesheet" href="css/style.css">


    <title>SENDER ID MAPPING</title>
    <%--<style>
        .modal.modalPopup {
            top: 0 !important;
            left: 0 !important;
            display: block;
        }

        .modalBackground {
            background-color: #000;
            opacity: 0.5;
        }
    </style>--%>
</head>
<body style="background-color: #F7F7F7">
    <form id="frm" runat="server">
        <cc1:ToolkitScriptManager ID="scrmg" runat="server" EnablePageMethods="true">
        </cc1:ToolkitScriptManager>
        <div class="container shadow rounded  mb-5 mt-3">
            <a href="index2.aspx" style="text-align:right;" >Dashboard</a>
            <div class="rounded" style="background-color: #064663;" runat="server">
                <div class="  text-center  text-secondary ">
                    <span>
                        <h4 style="color: white">SENDER ID MAPPING</h4>
                    </span>
                </div>
                <div class=" pb-0" style="background-color: #F7F7F7;">
                    <div class="row px-4 mt-1">

                        <div class="text-secondary " style="background-color: #064663; margin-top: 1rem;">
                            <span>
                                <h6 class="text-white">SENDER ID MAPPING</h6>
                            </span>
                        </div>
                        <div class="row mt-1 px-2">
                            <div class="col-md-3  ">
                                <label>Country Code</label>
                                <asp:DropDownList ID="ddlChangeCountry" runat="server" class="form-select">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-3 ">
                                <label>Route</label>
                                <asp:DropDownList ID="ddlChangeRoute" runat="server" class="form-select">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-1">
                                <asp:LinkButton ID="lnkbtnCr" runat="server" class="btn btn-success mt-4" Style="padding: 1px 3px;" OnClick="lnkbtnCr_Click">
                                Show
                                </asp:LinkButton>
                            </div>
                        </div>
                        <div class="row mt-1 px-2">
                            <div class="col-md-3 ">
                                <label>Sender ID</label>
                                <asp:TextBox ID="txtsenderid" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-1">
                                <asp:LinkButton ID="btnaddsenderid" runat="server" class="btn btn-success mt-4" Style="padding: 1px 3px;" OnClick="btnaddsenderid_Click">
                                Add
                                </asp:LinkButton>
                            </div>
                        </div>
                        <div class="row mt-2">
                            <div class="col-md-12">
                                <%--<asp:Repeater ID="RptChangeRoute" runat="server">
                                    <HeaderTemplate>
                                        <table class="table table-hover text-center table-sm table-bordered text-secondary">
                                            <thead class="">
                                                <tr>
                                                    <th>Sr.No.</th>
                                                    <th>Country Code</th>
                                                    <th>Route</th>
                                                    <th>Delete</th>
                                                </tr>
                                            </thead>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tbody>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblRowNumber" Text='<%# Container.ItemIndex + 1 %>' runat="server" />
                                                </td>

                                                <td>
                                                    <asp:Label ID="lblcccode" Text='<%#Eval("CountryCode")%>' runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblroute" Text='<%#Eval("Route")%>' runat="server"></asp:Label>
                                                    <asp:Label ID="lblsmppaccid" Visible="false" Text='<%#Eval("smppaccountid")%>' runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CommandArgument='<%# Container.ItemIndex %>'><i class="fa icon-fa2" data-bs-toggle="tooltip"
                                                data-bs-placement="right" title="Remove">&#xf00d;</i></asp:LinkButton>
                                                </td>
                                            </tr>

                                        </tbody>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblEmptyData" runat="server" Visible='<%# ((Repeater)Container.NamingContainer).Items.Count == 0 %>' Text="No items found" />
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>--%>

                                <div class="table-responsive">
                                    <asp:GridView ID="grv" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
                                        runat="server" Width="100%" CellPadding="10" BorderColor="#ede8e8" Class="table table-hover text-center table-sm table-striped text-secondary table-bordered dt-responsive nowrap dataTable-view">
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Sr.No.">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Country Code">
                                                <ItemTemplate>
                                                    <asp:Label ID="ddlcountry" runat="server" Text='<%#Eval("CountryCode")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Route">
                                                <ItemTemplate>
                                                    <asp:Label ID="ddlRoute" runat="server" Text='<%#Eval("Route")%>'></asp:Label>
                                                    <asp:Label ID="lblsmppaccid" runat="server" Text='<%#Eval("smppaccountid")%>' Visible="false"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Sender ID">
                                                <ItemTemplate>
                                                    <asp:Label ID="ddlSenderId" runat="server" Text='<%#Eval("SenderId")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Delete">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkbtndlt" runat="server" OnClick="lnkbtndlt_Click"><i class="fa icon-fa2" data-bs-toggle="tooltip"
                                                    data-bs-placement="right" title="Remove">&#xf00d;</i></asp:LinkButton>
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
    </form>
</body>
</html>
