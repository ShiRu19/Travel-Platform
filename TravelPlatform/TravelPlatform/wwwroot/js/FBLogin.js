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
});

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
                        localStorage.setItem("access_token", response.data.accessToken);
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

function FBLogin() {
    FB.getLoginStatus(function (res) {
        if (res.status !== "connected") {
            if (res.status !== "not_authorized" && res.status !== "unknown")
                return false;

            FB.login(function (res) {
                if (res.status !== "connected") {
                    console.log(res.status);
                    toastr.error("Facebook 帳號無法登入", '錯誤');
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
        user.Access_token_fb = res.authResponse.accessToken;
        return PostUserSignIn(user);
    });

    return false;
}