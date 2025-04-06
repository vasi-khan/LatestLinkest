<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RouteSetting.aspx.cs" Inherits="eMIMPanel.RouteSetting" %>
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

    <link href="css/OffLineCDN/bootstrap.min.css" rel="stylesheet" />

    <title>Route Setting</title>
    <style>
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
</head>
<body style="background-color: #F7F7F7">
    
    
        
    <form id="frm" runat="server">
        <cc1:ToolkitScriptManager ID="scrmg" runat="server" EnablePageMethods="true">
    </cc1:ToolkitScriptManager>
        <asp:UpdatePanel runat="server">
        <ContentTemplate>
    <div class="container shadow rounded  mb-5 mt-3">
         <a href="index2.aspx" style="text-align:right;" >Dashboard</a>
        <div class="rounded" style="background-color: #064663;" runat="server">
            <div class="  text-center  text-secondary ">
                <span>
                    <h4 style="color: white">ROUTE SETTING</h4>
                
                    </span>
            </div>

            <div class=" pb-0" style="background-color: #F7F7F7;">


                <div class="row px-4 ">
                    <div class="col-md-2 col-11 mt-3">
                        <label4>
                        Profile ID</label>
                    </div>
                    <div class="col-md-3 col-7 mt-lg-3 mt-md-3">
                        <asp:TextBox ID="txtproid" runat="server" class="form-control"></asp:TextBox>
                        <%--<input class="form-control" type="text">--%>
                    </div>

                    <div class="col-md-2 col-5 mt-lg-3 mt-md-3">
                        <%--<button class="btn btn-success " style="padding: 1px 3px; ">Submit</button>--%>
                        <asp:LinkButton ID="LinkButton1" class="btn btn-success" Style="padding: 1px 3px;" runat="server" OnClick="LinkButton1_Click"><i class="fa fa-eye"></i> Show</asp:LinkButton>&nbsp; &nbsp;
                        <asp:LinkButton ID="LinkButton2" class="btn btn-success" Style="padding: 1px 3px;" runat="server" OnClick="LinkButton2_Click"><i class="fa fa-search"></i> Search</asp:LinkButton>
                        <%--<i class=" fa fa-info" data-bs-toggle="modal" data-bs-target="#staticBackdrop"></i>--%>
                    </div>
                    <div class="col-md-2  mt-lg-3 mt-md-3">

                        <label>Name</label>
                    </div>
                    <div class="col-md-3 col-11 mt-lg-3 mt-md-3">
                        <asp:TextBox ID="txtfullname" runat="server" class="form-control"></asp:TextBox>
                    </div>

                </div>
                <div class="row px-4 mt-1">
                    <div class="col-md-2  ">
                        <label>Default Country</label>
                    </div>
                    <div class="col-md-3 col-11">
                        <asp:TextBox ID="txtdefaultcountry" runat="server" class="form-control"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-2  ">
                        <label>Sender ID</label>
                    </div>
                    <div class="col-md-3 col-11">
                        <asp:TextBox ID="txtsender" runat="server" class="form-control"></asp:TextBox>
                    </div>

                    <div class="text-secondary " style="background-color: #064663; margin-top: 1rem;">
                        <span>
                            <h6 class="text-white">CHANGE ROUTE</h6>
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
                            <%--<button class="btn btn-success mt-4" style="padding: 1px 3px;">Submit</button>--%>
                            <asp:LinkButton ID="lnkbtnCr" runat="server" class="btn btn-success mt-4" style="padding: 1px 3px;" OnClick="lnkbtnCr_Click" OnClientClick="return Confirm2()">
                                Submit
                            </asp:LinkButton>
                        </div>
                    </div>
                    <div class="row mt-2">

                        <div class="col-md-12">
                            <asp:Repeater ID="RptChangeRoute" runat="server" OnItemCommand="RptChangeRoute_ItemCommand">
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
                                                        <td><asp:Label ID="lblRowNumber" Text='<%# Container.ItemIndex + 1 %>' runat="server" />
                                                        </td>

                                                        <td>
                                                            <asp:Label ID="lblcccode" Text='<%#Eval("CountryCode")%>' runat="server" ></asp:Label> 
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblroute" Text='<%#Eval("Route")%>' runat="server" ></asp:Label>
                                                            <asp:Label ID="lblsmppaccid" Visible="false" Text='<%#Eval("smppaccountid")%>' runat="server" ></asp:Label>
                                                           </td>
                                                        <td>
                                                           <%--<td>
                                                               <asp:Label runat="server" ID="deleteid" <>></asp:Label>
                                                           </td>--%>
                                                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CommandArgument='<%# Container.ItemIndex %>' OnClientClick=" return Confirm()"><i class="fa icon-fa2" data-bs-toggle="tooltip"
                                                data-bs-placement="right" title="Remove">&#xf00d;</i></asp:LinkButton>
                                                        </td>
                                             </tr>
                                     
                                           </tbody>
                                           </ItemTemplate>
     <FooterTemplate>
         <%--<asp:Label ID="lblEmptyData" runat="server" Visible='<%# ((Repeater)Container.NamingContainer).Items.Count == 0 %>' Text="No items found" />--%>
        </table>
    </FooterTemplate>
