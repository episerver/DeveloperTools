<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<DeveloperTools.Models.MemoryDumpModel>" MasterPageFile="../Shared/DeveloperTools.Master" %>
<%@ Assembly Name="DeveloperTools" %>
<%@ Import namespace="log4net.Core" %>
<%@ Import Namespace = "System.Linq" %>
<%@ Import namespace="DeveloperTools.Models" %>

 
 <asp:Content ID="Content" runat="server" ContentPlaceHolderID="MainRegion">

<link rel="stylesheet" type="text/css" href="http://ajax.aspnetcdn.com/ajax/jquery.dataTables/1.9.0/css/jquery.dataTables.css" />
<script type="text/javascript" language="javascript" src="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.7.1.min.js"></script>
<script src="http://ajax.aspnetcdn.com/ajax/jquery.dataTables/1.9.0/jquery.dataTables.min.js" type="text/javascript"></script>

<h1>Memory Dump</h1>
<p>A tool that you can dumps memory with different flag that can be analyized with windbg tool.</p>
     <br />
<div >
<% using (Html.BeginForm("Index", "MemoryDump", new {  }, FormMethod.Post))
{ %>
    <%=Html.Label("File Path")%>: <%=Html.TextBox("filepath", null, new { style="width:30%" }) %>
    <%=Html.Label("DumpType")%>: <%=Html.DropDownListFor(m => m.SelectedDumpValue, MemoryDumpModel.GetDumpTypes()) %>
    <input type="submit" value="DumpMemory" />
<% } %>
<p>
<%if (!String.IsNullOrEmpty(Model.Name)){ %>
 <%Response.Write(String.Format("A File with name{0} has been created under path {1}", Model.Name, Model.FilePath));}%> 
</p>
</div>

</asp:Content>