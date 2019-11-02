﻿
<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<DeveloperTools.Models.IOCModel>" MasterPageFile="../Shared/DeveloperTools.Master" %>
<%@ Assembly Name="EPiServer.DeveloperTools" %>
<%@ Import Namespace="DeveloperTools.Models" %>
<%@ Import Namespace="EPiServer.Shell.Web.Mvc.Html" %>

<asp:Content ID="Content" runat="server" ContentPlaceHolderID="MainRegion">
    <link rel="stylesheet" type="text/css" href="//ajax.aspnetcdn.com/ajax/jquery.dataTables/1.9.0/css/jquery.dataTables.css" />
    <script type="text/javascript" language="javascript" src="//ajax.aspnetcdn.com/ajax/jQuery/jquery-1.7.1.min.js"></script>
    <script src="//ajax.aspnetcdn.com/ajax/jquery.dataTables/1.9.0/jquery.dataTables.min.js" type="text/javascript"></script>

    <div class="epi-contentArea">
        <h1 class="EP-prefix">IoC Container</h1>
        <p>
            Dumps all types registered in the StructureMap container used by EPiServer.
        </p>
    </div>

    <div class="epi-formArea">
        <table cellpadding="0" cellspacing="0" border="0" class="display" id="theList">
        <thead>
            <tr>
                <th align="left">PluginType</th>
                <th align="left">ConcreteType</th>
                <th align="left">Scope</th>
                <th>Is Default</th>
            </tr>
        </thead>
        <tbody>
    <% foreach (IOCEntry entry in Model.IOCEntries)
       { %>
        <tr>
           <td><%= entry.PluginType %> </td>
            <td><%= entry.ConcreteType %> </td>
            <td><%= entry.Scope %> </td>
            <td><%= entry.IsDefault %> </td>
        </tr>
        <% }
     %>
    </tbody>
    </table>
    </div>

    <b>Retrieval errors:</b>
    <ul>
    <% foreach (string message in Model.LoadingErrors)
       { %>
        <li>
            <%= message%> <br />
            ________________________________________________________________
        </li>
        <% } %>
    </ul>

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