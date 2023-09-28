(function ($) {
    app.modals.CriarOuEditarPreMovimentoItemModal = function () {

        var _preMovimentoItemService = abp.services.app.estoquePreMovimentoItem;
        var _estoqueSolicitacaoItemService = abp.services.app.estoqueSolicitacaoItem;
        var _modalManager;
        var _$PreMovimentoItemInformationForm = null;
        var _preMovimentoItemForm = null;

        $(document).ready(function () {
            $('#quantidadeAtendida').mask('000.000.000,00', { reverse: true });
        });


        this.init = function (modalManager) {
            _modalManager = modalManager;
            _preMovimentoItemForm = $("form[name='PreMovimentoItemForm']");           
            _$PreMovimentoItemInformationForm = _modalManager.getModal().find('form[name=PreMovimentoItemInformationsForm]');
            _$PreMovimentoItemInformationForm.validate();

            $('ul.ui-autocomplete').css('z-index', '2147483647 !important');
            $('.modal-dialog').css('width', '900px');

            var timerID = setInterval(function () { $('#LaboratorioId').focus(); clearInterval(timerID); }, 1000);

            abp.event.on('app.preMovimentoLoteValidadeModalSaved', function (result) {               
                selectSWWithDefaultValue('.loteValidade',
                    '/api/services/app/EstoqueLoteValidade/ListarProdutoDropdownPorLaboratorio',
                    [$('#produtoId'), $('#EstoqueId'), $('#LaboratorioId'), $('#loteValidadeId')]);
                $("#loteValidadeId").val(result.id).trigger("select2:selectById", result.id);
            })
        };


        this.save = function () {
            
            if (!_$PreMovimentoItemInformationForm.valid()) {
                return;
            }

            var preMovimentoItem = _$PreMovimentoItemInformationForm.serializeFormToObject();
            console.log(preMovimentoItem);
            preMovimentoItem.quantidade = retirarMascara(preMovimentoItem.quantidadeResidual);

            if ($('#itens').val() != '') {
                lista = JSON.parse($('#itens').val());
            }
            var totalQuantidadeAtendida = 0;
            if ($('#idGrid').val() != '') {

                for (var i = 0; i < lista.length; i++) {
                    if (lista[i].IdGrid == $('#idGrid').val()) {
                        lista[i].QuantidadeAtendida = preMovimentoItem.quantidadeAtendida;
                        lista[i].LotesValidadesJson = preMovimentoItem.LotesValidadesJson;
                        lista[i].NumerosSerieJson = preMovimentoItem.NumerosSerieJson;

                        totalQuantidadeAtendida += lista[i].QuantidadeAtendida;
                    }
                }
            }
            else {
                preMovimentoItem.IdGrid = lista.length == 0 ? 1 : lista[lista.length - 1].IdGrid + 1;
                totalQuantidadeAtendida += preMovimentoItem.QuantidadeAtendida;
                lista.push(preMovimentoItem);
            }

            if (parseFloat(totalQuantidadeAtendida) > parseFloat($("#quantidadeSolicitada").val())) {
                abp.notify.error("Não é possivel atender uma quantidade maior que a solicitada.");
            }
            else {
                $('#itens').val(JSON.stringify(lista));
                abp.event.trigger('app.CriarOuEditarPreMovimentoItemModalSaved');
                _modalManager.close();
            }

        };


        function retirarMascara(valor) {
            if (!valor) return;
            while (valor.indexOf('_') != -1) valor = valor.replace('_', '');
            valor = valor.replace(',', '.');

            return valor;
        }

        $('input[name="Validade"]').daterangepicker({
            "singleDatePicker": true,
            "showDropdowns": true,
            autoUpdateInput: false,
            changeYear: true,
            yearRange: 'c-10:c+10',
            showOn: "both",
            "locale": {
                "format": "MM/DD/YYYY",
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
                $('input[name="Validade"]').val(selDate.format('L')).addClass('form-control edited');
            });

        var _$loteValidadeTable2 = $('#loteValidadeTable');
        var _$numeroSerieTable = $('#numeroSerieTable');



        function retornarListaLoteValidade(filtro) {
            var js = $('#lotesValidadesJson').val();
            var res = _preMovimentoItemService.listarLoteValidadeJson({ data: js });
            return res;
        }


        _$loteValidadeTable2.jtable({

            title: app.localize('LoteValidade'),
            paging: true,
            sorting: true,
            edit: false,
            create: false,
            multiSorting: true,


            actions: {
                listAction: {
                    method: retornarListaLoteValidade
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },

                actions: {
                    title: app.localize('Actions'),
                    width: '8%',
                    sorting: false,
                    display: function (data) {
                        console.log(data);
                        var $span = $('<span></span>');
                        if (data.record.id == 0) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                .appendTo($span)
                                .click(function (e) {
                                    e.preventDefault();
                                    excluirLoteValidade(e, data);
                                });
                        }
                        return $span;
                    }
                },

                Laboratorio: {
                    title: app.localize('Laboratorio'),
                    width: '40%',
                    display: function (data) {
                        if (data.record) {
                            return data.record.laboratorio;
                        }
                    }
                },
                Lote: {
                    title: app.localize('Lote'),
                    width: '10%',
                    display: function (data) {
                        if (data.record) {
                            return data.record.lote;
                        }
                    }
                },

                Validade: {
                    title: app.localize('Validade2'),
                    width: '20%',
                    display: function (data) {
                        if (data.record.validade) {
                            return moment(data.record.validade).format("L");
                        }
                    }
                },

                Quantidade: {
                    title: app.localize('Quantidade'),
                    width: '10%',
                    display: function (data) {
                        if (data.record.quantidade) {
                            return posicionarDireita(data.record.quantidade.toFixed(2));
                        }
                    }
                },
            }
        });

        function getLoteValidadeTable(reload) {

            if (reload) {
                _$loteValidadeTable2.jtable('reload');
            } else {

                _$loteValidadeTable2.jtable('load', { filtro: $('#id').val() });
            }
        }

        getLoteValidadeTable();

        function retornarNumeroSerie(filtro) {

            var js = $('#numerosSerieJson').val();
            var res = _preMovimentoItemService.listarNumeroSerieJson({ data: js });
            return res;
        }

        _$numeroSerieTable.jtable({

            title: app.localize('NumeroSerie'),
            paging: true,
            sorting: true,
            edit: false,
            create: false,
            multiSorting: true,


            actions: {
                listAction: {
                    method: retornarNumeroSerie
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },

                actions: {

                    title: app.localize('Actions'),
                    width: '8%',
                    sorting: false,
                    display: function (data) {
                        var $span = $('<span></span>');
                        $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                            .appendTo($span)
                            .click(function (e) {
                                e.preventDefault();

                                excluirNumeroSerie(data);
                            });
                        return $span;
                    }
                },

                NumeroSerie: {
                    title: app.localize('NumeroSerie'),
                    width: '40%',
                    display: function (data) {
                        if (data.record) {
                            return data.record.numeroSerie;
                        }
                    }
                }
            }
        });

        function getNumeroSerieTable(reload) {

            if (reload) {
                _$numeroSerieTable.jtable('reload');
            } else {

                _$numeroSerieTable.jtable('load', { filtro: $('#id').val() });
            }
        }

        getNumeroSerieTable();

        $('#inserirProduto').on('click', function () {
            $('#inserir').buttonBusy(true);

            if (!_preMovimentoItemForm.valid()) {
                $('#inserir').buttonBusy(false);
                return;
            }

            if (parseFloat($("#inserirQuantidade").val()) > parseFloat($("#quantidadeResidual").val())) {
                abp.notify.error("Não é possível atender uma quantidade maior que a solicitada.");
                $('#inserir').buttonBusy(false);
                return;
            }

            $("#quantidadeAtendida").val(parseFloat($("#quantidadeAtendida").val()) + parseFloat($("#inserirQuantidade").val()));
            $("#quantidadeResidual").val(parseFloat($("#quantidadeSolicitada").val()) - parseFloat($("#quantidadeAtendida").val()));
        });

        $('#desfazerLancamento').on('click', function () {
            var solicitacaoItemId = $('#SolicitacaoItemId').val();
            _estoqueSolicitacaoItemService.obter(solicitacaoItemId).then(data => {
                console.log('data:', data);
                if (data) {
                    $('#quantidadeAtendida').val(data.quantidadeAtendida);
                    $('#quantidadeResidual').val(data.quantidade - data.quantidadeAtendida);
                }
            });
        });

        $('#inserir').on('click', function () {
            var lista = [];
            var listaTemp = [];
            $('#inserir').buttonBusy(true);
            if (!_preMovimentoItemForm.valid()) {
                $('#inserir').buttonBusy(false);
                return;
            }

            var preMovimentoItem = _preMovimentoItemForm.serializeFormToObject();

            if ($('#lotesValidadesJson').val() != '') {
                lista = JSON.parse($('#lotesValidadesJson').val());
            }
            listaTemp = _.clone(lista);

            var totalQuantidadeAtendida = 0;
            if ($('#idGridLoteValidade').val() != '') {

                for (var i = 0; i < lista.length; i++) {
                    if (listaTemp[i].IdGridLoteValidade == $('#idGridLoteValidade').val()) {
                        listaTemp[i].LaboratorioId = $('#LaboratorioId').val();
                        listaTemp[i].Lote = $('#LoteId').val();
                        listaTemp[i].Validade = $('#validadeId').val();
                        listaTemp[i].Quantidade = retirarMascara($('#quantidadeLote').val());
                        listaTemp[i].LoteValidadeId = $('#loteValidadeId').val();

                        totalQuantidadeAtendida += parseFloat(listaTemp[i].Quantidade);
                        break;
                    }

                }
            }
            else {
                preMovimentoItem.Quantidade = retirarMascara(preMovimentoItem.quantidadeLote);
                totalQuantidadeAtendida += parseFloat(preMovimentoItem.quantidadeLote);
                listaTemp.push(preMovimentoItem);
            }

            if (totalQuantidadeAtendida > parseFloat($("#quantidadeResidual").val().replace(',', '.'))) {
                abp.notify.error("Não é possivel atender uma quantidade maior que a solicitada.");
            }
            else {
                lista = listaTemp;
                $('#lotesValidadesJson').val(JSON.stringify(lista));

                $('#idGridLoteValidade').val('');
                $('#LaboratorioId').val(null).trigger("change");
                $('#LoteId').val('');
                $('#validadeId').val('');
                $('#quantidadeLote').val('');

                $('#loteValidadeId').val(null).trigger("change");

                getLoteValidadeTable();
                atulizarQuantidadeAtendida();

                $('#LaboratorioId').focus();
            }

            $('#inserir').buttonBusy(false);
        });

        $('#inserirNuneroSerie').on('click', function () {

            var _$NumeroSerieForm = $('form[name=NumeroSerieForm');
            var preMovimentoItem = _$NumeroSerieForm.serializeFormToObject();

            if ($('#numerosSerieJson').val() != '') {
                lista = JSON.parse($('#numerosSerieJson').val());
            }

            preMovimentoItem.IdGridNumeroSerie = lista.length == 0 ? 1 : lista[lista.length - 1].IdGridNumeroSerie + 1;
            lista.push(preMovimentoItem);

            $('#numeroSerie').val('');

            $('#numerosSerieJson').val(JSON.stringify(lista));

            getNumeroSerieTable();
            atualizarQuantidadeAtendidaNumeroSerie();
            $('#numeroSerie').focus();
        });

        function isSameLoteValidade(a, b) {
            return a.loteValidadeId == b.loteValidadeId && a.quantidadeLote == b.quantidade;
        }

        function excluirLoteValidade(e, data) {
            var rowData = $(e.currentTarget).parents("tr").data("record");
            abp.message.confirm(
                app.localize('DeleteWarning', rowData.lote),
                function (isConfirmed) {
                    if (isConfirmed) {
                        lista = JSON.parse($('#lotesValidadesJson').val());
                        for (var i = 0; i < lista.length; i++) {
                            if (isSameLoteValidade(lista[i], rowData)) {
                                lista.splice(i, 1);
                                $('#lotesValidadesJson').val(JSON.stringify(lista));
                                break;
                            }
                        }
                        getLoteValidadeTable();
                        atulizarQuantidadeAtendida();
                    }
                }
            );
        }


        function excluirNumeroSerie(data) {
            abp.message.confirm(
                app.localize('DeleteWarning', data.record.numeroSerie),
                function (isConfirmed) {
                    if (isConfirmed) {
                        lista = JSON.parse($('#numerosSerieJson').val());

                        for (var i = 0; i < lista.length; i++) {
                            if (lista[i].IdGridNumeroSerie == data.record.idGridNumeroSerie) {
                                lista.splice(i, 1);
                                $('#numerosSerieJson').val(JSON.stringify(lista));
                                break;
                            }
                        }
                        getNumeroSerieTable();
                        atualizarQuantidadeAtendidaNumeroSerie();
                    }
                }
            );
        }

        function atulizarQuantidadeAtendida() {
            lista = JSON.parse($('#lotesValidadesJson').val());
            var quantidade = parseFloat(0);
            for (var i = 0; i < lista.length; i++) {
                quantidade += parseFloat(lista[i].Quantidade);
            }

            $('#quantidadeAtendida').val(quantidade);
            atualizaQuantidadeResidual();
        }

        function atualizarQuantidadeAtendidaNumeroSerie() {
            lista = JSON.parse($('#numerosSerieJson').val());
            $('#quantidadeAtendida').val(lista.length);
            atualizaQuantidadeResidual();
        }

        function atualizaQuantidadeResidual() {
            $('#quantidadeResidual').val(
                numeral($("#quantidadeSolicitada").val()).difference($("#quantidadeAtendida").val()));
        }

        selectSW('.selectProduto', "/api/services/app/produto/ListarProdutoDropdown");

        selectSWMultiplosFiltros('.loteValidade', '/api/services/app/EstoqueLoteValidade/ListarProdutoDropdownPorLaboratorio', ['produtoId', 'EstoqueId', 'LaboratorioId', 'loteValidadeId']);


        $('#LaboratorioId').on('change', function () {
            selectSWMultiplosFiltros('.loteValidade', '/api/services/app/EstoqueLoteValidade/ListarProdutoDropdownPorLaboratorio', ['produtoId', 'EstoqueId', 'LaboratorioId', 'loteValidadeId']);

        });

        $('#loteValidadeId').on('select2:select', function () {
            $('#quantidadeLote').focus();
        });

        selectSW('.selectLaboratorio', "/api/services/app/produtoLaboratorio/ListarDropdown");

        var _createOrEditLoteValidadeModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/PreMovimentos/CriarOuEditarLoteValidadeModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/PreMovimentos/_CriarOuEditarLoteValidadeModal.js',
            modalClass: 'CriarOuEditarEstoquePreMovimentoLoteValidadeDtoModalViewModel'
        });

        $('#btn-novo-LoteValidade').click(function (e) {
            e.preventDefault()
            _createOrEditLoteValidadeModal.open({ preMovimentoItemId: $('#Id').val() });
        })
    };
})(jQuery);