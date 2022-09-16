(function ($) {
    $.fn.serializeFiles = function () {
        var form = $(this),
            formData = new FormData()
        formParams = form.serializeArray();

        $.each(form.find('input[type="file"]'), function (i, tag) {
            $.each($(tag)[0].files, function (i, file) {
                formData.append(tag.name, file);
            });
        });

        $.each(formParams, function (i, val) {
            formData.append(val.name, val.value);
        });

        return formData;
    };
})(jQuery);