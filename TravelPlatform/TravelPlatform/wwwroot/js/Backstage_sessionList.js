window.onload = function () {
    const urlParams = new URLSearchParams(window.location.search);
    const id = urlParams.get('id');

    GetOpenTravelList(id);
    GetCloseTravelList(id);
}
function GetOpenTravelList(id) {
    axios.get("/api/v1.0/BackstageTravel/GetOpenTravelSessionList?id=" + id)
        .then((response) => {
            var datas = response.data.data;

            datas.forEach((data) => {
                var id = data.id;
                var productNumber = data.productNumber;
                var title = data.title;
                var departureDate = data.departureDate;
                var days = data.days;
                var price = data.price;
                var remainingSeats = data.remainingSeats;
                var seats = data.seats;
                var groupStatus = data.groupStatus;

                var item = `<tr><td class="session-id" data-id=${id}>#</td>\
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
                            <a class="btn btn-primary btn-sm" href="#"><i class="fas fa-folder"></i>View</a>\
                            <a class="btn btn-info btn-sm" href="#"><i class="fas fa-pencil-alt"></i>Edit</a>\
                            <a class="btn btn-danger btn-sm" href="#"><i class="fas fa-trash"></i>Delete</a>\
                        </td></tr>`;

                $("#travel-table-open tbody").append(item);
            });
        })
        .catch((error) => console.log(error));
}

function GetCloseTravelList(id) {
    axios.get("/api/v1.0/BackstageTravel/GetCloseTravelSessionList?id=" + id)
        .then((response) => {
            var datas = response.data.data;

            datas.forEach((data) => {
                var id = data.id;
                var productNumber = data.productNumber;
                var title = data.title;
                var departureDate = data.departureDate;
                var days = data.days;
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
                            <a class="btn btn-primary btn-sm" href="#"><i class="fas fa-folder"></i>View</a>\
                            <a class="btn btn-info btn-sm" href="#"><i class="fas fa-pencil-alt"></i>Edit</a>\
                            <a class="btn btn-danger btn-sm" href="#"><i class="fas fa-trash"></i>Delete</a>\
                        </td></tr>`;

                $("#travel-table-close tbody").append(item);
            });
        })
        .catch((error) => console.log(error));
}