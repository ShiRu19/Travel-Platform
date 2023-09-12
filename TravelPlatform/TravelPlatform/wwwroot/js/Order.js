var remainingSeats = 100;
var price = 0;
$(function () {
    const urlParams = new URLSearchParams(window.location.search);
    productNumber = urlParams.get('productNumber');

    CheckLoginRequired().then(function (profile) {
        GetSessionDetail(productNumber);
    });

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
        window.location.href = `/Order_applicantInfo.html?qty=${qty}&productNumber=${productNumber}&nation=�x�W`;
    });
});

async function GetSessionDetail(productNumber) {
    await axios.get(`/api/v1.0/ForestageTravel/GetSessionDetail?productNumber=${productNumber}`)
        .then((response) => {
            var data = response.data;

            /* ==================
             *   Date range
             * ==================
             */
            var day_list = ['��', '�@', '�G', '�T', '�|', '��', '��'];

            // ����϶�_�_�l
            var dateRangeStart_utcDate = new Date(data.departure_date_start + "Z");
            var dateRangeStart_year = dateRangeStart_utcDate.getFullYear();
            var dateRangeStart_month = ("0" + (dateRangeStart_utcDate.getMonth() + 1)).slice(-2);
            var dateRangeStart_date = ("0" + dateRangeStart_utcDate.getDate()).slice(-2);
            var dateRangeStart_day = dateRangeStart_utcDate.getDay();
            var dateRangeStart = `${dateRangeStart_year}/${dateRangeStart_month}/${dateRangeStart_date}(${day_list[dateRangeStart_day]})`;

            // ����϶�_�_�l
            var dateRangeEnd_utcDate = new Date(data.departure_date_end + "Z");
            var dateRangeEnd_year = dateRangeEnd_utcDate.getFullYear();
            var dateRangeEnd_month = ("0" + (dateRangeEnd_utcDate.getMonth() + 1)).slice(-2);
            var dateRangeEnd_date = ("0" + dateRangeEnd_utcDate.getDate()).slice(-2);
            var dateRangeEnd_day = dateRangeEnd_utcDate.getDay();
            var dateRangeEnd = `${dateRangeEnd_year}/${dateRangeEnd_month}/${dateRangeEnd_date}(${day_list[dateRangeEnd_day]})`;

            /* ===================
             *   Remaining seats
             * ===================
             */
            remainingSeats = data.remaining_seats;

            /* =========
             *   Price 
             * =========
             */
            price = data.price;

            let total = (data.price).toLocaleString('zh-tw', {
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
            $("#total-number").html(`${total}`);
        })
        .catch((error) => {
            if (error.response.status === 404) {
                location.replace("/pages/404.html");
            }
            else {
                ShowErrorMessage(error);
                toastr.error('��p...���o������T�ɵo�ͤF�@�ǿ��~�A�ЦA�դ@���I', '���~');
            }
        })
}