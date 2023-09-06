$(function () {
    $("#register").on("click", function () {
        var fullName = $("#fullName").val();
        var email = $("#email").val();
        var password = $("#password").val();
        var password_retype = $("#password_retype").val();
        var agreeTerms = document.querySelectorAll('input[type=checkbox]:checked').length == 1;

        if (fullName === '' || email === '' || password === '' || password_retype === '') {
            toastr.warning('��ƽж�g����', 'ĵ�i');
            return;
        }
        if (!agreeTerms) {
            toastr.info('�ЦP�N�����|��', '����');
            return;
        }
        if (password_retype !== password) {
            toastr.error('�K�X���ҿ��~�A�нT�{�⦸�K�X�O�_�ۦP', 'ĵ�i');
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
                '���U���\',
                '���\',
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
            toastr.error('��p...�o�ͤF�@�ǿ��~�A�ЦA�դ@���I', '���~');
        });
}