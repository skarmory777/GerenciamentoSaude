(function ($) {
    app.modals.CriarOuEditarBIModal = function () {

        var _BIsService = abp.services.app.bi;

        var _modalManager;
        var _$BIInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
            _$BIInformationForm = _modalManager.getModal().find('form[name=BIInformationsForm]');
            _$BIInformationForm.validate();
            $('.select2').css('width', '100%');
        };

        this.save = function () {
            if (!_$BIInformationForm.valid()) {
                return;
            }

            var bi = _$BIInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _BIsService.criarOuEditar(bi)
                 .done(function () {
                     abp.notify.success(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarBIModalSaved');
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
        CamposRequeridos();
        aplicarDateSingle();
        aplicarDateRange();
        aplicarSelect2Padrao();

        $('#operacao-id').select2({
            ajax: {
                url: '/api/services/app/operacao/ListarPorModuloDropdown',
                dataType: 'json',
                delay: 250,
                method: 'Post',
                data: function (params) {
                    if (params.page == undefined)
                        params.page = '1';
                    return {
                        search: params.term,
                        page: params.page,
                        totalPorPagina: 10,
                        filtro: $('#modulo-id').val()
                    };
                },
                processResults: function (data, params) {
                    params.page = params.page || 1;

                    return {
                        results: data.result.items,
                        pagination: {
                            more: (params.page * 10) < data.result.totalCount
                        }
                    };
                },
                cache: true
            },
            escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
            minimumInputLength: 0
        })
    .on('change', function () {
        if ($(this).val() > 0) {
            $('#medico-especialidade-id').removeAttr('disabled');
        }
        else {
            $('#medico-especialidade-id').attr('disabled', 'disabled');
        }
    });

    };
})(jQuery);