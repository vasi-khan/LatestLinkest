<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="CustomMaster.aspx.cs" Inherits="eMIMPanel.CustomMaster" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">



        <%--function shownoofField() {

            var nooffields = document.getElementById('<%=txtnooffield.ClientID%>').value;

            if (nooffields>=='1')
            {
                alert('Enter Maximum NoOfField is 6 ');
                return false;
                                          

            }
            if (nooffields < 1)
            {
                 alert('Enter Mimmum NoOfField is 1 ');
                return false;
            }


        }--%>
    </script>
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updFormArea" runat="server"> 
       <ContentTemplate>
    
    <main>
        <div class="row">
            <div class="col-sm-12 col-lg-12">
                <div class="card nav-pills2">
                    <button type="button" class="btn btn-warning  w-100" > <span> Custom Master</span>

                    </button>

                    <div class="card-body pb-5 mb-3"> <!---cardstart---->
                        <div class="row">
                        <div class="row">
                            <div class="col-sm-2">
                                <label class="form-label"> Page Name </label>
                            </div>
                            <div class="col-sm-2">
                                <asp:TextBox runat="server" ID="txtpagename" class="form-control"></asp:TextBox>
                            </div>
                            <div class="col-sm-2">
                                <label  class="form-label"> Table Name</label>
                            </div>
                            <div class="col-sm-2">
                                <asp:TextBox runat="server" ID="txttablename" class="form-control" ></asp:TextBox>
                            </div>
                            <div class="col-sm-2">
                                <label class="form-label"> No Of Field</label>
                            </div>
                            <div class="col-sm-2">
                                <asp:TextBox runat="server" CssClass="form-control" ID="txtnooffield" MaxLength="1"   OnTextChanged ="txtnooffield_TextChanged" AutoPostBack="true"></asp:TextBox>
                            </div>
                        </div>
                          <!---------first-field Start------>
                        
                        <div runat="server" ID="div1" visible="false">

                        <div class="row pt-1">
                            <label class="form-label"> <b>1st Field </b></label>
                            <div class="row">
                                <div class="col-sm-2">
                                    <lable class="form-label"> Field SeqNo</lable>
                                </div>
                                <div class="col-sm-2">
                                    <asp:TextBox runat="server" ID="txtf1seqno" CssClass="form-control" MaxLength="2"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">

                           
                            <div class="col-sm-2">
                                <label class="form-label" >Field Name</label>
                            </div>
                            <div class="col-sm-2">
                                <asp:TextBox runat="server" ID="txtfieldnamef1" class="form-control"></asp:TextBox>
                            </div>
                            
                            <div class="col-sm-2">
                                <label class="form-label">Lable Caption</label>
                            </div>

                            <div class="col-sm-2">
                                <asp:TextBox runat="server" ID="txtlablecaptionf1" class="form-control"></asp:TextBox>
                            </div>
                           <div class="col-sm-2">
                               <label class="form-label">Control Type</label>
                           </div>
                            <div class="col-sm-2">
                                <asp:DropDownList runat="server" ID="ddlcontroltypef1" CssClass="form-control" OnSelectedIndexChanged="ddlcontroltype1_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="TextBox"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="Dropdown"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="CheckBox"></asp:ListItem>
                                    <asp:ListItem Value="4" Text="RadioButton"></asp:ListItem>
                                    <asp:ListItem Value="5" Text="DatePicker"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                                 </div>
                            <div class="row pt-1 border rounded shadow mt-3" id="ddlshow1" Visible="false" runat="server">
                                <div class="col-sm-6 ">
                                    <div class="row py-3 px-2">
                                        <div class="col-sm-12 px-3 text-start "></div>
                                        <asp:RadioButton runat="server"  id="rbl1a"  GroupName="A" />
                                        <div class="row">
                                        <div class="col-sm-4 mt-2">
                                            <label> DropDown From Table </label>
                                        </div>
                                        <div class="col-sm-8 mt-2">
                                            <asp:TextBox runat="server" ID="txtddltablef1" class="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-4 mt-2">
                                            <label class="form-label"> DropDown Value Field</label>
                                        </div>
                                        <div class="col-sm-8 mt-2">
                                            <asp:TextBox runat="server" ID="txtddlvaluefieldf1" class="form-control"></asp:TextBox>
                                         </div>
                                        <div class="col-sm-4 mt-2">
                                            <label class="form-label"> DropDown Text Field</label>
                                        </div>
                                        <div class="col-sm-8 mt-2">
                                            <asp:TextBox runat="server" ID="txtddltextvaluef1" class="form-control"></asp:TextBox>
                                         </div>
                                    </div>

                                        <div class="row">
                                            <div class="col-sm-4 mt-2">
                                                <asp:TextBox runat="server" ReadOnly="true" CssClass="form-control" Text="Condition"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-8 mt-2">
                                                <asp:TextBox runat="server" CssClass="form-control" ID="conditionf1_ddla"></asp:TextBox>
                                            </div>


                                        </div>

                                        <div class ="row mt-1">
                                            <div class ="col-sm-8">
                                                <label class="form-label"> Condition Variable Name</label>
                                            </div>
                                            <div class="col-sm-4">
                                                <asp:TextBox runat="server" CssClass="form-control" ID="conditonf1_varName_rbla" ></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="row mt-1">
                                            
                                              <div class ="col-sm-8">
                                                <label class="form-label"> Condition Variable From Field No</label>
                                            </div>
                                            <div class="col-sm-4">
                                                <asp:TextBox runat="server" CssClass="form-control" ID="conditionf1_fieldname_rbla" ></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-12" style="align-content:center">
                                                <asp:Button runat="server" ID="btnf1_rdla" Text="Submit" class="btn btn-success mt-2 " OnClick="btnf1_rdla_Click" />
                                            </div>
                                        </div>
                                        <div class="row">
                                           <asp:GridView runat="server" ID="grdf1_rbla"  class="table  text-center table-sm  table-bordered mt-2 " AutoGenerateColumns="false" OnRowCommand="grdf1_rbla_RowCommand">
                                               <Columns>
                                                   


                                                   <asp:TemplateField HeaderText="S No">
                                                       <ItemTemplate>
                                                           <%#Container.DataItemIndex+1 %>
                                                       </ItemTemplate>
                                                   </asp:TemplateField>

                                                   <asp:TemplateField HeaderText="Variable Name">
                                                       <ItemTemplate>
                                                          <%#Eval("VariableName") %>
                                                       </ItemTemplate>
                                                   </asp:TemplateField>

                                                   <asp:TemplateField HeaderText=" Variable Value Field No">
                                                       <ItemTemplate>
                                                           <%#Eval("VariableFieldNo") %>    
                                                       </ItemTemplate>
                                                   </asp:TemplateField>
                                                   <asp:TemplateField HeaderText="Delete">
                                                       <ItemTemplate>
                                                          <asp:Button runat="server" Text="Delete" ID="dltf1_rdla" CssClass="btn btn-warning btn-sm" CommandArgument=' <%#Container.DataItemIndex+1 %> '   CommandName="Delete" />
                                                       </ItemTemplate>
                                                   </asp:TemplateField>
                                               </Columns>

                                           </asp:GridView>
                                            </div>

                                    </div>
                                     

                                </div>
                                <div class="col-sm-6">
                                    <asp:RadioButton runat="server" ID="rbl1b" GroupName="A" />
                                    <div class="row">
                                        <div class="col-sm-12">
                                            <label class="form-label"> DropDown Populate Query</label>
                                        </div>
                                        <div class="col-sm-12">
                                            <asp:TextBox runat="server" ID="txtrblb1" class="form-control" ></asp:TextBox>
                                        </div>
                                        <div class="col-sm-4 mt-2">
                                            <label class="form-label"> DropDown Value Field</label>
                                        </div>
                                        <div class="col-sm-8 mt-2">
                                            <asp:TextBox runat="server" ID="txtddlvaluef1b" class="form-control"></asp:TextBox>
                                         </div>
                                        <div class="col-sm-4 mt-2">
                                            <label class="form-label"> DropDown Text Field</label>
                                        </div>
                                          <div class="col-sm-8 mt-2">
                                            <asp:TextBox runat="server" ID="txtddltextfieldf1b" class="form-control"></asp:TextBox>
                                         </div>

                                         <div class="row">
                                            <div class="col-sm-4 mt-2">
                                                <asp:TextBox runat="server" ReadOnly="true" CssClass="form-control" Text="Condition"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-8 mt-2">
                                                <asp:TextBox runat="server" CssClass="form-control" ID="txtConditionf1_rdlb"></asp:TextBox>
                                            </div>


                                        </div>

                                        <div class ="row mt-1">
                                            <div class ="col-sm-8">
                                                <label class="form-label"> Condition Variable Name</label>
                                            </div>
                                            <div class="col-sm-4">
                                                <asp:TextBox runat="server" CssClass="form-control" ID="txtconditionf1_varName_rdlb" ></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="row mt-1">
                                            
                                              <div class ="col-sm-8">
                                                <label class="form-label"> Condition Variable From Field No</label>
                                            </div>
                                            <div class="col-sm-4">
                                                <asp:TextBox runat="server" CssClass="form-control" ID="txtconditionf1_fieldname_rdlb" ></asp:TextBox>
                                            </div>
                                        </div>

                                         <div class="row">
                                            <div class="col-sm-12" style="align-content:center">
                                                <asp:Button runat="server" ID="btnf1_rblb" Text="Submit" class="btn btn-success mt-2 " OnClick="btnf1_rblb_Click" />
                                            </div>
                                    </div>
                                        <div class="row">
                                            <asp:GridView  runat="server" ID="grdf1_rblb" class="table  text-center table-sm  table-bordered mt-2" AutoGenerateColumns="false" OnRowCommand="grdf1_rblb_RowCommand" >
                                                <Columns>
                                                   
                                                    <asp:TemplateField HeaderText="S No">
                                                       <ItemTemplate>
                                                           <%#Container.DataItemIndex+1 %>
                                                       </ItemTemplate>
                                                   </asp:TemplateField>

                                                   <asp:TemplateField HeaderText="Variable Name">
                                                       <ItemTemplate>
                                                          <%#Eval("VariableName") %>
                                                       </ItemTemplate>
                                                   </asp:TemplateField>

                                                   <asp:TemplateField HeaderText=" Variable Value Field No">
                                                       <ItemTemplate>
                                                           <%#Eval("VariableFieldNo") %>    
                                                       </ItemTemplate>
                                                   </asp:TemplateField>
                                                   <asp:TemplateField HeaderText="Delete">
                                                       <ItemTemplate>
                                                          <asp:Button runat="server" Text="Delete" ID="dltf1_rdla" CssClass="btn btn-warning btn-sm" CommandArgument=' <%#Container.DataItemIndex+1 %> '   CommandName="Delete" />
                                                       </ItemTemplate>
                                                   </asp:TemplateField>
                                               </Columns>
                                            </asp:GridView>
                                            </div>

                                </div>
                              
                            </div>


                            <div runat="server" id="divradiobutton1" Visible="false" >
                                <div class="row py-1  border rounded shadow mt-3">
                                    <div class="col-sm-8 col-lg-8">
                                       
                                        </div>
                                    <div class="row">
                                       
                                        <div class="col-sm-2"></div>
                                        <div class="col-sm-2">
                                            <lable> Button Text</lable>
                                        </div>
                                        <div class="col-sm-1"></div>
                                        <div class="col-sm-1">
                                            <label>Value</label>
                                        </div>
                                         <div class="col-sm-2"></div>
                                        <div class="col-sm-2">
                                            <label>Default Select</label>
                                        </div>
                                    </div>
                                    <div class="row mt-3 px-4">
                                       
                                        <div class="col-sm-1">
                                            <label>1</label>
                                            <asp:CheckBox runat="server" ID="chkf1_chk1" class="form-check" />
                                        </div>
                                     
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtF1_chk1" CssClass="form-control" ></asp:TextBox>
                                        </div>
                                       
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtvalf1_chk1" CssClass="form-control"> </asp:TextBox>
                                        </div>
                                         <div class="col-sm-1"></div>
                                        <div class="col-sm-1">
                                            <asp:RadioButton runat="server" ID="rblf1_l1" GroupName="F1A" />
                                          
                                        </div>
                                                                            </div>
                                    <div class="row mt-2 px-4">
                                       
                                        <div class="col-sm-1">
                                            <label>2</label>
                                            <asp:CheckBox runat="server" ID="chkf1_chk2" class="form-check" />
                                        </div>
                                       
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtf1_chk2" CssClass="form-control" ></asp:TextBox>
                                        </div>
                                       
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtvalf1_chk2" CssClass="form-control"> </asp:TextBox>
                                        </div>
                                         <div class="col-sm-1"></div>
                                         <div class="col-sm-1">
                                              <asp:RadioButton runat="server" ID="rblf1_l2" GroupName="F1A" />
                                          
                                        </div>
                                                                            </div>
                                    <div class="row mt-2 px-4">
                                       
                                        <div class="col-sm-1">
                                            <label>3</label>
                                            <asp:CheckBox runat="server" ID="chkf1_chk3" class="form-check" />
                                        </div>
                                      
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtf1_chk3" CssClass="form-control" ></asp:TextBox>
                                        </div>
                                      
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtvalf1_chk3" CssClass="form-control"> </asp:TextBox>
                                        </div>
                                        <div class="col-sm-1"></div>
                                         <div class="col-sm-1">
                                              <asp:RadioButton runat="server" ID="rblf1_l3" GroupName="F1A" />
                                            
                                        </div>
                                                                            </div>
                                    <div class="row mt-2 px-4">
                                       
                                        <div class="col-sm-1">
                                            <label>4</label>
                                            <asp:CheckBox runat="server" ID="chkf1_chk4" class="form-check" />
                                        </div>
                                       
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtf1_chk4" CssClass="form-control" ></asp:TextBox>
                                        </div>
                                        
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtvalf1_chk4" CssClass="form-control"> </asp:TextBox>
                                        </div>
                                         <div class="col-sm-1"></div>
                                         <div class="col-sm-1">
                                             <asp:RadioButton runat="server" ID="rblf1_l4" GroupName="F1A" />
                                            
                                        </div>
                                                                            </div>
                                    <div class="row mt-2 px-4">
                                       
                                        <div class="col-sm-1">
                                            <label>5</label>
                                            <asp:CheckBox runat="server" ID="chkf1_chk5" class="form-check" />
                                        </div>
                                      
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtf1_chk5" CssClass="form-control" ></asp:TextBox>
                                        </div>
                                        
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtvalf1_chk5" CssClass="form-control"> </asp:TextBox>
                                        </div>
                                         <div class="col-sm-1"></div>
                                         <div class="col-sm-1">
                                               <asp:RadioButton runat="server" ID="rblf1_l5" GroupName="F1A" />
                                            
                                        </div>
                                                                            </div>
                                    <div class="row mt-2 px-4">
                                       
                                        <div class="col-sm-1">
                                            <label>6</label>
                                            <asp:CheckBox runat="server" ID="chkf1_chk6" class="form-check" />
                                        </div>
                                      
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtf1_chk6" CssClass="form-control" ></asp:TextBox>
                                        </div>
                                     
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtvalf1_chk6" CssClass="form-control"> </asp:TextBox>
                                        </div>
                                         <div class="col-sm-1"></div>
                                         <div class="col-sm-1">
                                              <asp:RadioButton runat="server" ID="rblf1_l6" GroupName="F1A" />
                                           
                                        </div>
                                                                            </div>
                                </div>
                            </div>

                        </div>
                            </div>
                        <!---------first-field End------>

                        <!---------second-field Start------>

                         <div runat="server" ID="div2" visible="false">

                        <div class="row pt-1">
                            <label class="form-label"> <b>2st Field </b></label>
                              <div class="row">
                                <div class="col-sm-2">
                                    <lable class="form-label"> Field SeqNo</lable>
                                </div>
                                <div class="col-sm-2">
                                    <asp:TextBox runat="server" ID="txtf2seqno" CssClass="form-control" MaxLength="2"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">

                            <div class="col-sm-2">
                                <label class="form-label" >Field Name</label>
                            </div>
                            <div class="col-sm-2">
                                <asp:TextBox runat="server" ID="txtfilednamef2" class="form-control"></asp:TextBox>
                            </div>
                            
                            <div class="col-sm-2">
                                <label class="form-label">Lable Caption</label>
                            </div>

                            <div class="col-sm-2">
                                <asp:TextBox runat="server" ID="txtlablecaptionF2" class="form-control"></asp:TextBox>
                            </div>
                           <div class="col-sm-2">
                               <label class="form-label">Control Type</label>
                           </div>
                            <div class="col-sm-2">
                                <asp:DropDownList runat="server" ID="ddlcontroltypef2" CssClass="form-control" OnSelectedIndexChanged="ddlcontroltypef2_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="TextBox"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="DropDown"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="CheckBox"></asp:ListItem>
                                    <asp:ListItem Value="4" Text="RadioButton"></asp:ListItem>
                                    <asp:ListItem Value="5" Text="DatePicker"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                                </div>
                            <div class="row pt-1  border rounded shadow mt-3" id="ddlshow2" runat="server" visible="false">
                                <div class="col-sm-6 ">
                                    <div class="row">
                                        <div class="col-sm-1 mt-2"></div>
                                        <asp:RadioButton runat="server"  id="rdlf2a"  GroupName="A" style="margin-left: -15px;"/>
                                        <div class="col-sm-4 mt-2">
                                            <label> DropDown From Table </label>
                                        </div>
                                        <div class="col-sm-8 mt-2">
                                            <asp:TextBox runat="server" ID="txtddltablef2" class="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-4 mt-2">
                                            <label class="form-label"> DropDown Value Field</label>
                                        </div>
                                        <div class="col-sm-8 mt-2">
                                            <asp:TextBox runat="server" ID="txtddlvaluefiedlf2" class="form-control"></asp:TextBox>
                                         </div>
                                        <div class="col-sm-4 mt-2">
                                            <label class="form-label"> DropDown Text Field</label>
                                        </div>
                                        <div class="col-sm-8 mt-2">
                                            <asp:TextBox runat="server" ID="ddltextfieldf2" class="form-control"></asp:TextBox>
                                         </div>
                                    </div>
                                     

                                
                                 <div class="row">
                                            <div class="col-sm-4 mt-2">
                                                <asp:TextBox runat="server" ReadOnly="true" CssClass="form-control" Text="Condition"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-8 mt-2">
                                                <asp:TextBox runat="server" CssClass="form-control" ID="txtcondtionf2_rbla"></asp:TextBox>
                                            </div>


                                        </div>

                                        <div class ="row mt-1">
                                            <div class ="col-sm-8">
                                                <label class="form-label"> Condition Variable Name</label>
                                            </div>
                                            <div class="col-sm-4">
                                                <asp:TextBox runat="server" CssClass="form-control" ID="txtcondtionvarnamef2_rbla" ></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="row mt-1">
                                            
                                              <div class ="col-sm-8">
                                                <label class="form-label"> Condition Variable From Field No</label>
                                            </div>
                                            <div class="col-sm-4">
                                                <asp:TextBox runat="server" CssClass="form-control" ID="txtcondtiionvarif2_rbla" ></asp:TextBox>
                                            </div>
                                        </div>

                                         <div class="row">
                                            <div class="col-sm-12" style="align-content:center">
                                                <asp:Button runat="server" ID="btnF2_rdla" Text="Submit" class="btn btn-success mt-2 " OnClick="btnF2_rdla_Click" />
                                            </div>
                                    </div>
                                        <div class="row">
                                            <asp:GridView  runat="server" ID="grdF2_rdla" class="table  text-center table-sm  table-bordered mt-2" AutoGenerateColumns="false" OnRowCommand="grdF2_rdla_RowCommand" >
                                                <Columns>
                                                   
                                                    <asp:TemplateField HeaderText="S No">
                                                       <ItemTemplate>
                                                           <%#Container.DataItemIndex+1 %>
                                                       </ItemTemplate>
                                                   </asp:TemplateField>

                                                   <asp:TemplateField HeaderText="Variable Name">
                                                       <ItemTemplate>
                                                          <%#Eval("VariableName") %>
                                                       </ItemTemplate>
                                                   </asp:TemplateField>

                                                   <asp:TemplateField HeaderText=" Variable Value Field No">
                                                       <ItemTemplate>
                                                           <%#Eval("VariableFieldNo") %>    
                                                       </ItemTemplate>
                                                   </asp:TemplateField>
                                                   <asp:TemplateField HeaderText="Delete">
                                                       <ItemTemplate>
                                                          <asp:Button runat="server" Text="Delete" ID="dltf2_rdla" CssClass="btn btn-warning btn-sm" CommandArgument=' <%#Container.DataItemIndex+1 %> '   CommandName="Delete" />
                                                       </ItemTemplate>
                                                   </asp:TemplateField>
                                               </Columns>
                                            </asp:GridView>
                                            </div>
                                    </div>

                                <div class="col-sm-6">
                                    <asp:RadioButton runat="server" ID="rblf2b" GroupName="A" />
                                    <div class="row">
                                        <div class="col-sm-12 mt-2">
                                            <label class="form-label"> DropDown Populate Query</label>
                                        </div>
                                        <div class="col-sm-12 mt-2">
                                            <asp:TextBox runat="server" ID="txtddlpopulatequeryf2" class="form-control" ></asp:TextBox>
                                        </div>


                                         <div class="col-sm-4 mt-2">
                                            <label class="form-label"> DropDown Value Field</label>
                                        </div>
                                        <div class="col-sm-8 mt-2">
                                            <asp:TextBox runat="server" ID="txtddlvaluef2b" class="form-control"></asp:TextBox>
                                         </div>
                                        <div class="col-sm-4 mt-2">
                                            <label class="form-label"> DropDown Text Field</label>
                                        </div>
                                          <div class="col-sm-8 mt-2">
                                            <asp:TextBox runat="server" ID="txtddltextfieldf2b" class="form-control"></asp:TextBox>
                                         </div>
                                    </div>
                                    

                                       <div class="row">
                                            <div class="col-sm-4 mt-2">
                                                <asp:TextBox runat="server" ReadOnly="true" CssClass="form-control" Text="Condition"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-8 mt-2">
                                                <asp:TextBox runat="server" CssClass="form-control" ID="txtconditionF2_rdlb"></asp:TextBox>
                                            </div>


                                        </div>

                                        <div class ="row mt-1">
                                            <div class ="col-sm-8">
                                                <label class="form-label"> Condition Variable Name</label>
                                            </div>
                                            <div class="col-sm-4">
                                                <asp:TextBox runat="server" CssClass="form-control" ID="txtconditionvalnameF2_rdlb" ></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="row mt-1">
                                            
                                              <div class ="col-sm-8">
                                                <label class="form-label"> Condition Variable From Field No</label>
                                            </div>
                                            <div class="col-sm-4">
                                                <asp:TextBox runat="server" CssClass="form-control" ID="txtconditionvalfieldnoF2_rdlb" ></asp:TextBox>
                                            </div>
                                        </div>

                                         <div class="row">
                                            <div class="col-sm-12" style="align-content:center">
                                                <asp:Button runat="server" ID="btnsubmitF2_rdlb" Text="Submit" class="btn btn-success mt-2 " OnClick="btnsubmitF2_rdlb_Click" />
                                            </div>
                                    </div>
                                        <div class="row">
                                            <asp:GridView  runat="server" ID="grdF2_rdlb" class="table  text-center table-sm  table-bordered mt-2" AutoGenerateColumns="false" OnRowCommand="grdF2_rdlb_RowCommand" >
                                                <Columns>
                                                   
                                                    <asp:TemplateField HeaderText="S No">
                                                       <ItemTemplate>
                                                           <%#Container.DataItemIndex+1 %>
                                                       </ItemTemplate>
                                                   </asp:TemplateField>

                                                   <asp:TemplateField HeaderText="Variable Name">
                                                       <ItemTemplate>
                                                          <%#Eval("VariableName") %>
                                                       </ItemTemplate>
                                                   </asp:TemplateField>

                                                   <asp:TemplateField HeaderText=" Variable Value Field No">
                                                       <ItemTemplate>
                                                           <%#Eval("VariableFieldNo") %>    
                                                       </ItemTemplate>
                                                   </asp:TemplateField>
                                                   <asp:TemplateField HeaderText="Delete">
                                                       <ItemTemplate>
                                                          <asp:Button runat="server" Text="Delete" ID="dltF2_rdlb" CssClass="btn btn-warning btn-sm" CommandArgument=' <%#Container.DataItemIndex+1 %> '   CommandName="Delete" />
                                                       </ItemTemplate>
                                                   </asp:TemplateField>
                                               </Columns>
                                            </asp:GridView>
                                            </div>
                                    </div>
                                


                                
                              
                            </div>


                            <div runat="server" id="divradiobutton2"  visible="false">
                                <div class="row p-1  border rounded shadow mt-3">
                                    <div class="col-sm-8 col-lg-8">
