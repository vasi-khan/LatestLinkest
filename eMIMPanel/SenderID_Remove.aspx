<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="SenderID_Remove.aspx.cs" Inherits="eMIMPanel.SenderID_Remove" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <main>
                <div class="container-fluid">
                    <nav aria-label="breadcrumb" class="my-3">
                <ol class="breadcrumb breadcrumb-info">
                    <li class="breadcrumb-item"><a href="#">Home</a></li>
                    <li class="breadcrumb-item active" aria-current="page">SenderID Remove</li>
                </ol>
            </nav>
                     <div class="row">
                        <div class="col-12" style="height:200px">
                            <div class="card mb-4 bg-primary border-light shadow-soft">
                                <div class="row">
                                    <div class="col-sm-4">
                                <asp:Label runat="server" CssClass="col-form-label">UserID</asp:Label>
                                <asp:TextBox runat="server" ID="txtSenderID" class="form-control"></asp:TextBox>
                                        </div>
                                    <div class="col-sm-2">
                                        <asp:Button runat="server"  Text="Show" ID="btnShow"  OnClick="btnShow_Click" Class="btn btn-primary text-danger ml-auto" Visible="false"/>
                                    </div>
                                    </div>

                                <div class="row">
                                    
                                   <div class="card-body">
                                        <asp:GridView runat="server" Width="100%" CellPadding="10" ID="grd" PageSize="10" AutoGenerateColumns="false" Class="table table-striped table-bordered dt-responsive nowrap dataTable-view" AllowPaging="True" OnPageIndexChanging="grd_PageIndexChanging">
                                            <Columns>
                                                <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Sr No">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                <asp:TemplateField HeaderText="SenderID" HeaderStyle-Width="30%">
                                                    <ItemTemplate >
                                                        <asp:label runat="server" Text='<%#Eval("senderid") %>' style="width:100px"></asp:label>
                                                        <asp:HiddenField runat="server" Value='<%#Eval("id") %>' ID="HD_ID" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Remove" HeaderStyle-Width="10%">
                                                    <ItemTemplate>
                                                        <asp:CheckBox runat="server" ID="chk"  />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                     
                              
                               </div>

                             
                                <div class="row">
                                    <div class="col-sm-4"></div>
                                    <div class="col-sm-4">
                                        <asp:Button runat="server" Text="Show" Class="btn btn-primary text-danger ml-auto" ID="finSub" OnClick="finSub_Click" />
                                    </div>
                                    <div class="col-sm-4"></div>
                                    
                                </div>
                                   
                            </div>



                     </div>
                    </div>
        </main>
</asp:Content>
