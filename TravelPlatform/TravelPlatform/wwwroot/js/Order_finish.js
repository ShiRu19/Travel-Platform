$(function () {
    CheckLoginRequired().then(function (profile) {
        const urlParams = new URLSearchParams(window.location.search);
        const orderId = urlParams.get('id');
        $("#order-id").html(orderId);
    });
});