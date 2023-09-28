(function () {
    $(function () {
        abp.event.on('reloadTab', (event) => {
            $('#RefreshProntuariosEletronicosListButton-' + localStorage["AtendimentoId"] + '-' + sessionStorage["OperacaoId"]).trigger("click");
        });

        abp.event.off('UpdateModalJustificativaViewModel', UpdateModalJustificativaViewModel)
        abp.event.on('UpdateModalJustificativaViewModel', UpdateModalJustificativaViewModel)

        var _$ProntuarioEletronicoTable = $('#ProntuarioEletronicoTable-' + localStorage["AtendimentoId"] + '-' + sessionStorage["OperacaoId"]);
        var _ProntuariosEletronicosService = abp.services.app.prontuarioEletronico;
        //var _prescricaoMedicaService = abp.services.app.prescricaoMedica;
        var _$filterForm = $('#ProntuariosEletronicosFilterForm-' + localStorage["AtendimentoId"] + '-' + sessionStorage["OperacaoId"]);

        var _selectedDateRange = {
            startDate: moment().startOf('day'),
            endDate: moment().endOf('day')
        };

        _$filterForm.find('.date-range-picker').daterangepicker(
            $.extend(true, app.createDateRangePickerOptions(), _selectedDateRange),
            function (start, end, label) {
                _selectedDateRange.startDate = start.format('YYYY-MM-DDT00:00:00Z');
                _selectedDateRange.endDate = end.format('YYYY-MM-DDT23:59:59.999Z');
            });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.'+sessionStorage.getItem("ActivePage")),
            edit: abp.auth.hasPermission('Pages.Tenant.' + sessionStorage.getItem("ActivePage")),
            'delete': abp.auth.hasPermission('Pages.Tenant.' + sessionStorage.getItem("ActivePage"))
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/ProntuariosEletronicos/CriarOuEditarProntuarioEletronico',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Assistenciais/ProntuarioEletronico/CriarOuEditarProntuarioEletronicoViewModel.js',
            modalClass: 'CriarOuEditarProntuarioEletronicoModal'
        });

        var _modalReativacao = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/ProntuariosEletronicos/ReativacaoProntuarioEletronico',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Assistenciais/ProntuarioEletronico/ModalReativacao/ModalReativacaoViewModel.js',
            modalClass: 'ReativacaoProntuarioEletronicoModal'
        });

        var _modalJustificativa = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/ProntuariosEletronicos/JustificativaProntuarioEletronico',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Assistenciais/ProntuarioEletronico/ModalJustificativa/ModalJustificativaViewModel.js',
            modalClass: 'JustificativaProntuarioEletronicoModal'
        });

        _$ProntuarioEletronicoTable.jtable({
            //title: app.localize('ProntuarioEletronico'),
            paging: true,
            sorting: true,
            multiSorting: true,
            selecting: true,
            selectingCheckboxes: true,
            actions: {
                listAction: {
                    method: _ProntuariosEletronicosService.listar
                }
            },
            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '10%',
                    sorting: false,
                    display: function (data) {
                        var $span = $('<span></span>');
                        if (data.record.enableEdit) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                .appendTo($span)
                                .click(function (e) {
                                    e.preventDefault();
                                    _ProntuariosEletronicosService.obter(data.record.id).then((prontuarioEletronicoData) => {
                                        const preencherData = {
                                            nomeClasse: "ProntuarioEletronico",
                                            formConfigId: prontuarioEletronicoData.formConfigId,
                                            registroClasseId: prontuarioEletronicoData.id,
                                            formRespostaId: prontuarioEletronicoData.formRespostaId,
                                            atendimentoId: prontuarioEletronicoData.atendimentoId,
                                            leitoId: prontuarioEletronicoData.leitoId,
                                            atendimentoLeitoId: prontuarioEletronicoData.atendimento != null ? prontuarioEletronicoData.atendimento.leitoId : null,
                                            habilitaAlteracaoLeito: true
                                        }
                                        const url = `/Mpa/GeradorFormularios/_Preencher?${$.param(preencherData)}`
                                        sessionStorage["dataAdmissao"] = data.record.dataAdmissao;
                                        criarNewAba(sessionStorage["id"], sessionStorage["dataRegistro"], sessionStorage["codigoAtendimento"], sessionStorage["paciente"], url, "Prontuário", data.record.id);
                                    })
                                    //if (data.record.formRespostaId && data.record.formRespostaId > 0) {
                                    //    const preencherData = {
                                    //        nomeClasse: "ProntuarioEletronico",
                                    //        formConfigId: $('#form-config-id-' + data.atendimentoId + '-' + data.operacaoId).val(),
                                    //        registroClasseId: data.id,
                                    //        formRespostaId: data.formRespostaId,
                                    //        atendimentoId: data.atendimentoId,
                                    //        leitoId: prontuarioEletronico.LeitoId,
                                    //        atendimentoLeitoId: prontuarioEletronico.AtendimentoLeitoId,
                                    //        habilitaAlteracaoLeito: true
                                    //    }

                                    //    url = '/Mpa/GeradorFormularios/_Preencher';//'@(Url.Action("_EditarPreenchimento","GeradorFormularios"))';
                                    //    url += '?nomeClasse=ProntuarioEletronico';
                                    //    url += '&formRespostaId=' + data.record.formRespostaId; //$('#form-resposta-id').val();
                                    //    url += '&registroClasseId=' + data.record.id;
                                    //    url += '&atendimentoId=' + localStorage["AtendimentoId"]; // necessario para popular adequadamente campos reservados dos forms.dinamicos

                                    //    sessionStorage["dataAdmissao"] = data.record.dataAdmissao;
                                    //    // $('#formulario-dinamico-area-' + localStorage["AtendimentoId"]).attr('src', url);

                                    //    //  $('#menu-modulo-' + localStorage["AtendimentoId"]).addClass('hidden');
                                    //    // $('#conteudo-modulo-' + localStorage["AtendimentoId"]).removeClass('hidden');

                                    //    criarNewAba(sessionStorage["id"], sessionStorage["dataRegistro"], sessionStorage["codigoAtendimento"], sessionStorage["paciente"], url, "Prontuário", data.record.id);
                                    //    //$('<li id="atendimento-' + pagina + '" name="Atendimento-' + codigoAtendimento + '"><a id="link-atendimento-' + codigoAtendimento + '" href="#conteudo-atendimento-' + pagina + '" data-toggle="tab" title="' + paciente + '" onclick="atualizarAtendimento(' + "'" + codigoAtendimento + "'" + ');"> ' + format[3] + '  ' + '<i class="fa fa-close"></i></a></li>').appendTo('#abas-' + localStorage["TipoAtendimento"]);
                                    //    // $('<div class="tab-pane" id="conteudo-atendimento-' + pagina + '" style="padding:5px;">').appendTo('#conteudo-abas-' + localStorage["TipoAtendimento"]).load(url);


                                    //    // alert('assaas');
                                    //}
                                    //else {
                                    //    _createOrEditModal.open({ id: data.record.id, operacaoId: sessionStorage["OperacaoId"], atendimentoId: localStorage["AtendimentoId"] });
                                    //}
                                    ////$('#CriarOuEditarProntuarioEletronicoArea').html('');
                                    ////$('#FormularioDinamicoArea').html('');
                                    ////LerParcial('CriarOuEditarProntuarioEletronicoArea', '/mpa/Assistenciais/CriarOuEditarProntuarioEletronico/' + data.record.id);
                                });
                        }
                        if (data.record.enableDelete) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    deleteProntuariosEletronicos(data.record);
                                });
                        }
                        return $span;
                    }
                },
                codigo: {
                    title: app.localize('Formulario'),
                    width: '10%',
                    display: function (data) {
                        return data.record.formulario;
                    }
                },
                dataAdmissao: {
                    title: app.localize('Data'),
                    width: '10%',
                    display: function (data) {
                        return moment(data.record.dataAdmissao).format('L LT');
                    }
                },
                //codigoAtendimento: {
                //    title: app.localize('Atendimento'),
                //    width: '10%',
                //},
                //paciente: {
                //    title: app.localize('Paciente'),
                //    width: '15%',
                //    //display: function (data) {
                //    //    return app.localize(data.record.operacao.descricao);
                //    //}
                //},
                medico: {
                    title: app.localize('Medico'),
                    width: '15%',
                    //display: function (data) {
                    //    return app.localize(data.record.operacao.descricao);
                    //}
                },
                'AssProntuario.CreatorUserId': {
                    title: app.localize('Usuário'),
                    width: '15%',
                    display: function (data) {
                        return app.localize(data.record.usuario);
                    }
                },
                unidadeOrganizacional: {
                    title: app.localize('UnidadeOrganizacional'),
                    width: '15%',
                    //display: function (data) {
                    //    return app.localize(data.record.operacao.descricao);
                    //}
                },
                //empresa: {
                //    title: app.localize('Empresa'),
                //    width: '15%',
                //    //display: function (data) {
                //    //    return app.localize(data.record.operacao.descricao);
                //    //}
                //},
                //formulario: {
                //    title: app.localize('Formulário'),
                //    width: '20%',
                //    //display: function (data) {
                //    //    return app.localize(data.record.operacao.descricao);
                //    //}
                //},

            },
            selectionChanged: function () {
                var $selectedRows = $('#ProntuarioEletronicoTable-' + localStorage["AtendimentoId"] + '-' + sessionStorage["OperacaoId"]).jtable('selectedRows');

                if ($selectedRows.length > 0) {
                    $('#criarRegistroButton').enable(true);

                    $selectedRows.each(function () {
                        var record = $(this).data('record');
                        exibirRelatorio(record.id);
                    });
                }
            },
            recordsLoaded: function (event, data) {
                if($("div[id^='ProntuarioEletronicoTable'] .jtable-main-container tr.jtable-data-row:first input[type=checkbox]").length === 0) {
                    return exibirRelatorio(0);
                }
                $("div[id^='ProntuarioEletronicoTable'] .jtable-main-container tr.jtable-data-row:first input[type=checkbox]").trigger('click');
            }
        });



        function UpdateModalJustificativaViewModel() {
            getProntuariosEletronicos();
        }

        function getProntuariosEletronicos() {
            if (_$ProntuarioEletronicoTable.length && _$ProntuarioEletronicoTable.data("hikJtable") !== null) {
                _$ProntuarioEletronicoTable.jtable('load', createRequestParams());
            }
        }

        function deleteProntuariosEletronicos(prontuarioEletronico) {
            abp.message.confirm(
                app.localize('DeleteWarning', prontuarioEletronico.formulario),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _modalJustificativa.open({ prontuarioEletronicoId: prontuarioEletronico.id });                        
                    }
                }
            );
        }

        function exibirRelatorio(registroId) {
            var action = "ObterArquivoNomePorIdEOperacao?registroId=" + registroId + "&operacaoId=" + sessionStorage["OperacaoId"];
            var caminho = "/mpa/RegistroArquivo/" + action;
            $.post(caminho).then(res => {
                var path = window.location.href.split(window.location.pathname)[0].split("//")[1];
                $("#file-frame").attr("src", "//" + path + "/libs/pdfjs/web/viewer.html?file=" + res + "&locale=pt-BR");
            });
        }

        function createRequestParams() {
            var prms = {};
            _$filterForm.serializeArray().map(function (x) { prms[x.name] = x.value; });
            return $.extend(prms, _selectedDateRange);
        }

        //$('#ShowAdvancedFiltersSpan-' + localStorage["AtendimentoId"]).click(function () {
        //    $('#ShowAdvancedFiltersSpan-' + localStorage["AtendimentoId"]).hide();
        //    $('#HideAdvancedFiltersSpan-' + localStorage["AtendimentoId"]).show();
        //    $('#AdvacedAuditFiltersArea-' + localStorage["AtendimentoId"]).slideDown();
        //});
        //$('#HideAdvancedFiltersSpan-' + localStorage["AtendimentoId"]).click(function () {
        //    $('#HideAdvancedFiltersSpan-' + localStorage["AtendimentoId"]).hide();
        //    $('#ShowAdvancedFiltersSpan-' + localStorage["AtendimentoId"]).show();
        //    $('#AdvacedAuditFiltersArea-' + localStorage["AtendimentoId"]).slideUp();
        //});

        $('#CreateNewProntuarioEletronicoButton-' + +localStorage["AtendimentoId"] + '-' + sessionStorage["OperacaoId"]).click(function (e) {
            e.preventDefault();

            abp.services.app.operacao.obterPorNome(sessionStorage["ActivePage"]).then((resOperacao) => {
                abp.services.app.atendimento.obter(localStorage["AtendimentoId"]).then((resAtendimento) => {
                    $.post("/api/services/app/formConfig/listarRelacionadosDropdown",
                        {
                            id: resOperacao.id,
                            filtro: resAtendimento.unidadeOrganizacionalId,
                            filtros: [null, localStorage["AtendimentoId"]],
                            page: 0,
                            totalPorPagina: 100
                        }).then((formConfigData) => {
                            if (formConfigData.result && formConfigData.result.items && formConfigData.result.items.length === 1) {
                                var prontuarioEletronico = {
                                    formConfigId: formConfigData.result.items[0].id,
                                    OperacaoId: resOperacao.id,
                                    dataAdmissao: new Date(),
                                    AtendimentoId: localStorage["AtendimentoId"],
                                    UnidadeOrganizacionalId: resAtendimento.unidadeOrganizacionalId
                                };

                                abp.services.app.prontuarioEletronico.criarOuEditar(prontuarioEletronico).then(
                                    (resProntuarioEletronico) => {

                                        var url = '/Mpa/GeradorFormularios/_Preencher';
                                        url += '?nomeClasse=ProntuarioEletronico';
                                        url += '&formConfigId=' + formConfigData.result.items[0].id;
                                        url += '&registroClasseId=' + resProntuarioEletronico.id;
                                        url += '&formRespostaId=' + resProntuarioEletronico.formRespostaId;
                                        url += '&atendimentoId=' + localStorage["AtendimentoId"];
                                        sessionStorage["dataAdmissao"] = resProntuarioEletronico.dataAdmissao;
                                        criarNewAba(sessionStorage["id"],
                                            sessionStorage["dataRegistro"],
                                            sessionStorage["codigoAtendimento"],
                                            sessionStorage["paciente"],
                                            url,
                                            "Prontuário");
                                    });


                            }
                            else {
                                _createOrEditModal.open({ operacaoId: sessionStorage["OperacaoId"], atendimentoId: localStorage["AtendimentoId"] });
                            }
                        });
                })
            });
        });

        $('.ExibirModalAtivacao').click(function () {
            _modalReativacao.open({ operacaoId: sessionStorage["OperacaoId"], atendimentoId: localStorage["AtendimentoId"] });
        });


        $('#ExportarProntuariosEletronicosParaExcelButton-' + localStorage["AtendimentoId"] + '-' + sessionStorage["OperacaoId"]).click(function () {
            _ProntuariosEletronicosService
                .listarParaExcel({
                    filtro: $('#ProntuariosEletronicosTableFilter-' + localStorage["AtendimentoId"] + '-' + sessionStorage["OperacaoId"]).val(),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetProntuariosEletronicosButton-' + localStorage["AtendimentoId"] + '-' + sessionStorage["OperacaoId"]).click(function (e) {
            e.preventDefault();
            getProntuariosEletronicos();
        });

        $('#RefreshProntuariosEletronicosListButton-' + localStorage["AtendimentoId"] + '-' + sessionStorage["OperacaoId"]).click(function (e) {
            e.preventDefault();
            getProntuariosEletronicos();
        });

        abp.event.on('app.CriarOuEditarProntuarioEletronicoModalSaved', function () {
            getProntuariosEletronicos();
        });

        getProntuariosEletronicos();

        $('#ProntuariosEletronicosTableFilter-' + localStorage["AtendimentoId"] + '-' + sessionStorage["OperacaoId"]).focus();
    });
})();
