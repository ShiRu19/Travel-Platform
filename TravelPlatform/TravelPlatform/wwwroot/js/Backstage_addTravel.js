$(function () {
    $("form").submit(function (e) {
        var formData = createTravelFormData();
        console.log(formData);
        postTravel(formData);
    });

    //Initialize Select2 Elements
    $('.select2').select2();

    //Initialize Select2 Elements
    $('.select2bs4').select2({
        theme: 'bootstrap4'
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

    var attractionNum = 2;
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

    var sessionNum = 2;
    $("#add-new-session-btn").on("click", function () {
        var sessionItem = `
                    <div class="card-session" data-sessionNum="${sessionNum}">
                            <div class="card card-primary">
                                <div class="card-header">
                                    <h3 class="card-title">行程場次-${sessionNum}</h3>

                                    <div class="card-tools">
                                        <button type="button" class="btn btn-tool" data-card-widget="collapse" title="Collapse">
                                            <i class="fas fa-minus"></i>
                                        </button>
                                    </div>
                                </div>
                                <div class="card-body">
                                    <div class="card-row">
                                        <!-- Product Number -->
                                        <div class="form-group card-column-1">
                                            <label>場次編號:</label>
                                            <div class="input-group" data-target-input="nearest">
                                                <input class="form-control" id="product-number-${sessionNum}" type="text" placeholder="輸入行程編號">
                                            </div>
                                        </div>
                                    </div>
                                    <div class="card-row">
                                        <!-- Date -->
                                        <div class="form-group card-column-1">
                                            <label>出發日期:</label>
                                            <div class="input-group date" data-target-input="nearest">
                                                <input type="text" id="departure-date-${sessionNum}" class="form-control datetimepicker-input" data-target="#departure-date-${sessionNum}" />
                                                <div class="input-group-append" data-target="#departure-date-${sessionNum}" data-toggle="datetimepicker">
                                                    <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="card-row">
                                        <!-- Price -->
                                        <div class="form-group card-column-1">
                                            <label>價錢:</label>
                                            <div class="input-group" data-target-input="nearest">
                                                <input class="form-control" id="price-${sessionNum}" type="text" placeholder="輸入成人價錢">
                                            </div>
                                        </div>
                                    </div>
                                    <div class="card-row">
                                        <!-- Applicants -->
                                        <div class="form-group card-column-1">
                                            <label>已報名人數:</label>
                                            <div class="input-group" data-target-input="nearest">
                                                <input class="form-control" id="applicants-${sessionNum}" type="text" placeholder="輸入目前報名人數">
                                            </div>
                                        </div>
                                        <!-- Seats -->
                                        <div class="form-group card-column-2">
                                            <label>席次:</label>
                                            <div class="input-group" data-target-input="nearest">
                                                <input class="form-control" id="seats-${sessionNum}" type="text" placeholder="輸入總席次">
                                            </div>
                                        </div>
                                    </div>
                                    <div class="card-row">
                                        <!-- Bootstrap Switch -->
                                        <div class="form-group">
                                            <label>出團狀態:</label>
                                            <div style="display: flex;">
                                                <div>
                                                    <input type="radio" id="radio-status-success-${sessionNum}" name="status-${sessionNum}" value="1" checked />
                                                    <label for="radio-status-success-${sessionNum}">已成團</label>
                                                </div>

                                                <div class="card-column-2">
                                                    <input type="radio" id="radio-status-unsuccess-${sessionNum}" name="status-${sessionNum}" value="0" />
                                                    <label for="radio-status-success-${sessionNum}">尚未成團</label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>`;
        $(".card-sessions").append(sessionItem);
        $(`#departure-date-${sessionNum}`).datetimepicker({
            format: 'L'
        });
        sessionNum += 1;
    });
});

function createTravelFormData() {
    var formData = new FormData();

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

    // TravelSession
    var s = Array.from($(".card-session"))

    s.forEach((session) => {
        var num = session.dataset.sessionnum;

        formData.append(`TravelSession[${num-1}].ProductNumber`, $(`#product-number-${num}`).val());
        formData.append(`TravelSession[${num-1}].Price`, $(`#price-${num}`).val());
        formData.append(`TravelSession[${num-1}].DepartureDate`, $(`#departure-date-${num}`).val());
        formData.append(`TravelSession[${num-1}].Applicants`, $(`#applicants-${num}`).val());
        formData.append(`TravelSession[${num-1}].Seats`, $(`#seats-${num}`).val());
        formData.append(`TravelSession[${num-1}].GroupStatus`, $(`input[name=status-${num}]:checked`, '#myForm').val());
    });

    return formData;
}

async function postTravel(formData) {
    await axios.post('/api/v1.0/BackstageTravel/AddTravel', formData)
        .then((response) => {
            toastr.success(
                '行程新增成功',
                '成功',
                {
                    timeOut: 2000,
                    fadeOut: 2000,
                    onHidden: function () {
                        window.location.href = "/admin/Backstage_TravelList.html";
                    }
                }
            );
        })
        .catch((error) => {
            console.log(error);
            toastr.error('抱歉...發生了一些錯誤，請再試一次！', '錯誤');
        });
}