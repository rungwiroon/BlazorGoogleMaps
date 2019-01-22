
window.googleMapPolygonJsFunctions = {
    init: function (guid, jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let options = args[0];

        console.log("Init polyline" + guid);
        console.dir(options);

        if (options.map !== null && typeof options.map !== 'undefined')
            options.map = window._blazorGoogleMaps[options.map];

        window._blazorGoogleMapsPolygons = window._blazorGoogleMapsPolygons || [];
        window._blazorGoogleMapsPolygons[guid] = new google.maps.Polygon(options);

        return true;
    },

    dispose: function (guid) {
        let polygon = window._blazorGoogleMapsPolygons[guid];
        polygon.setMap(null);
        delete window._blazorGoogleMapsPolygons[guid];

        return true;
    },

    invoke: function (guid, methodName, jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let polygon = window._blazorGoogleMapsPolygons[guid];

        //console.dir(polygon);

        if (typeof args === 'undefined') {
            polygon[methodName]();
        } else {
            polygon[methodName](...args);
        }

        return true;
    },

    setMap: function (guid, mapDivId) {
        let polygon = window._blazorGoogleMapsPolygons[guid];
        let map = window._blazorGoogleMaps[mapDivId];

        polygon.setMap(map);
    }
};