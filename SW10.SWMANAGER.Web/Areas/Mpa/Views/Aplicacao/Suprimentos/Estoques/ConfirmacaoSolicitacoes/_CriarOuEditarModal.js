
(function ($) {
    $(function () {
        var summerNoteOptions = {
            toolbar: [
                ['printSize', ['printSize']],
                ['style', ['bold', 'italic', 'underline']],
                ['fontsize', ['fontsize']],
                ['fontname', ['fontname']],
                ['font', ['font', 'strikethrough', 'superscript', 'subscript']],
                ['color', ['color']],
                ['para', ['ul', 'ol', 'paragraph']],
                ['height', ['height']],
                ['misc', ['codeview', 'fullscreen']],
                ['table', ['table']]
            ],
            width: '100%',
            height: 150,
            padding: 15,
            disableResizeEditor: true
        };

        $(document).ready(function () {
            $.summernote.options.lineHeights = ["0", "0.2", "0.4", "0.6", "0.8", "1.0"];
            $('.text-editor').summernote(summerNoteOptions).summernote('disable');
            CamposRequeridos();
            configurarCampos();
        });

        this.init = function (modalManager) {
            $('.modal-dialog').css({ 'width': '90%', 'max-width': '1100px' });
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };

            atendimentoChange();
            configurarCampos();
        };

        $('.modal-dialog').css('width', '1800px');

        $.validator.setDefaults({ ignore: ":hidden:not(select)" });

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
                    }
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
                return false;
            },
            change: function (event, ui) {
                event.preventDefault();
                if (ui.item == null) {
                    $('#empresa-Id').val(0);
                    $("#empresa-search").val('').focus();
                    abp.notify.info(app.localize("EstadoInvalido"));
                    return false;
                }
            }
        });

        $('#Movimento').on('load', function () {
            var d = new Date();
            var n = d.getDate();
            $('#movimento').val(moment().format("L LT"));
        });

        var _preMovimentoService = abp.services.app.estoquePreMovimento;
        var produtoSaldoService = abp.services.app.produtoSaldo;
        var _$EstoquePreMovimentoItemTable = $('#EstoquePreMovimentoItemTable');

        var _createOrEditPreMovimentoItemModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/ConfirmacaoSolicitacoes/CriarOuEditarPreMovimentoItemModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/ConfirmacaoSolicitacoes/_CriarOuEditarPreMovimentoItemModal.js',
            modalClass: 'CriarOuEditarPreMovimentoItemModal'
        });

        $('#btn-novo-PreMovimentoItem').click(function (e) {
            e.preventDefault()
            _createOrEditPreMovimentoItemModal.open({ id: 0 });
        });

        $('#salvar-PreMovimento').click(function (e) {
            e.preventDefault();

            var _$preMovimentoInformationsForm = $('form[name=preMovimentoInformationsForm]');

            _$preMovimentoInformationsForm.validate();

            if (!_$preMovimentoInformationsForm.valid()) {
                return;
            }

            var preMovimento = _$preMovimentoInformationsForm.serializeFormToObject();

            _preMovimentoService.atenderSolicitacao(preMovimento)
                .done(function (data) {
                    if (data.errors.length > 0) {
                        errorHandler(data.errors);
                    } else {
                        abp.notify.info(app.localize('SavedSuccessfully'));
                        $('#id').val(data.returnObject.id);
                        $('.modal-imprimir').modal('toggle');
                    }
                })
                .always(function () {
                });
        });

        $('#vincularLoteAutomatico').click(function (e) {
            e.preventDefault();

            const button = $(this);
            button.buttonBusy(true);

            var estoqueLoteValidadeAppService = abp.services.app.estoqueLoteValidade;

            if ($('#itens').val()) {

                var listaItens = JSON.parse($('#itens').val());

                for (let lista of listaItens) {
                    // Verifica se o item contêm lote validade e não realiza o vinculo automático do lote para itens que estão "Totalmemte Atendidos" ou "Suspensos"
                    if (lista.IsLote === true && (lista.estadoSolicitacaoItemId !== 6 && lista.estadoSolicitacaoItemId !== 8)) {
                        var quantidadeResidual = numeral(lista.QuantidadeSolicitada).difference(lista.QuantidadeAtendida);
                        // Somente para itens que ainda possuem quantidade residual, ou seja, que ainda não foram totalmente atendidos
                        if (quantidadeResidual > 0) {
                            // Obtêm os Lotes Validade do item
                            estoqueLoteValidadeAppService.obterPorProdutoEstoqueLaboratorio(lista.ProdutoId, $("#EstoqueId").val(), null, { async: false }).done(function (listaLoteValidade) {
                                let lotesValidadesJson = [];

                                if (lista.LotesValidadesJson) {
                                    lotesValidadesJson = JSON.parse(lista.LotesValidadesJson);
                                }

                                for (var y = 0; y < listaLoteValidade.length; y++) {                         
                                    var quantidade = quantidadeResidual >= listaLoteValidade[y].quantidade ? listaLoteValidade[y].quantidade : quantidadeResidual;

                                    lote = {
                                        'IdGridLoteValidade': _.maxBy(lotesValidadesJson, 'IdGridLoteValidade') ? (_.maxBy(lotesValidadesJson, 'IdGridLoteValidade').IdGridLoteValidade + 1) : 1,
                                        'LoteValidadeId': listaLoteValidade[y].loteValidadeId,
                                        'Quantidade': parseFloat(quantidade) || 0
                                    }

                                    lotesValidadesJson.push(lote);

                                    quantidadeResidual = numeral(quantidadeResidual).difference(quantidade);

                                    if (quantidadeResidual == 0) {
                                        break;
                                    }
                                }

                                lista.LotesValidadesJson = JSON.stringify(lotesValidadesJson);
                                lista.QuantidadeAtendida = _.sumBy(lotesValidadesJson, "Quantidade");
                            });
                        }
                    }
                }

                $('#itens').val(JSON.stringify(listaItens));

                getEstoquePreMovimentoItemTable();

                button.buttonBusy(false);
            }
        });

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
        }, function (selDate) {
            $('input[name="Movimento"]').val(selDate.format('L')).addClass('form-control edited');
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

        $('.close-button').on('click', function () {
            location.href = '/mpa/ConfirmacaoSolicitacoes';
        });

        function retornarLista(filtro) {
            var res = undefined;
            if ($('#itens').val() != '') {

                var js = $('#itens').val();
                res = _preMovimentoService.listarItensJson({ data: js});
                return res;
            }
            else {
                res = _preMovimentoService.listarItens({ filtro: $('#id').val() });
                return res;
            }
        }

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
                        method: retornarLista
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
                        width: '5%',
                        sorting: false,
                        display: function (data) {
                            if (data.record.estadoSolicitacaoItemId !== 6 && data.record.estadoSolicitacaoItemId !== 8) {
                                var $span = $('<span style="display: flex; justify-content: center;"></span>');
                                $('<button class="btn btn-default btn-xs text-center" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                    .appendTo($span)
                                    .click(function (e) {
                                        e.preventDefault();
                                        _createOrEditPreMovimentoItemModal.open({ item: JSON.stringify(data.record) });
                                    });
                                return $span;
                            }
                        }
                    },
                    estadoSolicitacaoItemId: {
                        title: app.localize('Status'),
                        width: '10%',
                        display: function (data) {
                            switch (data.record.estadoSolicitacaoItemId) {
                                case 1: {
                                    return '<span class="label label-info">' + app.localize('Aguardando Confirmação') + '</span>';
                                }
                                case 2: {
                                    return '<span class="label label-success">' + app.localize('Confirmado') + '</span>';
                                }
                                case 3: {
                                    return '<span class="label label-warning">' + app.localize('Pendente informação') + '</span>';
                                }
                                case 4: {
                                    return '<span class="label label-warning">' + app.localize('Pendente') + '</span>';
                                }
                                case 5: {
                                    return '<span class="label label-warning">' + app.localize('Parcialmente Atendido') + '</span>';
                                }
                                case 6: {
                                    return '<span class="label label-success">' + app.localize('Totalmente Atendido') + '</span>';
                                }
                                case 7: {
                                    return '<span class="label label-danger">' + app.localize('Parcialmente Suspensa') + '</span>';
                                }
                                case 8: {
                                    return '<span class="label label-danger">' + app.localize('Suspensa') + '</span>';
                                }
                                default: {
                                    return '';
                                }
                            }
                        }
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
                    quantidadeSolicitada: {
                        title: app.localize('QuantidadeSolicitada'),
                        width: '8%',
                        display: function (data) {
                            if (data.record) {
                                return posicionarDireita(data.record.quantidadeSolicitada || 0);
                            }
                        }
                    },
                    quantidadeAtendida: {
                        title: app.localize('QuantidadeAtendida'),
                        width: '8%',
                        display: function (data) {
                            if (data.record) {
                                return posicionarDireita(data.record.quantidadeAtendida || 0);
                            }
                        }
                    },
                    quantidade: {
                        title: app.localize('QuantidadePendente'),
                        width: '8%',
                        display: function (data) {
                            if (data.record) {
                                return posicionarDireita(data.record.quantidade || 0);
                            }
                        }
                    },
                    loteSugerido: {
                        title: app.localize('Lote Sugerido'),
                        width: '17%',
                        display: function (data) {
                            return data.record.loteSugeridoName;
                        }
                    },
                    Unidade: {
                        title: app.localize('Unidade'),
                        width: '14%',
                        display: function (data) {
                            if (data.record.produtoUnidade) {
                                return data.record.produtoUnidade;
                            }
                        }
                    },
                }
            });

        function getEstoquePreMovimentoItemTable(reload) {

            if (reload) {
                _$EstoquePreMovimentoItemTable.jtable('reload');
            } else {
                _$EstoquePreMovimentoItemTable.jtable('load', { filtro: $('#id').val() });
            }
        }

        getEstoquePreMovimentoItemTable();

        $('#EstTipoMovimentoId').change(function () {
            configurarCampos();
        });

        function configurarCampos() {
            var valor = $('#EstTipoMovimentoId').val();

            if (valor == '3') {
                $('#grupoOrganizacional').hide();
                $('#grupoOrganizacional').val('');
                $('#medico').show();
                $('#atendimento').show();
            }
            else {
                $('#grupoOrganizacional').show();
                $('#paciente').hide();
                $('#medico').hide();
                $('#atendimento').hide();
                $('#paciente').val('');
                $('#medico').val('');
                $('#atendimento').val('');
            }

            if (valor == 2) {
                $("#grupoOrganizacional label").html("Setor Destino");
            }
            else {
                $("#grupoOrganizacional label").html("Unidade");
            }



            if (valor == 4) {
                $('#motivoPerdaId').show();

                $("#grupoOrganizacional").hide();
                $('.selectUnidadeOrganizacional').val('');
            }
            else {
                $("#grupoOrganizacional").show();
                $('#motivoPerdaId').hide();
                $('#motivoPerdaId').val('');
            }

        }

        $('#atendimentoId').change(function () {
            atendimentoChange();
        });

        function atendimentoChange() {

            var valor = $('#atendimentoId').val();

            if (valor == '' || valor == '0') {
                // $("#MedicoSolcitanteId").attr("disabled", false).change();
                $("#divMedico").removeClass('hidden');
                $("#medicoSolcitanteId").addClass('hidden');
                $("#pacienteInputId").addClass('hidden');
                $("#divPaciente").removeClass('hidden');

            }
            else {
                $.ajax({
                    url: "/mpa/Saidas/SelecionarAtendimento/" + valor,
                    success: function (data) {

                        $("#pacienteInputId").removeClass('hidden');
                        $("#divPaciente").addClass('hidden');
                        //$("#MedicoSolcitanteId").val(data.MedicoId).change()
                        //                   .selectpicker('refresh');

                        //  $("#MedicoSolcitanteId").attr("disabled", true).change();

                        $("#medicoSolcitanteId").removeClass('hidden');
                        $("#divMedico").addClass('hidden');

                        $("#pacienteInputId").val(data.Paciente.CodigoPaciente + ' - ' + data.Paciente.NomeCompleto);

                        $("#medicoSolcitanteId").val(' - ' + data.Medico.NomeCompleto);
                    }
                });
            }
        }

        $('#produtoId').on('select2:select', function () {
            if ($('#itens').val() != '') {
                lista = JSON.parse($('#itens').val());
            }

            for (var i = 0; i < lista.length; i++) {
                if (lista[i].ProdutoId == $('#produtoId').val()) {

                    _createOrEditPreMovimentoItemModal.open({ item: JSON.stringify(lista[i]) });
                }
            }
        });

        //selectSW('.selectForncedor', "/api/services/app/fornecedor/ListarDropdown");
        //selectSW('.selectAtendimento', "/api/services/app/Atendimento/ListarDropdown");
        //selectSW('.selectPaciente', "/api/services/app/Paciente/ListarDropdown");
        //selectSW('.selectMedico', "/api/services/app/Medico/ListarDropdown");
        //selectSW('.selectProduto', "/api/services/app/produto/ListarProdutoDropdown");

        abp.event.on('app.CriarOuEditarPreMovimentoItemModalSaved', function () {
            getEstoquePreMovimentoItemTable();
        });

        var _imprimirEntrada = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/ConfirmacaoSolicitacoes/VisualizarIndex'
        });

        $('#btnImprimir').on('click', function (e) {
            _imprimirEntrada.open({ solicitacaoId: $('#id').val() });
        });

        $('#codigoBarra').on('keypress', function (event) {
            //Tecla 13 = Enter

            if (event.which == 13) {

                event.preventDefault();
                var codigoBarraAppService = abp.services.app.codigoBarra;

                codigoBarraAppService.obterValorEtiqueta($('#codigoBarra').val()).then(res => {
                    if (res == null || res == undefined) {
                        abp.notify.error("Não foi possível achar o material correspondente a etiqueta")
                        return;
                    }
                    if ($('#itens').val() && res && res.produtoId != 0) {
                        var lista = JSON.parse($('#itens').val());
                        var listaProduto = _.findLast(lista, (item) => item.ProdutoId === res.produtoId);
                        var lote = undefined;
                        if (listaProduto) {
                            let lotesValidadesJson = null;
                            if (listaProduto.LotesValidadesJson) {
                                lotesValidadesJson = JSON.parse(listaProduto.LotesValidadesJson);
                                lote = _.findLast(lotesValidadesJson, (item) => item.LoteValidadeId === res.loteValidadeId);

                                if (lote) {
                                    lote.Quantidade = parseFloat(lote.Quantidade) + parseFloat($("#quantidade").val()) || 0;
                                }
                                else {
                                    lote = {
                                        'IdGridLoteValidade': _.maxBy(lotesValidadesJson, 'idGridLoteValidade') || 1,
                                        'LoteValidadeId': res.loteValidadeId,
                                        'Quantidade': parseFloat($("#quantidade").val()) || 0
                                    }
                                    lotesValidadesJson.push(lote);
                                }
                            }
                            else {
                                lotesValidadesJson = [];
                                lote = {
                                    "IdGridLoteValidade": 1,
                                    'LoteValidadeId': res.loteValidadeId,
                                    'Quantidade': parseFloat($("#quantidade").val()) || 0
                                };

                                lotesValidadesJson.push(lote);
                            }
                            var quantidadeAtendida = _.sumBy(lotesValidadesJson, "Quantidade");
                            console.log(quantidadeAtendida, listaProduto);

                            if (quantidadeAtendida > listaProduto.QuantidadeSolicitada) {
                                abp.notify.error("Não é possivel atender uma quantidade maior que a solicitada.");
                            } else {
                                const validarProdutoSaldo = {
                                    produtoId: res.produtoId,
                                    loteValidadeId: res.loteValidadeId,
                                    estoqueId: $("#EstoqueId").val(),
                                    isEntrada: $("#isEntrada").val(),
                                    quantidade: lote.Quantidade
                                };

                                produtoSaldoService.validaSaldoPorProdutoLoteValidadeEstoque(validarProdutoSaldo).then(data => {
                                    if (data.errors.length > 0) {
                                        errorHandler(data.errors);
                                        return;
                                    }
                                    listaProduto.LotesValidadesJson = JSON.stringify(lotesValidadesJson);

                                    listaProduto.QuantidadeAtendida = _.sumBy(lotesValidadesJson, "Quantidade");
                                    $('#itens').val(JSON.stringify(lista));

                                    getEstoquePreMovimentoItemTable();
                                });
                            }

                            $('#codigoBarra').val('');
                            $('#quantidade').val('1');
                            $('#codigoBarra').focus();

                        }
                    }
                });


                return;
                ////inserirProdutoCodigoBarra();
                //var preMovimentoItem = {};
                //preMovimentoItem.Quantidade = retirarMascara(preMovimentoItem.Quantidade);
                //if ($('#itens').val() != '') {
                //    lista = JSON.parse($('#itens').val());
                //}

                //if ($('#idGrid').val() != '') {
                //    for (var i = 0; i < lista.length; i++) {
                //        if (lista[i].IdGrid == $('#idGrid').val()) {
                //            lista[i].QuantidadeAtendida = preMovimentoItem.QuantidadeAtendida;
                //            lista[i].LotesValidadesJson = preMovimentoItem.LotesValidadesJson;
                //            lista[i].NumerosSerieJson = preMovimentoItem.NumerosSerieJson;
                //        }
                //    }
                //}
                //else {
                //    preMovimentoItem.IdGrid = lista.length == 0 ? 1 : lista[lista.length - 1].IdGrid + 1;
                //    lista.push(preMovimentoItem);
                //}
                //$('#itens').val(JSON.stringify(lista));
            }
        });



        function inserirProdutoCodigoBarra() {
            var estoquePreMovimentoItemAppService = abp.services.app.estoquePreMovimentoItem;
            estoquePreMovimentoItemAppService.criarSaidaPorCodigoBarra($('#codigoBarra').val(), $('#EstoqueId').val(), $('#id').val(), $('#quantidade').val())
                .done(function (data) {
                    if (data.errors.length > 0) {
                        errorHandler(data.errors);
                    }
                    else {
                        if (data.warnings.length > 0) {
                            errorHandler(data.warnings,"warning");
                        }

                        $('#codigoBarra').val('');
                        $('#quantidade').val('1');
                        $('#codigoBarra').focus();

                        getEstoquePreMovimentoItemTable();
                    }
                });
        }

        function fecharAction() {
            $('.modal-imprimir').modal('hide');
            // location.href = '/mpa/ConfirmacaoSolicitacoes';
        }

        $('.modal-imprimir').on('hidden.bs.modal', function () {

        });
        function naoImprimir() {
            fecharAction();
        }

        function voltarAction() {
            location.href = '/mpa/ConfirmacaoSolicitacoes';
        }

        $('.btn-voltar').on("click", voltarAction);


        $('.naoImprimir').on("click", naoImprimir);

        function imprimirAction() {
            imprimir($("#id").val());
            fecharAction();
        }
        $('.imprimir-tudo').on("click", imprimirAction);

        function imprimir(id) {
            fecharAction();
            $.removeCookie("XSRF-TOKEN");
            printJS({
                printable: '/Mpa/ConfirmacaoSolicitacoes/imprimirSolicitacaoBaixa?preMovimentoId=' + id, type: 'pdf',
                onPrintDialogClose: () => {
                    voltarAction();
                }
            });

        }
    });

})(jQuery);