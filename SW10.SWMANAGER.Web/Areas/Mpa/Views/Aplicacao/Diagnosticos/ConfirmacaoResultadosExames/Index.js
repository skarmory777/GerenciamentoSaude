(function () {

    $(function () {

        //remover isso

        $('.modal-dialog').css('width', '1800px');

        var _$coletasTable = $('#coletasTable');
        var _resultadoService = abp.services.app.resultado;
        var _$filterForm = $('#PreMovimentoFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Suprimentos.Estoque.PreMovimento.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Suprimentos.Estoque.PreMovimento.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Suprimentos.Estoque.PreMovimento.Delete')
        };


        var _ModalVisualizarExamePorColeta = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/ResultadoLaboratorio/ModalVisualizarExamePorRegistroColeta',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Diagnosticos/Laboratorio/_ModalVisualizacaoResultado.js',
            modalClass: 'ModalVisualizacaoResultado'
        });

        var _ModalListaExamesModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/ConferenciaResultadoExames/ExibirExames',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Diagnosticos/ConfirmacaoResultadosExames/ListaExames.js',
            modalClass: 'ListaExamesModal'
        });


        _$coletasTable.jtable({

            title: app.localize('Confirmacao'),
            paging: true,
            sorting: true,
            multiSorting: true,


            //rowUpdated: function (event, data) {
            //    if (data) {
            //        if (data.record.cor) {
            //            data.row[0].cells[0].setAttribute('bgcolor', data.record.cor);
            //            data.row[0].cells[0].setAttribute('color', data.record.cor);
            //        }
            //    }
            //},

            //rowInserted: function (event, data) {
            //   

            //    if (data) {
            //        if (data.record.cor) {
            //            data.row[0].cells[0].setAttribute('bgcolor', data.record.cor);
            //            data.row[0].cells[0].setAttribute('color', data.record.cor);
            //        }
            //    }
            //},



            actions: {
                listAction: {
                    method: _resultadoService.listarNaoConferido
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },


                //Status: {
                //    width: '4%',
                //    display: function (data) {
                //        return "  ";
                //    }
                //},

                actions: {
                    title: app.localize('Actions'),
                    width: '8%',
                    sorting: false,
                    display: function (data) {
                        var $span = $('<span></span>');

                        $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-binoculars"></i></button>')
                       .appendTo($span)
                       .click(function () {
                           _ModalVisualizarExamePorColeta.open({ coletaId: data.record.id });
                           // location.href = 'GestaoLaudos/CriarOuEditarModal/' + data.record.id
                       });

                        $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-list"></i></button>')
                      .appendTo($span)
                      .click(function () {
                          _ModalListaExamesModal.open({ coletaId: data.record.id });
                          // location.href = 'GestaoLaudos/CriarOuEditarModal/' + data.record.id
                      });


                        return $span;
                    }
                },



                Codigo: {
                    title: app.localize('N.I.C'),
                    width: '5%',
                    display: function (data) {
                        return data.record.codigo;
                    }
                },

                Paciente: {
                    title: app.localize('Paciente'),
                    width: '15%',
                    display: function (data) {
                        return data.record.paciente;
                    }
                },

                DataColeta: {
                    title: app.localize('DataColeta'),
                    width: '15%',
                    display: function (data) {
                        return moment(data.record.dataColeta).format('L');
                    }
                },
                MedicoSolicitante: {
                    title: app.localize('MedicoSolicitante'),
                    width: '15%',
                    display: function (data) {
                        return data.record.nomeMedicoSolicitante;
                    }
                }


            }

        });

        var _selectedDateRange = {
            startDate: moment().startOf('day'),
            endDate: moment().endOf('day')
        };


        function getColetas(reload) {
            if (reload) {
                _$coletasTable.jtable('reload');
            } else {
                _$coletasTable.jtable('load', {
                    // filtro: $('#PreMovimentoTableFilter').val(),

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

        getColetas();

        //   $('#EntradasTableFilter').focus();

        _$filterForm.find('input.date-range-picker').daterangepicker(
           $.extend(true, app.createDateRangePickerOptions(), _selectedDateRange),
           function (start, end, label) {
               _selectedDateRange.startDate = start.format('YYYY-MM-DDT00:00:00Z');
               _selectedDateRange.endDate = end.format('YYYY-MM-DDT23:59:59.999Z');
           });


        function exibirMovimentacaoParaConfirmacao(data) {
           
            switch (data.tipoOperacaoId) {
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
        //  selectSW('.selectTipoOperacoes', "/api/services/app/tipooperacao/ListarDropdown");






    });
})();