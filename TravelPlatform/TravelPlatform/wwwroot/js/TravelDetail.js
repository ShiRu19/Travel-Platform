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
                            </div>
                        </div>

                        <div class="split-container">
                            <div id="split-text">
                                ��h���~��T
                            </div>
                            <div id="split-horizontal-2"></div>
                        </div>

                        <div class="product-info-more">
                            <a href="${travelInfo.pdfUrl}" download="${travelInfo.title}.pdf">�ԲӦ�{����</a>
                        </div>
                    </div>
                </div>`

            $(".section-right").append(item);
        })
        .catch((error) => console.log(error));

}