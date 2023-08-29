var connection = new signalR.HubConnectionBuilder().withUrl("/hubs/ChatHub").build();

var myRoomId = "";
var myUserId = "";
var chatMessage = new Object();

$(function () {
    CheckLogin();

    const urlParams = new URLSearchParams(window.location.search);
    myUserId = urlParams.get('id');

    //�PServer�إ߳s�u
    connection.start().then(function () {
        console.log("Hub �s�u����");

        // �[�J��ѫ�
        connection.invoke("JoinGroup", myUserId.toString()).catch(function (error) {
            alert("�L�k�[�J��ѫ�: " + error.toString());
        });

        chatMessage.roomId = myUserId;
        chatMessage.senderId = 1;

        GetChatRecord(myUserId);
    }).catch(function (err) {
        alert('�s�u���~: ' + err.toString());
    });

    // ��s�s�u User ID
    connection.on("YourUserID", function (id) {
        myUserId = id;
    });

    // ��s�s�u Room ID
    connection.on("YourRoomID", function (id) {
        myRoomId = id;
    });

    // �ǰe�T��
    $('#sendButton').on('click', function () {
        var msg = $("#inputMsg").val();
        connection.invoke("SendMessage", myRoomId, 'Client', msg).catch(function (err) {
            alert('�ǰe���~: ' + err.toString());
        });
        $("#inputMsg").val('');
        chatMessage.message = msg;
        SaveChatMessage(chatMessage);
    });

    // ��s�T��
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
});

async function GetChatRecord(roomId) {
    $("#chatroomContent").html('');
    await axios.get(`/api/v1.0/Chat/GetChatRecord?roomId=${roomId}`)
        .then((response) => {
            var records = response.data.record;

            records.forEach((record) => {
                if (record.sender === 0) {
                    var item_left = `<div class="col-md-12">
                                <h4>
                                    <p>Admin</p>
                                    <span class="badge badge-light badge-pill">${record.message}</span>
                                </h4>
                            </div>`;
                    $("#chatroomContent").append(item_left);
                }
                else {
                    var item_right = `<div class="col-md-12 text-right">
                                <h4>
                                    <p>Client</p>
                                    <span class="badge badge-light badge-pill">${record.message}</span>
                                </h4>
                            </div>`;
                    $("#chatroomContent").append(item_right);
                }
            });
        })
        .catch((error) => {
            alert(error);
        })
};

async function SaveChatMessage(chatMessage) {
    await axios.post(`/api/v1.0/Chat/SaveChatMessage`, chatMessage)
        .catch((error) => alert(error));
}