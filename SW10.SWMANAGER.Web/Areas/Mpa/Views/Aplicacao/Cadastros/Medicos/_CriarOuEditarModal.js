(function ($) {

    app.modals.CriarOuEditarMedicoModal = function () {


        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Medico.MedicoEspecialidade.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Medico.MedicoEspecialidade.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Medico.MedicoEspecialidade.Delete')
        };

        $('.cep').mask('00000-000');
        $(".cpf").mask('000.000.000-00', { reverse: true });

        var _$MedicosEspecialidadesTable = $('#MedicosEspecialidadesTable');


        var _medicoEspecialidadeService = abp.services.app.medicoEspecialidade;

        var listaEspecialidade = [];

        $('#salvar-medico-especialidade').on('click', function (e) {
            e.preventDefault();


            if ($('#especialidade-id').val() == '' || $('#especialidade-id').val() == null) {
                return;
            }

            var _$medicoEspecialidadeInformationsForm = $('form[name=MedicoEspecialidadeInformationsForm]');
            var medicoEspecialidade = _$medicoEspecialidadeInformationsForm.serializeFormToObject();


            if ($('#especialidades').val() != '') {
               
                listaEspecialidade = JSON.parse($('#especialidades').val());
            }

            if ($('#id-grid-medico-especialidade').val() != '') {

                for (var i = 0; i < listaEspecialidade.length; i++) {
                    if (listaEspecialidade[i].IdGridMedicoEspecialidade == $('#id-grid-medico-especialidade').val()) {

                        var especialidade = $('#especialidade-id').select2('data');
                        if (especialidade && especialidade.length > 0) {

                            listaEspecialidade[i].Especialidade.Descricao = especialidade[0].text;
                        }


                        listaEspecialidade[i].IdEspecialidade = $('#especialidade-id').val();

                        _$MedicosEspecialidadesTable.jtable('updateRecord', {
                            record: listaEspecialidade[i]
                            , clientOnly: true
                        });

                    }
                }
            }
            else {
                medicoEspecialidade.IdGridMedicoEspecialidade = listaEspecialidade.length == 0 ? 1 : listaEspecialidade[listaEspecialidade.length - 1].IdGridMedicoEspecialidade + 1;

                var campoEspecialidade = $('#especialidade-id').select2('data');
                if (campoEspecialidade && campoEspecialidade.length > 0) {
                    medicoEspecialidade.Descricao = campoEspecialidade[0].text;
                }
                medicoEspecialidade.IdEspecialidade = $('#especialidade-id').val();

                listaEspecialidade.push(medicoEspecialidade);
                _$MedicosEspecialidadesTable.jtable('addRecord', {
                    record: medicoEspecialidade
                    , clientOnly: true
                });
            }

            $('#especialidades').val(JSON.stringify(listaEspecialidade));
            $('#id-grid-medico-especialidade').val('');
            $('#especialidade-id').val('').trigger('change');
            $('#especialidade-id').focus();

        });




        //    e.preventDefault();
        //    ;
        //    var itemProcessado = false;
        //    var _$medicoEspecialidadeForm = $('form[name="MedicoEspecialidadeInformationsForm"]');
        //    //_$medicoEspecialidadeForm.validate();
        //    //if (!_$medicoEspecialidadeForm.valid()) {
        //    //    return;
        //    //}
        //    var medicoEspecialidade = _$medicoEspecialidadeForm.serializeFormToObject();

        //    var medicoEspecialidadeList = $('#medico-especialidade-list').val();
        //    localStorage["medicoEspecialidadeList"] = medicoEspecialidadeList;
        //    if (!localStorage["medicoEspecialidadeList"] || (localStorage["medicoEspecialidadeList"] && localStorage["medicoEspecialidadeList"] == '[]')) {
        //        localStorage["medicoEspecialidadeList"] = '';
        //    }
        //    if (localStorage["medicoEspecialidadeList"] != '') {
        //        var lista = JSON.parse(localStorage["medicoEspecialidadeList"]);
        //    }
        //    else {
        //        var lista = [];
        //    }
        //    //if ($('#id-grid-formulas-estoque').val() != '' && $('#formula-estoque-list').val() != '[]') {
        //    if (lista.length > 0) {
        //        for (var i = 0; i < lista.length; i++) {
        //            if (lista[i].IdGridMedicosEspecialidade == $('#id-grid-medico-especialidade').val()) {
        //                //editando o registro
        //                lista[i].Id = medicoEspecialidade.Id;
        //                //lista[i].Codigo = $('#codigo-formula-estoque').val();
        //                //lista[i].Descricao = $('#descricao-formula-especialidade').val();
        //                //lista[i].EstoqueId = medicoEspecialidade.EstoqueId;
        //                //lista[i].ProdutoId = medicoEspecialidade.ProdutoId;
        //                //lista[i].UnidadeId = medicoEspecialidade.UnidadeId;
        //                //lista[i].PrescricaoItemId = medicoEspecialidade.PrescricaoItemId;
        //                //lista[i].IsPrincipal = medicoEspecialidade.IsPrincipal;
        //                lista[i].IdGridMedicosEspecialidade = medicoEspecialidade.IdGridMedicosEspecialidade;
        //                itemProcessado = true;


        //                _$MedicosEspecialidadesTable.jtable('updateRecord', {
        //                    record: lista[i]
        //                    , clientOnly: true
        //                });


        //                break;
        //            }
        //        }
        //        if (!itemProcessado) {
        //            medicoEspecialidade.IdGridMedicosEspecialidade = lista.length == 0 ? 1 : lista[lista.length - 1].IdGridMedicosEspecialidade + 1;
        //            lista.push(medicoEspecialidade);

        //            _$MedicosEspecialidadesTable.jtable('addRecord', {
        //                record: medicoEspecialidade
        //                , clientOnly: true
        //            })

        //        }
        //    }
        //    else {
        //        medicoEspecialidade.IdGridMedicosEspecialidade = lista.length == 0 ? 1 : lista[lista.length - 1].IdGridMedicosEspecialidade + 1;
        //        lista.push(medicoEspecialidade);

        //        _$MedicosEspecialidadesTable.jtable('addRecord', {
        //            record: medicoEspecialidade
        //            , clientOnly: true
        //        })
        //    }

        //    $('#medico-especialidade-list').val(JSON.stringify(lista));
        //    localStorage["medicoEspecialidadeList"] = JSON.stringify(lista);
        //    abp.notify.info(app.localize('ListaAtualizada'));
        //    //abp.event.trigger('app.CriarOuEditarmedicoEspecialidadeModalSaved');
        //    $('#FormulasEstoquesTable').jtable('load', {
        //        prescricaoItemId: $('#medico-especialidade-id').val()
        //    });
        //    //getFormulasEstoques();
        //    $('#CreateNewMedicoEspecialidadeButton').trigger('click');
        //});

        function getEspecialidade(reload) {

            //;
           

            var allRows = _$MedicosEspecialidadesTable.find('.jtable-data-row')

            if (allRows == null || allRows.length == 0) {

                //$.each(allRows, function () {
                //    var id = $(this).attr('data-record-key');
                //    _$MedicosEspecialidadesTable.jtable('deleteRecord', { key: id, clientOnly: true });
                //});


                listaEspecialidade = JSON.parse($('#especialidades').val());

                for (var i = 0; i < listaEspecialidade.length; i++) {
                    var item = listaEspecialidade[i];
                    _$MedicosEspecialidadesTable.jtable('addRecord', {
                        record: item
                        , clientOnly: true
                    });
                }
            }
        }

        //if (reload) {
        //    _$MedicosEspecialidadesTable.jtable('reload');
        //} else {
        //    _$MedicosEspecialidadesTable.jtable('load', {


        //       // filtro: $('#id').val()
        //    });
        //}


        _$MedicosEspecialidadesTable.jtable({

            title: app.localize('Especialidade Médica'),
            sorting: true,
            edit: false,
            create: false,

            //actions: {
            //    listAction: {
            //        method: _medicoEspecialidadeService.listarPorMedico
            //    }
            //},

            fields: {
                IdGridMedicoEspecialidade
                : {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '8%',
                    sorting: false,
                    display: function (data) {
                        var $span = $('<span></span>');

                        //$('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                        //    .appendTo($span)
                        //    .click(function (e) {
                        //        e.preventDefault();
                        //        ;
                        //        editarEspecialidade(data.record);

                        //    });

                        $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                            .appendTo($span)
                            .click(function (e) {
                                ;
                                e.preventDefault();
                                deleteEspecialidade(data.record);
                            });


                        return $span;
                    }
                },
                Especialidade: {
                    title: app.localize('Especialidade'),
                    width: '15%',
                    display: function (data) {
                        ;
                        if (data) {
                            return data.record.Descricao;
                        }
                    }
                },

            }
        });

        //getEspecialidade();

        function editarEspecialidade(record) {

            $('#especialidade-id').append($("<option>") //add option tag in select
                .val(record.especialidadeId) //set value for option to post it
                .text(record.especialidade.descricao)
            ) //set a text for show in select
                .val(record.especialidadeId) //select option of select2
                .trigger("change");

        }

        function deleteEspecialidade(record) {
            ;
            abp.message.confirm(
                app.localize('DeleteWarning', record.Descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        lista = JSON.parse($('#especialidades').val());
                        for (var i = 0; i < lista.length; i++) {
                            ;
                            if (lista[i].IdGridMedicoEspecialidade == record.IdGridMedicoEspecialidade) {
                                lista.splice(i, 1);
                                $('#especialidades').val(JSON.stringify(lista));

                                _$MedicosEspecialidadesTable.jtable('deleteRecord', {
                                    key: record.IdGridMedicoEspecialidade
                                    , clientOnly: true
                                });

                                break;
                            }
                        }
                    }
                }
            );
        }


        //function deleteEspecialidade(record) {
        //    abp.message.confirm(
        //        app.localize('DeleteWarning', record.primeiroNome),
        //        function (isConfirmed) {
        //            if (isConfirmed) {
        //                _medicoEspecialidadeService.excluir(record)
        //                    .done(function () {
        //                        getEspecialidade(true);
        //                        abp.notify.success(app.localize('SuccessfullyDeleted'));
        //                    });
        //            }
        //        }
        //    );
        //}

        //var _$MedicoEspecialidadesTable = $('#MedicoEspecialidadesTable');

        var _medicosService = abp.services.app.medico;
        var _sisPessoaService = abp.services.app.sisPessoa;

        var fixaModal = false;

        var _modalManager;
        var _$medicoInformationForm = null;

        var _$medicoEspecialidadesInformationForm = null;


        //var _createMedicoEspecialidadeModal = new app.ModalManager({
        //    viewUrl: abp.appPath + 'Mpa/Medicos/CriarMedicoEspecialidadeModal',
        //    scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Medicos/_CriarMedicoEspecialidadeModal.js',
        //    modalClass: 'CriarMedicoEspecialidadeModal'
        //});

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Medicos/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Medicos/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarMedicoModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$medicoInformationForm = _modalManager.getModal().find('form[name=MedicoInformationsForm]');
            _$medicoInformationForm.validate();
            //_$medicoEspecialidadesInformationForm = _modalManager.getModal().find('form[name=MedicoInformationsForm]');

            //atualizarTabela();

            _modalManager.getModal().find('#div-btn-fixa-modal').show();

            var btnFixaModal = _modalManager.getModal().find('#btn-fixa-modal:last');

            //btnFixaModal.addClass('blue');

            btnFixaModal.on('click', function (e) {
                fixaModal = !fixaModal;
                if (fixaModal) {
                    btnFixaModal.addClass('blue');
                } else {
                    btnFixaModal.removeClass('blue');
                }
            });

            $('.modal-dialog').css({ 'width': '100%', 'max-width': '900px' });
            //$('div.form-group select').addClass('form-control selectpicker');
            //Forçando o modal a aceitar sobreposição por causa dos selects do datarangepicker
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
            //Ativando autocomplete no combobox

            $('ul.ui-autocomplete').css('z-index', '2147483647');
            $('.selectpicker').selectpicker('refresh');

            $('.select2').css('width', '100%');

        };

        this.save = function () {
           

            _$medicoInformationForm.validate();

            if (!_$medicoInformationForm.valid()) {
                return;
            }

            var camposobrigatorios = $('#cpf').val() == ''
                                  || $('#conselho-id').val() == ''
                                  || $('#nascimento').val() == ''
                                  || $('#nomeCompleto').val() == ''
                                  || $('#cep').val() == '';

            if (camposobrigatorios) {

                alert('Existem campos obrigatórios não preenchidos.')
                return;
            }


            var medico = _$medicoInformationForm.serializeFormToObject();

            medico.Cpf = RetirarMascaraPadrao(medico.Cpf);
            medico.Cep = RetirarMascaraPadrao(medico.Cep);

            medico.MedicoEspecialidadeList = $('#especialidades').val();

            medico.Observacao = $('#observacao').val()

            _modalManager.setBusy(true);

            var editMode = $('#is-edit-mode').val();

            _medicosService.criarOuEditar(medico)
                .done(function (data) {
                    abp.notify.info(app.localize('SavedSuccessfully'));

                    // Fixar modal ou nao, apos save
                    if (!fixaModal) {
                        _modalManager.close();
                    } else {
                        if (editMode) {
                            $('#label-gerais').click();
                        } else {
                            limparFormulario();
                        };
                    };

                    abp.event.trigger('app.CriarOuEditarMedicoModalSaved');

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
            $('#idade').load('/mpa/Medicos/ObterIdade?data=' + output.getFullYear() + '-' + (output.getMonth() + 1) + '-' + output.getDate());
        }


        $('#capturar-foto').click(function (e) {
            e.preventDefault();
            if ($('#area-captura').html() === '') {
                $('#area-captura').load("/mpa/medicos/_CarregarFoto", function () {
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

        $('#capturar-imagem').click(function (e) {
            e.preventDefault();
            $('<input>', {
                'id': 'file',
                'class': 'hidden',
                'name': 'File',
                'type': 'file',
                'accept': '.jpg', //'image/jpeg .jpg',
                'onchange': lerImagemForm(this, 'assinatura-digital', 'assinatura-digital-mime-type', 'assinatura-digital-img')
            }).appendTo('body');

            $('#file').change(function () {
                lerImagemForm(this, 'assinatura-digital', 'assinatura-digital-mime-type', 'assinatura-digital-img');
            })
                .click();
        });

        $('#btn-nova-especialidade').click(function (e) {
            e.preventDefault()
            $('#medico-especialidade-parcial').load('/Medicos/_CriarOuEditarMedicoEspecialidadeModal?medicoId=' + $('#id').val());
        });

        function atualizarTabela() {
          //  $('#MedicoEspecialidadesTable').load('/Medicos/_MedicoEspecialidades?id=' + $('#id').val());
        }

        $('#cor-agendamento-consulta').minicolors({
            control: $('#cor-agendamento-consulta').attr('data-control') || 'hue',
            defaultValue: $('#cor-agendamento-consulta').attr('data-defaultValue') || '',
            format: $('#cor-agendamento-consulta').attr('data-format') || 'hex',
            keywords: $('#cor-agendamento-consulta').attr('data-keywords') || '',
            inline: $('#cor-agendamento-consulta').attr('data-inline') === 'true',
            letterCase: $('#cor-agendamento-consulta').attr('data-letterCase') || 'lowercase',
            opacity: $('#cor-agendamento-consulta').attr('data-opacity'),
            position: $('#cor-agendamento-consulta').attr('data-position') || 'bottom left',
            swatches: $('#cor-agendamento-consulta').attr('data-swatches') ? $(this).attr('data-swatches').split('|') : [],
            change: function (value, opacity) {
                if (!value) return;
                if (opacity) value += ', ' + opacity;
                if (typeof console === 'object') {
                    //console.log(value);
                }
                swatches: $('#cor-agendamento-consulta').addClass('edited')
            },
            theme: 'bootstrap'
        }).addClass('edited');

        //$('#profissao-search')
        //    .autocomplete({
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
        //        },
        //    });

        //$('#naturalidade-search')
        //    .autocomplete({
        //        minLength: 3,
        //        delay: 0,
        //        source: function (request, response) {
        //            var term = $('#naturalidade-search').val();
        //            var url = '/mpa/naturalidades/autocomplete';
        //            var fullUrl = url + '/?term=' + term;
        //            $.getJSON(fullUrl, function (data) {
        //                if (data.result.length == 0) {
        //                    $('#naturalidade-id').val(0);
        //                    $("#naturalidade-search").focus();
        //                    abp.notify.info(app.localize("ListaVazia"));
        //                    return false;
        //                };
        //                response($.map(data.result, function (item) {
        //                    $('#naturalidade-id').val(0);
        //                    return {
        //                        label: item.cidadeOrigem,
        //                        value: item.cidadeOrigem,
        //                        realValue: item.id
        //                    };
        //                }));
        //            });
        //        },
        //        select: function (event, ui) {
        //            $('#naturalidade-id').val(ui.item.realValue);
        //            $('#naturalidade-search').val(ui.item.value);
        //            //$('.save-button').removeAttr('disabled');
        //            return false;
        //        },
        //        change: function (event, ui) {
        //            event.preventDefault();
        //            if (ui.item == null) {
        //                //$('.save-button').attr('disabled', 'disabled');
        //                $('#naturalidade-id').val(0);
        //                $("#naturalidade-search").val('').focus();
        //                abp.notify.info(app.localize("NaturalidadeInvalida"));
        //                return false;
        //            }
        //        },
        //    });

        //$('#pais-search')
        //    .autocomplete({
        //        minLength: 2,
        //        delay: 0,
        //        source: function (request, response) {
        //            var term = $('#pais-search').val();
        //            var url = '/mpa/paises/autocomplete';
        //            var fullUrl = url + '/?term=' + term;
        //            $.getJSON(fullUrl, function (data) {
        //                if (data.result.length == 0) {
        //                    $('#pais-id').val(0);
        //                    $("#pais-search").focus();
        //                    abp.notify.info(app.localize("ListaVazia"));
        //                    return false;
        //                };
        //                response($.map(data.result, function (item) {
        //                    $('#pais-id').val(0);
        //                    return {
        //                        label: item.nome,
        //                        value: item.nome,
        //                        realValue: item.id
        //                    };
        //                }));
        //            });
        //        },
        //        select: function (event, ui) {
        //            $('#pais-id').val(ui.item.realValue);
        //            $('#pais-search').val(ui.item.value);
        //            //$('.save-button').removeAttr('disabled');
        //            return false;
        //        },
        //        change: function (event, ui) {
        //            event.preventDefault();
        //            if (ui.item == null) {
        //                //$('.save-button').attr('disabled', 'disabled');
        //                $('#pais-id').val(0);
        //                $("#pais-search").val('').focus();
        //                abp.notify.info(app.localize("PaisInvalido"));
        //                return false;
        //            }
        //        },
        //    });

        //$('#estado-search')
        //    .autocomplete({
        //        minLength: 2,
        //        delay: 0,
        //        source: function (request, response) {
        //            var term = $('#estado-search').val();
        //            var url = '/mpa/estados/autocomplete';
        //            if ($('#pais-id').length > 0) {
        //                term += "&paisId=" + $('#pais-id').val();
        //            }

        //            var fullUrl = url + '/?term=' + term;
        //            $.getJSON(fullUrl, function (data) {
        //                if (data.result.length == 0) {
        //                    $('#estado-id').val(0);
        //                    $("#estado-search").focus();
        //                    abp.notify.info(app.localize("ListaVazia"));
        //                    return false;
        //                };
        //                response($.map(data.result, function (item) {
        //                    $('#estado-id').val(0);
        //                    return {
        //                        label: item.nome,
        //                        value: item.nome,
        //                        realValue: item.id
        //                    };
        //                }));
        //            });
        //        },
        //        select: function (event, ui) {
        //            $('#estado-id').val(ui.item.realValue);
        //            $('#estado-search').val(ui.item.value);
        //            //$('.save-button').removeAttr('disabled');
        //            return false;
        //        },
        //        change: function (event, ui) {
        //            event.preventDefault();
        //            if (ui.item == null) {
        //                //$('.save-button').attr('disabled', 'disabled');
        //                $('#estado-id').val(0);
        //                $("#estado-search").val('').focus();
        //                abp.notify.info(app.localize("EstadoInvalido"));
        //                return false;
        //            }
        //        },
        //    });

        //$('#cidade-search')
        //    .autocomplete({
        //        minLength: 3,
        //        delay: 0,
        //        source: function (request, response) {
        //            var term = $('#cidade-search').val();
        //            var url = '/mpa/cidades/autocomplete';
        //            if ($('#estado-id').length > 0) {
        //                term += "&estadoId=" + $('#estado-id').val();
        //            }
        //            var fullUrl = url + '/?term=' + term;

        //            $.getJSON(fullUrl, function (data) {
        //                if (data.result.length == 0) {
        //                    $('#cidade-id').val(0);
        //                    $("#cidade-search").focus();
        //                    abp.notify.info(app.localize("ListaVazia"));
        //                    return false;
        //                };
        //                response($.map(data.result, function (item) {
        //                    $('#cidade-id').val(0);
        //                    return {
        //                        label: item.nome,
        //                        value: item.nome,
        //                        realValue: item.id
        //                    };
        //                }));
        //            });
        //        },
        //        select: function (event, ui) {
        //            $('#cidade-id').val(ui.item.realValue);
        //            $('#cidade-search').val(ui.item.value);
        //            //$('.save-button').removeAttr('disabled');
        //            return false;
        //        },
        //        change: function (event, ui) {
        //            event.preventDefault();
        //            if (ui.item == null) {
        //                //$('.save-button').attr('disabled', 'disabled');
        //                $('#cidade-id').val(0);
        //                $("#cidade-search").val('').focus();
        //                abp.notify.info(app.localize("CidadeInvalida"));
        //                return false;
        //            }
        //        },
        //    });

        $('#tab-medico-especialidades').on('click', function (e) {
            e.preventDefault();

           

            getEspecialidade();
            if (!$('#is-edit-mode').val()) {
                abp.notify.error(app.localize('IncluindoRegistro'));
            }

            
            atualizarTabela($('#id').val());
        });


        //function mudaEstadoDoBotao(input) {
        //    if (!$(input).hasClass('active')) {
        //        //alert('sim')
        //        $(input).removeClass('blue');
        //        $(input).addClass('grey-silver');
        //    }
        //    else {
        //        //alert('nao')
        //        $(this).removeClass('grey-silver');
        //        $(this).addClass('blue');
        //    };
        //};

        function botoesabas(sender) {

            $("[name='botoes-abas']").each(function () {

                if ($(this).attr('id') != sender) {
                    //alert('sim')
                    $(this).removeClass('blue');
                    $(this).addClass('dark');
                }
                else {
                    //alert('nao')
                    $(this).removeClass('dark');
                    $(this).addClass('blue');
                };

            });
        };


        $('#label-gerais').on('click', function (e) {
            e.preventDefault();
            $('#MedicoInformationsTab').show();
            $('#ComplementaresInformationsTab').hide();
            $('#ConfiguracoesInformationsTab').hide();
            $('#MedicoEspecialidadesInformationsTab').hide();
            $('#ObservacoesInformationsTab').hide();
            botoesabas($(this).attr('id'));
        });

        $('#label-complementares').on('click', function (e) {
            e.preventDefault();
            $('#ComplementaresInformationsTab').show();
            $('#MedicoInformationsTab').hide();
            $('#ConfiguracoesInformationsTab').hide();
            $('#MedicoEspecialidadesInformationsTab').hide();
            $('#ObservacoesInformationsTab').hide();
            botoesabas($(this).attr('id'));
        });

        $('#label-especialidades').on('click', function (e) {
            e.preventDefault();
            getEspecialidade();
            $('#MedicoEspecialidadesInformationsTab').show();
            $('#ComplementaresInformationsTab').hide();
            $('#MedicoInformationsTab').hide();
            $('#ConfiguracoesInformationsTab').hide();
            $('#ObservacoesInformationsTab').hide();
            botoesabas($(this).attr('id'));
        });

        $('#label-configuracoes').on('click', function (e) {
            e.preventDefault();
            $('#ConfiguracoesInformationsTab').show();
            $('#MedicoInformationsTab').hide();
            $('#ComplementaresInformationsTab').hide();
            $('#MedicoEspecialidadesInformationsTab').hide();
            $('#ObservacoesInformationsTab').hide();
            botoesabas($(this).attr('id'));
        });

        $('#label-observacoes').on('click', function (e) {
            e.preventDefault();
            $('#ObservacoesInformationsTab').show();
            $('#ConfiguracoesInformationsTab').hide();
            $('#MedicoInformationsTab').hide();
            $('#ComplementaresInformationsTab').hide();
            $('#MedicoEspecialidadesInformationsTab').hide();
            botoesabas($(this).attr('id'));
        });


        function limparFormulario() {
            
            $("input[type='selectpicker']").each(function () {
                $(this).val("").change().selectpicker('refresh');
            });
        };

        $('.save-button').attr('background', '#72D313');
        $('.close-button').attr('background', '#F80E3F');


        $('#cpf').on('change', function (e) {

            e.preventDefault();

            ;

            if (retirarMascara($('#cpf').val()) != '') {
                _medicosService.obterPorCPF(retirarMascara($('#cpf').val()))
                    .done(function (data) {
                        ;
                        if (data) {
                            abp.notify.info('Já existe médico com o CPF informado.');
                            $('#cpf').val('');
                        }
                        else {
                            _sisPessoaService.obterPorCPF(retirarMascara($('#cpf').val()))
                                .done(function (data) {
                                    ;
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
            $('#emissao').val(data.emissaoRg);
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

            var base64 = data.foto.toString('base64')
            var imgSrc = "data:" + data.fotoMimeType + ";base64," + base64;

            $("#foto-medico").attr("src", imgSrc);
            $('#foto-mime-type').val(data.fotoMimeType);

            if (data.listaEnderecos.length > 0) {

                $('#TipoLogradouroId').val(data.listaEnderecos[0].tipoLogradouroId).trigger("change");;
                $('#cep').val(data.listaEnderecos[0].cep);
                $('#pais-id').val(data.listaEnderecos[0].paisId);
                $('#estado-id').val(data.listaEnderecos[0].estadoId);
                $('#cidade-id').val(data.listaEnderecos[0].cidadeId);
                $('#logradouro').val(data.listaEnderecos[0].logradouro);
                $('#numero').val(data.listaEnderecos[0].numero);
                $('#complemento').val(data.listaEnderecos[0].complemento);
                $('#bairro').val(data.listaEnderecos[0].bairro);
            }
        }
        aplicarDateSingle();
        aplicarDateRange();
        aplicarSelect2Padrao();
        CamposRequeridos();

    };
})(jQuery);