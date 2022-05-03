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

//export { googleMapsObjectManager }


//window.customRendererLib = {
//    interpolatedRenderer: {
//        render: function ({ count, position }, stats) {
//            const color = count > Math.max(5, stats.clusters.markers.mean) ? "#F00" : "#00F";

//            let countText;
//            try {
//                let formatter = Intl.NumberFormat('en', { notation: 'compact' });
//                countText = formatter.format(count);
//            } catch {
//                countText = String(count);
//            }

//            // create svg url with fill color
//            const svg = window.btoa(`
//  <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 240 240">
//    <circle cx="120" cy="120" opacity=".1" r="120" fill="#000000"/>
//    <circle cx="120" cy="120" opacity="1" r="100" fill="#ffffff"/>
//    <circle cx="120" cy="120" opacity="1" r="64" fill="${color}"/>
//  </svg>`);
//            // create marker using svg icon
//            return new google.maps.Marker({
//                position,
//                icon: {
//                    url: `data:image/svg+xml;base64,${svg}`,
//                    scaledSize: new google.maps.Size(50, 50),
//                },
//                label: {
//                    text: countText,
//                    color: "#ffffff",
//                    fontSize: "16px",
//                    fontWeight: "bold",
//                    fontFamily: "Open Sans"
//                },
//                // adjust zIndex to be above other markers
//                zIndex: Number(google.maps.Marker.MAX_ZINDEX) + count,
//            });
//        },
//    }
//};