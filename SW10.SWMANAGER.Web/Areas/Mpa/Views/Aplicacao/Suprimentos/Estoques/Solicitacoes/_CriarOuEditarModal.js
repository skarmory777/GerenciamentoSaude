
(function ($) {
    $(function () {
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
            height: 150,
            padding: 15,
            disableResizeEditor: true
        };

        $.validator.addMethod('greater-than', function (value, el, param) {
            return value > param;
        }, jQuery.validator.format("Valor precisar ser maior que {0}"));

        $(document).ready(function () {

            $.summernote.options.lineHeights = ["0", "0.2", "0.4", "0.6", "0.8", "1.0"];
            $('.text-editor').summernote(summerNoteOptions);

            CamposRequeridos();

        });

        var _modalManager;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            atendimentoChange();
        };



        $('.modal-dialog').css('width', '1800px');

        $('#empresa-search').autocomplete({
            minLength: 2,
            delay: 0,
            source: function (request, response) {
                var term = $('#empresa-search').val();
                var url = '/mpa/empresas/autocompleteDescricao';


                var fullUrl = url + '/?term=' + term;
                $.getJSON(fullUrl, function (data) {
                    if (data.result.length == 0) {
                        $('#empresa-Id').val(0);
                        $("#empresa-search").focus();
                        abp.notify.info(app.localize("ListaVazia"));
                        return false;
                    };
                    response($.map(data.result, function (item) {
                        $('#empresa-Id').val(0);
                        return {
                            label: item.nome,
                            value: item.nome,
                            realValue: item.id
                        };
                    }));
                });
            },
            select: function (event, ui) {
                $('#empresa-Id').val(ui.item.realValue);
                $('#empresa-search').val(ui.item.value);
                //$('.save-button').removeAttr('disabled');
                return false;
            },
            change: function (event, ui) {
                event.preventDefault();
                if (ui.item == null) {
                    //$('.save-button').attr('disabled', 'disabled');
                    $('#empresa-Id').val(0);
                    $("#empresa-search").val('').focus();
                    abp.notify.info(app.localize("EstadoInvalido"));
                    return false;
                }
            },
        });

        $('#Movimento').on('load', function () {
            var d = new Date();
            var n = d.getDate();
            $('#movimento').val(moment().format("L LT"));
        });


        var _preMovimentoService = abp.services.app.estoquePreMovimento;
        var _estoquePreMovimentoService = abp.services.app.estoquePreMovimento;
        var _estoquePreMovimentoItemService = abp.services.app.estoquePreMovimentoItem;
        var _unidadeService = abp.services.app.unidade;
        var _estoqueKitItemService = abp.services.app.estoqueKitItem;
        var _$EstoquePreMovimentoItemTable = $('#EstoquePreMovimentoItemTable');

        var _createOrEditPreMovimentoItemModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Solicitacao/CriarOuEditarPreMovimentoItemModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/Solicitacoes/_CriarOuEditarPreMovimentoItemModal.js',
            modalClass: 'CriarOuEditarPreMovimentoItemModal'
        });

        var _ErrorModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Erros/ExibirErros',
        });

        var _createOrEditPreMovimentoKitEstoqueItemModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Solicitacao/CriarOuEditarPreMovimentoKitEstoqueItemModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/Solicitacoes/_CriarOuEditarPreMovimentoKitEstoqueItemModal.js',
            modalClass: 'CriarOuEditarPreMovimentoKitEstoqueItemModal'
        });

        $('#btn-novo-PreMovimentoItem').click(function (e) {
            e.preventDefault()
            _createOrEditPreMovimentoItemModal.open({ id: 0 });
        });

        $('#salvar-PreMovimento').click(function (e) {
            e.preventDefault()
            const button = $(this);
            button.buttonBusy(true);
            $("#produtoId").removeAttr("required");
            $("#QuantidadeItemid").removeAttr("required");
            $("#QuantidadeItemid").removeData();
            $("#ProdutoUnidadeId").removeAttr("required");
            CamposRequeridos();
            $('form[name=preMovimentoInformationsForm]').validate();

            var _$preMovimentoInformationsForm = $('form[name=preMovimentoInformationsForm]');

            _$preMovimentoInformationsForm.validate();

            if (!_$preMovimentoInformationsForm.valid()) {
                button.buttonBusy(false);
                return;
            }

            var preMovimento = _$preMovimentoInformationsForm.serializeFormToObject();
            if (preMovimento.Emissao) {
                preMovimento.Emissao = moment(preMovimento.Emissao, "DD/MM/YYYY").format();
            }
            var obs = $("#observacao").summernote("code");
            try {
                preMovimento.Observacao = $(obs).text();
            }
            catch{
                preMovimento.Observacao = $("<div>"+obs+"</div>").text();
            }

            var editMode = $('#is-edit-mode').val();

            _preMovimentoService.criarOuEditarSolicitacao(preMovimento)
                .done(function (data) {
                    if (data.errors.length > 0) {
                        _ErrorModal.open({ erros: data.errors });
                    }
                    else {
                        abp.notify.info(app.localize('SavedSuccessfully'));
                        $('#id').val(data.returnObject.id);
                        //$("#itens").val(data.returnObject.itens);
                        $("#isEntrada").val(data.returnObject.isEntrada);
                        $("#documento").val(data.returnObject.documento);
                        
                        abp.services.app.estoquePreMovimentoItem.obterItensSolicitacaoPorPreMovimento($('#id').val())
                            .done(function (res) {
                            $("#itens").val(JSON.stringify(res));
                            getEstoquePreMovimentoItemTable();
                        });
                        $('.modal-imprimir').modal('toggle');
                    }
                })
                .always(function () {
                    button.buttonBusy(false);
                });
        });

        $('input[name="Movimento"]').daterangepicker({
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
                $('input[name="Movimento"]').val(selDate.format('L')).addClass('form-control edited');
                // obterIdade(selDate);
            });

        $('input[name="Emissao"]').daterangepicker({
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
                $('input[name="Emissao"]').val(selDate.format('L')).addClass('form-control edited');
            });

        $('input[name="HoraPrescrita"]').daterangepicker({
            "singleDatePicker": true,
            "showDropdowns": true,
            autoUpdateInput: false,
            // maxDate: new Date(),
            changeYear: true,
            yearRange: 'c-10:c+10',
            showOn: "both",
            timePicker: true,
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
                $('input[name="HoraPrescrita"]').val(selDate.format('L HH:mm')).addClass('form-control edited');
            });

        abp.event.on('app.CriarOuEditarPreMovimentoItemModalSaved', function () {
            getEstoquePreMovimentoItemTable();
        });


        $('.close-button').on('click', function () {
            location.href = '/mpa/Solicitacao';
        });

        function retornarLista(filtro) {
            if ($('#itens').val() != '') {
                var js = $('#itens').val();
                var res = _preMovimentoService.listarItensJson({ data: js });  //  '"{Result":"OK","Records":' + js + '}'
                return res;
            }
            else {
                var res = _preMovimentoService.listarItens({ filtro: $('#id').val() });
                return res;
            }
        }

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
                        method: retornarLista
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
                        width: '7%',
                        sorting: false,
                        display: function (data) {
                            var $span = $('<span></span>');

                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                .appendTo($span)
                                .click(function (e) {
                                    e.preventDefault();
                                    //_createOrEditPreMovimentoItemModal.open({ item: JSON.stringify(data.record) });
                                    editPreMovimentoItem(data.record);
                                });

                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                .appendTo($span)
                                .click(function (e) {
                                    e.preventDefault();
                                    deletePreMovimentoItem(data.record);
                                });

                            if (data.record.isValidade || data.record.isLote) {
                                $('<button class="btn btn-default btn-xs" title="' + app.localize('LoteValidade') + '"><i class="fa fa-calendar"></i></button>')
                                    .appendTo($span)
                                    .click(function (e) {
                                        e.preventDefault();
                                        abrirPreMovimentoItemLoteValidade(data.record);
                                    });
                            }

                            return $span;
                        }
                    },
                    estadoSolicitacaoItemId: {
                        title: app.localize('Status'),
                        width: '6%',
                        display: function (data) {
                            switch (data.record.estadoSolicitacaoItemId) {
                                case 1: {
                                    return '<span class="label label-info">' + app.localize('Aguardando Confirmação') + '</span>';
                                }
                                case 2: {
                                    return '<span class="label label-success">' + app.localize('Confirmado') + '</span>';
                                }
                                case 3: {
                                    return '<span class="label label-info">' + app.localize('Pendente informação') + '</span>';
                                }
                                case 4: {
                                    return '<span class="label label-info">' + app.localize('Pendente') + '</span>';
                                }
                                case 5: {
                                    return '<span class="label label-warning">' + app.localize('Parcialmente Atendido') + '</span>';
                                }
                                case 6: {
                                    return '<span class="label label-success">' + app.localize('Totalmente Atendido') + '</span>';
                                }
                                case 7: {
                                    return '<span class="label label-danger">' + app.localize('Parcialmente Suspensa') + '</span>';
                                }
                                case 8: {
                                    return '<span class="label label-danger">' + app.localize('Suspensa') + '</span>';
                                }
                                default: {
                                    return '';
                                }
                            }
                        }
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

                    quantidade: {
                        title: app.localize('Quantidade'),
                        width: '10%',
                        display: function (data) {
                            if (data.record.quantidade) {
                                return posicionarDireita(data.record.quantidade);
                            }
                        }
                    },
                    Unidade: {
                        title: app.localize('Unidade'),
                        width: '20%',
                        display: function (data) {
                            if (data.record.produtoUnidade) {
                                return data.record.produtoUnidade;
                            }
                        }
                    },
                }
            });

        function deletePreMovimentoItem(preMovimentoItem) {
            if (preMovimentoItem.estoqueKitItemId !== null) {
                abp.message.confirmHtml("",
                    app.localize('ExcluirItensDoKitEstoque'),
                    function (isConfirmed) {
                        removerPreMovimentoItem(preMovimentoItem, isConfirmed);
                    });

            } else {
                abp.message.confirm(
                    app.localize('DeleteWarning', preMovimentoItem.produto),
                    function (isConfirmed) {
                        if (isConfirmed) {
                            removerPreMovimentoItem(preMovimentoItem, false);
                        }
                    }
                );
            }
        }

        function removerPreMovimentoItem(preMovimentoItem, removerKitEstoque) {
            if (removerKitEstoque) {
                _estoqueKitItemService.obterItensKit(preMovimentoItem.estoqueKitItemId, { async: false })
                    .done(function (estoqueKitItens) {
                        for (var i = 0; i < estoqueKitItens.length; i++) {
                            var lista = JSON.parse($('#itens').val());
                            const index = _.findIndex(lista, (x) => x.EstoqueKitItemId !== null && x.EstoqueKitItemId === parseInt(estoqueKitItens[i].id));
                            if (index !== -1) {
                                lista.splice(index, 1);
                                if (lista && lista.length) {
                                    idGrid = 1;
                                    _.forEach(lista, (x) => x.IdGrid = x.idGrid = idGrid++);
                                }
                                $('#itens').val(JSON.stringify(lista));
                            }
                        }
                    });
            }
            else {
                lista = JSON.parse($('#itens').val());

                const index = _.findIndex(lista, (x) => x.IdGrid === parseInt(preMovimentoItem.idGrid) || x.idGrid === parseInt(preMovimentoItem.idGrid));
                if (index !== -1) {
                    lista.splice(index, 1);
                    if (lista && lista.length) {
                        idGrid = 1;
                        _.forEach(lista, (x) => x.IdGrid = x.idGrid = idGrid++);
                    }
                    $('#itens').val(JSON.stringify(lista));
                }
            }

            getEstoquePreMovimentoItemTable();
        }


        function editPreMovimentoItem(preMovimentoItem) {
            $('#produtoId').append($("<option/>") //add option tag in select
                .val(preMovimentoItem.produtoId) //set value for option to post it
                .text(preMovimentoItem.produto)) //set a text for show in select
                .val(preMovimentoItem.produtoId) //select option of select2
                .trigger("change");

            $('#QuantidadeItemid').val(preMovimentoItem.quantidade);
            $('#idGrid').val(preMovimentoItem.idGrid);
            $("#PreMovimentoItemId").val(preMovimentoItem.id);

            $('#ProdutoUnidadeId').append($("<option/>") //add option tag in select
                .val(preMovimentoItem.produtoUnidadeId) //set value for option to post it
                .text(preMovimentoItem.produtoUnidade)) //set a text for show in select
                .val(preMovimentoItem.produtoUnidadeId).trigger("change");

        }

        function getEstoquePreMovimentoItemTable(reload) {

            if (reload) {
                _$EstoquePreMovimentoItemTable.jtable('reload');
            } else {
                _$EstoquePreMovimentoItemTable.jtable('load', { filtro: $('#id').val() });//, entradaConfirmada: $('#entradaConfirmadaId').val() });
            }
        }

        getEstoquePreMovimentoItemTable();

        function configurarCampos() {

            var valor = $('#EstTipoMovimentoId').val();
            var tipoOperacaoId = $("#EstTipoOperacaoId").val();

            if (valor == '2') {
                selectSW('.selectUnidadeOrganizacional', "/api/services/app/unidadeOrganizacional/ListarDropdownEstoque");
            }
            else {
                selectSW('.selectUnidadeOrganizacional', "/api/services/app/unidadeOrganizacional/ListarDropdownComAtendimentoPorUsuario");
            }

            selectSW('.selectProduto', "/api/services/app/produto/ListarProdutoPorEstoqueIdDropdown", $('.selectEstoque'));

            $(".selectAtendimento").removeAttr("required");
            $(".selectAtendimento").parent().find('label[for="atendimentoId"] .required-label').remove();
            $("#UnidadeOrganizacionalId").removeAttr("required");
            if (valor == '3') {
                if (tipoOperacaoId == '4') {
                    selectSWMultiplosFiltros('.selectProduto', "/api/services/app/produto/ListarProdutoSaidaPorEstoqueIdEAtendimentoDropdown", ['.selectEstoque', '.selectAtendimento']);
                }

                $('#grupoOrganizacional').hide();
                $('#grupoOrganizacional').val('');
                $('#paciente').show();
                $('#medico').show();
                $('#atendimento').show();
                $(".selectAtendimento").attr("required", "required");
                $("#atendimento-tipo").show();
                $('#divHoraPrescrita').show();
                if ($(".selectTipoSolicitacao").val() != "4") {
                    $('#checkAtendimento').show();
                }
            }
            else {
                $('#grupoOrganizacional').show();
                $('#paciente').hide();
                $('#medico').hide();
                $('#atendimento').hide();
                $("#atendimento-tipo").hide();
                $('#divHoraPrescrita').hide();
                $('#checkAtendimento').hide();
                $('#paciente').val('');
                $('#medico').val('');
                $('#atendimento').val('');
                $('#horaPrescrita').val('');
            }

            if (valor == '2') {
                if (tipoOperacaoId == '4') {
                    selectSWMultiplosFiltros('.selectProduto', "/api/services/app/produto/ListarProdutoSaidaPorEstoqueIdESetorDropdown", ['.selectEstoque', '.selectUnidadeOrganizacional']);
                }

                $("#grupoOrganizacional label").html("Setor");
                $("#UnidadeOrganizacionalId").attr("required", "required");
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

            CamposRequeridos();
        }

        $('#atendimentoId').change(function () {
            atendimentoChange();
        });

        function atendimentoChange() {

            var valor = $('#atendimentoId').val();

            if (valor == '' || valor == '0') {
                // $("#MedicoSolcitanteId").attr("disabled", false).change();
                $("#divMedico").removeClass('hidden');
                $("#medicoSolcitante").addClass('hidden');
                $("#pacienteInputId").addClass('hidden');
                $("#divPaciente").removeClass('hidden');

            }
            else {
                $.ajax({
                    url: "/mpa/Saidas/SelecionarAtendimento/" + valor,
                    success: function (data) {
                        $("#pacienteInputId").removeClass('hidden');
                        $("#divPaciente").addClass('hidden');
                        //$("#MedicoSolcitanteId").val(data.MedicoId).change()
                        //                   .selectpicker('refresh');

                        //  $("#MedicoSolcitanteId").attr("disabled", true).change();

                        $("#medicoSolcitante").removeClass('hidden');
                        $("#divMedico").addClass('hidden');
                        $("#pacienteInputId").val(data.PacienteNome);//.CodigoPaciente + ' - ' + data.Paciente.NomeCompleto);
                        $("#medicoSolcitante").val(data.MedicoNome);

                        $('#UnidadeOrganizacionalId').append($("<option>").val(data.UnidadeOrganizacionalId).text(data.UnidadeOrganizacional))
                            .val(data.UnidadeOrganizacionalId)
                            .trigger("change").change();

                        $("#pacienteInputId").attr("disabled", true).change();
                        $("#medicoSolcitante").attr("disabled", true).change();
                    }
                });
            }
        }

        $('#produtoId').on('select2:select', function () {
            if ($('#produtoId').val() != null && $('#produtoId').val() != '') {
                abp.services.app.produto.obterUnidadePorProduto($('#produtoId').val())
                    .done(function (data) {
                        if (data.items.length == 1) {
                            $('#ProdutoUnidadeId').append($("<option>").val(data.items[0].id).text(data.items[0].descricao)).val(data.items[0].id).trigger("change");
                            return;
                        }
                        $('#ProdutoUnidadeId').val(null).trigger("change");
                    });

            }
            else {
                $('#ProdutoUnidadeId').val(null).trigger("change");
            }

            selectSWMultiplosFiltros('.selectProdutoUnidade', '/api/services/app/ProdutoUnidade/ListarUnidadePorProdutoDropdown', ['produtoId']);
            $('#QuantidadeItemid').focus();
        });

        $('#ProdutoUnidadeId').on('select2:select', function () {
            $('#salvar-PreMovimento-Item').focus();
        });

        $('#QuantidadeItemid').keypress(function (e) {
            if (e.which == 13) {
                $('#ProdutoUnidadeId').focus();
                return false;
            }
        });

        var lista = [];

        $('#salvar-PreMovimento-Item').click(function (e) {
            e.preventDefault();
            const button = $(this);
            $(button).buttonBusy(true);
            var form = $('form[name=preMovimentoInformationsForm]');
            $("#produtoId").attr("required","required");
            $("#QuantidadeItemid").attr("required", "required");
            $("#ProdutoUnidadeId").attr("required", "required");
            $("#QuantidadeItemid").data("rule-number", true).data("rule-greater-than", 0);
            CamposRequeridos();
            $('form[name=preMovimentoInformationsForm]').validate();
            if (!$('form[name=preMovimentoInformationsForm]').valid()) {
                $(button).buttonBusy(false);
                return;
            }

            var preMovimentoItem = {}
            preMovimentoItem.Id = $("#PreMovimentoItemId").val();
            preMovimentoItem.IdGrid = $('#idGrid').val();
            preMovimentoItem.Quantidade = retirarMascara($('#QuantidadeItemid').val());

            _unidadeService.obterQuantidadeReferencia($('#ProdutoUnidadeId').val(), preMovimentoItem.Quantidade)
                .done(function (data) {
                    debugger;
                    preMovimentoItem.Quantidade = data;
                    if ($('#itens').val() != '') {
                        lista = JSON.parse($('#itens').val());
                    }

                    if ($('#idGrid').val() != '') {
                        var index = _.findIndex(lista, (x) => x.IdGrid === parseInt($('#idGrid').val()) || x.idGrid === parseInt($('#idGrid').val()));
                        if (index !== -1) {
                            lista[index]["QuantidadeSolicitada"] = lista[index]["quantidade"] = lista[index]["Quantidade"] = preMovimentoItem.Quantidade;
                            lista[index]["ProdutoId"] = lista[index]["produtoId"] = $('#produtoId').val();
                            //  lista[i].NumeroSerie = $('#NumeroSerie').val();
                            lista[index].ProdutoUnidadeId = lista[index]["produtoUnidadeId"] = $('#ProdutoUnidadeId').val();
                        }
                    }
                    else
                    {
                        if (lista.length == 0) {
                            preMovimentoItem.IdGrid = preMovimentoItem.idGrid = 1;
                        }
                        else {
                            const valor = (lista[lista.length - 1].IdGrid ?? lista[lista.length - 1].idGrid) + 1;
                            preMovimentoItem.IdGrid = preMovimentoItem.idGrid = valor;
                        }

                        preMovimentoItem.Id = 0;
                        preMovimentoItem["ProdutoId"] = preMovimentoItem["ProdutoId"] = $('#produtoId').val();
                        preMovimentoItem["ProdutoUnidadeId"] = preMovimentoItem["ProdutoUnidadeId"] = $('#ProdutoUnidadeId').val();
                        lista.push(preMovimentoItem);
                    }
                    if (lista && lista.length) {
                        idGrid = 1;
                        _.forEach(lista, (x) => x.IdGrid = x.idGrid = idGrid++);
                    }
                    debugger;
                    $('#itens').val(JSON.stringify(lista));
                })
                .always(function () {
                    $("#PreMovimentoItemId").val(0);
                    $('#produtoId').val(null).trigger("change");
                    $('#QuantidadeItemid').val('');
                    $('#idGrid').val('');
                    $('#ProdutoUnidadeId').val(null).trigger("change");
                    getEstoquePreMovimentoItemTable();
                    $('#produtoId').focus();

                    $("#produtoId").removeAttr("required");
                    $("#QuantidadeItemid").removeAttr("required");
                    $("#ProdutoUnidadeId").removeAttr("required");
                    $("#QuantidadeItemid").removeData();
                    CamposRequeridos();
                    $('form[name=preMovimentoInformationsForm]').validate();
                    $(button).buttonBusy(false);
                });
        });

        $('#btn-novo-PreMovimentoKitEstoqueItem').click(function (e) {
            e.preventDefault();

            CamposRequeridos();

            $('form[name=preMovimentoInformationsForm]').validate();
            if (!$('form[name=preMovimentoInformationsForm]').valid()) {
                return;
            }

            _createOrEditPreMovimentoKitEstoqueItemModal.open();
        });

        function retirarMascara(valor) {

            while (valor.indexOf('_') != -1) valor = valor.replace('_', '');
            while (valor.indexOf('.') != -1) valor = valor.replace('.', '');

            valor = valor.replace(',', '.');

            return valor;
        }

        $('#ckAtendimento').on('click', function (e) {

            var checkbox = e.target;
            if (checkbox.checked) {
                selectSWMultiplosFiltros('.selectAtendimento', "/api/services/app/Atendimento/ListarDropdown", ['UnidadeOrganizacionalId']);
            }
            else {
                selectSWMultiplosFiltrosCheckbox('.selectAtendimento', "/api/services/app/Atendimento/ListarAtendimentosEmAbertoDropdown");
            }

        });

        $('#ckTodosPacientes').on('click', function (e) {
            var checkbox = e.target;
            if (checkbox.checked) {
                selectSW('.selectPaciente', "/api/services/app/Paciente/ListarDropdown");
            }
            else {
                selectSW('.selectPaciente', "/api/services/app/Atendimento/ListarPacientesSemAlta");
            }

        });

        function fecharAction() {
            $('.modal-imprimir').modal('hide');
            // location.href = '/mpa/ConfirmacaoSolicitacoes';
        }

        $('.modal-imprimir').on('hidden.bs.modal', function () {

        });
        function naoImprimir() {
            fecharAction();
        }

        $('.naoImprimir').on("click", naoImprimir);

        function voltarAction() {
            location.href = '/mpa/Solicitacao';
        }

        $('.btn-voltar').on("click", voltarAction);


        $(".internacao").click(function () {
            if ($(this).prop("checked")) {
                $(".emergencia").prop("checked", null);
            }
        });

        $(".emergencia").click(function () {
            if ($(this).prop("checked")) {
                $(".internacao").prop("checked", null);
            }
        });

        function imprimirAction() {
            imprimir($("#id").val());
        }
        $('.imprimir-tudo').on("click", imprimirAction);

        function imprimir(id) {
            fecharAction();
            $.removeCookie("XSRF-TOKEN");
            printJS({
                printable: '/Mpa/Solicitacao/imprimirSolicitacao?preMovimentoId=' + id, type: 'pdf', showModal: false,
                onPrintDialogClose: () => {
                    voltarAction();
                }
            });

        }



        selectSW('.selectForncedor', "/api/services/app/fornecedor/ListarDropdown");
        selectSWMultiplosFiltrosCheckbox('.selectAtendimento', "/api/services/app/Atendimento/ListarAtendimentosEmAbertoDropdown");
        selectSW('.selectPaciente', "/api/services/app/Atendimento/ListarPacientesSemAlta");
        selectSW('.selectMedico', "/api/services/app/Medico/ListarDropdown");
        selectSW('.selectProduto', "/api/services/app/produto/ListarProdutoPorEstoqueIdDropdown", $('.selectEstoque'));
        selectSWMultiplosFiltros('.selectProdutoUnidade', '/api/services/app/ProdutoUnidade/ListarUnidadePorProdutoDropdown', ['produtoId']);
        selectSW('.selectEmpresa', "/api/services/app/empresa/ListarDropdownPorUsuario");
        selectSW('.selectEstoque', "/api/services/app/estoque/ResultDropdownList");
        selectSW('.selectUnidadeOrganizacional', "/api/services/app/UnidadeOrganizacional/ListarDropdownPorUsuario");
        selectSW('.selectTipoMovimento', "/api/services/app/tipomovimento/ListarDropdownDevolucao");
        selectSW(".selectMotivoPerdaProduto", "/api/services/app/motivoPerdaProduto/ListarDropDown");

        $('#EstTipoMovimentoId').change(function () {
            configurarCampos();
        }).on("select2-opening", function (arg) {
            var elem = $(arg.target);
            if ($("#s2id_" + elem.attr("id") + " ul").hasClass("myErrorClass")) {
                $(".select2-drop ul").addClass("myErrorClass");
            } else {
                $(".select2-drop ul").removeClass("myErrorClass");
            }
        });


        $(".selectEstoque").change(function () {
            if ($(".selectTipoSolicitacao").val() == "4") {
                selectSWMultiplosFiltros('.selectAtendimento', "/api/services/app/Atendimento/ListarAtendimentosComSaidaDropdown", ['EstoqueId']);
            }
        })


        $(".selectTipoSolicitacao").select2({ placeholder: "Informe um tipo de solicitacao" })
            .on("change", function (event) {
                event.preventDefault();
                selectSWMultiplosFiltrosCheckbox('.selectAtendimento', "/api/services/app/Atendimento/ListarAtendimentosEmAbertoDropdown");
                switch ($(".selectTipoSolicitacao").val()) {
                    case "3": {
                        selectSW('.selectTipoMovimento', "/api/services/app/TipoMovimento/ListarDropdownSolicitacaoSaida");
                        debugger;
                        break;
                    }
                    case "1": {
                        selectSW('.selectTipoMovimento', "/api/services/app/TipoMovimento/ListarDropdownEntrada");
                        break;
                    }
                    case "4": {
                        selectSW('.selectTipoMovimento', "/api/services/app/TipoMovimento/ListarDropdownDevolucao");
                        selectSWMultiplosFiltros('.selectAtendimento', "/api/services/app/Atendimento/ListarAtendimentosComSaidaDropdown", ['EstoqueId']);

                        break;
                    }
                    default: {
                        $('.selectTipoMovimento').select2("destroy");
                    }
                }

            });

        function selectSWMultiplosFiltrosCheckbox(classe, url, filtros) {
            $(classe).css('width', '100%');
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };

            function filtrar() {
                var filtroValores = [];
                for (i = 0; i < filtros.length; i++) {
                    var campo = $(filtros[i]).prop("checked");
                    filtroValores.push(campo);
                }
                return filtroValores;
            }

            $(classe).select2({
                allowClear: true,
                placeholder: app.localize("SelecioneLista"),
                ajax: {
                    url: url,
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
                            filtros: [
                                $(".emergencia").prop("checked") ? 0 : 1,
                                $("#UnidadeOrganizacionalId").val()
                            ]
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

    });

})(jQuery);