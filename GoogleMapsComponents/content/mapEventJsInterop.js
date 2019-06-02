
window.googleMapEventJsFunctions = {

    addMapEvent2: function (guid, mapId, eventFunctionName, eventName, dotNetHandler) {
        console.log("Add event for map : " + mapId + ", event : " + guid + ", " + eventName);

        if (window._blazorGoogleMaps === null || window._blazorGoogleMaps === 'undefined') {
            console.error("maps collection is not initialize.");
            return false;
        }

        window._blazorMapEvents = window._blazorMapEvents || [];

        window._blazorMapEvents[guid] = window._blazorGoogleMaps[mapId][eventFunctionName](eventName, async function (args) {
            //console.log("Event " + eventName + " fired.");
            //console.dir(args);

            //window._blazorMapEventArgs = window._blazorMapEventArgs || [];

            let jsonString = JSON.stringify(args);

            await dotNetHandler.invokeMethodAsync('Invoke', jsonString);
        });

        return true;
    },

    addListener2: function (guid, mapId, eventName, dotNetInstance) {
        return window.googleMapEventJsFunctions.addMapEvent2(
            guid, mapId, "addListener", eventName, dotNetInstance);
    },

    addListenerOnce2: function (guid, mapId, eventName, dotNetInstance) {
        return window.googleMapEventJsFunctions.addMapEvent2(
            guid, mapId, "addListenerOnce", eventName, dotNetInstance);
    },

    removeListener: function (guid) {
        var eventRef = window._blazorMapEvents[guid];
        eventRef.remove();
    },

    clearListeners: function (divId, eventName) {
        window._blazorGoogleMaps[divId].clearListeners(eventName);
    },

    clearInstanceListeners: function (divId) {
        window._blazorGoogleMaps[divId].clearInstanceListeners();
    },

    addMarkerListener: function (eventGuid, markerGuid, eventName) {
        console.log("Add listener for marker : " + markerGuid + ", event : " + eventGuid + ", " + eventName);

        if (window._blazorGoogleMapsMarkers === null || window._blazorGoogleMapsMarkers === 'undefined') {
            console.log("markers collection is not initialize.");
            return false;
        }

        window._blazorMapEvents = window._blazorMapEvents || [];

        window._blazorMapEvents[eventGuid] = window._blazorGoogleMapsMarkers[markerGuid].addListener(eventName, async function (args) {
            //console.log("Event " + eventName + " fired.");

            let timestamp = + new Date();
            let eventArgId = eventGuid + "_" + timestamp;

            window._blazorMapEventArgs = window._blazorMapEventArgs || [];

            if (args !== null && typeof args !== 'undefined') {
                console.dir(args);

                args["id"] = eventArgId;
                window._blazorMapEventArgs[eventArgId] = args;
            }

            let jsonString = JSON.stringify(args);

            await DotNet.invokeMethodAsync('GoogleMapsComponents', 'NotifyMarkerEvent', eventGuid, jsonString)
                .then(_ => {
                    console.log("Remove event args : " + eventArgId);
                    delete window._blazorMapEventArgs[eventArgId];
                });
        });

        return true;
    },

    invokeEventArgsFunction: function (id, functionName) {
        //console.log("Invoke event function : " + functionName + "for event id : " + id);
        //console.dir(window._blazorMapEventArgs);
        //console.dir(window._blazorMapEventArgs[id]);

        window._blazorMapEventArgs[id][functionName]();

        return true;
    }
};