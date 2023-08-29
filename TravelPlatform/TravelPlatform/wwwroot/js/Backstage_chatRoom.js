var connection = new signalR.HubConnectionBuilder().withUrl("/hubs/ChatHub").build();
var myRoomId = "";
var myUserId = "";
$(function () {
    // 與 Server 建立連線
    connection.start().then(function () {
        console.log("Hub 連線完成");
    }).catch(function (err) {
        alert('連線錯誤: ' + err.toString());
    });

    // 加入聊天室
    $(".roomId").on("click", function () {
        let room_id = $(this)[0].dataset.roomid
        connection.invoke("JoinGroup", room_id).catch(function (err) {
            alert('無法加入: ' + err.toString());
        });
        $("#title-roomId").html(' - ' + room_id);
        $("#chatroomContent").html('');
    });

    //傳送訊息
    $('#sendButton').on('click', function () {
        var msg = $("#inputMsg").val();
        connection.invoke("SendMessage", myRoomId, 'Admin', msg).catch(function (err) {
            alert('傳送錯誤: ' + err.toString());
        });
        $("#inputMsg").val("");
    });

    // 更新連線 Room ID
    connection.on("YourRoomID", function (id) {
        myRoomId = id;
        console.log("Room id = " + myRoomId);
    });

    // 更新連線 User ID
    connection.on("YourUserID", function (id) {
        myUserId = id;
        console.log("User id = " + myUserId);
    });

    // 更新連線 Room ID 列表事件
    connection.on("UpdRooms", function (jsonList) {
        var list = JSON.parse(jsonList);
        $("#IDList-room li").remove();
        for (i = 0; i < list.length; i++) {
            $("#IDList-room").append($("<li></li>").attr("class", "list-group-item").text(list[i]));
        }
    });

    // 更新聊天內容事件
    connection.on("UpdContent", function (userId, msg) {
        if (userId === 'Client') {
            var item_left = `<div class="col-md-12">
                                <h4>
                                    <p>${userId}</p>
                                    <span class="badge badge-light badge-pill">${msg}</span>
                                </h4>
                            </div>`;
            $("#chatroomContent").append(item_left);
        }
        else {
            var item_right = `<div class="col-md-12 text-right">
                                <h4>
                                    <p>${userId}</p>
                                    <span class="badge badge-light badge-pill">${msg}</span>
                                </h4>
                            </div>`;
            $("#chatroomContent").append(item_right);
        }
    });
});