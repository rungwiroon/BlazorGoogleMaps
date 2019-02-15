
window.googleMapInfoWindowJsFunctions = {
    init: function (guid, jsonArgs) {
        let args = JSON.parse(jsonArgs);

        //console.log("Init info window" + guid);
        //console.dir(args);

        //if (options.map !== null && typeof options.map !== 'undefined')
        //    options.map = window._blazorGoogleMaps[options.map];

        window._blazorGoogleMapsInfoWindows = window._blazorGoogleMapsInfoWindows || [];
        window._blazorGoogleMapsInfoWindows[guid] = new google.maps.InfoWindow(...args);

        return true;
    },

    dispose: function (guid) {
        //let infoWindow = window._blazorGoogleMapsInfoWindows[guid];
        //infoWindow.setMap(null);
        delete window._blazorGoogleMapsInfoWindows[guid];

        return true;
    },

    open: function (guid, jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let infoWindow = window._blazorGoogleMapsInfoWindows[guid];
        let map = window._blazorGoogleMaps[args[0]];

        //console.log("Open info window" + guid);

        infoWindow.open(map);

        return true;
    },

    invoke: function (guid, methodName, jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let infoWindow = window._blazorGoogleMapsInfoWindows[guid];

        //console.dir(infoWindow);

        if (typeof args === 'undefined') {
            infoWindow[methodName]();
        } else {
            infoWindow[methodName](...args);
        }

        return true;
    }
}