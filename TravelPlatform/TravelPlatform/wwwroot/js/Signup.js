$(function () {
    $("#register").on("click", function () {
        var fullName = $("#fullName").val();
        var email = $("#email").val();
        var password = $("#password").val();
        var password_retype = $("#password_retype").val();
        var agreeTerms = document.querySelectorAll('input[type=checkbox]:checked').length == 1;

        if (fullName === '' || email === '' || password === '' || password_retype === '') {
            alert("資料請填寫完整");
            return;
        }
        if (!agreeTerms) {
            alert("請同意成為會員");
            return;
        }
        if (password_retype !== password) {
            alert("密碼驗證錯誤，請確認兩次密碼是否相同");
            return;
        }

        var user = new Object();
        user.fullName = fullName;
        user.email = email;
        user.password = password;

        signup(user);
    });
});

async function signup(user) {
    // TODO: 連接註冊 API
}