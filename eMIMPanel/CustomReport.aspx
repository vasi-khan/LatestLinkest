<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="CustomReport.aspx.cs" Inherits="eMIMPanel.CustomReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <style>
    .form-check .form-check-input {
    float: left;
    margin-left: 0em;
}
    /*.form-control {
    display: block;
    width: 100%;
    height: calc(1.5em + 1.2rem + 0.0625rem);
    padding: 0.6rem 0.75rem;
    font-size: 1rem;
    font-weight: 300;
    line-height: 1.5;
   
    box-shadow: inset 2px 2px 5px #b8b9be, inset -3px -3px 7px #ffffff;
    transition: all 0.3s ease-in-out;
}*/
.form-control-sm {

    padding: 0.6rem 0.75rem;
    height: calc(1.5em + 1.2rem + 0.0625rem);
   
   color: #44476A;
    background-color: #e6e7ee;
    background-clip: padding-box;
    border: 0.0625rem solid #D1D9E6;
    border-radius: 0.55rem;
     box-shadow: inset 2px 2px 5px #b8b9be, inset -3px -3px 7px #ffffff;
}
.form-control-sm:focus {
 
    border: 1px solid #eae8e8;

}
</style>

    <main>
         <nav aria-label="breadcrumb" class="my-3">
                <ol class="breadcrumb breadcrumb-info">
                    <li class="breadcrumb-item"><a href="#">Home</a></li>
                    <li class="breadcrumb-item active" aria-current="page">Custom Report</li>
                </ol>
            </nav>
        <div class="row">
          
            <div class="col-sm-12 col-lg-12">
                <div class="card nav-pills2">
             
                     <div class="card-body">
                       <div>
                           <div class="row">
                           <div class="col-sm-5">
                             <div class="form-group">
                                 <label class="form-label" for="exampleInputText1">Report Name </label>
                                 <asp:TextBox runat="server" ID="txtreportname"  class="form-control" placeholder="Enter Name" MaxLength="30"></asp:TextBox>
                                <%-- <input type="name" class="form-control" id="exampleInputText1" placeholder="Enter Name">--%>
                             </div>
                         </div>
                         <div class="col-sm-5">
                            <div class="form-group">
                                <label class="form-label" for="exampleInputText1">Procedure Name </label>
                                  <asp:TextBox runat="server" ID="txtprocedurename"  class="form-control" placeholder="Enter Name" MaxLength="40"></asp:TextBox>
                            <%--    <input type="name" class="form-control" id="exampleInputText1" placeholder="Enter Name">--%>
                            </div>
                        </div>
        </div>

    <div class="row">
        <div class="col-md-4">
    
        <div class="form-group" style="margin-top:22px">
            <div class="form-group">
                <asp:CheckBox runat="server" ID="chk1"  ></asp:CheckBox> 
               <%-- <input type="checkbox" value="" id="flexCheckDefault">--%>
                <label class="form-check-label" for="flexCheckDefault">
                    FilterName1
                </label>
                <asp:TextBox runat="server" ID="txtFN1" class=" form-control-sm" placeholder="Enter Name"></asp:TextBox>
             <%--   <input type="name" class="form-control-sm "  placeholder="Enter Name">--%>
            </div>
        </div>
        </div>
        <div class="col-md-3 position-relative">
          <label for="validationTooltip04" class="form-label">Type </label>
        
            <asp:DropDownList runat="server" class="custom-select" ID="ddltype1">
                <asp:ListItem value="0" Text="Choose--"></asp:ListItem>
                <asp:ListItem value="1" Text="Varchar(1000)"></asp:ListItem>
                <asp:ListItem value="2" Text="Nvarchar(1000)"></asp:ListItem>
                <asp:ListItem value="3" Text="DateTime"></asp:ListItem>
                <asp:ListItem value="4" Text="Bit"></asp:ListItem>
            </asp:DropDownList>
          <div class="invalid-tooltip">
             Please select a valid state.
          </div>
        </div>
    
    
        <div class="col-sm-3">
          <div class="form-group">
              <label class="form-label" for="exampleInputText1">Control Label Text        </label>
            <%--  <input type="name" class="form-control" id="exampleInputText1" placeholder="Enter Name">--%>
              <asp:TextBox runat="server" class="form-control" ID="CLT1" placeholder="Enter Name"  ></asp:TextBox>
          </div>
        </div>
        </div>
        <div class="row">
            <div class="col-md-4">
        
            <div class="form-group" style="margin-top:22px">
                <div class="form-group">
                    <asp:CheckBox runat="server" ID="chk2"  ></asp:CheckBox> 
               
                    <label class="form-check-label" for="flexCheckDefault">
                     FilterName2
                    </label>
                   <asp:TextBox runat="server" ID="txtFN2" class="form-control-sm" placeholder="Enter Name"></asp:TextBox>
                </div>
            </div>
            </div>
            <div class="col-md-3 position-relative">
              <label for="validationTooltip04" class="form-label">Type </label>
             <asp:DropDownList runat="server" class="custom-select" ID="ddltype2">
                <asp:ListItem value="0" Text="Choose--"></asp:ListItem>
                <asp:ListItem value="1" Text="Varchar(1000)"></asp:ListItem>
                <asp:ListItem value="2" Text="Nvarchar(1000)"></asp:ListItem>
                <asp:ListItem value="3" Text="DateTime"></asp:ListItem>
                <asp:ListItem value="4" Text="Bit"></asp:ListItem>
            </asp:DropDownList>
              <div class="invalid-tooltip">
                 Please select a valid state.
              </div>
            </div>
        
        
            <div class="col-sm-3">
              <div class="form-group">
                  <label class="form-label" for="exampleInputText1">Control Label Text        </label>
                   <asp:TextBox runat="server" class="form-control" ID="CLT2" placeholder="Enter Name"  ></asp:TextBox>
              </div>
            </div>
            </div>
            <div class="row">
                <div class="col-md-4">
            
                <div class="form-group" style="margin-top:22px">
                    <div class="form-group">
                           <asp:CheckBox runat="server" ID="chk3"  ></asp:CheckBox> 
                        <label class="form-check-label" for="flexCheckDefault">
                            FilterName3
                        </label>
                        <asp:TextBox runat="server" class=" form-control-sm" ID="txtFN3" placeholder="Enter Name"  ></asp:TextBox>
                    </div>
                </div>
                </div>
                <div class="col-md-3 position-relative">
                  <label for="validationTooltip04" class="form-label">Type </label>
               
                    <asp:DropDownList runat="server" class="custom-select" ID="ddltype3">
                <asp:ListItem value="0" Text="Choose--"></asp:ListItem>
                <asp:ListItem value="1" Text="Varchar(1000)"></asp:ListItem>
                <asp:ListItem value="2" Text="Nvarchar(1000)"></asp:ListItem>
                <asp:ListItem value="3" Text="DateTime"></asp:ListItem>
                <asp:ListItem value="4" Text="Bit"></asp:ListItem>
            </asp:DropDownList>
                 
                  <div class="invalid-tooltip">
                     Please select a valid state.
                  </div>
                </div>
            
            
                <div class="col-sm-3">
                  <div class="form-group">
                      <label class="form-label" for="exampleInputText1">Control Label Text        </label>
                      <asp:TextBox runat="server" class="form-control" ID="CLT3" placeholder="Enter Name"  ></asp:TextBox>

                  </div>
                </div>
                </div>
                <div class="row">
                    <div class="col-md-4">
                
                    <div class="form-group" style="margin-top:22px">
                        <div class="form-group">
                            <asp:CheckBox runat="server" ID="chk4"  ></asp:CheckBox>
                            <label class="form-check-label" for="flexCheckDefault">
                                FilterName4
                            </label>
                           <asp:TextBox runat="server" class=" form-control-sm" ID="FN4" placeholder="Enter Name"  ></asp:TextBox>

                        </div>
                    </div>
                    </div>
                    <div class="col-md-3 position-relative">
                      <label for="validationTooltip04" class="form-label">Type </label>
                      <asp:DropDownList runat="server" class="custom-select" ID="ddltype4">
                <asp:ListItem value="0" Text="Choose--"></asp:ListItem>
                <asp:ListItem value="1" Text="Varchar(1000)"></asp:ListItem>
                <asp:ListItem value="2" Text="Nvarchar(1000)"></asp:ListItem>
                <asp:ListItem value="3" Text="DateTime"></asp:ListItem>
                <asp:ListItem value="4" Text="Bit"></asp:ListItem>
            </asp:DropDownList>
                      <div class="invalid-tooltip">
                         Please select a valid state.
                      </div>
                    </div>
                
                
                    <div class="col-sm-3">
                      <div class="form-group">
                          <label class="form-label" for="exampleInputText1">Control Label Text        </label>
                          <asp:TextBox runat="server" class="form-control" ID="CLT4" placeholder="Enter Name"  ></asp:TextBox>
                      </div>
                    </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                    
                        <div class="form-group" style="margin-top:22px">
                            <div class="form-group">
                                <asp:CheckBox runat="server" ID="chk5"  ></asp:CheckBox> 
                                <label class="form-check-label" for="flexCheckDefault">
                                    FilterName5
                                </label>
                               <asp:TextBox runat="server" class=" form-control-sm" ID="txtFN5" placeholder="Enter Name"  ></asp:TextBox>

                            </div>
                        </div>
                        </div>
                        <div class="col-md-3 position-relative">
                          <label for="validationTooltip04" class="form-label">Type </label>
                          <asp:DropDownList runat="server" class="custom-select" ID="ddltype5">
                <asp:ListItem value="0" Text="Choose--"></asp:ListItem>
                <asp:ListItem value="1" Text="Varchar(1000)"></asp:ListItem>
                <asp:ListItem value="2" Text="Nvarchar(1000)"></asp:ListItem>
                <asp:ListItem value="3" Text="DateTime"></asp:ListItem>
                <asp:ListItem value="4" Text="Bit"></asp:ListItem>
            </asp:DropDownList>
                          <div class="invalid-tooltip">
                             Please select a valid state.
                          </div>
                        </div>
                    
                    
                        <div class="col-sm-3">
                          <div class="form-group">
                              <label class="form-label" for="exampleInputText1">Control Label Text        </label>
                               <asp:TextBox runat="server" class="form-control" ID="CLT5" placeholder="Enter Name"  ></asp:TextBox>
                          </div>
                        </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                        
                            <div class="form-group" style="margin-top:22px">
                                <div class="form-group">
                                     <asp:CheckBox runat="server" ID="chk6"  ></asp:CheckBox> 
                                    <label class="form-check-label" for="flexCheckDefault">
                                        FilterName6
                                    </label>
                                    <asp:TextBox runat="server" class=" form-control-sm" ID="FN6" placeholder="Enter Name"  ></asp:TextBox>
                                </div>
                            </div>
                            </div>
                            <div class="col-md-3 position-relative">
                              <label for="validationTooltip04" class="form-label">Type </label>
                              <asp:DropDownList runat="server" class="custom-select" ID="ddltype6">
                <asp:ListItem value="0" Text="Choose--"></asp:ListItem>
                <asp:ListItem value="1" Text="Varchar(1000)"></asp:ListItem>
                <asp:ListItem value="2" Text="Nvarchar(1000)"></asp:ListItem>
                <asp:ListItem value="3" Text="DateTime"></asp:ListItem>
                <asp:ListItem value="4" Text="Bit"></asp:ListItem>
            </asp:DropDownList>
                              <div class="invalid-tooltip">
                                 Please select a valid state.
                              </div>
                            </div>
                        
                        
                            <div class="col-sm-3">
                              <div class="form-group">
                                  <label class="form-label" for="exampleInputText1">Control Label Text        </label>
                                  <asp:TextBox runat="server" class="form-control" ID="CLT6" placeholder="Enter Name"  ></asp:TextBox>
                              </div>
                            </div>
                            </div>
                            <div class="row">
                                <div class="col-md-4">
                            
                                <div class="form-group" style="margin-top:22px">
                                    <div class="form-group">
                                         <asp:CheckBox runat="server" ID="chk7"  ></asp:CheckBox> 
                                        <label class="form-check-label" for="flexCheckDefault">
                                            FilterName7
                                        </label>
                                         <asp:TextBox runat="server" class=" form-control-sm" ID="txtFN7" placeholder="Enter Name"  ></asp:TextBox>

                                    </div>
                                </div>
                                </div>
                                <div class="col-md-3 position-relative">
                                  <label for="validationTooltip04" class="form-label">Type </label>
                                 <asp:DropDownList runat="server" class="custom-select" ID="ddltype7">
                <asp:ListItem value="0" Text="Choose--"></asp:ListItem>
                <asp:ListItem value="1" Text="Varchar(1000)"></asp:ListItem>
                <asp:ListItem value="2" Text="Nvarchar(1000)"></asp:ListItem>
                <asp:ListItem value="3" Text="DateTime"></asp:ListItem>
                <asp:ListItem value="4" Text="Bit"></asp:ListItem>
            </asp:DropDownList>
                                  <div class="invalid-tooltip">
                                     Please select a valid state.
                                  </div>
                                </div>
                            
                            
                                <div class="col-sm-3">
                                  <div class="form-group">
                                      <label class="form-label" for="exampleInputText1">Control Label Text        </label>
                                     
                                      <asp:TextBox runat="server" class=" form-control" ID="CLT7" placeholder="Enter Name"  ></asp:TextBox>
                                  </div>
                                </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4">
                                
                                    <div class="form-group" style="margin-top:22px">
                                        <div class="form-group">
                                           <asp:CheckBox runat="server" ID="chk8"  ></asp:CheckBox> 
                                            <label class="form-check-label" for="flexCheckDefault">
                                                FilterName8
                                            </label>
                                           <asp:TextBox runat="server" class=" form-control-sm" ID="txtFN8" placeholder="Enter Name"  ></asp:TextBox>
                                        </div>
                                    </div>
                                    </div>
                                    <div class="col-md-3 position-relative">
                                      <label for="validationTooltip04" class="form-label">Type </label>
                                     
 <asp:DropDownList runat="server" class="custom-select" ID="ddltype8">
                <asp:ListItem value="0" Text="Choose--"></asp:ListItem>
                <asp:ListItem value="1" Text="Varchar(1000)"></asp:ListItem>
                <asp:ListItem value="2" Text="Nvarchar(1000)"></asp:ListItem>
                <asp:ListItem value="3" Text="DateTime"></asp:ListItem>
                <asp:ListItem value="4" Text="Bit"></asp:ListItem>
            </asp:DropDownList>
                                      <div class="invalid-tooltip">
                                         Please select a valid state.
                                      </div>
                                    </div>
                                
                                
                                    <div class="col-sm-3">
                                      <div class="form-group">
                                          <label class="form-label" for="exampleInputText1">Control Label Text        </label>
                                   
                            <asp:TextBox runat="server" class=" form-control" ID="CLT8" placeholder="Enter Name"  ></asp:TextBox>
                                      </div>
                                    </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-4">
                                    
                                        <div class="form-group" style="margin-top:22px">
                                            <div class="form-group">
                                                 <asp:CheckBox runat="server" ID="chk9"  ></asp:CheckBox> 
                                                <label class="form-check-label" for="flexCheckDefault">
                                                    FilterName9
                                                </label>
                                               
                                        <asp:TextBox runat="server" class="form-control-sm" ID="txtFN9" placeholder="Enter Name"  ></asp:TextBox>
                                            </div>
                                        </div>
                                        </div>
                                        <div class="col-md-3 position-relative">
                                          <label for="validationTooltip04" class="form-label">Type </label>
                                          <asp:DropDownList runat="server" class="custom-select" ID="ddltype9">
                <asp:ListItem value="0" Text="Choose--"></asp:ListItem>
                <asp:ListItem value="1" Text="Varchar(1000)"></asp:ListItem>
                <asp:ListItem value="2" Text="Nvarchar(1000)"></asp:ListItem>
                <asp:ListItem value="3" Text="DateTime"></asp:ListItem>
                <asp:ListItem value="4" Text="Bit"></asp:ListItem>
            </asp:DropDownList>
                                          <div class="invalid-tooltip">
                                             Please select a valid state.
                                          </div>
                                        </div>
                                    
                                    
                                        <div class="col-sm-3">
                                          <div class="form-group">
                                              <label class="form-label" for="exampleInputText1">Control Label Text        </label>
                                             
                                        <asp:TextBox runat="server" class="form-control" ID="CLT9" placeholder="Enter Name"  ></asp:TextBox>
                                          </div>
                                        </div>
                                        </div>
 
                                        <div class="row">
                                            <div class="col-md-4">
                                        
                                            <div class="form-group" style="margin-top:22px">
                                                <div class="form-group">
                                                    <asp:CheckBox runat="server" ID="chk10"  ></asp:CheckBox> 
                                                    <label class="form-check-label" for="flexCheckDefault">
                                                        FilterName10
                                                    </label>
                                                    
                                         <asp:TextBox runat="server" class=" form-control-sm" ID="txtFN10" placeholder="Enter Name"  ></asp:TextBox>

                                                </div>
                                            </div>
                                            </div>
                                            <div class="col-md-3 position-relative">
                                              <label for="validationTooltip04" class="form-label">Type </label>
                                              <asp:DropDownList runat="server" class="custom-select" id="ddltype10">
                <asp:ListItem value="0" Text="Choose--"></asp:ListItem>
                <asp:ListItem value="1" Text="Varchar(1000)"></asp:ListItem>
                <asp:ListItem value="2" Text="Nvarchar(1000)"></asp:ListItem>
                <asp:ListItem value="3" Text="DateTime"></asp:ListItem>
                <asp:ListItem value="4" Text="Bit"></asp:ListItem>
            </asp:DropDownList>
                                              <div class="invalid-tooltip">
                                                 Please select a valid state.
                                              </div>
                                            </div>
                                        
                                        
                                            <div class="col-sm-3">
                                              <div class="form-group">
                                                  <label class="form-label" for="exampleInputText1">Control Label Text        </label>
                                                 

                                                <asp:TextBox runat="server" class="form-control" ID="CLT10" placeholder="Enter Name"  ></asp:TextBox>
                                              </div>
                                            </div>
                                            </div>
     
                 </div>
                 <div class="text-center mt-3">
                    <asp:Button runat="server" ID="btnsave" Text="Save" class="btn btn-success" OnClick="btnsave_Click"></asp:Button>
                    <asp:Button runat="server" ID="btnReset" Text="Reset" class="btn btn-danger" OnClick="btnReset_Click"></asp:Button>
                  
                  </div>

                     <div class="col-md-12 mt-3 pb-5 mb-2">
                      <div class="table-responsive">
                          <asp:GridView runat="server" ID="grd_bind" class="table table-striped table-bordered  " AutoGenerateColumns="false" >
                              <Columns>
                                  <asp:TemplateField HeaderText="Sr.No.">
                                      <ItemTemplate >
                                          <%# Container.DataItemIndex+1 %>
                                      </ItemTemplate>
                                  </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Report Name">
                                      <ItemTemplate >
                                          <%#Eval("reportname") %>
                                      </ItemTemplate>
                                  </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Procedure Name">
                                      <ItemTemplate >
                                          <%#Eval("procname") %>
                                      </ItemTemplate>
                                  </asp:TemplateField>

                                    <asp:TemplateField HeaderText="NoOfFilter">
                                      <ItemTemplate >
                                          <%#Eval("nof") %>
                                      </ItemTemplate>
                                  </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Active">
                                      <ItemTemplate >
                                          <asp:HiddenField runat="server" ID="hd_Active" Value='<%#Eval("IsActive") %>' />
                                          <asp:HiddenField runat="server" ID="hd_idchk" Value='<%#Eval("id") %>' />
                                         <asp:CheckBox runat="server" ID="chkbox" Checked="true"  AutoPostBack="true" OnCheckedChanged="chkbox_CheckedChanged" />
                                      </ItemTemplate>
                                  </asp:TemplateField>

                                    <asp:TemplateField HeaderText="View">
                                      <ItemTemplate >
                                          <asp:HiddenField runat="server" ID="hd_id" Value='<%#Eval("id") %>' />
                                         <asp:Button runat="server" Text="View" ID="btnview" OnClick="btnview_Click" />
                                      </ItemTemplate>
                                  </asp:TemplateField>

                              </Columns>

                          </asp:GridView> 
                      <%--<table id="example1" 
                      <thead>
                      <tr>
                      <th>Sr.No.</th>
                      <th >Report Name</th>
                      <th>Procedure Name</th>
                      <th>No. of Filter </th>
                      <th>Active
                    </th>
                   
                      <th>View
                    </th>
                      
                      </tr>
                      </thead>
                      <tbody>
                      <tr>
                      <td>1</td>
                      <td>Details</td>
                      
                      <td ></td>
                      <td>0.00</td>
                    
                      <td>  <input type="checkbox" value="" id="flexCheckDefault"></td>
                      <td ><a href="#" ><i class="fa fa-eye view"></i></a></td>
                    
                      </tr>
                      <tr>
                      <tr>
                      <td>2</td>
                      <td>Details</td>
                      <td></td>
                     
                      <td>0.00</td>
                      <td>  <input type="checkbox" value="" id="flexCheckDefault"></td>
                      <td class="tr1"><a href="#" ><i class="fa fa-eye edit"></i></a></td>
                      </tr>
                      
                      <tr>
                      <td>3</td>
                      <td>Details</td>
                      <td></td>
                      
                      <td>0.00</td>
                      <td>  <input type="checkbox" value="" id="flexCheckDefault"></td>
                      <td class="tr1"><a href="#" ><i class="fa fa-eye edit"></i></a></td>
                      </tr>
                      
                    
                  
                      </tbody>
                      
                      </table>--%>
                    
                      
                      </div>
                      </div><!--Table close-->
    
                 </div>
               
                
             </div>
              
               
            </div>
            </div>
       

    </main>
</asp:Content>
