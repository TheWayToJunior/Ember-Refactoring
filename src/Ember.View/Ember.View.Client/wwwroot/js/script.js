$(window).scroll(function () {
    if ($(this).scrollTop() > $("div.intro").height() - 50) {
        $('#header').addClass('header-background');
    } else {
        $('#header').removeClass('header-background');
    }
});

window.scrollToElementId = (elementId) => {
    var element = document.getElementById(elementId);
    if (!element) {
        console.warn('element was not found', elementId);
        return false;
    }

    element.scrollIntoView({ block: "start", behavior: "smooth" });
    return true;
}
window.scrollLineOn = (dotnetObject, elementId) => {
    var element = document.getElementById(elementId);
    $(window).on('scroll.line', function () {
        if ($(window).scrollTop() >= element.offsetTop / 2) {
            dotnetObject.invokeMethodAsync('SetMaxHeight', $(element).height());
            if ($(window).width() > 770) {
                dotnetObject.invokeMethodAsync('SetHeight', $(window).scrollTop() - element.offsetTop / 2);
                return;
            }
        }
        dotnetObject.invokeMethodAsync('SetHeight', 0);
    });
}

window.scrollLineOff = function (){
    $(window).off('scroll.line');
}

window.initSlick = function () {
    $('.center').slick({
        centerMode: true,
        centerPadding: '60px',
        slidesToShow: 3,
        autoplay: true,
        responsive: [
            {
                breakpoint: 1256,
                settings: {
                    arrows: false,
                    centerMode: true,
                    centerPadding: '40px',
                    slidesToShow: 2
                }
            },
            {
                breakpoint: 768,
                settings: {
                    arrows: false,
                    centerMode: true,
                    centerPadding: '40px',
                    slidesToShow: 1
                }
            },
        ]
    });
};

window.initCounter = function () {
    $('.counter').counterUp({
        delay: 10,
        time: 1000,
        triggerOnce: true
    });
}

window.initMap = function () {
    var map = new google.maps.Map(
        document.getElementById('map'),{
            zoom: 14, center: { lat: 48.30627, lng: 38.0077283 }
    });

    var marker = new google.maps.Marker({
        position: { lat: 48.306820, lng: 38.028227 },
        map: map
    });
}

window.initPieChart = function (roleNames, roleCounts) {
    $('#pieChart').remove();
    $('#graph-container').append('<canvas id="pieChart"><canvas>');

    var ctxP = document.getElementById("pieChart").getContext('2d');
    var myPieChart = new Chart(ctxP, {
        type: 'pie',
        data: {
            labels: roleNames,
            datasets: [{
                data: roleCounts,
                backgroundColor: ["#F7464A", "#46BFBD", "#FDB45C", "#95dd7d"],
                hoverBackgroundColor: ["#FF5A5E", "#5AD3D1", "#FFC870", "#95dd7d"]
            }]
        },
        options: {
            responsive: true
        }
    });
}

window.initChartBar = function (weekday) {
    $('#easionChartjsBar').remove();
    $('#graph-container').append('<canvas id="easionChartjsBar"><canvas>');

    var ctx = document.getElementById("easionChartjsBar").getContext('2d');
    var myChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"],
            datasets: [{
                label: 'Blue',
                data: weekday,
                backgroundColor: window.chartColors.primary,
                borderColor: 'transparent'
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