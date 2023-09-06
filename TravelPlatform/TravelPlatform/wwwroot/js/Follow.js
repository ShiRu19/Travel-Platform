$(function () {
    CheckLoginRequired().then((profile) => {
        axios.get(`/api/v1.0/Record/GetFollowList?userId=${profile.id}`, config)
            .then((response) => {
                var openTravels = response.data.open;
                var closeTravels = response.data.close;

                openTravels.forEach((openTravel) => {
                    var item_open = `<a class="product" href="TravelDetail.html?id=${openTravel.id}">
                                    <img class="main-images" src="${openTravel.mainImageUrl}" />
                                    <h5 class="product-title">${openTravel.title}</h5>    
                                    <span class="product-date-range">${openTravel.dateRangeStart} ~ ${openTravel.dateRangeEnd}</span>
                                    <div class="travel-detail-btn-row">
                                        <div class="travel-detail-btn-container">
                                            <div class="travel-detail-btn">
                                                <div>檢視行程</div>
                                            </div>
                                            <div class="travel-detail-btn-icon">></div>
                                        </div>
                                    </div>
                                </a>`;

                    $(".products").append(item_open);
                })

                closeTravels.forEach((closeTravel) => {
                    var item_close = `<div class="product product-close">
                                        <img class="main-images" src="${closeTravel.mainImageUrl}" />
                                        <h5 class="product-title">${closeTravel.title}</h5>    
                                        <span class="product-date-range">${closeTravel.dateRangeStart} ~ ${closeTravel.dateRangeEnd}</span>
                                        <div class="travel-detail-btn-row">
                                            <div class="travel-detail-btn-container">
                                                <div class="travel-detail-btn">
                                                    <div>檢視行程</div>
                                                </div>
                                                <div class="travel-detail-btn-icon">></div>
                                            </div>
                                        </div>
                                    </div>`;

                    $(".products").append(item_close);
                })
            })
            .catch((error) => {
                console.log(error);
                alert("抱歉...發生了一些錯誤，請再試一次！");
            })
    });
});