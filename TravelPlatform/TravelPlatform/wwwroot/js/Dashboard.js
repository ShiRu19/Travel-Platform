$(function () {
    var startDate = '01-01-2023';
    var endDate = '12-31-2023';
    var nation = '台灣';
    ShowDomesticGroupStatus(startDate, endDate); // Stacked Bar Chart
    ShowDomesticSalesVolume(startDate, endDate); // Pie Chart
    ShowSalesVolumeOfMonth(nation, startDate, endDate); // Bar Chart
    ShowSalesOfMonth(nation, startDate, endDate); // Bar Chart
    ShowTopFiveOfFollows(); // Bar Chart
});


async function ShowDomesticGroupStatus(startDate, endDate) {
    var groupStatus = await axios.get(`/api/v1.0/Dashboard/GetDomesticGroupStatus?start=${startDate}&end=${endDate}`)
        .then((response) => {
            return response.data.data;
        })
        .catch((error) => {
            console.log(error);
            toastr.error('抱歉...發生了一些錯誤，請再試一次！', '錯誤');
        });

    var locationLabels = [];
    var success = [];
    var failure = [];
    var rate = [];
    
    for (const [key, value] of Object.entries(groupStatus)) {
        locationLabels.push(key);
        success.push(value.success);
        failure.push(value.failure);
        var r = value.success / (value.success + value.failure) * 100;
        rate.push(r.toFixed(2));
    }

    var stackedBarChartCanvas = $('#domestic-group-status').get(0).getContext('2d');

    const data = {
        labels: locationLabels,
        datasets: [{
            label: '成團',
            data: success,
            yAxisID: 'y-axis',
            backgroundColor: [
                'rgba(0, 91, 255, 0.2)',
                'rgba(0, 91, 255, 0.2)',
                'rgba(0, 91, 255, 0.2)'
            ],
            boarderColor: [
                'rgba(0, 91, 255, 0.5)',
                'rgba(0, 91, 255, 0.5)',
                'rgba(0, 91, 255, 0.5)'
            ],
            borderWidth: 1,
            order: 2
        },
        {
            label: '未成團',
            data: failure,
            yAxisID: 'y-axis',
            backgroundColor: [
                'rgba(135, 91, 255, 0.2)',
                'rgba(135, 91, 255, 0.2)',
                'rgba(135, 91, 255, 0.2)'
            ],
            borderColor: [
                'rgba(135, 91, 255, 0.5)',
                'rgba(135, 91, 255, 0.5)',
                'rgba(135, 91, 255, 0.5)'
            ],
            borderWidth: 1,
            order: 2
        },
        {
            label: '成團率',
            data: rate,
            yAxisID: 'percentage-axis',
            fill: false,
            borderColor: [
                'rgba(245, 24, 66, 1)',
                'rgba(245, 24, 66, 1)',
                'rgba(245, 24, 66, 1)'
            ],
            tension: 0.4,
            type: 'line',
            order: 1,
        }]
    };

    var stackedBarChartOptions = {
        responsive: true,
        maintainAspectRatio: false,
        scales: {
            xAxes: [{
                stacked: true,
            }],
            yAxes: [
                {
                    id: 'y-axis',
                    stacked: true,
                },
                {
                    id: 'percentage-axis',
                    position: 'right',
                    min: 0,
                    max: 100,
                    stepSize: 20,
                    ticks: {
                        callback: function (value, index, values) {
                            return value + '%';
                        }
                    }
                }
            ]
        }
    };

    new Chart(stackedBarChartCanvas, {
        type: 'bar',
        data: data,
        options: stackedBarChartOptions
    });
}

