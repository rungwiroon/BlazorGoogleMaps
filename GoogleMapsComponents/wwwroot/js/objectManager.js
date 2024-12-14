window.blazorGoogleMaps = window.blazorGoogleMaps || function () {
    function stringToFunction(str) {
        const arr = str.split(".");
        let fn = window || this;

        for (const key of arr) {
            if (fn[key] === undefined) {
                throw new Error(`Property '${key}' not found`);
            }
            fn = fn[key];
        }

        if (typeof fn !== "function") {
            throw new TypeError("Function not found");
        }

        return fn;
    }


    const extendableStringify = (obj, replacer, space) => {
        const beforeStringify = window?.blazorGoogleMapsBeforeStringify || this?.blazorGoogleMapsBeforeStringify;
        if (typeof beforeStringify === 'function') {
            obj = beforeStringify(obj);
        }
        return JSON.stringify(obj, replacer, space);
    };

    let mapObjects = {};
    let controlParents = {};
    const dateFormat = /^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}/;


    // Add the object to the map for "managed" objects, tries to set the guidString to 
    // be able to dispose of them later on
    function addMapObject(uuid, obj) {
        if (obj && typeof(obj) === "object" && "set" in obj) {
            obj.set("guidString", uuid);
        }
        mapObjects[uuid] = obj;
    }
    
    //Strip circular dependencies, map object and functions
    //https://stackoverflow.com/questions/11616630/how-can-i-print-a-circular-structure-in-a-json-like-format
    const getCircularReplacer = () => {
        const seen = new WeakSet();
        return (key, value) => {
            if (key == "map") return undefined;
            if (typeof value == 'function') return undefined;

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
        // Check if the item is a DotNet object reference with the "invokeMethodAsync" method
        if (item !== null && typeof item === "object" && "invokeMethodAsync" in item) {
            return async function (...args) {
                if (args === null || typeof args === "undefined") {
                    await item.invokeMethodAsync("Invoke");
                }

                const guid = blazorGoogleMaps.objectManager.addObject(args[0]);

                if (args.length === 1 && typeof args[0].marker !== "undefined") {
                    const markerBackup = args[0].marker;
                    args[0].marker = null;
                    await item.invokeMethodAsync("Invoke", extendableStringify(args, getCircularReplacer()), guid);
                    args[0].marker = markerBackup;
                } else {
                    await item.invokeMethodAsync("Invoke", extendableStringify(args, getCircularReplacer()), guid);
                }

                blazorGoogleMaps.objectManager.disposeObject(guid);
            };
        }

        if (typeof item !== "string") {
            return item;
        }

        let parsedItem;

        try {
            parsedItem = JSON.parse(item, dateObjectReviver);
        } catch (e) {
            try {
                // Fallback to using eval to handle cases with functions in the JSON string
                parsedItem = eval(`(${item})`);
            } catch (e2) {
                // Return the item as is if parsing fails
                return item;
            }
        }

        if (typeof item === "string" && item.startsWith("google.maps.drawing.OverlayType")) {
            const overlayTypeMapping = {
                "google.maps.drawing.OverlayType.CIRCLE": google.maps.drawing.OverlayType.CIRCLE,
                "google.maps.drawing.OverlayType.MARKER": google.maps.drawing.OverlayType.MARKER,
                "google.maps.drawing.OverlayType.POLYGON": google.maps.drawing.OverlayType.POLYGON,
                "google.maps.drawing.OverlayType.POLYLINE": google.maps.drawing.OverlayType.POLYLINE,
                "google.maps.drawing.OverlayType.RECTANGLE": google.maps.drawing.OverlayType.RECTANGLE
            };
            return overlayTypeMapping[item] || item;
        }

        if (typeof parsedItem === "object" && parsedItem !== null) {
            if ("guidString" in parsedItem) {
                return mapObjects[parsedItem.guidString];
            } else {
                for (let propertyName in parsedItem) {
                    let propertyValue = parsedItem[propertyName];

                    // Convert specific Google Maps Animation strings to their corresponding objects
                    if (typeof propertyValue === "string" && propertyValue.startsWith("google.maps.Animation")) {
                        const animationMapping = {
                            "google.maps.Animation.DROP": google.maps.Animation.DROP,
                            "google.maps.Animation.BOUNCE": google.maps.Animation.BOUNCE
                        };
                        parsedItem[propertyName] = animationMapping[propertyValue] || propertyValue;
                    }

                    if (propertyName == "icons") {
                        if (Array.isArray(propertyValue)) {
                            propertyValue.forEach(item => {
                                var iconPath = item.icon.path;
                                if (iconPath) {
                                    const symbolPathMapping = {
                                        "BACKWARD_CLOSED_ARROW": google.maps.SymbolPath.BACKWARD_CLOSED_ARROW,
                                        "BACKWARD_OPEN_ARROW": google.maps.SymbolPath.BACKWARD_OPEN_ARROW,
                                        "CIRCLE": google.maps.SymbolPath.CIRCLE,
                                        "FORWARD_CLOSED_ARROW": google.maps.SymbolPath.FORWARD_CLOSED_ARROW,
                                        "FORWARD_OPEN_ARROW": google.maps.SymbolPath.FORWARD_OPEN_ARROW,
                                    };

                                    item.icon.path = symbolPathMapping[item.icon.path];
                                }
                            });
                        }
                    }

                    // Convert specific Google Maps CollisionBehavior strings to their corresponding objects
                    if (typeof propertyValue === "string" && propertyValue.startsWith("google.maps.CollisionBehavior")) {
                        const collisionBehaviorMapping = {
                            "google.maps.CollisionBehavior.REQUIRED": google.maps.CollisionBehavior.REQUIRED,
                            "google.maps.CollisionBehavior.REQUIRED_AND_HIDES_OPTIONAL": google.maps.CollisionBehavior.REQUIRED_AND_HIDES_OPTIONAL,
                            "google.maps.CollisionBehavior.OPTIONAL_AND_HIDES_LOWER_PRIORITY": google.maps.CollisionBehavior.OPTIONAL_AND_HIDES_LOWER_PRIORITY
                        };
                        parsedItem[propertyName] = collisionBehaviorMapping[propertyValue] || propertyValue;
                    }

                    // Convert position strings to Google Maps positions
                    if (typeof propertyValue === "object" && propertyValue !== null && propertyValue.position !== undefined) {
                        propertyValue.position = getGooglePositionFromString(propertyValue.position);
                    }

                    if (propertyName === "renderingType" && propertyValue !== null) {
                        const renderingTypeMap = {
                            "google.maps.RenderingType.RASTER": google.maps.RenderingType.RASTER,
                            "google.maps.RenderingType.UNINITIALIZED": google.maps.RenderingType.UNINITIALIZED,
                            "google.maps.RenderingType.VECTOR": google.maps.RenderingType.VECTOR
                        };

                        parsedItem[propertyName] = renderingTypeMap[propertyValue] || google.maps.RenderingType.RASTER;
                    }

                    // Handle nested drawingModes property
                    if (typeof propertyValue === "object" && propertyValue !== null && "drawingModes" in propertyValue) {
                        const drawingModeMapping = {
                            "google.maps.drawing.OverlayType.CIRCLE": google.maps.drawing.OverlayType.CIRCLE,
                            "google.maps.drawing.OverlayType.MARKER": google.maps.drawing.OverlayType.MARKER,
                            "google.maps.drawing.OverlayType.POLYGON": google.maps.drawing.OverlayType.POLYGON,
                            "google.maps.drawing.OverlayType.POLYLINE": google.maps.drawing.OverlayType.POLYLINE,
                            "google.maps.drawing.OverlayType.RECTANGLE": google.maps.drawing.OverlayType.RECTANGLE
                        };

                        propertyValue.drawingModes = propertyValue.drawingModes.map(drawingMode => drawingModeMapping[drawingMode] || drawingMode);
                    }

                    // Handle nested objects with a guidString property
                    if (typeof propertyValue === "object" && propertyValue !== null && "guidString" in propertyValue) {
                        parsedItem[propertyName] = mapObjects[propertyValue.guidString];
                    }
                }
                return parsedItem;
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
                r.overview_polyline = ''; // overview_polyline is a string, not an array
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
        const positionMap = {
            "BOTTOM_CENTER": google.maps.ControlPosition.BOTTOM_CENTER,
            "BOTTOM_LEFT": google.maps.ControlPosition.BOTTOM_LEFT,
            "BOTTOM_RIGHT": google.maps.ControlPosition.BOTTOM_RIGHT,
            "LEFT_BOTTOM": google.maps.ControlPosition.LEFT_BOTTOM,
            "LEFT_CENTER": google.maps.ControlPosition.LEFT_CENTER,
            "LEFT_TOP": google.maps.ControlPosition.LEFT_TOP,
            "RIGHT_BOTTOM": google.maps.ControlPosition.RIGHT_BOTTOM,
            "RIGHT_CENTER": google.maps.ControlPosition.RIGHT_CENTER,
            "RIGHT_TOP": google.maps.ControlPosition.RIGHT_TOP,
            "TOP_CENTER": google.maps.ControlPosition.TOP_CENTER,
            "TOP_LEFT": google.maps.ControlPosition.TOP_LEFT,
            "TOP_RIGHT": google.maps.ControlPosition.TOP_RIGHT
        };

        // Return the corresponding ControlPosition or default to BOTTOM_CENTER if not found
        return positionMap[positionString] || google.maps.ControlPosition.BOTTOM_CENTER;
    }

    let directionService = {
        route: async function (request, options) {
            let renderer = this;

            let promise = new Promise((resolve, reject) => {
                let directionsService = new google.maps.DirectionsService();
                directionsService.route(request, (result, status) => {
                    if (status == 'OK') {
                        resolve(result);
                    } else {
                        reject(status);
                    }
                });
            });

            try {
                let result = await promise;

                if (typeof renderer.setDirections === "function") {
                    renderer.setDirections(result);
                }

                let jsonRest = extendableStringify(cleanDirectionResult(result, options));
                return jsonRest;
            } catch (error) {
                console.log(error);
                return error;
            }
        }
    };


    //It is impossible to pass to pass HTMLElement from blazor to google maps
    //Due to this we need to create it in js side.
    function getAdvancedMarkerElementContent(functionName, content) {
        if (functionName === "google.maps.marker.AdvancedMarkerView" || functionName === "google.maps.marker.AdvancedMarkerElement") {
            if (content) {
                //Old code. It work when creating with options but not when SetContent
                //let isPinElement = content.dotnetTypeName === "GoogleMapsComponents.Maps.PinElement";
                const isHtmlContent = typeof content === 'string' && content.startsWith("<");

                if (isHtmlContent) {
                    let template = document.createElement('template');
                    template.innerHTML = content.trim();
                    return template.content.firstChild;
                } else {
                    let pin = new google.maps.marker.PinElement({
                        background: content.background,
                        borderColor: content.borderColor,
                        glyphColor: content.glyphColor,
                        scale: content.scale,
                    });

                    let glyph = content.glyph;
                    if (glyph) {
                        const isGlyphHtmlContent = typeof glyph === 'string' && glyph.startsWith("<");
                        if (isGlyphHtmlContent) {
                            let template = document.createElement('div');
                            template.innerHTML = glyph.trim();
                            pin.glyph = template;
                        } else {
                            pin.glyph = glyph.startsWith("http") ? new URL(glyph) : glyph;
                        }
                    }

                    return pin.element;
                }
            }
        }

        return null;
    }


    return {
        objectManager: {
            get mapObjects() { return mapObjects; },
            initMap: async function (apiOptions) {
                const librariesToLoad = apiOptions["libraries"];
                delete apiOptions["libraries"];

                (g => { var h, a, k, p = "The Google Maps JavaScript API", c = "google", l = "importLibrary", q = "__ib__", m = document, b = window; b = b[c] || (b[c] = {}); var d = b.maps || (b.maps = {}), r = new Set, e = new URLSearchParams, u = () => h || (h = new Promise(async (f, n) => { await (a = m.createElement("script")); e.set("libraries", [...r] + ""); for (k in g) e.set(k.replace(/[A-Z]/g, t => "_" + t[0].toLowerCase()), g[k]); e.set("callback", c + ".maps." + q); a.src = `https://maps.${c}apis.com/maps/api/js?` + e; d[q] = f; a.onerror = () => h = n(Error(p + " could not load.")); a.nonce = m.querySelector("script[nonce]")?.nonce || ""; m.head.append(a) })); d[l] ? console.warn(p + " only loads once. Ignoring:", g) : d[l] = (f, ...n) => r.add(f) && u().then(() => d[l](f, ...n)) })(
                    apiOptions
                );

                var libArray = librariesToLoad.split(',');
                for (var i = 0; i < libArray.length; i++) {
                    var library = libArray[i];
                    await google.maps.importLibrary(library);
                }
            },

            createObject: function (args) {
                mapObjects = mapObjects || [];

                let args2 = args.slice(2).map(arg => tryParseJson(arg));

                let functionName = args[1];
                let advancedMarkerElementContent = getAdvancedMarkerElementContent(functionName, args2.length > 0 ? args2[0]?.content : null);
                if (advancedMarkerElementContent !== null) {
                    args2[0].content = advancedMarkerElementContent;
                }

                let constructor = stringToFunction(functionName);
                let obj = new constructor(...args2);
                let guid = args[0];
                addMapObject(guid, obj)
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

                for (let i = 0, len = args2.length; i < len; i++) {
                    const constructorArgs = args2[i];
                    let advancedMarkerElementContent = getAdvancedMarkerElementContent(functionName, constructorArgs.content);
                    if (advancedMarkerElementContent !== null) {
                        constructorArgs.content = advancedMarkerElementContent;
                    }

                    let obj = new constructor(constructorArgs);
                    addMapObject(guids[i], obj);
                }
            },



            addObject: function (obj, guid) {
                if (guid === null || typeof guid === "undefined") {
                    guid = uuidv4();
                }

                mapObjects = mapObjects || [];
                addMapObject(guid, obj);

                return guid;
            },

            addControls(args) {
                let mapGuid = args[0];
                let map = mapObjects[mapGuid];
                let elem = args[2];
                if (!elem) return
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
                const keysToRemove = [];

                for (const key in mapObjects) {
                    if (mapObjects.hasOwnProperty(key)) {
                        const element = mapObjects[key];
                        if (
                            "guidString" in element && // Element has a guidString property (inherited is important for advanced marker)
                            "map" in element && // Element has a map property (inherited is important for advanced marker)
                            element.map && // Element has a map
                            "guidString" in element.map && // Map has a guidString
                            element.map.guidString === mapGuid // The guidString is matching our current guidString
                        ) {
                            keysToRemove.push(element.guidString);
                        }
                    }
                }
                for (const keyToRemove in keysToRemove) {
                    if (keysToRemove.hasOwnProperty(keyToRemove)) {
                        const elementToRemove = keysToRemove[keyToRemove];
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

                if (controlParents !== null && Object.keys(controlParents) === 0) {
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

            invokeProperty: async function (args) {
                const args2 = args.slice(2).map(arg => tryParseJson(arg));
                const obj = mapObjects[args[0]];
                const functionToInvoke = args[1];

                const processedArgs = args2.map(element => {
                    if (element && element.hasOwnProperty("lat") && element.hasOwnProperty("lng")) {
                        return new google.maps.LatLng(element.lat, element.lng);
                    }
                    return element;
                });

                // Could be an issue in future. Currently, it is used by AdvancedMarkerElement
                const advancedMarkerElementContent = getAdvancedMarkerElementContent(
                    "google.maps.marker.AdvancedMarkerElement",
                    functionToInvoke === "content" ? processedArgs[0] : null
                );
                if (advancedMarkerElementContent !== null) {
                    processedArgs[0] = advancedMarkerElementContent;
                }

                try {
                    if (processedArgs.length > 0) {
                        obj[functionToInvoke] = processedArgs[0]
                    } else {
                        const result = obj[functionToInvoke];
                        if (result && typeof result === "object") {
                            return JSON.parse(extendableStringify(result, getCircularReplacer()));
                        } else {
                            return result;
                        }
                    }
                } catch (e) {
                    console.error(`Error invoking property: Function: ${functionToInvoke} Arguments: ${JSON.stringify(processedArgs)} Error: ${e}`);
                }
            },

            invoke: async function (args) {
                const [objectId, functionToInvoke, ...restArgs] = args;
                const obj = mapObjects[objectId];
                const args2 = restArgs.map(arg => tryParseJson(arg));

                // Handle LatLng objects
                const formattedArgs = args2.map(arg => {
                    if (arg && arg.hasOwnProperty("lat") && arg.hasOwnProperty("lng")) {
                        return new google.maps.LatLng(arg.lat, arg.lng);
                    }
                    return arg;
                });

                try {
                    switch (functionToInvoke) {
                        case "blazorGoogleMaps.directionService.route":
                            const responseOrError = await directionService.route.call(obj, formattedArgs[0], formattedArgs[1]);
                            return responseOrError;

                        case "setData":
                            const pointArray = new google.maps.MVCArray();
                            formattedArgs[0].forEach(cord => {
                                const location = cord.hasOwnProperty("weight")
                                    ? { location: new google.maps.LatLng(cord.location.lat, cord.location.lng), weight: cord.weight }
                                    : new google.maps.LatLng(cord.lat, cord.lng);
                                pointArray.push(location);
                            });
                            return obj.setData(pointArray);

                        case "getDirections":
                            const dirRequestOptions = formattedArgs[0];
                            const result = obj[functionToInvoke]();
                            const jsonRest = extendableStringify(cleanDirectionResult(result, dirRequestOptions));
                            return jsonRest;

                        case "getProjection":
                            const projection = obj[functionToInvoke](...formattedArgs);
                            addMapObject(restArgs[0], projection);
                            return;

                        case "createPath":
                            const pathProjection = obj.getPath();
                            addMapObject(restArgs[0], pathProjection);
                            return;

                        case "fromLatLngToPoint":
                            return obj[functionToInvoke](formattedArgs[0]);

                        case "google.maps.marker.PinElement":
                            return new google.maps.marker.PinElement(formattedArgs[0]);

                        case "addListenerOnce":
                            return new google.maps.event.addListenerOnce(obj, formattedArgs[0], formattedArgs[1]);

                        case "getPaths":
                            const paths = obj[functionToInvoke](...formattedArgs);
                            return paths.getArray().map(coords => coords.getArray());

                        case "removeAllFeatures":
                            obj.forEach(feature => obj.remove(feature));
                            return null;

                        case "overrideStyle":
                            const featureId = restArgs[0].replace(/"/g, "");
                            const feature = mapObjects[featureId];
                            const data = mapObjects[objectId];
                            const request = tryParseJson(restArgs[1]);
                            return data.overrideStyle(feature, request);

                        default:
                            if (google.maps.places !== undefined && obj instanceof google.maps.places.AutocompleteService ||
                                (google.maps.places !== undefined && obj instanceof google.maps.places.PlacesService) ||
                                obj instanceof google.maps.Geocoder) {
                                return new Promise((resolve, reject) => {
                                    obj[functionToInvoke](formattedArgs[0], (result, status) => {
                                        if (obj instanceof google.maps.places.AutocompleteService) {
                                            resolve({ predictions: result, status: status });
                                        } else {
                                            resolve({ results: result, status: status });
                                        }
                                    });
                                });
                            } else {
                                const result = obj[functionToInvoke](...formattedArgs);
                                if (result && typeof result === "object") {
                                    if (result.hasOwnProperty("geocoded_waypoints") && result.hasOwnProperty("routes")) {
                                        return extendableStringify(cleanDirectionResult(result));
                                    }
                                    if ("getArray" in result) {
                                        return result.getArray();
                                    }
                                    if (functionToInvoke === "addListener") {
                                        return result;
                                    }
                                    if (functionToInvoke === "addGeoJson") {
                                        return result.map(coords => this.addObject(coords));
                                    }
                                    if ("get" in result) {
                                        return result.get("guidString");
                                    } else if ("dotnetTypeName" in result) {
                                        return extendableStringify(result, getCircularReplacer());
                                    } else {
                                        return JSON.parse(extendableStringify(result, getCircularReplacer()));
                                    }
                                } else if (functionToInvoke === "remove") {
                                    this.disposeObject(objectId);
                                } else {
                                    return result;
                                }
                            }
                    }
                } catch (e) {
                    console.log(e);
                    console.log(`\nfunctionToInvoke: ${functionToInvoke}\nargs: ${JSON.stringify(args2)}\n`);
                }
            },

            //Function could be extended in future: at the moment it is scoped for 
            //simple "Get" and "Set" properties of multiple objects of the same type
            invokeMultiple: async function (args) {
                const guids = JSON.parse(args[0]);
                const otherArg = args[1];
                const args2 = args.slice(2).map(arg => tryParseJson(arg));

                const results = {};
                const objs = guids.map(guid => mapObjects[guid]);

                // Collect promises
                const promises = guids.map((guid, index) => {
                    const args3 = [guid, otherArg, args2[index]];
                    const result = blazorGoogleMaps.objectManager.invoke(args3);

                    // Return a promise that resolves to the result
                    return Promise.resolve(result).then(resolvedResult => {
                        results[guid] = resolvedResult;
                    });
                });

                // Await all promises concurrently
                await Promise.all(promises);

                return results;
            },
            invokeWithReturnedObjectRef: async function (args) {
                const result = await blazorGoogleMaps.objectManager.invoke(args);
                const uuid = uuidv4();
                // This is needed to be able to remove events from map
                addMapObject(uuid, result)

                return uuid;
            },
            drawingManagerOverlaycomplete: function (args) {
                const [uuid, act] = args;
                const drawingManager = mapObjects[uuid];

                google.maps.event.addListener(drawingManager, "overlaycomplete", event => {
                    const overlayUuid = uuidv4();
                    addMapObject(overlayUuid, event.overlay)

                    const returnObj = extendableStringify([{ type: event.type, uuid: overlayUuid.toString() }]);

                    act.invokeMethodAsync("Invoke", returnObj, uuid);
                });
            },

            invokeMultipleWithReturnedObjectRef: function (args) {

                const guids = args[0];
                const otherArgs = args.slice(1, -1);
                const what = args[args.length - 1];

                const results = {};

                for (let i = 0; i < guids.length; i++) {
                    const uuid = uuidv4();
                    const args2 = [...guids[i], ...otherArgs, what[i]];

                    results[uuid] = blazorGoogleMaps.objectManager.invoke(args2);
                }

                return results;
            },

            //add event listeners to multiple objects of the same type
            addMultipleListeners: async function (args) {
                const guids = JSON.parse(args[0]);
                const eventName = args[1];
                const args2 = args.slice(2).map(arg => tryParseJson(arg));

                // Using Promise.all to handle multiple async operations if needed
                await Promise.all(guids.map((guid, index) => {
                    const obj = mapObjects[guid];
                    const additionalArgs = Array.isArray(args2[index]) ? args2[index] : [args2[index]];
                    const args3 = [guid, "addListener", eventName, ...additionalArgs];

                    return blazorGoogleMaps.objectManager.invoke(args3);
                }));

                return true;
            },

            readObjectPropertyValue: function (args) {
                const [objectId, property] = args;
                const obj = mapObjects[objectId];

                return obj?.[property];
            },

            readObjectPropertyValueWithReturnedObjectRef: function (args) {

                const [objectId, property] = args;
                const obj = mapObjects[objectId];
                const result = obj?.[property];
                const uuid = uuidv4();
                
                addMapObject(uuid, result)

                return uuid;
            },

            readObjectPropertyValueAndMapToArray: function (args) {
                const [objectId, property, propsJson] = args;
                const obj = mapObjects[objectId];
                let result = obj?.[property];
                const props = tryParseJson(propsJson);

                if (result && Array.isArray(result)) {
                    props.forEach(prop => {
                        result = result.map(item => item?.[prop]);
                    });
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
                addMapObject(guid, markerCluster)
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
            },
            updateAdvancedComponent: function (id, options, callbackRef) {
                const collisionBehaviorMapping = [
                    google.maps.CollisionBehavior.REQUIRED,
                    google.maps.CollisionBehavior.REQUIRED_AND_HIDES_OPTIONAL,
                    google.maps.CollisionBehavior.OPTIONAL_AND_HIDES_LOWER_PRIORITY,
                ];

                const {
                    position,
                    title,
                    zIndex,
                    gmpClickable,
                    gmpDraggable,
                    collisionBehavior,
                    mapId,
                    componentId,
                } = options;

                const invokeCallback = (method, ...args) => {
                    callbackRef?.invokeMethodAsync(method, ...args);
                };

                const setupClickListener = (marker, isEnabled) => {
                    if (isEnabled) {
                        marker.clickListener = marker.addListener("click", () => {
                            invokeCallback('OnMarkerClicked', id);
                        });
                    } else if (marker.clickListener) {
                        google.maps.event.removeListener(marker.clickListener);
                        delete marker.clickListener;
                    }
                };

                const setupDragListener = (marker, isEnabled) => {
                    if (isEnabled) {
                        marker.dragListener = marker.addListener("dragend", (event) => {
                            invokeCallback('OnMarkerDrag', id, marker.position);
                        });
                    } else if (marker.dragListener) {
                        google.maps.event.removeListener(marker.dragListener);
                        delete marker.dragListener;
                    }
                };

                const existingMarker = mapObjects[id];
                if (existingMarker) {
                    const clickChanged = existingMarker.gmpClickable !== gmpClickable;
                    const dragChanged = existingMarker.gmpDraggable !== gmpDraggable;

                    Object.assign(existingMarker, {
                        position,
                        title,
                        zIndex,
                        gmpClickable,
                        gmpDraggable,
                        collisionBehavior: collisionBehaviorMapping[collisionBehavior],
                    });

                    if (clickChanged) setupClickListener(existingMarker, gmpClickable);
                    if (dragChanged) setupDragListener(existingMarker, gmpDraggable);
                    return;
                }

                const map = mapObjects[mapId];

                const content = document.querySelector(`#${componentId}`);
                if (!content) {
                    console.warn("Marker tried to render without a target component");
                    return;
                }

                const advancedMarkerElement = new google.maps.marker.AdvancedMarkerElement({
                    map,
                    content,
                    position,
                    title,
                    zIndex,
                    gmpClickable,
                    gmpDraggable,
                    collisionBehavior: collisionBehaviorMapping[collisionBehavior],
                });

                advancedMarkerElement.guidString = id;

                setupClickListener(advancedMarkerElement, gmpClickable);
                setupDragListener(advancedMarkerElement, gmpDraggable);

                addMapObject(id, advancedMarkerElement);
            },
            disposeAdvancedMarkerComponent: function (id) {
                const existingMarker = mapObjects[id];
                if (!existingMarker) return;
                existingMarker.map = null;
                this.disposeObject(id);
            }
        }
    };
}();
