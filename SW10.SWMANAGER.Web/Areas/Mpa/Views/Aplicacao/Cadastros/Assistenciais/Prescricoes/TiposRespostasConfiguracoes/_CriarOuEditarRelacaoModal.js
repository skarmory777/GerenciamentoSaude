(function ($) {
    app.modals.CriarOuEditarTipoRespostaConfiguracaoElementoHtmlModal = function () {

        var _tipoRespostaConfiguracaoElementoHtmlService = abp.services.app.tipoRespostaConfiguracaoElementoHtml;
        var _modalManager;
        var _$formTipoRespostaConfiguracaoElementoHtmlService = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;
            _$formTipoRespostaConfiguracaoElementoHtmlService = _modalManager.getModal().find('form[name=TipoRespostaConfiguracaoElementoHtmlInformationsForm]');
            _$formTipoRespostaConfiguracaoElementoHtmlService.validate();
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
            $('.modal-dialog').css({ 'width': '90%', 'max-width': '990px' });
        };

        this.save = function () {
            if (!_$formTipoRespostaConfiguracaoElementoHtmlService.valid()) {
                return;
            }
            var tipoRespostaConfiguracaoElementoHtml = _$formTipoRespostaConfiguracaoElementoHtmlService.serializeFormToObject();
            _modalManager.setBusy(true);
            _tipoRespostaConfiguracaoElementoHtmlService.criarOuEditar(tipoRespostaConfiguracaoElementoHtml, { async: false })
                 .done(function () {
                     localStorage["FecharModal"] = true;
                     abp.notify.success(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarTipoRespostaConfiguracaoElementoHtmlModalSaved');
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };

        function deleteTipoRespostaConfiguracao(tipoRespostaConfiguracaoElementoHtml) {
            abp.message.confirm(
                app.localize('DeleteWarning', tipoRespostaConfiguracaoElementoHtml.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _tipoRespostaConfiguracaoElementoHtmlService.excluir(tipoRespostaConfiguracaoElementoHtml)
                            .done(function () {
                                abp.notify.success(app.localize('SuccessfullyDeleted'));
                            });
                    }
                }
            );
        }

        $("#elemento-html-id").select2({
            allowClear: true,
            placeholder: app.localize("SelecioneLista"),
            ajax: {
                url: "/api/services/app/ElementoHtml/listarDropdown",
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
        });

    };
})(jQuery);