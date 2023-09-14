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

async function CheckLoginRequired() {
    // Check login

    if (accessToken === null) {
        window.location.href = "/Login.html";
    }

    axios.get("/api/v1.0/user/profile", config)
        .then((response) => {
            profile = response.data.data;
            $("#profile-name").html(profile.name);
            $("#profile-login-unsuccess").hide();
            $("#profile-login-success").show();
            id = profile.id;
        })
        .catch((error) => {
            ShowErrorMessage(error);

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
            var profile = response.data.data;
            $("#profile-name").html(profile.name);
            $("#profile-login-unsuccess").hide();
            $("#profile-login-success").show();
            id = profile.id;
        })
        .catch((error) => {
            ShowErrorMessage(error);

            localStorage.removeItem("access_token");
            $("#profile-login-unsuccess").show();
            $("#profile-login-success").hide();
        });
}