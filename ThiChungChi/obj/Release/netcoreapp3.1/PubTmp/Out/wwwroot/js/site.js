var API_URL = "https://localhost:44331/";
$(function() {
    $("#alert-box").removeClass('hide');
    $("#alert-box").delay(3000).slideUp(500);
    setInterval(show_expired, 2000);
});
function show_expired() {
    $('#show_expired').modal('hide');
}
$('.datepicker').datepicker({
    language: 'vi'
});
function Message(msg) {
    if (msg === "success") {
        $.notify("Thao tác thành công", `${msg}`);
    } else {
        $.notify("Thao tác thất bại", `${msg}`);
    }
}

function epochToTime(str) {
    if (str === 0)
        return "";
    var date = new Date(str * 1000),
        month = ("0" + (date.getMonth() + 1)).slice(-2),
        day = ("0" + date.getDate()).slice(-2);
    return [day, month, date.getFullYear()].join("/");
}