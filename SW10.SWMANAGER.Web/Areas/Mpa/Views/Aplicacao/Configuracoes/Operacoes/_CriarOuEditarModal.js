(function ($) {
    app.modals.CriarOuEditarOperacaoModal = function () {

        var _operacoesService = abp.services.app.operacao;

        var _modalManager;
        var _$operacaoInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$operacaoInformationForm = _modalManager.getModal().find('form[name=OperacaoInformationsForm]');
            _$operacaoInformationForm.validate({ ignore: "" });
            $('.modal-dialog').css({ 'width': '90%', 'max-width': '1100px' });
            $('div.form-group select').addClass('form-control selectpicker');
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
        };

        this.save = function () {
            if (!_$operacaoInformationForm.valid()) {
                return;
            }

            var operacao = _$operacaoInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);

            _operacoesService.criarOuEditar(operacao)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarOperacaoModalSaved');
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };

        $("#pagina-id").select2({
            allowClear: true,
            placeholder: app.localize("SelecioneLista"),
            ajax: {
                url: "/api/services/app/operacao/ListarPermissoesDropdown",
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
                        totalPorPagina: 10
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
        }).on('change', function (e) {
            e.preventDefault();
            var data=$("#pagina-id").select2('data');
            $('#name').val(data[0].text);
        });

    };
})(jQuery);