(function () {
    app.modals.ListaExamesModal = function () {

        var _modalManager;

        this.init = function (modalManager) {
            _modalManager = modalManager;
           

        };

        //remover isso

        $('.modal-dialog').css('width', '900px');

        var _$examesTable = $('#examesTable');
        var _$historicoTable = $('#historicoTable');

        
        var _resultadoExameService = abp.services.app.resultadoExame;
        var _resultadoLaudoService = abp.services.app.resultadoLaudo;

        var _$filterForm = $('#PreMovimentoFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Suprimentos.Estoque.PreMovimento.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Suprimentos.Estoque.PreMovimento.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Suprimentos.Estoque.PreMovimento.Delete')
        };


        var _ModalVisualizarExamePorColeta = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/ConferenciaResultadoExames/ModalVisualizarExame',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Diagnosticos/Laboratorio/_ModalVisualizacaoResultado.js',
            modalClass: 'ModalVisualizacaoResultado'
        });

        var list = [];

        _$examesTable.jtable({

            title: app.localize('Confirmacao'),
            paging: true,
            sorting: true,
            multiSorting: true,
            selecting: true,
            selectingCheckboxes: true,
            multiselect: true,

            rowInserted: function (event, data) {
               

                if (data) {
                    if (data.record.cor) {
                        data.row[0].cells[1].setAttribute('bgcolor', data.record.cor);
                        data.row[0].cells[1].setAttribute('color', data.record.cor);

                    
                           // .setAttribute("jtable-selecting-column", "false");
                    }

                    if (data.record.exameStatusId != 3) {
                        data.row[0].cells[0].querySelector('input').setAttribute("disabled", "disabled");
                    }
                    data.row[0].cells[0].classList.remove('jtable-selecting-column')


                }
            },

            rowUpdated: function (event, data) {
                if (data) {
                    if (data.record.cor) {
                        data.row[0].cells[1].setAttribute('bgcolor', data.record.cor);
                        data.row[0].cells[1].setAttribute('color', data.record.cor);

                        data.row[0].cells[0].setAttribute("readonly", true);
                    }
                }
            },

            actions: {
                listAction: {
                    method: _resultadoExameService.listarNaoConferidos
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },
                 Status: {
                    width: '4%',
                    display: function (data) {
                        return "  ";
                    }
                },

                actions: {
                    title: app.localize('Actions'),
                    width: '8%',
                    sorting: false,
                    selecting: false,
                    display: function (data) {
                        var $span = $('<span></span>');

                        $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-binoculars"></i></button>')
                       .appendTo($span)
                       .click(function () {
                           _ModalVisualizarExamePorColeta.open({ resultadoExameId: data.record.id });
                           // location.href = 'GestaoLaudos/CriarOuEditarModal/' + data.record.id
                       });

                        $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-history"></i></button>')
                     .appendTo($span)
                     .click(function () {
                         exibirHistorio(data.record.faturamentoItemId, data.record.exame);
                         // location.href = 'GestaoLaudos/CriarOuEditarModal/' + data.record.id
                     });


                        return $span;
                    }
                },


                Exame: {
                    title: app.localize('Exame'),
                    width: '65%',
                    display: function (data) {
                       
                        return data.record.exame;
                    }
                },

            }

         , selectionChanged: function () {

            

             var $selectedRows = _$examesTable.jtable('selectedRows');
             list = [];

             if ($selectedRows.length > 0) {
                 //Show selected rows

                 $selectedRows.each(function () {
                     var record = $(this).data('record');
                     list.push(record.id);
                 });

             }


         }

        });

        var _selectedDateRange = {
            startDate: moment().startOf('day'),
            endDate: moment().endOf('day')
        };


        function getColetas(reload) {
            if (reload) {
                _$examesTable.jtable('reload');
            } else {

                _$examesTable.jtable('load', {
                    coletaId: $('#coletaId').val(),

                });
            }
        }


        
        _$historicoTable.jtable({

            title: app.localize('Historico'),
            paging: true,
            sorting: true,
            multiSorting: true,
         
            actions: {
                listAction: {
                    method: _resultadoLaudoService.listarHistorioExamePorPaciente
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },

                DataColeta: {
                    title: app.localize('DataColeta'),
                    width: '20%',
                    display: function (data) {
                      return  moment(data.record.dataColeta).format('L');
                    }
                },


                Exame: {
                    title: app.localize('ItemExame'),
                    width: '50%',
                    display: function (data) {
                        return data.record.exame;
                    }
                },

                Resultado: {
                    title: app.localize('Resultado'),
                    width: '30%',
                    display: function (data) {
                        return data.record.resultado;
                    }
                },


            }

      

        });

        function getHistorico(reload) {
            if (reload) {
                _$historicoTable.jtable('reload');
            } else {

                _$historicoTable.jtable('load', {
                    exameId: $('#exameId').val(),
                    pacienteId: $('#pacienteId').val()

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


        $('.close-button').on('click', function () {
            location.href = '/mpa/ConferenciaResultadoExames';
        });


        $('#salvar').on('click', function () {
            _resultadoExameService.registrarConferenciaExames(list);
        });
        
        function exibirHistorio(exameId, exameNome)
        {
           
            $('#exame').text(exameNome);

            _$historicoTable.jtable('load', {
                exameId: exameId,
                pacienteId: $('#pacienteId').val()

              

            });
        }

    }
})();