$(function () {
    CheckLoginToShowName();
    GetTravelList();
});

async function GetTravelList() {
    return await axios.get("/api/v1.0/ForestageTravel/GetTravelList")
        .then((response) => {
            var travels = response.data.data;
            travels.forEach((travel) => {
                // 日期區間_起始
                var dateRangeStart_utcDate = new Date(travel.dateRangeStart + "Z");
                var dateRangeStart_year = dateRangeStart_utcDate.getFullYear();
                var dateRangeStart_month = ("0" + (dateRangeStart_utcDate.getMonth() + 1)).slice(-2);
                var dateRangeStart_date = ("0" + dateRangeStart_utcDate.getDate()).slice(-2);
                var dateRangeStart = `${dateRangeStart_year}/${dateRangeStart_month}/${dateRangeStart_date}`;

                // 日期區間_起始
                var dateRangeEnd_utcDate = new Date(travel.dateRangeEnd + "Z");
                var dateRangeEnd_year = dateRangeEnd_utcDate.getFullYear();
                var dateRangeEnd_month = ("0" + (dateRangeEnd_utcDate.getMonth() + 1)).slice(-2);
                var dateRangeEnd_date = ("0" + dateRangeEnd_utcDate.getDate()).slice(-2);
                var dateRangeEnd = `${dateRangeEnd_year}/${dateRangeEnd_month}/${dateRangeEnd_date}`;

                var item = `<a class="product" href="TravelDetail.html?id=${travel.id}">
                                <img class="main-images" src="${travel.mainImageUrl}" />
                                <h5 class="product-title">${travel.title}</h5>    
                                <span class="product-date-range">${dateRangeStart} ~ ${dateRangeEnd}</span>
                                <div class="travel-detail-btn-row">
                                    <div class="travel-detail-btn-container">
                                        <div class="travel-detail-btn">
                                            <div>檢視行程</div>
                                        </div>
                                        <div class="travel-detail-btn-icon">></div>
                                    </div>
                                </div>
                            </a>`
                $(".products").append(item);
                $("#loading").hide();
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