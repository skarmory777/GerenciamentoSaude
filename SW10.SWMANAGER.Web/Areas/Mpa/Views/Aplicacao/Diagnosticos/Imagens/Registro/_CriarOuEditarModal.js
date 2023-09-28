(function ($) {
    app.modals.CriarOuEditarModal = function () {

        var _registroExemesService = abp.services.app.registroExemes;
        var _atendimentoService = abp.services.app.atendimento;
        var _medicoService = abp.services.app.medico;

        var _modalManager;
        var _$registroExameInformationsForm = null;
        var _$exameTable = $('#exameTable');

        var _ErrorModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Erros/ExibirErros',
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$registroExameInformationsForm = $('form[name=RegistroExameInformationsForm]');
            _$registroExameInformationsForm.validate();
            CamposRequeridos();
            $('.modal-dialog').css('width', '900px');
            $('.select2').css('width', '100%');
            if ($('#atendimento-id').val() > 0) {
                $('#atendimento-id').trigger('change');
            }
        };

        $('#salvar').click(function (e) {
            e.preventDefault()
            _$registroExameInformationsForm = $('form[name=RegistroExameInformationsForm]');
            _$registroExameInformationsForm.validate();

            if (!_$registroExameInformationsForm.valid()) {
                return;
            }

            var registroExame = _$registroExameInformationsForm.serializeFormToObject();

            _registroExemesService.criarOuEditar(registroExame)
                 .done(function (data) {

                     if (data.errors.length > 0) {
                         _ErrorModal.open({ erros: data.errors });
                     }
                     else {

                         abp.notify.info(app.localize('SavedSuccessfully'));

                         location.href = '/mpa/registroExames';

                     }
                 })
                .fail(function (data) {
                    if (data.length > 0) {
                        _ErrorModal.open({ erros: errors });
                    }

                })

                .always(function (data) {
                });
        });

        $('.close').on('click', function () {
            location.href = '/mpa/registroExames';
        });

        $('.close-button').on('click', function () {
            location.href = '/mpa/registroExames';
        });

        var lista = [];

        $('input[name="DataRegistro"]').daterangepicker({
            "singleDatePicker": true,
            "showDropdowns": true,
            autoUpdateInput: false,
            //  maxDate: new Date(),
            changeYear: true,
            yearRange: 'c-10:c+10',
            showOn: "both",
            timePicker: true,
            timePicker24Hour: true,

            onChange: function (date) {
                alert(date)
            }, //changeSolicitacao(),

            "locale": {
                "format": moment.locale().toUpperCase() === 'PT-BR' ? "DD/MM/YYYY HH:mm" : moment.locale().toUpperCase() === 'US' ? "MM/DD/YYYY" : "YYYY-MM-DD HH:mm",
                "separator": " - ",
                "applyLabel": "Apply",
                "cancelLabel": "Cancel",
                "fromLabel": "From",
                "toLabel": "To",
                "customRangeLabel": "Custom",
                "daysOfWeek": [
                    app.localize('Dom'),
                    app.localize('Seg'),
                    app.localize('Ter'),
                    app.localize('Qua'),
                    app.localize('Qui'),
                    app.localize('Sex'),
                    app.localize('Sab')
                ],
                "monthNames": [
                    app.localize("Jan"),
                    app.localize("Fev"),
                    app.localize("Mar"),
                    app.localize("Abr"),
                    app.localize("Mai"),
                    app.localize("Jun"),
                    app.localize("Jul"),
                    app.localize("Ago"),
                    app.localize("Set"),
                    app.localize("Out"),
                    app.localize("Nov"),
                    app.localize("Dez"),
                ],
                "firstDay": 0
            }
        },
        function (selDate) {
            $('input[name="DataRegistro"]').val(selDate.format('L LT')).addClass('form-control edited');
        });

        $('input[name="DataRegistro"]').on('apply.daterangepicker', function () {
            //changeSolicitacao();
        });

        $('#isContraste').on('click', function (e) {

            var checkbox = e.target;
            if (checkbox.checked) {
                $('#divQtdContraste').show();
            }
            else {
                $('#divQtdContraste').hide();
            }

        });

        _$exameTable.jtable({
            title: app.localize('Itens'),
            //paging: true,
            sorting: true,
            edit: false,
            create: false,
            multiSorting: true,

            fields:
            {
                IdGrid: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '10%',
                    sorting: false,
                    display: function (data) {
                        var $span = $('<span></span>');

                        $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                          .appendTo($span)
                          .click(function (e) {
                              e.preventDefault();
                              deleteRegistro(data.record);
                          });
                        console.log(data);
                        if (data.record.AccessNumber) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('AccessNumber') + '"><i class="fas fa-images"></i></button>')
                                .appendTo($span)
                                .click(function (e) {
                                    //_createOrEditModal.open({ id: data.record.id });
                                    e.preventDefault();
                                    window.open(data.record.AccessNumber);
                                    
                                });
                        }
                        return $span;
                    }
                },

                ExameDescricao: {
                    title: app.localize('Exame'),
                    width: '90%',
                    display: function (data) {
                        if (data.record.ExameDescricao) {
                            return data.record.ExameDescricao;
                        }
                    }
                },




            }
        });

        function geExameTable(reload) {

            lista = JSON.parse($('#examesJson').val());

            var allRows = _$exameTable.find('.jtable-data-row')

            $.each(allRows, function () {
                var id = $(this).attr('data-record-key');

                _$exameTable.jtable('deleteRecord', { key: id, clientOnly: true });

            });

            for (var i = 0; i < lista.length; i++) {
                var item = lista[i];
                _$exameTable.jtable('addRecord', {
                    record: item
                    , clientOnly: true
                });
            }
        }

        $('#inserir').click(function (e) {
            e.preventDefault();

            inserir($('#exameId'))

        });

        $('#inserirExameSolicitado').click(function (e) {
            e.preventDefault();

            inserir($('#examesSolicitadosId'))

        });

        function inserir(campo) {

            if ($('#examesJson').val() != '') {
                lista = JSON.parse($('#examesJson').val());
            }

            if ($('#idGrid').val() != '') {

            }
            else {

                var registroExame = { IdGrid: 0, ExameId: 0, ExameDescricao: "" };

                registroExame.IdGrid = lista.length == 0 ? 1 : lista[lista.length - 1].IdGrid + 1;

                var exame = campo.select2('data');
                if (exame && exame.length > 0) {

                    registroExame.ExameDescricao = exame[0].text;
                }

                registroExame.ExameId = campo.val();
                // registroExame.IsContraste = $('#isContraste')[0].checked;
                //  registroExame.VolumeContrasteTotal = $('#qtdeConstraste').val();


                lista.push(registroExame);

                _$exameTable.jtable('addRecord', {
                    record: registroExame
                  , clientOnly: true
                });

            }
            $('#examesJson').val(JSON.stringify(lista));
            campo.val('').trigger('change');
            campo.focus();
        }

        geExameTable();

        function deleteRegistro(exame) {
            abp.message.confirm(
                app.localize('DeleteWarning', exame.ExameDescricao),
                function (isConfirmed) {
                    if (isConfirmed) {

                        lista = JSON.parse($('#examesJson').val());

                        for (var i = 0; i < lista.length; i++) {
                            if (lista[i].IdGrid == exame.IdGrid) {
                                lista.splice(i, 1);
                                $('#examesJson').val(JSON.stringify(lista));

                                _$exameTable.jtable('deleteRecord', {
                                    key: exame.IdGrid
                                , clientOnly: true
                                });

                                break;
                            }
                        }

                    }
                }
            );
        }

        //selectSWMultiplosFiltros('.selectAtendimento', "/api/services/app/Atendimento/ListarAtendimentosAmbulatorioInternacao", [{ valor: $('#AmbulatorioInternacao').val() == 1 }, { valor: $('#AmbulatorioInternacao').val() == 2 }]);
        if ($('#atendimento-id').val() > 0 && $('#ambulatorio-internacao').val() > 0) {
            criarSelect2Atendimento();
        }
        selectSW('.selectConvenio', "/api/services/app/Convenio/ListarDropdown");
        selectSW('.selectMedico', "/api/services/app/Medico/ListarDropdown");
        selectSW('.selectTecnico', "/api/services/app/tecnico/ListarDropdown");
        selectSW('.selectFaturamentoItem', "/api/services/app/FaturamentoItem/ListarDiagnosticoImagemDropdown");
        selectSW('.selectCentroCusto', "/api/services/app/CentroCusto/ListarDropdownCodigoCentroCusto");
        selectSW('.selectTipoLeito', "/api/services/app/TipoAcomodacao/ListarDropdown");
        selectSW('.selectLeito', "/api/services/app/Leito/ListarDropdown");
        selectSW('.selectExame', "/api/services/app/FaturamentoItem/ListarDiagnosticoImagemDropdown");
        selectSWMultiplosFiltros('.selectExamesSolicitados', "/api/services/app/SolicitacaoExameItem/ListarDropdownNaoRegistradoPorAtendimento", ['atendimento-id']);
        selectSW('.selectTurno', "/api/services/app/Turno/ListarDropdown");

        $('#ambulatorio-internacao').on('change', function () {
            if ($(this).val() > 0) {
                $('#atendimento-id').removeClass('hidden');
                //selectSWMultiplosFiltros('.selectAtendimento', "/api/services/app/Atendimento/ListarAtendimentosAmbulatorioInternacao", [{ valor: $('#AmbulatorioInternacao').val() == 1 }, { valor: $('#AmbulatorioInternacao').val() == 2 }]);
                criarSelect2Atendimento();
            }
            else {
                $('#atendimento-id').addClass('hidden');
            }
        });

        //function changeSolicitacao() {
        //    selectSWMultiplosFiltros('.selectExamesSolicitados', "/api/services/app/SolicitacaoExameItem/ListarDropdownNaoRegistradoPorAtendimento", ['atendimento-id', 'dataRegistro']);
        //}

        //selectSWMultiplosFiltros('.selectExamesSolicitados', "/api/services/app/SolicitacaoExameItem/ListarDropdownNaoRegistradoPorAtendimento", ['atendimento-id', 'dataRegistro']);

        function criarSelect2Atendimento() {
            $('#atendimento-id').select2({
                allowClear: true,
                placeholder: app.localize("SelecioneLista"),
                ajax: {
                    url: '/api/services/app/paciente/ListarPacientesEmAtendimentoDropdown',
                    dataType: 'json',
                    delay: 250,
                    method: 'Post',

                    data: function (params) {
                        if (params.page == undefined)
                            params.page = '1';

                        return {
                            search: params.term,
                            page: params.page,
                            totalPorPagina: 10,
                            filtros: [$('#ambulatorio-internacao').val() == 1, $('#ambulatorio-internacao').val() == 2]
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
            }).on('change', function () {
                if ($(this).val() != '' && $(this).val() != null && $(this).val() > 0) {
                    _atendimentoService.obter($(this).val())
                       .done(function (data) {
                           if (data.convenio) {
                               $('#convenioId').append($("<option>")
                                               .val(data.convenioId)
                                               .text(data.convenio.nomeFantasia)
                                                )
                                                .val(data.convenioId)
                                                .trigger("change");
                           }

                           if (data.isInternacao && data.leito) {

                               $('#leitoId').append($("<option>")
                                               .val(data.leitoId)
                                               .text(data.leito.descricao)
                                                )
                                                .val(data.leitoId)
                                                .trigger("change");

                               if (data.leito.tipoAcomodacao) {
                                   $('#tipoLeitoId').append($("<option>")
                                             .val(data.leito.tipoAcomodacaoId)
                                             .text(data.leito.tipoAcomodacao.descricao)
                                              )
                                              .val(data.leito.tipoAcomodacaoId)
                                              .trigger("change");
                               }
                           }

                           if (data.medico != null) {
                               $('#medico-solicitante-id').append($("<option>")
                                             .val(data.medicoId)
                                             .text(data.medico.nomeCompleto)
                                              )
                                              .val(data.medicoId)
                                              .trigger("change");
                               $('#crm').val(data.medico.numeroConselho);
                               $('#medico-solicitante').val(data.medico.nomeCompleto);
                           }
                       });
                    //changeSolicitacao();
                }
            });
        }

        $('#medico-solicitante-id').on('change', function () {
            _medicoService.obter($(this).val())
               .done(function (data) {
                   $('#crm').val(data.numeroConselho);
                   $('#medico-solicitante').val(data.nomeCompleto);
               });
        });
    }
})(jQuery);