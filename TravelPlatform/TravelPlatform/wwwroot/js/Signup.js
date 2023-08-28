$(function () {
    $("#register").on("click", function () {
        var fullName = $("#fullName").val();
        var email = $("#email").val();
        var password = $("#password").val();
        var password_retype = $("#password_retype").val();
        var agreeTerms = document.querySelectorAll('input[type=checkbox]:checked').length == 1;

        if (fullName === '' || email === '' || password === '' || password_retype === '') {
            alert("��ƽж�g����");
            return;
        }
        if (!agreeTerms) {
            alert("�ЦP�N�����|��");
            return;
        }
        if (password_retype !== password) {
            alert("�K�X���ҿ��~�A�нT�{�⦸�K�X�O�_�ۦP");
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
    // TODO: �s�����U API
}