let config = {
    headers: {
        Authorization: `Bearer ${localStorage.getItem("access_token")}`,
    }
}

$(function () {
    axios.get("/api/v1.0/User/CheckAdminRole", config)
        .catch((error) => {
            ShowErrorMessage(error);

            if (error.response.status === 401) {
                window.location.href = "/pages/401.html";
            }
            else if (error.response.status === 403) {
                window.location.href = "/pages/403.html";
            }
            else {
                window.location.href = "/pages/404.html";
            }
        });
});