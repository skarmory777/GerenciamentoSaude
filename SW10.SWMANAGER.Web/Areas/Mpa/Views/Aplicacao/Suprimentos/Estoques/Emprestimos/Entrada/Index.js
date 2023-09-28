(function () {

    $(function () {

        //remover isso

        $('.modal-dialog').css('width', '1800px');

        var _$PreMovimentoTable = $('#PreMovimentoTable');
        var _preMovimentoService = abp.services.app.estoquePreMovimento;
        var _$filterForm = $('#PreMovimentoFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Suprimentos.Estoque.Emprestimo.Entrada.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Suprimentos.Estoque.Emprestimo.Entrada.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Suprimentos.Estoque.Emprestimo.Entrada.Delete')
        };

        _$PreMovimentoTable.jtable({

            title: app.localize('Entrada'),
            paging: true,
            sorting: true,
            multiSorting: true,


            actions: {
                listAction: {
                    method: _preMovimentoService.listarEmprestimosEntrada
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
                                    //_createOrEditModal.open({ id: data.record.id });
                                    location.href = '/Mpa/Emprestimos/CriarOuEditarEntradaModal/' + data.record.id
                                });
                        }

                        // Somente é possível remover quando é "Pendente"
                        if (_permissions.delete && data.record.preMovimentoEstadoId === 4) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    deletePreMovimentos(data.record);
                                });
                        }

                        if (data.record.preMovimentoEstadoId == 2 || data.record.preMovimentoEstadoId == 5) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Devolver') + '"><i class="fa fa-redo"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    devolucaoPreMovimentos(data.record);
                                });
                        }

                        return $span;
                    }
                },
                preMovimentoEstadoId: {
                    title: app.localize('Status'),
                    width: '6%',
                    display: function (data) {
                        switch (data.record.preMovimentoEstadoId) {
                            case 2: {
                                return '<span class="label label-info">' + app.localize('Emprestado') + '</span>';
                            }
                            case 5: {
                                return '<span class="label label-info">' + app.localize('Devolvido Parcialmente') + '</span>';
                            }
                            case 6: {
                                return '<span class="label label-success">' + app.localize('Devolvido') + '</span>';
                            }
                        }
                    }
                },

                //SaidaConfirmada: {
                //    title: app.localize('Confirmada'),
                //    width: '7%',
                //    display: function (data) {
                //        if (data.record.preMovimentoEstadoId == 2) {
                //            return '<span class="label label-success">' + app.localize('Yes') + '</span>';
                //        } else {
                //            return '<span class="label label-default">' + app.localize('No') + '</span>';
                //        }
                //    }
                //},
                Empresa: {
                    title: app.localize('Empresa'),
                    width: '15%',
                    display: function (data) {
                        return data.record.empresa;
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


                Usuario: {
                    title: app.localize('Usuario'),
                    width: '15%',
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
                    fornecedorId: $('#FornecedorId').val(),
                    peridoDe: _selectedDateRange.startDate,//  $('#PeridoDe').val(),
                    peridoAte: _selectedDateRange.endDate, //$('#PeridoAte').val()
                    tipoMovimentoId: $('#EstTipoMovimentoId').val(),
                    isEntrada: false
                });
            }
        }
        function devolucaoPreMovimentos(PreMovimento) {
            if (PreMovimento != null) {
                location.href = `/Mpa/Emprestimos/CriarOuEditarDevolucaoModal/${PreMovimento.id}`;
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
            location.href = '/Mpa/Emprestimos/CriarOuEditarEntradaModal/';
            //_createOrEditModal.open();
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

        _$filterForm.find('input.date-range-picker').daterangepicker(
           $.extend(true, app.createDateRangePickerOptions(), _selectedDateRange),
           function (start, end, label) {
               _selectedDateRange.startDate = start.format('YYYY-MM-DDT00:00:00Z');
               _selectedDateRange.endDate = end.format('YYYY-MM-DDT23:59:59.999Z');

               getPreMovimentos();
           });


        selectSW('.selectTipoSaida', "/api/services/app/tipomovimento/ListarDropdownSaida");
    });
})();