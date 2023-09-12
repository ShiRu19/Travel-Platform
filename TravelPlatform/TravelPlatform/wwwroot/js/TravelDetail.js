var userId = 0;

$(function () {
    const urlParams = new URLSearchParams(window.location.search);
    const id = urlParams.get('id');

    CheckLoginToShowName();
    GetTravelDetail(id);
});

async function GetTravelDetail(id) {
    await axios.get(`/api/v1.0/ForestageTravel/GetTravelDetail?id=${id}`)
        .then((response) => {
            var data = response.data;

            var travelInfo = data.travelInfo[0];
            var travelSessions = data.travelSessions;
            var nation = travelInfo.nation == "�x�W" ? "�ꤺ�ȹC" : "��~�ȹC";

            /* ==================
             *   Travel sessions
             * ==================
             */
            var sessions = "";
            var application_btn = "";
            var applyUrl = "/Order.html?productNumber=";
            var day_list = ['��', '�@', '�G', '�T', '�|', '��', '��'];

            travelSessions.forEach((travelSession) => {
                // �X�o���
                var departureDate_utcDate = new Date(travelSession.departureDate + "Z");
                var departureDate_year = departureDate_utcDate.getFullYear();
                var departureDate_month = ("0" + (departureDate_utcDate.getMonth() + 1)).slice(-2);
                var departureDate_date = ("0" + departureDate_utcDate.getDate()).slice(-2);
                var departureDate_day = departureDate_utcDate.getDay();
                var departureDate = `${departureDate_year}/${departureDate_month}/${departureDate_date}(${day_list[departureDate_day]})`;

                // ����
                let price = (travelSession.price).toLocaleString('zh-tw', {
                    style: 'currency',
                    currency: 'TWD',
                    minimumFractionDigits: 0
                });

                // �i�_���W
                if (travelSession.remainingSeats > 0) {
                    application_btn = `<td>
                                            <a href="${applyUrl + travelSession.productNumber}">
                                                <button type="button" class="btn btn-block btn-outline-primary btn-sm">�ߧY���W</button>
                                            </a>
                                        </td>`;
                }
                else {
                    application_btn = "";
                }

                // session
                sessions += `<tr style="height: 40px;">
                                <td>${departureDate}</td>
                                <td>${travelSession.remainingSeats}</td>
                                <td>${travelSession.seats}</td>
                                <td>${travelSession.groupStatus}</td>
                                <td>${price}</td>
                                ${application_btn}
                            </tr>`;
            });

            /* ===============
             *   Date range
             * ===============
             */
            // ����϶�_�_�l
            var dateRangeStart_utcDate = new Date(travelInfo.dateRangeStart + "Z");
            var dateRangeStart_year = dateRangeStart_utcDate.getFullYear();
            var dateRangeStart_month = ("0" + (dateRangeStart_utcDate.getMonth() + 1)).slice(-2);
            var dateRangeStart_date = ("0" + dateRangeStart_utcDate.getDate()).slice(-2);
            var dateRangeStart = `${dateRangeStart_year}/${dateRangeStart_month}/${dateRangeStart_date}`;

            // ����϶�_�_�l
            var dateRangeEnd_utcDate = new Date(travelInfo.dateRangeEnd + "Z");
            var dateRangeEnd_year = dateRangeEnd_utcDate.getFullYear();
            var dateRangeEnd_month = ("0" + (dateRangeEnd_utcDate.getMonth() + 1)).slice(-2);
            var dateRangeEnd_date = ("0" + dateRangeEnd_utcDate.getDate()).slice(-2);
            var dateRangeEnd = `${dateRangeEnd_year}/${dateRangeEnd_month}/${dateRangeEnd_date}`;

            /* =================
             *   Follow button
             * =================
             */
            var like = `<div class="likes-container">
                            <div class="btn btn-app shown" id="add-like">
                                <i class="fas fa-heart"></i> �[�J�l��
                            </div>
                            <div class="btn btn-app hidden" id="cancel-like">
                                <i class="fas fa-heart" style="color: #ff0000"></i> �����l��
                            </div>
                        </div>`;

            /* ====================
             *  HTML travel detail
             * ====================
             */
            var item = `<div id="page-container">
                            <div id="content-wrap">
                                <div class="product-detail-container">
                                    <img id="product-main-image" src="${travelInfo.mainImageUrl}" alt="${travelInfo.title}" />
                                    <div id="product-main-info-container">
                                        <div id="product-travel-category"><strong>${nation}</strong></div>
                                        <div id="product-title">${travelInfo.title}</div>
                                        <div id="product-info-row">
                                            <div id="product-date">${dateRangeStart} ~ ${dateRangeEnd}</div>
                                            <div class="pdf-container">
                                                <i class="fa fa-info-circle"></i>
                                                <a href="${travelInfo.pdfUrl}" target="_blank">�ԲӦ�{����</a>
                                            </div>
                                        </div>
                                        <div id="split-horizontal-1"></div>

                                        <div id="product-sessions-container">
                                            <table class="sessionTable">
                                                <thead>
                                                    <tr style="height: 40px;">
                                                        <td>�X�o���</td>
                                                        <td>�i��</td>
                                                        <td>�u��</td>
                                                        <td>���Ϊ��A</td>
                                                        <td>����</td>
                                                        <td>���W</td>
                                                    </tr>
                                                </thead>
                                                <tbody class="column">
                                                    ${sessions}
                                                </tbody>
                                            </table>
                                        </div>
                                        ${like}
                                    </div>
                                </div>
                            </div>
                        </div>`;

            $(".section-right").append(item);

            /* =======================
             *  Check follow by user
             * =======================
             */
            CheckFollow(id);

            /* ================
             *   Follow event
             * ================
             */
            $("#add-like").on("click", function () {
                AddFollow(id);
            });
            $("#cancel-like").on("click", function () {
                CancelFollow(id);
            });
        })
        .catch((error) => {
            ShowErrorMessage(error);
            toastr.error('��p...���J�ɵo�ͤF�@�ǿ��~�A�ЦA�դ@���I', '���~');
        });
}

