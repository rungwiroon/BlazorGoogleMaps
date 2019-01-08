
window.googleMapEventJsFunctions = {
    addListener: function (guid, mapId, eventName) {
        console.log("Add listener for : " + guid + ", " + mapId + ", " + eventName);

        if (window._blazorGoogleMaps === null || window._blazorGoogleMaps === 'undefined') {
            console.log("maps collection is not initialize.");
            return false;
        }

        window._blazorMapEvents = window._blazorMapEvents || [];

        window._blazorMapEvents[guid] = window._blazorGoogleMaps[mapId].addListener(eventName, async function () {
            console.log("Event " + eventName + " fired.");
            await DotNet.invokeMethodAsync('SharedComponents', 'NotifyEvent', guid);
        });

        return true;
    },

    removeListener: function (guid) {
        var eventRef = window._blazorMapEvents[guid];
        eventRef.remove();
    }
}