window.blazorGoogleMaps = window.blazorGoogleMaps || function () {
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

    const extendableStringify = function (obj, replacer, space) {
        if (window.blazorGoogleMapsBeforeStringify) {
            obj = window.blazorGoogleMapsBeforeStringify(obj);
        }
        return JSON.stringify(obj, replacer, space);
    };

    let mapObjects = {};
    let controlParents = {}
    const dateFormat = /^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}/;

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

    function dateObjectReviver(key, value) {
        if (typeof value === "string" && dateFormat.test(value)) {
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

                var guid = blazorGoogleMaps.objectManager.addObject(args[0]);

                if (args.length == 1 && typeof args[0].marker !== "undefined") {
                    var n = args[0].marker;
                    args[0].marker = null;
                    await item.invokeMethodAsync("Invoke", extendableStringify(args, getCircularReplacer()), guid);
                    args[0].marker = n;
                }
                else {
                    await item.invokeMethodAsync("Invoke", extendableStringify(args, getCircularReplacer()), guid);
                }

                blazorGoogleMaps.objectManager.disposeObject(guid);
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

        if (item !== null && item.startsWith("google.maps.drawing.OverlayType")) {
            switch (item) {
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
        

        if (typeof item2 === "object" && item2 !== null) {
            if ("guidString" in item2) {
                //console.log("Found object has Guid property.");
                return mapObjects[item2.guidString];
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
                        item2[propertyName] = mapObjects[propertyValue.guidString];
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
        let tmpdirobj = JSON.parse(extendableStringify(dirResponse));

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
            case "LEFT_TOP":
                return google.maps.ControlPosition.LEFT_TOP;
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

    let directionService = {
        route: async function (request, options) {
            let renderer = this;

            let promise = new Promise((resolve, reject) => {
                let directionsService = new google.maps.DirectionsService();
                directionsService.route(request, (result, status) => {
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
                if (typeof renderer.setDirections === "function") {
                    renderer.setDirections(result);
                }

                let jsonRest = extendableStringify(cleanDirectionResult(result, options));
                //console.log(extendableStringify(jsonRest));
                return jsonRest;
            } catch (error) {
                console.log(error);
                return error;
            }
        }
    };

    return {
        objectManager: {
            get mapObjects() { return mapObjects; },
            createObject: function (args) {
                mapObjects = mapObjects || [];
                

                let args2 = args.slice(2).map(arg => tryParseJson(arg));
                //console.log(args2);
                let functionName = args[1];
                if (functionName == "google.maps.marker.AdvancedMarkerView") {
                    var content = args2[0].content;
                    if (content != null && content !== undefined) {
                        var template = document.createElement('template');
                        content = content.trim();
                        template.innerHTML = content;
                        args2[0].content = template.content.firstChild;
                    }
                }
                let constructor = stringToFunction(functionName);
                let obj = new constructor(...args2);
                let guid = args[0];

                if ("set" in obj) {
                    obj.set("guidString", guid);
                }

                mapObjects[guid] = obj;
            },

            //Used to create multiple objects of the same type passing a set of creation parameters coherent 
            //with object we need to create 
            //This allows a single JSInteropt invocation for multiple objects creation with a consistent gain 
            //in terms of performance
            createMultipleObject: function (args) {
                mapObjects = mapObjects || [];

                let args2 = args.slice(2).map(arg => tryParseJson(arg));
                //console.log(args2);
                let functionName = args[1];
                let constructor = stringToFunction(functionName);

                let guids = JSON.parse(args[0]);

                for (var i = 0, len = args2.length; i < len; i++) {
                    let obj = new constructor(args2[i]);

                    if ("set" in obj) {
                        obj.set("guidString", guids[i]);
                    }

                    mapObjects[guids[i]] = obj;
                }
            },

            addObject: function (obj, guid) {
                if (guid === null || typeof guid === "undefined") {
                    guid = uuidv4();
                }

                mapObjects = mapObjects || [];
                mapObjects[guid] = obj;

                return guid;
            },

            addControls(args) {
                let mapGuid = args[0];
                let map = mapObjects[mapGuid];
                let elem = args[2];
                if(!elem) return
                //I know i am lazy. Two quotes appear after serialization
                let position = getGooglePositionFromString(args[1].replace("\"", "").replace("\"", ""));

                // check if the control already exists
                var controls = map.controls[position].getArray();
                for (var i = 0; i < controls.length; i++) {
                    if (controls[i].id === elem.id) {
                        return;
                    }
                }

                if (controlParents == null) {
                    controlParents = {}
                }
                if (controlParents.hasOwnProperty(mapGuid) == false) {
                    controlParents[mapGuid] = {}
                }
                if (controlParents[mapGuid].hasOwnProperty(elem.id) == false && !controlParents[mapGuid][elem.id]) {
                    let parentElement = elem.parentElement
                    controlParents[mapGuid][elem.id] = parentElement;
                }
                
                elem.style.display = "block";
                map.controls[position].push(elem);
            },
            appendControlElementToOriginalParent(mapGuid, control) {
                const parent = controlParents[mapGuid][control.id];
                if (parent) {
                    parent.appendChild(control);
                    control.style.display = "none";
                }
                delete controlParents[mapGuid][control.id];
            },
            internalRemoveControlAt(mapGuid, position, controlIndex) {
                const map = mapObjects[mapGuid];
                if (controlIndex !== -1) {
                    let control = map.controls[position].removeAt(controlIndex);
                    this.appendControlElementToOriginalParent(mapGuid, control);
                }
            },
            internalRemoveControls(mapGuid, position) {
                const map = mapObjects[mapGuid];
                for (let i = map.controls[position].length - 1; i >= 0; i--) {
                    this.internalRemoveControlAt(mapGuid, position, i);
                }
            },
            removeControl(args) {
                const mapGuid = args[0];
                const map = mapObjects[mapGuid];
                const position = getGooglePositionFromString(args[1].replace(/"/g, ""));

                const elemId = args[2].id;
                const controlIndex = map.controls[position].getArray().findIndex(control => control.id === elemId);

                this.internalRemoveControlAt(mapGuid, position, controlIndex);
            },
            removeControls(args) {
                const mapGuid = args[0];
                const position = getGooglePositionFromString(args[1].replace(/"/g, ""));
                this.internalRemoveControls(mapGuid, position);
            },
            addImageLayer(args) {
                let map = mapObjects[args[0]];
                let elem = mapObjects[args[1]];

                map.overlayMapTypes.push(elem);
            },
            removeImageLayer(args) {
                let map = mapObjects[args[0]];
                let elem = mapObjects[args[1]];

                var arr = map.overlayMapTypes.getArray();
                for (var i = 0, len = arr.length; i < len; i++) {
                    if (arr[i].name === elem.name) {
                        map.overlayMapTypes.removeAt(i);
                        return;
                    }
                }
            },
            removeAllImageLayers(args) {
                let map = mapObjects[args[0]];

                var arr = map.overlayMapTypes.clear();
            },
            disposeMapElements(mapGuid) {
                var keysToRemove = [];

                for (var key in mapObjects) {
                    if (mapObjects.hasOwnProperty(key)) {
                        var element = mapObjects[key];
                        if (element.hasOwnProperty("map")
                            && element.hasOwnProperty("guidString")
                            && element.map !== null
                            && element.map !== undefined
                            && element.map.guidString === mapGuid) {
                            keysToRemove.push(element.guidString);
                        }
                    }
                }
                for (var keyToRemove in keysToRemove) {
                    if (keysToRemove.hasOwnProperty(keyToRemove)) {
                        var elementToRemove = keysToRemove[keyToRemove];
                        delete mapObjects[elementToRemove];
                    }
                }

                if (controlParents !== null && controlParents.hasOwnProperty(mapGuid)) {
                    const map = mapObjects[mapGuid];
                    for (let position in map.controls) {
                        this.internalRemoveControls(mapGuid, position);
                    }
                    delete controlParents[mapGuid];
                }

                if (controlParents !== null && Object.keys(controlParents) == 0) {
                    controlParents = null;
                }
            },

            disposeObject: function (guid) {
                delete mapObjects[guid];
            },

            disposeMultipleObjects: function (guids) {
                for (var i = 0, len = guids.length; i < len; i++) {
                    this.disposeObject(guids[i]);
                }
            },

            invoke: async function (args) {
                let args2 = args.slice(2).map(arg => tryParseJson(arg));

                let obj = mapObjects[args[0]];
                let functionToInvoke = args[1];

                //We make check if element is LatLng and cast it.
                //It could be bug here.
                if (Array.isArray(args2) && args2.length > 0) {
                    var cloneArgs = args2;
                    args2 = new Array();
                    for (let i = 0, len = cloneArgs.length; i < len; i++) {
                        var element = cloneArgs[i];
                        if (element != null && element !== undefined && element.hasOwnProperty("lat") && element.hasOwnProperty("lng")) {
                            args2.push(new google.maps.LatLng(element.lat, element.lng));
                        } else {
                            args2.push(element);
                        }
                    }
                }

                //If function is route, then handle callback in promise.
                if (functionToInvoke == "blazorGoogleMaps.directionService.route") {
                    var responseOrError = await directionService.route.call(obj, args2[0], args2[1]);
                    return responseOrError;
                }
                //Used in HeatampLayer. We must use LatLng since LatLngLiteral doesnt work
                else if (functionToInvoke == "setData") {
                    var pointArray = new google.maps.MVCArray();
                    for (i = 0, len = args2[0].length; i < len; i++) {
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

                    let jsonRest = extendableStringify(cleanDirectionResult(result, dirRequestOptions));
                    return jsonRest;
                }
                else if (functionToInvoke == "getProjection") {

                    try {
                        var projection = obj[functionToInvoke](...args2);
                        mapObjects[args[2]] = projection;
                    } catch (e) {
                        console.log(e);
                    }
                }
                else if (functionToInvoke == "createPath") {

                    try {
                        var projection = mapObjects[args[0]].getPath();
                        mapObjects[args[2]] = projection;
                    } catch (e) {
                        console.log(e);
                    }
                }
                else if (functionToInvoke == "fromLatLngToPoint") {
                    try {
                        var point = obj[functionToInvoke](args2[0]);
                        return point;
                    } catch (e) {
                        console.log(e);
                    }
                }
                else if (functionToInvoke === "addListenerOnce") {
                    const eventId = new google.maps.event.addListenerOnce(obj, args2[0], args2[1]);
                    return eventId;
                }
                else if (google.maps.places !== undefined && obj instanceof google.maps.places.AutocompleteService) {
                    //AutocompleteService predictions to handle callbacks in the promise
                    return new Promise(function (resolve, reject) {
                        try {
                            obj[functionToInvoke](args2[0], function (result, status) {
                                resolve({ predictions: result, status: status });
                            });
                        } catch (e) {
                            console.log(e);
                            reject(e);
                        }
                    });
                }
                else if (google.maps.places !== undefined && obj instanceof google.maps.places.PlacesService) {
                    //PlacesService results to handle callbacks in the promise
                    return new Promise(function (resolve, reject) {
                        try {
                            obj[functionToInvoke](args2[0], function (result, status) {
                                var results = (result == null || result instanceof Array) ? result : [result];
                                resolve({ results: results, status: status });
                            });
                        } catch (e) {
                            console.log(e);
                            reject(e);
                        }
                    });
                }
                else if (obj instanceof google.maps.Geocoder) {
                    //Geocoder results to handle callback in the promise
                    return new Promise(function (resolve, reject) {
                        try {
                            obj[functionToInvoke](args2[0], function (result, status) {
                                resolve({ results: result, status: status });
                            });
                        } catch (e) {
                            console.log(e);
                            reject(e);
                        }
                    });
                }
                else if (functionToInvoke == "getPaths") {
                    // Polygon.getPaths returns nested MVCArray
                    // https://developers.google.com/maps/documentation/javascript/reference/polygon#Polygon.getPaths
                    try {
                        var paths = obj[functionToInvoke](...args2);
                        var nestedCoords = [];
                        // MVC array
                        paths.forEach(coords => {
                            nestedCoords.push(coords.getArray());
                        });
                        return nestedCoords;
                    } catch (e) {
                        console.log(e);
                    }
                }
                else if (functionToInvoke == "removeAllFeatures") {
                    //Artificial function to remove all features from the data layer
                    try {
                        obj.forEach(function (feature) {
                            obj.remove(feature);
                        });
                        return null;
                    } catch (e) {
                        console.log(e);
                    }
                }
                else if (functionToInvoke == "overrideStyle") {

                    try {
                        var featureId = args[2].replace('"', "").replace('"', "");
                        var feature = mapObjects[featureId];
                        var data = mapObjects[args[0]];
                        var request = tryParseJson(args[3]);
                        data.overrideStyle(feature, request);
                    } catch (e) {
                        console.log(e);
                    }
                }
                else {
                    var result = null;
                    try {
                        result = obj[functionToInvoke](...args2);
                    } catch (e) {
                        console.log(e);
                        console.log("\nfunctionToInvoke: " + functionToInvoke + "\nargs: " + args2 + "\n");
                    }

                    if (result !== null
                        && typeof result === "object") {
                        if (result.hasOwnProperty("geocoded_waypoints") && result.hasOwnProperty("routes")) {

                            let jsonRest = extendableStringify(cleanDirectionResult(result));
                            return jsonRest;
                        }
                        if ("getArray" in result) {
                            return result.getArray();
                        }
                        //It is event handler. Dont serialize it
                        if ("addListener" == functionToInvoke) {
                            return result;
                        }

                        if ("addGeoJson" == functionToInvoke) {
                            var resultGuids = [];
                            result.forEach(coords => {
                                var addedObjGuid = this.addObject(coords);
                                resultGuids.push(addedObjGuid);
                            });

                            return resultGuids;
                        }

                        if ("get" in result) {
                            return result.get("guidString");
                        } else if ("dotnetTypeName" in result) {
                            return extendableStringify(result, getCircularReplacer());
                        } else {
                            return JSON.parse(extendableStringify(result, getCircularReplacer()));
                        }
                    } else if (functionToInvoke === "remove") {
                        this.disposeObject(args[0]);
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

                for (var i = 0, len = guids.length; i < len; i++) {
                    objs[i] = mapObjects[guids[i]];
                    let args3 = [];
                    args3 = args3.concat(guids[i]).concat(args[1]).concat(args2[i]);

                    let result = blazorGoogleMaps.objectManager.invoke(args3);

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

            invokeWithReturnedObjectRef: async function (args) {
                let result = blazorGoogleMaps.objectManager.invoke(args);
                let uuid = uuidv4();

                //console.log("invokeWithReturnedObjectRef " + uuid);
                //This is needed to be able to remove events from map
                mapObjects[uuid] = await result;

                return uuid;
            },

            drawingManagerOverlaycomplete: function (args) {
                var uuid = args[0];
                var act = args[1];

                let drawingManager = mapObjects[uuid];
                google.maps.event.addListener(drawingManager, "overlaycomplete", function (event) {
                    let overlayUuid = uuidv4();
                    mapObjects[overlayUuid] = event.overlay;
                    let returnObj = extendableStringify([{ type: event.type, uuid: overlayUuid.toString() }]);
                    act.invokeMethodAsync("Invoke", returnObj, uuid);
                });
            },

            invokeMultipleWithReturnedObjectRef: function (args) {

                let guids = args[0];
                let otherArgs = args.slice(1, args.length - 1);
                let what = args[args.length - 1];

                let results = {};

                for (var i = 0, len = guids.length; i < len; i++) {
                    let uuid = uuidv4();
                    let args2 = [];
                    args2 = args2.concat(guids[i]).concat(otherArgs).concat(what[i]);

                    results[uuid] = blazorGoogleMaps.objectManager.invoke(args2);
                }

                return results;
            },

            //add event listeners to multiple objects of the same type
            addMultipleListeners: async function (args) {
                let args2 = args.slice(2).map(arg => tryParseJson(arg));

                var results = {};
                let objs = [];
                let guids = JSON.parse(args[0]);

                for (var i = 0, len = guids.length; i < len; i++) {
                    objs[i] = mapObjects[guids[i]];
                    let args3 = [];
                    args3 = args3.concat(guids[i]).concat("addListener").concat(args[1]).concat(args2[i]);

                    let result = blazorGoogleMaps.objectManager.invoke(args3);
                }

                //console.log(results);

                return !0;
            },

            readObjectPropertyValue: function (args) {
                let obj = mapObjects[args[0]];

                return obj[args[1]];
            },

            readObjectPropertyValueWithReturnedObjectRef: function (args) {

                let obj = mapObjects[args[0]];

                let result = obj[args[1]];
                let uuid = uuidv4();

                mapObjects[uuid] = result;

                return uuid;
            },

            readObjectPropertyValueAndMapToArray: function (args) {
                let obj = mapObjects[args[0]];
                let result = obj[args[1]];
                let props = tryParseJson(args[2]);
                for (var i = 0, len = props.length; i < len; i++) {
                    result = result.map((x) => x[props[i]]);
                }
                return result;
            },

            //based on https://googlemaps.github.io/js-markerclusterer/
            createClusteringMarkers(guid, mapGuid, markers, options) {
                const map = mapObjects[mapGuid];

                const originalMarkers = markers.map((marker, i) => {
                    return mapObjects[marker.guid];
                });

                const markerClustererOptions = {
                    map: map,
                    markers: originalMarkers,
                };

                if (options && options.rendererObjectName) {
                    const splits = options.rendererObjectName.split(".");
                    try {
                        let renderer = window[splits[0]];
                        for (i = 1, len = splits.length; i < len; i++) {
                            renderer = renderer[splits[i]];
                        }
                        markerClustererOptions.renderer = renderer;
                    } catch (e) {
                        console.log(e);
                    }
                }

                if (options && options.algorithmObjectName) {
                    const splits = options.algorithmObjectName.split(".");
                    try {
                        let algorithm = window[splits[0]];
                        for (i = 1, len = splits.length; i < len; i++) {
                            algorithm = algorithm[splits[i]];
                        }
                        markerClustererOptions.algorithm = algorithm;
                    } catch (e) {
                        console.log(e);
                    }
                }

                if (options && !options.zoomOnClick) {
                    markerClustererOptions.onClusterClick = () => { };
                }

                const markerCluster = new markerClusterer.MarkerClusterer(markerClustererOptions);

                /*        const newMarkers = trees.map(({ geometry }, i) => new google.maps.Marker({
                            position: {
                                lat: geometry.coordinates[1],
                                lng: geometry.coordinates[0],
                            },
                            label: labels[i % labels.length],
                            map,
                        }));
                        const markerCluster = new markerClusterer.MarkerClusterer({
                            map: map,
                            markers: newMarkers
                        });
                */

                if ("set" in markerCluster) {
                    markerCluster.set("guidString", guid);
                }

                mapObjects[guid] = markerCluster;
            },

            removeClusteringMarkers(guid, markers, noDraw) {
                const originalMarkers = markers.map((marker, i) => {
                    return mapObjects[marker.guid];
                });

                mapObjects[guid].removeMarkers(originalMarkers, noDraw);
            },

            addClusteringMarkers(guid, markers, noDraw) {
                const originalMarkers = markers.map((marker, i) => {
                    return mapObjects[marker.guid];
                });

                mapObjects[guid].addMarkers(originalMarkers, noDraw);
            }
        }
    };
}();
