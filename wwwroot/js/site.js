// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function searchDataTable(id, columnData, url, disableColumn) {
    var array = [];
    $.each(disableColumn.split(','), function (idx, val) {
        array.push(parseInt(val));
    });
    if (disableColumn == '') { disableColumn = 0; }
    var table = $(id).DataTable();
    if ($.fn.dataTable.isDataTable(id)) {
        table.destroy();
        $(id).find('tbody').empty();
    }
    var table = $(id).DataTable({
        "paging": true,
        "lengthChange": false,
        "searching": true,
        "processing": true,
        "ordering": true,
        "info": true,
        "autoWidth": true,
        "responsive": false,
        "order": [[0, 'asc']],
        "columnDefs": [
            { orderable: false, targets: array },
            { className: "text-wrap", targets: "_all" },
            { defaultContent: '', targets: "_all" },
        ],
        "ajax": {
            "url": url,
            "type": "GET",
            "datatype": "json"
        },
        "columns": columnData,
        "language": {
            "sProcessing": "Đang tải dữ liệu...",
            "sLengthMenu": "Xem _MENU_ mục",
            "sZeroRecords": "Không tìm thấy dòng nào phù hợp",
            "sInfo": "Đang xem _START_ đến _END_ trong tổng số _TOTAL_ mục",
            "sInfoEmpty": "Đang xem 0 đến 0 trong tổng số 0 mục",
            "sInfoFiltered": "(được lọc từ _MAX_ mục)",
            "sInfoPostFix": "",
            "sSearch": "Tìm:",
            "sUrl": "",
            "oPaginate": {
                "sFirst": "Đầu",
                "sPrevious": "Trước",
                "sNext": "Tiếp",
                "sLast": "Cuối"
            }
        },
    });
    return table;
}

    //ReportPopupElement.on('click', '[data-save="modal"]', function (event) {
    //    event.preventDefault();
    //    var form = $(this).parents('.modal').find('form');
    //    var actionUrl = form.attr('action');
    //    var methodType = form.attr('method');

    //    console.log(form.serialize());
    //    $.ajax({
    //        type: methodType,
    //        url: actionUrl,
    //        data: form.serialize(),
    //        success: function (data) {
    //            alert("Thành công!");
    //        },
    //        error: function (xhr, desc, err) {
    //            alert("Lỗi!");
    //        }
    //    }).done(function (data) {
    //        ReportPopupElement.find('.modal').modal('hide');
    //        location.reload();
    //    })
    //});

$(function () {
    toastr.options = {
        "closeButton": false,
        "debug": false,
        "newestOnTop": false,
        "progressBar": true,
        "positionClass": "toast-top-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }
});

$.fn.clearData = function ($form) {
    $form.find('input:text, input:password, input:file, select, textarea').val('');
    $form.find('input:radio, input:checkbox')
        .removeAttr('checked').removeAttr('selected');
}

$.fn.callModal = function (url) {

    var ReportPopupElement = $('#ReportPopup');
    $.ajax({
        url: url,
        dataType: 'html',
        success: function (data) {
            $("body").find(".modal-backdrop").remove();
            ReportPopupElement.html(data);
            ReportPopupElement.find('.modal').modal('show');
        }, error: function (xhr, status) {
            switch (status) {
                case 404:
                    $(this).callToast("error", 'Lỗi!', 'Đường dẫn không đúng hoặc tính năng không tồn tại!');
                    break;
                case 500:
                    $(this).callToast("error", 'Lỗi!', 'Không kết nối được tới Server!');
                    break;
                case 0:
                    $(this).callToast("error", 'Lỗi!', 'Hệ thống không phản hồi!');
                    break;
                default:
                    $(this).callToast("error", 'Lỗi!', 'Sự cố không xác định! Lỗi: ' + status);
            }
        }
    });
}

$.fn.callToast = function (status, title, msg) {
    toastr.options = {
        "closeButton": false,
        "debug": true,
        "newestOnTop": false,
        "progressBar": true,
        "positionClass": "toast-top-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }
    if (status == "success") {
        toastr.success(msg, title)
    }
    else if (status == "info") {
        toastr.info(msg, title)
    }
    else if (status == "warning") {
        toastr.warning(msg, title)
    }
    else if (status == "error") {
        toastr.error(msg, title)
    }
}

$.fn.callDataTable = function (disableColumn, pageLength) {
    var array = [];
    $.each(disableColumn.split(','), function (idx, val) {
        array.push(parseInt(val));
    });
    if (disableColumn == '') { disableColumn = 0; }

    var table = $(this).DataTable({
        "paging": true,
        "lengthChange": false,
        "pageLength": pageLength,
        "searching": true,
        "processing": true,
        "ordering": true,
        "info": true,
        "autoWidth": true,
        "responsive": true,
        "order": [[0, 'asc']],
        "columnDefs": [
            { orderable: false, targets: array },
            { className: "text-wrap", targets: "_all" },
            { defaultContent: '', targets: "_all" },
        ],
        "language": {
            "sProcessing": "Đang tải dữ liệu...",
            "sLengthMenu": "Xem _MENU_ mục",
            "sZeroRecords": "Không tìm thấy dòng nào phù hợp",
            "sInfo": "Đang xem _START_ đến _END_ trong tổng số _TOTAL_ mục",
            "sInfoEmpty": "Đang xem 0 đến 0 trong tổng số 0 mục",
            "sInfoFiltered": "(được lọc từ _MAX_ mục)",
            "sInfoPostFix": "",
            "sSearch": "Tìm:",
            "sUrl": "",
            "oPaginate": {
                "sFirst": "Đầu",
                "sPrevious": "Trước",
                "sNext": "Tiếp",
                "sLast": "Cuối"
            }
        },
    });
    return table;
}








