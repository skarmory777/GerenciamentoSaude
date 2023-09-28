(function ($) {
    app.modals.CriarOuEditarSolicitacaoExameItemModal = function () {

        var _solicitacoesExamesItensService = abp.services.app.solicitacaoExameItem;

        var _modalManager;
        var _$solicitacaoExameItemInformationForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            //_$solicitacaoExameItemInformationForm = _modalManager.getModal().find('form[name=SolicitacaoExameItemInformationsForm]');
            //_$solicitacaoExameItemInformationForm.validate();
            //$('.modal-dialog').css({ 'width': '90%', 'max-width': '1100px' });
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
        };

        this.save = function () {
            _$solicitacaoExameItemInformationForm = _modalManager.getModal().find('form[name=SolicitacaoExameItemInformationsForm-' + localStorage["AtendimentoId"] + ']');
            _$solicitacaoExameItemInformationForm.validate();
            if (!_$solicitacaoExameItemInformationForm.valid()) {
                return;
            }

            var solicitacaoExameItem = _$solicitacaoExameItemInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _solicitacoesExamesItensService.criarOuEditar(solicitacaoExameItem, { async: false, cahche: false })
                 .done(function () {
                     abp.notify.success(app.localize('SavedSuccessfully'));
                     abp.event.trigger('app.CriarOuEditarSolicitacaoExameItemModalSaved');
                     $('#RefreshSolicitacaoExameItensListButton-' + localStorage["AtendimentoId"]).trigger('click');
                     _$solicitacaoExameItemInformationForm.trigger('reset');
                     $('#faturamento-item-id-' + localStorage["AtendimentoId"]).val(null).trigger('change');
                     $('#material-id-' + localStorage["AtendimentoId"]).val(null).trigger('change');
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };

        function getSolicitacaoExameItens() {
            $('#RefreshSolicitacaoExameItensListButton-' + localStorage["AtendimentoId"]).trigger('click');
        }

        var fatItem = $('#faturamento-item-id-' + localStorage["AtendimentoId"]).select2({
            allowClear: true,
            placeholder: app.localize("SelecioneLista"),
            ajax: {
                url: "/api/services/app/faturamentoItem/ListarDropdownExame",
                dataType: 'json',
                delay: 250,
                method: 'Post',
                data: function (params) {
                    if (params.page == undefined)
                        params.page = '1';
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

        var labMaterial = $('#material-id-' + localStorage["AtendimentoId"]).select2({
            allowClear: true,
            placeholder: app.localize("SelecioneLista"),
            ajax: {
                url: "/api/services/app/material/ListarDropdown",
                dataType: 'json',
                delay: 250,
                method: 'Post',
                data: function (params) {
                    if (params.page == undefined)
                        params.page = '1';
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

        CamposRequeridos();
        aplicarDateSingle();
        aplicarDateRange();
    };
})(jQuery);