function CheckFollow(id) {
    var accessToken = localStorage.getItem("access_token");

    if (accessToken === null) {
        $("#add-like").removeClass("hidden").addClass("shown");
        $("#cancel-like").removeClass("shown").addClass("hidden");
        return;
    }

    CheckLoginRequired().then(function (profile) {
        userId = profile.id;

        axios.get(`/api/v1.0/Record/CheckFollow?TravelId=${id}&UserId=${userId}`, config)
            .then((response) => {
                $("#add-like").removeClass("shown").addClass("hidden");
                $("#cancel-like").removeClass("hidden").addClass("shown");
            })
            .catch((error) => {
                $("#add-like").removeClass("hidden").addClass("shown");
                $("#cancel-like").removeClass("shown").addClass("hidden");
            })
    });
}

function AddFollow(id) {
    CheckLoginRequired().then(function (profile) {
        var follow = new Object();
        follow.TravelId = parseInt(id);
        follow.UserId = profile.id;

        axios.post("/api/v1.0/Record/AddFollow", follow, config)
            .then((response) => {
                $("#add-like").removeClass("shown").addClass("hidden");
                $("#cancel-like").removeClass("hidden").addClass("shown");
            })
            .catch((error) => {
                console.log(error);
                toastr.error('��p...�o�ͤF�@�ǿ��~�A�ЦA�դ@���I', '���~');
            })
    });
}

function CancelFollow(id) {
    CheckLoginRequired().then(function (profile) {
        var follow = new Object();
        follow.TravelId = parseInt(id);
        follow.UserId = userId;

        axios.post("/api/v1.0/Record/CancelFollow", follow, config)
            .then((response) => {
                $("#add-like").removeClass("hidden").addClass("shown");
                $("#cancel-like").removeClass("shown").addClass("hidden");
            })
            .catch((error) => {
                console.log(error);
                toastr.error('��p...�o�ͤF�@�ǿ��~�A�ЦA�դ@���I', '���~');
            })
    });
}