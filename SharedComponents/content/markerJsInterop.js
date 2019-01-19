
window.googleMapMarkerJsFunctions = {
    init: function (jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let guid = args[0];
        let options = args[1];

        console.log("Init marker" + guid);
        console.dir(options);

        if (options.map !== null && options.map !== 'undefined')
            options.map = window._blazorGoogleMaps[options.map];

        window._blazorGoogleMapsMarkers = window._blazorGoogleMapsMarkers || [];
        window._blazorGoogleMapsMarkers[guid] = new google.maps.Marker(options);

        return true;
    },

    dispose: function (guid) {
        let marker = window._blazorGoogleMapsMarkers[guid];
        marker.setMap(null);
        delete window._blazorGoogleMapsMarkers[guid];

        return true;
    },

    getAnimation: function (guid) {
        let marker = window._blazorGoogleMapsMarkers[guid];
        return marker.getAnimation();
    },

    getClickable: function (guid) {
        let marker = window._blazorGoogleMapsMarkers[guid];
        return marker.getClickable();
    },

    getCursor: function (guid) {
        let marker = window._blazorGoogleMapsMarkers[guid];
        return marker.getCursor();
    },

    getDraggable: function (guid) {
        let marker = window._blazorGoogleMapsMarkers[guid];
        return marker.getDraggable();
    },

    getIcon: function (guid) {
        let marker = window._blazorGoogleMapsMarkers[guid];
        return marker.getIcon();
    },

    getLabel: function (guid) {
        let marker = window._blazorGoogleMapsMarkers[guid];
        return marker.getLabel();
    },

    getMap: function (guid) {
        let marker = window._blazorGoogleMapsMarkers[guid];
        return marker.getMap().getDiv().getAttribute('id');
    },

    getPosition: function (guid) {
        let marker = window._blazorGoogleMapsMarkers[guid];
        return marker.getPosition();
    },

    getShape: function (guid) {
        let marker = window._blazorGoogleMapsMarkers[guid];
        return marker.getShape();
    },

    getTitle: function (guid) {
        let marker = window._blazorGoogleMapsMarkers[guid];
        return marker.getTitle();
    },

    getVisible: function (guid) {
        let marker = window._blazorGoogleMapsMarkers[guid];
        return marker.getVisible();
    },

    getZIndex: function (guid) {
        let marker = window._blazorGoogleMapsMarkers[guid];
        return marker.getZIndex();
    },

    setAnimation: function (jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let guid = args[0];
        let animation = args[1];

        let marker = window._blazorGoogleMapsMarkers[guid];
        marker.setAnimation(animation);
        return true;
    },

    setClickable: function (jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let guid = args[0];
        let flag = args[1];

        let marker = window._blazorGoogleMapsMarkers[guid];
        marker.setClickable(flag);
        return true;
    },

    setCursor: function (jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let guid = args[0];
        let cursor = args[1];

        let marker = window._blazorGoogleMapsMarkers[guid];
        marker.setCursor(cursor);
        return true;
    },

    setDraggable: function (jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let guid = args[0];
        let flag = args[1];

        let marker = window._blazorGoogleMapsMarkers[guid];
        marker.setDraggable(flag);
        return true;
    },

    setIcon: function (jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let guid = args[0];
        let icon = args[1];

        let marker = window._blazorGoogleMapsMarkers[guid];
        marker.setIcon(icon);
        return true;
    },

    setLabel: function (jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let guid = args[0];
        let label = args[1];

        let marker = window._blazorGoogleMapsMarkers[guid];
        marker.setLabel(label);
        return true;
    },

    setMap: function (jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let guid = args[0];
        let mapId = args[1];

        let marker = window._blazorGoogleMapsMarkers[guid];
        let map = null;

        if (mapId !== null && typeof mapIdp !== 'undefined')
            map = window._blazorGoogleMaps[mapId];

        marker.setMap(map);
        return true;
    },

    setOpacity: function (jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let guid = args[0];
        let opacity = args[1];

        let marker = window._blazorGoogleMapsMarkers[guid];
        marker.setOpacity(opacity);
        return true;
    },

    setOptions: function (jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let guid = args[0];
        let options = args[1];

        let marker = window._blazorGoogleMapsMarkers[guid];

        if (options.map !== null && typeof options.map !== 'undefined')
            options.map = window._blazorGoogleMaps[options.map];

        marker.setOptions(options);

        return true;
    },

    setPosition: function (jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let guid = args[0];
        let latLng = args[1];

        let marker = window._blazorGoogleMapsMarkers[guid];
        marker.setPosition(latLng);
        return true;
    },

    setShape: function (jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let guid = args[0];
        let shape = args[1];

        let marker = window._blazorGoogleMapsMarkers[guid];
        marker.setShape(shape);
        return true;
    },

    setTitle: function (jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let guid = args[0];
        let title = args[1];

        let marker = window._blazorGoogleMapsMarkers[guid];
        marker.setTitle(title);
        return true;
    },

    setVisible: function (jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let guid = args[0];
        let visible = args[1];

        let marker = window._blazorGoogleMapsMarkers[guid];
        marker.setVisible(visible);
        return true;
    },

    setZIndex: function (jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let guid = args[0];
        let zIndex = args[1];

        let marker = window._blazorGoogleMapsMarkers[guid];
        marker.setZIndex(zIndex);
        return true;
    }
};