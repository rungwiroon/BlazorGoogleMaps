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

const blazorGoogleMapsDateFormat = /^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}/;

function dateObjectReviver(key, value) {
    if (typeof value === "string" && blazorGoogleMapsDateFormat.test(value)) {
        return new Date(value);
    }
    return value;
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

            //Strip circular dependencies, map object and functions
            //https://stackoverflow.com/questions/11616630/how-can-i-print-a-circular-structure-in-a-json-like-format
            const getCircularReplacer = () => {
                const seen = new WeakSet();
                return (key, value) => {
                    if (key == "map") return undefined;
                    if (typeof (value) == 'function') return undefined;

                    if (typeof value === "object" && value !== null) {
                        if (seen.has(value)) {
                            return;
                        }
                        seen.add(value);
                    }
                    return value;
                };
            };


            //OLD code
            //await item.invokeMethodAsync("Invoke", JSON.stringify(args), guid);
            await item.invokeMethodAsync("Invoke", JSON.stringify(args, getCircularReplacer()), guid);

            googleMapsObjectManager.disposeObject(guid);
        };
    }

    if (typeof item !== "string")
        return item;

    let item2 = null;

    try {
        item2 = JSON.parse(item, dateObjectReviver);
    } catch (e) {
        try {
            // added to be able to include js functions in a json object (for ImageMapType).  JSON.parse(...) doesn't do that
            // example json string:
            // "{
            //    'getTileUrl': (coord, zoom) => {
            //                return '" + baseUrl + @"' + zoom + '/' + coord.x + '/' + coord.y;
            //            },
            //    'tileSize': new google.maps.Size(256, 256),
            //    'maxZoom': 23,
            //    'minZoom': 0,
            //    'opacity': 0.5,
            //    'name': 'myLayer'
            // }"
            item2 = eval("(" + item + ")");
        } catch (e2) {
            //Hm. Not sure why this one was here. 
            //Everything looks like working without it
            //return item.replace(/['"]+/g, '');
            return item;
        }
    }

    if (typeof item2 === "string" && item2 !== null) {
        if (item2.startsWith("google.maps.drawing.OverlayType")) {
            switch (item2) {
                case "google.maps.drawing.OverlayType.CIRCLE":
                    item2 = google.maps.drawing.OverlayType.CIRCLE;
                    break;
                case "google.maps.drawing.OverlayType.MARKER":
                    item2 = google.maps.drawing.OverlayType.MARKER;
                    break;
                case "google.maps.drawing.OverlayType.POLYGON":
                    item2 = google.maps.drawing.OverlayType.POLYGON;
                    break;
                case "google.maps.drawing.OverlayType.POLYLINE":
                    item2 = google.maps.drawing.OverlayType.POLYLINE;
                    break;
                case "google.maps.drawing.OverlayType.RECTANGLE":
                    item2 = google.maps.drawing.OverlayType.RECTANGLE;
                    break;
                default:
            }

            return item2;
        }
    }

    if (typeof item2 === "object" && item2 !== null) {
        if ("guidString" in item2) {
            //console.log("Found object has Guid property.");
            return window._blazorGoogleMapsObjects[item2.guidString];
        } else {
            for (var propertyName in item2) {
                let propertyValue = item2[propertyName];
                if (propertyValue !== null && typeof propertyValue === "string" && propertyValue.indexOf("google.maps.Animation") == 0) {
                    switch (propertyValue) {
                        case "google.maps.Animation.DROP":
                            item2[propertyName] = google.maps.Animation.DROP;
                            break;
                        case "google.maps.Animation.BOUNCE":
                            item2[propertyName] = google.maps.Animation.BOUNCE;
                            break;
                        default:
                    }
                }

                if (propertyValue !== null && typeof propertyValue === "string" && propertyValue.indexOf("google.maps.CollisionBehavior") == 0) {
                    switch (propertyValue) {
                        case "google.maps.CollisionBehavior.REQUIRED":
                            item2[propertyName] = google.maps.CollisionBehavior.REQUIRED;
                            break;
                        case "google.maps.CollisionBehavior.REQUIRED_AND_HIDES_OPTIONAL":
                            item2[propertyName] = google.maps.CollisionBehavior.REQUIRED_AND_HIDES_OPTIONAL;
                            break;
                        case "google.maps.CollisionBehavior.OPTIONAL_AND_HIDES_LOWER_PRIORITY":
                            item2[propertyName] = google.maps.CollisionBehavior.OPTIONAL_AND_HIDES_LOWER_PRIORITY;
                            break;
                        default:
                    }
                }

                if (propertyValue !== null && typeof propertyValue === "object" && propertyValue.position !== undefined) {
                    propertyValue.position = getGooglePositionFromString(propertyValue.position);
                }

                if (propertyValue !== null
                    && typeof propertyValue === "object"
                    && "drawingModes" in propertyValue
                    && propertyValue.drawingModes !== undefined) {
                    for (var drawingMode in propertyValue.drawingModes) {
                        let drawingModeValue = propertyValue.drawingModes[drawingMode];
                        switch (drawingModeValue) {
                            case "google.maps.drawing.OverlayType.CIRCLE":
                                propertyValue.drawingModes[drawingMode] = google.maps.drawing.OverlayType.CIRCLE;
                                break;
                            case "google.maps.drawing.OverlayType.MARKER":
                                propertyValue.drawingModes[drawingMode] = google.maps.drawing.OverlayType.MARKER;
                                break;
                            case "google.maps.drawing.OverlayType.POLYGON":
                                propertyValue.drawingModes[drawingMode] = google.maps.drawing.OverlayType.POLYGON;
                                break;
                            case "google.maps.drawing.OverlayType.POLYLINE":
                                propertyValue.drawingModes[drawingMode] = google.maps.drawing.OverlayType.POLYLINE;
                                break;
                            case "google.maps.drawing.OverlayType.RECTANGLE":
                                propertyValue.drawingModes[drawingMode] = google.maps.drawing.OverlayType.RECTANGLE;
                                break;
                            default:
                        }
                    }
                }

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
//ServerSide (Client Side have no issues) reach MaximumReceiveMessageSize (32kb) and crash if we return all data
//Workaround is to increase limit MaximumReceiveMessageSize
function cleanDirectionResult(dirResponse, dirRequestOptions) {
    let tmpdirobj = JSON.parse(JSON.stringify(dirResponse));

    tmpdirobj.routes.forEach((r) => {
        if (dirRequestOptions == undefined || dirRequestOptions.stripOverviewPath) {
            r.overview_path = [];
        }

        if (dirRequestOptions == undefined || dirRequestOptions.stripOverviewPolyline) {
            r.overview_polyline = '';//Previously was []. Why??? it is a string
        }

        r.legs.forEach((l) => {
            if (dirRequestOptions == undefined || dirRequestOptions.stripLegsSteps) {
                l.steps = [];
            } else {
                l.steps.forEach((step) => {
                    if (dirRequestOptions == undefined || dirRequestOptions.stripLegsStepsLatLngs) {
                        step.lat_lngs = [];
                    }

                    if (dirRequestOptions == undefined || dirRequestOptions.stripLegsStepsPath) {
                        step.path = [];
                    }
                });
            }
        });
    });

    return tmpdirobj;
}

function getGooglePositionFromString(positionString) {
    //https://developers.google.com/maps/documentation/javascript/reference/control#ControlPosition
    switch (positionString) {
        case "BOTTOM_CENTER":
            return google.maps.ControlPosition.BOTTOM_CENTER;
        case "BOTTOM_LEFT":
            return google.maps.ControlPosition.BOTTOM_LEFT;
        case "BOTTOM_RIGHT":
            return google.maps.ControlPosition.BOTTOM_RIGHT;
        case "LEFT_BOTTOM":
            return google.maps.ControlPosition.LEFT_BOTTOM;
        case "LEFT_CENTER":
            return google.maps.ControlPosition.LEFT_CENTER;
        case "LEFT_CENTER":
            return google.maps.ControlPosition.LEFT_CENTER;
        case "RIGHT_BOTTOM":
            return google.maps.ControlPosition.RIGHT_BOTTOM;
        case "RIGHT_CENTER":
            return google.maps.ControlPosition.RIGHT_CENTER;
        case "RIGHT_TOP":
            return google.maps.ControlPosition.RIGHT_TOP;
        case "TOP_CENTER":
            return google.maps.ControlPosition.TOP_CENTER;
        case "TOP_LEFT":
            return google.maps.ControlPosition.TOP_LEFT;
        case "TOP_RIGHT":
            return google.maps.ControlPosition.TOP_RIGHT;
        default:
            return google.maps.ControlPosition.BOTTOM_CENTER;
    }
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

    //Used to create multiple objects of the same type passing a set of creation parameters coherent 
    //with object we need to create 
    //This allows a single JSInteropt invocation for multiple objects creation with a consistent gain 
    //in terms of performance
    createMultipleObject: function (args) {
        window._blazorGoogleMapsObjects = window._blazorGoogleMapsObjects || [];

        let args2 = args.slice(2).map(arg => tryParseJson(arg));
        //console.log(args2);
        let functionName = args[1];
        let constructor = stringToFunction(functionName);

        let guids = JSON.parse(args[0]);

        for (var i = 0; i < args2.length; i++) {
            let args3 = [];
            args3.push(args2[i]);
            let obj = new constructor(...args3);

            if ("set" in obj) {
                obj.set("guidString", guids[i]);
            }

            window._blazorGoogleMapsObjects[guids[i]] = obj;
        }
    },

    addObject: function (obj, guid) {
        if (guid === null || typeof guid === "undefined") {
            guid = uuidv4();
        }

        window._blazorGoogleMapsObjects = window._blazorGoogleMapsObjects || [];
        window._blazorGoogleMapsObjects[guid] = obj;

        return guid;
    },

    addControls(args) {
        let map = _blazorGoogleMapsObjects[args[0]];
        let elem = args[2];
        //I know i am lazy. Two quotes appear after serialization
        let position = getGooglePositionFromString(args[1].replace("\"", "").replace("\"", ""));

        map.controls[position].push(elem);
    },
    addImageLayer(args) {
        let map = _blazorGoogleMapsObjects[args[0]];
        let elem = _blazorGoogleMapsObjects[args[1]];

        map.overlayMapTypes.push(elem);
    },
    removeImageLayer(args) {
        let map = _blazorGoogleMapsObjects[args[0]];
        let elem = _blazorGoogleMapsObjects[args[1]];

        var arr = map.overlayMapTypes.getArray();
        for (var i = 0; i < arr.length; i++) {
            if (arr[i].name === elem.name) {
                map.overlayMapTypes.removeAt(i);
                return;
            }
        }
    },
    removeAllImageLayers(args) {
        let map = _blazorGoogleMapsObjects[args[0]];

        var arr = map.overlayMapTypes.clear();
    },
    disposeMapElements(mapGuid) {
        var keysToRemove = [];

        for (var key in _blazorGoogleMapsObjects) {
            if (_blazorGoogleMapsObjects.hasOwnProperty(key)) {
                var element = _blazorGoogleMapsObjects[key];
                if (element.hasOwnProperty("map")
                    && element.hasOwnProperty("guidString")
                    && element.map.guidString === mapGuid) {
                    keysToRemove.push(element.guidString);
                }
            }
        }

        for (var keyToRemove in keysToRemove) {
            if (keysToRemove.hasOwnProperty(keyToRemove)) {
                var elementToRemove = keysToRemove[keyToRemove];
                delete window._blazorGoogleMapsObjects[elementToRemove];
            }
        }
    },

    disposeObject: function (guid) {
        delete window._blazorGoogleMapsObjects[guid];
    },

    disposeMultipleObjects: function (guids) {
        for (var i = 0; i < guids.length; i++) {
            this.disposeObject(guids[i]);
        }
    },

    invoke: async function (args) {
        let args2 = args.slice(2).map(arg => tryParseJson(arg));

        let obj = window._blazorGoogleMapsObjects[args[0]];
        let functionToInvoke = args[1];

        //If function is route, then handle callback in promise.
        if (functionToInvoke == "googleMapDirectionServiceFunctions.route") {
            let dirRequest = args2[0];
            let dirRequestOptions = args2[1];

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
                if (typeof obj.setDirections === "function") {
                    obj.setDirections(result);
                }

                let jsonRest = JSON.stringify(cleanDirectionResult(result, dirRequestOptions));
                //console.log(JSON.stringify(jsonRest));
                return jsonRest;
            } catch (error) {
                console.log(error);
                return error;
            }

        }
        //Used in HeatampLayer. We must use LatLng since LatLngLiteral doesnt work
        else if (functionToInvoke == "setData") {
            var pointArray = new google.maps.MVCArray();
            for (i = 0; i < args2[0].length; i++) {
                var cord = args2[0][i];

                if (cord.hasOwnProperty("weight")) {
                    var cordLocation = new google.maps.LatLng(cord.location.lat, cord.location.lng);
                    var location = { location: cordLocation, weight: cord.weight };
                    pointArray.push(location);
                } else {
                    pointArray.push(new google.maps.LatLng(cord.lat, cord.lng));
                }
            }

            try {
                result = obj.setData(pointArray);
            } catch (e) {
                console.log(e);
            }
        }
        else if (functionToInvoke == "getDirections") {
            let dirRequestOptions = args2[0];

            try {
                var result = obj[functionToInvoke]();
            } catch (e) {
                console.log(e);
            }

            let jsonRest = JSON.stringify(cleanDirectionResult(result, dirRequestOptions));
            return jsonRest;
        }
        else if (functionToInvoke == "getProjection") {

            try {
                var projection = obj[functionToInvoke](...args2);
                _blazorGoogleMapsObjects[args[2]] = projection;
            } catch (e) {
                console.log(e);
            }
        }
        else if (functionToInvoke == "fromLatLngToPoint") {

            try {
                //It expects lat() and lng() functions
                var lit = new google.maps.LatLng(args2[0].lat, args2[0].lng);
                var point = obj[functionToInvoke](lit);
                return point;
            } catch (e) {
                console.log(e);
            }

        } else {
            var result = null;
            try {
                result = obj[functionToInvoke](...args2);
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

    //Function could be extended in future: at the moment it is scoped for 
    //simple "Get" and "Set" properties of multiple objects of the same type
    invokeMultiple: async function (args) {
        let args2 = args.slice(2).map(arg => tryParseJson(arg));

        var results = {};
        let objs = [];
        let guids = JSON.parse(args[0]);

        for (var i = 0; i < guids.length; i++) {
            objs[i] = window._blazorGoogleMapsObjects[guids[i]];
            let args3 = [];
            args3 = args3.concat(guids[i]).concat(args[1]).concat(args2[i]);

            let result = googleMapsObjectManager.invoke(args3);

            if (Promise.resolve(result)) {
                results[guids[i]] = await result;
            }
            else {
                results[guids[i]] = result;
            }
        }

        //console.log(results);

        return results;
    },

    invokeWithReturnedObjectRef: function (args) {
        let result = googleMapsObjectManager.invoke(args);
        let uuid = uuidv4();

        //console.log("invokeWithReturnedObjectRef " + uuid);

        //Removed since here exists only events and whats point of having event in this array????
        //window._blazorGoogleMapsObjects[uuid] = result;

        return uuid;
    },

    drawingManagerOverlaycomplete: function (args) {
        var uuid = args[0];
        var act = args[1];

        let drawingManager = window._blazorGoogleMapsObjects[uuid];
        google.maps.event.addListener(drawingManager, "overlaycomplete", function (event) {
            let overlayUuid = uuidv4();
            window._blazorGoogleMapsObjects[overlayUuid] = event.overlay;
            let returnObj = JSON.stringify([{ type: event.type, uuid: overlayUuid.toString() }]);
            act.invokeMethodAsync("Invoke", returnObj, uuid);
        });
    },

    invokeMultipleWithReturnedObjectRef: function (args) {

        let guids = args[0];
        let otherArgs = args.slice(1, args.length - 1);
        let what = args[args.length - 1];

        let results = {};

        for (var i = 0; i < guids.length; i++) {
            let uuid = uuidv4();
            let args2 = [];
            args2 = args2.concat(guids[i]).concat(otherArgs).concat(what[i]);

            results[uuid] = googleMapsObjectManager.invoke(args2);
        }

        return results;
    },

    //add event listeners to multiple objects of the same type
    addMultipleListeners: async function (args) {
        let args2 = args.slice(2).map(arg => tryParseJson(arg));

        var results = {};
        let objs = [];
        let guids = JSON.parse(args[0]);

        for (var i = 0; i < guids.length; i++) {
            objs[i] = window._blazorGoogleMapsObjects[guids[i]];
            let args3 = [];
            args3 = args3.concat(guids[i]).concat("addListener").concat(args[1]).concat(args2[i]);

            let result = googleMapsObjectManager.invoke(args3);
        }

        //console.log(results);

        return !0;
    },

    readObjectPropertyValue: function (args) {
        let obj = window._blazorGoogleMapsObjects[args[0]];

        return obj[args[1]];
    },

    readObjectPropertyValueWithReturnedObjectRef: function (args) {

        let obj = window._blazorGoogleMapsObjects[args[0]];

        let result = obj[args[1]];
        let uuid = uuidv4();

        window._blazorGoogleMapsObjects[uuid] = result;

        return uuid;
    },

    addClusteringMarkers(guid, mapGuid, markers, options) {
        const map = window._blazorGoogleMapsObjects[mapGuid];

        const originalMarkers = markers.map((marker, i) => {
            return window._blazorGoogleMapsObjects[marker.guid];
        });

        const markerCluster = new MarkerClusterer(map, originalMarkers, options);

        if ("set" in markerCluster) {
            markerCluster.set("guidString", guid);
        }

        window._blazorGoogleMapsObjects[guid] = markerCluster;
    }
};

//export { googleMapsObjectManager }