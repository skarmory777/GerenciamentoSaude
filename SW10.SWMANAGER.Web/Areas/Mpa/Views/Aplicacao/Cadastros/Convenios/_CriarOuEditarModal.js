


(function ($) {
    app.modals.CriarOuEditarConvenioModal = function () {
        var _conveniosService = abp.services.app.convenio;
        var _sisPessoaService = abp.services.app.sisPessoa;



        var _modalManager;
        var _$convenioInformationForm = null;
        var _$urlServicoInformationsForm = null;
        //var _$webServicesTable = $("#WebServicesTable");

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Convenio.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Convenio.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Convenio.Delete')
        };

        var _ErrorModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Erros/ExibirErros',
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

            $('.cnpj').mask('00.000.000/0000-00', { reverse: true });
            $('.cep').mask('00000-000');

            _$convenioInformationForm = _modalManager.getModal().find('form[name=ConvenioInformationsForm]');
            _$convenioInformationForm.validate();

            CamposRequeridos();

            //_$urlServicoInformationsForm = _modalManager.getModal().find('form[name=URLServicoInformationsForm]');
            //_$urlServicoInformationsForm.validate();

            $('.modal-dialog').css({ 'width': '90%', 'max-width': '1100px' });
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
            // aplicarSelect2Padrao();





        };

        this.save = function () {
            if (!_$convenioInformationForm.valid()) {
                return;
            }


            var convenio = _$convenioInformationForm.serializeFormToObject();

            convenio.Cnpj = RetirarMascaraPadrao(convenio.Cnpj);
            convenio.Cep = RetirarMascaraPadrao(convenio.Cep);
            _modalManager.setBusy(true);


            let configuracaoResumoContaEmergencia = {}
            _$convenioInformationForm.find("input[name^='ConfiguracaoResumoContaEmergencia_']").each(function () {
                const input = $(this);
                let prop = input.attr("name").replace("ConfiguracaoResumoContaEmergencia_", "");
                let val = undefined;
                if (input.attr("type") == "checkbox") {
                    val = input.is(":checked")
                }

                configuracaoResumoContaEmergencia[prop] = val;
            })

            convenio.configuracaoResumoContaEmergencia = configuracaoResumoContaEmergencia;

            let configuracaoResumoContaInternacao = {}
            _$convenioInformationForm.find("input[name^='ConfiguracaoResumoContaInternacao_']").each(function () {
                const input = $(this);
                let prop = input.attr("name").replace("ConfiguracaoResumoContaInternacao_", "");
                let val = undefined;
                if (input.attr("type") == "checkbox") {
                    val = input.is(":checked")
                }

                configuracaoResumoContaInternacao[prop] = val;
            })

            convenio.configuracaoResumoContaInternacao = configuracaoResumoContaInternacao;

            _conveniosService.criarOuEditar(convenio)
                .done(function (data) {


                    if (data.errors.length > 0) {
                        _ErrorModal.open({ erros: data.errors });
                    }
                    else {
                        abp.notify.info(app.localize('SavedSuccessfully'));
                        _modalManager.close();
                        abp.event.trigger('app.CriarOuEditarConvenioModalSaved');
                    }
                })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };

        //date picker

        $('input[name="DataInicialContrato"]').daterangepicker({
            "singleDatePicker": true,
            "showDropdowns": true,
            maxDate: new Date() + 720,
            autoUpdateInput: false,
            changeYear: true,
            yearRange: 'c-50:c+5',
            showOn: "both",
            "locale": {
                "format": moment.locale().toUpperCase() === 'PT-BR' ? "DD/MM/YYYY" : moment.locale().toUpperCase() === 'US' ? "MM/DD/YYYY" : "YYYY/MM/DD",
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
                $('input[name="DataInicialContrato"]').val(selDate.format('L')).addClass('form-control edited');
            });

        $('input[name="DataUltimaRenovacaoContrato"]').daterangepicker({
            "singleDatePicker": true,
            "showDropdowns": true,
            maxDate: new Date() + 7200,
            autoUpdateInput: false,
            changeYear: true,
            yearRange: 'c-50:c+10',
            showOn: "both",
            "locale": {
                "format": moment.locale().toUpperCase() === 'PT-BR' ? "DD/MM/YYYY" : moment.locale().toUpperCase() === 'US' ? "MM/DD/YYYY" : "YYYY/MM/DD",
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
                $('input[name="DataUltimaRenovacaoContrato"]').val(selDate.format('L')).addClass('form-control edited');
            });

        $('input[name="DataProximaRenovacaoContrato"]').daterangepicker({
            "singleDatePicker": true,
            "showDropdowns": true,
            maxDate: new Date() + 20000,
            autoUpdateInput: false,
            changeYear: true,
            yearRange: 'c-5:c+50',
            showOn: "both",
            "locale": {
                "format": moment.locale().toUpperCase() === 'PT-BR' ? "DD/MM/YYYY" : moment.locale().toUpperCase() === 'US' ? "MM/DD/YYYY" : "YYYY/MM/DD",
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
                $('input[name="DataProximaRenovacaoContrato"]').val(selDate.format('L')).addClass('form-control edited');
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
            //if (typeof ($("input#file")) === "undefined") {
            $('<input>', {
                'id': 'file',
                'class': 'hidden',
                'name': 'File',
                'type': 'file',
                'onchange': lerImagemForm(this, 'logotipo', 'logotipo-mime-type', 'logotipo-img')
            }).appendTo('body');
            //}
            $('#file').change(function () {
                lerImagemForm(this, 'logotipo', 'logotipo-mime-type', 'logotipo-img');
            })
                .click();
        });

        $('#cnpj').on('change', function (e) {
            e.preventDefault();


            if (retirarMascara($('#cnpj').val()) != '') {
                _conveniosService.obterCNPJ(retirarMascara($('#cnpj').val()))
                    .done(function (data) {
                        if (data) {
                            abp.notify.info('Já existe convênio com o CNPJ informado.');
                            //$('#cnpj').val('');
                        }
                        else {
                            _sisPessoaService.obterPorCnpj(retirarMascara($('#cnpj').val()))
                                .done(function (data) {
                                    if (data) {

                                        $('#sisPessoaId').val(data.id);

                                        $('#nome-fantasia').val(data.nomeFantasia);
                                        $('#razao-social').val(data.razaoSocial);
                                        $('#inscricao-estadual').val(data.inscricaoEstadual);
                                        $('#inscricao-municipal').val(data.inscricaoMunicipal);
                                        $('#cep').val(data.cep);
                                        $('#pais-id').val(data.paisId);
                                        $('#estado-id').val(data.estadoId);
                                        $('#cidade-id').val(data.cidadeId);
                                        $('#logradouro').val(data.logradouro);
                                        $('#numero').val(data.numero);
                                        $('#complemento').val(data.complemento);
                                        $('#bairro').val(data.bairro);
                                    }
                                });

                        }
                    })
                    .always(function () {
                        //  _modalManager.setBusy(false);
                    });
            }
        });

        $('input[name="RdoAtendimento"]').on('change', function (e) {
            // e.preventDefault();
            switch ($(this).val()) {
                case '1':
                    $('#is-eletivo').val('true');
                    $('#is-urgencia').val('false');
                    break;
                case '2':
                    $('#is-eletivo').val('false');
                    $('#is-urgencia').val('true');
                    break;
                default:
                    $('#is-eletivo').val('true');
                    $('#is-urgencia').val('true');
                    break;
            }
        });

        var _$internavalosTable = $('#internavalosTable');

        _$internavalosTable.jtable({

            title: app.localize('Intervalos'),
            sorting: true,
            edit: false,
            create: false,
            multiSorting: true,


            rowInserted: function (event, data) {
                select: true
            },

            fields: {
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
                        $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                            .appendTo($span)
                            .click(function (e) {
                                e.preventDefault();
                                editInternvalo(data.record)
                            });

                        $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                            .appendTo($span)
                            .click(function (e) {
                                e.preventDefault();
                                //deleteRateio(data.record);
                            });

                        return $span;
                    }
                },
                Empresa: {
                    title: app.localize('Empresa'),
                    width: '15%',
                    display: function (data) {
                        return data.record.EmpresaDescricao;
                    }
                },

                Guia: {
                    title: app.localize('Guia'),
                    width: '15%',
                    display: function (data) {
                        return data.record.FaturamentoGuiaDescricao;
                    }
                },
                Inicio: {
                    title: app.localize('Inicio'),
                    width: '15%',
                    display: function (data) {
                        return data.record.Inicio;
                    }
                },
                Final: {
                    title: app.localize('Fim'),
                    width: '15%',
                    display: function (data) {
                        return data.record.Final;
                    }
                },

                AvisarFaltando: {
                    title: app.localize('AvisarFaltando'),
                    width: '10%',
                    display: function (data) {

                        return data.record.NumeroGuiasParaAviso;
                    }
                },

            }
        });

        var _$codigoCredenciadoTable = $('#codigoCredenciadoTable');

        _$codigoCredenciadoTable.jtable({

            title: app.localize('CodigoCredenciado'),
            sorting: true,
            edit: false,
            create: false,
            multiSorting: true,

            rowInserted: function (event, data) {
                select: true
            },

            fields: {
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
                                excluirCodigoCredenciado(e, data);
                            });

                        return $span;
                    }
                },
                Empresa: {
                    title: app.localize('Empresa'),
                    width: '15%',
                    display: function (data) {
                        return data.record.EmpresaDescricao;
                    }
                },
                CodCredenciado: {
                    title: app.localize('CodigoCredenciado'),
                    width: '15%',
                    display: function (data) {
                        return data.record.Codigo;
                    }
                }
            }
        });

        var listaCodigoCredenciado = [];

        function getCodigoCredenciado() {
            listaCodigoCredenciado = JSON.parse($('#codigoCredenciadoConveniosIndexJson').val());

            var allRows = _$codigoCredenciadoTable.find('.jtable-data-row')

            $.each(allRows, function () {
                var id = $(this).attr('data-record-key');
                _$codigoCredenciadoTable.jtable('deleteRecord', { key: id, clientOnly: true });
            });

            for (var i = 0; i < listaCodigoCredenciado.length; i++) {
                var item = listaCodigoCredenciado[i];

                _$codigoCredenciadoTable.jtable('addRecord', {
                    record: item
                    , clientOnly: true
                });
            }
        }

        var _$fatGrupoConvenioTable = $('#fatGrupoConvenioTable');

        _$fatGrupoConvenioTable.jtable({
            title: app.localize('GruposPorDia'),
            sorting: true,
            edit: false,
            create: false,
            multiSorting: true,
            rowInserted: function (event, data) {
                select: true
            },

            fields: {
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
                                excluirFatGrupoConvenio(e, data);
                            });

                        return $span;
                    }
                },
                GrupoDescricao: {
                    title: app.localize('Grupo'),
                    width: '15%',
                    display: function (data) {
                        return data.record.GrupoDescricao;
                    }
                },
                IsCobrancaDia: {
                    title: app.localize('IsCobrancaItemDia'),
                    width: '15%',
                    display: function (data) {
                        return data.record.IsCobrancaDia;
                    }
                }
            }
        });

        var listaFatGrupoConvenio = [];

        function getFatGrupoConvenio() {
            listaFatGrupoConvenio = JSON.parse($('#fatGrupoConvenioIndexJson').val());

            var allRows = _$fatGrupoConvenioTable.find('.jtable-data-row')

            $.each(allRows, function () {
                var id = $(this).attr('data-record-key');
                _$fatGrupoConvenioTable.jtable('deleteRecord', { key: id, clientOnly: true });
            });

            for (var i = 0; i < listaFatGrupoConvenio.length; i++) {
                var item = listaFatGrupoConvenio[i];

                _$fatGrupoConvenioTable.jtable('addRecord', {
                    record: item
                    , clientOnly: true
                });
            }
        }

        var lista = [];

        function getIntervalos() {
            lista = JSON.parse($('#intervaloGuiasConveniosIndexJson').val());

            var allRows = _$internavalosTable.find('.jtable-data-row')

            $.each(allRows, function () {
                var id = $(this).attr('data-record-key');
                _$internavalosTable.jtable('deleteRecord', { key: id, clientOnly: true });
            });

            for (var i = 0; i < lista.length; i++) {
                var item = lista[i];

                _$internavalosTable.jtable('addRecord', {
                    record: item
                    , clientOnly: true
                });
            }
        }

        getIntervalos();

        getCodigoCredenciado();

        getFatGrupoConvenio();

        $('#inserir').click(function (e) {
            e.preventDefault();

            // var _$rateioForm = $('form[name=RateioForm]');
            //_$rateioItemInformationsForm.validate();

            //if (!_$rateioItemInformationsForm.valid()) {
            //    return;
            //}

            var intervalo = {};

            if ($('#intervaloGuiasConveniosIndexJson').val() != '') {
                lista = JSON.parse($('#intervaloGuiasConveniosIndexJson').val());
            }

            if ($('#idGridIntervalo').val() != '') {

                for (var i = 0; i < lista.length; i++) {
                    if (lista[i].IdGrid == $('#idGridIntervalo').val()) {

                        var empresa = $('#empresaId').select2('data');
                        if (empresa && empresa.length > 0) {

                            lista[i].EmpresaDescricao = empresa[0].text;
                        }

                        lista[i].EmpresaId = $('#empresaId').val();


                        var guia = $('#guiaId').select2('data');
                        if (guia && guia.length > 0) {

                            lista[i].FaturamentoGuiaDescricao = guia[0].text;
                        }

                        lista[i].FaturamentoGuiaId = $('#guiaId').val();


                        lista[i].Inicio = $('#inicio').val();
                        lista[i].Final = $('#fim').val();
                        lista[i].NumeroGuiasParaAviso = $('#numeroGuiasParaAviso').val();


                        _$internavalosTable.jtable('updateRecord', {
                            record: lista[i]
                            , clientOnly: true
                        });

                    }
                }
            }
            else {
                intervalo.IdGrid = lista.length == 0 ? 1 : lista[lista.length - 1].IdGrid + 1;

                var empresa = $('#empresaId').select2('data');
                if (empresa && empresa.length > 0) {

                    intervalo.EmpresaDescricao = empresa[0].text;
                }

                intervalo.EmpresaId = $('#empresaId').val();

                var guia = $('#guiaId').select2('data');
                if (guia && guia.length > 0) {

                    intervalo.FaturamentoGuiaDescricao = guia[0].text;
                }

                intervalo.FaturamentoGuiaId = $('#guiaId').val();


                intervalo.Inicio = $('#inicio').val();
                intervalo.Final = $('#fim').val();
                intervalo.NumeroGuiasParaAviso = $('#numeroGuiasParaAviso').val();

                lista.push(intervalo);

                _$internavalosTable.jtable('addRecord', {
                    record: intervalo
                    , clientOnly: true
                });

            }

            $('#intervaloGuiasConveniosIndexJson').val(JSON.stringify(lista));

            $('#idGridIntervalo').val('');
            $('#empresaId').val('').trigger('change');
            $('#guiad').val('').trigger('change');
            $('#inicio').val('');
            $('#fim').val('');
            $('#numeroGuiasParaAviso').val('');

            $('#inserir > i').removeClass('fa-check');
            $('#inserir > i').addClass('fa-plus');

        });

        function isSameGridId(a, b) {
            return (a.IdGrid == b.IdGrid);
        }

        function excluirFatGrupoConvenio(e, data) {
            var rowData = $(e.currentTarget).parents("tr").data("record");
            abp.message.confirm(
                app.localize('DeleteWarning', rowData.GrupoDescricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        lista = JSON.parse($('#fatGrupoConvenioIndexJson').val());
                        for (var i = 0; i < lista.length; i++) {
                            if (isSameGridId(lista[i], rowData)) {
                                lista.splice(i, 1);
                                $('#fatGrupoConvenioIndexJson').val(JSON.stringify(lista));
                                break;
                            }
                        }
                        getFatGrupoConvenio();
                    }
                }
            );
        }

        function excluirCodigoCredenciado(e, data) {
            var rowData = $(e.currentTarget).parents("tr").data("record");
            abp.message.confirm(
                app.localize('DeleteWarning', rowData.Codigo),
                function (isConfirmed) {
                    if (isConfirmed) {
                        lista = JSON.parse($('#codigoCredenciadoConveniosIndexJson').val());
                        for (var i = 0; i < lista.length; i++) {
                            if (isSameGridId(lista[i], rowData)) {
                                lista.splice(i, 1);
                                $('#codigoCredenciadoConveniosIndexJson').val(JSON.stringify(lista));
                                break;
                            }
                        }
                        getCodigoCredenciado();
                    }
                }
            );
        }

        $('#inserirCodigoCredenciado').click(function (e) {
            e.preventDefault();

            var empresa = $('#empresaIdCodigoCredenciado').select2('data');
            if (!(empresa && empresa.length > 0)) {
                abp.notify.info(app.localize('EmpresaRequerida'));
                return;
            }

            if ($('#codigoCredenciado').val() == '') {
                abp.notify.info(app.localize('CodigoCredenciadoRequerido'));
                return;
            }

            var codigoCredenciado = {};

            if ($('#codigoCredenciadoConveniosIndexJson').val() != '') {
                listaCodigoCredenciado = JSON.parse($('#codigoCredenciadoConveniosIndexJson').val());
            }

            if ($('#idGridCodigoCredenciado').val() != '') {

                for (var i = 0; i < listaCodigoCredenciado.length; i++) {
                    if (listaCodigoCredenciado[i].IdGrid == $('#idGridCodigoCredenciado').val()) {

                        var empresa = $('#empresaIdCodigoCredenciado').select2('data');
                        if (empresa && empresa.length > 0) {

                            listaCodigoCredenciado[i].EmpresaDescricao = empresa[0].text;
                        }

                        listaCodigoCredenciado[i].Codigo = $('#codigoCredenciado').val();

                        _$codigoCredenciadoTable.jtable('updateRecord', {
                            record: listaCodigoCredenciado[i]
                            , clientOnly: true
                        });
                    }
                }
            }
            else {
                codigoCredenciado.IdGrid = listaCodigoCredenciado.length == 0 ? 1 : listaCodigoCredenciado[listaCodigoCredenciado.length - 1].IdGrid + 1;

                var empresa = $('#empresaIdCodigoCredenciado').select2('data');
                if (empresa && empresa.length > 0) {

                    codigoCredenciado.EmpresaDescricao = empresa[0].text;
                }

                codigoCredenciado.EmpresaId = $('#empresaIdCodigoCredenciado').val();
                codigoCredenciado.Codigo = $('#codigoCredenciado').val();

                listaCodigoCredenciado.push(codigoCredenciado);

                _$codigoCredenciadoTable.jtable('addRecord', {
                    record: codigoCredenciado
                    , clientOnly: true
                });
            }

            $('#codigoCredenciadoConveniosIndexJson').val(JSON.stringify(listaCodigoCredenciado));

            $('#idGridCodigoCredenciado').val('');
            $('#empresaIdCodigoCredenciado').val('').trigger('change');
            $('#codigoCredenciado').val('');

            $('#inserirCodigoCredenciado > i').removeClass('fa-check');
            $('#inserirCodigoCredenciado > i').addClass('fa-plus');
        });

        $('#inserirFatGrupoConvenio').click(function (e) {
            e.preventDefault();

            var empresa = $('#faturamentoGrupoId').select2('data');
            if (!(empresa && empresa.length > 0)) {
                abp.notify.info(app.localize('GrupoRequerido'));
                return;
            }

            var fatGrupoConvenio = {};

            if ($('#fatGrupoConvenioIndexJson').val() != '') {
                listaFatGrupoConvenio = JSON.parse($('#fatGrupoConvenioIndexJson').val());
            }


            if ($('#idGridFatGrupoConvenio').val() != '') {
                for (var i = 0; i < listaFatGrupoConvenio.length; i++) {
                    if (listaFatGrupoConvenio[i].IdGrid == $('#idGridFatGrupoConvenio').val()) {
                        var faturamentoGrupo = $('#faturamentoGrupoId').select2('data');
                        if (faturamentoGrupo && faturamentoGrupo.length > 0) {
                            listaFatGrupoConvenio[i].GrupoDescricao = faturamentoGrupo[0].text;
                        }

                        listaFatGrupoConvenio[i].GrupoId = $('#faturamentoGrupoId').val();
                        listaFatGrupoConvenio[i].IsCobrancaDia = $('#IsCobrancaDia').prop('checked') === true ? 'Sim' : 'Não';

                        _$fatGrupoConvenioTable.jtable('updateRecord', {
                            record: listaFatGrupoConvenio[i]
                            , clientOnly: true
                        });
                    }
                }
            }
            else {
                fatGrupoConvenio.IdGrid = listaFatGrupoConvenio.length == 0 ? 1 : listaFatGrupoConvenio[listaFatGrupoConvenio.length - 1].IdGrid + 1;

                var faturamentoGrupo = $('#faturamentoGrupoId').select2('data');
                if (faturamentoGrupo && faturamentoGrupo.length > 0) {

                    fatGrupoConvenio.GrupoDescricao = faturamentoGrupo[0].text;
                }

                fatGrupoConvenio.GrupoId = $('#faturamentoGrupoId').val();
                fatGrupoConvenio.IsCobrancaDia = $('#IsCobrancaDia').prop('checked') === true ? 'Sim' : 'Não';

                listaFatGrupoConvenio.push(fatGrupoConvenio);

                _$fatGrupoConvenioTable.jtable('addRecord', {
                    record: fatGrupoConvenio
                    , clientOnly: true
                });
            }

            $('#fatGrupoConvenioIndexJson').val(JSON.stringify(listaFatGrupoConvenio));

            $('#idGridFatGrupoConvenio').val('');
            $('#faturamentoGrupoId').val('').trigger('change');

            $('#inserirFatGrupoConvenio > i').removeClass('fa-check');
            $('#inserirFatGrupoConvenio > i').addClass('fa-plus');
        });

        function editInternvalo(intervalo) {

            $('#empresaId')
                .append($("<option>") //add option tag in select
                    .val(intervalo.EmpresaId) //set value for option to post it
                    .text(intervalo.EmpresaDescricao)
                ) //set a text for show in select
                .val(intervalo.EmpresaId) //select option of select2
                .trigger("change");

            $('#guiad')
                .append($("<option>") //add option tag in select
                    .val(intervalo.FaturamentoGuiaId) //set value for option to post it
                    .text(intervalo.FaturamentoGuiaDescricao)
                ) //set a text for show in select
                .val(intervalo.FaturamentoGuiaId) //select option of select2
                .trigger("change");

            $('#idGridIntervalo').val(intervalo.IdGrid);
            $('#inicio').val(intervalo.Inicio);
            $('#fim').val(intervalo.Final);
            $('#numeroGuiasParaAviso').val(intervalo.NumeroGuiasParaAviso);

            $('#inserir > i').removeClass('fa-plus');
            $('#inserir > i').addClass('fa-check');
        }

        selectSW('.selectFormaAutorizacao', "/api/services/app/FormaAutorizacao/ListarDropdown");
        selectSW('.selectEmpresa', "/api/services/app/Empresa/ListarDropdown");
        selectSW('.selectMedico', "/api/services/app/Medico/ListarDropdown");
        selectSW('.selecFaturamentoGrupo', "/api/services/app/FaturamentoGrupo/ListarDropdown");

        selectSW('.selectEspecialidade', "/api/services/app/Especialidade/ListarDropdownPorMedicoTodas", $('#medicoPadraoEmergenciaId'));

        $('#medicoPadraoEmergenciaId').on('change', function (e) {
            e.preventDefault();
            $('#especialidadePadraoEmergenciaId').val(null).trigger("change");
            selectSW('.selectEspecialidade', "/api/services/app/Especialidade/ListarDropdownPorMedicoTodas", $('#medicoPadraoEmergenciaId'));
        });

        $('#medicoPadraoInternacaoId').on('change', function (e) {
            e.preventDefault();
            $('#especialidadePadraoInternacaoId').val(null).trigger("change");
            selectSW('.selectEspecialidade', "/api/services/app/Especialidade/ListarDropdownPorMedicoTodas", $('#medicoPadraoInternacaoId'));
        });

        selectSW('.selectGuia', "/api/services/app/FaturamentoGuia/ListarDropdown");
    };
})(jQuery);