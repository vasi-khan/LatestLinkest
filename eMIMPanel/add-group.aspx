<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="add-group.aspx.cs" Inherits="eMIMPanel.add_group" %>

<%@ MasterType VirtualPath="~/Site2.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
    <script>
 function Confirm()
 {
     if(confirm(' Are you sure to delete this group \n Data can not be restore after delete. '))
        {
            return true;
        }
        else
        {
            return false;
        }
  }

 //function ConfirmA()
 //{
 //    window.location.href = 'add-group.aspx';
 // }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="scrmg" runat="server" EnablePageMethods="true">
    </asp:ToolkitScriptManager>

    <main>
       <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>--%>

            
        <div class="container-fluid">
            <nav aria-label="breadcrumb" class="my-3">
                <ol class="breadcrumb breadcrumb-info">
                    <li class="breadcrumb-item"><a href="#">Home</a></li>
                    <li class="breadcrumb-item"><a href="#">Phone Book</a></li>
                    <li class="breadcrumb-item active" aria-current="page">Add Group</li>
                </ol>
            </nav>
            <!-- Content Row -->
            <div class="row">
                <div class="col-12">
                    <div class="card card-body mb-4 bg-primary border-light shadow-soft">
                        <form>
                            <div class="form-row">
                                <div class="form-group col-md-2">
                                    <label for="#" class="font-weight-bold">Add Group</label>
                                    <asp:TextBox ID="txtGrNm" runat="server" class="form-control" MaxLength="50" placeholder="Enter Group Name"></asp:TextBox>
                                    
                                </div>
                                <div class="form-group col-md-2 mt-auto">
                                    <asp:LinkButton ID="btnCreate" runat="server" class="btn btn-primary text-secondary btn-block" OnClick="btnCreate_Click"> 
                                                    <span > <i class="fas fa-user-plus text-secondary"></i> Create</span>
                                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnUpdate" runat="server" Visible="false" class="btn btn-primary text-secondary btn-block" OnClick="btnUpdate_Click" > 
                                                    <span > <i class="fas fa-user-edit text-secondary"></i> Update</span>
                                                    </asp:LinkButton>
                                    <%--<a class="btn btn-primary text-secondary btn-block" href="#" role="button"><i class="fas fa-user-plus text-secondary"></i>Create </a>--%>
                                </div>
                            </div>
                        </form>
                    </div>
                </div>
            </div>

            <!-- Content Row -->
            <div class="row">
                <div class="col-xl-12 col-lg-12">
                    <!-- Basic Card Example -->
                    <div class="card bg-primary border-light shadow-soft mb-4">
                        <div class="card-header py-3 bg-primary d-flex justify-content-between flex-wrap align-content-center">
                            <h6 class="font-weight-bold my-lg-auto mb-3"><i class="far fa-clock"></i> Groups</h6>
                            
                        </div>
                        <div class="card-body">
                            <div class="table-responsive">
                                <asp:GridView ID="grv" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
                                runat="server" Width="100%" CellPadding="10" BorderColor="#ede8e8" Class="table table-striped table-bordered dt-responsive nowrap dataTable-view">
                                <Columns>
                                    <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Sr No">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Group Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgrpname" runat="server" Text='<%#Eval("grpname")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Edit">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnEdit" runat="server" OnClick="btnEdit_Click" class="btn btn-primary text-secondary" data-toggle="tooltip" data-placement="top" title="Edit"><i class="fas fa-user-edit"></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Delete">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btndlt" runat="server" OnClientClick="return Confirm();" class="btn btn-primary text-danger" data-toggle="tooltip" data-placement="top" title="Delete" OnClick="btndlt_Click"><i class="fas fa-times"></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Numbers">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnDw" runat="server" OnClick="btnDw_Click"  class="btn btn-primary text-secondary" ToolTip="Download" title="Download"><i class="fas fa-download"></i></asp:LinkButton>
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
                <%--</ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="grv" />
            </Triggers>
        </asp:UpdatePanel>--%>
    </main>
</asp:Content>
