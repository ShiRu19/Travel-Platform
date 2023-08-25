$(function () {
    var connection = new signalR.HubConnectionBuilder().withUrl("/hubs/ChatHub").build();

    var roomId = "";
    var userId = "";

    //�PServer�إ߳s�u
    connection.start().then(function () {
        console.log("Hub �s�u����");
    }).catch(function (err) {
        alert('�s�u���~: ' + err.toString());
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
        console.log(roomId);
    });

    // ��s�s�u User ID
    connection.on("YourUserID", function (id) {
        userId = id;
        console.log(userId);
    });

    connection.on("UpdContent", function (userId, msg) {
        console.log("111");
        var item_left = `<div class="col-md-12">
                            <h4>
                                <p>${userId}</p>
                                <span class="badge badge-light badge-pill">${msg}</span>
                            </h4>
                        </div>`;

        $("#chatroomContent").append(item_left);
    });

    // ��s��Ѥ��e�ƥ�
    //connection.on("UpdContent", function (userId, msg) {
    //    console.log("123456789");
    //    //var item_left = `<div class="col-md-12">
    //    //                    <h4>
    //    //                        <p>${userId}</p>
    //    //                        <span class="badge badge-light badge-pill">${msg}</span>
    //    //                    </h4>
    //    //                </div>`;

    //    //$("#chatroomContent").append(item_left);
    //});
})