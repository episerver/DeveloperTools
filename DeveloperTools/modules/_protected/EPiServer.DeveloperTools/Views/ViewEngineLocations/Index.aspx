<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<DeveloperTools.Models.ViewEngineLocationsModel>" MasterPageFile="../Shared/DeveloperTools.Master" %>
<%@ Import Namespace="EPiServer.DataAbstraction" %>

 <asp:Content ID="Content" runat="server" ContentPlaceHolderID="MainRegion">

<link rel="stylesheet" type="text/css" href="//ajax.aspnetcdn.com/ajax/jquery.dataTables/1.9.0/css/jquery.dataTables.css" />
<script type="text/javascript" language="javascript" src="//ajax.aspnetcdn.com/ajax/jQuery/jquery-1.7.1.min.js"></script>
<script src="//ajax.aspnetcdn.com/ajax/jquery.dataTables/1.9.0/jquery.dataTables.min.js" type="text/javascript"></script>
<h1>View Engine Locations</h1>
<p>Show a list of all view locations where Mvc would look for templates.</p>

<table cellpadding="0" cellspacing="0" border="0" class="display" id="theViewLocations">
    <thead>
        <tr>
            <th align="left">View Location</th>
            <th align="left">Engine</th>
        </tr>
    </thead>
    <tbody>
<% foreach (var l in Model.ViewLocations){%>
    <tr>
        <td><%:l.Location%></td>
        <td><%:l.EngineName%></td>
    </tr>
  <%}%>
  </tbody>
</table>

<table cellpadding="0" cellspacing="0" border="0" class="display" id="thePartialViewLocations">
    <thead>
        <tr>
            <th align="left">Partial View Location</th>
            <th align="left">Engine</th>
        </tr>
    </thead>
    <tbody>
<% foreach (var l in Model.PartialViewLocations){%>
    <tr>
        <td><%:l.Location%></td>
        <td><%:l.EngineName%></td>
    </tr>
  <%}%>
  </tbody>
</table>

<script>
    $(document).ready(function () {
        $('#theViewLocations').dataTable(
        {
            "aaSorting": [[0, "desc"]],
            "bPaginate": false,
            "bLengthChange": false,
            "bFilter": true,
            "bSort": true,
            "bInfo": false,
            "bAutoWidth": true
        });

        $('#thePartialViewLocations').dataTable(
        {
            "aaSorting": [[0, "desc"]],
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