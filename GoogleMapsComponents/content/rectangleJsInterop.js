
window.googleMapRectangleJsFunctions = {
    init: function (guid, jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let options = args[0];

        console.log("Init rectangle " + guid);
        console.dir(options);

        if (options.map !== null && typeof options.map !== 'undefined')
            options.map = window._blazorGoogleMaps[options.map];

        window._blazorGoogleMapsRectangles = window._blazorGoogleMapsRectangles || [];
        window._blazorGoogleMapsRectangles[guid] = new google.maps.Rectangle(options);

        return true;
    },

    dispose: function (guid) {
        let rectangle = window._blazorGoogleMapsRectangles[guid];
        rectangle.setMap(null);
        delete window._blazorGoogleMapsRectangles[guid];

        return true;
    },

    invoke: function (guid, methodName, jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let rectangle = window._blazorGoogleMapsRectangles[guid];

        //console.log("Invoke : " + methodName);
        //console.dir(rectangle);

        if (typeof args === 'undefined') {
            rectangle[methodName]();
        } else {
            //console.log("Invoke : " + methodName + " with args.");
            rectangle[methodName](...args);
        }

        return true;
    },

    setMap: function (guid, mapDivId) {
        let rectangle = window._blazorGoogleMapsRectangles[guid];
        let map = window._blazorGoogleMaps[mapDivId];

        rectangle.setMap(map);
    }
};