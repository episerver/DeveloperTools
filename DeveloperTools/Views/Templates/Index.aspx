<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<IEnumerable<EPiServer.DataAbstraction.TemplateModel>>" MasterPageFile="../Shared/DeveloperTools.Master" %>
<%@ Import Namespace="EPiServer.DataAbstraction" %> 

 <asp:Content ID="Content" runat="server" ContentPlaceHolderID="MainRegion">

<link rel="stylesheet" type="text/css" href="http://ajax.aspnetcdn.com/ajax/jquery.dataTables/1.9.0/css/jquery.dataTables.css" />
<script type="text/javascript" language="javascript" src="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.7.1.min.js"></script>
<script src="http://ajax.aspnetcdn.com/ajax/jquery.dataTables/1.9.0/jquery.dataTables.min.js" type="text/javascript"></script>
<h1>Templates</h1>
<p>Show a list of all templates registered in the system by querying each content type for its templates and then making a summarized list of the unique templates found.</p>
 
<table cellpadding="0" cellspacing="0" border="0" class="display" id="theList">
	<thead>
		<tr>
			<th align="left">Name</th>
            <th align="left">Category</th>
			<th align="left">Default</th>
			<th align="left">Inherit</th>
            <th align="left">Tags</th>
            <th align="left">AvailableWithoutTag</th>
            <th align="left">Path</th>
            <th align="left">Type</th>
		</tr>
	</thead>
	<tbody>
<% foreach (var t in Model){%>
    <tr>
        <td><%:t.Name%></td>
        <td><%:t.TemplateTypeCategory%></td>
        <td><%:t.Default?"yes":"no"%></td>
        <td><%:t.Inherit?"yes":"no"%></td>
        <td><%:t.Tags==null ? "" : String.Join(",", t.Tags)%></td>
        <td><%:t.AvailableWithoutTag ? "Yes":"No"%></td>
        <td><%:t.Path%></td>
        <td><%:t.TemplateType%></td>
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