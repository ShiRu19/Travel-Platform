var connection = new signalR.HubConnectionBuilder().withUrl("/hubs/ChatHub").build();

var myRoomId = "";
var myUserId = "";
var myUserId = "";
$(function () {

    //與Server建立連線
    connection.start().then(function () {
        console.log("Hub 連線完成");

        // 加入聊天室
        myUserId = prompt("What is your user id?");
        connection.invoke("JoinGroup", myUserId.toString()).catch(function (error) {
            alert("無法加入聊天室: " + error.toString());
        });
    }).catch(function (err) {
        alert('連線錯誤: ' + err.toString());
    });

    // 更新連線 User ID
    connection.on("YourUserID", function (id) {
        myUserId = id;
        console.log("userId" + myUserId);
    });

    // 更新連線 Room ID
    connection.on("YourRoomID", function (id) {
        myRoomId = id;
        console.log(myRoomId);
    });

    // 傳送訊息
    $('#sendButton').on('click', function () {
        var msg = $("#inputMsg").val();
        connection.invoke("SendMessage", myRoomId, 'Client', msg).catch(function (err) {
            alert('傳送錯誤: ' + err.toString());
        });
        $("#inputMsg").val('');
    });

    // 更新訊息
    connection.on("UpdContent", function (userId, msg) {
        if (userId === 'Admin') {
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
})