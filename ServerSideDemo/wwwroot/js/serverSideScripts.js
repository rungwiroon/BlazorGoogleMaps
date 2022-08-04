function initMapLegend() {
    var iconBase = 'https://maps.google.com/mapfiles/kml/shapes/';
    var icons = {
        parking: {
            name: 'Parking',
            icon: iconBase + 'parking_lot_maps.png'
        },
        library: {
            name: 'Library',
            icon: iconBase + 'library_maps.png'
        },
        info: {
            name: 'Info',
            icon: iconBase + 'info-i_maps.png'
        }
    };

    var legend = document.getElementById('legend');
    for (var key in icons) {
        var type = icons[key];
        var name = type.name;
        var icon = type.icon;
        var div = document.createElement('div');
        div.innerHTML = '<img src="' + icon + '"> ' + name;
        legend.appendChild(div);
    }
}

google.maps.Polyline.prototype.AddListeners = function () {
    // getpath
    console.log('Add Listeners Called.');
    var poly = this;
    var path = this.getPath();

    // addlistener insert_at
    event.initEvent('insert_at', true, true);
    google.maps.event.addListener(path, 'insert_at', function (vertex) {
        // event auf polyline auslösen
        console.log('Vertex ' + vertex + ' inserted to path.');
        google.maps.event.trigger(poly, 'insert_at', vertex);
    });

    // addlistener set_at
    event.initEvent('set_at', true, true);
    google.maps.event.addListener(path, 'set_at', function (vertex) {
        // event auf polyline auslösen
        console.log('Vertex ' + vertex + ' set on path.');
        google.maps.event.trigger(poly, 'set_at', vertex);
    });

    // addlistener remove_at
    event.initEvent('remove_at', true, true);
    google.maps.event.addListener(path, 'remove_at', function (vertex) {
        // event auf polyline auslösen
        console.log('Vertex ' + vertex + ' removed from path.');
        google.maps.event.trigger(poly, 'remove_at', vertex);
    });
}
google.maps.Polygon.prototype.AddListeners = function () {
    // getpath
    console.log('Add Listeners Called.');
    var poly = this;
    var path = this.getPath();

    // addlistener insert_at
    event.initEvent('insert_at', true, true);
    google.maps.event.addListener(path, 'insert_at', function (vertex) {
        // event auf polyline auslösen
        console.log('Vertex ' + vertex + ' inserted to path.');
        google.maps.event.trigger(poly, 'insert_at', vertex);
    });

    // addlistener set_at
    event.initEvent('set_at', true, true);
    google.maps.event.addListener(path, 'set_at', function (vertex) {
        // event auf polyline auslösen
        console.log('Vertex ' + vertex + ' set on path.');
        google.maps.event.trigger(poly, 'set_at', vertex);
    });

    // addlistener remove_at
    event.initEvent('remove_at', true, true);
    google.maps.event.addListener(path, 'remove_at', function (vertex) {
        // event auf polyline auslösen
        console.log('Vertex ' + vertex + ' removed from path.');
        google.maps.event.trigger(poly, 'remove_at', vertex);
    });
}

window.customRendererLib = {
    interpolatedRenderer: {
        render: function ({ count, position }, stats) {
            let countText;
            try {
                let formatter = Intl.NumberFormat('en', { notation: 'compact' });
                countText = formatter.format(count);
            } catch {
                countText = String(count);
            }

            // create marker using svg icon
            return new google.maps.Marker({
                position,
                icon: {
                    url: "data:image/svg+xml;base64,CiAgPHN2ZyBmaWxsPSIjMDAwMGZmIiB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHZpZXdCb3g9IjAgMCAyNDAgMjQwIj4KICAgIDxjaXJjbGUgY3g9IjEyMCIgY3k9IjEyMCIgb3BhY2l0eT0iLjYiIHI9IjcwIiAvPgogICAgPGNpcmNsZSBjeD0iMTIwIiBjeT0iMTIwIiBvcGFjaXR5PSIuMyIgcj0iOTAiIC8+CiAgICA8Y2lyY2xlIGN4PSIxMjAiIGN5PSIxMjAiIG9wYWNpdHk9Ii4yIiByPSIxMTAiIC8+CiAgPC9zdmc+",
                    scaledSize: new google.maps.Size(50, 50),
                },
                label: {
                    text: countText,
                    color: "#ffffff",
                    fontSize: "16px",
                    fontWeight: "bold",
                    fontFamily: "Poppins"
                },
                // adjust zIndex to be above other markers
                zIndex: Number(google.maps.Marker.MAX_ZINDEX) + count,
            });
        },
    }
};