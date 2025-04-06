<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" ValidateRequest="false" AutoEventWireup="true" Debug="true" CodeBehind="AddTemplateAdmin.aspx.cs" Inherits="eMIMPanel.AddTemplateAdmin" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<title>MIM Add New Template</title>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="scrmg" runat="server" EnablePageMethods="true">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="container-fluid">
                <nav aria-label="breadcrumb" class="my-3">
                    <ol class="breadcrumb breadcrumb-info">
                        <li class="breadcrumb-item"><a href="#">Home</a></li>
                        <li class="breadcrumb-item active" aria-current="page">Add Template</li>
                    </ol>
                </nav>
                <!-- Content Row -->

                <div class="row">
                    <!-- Area Chart -->
                    <div class="col-xl-12 col-lg-12">
                        <!-- Basic Card Example -->
                        <div class="card bg-primary border-light shadow-soft mb-4">
                            <div class="card-header py-3 bg-primary">
                                <div class="row">
                                    <div class="col-md-10">
                                        <h6 class="m-0 font-weight-bold"><i class="far fa-user-circle"></i>Add New Template
                                     <asp:LinkButton class="btn btn-primary btn-icon-split" ID="lnkbtnBoth" ToolTip="Click here for Add template in Panel and API." runat="server" PostBackUrl="~/AddTemplateAdminNew.aspx">
                                           <span class="text-success font-weight-bold">for Both</span>
                                            <span class="text-success"><i class="fas fa-plus"></i></span>                                           
                                     </asp:LinkButton>
                                        </h6>
                                    </div>
                                </div>
                            </div>
                            <div class="card-body px-4">
                                <asp:Panel ID="pnlMain" runat="server">
                                    <div class="form-row">
                                        <div class="col-md-6 mb-6">
                                            <asp:RadioButtonList ID="rblType" runat="server" AutoPostBack="true" RepeatDirection="Horizontal" OnSelectedIndexChanged="rblType_SelectedIndexChanged">
                                                <asp:ListItem Text="API" Value="API"></asp:ListItem>
                                                <asp:ListItem Text="SMS PANEL" Value="PANEL" Selected="True"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                        <div class="col-md-3 mb-3" id="divSenderId" runat="server">
                                            <asp:TextBox class="form-control" ID="txtSenderId" runat="server" placeholder="Sender Id" ToolTip="Sender Id" />
                                            <div class="valid-feedback">
                                                Looks good!                             
                                            </div>
                                        </div>
                                        <div class="col-md-3 mb-3" id="divUser" runat="server">
                                            <asp:TextBox class="form-control" ID="txtUser" runat="server" placeholder="User Id" ToolTip="User Id" />
                                            <div class="valid-feedback">
                                                Looks good!                             
                                            </div>
                                        </div>
                                        <div class="col-md-3 mb-3" style="text-align: right;">
                                            <asp:LinkButton class="btn btn-primary btn-icon-split" ID="lnkShow" runat="server" OnClick="lnkShow_Click">
                                            <span class="text-success"><i class="fas fa-eye"></i></span>
                                            <span class="text-success font-weight-bold">Show</span>
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                    <!-- New Row-->
                                    <div class="card bg-primary shadow-inset border-light mb-3">
                                        <div class="card-body p-3">
                                            <div class="formrepeat  ">
                                                <div class="form-row">
                                                    <div class="col-md-5 mb-3">
                                                        <asp:TextBox class="form-control" ID="txtTempId" runat="server" placeholder="Template Id" ToolTip="Template Id" />
                                                        <div class="valid-feedback">
                                                            Looks good!                             
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6 mb-3">
                                                        <asp:TextBox class="form-control" ID="txtTempName" runat="server" placeholder="Template Name" ToolTip="Template Name" />
                                                        <div class="valid-feedback">
                                                            Looks good!                             
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-row">
                                                    <div class="col-md-11 mb-12">
                                                        <asp:TextBox class="form-control" ID="txtTemplateContent" runat="server" placeholder="Template Content" TextMode="MultiLine" Rows="5" ToolTip="Template Content" />
                                                        <div class="valid-feedback">
                                                            Looks good!                             
                                                        </div>
                                                    </div>

                                                    <div class="col-md-1 text-center ">
                                                        <button class="input-group-text" for="inputGroupFile02" onclick="myFunctionx()"><i class="fa fa-plus"></i></button>

                                                    </div>



                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- New Row-->


                                    <!-- New Row-->

                                    <div class="card bg-primary shadow-inset border-light mb-3 r1 " id="Userx1">
                                        <div class="card-body p-3">
                                            <div class="formrepeat card ">
                                                <div >
                                                    <div class="form-row">
                                                        <div class="col-md-5 mb-3">
                                                            <asp:TextBox class="form-control" ID="txtTempId1" runat="server" placeholder="Template Id" ToolTip="Template Id" />
                                                            <div class="valid-feedback">
                                                                Looks good!                             
                                                            </div>
                                                        </div>
                                                        <div class="col-md-6 mb-3">
                                                            <asp:TextBox class="form-control" ID="txtTempName1" runat="server" placeholder="Template Name" ToolTip="Template Name" />
                                                            <div class="valid-feedback">
                                                                Looks good!                             
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="form-row">
                                                        <div class="col-md-11 mb-12">
                                                            <asp:TextBox class="form-control" ID="txtTemplateContent1" runat="server" placeholder="Template Content" TextMode="MultiLine" Rows="5" ToolTip="Template Content" />
                                                            <div class="valid-feedback">
                                                                .  Looks good!                             
                                                            </div>
                                                        </div>
                                                        <div class="col-md-1 text-center ">
                                                            <button class="input-group-text" for="inputGroupFile02" onclick="myFunctionx1()"><i class="fa fa-plus"></i></button>

                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                    <!-- New Row-->
                                    <div class="card bg-primary shadow-inset border-light mb-3 r2" id="Userx2">
                                        <div class="card-body p-3">
                                            <div class="formrepeat card ">
                                                <!-- New Row-->
                                                <div  >
                                                    <div class="form-row">
                                                        <div class="col-md-5 mb-3">
                                                            <asp:TextBox class="form-control" ID="txtTempId2" runat="server" placeholder="Template Id" ToolTip="Template Id" />
                                                            <div class="valid-feedback">
                                                                Looks good!                             
                                                            </div>
                                                        </div>
                                                        <div class="col-md-6 mb-3">
                                                            <asp:TextBox class="form-control" ID="txtTempName2" runat="server" placeholder="Template Name" ToolTip="Template Name" />
                                                            <div class="valid-feedback">
                                                                Looks good!                             
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="form-row">
                                                        <div class="col-md-11 mb-12">
                                                            <asp:TextBox class="form-control" ID="txtTemplateContent2" runat="server" placeholder="Template Content" TextMode="MultiLine" Rows="5" ToolTip="Template Content" />
                                                            <div class="valid-feedback">
                                                                Looks good!                             
                                                            </div>
                                                        </div>
                                                        <div class="col-md-1 text-center ">
                                                            <button class="input-group-text" for="inputGroupFile02" onclick="myFunctionx2()"><i class="fa fa-plus"></i></button>

                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- New Row-->
                                    <div class="card bg-primary shadow-inset border-light mb-3 r3" id="Userx3">
                                        <div class="card-body p-3">
                                            <!-- New Row-->
                                            <!-- New Row-->
                                            <div  >
                                                <div class="form-row">
                                                    <div class="col-md-5 mb-3">
                                                        <asp:TextBox class="form-control" ID="txtTempId3" runat="server" placeholder="Template Id" ToolTip="Template Id" />
                                                        <div class="valid-feedback">
                                                            Looks good!                             
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6 mb-3">
                                                        <asp:TextBox class="form-control" ID="txtTempName3" runat="server" placeholder="Template Name" ToolTip="Template Name" />
                                                        <div class="valid-feedback">
                                                            Looks good!                             
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="form-row">
                                                    <div class="col-md-11 mb-12">
                                                        <asp:TextBox class="form-control" ID="txtTemplateContent3" runat="server" placeholder="Template Content" TextMode="MultiLine" Rows="5" ToolTip="Template Content" />
                                                        <div class="valid-feedback">
                                                            Looks good!                             
                                                        </div>
                                                    </div>
                                                    <div class="col-md-1 text-center ">
                                                        <button class="input-group-text" for="inputGroupFile02" onclick="myFunctionx3()"><i class="fa fa-plus"></i></button>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- New Row-->

                                    <!-- New Row-->

                                    <!-- New Row-->
                                    <!-- New Row-->

                                    <div class="card bg-primary shadow-inset border-light mb-3 r4" id="Userx4">
                                        <div class="card-body p-3" >
                                            <div >
                                                <div class="form-row">
                                                    <div class="col-md-5 mb-3">
                                                        <asp:TextBox class="form-control" ID="txtTempId4" runat="server" placeholder="Template Id" ToolTip="Template Id" />
                                                        <div class="valid-feedback">
                                                            Looks good!                             
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6 mb-3">
                                                        <asp:TextBox class="form-control" ID="txtTempName4" runat="server" placeholder="Template Name" ToolTip="Template Name" />
                                                        <div class="valid-feedback">
                                                            Looks good!                             
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="form-row">
                                                    <div class="col-md-11 mb-12">
                                                        <asp:TextBox class="form-control" ID="txtTemplateContent4" runat="server" placeholder="Template Content" TextMode="MultiLine" Rows="5" ToolTip="Template Content" />
                                                        <div class="valid-feedback">
                                                            Looks good!                             
                                                        </div>
                                                    </div>
                                                    <div class="col-md-1 text-center ">
                                                        <button class="input-group-text" for="inputGroupFile02" onclick="myFunctionx4()"><i class="fa fa-plus"></i></button>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- New Row-->

                                    <!-- New Row-->


                                    <div class="card bg-primary shadow-inset border-light mb-3 r5" id="Userx5">
                                        <div class="card-body p-3">
                                            <div   >
                                                <div class="form-row">
                                                    <div class="col-md-5 mb-3">
                                                        <asp:TextBox class="form-control" ID="txtTempId5" runat="server" placeholder="Template Id" ToolTip="Template Id" />
                                                        <div class="valid-feedback">
                                                            Looks good!                             
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6 mb-3">
                                                        <asp:TextBox class="form-control" ID="txtTempName5" runat="server" placeholder="Template Name" ToolTip="Template Name" />
                                                        <div class="valid-feedback">
                                                            Looks good!                             
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="form-row">
                                                    <div class="col-md-11 mb-12">
                                                        <asp:TextBox class="form-control" ID="txtTemplateContent5" runat="server" placeholder="Template Content" TextMode="MultiLine" Rows="5" ToolTip="Template Content" />
                                                        <div class="valid-feedback">
                                                            Looks good!                             
                                                        </div>
                                                    </div>
                                                    <div class="col-md-1 text-center ">
                                                        <button class="input-group-text" for="inputGroupFile02" onclick="myFunctionx5()"><i class="fa fa-plus"></i></button>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- New Row-->

                                    <!-- New Row-->

                                    <!-- New Row-->


                                    <div class="card bg-primary shadow-inset border-light mb-3 r6"  id="Userx6">
                                        <div class="card-body p-3">

                                            <div >
                                                <div class="form-row">
                                                    <div class="col-md-5 mb-3">
                                                        <asp:TextBox class="form-control" ID="txtTempId6" runat="server" placeholder="Template Id" ToolTip="Template Id" />
                                                        <div class="valid-feedback">
                                                            Looks good!                             
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6 mb-3">
                                                        <asp:TextBox class="form-control" ID="txtTempName6" runat="server" placeholder="Template Name" ToolTip="Template Name" />
                                                        <div class="valid-feedback">
                                                            Looks good!                             
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="form-row">
                                                    <div class="col-md-11 mb-12">
                                                        <asp:TextBox class="form-control" ID="txtTemplateContent6" runat="server" placeholder="Template Content" TextMode="MultiLine" Rows="5" ToolTip="Template Content" />
                                                        <div class="valid-feedback">
                                                            Looks good!                             
                                                        </div>
                                                    </div>
                                                    <div class="col-md-1 text-center ">
                                                        <button class="input-group-text" for="inputGroupFile02" onclick="myFunctionx6()"><i class="fa fa-plus"></i></button>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- New Row-->

                                    <!-- New Row-->

                                    <!-- New Row-->


                                    <div class="card bg-primary shadow-inset border-light mb-3 r7" id="Userx7">
                                        <div class="card-body p-3">
                                            <div  >
                                                <div class="form-row">
                                                    <div class="col-md-5 mb-3">
                                                        <asp:TextBox class="form-control" ID="txtTempId7" runat="server" placeholder="Template Id" ToolTip="Template Id" />
                                                        <div class="valid-feedback">
                                                            Looks good!                             
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6 mb-3">
                                                        <asp:TextBox class="form-control" ID="txtTempName7" runat="server" placeholder="Template Name" ToolTip="Template Name" />
                                                        <div class="valid-feedback">
                                                            Looks good!                             
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="form-row">
                                                    <div class="col-md-11 mb-12">
                                                        <asp:TextBox class="form-control" ID="txtTemplateContent7" runat="server" placeholder="Template Content" TextMode="MultiLine" Rows="5" ToolTip="Template Content" />
                                                        <div class="valid-feedback">
                                                            Looks good!                             
                                                        </div>
                                                    </div>
                                                    <div class="col-md-1 text-center ">
                                                        <button class="input-group-text" for="inputGroupFile02" onclick="myFunctionx7()"><i class="fa fa-plus"></i></button>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- New Row-->

                                    <!-- New Row-->

                                    <!-- New Row-->

                                    <div class="card bg-primary shadow-inset border-light mb-3 r8" id="Userx8">
                                        <div class="card-body p-3">
                                            <div  >
                                                <div class="form-row">
                                                    <div class="col-md-5 mb-3">
                                                        <asp:TextBox class="form-control" ID="txtTempId8" runat="server" placeholder="Template Id" ToolTip="Template Id" />
                                                        <div class="valid-feedback">
                                                            Looks good!                             
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6 mb-3">
                                                        <asp:TextBox class="form-control" ID="txtTempName8" runat="server" placeholder="Template Name" ToolTip="Template Name" />
                                                        <div class="valid-feedback">
                                                            Looks good!                             
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="form-row">
                                                    <div class="col-md-11 mb-12">
                                                        <asp:TextBox class="form-control" ID="txtTemplateContent8" runat="server" placeholder="Template Content" TextMode="MultiLine" Rows="5" ToolTip="Template Content" />
                                                        <div class="valid-feedback">
                                                            Looks good!                             
                                                        </div>
                                                    </div>
                                                    <div class="col-md-1 text-center ">
                                                        <button class="input-group-text" for="inputGroupFile02" onclick="myFunctionx8()"><i class="fa fa-plus"></i></button>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- New Row-->


                                    <!-- New Row-->


                                    <div class="card bg-primary shadow-inset border-light mb-3 r9"  id="Userx9">
                                        <div class="card-body p-3">
                                            <div>
                                                <div class="form-row">
                                                    <div class="col-md-5 mb-3">
                                                        <asp:TextBox class="form-control" ID="txtTempId9" runat="server" placeholder="Template Id" ToolTip="Template Id" />
                                                        <div class="valid-feedback">
                                                            Looks good!                             
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6 mb-3">
                                                        <asp:TextBox class="form-control" ID="txtTempName9" runat="server" placeholder="Template Name" ToolTip="Template Name" />
                                                        <div class="valid-feedback">
                                                            Looks good!                             
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="form-row">
                                                    <div class="col-md-11 mb-12">
                                                        <asp:TextBox class="form-control" ID="txtTemplateContent9" runat="server" placeholder="Template Content" TextMode="MultiLine" Rows="5" ToolTip="Template Content" />
                                                        <div class="valid-feedback">
                                                            Looks good!                             
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- New Row-->
<!--
                                    <div class="form-row" id="divTempWord" runat="server">
                                        <div class="col-md-12 mb-12 mt-2">
                                            <asp:TextBox class="form-control" ID="txtTempWord" runat="server" placeholder="Template Phrase (separate with ; semi colon)" TextMode="MultiLine" Rows="5" ToolTip="Template Variable" />
                                            <div class="valid-feedback">
                                                Looks good!                             
                                            </div>
                                        </div>
