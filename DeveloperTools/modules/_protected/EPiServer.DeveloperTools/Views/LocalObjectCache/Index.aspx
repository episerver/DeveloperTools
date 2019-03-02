<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<DeveloperTools.Models.LocalObjectCache>" MasterPageFile="../Shared/DeveloperTools.Master" %>

<asp:Content ID="Styles" runat="server" ContentPlaceHolderID="HeaderStyles">
    <style type="text/css">
        .table-column-width {
            width: 30%;
        }

        .stripe tbody tr:nth-child(even) {
            background-color: #f0f2f2;
        }
    </style>
</asp:Content>

<asp:Content ID="Content" runat="server" ContentPlaceHolderID="MainRegion">
    <link rel="stylesheet" type="text/css" href="//ajax.aspnetcdn.com/ajax/jquery.dataTables/1.9.0/css/jquery.dataTables.css" />
    <script type="text/javascript" language="javascript" src="//ajax.aspnetcdn.com/ajax/jQuery/jquery-1.7.1.min.js"></script>
    <script src="//ajax.aspnetcdn.com/ajax/jquery.dataTables/1.9.0/jquery.dataTables.min.js" type="text/javascript"></script>

    <div class="epi-contentContainer epi-padding">
        <div class="epi-contentArea">
            <h1>Local Object Cache</h1>
            <p class="EP-systemInfo">This tool shows all of the current items in the local object cache, and allows the deletion of one or more cached items.</p>
        </div>

        <div class="epi-contentArea epi-formArea">
            <%using (Html.BeginForm("Index", "LocalObjectCache", FormMethod.Post))
              {  %>
                <%= Html.AntiForgeryToken() %>

                <table class="table">
                    <tr>
                        <td>Filter By</td>
                        <td><%= Html.DropDownListFor(m => m.FilteredBy, Model.Choices)  %></td>
                        <td>
                            <span class="epi-cmsButton">
                                <input class="epi-cmsButton-text epi-cmsButton-tools epi-cmsButton-Refresh" type="submit" name="filter" id="filter" value="Filter" onmouseover="EPi.ToolButton.MouseDownHandler(this)" onmouseout="EPi.ToolButton.ResetMouseDownHandler(this)" />
                            </span>
                        </td>
                    </tr>
                </table>
            <% } %>

            <% using (Html.BeginForm("Action", "LocalObjectCache", FormMethod.Post))
               { %>
                <div class="epi-contentArea">
                    <p class="EP-systemInfo">Total count of cached items: <%= Model.CachedItems.Count() %></p>
                </div>
                <div class="epi-buttonDefault">
                    <span class="epi-cmsButton">
                        <input class="epi-cmsButton-text epi-cmsButton-tools epi-cmsButton-Delete" type="submit" name="RemoveLocalCache" id="RemoveLocalCache" value="Remove Local Cache Items" onmouseover="EPi.ToolButton.MouseDownHandler(this)" onmouseout="EPi.ToolButton.ResetMouseDownHandler(this)" />
                    </span>
                    <span class="epi-cmsButton">
                        <input class="epi-cmsButton-text epi-cmsButton-tools epi-cmsButton-Delete" type="submit" name="removeLocalRemoteCache" id="removeLocalRemoteCache" value="Remove Local and Remote Cache Items" onmouseover="EPi.ToolButton.MouseDownHandler(this)" onmouseout="EPi.ToolButton.ResetMouseDownHandler(this)" />
                    </span>
                </div>

                <table class="table table-condensed table-bordered table-condensed stripe">
                    <thead>
                        <tr>
                            <th><input type="checkbox" id="clearAll" name="clearAll" onClick="toggle(this)" value="true" /></th>
                            <th class="table-column-width">Key</th>
                            <th class="table-column-width">Type</th>
                            <th>Size</th>
                            <%--<th class="table-column-width">@(string.IsNullOrWhiteSpace(Model.FilteredBy) ? "Value" : "Name (ID) Published")</th>--%>
                        </tr>
                    </thead>
                    <tbody>
                        <% foreach (var item in Model.CachedItems)
                        { %>
                            <tr>
                                <td class="center"><input type="checkbox" id="<%= item.Key %>" name="cacheKeys" value="<%= item.Key %>" /></td>
                                <td><%= item.Key %></td>
                                <td><%= item.Value.GetType() %></td>
                                <td><%= item.Size %></td>
                                <%--<td>
                                    <% if (item.Value is IContent)
                                        {
                                            (item.Value as IContent).Name;
                                         %>
                                        <span class="badge badge-warning"><%((item.Value as IContent).ContentLink.ID); %></span>
                                    <%
                                        }
                                        if (item.Value is PageData)
                                        {
                                            ((item.Value as PageData).StartPublish);
                                        } %>
                                </td>--%>
                            </tr>
                        <% } %>
                    </tbody>
                </table>

                <div class="epi-buttonDefault">
                    <span class="epi-cmsButton">
                        <input class="epi-cmsButton-text epi-cmsButton-tools epi-cmsButton-Delete" type="submit" name="RemoveLocalCache" id="RemoveLocalCacheBottom" value="Remove Local Cache Items" onmouseover="EPi.ToolButton.MouseDownHandler(this)" onmouseout="EPi.ToolButton.ResetMouseDownHandler(this)" />
                    </span>
                    <span class="epi-cmsButton">
                        <input class="epi-cmsButton-text epi-cmsButton-tools epi-cmsButton-Delete" type="submit" name="removeLocalRemoteCache" id="removeLocalRemoteCacheBottom" value="Remove Local and Remote Cache Items" onmouseover="EPi.ToolButton.MouseDownHandler(this)" onmouseout="EPi.ToolButton.ResetMouseDownHandler(this)" />
                    </span>
                </div>
            <% } %>
        </div>
    </div>

    <script language="JavaScript">
        function toggle(source) {
            checkboxes = document.getElementsByName('cacheKeys');
            for (var i = 0, n = checkboxes.length; i < n; i++) {
                checkboxes[i].checked = source.checked;
            }
        }
    </script>
</asp:Content>