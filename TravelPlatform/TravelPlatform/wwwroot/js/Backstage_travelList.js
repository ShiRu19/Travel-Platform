window.onload = function () {
    GetOpenTravelList();
    GetCloseTravelList();
}
function GetOpenTravelList() {
    axios.get("/api/v1.0/BackstageTravel/GetOpenTravelList")
        .then((response) => {
            var datas = response.data.data.travels;

            datas.forEach((data) => {
                var id = data.id;
                var title = data.title;

                // 日期區間_起始
                var dateRangeStart_utcDate = new Date(data.dateRangeStart + "Z");
                var dateRangeStart_year = dateRangeStart_utcDate.getFullYear();
                var dateRangeStart_month = ("0" + (dateRangeStart_utcDate.getMonth() + 1)).slice(-2);
                var dateRangeStart_date = ("0" + dateRangeStart_utcDate.getDate()).slice(-2);
                var dateRangeStart = `${dateRangeStart_year}/${dateRangeStart_month}/${dateRangeStart_date}`;

                // 日期區間_起始
                var dateRangeEnd_utcDate = new Date(data.dateRangeEnd + "Z");
                var dateRangeEnd_year = dateRangeEnd_utcDate.getFullYear();
                var dateRangeEnd_month = ("0" + (dateRangeEnd_utcDate.getMonth() + 1)).slice(-2);
                var dateRangeEnd_date = ("0" + dateRangeEnd_utcDate.getDate()).slice(-2);
                var dateRangeEnd = `${dateRangeEnd_year}/${dateRangeEnd_month}/${dateRangeEnd_date}`;

                var dateRange = `${dateRangeStart} ~ ${dateRangeEnd}`;

                var days = data.days;
                var nation = data.nation;
                var departureLocation = data.departureLocation;

                var item = `<tr><td>#</td>
                            <td><a href="/TravelDetail.html?id=${id}">${title}</a></td>
                            <td>${dateRange}</td>
                            <td>${days}</td>
                            <td>${nation}</td>
                            <td>${departureLocation}</td>
                            <td class="project-actions text-right">
                                <a class="btn btn-primary btn-sm" href="/admin/Backstage_SessionList.html?id=${id}"><i class="fas fa-folder"></i>詳細場次</a>
                                <a class="btn btn-info btn-sm" href="/admin/Backstage_Edit_Travel.html?id=${id}"><i class="fas fa-pencil-alt"></i>編輯</a>
                            </td></tr>`;

                $("#travel-table-open tbody").append(item);
                $("#loading-travel-open").hide();
            });
        })
        .catch((error) => {
            ShowErrorMessage(error);
            toastr.error('抱歉...發生了一些錯誤，請再試一次！', '錯誤');
        });
}

function GetCloseTravelList() {
    axios.get("/api/v1.0/BackstageTravel/GetCloseTravelList")
        .then((response) => {
            var datas = response.data.data.travels;

            datas.forEach((data) => {
                var id = data.id;
                var title = data.title;

                // 日期區間_起始
                var dateRangeStart_utcDate = new Date(data.dateRangeStart + "Z");
                var dateRangeStart_year = dateRangeStart_utcDate.getFullYear();
                var dateRangeStart_month = ("0" + (dateRangeStart_utcDate.getMonth() + 1)).slice(-2);
                var dateRangeStart_date = ("0" + dateRangeStart_utcDate.getDate()).slice(-2);
                var dateRangeStart = `${dateRangeStart_year}/${dateRangeStart_month}/${dateRangeStart_date}`;

                // 日期區間_起始
                var dateRangeEnd_utcDate = new Date(data.dateRangeEnd + "Z");
                var dateRangeEnd_year = dateRangeEnd_utcDate.getFullYear();
                var dateRangeEnd_month = ("0" + (dateRangeEnd_utcDate.getMonth() + 1)).slice(-2);
                var dateRangeEnd_date = ("0" + dateRangeEnd_utcDate.getDate()).slice(-2);
                var dateRangeEnd = `${dateRangeEnd_year}/${dateRangeEnd_month}/${dateRangeEnd_date}`;

                var dateRange = `${dateRangeStart} ~ ${dateRangeEnd}`;

                var days = data.days;
                var nation = data.nation;
                var departureLocation = data.departureLocation;

                var item = `<tr><td>#</td>
                            <td><a href="/TravelDetail.html?id=${id}">${title}</a></td>
                            <td>${dateRange}</td>
                            <td>${days}</td>
                            <td>${nation}</td>
                            <td>${departureLocation}</td>
                            <td class="project-actions text-right">
                                <a class="btn btn-primary btn-sm" href="/admin/Backstage_SessionList.html?id=${id}"><i class="fas fa-folder"></i>詳細場次</a>
                                <a class="btn btn-info btn-sm" href="/admin/Backstage_Edit_Travel.html?id=${id}"><i class="fas fa-pencil-alt"></i>編輯</a>
                            </td></tr>`;

                $("#travel-table-close tbody").append(item);
                $("#loading-travel-close").hide();
            });
        })
        .catch((error) => {
            ShowErrorMessage(error);
            toastr.error('抱歉...發生了一些錯誤，請再試一次！', '錯誤');
        });
}