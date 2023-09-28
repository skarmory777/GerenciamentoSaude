(function ($) {
    app.modals.ProdutosModal = function () {
        var _prescricoesItensService = abp.services.app.prescricaoItem;
        var _modalManager;
        var _$produtosDisponiveisTable = $('#ProdutosDisponiveisTable');
        var _$produtosIncluidosTable = $('#ProdutosIncluidosTable');
        var aProdutosSelecionados = [];
        var aProdutosRemovidos = [];
        var aProdutosIncluidos = [];

        this.init = function (modalManager) {
            _modalManager = modalManager;
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
            $('.modal-dialog').css({ 'width': '90%', 'max-width': '990px' });
            $('.select2').css('width', '100%');
            CamposRequeridos();
        };

        this.save = function () {
            if (aProdutosRemovidos.length === 0 && aProdutosSelecionados.length === 0) {
                abp.notify.error(app.localize("NadaAlterar"));
                return false;
            }
            //itens removidos
            _modalManager.setBusy(true);
            if (aProdutosRemovidos.length > 0) {
                abp.notify.info(app.localize('ExcluindoDesmarcados'));
                _prescricoesItensService.excluirPorProduto(aProdutosRemovidos)
                        .done(function () {
                            abp.notify.success(app.localize("SuccessfullyDeleted"));
                            getProdutosDisponiveis();
                            getProdutosIncluidos();
                            abp.event.trigger('app.ProdutosModalSaved');
                        })
            }
            //itens incluídos
            if (aProdutosSelecionados.length > 0) {
                abp.notify.info(app.localize('IncluindoMarcados'));
                _prescricoesItensService.inserirPorProduto({
                    estoqueId: $('#produtos-disponiveis-estoque-id').val(),
                    grupoId: $('#produtos-disponiveis-grupo-id').val(),
                    divisaoId: $('#produtos-disponiveis-divisao-id').val(),
                    ids: aProdutosSelecionados
                })
                    .done(function () {
                        abp.notify.success(app.localize("SavedSuccessfully"));
                        getProdutosDisponiveis();
                        getProdutosIncluidos();
                        abp.event.trigger('app.ProdutosModalSaved');
                    })
            }
            _modalManager.setBusy(false);
        };

        _$produtosDisponiveisTable.jtable({
            title: app.localize('ProdutosDisponiveis'),
            paging: true,
            sorting: true,
            multiSorting: true,
            selecting: true, //Enable selecting
            multiselect: true, //Allow multiple selecting
            selectingCheckboxes: true, //Show checkboxes on first column
            actions: {
                listAction: {
                    method: _prescricoesItensService.listarProdutosDisponiveis
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
                $('#ProdutosDisponiveisTable tr.jtable-row-selected').each(function () {
                    idsSelecionados.push($(this).data('record-key'));
                });
                aProdutosSelecionados = idsSelecionados;
            },
        });

        _$produtosIncluidosTable.jtable({
            title: app.localize('ProdutosIncluidos'),
            paging: true,
            sorting: true,
            multiSorting: true,
            selecting: true, //Enable selecting
            multiselect: true, //Allow multiple selecting
            selectingCheckboxes: true, //Show checkboxes on first column
            actions: {
                listAction: {
                    method: _prescricoesItensService.listarProdutosIncluidos
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
                        //if (aProdutosIncluidos.length == 0) {
                        aProdutosIncluidos.push(data.record.id);
                        //}
                        return data.record.nome;
                    },
                },
            },
            selectionChanged: function () {
                var idsRemovidos = [];
                $('#ProdutosIncluidosTable tr.jtable-data-row:not(.jtable-row-selected)').each(function () {
                    idsRemovidos.push($(this).data('record-key'));
                });
                aProdutosRemovidos = idsRemovidos;
                var idsIncluidos = [];
                $('#ProdutosIncluidosTable tr.jtable-row-selected').each(function () {
                    idsIncluidos.push($(this).data('record-key'));
                });
                aProdutosIncluidos = idsIncluidos;
            },
            recordsLoaded: function (event, data) {
                $('#ProdutosIncluidosTable tr.jtable-data-row:not(.jtable-row-selected)').click();
            }
        });

        function getProdutosIncluidos() {
            _$produtosIncluidosTable.jtable('load', {
                filtro: $('#ProdutosIncluidosTableFilter').val(),
                id: $('#produtos-incluidos-grupo-id').val()
            });
        }

        function getProdutosDisponiveis() {
            _$produtosDisponiveisTable.jtable('load', {
                filtro: $('#ProdutosDisponiveisTableFilter').val(),
                id: $('#produtos-disponiveis-grupo-id').val()
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

        $('#RefreshProdutosDisponiveisListButton').on('click', function (e) {
            e.preventDefault();
            getProdutosDisponiveis();
        });

        $('#RefreshProdutosIncluidosListButton').on('click', function (e) {
            e.preventDefault();
            getProdutosIncluidos();
        });

        $('#produtos-disponiveis-grupo-id').select2({
            allowClear: true,
            placeholder: app.localize("SelecioneLista"),
            ajax: {
                url: '/api/services/app/grupo/ListarDropdownGruposPorEstoque',
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
                        filtro: $('#produtos-disponiveis-estoque-id').val(),
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

        $('#produtos-disponiveis-divisao-id').select2({
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

        $('#produtos-disponiveis-estoque-id').select2({
            allowClear: true,
            placeholder: app.localize("SelecioneLista"),
            ajax: {
                url: '/api/services/app/estoque/listarDropdown',
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

        $('#produtos-incluidos-estoque-id').select2({
            allowClear: true,
            placeholder: app.localize("SelecioneLista"),
            ajax: {
                url: '/api/services/app/estoque/listarDropdown',
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

        $('#produtos-incluidos-grupo-id').select2({
            allowClear: true,
            placeholder: app.localize("SelecioneLista"),
            ajax: {
                url: '/api/services/app/grupo/ListarDropdownGruposPorEstoque',
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
                        filtro: $('#produtos-incluidos-estoque-id').val()
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

        getProdutosDisponiveis();
        getProdutosIncluidos();
    };
})(jQuery);
