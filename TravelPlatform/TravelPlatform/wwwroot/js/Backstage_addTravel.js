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
                            <input class="form-control" type="text" placeholder="���I or ����">
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
                                        <div class="input-group" id="product-number-${sessionNum}" data-target-input="nearest">
                                            <input class="form-control" type="text" placeholder="��J��{�s��">
                                        </div>
                                    </div>
                                </div>
                                <div class="card-row">
                                    <!-- Date -->
                                    <div class="form-group card-column-1">
                                        <label>�X�o���:</label>
                                        <div class="input-group date" id="departure-date-${sessionNum}" data-target-input="nearest">
                                            <input type="text" class="form-control datetimepicker-input" data-target="#departure-date-${sessionNum}">
                                            <div class="input-group-append" data-target="#departure-date-${sessionNum}" data-toggle="datetimepicker">
                                                <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- days -->
                                    <div class="form-group card-column-2">
                                        <label>�Ѽ�:</label>
                                        <div class="input-group" id="days-${sessionNum}" data-target-input="nearest">
                                            <input class="form-control" type="text" placeholder="��J��{�Ѽ�">
                                        </div>
                                    </div>
                                </div>
                                <div class="card-row">
                                    <!-- Applicants -->
                                    <div class="form-group card-column-1">
                                        <label>�w���W�H��:</label>
                                        <div class="input-group" id="applicants-${sessionNum}" data-target-input="nearest">
                                            <input class="form-control" type="text" placeholder="��J�ثe���W�H��">
                                        </div>
                                    </div>
                                    <!-- Seats -->
                                    <div class="form-group card-column-2">
                                        <label>�u��:</label>
                                        <div class="input-group" id="seats-${sessionNum}" data-target-input="nearest">
                                            <input class="form-control" type="text" placeholder="��J�`�u��">
                                        </div>
                                    </div>
                                </div>
                                <div class="card-row">
                                    <!-- Bootstrap Switch -->
                                    <div class="form-group">
                                        <label>�X�Ϊ��A:</label>
                                        <div style="display: flex;">
                                            <div>
                                                <input type="radio" id="radio-status-success-${sessionNum}" name="status-${sessionNum}" value="�w����" checked />
                                                <label for="radio-status-success-1">�w����</label>
                                            </div>

                                            <div class="card-column-2">
                                                <input type="radio" id="radio-status-unseccess-${sessionNum}" name="status-${sessionNum}" value="�|������" />
                                                <label for="radio-status-success-1">�|������</label>
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