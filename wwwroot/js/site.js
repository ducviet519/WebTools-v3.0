// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function () {
    //Initialize Select2 Elements
    $('.select2').select2()

})
//Table
$(function () {
    $('#tableReport').DataTable({
        "paging": true,
        "lengthChange": false,
        "searching": false,
        "ordering": true,
        "info": true,
        "autoWidth": false,
        "responsive": true,
    });
});

$(function () {
    //Datemask dd/mm/yyyy
    $('#datemask').inputmask('dd/mm/yyyy', { 'placeholder': 'dd/mm/yyyy' })
    $('[data-mask]').inputmask()

    //Date picker
    $('#reservationdate').datetimepicker({
        format: 'DD/MM/YYYY'
    });
});

//File upload show name file
$(function () {
    bsCustomFileInput.init();
});

//PopUp cho <a>
$(function () {
    var ReportPopupElement = $('#ReportPopup');
    $('a[data-toggle="ajax-modal"]').click(function (event) {
        var url = $(this).data('url');
        $.get(url).done(function (data) {
            ReportPopupElement.html(data);
            ReportPopupElement.find('.modal').modal('show');
        });
    });

    ReportPopupElement.on('click', '[data-save="modal"]', function (event) {

        if ($("#myForm").valid()) { //check if form is valid using model annotation
            $('#myForm').submit();
        }
        else {
            return false;
        }
    });
    $("#myForm").on("submit", function (event) {
        event.preventDefault();
        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');
        var methodType = form.attr('method');

        console.log(form.serialize());
        $.ajax({
            type: methodType,
            url: actionUrl,
            data: form.serialize(),
            success: function (data) {
                alert("Thành công!");
            },
            error: function (xhr, desc, err) {
                alert("Lỗi!");
            }
        }).done(function (data) {
            ReportPopupElement.find('.modal').modal('hide');
            location.reload();
        })
    });


});
