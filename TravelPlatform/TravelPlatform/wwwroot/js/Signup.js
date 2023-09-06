$(function () {
    $("#register").on("click", function () {
        var fullName = $("#fullName").val();
        var email = $("#email").val();
        var password = $("#password").val();
        var password_retype = $("#password_retype").val();
        var agreeTerms = document.querySelectorAll('input[type=checkbox]:checked').length == 1;

        if (fullName === '' || email === '' || password === '' || password_retype === '') {
            toastr.warning('資料請填寫完整', '警告');
            return;
        }
        if (!agreeTerms) {
            toastr.info('請同意成為會員', '提醒');
            return;
        }
        if (password_retype !== password) {
            toastr.error('密碼驗證錯誤，請確認兩次密碼是否相同', '警告');
            return;
        }

        var user = new Object();
        user.Name = fullName;
        user.Email = email;
        user.Password = password;
        user.Role = 'user';

        signup(user);
    });
});

async function signup(user) {
    await axios.post("/api/v1.0/User/SignUp", user)
        .then((response) => {
            toastr.success(
                '註冊成功',
                '成功',
                {
                    timeOut: 2000,
                    fadeOut: 2000,
                    onHidden: function () {
                        localStorage.setItem("access_token", response.data.accessToken);localStorage.setItem("access_token", response.data.accessToken);
                        window.location.href = "/index.html";
                    }
                }
            );
        })
        .catch((error) => {
            console.log(error);
            console.log(error.response.data.error + " " + error.response.data.message);
            toastr.error('抱歉...發生了一些錯誤，請再試一次！', '錯誤');
        });
}