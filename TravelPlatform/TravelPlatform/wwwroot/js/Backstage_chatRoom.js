var connection = new signalR.HubConnectionBuilder().withUrl("/hubs/ChatHub").build();
var roomId = "";
var userId = "";
$(function () {
    // �P Server �إ߳s�u
    connection.start().then(function () {
        console.log("Hub �s�u����");
    }).catch(function (err) {
        alert('�s�u���~: ' + err.toString());
    });

    // �[�J��ѫ�
    $("#join-btn").on("click", function () {
        let room_id = $("#room-id").val();
        connection.invoke("JoinGroup", room_id).catch(function (err) {
            alert('�L�k�[�J: ' + err.toString());
        });
    });

    //�ǰe�T��
    $('#sendButton').on('click', function () {
        var msg = $("#inputMsg").val();
        connection.invoke("SendMessage", roomId, userId, msg).catch(function (err) {
            alert('�ǰe���~: ' + err.toString());
        });
    });

    // ��s�s�u Room ID
    connection.on("YourRoomID", function (id) {
        roomId = id;
    });

    // ��s�s�u User ID
    connection.on("YourUserID", function (id) {
        userId = id;
    });

    // ��s�s�u Room ID �C��ƥ�
    connection.on("UpdRooms", function (jsonList) {
        var list = JSON.parse(jsonList);
        $("#IDList-room li").remove();
        for (i = 0; i < list.length; i++) {
            $("#IDList-room").append($("<li></li>").attr("class", "list-group-item").text(list[i]));
        }
    });

    // ��s�s�u User ID �C��ƥ�
    connection.on("UpdUsers", function (jsonList) {
        var list = JSON.parse(jsonList);
        $("#IDList-user li").remove();
        for (i = 0; i < list.length; i++) {
            $("#IDList-user").append($("<li></li>").attr("class", "list-group-item").text(list[i]));
        }
    });

    // ��s�U�ж��H��
    connection.on("UpdUserCount", function (jsonList) {
        console.log(jsonList);
    });

    // ��s��Ѥ��e�ƥ�
    connection.on("UpdContent", function (userId, msg) {
        var item_left = `<div class="col-md-12">
                            <h4>
                                <p>${userId}</p>
                                <span class="badge badge-light badge-pill">${msg}</span>
                            </h4>
                        </div>`;

        $("#chatroomContent").append(item_left);
    });
});