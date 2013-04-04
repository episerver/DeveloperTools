<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<DeveloperTools.Models.LogSettingAndEvents>" MasterPageFile="../Shared/DeveloperTools.Master" %>
<%@ Assembly Name="DeveloperTools" %>
<%@ Import namespace="log4net.Core" %>
<%@ Import Namespace = "System.Linq" %>
<%@ Import namespace="DeveloperTools.Models" %>

 
 <asp:Content ID="Content" runat="server" ContentPlaceHolderID="MainRegion">

<link rel="stylesheet" type="text/css" href="http://ajax.aspnetcdn.com/ajax/jquery.dataTables/1.9.0/css/jquery.dataTables.css" />
<script type="text/javascript" language="javascript" src="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.7.1.min.js"></script>
<script src="http://ajax.aspnetcdn.com/ajax/jquery.dataTables/1.9.0/jquery.dataTables.min.js" type="text/javascript"></script>

<h1>Log Viewer</h1>
<p>A tool that uses an in-memory appender to catch ALL log messages. WARNING! Never use in production, it will seriously harm performance.</p>

<div>
Start/stop in-memory appender:  <strong><%= !Model.IsStarted ? Html.ActionLink("Start", "Start") : Html.ActionLink("Stop", "Stop")%></strong>
</div>
<hr />
<div>

<% using (Html.BeginForm("Index", "LogViewer", new {  }, FormMethod.Post))
{ %>
    <%=Html.Label("Level")%>: <%=Html.DropDownListFor(m=>m.LoggerSetting.LevelValue, LoggerSettings.GetLevels()) %>
    <%=Html.Label("Logger Name")%>: <%=Html.TextBoxFor(m=>m.LoggerSetting.LoggerName) %>
    <%=Html.Label("Start Date")%>: <%= Html.TextBoxFor(m=>m.LoggerSetting.StartDate) %>
    <%=Html.Label("End Date")%>: <%= Html.TextBoxFor(m=>m.LoggerSetting.EndDate)%>
    <%=Html.Label("Thread Id")%>: <%= Html.TextBoxFor(m=>m.LoggerSetting.ThreadName)%>
    <%=Html.Label("UserName ")%>: <%= Html.TextBoxFor(m=>m.LoggerSetting.UserName)%>
    <input type="submit" value="Filter" />
    Hits: <%:Model.LoggingEvents.Count()%>
<% } %>
</div>
<hr />
<table cellpadding="0" cellspacing="0" border="0" class="display" id="theList">
	<thead>
		<tr>
            <th align="left" width="10%">Date</th>
			<th align="left">Message</th>
			<th align="left">LoggerName</th>
            <th align="left">Level</th>
			<th align="left">ThreadName</th>
		</tr>
	</thead>
	<tbody>
<% foreach (log4net.Core.LoggingEvent logevent in Model.LoggingEvents){%>
 <%if (logevent.Level == Level.Error)%>
        <%Response.Write("<tr style='background-color:Red'>");%>
        <%else if (logevent.Level == Level.Warn)%>
        <%Response.Write("<tr style='background-color:Yellow'>");%>
        <%else %>
        <%Response.Write("<tr>");%>
        <td><%=logevent.TimeStamp%></td>
        <td title="<%:logevent.RenderedMessage%>"><%:logevent.RenderedMessage.Length > 200 ? logevent.RenderedMessage.Substring(0, 200) + " (..)" : logevent.RenderedMessage%></td>
        <td><%=logevent.LoggerName%></td>
        <td><%=logevent.Level.DisplayName%></td>
        <td><%=logevent.ThreadName.ToString()%></td>
    </tr>
    <%} %>
	</tbody>
</table>

<script>
    $(document).ready(function () {
        $('#theList').dataTable(
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