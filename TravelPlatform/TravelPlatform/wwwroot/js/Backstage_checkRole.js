let config = {
    headers: {
        Authorization: `Bearer ${localStorage.getItem("access_token")}`,
    }
}

$(function () {
    axios.get("/api/v1.0/User/CheckAdminRole", config)
        .catch((error) => {
            console.log(error);
            if (error.response.status === 401 || error.response.status === 403) {
                window.location.href = "/pages/404.html";
            }
        });
});