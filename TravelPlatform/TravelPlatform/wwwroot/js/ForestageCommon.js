var accessToken = localStorage.getItem("access_token");

if (accessToken === null) {
    $("#profile-login-unsuccess").show();
    $("#profile-login-success").hide();
}

let config = {
    headers: {
        Authorization: `Bearer ${accessToken}`,
    }
}

var profile;
var id = 0;

$(function () {
    // Go to chat room
    $("#contact").on("click", function () {
        if (id === 0) {
            window.location.href = "/Login.html";
        }
        else {
            window.location.href = "/Chatroom.html";
        }
    });
});

async function CheckLoginRequired() {
    // Check login

    if (accessToken === null) {
        window.location.href = "/Login.html";
    }

    axios.get("/api/v1.0/user/profile", config)
        .then((response) => {
            $("#profile-name").html(response.data.name);
            $("#profile-login-unsuccess").hide();
            $("#profile-login-success").show();
            profile = response.data;
            id = response.data.id;
        })
        .catch((error) => {
            console.log(error);

            localStorage.removeItem("access_token");
            window.location.href = "/Login.html";
        });

    return new Promise(function (resolve, reject) {
        setTimeout(function () {
            resolve(profile);
        }, 300);
    });
}

async function CheckLoginToShowName() {
    // Check login
    var accessToken = localStorage.getItem("access_token");
    if (accessToken === null) {
        $("#profile-login-unsuccess").show();
        $("#profile-login-success").hide();
    }

    axios.get("/api/v1.0/user/profile", config)
        .then((response) => {
            $("#profile-name").html(response.data.name);
            $("#profile-login-unsuccess").hide();
            $("#profile-login-success").show();
            id = response.data.id;
        })
        .catch((error) => {
            console.log(error);
            localStorage.removeItem("access_token");
            $("#profile-login-unsuccess").show();
            $("#profile-login-success").hide();
        });
}