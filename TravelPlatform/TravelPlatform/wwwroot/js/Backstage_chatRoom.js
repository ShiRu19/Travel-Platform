var connection = new signalR.HubConnectionBuilder().withUrl("/hubs/ChatHub").build();
var myRoomId = "";
var myUserId = "";
var chatMessage = new Object();

$(function () {
    // 與 Server 建立連線
    connection.start().then(function () {
        console.log("Hub 連線完成");
        UpdateRoomList();
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
        
        chatMessage.roomId = room_id;
        chatMessage.senderId = 0;

        GetChatRecord(room_id);
    });

    //傳送訊息
    $('#sendButton').on('click', function () {
        var msg = $("#inputMsg").val();
        connection.invoke("SendMessage", myRoomId, 'Admin', msg).catch(function (err) {
            alert('傳送錯誤: ' + err.toString());
        });
        $("#inputMsg").val("");

        chatMessage.message = msg;
        SaveChatMessage(chatMessage);
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

    // 更新連線 Room ID 列表

    connection.on("UpdRooms", function (jsonList) {
        UpdateRoomList();
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

async function GetChatRecord(roomId) {
    $("#chatroomContent").html('');

    await axios.get(`/api/v1.0/Chat/GetChatRecord?roomId=${roomId}`)
        .then((response) => {
            var records = response.data.record;

            records.forEach((record) => {
                if (record.sender === 1) {
                    var item_left = `<div class="col-md-12">
                                <h4>
                                    <p>Client</p>
                                    <span class="badge badge-light badge-pill">${record.message}</span>
                                </h4>
                            </div>`;
                    $("#chatroomContent").append(item_left);
                }
                else {
                    var item_right = `<div class="col-md-12 text-right">
                                <h4>
                                    <p>Admin</p>
                                    <span class="badge badge-light badge-pill">${record.message}</span>
                                </h4>
                            </div>`;
                    $("#chatroomContent").append(item_right);
                }
            });
        })
        .catch((error) => {
            console.log(error);
            alert("抱歉...發生了一些錯誤，請再試一次！");
        })
};

async function SaveChatMessage(chatMessage) {
    await axios.post(`/api/v1.0/Chat/SaveChatMessage`, chatMessage)
        .catch((error) => {
            console.log(error);
            alert("抱歉...發生了一些錯誤，請再試一次！");
        });
}

function UpdateRoomList() {
    $("#chatroom-list tr").remove();

    axios.get("/api/v1.0/Chat/GetChatRoomList")
        .then((response) => {
            var list = response.data;
            for (i = 0; i < list.length; i++) {
                var item = `<tr>
                            <td class="roomId" data-roomid="${list[i]}" onclick="JoinRoom(this)">${list[i]}</td>
                        </tr>`
                $("#chatroom-list").append(item);
            }
        })
        .catch((error) => {
            console.log(error);
            alert("抱歉...發生了一些錯誤，請再試一次！");
        });
}

function JoinRoom(room) {
    let room_id = room.dataset.roomid;
    console.log(room_id);
    connection.invoke("JoinGroup", room_id).catch(function (err) {
        alert('無法加入: ' + err.toString());
    });

    $("#title-roomId").html(' - ' + room_id);

    chatMessage.roomId = room_id;
    chatMessage.senderId = 0;

    GetChatRecord(room_id);
}