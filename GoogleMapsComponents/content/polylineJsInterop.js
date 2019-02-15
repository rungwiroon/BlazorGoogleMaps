
window.googleMapPolylineJsFunctions = {
    init: function (guid, jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let options = args[0];

        //console.log("Init polyline " + guid);
        //console.dir(options);

        if (options.map !== null && typeof options.map !== 'undefined')
            options.map = window._blazorGoogleMaps[options.map];

        window._blazorGoogleMapsPolylines = window._blazorGoogleMapsPolylines || [];
        window._blazorGoogleMapsPolylines[guid] = new google.maps.Polyline(options);

        return true;
    },

    dispose: function (guid) {
        let polyline = window._blazorGoogleMapsPolylines[guid];
        polyline.setMap(null);
        delete window._blazorGoogleMapsPolylines[guid];

        return true;
    },

    invoke: function (guid, methodName, jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let polyline = window._blazorGoogleMapsPolylines[guid];

        //console.log(polyline);
        //console.log(methodName);
        //console.dir(args);

        if (typeof args === 'undefined') {
            polyline[methodName]();
        } else {
            polyline[methodName](...args);
        }

        return true;
    },

    setMap: function (guid, mapDivId) {
        let polyline = window._blazorGoogleMapsPolylines[guid];
        let map = window._blazorGoogleMaps[mapDivId];

        polyline.setMap(map);
    }
};