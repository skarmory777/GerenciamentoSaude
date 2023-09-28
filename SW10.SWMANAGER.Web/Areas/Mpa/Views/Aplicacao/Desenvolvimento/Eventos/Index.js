(function () {
    $(function () {
        //data table
        var _$EventosTable = $('#EventosTable');

        // Eventos
        var _EventosService = abp.services.app.evento;

        //var _permissions = {
        //    create: abp.auth.hasPermission('Pages.Tenant.Atendimento.CadastrosGlobais.Paciente.Create'),
        //    edit: abp.auth.hasPermission('Pages.Tenant.Atendimento.CadastrosGlobais.Paciente.Edit'),
        //    'delete': abp.auth.hasPermission('Pages.Tenant.Atendimento.CadastrosGlobais.Paciente.Delete')
        //};

        console.log("aqui");
        var _$filterForm = $('#EventosFilterForm');

        //Date Range Picker
        var _selectedDateRange = {
            startDate: moment().startOf('day'),
            endDate: moment().endOf('day')
        };

        _$filterForm.find('.date-range-picker').daterangepicker(
            $.extend(true, app.createDateRangePickerOptions(), _selectedDateRange),
            function (start, end, label) {
                _selectedDateRange.startDate = start.format('YYYY-MM-DDT00:00:00Z');
                _selectedDateRange.endDate = end.format('YYYY-MM-DDT23:59:59.999Z');
            });

          $(".select2Paciente").select2({
            language: 'fr',
            ajax: {
                url: "/api/services/app/paciente/ListarDropdown",
                dataType: 'json',
                delay: 250,
                method: 'Post',
                data: function (params) {
                    if (params.page == undefined)
                        params.page = '1';
                    return {
                        search: params.term,
                        page: params.page,
                        totalPorPagina: 10
                    };
                },
                processResults: function (data, params) {
                    params.page = params.page || 1;
                    //console.log('data: ',data);
                    return {

                        results: data.result.items,
                        pagination: {
                            more: (params.page * 10) < data.result.totalCount
                        }
                    };
                },
                cache: true
            },
            escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
            minimumInputLength: 0

          });
          $(".select2Origem").select2({
              ajax: {
                  url: "/api/services/app/origem/ListarDropdown",
                  dataType: 'json',
                  delay: 250,
                  method: 'Post',
                  data: function (params) {
                      if (params.page == undefined)
                          params.page = '1';
                      return {
                          search: params.term,
                          page: params.page,
                          totalPorPagina: 10
                      };
                  },
                  processResults: function (data, params) {
                      params.page = params.page || 1;

                      return {
                          results: data.result.items,
                          pagination: {
                              more: (params.page * 10) < data.result.totalCount
                          }
                      };
                  },
                  cache: true
              },
              escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
              minimumInputLength: 0
          });

        // Eventos Modal
        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Internacoes/Eventos',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Desenvolvimento/Eventos/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarEventoModalViewModel'
        });

        //JTABLE VISITANTES
        _$EventosTable.jtable({
            title: app.localize('Eventos'),
            paging: true,
            sorting: true,
            multiSorting: true,
            actions: {
                listAction: {
                    method: abp.services.app.evento.listarTodos
                }
            },
            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '5%',
                    sorting: false,
                    display: function (data) {
                        var $span = $('<span></span>');

                        //if (_permissions.edit) {
                        $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                           .appendTo($span)
                           .click(function () {
                               //waitingDialog.show();
                               //editarAtendimento(data.record);
                               _createOrEditModal.open({ id: data.record.id });
                           });

                        //}
                        //if (_permissions.delete) {
                        $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                            .appendTo($span)
                            .click(function () {
                                deleteAtendimentos(data.record);
                            });
                        return $span;
                    }
                },

                Visitante: {
                    title: app.localize('Visitante'),
                    width: '6%',
                    display: function (data) {
                        return data.record.nome;
                    }
                },
                documento: {
                    title: app.localize('Documento'),
                    width: '4%',
                    display: function (data) {
                        return data.record.documento;
                    }
                },
                dataEntrada: {
                    title: app.localize('Entrada'),
                    width: '4%',
                    display: function (data) {
                        return moment(data.record.dataEntrada).format('L');
                    }
                },

                dataSaida: {
                    title: app.localize('Saida'),
                    width: '4%',
                    display: function (data) {
                        return moment(data.record.dataSaida).format('L');
                    }
                },

                paciente: {
                    title: app.localize('Paciente'),
                    width: '7%',
                    display: function (data) {
                        if (data.record.atendimento) {
                            return data.record.atendimento.paciente.nomeCompleto;
                        }
                    }
                }
                ,
                dataRegistro: {
                    title: app.localize('DataInternacao'),
                    width: '5%',
                    display: function (data) {
                        if (data.record.atendimento) {
                            return moment(data.record.atendimento.dataRegistro).format('L LT');
                        }
                    }
                },
                dataAlta: {
                    title: app.localize('DataAlta'),
                    width: '6%',
                    display: function (data) {
                        //return moment(data.record.dataAlta).format("DD/MM/YYYY HH:mm");
                        if (data.record.atendimento) {
                            return moment((data.record.atendimento.dataAlta ? data.record.atendimento.dataAlta.format("DD/MM/YYYY HH:mm") : null));
                        }
                    }
                },
                acompanhante: {
                    title: app.localize('Acompanhante'),
                    width: '6%'
                    ,
                    display: function (data) {
                        var teste;
                        var label;
                        if (data.record.isMedico) {
                            teste = "Yes";
                            label = "success";
                        } else {
                            teste = "No";
                            label = "default";
                        }
                        return '<div style="text-align:center;">' + '<span class="label label-' + label + ' content-center text-center">' + app.localize(teste) + '</span>' + '</div>';
                    }
                }
                ,

                medico: {
                    title: app.localize('Medico'),
                    width: '6%'
                   ,
                    display: function (data) {
                       // return (data.record.isMedico ? "Sim" : "Não");
                        var teste;
                        var label;
                        if (data.record.isMedico) {
                            teste = "Yes";
                            label = "success";
                        } else {
                            teste = "No";
                            label = "default";
                        }
                        return '<div style="text-align:center;">' + '<span class="label label-'+label+' content-center text-center">' + app.localize(teste) + '</span>' + '</div>';

                    }
                }
                ,

                leito: {
                    title: app.localize('leito'),
                    width: '5%',
                    display: function (data) {
                        if (data.record.atendimento.leito) {
                            return data.record.atendimento.leito.descricao;
                        }
                    }
                }
              
                //unidadeOrganizacioanl: {
                //    title: app.localize('unidadeOrganizacioanl'),
                //    width: '4%',
                //    display: function (data) {
                //        if (data.record.atendimento.unidadeOrganizacional) {
                //            return data.record.atendimento.unidadeOrganizacional.descricao;
                //        }
                //    }
                //}
            }
        });

        $('#RefreshEventossButton').click(function (e) {
            e.preventDefault();
            getEventos();
        });

        //Date Range Picker
        var _$filterForm = $('#EventosFilterForm');

        var _selectedDateRange = {
            startDate: moment().startOf('day'),
            endDate: moment().endOf('day')
        };

        _$filterForm.find('.date-range-picker').daterangepicker(
            $.extend(true, app.createDateRangePickerOptions(), _selectedDateRange),
            function (start, end, label) {
                _selectedDateRange.startDate = start.format('YYYY-MM-DDT00:00:00Z');
                _selectedDateRange.endDate = end.format('YYYY-MM-DDT23:59:59.999Z');
            });

        function createRequestParams() {
            var prms = {};
            _$filterForm.serializeArray().map(function (x) { prms[x.name] = x.value; });
            return $.extend(prms, _selectedDateRange);
        }

        function getEventos() {
            _$EventosTable.jtable('load', createRequestParams());
        }

        _$EventosTable.jtable('load', null);

        getEventos();

    });

})();

