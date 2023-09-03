$(function () {
    CheckLoginToShowName();
    GetTravelList();
});

async function GetTravelList() {
    return await axios.get("/api/v1.0/ForestageTravel/GetTravelList")
        .then((response) => {
            var travels = response.data.data;
            travels.forEach((travel) => {
                var item = `<a class="product" href="TravelDetail.html?id=${travel.id}">
                                <img class="main-images" src="${travel.mainImageUrl}" />
                                <h5 class="product-title">${travel.title}</h5>    
                                <span class="product-date-range">${travel.dateRangeStart} ~ ${travel.dateRangeEnd}</span>
                                <div class="travel-detail-btn-row">
                                    <div class="travel-detail-btn-container">
                                        <div class="travel-detail-btn">
                                            <div>ÀËµø¦æµ{</div>
                                        </div>
                                        <div class="travel-detail-btn-icon">></div>
                                    </div>
                                </div>
                            </a>`
                $(".products").append(item);
            })
        })
        .catch((error) => console.log(error));
}

function CheckLogin() {
    var accessToken = localStorage.getItem("access_token");
    if (accessToken === null) {
        $("#profile-login-unsuccess").show();
        $("#profile-login-success").hide();
        return;
    }

    let config = {
        headers: {
            Authorization: `Bearer ${accessToken}`,
        }
    }

    axios.get("/api/v1.0/user/profile", config)
        .then((response) => {
            $("#profile-name").html(response.data.name);
            $("#profile-login-unsuccess").hide();
            $("#profile-login-success").show();
            id = response.data.id;
        })
        .catch((error) => {
            console.log(error);
            localStorage.removeItem("access_token");
            $("#profile-login-unsuccess").show();
            $("#profile-login-success").hide();
        });
}