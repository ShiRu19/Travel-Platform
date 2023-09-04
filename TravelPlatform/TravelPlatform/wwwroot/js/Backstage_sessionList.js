window.onload = function () {
    const urlParams = new URLSearchParams(window.location.search);
    const id = urlParams.get('id');

    GetOpenTravelList(id);
    GetCloseTravelList(id);

    $("#add-new-session").on("click", function () {
        window.location.href = `/admin/Backstage_AddSession.html?id=${id}`;
    });
}
function GetOpenTravelList(travelId) {
    axios.get("/api/v1.0/BackstageTravel/GetOpenTravelSessionList?id=" + travelId)
        .then((response) => {
            var datas = response.data.data;
            var title = $("#travel-title").html();
            var days = 0;

            if (datas.length !== 0) {
                title = datas[0].title;
                days = datas[0].days;
            }

            datas.forEach((data) => {
                var sessionId = data.id;
                var productNumber = data.productNumber;
                var departureDate = data.departureDate;
                var price = data.price;
                var remainingSeats = data.remainingSeats;
                var seats = data.seats;
                var groupStatus = data.groupStatus;

                var item = `<tr><td class="session-id" data-id=${sessionId}>#</td>\
                            <td><a>${productNumber}</a></td>\
                            <td>${title}</td>\
                            <td>${departureDate}</td>\
                            <td>${days}</td>\
                            <td>${price}</td>\
                            <td>${remainingSeats}</td>\
                            <td>${seats}</td>`;

                if (groupStatus === 1) {
                    item += `<td class="project-state"><span class="badge badge-success">已開團</span></td>`;
                }
                else {
                    item += `<td class="project-state"><span class="badge badge-warning">未開團</span></td>`;
                }
                item += `<td class="project-actions text-right">\
                            <a class="btn btn-info btn-sm" href="/admin/Backstage_Edit_Session.html?travelId=${travelId}&sessionId=${sessionId}"><i class="fas fa-pencil-alt"></i>編輯</a>\
                            <a class="btn btn-danger btn-sm" href="#"><i class="fas fa-trash"></i>刪除</a>\
                        </td></tr>`;

                $("#travel-table-open tbody").append(item);
            });
            $("#travel-title").html(title);
        })
        .catch((error) => {
            console.log(error);
            alert("抱歉...發生了一些錯誤，請再試一次！");
        });
}

function GetCloseTravelList(id) {
    axios.get("/api/v1.0/BackstageTravel/GetCloseTravelSessionList?id=" + id)
        .then((response) => {
            var datas = response.data.data;
            var title = $("#travel-title").html();
            var days = 0;

            if (datas.length !== 0) {
                title = datas[0].title;
                days = datas[0].days;
            }

            datas.forEach((data) => {
                var id = data.id;
                var productNumber = data.productNumber;
                var departureDate = data.departureDate;
                var price = data.price;
                var remainingSeats = data.remainingSeats;
                var seats = data.seats;
                var groupStatus = data.groupStatus;

                var item = `<tr><td data-id=${id}>#</td>\
                            <td><a>${productNumber}</a></td>\
                            <td>${title}</td>\
                            <td>${departureDate}</td>\
                            <td>${days}</td>\
                            <td>${price}</td>\
                            <td>${remainingSeats}</td>\
                            <td>${seats}</td>`;

                if (groupStatus === 1) {
                    item += `<td class="project-state"><span class="badge badge-success">已開團</span></td>`;
                }
                else {
                    item += `<td class="project-state"><span class="badge badge-warning">未開團</span></td>`;
                }
                item += `<td class="project-actions text-right">\
                            <a class="btn btn-info btn-sm" href="#"><i class="fas fa-pencil-alt"></i>編輯</a>\
                            <a class="btn btn-danger btn-sm" href="#"><i class="fas fa-trash"></i>刪除</a>\
                        </td></tr>`;

                $("#travel-table-close tbody").append(item);
            });
            $("#travel-title").html(title);
        })
        .catch((error) => {
            console.log(error);
            alert("抱歉...發生了一些錯誤，請再試一次！");
        });
}