var connection = new signalR.HubConnectionBuilder().withUrl("/hubs/ChatHub").build();

$(function () {
    //�PServer�إ߳s�u
    connection.start().then(function () {
        console.log("Hub �s�u����");
    }).catch(function (err) {
        toastr.error("�s�u���~: " + error.toString(), '���~');
    });

    // ��s�s�T���q�� (Client��)
    connection.on("UpdClientNotification", function (notification) {
        console.log(notification);
        if (notification) {
            $(".notification-circle").removeClass("hidden").addClass("shown");
        }
        else {
            $(".notification-circle").removeClass("shown").addClass("hidden");
        }
    });

    // ��s�s�T���q�� (Admin��)
    connection.on("UpdAdminNotification", function (notification) {
        console.log(notification);
    });
});