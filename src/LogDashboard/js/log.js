moment.locale('zh-cn',
    {
        months: '一月_二月_三月_四月_五月_六月_七月_八月_九月_十月_十一月_十二月'.split('_'),
        monthsShort: '1月_2月_3月_4月_5月_6月_7月_8月_9月_10月_11月_12月'.split('_'),
        weekdays: '星期日_星期一_星期二_星期三_星期四_星期五_星期六'.split('_'),
        weekdaysShort: '周日_周一_周二_周三_周四_周五_周六'.split('_'),
        weekdaysMin: '日_一_二_三_四_五_六'.split('_'),
        longDateFormat: {
            LT: 'HH:mm',
            LTS: 'HH:mm:ss',
            L: 'YYYY-MM-DD',
            LL: 'YYYY年MM月DD日',
            LLL: 'YYYY年MM月DD日Ah点mm分',
            LLLL: 'YYYY年MM月DD日ddddAh点mm分',
            l: 'YYYY-M-D',
            ll: 'YYYY年M月D日',
            lll: 'YYYY年M月D日 HH:mm',
            llll: 'YYYY年M月D日dddd HH:mm'
        },
        meridiemParse: /凌晨|早上|上午|中午|下午|晚上/,
        meridiemHour: function (hour, meridiem) {
            if (hour === 12) {
                hour = 0;
            }
            if (meridiem === '凌晨' ||
                meridiem === '早上' ||
                meridiem === '上午') {
                return hour;
            } else if (meridiem === '下午' || meridiem === '晚上') {
                return hour + 12;
            } else {
                // '中午'
                return hour >= 11 ? hour : hour + 12;
            }
        },
        meridiem: function (hour, minute, isLower) {
            const hm = hour * 100 + minute;
            if (hm < 600) {
                return '凌晨';
            } else if (hm < 900) {
                return '早上';
            } else if (hm < 1130) {
                return '上午';
            } else if (hm < 1230) {
                return '中午';
            } else if (hm < 1800) {
                return '下午';
            } else {
                return '晚上';
            }
        },
        calendar: {
            sameDay: '[今天]LT',
            nextDay: '[明天]LT',
            nextWeek: '[下]ddddLT',
            lastDay: '[昨天]LT',
            lastWeek: '[上]ddddLT',
            sameElse: 'L'
        },
        dayOfMonthOrdinalParse: /\d{1,2}(日|月|周)/,
        ordinal: function (number, period) {
            switch (period) {
                case 'd':
                case 'D':
                case 'DDD':
                    return number + '日';
                case 'M':
                    return number + '月';
                case 'w':
                case 'W':
                    return number + '周';
                default:
                    return number;
            }
        },
        relativeTime: {
            future: '%s内',
            past: '%s前',
            s: '几秒',
            ss: '%d秒',
            m: '1分钟',
            mm: '%d分钟',
            h: '1小时',
            hh: '%d小时',
            d: '1天',
            dd: '%d天',
            M: '1个月',
            MM: '%d个月',
            y: '1年',
            yy: '%d年'
        },
        week: {
            // GB/T 7408-1994《数据元和交换格式·信息交换·日期和时间表示法》与ISO 8601:1988等效
            dow: 1, // Monday is the first day of the week.
            doy: 4 // The week that contains Jan 4th is the first week of the year.
        }
    });


var searchInput = {
    Page: 1,
    PageSize: 10,
    All: true
};
var mapPath;
$(function () {
    mapPath = $($("#mapPath")[0]).attr("href");
    $("#searchBtn").click(function (e) {
        e.preventDefault();
        search();
    });

    /**
    * Sidebar Dropdown
    */
    $('.nav-dropdown-toggle').on('click', function (e) {
        e.preventDefault();
        $(this).parent().toggleClass('open');
    });

    // open sub-menu when an item is active.
    $('ul.nav').find('a.active').parent().parent().parent().addClass('open');

    /**
     * Sidebar Toggle
     */
    $('.sidebar-toggle').on('click', function (e) {
        e.preventDefault();
        $('body').toggleClass('sidebar-hidden');
    });

    /**
     * Mobile Sidebar Toggle
     */
    $('.sidebar-mobile-toggle').on('click', function () {
        $('body').toggleClass('sidebar-mobile-show');
    });
});


