var createCostSalesChart = function (element, dataLabels, dataValuesCosts, dataValuesSales, dataValuesProfit) {
    var ctx = document.getElementById(element).getContext('2d');
    new Chart(ctx, {
        type: "bar",
        data: {
            labels: dataLabels,
            datasets: [{
                label: "Costos",
                data: dataValuesCosts,
                backgroundColor: "rgba(255, 99, 132, 0.2)",
                borderColor: "rgba(255, 99, 132, 1)",
                borderWidth: 1,
                order: 1
            }, {
                    label: "Ventas",
                    data: dataValuesSales,
                    backgroundColor: "rgba(75, 192, 192, 0.2)",
                    borderColor: "rgba(75, 192, 192, 1)",
                    borderWidth: 1,
                    order: 2
                }, {
                    label: "Ganancia",
                    type: "line",
                    data: dataValuesProfit,
                    borderColor: "rgba(0, 0, 0)",  
                    backgroundColor: "rgba(0, 0, 0, 0)",
                    order: 3
                }],
        },
        options: {
            title: {
                display: true,
                text: "Costos, Ventas y Ganancias"
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

var createChart = function (element, dataLabels, dataValues, chartType, title, labelsTitle, backgroundColor, borderColor) {
    var ctx = document.getElementById(element).getContext('2d');
    if (chartType != "polarArea") {
        var options = {
            title: {
                display: true,
                text: title
            },
            scales: {
                yAxes: [{
                    ticks: {
                        beginAtZero: true
                    }
                }]
            }
        }
    }
    else {
        var options = {
            title: {
                display: true,
                text: title
            }
        }
    }
    new Chart(ctx, {
        type: chartType,
        data: {
            labels: dataLabels,
            datasets: [{
                label: labelsTitle,
                data: dataValues,
                backgroundColor: backgroundColor == null ? "rgba(153, 102, 255, 0.2)" : backgroundColor,
                borderColor: borderColor == null ? "rgba(153, 102, 255, 1)" : borderColor,
                borderWidth: 1,
            }],
        },
        options: options
    });
}

var randomColors = function (quantity) {
    var letters = "0123456789ABCDEF";
    var colors = [];
    for (var a = 0; a < quantity; a++) {
        var color = '#';
        for (var i = 0; i < 6; i++) {
            color += letters[(Math.floor(Math.random() * 16))];
        }
        colors.push(color)
    }
    return colors
}