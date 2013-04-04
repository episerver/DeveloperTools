<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<DeveloperTools.Models.AssembliesModel>" MasterPageFile="../Shared/DeveloperTools.Master" %>

 <%@ Assembly Name="DeveloperTools" %>
<%@ Import Namespace="DeveloperTools.Models" %>
<%@ Import Namespace="EPiServer.Shell.Web.Mvc.Html" %>
<%@ Import Namespace="System.Reflection" %>

 <asp:Content ID="Content" runat="server" ContentPlaceHolderID="MainRegion">

<link rel="stylesheet" type="text/css" href="http://ajax.aspnetcdn.com/ajax/jquery.dataTables/1.9.0/css/jquery.dataTables.css" />
<script type="text/javascript" language="javascript" src="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.7.1.min.js"></script>
<script src="http://ajax.aspnetcdn.com/ajax/jquery.dataTables/1.9.0/jquery.dataTables.min.js" type="text/javascript"></script>

<h1>Loaded Assemblies</h1>
<p>Dumps all loaded assemblies in the current AppDomain</p>

<table cellpadding="0" cellspacing="0" border="0" class="display" id="theList">
	<thead>
		<tr>
            <th align="left">Name</th>
			<th align="left">AssemblyVersion</th>
			<th align="left">FileVersion</th>
			<th>Location</th>
		</tr>
	</thead>
	<tbody>
<% foreach (AssemblyInfo entry in Model.Assemblies)
   { %>
    <tr>
        <td><%= entry.Name %> </td>
        <td><%= entry.AssemblyVersion%> </td>
        <td> <%= entry.FileVersion%> </td>
        <td><%= entry.Location%></td>
    </tr>
    <% } %>
</tbody>
</table>
    
<h2>Environment Variables</h2> 
<pre> 
<table>     
<% 
    var variables = Environment.GetEnvironmentVariables(); 
    foreach (DictionaryEntry entry in variables) 
    { 
        Response.Write("<tr><td>"); 
        Response.Write(entry.Key); 
        Response.Write("</td><td>"); 
        Response.Write(entry.Value); 
        Response.Write("</td></tr>"); 
    } 
%> 
</table> 
</pre> 
 
<h2>Misc</h2> 
<pre> 
Response.Filter = <%= Request.Filter.ToString() %> 
Request.ApplicationPath = <%= Request.ApplicationPath %> 
Request.PhysicalApplicationPath = <%= Request.PhysicalApplicationPath %> 
Request.PhysicalPath = <%= Request.PhysicalPath %> 
Request.UrlReferrer = <%= Request.UrlReferrer %> 
Request.UserLanguages = <%= string.Join(",", (Request.UserLanguages ?? new string[0])) %> 
</pre>

<script>
    $(document).ready(function () {
        $('#theList').dataTable(
        {
            "aaSorting": [[1, "desc"]],
            "bPaginate": false,
            "bLengthChange": false,
            "bFilter": true,
            "bSort": true,
            "bInfo": false,
            "bAutoWidth": true
        });
    });
</script>
</asp:Content>