-->
                                    </div>
                                    <%--<div class="form-row">
                                <div class="col-md-4 mb-12 mt-2">

                                    </div>
                            </div>--%>

                                    <div class="row  mt-3 gx-0">
                                        <div class="col-1 mt-2 p-0 pl-3">
                                            <asp:Label ID="lblusr1" runat="server" Text="UserId"></asp:Label>
                                        </div>
                                        <div class="col-3 p-0">
                                            <%--<input type="text" class="form-control">--%>
                                            <asp:TextBox ID="txtuserid1"  runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-1 text-center">
                                            <button class="input-group-text" for="inputGroupFile02" onclick="myFunction()">+</button>
                                        </div>
                                    </div>

                                    <div class=" row mt-2 display1" id="UserId1">
                                        <div class="col-1 mt-2 p-0 pl-3">
                                            <asp:Label ID="lblusr2" runat="server" Text="UserId"></asp:Label>
                                        </div>
                                        <div class="col-3 p-0">
                                            <asp:TextBox ID="txtuserid2" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-1 text-center ">
                                            <button class="input-group-text" for="inputGroupFile02" onclick="myFunction1()">+</button>

                                        </div>
                                    </div>

                                    <div class=" row mt-2 display2" id="UserId2">
                                        <div class="col-1 mt-2 p-0 pl-3">
                                            <asp:Label ID="lblusr3" runat="server" Text="UserId"></asp:Label>
                                        </div>
                                        <div class="col-3 p-0">
                                            <asp:TextBox ID="txtuserid3" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-1 text-center">
                                            <button class="input-group-text" for="inputGroupFile02" onclick="myFunction2()">+</button>

                                        </div>
                                    </div>

                                    <div class=" row mt-2 display3" id="UserId3">
                                        <div class="col-1 mt-2 p-0 pl-3">
                                            <asp:Label ID="lblusr4" runat="server" Text="UserId"></asp:Label>
                                        </div>
                                        <div class="col-3 p-0">
                                            <asp:TextBox ID="txtuserid4" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-1 text-center">
                                            <button class="input-group-text" for="inputGroupFile02" onclick="myFunction3()">+</button>

                                        </div>
                                    </div>

                                    <div class=" row mt-2 display4" id="UserId4">
                                        <div class="col-1 mt-2 p-0 pl-3">
                                            <asp:Label ID="lblusr5" runat="server" Text="UserId"></asp:Label>
                                        </div>
                                        <div class="col-3 p-0">
                                            <asp:TextBox ID="txtuserid5" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-1 text-center">
                                            <button class="input-group-text" for="inputGroupFile02" onclick="myFunction4()">+</button>

                                        </div>
                                    </div>

                                    <div class=" row mt-2 display5" id="UserId5">
                                        <div class="col-1 mt-2 p-0 pl-3">
                                            <asp:Label ID="lblusr6" runat="server" Text="UserId"></asp:Label>
                                        </div>
                                        <div class="col-3 p-0">
                                            <asp:TextBox ID="txtuserid6" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-1 text-center">
                                            <button class="input-group-text" for="inputGroupFile02" onclick="myFunction5()">+</button>

                                        </div>
                                    </div>

                                    <div class=" row mt-2 display6" id="UserId6">
                                        <div class="col-1 mt-2 p-0 pl-3">
                                            <asp:Label ID="lblusr7" runat="server" Text="UserId"></asp:Label>
                                        </div>
                                        <div class="col-3 p-0">
                                            <asp:TextBox ID="txtuserid7" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-1 text-center">
                                            <button class="input-group-text" for="inputGroupFile02" onclick="myFunction6()">+</button>

                                        </div>
                                    </div>

                                    <div class=" row mt-2 display7" id="UserId7">
                                        <div class="col-1 mt-2 p-0 pl-3">
                                            <asp:Label ID="lblusr8" runat="server" Text="UserId"></asp:Label>
                                        </div>
                                        <div class="col-3 p-0">
                                            <asp:TextBox ID="txtuserid8" runat="server" class="form-control "></asp:TextBox>
                                        </div>
                                        <div class="col-1 text-center">
                                            <button class="input-group-text" for="inssputGroupFile02" onclick="myFunction7()">+</button>

                                        </div>
                                    </div>

                                    <div class=" row px mt-2 display8" id="UserId8">
                                        <div class="col-1 mt-2 p-0 pl-3">
                                            <asp:Label ID="lblusr9" runat="server" Text="UserId"></asp:Label>
                                        </div>
                                        <div class="col-3 p-0">
                                            <asp:TextBox ID="txtuserid9" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-1 text-center">
                                            <button class="input-group-text" for="inssputGroupFile02" onclick="myFunction8()">+</button>

                                        </div>
                                    </div>

                                    <div class=" row mt-2 display9" id="UserId9">
                                        <div class="col-1 mt-2 p-0 pl-3">
                                            <asp:Label ID="lblusr10" runat="server" Text="UserId "></asp:Label>
                                        </div>
                                        <div class="col-3 p-0">
                                            <asp:TextBox ID="txtuserid10" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <%--  --%>



                                    <div class="form-row px-2 mt-3">
                                        <div class="col-md-10" style="text-align: left;">
                                            <asp:LinkButton class="btn btn-primary btn-icon-split  mt-2" ID="btnSave" runat="server" OnClick="btnSave_Click">
                                            <span class="text-success"><i class="fas fa-save"></i></span>
                                            <span class="text-success font-weight-bold">Save</span>
                                            </asp:LinkButton>
                                        </div>
                                        <div class="col-md-2" style="text-align: right;">
                                            <asp:LinkButton runat="server" ID="lnkDownload" OnClick="lnkDownload_Click" class="btn btn-primary btn-icon-split  mt-2">
                                                    Download <i class="fas fa-download" aria-hidden="true"></i>
                                            </asp:LinkButton>
                                        </div>















                                        <div class="col-12 p-2"></div>
                                        <div class="col-12 p-2" runat="server" id="filtershow" visible="false">
                                            <div class="col-12 p-2">
                                                <asp:Label ID="lblHeader" runat="server">FILTER TEMPLATE</asp:Label>
                                            </div>
                                            <div class="row">
                                                <div class="col-3 p-2">
                                                    <asp:TextBox ID="txtTemplateId" runat="server" class="form-control" Placeholder="Template Id"></asp:TextBox>
                                                </div>
                                                <div class="col-3 p-2">
                                                    <asp:TextBox ID="txtTemplateName" runat="server" class="form-control" Placeholder="Template Name"></asp:TextBox>
                                                    .    
                                                </div>
                                                <div class="col-3 p-2">
                                                    <asp:TextBox ID="txtTemplate" runat="server" class="form-control" Placeholder="Template Text"></asp:TextBox>
                                                </div>
                                                <div class="col-1 p-2">
                                                    <asp:Button class="btn btn-primary btn-icon-split" ID="BtnFilter" runat="server" Text="Filter" OnClick="BtnFilter_Click" />
                                                </div>
                                                <div class="col-1 p-2">
                                                    <asp:Button class="btn btn-primary btn-icon-split" ID="BtnReset" runat="server" Text="Reset" OnClick="BtnReset_Click" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="mt-5">
                                        <asp:GridView ID="grvTemplate" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
                                            runat="server" Width="100%" CellPadding="10"
                                            BorderColor="#ede8e8" Class="table table-striped table-bordered dt-responsive dataTable-view">
                                            <Columns>
                                                <asp:TemplateField HeaderStyle-Width="5%" HeaderText="Sr No">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="5%">
                                                    <HeaderTemplate>
                                                        <asp:CheckBox runat="server" ID="chkHeader" class="control-check" AutoPostBack="true" OnCheckedChanged="chkHeader_CheckedChanged" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox runat="server" ID="chkitem" class="control-check" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Template Id" HeaderStyle-Width="10%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTemplateId" runat="server" Width="100%" Text='<%#Eval("TemplateId")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Template Name And Create Date" HeaderStyle-Width="25%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTemplateName" runat="server" Text='<%#Eval("tempname")%>' Width="100%"></asp:Label>
                                                        <asp:Label ID="lblTemplateinsertdate" runat="server" Text='<%#Eval("insertdate")%>' Width="50%"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Template" HeaderStyle-Width="60%">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtTemplate" runat="server" TextMode="MultiLine" ReadOnly="true" Text='<%#Eval("Template")%>' Rows="3" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Action" HeaderStyle-Width="20%">

                                                    <HeaderTemplate>
                                                        Action
                                                <asp:LinkButton Text="Action" runat="server" ID="multileDelete" class="mx-1 btn btn-primary text-danger" OnClick="multileDelete_Click" OnClientClick="return Confirm();"> <span class="text-danger"> <i class="fas fa-trash"></i> </span></asp:LinkButton></asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>

                                                        <asp:LinkButton ID="lnkDelete" runat="server" class="mx-1 btn btn-primary text-danger"
                                                            OnClientClick="return confirm('Do you want to Remove Template ?');" OnClick="lnkDelete_Click"
                                                            data-toggle="tooltip" data-placement="top" title="" data-original-title="Declined"> 
                                                         <span class="text-danger"> <i class="fas fa-trash"></i> </span></asp:LinkButton>
                                                        <asp:LinkButton ID="lnkTest" runat="server" class="mx-1 btn btn-primary table-success"
                                                            OnClick="lnkTest_Click"
                                                            data-toggle="tooltip" data-placement="top" title="" data-original-title="Send SMS"> 
                                                         <span class="text-success"> <i class="fas fa-share"></i> </span></asp:LinkButton>
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
                                </asp:Panel>

                            </div>
                        </div>

                    </div>

                    <!-- Pie Chart -->
                    <div class="col-xl-4 col-lg-5"></div>
                </div>

                <%-- Template Test POPUP For API --%>
                <asp:Panel ID="pnlPopUp_NUMBER" runat="server" CssClass="modalPopup" Style="display: none;">
                    <div style="overflow-y: auto; overflow-x: hidden; max-height: 500px; width: 600px;">
                        <div class="modal-header">
                            <asp:Label ID="Label132" runat="server" CssClass="modal-title" Text="Send Message"></asp:Label>
                        </div>
                        <div class="modal-body">
                            <div>
                                <div class="form-row">
                                    <div class="col-md-6">
                                        <asp:TextBox class="form-control" ID="txtTestUser" runat="server" placeholder="User Name" ToolTip="User" />
                                    </div>
                                    <div class="col-md-6">
                                        <asp:TextBox class="form-control" ID="txtTestMobile" runat="server" placeholder="Mobile" ToolTip="Mobile" />
                                    </div>
                                </div>

                                <div class="form-row mt-2">
                                    <div class="col-md-6">
                                        <asp:TextBox class="form-control" ID="txtTestSender" ReadOnly="true" runat="server" placeholder="Sender Id" ToolTip="Sender Id" />
                                    </div>
                                </div>

                                <div class="form-row">
                                    <div class="col-md-12 mb-12 mt-2">
                                        <asp:TextBox class="form-control" ID="txtTestMessage" runat="server" placeholder="Message" TextMode="MultiLine" Rows="5" ToolTip="Message" />

                                    </div>
                                </div>

                                <div class="form-row mt-2">
                                    <div class="col-md-4">
                                        <asp:Label class="form-control" ID="lblTotalMessage" runat="server" />
                                    </div>
                                    <div class="col-md-8">
                                        <asp:Label class="form-control" ID="lblMessageLength" runat="server" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div align="center" class="modal-footer">
                            <div class="row">
                                <div class="col-md-6">
                                    <asp:Button ID="btnSend" runat="server" Text="Send" class="btn btn-primary" OnClick="btnSend_Click" />

                                </div>
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

                <%-- Template Test POPUP For SMS PANEL --%>

                <asp:Panel ID="PanelLinkext" runat="server" CssClass="modalPopup" Style="display: none;">
                    <div style="overflow-y: auto; overflow-x: hidden; max-height: 500px;">
                        <div class="modal-header">
                            <asp:Label ID="Label1" runat="server" CssClass="modal-title" Text="Send Message"></asp:Label>
                        </div>
                        <div class="modal-body">
                            <div>
                                <div class="form-row">
                                    <div class="col-md-6">
                                        <asp:TextBox class="form-control" ID="txtLinkUser" runat="server" placeholder="User Name" ToolTip="User" />
                                    </div>
                                    <div class="col-md-6">
                                        <asp:TextBox class="form-control" ID="txtLinkMobile" runat="server" placeholder="Mobile" ToolTip="Mobile" />
                                    </div>
                                </div>

                                <div class="form-row mt-2">
                                    <div class="col-md-6">
                                        <asp:DropDownList class="form-control" ID="ddlLinkrSender" runat="server" placeholder="Sender Id" />
                                    </div>
                                </div>

                                <div class="form-row">
                                    <div class="col-md-12 mb-12 mt-2">
                                        <asp:TextBox class="form-control" ID="txtLinkMessage" runat="server" placeholder="Message" TextMode="MultiLine" Rows="5" ToolTip="Message" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div align="center" class="modal-footer">
                            <div class="row">
                                <div class="col-md-6">
                                    <asp:Button ID="btnLinkSend" runat="server" Text="Send" class="btn btn-primary" OnClick="btnLinkSend_Click" />
                                </div>
                                <div class="col-md-6">
                                    <button id="Button2" runat="server" class="btn btn-primary">Cancel</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
                <asp:LinkButton ID="LinkButtonLinkext" runat="server"></asp:LinkButton>
                <asp:ModalPopupExtender ID="pnlPopUp_Linkext_ModalPopupExtender" runat="server" BackgroundCssClass="modalBackground"
                    PopupControlID="PanelLinkext" TargetControlID="LinkButtonLinkext" CancelControlID="Button2">
                </asp:ModalPopupExtender>

                <style type="text/css">
                    .r1 {
                        display: none;
                        
                    }

                    .r2 {
                        display: none;
                        
                    }

                    .r3 {
                        display: none;
                    }

                    .r4 {
                        display: none;
                    }

                    .r5 {
                        display: none;
                    }

                    .r6 {
                        display: none;
                    }

                    .r7 {
                        display: none;
                    }

                    .r8 {
                        display: none;
                    }

                    .r9 {
                        display: none;
                    }





                    .display1 {
                        display: none;
                    }

                    .display2 {
                        display: none;
                    }

                    .display3 {
                        display: none;
                    }

                    .display4 {
                        display: none;
                    }

                    .display5 {
                        display: none;
                    }

                    .display6 {
                        display: none;
                    }

                    .display7 {
                        display: none;
                    }

                    .display8 {
                        display: none;
                    }

                    .display9 {
                        display: none;
                    }

                    /*.display10 {
                display: none;
            }*/
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
                        box-shadow: 0 5px 10px rgba(0,
                    }


                    ed;
                    lef
                    b ckgro
                    o
                    z-index: 1800;
                    min-heig t: 100%;
                    : 100%;
                    r

                    ;
                    filter: alpha(opacity=50);
                    display: inline-block;
                    z-index: 1000;
                    }
                </style>
           
        </ContentTemplate>
    </asp:UpdatePanel>
    <script>
        //start

        $(document).ready(function () {
            $(".userx").keyup(function () {
                if ($(this).val().length >= 6) {
                    $(this).attr('disabled', 'disabled');
                }

            });
        });

        


        function myFunctionx() {
            // alert("inside functionx");
            document.getElementById("Userx1").classList.remove('r1');

            event.preventDefault();

        }
        function myFunctionx1() {
            //alert("Inside function x1");
            document.getElementById("Userx2").classList.remove('r2');

            event.preventDefault();
        }
        function myFunctionx2() {
            //alert("inside functionx");
            document.getElementById("Userx3").classList.remove('r3');

            event.preventDefault();

        }
        function myFunctionx3() {
            //alert("Inside function x1");
            document.getElementById("Userx4").classList.remove('r4');

            event.preventDefault();
        }
        function myFunctionx4() {
            // alert("inside functionx");
            document.getElementById("Userx5").classList.remove('r5');

            event.preventDefault();

        }
        function myFunctionx5() {
            // alert("Inside function x1");
            document.getElementById("Userx6").classList.remove('r6');

            event.preventDefault();
        }

        function myFunctionx6() {
            //alert("Inside function x1");
            document.getElementById("Userx7").classList.remove('r7');

            event.preventDefault();
        }
        function myFunctionx7() {
            //alert("Inside function x1");
            document.getElementById("Userx8").classList.remove('r8');

            event.preventDefault();
        }
        function myFunctionx8() {
            // alert("Inside function x1");
            document.getElementById("Userx9").classList.remove('r9');

            event.preventDefault();
        }





        //end


        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Do you want to Delete Template ID <%#Session["TotalTemplateIDdelete"]%>?")) {
                //confirm_value.value = "Yes";
                return true;
            } else {
                //confirm_value.value = "No";
                return false;
            }
            document.forms[0].appendChild(confirm_value);
        }



        function myFunction() {
            document.getElementById("UserId1").classList.remove('display1');
            event.preventDefault();

        }
        function myFunction1() {
            document.getElementById("UserId2").classList.remove('display2');
            event.preventDefault();
        }
        function myFunction2() {
            document.getElementById("UserId3").classList.remove('display3');
            event.preventDefault();

        }
        function myFunction3() {
            document.getElementById("UserId4").classList.remove('display4');
            event.preventDefault();
        }
        function myFunction4() {
            document.getElementById("UserId5").classList.remove('display5');
            event.preventDefault();

        }
        function myFunction5() {
            document.getElementById("UserId6").classList.remove('display6');
            event.preventDefault();
        }
        function myFunction6() {
            document.getElementById("UserId7").classList.remove('display7');
            event.preventDefault();

        }
        function myFunction7() {
            document.getElementById("UserId8").classList.remove('display8');
            event.preventDefault();
        }
        function myFunction8() {
            document.getElementById("UserId9").classList.remove('display9');
            event.preventDefault();

        }




    </script>
</asp:Content>
