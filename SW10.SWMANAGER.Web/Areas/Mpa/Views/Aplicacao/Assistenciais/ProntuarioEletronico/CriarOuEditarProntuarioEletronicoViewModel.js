(function ($) {
    app.modals.CriarOuEditarProntuarioEletronicoModal = function () {
        var _prontuariosEletronicosService = abp.services.app.prontuarioEletronico;
        var _atendimentoService = abp.services.app.atendimento;
        var _operacaoService = abp.services.app.operacao;
        var _atendimento;
        var operacao;
        var _modalManager;
        var _$prontuarioEletronicoInformationForm = null;
        var atendimentoId = parseInt(localStorage["AtendimentoId"]);
        var operacaoId = sessionStorage["OperacaoId"];

        var pagina = sessionStorage["ActivePage"];
        //debugger;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$prontuarioEletronicoInformationForm = $('form[name=ProntuarioEletronicoInformationsForm-' + localStorage["AtendimentoId"] + '-' + sessionStorage["OperacaoId"] + ']');
            _$prontuarioEletronicoInformationForm.validate();

            //resolver conflito entre modalmanager e select2: input não recebe o cursor
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
            $.ajax({
                url: "/api/services/app/atendimento/obter?id=" + atendimentoId,
                method: 'POST',
                //dataType: 'json',
                async: false,
                success: function (data) {
                    //debugger;
                    _atendimento = data.result;

                }
            });

            $.ajax({
                url: "/api/services/app/operacao/obterPorNome?name=" + pagina,
                method: 'POST',
                dataType: 'json',
                async: false,
                success: function (data) {
                    //debugger;
                    operacao = data.result;
                }
            });
        };

        this.save = function () {
            if (!_$prontuarioEletronicoInformationForm.valid()) {
                return;
            }
            //debugger;
            var prontuarioEletronico = _$prontuarioEletronicoInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _prontuariosEletronicosService.criarOuEditar(prontuarioEletronico)
                .done(function (data) {
                    $('#id-' + localStorage["AtendimentoId"] + '-' + sessionStorage["OperacaoId"]).val(data.id);
                    $('#operacao-id-' + localStorage["AtendimentoId"] + '-' + sessionStorage["OperacaoId"]).val(data.operacaoId);
                    $('#creator-user-id-' + localStorage["AtendimentoId"] + '-' + sessionStorage["OperacaoId"]).val(data.creatorUserId);
                    $('#codigo-' + localStorage["AtendimentoId"] + '-' + sessionStorage["OperacaoId"]).val(data.codigo);
                    $('#codigo-label-' + localStorage["AtendimentoId"] + '-' + sessionStorage["OperacaoId"]).removeAttr('disabled');
                    $('#codigo-label-' + localStorage["AtendimentoId"] + '-' + sessionStorage["OperacaoId"]).val(data.codigo);
                    $('#codigo-label-' + localStorage["AtendimentoId"] + '-' + sessionStorage["OperacaoId"]).attr('disabled', 'disabled');
                    abp.notify.success(app.localize('SavedSuccessfully'));
                    abp.event.trigger('app.CriarOuEditarProntuarioEletronicoModalSaved');
                    const preencherData = {
                        nomeClasse: "ProntuarioEletronico",
                        formConfigId: $('#form-config-id-' + data.atendimentoId + '-' + data.operacaoId).val(),
                        registroClasseId: data.id,
                        formRespostaId: data.formRespostaId,
                        atendimentoId: data.atendimentoId,
                        leitoId: prontuarioEletronico.LeitoId,
                        atendimentoLeitoId: prontuarioEletronico.AtendimentoLeitoId,
                        habilitaAlteracaoLeito:true
                    }
                    url = `/Mpa/GeradorFormularios/_Preencher?${$.param(preencherData)}`;

                    $('#RefreshProntuariosEletronicosListButton-' + localStorage["AtendimentoId"] + '-' + sessionStorage["OperacaoId"]).click();
                    // $('#menu-modulo-' + localStorage["AtendimentoId"]).addClass('hidden');
                    //$('#conteudo-modulo-' + localStorage["AtendimentoId"]).removeClass('hidden');

                    // $('#formulario-dinamico-area-' + localStorage["AtendimentoId"]).attr('src', url);
                    sessionStorage["dataAdmissao"] = data.dataAdmissao;
                    criarNewAba(sessionStorage["id"], sessionStorage["dataRegistro"], sessionStorage["codigoAtendimento"], sessionStorage["paciente"], url, "Prontuário");


                    _modalManager.close();
                })
                .always(function () {
                    _modalManager.setBusy(true);
                });
        };

        $('#unidade-organizacional-id-' + localStorage["AtendimentoId"] + '-' + sessionStorage["OperacaoId"]).select2({
            allowClear: true,
            placeholder: app.localize("SelecioneLista"),
            ajax: {
                url: "/api/services/app/unidadeOrganizacional/ListarDropdown",
                dataType: 'json',
                delay: 250,
                method: 'Post',
                data: function (params) {
                    if (params.page === undefined)
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

        $("#cbo-atendimento-id-" + localStorage["AtendimentoId"] + '-' + sessionStorage["OperacaoId"]).select2({
            allowClear: true,
            placeholder: app.localize("SelecioneLista"),
            ajax: {
                url: "/api/services/app/atendimento/ListarDropdown",
                dataType: 'json',
                delay: 250,
                method: 'Post',
                initSelection: function (element, callback) {
                    callback($.map(element.val().split(','), function (id) {
                        return { id: id, text: id };
                    }));
                },
                data: function (params) {
                    if (params.page === undefined)
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

        var elemento = $('#form-config-id-' + localStorage["AtendimentoId"] + '-' + sessionStorage["OperacaoId"]);
        if (elemento.hasClass('select2')) {
            $('#form-config-id-' + localStorage["AtendimentoId"] + '-' + sessionStorage["OperacaoId"]).select2({
                allowClear: true,
                placeholder: app.localize("SelecioneLista"),
                ajax: {
                    url: "/api/services/app/formConfig/listarRelacionadosDropdown",
                    dataType: 'json',
                    delay: 250,
                    method: 'Post',
                    data: function (params) {
                        //debugger;
                        if (params.page === undefined)
                            params.page = '1';
                        return {
                            search: params.term,
                            page: params.page,
                            totalPorPagina: 10,
                            id: operacao.id,
                            filtro: $('#unidade-organizacional-id-' + localStorage["AtendimentoId"] + '-' + sessionStorage["OperacaoId"]).val(),
                            filtros: [null, localStorage["AtendimentoId"]]
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
})(jQuery);