

//CriarOuEditarResultadoModal

(function ($) {
    app.modals.CriarOuEditarResultadoModal = function () {

        var _resultadosService = abp.services.app.resultado;
        var _$examesTable = $('#ExamesTable');
        var _examesService = abp.services.app.resultadoExame;
        var _materialService = abp.services.app.material;
        var _exameService = abp.services.app.exame;
        var _$examesFilterForm = $('#ExamesFilterForm');
        var _faturamentoItemService = abp.services.app.faturamentoItem;
        var _atendimentosService = abp.services.app.atendimento;
        var _medicoService = abp.services.app.medico;

        var _modalManager;
        var _$ResultadoInformationForm = null;
        var _$ResultadoExameInformationForm = null;

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.ResultadoExame.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.ResultadoExame.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.ResultadoExame.Delete')
        };

        this.init = function(modalManager) {
            _modalManager = modalManager;
            CamposRequeridos();
            $('.modal-dialog').css('width', '100%');
            //  $('.modal-dialog').css({ 'width': '90%', 'max-width': '1100px' });
            _$ResultadoInformationForm = $('form[name=ResultadoInformationsForm]');
            _$ResultadoExameInformationForm = $('form[name=ResultadoExameInformationsForm]');
            _$ResultadoInformationForm.validate();
            _$ResultadoExameInformationForm.validate();
            $('.select2').css('width', '100%');
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
            $('#quantidade').val('1');
            getExames();
            //$('#omitir-sw-div-retratil-exames-table').trigger('click');

        };

        $('.save-button').on('click', function (e) {
            e.preventDefault();
            if (!_$ResultadoInformationForm.valid()) {
                return;
            }
            var resultado = _$ResultadoInformationForm.serializeFormToObject();
            resultado.ResultadosExamesList = $('#resultados-exames-list').val();

            //  resultado.IsRn = $('#isRn')[0].check;//   .swChkValor();
            resultado.IsEmail = $('#isEmail').swChkValor();
            resultado.IsEmergencia = $('#isEmergencia').swChkValor();
            //resultado.IsUrgente = $('#isUrgente').swChkValor();
            resultado.IsAvisoLab = $('#isAvisoLab').swChkValor();
            resultado.IsAvisoMed = $('#isAvisoMed').swChkValor();
            resultado.IsVisualiza = $('#isVisualiza').swChkValor();
            //resultado.IsRotina = $('#isRotina').swChkValor();
            resultado.IsTransferencia = $('#isTransferencia').swChkValor();
            resultado.IsCiente = $('#isCiente').swChkValor();

            abp.ui.setBusy();

            _resultadosService.criarOuEditar(resultado)
                .done(function () {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    //_modalManager.close();
                    abp.event.trigger('app.CriarOuEditarResultadoModalSaved');
                    abp.event.trigger('app.atualizaColeta', {resultadoId: resultado.Id})
                    //location.href = '/mpa/resultados';
                    _modalManager.close();
                })
                .always(function () {
                    abp.ui.clearBusy();
                });
        });

        CamposRequeridos();
        aplicarDateSingle();

        $("#tecnico-id").select2({
            allowClear: true,
            placeholder: app.localize("SelecioneLista"),
            ajax: {
                url: "/api/services/app/tecnico/ListarDropdown",
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

        $("#responsavel-id").select2({
            allowClear: true,
            placeholder: app.localize("SelecioneLista"),
            ajax: {
                url: "/api/services/app/tecnico/ListarDropdown",
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

        $("#tecnico-coleta-id").select2({
            allowClear: true,
            placeholder: app.localize("SelecioneLista"),
            ajax: {
                url: "/api/services/app/tecnico/ListarDropdown",
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

        $("#usuario-conferido-id").select2({
            allowClear: true,
            placeholder: app.localize("SelecioneLista"),
            ajax: {
                url: "/api/services/app/user/ListarDropdown",
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

        $("#usuario-digitado-id").select2({
            allowClear: true,
            placeholder: app.localize("SelecioneLista"),
            ajax: {
                url: "/api/services/app/user/ListarDropdown",
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

        $("#usuario-entrega-id").select2({
            allowClear: true,
            placeholder: app.localize("SelecioneLista"),
            ajax: {
                url: "/api/services/app/user/ListarDropdown",
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

        $("#usuario-ciente-id").select2({
            allowClear: true,
            placeholder: app.localize("SelecioneLista"),
            ajax: {
                url: "/api/services/app/user/ListarDropdown",
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

        $("#leito-atual-id").select2({
            allowClear: true,
            placeholder: app.localize("SelecioneLista"),
            ajax: {
                url: "/api/services/app/leito/ListarDropdown",
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

        $("#local-atual-id").select2({
            allowClear: true,
            placeholder: app.localize("SelecioneLista"),
            ajax: {
                url: "/api/services/app/unidadeOrganizacional/ListarDropdown",
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

        $("#exame-id").select2({
            allowClear: true,
            placeholder: app.localize("SelecioneLista"),
            ajax: {
                url: "/api/services/app/exame/ListarDropdown",
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
            $('#faturamento-item-id').val($(this).val());
            $('#exame').val($(this).text());
        });

        $("#faturamento-conta-item-id").select2({
            allowClear: true,
            placeholder: app.localize("SelecioneLista"),
            ajax: {
                url: "/api/services/app/faturamento-conta-item/ListarDropdown",
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
            $('#faturamento-conta-item').val($(this).text());
        });

        $("#material-id").select2({
            allowClear: true,
            placeholder: app.localize("SelecioneLista"),
            ajax: {
                url: "/api/services/app/material/ListarDropdown",
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
            $('#material').val($(this).text());
        });

        $("#resultado-status-id").select2({
            allowClear: true,
            placeholder: app.localize("SelecioneLista"),
            ajax: {
                url: "/api/services/app/resultadostatus/ListarDropdown",
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

        $("#autorizacao-procedimento-id").select2({
            allowClear: true,
            placeholder: app.localize("SelecioneLista"),
            ajax: {
                url: "/api/services/app/autorizacaoprocedimento/ListarDropdown",
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

        $("#prioridade-id").on('change', function () {
            var option = $(this).val();
            if (option == 1) {
                $('#is-urgente').val(true);
                $('#is-rotina').val(false);
            }
            else if (option == 2) {
                $('#is-urgente').val(false);
                $('#is-rotina').val(true);
            }
            else {
                abp.notify.error(app.localize('SelecionarLista'));
                return;
            }
        });
        var format =
            moment.locale().toUpperCase() === 'PT-BR'
                ? "DD/MM/YYYY hh:mm"
                : moment.locale().toUpperCase() === 'US'
                    ? "MM/DD/YYYY hh:mm"
                    : "YYYY-MM-DD hh:mm";
        $('#data-coleta').daterangepicker({
            "singleDatePicker": true,
            "showDropdowns": true,
            "timePicker": true,
            "startDate": $('#data-coleta').val() ? moment($('#data-coleta').val(), format) : moment(),
            //autoUpdateInput: false,
            //maxDate: new Date(),
            changeYear: true,
            //yearRange: 'c-10:c+10',
            showOn: "both",
            "locale": {
                "format": format,
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
            function (start, end, label) {
                console.log(start, end, label);
                $('#data-coleta').val(moment(end).format('L LT'));
                $('#data-coleta').trigger('input');
                $('#data-coleta').trigger('change');
            });

        $('#is-rn').on('click', function () {
            if ($(this).is(':checked')) {
                $('#gemelar-area').show();
            }
            else {
                $('#gemelar-area').hide();
                $('#gemelar').val('');
            }
        });

        $('.close-button').on('click', function (e) {
            e.preventDefault();
            location.href = '/mpa/resultados';
        });

        if ($('#is-rn').is(':checked')) {
            $('#gemelar-area').show();
        }
        else {
            $('#gemelar-area').hide();
            $('#gemelar').val('');
        }

        /*******************************/
        /*       Resultado Exame       */
        /*******************************/

        _$examesTable.jtable({
            title: app.localize('Exames'),
            paging: true,
            sorting: true,
            multiSorting: true,

            rowUpdated: function (event, data) {
                if (data) {
                    if (data.record.cor) {
                        data.row[0].cells[0].setAttribute('bgcolor', data.record.cor);
                        data.row[0].cells[0].setAttribute('color', data.record.cor);
                    }
                }
            },

            rowInserted: function (event, data) {


                if (data) {
                    if (data.record.cor) {
                        data.row[0].cells[0].setAttribute('bgcolor', data.record.cor);
                        data.row[0].cells[0].setAttribute('color', data.record.cor);
                    }
                }
            },

            actions: {
                listAction: {
                    method: retornarLista
                }
            },
            fields: {
                id: {
                    key: true,
                    list: false
                },
                statusExame: {
                    title: app.localize('Status'),
                    width: '15px',
                    display: function (data) {
                        var $span = $('<div  title="' + data.record.exameStatus + '"></div>');
                        $('<span class="sw-btn-display" style="background-color:' + data.record.exameStatusCor + ';"></span>')
                            .appendTo($span);
                        return $span;

                        //return data.record.statusExame;
                    }
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '10%',
                    sorting: false,
                    display: function (data) {
                        var $span = $('<span></span>');
                        if (_permissions.edit && data.record.exameStatusId != 4) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                .appendTo($span)
                                .click(function (e) {
                                    e.preventDefault();
                                    //_createOrEditResultadoExameModal.open({ id: data.record.id });
                                    $('#exibir-sw-div-retratil-exames-table').click();
                                    $('.legendform span:first-child').html(app.localize('Edit') + ' ' + app.localize('ResultadoExame'));
                                    editarRegistroExame(data.record.id, data.record.idGridResultadoExame);
                                });
                        }
                        if (_permissions.delete && data.record.exameStatusId != 4) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                .appendTo($span)
                                .click(function (e) {
                                    e.preventDefault();
                                    deleteResultados(data.record);
                                });
                        }
                        return $span;
                    }
                },
                exame: {
                    title: app.localize('Exame'),
                    width: '65%'
                },
                material: {
                    title: app.localize('Material'),
                    width: '10%'
                },
                quantidade: {
                    title: app.localize('Quantidade'),
                    width: '10%'
                },
                isSigiloso: {
                    title: app.localize('Sigiloso'),
                    width: '6%',
                    display: function (data) {
                        if (data.record.isSigiloso) {
                            return '<span class="label label-success">' + app.localize('Yes') + '</span>';
                        } else {
                            return '<span class="label label-default">' + app.localize('No') + '</span>';
                        }
                    }
                },
            },
        });

        function getExames() {


            _$examesTable.jtable('load', {

                //  filtro: _$examesFilterForm.val(),
                //  id: $('#resultado-id-exame').val()
            });
        }

        function novoRegistroExame() {
            $('#exame-id').val(null).change();
            $('#faturamento-conta-item-id').val(null).change();
            $('#faturamento-item-id').val('');
            $('#material-id').val(null).change();
            $('#quantidade').val('1');
            $('#motivo-pendente-exame').val('');
            $('#observacao-exame').val('');
            $('#is-digitado').removeAttr('checked');
            $('#is-conferido').removeAttr('checked');
            $('#is-pendente').removeAttr('checked');
            $('#is-sigiloso').removeAttr('checked');
            $('#id-registro-exame').val(0);
            $('#idGridResultadoExame').val('');
            $('#exame-status-id').val('1');

            $('#salvar-resultado-exame i').removeClass('fa-check').addClass('fa-plus');
        }

        function editarRegistroExame(id, idGrid) {
            abp.ui.setBusy();
            var list = JSON.parse($('#resultados-exames-list').val());

            for (var i = 0; i < list.length; i++) {
                if (list[i].IdGridResultadoExame == idGrid) {
                    var data = list[i];
                    break;
                }
            }

            if (data.FaturamentoItemId != null) {
                _faturamentoItemService.obter(data.FaturamentoItemId).done(function (result) {
                    $('#exame-id')
                        .append($("<option>") //add option tag in select
                            .val(data.FaturamentoItemId) //set value for option to post it
                            .text(result.codigo + ' - ' + result.descricao)
                        ) //set a text for show in select
                        .val(data.FaturamentoItemId) //select option of select2
                        .trigger("change");
                    $('#faturamento-item-id').val(data.FaturamentoItemId);

                    //.append($('<option value="' + data.ExameId + '">' + result.codigo + ' - ' + result.descricao + '</option>'))
                    //.val(data.ExameId)
                    //.trigger('change');
                });
            }
            else {
                $('#exame-id').val(null).change();
                $('#faturamento-item-id').val('');
            }
            if (data.FaturamentoContaId != null) {
                _faturamentoContaItemService.obter(data.FaturamentoContaId).done(function (result) {
                    $('#faturamento-conta-item-id')
                        .append($('<option value="' + data.FaturamentoContaId + '">' + result.codigo + ' - ' + result.descricao + '</option>'))
                        .val(data.FaturamentoContaId)
                        .trigger('change');
                });
            }
            else {
                $('#faturamento-conta-item-id').val(null).change();
            }
            if (data.MaterialId != null) {
                _materialService.obter(data.MaterialId).done(function (result) {
                    $('#material-id')
                        .append($('<option value="' + data.MaterialId + '">' + result.codigo + ' - ' + result.descricao + '</option>'))
                        .val(data.MaterialId)
                        .trigger('change');
                });
            }
            else {
                $('#material-id').val(null).change();
            }
            $('#quantidade').val(data.Quantidade);
            $('#motivo-pendente-exame').val(data.MotivoPendenteExame);
            $('#observacao-exame').val(data.ObservacaoExame);
            $('#exame-status-id').val(data.ExameStatusId);
            //if (data.IsDigitado) {
            //    $('#is-digitado').attr('checked');
            //} else {
            //    $('#is-digitado').removeAttr('checked');
            //}
            //if (data.IsConferido) {
            //    $('#is-conferido').attr('checked');
            //} else {
            //    $('#is-conferido').removeAttr('checked');
            //}
            //if (data.IsPendente) {
            //    $('#is-pendente').attr('checked');
            //} else {
            //    $('#is-pendente').removeAttr('checked');
            //}

            if (data.IsSigiloso) {
                $('#is-sigiloso').prop('checked', true);
            } else {
                $('#is-sigiloso').removeAttr('checked');
            }
            $('#id-registro-exame').val(data.Id);
            $('#idGridResultadoExame').val(data.IdGridResultadoExame);
            $('#salvar-resultado-exame i').removeClass('fa-plus').addClass('fa-check');

            abp.ui.clearBusy();
        }

        function retornarLista() {

            var list = $('#resultados-exames-list').val();
            var res = _examesService.listarJson(JSON.parse(list), localStorage['DivisaoId']);
            return res;

        }

        function deleteExames(exame) {
            abp.message.confirm(
                app.localize('DeleteWarning', Resultado.Nome),
                function (isConfirmed) {
                    var lista = JSON.parse($('#resultados-exames-list').val());
                    for (var i = 0; i < lista.length; i++) {
                        if (lista[i].IdGridPrescricaoItemResposta == exame.idGridPrescricaoItemResposta) {
                            if (lista[i].IsDeleted) {
                                lista[i].IsDeleted = false;
                                lista[i].DeleterUserId = '';
                            }
                            else {
                                lista[i].IsDeleted = true;
                                lista[i].DeleterUserId = abp.session.userId;
                            }
                            break;
                        }
                    }
                    $('#resultados-exames-list').val(JSON.stringify(lista));
                    abp.notify.info(app.localize('ListaAtualizada'));
                    abp.event.trigger('app.CriarOuEditarResultadoExameModalSaved');
                    getExames();
                }
            );
        }

        $('#salvar-resultado-exame').on('click', function (e) {
            e.preventDefault();

            var resultadoForm = _$ResultadoInformationForm;
            var resultadoExameForm = _$ResultadoExameInformationForm;
            var list = $('#resultados-exames-list').val();
            resultadoExameForm.validate();

            if (!resultadoExameForm.valid()) {
                return;
            }

            resultadoForm.serializeFormToObject();
            var form1 = resultadoExameForm.serializeFormToObject();
            if (list != '') {
                var lista = JSON.parse(list);
            }
            else {
                var lista = [];
            }

            if (lista.length > 0) {
                var itemProcessado = false;
                for (var i = 0; i < lista.length; i++) {

                    //exemplo
                    //if (lista[i].IdGrid == $('#idGridLancamento').val()) {

                    if (lista[i].IdGridResultadoExame == form1.IdGridResultadoExame) {
                        lista[i].Quantidade = form1.Quantidade;
                        lista[i].ExameId = form1.ExameId;

                        if ($('#exame-id').val() != '' && $('#exame-id').val() != null) {

                            var exameCampo = $('#exame-id').select2('data');
                            if (exameCampo && exameCampo.length > 0) {
                                lista[i].Exame = exameCampo[0].text;
                                lista[i].FaturamentoItemId = exameCampo[0].id;
                            }
                        }

                        if ($('#exameSolicitados-id').val() != '' && $('#exameSolicitados-id').val() != null) {

                            var exameCampo = $('#exameSolicitados-id').select2('data');
                            if (exameCampo && exameCampo.length > 0) {

                                lista[i].Exame = exameCampo[0].text;
                                lista[i].FaturamentoItemId = exameCampo[0].id;
                            }
                        }

                        lista[i].FaturamentoContaItemId = form1.FaturamentoContaItemId;
                        lista[i].FaturamentoContaItem = form1.FaturamentoContaItem;
                        lista[i].DataDigitado = form1.DataDigitado;
                        lista[i].UsuarioDigitadoId = form1.UsuariodigitadoId;
                        lista[i].DataConferido = form1.DataConferido;
                        lista[i].UsuarioConferidoId = form1.UsuarioConferidoId;
                        lista[i].DataPendente = form1.DataPendente;
                        lista[i].UsuarioPendenteId = form1.UsuarioPendenteId;
                        lista[i].MotivoPendenteExame = form1.MotivoPendenteExame;
                        lista[i].MaterialId = form1.MaterialId;

                        var materialCampo = $('#material-id').select2('data');
                        if (materialCampo && materialCampo.length > 0) {

                            lista[i].Material = materialCampo[0].text;
                        }

                        //lista[i].Material = form1.Material;

                        lista[i].IsSigiloso = form1.IsSigiloso;
                        lista[i].Observacao = form1.Observacao;
                        lista[i].IdGridResultadoExame = form1.IdGridResultadoExame;
                        itemProcessado = true;
                        break;
                    }
                }
                if (!itemProcessado) {
                    form1.IdGridResultadoExame = lista.length == 0 ? 1 : lista[lista.length - 1].IdGridResultadoExame + 1;
                    form1.ResultadoId = $('#id').val();

                    if ($('#exame-id').val() != '' && $('#exame-id').val() != null) {

                        var exameCampo = $('#exame-id').select2('data');
                        if (exameCampo && exameCampo.length > 0) {

                            form1.Exame = exameCampo[0].text;
                            form1.FaturamentoItemId = exameCampo[0].id;
                        }
                    }

                    if ($('#exameSolicitados-id').val() != '' && $('#exameSolicitados-id').val() != null) {

                        var exameCampo = $('#exameSolicitados-id').select2('data');
                        if (exameCampo && exameCampo.length > 0) {

                            form1.Exame = exameCampo[0].text;
                            form1.FaturamentoItemId = exameCampo[0].id;
                        }
                    }

                    var materialCampo = $('#material-id').select2('data');
                    if (materialCampo && materialCampo.length > 0) {

                        form1.Material = materialCampo[0].text;

                    }

                    lista.push(form1);
                }
            }
            else {
                form1.IdGridResultadoExame = lista.length == 0 ? 1 : lista[lista.length - 1].IdGridResultadoExame + 1;
                form1.ResultadoId = $('#id').val();


                if ($('#exame-id').val() != '' && $('#exame-id').val() != null) {

                    var exameCampo = $('#exame-id').select2('data');
                    if (exameCampo && exameCampo.length > 0) {

                        form1.Exame = exameCampo[0].text;
                        form1.FaturamentoItemId = exameCampo[0].id;
                    }
                }

                if ($('#exameSolicitados-id').val() != '' && $('#exameSolicitados-id').val() != null) {

                    var exameCampo = $('#exameSolicitados-id').select2('data');
                    if (exameCampo && exameCampo.length > 0) {

                        form1.Exame = exameCampo[0].text;
                        form1.FaturamentoItemId = exameCampo[0].id;
                    }
                }




                var materialCampo = $('#material-id').select2('data');
                if (materialCampo && materialCampo.length > 0) {

                    form1.Material = materialCampo[0].text;
                }


                lista.push(form1);
            }
            $('#resultados-exames-list').val(JSON.stringify(lista));
            abp.notify.info(app.localize('ListaAtualizada'));
            abp.event.trigger('app.CriarOuEditarResultadoExameModalSaved');
            getExames();
            novoRegistroExame();
        });

        $('#omitir-sw-div-retratil-exames-table').on('click', function () {
            $('.legendform span:first-child').html(app.localize('CadastroResultadoExames'));
            novoRegistroExame();
        });

        $('#CreateNewResultadoExameButton').click(function (e) {
            e.preventDefault();
            //_createOrEditResultadoExameModal.open({ resultadoId: $('#resultado-id-exame').val() });
            $('#exibir-sw-div-retratil-exames-table').click();
            $('.legendform span:first-child').html(app.localize('CreateNew') + ' ' + app.localize('ResultadoExame'));
            novoRegistroExame();
        });

        $('#GetExamesButton').click(function (e) {
            e.preventDefault();
            getExames();
        });

        $('#RefreshExamesButton').click(function (e) {
            e.preventDefault();
            getExames();
        });

        abp.event.on('app.CriarOuEditarResultadoExameModalSaved', function () {
            getExames(true);
        });

        //$('body').addClass('page-sidebar-closed');

        //$('.page-sidebar-menu').addClass('page-sidebar-menu-closed');


        $('#atendimentoId').on('change', function (e) {
            e.preventDefault();

            _atendimentosService.obter($('#atendimentoId').val())
                .done(function (result) {

                    if (result.convenio) {


                        $('#convenioId')
                            .append($('<option value="' + result.convenio.id + '">' + result.convenio.nomeFantasia + '</option>'))
                            .val(result.convenio.id)
                            .trigger('change');
                    }



                    if (result.leito) {
                        $('#leito-atual-id')
                            .append($('<option value="' + result.leito.id + '">' + result.leito.codigo + ' - ' + result.leito.descricao + '</option>'))
                            .val(result.leito.id)
                            .trigger('change');
                    }

                });



            $('#atendimento-id').val($('#atendimentoId').val())

            selectSWMultiplosFiltros('.selectExamesSolicitados', "/api/services/app/SolicitacaoExameItem/ListarDropdownExameLaboratorioNaoRegistradoPorAtendimento", ['atendimento-id']);


        });


        $('#exame-id').on('change', function (e) {
            e.preventDefault();




            if ($('#exame-id').val() != null && $('#exame-id').val() != '') {
                carregarDadosExame($('#exame-id').val());
            }


            if ($('#exameSolicitados-id').val() != '' && $('#exameSolicitados-id').val() != null) {

                $('#exameSolicitados-id')
                    .append($('<option value="">' + '</option>'))
                    .text('')
                    .val('')
                //.trigger('change');

            }

        });


        $('#exameSolicitados-id').on('change', function (e) {
            e.preventDefault();
            // $('#exame-id').val('');


            if ($('#exameSolicitados-id').val() != null && $('#exameSolicitados-id').val() != '') {
                carregarDadosExame($('#exameSolicitados-id').val());
            }



            if ($('#exame-id').val() != '' && $('#exame-id').val() != null) {
                $('#exame-id')
                    .append($('<option value="">' + '</option>'))
                    .text('')
                    .val('')
                //.trigger('change');
            }
        });


        $('#medicoSolicitanteId').on('change', function (e) {
            e.preventDefault();


            if ($('#medicoSolicitanteId').val() != null && $('#medicoSolicitanteId').val() != '') {
                _medicoService.obter($('#medicoSolicitanteId').val())
                    .done(function (result) {
                        if (result != null) {
                            $('#nomeMedicoSolicitante').val(result.nomeCompleto);
                            $('#CRMSolicitante').val(result.numeroConselho);
                        }
                    });
            }
        });

        function carregarDadosExame(exameId) {
            _faturamentoItemService.obter(exameId)
                .done(function (result) {
                    if (result != null) {
                        $('#quantidade').val(result.qtdFatura);


                        if (result.material != null) {



                            var material = result.material.descricao;


                            $('#material-id').append($("<option>")
                                .val(result.materialId)
                                .text(material)
                            )
                                .val(result.materialId)
                                .trigger("change");
                        }
                        else {
                            $('#material-id')
                                .append($('<option value="">' + '</option>'))
                                .text('')
                                .val('')
                        }

                    }
                });
        }



        selectSW('.selectExame', "/api/services/app/faturamentoItem/ListarExameLaboratorialDropdown");
        //selectSW('.selectAtendimento', "/api/services/app/Atendimento/ListarAtendimentosSemAlta");

        selectSWMultiplosFiltros('.selectAtendimento', "/api/services/app/Atendimento/ListarAtendimentosAmbulatorioInternacao", [{ valor: $('#AmbulatorioInternacao').val() == 1 }, { valor: $('#AmbulatorioInternacao').val() == 2 }]);

        $('#AmbulatorioInternacao').on('click', function () {
            selectSWMultiplosFiltros('.selectAtendimento', "/api/services/app/Atendimento/ListarAtendimentosAmbulatorioInternacao", [{ valor: $('#AmbulatorioInternacao').val() == 1 }, { valor: $('#AmbulatorioInternacao').val() == 2 }]);
        });

        selectSW('.selectConvenio', "/api/services/app/Convenio/ListarDropdown");
        selectSW('.selectCentroCusto', "/api/services/app/CentroCusto/ListarDropdownCodigoCentroCusto");
        selectSW('.selectTipoLeito', "/api/services/app/TipoAcomodacao/ListarDropdown");
        selectSW('.selectLeito', "/api/services/app/Leito/ListarDropdown");
        selectSW('.selectTurno', "/api/services/app/Turno/ListarDropdown");
        selectSW('.selectMedicoSolicitante', "/api/services/app/Medico/ListarDropdown");
        selectSW('.select-terceirizado', "/api/services/app/terceirizado/ListarDropdown");
        selectSW('.select-local-utilizacao', "/api/services/app/unidadeorganizacional/ListarDropdownlocalutilizacao");

        selectSWMultiplosFiltros('.selectExamesSolicitados', "/api/services/app/SolicitacaoExameItem/ListarDropdownExameLaboratorioNaoRegistradoPorAtendimento", ['atendimento-id']);


        //init();
    }
})(jQuery);