</asp:Repeater>
                        </div>

                    </div>
                    <body style="background-color: #F7F7F7">
                   <div class="text-secondary" style="background-color:#064663; margin-top: 1rem;">
                        <span>
                            <h6 class="text-white">ACCOUNTS</h6>
                        </span>
                    </div>
                       <!-------- Show SMPAccountList Detail------->
                        <div class="row mt-2">

                        <div class="col-md-12">
                            <div class="table-responsive">
                                <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="RptSenderId_ItemCommand">
                                 <HeaderTemplate>
                                        <table class="table table-hover text-center table-sm table-bordered text-secondary">
                                          <thead class="">
                                            <tr>
                                                <th>Sr.No.</th>
                                                <th>UserID</th>
                                                <th>AccountID</th>
                                                <th>Country Code</th>
                                                
                                            </tr>
                                          </thead>
                                             </HeaderTemplate>
                                             <ItemTemplate>
                                          <tbody>
                                           <tr>
                                                        <td><asp:Label ID="lblRowNumber1" Text='<%# Container.ItemIndex + 1 %>' runat="server" />
                                                        </td>
                                                         
                                                        <td>
                                                            <asp:Label ID="Label1" Text='<%#Eval("userid")%>' runat="server" ></asp:Label>
                                                            
                                                        </td>
                                                           <td>
                                                               <asp:Label ID="Label2" Text='<%#Eval("smppaccountid")%>' runat="server"></asp:Label>
                                                           </td>
                                                        <td>
                                                            <asp:Label ID="Label3" Text='<%#Eval("countrycode")%>' runat="server" ></asp:Label>
                                                           </td>
                                                           
                                                        
                                             </tr>
                                     
                                           </tbody>
                                           </ItemTemplate>
     <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>
                                
                            </div>
                          
                        </div>
                    </div>

                        </body>
                    <!------%>--- Show SMPAccountList Detail- END------>
                     <div runat="server" id="divsenderid" visible="false">
                    <div class="row mt-1 px-2">
                        <div class="col-md-3  ">
                            <label>Country Code</label>
                            <asp:DropDownList ID="ddlSenderCC" runat="server" class="form-select">
                            </asp:DropDownList>
                        </div>
                        <!-- <div class="col-md-1">
                            </div> -->
                        <div class="col-md-3 ">
                            <label>Route</label>
                            <asp:DropDownList ID="ddlSenderRoute" runat="server" class="form-select" OnSelectedIndexChanged="ddlSenderRoute_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-3  ">
                            <label>Sender ID</label>
                            <asp:DropDownList ID="ddlSenderID" runat="server" class="form-select">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-1">
                            <asp:LinkButton ID="lnkbtnsender" runat="server" class="btn btn-success mt-4" style="padding: 1px 3px;" OnClick="lnkbtnsender_Click">
                                Submit
                            </asp:LinkButton>
                            <%--<button class="btn  btn-success mt-4" style="padding: 1px 3px;">Sumbit</button>--%>
                        </div>
                    </div>
                    <div class="row mt-2">
                       
                        <div class="col-md-12">
                            <div class="table-responsive">
                                <asp:Repeater ID="RptSenderId" runat="server" OnItemCommand="RptSenderId_ItemCommand">
                                 <HeaderTemplate>
                                        <table class="table table-hover text-center table-sm table-bordered text-secondary">
                                          <thead class="">
                                            <tr>
                                                <th>Sr.No.</th>
                                                <th>Country Code</th>
                                                <th>Route</th>
                                                <th>Sender ID</th>
                                                <th>Delete</th>
                                            </tr>
                                          </thead>
                                             </HeaderTemplate>
                                             <ItemTemplate>
                                          <tbody>
                                           <tr>
                                                        <td><asp:Label ID="lblRowNumber1" Text='<%# Container.ItemIndex + 1 %>' runat="server" />
                                                        </td>
                                                         
                                                        <td>
                                                            <asp:Label ID="lblsendercccode" Text='<%#Eval("CountryCode")%>' runat="server" ></asp:Label>
                                                            <asp:Label ID="Label1" Text='<%#Eval("smppaccountid")%>' runat="server" Visible="false"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblsenderroute" Text='<%#Eval("Route")%>' runat="server" ></asp:Label>
                                                           </td>
                                                           <td>
                                                            <asp:Label ID="lblsenderid" Text='<%#Eval("Senderid")%>' runat="server" ></asp:Label>
                                                           </td>
                                                        <td>
                                                            <asp:LinkButton ID="lnkbtndlt" runat="server" CommandName="Delete" CommandArgument='<%# Container.ItemIndex %>'><i class="fa icon-fa2" data-bs-toggle="tooltip"
                                                data-bs-placement="right" title="Remove">&#xf00d;</i></asp:LinkButton>
                                                        </td>
                                             </tr>
                                     
                                           </tbody>
                                           </ItemTemplate>
     <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>
                                <%--<table class="table table-hover text-center table-sm  table-bordered text-secondary">
                                    <thead>
                                        <tr>
                                            <th>Country Code</th>
                                            <th>Route</th>
                                            <th>Sender ID</th>
                                            <th>Delete</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                            <td><i class="fa  icon-fa2" data-bs-toggle="tooltip"
                                                data-bs-placement="right" title="Remove">&#xf00d;</i></td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                            <td><i class="fa  icon-fa2" data-bs-toggle="tooltip"
                                                data-bs-placement="right" title="Remove">&#xf00d;</i></td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                            <td><i class="fa  icon-fa2" data-bs-toggle="tooltip"
                                                data-bs-placement="right" title="Remove">&#xf00d;</i></td>
                                        </tr>
                                    </tbody>
                                </table>--%>
                            </div>
                          
                        </div>
                    </div>
                        </div>
                    <div runat="server" visible="false">
                      <div class="row mb-3">
                                <div class="col text-center">
                                    <asp:LinkButton ID="lnkbtnsave" runat="server" class="btn btn-success" style="padding: 1px 3px;" OnClick="lnkbtnsave_Click" >
                                <i class="fa fa-save"></i> Save
                            </asp:LinkButton>
                                    <asp:LinkButton ID="lnkbtnreset" runat="server" class="btn btn-danger" style="padding: 1px 3px;" OnClick="lnkbtnreset_Click">
                                <i class="fa fa-trash"></i> Reset
                            </asp:LinkButton>
                                    <%--<button class="btn  btn-success" style="padding: 1px 3px;">Show</button>
                                    <button class="btn btn-danger" style="padding: 1px 3px;">Reset</button>--%>
                                </div>
                            </div>
                        </div>
                </div>

            </div>
        
    </div>

    </div>

            <asp:LinkButton ID="lnkDetail" runat="server"></asp:LinkButton>
    <asp:Panel ID="pnlPopUp_Detail" runat="server" CssClass="modal modalPopup" Style="display: none;">

       
        <div class="modal-dialog modal-xl">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title text-secondary" id="staticBackdropLabel"></h5>
                    <button type="button" id="btnCancel" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div class="modal-body">
                        <div class="row text-start">
                            <div class="col-md-3 ">
                                <label>Profile ID</label>
                                <asp:TextBox ID="txtprofile" runat="server" class="form-control" MaxLength="15"
                                    onkeypress="return onlyTextAndNumber(event)"></asp:TextBox>
                            </div>
                            <div class="col-md-3 mb-1">
                                <label>
                                    Name</label>
                                <asp:TextBox ID="txtname" runat="server" class="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-3 mb-1">
                                <label>
                                    Company Name</label>
                                <asp:TextBox ID="txtcompname" runat="server" class="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-3 mb-1 ">
                                <asp:LinkButton ID="lnkbtnshow" class="btn btn-success mt-4" Style="padding: 1px 5px;" runat="server" OnClick="lnkbtnshow_Click"><i class="fa fa-search"></i> Show</asp:LinkButton>
                            </div>
                        </div>
                    <div class=" table-responsive-sm mt-3 ">
                       <asp:Repeater ID="rpItems" runat="server" >
                                 <HeaderTemplate>
                                        <table class="table table-hover text-center table-sm table-bordered text-secondary">
                                          <thead class="">
                                            <tr>
                                                <th>Sr.No.</th>
                                              <th>Profile ID</th>
                                              <th>Full Name</th>
                                              <th>Company Name</th>
                                              <th>Select</th>
                                            </tr>
                                          </thead>
                                             </HeaderTemplate>
                                             <ItemTemplate>
                                          <tbody>
                                           <tr>
                                                        <td><asp:Label ID="lblRowNumber" Text='<%# Container.ItemIndex + 1 %>' runat="server" />
                                                           
                                                        </td>

                                                        <td>
                                                            <asp:Label ID="lblpid" Text='<%#Eval("username")%>' runat="server" ></asp:Label>
                                                            <asp:Label ID="lblprofileid1" Text='<%#Eval("username")%>' runat="server" Visible="false"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblItemName" Text='<%#Eval("FULLNAME")%>' runat="server" ></asp:Label>
                                                           </td>
                                                        <td>
                                                            <asp:Label ID="lblQntyRqrd" Text='<%#Eval("COMPNAME")%>' runat="server" ></asp:Label>
                                                           </td>
                                                        <td>
                                                            <asp:LinkButton ID="lnkselect" class="btn  btn-success" runat="server" OnClick="lnkselect_Click">Select</asp:LinkButton>
                                                        </td>
                                             </tr>
                                     
                                           </tbody>
                                           </ItemTemplate>
     <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>
                    </div>
                </div>
                <%--<div class="modal-footer">
                    <button id="" runat="server" class="btn btn-primary">Close</button>
                </div>--%>
            </div>
        </div>
            </div>
           
    </asp:Panel>

    <cc1:ModalPopupExtender ID="pnlPopUp_Detail_ModalPopupExtender" runat="server" PopupControlID="pnlPopUp_Detail"
        TargetControlID="lnkDetail" BehaviorID="mpeAddUpdateEmployee" CancelControlID="btnCancel"
        BackgroundCssClass="modalBackground">
    </cc1:ModalPopupExtender>
               </ContentTemplate>
    </asp:UpdatePanel>
    <script>

        //Conformation Message
         function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Do you want to Delete data?")) {
                confirm_value.value = "Yes";
                return true;
            } else {
                confirm_value.value = "No";
                return false;
            }
            document.forms[0].appendChild(confirm_value);
        }

          function Confirm2() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Do You Want Submit a Record?")) {
                confirm_value.value = "Yes";
                return true;
            } else {
                confirm_value.value = "No";
                return false;
            }
            document.forms[0].appendChild(confirm_value);
        }
        // TextArea====================================
        var myText = document.getElementById("exampleFormControlTextarea1");
        var result = document.getElementById("result");
        var limit = 200;
        result.textContent = 0 + "/" + limit;

        myText.addEventListener("input", function () {

            var textLength = myText.value.length;

            result.textContent = textLength + "/" + limit;

        });

        // function for only type number 
        function onlyNumberKey(event) {
            // Only ASCII character in that range allowed
            var ASCIICode = (event.which) ? event.which : event.keyCode
            if (ASCIICode > 31 && (ASCIICode < 48 || ASCIICode > 57))
                return false;
            return true;
        }
        // function for type number and text only
        function onlyTextAndNumber(event) {
            var regex = new RegExp("^[a-zA-Z0-9]+$");
            var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
            if (!regex.test(key)) {
                event.preventDefault();
                return false;
            }
        }
        // function for type number and (.) only
        function isNumberKey(event) {
            var charCode = (event.which) ? event.which : event.keyCode;
            if (charCode != 46 && charCode > 31
                && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
    </script>





    <!-- Optional JavaScript; choose one of the two! -->

    <!-- Option 1: Bootstrap Bundle with Popper -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.bundle.min.js"
        integrity="sha384-ka7Sk0Gln4gmtz2MlQnikT1wXgYsOg+OMhuP+IlRH9sENBO0LRn5q+8nbTov4+1p"
        crossorigin="anonymous"></script>

    <!-- Option 2: Separate Popper and Bootstrap JS -->
    <!--
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.10.2/dist/umd/popper.min.js" integrity="sha384-7+zCNj/IqJ95wo16oMtfsKbZ9ccEh31eOz1HGyDuCQ6wgnyJNSYdrPa03rtR1zdB" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.min.js" integrity="sha384-QJHtvGhmr9XOIpI6YVutG+2QOK9T+ZnN4kzFN1RtK3zEFEIsxhlmWl5/YESvpZ13" crossorigin="anonymous"></script>
    -->
    </form>
        
</body>

</html>
