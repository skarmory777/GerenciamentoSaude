(function ($) {
    app.modals.SelecionarTiposPrescricaoModal = function () {

        var aTiposPrescricao = [];

        var _divisaoService = abp.services.app.divisao;

        var _modalManager;

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Assistenciais.Prescricao.Divisao.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Assistenciais.Prescricao.Divisao.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Assistenciais.Prescricao.Divisao.Delete')
        };

        this.init = function (modalManager) {
            _modalManager = modalManager;
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
        };

        this.save = function () {
            //criar o objeto do tipo SolicitacaoExameItem
            if (aTiposPrescricao.length > 0) {
                _modalManager.setBusy(true);
                for (var i = 0; i < aTiposPrescricao.length; i++) {
                    var subdivisao = aTiposPrescricao[i];
                    //console.log(aTiposPrescricao[i]);
                    subdivisao.DivisaoPrincipalId = $('#divisao-principal-id-selecionar').val();
                    _divisaoService.salvarTipoPrescricao(subdivisao)
                    .done(function (data) {
                        abp.notify.success(app.localize('SavedSuccessfully'));
                        abp.event.trigger('app.SelecionarTipoPrescricaoModalSaved');
                    })
                    .always(function () {
                        _modalManager.setBusy(false);
                    })
                }
                _modalManager.close();
            }

            else {
                abp.notify.warn(app.localize('SelecioneLista'));
            }

        };

        function getDivisaoItens() {
            $('#TiposPrescricaoTable').jtable('load');
        }


        $('#TiposPrescricaoTable').jtable({
            title: app.localize('TiposPrescricaoDisponiveis'),
            paging: true,
            sorting: true,
            multiSorting: true,
            selecting: true, //Enable selecting
            multiselect: true, //Allow multiple selecting
            selectingCheckboxes: true, //Show checkboxes on first column
            actions: {
                listAction: {
                    method: _divisaoService.listarTiposPrescricaoSemRelacao
                }
            },
            fields: {
                id: {
                    key: true,
                    list: false
                },
                faturamentoItemId: {
                    list: false,
                    display: function (data) {
                        return data.record.faturamentoItemId;
                    }

                },
                codigo: {
                    title: app.localize('Codigo'),
                    width: '40%',
                    sorting: false,
                    //display: function (data) {
                    //    return zeroEsquerda(data.record.faturamentoItem.codigo, '8');
                    //}
                },
                descricao: {
                    title: app.localize('Descricao'),
                    width: '40%',
                    sorting: false,
                    //display: function (data) {
                    //    return data.record.faturamentoItem.descricao;
                    //}
                }
            },
            selectionChanged: function () {
                //Get all selected rows
                var $selectedRows = $('#TiposPrescricaoTable').jtable('selectedRows');
                if ($selectedRows.length > 0) {
                    //Show selected rows
                    var list = [];
                    var i = 0;
                    $selectedRows.each(function () {
                        var record = $(this).data('record');
                        list[i] = record;
                        i++;
                    })
                    aTiposPrescricao = [];
                    aTiposPrescricao = list;
                }
            }
        });
        getDivisaoItens();
    };
})(jQuery);