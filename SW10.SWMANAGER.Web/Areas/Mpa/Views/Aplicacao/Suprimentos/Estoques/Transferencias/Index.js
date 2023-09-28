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

            title: app.localize('Transferencia'),
            paging: true,
            sorting: true,
            multiSorting: true,


            actions: {
                listAction: {
                    method: _preMovimentoService.listarTransferencia
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
                                    location.href = 'Transferencias/CriarOuEditarModal/' + data.record.id
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

                TransferenciaConfirmada: {
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

                Documento: {
                    title: app.localize('Documento'),
                    width: '15%',
                    display: function (data) {
                        return data.record.documento;
                    }
                },

                EstoqueSaida: {
                    title: app.localize('EstoqueSaida'),
                    width: '15%',
                    display: function (data) {
                        return data.record.estoqueSaida;
                    }
                },

                EstoqueEntrada: {
                    title: app.localize('EstoqueEntrada'),
                    width: '15%',
                    display: function (data) {
                        return data.record.estoqueEntrada;
                    }
                },

               
                Movimento: {
                    title: app.localize('Movimento'),
                    width: '15%',
                    display: function (data) {
                        return moment(data.record.movimento).format('L');
                    }
                },
                //Documento: {
                //    title: app.localize('Documento'),
                //    width: '15%',
                //    display: function (data) {
                //        return data.record.documento;
                //    }
                //},

                
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
                   peridoDe: _selectedDateRange.startDate,//  $('#PeridoDe').val(),
                    peridoAte: _selectedDateRange.endDate, //$('#PeridoAte').val()
                    isEntrada: false
                });
            }
        }

        function deletePreMovimentos(transferencia) {

            abp.message.confirm(
                app.localize('DeleteWarning', transferencia.documento),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _preMovimentoService.excluirTransferencia(transferencia.id)
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
            location.href = 'Transferencias/CriarOuEditarModal/';
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
           });




       



    });
})();