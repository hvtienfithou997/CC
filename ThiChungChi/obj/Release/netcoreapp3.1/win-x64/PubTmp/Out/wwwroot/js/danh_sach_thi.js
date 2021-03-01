function taiKhoanChecked(id) {
    
    let me = $(`input#${id}`);
    if (!me.is(":checked")) {
        $("ul.list_account_checked").find(`li[id="item_${me.val()}"]`).remove();
    }
    else {
        let name = $(`#${id}`).attr('data-name');
        let username = $(`#${id}`).attr('data-user');
        let input = `<input class='form-control d-none' readonly name='list_tai_khoan_need' value='${username}'>`;
        let row = `<li class='list-group-item' id="item_${username}"> ${input} ${name} (${username})</li>`;
        $("ul.list_account_checked").append(row);
    }

    var num = $("#choosed ul.list_account_checked li").length;
    $("#total").html(`<code>(${num})</code>`);
}



