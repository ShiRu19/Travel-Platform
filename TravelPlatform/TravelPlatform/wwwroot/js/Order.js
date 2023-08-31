var remainingSeats = 100;
var price = 0;
$(function () {
    const urlParams = new URLSearchParams(window.location.search);
    productNumber = urlParams.get('productNumber');

    GetSessionDetail(productNumber);

    $(".fa-plus-circle").on("click", function () {
        let num = Math.min(Number($("#quantity_num").html()) + 1, remainingSeats);
        $("#quantity_num").html(num);
        let total = (num * price).toLocaleString('zh-tw', {
            style: 'currency',
            currency: 'TWD',
            minimumFractionDigits: 0
        });
        $("#total-number").html(total);
    });

    $(".fa-minus-circle").on("click", function () {
        let num = Math.max(Number($("#quantity_num").html()) - 1, 0);
        $("#quantity_num").html(num);
        let total = (num * price).toLocaleString('zh-tw', {
            style: 'currency',
            currency: 'TWD',
            minimumFractionDigits: 0
        }); 
        $("#total-number").html(total);
    });

});

async function GetSessionDetail(productNumber) {
    await axios.get(`/api/v1.0/ForestageTravel/GetSessionDetail?productNumber=${productNumber}`)
        .then((response) => {
            console.log(response.data);
            var data = response.data;
            $("#travel-title").html(data.title);
            $("#departure-date-start").html(data.departure_date_start);
            $("#departure-date-end").html(data.departure_date_end);
            $("#departure-days").html(data.days);
            $("#product-number").html(data.product_number);

            remainingSeats = data.remaining_seats;
            price = data.price;
        })
        .catch((error) => {
            console.log(error);
        })
}