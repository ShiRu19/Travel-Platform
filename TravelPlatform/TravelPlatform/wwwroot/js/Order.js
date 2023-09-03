var remainingSeats = 100;
var price = 0;
$(function () {
    const urlParams = new URLSearchParams(window.location.search);
    productNumber = urlParams.get('productNumber');

    CheckLoginRequired();
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
        let num = Math.max(Number($("#quantity_num").html()) - 1, 1);
        $("#quantity_num").html(num);
        let total = (num * price).toLocaleString('zh-tw', {
            style: 'currency',
            currency: 'TWD',
            minimumFractionDigits: 0
        }); 
        $("#total-number").html(total);
    });

    $("#next-btn").on("click", function () {
        let qty = $("#quantity_num").html();
        window.location.href = `/Order_applicantInfo.html?qty=${qty}&productNumber=${productNumber}&nation=¥xÆW`;
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

            remainingSeats = data.remaining_seats;
            price = data.price;

            let total = (data.price).toLocaleString('zh-tw', {
                style: 'currency',
                currency: 'TWD',
                minimumFractionDigits: 0
            });
            $("#total-number").html(`${total}`);
        })
        .catch((error) => {
            console.log(error);
        })
}