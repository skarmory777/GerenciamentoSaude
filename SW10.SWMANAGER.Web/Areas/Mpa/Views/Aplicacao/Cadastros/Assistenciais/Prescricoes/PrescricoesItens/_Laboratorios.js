(function ($) {
    app.modals.LaboratoriosModal = function () {
        var _prescricoesItensService = abp.services.app.prescricaoItem;
        var _modalManager;
        var _$laboratoriosDisponiveisTable = $('#LaboratoriosDisponiveisTable');
        var _$laboratoriosIncluidosTable = $('#LaboratoriosIncluidosTable');
        var aLaboratoriosSelecionados = [];
        var aLaboratoriosRemovidos = [];
        var aLaboratoriosIncluidos = [];

        this.init = function (modalManager) {
            _modalManager = modalManager;
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
            $('.modal-dialog').css({ 'width': '90%', 'max-width': '990px' });
            $('.select2').css('width', '100%');
            CamposRequeridos();
        };

        this.save = function () {
            if (aLaboratoriosRemovidos.length === 0 && aLaboratoriosSelecionados.length === 0) {
                abp.notify.error(app.localize("NadaAlterar"));
                return false;
            }
            //itens removidos
            _modalManager.setBusy(true);
            if (aLaboratoriosRemovidos.length > 0) {
                abp.notify.info(app.localize('ExcluindoDesmarcados'));
                _prescricoesItensService.excluirPorFatItem(aLaboratoriosRemovidos)
                        .done(function () {
                            abp.notify.success(app.localize("SuccessfullyDeleted"));
                            getLaboratoriosDisponiveis();
                            getLaboratoriosIncluidos();
                            abp.event.trigger('app.LaboratoriosModalSaved');
                        })
            }
            //itens incluídos
            if (aLaboratoriosSelecionados.length > 0) {
                abp.notify.info(app.localize('IncluindoMarcados'));
                _prescricoesItensService.inserirPorFatItem({
                    estoqueId: $('#laboratorios-disponiveis-estoque-id').val(),
                    grupoId: $('#laboratorios-disponiveis-grupo-id').val(),
                    divisaoId: $('#laboratorios-disponiveis-divisao-id').val(),
                    ids: aLaboratoriosSelecionados
                })
                    .done(function () {
                        abp.notify.success(app.localize("SavedSuccessfully"));
                        getLaboratoriosDisponiveis();
                        getLaboratoriosIncluidos();
                        abp.event.trigger('app.LaboratoriosModalSaved');
                    })
            }
            _modalManager.setBusy(false);
        };

        _$laboratoriosDisponiveisTable.jtable({
            title: app.localize('LaboratoriosDisponiveis'),
            paging: true,
            sorting: true,
            multiSorting: true,
            selecting: true, //Enable selecting
            multiselect: true, //Allow multiple selecting
            selectingCheckboxes: true, //Show checkboxes on first column
            actions: {
                listAction: {
                    method: _prescricoesItensService.listarExamesLaboratoriaisDisponiveis
                }
            },
            fields: {
                id: {
                    key: true,
                    list: false
                },
                nome: {
                    title: app.localize('Descricao'),
                    width: '60%'
                },
            },
            selectionChanged: function () {
                var idsSelecionados = [];
                $('#LaboratoriosDisponiveisTable tr.jtable-row-selected').each(function () {
                    idsSelecionados.push($(this).data('record-key'));
                });
                aLaboratoriosSelecionados = idsSelecionados;
            },
        });

        _$laboratoriosIncluidosTable.jtable({
            title: app.localize('LaboratoriosIncluidos'),
            paging: true,
            sorting: true,
            multiSorting: true,
            selecting: true, //Enable selecting
            multiselect: true, //Allow multiple selecting
            selectingCheckboxes: true, //Show checkboxes on first column
            actions: {
                listAction: {
                    method: _prescricoesItensService.listarExamesLaboratoriaisIncluidos
                }
            },
            fields: {
                id: {
                    key: true,
                    list: false
                },
                nome: {
                    title: app.localize('Descricao'),
                    width: '60%',
                    display: function (data) {
                        //if (aLaboratoriosIncluidos.length == 0) {
                        aLaboratoriosIncluidos.push(data.record.id);
                        //}
                        return data.record.nome;
                    },
                },
            },
            selectionChanged: function () {
                var idsRemovidos = [];
                $('#LaboratoriosIncluidosTable tr.jtable-data-row:not(.jtable-row-selected)').each(function () {
                    idsRemovidos.push($(this).data('record-key'));
                });
                aLaboratoriosRemovidos = idsRemovidos;
                var idsIncluidos = [];
                $('#LaboratoriosIncluidosTable tr.jtable-row-selected').each(function () {
                    idsIncluidos.push($(this).data('record-key'));
                });
                aLaboratoriosIncluidos = idsIncluidos;
            },
            recordsLoaded: function (event, data) {
                $('#LaboratoriosIncluidosTable tr.jtable-data-row:not(.jtable-row-selected)').click();
            }
        });

        function getLaboratoriosIncluidos() {
            _$laboratoriosIncluidosTable.jtable('load', {
                filtro: $('#LaboratoriosIncluidosTableFilter').val(),
                id: $('#laboratorios-incluidos-grupo-id').val()
            });
        }

        function getLaboratoriosDisponiveis() {
            _$laboratoriosDisponiveisTable.jtable('load', {
                filtro: $('#LaboratoriosDisponiveisTableFilter').val(),
                id: $('#laboratorios-disponiveis-grupo-id').val()
            });
        }

        $('.fa-close').on('click', function (e) {
            e.preventDefault();
            location.href = '/mpa/prescricoesitens';
        });

        $('.close-button').on('click', function (e) {
            e.preventDefault();
            location.href = '/mpa/prescricoesitens';
        });

        $('#RefreshLaboratoriosDisponiveisListButton').on('click', function (e) {
            e.preventDefault();
            getLaboratoriosDisponiveis();
        });

        $('#RefreshLaboratoriosIncluidosListButton').on('click', function (e) {
            e.preventDefault();
            getLaboratoriosIncluidos();
        });

        $('#laboratorios-disponiveis-grupo-id').select2({
            allowClear: true,
            placeholder: app.localize("SelecioneLista"),
            ajax: {
                url: '/api/services/app/faturamentogrupo/ListarDropdown',
                dataType: 'json',
                delay: 250,
                method: 'Post',
                data: function (params) {
                    if (params.page === undefined)
                        params.page = '1';
                    return {
                        search: params.term,
                        page: params.page,
                        totalPorPagina: 10,
                        id: $('#laboratorios-disponiveis-grupo-id').val(),
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

        $('#laboratorios-disponiveis-divisao-id').select2({
            allowClear: true,
            placeholder: app.localize("SelecioneLista"),
            ajax: {
                url: '/api/services/app/divisao/ListarDropdown',
                dataType: 'json',
                delay: 250,
                method: 'Post',
                data: function (params) {
                    if (params.page === undefined)
                        params.page = '1';
                    return {
                        search: params.term,
                        page: params.page,
                        totalPorPagina: 10,
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

        $('#laboratorios-incluidos-grupo-id').select2({
            allowClear: true,
            placeholder: app.localize("SelecioneLista"),
            ajax: {
                url: '/api/services/app/faturamentogrupo/ListarDropdown',
                dataType: 'json',
                delay: 250,
                method: 'Post',
                data: function (params) {
                    if (params.page === undefined)
                        params.page = '1';
                    return {
                        search: params.term,
                        page: params.page,
                        totalPorPagina: 10,
                        id: $('#laboratorios-incluidos-grupo-id').val()
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

        getLaboratoriosDisponiveis();
        getLaboratoriosIncluidos();
    };
})(jQuery);
