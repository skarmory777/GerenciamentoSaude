(function ($) {
    app.modals.SelecionarKitExameItemModal = function () {

        var aExames = [];

        var _solicitacaoExameItemService = abp.services.app.solicitacaoExameItem;
        var _kitExameItensService = abp.services.app.kitExameItem;

        var _modalManager;

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.SolicitacaoExameItem'),
            edit: abp.auth.hasPermission('Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.SolicitacaoExameItem'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.SolicitacaoExameItem')
        };

        this.init = function (modalManager) {
            _modalManager = modalManager;
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
        };

        this.save = function () {
            //criar o objeto do tipo SolicitacaoExameItem
            if (aExames.length > 0) {
                _modalManager.setBusy(true);
                for (var i = 0; i < aExames.length; i++) {
                    var solicitacaoExameItem = {
                        "Id": "0",
                        "FaturamentoItemId": aExames[i].fatItemId,
                        "MaterialId": aExames[i].materialId,
                        "SolicitacaoExameId": aExames[i].solicitacaoExameId,
                        "IsDeleted": "0",
                        "IsSistema": "0",
                        "CreationTime": moment(),
                        "CreatorUserId": "2" //descobrir como pegar o id do usuário logado via js
                    }
                    _solicitacaoExameItemService.criarOuEditar(solicitacaoExameItem, { async: false, cache: false })
                    .done(function (data) {
                        abp.notify.success(app.localize('SavedSuccessfully'));
                        abp.event.trigger('app.SelecionarKitExameItemModalSaved');
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

        function getKitExameItens() {
            $('#KitExameItensTable-' + localStorage["AtendimentoId"]).jtable('load', {
                Filtro: $('#cbo-kits-' + localStorage["AtendimentoId"]).val()
            });
        }

        $('#KitExameItensTable-' + localStorage["AtendimentoId"]).jtable({
            title: app.localize('SolicitacaoExameItem'),
            paging: true,
            sorting: true,
            multiSorting: true,
            selecting: true, //Enable selecting
            multiselect: true, //Allow multiple selecting
            selectingCheckboxes: true, //Show checkboxes on first column
            actions: {
                listAction: {
                    method: _kitExameItensService.listarPorKit
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
                    display: function (data) {
                        return zeroEsquerda(data.record.faturamentoItem.codigo, '8');
                    }
                },
                descricao: {
                    title: app.localize('Exame'),
                    width: '40%',
                    sorting: false,
                    display: function (data) {
                        return data.record.faturamentoItem.descricao;
                    }
                }
            },
            selectionChanged: function () {
                //Get all selected rows
                var $selectedRows = $('#KitExameItensTable-' + localStorage["AtendimentoId"]).jtable('selectedRows');
                if ($selectedRows.length > 0) {
                    //Show selected rows
                    var list = [];
                    var i = 0;
                    $selectedRows.each(function () {
                        var record = $(this).data('record');
                        list[i] = {
                            'fatItemId': record.faturamentoItemId,
                            'materialId': record.materialId ? record.materialId : '',
                            'solicitacaoExameId': $('#id-solicitacao-' + localStorage["AtendimentoId"]).val()
                        };
                        i++;
                    })
                    aExames = [];
                    aExames = list;
                }
                else {
                    $('#content-detalhe-solicitacao-exame-' + localStorage["AtendimentoId"]).addClass('hidden');
                }
            }
        });
        getKitExameItens();



       


    };
})(jQuery);