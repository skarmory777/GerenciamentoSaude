
(function ($) {

    $(function () {

        $('.modal-dialog').css('width', '1800px');

        $('#Movimento').on('load', function () {
            var d = new Date();
            var n = d.getDate();
            $('#movimento').val(moment().format("L LT"));
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Suprimentos.Estoque.PreMovimento.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Suprimentos.Estoque.PreMovimento.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Suprimentos.Estoque.PreMovimento.Delete')
        };

        var iValidador = {
            init: function () {
                // Execute seus códigos iniciais
                // ...
                //alert('Entrou no validador agora!');
                // Chame as funções desejadas...
                iValidador.outraFuncao();
            },
            outraFuncao: function () {
                // Códigos desejados...
            }
        };


        var _preMovimentoService = abp.services.app.estoquePreMovimento;
        var _movimentoService = abp.services.app.estoqueMovimento;


        var _estoquePreMovimentoItemService = abp.services.app.estoquePreMovimentoItem;
        var _$EstoquePreMovimentoItemTable = $('#EstoquePreMovimentoItemTable');

        var _createOrEditPreMovimentoItemModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Saidas/CriarOuEditarPreMovimentoItemModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/saidas/_CriarOuEditarPreMovimentoItemModal.js',
            modalClass: 'CriarOuEditarPreMovimentoItemModal'
        });

        var _createOrEditPreMovimentoKitEstoqueItemModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Saidas/CriarOuEditarPreMovimentoKitEstoqueItemModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/saidas/_CriarOuEditarPreMovimentoKitEstoqueItemModal.js',
            modalClass: 'CriarOuEditarPreMovimentoKitEstoqueItemModal'
        });

        var _ErrorModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Erros/ExibirErros',
        });

        $('#btn-novo-PreMovimentoKitEstoqueItem').click(function (e) {
            e.preventDefault()

            var _$preMovimentoInformationsForm = $('form[name=preMovimentoInformationsForm]');

            _$preMovimentoInformationsForm.validate();

            if (!_$preMovimentoInformationsForm.valid()) {
                return;
            }

            var preMovimento = _$preMovimentoInformationsForm.serializeFormToObject();

            if ($('#id').val() == '' || $('#id').val() == '0') {
                _preMovimentoService.criarGetIdSaida(preMovimento)
                    .done(function (data) {
                        abp.notify.info(app.localize('SavedSuccessfully'));
                        $('#id').val(data.id);
                        $('#DocumentoId').val(data.documento);

                        _createOrEditPreMovimentoKitEstoqueItemModal.open({ preMovimentoId: $('#id').val(), estoqueId: $('#EstoqueId').val() });

                    })
                    .always(function () {
                        //  _modalManager.setBusy(false);
                    });
            }
            else {

                _createOrEditPreMovimentoKitEstoqueItemModal.open({ preMovimentoId: $('#id').val(), id: 0, estoqueId: $('#EstoqueId').val() });
            }
        });



        $('#btn-novo-PreMovimentoItem').click(function (e) {
            e.preventDefault()


            var _$preMovimentoInformationsForm = $('form[name=preMovimentoInformationsForm]');

            _$preMovimentoInformationsForm.validate();

            if (!_$preMovimentoInformationsForm.valid()) {
                return;
            }

            var preMovimento = _$preMovimentoInformationsForm.serializeFormToObject();

            var editMode = $('#is-edit-mode').val();

            if ($('#id').val() == '' || $('#id').val() == '0') {

                _preMovimentoService.criarGetIdSaida(preMovimento)
                    .done(function (data) {
                        abp.notify.info(app.localize('SavedSuccessfully'));
                        $('#id').val(data.id);
                        $('#DocumentoId').val(data.documento);

                        _createOrEditPreMovimentoItemModal.open({ preMovimentoId: $('#id').val(), id: 0 });

                    })
                    .always(function () {
                        //  _modalManager.setBusy(false);
                    });
            }
            else {

                _createOrEditPreMovimentoItemModal.open({ preMovimentoId: $('#id').val(), id: 0 });
            }
        });

        $('#salvar-PreMovimento').click(function (e) {
            e.preventDefault()

            var _$preMovimentoInformationsForm = $('form[name=preMovimentoInformationsForm]');

            _$preMovimentoInformationsForm.validate();

            if (!_$preMovimentoInformationsForm.valid()) {
                return;
            }
            var preMovimento = _$preMovimentoInformationsForm.serializeFormToObject();

            if (preMovimento.Emissao) {
                preMovimento.Emissao = moment(preMovimento.Emissao, "DD/MM/YYYY").format();
            }
            var editMode = $('#is-edit-mode').val();

            _preMovimentoService.criarOuEditarSaida(preMovimento)
                .done(function (data) {


                    if (data.errors.length > 0) {
                        _ErrorModal.open({ erros: data.errors });
                    }
                    else {
                        if (data.warnings.length > 0) {
                            swal({
                                title: " ",
                                text: data.warnings[0].descricao,
                                type: "warning",
                                showCancelButton: false,
                                confirmButtonColor: "#DD6B55",
                                confirmButtonText: "Ok",
                                closeOnConfirm: false
                            }, () => {
                                location.href = '/mpa/saidas';
                            });
                        }
                        else {
                            abp.message.confirmHtml("",
                                "Deseja confirmar a saída?",
                                function (isConfirmed) {
                                    if (isConfirmed) {
                                        _movimentoService.gerarMovimentoEntrada(data.returnObject.id)
                                            .done(function (data) {
                                                if (data.errors.length > 0) {
                                                    _ErrorModal.open({ erros: data.errors });
                                                }
                                                else {
                                                    abp.notify.info(app.localize('SavedSuccessfully'));
                                                    location.href = '/mpa/saidas';
                                                }
                                            });
                                    }
                                    else {
                                        location.href = '/mpa/saidas';
                                    }
                                });
                        }
                    }

                })
                .always(function () {
                    //  _modalManager.setBusy(false);
                });
        });

        $('#codigoBarra').on('keypress', function (event) {
            //Tecla 13 = Enter
            debugger;

            if (event.which == 13) {
                event.preventDefault();



                var _$preMovimentoInformationsForm = $('form[name=preMovimentoInformationsForm');

                _$preMovimentoInformationsForm.validate();

                if (!_$preMovimentoInformationsForm.valid()) {
                    return;
                }

                var preMovimento = _$preMovimentoInformationsForm.serializeFormToObject();

                var editMode = $('#is-edit-mode').val();

                if ($('#id').val() == '' || $('#id').val() == '0') {

                    _preMovimentoService.criarGetIdSaida(preMovimento)
                        .done(function (data) {
                            debugger;
                            if (data.id > 0) {

                                abp.notify.info(app.localize('SavedSuccessfully'));
                                $('#id').val(data.id);
                                $('#DocumentoId').val(data.documento);

                                inserirProdutoCodigoBarra();

                            }
                        })
                        .always(function () {
                            //  _modalManager.setBusy(false);
                        });
                }
                else {
                    inserirProdutoCodigoBarra();
                }
            }


        });


        function inserirProdutoCodigoBarra() {


            var estoquePreMovimentoItemAppService = abp.services.app.estoquePreMovimentoItem;
            estoquePreMovimentoItemAppService.criarSaidaPorCodigoBarra($('#codigoBarra').val(), $('#EstoqueId').val(), $('#id').val(), $('#quantidade').val())
                .done(function (data) {
                    debugger;
                    if (data.errors.length > 0) {
                        _ErrorModal.open({ erros: data.errors });
                    }
                    else {

                        if (data.warnings.length > 0) {
                            _ErrorModal.open({ erros: data.warnings });
                        }

                        $('#codigoBarra').val('');
                        $('#quantidade').val('1');
                        $('#codigoBarra').focus();

                        getEstoquePreMovimentoItemTable();
                    }

                });

        }



        function retirarMascara(valor) {

            while (valor.indexOf('_') != -1) valor = valor.replace('_', '');
            while (valor.indexOf('.') != -1) valor = valor.replace('.', '');

            valor = valor.replace(',', '.');

            return valor;
        }

        function salvar(e) {

        }

        abp.event.on('app.CriarOuEditarPreMovimentoKitEstoqueItemModalSaved', function () {
            getEstoquePreMovimentoItemTable();
        });

        abp.event.on('app.CriarOuEditarPreMovimentoItemModalSaved', function () {
            getEstoquePreMovimentoItemTable();
        });

        var _estoquePreMovimentoService = abp.services.app.estoquePreMovimento;

        var _modalManager;

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };

        $('.close').on('click', function () {
            location.href = '/mpa/saidas';
        });

        $('.close-button').on('click', function () {
            const button = $(this);
            $(button).buttonBusy(true);

            if ($('#id').val() !== '' && $('#id').val() !== '0') {
                _estoquePreMovimentoService.listarItensSaida({ filtro: $('#id').val() })
                    .done(function (result) {
                        if (result.totalCount === 0) {
                            _preMovimentoService.excluir($('#id').val())
                                .done(function () {
                                    location.href = '/mpa/saidas';
                                });
                        } else {
                            location.href = '/mpa/saidas';
                        }
                    });
            } else {
                location.href = '/mpa/saidas';
            }
        });

        _$EstoquePreMovimentoItemTable.jtable
            ({
                title: app.localize('Item'),
                paging: true,
                sorting: true,
                edit: false,
                create: false,
                multiSorting: true,
                actions:
                {
                    listAction:
                    {
                        method: _estoquePreMovimentoService.listarItensSaida
                    },
                },
                fields:
                {
                    id: {
                        key: true,
                        list: false
                    },
                    actions: {
                        title: app.localize('Actions'),
                        width: '8%',
                        sorting: false,
                        display: function (data) {
                            var $span = $('<span></span>');

                            if (_permissions.edit && $('#PreMovimentoEstadoId').val() != 2) {
                                $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                    .appendTo($span)
                                    .click(function (e) {
                                        e.preventDefault();
                                        _createOrEditPreMovimentoItemModal.open({ id: data.record.id, preMovimentoId: $('#id').val(), estoqueId: $('#EstoqueId').val() });
                                    });
                            }

                            if (_permissions.delete && $('#PreMovimentoEstadoId').val() != 2) {

                                $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                    .appendTo($span)
                                    .click(function (e) {
                                        e.preventDefault();
                                        deletePreMovimentoItem(data.record);
                                    });
                            }
                            return $span;
                        }
                    },
                    PreMovimentoId: {
                        type: 'hidden',
                        defaultValue: function (data) {
                            return $('#id').val();
                        },
                    },
                    ProdutoId: {
                        title: app.localize('Produto'),
                        width: '30%',
                        display: function (data) {
                            if (data.record.produto) {
                                return data.record.produto;
                            }
                        }
                    },

                    Unidade: {
                        title: app.localize('Unidade'),
                        width: '15%',
                        display: function (data) {
                            if (data.record.unidade) {
                                return data.record.unidade;
                            }
                        }
                    },

                    quantidade: {
                        title: app.localize('Quantidade'),
                        width: '7%',
                        display: function (data) {
                            if (data.record.quantidade) {
                                return data.record.quantidade;
                            }
                        }
                    },

                    Lote: {
                        title: app.localize('Lote'),
                        width: '8%',
                        display: function (data) {
                            if (data.record.lote) {
                                return data.record.lote;
                            }
                        }
                    },

                    Validade: {
                        title: app.localize('Validade2'),
                        width: '8%',
                        display: function (data) {
                            if (data.record.validade && moment(data.record.validade).format('DD/MM/YYYY') != '01/01/0001') {
                                return moment(data.record.validade).format('DD/MM/YYYY');
                            }
                        }
                    }
                    ,

                    Laboratorio: {
                        title: app.localize('Laboratorio'),
                        width: '30%',
                        display: function (data) {
                            if (data.record.laboratorio) {
                                return data.record.laboratorio;
                            }
                        }
                    }


                }
            });


        function deletePreMovimentoItem(preMovimentoItem) {

            if (preMovimentoItem.estoqueKitItemId !== null) {
                abp.message.confirmHtml("",
                    app.localize('ExcluirItensDoKitEstoque'),
                    function (isConfirmed) {
                        if (isConfirmed) {
                            _estoquePreMovimentoItemService.excluirTodosItensKitEstoque(preMovimentoItem.id, preMovimentoItem.estoqueKitItemId)
                                .done(function () {
                                    getEstoquePreMovimentoItemTable(true);
                                    abp.notify.success(app.localize('SuccessfullyDeleted'));
                                });
                        }
                        else {
                            _estoquePreMovimentoItemService.excluir(preMovimentoItem.id)
                                .done(function () {
                                    getEstoquePreMovimentoItemTable(true);
                                    abp.notify.success(app.localize('SuccessfullyDeleted'));
                                });
                        }
                    });
            } else {
                abp.message.confirm(
                    app.localize('DeleteWarning', preMovimentoItem.produto.descricao),
                    function (isConfirmed) {
                        if (isConfirmed) {
                            _estoquePreMovimentoItemService.excluir(preMovimentoItem.id)
                                .done(function () {
                                    getEstoquePreMovimentoItemTable(true);
                                    abp.notify.success(app.localize('SuccessfullyDeleted'));
                                });
                        }
                    }
                );
            }
        }

        function getEstoquePreMovimentoItemTable(reload) {

            if (reload) {
                _$EstoquePreMovimentoItemTable.jtable('reload');
            } else {
                _$EstoquePreMovimentoItemTable.jtable('load', { filtro: $('#id').val() });
            }
        }


        getEstoquePreMovimentoItemTable();

        $('#EstTipoMovimentoId').change(function () {
            configurarCampos();
        });
        configurarCampos();
        function configurarCampos() {

            var valor = $('#EstTipoMovimentoId').val();

            if (valor == '2') {
                selectSW('.selectUnidadeOrganizacional', "/api/services/app/unidadeOrganizacional/ListarDropdownEstoque");
            }
            else {
                selectSW('.selectUnidadeOrganizacional', "/api/services/app/unidadeOrganizacional/ListarDropdown");
            }

            if (valor == '3' || valor == '5') {
                $('#grupoOrganizacional').hide();
                $('#fornecedor').hide();
                $('#grupoOrganizacional').val('');
                $('#paciente').show();
                $('#medico').show();
                $('#atendimento').show();
                $('#tipoAtendimento').show();
            }
            else if (valor == '6') {
                $('#fornecedor').show();
                $('#paciente').hide();
                $('#medico').hide();
                $('#atendimento').hide();
                $('#tipoAtendimento').hide();

                $('#paciente').val('');
                $('#medico').val('');
                $('#atendimento').val('');
                $('#grupoOrganizacional').hide();
                $('#UnidadeOrganizacionalId').val('');

            }

            else {
                $('#grupoOrganizacional').show();
                $('#paciente').hide();
                $('#medico').hide();
                $('#atendimento').hide();
                $('#tipoAtendimento').hide();


                $('#paciente').val('');
                $('#medico').val('');
                $('#atendimento').val('');
                $('#fornecedor').hide();
                $('#tipoAtendimento').val('');
            }


            if (valor == '2') {
                $("#grupoOrganizacional label").html("Setor Destino");
            }
            else {
                $("#grupoOrganizacional label").html("Unidade");
            }

            if (valor == '4') {
                $('#motivoPerdaId').show();

                $("#grupoOrganizacional").hide();
                $('.selectUnidadeOrganizacional').val('');
            }
            else {
                $("#grupoOrganizacional").show();
                $('#motivoPerdaId').hide();
                $('#motivoPerdaId').val('');
            }

        }

        $('#AtendimentoId').change(function () {

            var valor = $('#AtendimentoId').val();

            if (valor == '' || valor == '0') {
                $("#PacienteId").attr("disabled", false).change();
                $("#MedicoSolcitanteId").attr("disabled", false).change();
            }
            else {
                $.ajax({
                    url: "/mpa/Saidas/SelecionarAtendimento/" + valor,
                    success: function (data) {

                        //$("#PacienteId").val(data.PacienteId).change()
                        //                    .selectpicker('refresh');



                        $('#PacienteId').append($("<option>").val(data.PacienteId)
                            .text(data.PacienteNome)
                        )
                            .val(data.PacienteId)
                            .trigger("change");


                        $('#MedicoSolcitanteId').append($("<option>").val(data.MedicoId)
                            .text(data.MedicoNome)
                        )
                            .val(data.MedicoId)
                            .trigger("change");

                        $('#UnidadeOrganizacionalId').append($("<option>").val(data.UnidadeOrganizacionalId).text(data.UnidadeOrganizacional))
                            .val(data.UnidadeOrganizacionalId)
                            .trigger("change").change();




                        //$("#MedicoSolcitanteId").val(data.MedicoId).change()
                        //                   .selectpicker('refresh');

                        $("#PacienteId").attr("disabled", true).change();
                        $("#MedicoSolcitanteId").attr("disabled", true).change();


                    }

                });
            }


        });

        $('input[name="Emissao"]').daterangepicker({
            "singleDatePicker": true,
            "showDropdowns": true,
            autoUpdateInput: false,
            //  maxDate: new Date(),
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

        $('#TipoAtendimentoId').on('change', function () {
            selectSWMultiplosFiltros('.selectAtendimento', "/api/services/app/atendimento/ListarAtendimentosEmAbertoDropdown", ['TipoAtendimentoId', 'UnidadeOrganizacionalId']);
        });

        var _imprimirEntrada = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/RelatorioEntrada'

        });

        $('#btnImprimir').on('click', function (e) {

            _imprimirEntrada.open({ preMovimentoId: $('#id').val() });
        });

        selectSWMultiplosFiltros('.selectAtendimento', "/api/services/app/atendimento/ListarAtendimentosEmAbertoDropdown", ['TipoAtendimentoId', 'UnidadeOrganizacionalId']);

        selectSW('.selectUnidadeOrganizacional', "/api/services/app/unidadeOrganizacional/ListarDropdown");
        selectSW('.selectEstoque', "/api/services/app/estoque/ResultDropdownList");
        selectSW('.selectTipoSaida', "/api/services/app/tipomovimento/ListarDropdownSaida");
        selectSW('.selectEmpresa', "/api/services/app/empresa/ListarDropdown");
        selectSW('.selectFornecedor', "/api/services/app/Fornecedor/ListarDropdown");
        selectSW('.selectMedico', "/api/services/app/Medico/ListarDropdown");


    });





})(jQuery);