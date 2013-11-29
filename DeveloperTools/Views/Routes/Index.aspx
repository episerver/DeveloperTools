
<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<IEnumerable<DeveloperTools.Models.RouteModel>>" MasterPageFile="../Shared/DeveloperTools.Master" %>
<%@ Assembly Name="DeveloperTools" %>
<%@ Import Namespace="DeveloperTools.Models" %>
<%@ Import Namespace="EPiServer.Shell.Web.Mvc.Html" %>
<%@ Import Namespace="EPiServer.Shell.Web.Routing" %>

<asp:Content ID="Content" runat="server" ContentPlaceHolderID="MainRegion">

<link rel="stylesheet" type="text/css" href="http://ajax.aspnetcdn.com/ajax/jquery.dataTables/1.9.0/css/jquery.dataTables.css" />
<script type="text/javascript" language="javascript" src="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.7.1.min.js"></script>
<script src="http://ajax.aspnetcdn.com/ajax/jquery.dataTables/1.9.0/jquery.dataTables.min.js" type="text/javascript"></script>

    <h1>Routes</h1>
    <p>
        Dumps all routes registered in the Application and you can find out which routes can handle a request.
    </p>

<div >
<% using (Html.BeginForm("Index", "Routes", new {  }, FormMethod.Post))
{ %>
    <%=Html.Label("Url ")%>: <%=Html.TextBox("url", "http://", new { style="width:80%" }) %>
    <input type="submit" value="FindRoute" />
<% } %>
</div>
<table cellpadding="0" cellspacing="0" border="0" class="display" id="theList">
	<thead>
		<tr>
            <th align="left">Order</th>
            <th align="left">Name</th>
            <th align="left">Type</th>
            <th align="left">Url</th>
            <th align="left">RouteHandler </th>
            <th align="left">Defaults </th>
		</tr>
	</thead>
	<tbody>
<% foreach (var entry in Model)
   { %>
        <%if (entry.IsSelected)%>
            <%Response.Write("<tr style='background-color:LightGreen'>");%>    
        <%else %>
            <%Response.Write("<tr>");%>
       <td><%=entry.Order %> </td>
       <td><%=entry.Name %> </td>
       <td><%=entry.Type %> </td>
       <td><%=entry.Url %> </td>
       <td><%=entry.RouteHandler%> </td>
       <td><%=entry.Defaults %> </td>
       </tr>
       <%if (entry.IsSelected){%>
            <%Response.Write("<tr style='background-color:LightGreen'>");%> 
            <%Response.Write("<td>");%>
            <%Response.Write(entry.Order);%>
            <%Response.Write("</td><td>");%>
            <%Response.Write(entry.DataTokens);%>
            <%Response.Write("</td><td></td><td></td><td></td><td></td></tr>");%>

            <%Response.Write("<tr style='background-color:LightGreen'>");%> 
            <%Response.Write("<td>");%>
            <%Response.Write(entry.Order);%>
            <%Response.Write("</td><td>");%>
            <%Response.Write(entry.Values);%>
            <%Response.Write("</td><td></td><td></td><td></td><td></td></tr>");%>
            <%}%>
    <%}
 %>
</tbody>
</table>


<script>
    $(document).ready(function () {
        $('#theList').dataTable(
        {
            "aaSorting": [[0, "asc"]],
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