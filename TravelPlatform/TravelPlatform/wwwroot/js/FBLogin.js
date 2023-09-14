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