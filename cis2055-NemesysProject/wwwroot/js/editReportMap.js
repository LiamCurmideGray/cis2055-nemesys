let map;
var pinpoints = [];
var latitude = parseFloat(document.getElementById("Latitude").value);
var longitude = parseFloat(document.getElementById("Longitude").value);

function initMap() {
    const myLatLng = { lat: latitude, lng: longitude };
    map = new google.maps.Map(document.getElementById("map"), {
        center: { lat: latitude, lng: longitude },
        zoom: 17,
        options: {
            gestureHandling: 'greedy'
        }
    });
    var originalPoint = new google.maps.Marker({
        position: myLatLng,
        map,
        title: "Original hazard pinpoint",
    });
    map.addListener("click", (mapsMouseEvent) => {
        originalPoint.setMap(null);
        addPinpoint(map, mapsMouseEvent)
    });
    originalPoint.addListener("rightclick", function () {
        originalPoint.setMap(null);
        document.getElementById("Latitude").value = null;
        document.getElementById("Longitude").value = null;
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
        document.getElementById("Latitude").value = null;
        document.getElementById("Longitude").value = null;
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

    var lat = mapsMouseEvent.latLng.lat();
    var lng = mapsMouseEvent.latLng.lng();
    document.getElementById("Latitude").value = lat;
    document.getElementById("Longitude").value = lng;
}