<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<IEnumerable<EPiServer.DataAbstraction.ContentType>>" MasterPageFile="../Shared/DeveloperTools.Master" %>
 
 <asp:Content ID="Content" runat="server" ContentPlaceHolderID="MainRegion">

<link rel="stylesheet" type="text/css" href="http://ajax.aspnetcdn.com/ajax/jquery.dataTables/1.9.0/css/jquery.dataTables.css" />
<script type="text/javascript" language="javascript" src="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.7.1.min.js"></script>
<script src="http://ajax.aspnetcdn.com/ajax/jquery.dataTables/1.9.0/jquery.dataTables.min.js" type="text/javascript"></script>

<h1>Revert Content Types to Default</h1>
 <p>Resets overridden values stored in the database for a content type and all its properties.</p>
<form method="post">
<%= Html.AntiForgeryToken()%>
<table cellpadding="0" cellspacing="0" border="0" class="display" id="theList">
	<thead>
		<tr>
            <th align="left"><span ondblclick="selectAll()">Selected</span></th>
            <th align="left">DisplayName</th>
			<th align="left">FullName</th>
			<th align="left">ModelType</th>
			<th align="left">ID</th>
			<th align="left">Identity</th>
		</tr>
	</thead>
	<tbody>
<% foreach (var m in Model){%>
    <tr>
        <td><input type="checkbox" name="selectedObjects" value="<%:m.GUID%>"/></td>
        <td><%:m.DisplayName%></td>
        <td><%:m.FullName%></td>
        <td><%:m.ModelType%></td>
        <td><%:m.ID%></td>
        <td><%:m.GUID%></td>
    </tr>
<%}%>
</tbody>
</table>
<span class ="epi-cmsButton">
<input type="submit" value="Revert To Default" class="epi-cmsButton-text epi-cmsButton-tools epi-cmsButton-Delete" />
</span>
</form>

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

    function selectAll() {
        $('input:checkbox').attr('checked', 'checked');

    }
</script>
</asp:Content>
