(function ($) {
    app.modals.ImagensModal = function () {
        var _prescricoesItensService = abp.services.app.prescricaoItem;
        var _modalManager;
        var _$imagensDisponiveisTable = $('#ImagensDisponiveisTable');
        var _$imagensIncluidosTable = $('#ImagensIncluidosTable');
        var aImagensSelecionados = [];
        var aImagensRemovidos = [];
        var aImagensIncluidos = [];

        this.init = function (modalManager) {
            _modalManager = modalManager;
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
            $('.modal-dialog').css({ 'width': '90%', 'max-width': '990px' });
            $('.select2').css('width', '100%');
            CamposRequeridos();
        };

        this.save = function () {
            if (aImagensRemovidos.length === 0 && aImagensSelecionados.length === 0) {
                abp.notify.error(app.localize("NadaAlterar"));
                return false;
            }
            //itens removidos
            _modalManager.setBusy(true);
            if (aImagensRemovidos.length > 0) {
                abp.notify.info(app.localize('ExcluindoDesmarcados'));
                _prescricoesItensService.excluirPorFatItem(aImagensRemovidos)
                        .done(function () {
                            abp.notify.success(app.localize("SuccessfullyDeleted"));
                            getImagensDisponiveis();
                            getImagensIncluidos();
                            abp.event.trigger('app.ImagensModalSaved');
                        })
            }
            //itens incluídos
            if (aImagensSelecionados.length > 0) {
                abp.notify.info(app.localize('IncluindoMarcados'));
                _prescricoesItensService.inserirPorFatItem({
                    estoqueId: $('#imagens-disponiveis-estoque-id').val(),
                    grupoId: $('#imagens-disponiveis-grupo-id').val(),
                    divisaoId: $('#imagens-disponiveis-divisao-id').val(),
                    ids: aImagensSelecionados
                })
                    .done(function () {
                        abp.notify.success(app.localize("SavedSuccessfully"));
                        getImagensDisponiveis();
                        getImagensIncluidos();
                        abp.event.trigger('app.ImagensModalSaved');
                    })
            }
            _modalManager.setBusy(false);
        };

        _$imagensDisponiveisTable.jtable({
            title: app.localize('ImagensDisponiveis'),
            paging: true,
            sorting: true,
            multiSorting: true,
            selecting: true, //Enable selecting
            multiselect: true, //Allow multiple selecting
            selectingCheckboxes: true, //Show checkboxes on first column
            actions: {
                listAction: {
                    method: _prescricoesItensService.listarExamesImagemDisponiveis
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
                $('#ImagensDisponiveisTable tr.jtable-row-selected').each(function () {
                    idsSelecionados.push($(this).data('record-key'));
                });
                aImagensSelecionados = idsSelecionados;
            },
        });

        _$imagensIncluidosTable.jtable({
            title: app.localize('ImagensIncluidos'),
            paging: true,
            sorting: true,
            multiSorting: true,
            selecting: true, //Enable selecting
            multiselect: true, //Allow multiple selecting
            selectingCheckboxes: true, //Show checkboxes on first column
            actions: {
                listAction: {
                    method: _prescricoesItensService.listarExamesImagemIncluidos
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
                        //if (aImagensIncluidos.length == 0) {
                        aImagensIncluidos.push(data.record.id);
                        //}
                        return data.record.nome;
                    },
                },
            },
            selectionChanged: function () {
                var idsRemovidos = [];
                $('#ImagensIncluidosTable tr.jtable-data-row:not(.jtable-row-selected)').each(function () {
                    idsRemovidos.push($(this).data('record-key'));
                });
                aImagensRemovidos = idsRemovidos;
                var idsIncluidos = [];
                $('#ImagensIncluidosTable tr.jtable-row-selected').each(function () {
                    idsIncluidos.push($(this).data('record-key'));
                });
                aImagensIncluidos = idsIncluidos;
            },
            recordsLoaded: function (event, data) {
                $('#ImagensIncluidosTable tr.jtable-data-row:not(.jtable-row-selected)').click();
            }
        });

        function getImagensIncluidos() {
            _$imagensIncluidosTable.jtable('load', {
                filtro: $('#ImagensIncluidosTableFilter').val(),
                id: $('#imagens-incluidos-grupo-id').val()
            });
        }

        function getImagensDisponiveis() {
            _$imagensDisponiveisTable.jtable('load', {
                filtro: $('#ImagensDisponiveisTableFilter').val(),
                id: $('#imagens-disponiveis-grupo-id').val()
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

        $('#RefreshImagensDisponiveisListButton').on('click', function (e) {
            e.preventDefault();
            getImagensDisponiveis();
        });

        $('#RefreshImagensIncluidosListButton').on('click', function (e) {
            e.preventDefault();
            getImagensIncluidos();
        });

        $('#imagens-disponiveis-grupo-id').select2({
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
                        id: $('#imagens-disponiveis-grupo-id').val(),
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

        $('#imagens-disponiveis-divisao-id').select2({
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

        $('#imagens-incluidos-grupo-id').select2({
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
                        id: $('#imagens-incluidos-grupo-id').val()
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

        getImagensDisponiveis();
        getImagensIncluidos();
    };
})(jQuery);
