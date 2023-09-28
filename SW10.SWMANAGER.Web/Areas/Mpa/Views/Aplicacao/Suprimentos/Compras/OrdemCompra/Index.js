(function () {
    $(function () {

        /*  Permissões ↓
        ----------------------------------------------------------------------------------------------------------------- */
        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Suprimentos.OrdemCompra.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Suprimentos.OrdemCompra.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Suprimentos.OrdemCompra.Delete')
        };

        /*  Servicos ↓
        ----------------------------------------------------------------------------------------------------------------- */    
        var _ordemCompraService = abp.services.app.ordemCompra;

            /*  Vars Globais ↓
        ----------------------------------------------------------------------------------------------------------------- */
        var _$ordemCompraTable = $('#ordemCompraTable');

        /*  Sets iniciais ↓
        ----------------------------------------------------------------------------------------------------------------- */
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

        $('#createNew').click(function (e) {
            e.preventDefault();
            location.href = '/OrdemCompra/CriarOuEditarModal/';
        });

        _$ordemCompraTable.jtable({

            title: app.localize('OrdensCompra'),
            paging: true,
            sorting: true,
            multiSorting: true,
            multiselect: true,

            actions: {
                listAction: {
                    method: _ordemCompraService.listar
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

                        if ((_permissions.edit)) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    location.href = 'OrdemCompra/CriarOuEditarModal/' + data.record.id
                                });
                        }

                        if (_permissions.delete) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    deleteOrdemCompra(data.record);
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
                    title: app.localize('NumeroOrdemCompra'),
                    width: '10%',
                    listClass: 'text-center',
                    display: function (data) {
                        if (data) {
                            return data.record.codigo;
                        }
                    }
                },

                DataOrdemCompra: {
                    title: app.localize('DataOrdemCompra'),
                    width: '10%',
                    listClass: 'text-center',
                    display: function (data) {
                        return data.record.dataOrdemCompra !== null ? moment(data.record.dataOrdemCompra).format('L') : '';
                    }
                },

                DataPrevistaEntrega: {
                    title: app.localize('DataPrevistaEntrega'),
                    width: '10%',
                    listClass: 'text-center',
                    display: function (data) {
                        debugger;

                        return data.record.dataPrevistaEntrega !== null ? moment(data.record.dataPrevistaEntrega).format('L') : '';
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

                OrdemCompraStatus: {
                    title: app.localize('Status'),
                    width: '15%',
                    display: function (data) {
                        if (data) {
                            return data.record.ordemCompraStatus;
                        }
                    }
                },
            }
        });

        var filtroJTableLocal = {
            EmpresaId: $('#comboEmpresa option:selected').val(),
            UnidadeOrganizacionalId: $('#UnidadeOrganizacionalId option:selected').val(),
            Codigo: $('#codigo').val(),
            OrdemCompraStatusId: $('#comboOrdemCompraStatus').val(),
            StartDate: _selectedDateRangeLocal.startDate,
            EndDate: _selectedDateRangeLocal.endDate,

            get() {
                this.EmpresaId = $('#comboEmpresa option:selected').val();
                this.Codigo = $('#codigo').val();
                this.OrdemCompraStatusId = $('#comboOrdemCompraStatus').val();
                this.StartDate = _selectedDateRangeLocal.startDate;
                this.EndDate = _selectedDateRangeLocal.endDate;
                this.UnidadeOrganizacionalId = $('#UnidadeOrganizacionalId option:selected').val();
                return this;
            }
        };

        function deleteOrdemCompra(OrdemCompra) {
            abp.message.confirm(
                app.localize('DeleteWarning', OrdemCompra.codigo),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _ordemCompraService.excluir(OrdemCompra.id)
                            .done(function () {
                                getRegistros();
                                abp.notify.success(app.localize('SuccessfullyDeleted'));
                            });
                    }
                }
            );
        }

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
                printable: `/Mpa/OrdemCompra/GerarRelatorio?${parameters}`,
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
                _$ordemCompraTable.jtable('reload');
            } else {
                _$ordemCompraTable.jtable('load', filtroJTableLocal.get());
            }
        }

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

        abp.event.on('app.CriarOuEditarCompraOrdemCompraSaved', function () {
            getRegistros(true);
        });

        $('#tableFilter').focus();

        getRegistros();

    });
})();