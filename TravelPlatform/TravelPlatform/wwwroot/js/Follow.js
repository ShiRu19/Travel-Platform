$(function () {
    CheckLoginRequired().then((profile) => {
        axios.get(`/api/v1.0/Record/GetUserFollowList?userId=${profile.id}`, config)
            .then((response) => {
                var openTravels = response.data.data.open;
                var closeTravels = response.data.data.close;

                var travels = "";

                openTravels.forEach((openTravel) => {
                    // 日期區間_起始
                    var dateRangeStart_utcDate = new Date(openTravel.dateRangeStart + "Z");
                    var dateRangeStart_year = dateRangeStart_utcDate.getFullYear();
                    var dateRangeStart_month = ("0" + (dateRangeStart_utcDate.getMonth() + 1)).slice(-2);
                    var dateRangeStart_date = ("0" + dateRangeStart_utcDate.getDate()).slice(-2);
                    var dateRangeStart = `${dateRangeStart_year}/${dateRangeStart_month}/${dateRangeStart_date}`;

                    // 日期區間_起始
                    var dateRangeEnd_utcDate = new Date(openTravel.dateRangeEnd + "Z");
                    var dateRangeEnd_year = dateRangeEnd_utcDate.getFullYear();
                    var dateRangeEnd_month = ("0" + (dateRangeEnd_utcDate.getMonth() + 1)).slice(-2);
                    var dateRangeEnd_date = ("0" + dateRangeEnd_utcDate.getDate()).slice(-2);
                    var dateRangeEnd = `${dateRangeEnd_year}/${dateRangeEnd_month}/${dateRangeEnd_date}`;

                    var item_open = `<a class="product" href="TravelDetail.html?id=${openTravel.id}">
                                    <img class="main-images" src="${openTravel.mainImageUrl}" />
                                    <h5 class="product-title">${openTravel.title}</h5>    
                                    <span class="product-date-range">${dateRangeStart} ~ ${dateRangeEnd}</span>
                                    <div class="travel-detail-btn-row">
                                        <div class="travel-detail-btn-container">
                                            <div class="travel-detail-btn">
                                                <div>檢視行程</div>
                                            </div>
                                            <div class="travel-detail-btn-icon">></div>
                                        </div>
                                    </div>
                                </a>`;

                    travels += item_open;
                })

                closeTravels.forEach((closeTravel) => {
                    // 日期區間_起始
                    var dateRangeStart_utcDate = new Date(closeTravel.dateRangeStart + "Z");
                    var dateRangeStart_year = dateRangeStart_utcDate.getFullYear();
                    var dateRangeStart_month = ("0" + (dateRangeStart_utcDate.getMonth() + 1)).slice(-2);
                    var dateRangeStart_date = ("0" + dateRangeStart_utcDate.getDate()).slice(-2);
                    var dateRangeStart = `${dateRangeStart_year}/${dateRangeStart_month}/${dateRangeStart_date}`;

                    // 日期區間_起始
                    var dateRangeEnd_utcDate = new Date(closeTravel.dateRangeEnd + "Z");
                    var dateRangeEnd_year = dateRangeEnd_utcDate.getFullYear();
                    var dateRangeEnd_month = ("0" + (dateRangeEnd_utcDate.getMonth() + 1)).slice(-2);
                    var dateRangeEnd_date = ("0" + dateRangeEnd_utcDate.getDate()).slice(-2);
                    var dateRangeEnd = `${dateRangeEnd_year}/${dateRangeEnd_month}/${dateRangeEnd_date}`;

                    var item_close = `<div class="product product-close">
                                        <img class="main-images" src="${closeTravel.mainImageUrl}" />
                                        <h5 class="product-title">${closeTravel.title}</h5>    
                                        <span class="product-date-range">${dateRangeStart} ~ ${dateRangeEnd}</span>
                                        <div class="travel-detail-btn-row">
                                            <div class="travel-detail-btn-container">
                                                <div class="travel-detail-btn">
                                                    <div>檢視行程</div>
                                                </div>
                                                <div class="travel-detail-btn-icon">></div>
                                            </div>
                                        </div>
                                    </div>`;

                    travels += item_close;
                })

                $(".products").append(travels);
                $("#loading").hide();
            })
            .catch((error) => {
                console.log(error);
                toastr.error('抱歉...發生了一些錯誤，請再試一次！', '錯誤');
            })
    });
});