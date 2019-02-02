
window.googleMapCircleJsFunctions = {
    init: function (guid, jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let options = args[0];

        console.log("Init circle" + guid);
        console.dir(options);

        if (options.map !== null && typeof options.map !== 'undefined')
            options.map = window._blazorGoogleMaps[options.map];

        window._blazorGoogleMapsCircles = window._blazorGoogleMapsCircles || [];
        window._blazorGoogleMapsCircles[guid] = new google.maps.Circle(options);

        return true;
    },

    dispose: function (guid) {
        let circle = window._blazorGoogleMapsCircles[guid];
        circle.setMap(null);
        delete window._blazorGoogleMapsCircles[guid];

        return true;
    },

    invoke: function (guid, methodName, jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let circle = window._blazorGoogleMapsCircles[guid];

        //console.dir(polygon);

        if (typeof args === 'undefined') {
            return circle[methodName]();
        } else {
            return circle[methodName](...args);
        }

        //return true;
    },

    setMap: function (guid, mapDivId) {
        let circle = window._blazorGoogleMapsCircles[guid];
        let map = window._blazorGoogleMaps[mapDivId];

        circle.setMap(map);
    }
};