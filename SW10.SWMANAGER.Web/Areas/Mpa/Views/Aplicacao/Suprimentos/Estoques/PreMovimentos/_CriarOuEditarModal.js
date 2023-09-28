
(function ($) {
    numeral.locale("pt-br");
    $(function () {
        $('body').addClass('page-sidebar-closed');

        $('.page-sidebar-menu').addClass('page-sidebar-menu-closed');

        const currencyMaskTemplate = {
            mask: 'R$num',
            blocks: {
                num: {
                    mask: Number,
                    thousandsSeparator: '.',
                    scale: 2,	// digits after decimal
                    signed: true, // allow negative
                    normalizeZeros: true,  // appends or removes zeros at ends
                    radix: ',',  // fractional delimiter
                    padFractionalZeros: true,  // if true, then pads zeros at end to the length of scale
                    allowDecimal: true
                }
            },
        };

        const numberMaskTemplate = {
            mask: 'num',
            blocks: {
                num: {
                    mask: Number,
                    thousandsSeparator: '.',
                    scale: 2,	// digits after decimal
                    signed: true, // allow negative
                    normalizeZeros: true,  // appends or removes zeros at ends
                    radix: ',',  // fractional delimiter
                    padFractionalZeros: true,  // if true, then pads zeros at end to the length of scale
                    allowDecimal: true,

                }
            },
        };
        var maskFields = {};
        $(document).ready(function () {
            //IMask(document.querySelector("#totalDocumento"), maskTemplate);

            var currencyFields = [
                'totalDocumento', 'valorICMS', 'descontoPer',
                'ValorAcrescimo', //'frete',
                'ValorFrete', 'totalProdutoId', //'ValorDesconto',
                'valor', 'juros', 'multa',
                'acrescimoDecrescimo', 'total', 'parcelaInicial'
            ];
            var numericFields = ['ICMSPer', 'FretePer'];

            _.forEach(currencyFields, (item) => {
                const el = document.querySelector(`#${item}`);
                if (el) {
                    maskFields[item] = IMask(el, currencyMaskTemplate);
                }
            })
            _.forEach(numericFields, (item) => {
                const el = document.querySelector(`#${item}`);
                if (el) {
                    maskFields[item] = IMask(el, numberMaskTemplate);
                }
            })

            CamposRequeridos();
            showTipoMovimento($('#EstTipoMovimentoId').val(), !($('#id') != '0' || $('#id') != ''));


            abp.event.on("selecionaNotaPendente", onSelecionaNotaPendente);

        });

        //$('.modal-dialog').css('width', '1800px');
        const modalLoader = $(".modal.loader").modal({ backdrop: 'static', show: false });


        $('.chosen-select').chosen({ no_results_text: app.localize("NotFound"), width: '100%' });
        $.validator.setDefaults({ ignore: ":hidden:not(select)" });

        // validation of chosen on change
        $('ul.ui-autocomplete').css('z-index', '2147483647 !important');


        //$('#totalDocumento').change(function () {
        //    CalcularValorICMS();
        //});

        //$('#ICMSPer').change(function () {
        //    CalcularValorICMS();
        //});

        function CalcularValorICMS() {
            var valorIcms = parseFloat(trataMask(maskFields, 'totalDocumento')) * parseFloat(trataMask(maskFields, 'ICMSPer')) / 100;
            maskFields['valorICMS'].unmaskedValue = valorIcms.toString();
        }


        //$('#totalProdutoId, #freteId, #ValorDesconto, #ValorAcrescimo').change(function () {
        //    CalcularTotalProduto();
        //});


        function CalcularTotalProduto() {
            var totalProduto = ($('#totalProdutoId').val() != '') ? parseFloat(trataMask(maskFields, 'totalProdutoId')) : 0;

            var valorFrete = ($('#ValorFrete').val() != '') ? parseFloat(trataMask(maskFields, 'ValorFrete')) : 0;
            var valorDesconto = ($('#ValorDesconto').val() != '') ? parseFloat(trataMask(maskFields, 'ValorDesconto')) : 0;
            var valorAcrescimo = ($('#ValorAcrescimo').val() != '') ? parseFloat(trataMask(maskFields, 'ValorAcrescimo')) : 0;

            var valorTotalProduto = totalProduto + valorFrete - valorDesconto + valorAcrescimo;
            maskFields['totalDocumento'].unmaskedValue = valorTotalProduto.toString();
            CalcularValorICMS();
        }

        function CalcularValorFrete() {
            var valorDesconto = 0;
            if ($('#FretePer').val() != '') {
                valorDesconto = parseFloat(trataMask(maskFields, 'freteId')) * parseFloat(trataMask(maskFields, 'FretePer')) / 100;
            }
            var valorFrete = parseFloat(trataMask(maskFields, 'freteId')) - valorDesconto;
            maskFields['ValorFrete'].unmaskedValue = valorFrete.toString();
        }

        function CalcularValorDesconto() {
            var valorDesconto = parseFloat(trataMask(maskFields, 'totalDocumento')) * parseFloat(trataMask(maskFields, 'DescontoPer')) / 100;
            maskFields['ValorDesconto'].unmaskedValue = valorDesconto.toString();
        }

        $('#empresa-search').autocomplete({
            minLength: 2,
            delay: 0,
            source: function (request, response) {
                var term = $('#empresa-search').val();
                var url = '/mpa/empresas/autocompleteDescricao';


                var fullUrl = url + '/?term=' + term;
                $.getJSON(fullUrl, function (data) {
                    if (data.result.length == 0) {
                        $('#empresa-Id').val(0);
                        $("#empresa-search").focus();
                        abp.notify.info(app.localize("ListaVazia"));
                        return false;
                    };
                    response($.map(data.result, function (item) {
                        $('#empresa-Id').val(0);
                        return {
                            label: item.nome,
                            value: item.nome,
                            realValue: item.id
                        };
                    }));
                });
            },
            select: function (event, ui) {
                $('#empresa-Id').val(ui.item.realValue);
                $('#empresa-search').val(ui.item.value);
                //$('.save-button').removeAttr('disabled');
                return false;
            },
            change: function (event, ui) {
                event.preventDefault();
                if (ui.item == null) {
                    //$('.save-button').attr('disabled', 'disabled');
                    $('#empresa-Id').val(0);
                    $("#empresa-search").val('').focus();
                    abp.notify.info(app.localize("EstadoInvalido"));
                    return false;
                }
            },
        });

        $('#fornecedor-search').autocomplete({
            minLength: 2,
            delay: 0,
            source: function (request, response) {
                var term = $('#fornecedor-search').val();
                var url = '/mpa/fornecedores/autocomplete';


                var fullUrl = url + '/?term=' + term;
                $.getJSON(fullUrl, function (data) {
                    if (data.result.length == 0) {
                        $('#fornecedor-Id').val(0);
                        $("#fornecedor-search").focus();
                        abp.notify.info(app.localize("ListaVazia"));
                        return false;
                    };
                    response($.map(data.result, function (item) {
                        $('#fornecedor-Id').val(0);
                        return {
                            label: item.nome,
                            value: item.nome,
                            realValue: item.id
                        };
                    }));
                });
            },
            select: function (event, ui) {
                $('#fornecedor-Id').val(ui.item.realValue);
                $('#fornecedor-search').val(ui.item.value);
                //$('.save-button').removeAttr('disabled');
                return false;
            },
            change: function (event, ui) {
                event.preventDefault();
                if (ui.item == null) {
                    //$('.save-button').attr('disabled', 'disabled');
                    $('#fornecedor-Id').val(0);
                    $("#fornecedor-search").val('').focus();
                    abp.notify.info(app.localize("EstadoInvalido"));
                    return false;
                }
            },
        });

        $('#centroCusto-search').autocomplete({
            minLength: 2,
            delay: 0,
            source: function (request, response) {
                var term = $('#centroCusto-search').val();
                var url = '/mpa/centrosCustos/autocomplete';

                var fullUrl = url + '/?term=' + term;
                $.getJSON(fullUrl, function (data) {
                    if (data.result.length == 0) {
                        $('#centroCusto-Id').val(0);
                        $("#centroCusto-search").focus();
                        abp.notify.info(app.localize("ListaVazia"));
                        return false;
                    };
                    response($.map(data.result, function (item) {
                        $('#centroCusto-Id').val(0);
                        return {
                            label: item.nome,
                            value: item.nome,
                            realValue: item.id
                        };
                    }));
                });
            },
            select: function (event, ui) {
                $('#centroCusto-Id').val(ui.item.realValue);
                $('#centroCusto-search').val(ui.item.value);
                //$('.save-button').removeAttr('disabled');
                return false;
            },
            change: function (event, ui) {
                event.preventDefault();
                if (ui.item == null) {
                    //$('.save-button').attr('disabled', 'disabled');
                    $('#centroCusto-Id').val(0);
                    $("#centroCusto-search").val('').focus();
                    abp.notify.info(app.localize("CentroCustoInvalido"));
                    return false;
                }
            },
        });

        $('#Movimento').on('load', function () {
            var d = new Date();
            var n = d.getDate();
            $('#movimento').val(moment().format("L LT"));
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Suprimentos.Estoque.PreMovimento.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Suprimentos.Estoque.PreMovimento.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Suprimentos.Estoque.PreMovimento.Delete')
        };

        var iValidador = {
            init: function () {
                // Execute seus códigos iniciais
                // ...
                //alert('Entrou no validador agora!');
                // Chame as funções desejadas...
                iValidador.outraFuncao();
            },
            outraFuncao: function () {
                // Códigos desejados...
            }
        };

        var _preMovimentoService = abp.services.app.estoquePreMovimento;
        var _movimentoService = abp.services.app.estoqueMovimento;

        var _estoquePreMovimentoItemService = abp.services.app.estoquePreMovimentoItem;
        var _$EstoquePreMovimentoItemTable = $('#EstoquePreMovimentoItemTable');
        var _$valesTable = $('#valesTable');
        var _$notaTable = $('#notaTable');
        var _$itensConsignadosTable = $('#itensConsignadosTable');

        var _createOrEditPreMovimentoItemModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/PreMovimentos/CriarOuEditarPreMovimentoItemModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/PreMovimentos/_CriarOuEditarPreMovimentoItemModal.js',
            modalClass: 'CriarOuEditarPreMovimentoItemModal'
        });

        var _createOrEditLoteValidadeModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/PreMovimentos/InformarLoteValidadeModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/PreMovimentos/_InformarLoteValidade.js',
            modalClass: 'EstoquePreMovimentoLoteValidadeProduto'
        });


        var _notasPendentesModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Sefaz/NotasPendente',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/sefaz/notas-pendentes.js',
            modalClass: 'sefazNotasPendentes',
            focusFunction: () => { }
        });

        var _ErrorModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Erros/ExibirErros',
        });

        var _createOrEditLoteValidadeProdutoModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/PreMovimentos/InformarLoteValidadeProdutoModal',
            modalClass: 'EstoquePreMovimentoLoteValidadeProdutoViewModel'
        });


        var _importacaoProdutosModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/PreMovimentos/CarregarRelacionarImportacaoProdutos',
            modalClass: 'ImportacaoProdutosViewModel'
        });

        $('#btn-buscarNotas').click(function (e) {
            e.preventDefault()
            sincronizarNotasSefaz();
        });


        var _lotesValidades = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/PreMovimentos/BuscarLotesValidades'
            , modalClass: 'ImportacaoProdutosViewModel'

        });

        $('#btnLoteValidade').click(function (e) {
            e.preventDefault();


            _lotesValidades.open({ preMovimentoId: $('#id').val(), chave: $('#NumeroNotaFiscal').val(), empresaId: $('#EmpresaId').val() });
        });


        $('#btn-notaPendentes').click(function (e) {
            e.preventDefault();
            _notasPendentesModal.open({ empresaId: $('#EmpresaId').val() });
        })


        function sincronizarNotasSefaz() {

            $('#btn-buscarNotas').buttonBusy(true);
            //abp.message.confirmHtml("", (app.localize('ConfirmSincronizarNotaFiscal')), (isConfirmed) => {


            //});

            let tentativas = 1;
            const maximoTentativas = 1;
            executaBuscaSefaz(tentativas, maximoTentativas);

            function executaBuscaSefaz(tentativas, maximoTentativas) {
                $('.selectTipoMovimento')
                    .append($("<option/>") //add option tag in select
                        .val(1) //set value for option to post it
                        .text(" - Nota Fiscal"))
                    .val(1)
                    .trigger("change");
                modalLoader.find(".loading").html(`Carregando nota <span>.</span><span>.</span><span>.</span>`);
                modalLoader.modal("show");
                $.ajax({
                    url: "/PreMovimentos/BuscarNfe",
                    data: { chave: $('#NumeroNotaFiscal').val(), empresaId: $('#EmpresaId').val(), estoqueId: $('#EstoqueId').val(), movimentoId: $('#id').val() },
                    type: "GET",
                    timeout: 864000,
                    cache: false,
                    async: true,
                    beforeSend: function () {
                        $('#btn-sincronizar').buttonBusy(true);
                    },
                    complete: function () {
                        $('#btn-sincronizar').buttonBusy(false);
                    },
                    success: function (result) {
                        if (result.Errors.length > 0) {
                            if (tentativas < maximoTentativas) {
                                executaBuscaSefaz(++tentativas, maximoTentativas);
                            }
                            else {
                                modalLoader.modal("hide");
                                _ErrorModal.open({ erros: result.Errors });
                                $('#btn-buscarNotas').buttonBusy(false);
                            }
                        }
                        else {
                            $('#DocumentoId').val(result.ReturnObject.Documento);
                            $('#SerieId').val(result.ReturnObject.Serie);
                            $('#Emissao').val(moment(result.ReturnObject.Emissao).format('L'));
                            maskFields['valorICMS'].unmaskedValue = result.ReturnObject.ValorICMS.toString();
                            maskFields['totalDocumento'].unmaskedValue = result.ReturnObject.TotalDocumento.toString()
                            maskFields['totalProdutoId'].unmaskedValue = result.ReturnObject.TotalProduto.toString()
                            maskFields['descontoPer'].unmaskedValue = result.ReturnObject.DescontoPer.toString()
                            maskFields['ValorAcrescimo'].unmaskedValue = result.ReturnObject.AcrescimoDecrescimo.toString()
                            maskFields['ValorAcrescimo'].unmaskedValue = result.ReturnObject.AcrescimoDecrescimo.toString()
                            $('#EstTipoMovimentoId').val(result.ReturnObject.EstTipoMovimentoId).trigger("change");
                            $('#NFeItens').val(result.ReturnObject.NFeItens);

                            $('.selectTipoMovimento')
                                .append($("<option/>") //add option tag in select
                                    .val(1) //set value for option to post it
                                    .text(" - Nota Fiscal"))
                                .val(1)
                                .trigger("change");

                            // $('#CFOPId').val(result.ReturnObject.CFOPId).trigger("change");

                            $('#CFOPId')
                                .append($("<option/>") //add option tag in select
                                    .val(result.ReturnObject.CFOP.Id) //set value for option to post it
                                    .text(result.ReturnObject.CFOP.Descricao))
                                .val(result.ReturnObject.CFOP.Id)
                                .trigger("change");





                            $('#id').val(result.ReturnObject.Id);
                            $('#FornecedorId')
                                .append($("<option/>") //add option tag in select
                                    .val(result.ReturnObject.Fornecedor.Id) //set value for option to post it
                                    .text(result.ReturnObject.Fornecedor.Descricao))
                                .val(result.ReturnObject.Fornecedor.Id)
                                .trigger("change");

                            if (result.ReturnObject.Frete_Forncedor != undefined && result.ReturnObject.Frete_Forncedor != null) {

                                $('#Frete_FornecedorId')
                                    .append($("<option/>") //add option tag in select
                                        .val(result.ReturnObject.Frete_Forncedor.Id) //set value for option to post it
                                        .text(result.ReturnObject.Frete_Forncedor.Descricao))
                                    .val(result.ReturnObject.Frete_Forncedor.Id)
                                    .trigger("change");
                            }


                            if (result.ReturnObject.TipoFrete != undefined && result.ReturnObject.TipoFrete != null) {

                                $('#TipoFreteId')
                                    .append($("<option/>") //add option tag in select
                                        .val(result.ReturnObject.TipoFrete.Id) //set value for option to post it
                                        .text(result.ReturnObject.TipoFrete.Descricao))
                                    .val(result.ReturnObject.TipoFrete.Id)
                                    .trigger("change");
                            }



                            if (result.ReturnObject.ImportacaoProdutos.length > 0) {

                                _importacaoProdutosModal.open({ importacaoProdutosRegistrados: JSON.stringify(result.ReturnObject.ImportacaoProdutos), fornecedorId: result.ReturnObject.Fornecedor.Id, CNPJNota: result.ReturnObject.CNPJNota });
                            }
                            else {
                                getEstoquePreMovimentoItemTable();
                            }


                            $('#lancamentosJson').val(result.ReturnObject.LancamentosJson);
                            getRegistros();

                            modalLoader.modal("hide");
                            $('#btn-buscarNotas').buttonBusy(false);

                        }
                    }
                });
            }
        }


        $('#btn-novo-PreMovimentoItem').click(function (e) {
            e.preventDefault()

            var _$preMovimentoInformationsForm = $('form[name=preMovimentoInformationsForm]');

            _$preMovimentoInformationsForm.validate();

            if (!_$preMovimentoInformationsForm.valid()) {
                return;
            }

            var preMovimento = _$preMovimentoInformationsForm.serializeFormToObject();

            preMovimento.ValorICMS = trataMask(maskFields, 'valorICMS');
            preMovimento.TotalDocumento = trataMask(maskFields, 'totalDocumento');
            // preMovimento.ICMSPer = retirarMascara(preMovimento.ICMSPer);
            preMovimento.DescontoPer = trataMask(maskFields, 'descontoPer');
            // preMovimento.ValorDesconto = retirarMascara(preMovimento.ValorDesconto);
            preMovimento.AcrescimoDecrescimo = trataMask(maskFields, 'ValorAcrescimo');
            //  preMovimento.FretePer = retirarMascara(preMovimento.FretePer);
            preMovimento.ValorFrete = trataMask(maskFields, 'ValorFrete');
            preMovimento.IsEntrada = true;

            //  _modalManager.setBusy(true);
            var editMode = $('#is-edit-mode').val();

            if ($('#id').val() == '' || $('#id').val() == '0') {

                _preMovimentoService.criarGetIdEntrada(preMovimento)
                    .done(function (data) {
                        abp.notify.info(app.localize('SavedSuccessfully'));
                        $('#id').val(data.id);

                        _createOrEditPreMovimentoItemModal.open({ preMovimentoId: $('#id').val(), id: 0 });

                    })
                    .always(function () {
                        //  _modalManager.setBusy(false);
                    });
            }
            else {

                _createOrEditPreMovimentoItemModal.open({ preMovimentoId: $('#id').val(), id: 0 });


                //location.href = '/Mpa/PreMovimentos/CriarOuEditarPreMovimentoItemModal';
            }
        });

        $('#salvar-PreMovimento').click(function (e) {
            e.preventDefault()

            if ($("#CentroCustoId").val() == "0") {
                $("#CentroCustoId").val(null);
            }

            var _$preMovimentoInformationsForm = $('form[name=preMovimentoInformationsForm');

            _$preMovimentoInformationsForm.validate();

            if (!_$preMovimentoInformationsForm.valid()) {
                return;
            }

            var preMovimento = _$preMovimentoInformationsForm.serializeFormToObject();

            preMovimento.TotalDocumento = trataMask(maskFields, 'totalDocumento');
            preMovimento.TotalProduto = trataMask(maskFields, 'totalProdutoId');
            //  preMovimento.ICMSPer = retirarMascara(preMovimento.ICMSPer);
            preMovimento.DescontoPer = trataMask(maskFields, 'descontoPer');
            preMovimento.ValorDesconto = trataMask(maskFields, 'descontoPer');
            //   preMovimento.ValorDesconto = retirarMascara(preMovimento.ValorDesconto);
            preMovimento.ValorAcrescimo = trataMask(maskFields, 'ValorAcrescimo');
            preMovimento.AcrescimoDecrescimo = trataMask(maskFields, 'acrescimoDecrescimo');
            //   preMovimento.FretePer = retirarMascara(preMovimento.FretePer);
            preMovimento.ValorFrete = trataMask(maskFields, 'ValorFrete');
            //    preMovimento.Frete = retirarMascara(preMovimento.Frete);
            preMovimento.ValorICMS = trataMask(maskFields, 'valorICMS');

            preMovimento.ParcelaInicial = trataMask(maskFields, 'parcelaInicial');
            preMovimento.Valor = trataMask(maskFields, 'valor');
            preMovimento.Juros = trataMask(maskFields, 'juros');
            preMovimento.Multa = trataMask(maskFields, 'multa');
            preMovimento.AcrescimoDescrecimo = trataMask(maskFields, 'acrescimoDecrescimo');
            preMovimento.Total = trataMask(maskFields, 'total');
            if ($("#Emissao").val()) {
                preMovimento.Emissao = moment($("#Emissao").val(), "DD/MM/YYYY").format();
            }
            if ($("#Movimento").val()) {
                preMovimento.Movimento = moment($("#Movimento").val(), "DD/MM/YYYY").format();
            }
            if ($("#dataCadastro").val()) {
                preMovimento.DataCadastro = moment($("#DataCadastro").val(), "DD/MM/YYYY").format();
            }

            if ($("#dataPrimeiraParcela").val()) {
                preMovimento.DataPrimeiraParcela = moment($("#dataPrimeiraParcela").val(), "DD/MM/YYYY").format();
            }

            preMovimento.IsEntrada = true;
            //  _modalManager.setBusy(true);
            var editMode = $('#is-edit-mode').val();

            //  _preMovimentoService.criarOuEditar(preMovimento)
            //      .done(function (data) {

            preMovimento.fornecedorId = preMovimento.FornecedorId;

            debugger;
            $.ajax({
                url: "/PreMovimentos/Salvar",
                data: { input: JSON.stringify(preMovimento) },
                type: "POST",
                timeout: 864000,
                cache: false,
                async: false,
                beforeSend: function (result) {
                    //  $('#btn-sincronizar').buttonBusy(true);
                },
                complete: function (result) {
                    //$('#btn-sincronizar').buttonBusy(false);
                },
                success: function (result) {
                    if (result.Result) {
                        result = result.Result;
                    } else if (result.result) {
                        result = result.result;
                    }

                    if (result.Errors.length > 0) {
                        _ErrorModal.open({ erros: result.Errors });
                    }
                    else {

                        abp.notify.info(app.localize('SavedSuccessfully'));
                        $('#id').val(result.ReturnObject.Id);

                        if (result.ReturnObject.possuiLoteValidade) {
                            _createOrEditLoteValidadeModal.open({ preMovimentoId: result.ReturnObject.Id });
                        }
                        else {
                            abp.message.confirmHtml("",
                                "Deseja confirmar o pré movimento?",
                                function (isConfirmed) {
                                    if (isConfirmed) {
                                        console.log(result, result.ReturnObject.Id);
                                        _movimentoService.gerarMovimentoEntrada(result.ReturnObject.Id)
                                            .done(function (data) {
                                                if (data.errors.length > 0) {
                                                    _ErrorModal.open({ erros: data.errors });
                                                }
                                                else {
                                                    abp.notify.info(app.localize('SavedSuccessfully'));
                                                    location.href = '/mpa/preMovimentos';
                                                }
                                            });
                                    }
                                    else {
                                        location.href = '/mpa/preMovimentos';
                                    }
                                }
                            );




                            //  $('#divConfirmarEntrada').show();


                        }
                    }

                }
                , error: function (result, execption, a, b, c, d) {

                    //$('#btn-sincronizar').buttonBusy(false);
                },

                fail: function (result) {

                    //$('#btn-sincronizar').buttonBusy(false);
                },





                //.always(function () {
                //  _modalManager.setBusy(false);
                //});

            });
        });

        //$('#ConfirmarEntradaButton').click(function (e) {
        //    e.preventDefault()

        //    _movimentoService.gerarMovimentoEntrada($('#id').val())
        //        .done(function (data) {

        //            if (data.errors.length > 0) {
        //                _ErrorModal.open({ erros: data.errors });
        //            }
        //            else {
        //                 abp.notify.info(app.localize('SavedSuccessfully'));
        //                 location.href = '/mpa/preMovimentos';
        //            }
        //        });
        //});

        function retirarMascara(valor) {

            while (valor.indexOf('_') != -1) valor = valor.replace('_', '');
            while (valor.indexOf('.') != -1) valor = valor.replace('.', '');

            valor = valor.replace(',', '.');

            return valor;
        }

        function salvar(e) {

        }


        $('input[name="Movimento"]').daterangepicker({
            "singleDatePicker": true,
            "showDropdowns": true,
            autoUpdateInput: false,
            maxDate: new Date(),
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
                $('input[name="Movimento"]').val(selDate.format('L')).addClass('form-control edited');
                // obterIdade(selDate);
            });

        $('input[name="Emissao"]').daterangepicker({
            "singleDatePicker": true,
            "showDropdowns": true,
            autoUpdateInput: false,
            maxDate: new Date(),
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
                $('input[name="Emissao"]').val(selDate.format('L')).addClass('form-control edited');
            });


        abp.event.on('app.CriarOuEditarPreMovimentoItemModalSaved', function () {
            getEstoquePreMovimentoItemTable();
        });

        var _estoquePreMovimentoService = abp.services.app.estoquePreMovimento;

        var _modalManager;

        this.init = function (modalManager) {
            _modalManager = modalManager;

        };

        $('.close').on('click', function () {
            location.href = '/mpa/preMovimentos';
        });

        $('.close-button').on('click', function () {
            location.href = '/mpa/preMovimentos';
        });

        _$EstoquePreMovimentoItemTable.jtable
            ({
                title: app.localize('Item'),
                paging: true,
                sorting: true,
                edit: false,
                create: false,
                multiSorting: true,
                actions:
                {
                    listAction:
                    {
                        method: _estoquePreMovimentoService.listarItens
                    },
                },
                fields:
                {
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

                            if (_permissions.edit && $('#PreMovimentoEstadoId').val() != 2) {
                                $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                    .appendTo($span)
                                    .click(function (e) {
                                        e.preventDefault();
                                        _createOrEditPreMovimentoItemModal.open({ id: data.record.id, preMovimentoId: $('#id').val() });




                                    });
                            }

                            if (_permissions.delete && $('#PreMovimentoEstadoId').val() != 2) {

                                $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                    .appendTo($span)
                                    .click(function (e) {
                                        e.preventDefault();
                                        deletePreMovimentoItem(data.record);
                                    });
                            }

                            if (data.record.isValidade || data.record.isLote) {
                                $('<button class="btn btn-default btn-xs" title="' + app.localize('LoteValidade') + '"><i class="fa fa-calendar"></i></button>')
                                    .appendTo($span)
                                    .click(function (e) {
                                        e.preventDefault();
                                        abrirPreMovimentoItemLoteValidade(data.record);
                                    });
                            }
                            return $span;
                        }
                    },
                    PreMovimentoId: {
                        type: 'hidden',
                        defaultValue: function (data) {
                            return $('#id').val();
                        },
                    },

                    ProdutoId: {
                        title: app.localize('Produto'),
                        width: '25%',
                        display: function (data) {
                            if (data.record.produto) {
                                return data.record.produto;
                            }
                        }
                    },
                    //NumeroSerie: {
                    //    title: app.localize('NumeroSerie'),
                    //    width: '10%',
                    //    display: function (data) {
                    //        if (data.record.numeroSerie) {
                    //            return data.record.numeroSerie;
                    //        }
                    //    }
                    //},
                    LoteValidades: {
                        title: app.localize('Lote Validades'),
                        width: '10%',
                        display: function (data) {
                            if (data.record.loteValidades && data.record.loteValidades.length != 0) {
                                let result = `<div>`;
                                _.forEach(data.record.loteValidades, (item) => {
                                    var resultItem = `<div class="row">`;
                                    resultItem += `<span class="col-md-4"> Lt: ${item.lote} </span>`;
                                    if (item.validade) {
                                        resultItem += `<span class="col-md-4"> V: ${moment(item.validade).format("DD/MM/YYYY")} </span>`;
                                    }
                                    if (item.quantidade) {
                                        resultItem += `<span class="col-md-4"> Qt: ${posicionarDireita(numeral(item.quantidade).format("0,0.00"))}</span>`;
                                    }
                                    result += `${resultItem} </div>`;
                                });
                                return `${result} </div> `;
                            }
                        }
                    },

                    Unidade: {
                        title: app.localize('Unidade'),
                        width: '10%',
                        display: function (data) {
                            if (data.record.unidade) {
                                return data.record.unidade;
                            }
                        }
                    },

                    quantidade: {
                        title: app.localize('Quantidade'),
                        width: '10%',
                        display: function (data) {
                            if (data.record.quantidade) {
                                return posicionarDireita(numeral(data.record.quantidade).format("0,0.00"));
                            }
                        }
                    },

                    CustoUnitario: {
                        title: app.localize('CustoUnitario'),
                        width: '10%',
                        display: function (data) {
                            if (data.record.custoUnitario) {
                                return posicionarDireita(numeral(data.record.custoUnitario).format("$0,0.00000"));
                            }
                        }
                    },

                    CustoTotal: {
                        title: app.localize('CustoTotal'),
                        width: '10%',
                        display: function (data) {
                            if (data.record.custoTotal) {
                                return posicionarDireita(numeral(data.record.custoTotal).format("$0,0.00000"));
                            }
                        }
                    },

                    ValorIPI: {
                        title: app.localize('ValorIPI'),
                        width: '10%',
                        display: function (data) {
                            if (data.record.valorIPI) {
                                return posicionarDireita(numeral(data.record.valorIPI).format("$0,0.00000"));
                            }
                        }
                    },

                    ValorICMS: {
                        title: app.localize('ValorICMS'),
                        width: '10%',
                        display: function (data) {
                            if (data.record.valorICMS) {
                                return posicionarDireita(numeral(data.record.valorICMS).format("$0,0.00000"));
                            }
                        }
                    },

                    
                }
            });

        _$valesTable.jtable
            ({
                title: app.localize('Vales'),
                paging: true,
                sorting: true,
                edit: false,
                create: false,
                multiSorting: true,
                actions:
                {
                    listAction:
                    {
                        method: _movimentoService.listarVales
                    },
                },
                fields:
                {
                    id: {
                        key: true,
                        list: false
                    },
                    Empresa: {
                        title: app.localize('Empresa'),
                        width: '15%',
                        display: function (data) {
                            return data.record.empresa;
                        }
                    },

                    Fornecedor: {
                        title: app.localize('Fornecedor'),
                        width: '15%',
                        display: function (data) {
                            return data.record.fornecedor;
                        }
                    },
                    Emissao: {
                        title: app.localize('Emissao'),
                        width: '15%',
                        display: function (data) {
                            return moment(data.record.emissao).format('L');
                        }
                    },

                    Documento: {
                        title: app.localize('Documento'),
                        width: '10%',
                        display: function (data) {
                            return data.record.documento;
                        }
                    },

                    Valor: {
                        title: app.localize('Valor'),
                        width: '10%',
                        display: function (data) {
                            return posicionarDireita(numeral(data.record.valorDocumento).format("$0,0.00000"));
                        }
                    }

                }
            });

        _$notaTable.jtable
            ({
                title: app.localize('Vales'),
                paging: true,
                sorting: true,
                edit: false,
                create: false,
                multiSorting: true,
                actions:
                {
                    listAction:
                    {
                        method: _movimentoService.listarNota
                    },
                },
                fields:
                {
                    id: {
                        key: true,
                        list: false
                    },
                    Empresa: {
                        title: app.localize('Empresa'),
                        width: '15%',
                        display: function (data) {
                            return data.record.empresa;
                        }
                    },

                    Fornecedor: {
                        title: app.localize('Fornecedor'),
                        width: '15%',
                        display: function (data) {
                            return data.record.fornecedor;
                        }
                    },
                    Emissao: {
                        title: app.localize('Emissao'),
                        width: '15%',
                        display: function (data) {
                            return moment(data.record.emissao).format('L');
                        }
                    },

                    Documento: {
                        title: app.localize('Documento'),
                        width: '10%',
                        display: function (data) {
                            return data.record.documento;
                        }
                    },

                    Valor: {
                        title: app.localize('Valor'),
                        width: '10%',
                        display: function (data) {

                            return posicionarDireita(numeral(data.record.valorDocumento).format("$0,0.00000"));
                        }
                    }

                }
            });



        _$itensConsignadosTable.jtable
            ({
                title: app.localize('Item'),
                paging: true,
                sorting: true,
                edit: false,
                create: false,
                multiSorting: true,
                actions:
                {
                    listAction:
                    {
                        method: _movimentoService.listarItensConsignados
                    },
                },
                fields:
                {
                    id: {
                        key: true,
                        list: false
                    },

                    PreMovimentoId: {
                        type: 'hidden',
                        defaultValue: function (data) {
                            return $('#id').val();
                        },
                    },

                    ProdutoId: {
                        title: app.localize('Produto'),
                        width: '30%',
                        display: function (data) {
                            if (data.record.produto) {
                                return data.record.produto;
                            }
                        }
                    },
                    NumeroSerie: {
                        title: app.localize('NumeroSerie'),
                        width: '10%',
                        display: function (data) {
                            if (data.record.numeroSerie) {
                                return data.record.numeroSerie;
                            }
                        }
                    },

                    quantidade: {
                        title: app.localize('Quantidade'),
                        width: '10%',
                        display: function (data) {
                            if (data.record.quantidade) {
                                return posicionarDireita(numeral(data.record.quantidade).format("0,0.00"));
                            }
                        }
                    },

                    CustoUnitario: {
                        title: app.localize('CustoUnitario'),
                        width: '10%',
                        display: function (data) {
                            if (data.record.custoUnitario) {
                                return posicionarDireita(numeral(data.record.custoUnitario).format("$0,0.00000"));
                            }
                        }
                    },

                    CustoTotal: {
                        title: app.localize('CustoTotal'),
                        width: '10%',
                        display: function (data) {
                            if (data.record.custoTotal) {
                                return posicionarDireita(numeral(data.record.custoTotal).format("$0,0.00000"));
                            }
                        }
                    },

                    Unidade: {
                        title: app.localize('Unidade'),
                        width: '20%',
                        display: function (data) {
                            if (data.record.unidade) {
                                return data.record.unidade;
                            }
                        }
                    },
                }
            });



        function getValesTable(reload) {

            if (reload) {
                _$valesTable.jtable('reload');
            } else {

                _$valesTable.jtable('load', { filtro: $('#id').val() });
            }
        }

        function getNotaTable(reload) {

            if (reload) {
                _$notaTable.jtable('reload');
            } else {

                _$notaTable.jtable('load', { filtro: $('#id').val() });
            }
        }

        function getItensConsignados(reload) {

            if (reload) {
                _$itensConsignadosTable.jtable('reload');
            } else {

                _$itensConsignadosTable.jtable('load', { filtro: $('#id').val() });
            }
        }



        function deletePreMovimentoItem(preMovimentoItem) {

            abp.message.confirm(
                app.localize('DeleteWarning', preMovimentoItem.produto.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _estoquePreMovimentoItemService.excluir(preMovimentoItem.id)
                            .done(function () {
                                getEstoquePreMovimentoItemTable(true);
                                abp.notify.success(app.localize('SuccessfullyDeleted'));
                            });
                    }
                }
            );
        }


        function getEstoquePreMovimentoItemTable(reload) {

            if (reload) {
                _$EstoquePreMovimentoItemTable.jtable('reload');
            } else {
                _$EstoquePreMovimentoItemTable.jtable('load', { filtro: $('#id').val(), entradaConfirmada: $('#entradaConfirmadaId').val() });
            }
        }


        function abrirPreMovimentoItemLoteValidade(preMovimentoItem) {
            _createOrEditLoteValidadeProdutoModal.open({ preMovimentoItemId: preMovimentoItem.id, produtoId: preMovimentoItem.produtoId });

        }


        getEstoquePreMovimentoItemTable();
        getValesTable();
        getNotaTable();
        getItensConsignados();

        $('#AplicacaoDiretaId').on('click', function (e) {


            var checkbox = e.target;
            if (checkbox.checked) {
                $('#divPaciente').show();
            }
            else {
                $('#divPaciente').hide();
            }

        });



        var _imprimirEntrada = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/RelatorioEntrada'

        });


        $('#btnImprimir').on('click', function (e) {
            _imprimirEntrada.open({ preMovimentoId: $('#id').val() });
        });




        selectSW('.selectForncedor', "/api/services/app/fornecedor/ListarDropdownSisFornecedor");
        selectSW('.selectCFOP', "/api/services/app/cfop/ListarDropdown");
        selectSW('.selectTipoFrete', "/api/services/app/tipoFrete/ListarDropdown");
        selectSW('.selectEstoque', "/api/services/app/estoque/ResultDropdownList");
        selectSW('.selectEmpresa', "/api/services/app/empresa/ListarDropdownPorUsuario");
        selectSW('.selectCentroCusto', "/api/services/app/centrocusto/ListarDropdown");
        selectSW('.selectOrdemCompra', "/api/services/app/ordemcompra/ListarDropdown");
        selectSW('.selectPaciente', "/api/services/app/paciente/ListarDropdown");
        selectSW('.selectTipoMovimento', "/api/services/app/tipomovimento/ListarDropdownEntrada");

        $('#EstTipoMovimentoId').change(function (e) {
            showTipoMovimento(e.target.value, ($('#id') != '0' || $('#id') != ''));
        });

        function showTipoMovimento(id, isEdit) {
            $('.tipoFrete').show();
            $('.valorFrete').show();
            $('.fornecedor').show();
            $('.ordemCompra').show();
            $('.serie').show();
            $('.CFOP').show();
            $('.numeroNotaFiscal').show();
            $('.buscarNota').show();
            $('#financeiroAba').show();
            $('.dataEmissao').show();
            $('.valorICMS').show();
            $('.valorIPI').show();
            $('.totalProduto').show();
            $('.valorDesconto').show();
            $('.valorAcrescimo').show();
            $('.totalDocumento').show();
            $('.contabiliza').show();
            $('.aplicacaoDireta').show();
            $('.transportadora').show();
            $('.paciente').show();
            $(".calculoImpostos").show();
            document.querySelector('.tipoMovimento').className = "col-sm-2 tipoMovimento";
            document.querySelector('.estoque').className = "col-sm-3 estoque";
            document.querySelector('.documento').className = "col-sm-2 documento";
            document.querySelector('.dataEntrada').className = "col-sm-3 dataEntrada";
            document.querySelector('.centroCusto').className = "col-sm-3 centroCusto";
            //document.querySelector('#viewClear').className = "clearfix";
            document.querySelector('#FornecedorId').required = true;

            if (id != 0 && id != null && id != '') {
                var _estoquePreMovimentoService = abp.services.app.estoquePreMovimento;
                var t = _estoquePreMovimentoService.obterTipoMovimentoEntrada(id)
                    .done(function (data) {
                        let srcSearch = 'Inventário';
                        localStorage['inventario'] = data.id;
                        if (!data.isOrdemCompra) {
                            $('.ordemCompra').hide();
                        }
                        if (!data.isPessoa) {
                            $('.fornecedor').hide();
                        }
                        if (!data.isOrdemCompraObrigatoria) {
                            //todo
                        }
                        if (!data.isFiscal) {
                            $('.serie').hide();
                            $('.dataEmissao').hide();
                            $('.CFOP').hide();
                            $('.numeroNotaFiscal').hide();
                            $('.buscarNota').hide();
                            $('.valorICMS').hide();
                            $('.valorIPI').hide();
                            $('.totalProduto').hide();
                            $('.valorDesconto').hide();
                            $('.valorAcrescimo').hide();
                            $('.totalDocumento').hide();

                            $(".calculoImpostos").hide();
                        }
                        if (!data.isFrete) {
                            $('.tipoFrete').hide();
                            $('.valorFrete').hide();
                            $('.contabiliza').hide();
                            $('.aplicacaoDireta').hide();
                            $('.transportadora').hide();
                        }
                        if (!data.isFinanceiro) {
                            $('#financeiroAba').hide();
                            $('.paciente').hide();
                        }
                        if (data.descricao.toUpperCase().indexOf(srcSearch.toUpperCase()) > -1) {
                            document.querySelector('.tipoMovimento').className = "col-sm-4 tipoMovimento";
                            document.querySelector('.estoque').className = "col-sm-4 estoque";
                            document.querySelector('.documento').className = "col-sm-4 documento";
                            document.querySelector('.dataEntrada').className = "col-sm-4 dataEntrada";
                            document.querySelector('.centroCusto').className = "col-sm-4 centroCusto";
                            document.querySelector('#FornecedorId').required = false;
                            document.querySelector('#FornecedorId').parentElement.className = "form-group";
                            document.querySelector('#viewClear').className = "";

                            document.querySelector('li > a[href="#tabProduto"]').parentElement.className = "active";
                            document.querySelector('#tabProduto').className = "tab-pane in active";
                            document.querySelector('#financeiroAba').className = "";
                            document.querySelector('#tabFinanceiro').className = "tab-pane";

                            if (isEdit) {
                                let _$preMovimentoForm = $('form[name=preMovimentoInformationsForm');

                                var allRows = _$lancamentosTable.find('.jtable-data-row');
                                $.each(allRows, function () {
                                    var id = $(this).attr('data-record-key');
                                    _$lancamentosTable.jtable('deleteRecord', { key: id, clientOnly: true });
                                });

                                _$preMovimentoForm.find('#acrescimoDecrescimo')[0].value = "0,00";
                                _$preMovimentoForm.find('#ValorAcrescimo')[0].value = "0,00";
                                _$preMovimentoForm.find('#ano')[0].value = "";
                                _$preMovimentoForm.find('#anoCompetenciaParcelas')[0].value = "";
                                _$preMovimentoForm.find('#CFOPId')[0].value = "";
                                _$preMovimentoForm.find('#CFOPId')[0].textContent = '';
                                //_$preMovimentoForm.find('#CentroCustoId')[0].value = "0";
                                //_$preMovimentoForm.find('#CentroCustoId')[0].textContent = "";
                                _$preMovimentoForm.find('#codigoBarras')[0].value = "";
                                _$preMovimentoForm.find('#corLancamentoFundo')[0].value = "";
                                _$preMovimentoForm.find('#corLancamentoLetra')[0].value = "";
                                _$preMovimentoForm.find('#dataCadastro')[0].value = "";
                                _$preMovimentoForm.find('#dataPrimeiraParcela')[0].value = "";
                                _$preMovimentoForm.find('#dataVencimento')[0].value = "";
                                _$preMovimentoForm.find('#descontoPer')[0].value = "";
                                //_$preMovimentoForm.find('#DocumentoId')[0].value = "";
                                _$preMovimentoForm.find('#Emissao')[0].value = moment(new Date()).format('L');
                                //_$preMovimentoForm.find('#EmpresaId')[0].value = "0";
                                //_$preMovimentoForm.find('#EmpresaId')[0].textContent = '';
                                //_$preMovimentoForm.find('#EstoqueId')[0].value = "";
                                //_$preMovimentoForm.find('#EstoqueId')[0].textContent = '';
                                _$preMovimentoForm.find('#FornecedorId')[0].value = "";
                                _$preMovimentoForm.find('#FornecedorId')[0].textContent = '';
                                _$preMovimentoForm.find('#Frete_FornecedorId')[0].value = "";
                                _$preMovimentoForm.find('#Frete_FornecedorId')[0].textContent = '';
                                //_$preMovimentoForm.find('#idGridLancamento')[0].value = "";
                                _$preMovimentoForm.find('#pacienteId')[0].value = "";
                                _$preMovimentoForm.find('#pacienteId')[0].textContent = '';
                                _$preMovimentoForm.find('#OrdemId')[0].value = "";
                                _$preMovimentoForm.find('#OrdemId')[0].textContent = '';
                                _$preMovimentoForm.find('#juros')[0].value = "";
                                _$preMovimentoForm.find('#lancamentoId')[0].value = "";
                                _$preMovimentoForm.find('#lancamentosJson')[0].value = "[]";
                                _$preMovimentoForm.find('#linhaDigitavel')[0].value = "";
                                _$preMovimentoForm.find('#mes')[0].value = "";
                                _$preMovimentoForm.find('#mesCompetenciaParcelas')[0].value = "";
                                //_$preMovimentoForm.find('#movimento')[0].value = "";
                                _$preMovimentoForm.find('#multa')[0].value = "";
                                _$preMovimentoForm.find('#NFeItens')[0].value = "";
                                _$preMovimentoForm.find('#nossoNumero')[0].value = "";
                                _$preMovimentoForm.find('#NumeroNotaFiscal')[0].value = "";
                                _$preMovimentoForm.find('#pacienteId')[0].value = "";
                                _$preMovimentoForm.find('#parcela')[0].value = "1";
                                _$preMovimentoForm.find('#parcelaInicial')[0].value = "1";
                                _$preMovimentoForm.find('#PreMovimentoEstadoId')[0].value = "0";
                                _$preMovimentoForm.find('#quantidadeParcelas')[0].value = "1";
                                _$preMovimentoForm.find('#SerieId')[0].value = "1";
                                _$preMovimentoForm.find('#situacaoDescricao')[0].value = "01 - Aberto";
                                _$preMovimentoForm.find('#situacaoId')[0].value = "0";
                                _$preMovimentoForm.find('#TipoFreteId')[0].value = "";
                                _$preMovimentoForm.find('#TipoFreteId')[0].textContent = "";
                                _$preMovimentoForm.find('#total')[0].value = "";
                                _$preMovimentoForm.find('#totalDocumento')[0].value = "0";
                                _$preMovimentoForm.find('#totalProdutoId')[0].value = "0";
                                _$preMovimentoForm.find('#valor')[0].value = "";
                                _$preMovimentoForm.find('#ValorFrete')[0].value = "";
                                _$preMovimentoForm.find('#valorICMS')[0].value = "";
                            }
                        }

                    })
                    .always(function () {
                        //  _modalManager.setBusy(false);
                    });
            }
        }

        function getRegistros() {

            var allRows = _$lancamentosTable.find('.jtable-data-row')

            $.each(allRows, function () {
                var id = $(this).attr('data-record-key');
                _$lancamentosTable.jtable('deleteRecord', { key: id, clientOnly: true });
            });

            if ($('#lancamentosJson').val() != '') {
                lista = JSON.parse($('#lancamentosJson').val());
                for (var i = 0; i < lista.length; i++) {
                    var item = lista[i];

                    item.DataVencimento = moment(item.DataVencimento).format('L');
                    item.DataLancamento = moment(item.DataLancamento).format('L');
                    _$lancamentosTable.jtable('addRecord', {
                        record: item
                        , clientOnly: true
                    });
                }
            }

        }


        var _$lancamentosTable = $('#lancamentosTable');

        _$lancamentosTable.jtable({

            title: app.localize('Parcelas'),
            sorting: true,
            edit: false,
            create: false,
            multiSorting: true,
            selecting: true,
            selectingCheckboxes: true,

            rowUpdated: function (event, data) {
                if (data) {

                    if (data.record.CorLancamentoFundo) {

                        data.row[0].cells[2].setAttribute('bgcolor', data.record.CorLancamentoFundo);
                        data.row[0].cells[2].setAttribute('color', data.record.CorLancamentoLetra);

                        // data.row.css("background", data.record.CorLancamentoFundo);
                        // data.row.css("color", data.record.CorLancamentoLetra);
                    }

                }
            },

            rowInserted: function (event, data) {
                if (data) {

                    if (data.record.CorLancamentoFundo) {

                        data.row[0].cells[2].setAttribute('bgcolor', data.record.CorLancamentoFundo);
                        data.row[0].cells[2].setAttribute('color', data.record.CorLancamentoLetra);
                    }
                }


                if (data.record.IsSelecionado) {
                    editRegistro(data.record);
                    //  data.row.addClass('jtable-row-selected');

                    data.row.click();

                }

            },

            fields: {
                IdGrid: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '8%',
                    sorting: false,
                    display: function (data) {
                        var $span = $('<span></span>');
                        $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                            .appendTo($span)
                            .click(function (e) {
                                e.preventDefault();
                                editRegistro(data.record);
                                // getQuitacoes();
                            });

                        $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                            .appendTo($span)
                            .click(function (e) {
                                e.preventDefault();
                                deleteRegistro(data.record);
                            });

                        return $span;
                    }
                },

                Situacao: {
                    title: app.localize('Situacao'),
                    width: '15%',
                    display: function (data) {

                        return data.record.SituacaoDescricao;
                    }
                },

                Parcela: {
                    title: app.localize('Parcela'),
                    width: '10%',
                    display: function (data) {

                        return data.record.Parcela;
                    }
                },

                Vencimento: {
                    title: app.localize('Vencimento'),
                    width: '15%',
                    display: function (data) {

                        return data.record.DataVencimento;
                    }
                },

                Valor: {
                    title: app.localize('Valor'),
                    width: '10%',
                    display: function (data) {

                        if (data.record.ValorLancamento) {

                            return posicionarDireita(numeral(data.record.ValorLancamento).format("$0,0.00"));
                        }
                    }
                },

                NossoNumero: {
                    title: app.localize('NossoNumero'),
                    width: '30%',
                    display: function (data) {

                        return data.record.NossoNumero;
                    }
                },

                Competencia: {
                    title: app.localize('Competencia'),
                    width: '10%',
                    display: function (data) {
                        if (data.record.MesCompetencia == '' || !data.record.MesCompetencia) {
                            return;
                        }

                        return (data.record.MesCompetencia.length == 2 ? data.record.MesCompetencia : ('0' + data.record.MesCompetencia)) + "/" + data.record.AnoCompetencia;
                    }
                },

            }
        });

        var lista = [];


        getRegistros();

        $('#inserir').click(function (e) {
            e.preventDefault();


            var _$lancamentoInformationsForm = $('form[name=LancamentoInformationsForm]');

            //AltararValidacaoParcela(true);

            //if (!ValidarParcela()) {
            //    AltararValidacaoParcela(false);
            //    return;
            //}

            //AltararValidacaoParcela(false);



            var lancamento = _$lancamentoInformationsForm.serializeFormToObject();


            if ($('#lancamentosJson').val() != '') {
                lista = JSON.parse($('#lancamentosJson').val());
            }
            debugger;
            if ($('#idGridLancamento').val() != '') {

                for (var i = 0; i < lista.length; i++) {
                    if (lista[i].IdGrid == $('#idGridLancamento').val()) {

                        //var situacao = $('#situacaoId').select2('data');
                        //if (situacao && situacao.length > 0) {

                        //    lista[i].SituacaoDescricao = situacao[0].text;
                        //}


                        lista[i].SituacaoDescricao = $('#situacaoDescricao').val();

                        lista[i].SituacaoLancamentoId = $('#situacaoId').val();
                        lista[i].ValorLancamento = numeral(parseFloat(trataMask(maskFields, 'valor'),2)).value();
                        lista[i].ValorAcrescimoDecrescimo = numeral(parseFloat(trataMask(maskFields, 'acrescimoDecrescimo'),2)).value();
                        lista[i].Juros = numeral(parseFloat(trataMask(maskFields, 'juros'),2)).value();
                        lista[i].Multa = numeral(parseFloat(trataMask(maskFields, 'multa'),2)).value();
                        lista[i].Total = numeral(parseFloat(trataMask(maskFields, 'total'),2)).value();
                        lista[i].MesCompetencia = $('#mes').val();
                        lista[i].AnoCompetencia = $('#ano').val();
                        lista[i].DataVencimento = $('#dataVencimento').val();
                        lista[i].DataLancamento = $('#dataCadastro').val();
                        lista[i].CorLancamentoFundo = $('#corLancamentoFundo').val();
                        lista[i].CorLancamentoLetra = $('#corLancamentoLetra').val();
                        lista[i].CodigoBarras = $('#codigoBarras').val();
                        lista[i].NossoNumero = $('#nossoNumero').val();
                        lista[i].LinhaDigitavel = $('#linhaDigitavel').val();
                        lista[i].Parcela = $('#parcela').val();

                        _$lancamentosTable.jtable('updateRecord', {
                            record: lista[i]
                            , clientOnly: true
                        });

                    }
                }
            }
            else {
                lancamento.IdGrid = lista.length == 0 ? 1 : lista[lista.length - 1].IdGrid + 1;

                //lancamento.SituacaoLancamentoId = $('#situacaoId').val();

                //var situacao = $('#situacaoId').select2('data');
                //if (situacao && situacao.length > 0) {

                //    lancamento.SituacaoDescricao = situacao[0].text;
                //}

                lancamento.SituacaoDescricao = $('#situacaoDescricao').val();
                lancamento.SituacaoLancamentoId = $('#situacaoId').val();
                lancamento.ValorLancamento = numeral(parseFloat(trataMask(maskFields, 'valor'),2)).value();
                lancamento.ValorAcrescimoDecrescimo = numeral(parseFloat(trataMask(maskFields, 'acrescimoDecrescimo'),2)).value();
                lancamento.Juros = numeral(parseFloat(trataMask(maskFields, 'juros'),2)).value();
                lancamento.Multa = numeral(parseFloat(trataMask(maskFields, 'multa'),2)).value();
                lancamento.Total = numeral(parseFloat(trataMask(maskFields, 'total'),2)).value();
                lancamento.MesCompetencia = $('#mes').val();
                lancamento.AnoCompetencia = $('#ano').val();
                lancamento.DataVencimento = $('#dataVencimento').val();
                lancamento.DataLancamento = $('#dataCadastro').val();
                lancamento.CorLancamentoFundo = $('#corLancamentoFundo').val();
                lancamento.CorLancamentoLetra = $('#corLancamentoLetra').val();
                lancamento.CodigoBarras = $('#codigoBarras').val();
                lancamento.NossoNumero = $('#nossoNumero').val();
                lancamento.LinhaDigitavel = $('#linhaDigitavel').val();
                lancamento.Parcela = $('#parcela').val();

                lista.push(lancamento);

                _$lancamentosTable.jtable('addRecord', {
                    record: lancamento
                    , clientOnly: true
                });

            }
            $('#lancamentosJson').val(JSON.stringify(lista));
            limpar();
            //$('#lancamentosJson').val(JSON.stringify(lista));
            //$('#idGridLancamento').val('');
            //$('#situacaoDescricao').val('01 - Aberto');
            //$('#situacaoId').val(0);
            //$('#valor').val('');
            //$('#acrescimoDecrescimo').val('');
            //$('#juros').val('');
            //$('#multa').val('');
            //$('#total').val('');
            //$('#mes').val('');
            //$('#ano').val('');
            //$('#dataVencimento').val('');
            //$('#dataCadastro').val(moment(new Date()).format('L'));
            //$('#corLancamentoFundo').val('');
            //$('#corLancamentoLetra').val('');
            //$('#codigoBarras').val('');
            //$('#nossoNumero').val('');
            //$('#linhaDigitavel').val('');

            CalculaValorLancamento();

            $('#valor').focus();

        });

        function CalculaValorLancamento() {
            var totalLancamento = 0;

            for (var i = 0; i < lista.length; i++) {
                totalLancamento += parseFloat(lista[i].ValorLancamento);
            }

            $('#valorTotalParcelas').val(formatarValor(totalLancamento));
        }


        function calcularTotal() {
            var total = 0;

            var valor = $('#valor').val() != '' ? parseFloat(trataMask(maskFields, 'valor')) : 0;
            var juros = $('#juros').val() != '' ? parseFloat(trataMask(maskFields, 'juros')) : 0;
            var multa = $('#multa').val() != '' ? parseFloat(trataMask(maskFields, 'multa')) : 0;
            var acrescimoDecrescimo = $('#acrescimoDecrescimo').val() != '' ? parseFloat(trataMask(maskFields, 'acrescimoDecrescimo')) : 0;

            total = valor + juros + multa + acrescimoDecrescimo;

            maskFields['total'].unmaskedValue = total.toString();
        }

        $('.calcularTotal').on('blur', function (e) {
            e.preventDefault();

            calcularTotal();



        });

        function limpar() {
            $('#idGridLancamento').val('');
            $('#situacaoDescricao').val('01 - Aberto');
            $('#situacaoId').val(0);
            $('#valor').val('');
            $('#acrescimoDecrescimo').val('');
            $('#juros').val('');
            $('#multa').val('');
            $('#total').val('');
            $('#mes').val('');
            $('#ano').val('');
            $('#dataVencimento').val('');
            $('#dataCadastro').val(moment(new Date()).format('L'));
            $('#corLancamentoFundo').val('');
            $('#corLancamentoLetra').val('');
            $('#codigoBarras').val('');
            $('#nossoNumero').val('');
            $('#linhaDigitavel').val('');

            $('#parcela').val(lista.length + 1);

            // CalculaValorLancamento();



            $('#inserir > i').removeClass('fa-check');
            $('#inserir > i').addClass('fa-plus');
        }


        $('input[name="DataVencimento"]').daterangepicker({
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
                $('input[name="DataVencimento"]').val(selDate.format('L')).addClass('form-control edited');
            });


        $('input[name="DataPrimeiraParcela"]').daterangepicker({
            "singleDatePicker": true,
            "showDropdowns": true,
            autoUpdateInput: true,
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
                $('input[name="DataPrimeiraParcela"]').val(selDate.format('L')).addClass('form-control edited');
            });


        $('#gerarParcelas').on('click', function (e) {
            e.preventDefault();
            if ($('#totalDocumento').val() != '' && $('#quantidadeParcelas').val() != '') {
                var valorDocumento = numeral(parseFloat(trataMask(maskFields, 'totalDocumento'), 2)).value();

                var parcelaInicial = numeral(parseFloat(trataMask(maskFields, 'parcelaInicial'), 2)).value();
                console.log("parcelaInicial:", parcelaInicial);
                var quantidadeTotalParcelas = numeral($('#quantidadeParcelas').val()).value();
                var quantidadeParcelas = quantidadeTotalParcelas;
                var valorParcela = numeral(valorDocumento).divide(quantidadeParcelas).value();
                var data = $('#dataPrimeiraParcela').val().split('/');

                var dataCorrente = new Date(data[2], data[1] - 1, data[0]);

                for (var i = 0; i < quantidadeParcelas; i++) {
                    var lancamento = $('form[name=LancamentoInformationsForm]').serializeFormToObject();

                    lancamento.IdGrid = lista.length == 0 ? 1 : lista[lista.length - 1].IdGrid + 1;

                    //Situação: Aberto
                    lancamento.SituacaoLancamentoId = 1;
                    lancamento.SituacaoDescricao = '01 - Aberto';
                    lancamento.ValorLancamento = valorParcela;
                    lancamento.Total = valorParcela;
                    lancamento.MesCompetencia = $('#mesCompetenciaParcelas').val();
                    lancamento.AnoCompetencia = $('#anoCompetenciaParcelas').val();
                    lancamento.DataVencimento = moment(dataCorrente).format('L');
                    lancamento.DataLancamento = moment(new Date()).format('L');
                    lancamento.Parcela = i + parcelaInicial;
                    lancamento.NossoNumero = $('#DocumentoId').val() + ' - ' + lancamento.Parcela + '/' + quantidadeTotalParcelas;

                    if (i == quantidadeParcelas - 1) {

                        var somaTodasParcelasLancadas = 0;

                        for (var j = 0; j < lista.length; j++) {
                            somaTodasParcelasLancadas = numeral(lista[j].ValorLancamento).add(somaTodasParcelasLancadas).value();
                        }

                        lancamento.ValorLancamento = numeral(valorDocumento).subtract(somaTodasParcelasLancadas).value();
                    } else {
                        lancamento.ValorLancamento = numeral(valorParcela).value();
                    }

                    lista.push(lancamento);

                    _$lancamentosTable.jtable('addRecord', {
                        record: lancamento
                        , clientOnly: true
                    });

                    dataCorrente.setMonth(dataCorrente.getMonth() + 1);
                }

                $('#lancamentosJson').val(JSON.stringify(lista));

                CalculaValorLancamento();
            }
        });

        function editRegistro(lancamento) {
            $('#lancamentoId').val(lancamento.Id);
            maskFields['valor'].unmaskedValue = lancamento.ValorLancamento != null || lancamento.ValorLancamento != undefined ? lancamento.ValorLancamento.toString() : "0";
            $('#dataVencimento').val(lancamento.DataVencimento);
            $('#idGridLancamento').val(lancamento.IdGrid);
            $('#corFundoLancamento').val(lancamento.CorFundoLancamento);
            $('#corLentraLancamento').val(lancamento.CorLentraLancamento);
            maskFields['juros'].unmaskedValue = lancamento.Juros != null || lancamento.Juros != undefined ? lancamento.Juros.toString() : "0";
            maskFields['multa'].unmaskedValue = lancamento.Multa != null || lancamento.Multa != undefined ? lancamento.Multa.toString() : "0";
            maskFields['acrescimoDecrescimo'].unmaskedValue = lancamento.ValorAcrescimoDecrescimo != null || lancamento.ValorAcrescimoDecrescimo != undefined ? lancamento.ValorAcrescimoDecrescimo.toString() : "0";
            $('#total').val(lancamento.Total);
            $('#dataCadastro').val(lancamento.DataLancamento);
            $('#mes').val(lancamento.MesCompetencia);
            $('#ano').val(lancamento.AnoCompetencia);
            $('#situacaoId').val(lancamento.SituacaoLancamentoId);
            $('#situacaoDescricao').val(lancamento.SituacaoDescricao);
            $('#codigoBarras').val(lancamento.CodigoBarras);
            $('#nossoNumero').val(lancamento.NossoNumero);
            $('#linhaDigitavel').val(lancamento.LinhaDigitavel);
            $('#parcela').val(lancamento.Parcela);
            $('#idGridResultadoExame').val(lancamento.IdGridResultadoExame);





            // $('#inserir > i').removeClass('fa');
            $('#inserir > i').removeClass('fa-plus');
            // $('#inserir > i').addClass('glyphicon');
            $('#inserir > i').addClass('fa-check');


            calcularTotal();
        }

        function deleteRegistro(lancamento) {
            abp.message.confirm(
                app.localize('DeleteWarning', lancamento.NossoNumero),
                function (isConfirmed) {
                    if (isConfirmed) {



                        lista = JSON.parse($('#lancamentosJson').val());

                        for (var i = 0; i < lista.length; i++) {
                            if (lista[i].IdGrid == lancamento.IdGrid) {
                                lista.splice(i, 1);
                                $('#lancamentosJson').val(JSON.stringify(lista));

                                _$lancamentosTable.jtable('deleteRecord', {
                                    key: lancamento.IdGrid
                                    , clientOnly: true
                                });

                                break;
                            }
                        }

                        CalculaValorLancamento();
                        limpar();
                    }
                }
            );
        }

        function trataMask(maskFields, input) {
            const el = maskFields[input].unmaskedValue;
            if (el === "" || el === null || el === undefined) {
                return 0;
            }
            return el;
        }


        function onSelecionaNotaPendente(chaveNFE) {
            if (chaveNFE) {
                $('#NumeroNotaFiscal').val(chaveNFE);
                sincronizarNotasSefaz();

            }
        }
    });

})(jQuery);