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
                            <input class="form-control attractions" type="text" placeholder="���I or ����">
                        </div>
                    </div>`;

        $(".card-attractions").append(attractionItem);
        attractionNum += 1;
    });


    var sessionNum = 2;
    $("#add-new-session-btn").on("click", function () {
        var sessionItem = `
                    <div class="card-session" data-sessionNum="${sessionNum}">
                            <div class="card card-primary">
                                <div class="card-header">
                                    <h3 class="card-title">��{����-${sessionNum}</h3>

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
                                            <label>�����s��:</label>
                                            <div class="input-group" data-target-input="nearest">
                                                <input class="form-control" id="product-number-${sessionNum}" type="text" placeholder="��J��{�s��">
                                            </div>
                                        </div>
                                    </div>
                                    <div class="card-row">
                                        <!-- Date -->
                                        <div class="form-group card-column-1">
                                            <label>�X�o���:</label>
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
                                            <label>����:</label>
                                            <div class="input-group" data-target-input="nearest">
                                                <input class="form-control" id="price-${sessionNum}" type="text" placeholder="��J���H����">
                                            </div>
                                        </div>
                                    </div>
                                    <div class="card-row">
                                        <!-- Applicants -->
                                        <div class="form-group card-column-1">
                                            <label>�w���W�H��:</label>
                                            <div class="input-group" data-target-input="nearest">
                                                <input class="form-control" id="applicants-${sessionNum}" type="text" placeholder="��J�ثe���W�H��">
                                            </div>
                                        </div>
                                        <!-- Seats -->
                                        <div class="form-group card-column-2">
                                            <label>�u��:</label>
                                            <div class="input-group" data-target-input="nearest">
                                                <input class="form-control" id="seats-${sessionNum}" type="text" placeholder="��J�`�u��">
                                            </div>
                                        </div>
                                    </div>
                                    <div class="card-row">
                                        <!-- Bootstrap Switch -->
                                        <div class="form-group">
                                            <label>�X�Ϊ��A:</label>
                                            <div style="display: flex;">
                                                <div>
                                                    <input type="radio" id="radio-status-success-${sessionNum}" name="status-${sessionNum}" value="1" checked />
                                                    <label for="radio-status-success-${sessionNum}">�w����</label>
                                                </div>

                                                <div class="card-column-2">
                                                    <input type="radio" id="radio-status-unseccess-${sessionNum}" name="status-${sessionNum}" value="0" />
                                                    <label for="radio-status-success-${sessionNum}">�|������</label>
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

    var main_image_file = $("#travel-main-image");
    if (main_image_file.length > 0) {
        var file = main_image_file[0].files[0];
        formData.append('TravelInfo.MainImageFile', file);
    }

    var pdf_file = $("#travel-pdf");
    if (pdf_file.length > 0) {
        var file = pdf_file[0].files[0];
        formData.append('TravelInfo.PdfFile', file);
    }

    // TravelAttraction
    const attractions = Array.from($(".attractions"));
    var i = 0;
    attractions.forEach((attraction) => {
        formData.append(`TravelAttraction[${i}]`, attraction.value);
        i++;
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
    await axios.post('/api/v1.0/TravelBackstage/AddTravel', formData)
        .then((response) => {
            if (response.status === 200) {
                alert("Create success");
                location.reload();
            }
            else {
                alert("Create error");
            }
        })
        .catch((error) => { alert(error) });
}