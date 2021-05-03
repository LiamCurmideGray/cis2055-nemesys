let map;
var pinpoints = [];

function initMap() {
    map = new google.maps.Map(document.getElementById("map"), {
        center: { lat: 35.9375, lng: 14.3754 },
        zoom: 11,
        options: {
            gestureHandling: 'greedy'
        }
    });

    map.addListener("click", (mapsMouseEvent) => {
        addPinpoint(map, mapsMouseEvent)
    });
}

function addPinpoint(map, mapsMouseEvent) {
    var pinpoint = new google.maps.Marker({
        position: mapsMouseEvent.latLng,
        title: "Hazard Location"
    });


    pinpoint.addListener("rightclick", function () {
        for (let i = 0; i < pinpoints.length; i++) {
            if (pinpoints[i].pinpoint == pinpoint) {
                pinpoints.splice(i, 1);
                pinpoint.setMap(null);
            }
        }
        document.getElementById('latlng').innerHTML = null;
    });

    map.addListener("click", (mapsMouseEvent) => {
        for (let i = 0; i < pinpoints.length; i++) {
            if (pinpoints[i].pinpoint == pinpoint) {
                pinpoints.splice(i, 1);
                pinpoint.setMap(null);
            }
        }
        addPinpoint(map, mapsMouseEvent)
    });

    var pinpointObj = {
        pinpoint: pinpoint
    }

    if (pinpoints.length == 0) {
        pinpoints.push(pinpointObj);
        pinpoint.setMap(map);
    }

    var latlng = mapsMouseEvent.latLng;
    document.getElementById('latlng').innerHTML = latlng;
}