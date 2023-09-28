(function ($) {
    app.modals.CriarOuEditarTipoAtendimentoModal = function () {

        var _TiposAtendimentoService = abp.services.app.tipoAtendimento;

        var _modalManager;
        var _$TipoAtendimentoInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$TipoAtendimentoInformationForm = _modalManager.getModal().find('form[name=TipoAtendimentoInformationsForm]');
            _$TipoAtendimentoInformationForm.validate();
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };

            $('.select2').css('width', '100%');
        };

        this.save = function () {
            if (!_$TipoAtendimentoInformationForm.valid()) {
                return;
            }

            var tipoAtendimento = _$TipoAtendimentoInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _TiposAtendimentoService.criarOuEditar(tipoAtendimento)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarTipoAtendimentoModalSaved');
                     //location.reload();//seguindo o projeto pronto
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
        $('input[name="TipoAtendimento"]').on('change', function (e) {
            e.stopPropagation();
            if ($(this).attr('id') == 'rdo-is-internacao') {
                $('#is-ambulatorio-emergencia').val('false');
                $('#is-internacao').val('true');
            }
            else {
                $('#is-ambulatorio-emergencia').val('true');
                $('#is-internacao').val('false');
            }
        });

        $('#tabela-dominio-id').select2({
            allowClear: true,
            placeholder: app.localize("SelecioneLista"),
            ajax: {
                url: '/api/services/app/tabelaDominio/listarPorTipoAtendimentoDropdown',
                dataType: 'json',
                delay: 250,
                method: 'Post',

                data: function (params) {
                    //   //console.log('data: ', params, (params.page == undefined));
                    if (params.page == undefined)
                        params.page = '1';
                    //   //console.log('data: ', params);

                    return {
                        search: params.term,
                        page: params.page,
                        totalPorPagina: 10,
                        filtros: [$('#is-ambulatorio-emergencia').val(), $('#is-internacao').val()]
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
    };
})(jQuery);