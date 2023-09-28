(function ($) {
    app.modals.CriarOuEditarTipoAcomodacaoModal = function () {

        var _TiposAcomodacaoService = abp.services.app.tipoAcomodacao;

        var _modalManager;
        var _$TipoAcomodacaoInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };

            $('.select2').css('width', '100%');

            _$TipoAcomodacaoInformationForm = _modalManager.getModal().find('form[name=TipoAcomodacaoInformationsForm]');
            _$TipoAcomodacaoInformationForm.validate();
        };

        this.save = function () {
            if (!_$TipoAcomodacaoInformationForm.valid()) {
                return;
            }

            var tipoAcomodacao = _$TipoAcomodacaoInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _TiposAcomodacaoService.criarOuEditar(tipoAcomodacao)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarTipoAcomodacaoModalSaved');
                     //location.reload();//seguindo o projeto pronto
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };

        $('#tabela-dominio-id').select2({
            allowClear: true,
            placeholder: app.localize("SelecioneLista"),
            ajax: {
                url: '/api/services/app/tabelaDominio/listarDropdown',
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
                        tabelaDominioTiss: $('#tipo-tabela-dominio-id').val()
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