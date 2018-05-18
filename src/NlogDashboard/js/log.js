function loadList(page, pageSize) {
    var params = {
        Page: page,
        pageSize: pageSize

    }
    $.ajax({
        method: "post",
        url: mapPath + "/DashboardHandle/Searchlog",
        data: JSON.stringify(params),
        dataType: "text/html",
        success: function (data) {
            $("#tobdy").html(data);
        }
    });
}