var contact = {
    init: function () {
        contact.registerEvent();
    },
    registerEvent: function () {
        contact.initMap();
    },
    initMap: function () {
        var hcmute = { lat: parseFloat($('#hidLat').val()), lng: parseFloat($('#hidLng').val()) };
        var map = new google.maps.Map(document.getElementById('map'), {
            zoom: 17,
            center: hcmute
        });

        var contentString = $('#hidAddress').val();;
        var infowindow = new google.maps.InfoWindow({
            content: contentString
        });

        var marker = new google.maps.Marker({
            position: hcmute,
            map: map,
            title: $('#hidName').val()
        });
        marker.addListener("click", () => {

        });
        infowindow.open(map, marker);
    }
}
contact.init();