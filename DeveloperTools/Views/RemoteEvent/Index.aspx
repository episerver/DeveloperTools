<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<RemoteEventsModel>" MasterPageFile="../Shared/DeveloperTools.Master" %>

<%@ Assembly Name="DeveloperTools" %>
<%@ Import Namespace="DeveloperTools.Models" %>

<asp:Content ID="Content" runat="server" ContentPlaceHolderID="MainRegion">

    <link rel="stylesheet" type="text/css" href="http://ajax.aspnetcdn.com/ajax/jquery.dataTables/1.9.4/css/jquery.dataTables.css" />
    <script type="text/javascript" language="javascript" src="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.7.1.min.js"></script>
    <script src="http://ajax.aspnetcdn.com/ajax/jquery.dataTables/1.9.4/jquery.dataTables.min.js" type="text/javascript"></script>

    <h1>A tool uses for show diagnostic and statical Remote Events</h1>

    <p style="margin-bottom: .5em">
        Configuration status: <strong><%: Model.Enabled ? "Enabled" : "Disabled" %></strong><br />
        Provider: <strong><%: Model.ProviderName %></strong> (<%: Model.ProviderType %>)<br />
        Remote servers: <strong><%: Model.ServerState.Count() %></strong><br />
        Licensed servers: <strong><%: Model.ActiveServers.Count() %></strong> (<%: string.Join(", ", Model.ActiveServers) %>)
    </p>
    <table cellpadding="0" cellspacing="0" border="0" class="display dataTable" id="serverList">
        <thead>
            <tr>
                <th align="left">Server</th>
                <th align="left">Application</th>
                <th align="left">Last Contact</th>
            </tr>
        </thead>
        <tbody>
            <% foreach (var s in Model.ServerState)
               {%>
            <tr>
                <td><%: s.ServerName %></td>
                <td><%: s.ApplicationName %></td>
                <td><%: s.LastContact %></td>
            </tr>
            <%}%>
        </tbody>
    </table>

    <p style="margin-bottom: .5em">
        Number of Event Types: <strong><%: Model.RemoteEventModel != null ? Model.RemoteEventModel.Count():0%>.</strong><br />
        Total Number of Sent Events: <strong><%: Model.TotalNumberOfSentEvent%>.</strong><br />
        Total Number of Received Events: <strong><%: Model.TotalNumberOfReceivedEvent%>.</strong>
    </p>

    <table cellpadding="0" cellspacing="0" border="0" class="display dataTable" id="eventList">
        <thead>
            <tr>
                <th align="left">Name</th>
                <th align="left">Guid</th>
                <th align="left">Number of Sent Events</th>
                <th align="left">Number Of Received Events</th>
            </tr>
        </thead>
        <tbody>
            <% foreach (var m in Model.RemoteEventModel)
               {%>
            <tr>
                <td><%:m.Name%></td>
                <td><%:m.Guid%></td>
                <td><%:m.NumberOfSent%></td>
                <td><%:m.NumberOfReceived %></td>
            </tr>
            <%}%>
        </tbody>
    </table>
    <br />
    <br />
    <br />
    <div>
        <% using (Html.BeginForm("Index", "RemoteEvent", new { }, FormMethod.Post))
           { %>
            <%=Html.Label("EventId")%>: <%=Html.TextBoxFor(m=>m.SendRemoteEventModel.EventId)  %>
            <%=Html.Label("Param")%>: <%=Html.TextBoxFor(m=>m.SendRemoteEventModel.Param)  %>
            <%=Html.Label("Number Of Events")%>: <%=Html.TextBoxFor(m=>m.SendRemoteEventModel.NumberOfevents)  %>
            <input type="submit" value="Send Message" />
            Sent message: <%:Model.SendRemoteEventModel.NumberOfeventsSent%>
        <% } %>
    </div>

    <script>
        $(document).ready(function () {
            $('.dataTable').dataTable(
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
