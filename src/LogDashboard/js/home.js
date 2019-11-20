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
    charts[0].debug = output.Debug;
    charts[0].info = output.Info;
    charts[0].warn = output.Warn;
    charts[0].fatal = output.Fatal;
    charts[0].trace = output.Trace;
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
        charts[i - 1].debug = data.Debug;
        charts[i - 1].info = data.Info;
        charts[i - 1].warn = data.Warn;
        charts[i - 1].fatal = data.Fatal;
        charts[i - 1].trace = data.Trace;
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
                    }, {
                        label: 'debug',
                        data: opt.debug,
                        backgroundColor: 'rgba(156, 204, 101, 0.5)',
                        borderColor: '#9ccc65',
                        borderWidth: 1
                    }, {
                        label: 'info',
                        data: opt.info,
                        backgroundColor: 'rgba(38, 198, 218, 0.5)',
                        borderColor: '#26c6da',
                        borderWidth: 1
                    }, {
                        label: 'warn',
                        data: opt.warn,
                        backgroundColor: 'rgba(255, 202, 40, 0.5)',
                        borderColor: '#ffca28',
                        borderWidth: 1
                    }, {
                        label: 'fatal',
                        data: opt.fatal,
                        backgroundColor: 'rgba(150, 12, 10, 0.5)',
                        borderColor: '#960c0a',
                        borderWidth: 1
                    }, {
                        label: 'trace',
                        data: opt.trace,
                        backgroundColor: 'rgba(170, 170, 170, 0.5)',
                        borderColor: '#aaa',
                        borderWidth: 1
                    }
                ]
            },
            options: {
                scales: {
                    yAxes: [{
                        scaleLabel: {
                            display: true,
                            labelString: 'value'
                        },
                        ticks: {
                            beginAtZero: true
                        }
                    }]
                }

               
            }
        });
}