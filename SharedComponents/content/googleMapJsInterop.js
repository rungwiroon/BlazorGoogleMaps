
window.googleMapJsFunctions = {
    init: function (jsonArgs) {
        //return prompt(message, 'Type anything here');
        let args = JSON.parse(jsonArgs);
        let id = args[0];
        let mapOptions = args[1];

        console.log("init google map " + id + " with options : " + mapOptions);

        window._blazorGoogleMaps = [];
        //console.log("init google map 2 " + id);

        window._blazorGoogleMaps[id] = new google.maps.Map(document.getElementById(id), mapOptions);

        //console.log(document.getElementById(id));

        //let gMap = new google.maps.Map(document.getElementById(id));
        console.log("init google map 2 " + id);

        return true;
    },

    fitBounds: function (id, bounds) {
        window._blazorGoogleMaps[id].fitBounds(bounds);
    },

    getBounds: function (id) {
        return window._blazorGoogleMaps[id].getBounds();
    },

    getCenter: function (id) {
        return window._blazorGoogleMaps[id].getCenter();
    },

    getClickableIcons: function (id) {
        return window._blazorGoogleMaps[id].getClickableIcons();
    },

    getHeading: function (id) {
        return window._blazorGoogleMaps[id].getHeading();
    },

    getMapTypeId: function (id) {
        return window._blazorGoogleMaps[id].getMapTypeId();
    },

    getProjection: function (id) {
        return window._blazorGoogleMaps[id].getProjection();
    },

    getStreetView: function (id) {
        return window._blazorGoogleMaps[id].getStreetView();
    },

    getTilt: function (id) {
        return window._blazorGoogleMaps[id].getTilt();
    },

    getZoom: function (id) {
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

    setCenter: function (id, latlng) {
        return window._blazorGoogleMaps[id].setCenter(latlng);
    },

    setClickableIcons: function (id, value) {
        return window._blazorGoogleMaps[id].setClickableIcons(value);
    },

    setHeading: function (id, heading) {
        return window._blazorGoogleMaps[id].setHeading(heading);
    },

    setMapTypeId: function (id, mapTypeId) {
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
