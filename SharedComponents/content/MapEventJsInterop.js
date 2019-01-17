
window.googleMapEventJsFunctions = {
    addListener: function (guid, mapId, eventName) {
        console.log("Add listener for map : " + mapId + ", event : " + guid + ", " + eventName);

        if (window._blazorGoogleMaps === null || window._blazorGoogleMaps === 'undefined') {
            console.log("maps collection is not initialize.");
            return false;
        }

        window._blazorMapEvents = window._blazorMapEvents || [];

        window._blazorMapEvents[guid] = window._blazorGoogleMaps[mapId].addListener(eventName, async function (args) {
            console.log("Event " + eventName + " fired.");
            console.dir(args);
            await DotNet.invokeMethodAsync('SharedComponents', 'NotifyMapEvent', guid, args);
        });

        return true;
    },

    removeListener: function (guid) {
        var eventRef = window._blazorMapEvents[guid];
        eventRef.remove();
    },

    addMarkerListener: function (eventGuid, markerGuid, eventName) {
        console.log("Add listener for marker : " + markerGuid + ", event : " + eventGuid + ", " + eventName);

        if (window._blazoeGoogleMapsMarkers === null || window._blazoeGoogleMapsMarkers === 'undefined') {
            console.log("markers collection is not initialize.");
            return false;
        }

        window._blazorMapEvents = window._blazorMapEvents || [];

        window._blazorMapEvents[eventGuid] = window._blazoeGoogleMapsMarkers[markerGuid].addListener(eventName, async function (args) {
            console.log("Event " + eventName + " fired.");
            console.dir(args);
            await DotNet.invokeMethodAsync('SharedComponents', 'NotifyMarkerEvent', eventGuid, args);
        });

        return true;
    }
}