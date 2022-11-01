// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function searchDataTable(id, columnData, url, pageLength, disableColumn) {

    tableId = '#' + id;
    var array = [];
    $.each(disableColumn.split(','), function (idx, val) {
        array.push(parseInt(val));
    });
    if (disableColumn == '') { disableColumn = 0; }
    var table = $(tableId).DataTable();
    if ($.fn.dataTable.isDataTable(tableId)) {
        table.destroy();
        $(tableId).find('tbody').empty();
    }

    // Setup - add a text input to each footer cell
    $('#' + id + ' thead tr')
        .clone(true)
        .addClass('filters')
        .appendTo('#' + id + ' thead');

    var table = $(tableId).DataTable({
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
            { searchable: false, targets: 8 },
        ],
        "ajax": {
            "url": url,
            "type": "GET",
            "datatype": "json"
        },
        "columns": columnData,
        "initComplete": function () {
            var api = this.api();

            // For each column
            api
                .columns()
                .eq(0)
                .each(function (colIdx) {
                    // Set the header cell to contain the input element
                    if (colIdx != 8) {
                        var cell = $('.filters th').eq(
                            $(api.column(colIdx).header()).index()
                        );
                        var title = $(cell).text();
                        $(cell).html('<input type="text" class="form-control" placeholder="' + title + '" />');
                    }
                    // On every keypress in this input
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
                });
        },
    });
}


//PopUp cho <a>
$(document).ready(function () {

    var ReportPopupElement = $('#ReportPopup');

    $('button[data-toggle="ajax-modal"]').click(function (event) {
        $("body").find(".modal-backdrop").remove();
        var url = $(this).data('url');
        $.get(url).done(function (data) {
            ReportPopupElement.html(data);
            ReportPopupElement.find('.modal').modal('show');
        });
    });

    $('body').on('click', 'a[data-toggle="ajax-modal"]', function (event) {
        $("body").find(".modal-backdrop").remove();
        var url = $(this).data('url');
        $.get(url).done(function (data) {
            ReportPopupElement.html(data);
            ReportPopupElement.find('.modal').modal('show');
        });
    });

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
});

$(function () {
    //Initialize Select2 Elements
    $('.select2').select2()

    //Datemask dd/mm/yyyy
    $('#datemask').inputmask('dd/mm/yyyy', { 'placeholder': 'dd/mm/yyyy' })
    $('[data-mask]').inputmask()

    //Date picker
    $('#reservationdate').datetimepicker({
        format: 'DD/MM/YYYY'
    })

    //File upload show name file
    bsCustomFileInput.init()
});

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







