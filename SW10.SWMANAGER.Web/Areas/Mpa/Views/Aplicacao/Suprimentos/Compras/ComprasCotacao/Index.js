(function () {
    $(function () {

        /*  Permissões ↓
        ----------------------------------------------------------------------------------------------------------------- */
        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Suprimentos.CompraCotacao.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Suprimentos.CompraCotacao.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Suprimentos.CompraCotacao.Delete')
        };


        /*  Servicos ↓
        ----------------------------------------------------------------------------------------------------------------- */
        var _$requisicoesComprasTable = $('#requisicoesComprasTable');
        var _requisicoesCompraService = abp.services.app.compraRequisicao;
        var _$requisicoesCompraFilterForm = $('#contasPagarFilterForm');


        /*  Vars Globais ↓
        ----------------------------------------------------------------------------------------------------------------- */


        /*  Sets iniciais ↓
        ----------------------------------------------------------------------------------------------------------------- */
        $('#AdvacedContasMedicasFiltersArea').swPiqueEsconde('#ShowAdvancedFiltersSpan', '#HideAdvancedFiltersSpan');

        // Date range filtro
        var _selectedDateRangeLocal = {
            startDate: moment().startOf('month'),
            endDate: moment().endOf('month')
        };

        $('.date-range-picker').daterangepicker(
            $.extend(true, app.createDateRangePickerOptions(), _selectedDateRangeLocal),
            function (start, end, label) {
                debugger
                _selectedDateRangeLocal.startDate = start.format('YYYY-MM-DDT00:00:00Z');
                _selectedDateRangeLocal.endDate = end.format('YYYY-MM-DDT23:59:59.999Z');
            });
        // Fim - date range filtro

        $('#enviarCotacaoBionexo').click(function (e) {
            e.preventDefault();
            $(this).buttonBusy(true);

            var cotacoesSelecionadas = _$requisicoesComprasTable.jtable('selectedRows');

            var listaId = [];

            cotacoesSelecionadas.each(function () {
                var registro = $(this).data('record');

                listaId.push(registro.id);
            });

            if (listaId.length > 0) {
                _requisicoesCompraService.enviarCotacaoBionexo(listaId)
                    .done(function () {
                        abp.notify.info(app.localize('SavedSuccessfully'));
                        $(this).buttonBusy(false);

                        getRegistros();
                    })
                    .always(function () {
                        $(this).buttonBusy(false);
                    });
            }
        });

        _$requisicoesComprasTable.jtable({

            title: app.localize('Cotacoes'),
            paging: true,
            sorting: true,
            multiSorting: true,
            selecting: true,
            selectingCheckboxes: true,
            multiselect: true,

            actions: {
                listAction: {
                    method: _requisicoesCompraService.listarCotacao
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '10%',
                    listClass: 'text-center',
                    sorting: false,
                    display: function (data) {
                        var $span = $('<span></span>');
                        $('<button class="btn btn-default btn-xs" title="' + app.localize('Report') + '"><i class="fa fa-print"></i></button>')
                            .appendTo($span)
                            .click(function () {
                                gerarRelatorio(data.record.id)
                            });

                        if ((_permissions.edit) && (data.record.isEncerrada != true)) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    location.href = 'ComprasCotacao/CriarOuEditarModal/' + data.record.id
                                });
                        }

                        return $span;
                    }
                },

                Empresa: {
                    title: app.localize('Empresa'),
                    width: '20%',
                    display: function (data) {
                        if (data) {
                            return data.record.empresa;
                        }
                    }
                },

                Id: {
                    title: app.localize('NumeroRequisicao'),
                    width: '10%',
                    listClass: 'text-center',
                    display: function (data) {
                        if (data) {
                            return data.record.codigo;
                        }
                    }
                },

                DataRequisicao: {
                    title: app.localize('DataRequisicao'),
                    width: '10%',
                    listClass: 'text-center',
                    display: function (data) {
                        return moment(data.record.dataRequisicao).format('L');
                    }
                },

                DataInicioCotacao: {
                    title: app.localize('DataInicioCotacao'),
                    width: '10%',
                    listClass: 'text-center',
                    display: function (data) {
                        return data.record.dataInicioCotacao !== null ? moment(data.record.dataInicioCotacao).format('L') : '';
                    }
                },

                DataEnvioBionexo: {
                    title: app.localize('DataEnvioBionexo'),
                    width: '10%',
                    listClass: 'text-center',
                    display: function (data) {
                        return data.record.dataEnvioBionexo !== null ? moment(data.record.dataEnvioBionexo).format('L') : '';
                    }
                },

                DataLimiteEntrega: {
                    title: app.localize('DataLimiteEntregaRequisicao'),
                    width: '10%',
                    display: function (data) {
                        return moment(data.record.dataLimiteEntrega).format('L');
                    }
                },

                UnidadeOrganizacional: {
                    title: app.localize('Setor'),
                    width: '20%',
                    display: function (data) {
                        if (data) {
                            return data.record.unidadeOrganizacional;
                        }
                    }
                },
                IsUrgente: {
                    title: app.localize('IsUrgente'),
                    width: '10%',
                    listClass: 'text-center',
                    display: function (data) {
                        if (data.record.isUrgente) {
                            return '<div style="text-align:center;vertical-align:middle"> <span class="label bg-red-pink" style="text-align:center;">' + app.localize('Yes') + '</span> </div>'
                        } else {
                            return '<div style="text-align:center;vertical-align:middle"> <span class="label bg-default" style="text-align:center; color:dimgrey">' + app.localize('No') + '</span> </div>'
                        }
                    }
                },
                AprovacaoStatus: {
                    title: app.localize('StatusAprovacao'),
                    width: '15%',
                    display: function (data) {
                        if (data) {
                            return data.record.aprovacaoStatus;
                        }
                    }
                },
            }
        });

        var filtroJTableLocal = {
            empresaId: $('#comboEmpresa option:selected').val(),
            estoqueId: $('#comboEstoque option:selected').val(),
            motivoPedidoId: $('#comboMotivoPedido option:selected').val(),
            isUrgente: $('#checkUrgente').val(),
            codigo: $('#codigo').val(),
            statusRequisicao: $('#comboStatusRequisicao').val(),
            statusAprovacao: $('#comboStatusAprovacao').val(),
            StartDate: _selectedDateRangeLocal.startDate,
            EndDate: _selectedDateRangeLocal.endDate,

            get() {
                this.empresaId = $('#comboEmpresa option:selected').val();
                this.estoqueId = $('#comboEstoque option:selected').val();
                this.motivoPedidoId = $('#comboMotivoPedido option:selected').val();
                this.isUrgente = $('#checkUrgente').val(),
                this.codigo = $('#codigo').val();
                this.statusRequisicao = $('#comboStatusRequisicao').val();
                this.statusAprovacao = $('#comboStatusAprovacao').val();
                this.StartDate = _selectedDateRangeLocal.startDate;
                this.EndDate = _selectedDateRangeLocal.endDate;
                return this;
            }
        };

        function gerarRelatorio(compraCotacaoId) {
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
                compraCotacaoId: compraCotacaoId
            };

            $.removeCookie("XSRF-TOKEN");

            const parameters = $.param(data);

            printJS({
                printable: `/Mpa/ComprasCotacao/GerarRelatorio?${parameters}`,
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
        }

        function getRegistros(reload) {  
            if (reload) {
                _$requisicoesComprasTable.jtable('reload');
            } else {
                _$requisicoesComprasTable.jtable('load', filtroJTableLocal.get());
            }
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

        $('#checkUrgente').change(function () {
            if ($(this).is(':checked')) {
                $(this).val(true);
            } else {
                $(this).val("");
            };
        });

        $('#refreshButton').click(function (e) {
            try {
                e.preventDefault();

                $(this).buttonBusy(true);

                getRegistros();
            }
            finally {

                $(this).buttonBusy(false);
            }
        });

        abp.event.on('app.CriarOuEditarCompraRequisicaoSaved', function () {
            getRegistros(true);
        });

        $('#tableFilter').focus();

        getRegistros();

    });
})();