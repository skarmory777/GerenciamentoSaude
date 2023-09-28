(function ($) {

    app.modals.CriarOuEditarPacienteModal = function () {

        var _pacientesService = abp.services.app.paciente;

        var _modalManager;
        var _$pacienteInformationForm = null;

        var _$pacienteConveniosInformationForm = null;
        var _$pacientePesosInformationForm = null;
        var _$pacientePlanosInformationForm = null;

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Pacientes/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Pacientes/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarPacienteModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$pacienteInformationForm = _modalManager.getModal().find('form[name=PacienteInformationsForm]');
            _$pacienteInformationForm.validate();
            
            atualizarTabela();

            $('.modal-dialog').css({ 'width': '90%', 'max-width': '1100px' });
            $('div.form-group select').addClass('form-control selectpicker');
            //Forçando o modal a aceitar sobreposição por causa dos selects do datarangepicker
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
            //Ativando autocomplete no combobox
            $('.chosen-select').chosen({ no_results_text: app.localize("NotFound"), width: '100%' });
            $('ul.ui-autocomplete').css('z-index', '2147483647');
        };

        this.save = function () {
            if (!_$pacienteInformationForm.valid()) {
                return;
            }

            var paciente = _$pacienteInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            var editMode = $('#is-edit-mode').val();

            _pacientesService.criarOuEditar(paciente)
                 .done(function (data) {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarPacienteModalSaved');
                     if (!editMode) {
                         _createOrEditModal.open({ id: data.id });
                     }
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };

        $('input[name="Nascimento"]').daterangepicker({
            "singleDatePicker": true,
            "showDropdowns": true,
            autoUpdateInput: false,
            maxDate: new Date(),
            changeYear: true,
            yearRange: 'c-10:c+10',
            showOn: "both",
            "locale": {
                "format": moment.locale().toUpperCase() === 'PT-BR' ? "DD/MM/YYYY" : moment.locale().toUpperCase() === 'US' ? "MM/DD/YYYY" : "YYYY-MM-DD",
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
            $('input[name="Nascimento"]').val(selDate.format('L')).addClass('form-control edited');
            obterIdade(selDate);
        });

        $('input[name="Emissao"]').daterangepicker({
            "singleDatePicker": true,
            "showDropdowns": true,
            maxDate: new Date(),
            autoUpdateInput: false,
            changeYear: true,
            yearRange: 'c-10:c+10',
            showOn: "both",
            "locale": {
                "format": moment.locale().toUpperCase() === 'PT-BR' ? "DD/MM/YYYY" : moment.locale().toUpperCase() === 'US' ? "MM/DD/YYYY" : "YYYY-MM-DD",
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
            $('input[name="Emissao"]').val(selDate.format('L')).addClass('form-control edited');
        });

        function obterIdade(date) {
            var output = new Date(date);
            $('#idade').load('/mpa/Pacientes/ObterIdade?data=' + output.getFullYear() + '-' + (output.getMonth() + 1) + '-' + output.getDate());
        }

        $('#capturar-foto').click(function (e) {
            e.preventDefault();
            if ($('#area-captura').html() === '') {
                $('#area-captura').load("/mpa/pacientes/_CarregarFoto", function () {
                    $(this).removeClass('hidden');
                    $('#capturar-foto').html(app.localize('EncerrarCaptura'));
                })
            }
            else {
                if (localMediaStream) {
                    localMediaStream.getVideoTracks()[0].stop();
                }
                $(this).html(app.localize('CapturarFoto'));
                $('#area-captura').html('').addClass('hidden');
            }
        });

        $('#btn-buscar-cep').click(function (e) {
            e.preventDefault();
            var cep = $('#cep').val().replace('-', '');
            if (isNaN(cep)) {
                abp.notify.info(app.localize("CepInvalido"));
                return false;
            }
            if (cep === '') {
                abp.notify.info(app.localize("InformarCep"));
                return false;
            }
            if (cep.length !== 8) {
                abp.notify.info(app.localize("TamanhoCep"));
                return false;
            }
            buscarCep(cep);
        });

        // Novo Convenio
        $('#btn-novo-convenio').click(function (e) {
            e.preventDefault()
            $('#paciente-convenio-parcial').load('/Pacientes/_CriarOuEditarPacienteConvenioModal?pacienteId=' + $('#id').val());
        });

        // Novo Peso (Modal inexistente...)
        $('#btn-novo-peso').click(function (e) {
            e.preventDefault()
            $('#paciente-peso-parcial').load('/Pacientes/_CriarOuEditarPacientePesoModal?pacienteId=' + $('#id').val());
        });

        // Novo Plano (Modal inexistente...)
        $('#btn-novo-plano').click(function (e) {
            e.preventDefault()
            $('#paciente-plano-parcial').load('/Pacientes/_CriarOuEditarPacientePlanoModal?pacienteId=' + $('#id').val());
        });

        function atualizarTabela() {
            // Convenios
            $('#PacienteConveniosTable').load('/Pacientes/_PacienteConvenios?id=' + $('#id').val());

            // Pesos
            $('#PacientePesosTable').load('/Pacientes/_PacientePesos?id=' + $('#id').val());

            // Planos
            $('#PacientePlanosTable').load('/Pacientes/_PacientePlanos?id=' + $('#id').val());
        }

        // AutoComplete
        $('#convenio-search')
            .autocomplete({
                minLength: 3,
                delay: 0,
                source: function (request, response) {
                    var term = $('#convenio-search').val();
                    var url = '/mpa/convenios/autocomplete';
                    var fullUrl = url + '/?term=' + term;
                    $.getJSON(fullUrl, function (data) {
                        if (data.result.length == 0) {
                            $('#convenio-id').val(0);
                            $("#convenio-search").focus();
                            abp.notify.info(app.localize("ListaVazia"));
                            return false;
                        };
                        response($.map(data.result, function (item) {
                            $('#convenio-id').val(0);
                            return {
                                value: item.nome,
                                label: item.nome,
                                realValue: item.id
                            };
                        }));
                    });
                },
                select: function (event, ui) {
                    $('#convenio-id').val(ui.item.realValue);
                    $('#convenio-search').val(ui.item.value);
                    //$('.save-button').removeAttr('disabled');
                    return false;
                },
                change: function (event, ui) {
                    event.preventDefault();
                    if (ui.item == null) {
                        //$('.save-button').attr('disabled', 'disabled');
                        $('#convenio-id').val(0);
                        $("#convenio-search").val('').focus();
                        abp.notify.info(app.localize("ConvenioInvalido"));
                        return false;
                    }
                },
            });

        $('#plano-search')
            .autocomplete({
                minLength: 3,
                delay: 0,
                source: function (request, response) {
                    var term = $('#plano-search').val();
                    var url = '/mpa/planos/autocomplete';
                    if ($('#convenio-id').length > 0) {
                        term += "&convenioId=" + $('#convenio-id').val();
                    }
                    var fullUrl = url + '/?term=' + term;

                    $.getJSON(fullUrl, function (data) {
                        if (data.result.length == 0) {
                            $('#plano-id').val(0);
                            $("#plano-search").focus();
                            abp.notify.info(app.localize("ListaVazia"));
                            return false;
                        };
                        response($.map(data.result, function (item) {
                            $('#plano-id').val(0);
                            return {
                                label: item.nome,
                                value: item.nome,
                                realValue: item.id
                            };
                        }));
                    });
                },
                select: function (event, ui) {
                    $('#plano-id').val(ui.item.realValue);
                    $('#plano-search').val(ui.item.value);
                    //$('.save-button').removeAttr('disabled');
                    return false;
                },
                change: function (event, ui) {
                    event.preventDefault();
                    if (ui.item == null) {
                        //$('.save-button').attr('disabled', 'disabled');
                        $('#plano-id').val(0);
                        $("#plano-search").val('').focus();
                        abp.notify.info(app.localize("PlanoInvalido"));
                        return false;
                    }
                },
            });

        $('#profissao-search')
            .autocomplete({
                minLength: 3,
                delay: 0,
                source: function (request, response) {
                    var term = $('#profissao-search').val();
                    var url = '/mpa/profissoes/autocomplete';
                    var fullUrl = url + '/?term=' + term;
                    $.getJSON(fullUrl, function (data) {
                        if (data.result.length == 0) {
                            $('#profissao-id').val(0);
                            $("#profissao-search").focus();
                            abp.notify.info(app.localize("ListaVazia"));
                            return false;
                        };
                        response($.map(data.result, function (item) {
                            $('#profissao-id').val(0);
                            return {
                                label: item.nome,
                                value: item.nome,
                                realValue: item.id
                            };
                        }));
                    });
                },
                select: function (event, ui) {
                    $('#profissao-id').val(ui.item.realValue);
                    $('#profissao-search').val(ui.item.value);
                    //$('.save-button').removeAttr('disabled');
                    return false;
                },
                change: function (event, ui) {
                    event.preventDefault();
                    if (ui.item == null) {
                        //$('.save-button').attr('disabled', 'disabled');
                        $('#profissao-id').val(0);
                        $("#profissao-search").val('').focus();
                        abp.notify.info(app.localize("ProfissaoInvalida"));
                        return false;
                    }
                }
            });

        $('#naturalidade-search')
            .autocomplete({
                minLength: 3,
                delay: 0,
                source: function (request, response) {
                    var term = $('#naturalidade-search').val();
                    var url = '/mpa/naturalidades/autocomplete';
                    var fullUrl = url + '/?term=' + term;
                    $.getJSON(fullUrl, function (data) {
                        if (data.result.length == 0) {
                            $('#naturalidade-id').val(0);
                            $("#naturalidade-search").focus();
                            abp.notify.info(app.localize("ListaVazia"));
                            return false;
                        };
                        response($.map(data.result, function (item) {
                            $('#naturalidade-id').val(0);
                            return {
                                label: item.cidadeOrigem,
                                value: item.cidadeOrigem,
                                realValue: item.id
                            };
                        }));
                    });
                },
                select: function (event, ui) {
                    $('#naturalidade-id').val(ui.item.realValue);
                    $('#naturalidade-search').val(ui.item.value);
                    //$('.save-button').removeAttr('disabled');
                    return false;
                },
                change: function (event, ui) {
                    event.preventDefault();
                    if (ui.item == null) {
                        //$('.save-button').attr('disabled', 'disabled');
                        $('#naturalidade-id').val(0);
                        $("#naturalidade-search").val('').focus();
                        abp.notify.info(app.localize("NaturalidadeInvalida"));
                        return false;
                    }
                },
            });

        $('#origem-search')
            .autocomplete({
                minLength: 3,
                delay: 0,
                source: function (request, response) {
                    var term = $('#origem-search').val();
                    var url = '/mpa/origens/autocomplete';
                    var fullUrl = url + '/?term=' + term;
                    $.getJSON(fullUrl, function (data) {
                        if (data.result.length == 0) {
                            $('#origem-id').val(0);
                            $("#origem-search").focus();
                            abp.notify.info(app.localize("ListaVazia"));
                            return false;
                        };
                        response($.map(data.result, function (item) {
                            $('#origem-id').val(0);
                            return {
                                label: item.descricao,
                                value: item.descricao,
                                realValue: item.id
                            };
                        }));
                    });
                },
                select: function (event, ui) {
                    $('#origem-id').val(ui.item.realValue);
                    $('#origem-search').val(ui.item.value);
                    //$('.save-button').removeAttr('disabled');
                    return false;
                },
                change: function (event, ui) {
                    event.preventDefault();
                    if (ui.item == null) {
                        //$('.save-button').attr('disabled', 'disabled');
                        $('#origem-id').val(0);
                        $("#origem-search").val('').focus();
                        abp.notify.info(app.localize("OrigemInvalida"));
                        return false;
                    }
                },
            });

        $('#pais-search')
            .autocomplete({
                minLength: 2,
                delay: 0,
                source: function (request, response) {
                    var term = $('#pais-search').val();
                    var url = '/mpa/paises/autocomplete';
                    var fullUrl = url + '/?term=' + term;
                    $.getJSON(fullUrl, function (data) {
                        if (data.result.length == 0) {
                            $('#pais-id').val(0);
                            $("#pais-search").focus();
                            abp.notify.info(app.localize("ListaVazia"));
                            return false;
                        };
                        response($.map(data.result, function (item) {
                            $('#pais-id').val(0);
                            return {
                                label: item.nome,
                                value: item.nome,
                                realValue: item.id
                            };
                        }));
                    });
                },
                select: function (event, ui) {
                    $('#pais-id').val(ui.item.realValue);
                    $('#pais-search').val(ui.item.value);
                    //$('.save-button').removeAttr('disabled');
                    return false;
                },
                change: function (event, ui) {
                    event.preventDefault();
                    if (ui.item == null) {
                        //$('.save-button').attr('disabled', 'disabled');
                        $('#pais-id').val(0);
                        $("#pais-search").val('').focus();
                        abp.notify.info(app.localize("PaisInvalido"));
                        return false;
                    }
                },
            });

        $('#estado-search')
            .autocomplete({
                minLength: 2,
                delay: 0,
                source: function (request, response) {
                    var term = $('#estado-search').val();
                    var url = '/mpa/estados/autocomplete';
                    if ($('#pais-id').length > 0) {
                        term += "&paisId=" + $('#pais-id').val();
                    }

                    var fullUrl = url + '/?term=' + term;
                    $.getJSON(fullUrl, function (data) {
                        if (data.result.length == 0) {
                            $('#estado-id').val(0);
                            $("#estado-search").focus();
                            abp.notify.info(app.localize("ListaVazia"));
                            return false;
                        };
                        response($.map(data.result, function (item) {
                            $('#estado-id').val(0);
                            return {
                                label: item.nome,
                                value: item.nome,
                                realValue: item.id
                            };
                        }));
                    });
                },
                select: function (event, ui) {
                    $('#estado-id').val(ui.item.realValue);
                    $('#estado-search').val(ui.item.value);
                    //$('.save-button').removeAttr('disabled');
                    return false;
                },
                change: function (event, ui) {
                    event.preventDefault();
                    if (ui.item == null) {
                        //$('.save-button').attr('disabled', 'disabled');
                        $('#estado-id').val(0);
                        $("#estado-search").val('').focus();
                        abp.notify.info(app.localize("EstadoInvalido"));
                        return false;
                    }
                },
            });

        $('#cidade-search')
            .autocomplete({
                minLength: 3,
                delay: 0,
                source: function (request, response) {
                    var term = $('#cidade-search').val();
                    var url = '/mpa/cidades/autocomplete';
                    if ($('#estado-id').length > 0) {
                        term += "&estadoId=" + $('#estado-id').val();
                    }
                    var fullUrl = url + '/?term=' + term;

                    $.getJSON(fullUrl, function (data) {
                        if (data.result.length == 0) {
                            $('#cidade-id').val(0);
                            $("#cidade-search").focus();
                            abp.notify.info(app.localize("ListaVazia"));
                            return false;
                        };
                        response($.map(data.result, function (item) {
                            $('#cidade-id').val(0);
                            return {
                                label: item.nome,
                                value: item.nome,
                                realValue: item.id
                            };
                        }));
                    });
                },
                select: function (event, ui) {
                    $('#cidade-id').val(ui.item.realValue);
                    $('#cidade-search').val(ui.item.value);
                    //$('.save-button').removeAttr('disabled');
                    return false;
                },
                change: function (event, ui) {
                    event.preventDefault();
                    if (ui.item == null) {
                        //$('.save-button').attr('disabled', 'disabled');
                        $('#cidade-id').val(0);
                        $("#cidade-search").val('').focus();
                        abp.notify.info(app.localize("CidadeInvalida"));
                        return false;
                    }
                },
            });

    };
})(jQuery);