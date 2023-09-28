
(function ($) {
    $(function () {
        const estoqueImportacaoProduto = $('#estoqueImportacaoProdutoTable');
        const estoqueImportacaoProdutoService = abp.services.app.estoqueImportacaoProduto;
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
        //const maskedQtdNovo = IMask(document.querySelector('.quantidadeNovo'), numberMaskTemplate);
        const maskedFator = IMask(document.querySelector('#fator'), numberMaskTemplate);
        const _ErrorModal = new app.ModalManager({ viewUrl: abp.appPath + 'Mpa/Erros/ExibirErros' });
        const modalLoader = $(".modal.loader").modal({ backdrop: 'static', show: false });
        let itemId = null;
        let gridCallback = null;
        $("#fator").mask('000.000.000.000', { reverse: true });

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
            $('<button class="btn btn-warning btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                .appendTo($span)
                .click(function (e) {
                    e.preventDefault();
                    gridOptions.api.selectNode(params.node, true);
                    gridOptions.api.ensureNodeVisible(params.node, 'middle');
                    editarItem(params.data);
                });

            this.eGui = $span[0];
            //this.eGui.innerHTML = params.value;
        };

        actionRenderer.prototype.getGui = function () {
            return this.eGui;
        };

        let gridOptions = {
            components: {
                'qtdCellRenderer': qtdCellRenderer,
                'actionRenderer': actionRenderer,
            },
            defaultColDef: {
                filter: true,
                sortable: false,
                floatingFilter: true,
            },
            //pagination: true,
            enableBrowserTooltips: false,
            tooltipShowDelay: 0,
            height: "600",
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
                headerName: app.localize('CNPJNota'),
                field: "cnpjNota",
                width: 100,
                filter: 'agTextColumnFilter',
                floatingFilter: true,
                resizable: false
            },
            {
                headerName: app.localize('UnidadeDescricao'),
                field: "unidadeDescricao",
                width: 70,
                resizable: false,
                filter: 'agTextColumnFilter',
                floatingFilter: true,
            },
            {
                headerName: app.localize('Fornecedor'),
                field: "forncecedorNomeFantasia",
                width: 100,
                resizable: false,
                filter: 'agTextColumnFilter',
                floatingFilter: true,
            },
            {
                headerName: app.localize('Fator'),
                field: "fator",
                width: 100,
                resizable: false,
                filter: false,
                cellRenderer: 'qtdCellRenderer'
            }];

        attachEventListeners();
        createEstoqueImportacaoProdutoTable();

        function attachEventListeners() {

            selectSW("#unidadeId", '/api/services/app/unidade//ListarDropdown');

            $('#btnAlterar').click(alterar);

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

        function editItemByIndex(index) {
            let node = gridOptions.api.getRowNode(index);
            console.log(node);
            gridOptions.api.selectNode(node, true);
            gridOptions.api.ensureNodeVisible(node, 'middle');
            editarItem(node.data);
        }

        function editarItem(record) {
            $('#id').val(record.id);
            $('#produto').val(record.produtoDescricao);
            $("#produtoId").val(record.produtoId);
            $('#cnpjNota').val(record.cnpjNota);
            $('#fator').val(record.fator);
            $("#unidadeId").val(record.unidadeId);
            var option = $("<option selected='selected'></option>").val(record.unidadeId).text(record.unidadeDescricao);
            $("#unidadeId").val(record.unidadeId).append(option).trigger("change");
            maskedFator.updateValue();
        }

        function alterar(e) {
            e.preventDefault();
            $("#btnAlterar").buttonBusy(true);
            const item = {
                id: $("#id").val(),
                produtoId: $("#produtoId").val(),
                unidadeId: $('#unidadeId').val(),
                fator: maskedFator.unmaskedValue,
            };

            estoqueImportacaoProdutoService.criarOuEditar(item).done((data) => {
                //gridData.push(data.returnObject);
                loadGridData(true);
                abp.notify.success(`${$('#produto').val()} adicionado com sucesso`);
            }).always(() => {
                clearItem();
                $('#btnAlterar').buttonBusy(false);
                $('.quantidadeNovo').focus();
            });
        }

        function createEstoqueImportacaoProdutoTable() {
            const cloneColumnDefs = _.clone(columnDefs);
            gridCallback = estoqueImportacaoProdutoService.listar;

            if (gridOptions.api && gridOptions.api.destroy) {
                gridOptions.api.destroy();
            }

            gridOptions.columnDefs = cloneColumnDefs;


            const eGridDiv = document.querySelector('#estoqueImportacaoProdutoTable');

            eGridDiv.style.width = $('.portlet.light').width();

            grid = new agGrid.Grid(eGridDiv, gridOptions);
            loadGridData(true);
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
                    const eGridDiv = document.querySelector('#estoqueImportacaoProdutoTable');
                    eGridDiv.style.width = $('.portlet.light').width();
                    eGridDiv.style.width = '100%';
                    gridOptions.api.setRowData(gridData);
                    gridOptions.api.doLayout();
                    gridOptions.api.sizeColumnsToFit();

                }
            }
        }

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

        function clearItem() {
            $('#id').val(null);
            $('#produto').val('');
            $("#produtoId").val(null);
            $('#cnpjNota').val('');
            $('#fator').val(null);
            $("#unidadeId").val(null);
            $("#unidadeId").trigger("change").trigger("select2:change");
            maskedFator.updateValue();
        }
    });

})(jQuery);