(function ($) {
    $.fn.setLoading = function () {
        var opt = {
            lines: 9, // The number of lines to draw
            length: 0, // The length of each line
            width: 10, // The line thickness
            radius: 15, // The radius of the inner circle
            corners: 1, // Corner roundness (0..1)
            rotate: 0, // The rotation offset
            color: '#000', // #rgb or #rrggbb
            speed: 1, // Rounds per second
            trail: 60, // Afterglow percentage
            shadow: false, // Whether to render a shadow
            hwaccel: false, // Whether to use hardware acceleration
            className: 'spinner', // The CSS class to assign to the spinner
            zIndex: 2e9, // The z-index (defaults to 2000000000)
            top: 'auto', // Top position relative to parent
            left: 'auto' // Left position relative to parent in px
        };

        var spinner = new Spinner(opt).spin(this[0]);
        return spinner;
    };
})(jQuery);



function loadList(page, pageSize) {
    var params = {
        Page: page,
        pageSize: pageSize

    };
    var loading = $("#LogList").setLoading();
    $.ajax({
        method: "post",
        url: mapPath + "/Dashboard/Searchlog",
        contentType: "application/json",
        data: JSON.stringify(params)

    }).done(function (data) {
        loading.stop();
        $("#LogList").html(data.Html);
    });
}

function search() {
    searchInput.Page = 1;
    searchInput.All = $("#all").is(":checked");
    searchInput.Unique = $("#unique").is(":checked");
    searchInput.ToDay = $("#today").is(":checked");
    searchInput.Hour = $("#hour").is(":checked");
    searchInput.Message = $("#Message").val();
    searchInput.Level = $("#Level").val();
    searchInput.StartTime = $("#StartTime").val();
    searchInput.EndTime = $("#EndTime").val();

    doSearch();
    return false;
}

function doSearch() {
    var loading = $("#LogList").setLoading();
    $.ajax({
        method: "post",
        url: mapPath + "/Dashboard/Searchlog",
        contentType: "application/json",
        data: JSON.stringify(searchInput)

    }).done(function (data) {
        loading.stop();
        $("#LogList").html(data.Html);
        $("#page").html(data.Page);
    });
}

function requestTrace(id) {
    $.ajax({
        method: "post",
        url: mapPath + "/Dashboard/RequestTrace",
        contentType: "application/json",
        data: JSON.stringify({ id: id })

    }).done(function (data) {
        $("#traceLogList").html(data);
        tableExpandable();
    });
}

function goPage(page) {
    searchInput.Page = page;
    doSearch();
}


function logInfo(id, modalId, bodyId) {

    $.ajax({
        method: "post",
        url: mapPath + "/Dashboard/LogInfo?id="+id,
        //contentType: "application/json",
        data: "{}"
    }).done(function (html) {
        $("#" + bodyId).html(html);
        $("#" + modalId).modal();

    });
}

function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", 'i');
    var result = window.location.search.substr(1).match(reg);
    if (result != null) {
        return decodeURIComponent(result[2]);
    } else {
        return null;
    }
}





function tableExpandable() {
    $('.table-expandable').each(function () {
        var table = $(this);
        table.children('thead').children('tr').append('<th class="col-1"></th>');
        table.children('tbody').children('tr').filter(':odd').hide();
        table.children('tbody').children('tr').filter(':even').click(function () {
            var element = $(this);
            element.next('tr').toggle('slow');
            element.find(".table-expandable-arrow").toggleClass("up");
        });
        table.children('tbody').children('tr').filter(':even').each(function () {
            var element = $(this);
            element.append('<td class="col-1">展开</td>');
        });
    });
}
