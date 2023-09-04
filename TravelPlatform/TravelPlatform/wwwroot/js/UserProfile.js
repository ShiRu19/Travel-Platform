$(function () {
    CheckLoginRequired().then(function (profile) {
        axios.get("/api/v1.0/user/CheckAdminRole", config)
            .then((response) => {
                $("#user-name").html(profile.name);
                $("#user-email").html(profile.email);
                $("#go-backstage-container").removeClass("hidden").addClass("shown");
            })
            .catch((error) => {
                $("#user-name").html(profile.name);
                $("#user-email").html(profile.email);
                $("#go-backstage-container").removeClass("shown").addClass("hidden");
            })
    });

    $("#go-backstage-btn").on("click", function () {
        window.location.href = "/admin/Dashboard.html";
    });

    $("#logout-btn").on("click", function () {
        localStorage.removeItem("access_token");
        window.location.href = "/Login.html";
    })
});