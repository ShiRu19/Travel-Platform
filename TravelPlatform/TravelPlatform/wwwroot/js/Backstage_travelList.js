window.onload = function () {
    GetTravelList();
}

function GetTravelList() {
    axios.get("/api/v1.0/TravelBackstage/TravelList")
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