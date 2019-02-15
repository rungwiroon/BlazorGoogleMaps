
window.googleMapEventJsFunctions = {
    addMapEvent: function (guid, mapId, eventFunctionName, eventName) {
        console.log("Add event for map : " + mapId + ", event : " + guid + ", " + eventName);

        if (window._blazorGoogleMaps === null || window._blazorGoogleMaps === 'undefined') {
            console.error("maps collection is not initialize.");
            return false;
        }

        window._blazorMapEvents = window._blazorMapEvents || [];

        window._blazorMapEvents[guid] = window._blazorGoogleMaps[mapId][eventFunctionName](eventName, async function (args) {
            console.log("Event " + eventName + " fired.");
            console.dir(args);

            let timestamp = + new Date();
            let eventArgsId = guid + "_" + timestamp;

            window._blazorMapEventArgs = window._blazorMapEventArgs || [];

            if (args !== null && typeof args !== 'undefined') {
                args["id"] = eventArgsId;
                window._blazorMapEventArgs[eventArgsId] = args;
            }

            let jsonString = JSON.stringify(args);

            await DotNet.invokeMethodAsync('GoogleMapsComponents', 'NotifyMapEvent', guid, jsonString)
                .then(_ => {
                    console.log("Remove event args : " + eventArgsId);
                    delete window._blazorMapEventArgs[eventArgsId];
                });
        });

        return true;
    },

    addListener: function (guid, mapId, eventName) {
        return window.googleMapEventJsFunctions.addMapEvent(guid, mapId, "addListener", eventName);
    },

    addListenerOnce: function (guid, mapId, eventName) {
        return window.googleMapEventJsFunctions.addMapEvent(guid, mapId, "addListenerOnce", eventName);
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
            console.log("Event " + eventName + " fired.");

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
        console.log("Invoke event function : " + functionName + "for event id : " + id);
        //console.dir(window._blazorMapEventArgs);
        //console.dir(window._blazorMapEventArgs[id]);

        window._blazorMapEventArgs[id][functionName]();

        return true;
    }
};