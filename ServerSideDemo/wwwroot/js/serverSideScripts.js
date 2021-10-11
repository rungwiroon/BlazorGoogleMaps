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

google.maps.Polyline.prototype.AddListeners = function()
{
    // getpath
    console.log('Add Listeners Called.');
    var poly = this;
    var path = this.getPath();

    // addlistener insert_at
    event.initEvent('insert_at', true, true);
    google.maps.event.addListener(path, 'insert_at', function(vertex) {
        // event auf polyline auslösen
        console.log('Vertex ' + vertex + ' inserted to path.');
        google.maps.event.trigger(poly, 'insert_at', vertex);
    });

    // addlistener set_at
    event.initEvent('set_at', true, true);
    google.maps.event.addListener(path, 'set_at', function(vertex) {
        // event auf polyline auslösen
        console.log('Vertex ' + vertex + ' set on path.');
        google.maps.event.trigger(poly, 'set_at', vertex);
    });

    // addlistener remove_at
    event.initEvent('remove_at', true, true);
    google.maps.event.addListener(path, 'remove_at', function(vertex) {
        // event auf polyline auslösen
        console.log('Vertex ' + vertex + ' removed from path.');
        google.maps.event.trigger(poly, 'remove_at', vertex);
    });
}
google.maps.Polygon.prototype.AddListeners = function()
{
    // getpath
    console.log('Add Listeners Called.');
    var poly = this;
    var path = this.getPath();

    // addlistener insert_at
    event.initEvent('insert_at', true, true);
    google.maps.event.addListener(path, 'insert_at', function(vertex) {
        // event auf polyline auslösen
        console.log('Vertex ' + vertex + ' inserted to path.');
        google.maps.event.trigger(poly, 'insert_at', vertex);
    });

    // addlistener set_at
    event.initEvent('set_at', true, true);
    google.maps.event.addListener(path, 'set_at', function(vertex) {
        // event auf polyline auslösen
        console.log('Vertex ' + vertex + ' set on path.');
        google.maps.event.trigger(poly, 'set_at', vertex);
    });

    // addlistener remove_at
    event.initEvent('remove_at', true, true);
    google.maps.event.addListener(path, 'remove_at', function(vertex) {
        // event auf polyline auslösen
        console.log('Vertex ' + vertex + ' removed from path.');
        google.maps.event.trigger(poly, 'remove_at', vertex);
    });
}