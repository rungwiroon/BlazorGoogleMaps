
window.googleMapHeatmapLayerJsFunctions = {
    init: function (guid, jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let options = args[0];

        //console.log("Init rectangle " + guid);
        //console.dir(options);

        if (options.map !== null && typeof options.map !== 'undefined')
            options.map = window._blazorGoogleMaps[options.map];

        window._blazorGoogleMapsHeatmapLayers = window._blazorGoogleMapsRectangles || [];
        window._blazorGoogleMapsHeatmapLayers[guid] = new google.maps.visualization.HeatmapLayer(options);

        return true;
    },

    dispose: function (guid) {
        let rectangle = window._blazorGoogleMapsHeatmapLayers[guid];
        rectangle.setMap(null);
        delete window._blazorGoogleMapsHeatmapLayers[guid];

        return true;
    },

    invoke: function (guid, methodName, jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let heatmapLayer = window._blazorGoogleMapsHeatmapLayers[guid];

        //console.log("Invoke : " + methodName);
        //console.dir(rectangle);

        if (typeof args === 'undefined') {
            heatmapLayer[methodName]();
        } else {
            //console.log("Invoke : " + methodName + " with args.");
            heatmapLayer[methodName](...args);
        }

        return true;
    },

    setMap: function (guid, mapDivId) {
        let heatmapLayer = window._blazorGoogleMapsHeatmapLayers[guid];
        let map = window._blazorGoogleMaps[mapDivId];

        heatmapLayer.setMap(map);
    }
};