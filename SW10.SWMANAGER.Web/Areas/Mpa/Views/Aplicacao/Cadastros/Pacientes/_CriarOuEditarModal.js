(function ($) {
    app.modals.CriarOuEditarPacienteModal = function () {

        $("#btn-fixa-modal").hide();

        var _pacientesService = abp.services.app.paciente;
        var _sisPessoaService = abp.services.app.sisPessoa;

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

            var _anexosModal = new app.ModalManager({
                viewUrl: abp.appPath + 'Mpa/Anexo/OpenModal',
                scriptUrl: abp.appPath + 'Areas/Mpa/Views/Common/Modals/Anexo/_AnexoModal.js',
                modalId: 'anexoModalId'
            });

            $('#btnAnexosDocumento').on('click', function (e) {
                e.preventDefault();
                _anexosModal.open({ anexoListaId: $("#anexoListaIdDocumento").val(), origemAnexoId: $("#id").val(), origemAnexoTabela: 'sispaciente' });
            });

            _$pacienteInformationForm = _modalManager.getModal().find('form[name=PacienteInformationsForm]');
            _$pacienteInformationForm.validate();

            atualizarTabela();

            $('.modal-dialog').css({ 'width': '90%', 'max-width': '1100px' });
            //Forçando o modal a aceitar sobreposição por causa dos selects do datarangepicker
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
            //Ativando autocomplete no combobox
            $('ul.ui-autocomplete').css('z-index', '2147483647');
            //Ativa o editor da observacao
            // $('.text-editor').jqte();

            $('.select2').css('width', '100%');

            //// Novo layout - Abas/Botoes
            //$('#label-gerais').on('click', function (e) {
            //    //
            //    e.preventDefault();
            //    $('#tab-gerais').click();
            //});
            //$('#label-complementares').on('click', function (e) {
            //    e.preventDefault();
            //    $('#tab-complementares').click();
            //});

            // Populando tipos telefone
            //   $('#').swSetCampo();

            _modalManager.getModal().on("hidden.bs.modal", function () {
                abp.event.trigger('app.AtualizaModalAgendamento');
            });

            applicarCamposComCapitalize();


            $('#label-gerais').on('click', function (e) {
                e.preventDefault();
                $('#PacienteInformationsTab').show();
                $('#ComplementaresInformationsTab').hide();
                $('#PacientePesosInformationsTab').hide();
                loadAllSelect2();
            });

            $('#label-complementares').on('click', function (e) {
                e.preventDefault();
                $('#ComplementaresInformationsTab').show();
                $('#PacienteInformationsTab').hide();
                $('#PacientePesosInformationsTab').hide();
                loadAllSelect2();
            });

            $('#label-pesos').on('click', function (e) {
                e.preventDefault();
                $('#PacientePesosInformationsTab').show();
                $('#PacienteInformationsTab').hide();
                $('#ComplementaresInformationsTab').hide();
                loadAllSelect2();
            });


            setTimeout(() => { loadAllSelect2(); }, 500);
        };
        $('.cep').mask('00000-000');
        $(".cpf").mask('000.000.000-00', { reverse: true });

        this.save = function () {

            if ($('.save-button').hasClass("button-busy")) {
                return;
            }

            $('.save-button').buttonBusy(true);
            _$pacienteInformationForm.validate();
            if (!_$pacienteInformationForm.valid()) {
                $('.save-button').buttonBusy(false);
                return;
            }

            if (existemCamposInvalidos())
                return;

            var paciente = _$pacienteInformationForm.serializeFormToObject();

            paciente.Cpf = RetirarMascaraPadrao(paciente.Cpf);
            paciente.Cep = RetirarMascaraPadrao(paciente.Cep);

            _pacientesService.criarOuEditar(paciente)
                .done(function (data) {
                    abp.notify.info(app.localize('SavedSuccessfully'));

                    var abaAtendimento = $('#abaAtendimentoId').val();
                    _modalManager.close();

                    if ($('#isAtendimento').val() == 'true' || $('#isAtendimento').val() == 'True') {

                        var paciente = _pacientesService.obter(data)
                            .done(function (dataPaciente) {

                                if (paciente) {
                                    $('#comboPaciente-' + abaAtendimento).append($("<option>") //add option tag in select
                                        .val(dataPaciente.id) //set value for option to post it
                                        .text(dataPaciente.codigoPaciente + ' - ' + dataPaciente.nomeCompleto + ' - ' + moment(dataPaciente.dataNascimento).format('L'))
                                    ) //set a text for show in select
                                        .val(dataPaciente.id) //select option of select2
                                        .trigger("change");
                                }
                            });
                    }
                    else {
                        abp.event.trigger('app.CriarOuEditarPacienteModalSaved');
                        abp.event.trigger('app.AtualizaModalAgendamento');
                    }


                })
                .always(function () {
                    $('.save-button').buttonBusy(false);
                });
        };

        function existemCamposInvalidos() {
            if ($('#cbo-SexoId').val() == null || $('#cbo-SexoId').val() == 0) {
                abp.notify.warn(app.localize('Informar Sexo'));
                $('.save-button').buttonBusy(false);
                return true;
            }

            if ($("#cpf").val() == null || $("#cpf").val() == '') {
                abp.notify.warn(app.localize('Informar Cpf'));
                $('.save-button').buttonBusy(false);
                return true;
            }

            if ($("select[name=TipoTelefone1Id]").val() == null || $("select[name=TipoTelefone1Id]").val() == '') {
                abp.notify.warn(app.localize('Informar Tipo Telefone'));
                $('.save-button').buttonBusy(false);
                return true;
            }

            if ($("input[name='Telefone1']").val() == null || $("input[name='Telefone1']").val() == '') {
                abp.notify.warn(app.localize('Informar Telefone'));
                $('.save-button').buttonBusy(false);
                return true;
            }

            if ($("#cep").val() == null || $("#cep").val() == '') {
                abp.notify.warn(app.localize('Informar Cep'));
                $('.save-button').buttonBusy(false);
                return true;
            }

            if ($("#cbo-tipo-logradouro-id").val() == null || $("#cbo-tipo-logradouro-id").val() == '') {
                abp.notify.warn(app.localize('Informar Tipo Logradouro'));
                $('.save-button').buttonBusy(false);
                return true;

            }

            if ($("#logradouro").val() == null || $("#logradouro").val() == '') {
                abp.notify.warn(app.localize('Informar Logradouro'));
                $('.save-button').buttonBusy(false);
                return true;

            }

            if ($("#numero").val() == null || $("#numero").val() == '') {
                abp.notify.warn(app.localize('Informar Número'));
                $('.save-button').buttonBusy(false);
                return true;

            }
            return false;
        }
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
                    $('input[name="Nascimento"]').val(selDate.format('L'));
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

            // Novo Peso (Modal inexistente...)
            $('#btn-novo-peso').click(function (e) {
                e.preventDefault()
                $('#paciente-peso-parcial').load('/Pacientes/_CriarOuEditarPacientePesoModal?pacienteId=' + $('#id').val());
            });

            function atualizarTabela() {
                // Pesos
                $('#PacientePesosTable').load('/Pacientes/_PacientePesos?id=' + $('#id').val());
            }

            //$('#profissao-search').autocomplete({
            //        minLength: 3,
            //        delay: 0,
            //        source: function (request, response) {
            //            var term = $('#profissao-search').val();
            //            var url = '/mpa/profissoes/autocomplete';
            //            var fullUrl = url + '/?term=' + term;
            //            $.getJSON(fullUrl, function (data) {
            //                if (data.result.length == 0) {
            //                    $('#profissao-id').val(0);
            //                    $("#profissao-search").focus();
            //                    abp.notify.info(app.localize("ListaVazia"));
            //                    return false;
            //                };
            //                response($.map(data.result, function (item) {
            //                    $('#profissao-id').val(0);
            //                    return {
            //                        label: item.nome,
            //                        value: item.nome,
            //                        realValue: item.id
            //                    };
            //                }));
            //            });
            //        },
            //        select: function (event, ui) {
            //            $('#profissao-id').val(ui.item.realValue);
            //            $('#profissao-search').val(ui.item.value);
            //            //$('.save-button').removeAttr('disabled');
            //            return false;
            //        },
            //        change: function (event, ui) {
            //            event.preventDefault();
            //            if (ui.item == null) {
            //                //$('.save-button').attr('disabled', 'disabled');
            //                $('#profissao-id').val(0);
            //                $("#profissao-search").val('').focus();
            //                abp.notify.info(app.localize("ProfissaoInvalida"));
            //                return false;
            //            }
            //        }
            //    });

            //aplicarSelect2Padrao();

            setTimeout(() => {
                CamposRequeridos();
                aplicarDateSingle();
                aplicarDateRange();
                loadAllSelect2();
            }, 500);
            $('#cpf').on('change', function (e) {
                //return;//temp
                e.preventDefault();

                if (retirarMascara($('#cpf').val()) != '') {
                    _pacientesService.obterPorCpf(retirarMascara($('#cpf').val()))
                        .done(function (data) {

                            if (data) {
                                abp.notify.info('Já existe médico com o CPF informado.');
                                $('#cpf').val('');
                            }
                            else {
                                _sisPessoaService.obterPorCPF(retirarMascara($('#cpf').val()))
                                    .done(function (data) {

                                        if (data) {
                                            carregarDadosPessoa(data);
                                        }
                                    });

                            }
                        })
                        .always(function () {
                            //  _modalManager.setBusy(false);
                        });
                }
            });

            function carregarDadosPessoa(data) {
                $('#sisPessoaId').val(data.id);

                $('#nomeCompleto').val(data.nomeCompleto);
                $('#email').val(data.email);
                $('#Sexo').val(data.sexo).trigger("change");;
                $('#nascimento').val(moment(data.nascimento).format('L'));
                $('#nomeMae').val(data.nomeMae);
                $('#nomePai').val(data.nomePai);
                $('#rg').val(data.rg);
                $('#emissao').val(moment(data.emissaoRg).format('L'));
                $('#emissor').val(data.emissor);
                $('#rg').val(data.rg);

                $('#TipoTelefone1').val(data.tipoTelefone1);
                $('#Telefone1').val(data.telefone1);
                $('#TipoTelefone2').val(data.tipoTelefone2);
                $('#Telefone2').val(data.telefone2);
                $('#TipoTelefone3').val(data.tipoTelefone3);
                $('#Telefone3').val(data.telefone3);
                $('#TipoTelefone4').val(data.tipoTelefone4);
                $('#Telefone4').val(data.telefone4);


                ;


                var base64 = data.foto.toString('base64')  //GetStringFromByteArray(data.foto);
                var imgSrc = "data:" + data.fotoMimeType + ";base64," + base64;//, data.fotoMimeType, base64);

                $("#foto-paciente").attr("src", imgSrc);
                $('#foto-mime-type').val(data.fotoMimeType);

                if (data.listaEnderecos.length > 0) {

                    // $('#tipo-locagradouro-id').val(data.listaEnderecos[0].tipoLogradouroId).trigger("change");;

                    if (data.listaEnderecos[0].tipoLogradouro != undefined && data.listaEnderecos[0].tipoLogradouro != null) {

                        $('#tipo-locagradouro-id')
                            .append($("<option/>") //add option tag in select
                                .val(data.listaEnderecos[0].tipoLogradouroId) //set value for option to post it
                                .text(data.listaEnderecos[0].tipoLogradouro.descricao)
                                .val(data.listaEnderecos[0].tipoLogradouroId).trigger("change")
                            );

                    }
                    ;


                    $('#pais-id')
                        .append($("<option/>") //add option tag in select
                            .val(data.listaEnderecos[0].paisId) //set value for option to post it
                            .text(data.listaEnderecos[0].pais.descricao)
                            .val(data.listaEnderecos[0].paisId).trigger("change")
                        );

                    $('#estado-id')
                        .append($("<option/>") //add option tag in select
                            .val(data.listaEnderecos[0].estadoId) //set value for option to post it
                            .text(data.listaEnderecos[0].estado.nome)
                            .val(data.listaEnderecos[0].estadoId).trigger("change")
                        );

                    $('#cidade-id')
                        .append($("<option/>") //add option tag in select
                            .val(data.listaEnderecos[0].cidadeId) //set value for option to post it
                            .text(data.listaEnderecos[0].cidade.descricao)
                            .val(data.listaEnderecos[0].cidadeId).trigger("change")
                        );


                    $('#cep').val(data.listaEnderecos[0].cep);
                    //$('#pais-id').val(data.listaEnderecos[0].paisId);
                    //$('#estado-id').val(data.listaEnderecos[0].estadoId);
                    //$('#cidade-id').val(data.listaEnderecos[0].cidadeId);
                    $('#logradouro').val(data.listaEnderecos[0].logradouro);
                    $('#numero').val(data.listaEnderecos[0].numero);
                    $('#complemento').val(data.listaEnderecos[0].complemento);
                    $('#bairro').val(data.listaEnderecos[0].bairro);
                }
            }

            function GetStringFromByteArray(array) {
                var result = "";
                for (var i = 0; i < array.length; i++) {
                    for (var j = 0; j < array[i].length; j++)
                        result += String.fromCharCode(array[i][j]);
                }
                return result;
            }




            function loadAllSelect2() {
                $(".select2Estado").select2({
                    allowClear: true,
                    placeholder: app.localize("SelecioneLista"),
                    ajax: {
                        url: "/api/services/app/estado/ListarDropdown",
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
                                filtro: $('.paisclass').val()
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
                $(".select2Cidade").select2({
                    allowClear: true,
                    placeholder: app.localize("SelecioneLista"),
                    ajax: {
                        url: "/api/services/app/cidade/ListarDropdown",
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
                                filtro: $('.select2Estado').val()
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
                $(".select2Sexo").select2({
                    allowClear: true,
                    placeholder: app.localize("SelecioneLista"),
                    ajax: {
                        url: "/api/services/app/sexo/ListarDropdown",
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
                                //,
                                //filtro: $('.paisclass').val()
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
                $(".select2TipoLogradouro").select2({
                    allowClear: true,
                    placeholder: app.localize("SelecioneLista"),
                    ajax: {
                        url: "/api/services/app/tipoLogradouro/ListarDropdown",
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
                                //,
                                //filtro: $('.paisclass').val()
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
                $(".select2Religiao").select2({
                    allowClear: true,
                    placeholder: app.localize("SelecioneLista"),
                    ajax: {
                        url: "/api/services/app/religiao/ListarDropdown",
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
                                filtro: $('.paisclass').val()
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
                $(".select2CorPele").select2({
                    allowClear: true,
                    placeholder: app.localize("SelecioneLista"),
                    ajax: {
                        url: "/api/services/app/corpele/ListarDropdown",
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
                                filtro: $('.select2Estado').val()
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
                $(".select2EstadoCivil").select2({
                    allowClear: true,
                    placeholder: app.localize("SelecioneLista"),
                    ajax: {
                        url: "/api/services/app/estadoCivil/ListarDropdown",
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
                                //,
                                //filtro: $('.paisclass').val()
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
                $(".select2Escolaridade").select2({
                    allowClear: true,
                    placeholder: app.localize("SelecioneLista"),
                    ajax: {
                        url: "/api/services/app/escolaridade/ListarDropdown",
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
                                filtro: $('.paisclass').val()
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
                $(".select2Naturalidade").select2({
                    allowClear: true,
                    placeholder: app.localize("SelecioneLista"),
                    ajax: {
                        url: "/api/services/app/naturalidade/ListarDropdown",
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
                                filtro: $('.select2Estado').val()
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
                $(".select2Nacionalidade").select2({
                    allowClear: true,
                    placeholder: app.localize("SelecioneLista"),
                    ajax: {
                        url: "/api/services/app/nacionalidade/ListarDropdown",
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
                                //,
                                //filtro: $('.paisclass').val()
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
                $(".select2TipoTelefone").select2({
                    allowClear: true,
                    placeholder: app.localize("SelecioneLista"),
                    ajax: {
                        url: "/api/services/app/tipoTelefone/ListarDropdown",
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
                                //,
                                //filtro: $('.paisclass').val()
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
            }
        };
    }) (jQuery);