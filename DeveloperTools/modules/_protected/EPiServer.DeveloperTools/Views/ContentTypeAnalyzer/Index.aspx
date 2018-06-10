<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<DeveloperTools.Models.ContentTypeAnalyzerModel>" MasterPageFile="../Shared/DeveloperTools.Master" %>
 <%@ Import Namespace="EPiServer.DataAbstraction.RuntimeModel" %>


 <asp:Content ID="Content" runat="server" ContentPlaceHolderID="MainRegion">

<link rel="stylesheet" type="text/css" href="//ajax.aspnetcdn.com/ajax/jquery.dataTables/1.9.0/css/jquery.dataTables.css" />
<script type="text/javascript" language="javascript" src="//ajax.aspnetcdn.com/ajax/jQuery/jquery-1.7.1.min.js"></script>
<script src="//ajax.aspnetcdn.com/ajax/jquery.dataTables/1.9.0/jquery.dataTables.min.js" type="text/javascript"></script>
<h1>Content Type Analyzer</h1>
<p>Show content type Synchronization status during initialization (if the content type be changed from admin you need to start the site to see changes)</p>

<table cellpadding="0" cellspacing="0" border="0" class="display" id="theList">
    <thead>
        <tr>
            <th align="left">Type</th>
            <th align="left">DisplayName</th>
            <th align="left">Name</th>
            <th align="left">SynchronizationStatus</th>
            <th align="left">Conflicts</th>
            <th align="left">Description</th>

        </tr>
    </thead>
    <tbody>
<% foreach (var m in Model.ContentTypes){%>
    <tr>
        <td><%:m.Type%></td>
        <td><%:m.DisplayName%></td>
        <td><%:m.Name%></td>
        <td <%:m.HasConflict ? "bgcolor=red":""%>><%:m.State%></td>
        <td>
            <% if(m.HasConflict){%>
            <ul>
            <% foreach (var c in m.Conflicts){%>
                <li><%:c.Name %> - Code = "<%:c.ContentTypeModelValue %>" vs DB = "<%:c.ContentTypeValue %>"</li>
            <%} %>
            </ul>
            <%} %>
        </td>
        <td><%:m.Description%></td>
    </tr>
  <%}%>
  </tbody>
</table>

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
