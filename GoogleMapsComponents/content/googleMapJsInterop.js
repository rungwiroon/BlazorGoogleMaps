
window.googleMapJsFunctions = {
    init: function (jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let id = args[0];
        let mapOptions = args[1];

        console.log("init google map " + id + " with options : ");
        console.dir(mapOptions);

        window._blazorGoogleMaps = window._blazorGoogleMaps || [];

        window._blazorGoogleMaps[id] = new google.maps.Map(document.getElementById(id), mapOptions);

        return true;
    },

    initByElement: function (id, element, optionsJson) {

        console.log("init google map " + id + " with options : ");
        console.dir(options);

        let options = JSON.parse(optionsJson);

        window._blazorGoogleMaps = window._blazorGoogleMaps || [];

        window._blazorGoogleMaps[id] = new google.maps.Map(element, options);

        return true;
    },

    dispose: function (id) {
        console.log("Dispose map " + id);

        delete window._blazorGoogleMaps[id];

        return true;
    },

    invoke: function (guid, methodName, jsonArgs) {
        let args = JSON.parse(jsonArgs);
        let map = window._blazorGoogleMaps[guid];

        console.log("Invoke " + methodName);
        console.dir(args);

        if (typeof args === 'undefined') {
            return map[methodName]();
        } else {
            return map[methodName](...args);
        }
    }
};
