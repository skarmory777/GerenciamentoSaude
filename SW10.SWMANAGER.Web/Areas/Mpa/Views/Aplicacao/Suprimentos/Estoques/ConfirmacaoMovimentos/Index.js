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

            title: app.localize('Confirmacao'),
            paging: true,
            sorting: true,
            multiSorting: true,


            actions: {
                listAction: {
                    method: _preMovimentoService.listarMovimentosPendente
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
                                    debugger;
                                    //_createOrEditModal.open({ id: data.record.id });
                                    // location.href = 'Saidas/CriarOuEditarModal/' + data.record.id
                                    exibirMovimentacaoParaConfirmacao(data.record);
                                });
                        }
                        return $span;
                    }
                },

               TipoMovimento: {
                   title: app.localize('TipoMovimento'),
                    width: '15%',
                    display: function (data) {
                        return data.record.tipoMovimento;
                    }
                },


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
                        return moment(data.record.dataEmissaoSaida).format('L');
                    }
                },
                Documento: {
                    title: app.localize('Documento'),
                    width: '15%',
                    display: function (data) {
                        return data.record.documento;
                    }
                },
                Valor: {
                    title: app.localize('Valor'),
                    width: '15%',
                    display: function (data) {
                        if (data.record.valorDocumento != null && data.record.valorDocumento != 0) {
                            return posicionarDireita(data.record.valorDocumento.toLocaleString("pt-BR", { style: "currency", currency: "BRL" }));//data.record.valorDocumento;

                            
                        }
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
                    tipoMovimentoId: $('#TipoMovimentoId').val(),
                    peridoDe: _selectedDateRange.startDate,//  $('#PeridoDe').val(),
                    peridoAte: _selectedDateRange.endDate, //$('#PeridoAte').val()
                    tipoOperacaoId: $('#EstTipoOperacaoId').val(),

                    isEntrada: false
                });
            }
        }

        function deletePreMovimentos(PreMovimento) {

            abp.message.confirm(
                app.localize('DeleteWarning', PreMovimento.documento),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _preMovimentoService.excluir(PreMovimento)
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
            location.href = 'Saidas/CriarOuEditarModal/';
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


        function exibirMovimentacaoParaConfirmacao(data)
        {
           
            switch (data.tipoOperacaoId)
            {
                case 1:
                    location.href = 'ConfirmacaoMovimentos/ConfirmarEntradaModal/' + data.id;
                    break;

                case 2:
                    location.href = 'ConfirmacaoMovimentos/TransferenciaModal/' + data.id;
                    break;

                case 3:
                    location.href = 'ConfirmacaoMovimentos/SaidaModal/' + data.id;
                    break;

                case 4:
                    location.href = 'ConfirmacaoMovimentos/DevolucaoModal/' + data.id;
                    break;
            }


        }
        selectSW('.selectTipoOperacoes', "/api/services/app/tipooperacao/ListarDropdown");


       



    });
})();