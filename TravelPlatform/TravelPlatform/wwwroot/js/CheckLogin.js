function CheckLogin() {
    var accessToken = localStorage.getItem("access_token");
    if (accessToken === null) {
        $("#profile-login-unsuccess").show();
        $("#profile-login-success").hide();
        return 0;
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
            return response.data.id;
        })
        .catch((error) => {
            console.log(error);
            localStorage.removeItem("access_token");
            $("#profile-login-unsuccess").show();
            $("#profile-login-success").hide();
            return 0;
        });
}