$(document).ready(function() {

});

function getDataTyping() {
    $.ajax
    ({
        type: 'POST',
        url: API_URL + '',
        dataType: 'json',
        data: {
            term: term,
            page: page
        },
        success: function (data) {
            data.data.forEach(x => {
                var rows = "<div class='checkbox'>" +
                    "<label class='checkbox-bootstrap checkbox-lg max'>" +
                    `<span class='checkbox-placeholder'></span>${x.fullname} (${x.username})` +
                    `<input type='checkbox' data-user='${x.username}' data-name='${x.fullname}' id='${x.id
                    }' onclick="taiKhoanChecked(this.id)" name='tai_khoan_check' value='${x.id}'>` +
                    "</label>" +
                    "</div>";
                $('#data_tai_khoan').append(rows);
            });
            paging(data.total, 'getData', page, data.page_size);
        },
        error: function (ex) {
            alert("Message: " + ex);
        }
    });
    return false;
}