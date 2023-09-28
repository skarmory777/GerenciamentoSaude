(function ($) {

    $(document).ready(function () {
        selectSW('.selecttipoDocumento', "/api/services/app/tipoDocumento/ListarDropdown");
        selectSW('.selectEmpresa', "/api/services/app/empresa/ListarDropdownPorUsuario");
        selectSW('.selectFornecedorContaPagar', "/api/services/app/sisPessoa/ListarDropdownSisIsPagar", { valor: 'false' });
        selectSW('.selectsituacaolancamento', "/api/services/app/SituacaoLancamento/ListarDropdown");
        selectSW('.selectContaAdministrativaEmpresa', "/api/services/app/ContaAdministrativa/ListarContaAdministrivaDespesaDropdown", $('#empresaRateioId'));
        selectSW('.selectCentroCusto', "/api/services/app/CentroCusto/ListarDropdownCodigoCentroCustoPorContaAdministrativa", $('#contaAdministrativaId'));

        CamposRequeridos();
        $('#valor').mask('000.000.000,00', { reverse: true });
        $('#juros').mask('000.000.000,00', { reverse: true });
        $('#multa').mask('000.000.000,00', { reverse: true });
        $('#acrescimoDecrescimo').mask('000.000.000,00', { reverse: true });
        $('#valorDesconto').mask('000.000.000,00', { reverse: true });
        $('#total').mask('000.000.000,00', { reverse: true });
        $('#valorRateio').mask('000.000.000,00', { reverse: true });
        $('#valorDocumento').mask('000.000.000,00', { reverse: true });
        $('#valorAcrescimoDecrescimo').mask('000.000.000,00', { reverse: true });
        $('#valorTotal').mask('000.000.000,00', { reverse: true });

        if ($("#EmpresaId").val() == 0) {
            $("#EmpresaId").val(null);
            $("#tipoDocumentoId").val(null);
            $("#valorDocumento").val(null);
        }

        SetCompetencia();
    });

    var _contasPagarService = abp.services.app.contasPagar;
    var _situacaoLancamentoService = abp.services.app.situacaoLancamento;
    var _quitacaoService = abp.services.app.quitacao;
    var _$contasPagarInformationsForm = null;
    var _$lancamentosTable = $('#lancamentosTable');
    var _$quitacoesTable = $('#quitacoesTable');

    var _anexosModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Mpa/Anexo/OpenModal',
        scriptUrl: abp.appPath + 'Areas/Mpa/Views/Common/Modals/Anexo/_AnexoModal.js',
        modalId: 'anexoModalId'
    });

    var _ErrorModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Mpa/Erros/ExibirErros',
    });

    var _createOrEditModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Mpa/Fornecedores/CriarOuEditarModal',
        scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Fornecedores/_CriarOuEditarModal.js',
        modalClass: 'CriarOuEditarFornecedorModal'
    });

    $('#editFornecedorButton').click(function (e) {
        e.preventDefault();
        var pessoaId = $("#pessoaId").val();
        if (pessoaId != "") {
            _contasPagarService.obterFornecedorId(pessoaId)
                .done(function (data) {
                    if (data != null) {
                        _createOrEditModal.open({ id: data });
                    }
                })
        }
    });

    var datePickerOptions = {
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
    };

    function SetCompetencia() {
        const dateNow = new Date();
        let month = dateNow.getMonth();
        let year = dateNow.getFullYear();
        $("#mesCompetenciaParcelas").val(month + 1);
        $("#anoCompetenciaParcelas").val(year);
    }

    this.init = function (modalManager) {
        _$contasPagarInformationsForm = $('form[name=contasPagarInformationsForm]');
        _$contasPagarInformationsForm.validate();
    };

    $("#EmpresaId").change(function () {
        if ($("#EmpresaId").select2('data') != null && $("#EmpresaId").select2('data').length) {
            let optionId = $("#EmpresaId").val();
            let optionText = $("#EmpresaId").select2('data')[0].text;
            if ($('#empresaRateioId').find("option[value='" + optionId + "']").length) {
                $('#empresaRateioId').val(optionId).trigger('change');
            } else {
                // Create a DOM Option and pre-select by default
                var newOption = new Option(optionText, optionId, true, true);
                // Append it to the select
                $('#empresaRateioId').append(newOption).trigger('change');
            }
        }
    })

    $('#salvar').click(function (e) {
        e.preventDefault()

        _$contasPagarInformationsForm = $('form[name=contasPagarInformationsForm]');
        _$contasPagarInformationsForm.validate();

        if (!_$contasPagarInformationsForm.valid()) {
            return;
        }

        var contasPagar = _$contasPagarInformationsForm.serializeFormToObject();

        contasPagar.ValorAcrescimoDecrescimo = retirarMascara(contasPagar.ValorAcrescimoDecrescimo);
        contasPagar.ValorDocumento = retirarMascara(contasPagar.ValorDocumento);
        contasPagar.valorDesconto = retirarMascara(contasPagar.valorDesconto);

        if ($("#Emissao").val() != null) {
            contasPagar.dataEmissao =
                new Date($("#Emissao").val().substring(6, 10), ($("#Emissao").val().substring(3, 5) - 1), $("#Emissao").val().substring(0, 2));
        }

        _contasPagarService.criarOuEditar(contasPagar)
            .done(function (data) {
                if (data.errors.length > 0) {
                    _ErrorModal.open({ erros: data.errors });
                }
                else {
                    abp.notify.info(app.localize('SavedSuccessfully'));

                    location.href = '/mpa/ContasPagar';
                }
            })
            .always(function () {
            });
    });

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
                            getQuitacoes();
                        });

                    $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                        .appendTo($span)
                        .click(function (e) {
                            e.preventDefault();
                            deleteRegistro(data.record);
                        });

                    var btnAttachment = '<button class="btn btn-default btn-xs" title="' + app.localize('Attachment') + '"><i class="fa fa-paperclip"></i></button>';

                    if (data.record.AnexoListaId != null) {
                        btnAttachment = '<button class="btn btn-info btn-xs" title="' + app.localize('Attachment') + '"><i class="fa fa-paperclip"></i></button>';
                    }

                    $(btnAttachment)
                        .appendTo($span)
                        .click(function (e) {
                            e.preventDefault();
                            _anexosModal.open({ anexoListaId: data.record.AnexoListaId, origemAnexoId: data.record.Id, origemAnexoTabela: 'finlancamento' });
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
                        return posicionarDireita(formatarValor(data.record.ValorLancamento));
                    }
                }
            },

            NossoNumero: {
                title: app.localize('NossoNumero'),
                width: '10%',
                display: function (data) {
                    return data.record.NossoNumero;
                }
            },

            Competencia: {
                title: app.localize('Competencia'),
                width: '10%',
                display: function (data) {
                    if (data.record?.MesCompetencia && data.record?.AnoCompetencia) {
                        if (data.record.MesCompetencia.length == 1) {
                            return ('0' + data.record.MesCompetencia) + "/" + data.record.AnoCompetencia;
                        } else {
                            return data.record.MesCompetencia + "/" + data.record.AnoCompetencia;
                        }
                    }
                }
            },

            Total: {
                title: app.localize('Total'),
                width: '10%',
                display: function (data) {

                    if (data.record.TotalLancamento) {
                        return posicionarDireita(formatarValor(data.record.TotalLancamento));
                    }
                }
            },

        }
    });

    var lista = [];


    function getRegistros() {

        lista = JSON.parse($('#lancamentosJson').val());

        var allRows = _$lancamentosTable.find('.jtable-data-row')

        $.each(allRows, function () {
            var id = $(this).attr('data-record-key');
            _$lancamentosTable.jtable('deleteRecord', { key: id, clientOnly: true });
        });

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

    getQuitacoes();

    getRegistros();

    $('#inserir').click(function (e) {
        e.preventDefault();


        var _$lancamentoInformationsForm = $('form[name=LancamentoInformationsForm]');
        //_$lancamentoInformationsForm.validate();

        //if (!_$lancamentoInformationsForm.valid()) {
        //    return;
        //}

        //$('#nossoNumero').validate();


        //if (!$('#nossoNumero').valid) {
        //    return;
        //}

        AltararValidacaoParcela(true);

        if (!ValidarParcela()) {
            AltararValidacaoParcela(false);
            return;
        }

        AltararValidacaoParcela(false);

        var lancamento = _$lancamentoInformationsForm.serializeFormToObject();

        if ($('#lancamentosJson').val() != '') {
            lista = JSON.parse($('#lancamentosJson').val());
        }

        if ($('#idGridLancamento').val() != '') {

            for (var i = 0; i < lista.length; i++) {
                if (lista[i].IdGrid == $('#idGridLancamento').val()) {

                    //var situacao = $('#situacaoId').select2('data');
                    //if (situacao && situacao.length > 0) {

                    //    lista[i].SituacaoDescricao = situacao[0].text;
                    //}


                    lista[i].SituacaoDescricao = $('#situacaoDescricao').val();

                    lista[i].SituacaoLancamentoId = $('#situacaoId').val();
                    lista[i].ValorLancamento = retirarMascara($('#valor').val());
                    lista[i].ValorAcrescimoDecrescimo = retirarMascara($('#acrescimoDecrescimo').val());
                    lista[i].Juros = retirarMascara($('#juros').val());
                    lista[i].Multa = retirarMascara($('#multa').val());
                    lista[i].TotalLancamento = retirarMascara($('#total').val());
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
            lancamento.SituacaoDescricao = $('#situacaoDescricao').val();
            lancamento.SituacaoLancamentoId = $('#situacaoId').val();
            lancamento.ValorLancamento = retirarMascara($('#valor').val());
            lancamento.ValorAcrescimoDecrescimo = retirarMascara($('#acrescimoDecrescimo').val());
            lancamento.Juros = retirarMascara($('#juros').val());
            lancamento.Multa = retirarMascara($('#multa').val());
            lancamento.TotalLancamento = retirarMascara($('#total').val());
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

        $('#valor').focus();
    });

    $('.calcularTotal').on('blur', function (e) {
        e.preventDefault();

        calcularTotal();
    });

    $('.calcularTotalDocumento').on('blur', function (e) {
        e.preventDefault();

        var valorDocumento = $('#valorDocumento').val() != '' ? parseFloat(retirarMascara($('#valorDocumento').val())) : 0;
        var valorAcrescimoDecrescimo = $('#valorAcrescimoDecrescimo').val() != '' ? parseFloat(retirarMascara($('#valorAcrescimoDecrescimo').val())) : 0;
        var valorDesconto = $('#valorDesconto').val() != '' ? parseFloat(retirarMascara($('#valorDesconto').val())) : 0;
        var total = valorDocumento + valorAcrescimoDecrescimo - valorDesconto;
        var totalFormatado = total.toLocaleString("pt-BR", { style: "currency", currency: "BRL" }).replace('R', '').replace('$', '');

        $('#valorTotal').val(totalFormatado);
    });

    function calcularTotal() {
        var total = 0;

        var valor = $('#valor').val() != '' ? parseFloat(retirarMascara($('#valor').val())) : 0;
        var juros = $('#juros').val() != '' ? parseFloat(retirarMascara($('#juros').val())) : 0;
        var multa = $('#multa').val() != '' ? parseFloat(retirarMascara($('#multa').val())) : 0;
        var acrescimoDecrescimo = $('#acrescimoDecrescimo').val() != '' ? parseFloat(retirarMascara($('#acrescimoDecrescimo').val())) : 0;

        total = valor + juros + multa + acrescimoDecrescimo;
        var totalFormatado = formatarValor(total);

        $('#total').val(totalFormatado);
    }

    $('#situacaoId').on('change', function (e) {

        if ($('#situacaoId').val() != '' && $('#situacaoId').val() != null) {
            _situacaoLancamentoService.obter($('#situacaoId').val())
                .done(function (data) {
                    $('#corLancamentoFundo').val(data.corLancamentoFundo);
                    $('#corLancamentoLetra').val(data.corLancamentoLetra);

                });
        }

    });

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

    $('input[name="DataCadastro"]').daterangepicker({
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
            $('input[name="DataCadastro"]').val(selDate.format('L')).addClass('form-control edited');
        });

    $('input[name="DataPrimeiraParcela"]').daterangepicker(datePickerOptions,
        function (selDate) {
            $('input[name="DataPrimeiraParcela"]').val(selDate.format('L')).addClass('form-control edited');
        });

    $('input[name="DataEmissao"]').daterangepicker(datePickerOptions,
        function (selDate) {
            $('input[name="DataEmissao"]').val(selDate.format('L')).addClass('form-control edited');
        });

    _$quitacoesTable.jtable({

        title: app.localize('Quitacoes'),
        sorting: true,
        edit: false,
        create: false,
        multiSorting: true,


        actions: {
            listAction: {
                method: _quitacaoService.listarQuitacoesPorLancamento
            }
        },

        fields: {
            IdGrid: {
                key: true,
                list: false
            },


            DataMovimento: {
                title: app.localize('DataMovimento'),
                width: '10%',
                display: function (data) {
                    return moment(data.record.dataMovimento).format('L');
                }
            },

            Juros: {
                title: app.localize('Juros'),
                width: '10%',
                display: function (data) {

                    if (data.record.juros) {
                        return posicionarDireita(formatarValor(data.record.juros));
                    }
                }
            },

            Multa: {
                title: app.localize('Multa'),
                width: '10%',
                display: function (data) {

                    if (data.record.moraMulta) {
                        return posicionarDireita(formatarValor(data.record.moraMulta));
                    }
                }
            },

            AcrescimoDecrescimo: {
                title: app.localize('Acrescimo'),
                width: '10%',
                display: function (data) {

                    if (data.record.acrescimo) {
                        return posicionarDireita(formatarValor(data.record.acrescimo));
                    }
                }
            },

            ValorQuitacao: {
                title: app.localize('Valor'),
                width: '10%',
                display: function (data) {

                    if (data.record.valorQuitacao) {
                        return posicionarDireita(formatarValor(data.record.valorQuitacao));
                    }
                }
            },

            MeioPagamento: {
                title: app.localize('MeioPagamento'),
                width: '10%',
                display: function (data) {
                    return data.record.meioPagamento;
                }
            },

            Numero: {
                title: app.localize('Numero'),
                width: '10%',
                display: function (data) {
                    return data.record.numero;
                }
            },

            ContaCorrente: {
                title: app.localize('ContaCorrente'),
                width: '10%',
                display: function (data) {
                    return data.record.contaCorrente;
                }
            },


        }
    });

    function getQuitacoes(reload) {

        if ($('#lancamentoId').val() != '') {
            if (reload) {
                _$quitacoesTable.jtable('reload');
            } else {


                _$quitacoesTable.jtable('load', {
                    filtro: $('#lancamentoId').val()
                });
            }
        }
    }

    function retirarMascara(valor) {

        while (valor.indexOf('.') != -1) valor = valor.replace('.', '');
        valor = valor.replace(',', '.');
        return valor;
    }

    function editRegistro(lancamento) {

        $('#lancamentoId').val(lancamento.Id);
        $('#valor').val(formatarValor(lancamento.ValorLancamento));
        $('#dataVencimento').val(lancamento.DataVencimento);
        $('#idGridLancamento').val(lancamento.IdGrid);
        $('#corFundoLancamento').val(lancamento.CorFundoLancamento);
        $('#corLentraLancamento').val(lancamento.CorLentraLancamento);
        $('#juros').val(formatarValor(lancamento.Juros));
        $('#multa').val(formatarValor(lancamento.Multa));
        $('#acrescimoDecrescimo').val(formatarValor(lancamento.ValorAcrescimoDecrescimo));
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

    function CalculaValorLancamento() {
        var totalLancamento = 0;

        for (var i = 0; i < lista.length; i++) {
            totalLancamento += parseFloat(lista[i].ValorLancamento);
        }

        $('#valorTotalParcelas').val(formatarValor(totalLancamento));
    }

    getQuitacoes();


    $('#dataCadastro').on('blur', function (e) {
        e.preventDefault();

        var dataCadatro = $('#dataCadastro').val();

        var lista = dataCadatro.split('/');

        if (lista.length > 0 && $('#mes').val() == '') {
            $('#mes').val(lista[1]);
        }

        if (lista.length > 1 && $('#ano').val() == '') {
            $('#ano').val(lista[2]);
        }

    });

    $('#gerarParcelas').on('click', function (e) {
        e.preventDefault();

        if ($('#valorDocumento').val() != '' && $('#quantidadeParcelas').val() != '') {
            var valorDocumento = parseFloat(retirarMascara($('#valorTotal').val()));

            var parcelaInicial = parseInt($('#parcelaInicial').val());

            var quantidadeTotalParcelas = parseFloat($('#quantidadeParcelas').val());

            var quantidadeParcelas = quantidadeTotalParcelas - parcelaInicial + 1;

            var valorParcela = valorDocumento / quantidadeParcelas;

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
                lancamento.NossoNumero = $('#numero').val() + ' - ' + lancamento.Parcela + '/' + quantidadeTotalParcelas;

                if (i == quantidadeParcelas - 1) {

                    var somaTodasParcelasLancadas = 0;

                    for (var j = 0; j < lista.length; j++) {
                        somaTodasParcelasLancadas += parseFloat(lista[j].ValorLancamento.toFixed(2));
                    }

                    lancamento.ValorLancamento = parseFloat((valorDocumento - somaTodasParcelasLancadas).toFixed(2));


                } else {
                    lancamento.ValorLancamento = parseFloat(valorParcela.toFixed(2));;
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

    $('#btnAnexosDocumento').on('click', function (e) {
        e.preventDefault();
        _anexosModal.open({ anexoListaId: $("#anexoListaIdDocumento").val(), origemAnexoId: $("#id").val(), origemAnexoTabela: 'findocumento' });
    });

    $('.limpar').on('click', function () {
        limpar();
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

        CalculaValorLancamento();

        $('#inserir > i').removeClass('fa-check');
        $('#inserir > i').addClass('fa-plus');
    }

    function InserirValidacaoParcela() {
        $('#nossoNumero').prop('required', true);
    }

    function AltararValidacaoParcela(obrigatorio) {
        $('#nossoNumero').prop('required', obrigatorio);
        $('#valor').prop('required', obrigatorio);
        $('#dataEmissao').prop('required', obrigatorio);
        $('#dataVencimento').prop('required', obrigatorio);
        $('#mes').prop('required', obrigatorio);
        $('#ano').prop('required', obrigatorio);

    }

    function ValidarParcela() {
        $('#nossoNumero').validate();

        $('#valor').validate();
        $('#dataEmissao').validate();
        $('#dataVencimento').validate();
        $('#mes').validate();
        $('#ano').validate();

        return ($('#nossoNumero').valid()
            && $('#valor').valid()
            //  && $('#dataEmissao').valid()
            //  && $('#dataVencimento').valid()
            && $('#mes').valid()
            && $('#ano').valid());
    }


})(jQuery);