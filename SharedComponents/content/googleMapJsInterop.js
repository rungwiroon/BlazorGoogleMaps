
window.googleMapJsFunctions = {
    init: function (jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let id = args[0];
        let mapOptions = args[1];

        console.log("init google map " + id + " with options : " + mapOptions);

        window._blazorGoogleMaps = window._blazorGoogleMaps || [];

        window._blazorGoogleMaps[id] = new google.maps.Map(document.getElementById(id), mapOptions);

        return true;
    },

    fitBounds: function (jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let id = args[0];
        let bounds = args[1];

        console.log("Fit bounds : " + id + " " + bounds);

        window._blazorGoogleMaps[id].fitBounds(bounds);

        return true;
    },

    getBounds: function (jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let id = args[0];

        console.log("Get bounds : " + id);

        return window._blazorGoogleMaps[id].getBounds().toJSON();
    },

    getCenter: function (jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let id = args[0];

        console.log("Get center : " + id);

        return window._blazorGoogleMaps[id].getCenter().toJSON();
    },

    getClickableIcons: function (jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let id = args[0];

        return window._blazorGoogleMaps[id].getClickableIcons();
    },

    getHeading: function (jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let id = args[0];

        return window._blazorGoogleMaps[id].getHeading();
    },

    getMapTypeId: function (jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let id = args[0];

        console.log("Get map type for ID : " + id);

        return window._blazorGoogleMaps[id].getMapTypeId();
    },

    getProjection: function (jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let id = args[0];

        return window._blazorGoogleMaps[id].getProjection();
    },

    getStreetView: function (jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let id = args[0];

        return window._blazorGoogleMaps[id].getStreetView();
    },

    getTilt: function (jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let id = args[0];

        return window._blazorGoogleMaps[id].getTilt();
    },

    getZoom: function (jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let id = args[0];

        return window._blazorGoogleMaps[id].getZoom();
    },

    panBy: function (id, x, y) {
        return window._blazorGoogleMaps[id].panBy(x, y);
    },

    panTo: function (id, latLng) {
        return window._blazorGoogleMaps[id].panTo(latLng);
    },

    panToBounds: function (id, latLngBounds) {
        return window._blazorGoogleMaps[id].getHeading(latLngBounds);
    },

    setCenter: function (jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let id = args[0];
        let latLng = args[1];

        console.log("Set center : " + id);

        return window._blazorGoogleMaps[id].setCenter(latLng);
    },

    setClickableIcons: function (jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let id = args[0];
        let value = args[1];

        return window._blazorGoogleMaps[id].setClickableIcons(value);
    },

    setHeading: function (jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let id = args[0];
        let heading = args[1];

        console.log("Set heading : " + id + " : " + heading);

        window._blazorGoogleMaps[id].setHeading(heading);

        return true;
    },

    setMapTypeId: function (jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let id = args[0];
        let mapTypeId = args[1];

        console.log("Set map type : " + id + " : " + mapTypeId);

        return window._blazorGoogleMaps[id].setMapTypeId(mapTypeId);
    },

    setOptions: function (id, options) {
        return window._blazorGoogleMaps[id].setOptions(options);
    },

    setStreetView: function (id, panorama) {
        return window._blazorGoogleMaps[id].setStreetView(panorama);
    },

    setTilt: function (id, tilt) {
        return window._blazorGoogleMaps[id].setTilt(tilt);
    },

    setZoom: function (id, zoom) {
        return window._blazorGoogleMaps[id].setZoom(zoom);
    }
};
