var travelId = 0;
var sessionId = 0;

$(function () {
    const urlParams = new URLSearchParams(window.location.search);
    travelId = urlParams.get('travelId');
    sessionId = urlParams.get('sessionId');
    
    GetSessionInfo();

    $("form").submit(function (e) {
        var correct = true;

        var applicants = Number($("#applicants").val());
        var seats = Number($("#seats").val());
        if (applicants > seats) {
            toastr.info("�w���W�H�Ƥ��i�j��u��");
            correct = false;
        }

        if (correct) {
            var formData = createSessionFormData();
            postSession(formData);
        }
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
    $('#departure-date').datetimepicker({
        format: 'L'
    });

    $("#cancel-btn").on("click", function () {
        window.location.href = `/admin/Backstage_SessionList.html?id=${travelId}`;
    })
})

async function GetSessionInfo() {
    await axios.get(`/api/v1.0/BackstageTravel/GetSessionInfo?id=${sessionId}`)
        .then((response) => {
            var sessionInfo = response.data;
            $("#product-number").val(sessionInfo.productNumber);
            $("#departure-date").val(sessionInfo.departureDate);
            $("#price").val(sessionInfo.price);
            $("#applicants").val(sessionInfo.applicants);
            $("#seats").val(sessionInfo.seats);

            if (sessionInfo.groupStatus === 1) {
                $("#radio-status-success").prop("checked", true);
            }
            else {
                $("#radio-status-unsuccess").prop("checked", true);
            }
        })
        .catch((error) => {
            console.log(error);
            toastr.error('��p...�o�ͤF�@�ǿ��~�A�ЦA�դ@���I', '���~');
        });
}

function createSessionFormData() {
    var formData = new FormData();

    formData.append("SessionId", sessionId);

    // TravelSession
    formData.append(`TravelSession.ProductNumber`, $(`#product-number`).val());
    formData.append(`TravelSession.Price`, $(`#price`).val());
    formData.append(`TravelSession.DepartureDate`, $(`#departure-date`).val());
    formData.append(`TravelSession.Applicants`, $(`#applicants`).val());
    formData.append(`TravelSession.Seats`, $(`#seats`).val());
    formData.append(`TravelSession.GroupStatus`, $(`input[name=status]:checked`, '#myForm').val());
    
    return formData;
}

async function postSession(formData) {
    await axios.post('/api/v1.0/BackstageTravel/EditSession', formData)
        .then((response) => {
            toastr.success(
                '�s�覨�\',
                '���\',
                {
                    timeOut: 2000,
                    fadeOut: 2000,
                    onHidden: function () {
                        window.location.href = `/admin/Backstage_SessionList.html?id=${travelId}`;
                    }
                }
            );
        })
        .catch((error) => {
            console.log(error);
            toastr.error('��p...�o�ͤF�@�ǿ��~�A�ЦA�դ@���I', '���~');
        });
}