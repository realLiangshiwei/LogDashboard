function monthChartLabels() {
    var now = new Date();
    month = parseInt(now.getMonth(), 10) + 1;
    var days = new Date(now.getYear(), month, 0).getDate();
    var res = [];
    for (var i = 1; i <= days; i++) {
        res.push(i + "d");
    }
    return res;
}

var charts = [
    { chart: $('#hourChart'), id: '#hourChart', labels: ["10m", "20m", "30m", "40m", '50m', '60m'] },
    {
        chart: $('#dayChart'),
        id: '#dayChart',
        labels: [
            "1h", "2h", "3h", "4h", '5h', '6h', '7h', '8h', '9h', '10h', '11h', '12h', '13h', '14h', '15h',
            '16h', '17h', '18h', '19h', '20h', '21h', '22h', '23h', '24h'
        ]
    },
    {
        chart: $('#weekChart'),
        id: '#weekChart',
        labels: ["Monday", "Tuesday", "Wednesday", "Thursday", 'Friday', 'Saturday', 'Sunday']
    },
    { chart: $('#monthChart'), id: '#monthChart', labels: monthChartLabels() }
];


$(function () {
    charts[0].all = output.All;
    charts[0].error = output.Error;
    initLineChart(charts[0]);
});

function getLogChart(i) {
    $.ajax({
        method: "post",
        url: mapPath + "/Dashboard/GetLogChart",
        data: JSON.stringify({ ChartDataType: i })
    }).done(function (data) {
        charts[i - 1].all = data.All;
        charts[i - 1].error = data.Error;
        initLineChart(charts[i - 1]);
    });
}

function initLineChart(opt) {
    new Chart(opt.chart,
        {
            type: 'line',
            data: {
                labels: opt.labels,
                datasets: [
                    {
                        label: 'all',
                        data: opt.all,
                        backgroundColor: 'rgba(66, 165, 245, 0.5)',
                        borderColor: '#2196F3',
                        borderWidth: 1
                    }, {
                        label: 'error',
                        data: opt.error,
                        backgroundColor: 'rgba(251, 105, 104, 0.5)',
                        borderColor: '#f75656',
                        borderWidth: 1
                    }
                ]
            },
            options: {
                legend: {
                    display: false
                },
                scales: {
                    yAxes: [
                        {
                            ticks: {
                                beginAtZero: true
                            }
                        }
                    ]
                }
            }
        });
}