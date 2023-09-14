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
                    toastr.info('���b���|�����U�A�Х��i����U�I', 'ĵ�i');
                }
                else {
                    toastr.error('��p...�b���αK�X���~�A�ЦA�դ@���I', '���~');
                }
            }
            else toastr.error('��p...�n�J�ɵo�ͤF�@�ǿ��~�A�ЦA�դ@���I', '���~');
        });
}

function UserLogin_Native(email, password) {
    var user = new Object();
    user.provider = "native";
    user.email = email;
    user.password = password;
    return PostUserSignIn(user);
}