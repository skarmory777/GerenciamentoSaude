(function () {
    $(function () {
        selectSW('.selectPessoa', "/api/services/app/sisPessoa/ListarDropdownPessoa");
        selectSW('.selectEmpresa', "/api/services/app/empresa/ListarDropdownPorUsuario");
        selectSW('.selectContaCorrente', "/api/services/app/ContaCorrente/ListarDropdown");
        selectSW('.selectMeioPagamento', "/api/services/app/MeioPagamento/ListarDropdown");

        var _$controleBancarioTable = $('#controleBancarioTable');
        var _quitacaoService = abp.services.app.quitacao;
        var _$controleBancarioFilterForm = $('#controleBancarioFilterForm');

        var _selectedDateRangeMovimento = {
            startDate: moment().startOf('month'),
            endDate: moment().endOf('month'),
            maxDate: "31/12/2030"
        };

        _$controleBancarioFilterForm.find('input.vencimento').daterangepicker(
            $.extend(true, app.createDateRangePickerOptions(), _selectedDateRangeMovimento),
            function (start, end, label) {
                _selectedDateRangeMovimento.startDate = start.format('YYYY-MM-DDT00:00:00Z');
                _selectedDateRangeMovimento.endDate = end.format('YYYY-MM-DDT23:59:59.999Z');
            }
        );

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Financeiro.ContasPagar.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Financeiro.ContasPagar.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Financeiro.ContasPagar.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Quitacao/CriarOuEditarModalPorLancamentos',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Financeiros/ContasPagar/_CriarOuEditarModalQuitacaoLancamentos.js',
            modalClass: 'CriarOuEditarQuitacaoModal'
        });

        var _ErrorModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Erros/ExibirErros',
        });

        getFiltersFromLocalStorage();

        _$controleBancarioTable.jtable({

            title: app.localize('ControleBancario'),
            paging: true,
            sorting: true,
            multiSorting: true,
            selecting: false,
            selectingCheckboxes: true,
            multiselect: false,
            pageSize: 25,

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
                    method: _quitacaoService.listarQuitacoes
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '4%',
                    sorting: false,
                    display: function (data) {
                        var $span = $('<span></span>');
                        //if (_permissions.edit) {
                        //    $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                        //        .appendTo($span)
                        //        .click(function () {
                        //            location.href = 'ContasPagar/CriarOuEditarModal/' + data.record.id
                        //        });
                        //}
                        if (_permissions.delete) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    deleteRegistro(data.record);
                                });
                        }

                        return $span;
                    }
                },

                Quitacao: {
                    title: app.localize('Quitacao'),
                    width: '5%',
                    display: function (data) {
                        return data.record.id;
                    }
                },

                Tipo: {
                    title: app.localize('Tipo'),
                    sorting: false,
                    width: '2%',
                    display: function (data) {
                        if (data.record.isCredito == 1) {
                            return '<span class="spn-tipo" title="' + app.localize('Credito') +'">&#128309;</span>';
                        }
                        if (data.record.isCredito == 0) {
                            return '<span class="spn-tipo" title="' + app.localize('Debito') + '">&#128308;</span>';
                        }
                    }
                },

                DataMovimento: {
                    title: app.localize('DataMovimento'),
                    width: '5%',
                    display: function (data) {
                        if (data.record.dataMovimento) {
                            return moment.utc(data.record.dataMovimento).format('L');
                        }
                    }
                },

                Pessoa: {
                    title: app.localize('Pessoa'),
                    sorting: false,
                    width: '20%',
                    display: function (data) {
                        return data.record.pessoaNome;
                    }
                },

                MeioPagamento: {
                    title: app.localize('MeioPagamento'),
                    width: '5%',
                    display: function (data) {
                        if (data) {
                            return data.record.meioPagamentoDescricao;
                        }
                    }
                },

                ContaCorrente: {
                    title: app.localize('ContaCorrente'),
                    width: '7%',
                    display: function (data) {
                        if (data) {
                            return data.record.contaCorrenteDescricao;
                        }
                    }
                },

                ValorTotal: {
                    title: app.localize('ValorTotal'),
                    width: '5%',
                    sorting: false,
                    display: function (data) {
                        if (data) {
                            return posicionarDireita(formatarValor(data.record.valorTotal));
                        }
                    }
                },

                DataCompensado: {
                    title: app.localize('DataCompensado'),
                    width: '5%',
                    display: function (data) {
                        if (data && data.record?.dataCompensado) {
                            return moment.utc(data.record.dataCompensado).format('L');
                        }
                    }
                },

                

                DataConsolidado: {
                    title: app.localize('DataConsolidado'),
                    width: '5%',
                    display: function (data) {
                        if (data && data.record?.dataConsolidado) {
                            return moment.utc(data.record.dataConsolidado).format('L');
                        }
                    }
                },

                Observacao: {
                    title: app.localize('Observacao'),
                    width: '5%',
                    display: function (data) {
                        if (data) {
                            return data.record.observacao;
                        }
                    }
                }
            }
        });

        function getRegistros(reload) {
            setFiltersToLocalStorage();
            if (reload) {
                _$controleBancarioTable.jtable('reload');
            } else {
                _$controleBancarioTable.jtable('load', {
                    pessoaId: $('#pessoaId').val(),
                    conciliacaoDe: _selectedDateRangeConciliacao.startDate,
                    conciliacaoAte: _selectedDateRangeConciliacao.endDate,
                    empresaId: $('#empresaFiltroId').val(),
                    contaCorrenteId: $('#contaCorrenteId').val(),
                    movimentoDe: $('#vencimento').val() != '' ? _selectedDateRangeMovimento.startDate : '',
                    movimentoAte: $('#vencimento').val() != '' ? _selectedDateRangeMovimento.endDate : '',
                    meioPagamentoId: $('#meioPagamentoFiltroId').val(),
                    tipoDocumentoId: $('#tipoDocumentoId').val(),
                    ignorarDataMovimento: $('#ignorarDataMovimento')[0].checked,
                    ignorarDataConciliacao: $('#ignorarConciliacao')[0].checked
                });
            }
        }

        function deleteRegistro(record) {
            abp.message.confirm(
                app.localize('DeleteWarning', app.localize('Quitacao') + ": " + record.id + " - " + record.pessoaNome),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _quitacaoService.excluir(record.id)
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

        //function createRequestParams() {
        //    var prms = {};
        //    _$filterForm.serializeArray().map(function (x) { prms[x.name] = x.value; });
        //    return $.extend(prms);
        //}

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

        $('#novoLancamento').click(function (e) {
            //_createOrEditModal.open();
            e.preventDefault();

            location.href = 'ControleBancario/CriarOuEditarModal/';
        });

        $('#novaTransferencia').click(function (e) {
            //_createOrEditModalTransferencia.open();
            e.preventDefault();

            location.href = 'ControleBancario/CriarOuEditarModalTransferencia/';
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

            var lancamentosSelecionados = _$controleBancarioTable.jtable('selectedRows');

            var listaId = [];

            lancamentosSelecionados.each(function () {
                var registro = $(this).data('record');

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
                if (_.isObject(_selectedDateRangeMovimento.startDate) && _selectedDateRangeMovimento.startDate._isAMomentObject) {
                    startDate = _selectedDateRangeMovimento.startDate.format('YYYY-MM-DDT00:00:00Z');
                } else {
                    startDate = _selectedDateRangeMovimento.startDate;
                }
                if (_.isObject(_selectedDateRangeMovimento.endDate) && _selectedDateRangeMovimento.endDate._isAMomentObject) {
                    endDate = _selectedDateRangeMovimento.endDate.format('YYYY-MM-DDT23:59:59.999Z');
                } else {
                    endDate = _selectedDateRangeMovimento.endDate;
                }
            }

            const data = {
                dataInicio: startDate,
                dataFim: endDate,
                isCredito: false,
                empresaId: $('#empresaFiltroId').val(),
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
                if (_.isObject(_selectedDateRangeMovimento.startDate) && _selectedDateRangeMovimento.startDate._isAMomentObject) {
                    startDate = _selectedDateRangeMovimento.startDate.format('YYYY-MM-DDT00:00:00Z');
                } else {
                    startDate = _selectedDateRangeMovimento.startDate;
                }
                if (_.isObject(_selectedDateRangeMovimento.endDate) && _selectedDateRangeMovimento.endDate._isAMomentObject) {
                    endDate = _selectedDateRangeMovimento.endDate.format('YYYY-MM-DDT23:59:59.999Z');
                } else {
                    endDate = _selectedDateRangeMovimento.endDate;
                }
            }

            const data = {
                dataInicio: startDate,
                dataFim: endDate,
                isCredito: false,
                empresaId: $('#empresaFiltroId').val(),
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
                if (_.isObject(_selectedDateRangeMovimento.startDate) && _selectedDateRangeMovimento.startDate._isAMomentObject) {
                    startDate = _selectedDateRangeMovimento.startDate.format('YYYY-MM-DDT00:00:00Z');
                } else {
                    startDate = _selectedDateRangeMovimento.startDate;
                }
                if (_.isObject(_selectedDateRangeMovimento.endDate) && _selectedDateRangeMovimento.endDate._isAMomentObject) {
                    endDate = _selectedDateRangeMovimento.endDate.format('YYYY-MM-DDT23:59:59.999Z');
                } else {
                    endDate = _selectedDateRangeMovimento.endDate;
                }
            }

            const data = {
                dataInicio: startDate,
                dataFim: endDate,
                isCredito: false,
                empresaId: $('#empresaFiltroId').val(),
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

        var _selectedDateRangeConciliacao = {
            startDate: moment().startOf('day'),
            endDate: moment().endOf('day'),
            maxDate: "31/12/2030"
        };

        _$controleBancarioFilterForm.find('input.emissao').daterangepicker(

            $.extend(true, app.createDateRangePickerOptions(), _selectedDateRangeConciliacao),
            function (start, end, label) {

                _selectedDateRangeConciliacao.startDate = start.format('YYYY-MM-DDT00:00:00Z');
                _selectedDateRangeConciliacao.endDate = end.format('YYYY-MM-DDT23:59:59.999Z');
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
            //localStorage['contasPagar[fornecedorId]'] = $("#pessoaId").val();
            //localStorage['contasPagar[fornecedorText]'] = $("#pessoaId").select2('data')[0]?.text;
            localStorage['contasPagar[empresaId]'] = $("#empresaFiltroId").val();
            localStorage['contasPagar[empresaText]'] = $("#empresaFiltroId").select2('data')[0]?.text;

            if (_$controleBancarioFilterForm.find('input.vencimento').data('daterangepicker')['chosenLabel']) {
                localStorage['contasPagar[dataVencimentoLabel]'] = _$controleBancarioFilterForm.find('input.vencimento').data('daterangepicker')['chosenLabel'];
            }

            localStorage['contasPagar[dataVencimento]'] = JSON.stringify({
                start: new Date(moment(_selectedDateRangeMovimento.startDate).startOf('day')),
                end: new Date(moment(_selectedDateRangeMovimento.endDate).endOf('day'))
            });
        };

        function getFiltersFromLocalStorage() {

            if (localStorage['contasPagar[dataVencimento]']) {
                let chosenLabel = localStorage['contasPagar[dataVencimentoLabel]'];
                let dateVal;
                if (chosenLabel && chosenLabel != 'undefined' && chosenLabel !== 'Intervalo personalizado') {
                    const datas = _$controleBancarioFilterForm.find('input.vencimento').data('ranges')[chosenLabel];

                    dateVal = datas[0].format('DD/MM/YYYY') + ' - ' + datas[1].format('DD/MM/YYYY')
                    _selectedDateRangeMovimento.startDate = datas[0].format('YYYY-MM-DDT00:00:00Z');
                    _selectedDateRangeMovimento.endDate = datas[1].format('YYYY-MM-DDT23:59:59.999Z');
                } else {
                    let date = JSON.parse(localStorage['contasPagar[dataVencimento]']);
                    dateVal = moment(date.start).format('DD/MM/YYYY') + ' - ' + moment(date.end).format('DD/MM/YYYY');
                    _selectedDateRangeMovimento.startDate = date.start;
                    _selectedDateRangeMovimento.endDate = date.end;
                }
                _$controleBancarioFilterForm.find('input.vencimento').val(dateVal).trigger('change');
            }

            if (localStorage['contasPagar[fornecedorId]'] && localStorage['contasPagar[fornecedorId]'] != 'null' &&
                localStorage['contasPagar[fornecedorText]'] && localStorage['contasPagar[fornecedorText]'] != 'undefined') {
                let optionId = localStorage['contasPagar[fornecedorId]'];
                let optionText = localStorage['contasPagar[fornecedorText]'];
                if ($('#pessoaId').find("option[value='" + optionId + "']").length) {
                    $('#pessoaId').val(optionId).trigger('change');
                } else {
                    // Create a DOM Option and pre-select by default
                    var newOption = new Option(optionText, optionId, true, true);
                    // Append it to the select
                    $('#pessoaId').append(newOption).trigger('change');
                }
            }

            if (localStorage['contasPagar[empresaId]'] && localStorage['contasPagar[empresaId]'] != 'null' &&
                localStorage['contasPagar[empresaText]'] && localStorage['contasPagar[empresaText]'] != 'undefined') {
                let optionId = localStorage['contasPagar[empresaId]'];
                let optionText = localStorage['contasPagar[empresaText]'];
                if ($('#empresaFiltroId').find("option[value='" + optionId + "']").length) {
                    $('#empresaFiltroId').val(optionId).trigger('change');
                } else {
                    // Create a DOM Option and pre-select by default
                    var newOption = new Option(optionText, optionId, true, true);
                    // Append it to the select
                    $('#empresaFiltroId').append(newOption).trigger('change');
                }
            }            
        };
    });
})();