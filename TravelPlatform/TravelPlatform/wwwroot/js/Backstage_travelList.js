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
                            <td><a href="/admin/TravelDetail.html?id=${id}">${title}</a></td>\
                            <td>${dateRange}</td>\
                            <td>${days}</td>\
                            <td>${nation}</td>\
                            <td>${departureLocation}</td>
                            <td class="project-actions text-right">\
                                <a class="btn btn-primary btn-sm" href="/admin/Backstage_SessionList.html?id=${id}"><i class="fas fa-folder"></i>�Բӳ���</a>\
                                <a class="btn btn-info btn-sm" href="/admin/Backstage_Edit_Travel.html?id=${id}"><i class="fas fa-pencil-alt"></i>�s��</a>\
                                <a class="btn btn-danger btn-sm" href="#"><i class="fas fa-trash"></i>����</a>\
                            </td></tr>`;

                $("#travel-table-open tbody").append(item);
            });
        })
        .catch((error) => {
            console.log(error);
            alert("��p...�o�ͤF�@�ǿ��~�A�ЦA�դ@���I");
        });
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
                            <td><a href="/admin/TravelDetail.html?id=${id}">${title}</a></td>\
                            <td>${dateRange}</td>\
                            <td>${days}</td>\
                            <td>${nation}</td>\
                            <td>${departureLocation}</td>
                            <td class="project-actions text-right">\
                                <a class="btn btn-primary btn-sm" href="/admin/Backstage_SessionList.html?id=${id}"><i class="fas fa-folder"></i>�Բӳ���</a>\
                                <a class="btn btn-info btn-sm" href="/admin/Backstage_Edit_Travel.html?id=${id}"><i class="fas fa-pencil-alt"></i>�s��</a>\
                                <a class="btn btn-danger btn-sm" href="#"><i class="fas fa-trash"></i>����</a>\
                            </td></tr>`;

                $("#travel-table-close tbody").append(item);
            });
        })
        .catch((error) => {
            console.log(error);
            alert("��p...�o�ͤF�@�ǿ��~�A�ЦA�դ@���I");
        });
}