(function ($) {
    app.modals.CriarOuEditarQuitacaoModal = function () {

        $(document).ready(function () {
            CamposRequeridos();
            
            $('#valor').mask('000.000.000,00', { reverse: true });
            $('#juros').mask('000.000.000,00', { reverse: true });
            $('#multa').mask('000.000.000,00', { reverse: true });
            $('#acrescimoDecrescimo').mask('000.000.000,00', { reverse: true });
            $('#total').mask('000.000.000,00', { reverse: true });
        });

        $('.modal-dialog').css('width', '900px');

        var _meioPagamentoService = abp.services.app.meioPagamento;
        var _quitacaoService = abp.services.app.quitacaoContasPagar;
        var _modalManager;
        var _$contasPagarInformationsForm = null;
        var _$lancamentosTable = $('#lancamentosTable');

        var _ErrorModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Erros/ExibirErros',
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$contasPagarInformationsForm = $('form[name=contasPagarInformationsForm]');
            _$contasPagarInformationsForm.validate();
        };

        $('#salvar').click(function (e) {
            e.preventDefault()
            _$quitacaoLancamentosInformationsForm = $('form[name=QuitacaoLancamentosInformationsForm]');
            _$quitacaoLancamentosInformationsForm.validate();

            if (!_$quitacaoLancamentosInformationsForm.valid()) {
                return;
            }

            var quitacao = _$quitacaoLancamentosInformationsForm.serializeFormToObject();

            _quitacaoService.criarOuEditar(quitacao)
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

        $('.close').on('click', function () {
            location.href = '/mpa/ContasPagar';
        });

        $('.close-button').on('click', function () {
            location.href = '/mpa/ContasPagar';
        });

        _$lancamentosTable.jtable({

            title: app.localize('Parcelas'),
            sorting: true,
            edit: false,
            create: false,
            multiSorting: true,

            //rowUpdated: function (event, data) {
            //    if (data) {
            //        if (data.record.CorLancamentoLetra) {
            //            data.row.css("background", data.record.CorLancamentoFundo);
            //            data.row.css("color", data.record.CorLancamentoLetra);
            //        }
            //    }
            //},

            //rowInserted: function (event, data) {
            //    if (data) {
            //        if (data.record.CorLancamentoLetra) {
            //            data.row.css("background", data.record.CorLancamentoFundo);
            //            data.row.css("color", data.record.CorLancamentoLetra);
            //        }
            //    }



            //},

            fields: {
                IdGrid: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '10%',
                    sorting: false,
                    display: function (data) {
                        var $span = $('<span></span>');
                        $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                            .appendTo($span)
                            .click(function (e) {
                                e.preventDefault();
                                editRegistro(data.record)
                            });

                        //$('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                        //    .appendTo($span)
                        //    .click(function (e) {
                        //        e.preventDefault();
                        //        deleteRegistro(data.record);
                        //    });

                        return $span;
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
                    title: app.localize('Vencimento2'),
                    width: '10%',
                    display: function (data) {
                        return data.record.DataVencimento;
                    }
                },
                Documento: {
                    title: app.localize('Documento'),
                    width: '15%',
                    display: function (data) {
                        return data.record.Documento;
                    }
                },

                Fornecedor: {
                    title: app.localize('Fornecedor'),
                    width: '15%',
                    display: function (data) {
                        return data.record.Fornecedor;
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



                ValorRestante: {
                    title: app.localize('ValorRestante'),
                    width: '10%',
                    display: function (data) {

                        if (data.record.ValorRestante) {
                            
                            return posicionarDireita(formatarValor(data.record.ValorRestante));
                        }
                    }
                },

                ValorQuitacao: {
                    title: app.localize('ValorQuitado'),
                    width: '10%',
                    display: function (data) {
                        if (data.record.ValorQuitacao) {
                            return posicionarDireita(formatarValor(data.record.ValorQuitacao));
                        }
                    }
                },

                ValorEfetivo: {
                    title: app.localize('ValorEfetivo'),
                    width: '10%',
                    display: function (data) {

                        if (data.record.ValorRestanteEfetivado) {
                            return posicionarDireita(formatarValor(data.record.ValorRestanteEfetivado));
                        }
                    }
                },

            }
        });

        var lista = [];

        function getRegistros() {
            var totalQuitacao = 0;                        
            var allRows = _$lancamentosTable.find('.jtable-data-row')
            lista = JSON.parse($('#lancamentosJson').val());

            $.each(allRows, function () {
                var id = $(this).attr('data-record-key');
                _$lancamentosTable.jtable('deleteRecord', { key: id, clientOnly: true });
            });

            for (var i = 0; i < lista.length; i++) {
                var item = lista[i];

                item.DataVencimento = moment(item.DataVencimento).format('L');
                item.DataLancamento = moment(item.DataLancamento).format('L');
                item.ValorEfetivo = item.ValorRestante;
                item.ValorQuitacao = item.ValorRestante;
                _$lancamentosTable.jtable('addRecord', {
                    record: item
                    , clientOnly: true
                });

                totalQuitacao += item.ValorQuitacao;
            }
            $('#valorTotalQuitacao').val(formatarValor(totalQuitacao));
            $('#lancamentosJson').val(JSON.stringify(lista));
            debugger;
        }

        getRegistros();

        $('#inserir').click(function (e) {
            e.preventDefault();

            var _$lancamentoInformationsForm = $('form[name=LancamentoInformationsForm]');
            var lancamento = _$lancamentoInformationsForm.serializeFormToObject();
            
            if ($('#lancamentosJson').val() != '') {
                lista = JSON.parse($('#lancamentosJson').val());
            }

            if ($('#idGridLancamento').val() != '') {

                for (var i = 0; i < lista.length; i++) {
                    if (lista[i].IdGrid == $('#idGridLancamento').val()) {

                        
                        var valorQuitacao = $('#valor').val() != '' ? parseFloat(retirarMascara($('#valor').val())) : 0;

                        lista[i].ValorRestante = lista[i].ValorRestanteEfetivado - valorQuitacao;

                        lista[i].ValorEfetivo = retirarMascara($('#valor').val());
                        lista[i].ValorQuitacao = retirarMascara($('#total').val());
                        lista[i].Acrescimo = retirarMascara($('#acrescimoDecrescimo').val());
                        lista[i].Juros = retirarMascara($('#juros').val());
                        lista[i].MoraMulta = retirarMascara($('#multa').val());
                        //lista[i].Total = retirarMascara($('#total').val());
                        //  lista[i].DataVencimento = moment(lista[i].DataVencimento).format('L');
                        lista[i].DataVencimento = $('#dataVencimento').val();
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

                lancamento.ValorEfetivo = retirarMascara($('#valor').val());
                lancamento.Acrescimo = retirarMascara($('#acrescimoDecrescimo').val());
                lancamento.MoraMulta = retirarMascara($('#juros').val());
                lancamento.Multa = retirarMascara($('#multa').val());
                lancamento.Total = retirarMascara($('#total').val());

                lista.push(lancamento);

                _$lancamentosTable.jtable('addRecord', {
                    record: lancamento
                  , clientOnly: true
                });

            }

            $('#lancamentosJson').val(JSON.stringify(lista));
            $('#idGridLancamento').val('');
            $('#valor').val('');
            $('#acrescimoDecrescimo').val('');
            $('#juros').val('');
            $('#multa').val('');
            $('#total').val('');

            //            CalculaValorLancamento();

            $('#valor').focus();

            var totalQuitacao = 0;

            for (var i = 0; i < lista.length; i++) {

                if (lista[i].ValorEfetivo != null && lista[i].ValorEfetivo != '') {
                    var valorEfetivo = parseFloat(lista[i].ValorEfetivo);
                    totalQuitacao = parseFloat(totalQuitacao) + parseFloat(valorEfetivo);
                }
            }
            $('#valorTotalQuitacao').val(formatarValor(totalQuitacao));
        });

        $('.calcularTotal').on('blur', function (e) {
            e.preventDefault();
            
            calcularTotal();
        });

        function calcularTotal() {
            var total = 0;

            var valor = $('#valor').val() != '' ? parseFloat(retirarMascara($('#valor').val())) : 0;
            var juros = $('#juros').val() != '' ? parseFloat(retirarMascara($('#juros').val())) : 0;
            var multa = $('#multa').val() != '' ? parseFloat(retirarMascara($('#multa').val())) : 0;
            var acrescimoDecrescimo = $('#acrescimoDecrescimo').val() != '' ? parseFloat(retirarMascara($('#acrescimoDecrescimo').val())) : 0;

            total = valor +  juros + multa + acrescimoDecrescimo;
            var totalFormatado = formatarValor(total);

            $('#total').val(totalFormatado);
        }


        function retirarMascara(valor) {

            while (valor.indexOf('.') != -1) valor = valor.replace('.', '');
            valor = valor.replace(',', '.');
            return (valor != '') ? parseFloat(valor) : '';
        }

        function editRegistro(lancamento) {


            $('#parcela').val(lancamento.Parcela);

            
            $('#vencimentoQuitacao').val(lancamento.DataVencimento);
            $('#documentoQuitacao').val(lancamento.Documento);
            $('#fornecedor').val(lancamento.Fornecedor);
            $('#valorLancamento').val(formatarValor(lancamento.ValorLancamento));

            if (lancamento.ValorEfetivo == null || lancamento.ValorEfetivo == '' || lancamento.ValorEfetivo == 0 )
            {
                $('#valor').val(formatarValor(lancamento.ValorRestante));
            }
            else
            {
                $('#valor').val(formatarValor(lancamento.ValorEfetivo));
            }

            
            $('#total').val(formatarValor(lancamento.ValorQuitacao));
            $('#juros').val(formatarValor(lancamento.Juros));
            $('#multa').val(formatarValor(lancamento.MoraMulta));
            $('#acrescimoDecrescimo').val(formatarValor(lancamento.Acrescimo));//.toLocaleString("pt-BR", { style: "currency", currency: "BRL" }).replace('R', '').replace('$', ''));

            $('#idGridLancamento').val(lancamento.IdGrid);

            calcularTotal();

        }

        //function deleteRegistro(lancamento) {
        //    abp.message.confirm(
        //        app.localize('DeleteWarning', lancamento.NossoNumero),
        //        function (isConfirmed) {
        //            if (isConfirmed) {



        //                lista = JSON.parse($('#lancamentosJson').val());

        //                for (var i = 0; i < lista.length; i++) {
        //                    if (lista[i].IdGrid == lancamento.IdGrid) {
        //                        lista.splice(i, 1);
        //                        $('#lancamentosJson').val(JSON.stringify(lista));

        //                        _$lancamentosTable.jtable('deleteRecord', {
        //                            key: lancamento.IdGrid
        //                        , clientOnly: true
        //                        });

        //                        break;
        //                    }
        //                }

        //                CalculaValorLancamento();
        //            }
        //        }
        //    );
        //}

        //function formatarValor(valor) {
        //    if (valor != '' && valor != null) {
        //        var retorno = valor.toLocaleString("pt-BR", { style: "currency", currency: "BRL" }).replace('R', '').replace('$', '');
        //        return retorno;
        //    }
        //    return '';

        //}

        //function CalculaValorLancamento() {
        //    var totalLancamento = 0;

        //    for (var i = 0; i < lista.length; i++) {
        //        totalLancamento += parseFloat(lista[i].ValorLancamento);
        //    }

        //    $('#valorTotalParcelas').val(formatarValor(totalLancamento));
        //}



        $('input[name="DataMovimento').daterangepicker({
            "singleDatePicker": true,
            "showDropdowns": true,
            autoUpdateInput: false,
            //   maxDate: new Date(),
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
            $('input[name="DataMovimento"]').val(selDate.format('L')).addClass('form-control edited');
        });

        $('input[name="DataCompensado').daterangepicker({
            "singleDatePicker": true,
            "showDropdowns": true,
            autoUpdateInput: false,
            //   maxDate: new Date(),
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
            $('input[name="DataCompensado"]').val(selDate.format('L')).addClass('form-control edited');
        });

        $('input[name="DataConsolidado').daterangepicker({
            "singleDatePicker": true,
            "showDropdowns": true,
            autoUpdateInput: false,
            //   maxDate: new Date(),
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
            $('input[name="DataConsolidado"]').val(selDate.format('L')).addClass('form-control edited');
        });

        $('input[name="Vencimento').daterangepicker({
            "singleDatePicker": true,
            "showDropdowns": true,
            autoUpdateInput: false,
            //   maxDate: new Date(),
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
           $('input[name="Vencimento"]').val(selDate.format('L')).addClass('form-control edited');
       });
        
        selectSW('.selectEmpresa', "/api/services/app/empresa/ListarDropdownPorUsuario");
        selectSW('.selectcontaCorrente', "/api/services/app/ContaCorrente/ListarPorEmpresaDropdown", $('#empresaId'));
        selectSW('.selectmeioPagamento', "/api/services/app/MeioPagamento/ListarDropdown");
        selectSW('.selectCheque', "/api/services/app/Cheque/ListarChequeNaoUtilziadoPorContaCorrenteDropdown", $('#contaCorrenteId'));

        $('#empresaId').on('change', function (e) {
            e.preventDefault();
            selectSW('.selectcontaCorrente', "/api/services/app/ContaCorrente/ListarPorEmpresaDropdown", $('#empresaId'));
        });



        $('#meioPagamentoId').on('change', function (e) {
            e.preventDefault();
            
            if ($('#meioPagamentoId').val() != '' && $('#meioPagamentoId').val() != null) {

                _meioPagamentoService.obter($('#meioPagamentoId').val())
                .done(function (data) {

                    //tipo de pagamento em cheque
                    if (data.tipoMeioPagamentoId == 2) {
                        $('#divNumero').hide();
                        $('#divCheque').show();

                        selectSW('.selectCheque', "/api/services/app/Cheque/ListarChequeNaoUtilziadoPorContaCorrenteDropdown", $('#contaCorrenteId'));
                    }
                    else {
                        $('#divNumero').show();
                        $('#divCheque').hide();
                    }

                })
             .always(function () {
             });

            }

        });
    }
})(jQuery);