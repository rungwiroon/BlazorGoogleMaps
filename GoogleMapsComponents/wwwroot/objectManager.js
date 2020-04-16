function stringToFunction(str) {
    let arr = str.split(".");

    let fn = window || this;
    for (let i = 0, len = arr.length; i < len; i++) {
        fn = fn[arr[i]];
    }

    if (typeof fn !== "function") {
        throw new Error("function not found");
    }

    return fn;
}

function tryParseJson(item) {
    //console.log(item);

    if (item !== null
        && typeof item === "object"
        && "invokeMethodAsync" in item) {
        //console.log("wrap dotnet object ref");

        return async function (...args) {
            if (args === null || typeof args === "undefined")
                await item.invokeMethodAsync("Invoke");

            //console.log(args);

            //let args2 = args.map(arg => {
            //    if (typeof arg === "object" && "toJson" in arg) {
            //        console.log("toJson");
            //        return arg.toJson();
            //    } else {
            //        return arg;
            //    }
            //});

            //console.log(args);

            var guid = googleMapsObjectManager.addObject(args[0]);

            await item.invokeMethodAsync("Invoke", JSON.stringify(args), guid);

            googleMapsObjectManager.disposeObject(guid);
        };
    }

    if (typeof item !== "string")
        return item;

    let item2 = null;

    try {
        item2 = JSON.parse(item);
    } catch (e) {
        return item.replace(/['"]+/g, '');
    }

    if (typeof item2 === "object" && item2 !== null) {
        if ("guidString" in item2) {
            //console.log("Found object has Guid property.");
            return window._blazorGoogleMapsObjects[item2.guidString];
        } else {
            for (var propertyName in item2) {
                let propertyValue = item2[propertyName];
                if (typeof propertyValue === "object"
                    && propertyValue !== null
                    && "guidString" in propertyValue) {
                    //console.log("Found object has Guid property.");
                    item2[propertyName] = window._blazorGoogleMapsObjects[propertyValue.guidString];
                }
            }

            return item2;
        }
    }

    return item.replace(/['"]+/g, '');
}

function uuidv4() {
    return ([1e7] + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, c =>
        (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
    );
}

//Strips the DirectionResult from some of the heaviest collections.
function cleanDirectionResult(dirResponse) {
    let tmpdirobj = JSON.parse(JSON.stringify(dirResponse));

    tmpdirobj.routes.forEach((r) => {
      //  r.overview_path = [];
        r.overview_polyline = [];

        r.legs.forEach((l) => {
            l.lat_lngs = [];
            l.path = [];
            l.steps = [];
        });
    });

    return tmpdirobj;
}

window.googleMapsObjectManager = {
    createObject: function (args) {
        window._blazorGoogleMapsObjects = window._blazorGoogleMapsObjects || [];

        let args2 = args.slice(2).map(arg => tryParseJson(arg));
        //console.log(args2);
        let functionName = args[1];
        let constructor = stringToFunction(functionName);
        let obj = new constructor(...args2);
        let guid = args[0];

        if ("set" in obj) {
            obj.set("guidString", guid);
        }

        window._blazorGoogleMapsObjects[guid] = obj;
    },

    addObject: function (obj, guid) {
        if (guid === null || typeof guid === "undefined") {
            guid = uuidv4();
        }

        window._blazorGoogleMapsObjects = window._blazorGoogleMapsObjects || [];
        window._blazorGoogleMapsObjects[guid] = obj;

        return guid;
    },

    disposeObject: function (guid) {
        delete window._blazorGoogleMapsObjects[guid];
    },

    invoke: async function (args) {
        let args2 = args.slice(2).map(arg => tryParseJson(arg));

        let obj = window._blazorGoogleMapsObjects[args[0]];


        //If function is route, then handle callback in promise.
        if (args[1] == "googleMapDirectionServiceFunctions.route") {
            let dirRequest = args2[0];

            let promise = new Promise((resolve, reject) => {
                let directionsService = new google.maps.DirectionsService();
                directionsService.route(dirRequest, (result, status) => {
                    if (status == 'OK') {
                        resolve(result);
                    }
                    else {
                        reject(status);
                    }
                });
            });

            //Wait for promise
            try {
                let result = await promise;
                obj.setDirections(result);
                
                let jsonRest = JSON.stringify(cleanDirectionResult(result));
                //let jsonRest = cleanDirectionResult(result);
                //console.log(JSON.stringify(jsonRest));
                //let jsonRest = JSON.stringify(result);
                return jsonRest;
            } catch (error) {
                console.log(error);
                return error;
            }

        }
        else {
            var result = null;
            try {
                result = obj[args[1]](...args2);
            } catch (e) {
                console.log(e);
            }

            if (result !== null
                && typeof result === "object") {
                if (result.hasOwnProperty("geocoded_waypoints") && result.hasOwnProperty("routes")) {
                    
                    let jsonRest = JSON.stringify(cleanDirectionResult(result));
                    return jsonRest;
                }
                if ("getArray" in result) {
                    return result.getArray();
                }
                if ("get" in result) {
                    return result.get("guidString");
                } else if ("dotnetTypeName" in result) {
                    return JSON.stringify(result);
                } else {
                    return result;
                }
            } else {
                return result;
            }
        }
    },

    invokeWithReturnedObjectRef: function (args) {
        //console.log(args);

        let result = googleMapsObjectManager.invoke(args);
        let uuid = uuidv4();

        //console.log("invokeWithReturnedObjectRef " + uuid);

        window._blazorGoogleMapsObjects[uuid] = result;

        return uuid;
    },

    readObjectPropertyValue: function (args) {
        let obj = window._blazorGoogleMapsObjects[args[0]];

        return obj[args[1]];
    },

    readObjectPropertyValueWithReturnedObjectRef: function (args) {
        //console.log(args);

        let obj = window._blazorGoogleMapsObjects[args[0]];

        //console.log(obj);

        let result = obj[args[1]];
        let uuid = uuidv4();

        window._blazorGoogleMapsObjects[uuid] = result;

        return uuid;
    }

    //invokeAsync: async function (guid, methodName, jsonArgs) {
    //    console.log("Invoke : route " + guid);

    //    //console.log("Invoke " + methodName);
    //    //console.dir(args);

    //    let args = [];

    //    if (typeof args === 'undefined') {
    //        args = JSON.parse(jsonArgs);
    //    }

    //    let obj = window._blazorGoogleMapsObjects[guid];

    //    let promise = new Promise((resolve, reject) => {
    //        return obj[methodName](...args, function (...args2) {

    //            //console.dir(args2);

    //            resolve(JSON.stringify(args2));
    //        });
    //    });

    //    return await promise;
    //}
};
