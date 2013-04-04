<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<IEnumerable<EPiServer.DataAbstraction.ContentType>>" MasterPageFile="../Shared/DeveloperTools.Master" %>
 
 <asp:Content ID="Content" runat="server" ContentPlaceHolderID="MainRegion">

<link rel="stylesheet" type="text/css" href="http://ajax.aspnetcdn.com/ajax/jquery.dataTables/1.9.0/css/jquery.dataTables.css" />
<script type="text/javascript" language="javascript" src="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.7.1.min.js"></script>
<script src="http://ajax.aspnetcdn.com/ajax/jquery.dataTables/1.9.0/jquery.dataTables.min.js" type="text/javascript"></script>

<h1>Revert contentypes to default</h1>

<table cellpadding="0" cellspacing="0" border="0" class="display" id="theList">
	<thead>
		<tr>
            <th align="left">Selected</th>
            <th align="left">DisplayName</th>
			<th align="left">FullName</th>
			<th align="left">ModelType</th>
			<th align="left">Identity</th>
		</tr>
	</thead>
	<tbody>
<% foreach (var m in Model){%>
    <tr>
        <td><input type="checkbox" checked="checked" id="<%:m.GUID%>"/></td>
        <td><%:m.DisplayName%></td>
        <td><%:m.FullName%></td>
        <td><%:m.ModelType%></td>
        <td><%:m.GUID%></td>
    </tr>
<%}%>
</tbody>
</table>

<script>
    $(document).ready(function () {
        $('#theList').dataTable(
        {
            "aaSorting": [[2, "desc"]],
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
