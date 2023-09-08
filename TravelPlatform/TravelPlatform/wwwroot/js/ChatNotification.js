var connection = new signalR.HubConnectionBuilder().withUrl("/hubs/ChatHub").build();

$(function () {
    //與Server建立連線
    connection.start().then(function () {
        console.log("Hub 連線完成");
    }).catch(function (err) {
        toastr.error("連線錯誤: " + error.toString(), '錯誤');
    });

    // 更新新訊息通知 (Client端)
    connection.on("UpdClientNotification", function (notification) {
        console.log(notification);
        if (notification) {
            $(".notification-circle").removeClass("hidden").addClass("shown");
        }
        else {
            $(".notification-circle").removeClass("shown").addClass("hidden");
        }
    });

    // 更新新訊息通知 (Admin端)
    connection.on("UpdAdminNotification", function (notification) {
        console.log(notification);
    });
});