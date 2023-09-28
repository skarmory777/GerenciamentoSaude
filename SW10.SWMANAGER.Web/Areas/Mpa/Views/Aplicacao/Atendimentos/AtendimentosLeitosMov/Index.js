(function () {
    $(function () {
       
        var _AtendimentoLeitoMovService = abp.services.app.atendimentoLeitoMovAppService;

        var _$AtendimentosLeitosMovFilterForm = $('#AtendimentosLeitosMovFilterForm');

        //function createRequestParams() {
        //    var prms = {};
        //    _$filterForm.serializeArray().map(function (x) { prms[x.name] = x.value; });
        //    return $.extend(prms, _selectedDateRange);
        //}

        //function getAtendimentosLeitosMov() {
        //    //_$AtendimentosLeitosMovTable.jtable('load', createRequestParams());
        //    _$AtendimentosLeitosMovTable.jtable('load');
        //}

        $('#RefreshAtendimentosLeitosMovsButton').click(function (e) {
            e.preventDefault();
            getAtendimentosLeitosMov();
        });

        // var _createOrEditModal = new app.ModalManager({
        //    viewUrl: abp.appPath + 'Mpa/Pacientes/CriarOuEditarModal',
        //    scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Pacientes/_CriarOuEditarModal.js',
        //    modalClass: 'CriarOuEditarPacienteModal'
        //});

        // AtendimentosLeitosMov Modal
        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/AtendimentoLeitoMov/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Atendimentos/AtendimentosLeitosMov/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarAtendimentoLeitoMovModalViewModel'
        });

        $('#TrocarLeitoButton').click(function () {
          //  _createOrEditModal.open();
        });

        function getAtendimentosLeitosMov() {
            _$AtendimentosLeitosMovTable.jtable('load', null);
        }

//==============================FIM DO PAGELOAD============================

        function deleteVisitante(visitante) {

            abp.message.confirm(
                app.localize('DeleteWarning', visitante),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _AtendimentoLeitoMovService.excluir(visitante.id)
                            .done(function () {
                                getVisitantes(true);
                                abp.notify.success(app.localize('SuccessfullyDeleted'));
                            });
                    }
                }
            );
        }

        var _$AtendimentosLeitosMovTable = $('#AtendimentosLeitosMovTable');

        //JTABLE AtendimentosLeitosMov
        _$AtendimentosLeitosMovTable.jtable({
            title: app.localize('AtendimentosLeitosMov'),
            paging: true,
            sorting: true,
            multiSorting: true,
            actions: {
                listAction: {
                    method: abp.services.app.atendimentoLeitoMov.listarFiltro
                }
            },
            fields: {
                id: {
                    key: true,
                    list: false
                },
                //actions: {
                //    title: app.localize('Actions'),
                //    width: '6%',
                //    sorting: false,
                //    display: function (data) {
                //        var $span = $('<span></span>');
                //        //if (_permissions.edit) {
                //        $('<button class="btn btn-default btn-xs" title="' + app.localize('Trasnferir') + '"><i class="fa fa-retweet"></i></button>')
                //           .appendTo($span)
                //           .click(function () {
                //               _createOrEditModal.open({ data: data.record });
                //           });
                //        //}
                //        //if (_permissions.delete) {
                //        $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                //            .appendTo($span)
                //            .click(function () {
                //                deleteVisitante(data.record);
                //            });
                //        return $span;
                //    }
                //},
                AtendimentoLeitoMov: {
                    title: app.localize('CodigoIntenacao'),
                    width: '3%',
                    display: function (data) {
                        if (data.record.atendimento) {
                            return data.record.atendimento.codigo;
                        }
                    }
                },
                Leito: {
                    title: app.localize('Leito'),
                    width: '6%',
                    display: function (data) {

                        if (data.record.leito)
                        {
                            return data.record.leito.descricao;
                        }
                    }
                },
                tipoLeito: {
                    title: app.localize('tipoLeito'),
                    width: '5%',
                    display: function (data) {
                        if (data.record.leito) {
                            if (data.record.leito.tipoAcomodacao) {
                                return data.record.leito.tipoAcomodacao.descricao;
                            }
                        }
                    }
                }
                ,
                paciente: {
                    title: app.localize('Paciente'),
                    width: '7%',
                    display: function (data) {
                        if (data.record.atendimento) {
                            if (data.record.atendimento.paciente) {
                                return data.record.atendimento.paciente.nomeCompleto;
                            }
                        }
                    }
                }
                ,
                dataInicial: {
                    title: app.localize('DataInicial'),
                    width: '4%',
                    display: function (data) {
                        return moment(data.record.dataInicial).format('L LT');
                    }
                },

                dataFinal: {
                    title: app.localize('DataFinal'),
                    width: '4%',
                    display: function (data) {
                        if (data.record.dataFinal) {
                            return moment(data.record.dataFinal).format('L LT');
                        }
                       
                    }
                },

               
                dataInclusao: {
                    title: app.localize('dataInclusao'),
                    width: '5%',
                    display: function (data) {
                        return moment(data.record.dataInclusao).format('L LT');
                    }
                },
                dataAlta: {
                    title: app.localize('Alta'),
                    width: '6%',
                    display: function (data) {
                        if (data.record.atendimento) {
                            if (data.record.atendimento.dataAlta) {
                                return moment(data.record.atendimento.dataAlta).format('L LT');
                            }
                        } else {
                            return ("");
                        }
                    }
                },
            }
        });

        getAtendimentosLeitosMov();

        abp.event.on('app.CriarOuEditarAtendimentoLeitoMovModalSaved', function () {
            getAtendimentosLeitosMov();
        });
        

    });

})();

