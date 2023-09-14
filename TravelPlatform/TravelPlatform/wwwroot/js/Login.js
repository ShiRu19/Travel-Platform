$(function () {
    // Native
    $("#singIn-btn").on("click", function () {
        var email = $("#native-login-email").val();
        var password = $("#native-login-password").val();

        if (email === '' || password === '') {
            toastr.warning('帳號密碼需填寫完整', '警告');
            return;
        }

        var res = UserLogin_Native(email, password);
    });
})

async function PostUserSignIn(data) {
    data = JSON.stringify(data);
    await axios.post('/api/v1.0/User/SignIn', JSON.parse(data))
        .then((response) => {
            toastr.success(
                '登入成功',
                '成功',
                {
                    timeOut: 2000,
                    fadeOut: 2000,
                    onHidden: function () {
                        localStorage.setItem("access_token", response.data.data.accessToken);
                        window.location.href = "/index.html";
                    }
                }
            );
        })
        .catch((error) => {
            ShowErrorMessage(error);
            if (error.response.status === 400) {
                if (error.response.data.error === "Data not found") {
                    toastr.info('此帳號尚未註冊，請先進行註冊！', '警告');
                }
                else {
                    toastr.error('抱歉...帳號或密碼錯誤，請再試一次！', '錯誤');
                }
            }
            else toastr.error('抱歉...登入時發生了一些錯誤，請再試一次！', '錯誤');
        });
}

function UserLogin_Native(email, password) {
    var user = new Object();
    user.provider = "native";
    user.email = email;
    user.password = password;
    return PostUserSignIn(user);
}