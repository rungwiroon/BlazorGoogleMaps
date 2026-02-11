window.mapDataPage = {
    getFeatureStyle: function (mapData, feature) {
        let fillColour = feature.getProperty('fillColor');
        let strokeColour = feature.getProperty('strokeColor');
        return {
            fillColor: fillColour,
            fillOpacity: 0.3,
            strokeColor: strokeColour,
            strokeWeight: 2,
            strokeOpacity: 1
        };
    }
}