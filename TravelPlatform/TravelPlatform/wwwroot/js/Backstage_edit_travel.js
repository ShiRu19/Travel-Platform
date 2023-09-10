var attractionNum = 2;
var id = 0;

$(function () {
    const urlParams = new URLSearchParams(window.location.search);
    id = urlParams.get('id');
    GetTravelInfo(id);

    //Initialize Select2 Elements
    $('.select2').select2();

    //Initialize Select2 Elements
    $('.select2bs4').select2({
        theme: 'bootstrap4'
    });

    $("form").submit(function (e) {
        var formData = createTravelFormData();
        postTravel(formData);
    });

    //Datemask dd/mm/yyyy
    $('#datemask').inputmask('dd/mm/yyyy', { 'placeholder': 'dd/mm/yyyy' });
    //Datemask2 mm/dd/yyyy
    $('#datemask2').inputmask('mm/dd/yyyy', { 'placeholder': 'mm/dd/yyyy' });
    //Money Euro
    $('[data-mask]').inputmask();

    //Date range picker
    $('#reservation').daterangepicker();

    //Date picker
    $('#departure-date-1').datetimepicker({
        format: 'L'
    });

    $("#add-new-attraction-btn").on("click", function () {
        var attractionItem = `
                    <div class="form-group card-column-1">
                        <div class="input-group" id="attraction-${attractionNum}" data-target-input="nearest">
                            <input class="form-control attractions" type="text" placeholder="景點 or 縣市">
                            <a class="btn btn-danger btn-sm delete-attraction" href="#"><i class="fas fa-trash"></i>刪除</a>
                        </div>
                    </div>`;

        $(".card-attractions").append(attractionItem);
        attractionNum += 1;
    });

    $(document).on('click', '.delete-attraction', function (e) {
        e.preventDefault();
        $(this).parent().parent().remove();
    });

    $("#change-pdf-btn").on('click', function () {
        $("#upload-new-pdf-container").show();
        $("#original-pdf-container").hide();
    });

    $("#change-main-image-btn").on('click', function () {
        $("#upload-new-main-image-container").show();
        $("#original-main-image-container").hide();
    })
});

var attractionNum = 1;

async function GetTravelInfo(id) {
    await axios.get("/api/v1.0/BackstageTravel/GetTravelInfo?id=" + id)
        .then((response) => {
            var travel = response.data.travel[0];
            var attractions = response.data.attractions;

            // 日期區間_起始
            var dateRangeStart_utcDate = new Date(travel.dateRangeStart + "Z");
            var dateRangeStart_year = dateRangeStart_utcDate.getFullYear();
            var dateRangeStart_month = ("0" + (dateRangeStart_utcDate.getMonth() + 1)).slice(-2);
            var dateRangeStart_date = ("0" + dateRangeStart_utcDate.getDate()).slice(-2);
            var dateRangeStart = `${dateRangeStart_month}/${dateRangeStart_date}/${dateRangeStart_year}`;

            // 日期區間_起始
            var dateRangeEnd_utcDate = new Date(travel.dateRangeEnd + "Z");
            var dateRangeEnd_year = dateRangeEnd_utcDate.getFullYear();
            var dateRangeEnd_month = ("0" + (dateRangeEnd_utcDate.getMonth() + 1)).slice(-2);
            var dateRangeEnd_date = ("0" + dateRangeEnd_utcDate.getDate()).slice(-2);
            var dateRangeEnd = `${dateRangeEnd_month}/${dateRangeEnd_date}/${dateRangeEnd_year}`;

            var dateRange = `${dateRangeStart} - ${dateRangeEnd}`;

            $("#title").val(travel.title);
            $("#reservation").val(dateRange);
            $("#days").val(travel.days);
            $("#departure_location").val(travel.departure_location);
            $("#nation").val(travel.nation);

            $("#pdf_url").attr('href', '/' + travel.pdf_url);
            $("#main_image_url").attr('href', '/' + travel.main_image_url);

            attractions.forEach((attraction) => {
                var attractionItem = `
                    <div class="form-group card-column-1">
                        <div class="input-group" id="attraction-${attractionNum}" data-target-input="nearest">
                            <input class="form-control attractions" type="text" placeholder="景點 or 縣市" value="${attraction.attraction}" />
                            <a class="btn btn-danger btn-sm delete-attraction" href="#"><i class="fas fa-trash"></i>刪除</a>
                        </div>
                    </div>`;

                $(".card-attractions").append(attractionItem);
                attractionNum += 1;
            });
        })
        .catch((error) => {
            console.log(error);
            toastr.error('抱歉...發生了一些錯誤，請再試一次！', '錯誤');
        });
}

function createTravelFormData() {
    var formData = new FormData();

    formData.append("Id", id);

    // TravelInfo
    formData.append("TravelInfo.Title", $("#title").val());

    var dateRange = $("#reservation").val().split(" - ");
    formData.append("TravelInfo.DateRangeStart", dateRange[0]);
    formData.append("TravelInfo.DateRangeEnd", dateRange[1]);

    formData.append("TravelInfo.Days", $("#days").val());
    formData.append("TravelInfo.DepartureLocation", $("#departure_location").val());

    formData.append("TravelInfo.Nation", $("#nation").val());

    var main_image_file = $("#travel-main-image");
    if (main_image_file.val() !== '') {
        var file = main_image_file[0].files[0];
        formData.append('TravelInfo.MainImageFile', file);
    }

    var pdf_file = $("#travel-pdf");
    if (pdf_file.val() !== '') {
        var file = pdf_file[0].files[0];
        formData.append('TravelInfo.PdfFile', file);
    }

    // TravelAttraction
    const attractions = Array.from($(".attractions"));
    var i = 0;
    attractions.forEach((attraction) => {
        if (attraction.value !== '') {
            formData.append(`TravelAttraction[${i}]`, attraction.value);
            i++;
        }
    });

    return formData;
}

async function postTravel(formData) {
    await axios.post('/api/v1.0/BackstageTravel/EditTravel', formData)
        .then((response) => {
            toastr.success(
                '編輯成功',
                '成功',
                {
                    timeOut: 2000,
                    fadeOut: 2000,
                    onHidden: function () {
                        window.location.reload();
                    }
                }
            );
        })
        .catch((error) => {
            console.log(error);
            toastr.error('抱歉...發生了一些錯誤，請再試一次！', '錯誤');
        });
}