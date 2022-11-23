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

$.fn.callDataTable_Input_Checkbox = function (id, columnData, url, pageLength) {
    var table = $(id).DataTable();
    if ($.fn.dataTable.isDataTable(id)) {
        table.destroy();
        $(id).find('thead .filters').remove();
        $(id).find('tbody').empty();
    }

    $(id + ' thead tr')
        .clone(true)
        .addClass('filters')
        .appendTo(id + ' thead');

    //$(id + 'thead th:eq(0)').empty()
    var table2 = $(id).DataTable({
        "pageLength": pageLength,
        "processing": true,
        "lengthChange": false,
        "autoWidth": true,
        "responsive": true,
        "orderCellsTop": true,
        "fixedHeader": true,
        "order": [[2, 'asc']],
        "ajax": {
            "url": url,
            "type": "GET",
            "datatype": "json"
        },
        "columns": columnData,
        "columnDefs": [
            { "orderable": false, "targets": [0, 1, 8] },
            { "className": "text-wrap", "targets": "_all" },
            { "defaultContent": '', "targets": "_all" },
            { "targets": 0, "checkboxes": { "selectRow": true } }
        ],
        "select": {
            "style": "multi",
            //"selector": 'td:first-child'
            "selector": 'td:not(:last-child)'           
        },
        "initComplete": function () {
            var api = this.api();

            // For each column
            api
                .columns()
                .eq(0)
                .each(function (colIdx) {
                    // Set the header cell to contain the input element
                    if (colIdx != 8 && colIdx != 0) {
                        var cell = $('.filters th').eq(
                            $(api.column(colIdx).header()).index()
                        );
                        var title = $(cell).text();
                        if (colIdx == 1) { $(cell).html('<input type="hidden">'); }
                        else { $(cell).html('<input type="text" class="form-control col" placeholder="' + title + '" />'); }

                    }
                    // On every keypress in this input
                    if (colIdx != 0) {
                        $(
                            'input',
                            $('.filters th').eq($(api.column(colIdx).header()).index())
                        )
                            .off('keyup change')
                            .on('change', function (e) {
                                // Get the search value
                                $(this).attr('title', $(this).val());
                                var regexr = '({search})'; //$(this).parents('th').find('select').val();

                                var cursorPosition = this.selectionStart;
                                // Search the column for that value
                                api
                                    .column(colIdx)
                                    .search(
                                        this.value != ''
                                            ? regexr.replace('{search}', '(((' + this.value + ')))')
                                            : '',
                                        this.value != '',
                                        this.value == ''
                                    )
                                    .draw();
                            })
                            .on('keyup', function (e) {
                                e.stopPropagation();
                                $(this).trigger('change');
                                $(this)
                                    .focus()[0]
                                    .setSelectionRange(cursorPosition, cursorPosition);
                            });
                    }
                });
        },
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
            },
            "select": {
                "rows": {
                    _: "Đã chọn %d dòng",
                    0: "Click vào dòng để chọn",
                    1: "Đã chọn 1 dòng"
                }
            }
        },
    });
    return table2;
}

function FormatCurrencyVND(sotien, style, type, digits) {
    /*style
    "decimal" 100.000.000,15.
    "currency" 100.000.000,15 VND.
    "percent" 10.000.000.015,00%.

    type
    "symbol: 100.000.000 ₫"
    "code: 100.000.000 VND"
    "name: 100.000.000 Đồng Việt Nam"*/
    if (digits === null || digits === "") {
        var CurrencyVND = new Intl.NumberFormat("vi-VN", {
            style: style,
            currency: "VND",
            currencyDisplay: type,
        }).format(sotien);
    }
    else {
        var CurrencyVND = new Intl.NumberFormat("vi-VN", {
            style: style,
            currency: "VND",
            currencyDisplay: type,
            minimumFractionDigits: digits
        }).format(sotien);
    }

    return CurrencyVND;
}

function FormatDatetime(datetime, type) {

    if (datetime == null || datetime == "") { return datetime = ""; }
    else {
        if (type == "1") { var responseDate = moment(datetime).format('DD/MM/YYYY'); }
        else if (type == "2") { var responseDate = moment(datetime).format('DD/MM/YYYY HH:mm'); }
        return responseDate;
    }
}

$.fn.clearData = function ($form) {
    $form.find('input:text, input:password, input:file, select, textarea').val('');
    $form.find('input:radio, input:checkbox')
        .removeAttr('checked').removeAttr('selected');
}

$.fn.callModal = function (url) {
    $($.fn.dataTable.tables(true)).DataTable()
        .columns.adjust()
        .responsive.recalc()
        .scroller.measure();
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

$.fn.callDataTable = function (id, columnData, url, pageLength, disableColumn) {
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
        "pageLength": pageLength,
        "searching": true,
        "processing": true,
        "ordering": true,
        "info": true,
        "autoWidth": true,
        "responsive": true,
        "ajax": {
            "url": url,
            "type": "GET",
            "datatype": "json"
        },
        "columns": columnData,
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

$.fn.callDataTableScroll = function (id, columnData, url) {
    var table = $(id).DataTable();
    if ($.fn.dataTable.isDataTable(id)) {
        table.destroy();
        $(id).find('tbody').empty();
    }

    var table = $(id).DataTable({
        "processing": true,
        "autoWidth": true,
        "responsive": true,
        "paging": false,
        "lengthChange": false,
        "searching": false,
        "info": true,
        "ordering": false,
        "fixedHeader": true,
        "pageLength": 300,
        "scrollY": 480,
        "scrollX": true,
        "scrollCollapse": true,
        "deferRender": true,
        "ajax": {
            "url": url,
            "type": "GET",
            "datatype": "json"
        },
        "columns": columnData,
        "columnDefs": [
            { className: "text-wrap", targets: "_all" },
            { defaultContent: '', targets: "_all" },
        ],
        "select": {
            "style": 'multi',
            "selector": 'td:first-child'
        },
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
            },
            "select": {
                "rows": {
                    _: "Đã chọn %d dòng",
                    0: "",
                    1: "Đã chọn 1 dòng"
                }
            }
        },
    });
    return table;
}

$.fn.callDataTableScroll_Json = function (id, columnData, data) {
    var table = $(id).DataTable();
    if ($.fn.dataTable.isDataTable(id)) {
        table.destroy();
        $(id).find('tbody').empty();
    }

    var table = $(id).DataTable({
        "processing": true,
        "autoWidth": true,
        "responsive": true,
        "lengthChange": false,
        "searching": false,
        "info": true,
        "ordering": false,
        "paging": false,
        "fixedHeader": true,
        "pageLength": 300,
        "scrollY": 480,
        "scrollX": true,
        "scrollCollapse": true,
        "deferRender": true,
        "data": data,
        "columns": columnData,
        "columnDefs": [
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











