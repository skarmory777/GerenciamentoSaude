(function () {

    $(function () {

        //remover isso

        $('.modal-dialog').css('width', '1800px');

        var _$PreMovimentoTable = $('#PreMovimentoTable');
        var _preMovimentoService = abp.services.app.estoquePreMovimento;
        var _$filterForm = $('#PreMovimentoFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Suprimentos.Estoque.PreMovimento.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Suprimentos.Estoque.PreMovimento.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Suprimentos.Estoque.PreMovimento.Delete')
        };

        _$PreMovimentoTable.jtable({

            title: app.localize('Entrada'),
            paging: true,
            sorting: true,
            multiSorting: true,


            actions: {
                listAction: {
                    method: _preMovimentoService.listar
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '6%',
                    sorting: false,
                    display: function (data) {
                        var $span = $('<span></span>');
                        if (_permissions.edit) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    //_createOrEditModal.open({ id: data.record.id });
                                    location.href = 'PreMovimentos/CriarOuEditarModal/' + data.record.id
                                });
                        }

                        if (_permissions.delete && data.record.preMovimentoEstadoId != 2) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    deletePreMovimentos(data.record);
                                });
                        }

                        return $span;
                    }
                },


                preMovimentoEstadoId: {
                    title: app.localize('Confirmada'),
                    width: '7%',
                    display: function (data) {
                        if (data.record.preMovimentoEstadoId == 2) {
                            return '<span class="label label-success">' + app.localize('Yes') + '</span>';
                        } else {
                            return '<span class="label label-default">' + app.localize('No') + '</span>';
                        }
                    }
                },

                Empresa: {
                    title: app.localize('Empresa'),
                    width: '10%',
                    display: function (data) {
                        return data.record.empresa;
                    }
                },

                Estoque: {
                    title: app.localize('Estoque'),
                    width: '15%',
                    display: function (data) {
                        return data.record.estoque;
                    }
                },

                dataEmissaoSaida: {
                    title: app.localize('Emissao'),
                    width: '7%',
                    display: function (data) {
                        return moment(data.record.dataEmissaoSaida).format('L');
                    }
                },
                Documento: {
                    title: app.localize('Documento'),
                    width: '10%',
                    display: function (data) {
                        return data.record.documento;
                    }
                },

                Fornecedor: {
                    title: app.localize('Fornecedor'),
                    width: '15%',
                    display: function (data) {
                        return data.record.fornecedor;
                    }
                },

                TipoMovimento: {
                    title: app.localize('TipoMovimento'),
                    width: '10%',
                    display: function (data) {
                        return data.record.tipoMovimento;
                    }
                },


                Valor: {
                    title: app.localize('Valor'),
                    width: '10%',
                    display: function (data) {
                        return posicionarDireita(data.record.valor.toLocaleString("pt-BR", { style: "currency", currency: "BRL" }));
                    }
                }
                ,
                Usuario: {
                    title: app.localize('Usuario'),
                    width: '20%',
                    display: function (data) {
                        return data.record.usuario;
                    }
                }
            }

        });

        var _selectedDateRange = {
            startDate: moment().startOf('day'),
            endDate: moment().endOf('day')
        };


        function getPreMovimentos(reload) {
            if (reload) {
                _$PreMovimentoTable.jtable('reload');
            } else {
                _$PreMovimentoTable.jtable('load', {
                    filtro: $('#PreMovimentoTableFilter').val(),
                    peridoDe: _selectedDateRange.startDate,//  $('#PeridoDe').val(),
                    peridoAte: _selectedDateRange.endDate, //$('#PeridoAte').val()
                    fornecedorId: $('#FornecedorId').val(),
                    tipoMovimentoId: $('#EstTipoMovimentoId').val(),
                    isEntrada: true
                });
            }
        }

        function deletePreMovimentos(PreMovimento) {

            abp.message.confirm(
                app.localize('DeleteWarning', PreMovimento.documento),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _preMovimentoService.excluir(PreMovimento.id)
                            .done(function () {
                                getPreMovimentos();
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
            $('#AdvacedAuditFiltersArea').slideDown();
        });

        $('#HideAdvancedFiltersSpan').click(function () {
            $('#HideAdvancedFiltersSpan').hide();
            $('#ShowAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideUp();
        });

        $('#CreateNewPreMovimentoButton').click(function () {
            location.href = 'PreMovimentos/CriarOuEditarModal/';
            //_createOrEditModal.open();
        });

        $('#ExportarPreMovimentoParaExcelButton').click(function () {
            _preMovimentoService
                .listarParaExcel({
                    
                    filtro: $('#PreMovimentoTableFilter').val(),
                    peridoDe: _selectedDateRange.startDate,
                    peridoAte: _selectedDateRange.endDate, 
                    fornecedorId: $('#FornecedorId').val(),
                    isEntrada: true,
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#RefreshAtendimentosButton').click(function (e) {

            e.preventDefault();
            getPreMovimentos();

        });

        //abp.event.on('app.CriarOuEditarEntradaModalSaved', function () {
        //    getPreMovimento(true);
        //});

        getPreMovimentos();

        $('#EntradasTableFilter').focus();

        debugger;


        _$filterForm.find('input.date-range-picker').daterangepicker(
           $.extend(true, app.createDateRangePickerOptions(), _selectedDateRange),
           function (start, end, label) {
               _selectedDateRange.startDate = start.format('YYYY-MM-DDT00:00:00Z');
               _selectedDateRange.endDate = end.format('YYYY-MM-DDT23:59:59.999Z');
               getPreMovimentos();
            });

        $('.select2').each(function () {
            $(this).on("change",
                function (e) {
                    getPreMovimentos();
                });
        });

        //function pad(value, length, caracter) {
        //    return (value.toString().length < length) ? pad(caracter + value, length) : value;
        //}

        selectSW('.selectForncedor', "/api/services/app/fornecedor/ListarDropdownSisFornecedor");
        selectSW('.selectTipoMovimento', "/api/services/app/tipomovimento/ListarDropdownEntrada");
        
    });
})();