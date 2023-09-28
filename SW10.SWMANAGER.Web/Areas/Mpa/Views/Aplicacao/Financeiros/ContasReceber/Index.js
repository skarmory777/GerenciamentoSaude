(function () {
    $(function () {
        selectSW('.selectForncedor', "/api/services/app/sisPessoa/ListarDropdownSisIsPagar", { valor: 'true' });
        selectSW('.selectEmpresa', "/api/services/app/empresa/ListarDropdownPorUsuario");
        selectSW('.selectSituacao', "/api/services/app/SituacaoLancamento/ListarDropdown");
        selectSW('.selectContaAdministrativa', "/api/services/app/ContaAdministrativa/ListarContaAdministrivaDespesaDropdown");
        selectSW('.selectCentroCusto', "/api/services/app/CentroCusto/ListarDropdownCodigoCentroCusto");
        selectSW('.selectMeioPagamento', "/api/services/app/MeioPagamento/ListarDropdown");
        selectSW('.selectTipoDocumento', "/api/services/app/tipoDocumento/ListarDropdown");

        var _$contasPagarTable = $('#contasPagarTable');
        var _contasReceberService = abp.services.app.contasReceber;
        var _$contasPagarFilterForm = $('#contasPagarFilterForm');

        var _selectedDateRangeVencimento = {
            startDate: moment().startOf('month'),
            endDate: moment().endOf('month'),
            maxDate: "31/12/2030"
        };

        _$contasPagarFilterForm.find('input.vencimento').daterangepicker(
            $.extend(true, app.createDateRangePickerOptions(), _selectedDateRangeVencimento),
            function (start, end, label) {
                _selectedDateRangeVencimento.startDate = start.format('YYYY-MM-DDT00:00:00Z');
                _selectedDateRangeVencimento.endDate = end.format('YYYY-MM-DDT23:59:59.999Z');
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Financeiro.ContasReceber.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Financeiro.ContasReceber.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Financeiro.ContasReceber.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Quitacao/CriarOuEditarModalPorLancamentos',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Financeiros/ContasReceber/_CriarOuEditarModalQuitacaoLancamentos.js',
            modalClass: 'CriarOuEditarQuitacaoModal'
        });

        var _ErrorModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Erros/ExibirErros',
        });

        getFiltersFromLocalStorage();

        _$contasPagarTable.jtable({

            title: app.localize('ContasReceber'),
            paging: true,
            sorting: true,
            multiSorting: true,
            selecting: true,
            selectingCheckboxes: true,
            multiselect: true,

            rowUpdated: function (event, data) {
                if (data) {
                    if (data.record.corLancamentoLetra) {
                        data.row[0].cells[2].setAttribute('color', data.record.corLancamentoLetra);
                    }

                    if (data.record.corLancamentoFundo) {
                        data.row[0].cells[2].setAttribute('bgcolor', data.record.corLancamentoFundo);
                    }
                }
            },

            rowInserted: function (event, data) {
                if (data) {
                    if (data.record.corLancamentoLetra) {
                        data.row[0].cells[2].setAttribute('color', data.record.corLancamentoLetra);
                    }

                    if (data.record.corLancamentoFundo) {
                        data.row[0].cells[2].setAttribute('bgcolor', data.record.corLancamentoFundo);
                    }
                }
            },

            actions: {
                listAction: {
                    method: _contasReceberService.listarLancamento
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
                        if (_permissions.edit) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    location.href = 'ContasReceber/CriarOuEditarModal/' + data.record.id
                                });
                        }

                        if (_permissions.delete) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    deleteRegistro(data.record);
                                });
                        }

                        if (data.record.fatEntregaLoteId != null) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Contas') + '"><i class="fa fa-file"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    window.open(
                                        'FaturamentoLotes/lote?loteId=' + data.record.fatEntregaLoteId,
                                        '_blank'
                                    );
                                });
                        }

                        return $span;
                    }
                },

                SituacaoLancamento: {
                    title: app.localize('Situacao'),
                    width: '5%',
                    display: function (data) {
                        return data.record.situacaoDescricao;
                    }
                },

                Emissao: {
                    title: app.localize('Emissao'),
                    width: '5%',
                    display: function (data) {
                        if (data.record.dataEmissao) {
                            return moment(data.record.dataEmissao).format('L');
                        }                        
                    }
                },

                Vencimento: {
                    title: app.localize('Vencimento'),
                    width: '5%',
                    display: function (data) {
                        if (data.record.dataVencimento) {
                            return moment(data.record.dataVencimento).format('L');
                        }
                    }
                },

                Competencia: {
                    title: app.localize('Competencia'),
                    width: '5%',
                    display: function (data) {
                        if (data.record.competencia) {
                            return data.record.competencia;
                        }
                    }
                },

                Documento: {
                    title: app.localize('Documento'),
                    width: '5%',
                    display: function (data) {
                        if (data) {
                            return data.record.documento;
                        }
                    }
                },
                Pessoa: {
                    title: app.localize('Pessoa'),
                    width: '15%',
                    display: function (data) {
                        if (data) {
                            return data.record.fornecedor;
                        }
                    }
                },

                TotalLancamento: {
                    title: app.localize('TotalLancamento'),
                    width: '5%',
                    display: function (data) {
                        if (data) {
                            return posicionarDireita(formatarValor(data.record.totalLancamento));
                        }
                    }
                },

                TotalQuitacao: {
                    title: app.localize('TotalQuitacao'),
                    width: '5%',
                    display: function (data) {
                        if (data) {
                            return posicionarDireita(formatarValor(data.record.totalQuitacao));
                        }
                    }
                },

                Empresa: {
                    title: app.localize('Empresa'),
                    width: '10%',
                    display: function (data) {
                        if (data) {
                            return data.record.empresaNome;
                        }
                    }
                },
            }
        });

        function getRegistros(reload) {
            setFiltersToLocalStorage();
            if (reload) {
                _$contasPagarTable.jtable('reload');
            } else {
                _$contasPagarTable.jtable('load', {
                    pessoaId: $('#pessoaId').val(),
                    emissaoDe: _selectedDateRange.startDate,
                    emissaoAte: _selectedDateRange.endDate,
                    empresaId: $('#empresaFiltroId').val(),
                    situacaoLancamentoId: $('#situacaoId').val(),
                    documento: $('#documento').val(),
                    contaAdministrativaId: $('#contaAdministrativaId').val(),
                    centroCustoId: $('#centroCustoId').val(),
                    vencimentoDe: $('#vencimento').val() != '' ? _selectedDateRangeVencimento.startDate : '',
                    vencimentoAte: $('#vencimento').val() != '' ? _selectedDateRangeVencimento.endDate : '',
                    meioPagamentoId: $('#meioPagamentoFiltroId').val(),
                    tipoDocumentoId: $('#tipoDocumentoId').val(),
                    ignorarVencimento: $('#ignorarVencimento')[0].checked,
                    ignorarEmissao: $('#ignorarEmissao')[0].checked

                });
            }
        }

        function deleteRegistro(record) {

            abp.message.confirm(
                app.localize('DeleteWarning', record.fornecedor),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _contasReceberService.excluir(record.id)
                            .done(function (data) {
                                getRegistros(true);
                                if (data.errors.length > 0) {
                                    _ErrorModal.open({ erros: data.errors });
                                }
                                else {
                                    abp.notify.success(app.localize('SuccessfullyDeleted'));
                                }
                            });
                    }
                }
            );
        }

        function createRequestParams() {
            var prms = {};
            _$filterForm.serializeArray().map(function (x) { prms[x.name] = x.value; });
            return $.extend(prms);
        }

        $('#ShowAdvancedFiltersSpan').click(function () {
            $('#ShowAdvancedFiltersSpan').hide();
            $('#HideAdvancedFiltersSpan').show();
            $('#AdvacedLancamentosFiltersArea').slideDown();
        });

        $('#HideAdvancedFiltersSpan').click(function () {
            $('#HideAdvancedFiltersSpan').hide();
            $('#ShowAdvancedFiltersSpan').show();
            $('#AdvacedLancamentosFiltersArea').slideUp();
        });

        $('.novo-lancamento').click(function (e) {
            //_createOrEditModal.open();
            e.preventDefault();

            location.href = 'ContasReceber/CriarOuEditarModal/';
        });


        $('#refreshLancamentosButton').click(function (e) {
            e.preventDefault();
            getRegistros();
        });

        abp.event.on('app.CriarOuEditarFeriadoModalSaved', function () {
            getRegistros(true);
        });



        $('#tableFilter').focus();


        $('#btnQuitarLancamentos').click(function (e) {
            e.preventDefault();

            var lancamentosSelecionados = _$contasPagarTable.jtable('selectedRows');

            var listaId = [];

            lancamentosSelecionados.each(function () {
                var registro = $(this).data('record');
                //      //console.log(JSON.stringify(registro));
                listaId.push(registro.id);
            });

            if (listaId.length > 0) {
                _createOrEditModal.open({ ids: listaId });
            }

        });

        $(".ContasAPagarRelatorioPorData").click(function (e) {
            const btn = $(this);
            btn.buttonBusy();
            let startDate = '';
            let endDate = '';
            if ($('#vencimento').val() != '') {
                if (_.isObject(_selectedDateRangeVencimento.startDate) && _selectedDateRangeVencimento.startDate._isAMomentObject) {
                    startDate = _selectedDateRangeVencimento.startDate.format('YYYY-MM-DDT00:00:00Z');
                } else {
                    startDate = _selectedDateRangeVencimento.startDate;
                }
                if (_.isObject(_selectedDateRangeVencimento.endDate) && _selectedDateRangeVencimento.endDate._isAMomentObject) {
                    endDate = _selectedDateRangeVencimento.endDate.format('YYYY-MM-DDT23:59:59.999Z');
                } else {
                    endDate = _selectedDateRangeVencimento.endDate;
                }
            }

            const data = {
                dataInicio: startDate,
                dataFim: endDate,
                isCredito: true,
                empresaId: $('#empresaFiltroId').val(),
                situacaoLancamentoId: $('#situacaoId').val(),
                pessoaId: $('#pessoaId').val()
            };

            $.removeCookie("XSRF-TOKEN");

            const parameters = $.param(data);

            printJS({
                printable: `/Mpa/ContasPagar/GerarRelatorio?${parameters}`,
                type: 'pdf',
                onLoadingStart: () => {
                    abp.ui.setBusy()
                },
                onLoadingEnd: () => {
                    abp.ui.clearBusy()
                },
                onPrintDialogClose: () => {
                    btn.buttonBusy(false);
                }
            })
        });

        $(".ContasAPagarRelatorioPorFornecedor").click(function (e) {
            const btn = $(this);
            btn.buttonBusy();
            let startDate = '';
            let endDate = '';
            if ($('#vencimento').val() != '') {
                if (_.isObject(_selectedDateRangeVencimento.startDate) && _selectedDateRangeVencimento.startDate._isAMomentObject) {
                    startDate = _selectedDateRangeVencimento.startDate.format('YYYY-MM-DDT00:00:00Z');
                } else {
                    startDate = _selectedDateRangeVencimento.startDate;
                }
                if (_.isObject(_selectedDateRangeVencimento.endDate) && _selectedDateRangeVencimento.endDate._isAMomentObject) {
                    endDate = _selectedDateRangeVencimento.endDate.format('YYYY-MM-DDT23:59:59.999Z');
                } else {
                    endDate = _selectedDateRangeVencimento.endDate;
                }
            }

            const data = {
                dataInicio: startDate,
                dataFim: endDate,
                isCredito: true,
                empresaId: $('#empresaFiltroId').val(),
                situacaoLancamentoId: $('#situacaoId').val(),
                pessoaId: $('#pessoaId').val()
            };
            $.removeCookie("XSRF-TOKEN");
            const parameters = $.param(data);
            printJS({
                printable: `/Mpa/ContasPagar/GerarRelatorioGroupNome?${parameters}`,
                type: 'pdf',
                onLoadingStart: () => {
                    abp.ui.setBusy()
                },
                onLoadingEnd: () => {
                    abp.ui.clearBusy()
                },
                onPrintDialogClose: () => {
                    btn.buttonBusy(false);
                }
            })
        });

        $(".QuitacaoPorData").click(function (e) {
            const btn = $(this);
            btn.buttonBusy();
            let startDate = '';
            let endDate = '';
            if ($('#vencimento').val() != '') {
                if (_.isObject(_selectedDateRangeVencimento.startDate) && _selectedDateRangeVencimento.startDate._isAMomentObject) {
                    startDate = _selectedDateRangeVencimento.startDate.format('YYYY-MM-DDT00:00:00Z');
                } else {
                    startDate = _selectedDateRangeVencimento.startDate;
                }
                if (_.isObject(_selectedDateRangeVencimento.endDate) && _selectedDateRangeVencimento.endDate._isAMomentObject) {
                    endDate = _selectedDateRangeVencimento.endDate.format('YYYY-MM-DDT23:59:59.999Z');
                } else {
                    endDate = _selectedDateRangeVencimento.endDate;
                }
            }

            const data = {
                dataInicio: startDate,
                dataFim: endDate,
                isCredito: true,
                empresaId: $('#empresaFiltroId').val(),
                situacaoLancamentoId: $('#situacaoId').val(),
                pessoaId: $('#pessoaId').val()
            };

            $.removeCookie("XSRF-TOKEN");

            const parameters = $.param(data);

            printJS({
                printable: `/Mpa/ContasPagar/GerarRelatorioQuitacao?${parameters}`,
                type: 'pdf',
                onLoadingStart: () => {
                    abp.ui.setBusy()
                },
                onLoadingStart: () => {
                    abp.ui.setBusy()
                },
                onLoadingEnd: () => {
                    abp.ui.clearBusy()
                },
                onPrintDialogClose: () => {
                    btn.buttonBusy(false);
                }
            })
        });


        function formatarValor(valor) {
            if (valor != '' && valor != null) {
                var retorno = valor.toLocaleString("pt-BR", { style: "currency", currency: "BRL" }).replace('R', '').replace('$', '');
                return retorno;
            }
            return '';

        }

        var _selectedDateRange = {
            startDate: moment().startOf('day'),
            endDate: moment().endOf('day'),
            maxDate: "31/12/2030"
        };

        _$contasPagarFilterForm.find('input.emissao').daterangepicker(

            $.extend(true, app.createDateRangePickerOptions(), _selectedDateRange),
            function (start, end, label) {

                _selectedDateRange.startDate = start.format('YYYY-MM-DDT00:00:00Z');
                _selectedDateRange.endDate = end.format('YYYY-MM-DDT23:59:59.999Z');
            });




        

        


        $('#emissao').on('cancel.daterangepicker', function (ev, picker) {
            //do something, like clearing an input
            $('#emissao').val('');
        });

        $('#vencimento').on('cancel.daterangepicker', function (ev, picker) {
            //do something, like clearing an input

            $('#vencimento').val('');
        });

        getRegistros();
        $('#emissao').val('');

        

        function setFiltersToLocalStorage() {
            localStorage['contasReceber[pessoaId]'] = $("#pessoaId").val();
            localStorage['contasReceber[pessoaText]'] = $("#pessoaId").select2('data')[0]?.text;
            localStorage['contasReceber[empresaId]'] = $("#empresaFiltroId").val();
            localStorage['contasReceber[empresaText]'] = $("#empresaFiltroId").select2('data')[0]?.text;
            localStorage['contasReceber[situacaoId]'] = $("#situacaoId").val();
            localStorage['contasReceber[situacaoText]'] = $("#situacaoId").select2('data')[0]?.text;

            if (_$contasPagarFilterForm.find('input.vencimento').data('daterangepicker')['chosenLabel']) {
                localStorage['contasPagar[dataVencimentoLabel]'] = _$contasPagarFilterForm.find('input.vencimento').data('daterangepicker')['chosenLabel'];
            }

            localStorage['contasReceber[dataVencimento]'] = JSON.stringify({
                start: new Date(moment(_selectedDateRangeVencimento.startDate).startOf('day')),
                end: new Date(moment(_selectedDateRangeVencimento.endDate).endOf('day'))
            });
        };

        function getFiltersFromLocalStorage() {

            if (localStorage['contasReceber[dataVencimento]']) {
                let chosenLabel = localStorage['contasReceber[dataVencimentoLabel]'];
                let dateVal;
                if (chosenLabel && chosenLabel != 'undefined' && chosenLabel !== 'Intervalo personalizado') {
                    const datas = _$contasPagarFilterForm.find('input.vencimento').data('ranges')[chosenLabel];

                    dateVal = datas[0].format('DD/MM/YYYY') + ' - ' + datas[1].format('DD/MM/YYYY')
                    _selectedDateRangeVencimento.startDate = datas[0].format('YYYY-MM-DDT00:00:00Z');
                    _selectedDateRangeVencimento.endDate = datas[1].format('YYYY-MM-DDT23:59:59.999Z');
                }
                else {
                    let date = JSON.parse(localStorage['contasReceber[dataVencimento]']);
                    dateVal = moment(date.start).format('DD/MM/YYYY') + ' - ' + moment(date.end).format('DD/MM/YYYY');
                    _selectedDateRangeVencimento.startDate = date.start;
                    _selectedDateRangeVencimento.endDate = date.end;
                }
                _$contasPagarFilterForm.find('input.vencimento').val(dateVal).trigger('change');
            }

            if (localStorage['contasReceber[pessoaId]'] && localStorage['contasReceber[pessoaId]'] != 'null' &&
                localStorage['contasReceber[pessoaText]'] && localStorage['contasReceber[pessoaText]'] != 'undefined') {
                let optionId = localStorage['contasReceber[pessoaId]'];
                let optionText = localStorage['contasReceber[pessoaText]'];
                if ($('#pessoaId').find("option[value='" + optionId + "']").length) {
                    $('#pessoaId').val(optionId).trigger('change');
                } else {
                    // Create a DOM Option and pre-select by default
                    var newOption = new Option(optionText, optionId, true, true);
                    // Append it to the select
                    $('#pessoaId').append(newOption).trigger('change');
                }
            }

            if (localStorage['contasReceber[empresaId]'] && localStorage['contasReceber[empresaId]'] != 'null' &&
                localStorage['contasReceber[empresaText]'] && localStorage['contasReceber[empresaText]'] != 'undefined') {
                let optionId = localStorage['contasReceber[empresaId]'];
                let optionText = localStorage['contasReceber[empresaText]'];
                if ($('#empresaFiltroId').find("option[value='" + optionId + "']").length) {
                    $('#empresaFiltroId').val(optionId).trigger('change');
                } else {
                    // Create a DOM Option and pre-select by default
                    var newOption = new Option(optionText, optionId, true, true);
                    // Append it to the select
                    $('#empresaFiltroId').append(newOption).trigger('change');
                }
            }

            if (localStorage['contasReceber[situacaoId]'] && localStorage['contasReceber[situacaoId]'] != 'null' &&
                localStorage['contasReceber[situacaoText]'] && localStorage['contasReceber[situacaoText]'] != 'undefined') {
                let optionId = localStorage['contasReceber[situacaoId]'];
                let optionText = localStorage['contasReceber[situacaoText]'];
                if ($('#situacaoId').find("option[value='" + optionId + "']").length) {
                    $('#situacaoId').val(optionId).trigger('change');
                } else {
                    // Create a DOM Option and pre-select by default
                    var newOption = new Option(optionText, optionId, true, true);
                    // Append it to the select
                    $('#situacaoId').append(newOption).trigger('change');
                }
            }

        };
    });
})();