(function () {
    $(function () {
        var _$registroExemesTable = $('#registroExemesTable');
        var _registroExemesService = abp.services.app.registroExemes;
        var _$contasPagarFilterForm = $('#contasPagarFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Diagnosticos.Imagens.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Diagnosticos.Imagens.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Diagnosticos.Imagens.Delete')
        };


        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/GestaoLaudos/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Diagnosticos/Imagens/GestaoLaudos/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarModal'
        });



        _$registroExemesTable.jtable({

            title: app.localize('RegistrosExames'),
            paging: true,
            sorting: true,
            multiSorting: true,
            selecting: true,
            selectingCheckboxes: true,
            multiselect: true,



            rowInserted: function (event, data) {
                if (data) {
                    //if (data.record.status==1) {
                    //    data.row[0].cells[2].setAttribute('color', data.record.corLancamentoLetra);
                    //}

                    if (data.record.status == 1) {
                        data.row[0].cells[2].setAttribute('bgcolor', '#daed0b');

                    } else if (data.record.status == 2) {
                        data.row[0].cells[2].setAttribute('bgcolor', '#138440');
                    } else if (data.record.status == 3) {
                        data.row[0].cells[2].setAttribute('bgcolor', '#0a3bed');
                        //data.row[0].cells[2].setAttribute('fgcolor', '#ffffff');
                    } else if (data.record.status == 4) {
                        data.row[0].cells[2].setAttribute('bgcolor', '#0aedca');
                    }



                }

            },


            actions: {
                listAction: {
                    method: _registroExemesService.listarMovimentosItens
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
                        if (_permissions.edit && data.record.status < 4) {
                            var label = '';
                            if (data.record.status == 1) {
                                label = 'Parecer';
                            }
                            else if (data.record.status == 2) {
                                label = 'Laudo';
                            } else if (data.record.status == 3) {
                                label = 'Revisar';
                            }
                            $('<button class="btn btn-default btn-xs" title="' + app.localize(label) + '"><i class="fa fa-check-square-o"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    //_createOrEditModal.open({ id: data.record.id });
                                    location.href = 'GestaoLaudos/CriarOuEditarModal/' + data.record.id
                                });
                        }
                        if (_permissions.edit && data.record.status > 2) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    //_createOrEditModal.open({ id: data.record.id });
                                    location.href = 'GestaoLaudos/EditarLaudoModal/' + data.record.id
                                });
                        }


                        return $span;
                    }
                },

                Status: {
                    title: app.localize('Status'),
                    width: '10%',
                    display: function (data) {

                        if (data.record.status == 1) {
                            return '<span style="color:#000;">' + app.localize('Pendente') + '</span>';
                        }
                        else if (data.record.status == 2) {
                            return '<span style="color:#000;">' + app.localize('Parecer') + '</span>';
                        } else if (data.record.status == 3) {
                            return '<span style="color:#fff;">'+app.localize('Laudo')+'</span>';
                        } else if (data.record.status == 4) {
                            return '<span style="color:#000;">' + app.localize('LaudoRevisado') + '</span>';
                        }

                    }
                },

                Registro: {
                    title: app.localize('Registro'),
                    width: '10%',
                    display: function (data) {
                        return data.record.codigo;
                    }
                },

                //InternacaoAmbulatorio: {
                //    title: app.localize('InternacaoAmbulatorio'),
                //    width: '10%',
                //    display: function (data) {
                //        return data.record.InternacaoAmbulatorio;
                //    }
                //},

                Paciente: {
                    title: app.localize('Paciente'),
                    width: '20%',
                    display: function (data) {
                        if (data) {
                            return data.record.pacienteDescricao;
                        }
                    }
                },

                Exame: {
                    title: app.localize('Exame'),
                    width: '20%',
                    display: function (data) {
                        return data.record.exame;
                    }
                },



                ConvenioDescricao: {
                    title: app.localize('Convenio'),
                    width: '7%',
                    display: function (data) {
                        if (data) {
                            return data.record.convenioDescricao;
                        }
                    }
                },


                Contraste: {
                    title: app.localize('Contraste'),
                    width: '3%',
                    listClass: 'Centralizado',
                    display: function (data) {
                        if (data.record.isContraste) {
                            return '<span class="label label-success">' + app.localize('Yes') + '</span>';
                        } else {
                            return '<span class="label label-default">' + app.localize('No') + '</span>';
                        }
                    }
                },


                //QtdContraste: {
                //    title: app.localize('QtdContraste'),
                //    width: '7%',
                //    display: function (data) {
                //        if (data) {
                //            return data.record.qtdContraste;
                //        }
                //    }
                //},

            }
        });

        function getRegistros(reload) {

            if (reload) {
                _$registroExemesTable.jtable('reload');
            } else {
                _$registroExemesTable.jtable('load', {
                    // filtro: $('#tableFilter').val()
                    convenioId: $('#convenioIndexId').val(),
                    pacienteId: $('#pacienteIndexId').val(),
                    emissaoDe: _selectedDateRangePeriodo.startDate,
                    emissaoAte: _selectedDateRangePeriodo.endDate,
                    horarioInicial: $('#convenioIndexId').val(),
                    horarioFinal: $('#convenioIndexId').val(),
                    modalidadeId: $('#modalidadeId').val(),
                });
            }
        }

        function deleteRegistro(record) {

            abp.message.confirm(
                app.localize('DeleteWarning', record.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _$contasPagarTable.excluir(record)
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

        $('.novo-lancamento').click(function (e) {

            e.preventDefault();

            _createOrEditModal.open();
            //location.href = 'RegistroExames/CriarOuEditarModal/';
        });


        $('#refreshLancamentosButton').click(function (e) {
            e.preventDefault();
            getRegistros();
        });

        abp.event.on('app.CriarOuEditarFeriadoModalSaved', function () {
            getRegistros(true);
        });

        $('#tableFilter').focus();

        var _selectedDateRangePeriodo = {
            startDate: moment().startOf('month'),
            endDate: moment().endOf('month'),
            maxDate: "31/12/2030"
        };

        _$contasPagarFilterForm.find('input.periodo').daterangepicker(
        $.extend(true, app.createDateRangePickerOptions(), _selectedDateRangePeriodo),
        function (start, end, label) {
            _selectedDateRangePeriodo.startDate = start.format('YYYY-MM-DDT00:00:00Z');
            _selectedDateRangePeriodo.endDate = end.format('YYYY-MM-DDT23:59:59.999Z');
        });










        $('#emissao').on('cancel.daterangepicker', function (ev, picker) {
            //do something, like clearing an input
            $('#emissao').val('');
        });

        $('#vencimento').on('cancel.daterangepicker', function (ev, picker) {
            //do something, like clearing an input

            $('#vencimento').val('');
        });



        getRegistros();
        $('#emissao').val('');

        selectSW('.selectConvenio', "/api/services/app/Convenio/ListarDropdown");
        selectSW('.selectPaciente', "/api/services/app/Paciente/ListarDropdown");
        selectSW('.selectModalidade', "/api/services/app/Modalidade/ListarDropdown");
    });
})();