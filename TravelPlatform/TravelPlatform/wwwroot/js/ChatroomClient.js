$(function () {
    var connection = new signalR.HubConnectionBuilder().withUrl("/hubs/ChatHub").build();

    //�PServer�إ߳s�u
    connection.start().then(function () {
        console.log("Hub �s�u����");
    }).catch(function (err) {
        alert('�s�u���~: ' + err.toString());
    });
})