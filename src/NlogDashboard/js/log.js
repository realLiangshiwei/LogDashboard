function loadList(page, pageSize) {
    var params = {
        Page: page,
        pageSize: pageSize

    };
    var loading = $("#LogList").setLoading();
    $.ajax({
        method: "post",
        url: mapPath + "/Dashboard/Searchlog",
        data: JSON.stringify(params)

    }).done(function (data) {
        loading.stop();
        $("#LogList").html(data.Html);
    });
}

var searchInput = {
    Page: 1,
    PageSize: 10,
    All: true
};

$(function () {
    $("#searchBtn").click(function (e) {
        e.preventDefault();
        search();
    });
});

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
        data: JSON.stringify(searchInput)

    }).done(function (data) {
        loading.stop();
        $("#LogList").html(data.Html);
        $("#page").html(data.Page);
    });
}

function goPage(page) {
    searchInput.Page = page;
    doSearch();
}

function showException(id) {
    $.ajax({
        method: "post",
        url: mapPath + "/Dashboard/GetException",
        data: JSON.stringify({ id: id })
    }).done(function (data) {

        $("#LogList").append('<div class="modal fade" id="' + id + '"> ' +
            '<div class= "modal-dialog modal-lg" role = "document">' +
            '<div class="modal-content">' +
            '<div class="modal-header bg-danger border-0">' +
            '<h5 class="modal-title text-white">Exception</h5>' +
            '<button type="button" class="close" data-dismiss="modal" aria-label="Close">' +
            '<span aria-hidden="true">×</span>' +
            '</button>' +
            '</div>' +
            '<div class="modal-body"><pre>' +
            '' + data + '</pre></div>' +
            '</div>' +
            '</div >' +
            '</div >');

        $('#' + id).modal();
    });
}

function logInfo(id) {
    $.ajax({
        method: "post",
        url: mapPath + "/Dashboard/LogInfo",
        data: JSON.stringify({ id: id })
    }).done(function (data) {
        $("#logInfoBody").html(data);
        $("#logInfoModal").modal();
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
