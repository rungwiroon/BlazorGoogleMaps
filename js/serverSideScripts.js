export function initMapLegend() {
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

export function createBunchOfCirclesInJs(mapId, numberOfCircles) {

    var map = window.blazorGoogleMaps.objectManager.mapObjects[mapId];
    for (let i = 0; i < numberOfCircles; i++) {
        const circle = new google.maps.Circle({
            strokColor: getRandomColor(),
            strokeOpacity: 0.6,
            strokeWeight: 2,
            fillColor: getRandomColor(),
            fillOpacity: 0.35,
            map: map,
            center: getRandomLocation(map.getCenter(), 10000), // 10km radius from the center
            radius: 2,
        });
    }
    function getRandomColor() {
        const letters = '0123456789ABCDEF';
        let color = '#';
        for (let i = 0; i < 6; i++) {
            color += letters[Math.floor(Math.random() * 16)];
        }
        return color;
    }

    function getRandomLocation(center, radius) {
        const x0 = center.lng();
        const y0 = center.lat();
        const rd = radius / 111300; // Convert radius to degrees

        const u = Math.random();
        const v = Math.random();

        const w = rd * Math.sqrt(u);
        const t = 2 * Math.PI * v;
        const x = w * Math.cos(t);
        const y = w * Math.sin(t);

        const newX = x / Math.cos(y0);

        return {
            lat: y0 + y,
            lng: x0 + newX
        };
    }
}
export function initServerSideScript() {
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

                // create advanced marker using svg icon
                const container = document.createElement("div");
                container.style.position = "relative";
                container.style.width = "50px";
                container.style.height = "50px";
                container.innerHTML = "<svg fill=\"#0000ff\" xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 240 240\"><circle cx=\"120\" cy=\"120\" opacity=\".6\" r=\"70\"></circle><circle cx=\"120\" cy=\"120\" opacity=\".3\" r=\"90\"></circle><circle cx=\"120\" cy=\"120\" opacity=\".2\" r=\"110\"></circle></svg>";

                const label = document.createElement("div");
                label.textContent = countText;
                label.style.position = "absolute";
                label.style.top = "50%";
                label.style.left = "50%";
                label.style.transform = "translate(-50%, -50%)";
                label.style.color = "#ffffff";
                label.style.fontSize = "16px";
                label.style.fontWeight = "bold";
                label.style.fontFamily = "Poppins";
                container.appendChild(label);

                return new google.maps.marker.AdvancedMarkerElement({
                    position,
                    content: container,
                    // adjust zIndex to be above other markers
                    zIndex: 100000 + count,
                });
            },
        }
    };
}
