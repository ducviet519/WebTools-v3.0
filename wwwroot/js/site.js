// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//showInPopup = (url, title) => {
//    $.ajax({
//        type: "GET",
//        url: url,
//        success: function (res) {
//            $("#form-modal .modal-body").html(res);
//            $("#form-modal .modal-title").html(title);
//            $("#form-modal").modal('show');
//        }
//    })
//}

//PopUp cho <a>
$(function () {
    var ReportPopupElement = $('#ReportPopup')
    $('a[data-toggle="ajax-modal"]').click(function (event) {
        var url = $(this).data('url');
        $.get(url).done(function (data) {
            ReportPopupElement.html(data);
            ReportPopupElement.find('.modal').modal('show');
        })
    })
    ReportPopupElement.on('click', '[data-save="modal"]', function (event) {
        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');
        var sendData = form.serialize();
        $.post(actionUrl, sendData).done(function (data) {
            ReportPopupElement.find('.modal').modal('hide');
            location.reload();
        })
    })
});