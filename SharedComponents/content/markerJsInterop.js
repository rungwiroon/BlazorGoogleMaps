
window.googleMapMarkerJsFunctions = {
    init: function (jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let guid = args[0];
        let options = args[1];

        console.log("Init marker" + guid);
        console.dir(options);

        if (options.map !== null && options.map !== 'undefined')
            options.map = window._blazorGoogleMaps[options.map];

        window._blazoeGoogleMapsMarkers = window._blazoeGoogleMapsMarkers || [];
        window._blazoeGoogleMapsMarkers[guid] = new google.maps.Marker(options);

        return true;
    },

    getAnimation: function (guid) {
        let marker = window._blazoeGoogleMapsMarkers[guid];
        return marker.getAnimation();
    },

    getClickable: function (guid) {
        let marker = window._blazoeGoogleMapsMarkers[guid];
        return marker.getClickable();
    },

    getCursor: function (guid) {
        let marker = window._blazoeGoogleMapsMarkers[guid];
        return marker.getCursor();
    },

    getDraggable: function (guid) {
        let marker = window._blazoeGoogleMapsMarkers[guid];
        return marker.getDraggable();
    },

    getIcon: function (guid) {
        let marker = window._blazoeGoogleMapsMarkers[guid];
        return marker.getIcon();
    },

    getLabel: function (guid) {
        let marker = window._blazoeGoogleMapsMarkers[guid];
        return marker.getLabel();
    },

    getMap: function (guid) {
        let marker = window._blazoeGoogleMapsMarkers[guid];
        return marker.getMap().getDiv().getAttribute('id');
    },

    getPosition: function (guid) {
        let marker = window._blazoeGoogleMapsMarkers[guid];
        return marker.getPosition();
    },

    getShape: function (guid) {
        let marker = window._blazoeGoogleMapsMarkers[guid];
        return marker.getShape();
    },

    getTitle: function (guid) {
        let marker = window._blazoeGoogleMapsMarkers[guid];
        return marker.getTitle();
    },

    getVisible: function (guid) {
        let marker = window._blazoeGoogleMapsMarkers[guid];
        return marker.getVisible();
    },

    getZIndex: function (guid) {
        let marker = window._blazoeGoogleMapsMarkers[guid];
        return marker.getZIndex();
    },

    setAnimation: function (jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let guid = args[0];
        let animation = args[1];

        let marker = window._blazoeGoogleMapsMarkers[guid];
        marker.setAnimation(animation);
        return true;
    },

    setClickable: function (jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let guid = args[0];
        let flag = args[1];

        let marker = window._blazoeGoogleMapsMarkers[guid];
        marker.setClickable(flag);
        return true;
    },

    setCursor: function (jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let guid = args[0];
        let cursor = args[1];

        let marker = window._blazoeGoogleMapsMarkers[guid];
        marker.setCursor(cursor);
        return true;
    },

    setDraggable: function (jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let guid = args[0];
        let flag = args[1];

        let marker = window._blazoeGoogleMapsMarkers[guid];
        marker.setDraggable(flag);
        return true;
    },

    setIcon: function (jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let guid = args[0];
        let icon = args[1];

        let marker = window._blazoeGoogleMapsMarkers[guid];
        marker.setIcon(icon);
        return true;
    },

    setLabel: function (jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let guid = args[0];
        let label = args[1];

        let marker = window._blazoeGoogleMapsMarkers[guid];
        marker.setLabel(label);
        return true;
    },

    setMap: function (jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let guid = args[0];
        let mapId = args[1];

        let marker = window._blazoeGoogleMapsMarkers[guid];
        let map = null;

        if (mapId !== null && mapIdp !== 'undefined')
            map = window._blazorGoogleMaps[mapId];

        marker.setMap(map);
        return true;
    },

    setOpacity: function (jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let guid = args[0];
        let opacity = args[1];

        let marker = window._blazoeGoogleMapsMarkers[guid];
        marker.setOpacity(opacity);
        return true;
    },

    setOptions: function (jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let guid = args[0];
        let options = args[1];

        let marker = window._blazoeGoogleMapsMarkers[guid];

        if (options.map !== null && options.map !== 'undefined')
            options.map = window._blazorGoogleMaps[options.map];

        marker.setOptions(options);

        return true;
    },

    setPosition: function (jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let guid = args[0];
        let latLng = args[1];

        let marker = window._blazoeGoogleMapsMarkers[guid];
        marker.setPosition(latLng);
        return true;
    },

    setShape: function (jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let guid = args[0];
        let shape = args[1];

        let marker = window._blazoeGoogleMapsMarkers[guid];
        marker.setShape(shape);
        return true;
    },

    setTitle: function (jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let guid = args[0];
        let title = args[1];

        let marker = window._blazoeGoogleMapsMarkers[guid];
        marker.setTitle(title);
        return true;
    },

    setVisible: function (jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let guid = args[0];
        let visible = args[1];

        let marker = window._blazoeGoogleMapsMarkers[guid];
        marker.setVisible(visible);
        return true;
    },

    setZIndex: function (jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let guid = args[0];
        let zIndex = args[1];

        let marker = window._blazoeGoogleMapsMarkers[guid];
        marker.setZIndex(zIndex);
        return true;
    }
}