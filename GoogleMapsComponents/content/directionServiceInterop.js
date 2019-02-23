
window.googleMapDirectionServiceFunctions = {
    init: function (guid) {

        //console.log("Init rectangle " + guid);
        //console.dir(options);

        window._blazorGoogleMapsDirectionServices = window._blazorGoogleMapsRectangles || [];
        window._blazorGoogleMapsDirectionServices[guid] = new google.maps.DirectionsService();

        return true;
    },

    dispose: function (guid) {
        delete window._blazorGoogleMapsDirectionServices[guid];

        return true;
    },

    //invoke: function (guid, methodName, jsonArgs) {
    //    let args = JSON.parse(jsonArgs);
    //    let rectangle = window._blazorGoogleMapsDirectionServices[guid];

    //    //console.log("Invoke : " + methodName);
    //    //console.dir(rectangle);

    //    if (typeof args === 'undefined') {
    //        rectangle[methodName]();
    //    } else {
    //        //console.log("Invoke : " + methodName + " with args.");
    //        rectangle[methodName](...args);
    //    }

    //    return true;
    //},

    route: async function (guid, jsonArgs) {
        console.log("Invoke : route " + guid);
        console.dir(window._blazorGoogleMapsDirectionServices);

        let args = JSON.parse(jsonArgs);
        let directionService = window._blazorGoogleMapsDirectionServices[guid];
        let promise = new Promise((resolve, reject) => {
            //setTimeout(() => resolve(true), 1000) // resolve

            directionService.route(args[0], function (response, status) {

                //console.dir(response);

                resolve(JSON.stringify({ response: response, status: status }));
            });
        });

        return await promise;
    }
};