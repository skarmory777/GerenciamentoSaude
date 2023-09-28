(function () {
    $(function () {

        /*  Permissões ↓
        ----------------------------------------------------------------------------------------------------------------- */
        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Suprimentos.CompraAprovacao.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Suprimentos.CompraAprovacao.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Suprimentos.CompraAprovacao.Delete')
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
        $("#comboStatusRequisicao").val('2');

        // Date range filtro
        var _selectedDateRangeLocal = {
            startDate: moment().startOf('month'),
            endDate: moment().endOf('month')
        };

        $('.date-range-picker').daterangepicker(
            $.extend(true, app.createDateRangePickerOptions(), _selectedDateRangeLocal),
            function (start, end, label) {
                _selectedDateRangeLocal.startDate = start.format('YYYY-MM-DDT00:00:00Z');
                _selectedDateRangeLocal.endDate = end.format('YYYY-MM-DDT23:59:59.999Z');
            });
        // Fim - date range filtro

        $('#createNew').click(function (e) {
            e.preventDefault();
            location.href = 'ComprasRequisicao/CriarOuEditarModal/';
        });

        _$requisicoesComprasTable.jtable({

            title: app.localize('RequisicoesCompras'),
            paging: true,
            sorting: true,
            multiSorting: true,
            actions: {
                listAction: {
                    method: _requisicoesCompraService.listar
                }
            },
            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '5%',
                    listClass: 'text-center',
                    sorting: false,
                    display: function (data) {
                        var $span = $('<span></span>');
                        if ((_permissions.edit) && (data.record.isEncerrada != true)) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('AprovacaoCompra') + '"><i class="fa fa-edit"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    location.href = 'ComprasAprovacao/CriarOuEditarModal/' + data.record.id
                                });
                        }

                        if ((_permissions.edit) && (data.record.isEncerrada === true)) {
                            $('<button class="btn btn red btn-xs" title="' + app.localize('VoltarRequisicaoStatusInicial') + '"><i class="fa fa-undo"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    abp.message.confirm(
                                        app.localize('VoltarRequisicaoStatusInicial'),
                                        function (isConfirmed) {
                                            if (isConfirmed) {
                                                voltarRequisicaoStatusInicial(data.record);
                                            }
                                        });
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

                DataLimiteEntrega: {
                    title: app.localize('DataLimiteEntregaRequisicao'),
                    width: '10%',
                    listClass: 'text-center',
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

                Estoque: {
                    title: app.localize('Estoque'),
                    width: '20%',
                    display: function (data) {
                        if (data) {
                            return data.record.estoque;
                        }
                    }
                },

                MotivoPedido: {
                    title: app.localize('MotivoPedido'),
                    width: '15%',
                    display: function (data) {
                        if (data) {
                            return data.record.motivoPedido;
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
                AprovacaoRequisicao: {
                    title: app.localize('StatusRequisicao'),
                    width: '15%',
                    display: function (data) {
                        if (data) {
                            return data.record.aprovacaoRequisicao;
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
            unidadeOrganizacionalId: $('#UnidadeOrganizacionalId option:selected').val(),
            estoqueId: $('#comboEstoque option:selected').val(),
            motivoPedidoId: $('#comboMotivoPedido option:selected').val(),
            isUrgente: $('#checkUrgente').val(),
            codigo: $('#codigo').val(),
            statusRequisicao: $('#comboStatusRequisicao').val(),
            statusAprovacao: $('#comboStatusAprovacao').val(),
            aprovacaoStatusId: $('#comboAprovacaoStatus option:selected').val(),
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

        function getRegistros(reload) {
           
            if (reload) {
                _$requisicoesComprasTable.jtable('reload');
            } else {
                _$requisicoesComprasTable.jtable('load', filtroJTableLocal.get());
            }
        }

        function voltarRequisicaoStatusInicial(requisicao) {
            _requisicoesCompraService.voltarRequisicaoStatusInicial(requisicao.id)
                .done(function () {
                    getRegistros(true);
                    abp.notify.info(app.localize('SavedSuccessfully'));
                })
                .always(function () {
                });
        }

        function deleteRegistro(record) {

            abp.message.confirm(
                app.localize('DeleteWarning', record.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _$requisicoesComprasTable.excluir(record)
                            .done(function () {
                                getRegistros(true);
                                abp.notify.success(app.localize('SuccessfullyDeleted'));
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

        $('#checkUrgente').change(function () {
            //            $(this).val($(this).is(':checked'));

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