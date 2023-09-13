var productNumber = "";
var qty = 0;
var orderId = 0;
$(function () {
    const urlParams = new URLSearchParams(window.location.search);
    productNumber = urlParams.get('productNumber');
    qty = urlParams.get('qty');
    orderId = urlParams.get('orderId');

    CheckLoginRequired().then(function (profile) {
        GetSessionDetail(productNumber);
    });

    $("form").submit(function (e) {
        UpdateAccountDigits();
    });
});

async function GetSessionDetail(productNumber) {
    await axios.get(`/api/v1.0/ForestageTravel/GetSessionDetail?productNumber=${productNumber}`)
        .then((response) => {
            var data = response.data.data;

            /* ==================
             *   Date range
             * ==================
             */
            var day_list = ['日', '一', '二', '三', '四', '五', '六'];

            // 日期區間_起始
            var dateRangeStart_utcDate = new Date(data.departure_date_start + "Z");
            var dateRangeStart_year = dateRangeStart_utcDate.getFullYear();
            var dateRangeStart_month = ("0" + (dateRangeStart_utcDate.getMonth() + 1)).slice(-2);
            var dateRangeStart_date = ("0" + dateRangeStart_utcDate.getDate()).slice(-2);
            var dateRangeStart_day = dateRangeStart_utcDate.getDay();
            var dateRangeStart = `${dateRangeStart_year}/${dateRangeStart_month}/${dateRangeStart_date}(${day_list[dateRangeStart_day]})`;

            // 日期區間_起始
            var dateRangeEnd_utcDate = new Date(data.departure_date_end + "Z");
            var dateRangeEnd_year = dateRangeEnd_utcDate.getFullYear();
            var dateRangeEnd_month = ("0" + (dateRangeEnd_utcDate.getMonth() + 1)).slice(-2);
            var dateRangeEnd_date = ("0" + dateRangeEnd_utcDate.getDate()).slice(-2);
            var dateRangeEnd_day = dateRangeEnd_utcDate.getDay();
            var dateRangeEnd = `${dateRangeEnd_year}/${dateRangeEnd_month}/${dateRangeEnd_date}(${day_list[dateRangeEnd_day]})`;

            /* =========
             *   Total 
             * =========
             */
            let total = (data.price * qty).toLocaleString('zh-tw', {
                style: 'currency',
                currency: 'TWD',
                minimumFractionDigits: 0
            });

            /* ====================
             *  HTML travel detail
             * ====================
             */
            $("#travel-title").html(data.title);
            $("#departure-date-start").html(dateRangeStart);
            $("#departure-date-end").html(dateRangeEnd);
            $("#departure-days").html(data.days);
            $("#product-number").html(data.product_number);
            $("#product-qty").html(qty);
            $("#product-total").html(`${total}`);
        })
        .catch((error) => {
            if (error.response.status === 404) {
                location.replace("/pages/404.html");
            }
            else {
                ShowErrorMessage(error);
                toastr.error('抱歉...取得場次資訊時發生了一些錯誤，請再試一次！', '錯誤');
            }
        })
}

async function UpdateAccountDigits() {
    var formData = new FormData();

    formData.append("OrderId", orderId);
    formData.append("UserId", profile.id);
    formData.append("AccountDigits", $("#account-five-digits").val());

    await axios.post('/api/v1.0/Order/UpdateOrderPayStatus', formData, config)
        .then((response) => {
            var orderId = response.data.data;
            window.location.replace(`/Order_Finish.html?id=${orderId}`);
        })
        .catch((error) => {
            ShowErrorMessage(error);
            toastr.error('抱歉...更新付款狀態時發生了一些錯誤，請再試一次！', '錯誤');
        });
}