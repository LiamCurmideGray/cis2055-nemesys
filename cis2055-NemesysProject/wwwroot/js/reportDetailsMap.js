let map;

function initMap(lat, lng) {
    var latitude = parseFloat(lat);
    var longitude = parseFloat(lng)
    const myLatLng = { lat: latitude, lng: longitude };
    map = new google.maps.Map(document.getElementById("map"), {
        center: { lat: latitude, lng: longitude },
        zoom: 17,
        options: {
            gestureHandling: 'greedy'
        }
    });
    originalPoint = new google.maps.Marker({
        position: myLatLng,
        map,
        title: "Original hazard pinpoint",
    });
}