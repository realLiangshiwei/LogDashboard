$(function () {
    $('#StartTime').datetimepicker({
        locale: 'zh-cn',
        Default: 'yyyy-mm-dd hh:mm:ss'
    });
    $('#EndTime').datetimepicker({
        locale: 'zh-cn',
        Default: 'yyyy-mm-dd hh:mm:ss'
    });
    setCheckbox("All", "all");
    setCheckbox("Today", "today");
    setCheckbox("Unique", "unique");
    setCheckbox("Hour", "hour");
});

function setCheckbox(str, id) {
    var val = getQueryString(str);
    if (val) {
        $("#" + id).prop("checked", true);
        searchInput[str] = val;
    }
}