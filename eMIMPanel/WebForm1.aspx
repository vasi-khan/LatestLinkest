<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="eMIMPanel.WebForm1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <%-- <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.3.7/css/bootstrap.min.css">--%>
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <html>
    <head>
        <title>Using Select2</title>
        <!-- Bootstrap CSS -->

    </head>
    <body>
        <div class="jumbotron">
            <div class="container bg-danger">
                <div class="col-md-6">
                    <label>Single Select2</label>
                    <div class="form-group row" id="divTempId" runat="server">
                        <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">Template ID</label>
                        <div class="col-sm-10">
                            <div id="div8" runat="server" style="pointer-events: all;">
                                <div class="row">
                                    <div class="col-lg-10">
                                        <asp:DropDownList ID="ddlTempID" runat="server" ClientIDMode="Static" class="custom-select" AutoPostBack="true"></asp:DropDownList>
                                    </div>

                                </div>
                            </div>

                            <div class="d-flex justify-content-start">
                                <p class="mb-1 mr-5">
                                    <span class="font-weight-bold">Template SMS: </span>
                                    <asp:Label ID="tt55" runat="server" Text=""></asp:Label>
                                </p>
                            </div>
                        </div>
                    </div>
                    <select id="single" class="js-states form-control">
                        <option>Java</option>
                        <option>Javascript</option>
                        <option>PHP</option>
                        <option>Visual Basic</option>
                    </select>
                </div>
            </div>
        </div>
        <!-- Select2 CSS -->
        <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/css/select2.min.css" rel="stylesheet" />
        <!-- jQuery -->
        <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
        <!-- Select2 -->
        <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/js/select2.min.js"></script>
        <script>
            $("#ddlTempID").select2({                 
                allowClear: true
            });
        </script>
    </body>
    </html>
</asp:Content>
