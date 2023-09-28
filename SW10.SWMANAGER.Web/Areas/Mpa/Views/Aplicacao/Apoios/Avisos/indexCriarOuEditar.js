//const moment = require("moment");


(function ($) {
    $('body').addClass('page-sidebar-closed');
    $('.page-sidebar-menu').addClass('page-sidebar-menu-closed');
    const avisoService = abp.services.app.avisos;
    const formAviso = $("#form-aviso");
    const summerNoteOptions = {
        toolbar: [
            ['printSize', ['printSize']],
            ['style', ['bold', 'italic', 'underline']],
            ['fontsize', ['fontsize']],
            ['fontname', ['fontname']],
            ['font', ['font', 'strikethrough', 'superscript', 'subscript']],
            ['color', ['color']],
            ['para', ['ul', 'ol', 'paragraph']],
            ['height', ['height']],
            ['misc', ['codeview', 'fullscreen']],
            ['table', ['table']]
        ],
        width: '100%',
        height: 250,
        padding: 15,
        disableResizeEditor: true
    };

    const _selectedDataProgramada = {
        minDate: moment().startOf('day'),
        maxDate: moment().add('10', 'year').endOf('day'),
        autoUpdateInput: false,
        startDate: undefined,
        endDate: undefined,
        "singleDatePicker": true,
        "timePicker": true,
        locale: {
            format: 'DD/MM/YYYY hh:mm A'
        },
        ranges: undefined,
        "momentFormatStart": "DD/MM/YYYY HH:mm:ss",
        "momentFormatEnd": "DD/MM/YYYY HH:mm:ss"
    };
    
    $.summernote.options.lineHeights = ["0", "0.2", "0.4", "0.6", "0.8", "1.0"];
    $('.text-editor').summernote(summerNoteOptions);
    createDateRangePicker($('#date-field-programada'), _selectedDataProgramada);
    $('.button-submit').click(function () {
        const button = $(this);
        button.buttonBusy(true);

        let dataProgramada = null;
        if (_selectedDataProgramada.startDate && _selectedDataProgramada.startDate._isAMomentObject) {
            dataProgramada = _selectedDataProgramada.startDate.format('YYYY-MM-DDTHH:mm:ssZ');
        }
        else {
            dataProgramada = _selectedDataProgramada.startDate;
        }
        let mensagem = $(".text-editor").summernote('code');

        // if (_.startsWith(mensagem, "<")) {
        //     mensagem = $($(".text-editor").summernote('code')).text();
        // }
        
        let grupos = $(".roles:checked").map((index,item)=> {
            return { AvisoId: $("avisoId").val(), RoleId: $(item).data("id") }
        }).get()
        
        let data = {id: $("#avisoId").val(), titulo: $("#titulo").val(), dataProgramada, mensagem, grupos }

        avisoService.criarOuEditar(data).then(() => {
            abp.notify.success(`Aviso salvo com sucesso`);
            button.buttonBusy(false);
            abp.event.trigger('app.CriarOuEditarAvisosModalSaved');
            window.location = '/Mpa/Avisos/'
        }).always(function () { $(this).buttonBusy(false); });
    })

    function createDateRangePicker(inputTag, selectedDateRange) {
        let baseOptions = app.createDateRangePickerOptions();
        baseOptions.ranges = undefined;
        let options = $.extend(true, baseOptions, selectedDateRange);
        $(inputTag).daterangepicker(options).on('apply.daterangepicker', function (ev, picker) {
            if (!options["singleDatePicker"]) {
                $(this).val(picker.startDate.format(options["momentFormatStart"]) + ' - ' + picker.endDate.format(options["momentFormatEnd"]));
                selectedDateRange.startDate = picker.startDate.format('YYYY-MM-DDT00:00:00Z');
                selectedDateRange.endDate = picker.endDate.format('YYYY-MM-DDT23:59:59.999Z');
                return;
            }
            
            $(this).val(picker.startDate.format(options["momentFormatStart"]));
            if (options["timePicker"]) {
                selectedDateRange.startDate = picker.startDate.format('YYYY-MM-DDTHH:mm:ssZ');
            }
            else {
                selectedDateRange.startDate = picker.startDate.format('YYYY-MM-DDT00:00:00Z');
            }
        }).on('cancel.daterangepicker', function (ev, picker) {
            $(this).val('');
        });
    }
    
})(jQuery);