async function ShowDomesticSalesVolume(startDate, endDate) {
    var salesVolume = await axios.get(`/api/v1.0/Dashboard/GetDomesticSalesVolume?start=${startDate}&end=${endDate}`)
        .then((response) => {
            return response.data.data;
        })
        .catch((error) => {
            console.log(error);
            toastr.error('抱歉...發生了一些錯誤，請再試一次！', '錯誤');
        });

    var locationLabels = [];
    var counts = [];
    var colors = ['#fcca03', '#115aad', '#ac1cff', '#00c0ef', '#3c8dbc', '#d2d6de'];

    salesVolume.forEach((sale) => {
        locationLabels.push(sale.location);
        counts.push(sale.count);
    })

    var pieData = {
        labels: locationLabels,
        datasets: [
            {
                data: counts,
                backgroundColor: colors,
            }
        ]
    }

    var pieChartCanvas = $('#domestic-sales-volume').get(0).getContext('2d');

    var pieOptions = {
        maintainAspectRatio: false,
        responsive: true,
    }

    new Chart(pieChartCanvas, {
        type: 'pie',
        data: pieData,
        options: pieOptions
    })
}

async function ShowSalesVolumeOfMonth(nation, startDate, endDate) {
    await axios.get(`/api/v1.0/Dashboard/GetSalesVolume?nation=${nation}&start=${startDate}&end=${endDate}`)
        .then((response) => {
            var data = response.data.data;

            var month = ['一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月'];
            var count = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];

            data.forEach((sale) => {
                count[sale.month-1] += sale.count;
            });

            var barChartCanvas = $('#sales-volume-month').get(0).getContext('2d')

            var barChartData = {
                labels: month,
                datasets: [
                    {
                        label: '訂單筆數',
                        backgroundColor: '#053085',
                        borderColor: 'rgba(60,141,188,0.8)',
                        pointRadius: false,
                        pointColor: '#3b8bba',
                        pointStrokeColor: 'rgba(60,141,188,1)',
                        pointHighlightFill: '#fff',
                        pointHighlightStroke: 'rgba(60,141,188,1)',
                        data: count
                    }
                ]
            }

            var barChartOptions = {
                responsive: true,
                maintainAspectRatio: false,
                datasetFill: false
            }

            new Chart(barChartCanvas, {
                type: 'bar',
                data: barChartData,
                options: barChartOptions
            })
        })
        .catch((error) => {
            console.log(error);
            toastr.error('抱歉...發生了一些錯誤，請再試一次！', '錯誤');
        });
}

async function ShowSalesOfMonth(nation, startDate, endDate) {
    await axios.get(`/api/v1.0/Dashboard/GetSales?nation=${nation}&start=${startDate}&end=${endDate}`)
        .then((response) => {
            var data = response.data.data;

            var month = ['一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月'];
            var sum = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];

            data.forEach((sale) => {
                sum[sale.month-1] += sale.sum;
            });

            var barChartCanvas = $('#sales-month').get(0).getContext('2d')

            var barChartData = {
                labels: month,
                datasets: [
                    {
                        label: '銷售金額',
                        backgroundColor: '#053085',
                        borderColor: 'rgba(60,141,188,0.8)',
                        pointRadius: false,
                        pointColor: '#3b8bba',
                        pointStrokeColor: 'rgba(60,141,188,1)',
                        pointHighlightFill: '#fff',
                        pointHighlightStroke: 'rgba(60,141,188,1)',
                        data: sum
                    }
                ]
            }

            var barChartOptions = {
                responsive: true,
                maintainAspectRatio: false,
                datasetFill: false
            }

            new Chart(barChartCanvas, {
                type: 'bar',
                data: barChartData,
                options: barChartOptions
            })
        })
        .catch((error) => {
            console.log(error);
            toastr.error('抱歉...發生了一些錯誤，請再試一次！', '錯誤');
        });
}

function ShowTopFiveOfFollows() {
    axios.get("/api/v1.0/Dashboard/GetFollowTopFive")
        .then((response) => {
            var datas = response.data.data;

            datas.forEach((data) => {
                var id = data.id;
                var title = data.title;
                var nation = data.nation;
                var days = data.days;
                var follows = data.follows;

                var item = `<tr>
                                <td><a href="/admin/TravelDetail.html?id=${id}">${title}</a></td>
                                <td>${nation}</td>
                                <td>${days}</td>
                                <td>${follows}</td>
                            </tr>`;

                $("#open-travel-follow-top-5").append(item);
            })
        })
        .catch((error) => {
            console.log(error);
            toastr.error('抱歉...發生了一些錯誤，請再試一次！', '錯誤');
        });
}
