//const moment = require("moment");


(function ($) {
    $('body').addClass('page-sidebar-closed');
    $('.page-sidebar-menu').addClass('page-sidebar-menu-closed');
    
    var disparoDeMensagemAppService = abp.services.app.disparoDeMensagem;
    var _selectedDateRangeNascimento = {
        autoUpdateInput: false,
        startDate: undefined,
        endDate: undefined,
        "minYear": 1900,
        "minDate": moment().add('-150', 'year').startOf('day'),
        "momentFormatStart": "DD/MM/YYYY",
        "momentFormatEnd": "DD/MM/YYYY"
    };
    var _selectedDataProgramada = {
        minDate: moment().startOf('day'),
        maxDate: moment().add('10', 'year').endOf('day'),
        autoUpdateInput: false,
        startDate: undefined,
        endDate: undefined,
        "singleDatePicker": true,
        "timePicker": true,
        locale: {
            format: 'DD/MM/YYYY hh:mm A'
        },
        ranges: undefined,
        "momentFormatStart": "DD/MM/YYYY HH:mm:ss",
        "momentFormatEnd": "DD/MM/YYYY HH:mm:ss"
    };
    var _selectedDateRangeAtendimento = {
        autoUpdateInput: false,
        startDate: undefined,
        endDate: undefined,
        "momentFormatStart": "DD/MM/YYYY",
        "momentFormatEnd": "DD/MM/YYYY"
    };

    var _selectedDateRangeAtendimentoAlta = {
        autoUpdateInput: false,
        startDate: undefined,
        endDate: undefined,
        "momentFormatStart": "DD/MM/YYYY",
        "momentFormatEnd": "DD/MM/YYYY"
    };

    var summerNoteOptions = {
        toolbar: [
            ['printSize', ['printSize']],
            ['style', ['bold', 'italic', 'underline']],
            ['fontsize', ['fontsize']],
            ['fontname', ['fontname']],
            ['font', ['font', 'strikethrough', 'superscript', 'subscript']],
            ['color', ['color']],
            ['para', ['ul', 'ol', 'paragraph']],
            ['height', ['height']],
            ['misc', ['codeview', 'fullscreen']],
            ['table', ['table']]
        ],
        width: '100%',
        height: 250,
        padding: 15,
        disableResizeEditor: true
    };

    const DisparoDeMensagemItemTipoEmail = 1;
    const DisparoDeMensagemItemTipoWhatsApp = 2;



    var mouseEnter = _.debounce(function (element) {
        var el = $(element);
        el.addClass("active");
        el.find(".close-button").animate({ width: '80px', height: '80px' });
    }, 150);

    var mouseLeave = _.debounce(function (element) {
        var el = $(element);
        el.removeClass("active", function () {
            el.find(".close-button").animate({ width: '0px', height: '80px' });
        });
    }, 150);

    var disparoDeMensagemItems = [],
    disparoDeMensagemItemsFiltrado = [];


    $('.modal-dialog').css({ 'min-width': '95%', 'min-height': '95%' });
    $.summernote.options.lineHeights = ["0", "0.2", "0.4", "0.6", "0.8", "1.0"];
    $('.text-editor').summernote(summerNoteOptions);

    selectSW('.selectPaciente', "/api/services/app/Paciente/ListarDropdown");
    createDateRangePicker($('#date-field-nascimento'), _selectedDateRangeNascimento);

    selectSW('.selectPais', "/api/services/app/Pais/ListarDropdown");
    selectSW('.selectEstado', "/api/services/app/Estado/ListarDropdown", $('.selectPais'));
    selectSW('.selectCidade', "/api/services/app/Cidade/ListarDropdown", $('.selectEstado'));

    selectSW('.selectAtendimento', "/api/services/app/Atendimento/ListarDropdown");
    selectSW('.selectUnidadeOrganizacional', "/api/services/app/UnidadeOrganizacional/ListarDropdownTodosPorUsuario");
    selectSW('.selectStatusAtendimento', "/api/services/app/atendimentoStatus/ListarDropdown");
    selectSW('.selectConvenioAtendimento', "/api/services/app/convenio/ListarDropdown");
    selectSW('.selectPlanoAtendimento', "/api/services/app/plano/ListarDropdown", $('.selectConvenioAtendimento'));
    createDateRangePicker($('#date-field-atendimento'), _selectedDateRangeAtendimento);

    createDateRangePicker($('#date-field-atendimento-alta'), _selectedDateRangeAtendimentoAlta);

    createDateRangePicker($('#date-field-programada'), _selectedDataProgramada);
    createTable($("#disparoDeMensagemTable"));


    $(".sexoMasculino").click(function () {
        if ($(this).prop("checked")) {
            $(".sexoFeminino").prop("checked", null);
        }

        createTable($("#disparoDeMensagemTable"));
    });

    $(".sexoFeminino").click(function () {
        if ($(this).prop("checked")) {
            $(".sexoMasculino").prop("checked", null);
        }
        createTable($("#disparoDeMensagemTable"));
    });

    $("#ultimoAtendimento").click(function () {
        createTable($("#disparoDeMensagemTable"));
    });

    if ($("#disparoDeMensagemId").val()) {
        disparoDeMensagemAppService.obter($("#disparoDeMensagemId").val()).then((res) => {
            $('#date-field-programada').val(moment(res.dataProgramada).format("DD/MM/YYYY HH:mm:ss"));
            _selectedDataProgramada.startDate = moment(res.dataProgramada).format("DD/MM/YYYY HH:mm:ss");
            $('.text-editor').summernote("code", res.mensagem);
            $("#titulo").val(res.titulo);
            disparoDeMensagemItems = res.disparoDeMensagemItems;
            createPagination();
        });
    } else {
        createPagination();
    }

    $(".select2").each(function () {
        if ($(this).data("select2")) {
            $(this).on("change", function () {
                createTable($("#disparoDeMensagemTable"));
            });
        }
    });


    $("#habilitarFiltroAtendimento").click(function () {
        if ($(this).prop("checked")) {
            $("#content-atendimento").show(500);
        }
        else {
            $("#content-atendimento").hide(500);
        }

        createTable($("#disparoDeMensagemTable"));
    });
    $("#filterButton").click(function () {
        createTable($("#disparoDeMensagemTable"));
    });


    $("#ckEnviarEmail").click(function () {
        createPagination();
    });

    $("#ckWhatsApp").click(function () {
        createPagination();
    });

    $(".button-submit").click(function (event) {
        $(this).buttonBusy(true);

        var dataProgramada = null;
        if (_selectedDataProgramada.startDate && _selectedDataProgramada.startDate._isAMomentObject) {
            dataProgramada = _selectedDataProgramada.startDate.format('YYYY-MM-DDTHH:mm:ssZ');
        }
        else {
            dataProgramada = _selectedDataProgramada.startDate;
        }
        var mensagem = $(".text-editor").summernote('code');

        if (_.startsWith(mensagem, "<")) {
            mensagem = $($(".text-editor").summernote('code')).text();
        }

        var model = {
            id: $("#disparoDeMensagemId").val() || 0,
            mensagem,
            titulo: $("#titulo").val(),
            disparoDeMensagemItems: disparoDeMensagemItemsFiltrado,
            dataProgramada: dataProgramada
        };

        if (_.isEmpty(model.dataProgramada) || _.isEmpty(model.mensagem) || _.isEmpty(model.disparoDeMensagemItems)) {
            abp.notify.error(`Preencher os campos data Programada, mensagem e selecionar itens para disparo`);
            $(this).buttonBusy(false);
        }
        else {
            disparoDeMensagemAppService.criarOuEditar(model)
                .then(() => {
                    abp.notify.success(`Disparo de mensagem salvo com sucesso`);
                    $(this).buttonBusy(false);
                    abp.event.trigger('app.CriarOuEditarDisparoDeMensagemModalSaved');
                    window.location = '/Mpa/DisparoDeMensagem/'
                }).fail(function () {
                    $(this).buttonBusy(false);
                });
        }
    });



    function createPagination() {
        console.log(disparoDeMensagemItems);
        var enviarEmail = $("#ckEnviarEmail").prop("checked");
        var enviarWhatsApp = $("#ckWhatsApp").prop("checked");
        disparoDeMensagemItemsFiltrado = _.filter(disparoDeMensagemItems, function (item) {
            if (enviarWhatsApp && item.disparoDeMensagemItemTipoId == DisparoDeMensagemItemTipoWhatsApp) {
                return true;
            }

            if (enviarEmail && item.disparoDeMensagemItemTipoId == DisparoDeMensagemItemTipoEmail) {
                return true;
            }
            return false;
        });

        console.log(disparoDeMensagemItemsFiltrado);

        if ($('.pagination-content').data("pagination")) {
            $('.pagination-content').pagination("destroy");
        }

        $('.pagination-content').pagination({
            ulClassName: "pagination",
            dataSource: disparoDeMensagemItemsFiltrado,
            showNavigator: true,
            showPrevious: true,
            showNext: true,
            inlineStyle: false,
            pageSize: 6,
            callback: function (data, pagination) {
                $(".list-group-item-action").off("mouseenter");
                $(".list-group-item-action").off("mouseleave");
                $(".list-group").empty();
                var content = "";
                _.forEach(data, (item) => {
                    content += modeloListGroup(item);
                });
                $(".list-group").html(content);

                $(".list-group-item-action").on("mouseenter", function (event) {
                    $(".list-group-item-action").not($(this)).each(function () {
                        var el = $(this);
                        el.removeClass("active", function () {
                            el.find(".close-button").animate({ width: '0px', height: '80px' });
                        });
                    });
                    mouseEnter(event.currentTarget);
                });
                $(".list-group-item-action").on("mouseleave", function (event) {
                    mouseLeave(event.currentTarget);
                });

                $(".list-group-item-action .delete-button").click(function () {
                    var modelData = $(this).parents(".list-group-item-action").data("content");
                    modelData = modelData.replace(/&quot/g, '"');
                    if (modelData != null) {
                        modelData = JSON.parse(modelData);
                        if (modelData.id == 0) {
                            disparoDeMensagemItems = _.filter(disparoDeMensagemItems, (x) =>
                                !(x.id == modelData.id
                                    && x.pessoaId == modelData.pessoaId
                                    && x.mensagem == modelData.mensagem
                                    && x.disparoDeMensagemItemTipoId == modelData.disparoDeMensagemItemTipoId
                                    && x.valor == modelData.valor));
                            abp.notify.warn(`${modelData.pessoa.nomeCompleto}  ${modelData.valor} excluído da lista de disparo`);
                            createPagination();
                        }
                        else {
                            disparoDeMensagemAppService.excluirItem(modelData.id).then((res) => {
                                abp.notify.warn(`${modelData.pessoa.nomeCompleto}  ${modelData.valor} excluído da lista de disparo`);
                                createPagination();
                            });
                        }

                    }

                });

            }
        });
    }

    function createDateRangePicker(inputTag, selectedDateRange) {
        var baseOptions = app.createDateRangePickerOptions();
        baseOptions.ranges = undefined;
        var options = $.extend(true, baseOptions, selectedDateRange);
        $(inputTag).daterangepicker(options).on('apply.daterangepicker', function (ev, picker) {
            if (!options["singleDatePicker"]) {
                $(this).val(picker.startDate.format(options["momentFormatStart"]) + ' - ' + picker.endDate.format(options["momentFormatEnd"]));

                selectedDateRange.startDate = picker.startDate.format('YYYY-MM-DDT00:00:00Z');
                selectedDateRange.endDate = picker.endDate.format('YYYY-MM-DDT23:59:59.999Z');
            }
            else {

                $(this).val(picker.startDate.format(options["momentFormatStart"]));
                if (options["timePicker"]) {
                    selectedDateRange.startDate = picker.startDate.format('YYYY-MM-DDTHH:mm:ssZ');
                }
                else {
                    selectedDateRange.startDate = picker.startDate.format('YYYY-MM-DDT00:00:00Z');
                }

            }

            createTable($("#disparoDeMensagemTable"));
        }).on('cancel.daterangepicker', function (ev, picker) {
            $(this).val('');
        });
    }
    function GetFilterModel() {
        return {
            pacienteId: $("#paciente-id").val(),
            nascimentoInicio: !_.isEmpty(_selectedDateRangeNascimento.startDate) ? moment(_selectedDateRangeNascimento.startDate).format("YYYY-MM-DDT00:00:00Z") : null,
            nascimentoFinal: !_.isEmpty(_selectedDateRangeNascimento.endDate) ? moment(_selectedDateRangeNascimento.endDate).format("YYYY-MM-DDT23:59:59.999Z") : null,
            sexoId: $(".sexoMasculino").prop("checked") ? 1 : $(".sexoFeminino").prop("checked") ? 2 : null,
            convenioId: $('.selectConvenioAtendimento').val(),
            planoId: $('.selectPlanoAtendimento').val(),
            paisId: $("#pais-id").val(),
            estadoId: $("#estado-id").val(),
            cidadeId: $("#cidade-id").val(),
            bairro: $("#bairro").val(),
            habilitarFiltroAtendimento: $("#habilitarFiltroAtendimento").prop("checked"),
            naoEnviarPacienteObito: $("#naoEnviarPacienteObito").prop("checked"),
            naoInternado: $("#naoInternado").prop("checked"),
            ultimoAtendimento: $("#ultimoAtendimento").prop("checked"),
            atendimentoInicio: !_.isEmpty(_selectedDateRangeAtendimento.startDate) ? moment(_selectedDateRangeAtendimento.startDate).format("YYYY-MM-DDT00:00:00Z") : null,
            atendimentoFinal: !_.isEmpty(_selectedDateRangeAtendimento.endDate) ? moment(_selectedDateRangeAtendimento.endDate).format("YYYY-MM-DDT23:59:59.999Z") : null,
            atendimentoAltaInicio: !_.isEmpty(_selectedDateRangeAtendimentoAlta.startDate) ? moment(_selectedDateRangeAtendimentoAlta.startDate).format("YYYY-MM-DDT00:00:00Z") : null,
            atendimentoAltaFinal: !_.isEmpty(_selectedDateRangeAtendimentoAlta.endDate) ? moment(_selectedDateRangeAtendimentoAlta.endDate).format("YYYY-MM-DDT23:59:59.999Z") : null,
            unidadeOrganizacionalId: $("#unidadeOrganizacional-id").val(),
            statusAtendimentoId: $("#StatusAtendimento-id").val(),

        };
    }

    function adicionarAction(event, data, type, bulk, displayNotification, result) {
        result = result ?? { totalPessoas: 0, totalWhatsApp: 0, totalEmail: 0 };
        type = type ?? null;
        bulk = bulk ?? false;
        displayNotification = displayNotification ?? true;
        if (!bulk) {
            event.stopPropagation();
            $(event.target).buttonBusy(true);
        }
        var hasError = false;
        var itemsToPush = [];
        var model = {
            id: 0,
            pessoaId: data.id,
            pessoa: {
                id: data.id,
                nomeCompleto: data.nomeCompleto
            },
            mensagem: null
        };

        if (type == DisparoDeMensagemItemTipoEmail || type == null) {
            if (!_.isEmpty(data.email1) || !_.isEmpty(data.email2) || !_.isEmpty(data.email3) || !_.isEmpty(data.email4)) {
                model.disparoDeMensagemItemTipoId = DisparoDeMensagemItemTipoEmail;
                itemsToPush = [data.email1, data.email2, data.email3, data.email4];
                _.forEach(itemsToPush, function (value) {
                    adicionarEachAction(value, model, hasError, displayNotification, result);
                });
                result.totalPessoas += 1;
            }
        }

        if (type == DisparoDeMensagemItemTipoWhatsApp || type == null) {
            if (!_.isEmpty(data.telefone1) || !_.isEmpty(data.telefone2) || !_.isEmpty(data.telefone3) || !_.isEmpty(data.telefone4)) {
                model.disparoDeMensagemItemTipoId = DisparoDeMensagemItemTipoWhatsApp;
                itemsToPush = [data.telefone1, data.telefone2, data.telefone3, data.telefone4];
                _.forEach(itemsToPush, function (value) {
                    adicionarEachAction(value, model, hasError, displayNotification, result);
                });
                result.totalPessoas += 1;
            }
        }

        if (!bulk) {
            abp.notify.success(`Adicionado na lista de disparo`);
            $(event.target).buttonBusy(false);
            createPagination();
        }
        return hasError;
    }

    function adicionarEachAction(item, model, hasError, displayNotification, result) {
        var modelToPush = JSON.parse(JSON.stringify(model));
        if (!_.isEmpty(item)) {
            modelToPush.valor = item;
            if (_.some(disparoDeMensagemItems, checkSome)) {
                if (displayNotification) {
                    if (modelToPush.disparoDeMensagemItemTipoId == DisparoDeMensagemItemTipoEmail) {
                        abp.notify.warn(`E-mail (${modelToPush.pessoa.nomeCompleto}) ${modelToPush.valor} já existe na lista de disparo`);
                    }
                    else if (modelToPush.disparoDeMensagemItemTipoId == DisparoDeMensagemItemTipoWhatsApp) {
                        abp.notify.warn(`Telefone (${modelToPush.pessoa.nomeCompleto}) ${modelToPush.valor} já existe na lista de disparo`);
                    }
                }
                hasError = true;
                return;
            }
            if (modelToPush.disparoDeMensagemItemTipoId == DisparoDeMensagemItemTipoEmail) {
                result.totalEmail += 1;
            }
            else if (modelToPush.disparoDeMensagemItemTipoId == DisparoDeMensagemItemTipoWhatsApp) {
                result.totalWhatsApp += 1;
            }
            result.totalPessoas += 1;
            disparoDeMensagemItems.push(modelToPush);
            hasError = false;
        }
        function checkSome(item) {
            return item.pessoaId == modelToPush.pessoaId && item.mensagem == modelToPush.mensagem
                && item.valor == modelToPush.valor && item.disparoDeMensagemItemTipo == modelToPush.disparoDeMensagemItemTipo;
        }
    }

    function createTable(tableElement) {
        const $span = $('<span  class="button-mass" style="display: flex;min-width: 92px;"></span>')
            .append(`<button class="btn pull-left" style="margin-right:1.5px;padding: 2px 4px; color:#000"  data-type="null" data-toggle="tooltip" title="Enviar Todos"><i class="fa fa-paper-plane" style="margin-top: 3px;font-size: 12px;"></i></button>`)
            .append(`<button class="btn pull-left" style="margin-right:1.5px; background-color:#337ab7;padding: 2px 4px;" data-type="${DisparoDeMensagemItemTipoEmail} data-toggle="tooltip" title="Enviar Email"><i class="fa fa-envelope-open-o" style="color:white;margin-top: 3px;font-size: 12px;"></i></button>`)
            .append(`<button class="btn pull-right" style="margin-left:1.5px; background-color:#009a4f;padding: 2px 4px;" data-toggle="tooltip" data-type="${DisparoDeMensagemItemTipoWhatsApp}" title="Enviar WhatsApp"><i class="fa fa-whatsapp" style="color:white;margin-top: 3px;font-size: 12px;"></i></button>`);
        const $spanSelectAll = $($span).append($(`<div></div>`))[0].outerHTML;
        console.log($spanSelectAll);
        const fields = {
            id: {
                key: true,
                list: false
            },
            actions: {
                title: $spanSelectAll,
                width: '5%',
                sorting: false,
                display: function (data) {
                    var $span = $('<span style="display: inline-block;min-width: 92px;"></span>');

                    if ((!_.isEmpty(data.record.email1) || !_.isEmpty(data.record.email2) || !_.isEmpty(data.record.email3) || !_.isEmpty(data.record.email4))
                        && (!_.isEmpty(data.record.telefone1) || !_.isEmpty(data.record.telefone2) || !_.isEmpty(data.record.telefone3) || !_.isEmpty(data.record.telefone4))) {
                        $span
                            .append(`<button type="button" class="btn pull-left" data-type="null" style="margin-right:1.5px;padding: 2px 4px;" data-toggle="tooltip" title="Enviar Todos"><i class="fa fa-paper-plane" style="margin-top: 3px;font-size: 12px;"></i></button>`)
                    }

                    if (!_.isEmpty(data.record.email1) || !_.isEmpty(data.record.email2) || !_.isEmpty(data.record.email3) || !_.isEmpty(data.record.email4)) {
                        $span
                            .append(`<button class="btn pull-left"  data-type="${DisparoDeMensagemItemTipoEmail}" style="margin-right:1.5px; background-color:#337ab7;padding: 2px 4px;" data-toggle="tooltip" title="Enviar Email"><i class="fa fa-envelope-open-o" style="color:white;margin-top: 3px;font-size: 12px;"></i></button>`)

                    }

                    if (!_.isEmpty(data.record.telefone1) || !_.isEmpty(data.record.telefone2) || !_.isEmpty(data.record.telefone3) || !_.isEmpty(data.record.telefone4)) {
                        $span
                            .append(`<button class="btn pull-right"  data-type="${DisparoDeMensagemItemTipoWhatsApp}" style="margin-left:1.5px; background-color:#009a4f;padding: 2px 4px;" data-toggle="tooltip" title="Enviar WhatsApp"><i class="fa fa-whatsapp" style="color:white;margin-top: 3px;font-size: 12px;"></i></button>`)

                    }
                    $span.find("button").click(function (event) {
                        return adicionarAction(event, data.record, $(this).data("type"), false, true);
                    });
                    return $span;
                }
            },
            nomeCompleto: {
                title: app.localize('Paciente'),
                width: '20%',
                display: function (data) {
                    return $("<div></div>").append(`<span style="font-weight:600"> ${data.record.nomeCompleto}</span>`);
                }
            },
            nascimento: {
                title: app.localize('Data Nascimento'),
                width: '8%',
                display: function (data) {
                    if (data.record.nascimento) {
                        var $span = $(`<div style="min-width: 110px;"></div>`);
                        $span.append(`<span class="pull-left"> ${moment(data.record.nascimento).format("DD/MM/YYYY")}</span>`);
                        $span.append(`<span class="pull-right" style="font-weight:600"> ${data.record.idade || ''}</span>`);
                        return $span;
                    }
                }
            },
            telefone1: {
                title: app.localize('Telefone 1'),
                width: '5.5%',
                display: function (data) {
                    return $(`<span class="pull-left text-primary" style="min-width: 85px;" > ${data.record.telefone1 || ''}</span>`);
                }
            },
            telefone2: {
                title: app.localize('Telefone 2'),
                width: '5.5%',
                display: function (data) {
                    return $(`<span class="pull-left text-info" style="min-width: 85px;" > ${data.record.telefone2 || ''}</span>`);
                }
            },
            telefone3: {
                title: app.localize('Telefone 3'),
                width: '5.5%',
                display: function (data) {
                    return $(`<span class="pull-left text-warning" style="min-width: 85px;" > ${data.record.telefone3 || ''}</span>`);
                }
            },
            telefone4: {
                title: app.localize('Telefone 4'),
                width: '5.5%',
                display: function (data) {
                    return $(`<span class="pull-left" style="min-width: 85px;" > ${data.record.telefone4 || ''}</span>`);
                }
            },
            Email1: {
                title: app.localize('Email 1'),
                width: '9%',
                display: function (data) {
                    return $(`<span class="pull-left text-primary" > ${data.record.email1 || ''}</span>`);
                }
            },
            Email2: {
                title: app.localize('Email 2'),
                width: '9%',
                display: function (data) {
                    return $(`<span class="pull-left text-info" > ${data.record.email2 || ''}</span>`);
                }
            },
            Email3: {
                title: app.localize('Email 3'),
                width: '9%',
                display: function (data) {
                    return $(`<span class="pull-left text-warning" > ${data.record.email3 || ''}</span>`);
                }
            },
            Email4: {
                title: app.localize('Email 4'),
                width: '9%',
                display: function (data) {
                    return $(`<span class="pull-left" > ${data.record.email4 || ''}</span>`);
                }
            }
        };

        const model = GetFilterModel();
        if (!_.isEmpty(model.paisId) || !_.isEmpty(model.estadoId) || !_.isEmpty(model.cidadeId) || !_.isEmpty(model.bairro)) {
            fields["pais"] = {
                title: app.localize('Pais'),
                width: '6%',
                display: function (data) {
                    return data.record.pais;
                }
            };
            fields["estado"] = {
                title: app.localize('Estado'),
                width: '6%',
                display: function (data) {
                    return data.record.estado;
                }
            };
            fields["cidade"] = {
                title: app.localize('Cidade'),
                width: '6%',
                display: function (data) {
                    return data.record.cidade;
                }
            };
            fields["bairro"] = {
                title: app.localize('Bairro'),
                width: '6%',
                display: function (data) {
                    return data.record.bairro;
                }
            };
        }

        if (model.habilitarFiltroAtendimento) {
            fields["atendimento"] = {
                title: app.localize('Atendimento'),
                width: '6%',
                display: function (data) {
                    return data.record.atendimento;
                }
            };
            fields["dataAtendimento"] = {
                title: app.localize('Data Atendimento'),
                width: '6%',
                display: function (data) {
                    if (data.record.dataAtendimento) {
                        return moment(data.record.dataAtendimento).format("DD/MM/YYYY hh:mm:ss");
                    }
                }
            };
            fields["dataAltaAtendimento"] = {
                title: app.localize('Data Alta Atendimento'),
                width: '6%',
                display: function (data) {
                    if (data.record.dataAltaAtendimento) {
                        return moment(data.record.dataAltaAtendimento).format("DD/MM/YYYY hh:mm:ss");
                    }
                }
            };
            fields["unidade"] = {
                title: app.localize('Unidade'),
                width: '6%',
                display: function (data) {
                    return data.record.unidade;
                }
            };
            fields["statusAtendimento"] = {
                title: app.localize('Status Atendimento'),
                width: '6%',
                display: function (data) {
                    return data.record.statusAtendimento;
                }
            };

        }

        if (tableElement.data("hikJtable")) {
            tableElement.jtable("destroy");
        }

        tableElement.jtable({
            title: app.localize('Listagem'),
            paging: true,
            sorting: true,
            multiSorting: true,
            actions: {
                listAction: {
                    method: disparoDeMensagemAppService.listarParaDisparo
                }
            },
            saveUserPreferences: false,
            fields: fields
        });

        tableElement.jtable("load", model);

        $(".jtable-column-header-text .button-mass button").click(function () {
            console.log($(this).data());
            var filterModel = GetFilterModel();
            filterModel.AllRows = true;
            var button = $(this);
            button.buttonBusy(true);
            disparoDeMensagemAppService.listarParaDisparo(filterModel).then(res => {
                debugger;
                console.log(res);

                var result = { totalPessoas: 0, totalWhatsApp: 0, totalEmail: 0 };
                var type = $(this).data("type");
                _.forEach(res.items, (data) => {
                    adicionarAction(event, data, type, true, false, result);
                });

                message = `Total de pacientes adicionados: <b>${result.totalPessoas ?? 0}</b>`;

                if (type == null) {
                    message += `<br/>Total de Telefones adicionados: <b>${result.totalWhatsApp?? 0}</b>`;
                    message += `<br/>Total de E-mails adicionados: <b>${result.totalEmail?? 0}</b>`;
                }
                else if (type == DisparoDeMensagemItemTipoEmail) {
                    message += `<br/>Total de E-mails adicionados: <b>${result.totalEmail ?? 0}</b>`;
                }
                else if (type == DisparoDeMensagemItemTipoWhatsApp) {
                    message += `<br/>Total de Telefones adicionados: <b>${result.totalWhatsApp ?? 0}</b>`;
                }

                abp.notify.success(message);

                button.buttonBusy(false);

                createPagination();
            }).fail(() => {
                button.buttonBusy(false);
            });


        });
    }

    function escapeDoubleQuotes(str) {
        return str.replace(/\\([\s\S])|(")/g, '&quot');
    }

    function modeloListGroup(data) {
        if (_.isEmpty(data)) {
            return "";
        }

        return `<a href="#" class="list-group-item list-group-item-action flex-column align-items-start" style="height: 80px; margin:3px 0px;" data-content="${escapeDoubleQuotes(JSON.stringify(data))}" >
            <div class="d-flex">
                <div style="width:calc(100% - 80px)" class="col-md-1">
                    <div class="d-flex w-100 justify-content-between">
                        <h5 style="margin:0px;font-weight:600;margin-left: -15px;">${data.pessoa.nomeCompleto}</h5>
                    </div>
                    <div class="row" style="margin-top: 7px;margin-bottom: 7px">
                        <label class="col-md-12">
                            ${modeloTipo(data.disparoDeMensagemItemTipoId)} ${data.valor}
                        </label>
                    </div>
                </div>
                <button type="button" class="col-md-1 pull-right btn default close-button delete-button">
                    <i class="col-md-12 fa fa-trash fa-2x icon"></i>
                    <span class="col-md-12 message" style="font-size: 10px;padding-top: 5px;">Excluir</span>
                </button>
            </div>
        </a>`;
    };

    function modeloTipo(disparoDeMensagemItemTipo) {
        if (disparoDeMensagemItemTipo == DisparoDeMensagemItemTipoEmail) {
            return `<i class="fa fa-envelope-open-o fa-2x" style="padding-right: 5px; color: #337ab7"></i>`;
        }
        if (disparoDeMensagemItemTipo == DisparoDeMensagemItemTipoWhatsApp) {
            return `<i class="fa fa-whatsapp fa-2x" style="padding-right: 5px;color: #009a4f"></i>`;
        }
    };
})(jQuery);