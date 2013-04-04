<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<IEnumerable<EPiServer.Framework.Initialization.TimeMeters>>" MasterPageFile="../Shared/DeveloperTools.Master" %>

 
 <asp:Content ID="Content" runat="server" ContentPlaceHolderID="MainRegion">

<link rel="stylesheet" type="text/css" href="http://ajax.aspnetcdn.com/ajax/jquery.dataTables/1.9.0/css/jquery.dataTables.css" />
<script type="text/javascript" language="javascript" src="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.7.1.min.js"></script>
<script src="http://ajax.aspnetcdn.com/ajax/jquery.dataTables/1.9.0/jquery.dataTables.min.js" type="text/javascript"></script>

<h1>Startup Performance</h1>

<p>Displays timing measurements from the initialization process, can be used to find modules causing slow startups.</p>
<p>Number of time meters: <strong><%:Model.Count()%></strong>. Total time: <strong><%: TimeSpan.FromMilliseconds(Model.Select(tm => tm.Counters).Sum(c => c.Values.Sum(s => s.ElapsedMilliseconds))).TotalSeconds.ToString("N")%> s</strong>.</p>

<table cellpadding="0" cellspacing="0" border="0" class="display" id="theList">
	<thead>
		<tr>
            <th align="left">Assembly</th>
			<th align="left">Type</th>
			<th align="left">Action</th>
			<th>Time (ms)</th>
		</tr>
	</thead>
	<tbody>
<% foreach (var m in Model){%>
    <%foreach (var item in m.Counters.OrderByDescending(s => s.Value.ElapsedMilliseconds)){ %>
    <tr>
    <td><%:m.Owner.Assembly.GetName().Name%></td>
    <td><%:m.Owner.Name%></td>
    <td><%: item.Key %></td>
    <td><%: item.Value.ElapsedMilliseconds%></td>
    </tr>
    <%}%>
<%}%>
</tbody>
</table>

<script>
    $(document).ready(function () {
        $('#theList').dataTable(
        {
            "aaSorting": [[3, "desc"]],
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
