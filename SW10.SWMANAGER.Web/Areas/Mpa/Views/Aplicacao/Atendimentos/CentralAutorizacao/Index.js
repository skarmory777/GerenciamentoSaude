(function () {

    $(function () {

        //remover isso

        $('.modal-dialog').css('width', '1800px');

        var _$AutorizacaoTable = $('#AutorizacaoTable');
        var _autorizacaoProcedimentoService = abp.services.app.autorizacaoProcedimento;
        var _$filterForm = $('#CentralAutorizacaoForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Suprimentos.Estoque.PreMovimento.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Suprimentos.Estoque.PreMovimento.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Suprimentos.Estoque.PreMovimento.Delete')
        };


        var _selectedDateRange = {
            startDate: moment().startOf('day'),
            endDate: moment().endOf('day')
        };


        var _createOrEditAutorizacaoProcedimentoItemModal = new app.ModalManager({
            viewUrl: abp.appPath + 'MPA/CentralAutorizacoes/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Atendimentos/CentralAutorizacao/_CriarOuEditarModal.js'
           
        });


        _$AutorizacaoTable.jtable({

            title: app.localize('Autorizacao'),
            paging: true,
            sorting: true,
            multiSorting: true,


            actions: {
                listAction: {
                    method: _autorizacaoProcedimentoService.listarAutorizacao
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
                        if (_permissions.edit) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    exibirAutorizacaoProcedimento(data.record);
                                });
                        }
                        return $span;
                    }
                },

                NumeroGuia: {
                    title: app.localize('Guia'),
                    width: '10%',
                    display: function (data) {
                        return data.record.numeroGuia;
                    }
                },



                Status: {
                    title: app.localize('Status'),
                    width: '5%',
                    display: function (data) {
                        return data.record.status;
                    }
                },


                Codigo: {
                    title: app.localize('CodigoAutorizacao'),
                    width: '5%',
                    display: function (data) {
                        return data.record.codigoAutorizacao;
                    }
                },

                IsOrtese: {
                    title: app.localize('Ortese'),
                    width: '5%',
                    display: function (data) {

                        if (data.record.isOrtese) {
                            return '<span class="label label-success">' + app.localize('Yes') + '</span>';
                        } else {
                            return '<span class="label label-default">' + app.localize('No') + '</span>';
                        }
                    }
                },

                DataAutorizacao: {
                    title: app.localize('Autorizacao'),
                    width: '7%',
                    display: function (data) {
                        if (data.record.dataAutorizacao) {
                            return moment(data.record.dataAutorizacao).format('L');
                        }
                    }
                },
                Paciente: {
                    title: app.localize('Paciente'),
                    width: '15%',
                    display: function (data) {
                        return data.record.paciente;
                    }
                },

                Convenio: {
                    title: app.localize('Convenio'),
                    width: '10%',
                    display: function (data) {
                        return data.record.convenio;
                    }
                },

                Item: {
                    title: app.localize('Item'),
                    width: '20%',
                    display: function (data) {
                        return data.record.faturamentoItem;
                    }
                },

                Medico: {
                    title: app.localize('Medico'),
                    width: '10%',
                    display: function (data) {
                        return data.record.medico;
                    }
                },

                Especialidade: {
                    title: app.localize('Especialidade'),
                    width: '20%',
                    display: function (data) {
                        return data.record.especialidade;
                    }
                },

                GrupoItem: {
                    title: app.localize('GrupoItem'),
                    width: '10%',
                    display: function (data) {
                        return data.record.grupoItem;
                    }
                },

                //Usuario: {
                //    title: app.localize('Usuario'),
                //    width: '20%',
                //    display: function (data) {
                //        return data.record.usuario;
                //    }
                //}
            }

        });

        function exibirAutorizacaoProcedimento(data) {
            location.href = 'CentralAutorizacoes/CriarOuEditarModal?id=' + data.id + '&itemId=' + data.itemId;
        }

        function getAutorizacoes(reload) {
            if (reload) {
                _$AutorizacaoTable.jtable('reload');
            } else {
                _$AutorizacaoTable.jtable('load', {
                    filtro: $('#filtro').val(),
                    peridoDe: _selectedDateRange.startDate,//  $('#PeridoDe').val(),
                    peridoAte: _selectedDateRange.endDate, //$('#PeridoAte').val()
                    convenioId: $('#convenioId').val(),
                    FaturamentoItemId: $('#faturamentoItemId').val()
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

        //$('#ShowAdvancedFiltersSpan').click(function () {
        //    $('#ShowAdvancedFiltersSpan').hide();
        //    $('#HideAdvancedFiltersSpan').show();
        //    $('#AdvacedAuditFiltersArea').slideDown();
        //});

        //$('#HideAdvancedFiltersSpan').click(function () {
        //    $('#HideAdvancedFiltersSpan').hide();
        //    $('#ShowAdvancedFiltersSpan').show();
        //    $('#AdvacedAuditFiltersArea').slideUp();
        //});

        $('#CreateNewPreMovimentoButton').click(function () {
            location.href = 'CentralAutorizacoes/CriarOuEditarModal/';
            //_createOrEditModal.open();
        });

        //$('#ExportarPreMovimentoParaExcelButton').click(function () {
        //    _preMovimentoService
        //        .listarParaExcel({

        //            filtro: $('#PreMovimentoTableFilter').val(),
        //            peridoDe: _selectedDateRange.startDate,
        //            peridoAte: _selectedDateRange.endDate, 
        //            fornecedorId: $('#FornecedorId').val(),
        //            isEntrada: true,
        //            maxResultCount: $('span.jtable-page-size-change select').val()
        //        })
        //        .done(function (result) {
        //            app.downloadTempFile(result);
        //        });
        //});

        $('#RefreshAtendimentosButton').click(function (e) {

            e.preventDefault();
            getAutorizacoes();

        });

        //abp.event.on('app.CriarOuEditarEntradaModalSaved', function () {
        //    getPreMovimento(true);
        //});

        getAutorizacoes();

        _$filterForm.find('input.date-range-picker').daterangepicker(
           $.extend(true, app.createDateRangePickerOptions(), _selectedDateRange),
           function (start, end, label) {
               console.log(start, end, label);
               _selectedDateRange.startDate = start.format('YYYY-MM-DDT00:00:00Z');
               _selectedDateRange.endDate = end.format('YYYY-MM-DDT23:59:59.999Z');
           });






        selectSW('.selectConvenio', "/api/services/app/Convenio/ListarDropdown");
        selectSW('.selecFaturamentoItem', "/api/services/app/FaturamentoItem/ListarFatItemDropdown");


    });
})();