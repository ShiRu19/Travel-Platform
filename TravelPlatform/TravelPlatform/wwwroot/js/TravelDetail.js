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
            var data = response.data.data;
            var travelInfo = data.travelInfo[0];
            var travelSessions = data.travelSessions;

            var nation = travelInfo.nation == "�x�W" ? "�ꤺ�ȹC" : "��~�ȹC";

            var sessions = "";
            var applyUrl = "/Order.html?productNumber=";

            travelSessions.forEach((travelSession) => {
                sessions += `<tr style="height: 40px;">
                                    <td>${travelSession.departureDate}</td>
                                    <td>${travelSession.remainingSeats}</td>
                                    <td>${travelSession.seats}</td>
                                    <td>${travelSession.groupStatus}</td>
                                    <td>${travelSession.price}</td>`;
                if (travelSession.remainingSeats > 0) {
                    sessions += `<td>
                                        <a href="${applyUrl + travelSession.productNumber}">
                                            <button type="button" class="btn btn-block btn-outline-primary btn-sm">�ߧY���W</button>
                                        </a>
                                    </td>
                                </tr>`;
                }
                else {
                    sessions += `</tr>`;
                }
            });


            var like = `<div class="likes-container">
                            <div class="btn btn-app shown" id="add-like">
                                <i class="fas fa-heart"></i> �[�J�l��
                            </div>
                            <div class="btn btn-app hidden" id="cancel-like">
                                <i class="fas fa-heart" style="color: #ff0000"></i> �����l��
                            </div>
                        </div>`;

            var item = `<div id="page-container">
                    <div id="content-wrap">
                        <div class="product-detail-container">
                            <img id="product-main-image" src="${travelInfo.mainImageUrl}" alt="${travelInfo.title}" />
                            <div id="product-main-info-container">
                                <div id="product-travel-category"><strong>${nation}</strong></div>
                                <div id="product-title">${travelInfo.title}</div>
                                <div id="product-date">${travelInfo.dateRangeStart} ~ ${travelInfo.dateRangeEnd}</div>
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

                        <div class="split-container">
                            <div id="split-text">
                                ��h���~��T
                            </div>
                            <div id="split-horizontal-2"></div>
                        </div>

                        <div class="product-info-more">
                            <a href="${travelInfo.pdfUrl}" target="_blank">�ԲӦ�{����</a>
                        </div>
                    </div>
                </div>`

            $(".section-right").append(item);

            CheckFollow(id);

            $("#add-like").on("click", function () {
                AddFollow(id);
            });
            $("#cancel-like").on("click", function () {
                CancelFollow(id);
            });
        })
        .catch((error) => console.log(error));
}

function CheckFollow(id) {
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
    var follow = new Object();
    follow.TravelId = parseInt(id);
    follow.UserId = userId;

    axios.post("/api/v1.0/Record/AddFollow", follow, config)
        .then((response) => {
            $("#add-like").removeClass("shown").addClass("hidden");
            $("#cancel-like").removeClass("hidden").addClass("shown");
        })
        .catch((error) => {
            console.log(error);
            alert("��p...�o�ͤF�@�ǿ��~�A�ЦA�դ@���I");
        })
}

function CancelFollow(id) {
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
            alert("��p...�o�ͤF�@�ǿ��~�A�ЦA�դ@���I");
        })
}