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
        user.Name = fullName;
        user.Email = email;
        user.Password = password;

        signup(user);
    });
});

async function signup(user) {
    await axios.post("/api/v1.0/User/SignUp", user)
        .then((response) => {
            alert("註冊成功");
            window.location.href = "/index.html";
        })
        .catch((error) => {
            console.log(error);
            console.log(error.response.data.error + " " + error.response.data.message);
            alert(error.response.data.error + " " + error.response.data.message);
        });
}