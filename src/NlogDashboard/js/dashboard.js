$(document).ready(function () {
    /**
     * Line Chart
     */
    var lineChart = $('#line-chart');

    if (lineChart.length > 0) {
        new Chart(lineChart, {
            type: 'line',
            data: {
                labels: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
                datasets: [{
                    label: 'Users',
                    data: [12, 19, 3, 5, 2, 3, 20, 33, 23, 12, 33, 10],
                    backgroundColor: 'rgba(66, 165, 245, 0.5)',
                    borderColor: '#2196F3',
                    borderWidth: 1
                }]
            },
            options: {
                legend: {
                    display: false
                },
                scales: {
                    yAxes: [{
                        ticks: {
                            beginAtZero: true
                        }
                    }]
                }
            }
        });
    }

    /**
     * Bar Chart
     */
    var barChart = $('#bar-chart');

    if (barChart.length > 0) {
        new Chart(barChart, {
            type: 'bar',
            data: {
                labels: ["Red", "Blue", "Cyan", "Green", "Purple", "Orange"],
                datasets: [{
                    label: '# of Votes',
                    data: [12, 19, 3, 5, 2, 3],
                    backgroundColor: [
                        'rgba(244, 88, 70, 0.5)',
                        'rgba(33, 150, 243, 0.5)',
                        'rgba(0, 188, 212, 0.5)',
                        'rgba(42, 185, 127, 0.5)',
                        'rgba(156, 39, 176, 0.5)',
                        'rgba(253, 178, 68, 0.5)'
                    ],
                    borderColor: [
                        '#F45846',
                        '#2196F3',
                        '#00BCD4',
                        '#2ab97f',
                        '#9C27B0',
                        '#fdb244'
                    ],
                    borderWidth: 1
                }]
            },
            options: {
                legend: {
                    display: false
                },
                scales: {
                    yAxes: [{
                        ticks: {
                            beginAtZero: true
                        }
                    }]
                }
            }
        });
    }

    /**
     * Radar Chart
     */
    var radarChart = $('#radar-chart');

    if (radarChart.length > 0) {
        new Chart(radarChart, {
            type: 'radar',
            data: {
                labels: ["Red", "Blue", "Cyan", "Green", "Purple", "Orange"],
                datasets: [{
                    label: 'Users',
                    data: [100, 45, 87, 50, 77, 20],
                    backgroundColor: 'rgba(244, 88, 70, 0.5)',
                    borderColor: '#F45846',
                    borderWidth: 1
                }, {
                    label: 'Votes',
                    data: [23, 55, 75, 54, 95, 100],
                    backgroundColor: 'rgba(33, 150, 243, 0.5)',
                    borderColor: '#2196F3',
                    borderWidth: 1
                }]
            }
        });
    }

    /**
     * Pie Chart
     */
    var pieChart = $('#pie-chart');

    if (pieChart.length > 0) {
        new Chart(pieChart, {
            type: 'pie',
            data: {
                labels: ["Red", "Blue", "Cyan", "Green", "Purple", "Orange"],
                datasets: [{
                    label: 'Users',
                    data: [100, 45, 87, 50, 77, 20],
                    backgroundColor: [
                        'rgba(244, 88, 70, 0.5)',
                        'rgba(33, 150, 243, 0.5)',
                        'rgba(0, 188, 212, 0.5)',
                        'rgba(42, 185, 127, 0.5)',
                        'rgba(156, 39, 176, 0.5)',
                        'rgba(253, 178, 68, 0.5)'
                    ],
                    borderColor: [
                        'rgba(244, 88, 70, 0.5)',
                        'rgba(33, 150, 243, 0.5)',
                        'rgba(0, 188, 212, 0.5)',
                        'rgba(42, 185, 127, 0.5)',
                        'rgba(156, 39, 176, 0.5)',
                        'rgba(253, 178, 68, 0.5)'
                    ],
                    borderWidth: 1
                }]
            }
        });
    }

    /**
     * Widget Line Chart
     */
    var wLineChart = $('.widget-line-chart');

    wLineChart.each(function (index, canvas) {
        new Chart(canvas, {
            type: 'line',
            data: {
                labels: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
                datasets: [{
                    label: 'Users',
                    data: [12, 19, 3, 5, 2, 3, 20, 33, 23, 12, 33, 10],
                    borderColor: '#fff',
                    borderWidth: 1,
                    fill: false,
                }]
            },
            options: {
                legend: {
                    display: false
                },
                scales: {
                    yAxes: [{
                        ticks: {
                            beginAtZero: true,
                            display: false,
                        },
                        gridLines: {
                            display: false,
                            drawBorder: false,
                        }
                    }],
                    xAxes: [{
                        ticks: {
                            display: false,
                        },
                        gridLines: {
                            display: false,
                            drawBorder: false,
                        }
                    }]
                }
            }
        });
    });
});
