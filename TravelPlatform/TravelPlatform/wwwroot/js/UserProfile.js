$(function () {
    // Check login
    var id = 0;
    var accessToken = localStorage.getItem("access_token");
    if (accessToken === null) {
        window.location.href = "/Login.html";
    }

    let config = {
        headers: {
            Authorization: `Bearer ${accessToken}`,
        }
    }

    axios.get("/api/v1.0/user/profile", config)
        .then((response) => {
            console.log(response.data);
            $("#profile-name").html(response.data.name);
            $("#profile-login-unsuccess").hide();
            $("#profile-login-success").show();

            $("#user-name").html(response.data.name);
            $("#user-email").html(response.data.email);
        })
        .catch((error) => {
            console.log(error);
            if (error.response.status === 401) {
                window.location.href = "/pages/401.html";
            }
            else if (error.response.status === 403) {
                window.location.href = "/pages/403.html";
            }
            else {
                localStorage.removeItem("access_token");
                window.location.href = "/Login.html";
            }
        });

    axios.get("/api/v1.0/user/CheckAdminRole", config)
        .then((response) => {
            $("#go-backstage-container").show();
        })
        .catch((error) => {
            $("#go-backstage-container").hide();
        })

    $("#go-backstage-btn").on("click", function () {
        window.location.href = "/admin/Dashboard.html";
    });

    $("#logout-btn").on("click", function () {
        localStorage.removeItem("access_token");
        window.location.href = "/Login.html";
    })
});