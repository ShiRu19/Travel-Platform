$(function () {
    // Native
    $("#singIn-btn").on("click", function () {
        var email = $("#native-login-email").val();
        var password = $("#native-login-password").val();

        if (email === '' || password === '') {
            toastr.warning('�b���K�X�ݶ�g����', 'ĵ�i');
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
                '�n�J���\',
                '���\',
                {
                    timeOut: 2000,
                    fadeOut: 2000,
                    onHidden: function () {
                        localStorage.setItem("access_token", response.data.accessToken);
                        window.location.href = "/index.html";
                    }
                }
            );
        })
        .catch((error) => {
            console.log(error);
            console.log(error.response.data.error + " " + error.response.data.message);
            toastr.error('��p...�o�ͤF�@�ǿ��~�A�ЦA�դ@���I', '���~');
        });
}

function UserLogin_Native(email, password) {
    var user = new Object();
    user.provider = "native";
    user.email = email;
    user.password = password;
    return PostUserSignIn(user);
}