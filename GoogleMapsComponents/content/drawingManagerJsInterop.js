
window.googleMapDrawingManagerJsFunctions = {
    init: function (jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let guid = args[0];
        let options = args[1];

        console.log("Init drawing manager" + guid);
        console.dir(options);

        if (options.map !== null && typeof options.map !== 'undefined')
            options.map = window._blazorGoogleMaps[options.map];

        window._blazorGoogleMapsDrawingManager = window._blazorGoogleMapsDrawingManager || [];
        window._blazorGoogleMapsDrawingManager[guid] = new google.maps.drawing.DrawingManager(options);

        return true;
    },

    dispose: function (guid) {
        let marker = window._blazorGoogleMapsDrawingManager[guid];
        marker.setMap(null);
        delete window._blazorGoogleMapsDrawingManager[guid];

        return true;
    },

    invoke: function (guid, methodName, jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let marker = window._blazorGoogleMapsDrawingManager[guid];

        console.log("Invoke " + methodName);
        console.dir(args);

        if (typeof args === 'undefined') {
            return marker[methodName]();
        } else {
            return marker[methodName](...args);
        }
    },

    setMap: function (jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let guid = args[0];
        let mapId = args[1];

        let marker = window._blazorGoogleMapsDrawingManager[guid];
        let map = null;

        if (mapId !== null && typeof mapIdp !== 'undefined')
            map = window._blazorGoogleMaps[mapId];

        marker.setMap(map);
        return true;
    }
};