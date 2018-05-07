<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<DeveloperTools.Models.ModuleDependencyViewModel>" MasterPageFile="../Shared/DeveloperTools.Master" %>
<%@ Import Namespace="Newtonsoft.Json" %>
<%@ Import Namespace="Newtonsoft.Json.Serialization" %>

<asp:Content ID="Styles" runat="server" ContentPlaceHolderID="HeaderStyles">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/vis/4.21.0/vis-network.min.css" integrity="sha256-tTIVWrgsLDcekkoaiqePYP86joMAiyp4KqEswPMmTfQ=" crossorigin="anonymous" />
    <style type="text/css">
        #graphContainer {
            margin: 0 auto;
            display: block;
            width: calc(100vw - 100px);
            height: calc(100vh - 200px);
        }

        #modulesnetwork {
            width: 100%;
            height: 100%;
            border: 1px solid red;
        }
    </style>
</asp:Content>

<asp:Content ID="Content" runat="server" ContentPlaceHolderID="MainRegion">
    <h2>Module Dependencies</h2>
    <div class="controls">
        <div class="control-wrapper"></div>
        <button class="form-control button">all modules</button>
    </div>
    <div id="graphContainer">
        <div id="modulesnetwork"></div>
    </div>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/vis/4.21.0/vis.min.js" integrity="sha256-JuQeAGbk9rG/EoRMixuy5X8syzICcvB0dj3KindZkY0=" crossorigin="anonymous"></script>
    <script type="text/javascript">
        var nodes = <%= JsonConvert.SerializeObject(Model.Nodes, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }) %>,
            edges = <%= JsonConvert.SerializeObject(Model.Links, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }) %>,
            network,
            allNodes,
            highlightActive = false;

        var nodesDataset = new vis.DataSet(nodes);
        var edgesDataset = new vis.DataSet(edges);

        function redrawAll() {
            var container = document.getElementById('modulesnetwork');
            var options = {
                nodes: {
                    shape: 'dot',
                    shadow: {
                        enabled: true
                    },
                    scaling: {
                        min: 10,
                        max: 100,
                        label: {
                            enabled: true,
                            min: 8,
                            max: 30,
                            drawThreshold: 10,
                            maxVisible: 50
                        }
                    },
                    font: {
                        size: 12,
                        face: 'Tahoma'
                    }
                },
                edges: {
                    width: 0.5,
                    arrows: {
                        to: {
                            enabled: true
                        }
                    },
                    smooth: {
                        type: 'cubicBezier'
                    },
                    color: { inherit: 'from' }
                },
                physics: {
                    maxVelocity: 37,
                    minVelocity: 0.79,
                    stabilization: {
                        iterations: 100
                    },
                    barnesHut: {
                        gravitationalConstant: -5000,
                        centralGravity: 0.1,
                        springLength: 225,
                        springConstant: 0.05,
                        avoidOverlap: 1
                    }
                },

                autoResize: true,
                width: '100%',
                height: '100%',
                interaction: {
                    tooltipDelay: 200
                }
            };

            var data = { nodes: nodesDataset, edges: edgesDataset };
            network = new vis.Network(container, data, options);
            allNodes = nodesDataset.get({ returnType: "Object" });
            network.on("click", neighbourhoodHighlight);
        }

        function neighbourhoodHighlight(params) {
            // if something is selected:
            if (params.nodes.length > 0) {
                highlightActive = true;
                var i, j;
                var selectedNode = params.nodes[0];
                var degrees = 2;

                // mark all nodes as hard to read.
                for (var nodeId in allNodes) {
                    allNodes[nodeId].color = 'rgba(200,200,200,0.5)';
                    if (allNodes[nodeId].hiddenLabel === undefined) {
                        allNodes[nodeId].hiddenLabel = allNodes[nodeId].label;
                        allNodes[nodeId].label = undefined;
                    }
                }

                var connectedNodes = network.getConnectedNodes(selectedNode);
                var allConnectedNodes = [];

                // get the second degree nodes
                for (i = 1; i < degrees; i++) {
                    for (j = 0; j < connectedNodes.length; j++) {
                        allConnectedNodes = allConnectedNodes.concat(network.getConnectedNodes(connectedNodes[j]));
                    }
                }

                // all second degree nodes get a different color and their label back
                for (i = 0; i < allConnectedNodes.length; i++) {
                    allNodes[allConnectedNodes[i]].color = 'rgba(150,150,150,0.75)';
                    if (allNodes[allConnectedNodes[i]].hiddenLabel !== undefined) {
                        allNodes[allConnectedNodes[i]].label = allNodes[allConnectedNodes[i]].hiddenLabel;
                        allNodes[allConnectedNodes[i]].hiddenLabel = undefined;
                    }
                }

                // all first degree nodes get their own color and their label back
                for (i = 0; i < connectedNodes.length; i++) {
                    allNodes[connectedNodes[i]].color = undefined;
                    if (allNodes[connectedNodes[i]].hiddenLabel !== undefined) {
                        allNodes[connectedNodes[i]].label = allNodes[connectedNodes[i]].hiddenLabel;
                        allNodes[connectedNodes[i]].hiddenLabel = undefined;
                    }
                }

                // the main node gets its own color and its label back.
                allNodes[selectedNode].color = undefined;
                if (allNodes[selectedNode].hiddenLabel !== undefined) {
                    allNodes[selectedNode].label = allNodes[selectedNode].hiddenLabel;
                    allNodes[selectedNode].hiddenLabel = undefined;
                }
            } else if (highlightActive === true) {
                // reset all nodes
                for (var nodeId in allNodes) {
                    allNodes[nodeId].color = undefined;
                    if (allNodes[nodeId].hiddenLabel !== undefined) {
                        allNodes[nodeId].label = allNodes[nodeId].hiddenLabel;
                        allNodes[nodeId].hiddenLabel = undefined;
                    }
                }
                highlightActive = false;
            }

            // transform the object into an array
            var updateArray = [];
            for (nodeId in allNodes) {
                if (allNodes.hasOwnProperty(nodeId)) {
                    updateArray.push(allNodes[nodeId]);
                }
            }

            nodesDataset.update(updateArray);
        }

        redrawAll();
    </script>
</asp:Content>