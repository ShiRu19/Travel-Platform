$(function () {
    // Facebook
    let FB_appID = "716019756949740";

    (function (d, s, id) {
        var js, fjs = d.getElementsByTagName(s)[0];
        if (d.getElementById(id)) return;
        js = d.createElement(s); js.id = id;
        js.src = "https://connect.facebook.net/en_US/sdk.js";
        fjs.parentNode.insertBefore(js, fjs);
    }(document, 'script', 'facebook-jssdk'));

    window.fbAsyncInit = function () {
        FB.init({
            appId: FB_appID, // FB appID
            cookie: true,  // enable cookies to allow the server to access the session
            xfbml: true,  // parse social plugins on this page
            version: 'v17.0' // use graph api version
        });
        FB.AppEvents.logPageView();
    };

    // Native
    $("#singIn-btn").on("click", function () {
        var email = $("#native-login-email").val();
        var password = $("#native-login-password").val();

        if (email === '' || password === '') {
            alert("帳號密碼需填寫完整");
            return;
        }

        var res = UserLogin_Native(email, password);
        if (res) {
            alert("登入成功！");
            window.location.href = "/index.html";
        }
        else alert("登入失敗...")
    });
});

async function PostUserSignIn(data) {
    data = JSON.stringify(data);
    // TODO: POST to sign in API
}

function FBLogin() {
    FB.getLoginStatus(function (res) {
        if (res.status !== "connected") {
            if (res.status !== "not_authorized" && res.status !== "unknown")
                return false;

            FB.login(function (res) {
                if (res.status !== "connected") {
                    console.log(res.status);
                    alert("Facebook 帳號無法登入");
                    return false;
                }
            })
        }

        return UserLogin_FB(res);
    });
}

function UserLogin_FB(res) {
    FB.api('/me?fields=id,name,email,picture{url}', function (response) {
        var user = new Object();
        user.provider = "facebook";
        user.access_token = res.authResponse.accessToken;
        return PostUserSignIn(user);
    });

    return false;
}

function UserLogin_Native(email, password) {
    var user = new Object();
    user.provider = "native";
    user.email = email;
    user.password = password;
    return PostUserSignIn(user);
}