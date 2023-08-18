$(function () {
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
    $('#departure_date').datetimepicker({
        format: 'L'
    });

    $("input[data-bootstrap-switch]").each(function () {
        $(this).bootstrapSwitch('state', $(this).prop('checked'));
    })

    var attractionNum = 2;
    $("#add-new-attraction-btn").on("click", function () {
        var attractionItem = `
                    <div class="form-group card-column-1">
                        <div class="input-group" id="attraction-${attractionNum}" data-target-input="nearest">
                            <input class="form-control" type="text" placeholder="景點 or 縣市">
                        </div>
                    </div>`;

        $(".card-attractions").append(attractionItem);
        attractionNum += 1;
    });


    var sessionNum = 2;
    $("#add-new-session-btn").on("click", function () {
        var sessionItem = `
                    <div class="card-session">
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
                                        <div class="input-group" id="product-number-${sessionNum}" data-target-input="nearest">
                                            <input class="form-control" type="text" placeholder="輸入行程編號">
                                        </div>
                                    </div>
                                </div>
                                <div class="card-row">
                                    <!-- Date -->
                                    <div class="form-group card-column-1">
                                        <label>出發日期:</label>
                                        <div class="input-group date" id="departure-date-${sessionNum}" data-target-input="nearest">
                                            <input type="text" class="form-control datetimepicker-input" data-target="#departure-date-${sessionNum}">
                                            <div class="input-group-append" data-target="#departure-date-${sessionNum}" data-toggle="datetimepicker">
                                                <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- days -->
                                    <div class="form-group card-column-2">
                                        <label>天數:</label>
                                        <div class="input-group" id="days-${sessionNum}" data-target-input="nearest">
                                            <input class="form-control" type="text" placeholder="輸入行程天數">
                                        </div>
                                    </div>
                                </div>
                                <div class="card-row">
                                    <!-- Applicants -->
                                    <div class="form-group card-column-1">
                                        <label>已報名人數:</label>
                                        <div class="input-group" id="applicants-${sessionNum}" data-target-input="nearest">
                                            <input class="form-control" type="text" placeholder="輸入目前報名人數">
                                        </div>
                                    </div>
                                    <!-- Seats -->
                                    <div class="form-group card-column-2">
                                        <label>席次:</label>
                                        <div class="input-group" id="seats-${sessionNum}" data-target-input="nearest">
                                            <input class="form-control" type="text" placeholder="輸入總席次">
                                        </div>
                                    </div>
                                </div>
                                <div class="card-row">
                                    <!-- Bootstrap Switch -->
                                    <div class="form-group">
                                        <label>出團狀態:</label>
                                        <div style="display: flex;">
                                            <div>
                                                <input type="radio" id="radio-status-success-${sessionNum}" name="status-${sessionNum}" value="已成團" checked />
                                                <label for="radio-status-success-1">已成團</label>
                                            </div>

                                            <div class="card-column-2">
                                                <input type="radio" id="radio-status-unseccess-${sessionNum}" name="status-${sessionNum}" value="尚未成團" />
                                                <label for="radio-status-success-1">尚未成團</label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>`;
        $(".card-sessions").append(sessionItem);
        sessionNum += 1;
    });
});