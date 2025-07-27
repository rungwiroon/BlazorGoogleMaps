﻿export function initMapLegend() {
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
}
