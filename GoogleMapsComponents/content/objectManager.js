var stringToFunction = function (str) {
    var arr = str.split(".");

    var fn = window || this;
    for (var i = 0, len = arr.length; i < len; i++) {
        fn = fn[arr[i]];
    }

    if (typeof fn !== "function") {
        throw new Error("function not found");
    }

    return fn;
};

function tryParseJson(item) {

    if (item.hasOwnProperty("invokeMethodAsync")) {
        return async function () {
            var args2 = arguments.map(arg => {
                if (arg !== Object(arg)) {
                    return arg;
                } else {
                    return JSON.stringify(args);
                }
            });

            await item.invokeMethodAsync('Invoke', ...args2);
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

    if (typeof item2 === "object" && item2 !== null)
        return item2;

    return item.replace(/['"]+/g, '');
}

window.googleMapsObjectManager = {
    createObject: function (args) {
        window._blazorGoogleMapsObjects = window._blazorGoogleMapsObjects || [];

        var args2 = args.slice(2).map(arg => tryParseJson(arg));

        //console.log(args2);

        let constructor = stringToFunction(args[1]);
        window._blazorGoogleMapsObjects[args[0].replace(/['"]+/g, '')] = new constructor(...args2);
    },

    disposeObject: function (guid) {
        delete window._blazorGoogleMapsObjects[guid];
    },

    invoke: function (args) {
        var args2 = args.slice(2).map(arg => tryParseJson(arg));

        let obj = window._blazorGoogleMapsObjects[args[0]];

        //console.log("Invoke " + methodName);
        //console.dir(window._blazorGoogleMapsObjects);
        //console.dir(args);
        //console.dir(args2);

        return obj[args[1]](...args2);
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