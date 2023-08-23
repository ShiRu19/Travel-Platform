window.onload = function () {
    GetOpenTravelList();
    GetCloseTravelList();
}
function GetOpenTravelList() {
    axios.get("/api/v1.0/BackstageTravel/GetOpenTravelList")
        .then((response) => {
            var datas = response.data.data;

            datas.forEach((data) => {
                var id = data.id;
                var title = data.title;
                var dateRange = data.dateRange;
                var days = data.days;
                var nation = data.nation;
                var departureLocation = data.departureLocation;

                var item = `<tr><td>#</td>\
                            <td><a href="/TravelDetail.html?id=${id}">${title}</a></td>\
                            <td>${dateRange}</td>\
                            <td>${days}</td>\
                            <td>${nation}</td>\
                            <td>${departureLocation}</td>
                            <td class="project-actions text-right">\
                                <a class="btn btn-primary btn-sm" href="#"><i class="fas fa-folder"></i>詳細場次</a>\
                                <a class="btn btn-info btn-sm" href="#"><i class="fas fa-pencil-alt"></i>編輯</a>\
                                <a class="btn btn-danger btn-sm" href="#"><i class="fas fa-trash"></i>關閉</a>\
                            </td></tr>`;

                $("#travel-table-open tbody").append(item);
            });
        })
        .catch((error) => console.log(error));
}

function GetCloseTravelList() {
    axios.get("/api/v1.0/BackstageTravel/GetCloseTravelList")
        .then((response) => {
            var datas = response.data.data;

            datas.forEach((data) => {
                var id = data.id;
                var title = data.title;
                var dateRange = data.dateRange;
                var days = data.days;
                var nation = data.nation;
                var departureLocation = data.departureLocation;

                var item = `<tr><td>#</td>\
                            <td><a href="/TravelDetail.html?id=${id}">${title}</a></td>\
                            <td>${dateRange}</td>\
                            <td>${days}</td>\
                            <td>${nation}</td>\
                            <td>${departureLocation}</td>
                            <td class="project-actions text-right">\
                                <a class="btn btn-primary btn-sm" href="#"><i class="fas fa-folder"></i>詳細場次</a>\
                                <a class="btn btn-info btn-sm" href="#"><i class="fas fa-pencil-alt"></i>編輯</a>\
                                <a class="btn btn-danger btn-sm" href="#"><i class="fas fa-trash"></i>關閉</a>\
                            </td></tr>`;

                $("#travel-table-close tbody").append(item);
            });
        })
        .catch((error) => console.log(error));
}

function GetTravelSessionList() {
    axios.get("/api/v1.0/BackstageTravel/GetTravelSessionList")
        .then((response) => {
            var datas = response.data;
            datas.forEach(function (data) {
                var id = data.id;
                var productNumber = data.productNumber;
                var title = data.title;
                var departure_date = data.departure_date.substring(0, 10);
                var days = data.days;
                var price = data.price;
                var remaining_seats = data.remaining_seats;
                var seats = data.seats;
                var group_status = data.group_status;

                var item = `<tr><td>#</td>\
                            <td><a>${productNumber}</a></td>\
                            <td>${title}</td>\
                            <td>${departure_date}</td>\
                            <td>${days}</td>\
                            <td>${price}</td>\
                            <td>${remaining_seats}</td>\
                            <td>${seats}</td>`;

                if (group_status === 1) {
                    item += `<td class="project-state"><span class="badge badge-success">已開團</span></td>`;
                }
                else {
                    item += `<td class="project-state"><span class="badge badge-warning">未開團</span></td>`;
                }
                item += `<td class="project-actions text-right">\
                            <a class="btn btn-primary btn-sm" href="#"><i class="fas fa-folder"></i>View</a>\
                            <a class="btn btn-info btn-sm" href="#"><i class="fas fa-pencil-alt"></i>Edit</a>\
                            <a class="btn btn-danger btn-sm" href="#"><i class="fas fa-trash"></i>Delete</a>\
                        </td></tr>`;

                $("#travel-table tbody").append(item);
            })
        })
        .catch((error) => {
            console.log(error);
            alert("Sorry, we have some error...\nPlease try again");
        });
}