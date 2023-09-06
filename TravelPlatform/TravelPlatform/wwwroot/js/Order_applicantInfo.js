var price = 0;
var sessionId = 0;
var productNumber = "";
var qty = 0;

$(function () {
    const urlParams = new URLSearchParams(window.location.search);
    const nation = urlParams.get('nation');
    qty = urlParams.get('qty');
    productNumber = urlParams.get('productNumber');

    CheckLoginRequired().then(function (profile) {
        GetSessionDetail(productNumber);
        GenerateTravelerForm(qty);
    });

    $("form").submit(function (e) {
        var formData = GenerateOrder(qty, nation, sessionId);
        console.log(formData);
        postOrder(formData);
    });
});

function GenerateTravelerForm(qty) {
    for (let i = 1; i <= qty; i++) {
        let item = `<!-- card -->
                    <div id="traveler-container">
                        <div class="card card-info" style="margin: 5px;">
                            <div class="card-header">
                                <h3 class="card-title">旅客 - ${i}</h3>
                            </div>
                            <div class="row">
                                <label class="traveler-label-text" for="traveler-name-${i}">姓名</label>
                                <input type="text" class="form-control col-3" id="traveler-name-${i}" placeholder="請輸入全名" required />
                                <label class="traveler-label-text">性別</label>
                                <div class="row">
                                    <div>
                                        <input type="radio" id="radio-man-${i}" name="sex-${i}" value="man" checked />
                                        <label class="traveler-label-text" for="radio-man-${i}">男</label>
                                    </div>

                                    <div class="card-column-2">
                                        <input type="radio" id="radio-women-${i}" name="sex-${i}" value="women" />
                                        <label class="traveler-label-text" for="radio-women-${i}">女</label>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <!-- Date -->
                                <label class="traveler-label-text">出生日期</label>
                                <div class="input-group date col-3" data-target-input="nearest">
                                    <input type="text" id="traveler-birthday-${i}" class="form-control datetimepicker-input" data-target="#traveler-birthday-${i}" />
                                    <div class="input-group-append" data-target="#traveler-birthday-${i}" data-toggle="datetimepicker">
                                        <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                    </div>
                                </div>
                                <!-- /.Date-->
                                <label class="traveler-label-text" for="traveler-phone-${i}">手機</label>
                                <input type="text" class="form-control col-3" id="traveler-phone-${i}" placeholder="請輸入手機號碼" required />
                            </div>
                        </div>
                    </div>
                    <!-- /.card -->`;
        $("#traveler-information-container").append(item);

        $(`#traveler-birthday-${i}`).datetimepicker({
            format: 'L'
        });
    }
}

function GenerateOrder(qty, nation) {
    var formData = new FormData();

    // Order
    formData.append("Nation", nation);
    formData.append("SessionId", sessionId);
    formData.append("Total", qty * price);
    formData.append("UserId", profile.id);
    formData.append("UserName", $("#contact-person-name").val());
    formData.append("UserEmail", $("#contact-person-email").val());
    formData.append("UserPhone", $("#contact-person-phone").val());

    for (let i = 1; i <= qty; i++) {
        formData.append(`OrderTravelers[${i-1}].Name`, $(`#traveler-name-${i}`).val());
        formData.append(`OrderTravelers[${i-1}].Sex`, $(`input[name=sex-${i}]:checked`, '#myForm').val());
        formData.append(`OrderTravelers[${i-1}].Birthday`, $(`#traveler-birthday-${i}`).val());
        formData.append(`OrderTravelers[${i-1}].PhoneNumber`, $(`#traveler-phone-${i}`).val());
        formData.append(`OrderTravelers[${i-1}].Price`, price);
    }

    return formData;
}

async function postOrder(formData) {
    await axios.post('/api/v1.0/Order/GenerateOrder', formData)
        .then((response) => {
            window.location.href = `/Order_Pay.html?qty=${qty}&productNumber=${productNumber}&orderId=${response.data.orderId}`;
        })
        .catch((error) => {
            console.log(error);
            toastr.error('抱歉...發生了一些錯誤，請再試一次！', '錯誤');
        });
}

async function GetSessionDetail(productNumber) {
    await axios.get(`/api/v1.0/ForestageTravel/GetSessionDetail?productNumber=${productNumber}`)
        .then((response) => {
            var data = response.data;
            price = data.price;
            sessionId = data.sessionId;
        })
        .catch((error) => {
            console.log(error);
        })
}