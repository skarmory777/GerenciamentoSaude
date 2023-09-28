(function ($) {
    app.modals.CriarOuEditarTipoRespostaTipoRespostaConfiguracaoModal = function () {

        var _tipoRespostaTipoRespostaConfiguracaoService = abp.services.app.tipoRespostaTipoRespostaConfiguracao;
        var _modalManager;
        var _$formTipoRespostaTipoRespostaConfiguracao = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;
            _$formTipoRespostaTipoRespostaConfiguracao = _modalManager.getModal().find('form[name=TipoRespostaConfiguracaoInformationsForm]');
            _$formTipoRespostaTipoRespostaConfiguracao.validate();
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
            $('.modal-dialog').css({ 'width': '90%', 'max-width': '990px' });
        };

        this.save = function () {
            if (!_$formTipoRespostaTipoRespostaConfiguracao.valid()) {
                return;
            }
            var tipoRespostaTipoRespostaConfiguracao = _$formTipoRespostaTipoRespostaConfiguracao.serializeFormToObject();
            _modalManager.setBusy(true);
            _tipoRespostaTipoRespostaConfiguracaoService.criarOuEditar(tipoRespostaTipoRespostaConfiguracao, { async: false })
                 .done(function () {
                     localStorage["FecharModal"] = true;
                     abp.notify.success(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarTipoRespostaTipoRespostaConfiguracaoModalSaved');
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };

        //function deleteTipoRespostaConfiguracao(tipoRespostaTipoRespostaConfiguracao) {
        //    abp.message.confirm(
        //        app.localize('DeleteWarning', tipoRespostaTipoRespostaConfiguracao.descricao),
        //        function (isConfirmed) {
        //            if (isConfirmed) {
        //                _tipoRespostaTipoRespostaConfiguracaoService.excluir(tipoRespostaTipoRespostaConfiguracao)
        //                    .done(function () {
        //                        abp.notify.success(app.localize('SuccessfullyDeleted'));
        //                    });
        //            }
        //        }
        //    );
        //}

        $("#tipo-resposta-configuracao-id").select2({
            allowClear: true,
            placeholder: app.localize("SelecioneLista"),
            ajax: {
                url: "/api/services/app/tipoRespostaConfiguracao/listarDropdown",
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