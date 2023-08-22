$(function () {
    GetTravelList();
});

async function GetTravelList() {
    return await axios.get("/api/v1.0/ForestageTravel/GetTravelList")
        .then((response) => {
            console.log(response.data.data);

            var travels = response.data.data;
            travels.forEach((travel) => {
                var item = `<a class="product" href="index.html">
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