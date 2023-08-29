
$(function () {
    // Check login
    var id = 0;
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

    // Go to chat room
    $("#contact").on("click", function () {
        if (id === 0) {
            window.location.href = "/Login.html";
        }
        else {
            window.location.href = "/ChatRoom.html?id=" + id;
        }
    });
});