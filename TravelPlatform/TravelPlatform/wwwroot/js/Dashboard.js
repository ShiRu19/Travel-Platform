$(function () {
    var startDate = '01-01-2023';
    var endDate = '12-31-2023';
    ShowDomesticGroupStatus(startDate, endDate); // Stacked Bar Chart
    ShowDomesticSalesVolume(startDate, endDate); // Pie Chart
});

async function ShowDomesticGroupStatus(startDate, endDate) {
    var groupStatus = await axios.get(`/api/v1.0/Dashboard/GetDomesticGroupStatus?start=${startDate}&end=${endDate}`)
        .then((response) => {
            return response.data.data;
        })
        .catch((error) => console.log(error));

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
        .catch((error) => console.log(error));

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