<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="ActivateAccount.aspx.cs" Inherits="eMIMPanel.ActivateAccount" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
     <style>
        .modalPopup1 {

             top:48px;
            min-height: 40px;
            position: fixed;
            z-index: 2000;
            padding: 0;
         
            border-radius: 6px;
            background-clip: padding-box;
            border: 1px solid rgba(0, 0, 0, 0.2);
            min-width: 700px;
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
        .modal-footer {

    border-top: 0px solid #D1D9E6!important;

}
        .btnCancelSch {

        }
 
        
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <main>
         <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
        <div class="container ">
             <nav aria-label="breadcrumb" class="my-3">
                <ol class="breadcrumb breadcrumb-info">
                    <li class="breadcrumb-item"><a href="#">Home</a></li>
                    <li class="breadcrumb-item"><a href="#">Pending to Active Accounts</a></li>
                </ol>
            </nav>
            <div class="row px-3">
               
                
           
                
           <div class="card card-body mb-4 bg-primary border-light shadow-soft ">
               
                        <div class="table-responsive">
                            <asp:GridView ID="grv2" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false" 
                                runat="server" Width="100%" CellPadding="10" BorderColor="#ede8e8" Class="table table-striped table-bordered text-center dt-responsive nowrap dataTable-view">
                                <Columns>
                                    <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Sr No">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                  
                                    <asp:TemplateField HeaderText="BD NAME">
                                        <ItemTemplate>
                                            <%#Eval("BDNAME") %>
                                           <%-- <asp:Label ID="lblsubmitdt" runat="server" Text='<%#Eval("BDNAME") %>'></asp:Label>--%>
                                           <asp:HiddenField ID="hdnid" runat="server" Value='<%#Eval("id")%>'></asp:HiddenField>
                                            
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ACCOUNT NAME">
                                        <ItemTemplate>
                                            <%#Eval("Account Name") %>
                                            <%--<asp:Label ID="lblsender" runat="server" Text='<%#Eval("Account Name") %>'></asp:Label>--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="COMPNAME">
                                        <ItemTemplate>
                                            <%#Eval("COMPANE") %>
                                            <%--<asp:Label ID="lbl3" runat="server" Text='<%#Eval("COMPANE") %>'></asp:Label>--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="REQUEST DATE">
                                        <ItemTemplate>
                                            <%#Eval("Regestion Date") %>
                                            <%--<asp:Label ID="lbl4" runat="server" Text='<%#Eval("Regestion Date") %>'></asp:Label>--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                      <asp:TemplateField HeaderText="REJECT" >
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnReject" runat="server"  class="btn btn-datatable btn-icon btn-transparent-dark px-2 py-0 " data-toggle="tooltip" data-placement="top" OnClick="btnReject_Click"><i class="fas fa-thumbs-down"></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField HeaderText="ACCEPT">
                                        <ItemTemplate>
                                            
                                            <asp:LinkButton ID="btnAccept" runat="server"  class="btn btn-datatable btn-icon btn-transparent-dark px-2 py-0" data-toggle="tooltip" data-placement="top" onCLick="btnAccept_Click" OnClientClick =" return confirm_meth()"><i class="fas fa-check"></i></asp:LinkButton>
                                            <%--<asp:LinkButton ID="btnAccept" runat="server"  class="btn btn-datatable btn-icon btn-transparent-dark px-2 py-0" data-toggle="tooltip" data-placement="top" onCLick="btnAccept_Click"><i class="far fa-file-excel"></i></asp:LinkButton>--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                   
                                    
                                </Columns>                                
                            </asp:GridView>
                        </div>
                     </div>
           <asp:Panel ID="pnlPopUp_SCHEDULE" runat="server" CssClass="modalPopup1 bg-primary border-light" Style="display: none; position: fixed;z-index: 10001;left: 324.5px;top: 48px; ">
        <div style="overflow-y: auto; overflow-x: hidden; max-height: 30%;">
            <div class="modal-header">
                <asp:Label ID="lblHeader" runat="server" CssClass="modal-title" >
                  Reject Reason
                </asp:Label>
            </div>
            <div class="modal-body p-2" > 
                <div class="row p-3 ">
                    <div class="col-12">
<asp:TextBox runat="server" ID="txtRejReason" Height="100" Width="100%" TextMode="MultiLine" CssClass="bg-primary form-control " style="word-break: break-word;" ></asp:TextBox>
                 <asp:Label runat="server" ID="lblid" Visible="false"></asp:Label>
                    </div>
                
                </div>
               
                  
                
                <div align="center" class="modal-footer py-2">
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-primary" onClick="btnSubmit_Click"/>
                            <asp:Button ID="btnCancelSch" runat="server" Text="Exit" class="btn btn-primary text-danger" onCLick="btnCancelSch_Click"  />
                </div>
            </div>
      
    </asp:Panel>
    <asp:LinkButton ID="lnkSCH" runat="server"></asp:LinkButton>
    <asp:ModalPopupExtender ID="pnlPopUp_SCHEDULE_ModalPopupExtender" runat="server" BackgroundCssClass="modalBackground" PopupControlID="pnlPopUp_SCHEDULE" TargetControlID="lnkSCH">
    </asp:ModalPopupExtender>

                </div>
                
      


        </div>
     
    </main>
    <script type="text/javascript">

        function confirm_meth() {
            if (confirm("Do you want to ACCEPT !Click 'YES'") == true) {
                return true;
            }
            else {
                return false;
            }
        }
    </script>
</asp:Content>

