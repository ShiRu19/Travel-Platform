$(function () {
    // Native
    $("#singIn-btn").on("click", function () {
        var email = $("#native-login-email").val();
        var password = $("#native-login-password").val();

        if (email === '' || password === '') {
            alert("帳號密碼需填寫完整");
            return;
        }

        var res = UserLogin_Native(email, password);
    });
})

async function PostUserSignIn(data) {
    data = JSON.stringify(data);
    await axios.post('/api/v1.0/User/SignIn', JSON.parse(data))
        .then((response) => {
            alert("登入成功");
            console.log(response.data);
            localStorage.setItem("access_token", response.data.accessToken);
            window.location.href = "/index.html";
        })
        .catch((error) => {
            console.log(error);
            console.log(error.response.data.error + " " + error.response.data.message);
            alert(error.response.data.error + " " + error.response.data.message);
        });
}

function UserLogin_Native(email, password) {
    var user = new Object();
    user.provider = "native";
    user.email = email;
    user.password = password;
    return PostUserSignIn(user);
}