<%--                                          <label> <span> Radio Button</span></label>--%>
                                        </div>
                                    <div class="row mt-3 px-4">
                                         <div class="row">
                                       
                                        <div class="col-sm-2"></div>
                                        <div class="col-sm-2">
                                            <lable> Button Text</lable>
                                        </div>
                                        <div class="col-sm-1"></div>
                                        <div class="col-sm-1">
                                            <label>Value</label>
                                        </div>
                                         <div class="col-sm-2"></div>
                                        <div class="col-sm-2">
                                            <label>Default Select</label>
                                        </div>
                                    </div>
                                       
                                        <div class="col-sm-1">
                                            <label>1</label>
                                            <asp:CheckBox runat="server" ID="chkf2_chk1" class="form-check" />
                                        </div>
                                        <%--<div class="col-sm-2">
                                            <label class="form-label"> Button Text</label>
                                        </div>--%>
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtbtnf2_chk1" CssClass="form-control" ></asp:TextBox>
                                        </div>
                                        <%--<div class="col-sm-1">
                                            <label class="form-label">Value</label>
                                        </div>--%>
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtvalf2_chk1" CssClass="form-control"> </asp:TextBox>
                                        </div>

                                        
                                                <div class="col-sm-1"></div>
                                        <div class="col-sm-1">
                                              <asp:RadioButton runat="server" ID="rblf2_l1" GroupName="F2A" />
                                           
                                        </div>
                                                                            </div>
                                    <div class="row mt-2 px-4">
                                       
                                        <div class="col-sm-1">
                                            <label>2</label>
                                            <asp:CheckBox runat="server" ID="chkf2_chk2" class="form-check" />
                                        </div>
                                     
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtbtnf2_chk2" CssClass="form-control" ></asp:TextBox>
                                        </div>
                                       
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtvalf2_chk2" CssClass="form-control"> </asp:TextBox>
                                        </div>
                                        
 <div class="col-sm-1"></div>
                                        <div class="col-sm-1">
                                            <asp:RadioButton runat="server" ID="rblf2_l2" GroupName="F2A" />
                                            
                                        </div>
                                                                            </div>
                                    <div class="row mt-2 px-4">
                                       
                                        <div class="col-sm-1">
                                            <label>3</label>
                                            <asp:CheckBox runat="server" ID="chkf2_chk3" class="form-check" />
                                        </div>
                                        
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtbtnf2_chk3" CssClass="form-control" ></asp:TextBox>
                                        </div>
                                       
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtvalf2_chk3" CssClass="form-control"> </asp:TextBox>
                                        </div>
                                        
                                            <div class="col-sm-1"></div>
                                        <div class="col-sm-1">
                                            <asp:RadioButton runat="server" ID="rblf2_l3" GroupName="F2A" />
                                          
                                        </div>
                                                                            </div>
                                    <div class="row mt-2 px-4">
                                       
                                        <div class="col-sm-1">
                                            <label>4</label>
                                            <asp:CheckBox runat="server" ID="chkf2_chk4" class="form-check" />
                                        </div>
                                       
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtbtn2f2_chk4" CssClass="form-control" ></asp:TextBox>
                                        </div>
                                        
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtvalf2_chk4" CssClass="form-control"> </asp:TextBox>
                                        </div>
                                        
 <div class="col-sm-1"></div>
                                        <div class="col-sm-1">
                                            <asp:RadioButton runat="server" ID="rblf2_l4" GroupName="F2A" />
                                           
                                        </div>
                                                                            </div>
                                    <div class="row mt-2 px-4">
                                       
                                        <div class="col-sm-1">
                                            <label>5</label>
                                            <asp:CheckBox runat="server" ID="chkf2_chk5" class="form-check" />
                                        </div>
                                        
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtbtnf2_chk5" CssClass="form-control" ></asp:TextBox>
                                            </div>
                                       
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtvalf2_chk5" CssClass="form-control"> </asp:TextBox>
                                        </div>
                                            
                                           <div class="col-sm-1"></div>
                                        <div class="col-sm-1">
                                            <asp:RadioButton runat="server" ID="rblf2_l5" GroupName="F2A" />
                                            
                                        </div>
                                                                            </div>
                                    <div class="row mt-2 px-4">
                                       
                                        <div class="col-sm-1">
                                            <label>6</label>
                                            <asp:CheckBox runat="server" ID="chkf2_chk6" class="form-check" />
                                        </div>
                                        
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtbtnf2_chk6" CssClass="form-control" ></asp:TextBox>
                                        </div>
                                        
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtvalf2_chk6" CssClass="form-control"> </asp:TextBox>
                                        </div>
                                        
 <div class="col-sm-1"></div>
                                        <div class="col-sm-1">
                                             <asp:RadioButton runat="server" ID="rblf2_l6" GroupName="F2A" />
                                            
                                        </div>
                                                                            </div>
                                </div>
                            </div>

                        </div>
                            </div>
                        <!---------second-field End------>




                          <!---------third-field Start------>
                         <div runat="server" ID="div3" visible="false">

                        <div class="row pt-1">
                            <label class="form-label"> <b>3st Field </b></label>
                               <div class="row">
                                <div class="col-sm-2">
                                    <lable class="form-label"> Field SeqNo</lable>
                                </div>
                                <div class="col-sm-2">
                                    <asp:TextBox runat="server" ID="txtf3seqno" CssClass="form-control" MaxLength="2"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row"> 
                            <div class="col-sm-2">
                                <label class="form-label" >Field Name</label>
                            </div>
                            <div class="col-sm-2">
                                <asp:TextBox runat="server" ID="txtfieldnamef3" class="form-control"></asp:TextBox>
                            </div>
                            
                            <div class="col-sm-2">
                                <label class="form-label">Lable Caption</label>
                            </div>

                            <div class="col-sm-2">
                                <asp:TextBox runat="server" ID="txtlablecaptionf3" class="form-control"></asp:TextBox>
                            </div>
                           <div class="col-sm-2">
                               <label class="form-label">Control Type</label>
                           </div>
                            <div class="col-sm-2">
                                <asp:DropDownList runat="server" ID="ddlcontroltypef3" CssClass="form-control" OnSelectedIndexChanged="ddlcontroltypef3_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="TextBox"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="DropDown"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="CheckBox"></asp:ListItem>
                                    <asp:ListItem Value="4" Text="RadioButton"></asp:ListItem>
                                    <asp:ListItem Value="5" Text="DatePicker"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                                </div>
                            <div class="row pt-1  border rounded shadow mt-3" id="ddlshow3" runat="server" Visible="false">
                                <div class="col-sm-6 ">
                                    <div class="row">
                                        <div class="col-sm-1 mt-2"></div>
                                        <asp:RadioButton runat="server"  id="rblf3a"  GroupName="A" style="margin-left: -15px;"/>
                                        <div class="col-sm-4 mt-2">
                                            <label> DropDown From Table </label>
                                        </div>
                                        <div class="col-sm-8 mt-2">
                                            <asp:TextBox runat="server" ID="txtddlf3rbla" class="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-4 mt-2">
                                            <label class="form-label">DropDown Value Field</label>
                                        </div>
                                        <div class="col-sm-8 mt-2">
                                            <asp:TextBox runat="server" ID="txtddlvaluefieldf3" class="form-control"></asp:TextBox>
                                         </div>
                                        <div class="col-sm-4 mt-2">
                                            <label class="form-label"> DropDown Text Field</label>
                                        </div>
                                        <div class="col-sm-8 mt-2">
                                            <asp:TextBox runat="server" ID="txtddltextfieldf3" class="form-control"></asp:TextBox>
                                         </div>
                                    </div>
                                       <div class="row">
                                            <div class="col-sm-4 mt-2">
                                                <asp:TextBox runat="server" ReadOnly="true" CssClass="form-control" Text="Condition"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-8 mt-2">
                                                <asp:TextBox runat="server" CssClass="form-control" ID="txtconditionF3_rdla"></asp:TextBox>
                                            </div>


                                        </div>

                                        <div class ="row mt-1">
                                            <div class ="col-sm-8">
                                                <label class="form-label"> Condition Variable Name</label>
                                            </div>
                                            <div class="col-sm-4">
                                                <asp:TextBox runat="server" CssClass="form-control" ID="txtconditionvarNameF3_rdla" ></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="row mt-1">
                                            
                                              <div class ="col-sm-8">
                                                <label class="form-label"> Condition Variable From Field No</label>
                                            </div>
                                            <div class="col-sm-4">
                                                <asp:TextBox runat="server" CssClass="form-control" ID="txtconditionvarFieldnameF3_rdla" ></asp:TextBox>
                                            </div>
                                        </div>

                                         <div class="row">
                                            <div class="col-sm-12" style="align-content:center">
                                                <asp:Button runat="server" ID="btnF3_rdla" Text="Submit" class="btn btn-success mt-2 " OnClick="btnF3_rdla_Click" />
                                            </div>
                                    </div>
                                        <div class="row">
                                            <asp:GridView  runat="server" ID="grdF3_rdla" class="table  text-center table-sm  table-bordered mt-2" AutoGenerateColumns="false" OnRowCommand="grdF3_rdla_RowCommand" >
                                                <Columns>
                                                   
                                                    <asp:TemplateField HeaderText="S No">
                                                       <ItemTemplate>
                                                           <%#Container.DataItemIndex+1 %>
                                                       </ItemTemplate>
                                                   </asp:TemplateField>

                                                   <asp:TemplateField HeaderText="Variable Name">
                                                       <ItemTemplate>
                                                          <%#Eval("VariableName") %>
                                                       </ItemTemplate>
                                                   </asp:TemplateField>

                                                   <asp:TemplateField HeaderText=" Variable Value Field No">
                                                       <ItemTemplate>
                                                           <%#Eval("VariableFieldNo") %>    
                                                       </ItemTemplate>
                                                   </asp:TemplateField>
                                                   <asp:TemplateField HeaderText="Delete">
                                                       <ItemTemplate>
                                                          <asp:Button runat="server" Text="Delete" ID="dltF3_rdla" CssClass="btn btn-warning btn-sm" CommandArgument=' <%#Container.DataItemIndex+1 %> '   CommandName="Delete" />
                                                       </ItemTemplate>
                                                   </asp:TemplateField>
                                               </Columns>
                                            </asp:GridView>
                                            </div>

                                </div>
                                <div class="col-sm-6">
                                    <asp:RadioButton runat="server" ID="rdlf3b" GroupName="A" />
                                    <div class="row">
                                        <div class="col-sm-12 mt-2">
                                            <label class="form-label"> DropDown Populate Query</label>
                                        </div>
                                        <div class="col-sm-12 mt-2">
                                            <asp:TextBox runat="server" ID="txtddlpopulatef3b" class="form-control" ></asp:TextBox>
                                        </div>

                                         <div class="col-sm-4 mt-2">
                                            <label class="form-label"> DropDown Value Field</label>
                                        </div>
                                        <div class="col-sm-8 mt-2">
                                            <asp:TextBox runat="server" ID="txtddlvaluef3b" class="form-control"></asp:TextBox>
                                         </div>
                                        <div class="col-sm-4 mt-2">
                                            <label class="form-label"> DropDown Text Field</label>
                                        </div>
                                          <div class="col-sm-8 mt-2">
                                            <asp:TextBox runat="server" ID="txtddltextf3b" class="form-control"></asp:TextBox>
                                         </div>
                                    </div>

                                      <div class="row">
                                            <div class="col-sm-4 mt-2">
                                                <asp:TextBox runat="server" ReadOnly="true" CssClass="form-control" Text="Condition"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-8 mt-2">
                                                <asp:TextBox runat="server" CssClass="form-control" ID="txtconditionF3_rdlb"></asp:TextBox>
                                            </div>


                                        </div>

                                        <div class ="row mt-1">
                                            <div class ="col-sm-8">
                                                <label class="form-label"> Condition Variable Name</label>
                                            </div>
                                            <div class="col-sm-4">
                                                <asp:TextBox runat="server" CssClass="form-control" ID="txtconditionVarNameF3_rdlb" ></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="row mt-1">
                                            
                                              <div class ="col-sm-8">
                                                <label class="form-label"> Condition Variable From Field No</label>
                                            </div>
                                            <div class="col-sm-4">
                                                <asp:TextBox runat="server" CssClass="form-control" ID="txtconditionVarFieldNoF3_rdlb" ></asp:TextBox>
                                            </div>
                                        </div>

                                         <div class="row">
                                            <div class="col-sm-12" style="align-content:center">
                                                <asp:Button runat="server" ID="btnsubmitF3_rdlb" Text="Submit" class="btn btn-success mt-2 " OnClick="btnsubmitF3_rdlb_Click" />
                                            </div>
                                    </div>
                                        <div class="row">
                                            <asp:GridView  runat="server" ID="grdF3_rdlb" class="table  text-center table-sm  table-bordered mt-2" AutoGenerateColumns="false"  OnRowCommand="grdF3_rdlb_RowCommand">
                                                <Columns>
                                                   
                                                    <asp:TemplateField HeaderText="S No">
                                                       <ItemTemplate>
                                                           <%#Container.DataItemIndex+1 %>
                                                       </ItemTemplate>
                                                   </asp:TemplateField>

                                                   <asp:TemplateField HeaderText="Variable Name">
                                                       <ItemTemplate>
                                                          <%#Eval("VariableName") %>
                                                       </ItemTemplate>
                                                   </asp:TemplateField>

                                                   <asp:TemplateField HeaderText=" Variable Value Field No">
                                                       <ItemTemplate>
                                                           <%#Eval("VariableFieldNo") %>    
                                                       </ItemTemplate>
                                                   </asp:TemplateField>
                                                   <asp:TemplateField HeaderText="Delete">
                                                       <ItemTemplate>
                                                          <asp:Button runat="server" Text="Delete" ID="DltF3_rdlb" CssClass="btn btn-warning btn-sm" CommandArgument=' <%#Container.DataItemIndex+1 %> '   CommandName="Delete" />
                                                       </ItemTemplate>
                                                   </asp:TemplateField>
                                               </Columns>
                                            </asp:GridView>
                                            </div>


                                </div>
                              
                            </div>


                            <div runat="server" id="divradiobutton3" Visible="false">
                                <div class="row p-1  border rounded shadow mt-3 ">
                                   
                                    <div class="row mt-3 px-4">
                                         <div class="row">
                                       
                                        <div class="col-sm-2"></div>
                                        <div class="col-sm-2">
                                            <lable> Button Text</lable>
                                        </div>
                                        <div class="col-sm-1"></div>
                                        <div class="col-sm-1">
                                            <label>Value</label>
                                        </div>
                                         <div class="col-sm-2"></div>
                                        <div class="col-sm-2">
                                            <label>Default Select</label>
                                        </div>
                                    </div>
                                       
                                        <div class="col-sm-1">
                                            <label>1</label>
                                            <asp:CheckBox runat="server" ID="f3_chk1" class="form-check" />
                                        </div>
                                       
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtbtnf3_chk1" CssClass="form-control" ></asp:TextBox>
                                        </div>
                                        
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtvaluef3_chk1" CssClass="form-control"> </asp:TextBox>
                                        </div>
                                        
 <div class="col-sm-1"></div>
                                        <div class="col-sm-1">
                                              <asp:RadioButton runat="server" ID="rblf3_l1" GroupName="F3A" />
                                           
                                        </div>
                                                                            </div>
                                    <div class="row mt-2 px-4">
                                       
                                        <div class="col-sm-1">
                                            <label>2</label>
                                            <asp:CheckBox runat="server" ID="f3_chk2" class="form-check" />
                                        </div>
                                       
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtbtnf3_chk2" CssClass="form-control" ></asp:TextBox>
                                        </div>
                                        
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtvalf3_chk2" CssClass="form-control"> </asp:TextBox>
                                        </div>
                                        
 <div class="col-sm-1"></div>
                                        <div class="col-sm-1">
                                             <asp:RadioButton runat="server" ID="rblf3_l2" GroupName="F3A" />
                                            
                                        </div>
                                                                            </div>
                                    <div class="row mt-2 px-4">
                                       
                                        <div class="col-sm-1">
                                            <label>3</label>
                                            <asp:CheckBox runat="server" ID="f3_chk3" class="form-check" />
                                        </div>
                                       
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtbtnf3_chk3" CssClass="form-control" ></asp:TextBox>
                                        </div>
                                        
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtvalf3_chk3" CssClass="form-control"> </asp:TextBox>
                                        </div>

                                        
 <div class="col-sm-1"></div>
                                        <div class="col-sm-1">
                                             <asp:RadioButton runat="server" ID="rblf3_l3" GroupName="F3A" />
                                           
                                        </div>
                                                                            </div>
                                    <div class="row mt-2 px-4">
                                       
                                        <div class="col-sm-1">
                                            <label>4</label>
                                            <asp:CheckBox runat="server" ID="f3chk4" class="form-check" />
                                        </div>
                                      
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtbtnf3chk4_chk1" CssClass="form-control" ></asp:TextBox>
                                        </div>
                                       
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtvalf3chk4_chk2" CssClass="form-control"> </asp:TextBox>
                                        </div>

                                        
 <div class="col-sm-1"></div>
                                        <div class="col-sm-1">
                                            <asp:RadioButton runat="server" ID="rblf3_l4" GroupName="F3A" />
                                            
                                        </div>
                                                                            </div>
                                    <div class="row mt-2 px-4">
                                       
                                        <div class="col-sm-1">
                                            <label>5</label>
                                            <asp:CheckBox runat="server" ID="f3chk5" class="form-check" />
                                        </div>
                                        
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtbtnf3chk5_chk1" CssClass="form-control" ></asp:TextBox>
                                        </div>
                                        
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtvalf3chk5_chk2" CssClass="form-control"> </asp:TextBox>
                                        </div>
                                        
 <div class="col-sm-1"></div>
                                        <div class="col-sm-1">
                                            <asp:RadioButton runat="server" ID="rblf3_l5" GroupName="F3A" />
                                            
                                        </div>
                                                                            </div>
                                    <div class="row mt-2 px-4">
                                       
                                        <div class="col-sm-1">
                                            <label>6</label>
                                            <asp:CheckBox runat="server" ID="f3chk6" class="form-check" />
                                        </div>
                                      
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtbtnf3chk6_chk1" CssClass="form-control" ></asp:TextBox>
                                        </div>
                                       
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtvalf3chk6_chk2" CssClass="form-control"> </asp:TextBox>
                                        </div>
                                        
 <div class="col-sm-1"></div>
                                        <div class="col-sm-1">
                                             <asp:RadioButton runat="server" ID="rblf3_l6" GroupName="F3A" />
                                            
                                        </div>
                                                                            </div>
                                </div>
                            </div>

                        </div>
                            </div>

                        <!---------third-field End------>



                         



                          <!---------fourth-field Start------>
                         <div runat="server" ID="div4" visible="false">

                        <div class="row pt-1">
                            <label class="form-label"> <b>4st Field </b></label>
                               <div class="row">
                                <div class="col-sm-2">
                                    <lable class="form-label"> Field SeqNo</lable>
                                </div>
                                <div class="col-sm-2">
                                    <asp:TextBox runat="server" ID="txtf4seqno" CssClass="form-control" MaxLength="2"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                            <div class="col-sm-2">
                                <label class="form-label" >Field Name</label>
                            </div>
                            <div class="col-sm-2">
                                <asp:TextBox runat="server" ID="txtfieldnamef4" class="form-control"></asp:TextBox>
                            </div>
                            
                            <div class="col-sm-2">
                                <label class="form-label">Lable Caption</label>
                            </div>

                            <div class="col-sm-2">
                                <asp:TextBox runat="server" ID="txtlablecaptionf4" class="form-control"></asp:TextBox>
                            </div>
                           <div class="col-sm-2">
                               <label class="form-label">Control Type</label>
                           </div>
                            <div class="col-sm-2">
                                <asp:DropDownList runat="server" ID="ddlcontroltypef4" CssClass="form-control" OnSelectedIndexChanged="ddlcontroltypef4_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="TextBox"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="DropDown"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="CheckBox"></asp:ListItem>
                                    <asp:ListItem Value="4" Text="RadioButton"></asp:ListItem>
                                    <asp:ListItem Value="5" Text="DatePicker"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                                </div>
                            <div class="row pt-1  border rounded shadow mt-3 " id="ddlshow4" runat="server"  visible="false">
                                <div class="col-sm-6 ">
                                    <div class="row">
                                        <div class="col-sm-1 mt-2"></div>
                                        <asp:RadioButton runat="server"  id="ddlrblf4a"  GroupName="A" style="margin-left: -15px;"/>
                                        <div class="col-sm-4 mt-2">
                                            <label> DropDown From Table </label>
                                        </div>
                                        <div class="col-sm-8 mt-2">
                                            <asp:TextBox runat="server" ID="txtddltablef4" class="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-4 mt-2">
                                            <label class="form-label"> DropDown Value Field</label>
                                        </div>
                                        <div class="col-sm-8 mt-2">
                                            <asp:TextBox runat="server" ID="txtddlvaluefieldf4" class="form-control"></asp:TextBox>
                                         </div>
                                        <div class="col-sm-4 mt-2">
                                            <label class="form-label"> DropDown Text Field</label>
                                        </div>
                                        <div class="col-sm-8 mt-2">
                                            <asp:TextBox runat="server" ID="txtddltextfieldf4" class="form-control"></asp:TextBox>
                                         </div>
                                    </div>
                                      <div class="row">
                                            <div class="col-sm-4 mt-2">
                                                <asp:TextBox runat="server" ReadOnly="true" CssClass="form-control" Text="Condition"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-8 mt-2">
                                                <asp:TextBox runat="server" CssClass="form-control" ID="txtConditionF4_rdla"></asp:TextBox>
                                            </div>


                                        </div>

                                        <div class ="row mt-1">
                                            <div class ="col-sm-8">
                                                <label class="form-label"> Condition Variable Name</label>
                                            </div>
                                            <div class="col-sm-4">
                                                <asp:TextBox runat="server" CssClass="form-control" ID="txtConditionVarNameF4_rdla" ></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="row mt-1">
                                            
                                              <div class ="col-sm-8">
                                                <label class="form-label"> Condition Variable From Field No</label>
                                            </div>
                                            <div class="col-sm-4">
                                                <asp:TextBox runat="server" CssClass="form-control" ID="txtConditionVarFieldnoF4_rdla" ></asp:TextBox>
                                            </div>
                                        </div>

                                         <div class="row">
                                            <div class="col-sm-12" style="align-content:center">
                                                <asp:Button runat="server" ID="btnsubmitF4_rdla" Text="Submit" class="btn btn-success mt-2 " OnClick="btnsubmitF4_rdla_Click" />
                                            </div>
                                    </div>
                                        <div class="row">
                                            <asp:GridView  runat="server" ID="grdF4_rdla" class="table  text-center table-sm  table-bordered mt-2" AutoGenerateColumns="false" OnRowCommand="grdF4_rdla_RowCommand" >
                                                <Columns>
                                                   
                                                    <asp:TemplateField HeaderText="S No">
                                                       <ItemTemplate>
                                                           <%#Container.DataItemIndex+1 %>
                                                       </ItemTemplate>
                                                   </asp:TemplateField>

                                                   <asp:TemplateField HeaderText="Variable Name">
                                                       <ItemTemplate>
                                                          <%#Eval("VariableName") %>
                                                       </ItemTemplate>
                                                   </asp:TemplateField>

                                                   <asp:TemplateField HeaderText=" Variable Value Field No">
                                                       <ItemTemplate>
                                                           <%#Eval("VariableFieldNo") %>    
                                                       </ItemTemplate>
                                                   </asp:TemplateField>
                                                   <asp:TemplateField HeaderText="Delete">
                                                       <ItemTemplate>
                                                          <asp:Button runat="server" Text="Delete" ID="DltF4_rdla" CssClass="btn btn-warning btn-sm" CommandArgument=' <%#Container.DataItemIndex+1 %> '   CommandName="Delete" />
                                                       </ItemTemplate>
                                                   </asp:TemplateField>
                                               </Columns>
                                            </asp:GridView>
                                            </div>
                                     

                                </div>
                                <div class="col-sm-6">
                                    <asp:RadioButton runat="server" ID="rblf4b" GroupName="A" />
                                    <div class="row">
                                        <div class="col-sm-12 mt-2">
                                            <label class="form-label"> DropDown Populate Query</label>
                                        </div>
                                        <div class="col-sm-12 mt-2">
                                            <asp:TextBox runat="server" ID="txtddlpopulatequeryf4" class="form-control" ></asp:TextBox>
                                        </div>

                                         <div class="col-sm-4 mt-2">
                                            <label class="form-label"> DropDown Value Field</label>
                                        </div>
                                        <div class="col-sm-8 mt-2">
                                            <asp:TextBox runat="server" ID="txtddlvaluef4b" class="form-control"></asp:TextBox>
                                         </div>
                                        <div class="col-sm-4 mt-2">
                                            <label class="form-label"> DropDown Text Field</label>
                                        </div>
                                          <div class="col-sm-8 mt-2">
                                            <asp:TextBox runat="server" ID="txtdf4bdltext" class="form-control"></asp:TextBox>
                                         </div>



                           

                                    </div>
                                      <div class="row">
                                            <div class="col-sm-4 mt-2">
                                                <asp:TextBox runat="server" ReadOnly="true" CssClass="form-control" Text="Condition"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-8 mt-2">
                                                <asp:TextBox runat="server" CssClass="form-control" ID="txtconditionF4_rdlb"></asp:TextBox>
                                            </div>


                                        </div>

                                        <div class ="row mt-1">
                                            <div class ="col-sm-8">
                                                <label class="form-label"> Condition Variable Name</label>
                                            </div>
                                            <div class="col-sm-4">
                                                <asp:TextBox runat="server" CssClass="form-control" ID="txtconditionVarNameF4_rdlb" ></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="row mt-1">
                                            
                                              <div class ="col-sm-8">
                                                <label class="form-label"> Condition Variable From Field No</label>
                                            </div>
                                            <div class="col-sm-4">
                                                <asp:TextBox runat="server" CssClass="form-control" ID="txtconditionvanFieldNoF4_rdlb" ></asp:TextBox>
                                            </div>
                                        </div>

                                         <div class="row">
                                            <div class="col-sm-12" style="align-content:center">
                                                <asp:Button runat="server" ID="btnSubmitF4_rdlb" Text="Submit" class="btn btn-success mt-2 " OnClick="btnSubmitF4_rdlb_Click" />
                                            </div>
                                    </div>
                                        <div class="row">
                                            <asp:GridView  runat="server" ID="grdF4_rdlb" class="table  text-center table-sm  table-bordered mt-2" AutoGenerateColumns="false"  OnRowCommand="grdF4_rdlb_RowCommand">
                                                <Columns>
                                                   
                                                    <asp:TemplateField HeaderText="S No">
                                                       <ItemTemplate>
                                                           <%#Container.DataItemIndex+1 %>
                                                       </ItemTemplate>
                                                   </asp:TemplateField>

                                                   <asp:TemplateField HeaderText="Variable Name">
                                                       <ItemTemplate>
                                                          <%#Eval("VariableName") %>
                                                       </ItemTemplate>
                                                   </asp:TemplateField>

                                                   <asp:TemplateField HeaderText=" Variable Value Field No">
                                                       <ItemTemplate>
                                                           <%#Eval("VariableFieldNo") %>    
                                                       </ItemTemplate>
                                                   </asp:TemplateField>
                                                   <asp:TemplateField HeaderText="Delete">
                                                       <ItemTemplate>
                                                          <asp:Button runat="server" Text="Delete" ID="DltF4_rdlb" CssClass="btn btn-warning btn-sm" CommandArgument=' <%#Container.DataItemIndex+1 %> '   CommandName="Delete" />
                                                       </ItemTemplate>
                                                   </asp:TemplateField>
                                               </Columns>
                                            </asp:GridView>
                                            </div>
                                </div>
                              
                            </div>


                            <div runat="server" id="divradiobutton4" visible="false">
                                <div class="row p-1">
                                    
                                    <div class="row mt-3 px-4">
                                         <div class="row">
                                       
                                        <div class="col-sm-2"></div>
                                        <div class="col-sm-2">
                                            <lable> Button Text</lable>
                                        </div>
                                        <div class="col-sm-1"></div>
                                        <div class="col-sm-1">
                                            <label>Value</label>
                                        </div>
                                         <div class="col-sm-2"></div>
                                        <div class="col-sm-2">
                                            <label>Default Select</label>
                                        </div>
                                    </div>
                                       
                                        <div class="col-sm-1">
                                            <label>1</label>
                                            <asp:CheckBox runat="server" ID="chkf4_chk1" class="form-check" />
                                        </div>
                                       
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtbtnchkf4_chk1_chk1" CssClass="form-control" ></asp:TextBox>
                                        </div>
                                        
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtbtnchkf4_chk1_chk2" CssClass="form-control"> </asp:TextBox>
                                        </div>

                                        
 <div class="col-sm-1"></div>
                                        <div class="col-sm-1">
                                             <asp:RadioButton runat="server" ID="rblf4_l1" GroupName="F4A" />
                                           
                                        </div>
                                                                            </div>
                                    <div class="row mt-2 px-4">
                                       
                                        <div class="col-sm-1">
                                            <label>2</label>
                                            <asp:CheckBox runat="server" ID="chkf4_chk2" class="form-check" />
                                        </div>
                                      
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtbtnchkf4_chk2_chk1" CssClass="form-control" ></asp:TextBox>
                                        </div>
                                       
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtvalchkf4_chk2_chk2" CssClass="form-control"> </asp:TextBox>
                                        </div>
                                        
 <div class="col-sm-1"></div>
                                        <div class="col-sm-1">
                                              <asp:RadioButton runat="server" ID="rblf4_l2" GroupName="F4A" />
                                           
                                        </div>
                                                                            </div>
                                    <div class="row mt-2 px-4">
                                       
                                        <div class="col-sm-1">
                                            <label>3</label>
                                            <asp:CheckBox runat="server" ID="chkf4_chk3" class="form-check" />
                                        </div>
                                       
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtbtnchkf4_chk3_chk1" CssClass="form-control" ></asp:TextBox>
                                        </div>
                                       
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtvalchkf4_chk3_chk2" CssClass="form-control"> </asp:TextBox>
                                        </div>
                                        
 <div class="col-sm-1"></div>
                                        <div class="col-sm-1">
                                             <asp:RadioButton runat="server" ID="rblf4_l3" GroupName="F4A" />
                                            
                                        </div>
                                                                            </div>
                                    <div class="row mt-2 px-4">
                                       
                                        <div class="col-sm-1">
                                            <label>4</label>
                                            <asp:CheckBox runat="server" ID="chkf4_chk4" class="form-check" />
                                        </div>
                                       
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtbtnchkf4_chk4_chk1" CssClass="form-control" ></asp:TextBox>
                                        </div>
                                      
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtvalchkf4_chk4_chk2" CssClass="form-control"> </asp:TextBox>
                                        </div>
                                        
 <div class="col-sm-1"></div>
                                        <div class="col-sm-1">
                                             <asp:RadioButton runat="server" ID="rblf4_l4" GroupName="F4A" />
                                            
                                        </div>
                                                                            </div>
                                    <div class="row mt-2 px-4">
                                       
                                        <div class="col-sm-1">
                                            <label>5</label>
                                            <asp:CheckBox runat="server" ID="chkf4_chk5" class="form-check" />
                                        </div>
                                        
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtbtnchkf4_chk5" CssClass="form-control" ></asp:TextBox>
                                        </div>
                                        
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtvalchkf4_chk4" CssClass="form-control"> </asp:TextBox>
                                        </div>
                                        
 <div class="col-sm-1"></div>
                                        <div class="col-sm-1">
                                            <asp:RadioButton runat="server" ID="rblf4_l5" GroupName="F4A" />
                                        </div>
                                                                            </div>
                                    <div class="row mt-2 px-4">
                                       
                                        <div class="col-sm-1">
                                            <label>6</label>
                                            <asp:CheckBox runat="server" ID="chkf4_chk6" class="form-check" />
                                        </div>
                                       
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtbtnchkf4_chk6" CssClass="form-control" ></asp:TextBox>
                                        </div>
                                        
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtvalchkf4_chk6" CssClass="form-control"> </asp:TextBox>
                                        </div>
                                        
 <div class="col-sm-1"></div>
                                        <div class="col-sm-1">
                                            <asp:RadioButton runat="server" ID="rblf4_l6" GroupName="F4A" />
                                           
                                        </div>
                                                                            </div>
                                </div>
                            </div>

                        </div>
                            </div>
                        <!---------fourth-field End------>



                         <!---------fifth-field Start------>
                         <div runat="server" ID="div5" visible="false">

                        <div class="row pt-1">
                            <label class="form-label"> <b>5st Field </b></label>
                               <div class="row">
                                <div class="col-sm-2">
                                    <lable class="form-label"> Field SeqNo</lable>
                                </div>
                                <div class="col-sm-2">
                                    <asp:TextBox runat="server" ID="txtf5seqno" CssClass="form-control" MaxLength="2"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                            <div class="col-sm-2">
                                <label class="form-label" >Field Name</label>
                            </div>
                            <div class="col-sm-2">
                                <asp:TextBox runat="server" ID="txtfieldnamef5" class="form-control"></asp:TextBox>
                            </div>
                            
                            <div class="col-sm-2">
                                <label class="form-label">Lable Caption</label>
                            </div>

                            <div class="col-sm-2">
                                <asp:TextBox runat="server" ID="txtf5lablecaption" class="form-control"></asp:TextBox>
                            </div>
                           <div class="col-sm-2">
                               <label class="form-label">Control Type</label>
                           </div>
                            <div class="col-sm-2">
                                <asp:DropDownList runat="server" ID="ddlcontroltypef5" CssClass="form-control" OnSelectedIndexChanged="ddlcontroltypef5_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="TextBox"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="DropDown"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="CheckBox"></asp:ListItem>
                                    <asp:ListItem Value="4" Text="RadioButton"></asp:ListItem>
                                    <asp:ListItem Value="5" Text="DatePicker"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                                </div>
                            <div class="row pt-1  border rounded shadow mt-3" ID="ddlshow5" runat="server" visible="false">
                                <div class="col-sm-6 ">
                                    <div class="row">
                                        <div class="col-sm-1 mt-2"></div>
                                        <asp:RadioButton runat="server"  id="rdlf5a"  GroupName="A" style="margin-left: -15px;"/>
                                        <div class="col-sm-4">
                                            <label> DropDown From Table </label>
                                        </div>
                                        <div class="col-sm-8 mt-2">
                                            <asp:TextBox runat="server" ID="txtddlfromtablef5" class="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-4 mt-1">
                                            <label class="form-label"> DropDown Value Field</label>
                                        </div>
                                        <div class="col-sm-8 mt-2">
                                            <asp:TextBox runat="server" ID="txtddlvaluefieldf5" class="form-control"></asp:TextBox>
                                         </div>
                                        <div class="col-sm-4 mt-2">
                                            <label class="form-label"> DropDown Text Field</label>
                                        </div>
                                        <div class="col-sm-8 mt-2">
                                            <asp:TextBox runat="server" ID="txtddltextfielf5" class="form-control"></asp:TextBox>
                                         </div>
                                    </div>
                                     
                                      <div class="row">
                                            <div class="col-sm-4 mt-2">
                                                <asp:TextBox runat="server" ReadOnly="true" CssClass="form-control" Text="Condition"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-8 mt-2">
                                                <asp:TextBox runat="server" CssClass="form-control" ID="txtconditionF5_rdla"></asp:TextBox>
                                            </div>


                                        </div>

                                        <div class ="row mt-1">
                                            <div class ="col-sm-8">
                                                <label class="form-label"> Condition Variable Name</label>
                                            </div>
                                            <div class="col-sm-4">
                                                <asp:TextBox runat="server" CssClass="form-control" ID="txtconditionvarNameF5_rdla" ></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="row mt-1">
                                            
                                              <div class ="col-sm-8">
                                                <label class="form-label"> Condition Variable From Field No</label>
                                            </div>
                                            <div class="col-sm-4">
                                                <asp:TextBox runat="server" CssClass="form-control" ID="txtconditionVarFieldNoF5_rdla" ></asp:TextBox>
                                            </div>
                                        </div>

                                         <div class="row">
                                            <div class="col-sm-12" style="align-content:center">
                                                <asp:Button runat="server" ID="btnSubmitF5_rdla" Text="Submit" class="btn btn-success mt-2 " OnClick="btnSubmitF5_rdla_Click" />
                                            </div>
                                    </div>
                                        <div class="row">
                                            <asp:GridView  runat="server" ID="grdF5_rdla" class="table  text-center table-sm  table-bordered mt-2" AutoGenerateColumns="false" OnRowCommand="grdF5_rdla_RowCommand" >
                                                <Columns>
                                                   
                                                    <asp:TemplateField HeaderText="S No">
                                                       <ItemTemplate>
                                                           <%#Container.DataItemIndex+1 %>
                                                       </ItemTemplate>
                                                   </asp:TemplateField>

                                                   <asp:TemplateField HeaderText="Variable Name">
                                                       <ItemTemplate>
                                                          <%#Eval("VariableName") %>
                                                       </ItemTemplate>
                                                   </asp:TemplateField>

                                                   <asp:TemplateField HeaderText=" Variable Value Field No">
                                                       <ItemTemplate>
                                                           <%#Eval("VariableFieldNo") %>    
                                                       </ItemTemplate>
                                                   </asp:TemplateField>
                                                   <asp:TemplateField HeaderText="Delete">
                                                       <ItemTemplate>
                                                          <asp:Button runat="server" Text="Delete" ID="DltF5_rdla" CssClass="btn btn-warning btn-sm" CommandArgument=' <%#Container.DataItemIndex+1 %> '   CommandName="Delete" />
                                                       </ItemTemplate>
                                                   </asp:TemplateField>
                                               </Columns>
                                            </asp:GridView>
                                            </div>

                                </div>
                                <div class="col-sm-6">
                                    <asp:RadioButton runat="server" ID="rblf5b" GroupName="A" />
                                    <div class="row">
                                        <div class="col-sm-12 mt-2">
                                            <label class="form-label"> DropDown Populate Query</label>
                                        </div>
                                        <div class="col-sm-12 mt-2">
                                            <asp:TextBox runat="server" ID="txtddlpopulatequeryf5" class="form-control" ></asp:TextBox>
                                        </div>


                                         <div class="col-sm-4 mt-2">
                                            <label class="form-label"> DropDown Value Field</label>
                                        </div>
                                        <div class="col-sm-8 mt-2">
                                            <asp:TextBox runat="server" ID="txtddlvaluef5b" class="form-control"></asp:TextBox>
                                         </div>
                                        <div class="col-sm-4 mt-2">
                                            <label class="form-label"> DropDown Text Field</label>
                                        </div>
                                          <div class="col-sm-8 mt-2">
                                            <asp:TextBox runat="server" ID="txtddltextf5b" class="form-control"></asp:TextBox>
                                         </div>
                                    </div>
                                      <div class="row">
                                            <div class="col-sm-4 mt-2">
                                                <asp:TextBox runat="server" ReadOnly="true" CssClass="form-control" Text="Condition"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-8 mt-2">
                                                <asp:TextBox runat="server" CssClass="form-control" ID="txtconditionF5_rdlb"></asp:TextBox>
                                            </div>


                                        </div>

                                        <div class ="row mt-1">
                                            <div class ="col-sm-8">
                                                <label class="form-label"> Condition Variable Name</label>
                                            </div>
                                            <div class="col-sm-4">
                                                <asp:TextBox runat="server" CssClass="form-control" ID="txtconditionVarNameF5_rdlb" ></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="row mt-1">
                                            
                                              <div class ="col-sm-8">
                                                <label class="form-label"> Condition Variable From Field No</label>
                                            </div>
                                            <div class="col-sm-4">
                                                <asp:TextBox runat="server" CssClass="form-control" ID="txtconditionvarFieldNoF5_rdlb" ></asp:TextBox>
                                            </div>
                                        </div>

                                         <div class="row">
                                            <div class="col-sm-12" style="align-content:center">
                                                <asp:Button runat="server" ID="btnSubmitF5_rdlb" Text="Submit" class="btn btn-success mt-2 " OnClick="btnSubmitF5_rdlb_Click" />
                                            </div>
                                    </div>
                                        <div class="row">
                                            <asp:GridView  runat="server" ID="grfF5_rdlb" class="table  text-center table-sm  table-bordered mt-2" AutoGenerateColumns="false"  OnRowCommand="grfF5_rdlb_RowCommand">
                                                <Columns>
                                                   
                                                    <asp:TemplateField HeaderText="S No">
                                                       <ItemTemplate>
                                                           <%#Container.DataItemIndex+1 %>
                                                       </ItemTemplate>
                                                   </asp:TemplateField>

                                                   <asp:TemplateField HeaderText="Variable Name">
                                                       <ItemTemplate>
                                                          <%#Eval("VariableName") %>
                                                       </ItemTemplate>
                                                   </asp:TemplateField>

                                                   <asp:TemplateField HeaderText=" Variable Value Field No">
                                                       <ItemTemplate>
                                                           <%#Eval("VariableFieldNo") %>    
                                                       </ItemTemplate>
                                                   </asp:TemplateField>
                                                   <asp:TemplateField HeaderText="Delete">
                                                       <ItemTemplate>
                                                          <asp:Button runat="server" Text="Delete" ID="DltF5_rdlb" CssClass="btn btn-warning btn-sm" CommandArgument=' <%#Container.DataItemIndex+1 %> '   CommandName="Delete" />
                                                       </ItemTemplate>
                                                   </asp:TemplateField>
                                               </Columns>
                                            </asp:GridView>
                                            </div>
                                </div>
                              
                            </div>


                            <div runat="server" id="divradiobutton5" visible="false">  
                                <div class="row p-1  border rounded shadow mt-3">
                                  
                                     <div class="row">
                                       
                                        <div class="col-sm-2"></div>
                                        <div class="col-sm-2">
                                            <lable> Button Text</lable>
                                        </div>
                                        <div class="col-sm-1"></div>
                                        <div class="col-sm-1">
                                            <label>Value</label>
                                        </div>
                                         <div class="col-sm-2"></div>
                                        <div class="col-sm-2">
                                            <label>Default Select</label>
                                        </div>
                                    </div>

                                    <div class="row mt-3 px-4">

                                       
                                        <div class="col-sm-1">
                                            <label>1</label>
                                            <asp:CheckBox runat="server" ID="chkf5_chk1" class="form-check" />
                                        </div>
                                       
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtbtnchkf5_chk1" CssClass="form-control" ></asp:TextBox>
                                        </div>
                                       
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtvaluechkf5_chk1" CssClass="form-control"> </asp:TextBox>
                                        </div>
                                        

                                <div class="col-sm-1"></div>
                                        <div class="col-sm-1">
                                            <asp:RadioButton runat="server" ID="rblf5_l1" GroupName="F5A" />
                                            
                                        </div>
                                                                            </div>
                                    <div class="row mt-2 px-4">
                                       
                                        <div class="col-sm-1">
                                            <label>2</label>
                                            <asp:CheckBox runat="server" ID="chkf5_chk2" class="form-check" />
                                        </div>
                                    
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtbtnchkf5_chk2" CssClass="form-control" ></asp:TextBox>
                                        </div>
                                     
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtvaluechkf5_chk2" CssClass="form-control"> </asp:TextBox>
                                        </div>
                                        

                                    <div class="col-sm-1"></div>
                                        <div class="col-sm-1">
                                            <asp:RadioButton runat="server" ID="rblf5_l2" GroupName="F5A" />
                                            
                                        </div>
                                                                            </div>
                                    <div class="row mt-2 px-4">
                                       
                                        <div class="col-sm-1">
                                            <label>3</label>
                                            <asp:CheckBox runat="server" ID="chkf5_chk3" class="form-check" />
                                        </div>
                                      
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtbtnchkf5_chk3" CssClass="form-control" ></asp:TextBox>
                                        </div>
                                      
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtvalchkf5_chk3" CssClass="form-control"> </asp:TextBox>
                                        </div>
                                        

 <div class="col-sm-1"></div>
                                        <div class="col-sm-1">
                                            <asp:RadioButton runat="server" ID="rblf5_l3" GroupName="F5A" />
                                          
                                        </div>
                                                                            </div>
                                    <div class="row mt-2 px-4">
                                       
                                        <div class="col-sm-1">
                                            <label>4</label>
                                            <asp:CheckBox runat="server" ID="chkf5_chk4" class="form-check" />
                                        </div>
                                     
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtbtnchkf5_chk4" CssClass="form-control" ></asp:TextBox>
                                        </div>
                                      
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtvalchkf5_chk4" CssClass="form-control"> </asp:TextBox>
                                        </div>
                                        

 <div class="col-sm-1"></div>
                                        <div class="col-sm-1">
                                              <asp:RadioButton runat="server" ID="rblf5_l4" GroupName="F5A" />
                                            
                                        </div>
                                                                            </div>
                                    <div class="row mt-2 px-4">
                                       
                                        <div class="col-sm-1">
                                            <label>5</label>
                                            <asp:CheckBox runat="server" ID="chkf5_chk5" class="form-check" />
                                        </div>
                                       
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtbtnchkf5_chk5" CssClass="form-control" ></asp:TextBox>
                                        </div>
                                        
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtvaluechkf5_chk5" CssClass="form-control"> </asp:TextBox>
                                        </div>
                                        

 <div class="col-sm-1"></div>
                                        <div class="col-sm-1">
                                             <asp:RadioButton runat="server" ID="rblf5_l5" GroupName="F5A" />
                                           
                                        </div>
                                                                            </div>
                                    <div class="row mt-2 px-4">
                                       
                                        <div class="col-sm-1">
                                            <label>6</label>
                                            <asp:CheckBox runat="server" ID="chkf5_chk6" class="form-check" />
                                        </div>
                                      
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtbtnchkf5_chk6" CssClass="form-control" ></asp:TextBox>
                                        </div>
                                       
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtvaluechkf5_chk6" CssClass="form-control"> </asp:TextBox>
                                        </div>
                                        

 <div class="col-sm-1"></div>
                                        <div class="col-sm-1">
                                              <asp:RadioButton runat="server" ID="rblf5_l6" GroupName="F5A" />
                                           
                                        </div>
                                                                            </div>
                                </div>
                            </div>

                        </div>
                            </div>
                        <!---------fifth-field End------>





                          <!---------six-field Start------>
                         <div runat="server" ID="div6" visible="false">

                        <div class="row pt-1">
                            <label class="form-label"> <b>6st Field </b></label>
                               <div class="row">
                                <div class="col-sm-2">
                                    <lable class="form-label"> Field SeqNo</lable>
                                </div>
                                <div class="col-sm-2">
                                    <asp:TextBox runat="server" ID="txtf6seqno" CssClass="form-control" MaxLength="2"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row"> 
                            <div class="col-sm-2">
                                <label class="form-label" >Field Name</label>
                            </div>
                            <div class="col-sm-2">
                                <asp:TextBox runat="server" ID="txtfieldnamef6" class="form-control"></asp:TextBox>
                            </div>
                            
                            <div class="col-sm-2">
                                <label class="form-label">Lable Caption</label>
                            </div>

                            <div class="col-sm-2">
                                <asp:TextBox runat="server" ID="txtlablecaptionf6" class="form-control"></asp:TextBox>
                            </div>
                           <div class="col-sm-2">
                               <label class="form-label">Control Type</label>
                           </div>
                            <div class="col-sm-2">
                                <asp:DropDownList runat="server" ID="ddlcontroltypef6" CssClass="form-control" OnSelectedIndexChanged="ddlcontroltypef6_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="TextBox"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="DropDown"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="CheckBox"></asp:ListItem>
                                    <asp:ListItem Value="4" Text="RadioButton"></asp:ListItem>
                                    <asp:ListItem Value="5" Text="DatePicker"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                                </div>
                            <div class="row pt-1  border rounded shadow mt-3" id="ddlshow6" runat="server" visible="false">
                                <div class="col-sm-6 ">
                                    <div class="row">
                                        <div class="col-sm-1"></div>
                                        <asp:RadioButton runat="server"  id="rblf6a"  GroupName="A" style="margin-left: -15px;"/>
                                        <div class="col-sm-4">
                                            <label> DropDown From Table </label>
                                        </div>
                                        <div class="col-sm-8">
                                            <asp:TextBox runat="server" ID="txtddlf6fromtable" class="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-4">
                                            <label class="form-label"> DropDown Value Field</label>
                                        </div>
                                        <div class="col-sm-8">
                                            <asp:TextBox runat="server" ID="txtddlvaluefieldf6" class="form-control"></asp:TextBox>
                                         </div>
                                        <div class="col-sm-4">
                                            <label class="form-label"> DropDown Text Field</label>
                                        </div>
                                        <div class="col-sm-8">
                                            <asp:TextBox runat="server" ID="txtddltextfieldf6" class="form-control"></asp:TextBox>
                                         </div>
                                    </div>
                                      <div class="row">
                                            <div class="col-sm-4 mt-2">
                                                <asp:TextBox runat="server" ReadOnly="true" CssClass="form-control" Text="Condition"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-8 mt-2">
                                                <asp:TextBox runat="server" CssClass="form-control" ID="txtconditionF6_rdla"></asp:TextBox>
                                            </div>


                                        </div>

                                        <div class ="row mt-1">
                                            <div class ="col-sm-8">
                                                <label class="form-label"> Condition Variable Name</label>
                                            </div>
                                            <div class="col-sm-4">
                                                <asp:TextBox runat="server" CssClass="form-control" ID="txtcomndtitionvarNameF6_rdla" ></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="row mt-1">
                                            
                                              <div class ="col-sm-8">
                                                <label class="form-label"> Condition Variable From Field No</label>
                                            </div>
                                            <div class="col-sm-4">
                                                <asp:TextBox runat="server" CssClass="form-control" ID="txtcomndtitionvarFieldNoF6_rdla" ></asp:TextBox>
                                            </div>
                                        </div>

                                         <div class="row">
                                            <div class="col-sm-12" style="align-content:center">
                                                <asp:Button runat="server" ID="btnsubmitF6_rdla" Text="Submit" class="btn btn-success mt-2 " OnClick="btnsubmitF6_rdla_Click" />
                                            </div>
                                    </div>
                                        <div class="row">
                                            <asp:GridView  runat="server" ID="grdF6_rdla" class="table  text-center table-sm  table-bordered mt-2" AutoGenerateColumns="false" OnRowCommand="grdF6_rdla_RowCommand" >
                                                <Columns>
                                                   
                                                    <asp:TemplateField HeaderText="S No">
                                                       <ItemTemplate>
                                                           <%#Container.DataItemIndex+1 %>
                                                       </ItemTemplate>
                                                   </asp:TemplateField>

                                                   <asp:TemplateField HeaderText="Variable Name">
                                                       <ItemTemplate>
                                                          <%#Eval("VariableName") %>
                                                       </ItemTemplate>
                                                   </asp:TemplateField>

                                                   <asp:TemplateField HeaderText=" Variable Value Field No">
                                                       <ItemTemplate>
                                                           <%#Eval("VariableFieldNo") %>    
                                                       </ItemTemplate>
                                                   </asp:TemplateField>
                                                   <asp:TemplateField HeaderText="Delete">
                                                       <ItemTemplate>
                                                          <asp:Button runat="server" Text="Delete" ID="dltf2_rdla" CssClass="btn btn-warning btn-sm" CommandArgument=' <%#Container.DataItemIndex+1 %> '   CommandName="Delete" />
                                                       </ItemTemplate>
                                                   </asp:TemplateField>
                                               </Columns>
                                            </asp:GridView>
                                            </div>
                                     

                                </div>
                                <div class="col-sm-6">
                                    <asp:RadioButton runat="server" ID="rblf6b" GroupName="A" />
                                    <div class="row">
                                        <div class="col-sm-12">
                                            <label class="form-label"> DropDown Populate Query</label>
                                        </div>
                                        <div class="col-sm-12">
                                            <asp:TextBox runat="server" ID="txtddlpopulatequeryf6" class="form-control" ></asp:TextBox>
                                        </div>
                                         <div class="col-sm-4 mt-2">
                                            <label class="form-label"> DropDown Value Field</label>
                                        </div>
                                        <div class="col-sm-8 mt-2">
                                            <asp:TextBox runat="server" ID="txtddlvaluef6b" class="form-control"></asp:TextBox>
                                         </div>
                                        <div class="col-sm-4 mt-2">
                                            <label class="form-label"> DropDown Text Field</label>
                                        </div>
                                          <div class="col-sm-8 mt-2">
                                            <asp:TextBox runat="server" ID="txtddltextf6b" class="form-control"></asp:TextBox>
                                         </div>
                                    </div>

                                      <div class="row">
                                            <div class="col-sm-4 mt-2">
                                                <asp:TextBox runat="server" ReadOnly="true" CssClass="form-control" Text="Condition"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-8 mt-2">
                                                <asp:TextBox runat="server" CssClass="form-control" ID="txtconditionf6_rdlb"></asp:TextBox>
                                            </div>


                                        </div>

                                        <div class ="row mt-1">
                                            <div class ="col-sm-8">
                                                <label class="form-label"> Condition Variable Name</label>
                                            </div>
                                            <div class="col-sm-4">
                                                <asp:TextBox runat="server" CssClass="form-control" ID="txtconditionvarNameF6_rdlb" ></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="row mt-1">
                                            
                                              <div class ="col-sm-8">
                                                <label class="form-label"> Condition Variable From Field No</label>
                                            </div>
                                            <div class="col-sm-4">
                                                <asp:TextBox runat="server" CssClass="form-control" ID="txtconditionvarFieldNoF6_rdlb" ></asp:TextBox>
                                            </div>
                                        </div>

                                         <div class="row">
                                            <div class="col-sm-12" style="align-content:center">
                                                <asp:Button runat="server" ID="btnsubmitF6_rdlb" Text="Submit" class="btn btn-success mt-2 " OnClick="btnsubmitF6_rdlb_Click" />
                                            </div>
                                    </div>
                                        <div class="row">
                                            <asp:GridView  runat="server" ID="grdF6_rdlb" class="table  text-center table-sm  table-bordered mt-2" AutoGenerateColumns="false" OnRowCommand="grdF6_rdlb_RowCommand" >
                                                <Columns>
                                                   
                                                    <asp:TemplateField HeaderText="S No">
                                                       <ItemTemplate>
                                                           <%#Container.DataItemIndex+1 %>
                                                       </ItemTemplate>
                                                   </asp:TemplateField>

                                                   <asp:TemplateField HeaderText="Variable Name">
                                                       <ItemTemplate>
                                                          <%#Eval("VariableName") %>
                                                       </ItemTemplate>
                                                   </asp:TemplateField>

                                                   <asp:TemplateField HeaderText=" Variable Value Field No">
                                                       <ItemTemplate>
                                                           <%#Eval("VariableFieldNo") %>    
                                                       </ItemTemplate>
                                                   </asp:TemplateField>
                                                   <asp:TemplateField HeaderText="Delete">
                                                       <ItemTemplate>
                                                          <asp:Button runat="server" Text="Delete" ID="dltf2_rdla" CssClass="btn btn-warning btn-sm" CommandArgument=' <%#Container.DataItemIndex+1 %> '   CommandName="Delete" />
                                                       </ItemTemplate>
                                                   </asp:TemplateField>
                                               </Columns>
                                            </asp:GridView>
                                            </div>
                                </div>
                              
                            </div>


                            <div runat="server" id="divradiobutton6" visible="false">
                                <div class="row p-1  border rounded shadow mt-3">
                                    <div class="row">
                                       
                                        <div class="col-sm-2"></div>
                                        <div class="col-sm-2">
                                            <lable> Button Text</lable>
                                        </div>
                                        <div class="col-sm-1"></div>
                                        <div class="col-sm-1">
                                            <label>Value</label>
                                        </div>
                                         <div class="col-sm-2"></div>
                                        <div class="col-sm-2">
                                            <label>Default Select</label>
                                        </div>
                                    </div>
                                    <div class="row mt-3 px-4">
                                       
                                        <div class="col-sm-1">
                                            <label>1</label>
                                            <asp:CheckBox runat="server" ID="chkf6_chk1" class="form-check" />
                                        </div>
                                      
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtbtnchkf6_chk1" CssClass="form-control" ></asp:TextBox>
                                        </div>
                                       
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtvaluechkf6_chk1" CssClass="form-control"> </asp:TextBox>
                                        </div>
                                        

 <div class="col-sm-1"></div>
                                        <div class="col-sm-1">
                                               <asp:RadioButton runat="server" ID="rblf6_l1" GroupName="F6A" />
                                           
                                        </div>
                                                                            </div>
                                    <div class="row mt-2 px-4">
                                       
                                        <div class="col-sm-1">
                                            <label>2</label>
                                            <asp:CheckBox runat="server" ID="chkf6_chk2" class="form-check" />
                                        </div>
                                        
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtbtnchkf6_chk2" CssClass="form-control" ></asp:TextBox>
                                        </div>
                                        
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtvaluechkf6_chk2" CssClass="form-control"> </asp:TextBox>
                                        </div>
                                        

 <div class="col-sm-1"></div>
                                        <div class="col-sm-1">
                                                <asp:RadioButton runat="server" ID="rblf6_l2" GroupName="F6A" />
                                           
                                        </div>
                                                                            </div>
                                    <div class="row mt-2 px-4">
                                       
                                        <div class="col-sm-1">
                                            <label>3</label>
                                            <asp:CheckBox runat="server" ID="chkf6_chk3" class="form-check" />
                                        </div>
                                       
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtbtnchkf6_chk3" CssClass="form-control" ></asp:TextBox>
                                        </div>
                                       
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtvaluechkf6_chk3" CssClass="form-control"> </asp:TextBox>
                                        </div>
                                        

 <div class="col-sm-1"></div>
                                        <div class="col-sm-1">
                                                 <asp:RadioButton runat="server" ID="rblf6_l3" GroupName="F6A" />
                                           
                                        </div>
                                                                            </div>
                                    <div class="row mt-2 px-4">
                                       
                                        <div class="col-sm-1">
                                            <label>4</label>
                                            <asp:CheckBox runat="server" ID="chkf6_chk4" class="form-check" />
                                        </div>
                                       
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtbtnchkf6_chk4" CssClass="form-control" ></asp:TextBox>
                                        </div>
                                        
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtvaluechkf6_chk4" CssClass="form-control"> </asp:TextBox>
                                        </div>
                                        

 <div class="col-sm-1"></div>
                                        <div class="col-sm-1">
                                                  <asp:RadioButton runat="server" ID="rblf6_l4" GroupName="F6A" />
                                            
                                        </div>
                                                                            </div>
                                    <div class="row mt-2 px-4">
                                       
                                        <div class="col-sm-1">
                                            <label>5</label>
                                            <asp:CheckBox runat="server" ID="chkf6_chk5" class="form-check" />
                                        </div>
                                       
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtbtnchkf6_chk5" CssClass="form-control" ></asp:TextBox>
                                        </div>
                                       
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtvaluechkf6_chk5" CssClass="form-control"> </asp:TextBox>
                                        </div>
                                        

 <div class="col-sm-1"></div>
                                        <div class="col-sm-1">
                                             <asp:RadioButton runat="server" ID="rblf6_l5" GroupName="F6A" />
                                           
                                        </div>
                                                                            </div>
                                    <div class="row mt-2 px-4">
                                       
                                        <div class="col-sm-1">
                                            <label>6</label>
                                            <asp:CheckBox runat="server" ID="chkf6_chk6" class="form-check" />
                                        </div>
                                       
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtbtnchkf6_chk6" CssClass="form-control" ></asp:TextBox>
                                        </div>
                                       
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtvaluechkf6_chk6" CssClass="form-control"> </asp:TextBox>
                                        </div>
                                        

 <div class="col-sm-1"></div>
                                        <div class="col-sm-1">
                                              <asp:RadioButton runat="server" ID="rblf6_l6" GroupName="F6A" />
                                            
                                        </div>
                                                                            </div>
                                </div>
                            </div>

                        </div>
                            </div>
                        <!---------six-field End------>
                    </div>


                       
                    <div class="row mt-4">
                        <div class="col-sm-4"></div>
                        <div class="col-sm-4">

                            <asp:Button runat="server" Text="Save"   class="btn btn-danger btn-sm" ID="btnsave"  style="align-content:center" OnClick="btnsave_Click" />
                        <asp:Button runat="server" Text="Reset"  class="btn btn-danger btn-sm" ID="btnreset"  style="align-content:center" />
                        </div>
                        <div class="col-sm-4"></div>
                        
                    </div>
                         </div><!---card End---->

                    
                </div>
            </div>
        </div>
    </main>
           </ContentTemplate>
        </asp:UpdatePanel>

</asp:Content>
