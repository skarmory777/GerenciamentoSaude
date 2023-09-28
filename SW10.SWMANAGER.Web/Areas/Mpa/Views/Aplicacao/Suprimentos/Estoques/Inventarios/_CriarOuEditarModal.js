
(function ($) {
    $(function () {
        const $inventariosProdutosTable = $('#inventariosProdutosTable');
        const inventarioMovimentoService = abp.services.app.inventario;
        const _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Suprimentos.Estoque.Inventario.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Suprimentos.Estoque.Inventario.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Suprimentos.Estoque.Inventario.Delete'),
            fecharInventario: abp.auth.hasPermission('Pages.Tenant.Suprimentos.Estoque.PreMovimento.FecharInventario')
        };
        const numberMaskTemplate = {
            mask: 'num',
            blocks: {
                num: {
                    mask: Number,
                    selectionStart: -1,
                    thousandsSeparator: '.',
                    scale: 0,	// digits after decimal
                    normalizeZeros: false,  // appends or removes zeros at ends
                    radix: ',',  // fractional delimiter
                    padFractionalZeros: false,  // if true, then pads zeros at end to the length of scale
                    allowDecimal: false
                }
            },
        };
        const maskedQtdNovo = IMask(document.querySelector('.quantidadeNovo'), numberMaskTemplate);
        const maskedQtd = IMask(document.querySelector('#quantidade'), numberMaskTemplate);
        const _ErrorModal = new app.ModalManager({ viewUrl: abp.appPath + 'Mpa/Erros/ExibirErros' });
        const modalLoader = $(".modal.loader").modal({ backdrop: 'static', show: false });
        let itemId = null;
        let gridCallback = null;
        $("#quantidade").mask('000.000.000.000', { reverse: true });

        var lista = [];



        const TODOS = "InventarioProdutos::TODOS";
        const CONTADOS = "InventarioProdutos::CONTADOS";
        const PENDENTES = "InventarioProdutos::PENDENTES";

        function qtdCellRenderer() {
        }

        // init method gets the details of the cell to be renderer
        qtdCellRenderer.prototype.init = function (params) {
            this.eGui = document.createElement('span');
            this.eGui.classList.add("text-left");
            this.eGui.classList.add("qtd-render");
            this.eGui.innerHTML = params.value;
        };

        qtdCellRenderer.prototype.getGui = function () {
            return this.eGui;
        };

        function actionRenderer() {

        }

        actionRenderer.prototype.init = function (params) {
            //this.eGui = document.createElement('span');
            //this.eGui.innerHTML = "oi";
            //return;
            //debugger;
            const span = document.createElement('span');
            var $span = $('<span></span>');
            if (params.data.statusInventarioItemId !== 4 && $("#statusInventarioId").val() !== "4") {
                $('<button class="btn btn-warning btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                    .appendTo($span)
                    .click(function (e) {
                        e.preventDefault();
                        gridOptions.api.selectNode(params.node, true);
                        gridOptions.api.ensureNodeVisible(params.node, 'middle');
                        editarItem(params.data);
                    });

                //debugger;
                if (!params.data.temDivergencia) {
                    $('<button class="btn btn-danger btn-xs" title="' + app.localize('Excluir') + '"><i class="fa fa-trash"></i></button>')
                        .appendTo($span)
                        .click(function (e) {
                            e.preventDefault();
                            excluirItem(e, params.data);
                        });
                }
            }

            this.eGui = $span[0];
            //this.eGui.innerHTML = params.value;
        };

        actionRenderer.prototype.getGui = function () {
            return this.eGui;
        };


        function statusRenderer() {

        }

        statusRenderer.prototype.init = function (params) {
            const span = document.createElement("span");
            switch (params.data.statusInventarioItemId) {
                case 1: {
                    span.classList.add("label","label-danger");
                    span.innerHTML = "Pendente";
                    break;
                }
                case 2: {
                    span.classList.add("label", "label-warning");
                    span.innerHTML = "Contado";
                    break;
                }
                case 4: {
                    span.classList.add("label", "label-success");
                    span.innerHTML = "Fechado";
                    break;
                }

                default: {
                    break;
                }
            }
            this.eGui = span;
        }

        statusRenderer.prototype.getGui = function () {
            return this.eGui;
        };

        let gridOptions = {
            components: {
                'qtdCellRenderer': qtdCellRenderer,
                'actionRenderer': actionRenderer,
                'statusRenderer': statusRenderer,
            },
            defaultColDef: {
                filter: true,
                sortable: false,
                floatingFilter: true,
            },
            //pagination: true,
            enableBrowserTooltips: false,
            tooltipShowDelay: 0,
            height: "300",
            rowData: null,
            columnDefs: null
        };

        const columnDefs = [
            {
                headerName: app.localize('Ações'),
                field: 'id',
                filter: false,
                width: 50,
                suppressMenu: true,
                cellRenderer: 'actionRenderer'
            },
            {
                headerName: app.localize('Produto'),
                field: "produtoDescricao",
                filter: 'agTextColumnFilter',
                floatingFilter: true
            },
            {
                headerName: app.localize('Lote'),
                field: "lote",
                width: 70,
                filter: 'agTextColumnFilter',
                floatingFilter: true,
                resizable: false
            },
            {
                headerName: app.localize('Validade2'),
                field: "validade",
                width: 70,
                resizable: false,
                filter: 'agTextColumnFilter',
                floatingFilter: true,
                valueGetter: (params) => {
                    if (params.data.validade) {
                        return moment(params.data.validade).format('L');
                    }
                    return "";
                }
            },
            {
                headerName: app.localize('Quantidade'),
                field: "quantidadeContagem",
                width: 80,
                resizable: false,
                filter: false,
                cellRenderer: 'qtdCellRenderer'
            }];


        selectSW('.selectEstoque', "/api/services/app/estoque/ListarDropdown");
        selectSW('.selectGrupo', "/api/services/app/grupo/ListarDropdownGruposPorEstoque", $('#estoqueId'));
        selectSW('.selectClasse', "/api/services/app/grupoClasse/ListarDropdown", $('#grupoId'));
        selectSW('.selectSubClasse', "/api/services/app/grupoSubClasse/ListarDropdown", $('#classeId'));
        selectSW('.selectProdutoNovo', "/api/services/app/produto/ListarProdutoDropdown");


        $('input[name="ValidadeNovo"]').daterangepicker({
            "singleDatePicker": true,
            "showDropdowns": true,
            autoUpdateInput: false,
            //  maxDate: new Date(),
            changeYear: true,
            yearRange: 'c-10:c+10',
            showOn: "both",
            "locale": {
                "format": moment.locale().toUpperCase() === 'PT-BR' ? "DD/MM/YYYY" : moment.locale().toUpperCase() === 'US' ? "MM/DD/YYYY" : "YYYY-MM-DD",
                "separator": " - ",
                "applyLabel": "Apply",
                "cancelLabel": "Cancel",
                "fromLabel": "From",
                "toLabel": "To",
                "customRangeLabel": "Custom",
                "daysOfWeek": [
                    app.localize('Dom'),
                    app.localize('Seg'),
                    app.localize('Ter'),
                    app.localize('Qua'),
                    app.localize('Qui'),
                    app.localize('Sex'),
                    app.localize('Sab')
                ],
                "monthNames": [
                    app.localize("Jan"),
                    app.localize("Fev"),
                    app.localize("Mar"),
                    app.localize("Abr"),
                    app.localize("Mai"),
                    app.localize("Jun"),
                    app.localize("Jul"),
                    app.localize("Ago"),
                    app.localize("Set"),
                    app.localize("Out"),
                    app.localize("Nov"),
                    app.localize("Dez"),
                ],
                "firstDay": 0
            }
        },
            function (selDate) {
                debugger;
                $('input[name="ValidadeNovo"]').val(selDate.format('L')).addClass('form-control edited');
            });
        $('input[name="ValidadeNovo"]').on('apply.daterangepicker', function (ev, picker) {
            $(this).val(picker.startDate.format('L')).addClass('form-control edited');
        });

        attachEventListeners();
        createInventarioProdutosTable(TODOS);

        function attachEventListeners() {

            $(".todosBox").click((e) => {
                e.preventDefault();
                createInventarioProdutosTable(TODOS);
            })

            $(".contadosBox").click((e) => {
                e.preventDefault();
                createInventarioProdutosTable(CONTADOS);
            })

            $(".pendentesBox").click((e) => {
                e.preventDefault();
                createInventarioProdutosTable(PENDENTES);
            })

            $("#novoItem").click(novoItem);

            $('#inserirQuantidadeNovo').click(inserirQuantidadeNovo);

            $('#salvarButton').click(salvarItem);

            $('#gerarInventarioButton').click(gerarInventario);

            $('#inserirQuantidade').click((e) => {
                e.preventDefault();
                inserir();
            });

            $('.key').keydown((e) => {
                if (e.key == "Enter") {
                    e.preventDefault();
                    inserir();
                }
            });

            $('.close').click((e) => {
                location.href = '/mpa/Inventarios';
            });

            $('.close-button').click((e) => {
                location.href = '/mpa/Inventarios';
            });
        }

        function inserir() {
            const item = {
                itemId: $('#invItemId').val(),
                produtoId: $('#produtoId').val(),
                lote: $('#lote').val(),
                gridId: $('#gridId').val(),
                validade: $('#validade').val(),
                quantidadeContagem: maskedQtd.unmaskedValue,
            };

            $('#inserirQuantidade').buttonBusy(true);
            inventarioMovimentoService.atualizarItem($("#id").val(), item)
                .done((data) => {
                    itemId = item.itemId;
                    var index = _.findIndex(gridData, (gridItem) => gridItem.invItemId == item.itemId);
                    if (index === -1) {
                        clearInventarioItem();
                        return;
                    }

                    gridData[index].quantidadeContagem = item.quantidadeContagem;
                    loadGridData(false);
                    if (gridData.length >= index + 1) {
                        editItemByIndex(index + 1);
                    }
                    abp.notify.success(`Item atualizado com sucesso`);

                }).always(() => {
                    $('#inserirQuantidade').buttonBusy(false);
                    $('#quantidade').focus();
                });
        }

        function editItemByIndex(index) {
            let node = gridOptions.api.getRowNode(index);
            console.log(node);
            gridOptions.api.selectNode(node, true);
            gridOptions.api.ensureNodeVisible(node, 'middle');
            editarItem(node.data);
        }

        function editarItem(record) {
            console.log(record);
            itemId = record.invItemId;
            $('#invItemId').val(record.invItemId);
            $('#produto').val(record.produtoDescricao);
            $("#produtoId").val(record.produtoId);
            $('#lote').val(record.lote);
            $('#validade').val(moment(record.validade).format('L'));
            $('#quantidade').val(record.quantidadeContagem);
            maskedQtd.updateValue();
            $('#gridId').val(record.gridId);

            $(".novoItemTitle").addClass("hidden");
            $(".novoItemContainer").addClass("hidden");

            $(".itemInventarioTitle").removeClass("hidden");
            $(".itemInventarioContainer").removeClass("hidden");
        }

        function inserirQuantidadeNovo(e) {
            e.preventDefault();
            const item = {
                itemId: 0,
                produtoId: $('.selectProdutoNovo').val(),
                lote: $('.loteNovo').val(),
                validade: $('.validadeNovo').val(),
                quantidadeContagem: maskedQtdNovo.unmaskedValue,
            };

            $('#inserirQuantidadeNovo').buttonBusy(true);
            inventarioMovimentoService.atualizarItem($("#id").val(), item).done((data) => {
                gridData.push(data.returnObject);
                loadGridData(false);
                abp.notify.success(`${$('.selectProdutoNovo').select2("data")[0].text} adicionado com sucesso`);
            }).always(() => {
                clearNovo();
                $('#inserirQuantidadeNovo').buttonBusy(false);
                $('.quantidadeNovo').focus();
            });
        }

        function salvarItem(e) {
            e.preventDefault();
            location.href = '/mpa/inventarios';
        }

        function excluirItem(e, record) {
            const item = {
                itemId: record.invItemId,
            };
            $(e.currentTarget).buttonBusy(true);
            inventarioMovimentoService.excluirItem($("#id").val(), item)
                .done(() => {
                    getInventariosProdutosTable(false);
                    abp.notify.success(`${record.produtoDescricao} excluído com sucesso`);
                }).always(() => {
                    $(e.currentTarget).buttonBusy(false);
                });
        }

        function gerarInventario(e) {
            e.preventDefault();
            modalLoader.find(".loading").html(`Gerando Inventário <span>.</span><span>.</span><span>.</span>`);
            modalLoader.modal("show");
            inventarioMovimentoService.gerarInventario($('#estoqueId').val(), $('#grupoId').val(), $('#classeId').val(), $('#subClasseId').val(), $('#id').val())
                .done(function (data) {
                    if (data.errors.length > 0) {
                        modalLoader.modal("hide");
                        _ErrorModal.open({ erros: data.errors });
                    }
                    else {

                        $('#id').val(data.returnObject.id);
                        $('#numeroContagem').val(data.returnObject.codigo);
                        $('#dataInventario').val(moment(data.returnObject.dataInventario).format('L'));
                        //$('#itensJson').val(data.returnObject.itensJson);
                        $('#status').val(data.returnObject.status);
                        $('#gerarInventarioButton').text('Adicionar itens');

                        $("#novoItem").removeClass("hidden");
                        $(".itemInventarioTitle").removeClass("hidden");
                        $(".itemInventarioContainer").removeClass("hidden");

                        $('#estoqueId').attr('disabled', 'disabled');

                        loadGridData(true);
                        modalLoader.modal("hide");
                    }
                });
        }

        //function createInventarioProdutosTable($inventariosProdutosTable, type) {
        //    if ($inventariosProdutosTable.data("hikJtable")) {
        //        $inventariosProdutosTable.jtable("destroy");
        //    }

        //    let jTableProperties = {
        //        title: app.localize('Itens'),
        //        sorting: true,
        //        paging: true,
        //        recordsLoaded() {
        //            atualizaItemGrid();
        //        }
        //    }

        //    const statusField = {
        //        "sInvItem.Descricao": {
        //            title: app.localize('Status'),
        //            width: '7%',
        //            display: function (data) {
        //                if (data.record.statusInventarioItemId == 1) {
        //                    return '<span class="label label-danger">Pendente</span>';
        //                }

        //                if (data.record.statusInventarioItemId == 2) {
        //                    return '<span class="label label-warning">' + 'Contado' + '</span>';
        //                }

        //                if (data.record.statusInventarioItemId == 4) {
        //                    return '<span class="label label-success">' + 'Fechado' + '</span>';
        //                }
        //            }
        //        }
        //    };
        //    switch (type) {
        //        case TODOS: {
        //            jTableProperties.title = app.localize('Todos');
        //            jTableProperties.fields = _.extend({}, defaultFields, statusField, tableFields);
        //            jTableProperties.actions = {
        //                listAction:
        //                {
        //                    method: inventarioMovimentoService.listarItensTodosPorInventario
        //                },
        //            };
        //            break;
        //        }
        //        case CONTADOS: {
        //            jTableProperties.title = app.localize('Contados');
        //            jTableProperties.fields = _.extend({}, defaultFields, tableFields);
        //            jTableProperties.actions = {
        //                listAction:
        //                {
        //                    method: inventarioMovimentoService.listarItensContadosPorInventario
        //                },
        //            };
        //            break;
        //        }
        //        case PENDENTES: {
        //            jTableProperties.title = app.localize('Pendentes');
        //            jTableProperties.fields = _.extend({}, defaultFields, tableFields);
        //            jTableProperties.actions = {
        //                listAction:
        //                {
        //                    method: inventarioMovimentoService.listarItensPendentesPorInventario
        //                },
        //            };
        //            break;
        //        }
        //        default: {
        //            jTableProperties = null;
        //        }
        //    }

        //    if (jTableProperties === null) {
        //        return;
        //    }
        //    console.log(jTableProperties);
        //    $inventariosProdutosTable.jtable(jTableProperties);
        //    getInventariosProdutosTable(true);
        //}

        function createInventarioProdutosTable(type) {
            const statusColumn = {
                headerName: app.localize('Status'),
                field: 'statusInventarioItemId',
                filter: false,
                width: 50,
                suppressMenu: true,
                cellRenderer:'statusRenderer'

            };
            const cloneColumnDefs = _.clone(columnDefs);
            switch (type) {
                case TODOS: {
                    gridCallback = inventarioMovimentoService.listarItensTodosPorInventario;
                    cloneColumnDefs.splice(1, 0, statusColumn);
                    break;
                }
                case CONTADOS: {
                    gridCallback = inventarioMovimentoService.listarItensContadosPorInventario;
                    break;
                }
                case PENDENTES: {
                    gridCallback = inventarioMovimentoService.listarItensPendentesPorInventario;
                    break;
                }
            }

            if (gridOptions.api && gridOptions.api.destroy) {
                gridOptions.api.destroy();
            }

            gridOptions.columnDefs = cloneColumnDefs;


            const eGridDiv = document.querySelector('#inventariosProdutosTable');

            eGridDiv.style.width = $('.portlet.light').width();

            grid = new agGrid.Grid(eGridDiv, gridOptions);
            loadGridData(true);
        }

        let gridData = [];
        function loadGridData(refreshData) {
            if (refreshData) {
                if (_.isFunction(gridCallback)) {
                    var requestParameters = createRequestParams();
                    gridCallback(requestParameters).done((res) => {
                        gridData = res.items;
                        internalLoadData(true);
                    });
                }
            }
            else {
                internalLoadData(false);
            }


            function internalLoadData(enableSetTimeout) {
                if (enableSetTimeout) {
                    setTimeout(function () {
                        action();
                    }, 1000);
                } else {
                    action();
                }

                function action() {
                    var params = {
                        force: true
                    };
                    gridOptions.api.refreshCells(params);
                    const eGridDiv = document.querySelector('#inventariosProdutosTable');
                    eGridDiv.style.width = $('.portlet.light').width();
                    eGridDiv.style.width = '100%';
                    gridOptions.api.setRowData(gridData);
                    gridOptions.api.doLayout();
                    gridOptions.api.sizeColumnsToFit();

                    updateDashboardInventario();
                }
            }
        }

        function createColumns(type) {

        }

        function updateDashboardInventario() {
            if ($("#id").val() == null || $("#id").val() == "" || $("#id").val() == "0") {
                $(".todosSpan").text(0);
                $(".contadosSpan").text(0);
                $(".pendentesSpan").text(0);
                return;
            }
            abp.services.app.inventario.dashboardInventarioEstoque($("#id").val()).done(res => {
                $(".todosSpan").text(res.totalItems);
                $(".contadosSpan").text(res.totalContado);
                $(".pendentesSpan").text(res.totalPendente);
            }).fail(() => {
                $(".todosSpan").text(0);
                $(".contadosSpan").text(0);
                $(".pendentesSpan").text(0);
            });
        }

        function createRequestParams() {
            var prms = {};
            var _$filterForm = $('#preMovimentoInformationsForm');
            _$filterForm.serializeArray().map(function (x) {
                if (!_.isUndefined(prms[x.name])) {
                    if (!_.isArray(prms[x.name])) {
                        prms[x.name] = [prms[x.name]];
                    }
                    prms[x.name].push(x.value);
                } else {
                    prms[x.name] = x.value;
                }
            });
            console.log(prms);
            return prms;
        }

        //function getInventariosProdutosTable(isReload) {
        //    if (isReload) {
        //        let params = createRequestParams();
        //        $inventariosProdutosTable.jtable('load', params);
        //    }
        //    else {
        //        $inventariosProdutosTable.jtable('reload');
        //    }
        //    updateDashboardInventario();
        //}

        function clearInventarioItem() {
            $('#invItemId').val(null);
            $('#produto').val('');
            $("#produtoId").val(null);
            $('#lote').val('');
            $('#validade').val(null);
            $('#quantidade').val(null);
            maskedQtd.updateValue();
        }

        function novoItem(e) {
            e.preventDefault();
            $(".novoItemTitle").removeClass("hidden");
            $(".novoItemContainer").removeClass("hidden");

            $(".itemInventarioTitle").addClass("hidden");
            $(".itemInventarioContainer").addClass("hidden");
        }

        function clearNovo() {
            //$('.selectProdutoNovo').val(null).trigger("change").trigger("select2.change");
            $('.loteNovo').val('');
            $('.validadeNovo').val(null);
            $('.quantidadeNovo').val(null);
            maskedQtdNovo.updateValue();
        }
    });

})(jQuery);