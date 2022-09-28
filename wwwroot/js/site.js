// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    // Setup - add a text input to each footer cell
    $('#tableReport thead tr')
        .clone(true)
        .addClass('filters')
        .appendTo('#tableReport thead');

    var table = $('#tableReport').DataTable({
        "paging": true,
        "lengthChange": false,
        "searching": true,
        "ordering": true,
        "info": true,
        "autoWidth": false,
        "responsive": true,
        "orderCellsTop": true,
        "fixedHeader": false,
        "columnDefs": [{ orderable: false, targets: 8 }],
        "order": [[4, 'desc']],
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
        "columnDefs": [
            { "searchable": false, "targets": 8 }
        ],
    });
});

//PopUp cho <a>
$(document).ready(function () {

    var ReportPopupElement = $('#ReportPopup');

    $('button[data-toggle="ajax-modal"]').click(function (event) {
        var url = $(this).data('url');
        $.get(url).done(function (data) {
            ReportPopupElement.html(data);
            ReportPopupElement.find('.modal').modal('show');
        });
    });

    $('body').on('click','a[data-toggle="ajax-modal"]',function (event) {
        var url = $(this).data('url');
        $.get(url).done(function (data) {
            ReportPopupElement.html(data);
            ReportPopupElement.find('.modal').modal('show');
        });
    });

    ReportPopupElement.on('click', '[data-save="modal"]', function (event) {
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

