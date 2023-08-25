$(function () {
    var connection = new signalR.HubConnectionBuilder().withUrl("/hubs/ChatHub").build();

    var roomId = "";
    var userId = "";

    //與Server建立連線
    connection.start().then(function () {
        console.log("Hub 連線完成");
    }).catch(function (err) {
        alert('連線錯誤: ' + err.toString());
    });

    //傳送訊息
    $('#sendButton').on('click', function () {
        var msg = $("#inputMsg").val();
        connection.invoke("SendMessage", roomId, userId, msg).catch(function (err) {
            alert('傳送錯誤: ' + err.toString());
        });
    });

    // 更新連線 Room ID
    connection.on("YourRoomID", function (id) {
        roomId = id;
        console.log(roomId);
    });

    // 更新連線 User ID
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

    // 更新聊天內容事件
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