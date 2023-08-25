$(function () {
    var connection = new signalR.HubConnectionBuilder().withUrl("/hubs/ChatHub").build();

    //與Server建立連線
    connection.start().then(function () {
        console.log("Hub 連線完成");
    }).catch(function (err) {
        alert('連線錯誤: ' + err.toString());
    });
})