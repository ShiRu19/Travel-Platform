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
            var data = response.data;
            $("#travel-title").html(data.title);
            $("#departure-date-start").html(data.departure_date_start);
            $("#departure-date-end").html(data.departure_date_end);
            $("#departure-days").html(data.days);
            $("#product-number").html(data.product_number);
            $("#product-qty").html(qty);

            let total = (data.price * qty).toLocaleString('zh-tw', {
                style: 'currency',
                currency: 'TWD',
                minimumFractionDigits: 0
            });
            $("#product-total").html(`${total}`);
        })
        .catch((error) => {
            console.log(error);
            alert("抱歉...發生了一些錯誤，請再試一次！");
        })
}

async function UpdateAccountDigits() {
    var formData = new FormData();

    formData.append("OrderId", orderId);
    formData.append("UserId", profile.id);
    formData.append("AccountDigits", $("#account-five-digits").val());

    await axios.post('/api/v1.0/Order/UpdateOrderPayStatus', formData, config)
        .then((response) => {
            var orderId = response.data.id;
            window.location.href = `/Order_Finish.html?id=${orderId}`;
        })
        .catch((error) => {
            console.log(error);
            alert("抱歉...發生了一些錯誤，請再試一次！");
        });
}