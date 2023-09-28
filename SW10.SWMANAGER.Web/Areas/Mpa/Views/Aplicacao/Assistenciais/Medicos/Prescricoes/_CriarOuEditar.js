$(function () {
    const _prescricoesService = abp.services.app.prescricaoMedica;
    const _prescricaoItemRespostaService = abp.services.app.prescricaoItemResposta;
    const _prescricaoItemService = abp.services.app.prescricaoItem;
    const _frequenciaService = abp.services.app.frequencia;
    const _solicitacaoAntimicrobianoService = abp.services.app.solicitacaoAntimicrobiano;
    const solicitacaoAutorizacaoAppService = abp.services.app.solicitacaoAutorizacao;
    const aHorasDia = ["00:00", "01:00", "02:00", "03:00", "04:00", "05:00", "06:00", "07:00", "08:00", "09:00", "10:00", "11:00", "12:00", "13:00", "14:00", "15:00", "16:00", "17:00", "18:00", "19:00", "20:00", "21:00", "22:00", "23:00"];

    const atendimentoId = $(".prescricao .portlet-body").find("#atendimentoId").val();
    var _$filterForm = $('#PrescricoesItensFilterForm');

    const numberMaskTemplate = {
        mask: 'num',
        blocks: {
            num: {
                mask: Number,
                thousandsSeparator: '.',
                scale: 2,	// digits after decimal
                signed: false, // allow negative
                normalizeZeros: true,  // appends or removes zeros at ends
                radix: ',',  // fractional delimiter
                padFractionalZeros: true,  // if true, then pads zeros at end to the length of scale
                allowDecimal: true
            }
        },
    };

    let campoQuantidade;

    const _permissions = {
        create: abp.auth.hasPermission('Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.Prescricao.Create'),
        edit: abp.auth.hasPermission('Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.Prescricao.Edit'),
        'delete': abp.auth.hasPermission('Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.Prescricao.Delete')
    };

    const _solicitacaoAntimicrobianoModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Mpa/SolicitacaoAntimicrobianos/SolicitacaoAntimicrobianoModal',
        scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Assistenciais/Medicos/SolicitacaoAntimicrobianos/_CriarOuEditarModal.js',
        modalClass: 'solicitacaoAntimicrobianoModal',
        modalId: 'solicitacaoAntimicrobianoModal',
    });

    const _createOrEditModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Mpa/Assistenciais/_CriarOuEditarRespostaModal',
        scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Assistenciais/Medicos/Prescricoes/_CriarOuEditarRespostaModal.js',
        modalClass: 'CriarOuEditarRespostaModal'
    });

    const _selecionarSubItemPrescricaoModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Mpa/Assistenciais/SelecionarSubItemPrescricaoModal',
        scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Assistenciais/Medicos/Prescricoes/_SelecionarSubItemPrescricaoModal.js',
        modalClass: 'selecionarSubItemPrescricaoModal',
        modalId: 'selecionarSubItemPrescricaoModal',
    });

    const _solicitacoesModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Mpa/Assistenciais/SolicitacoesModal',
        scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Assistenciais/Medicos/Prescricoes/Solicitacoes/_CriarOuEditarModal.js',
        modalClass: 'solicitacoesModal',
        modalId: 'solicitacoesModal',
    });

    _solicitacoesModal.onClose(() => {
        abp.event.trigger("loadPrescricaoCompleta", $("#id").val());
        $(".prescricao .portlet-body").find('#prescricao-item-id').focus();
        $("button.confirm").buttonBusy(false);
    })

    const _impressaoAcrescimosESuspensoesModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Mpa/Assistenciais/impressaoAcrescimosESuspensoes',
        scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Assistenciais/Medicos/Prescricoes/ImpressaoAcrescimosESuspensoesModal.js',
        modalId: 'impressaoAcrescimosESuspensoesModal',
        modalClass:'impressaoAcrescimosESuspensoesModal'
    });

    const prescricaoCompletaTab = `#PrescricaoCompletaTab`;
    const _ErrorModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Mpa/Erros/ExibirErros',
    });

    var divisaoForm = $('#form-divisao');

    var validatorForm;


    abp.event.off("loadCriarOuEditar", loadCriarOuEditar);
    abp.event.on("loadCriarOuEditar", loadCriarOuEditar);

    function loadCriarOuEditar() {
        $(".prescricao .portlet-body").find('.select2').css('width', '100%');
        //_modalManager = modalManager;
        $('.modal-imprimir .modal-dialog').css({'width': '90%', 'max-width': '1100px'});
        $.fn.modal.Constructor.prototype.enforceFocus = function () {
        };
        validatorForm = divisaoForm.validate({ignore: 'input[type=hidden]'});

        unloadCriarOuEditar();
        loaderStart();
        abp.event.on("aplicarConfiguracaoPrescricaoItem", aplicarConfiguracaoPrescricaoItem);
        abp.event.on("loadPrescricaoCompleta", prescricaoCompleta)
        abp.event.on("loadButtonsPrescricaoCompleta", loadButtonsPrescricaoCompleta);
        abp.event.on("loadFormItem", loadFormItem);

        abp.event.trigger("loadFormItem");
        abp.event.trigger("loadPrescricaoCompleta", $("#id").val());
    }

    function unloadCriarOuEditar() {
        abp.event.off("loadCriarOuEditar", loadCriarOuEditar);
        abp.event.off("aplicarConfiguracaoPrescricaoItem", aplicarConfiguracaoPrescricaoItem)
        abp.event.off("loadButtonsPrescricaoCompleta", loadButtonsPrescricaoCompleta)
        abp.event.off("loadFormItem", loadFormItem)
        abp.event.off("loadPrescricaoCompleta", prescricaoCompleta);
    }

    // Metodos Prescricao Completa
    function prescricaoCompleta(prescricaoMedicaId) {
        $(".prescricao .portlet-body #prescricao-completa-area").load('/mpa/assistenciais/_prescricaocompleta/', {
            atendimentoId: atendimentoId,
            'prescricaoMedicaId': prescricaoMedicaId
        });
    }

    function deletePrescricaoCompletaItem(registroId, descricao) {
        abp.message.confirmHtml("",
            app.localize('ExcluirPrescricaoItemWarning', descricao),
            function (isConfirmed) {
                if (isConfirmed) {
                    loaderStart();
                    _prescricoesService.excluirItemResposta(registroId).then(res => {
                        abp.notify.info(app.localize('ListaAtualizada'));
                        abp.event.trigger('app.CriarOuEditarPrescricaoModalSaved');
                        abp.event.trigger('loadPrescricaoCompleta',$("#id").val());
                    });
                }
            }
        );
    }
    

    function editPrescricaoItem(registroId, descricao, idGrid, idDivisao) {
        return _prescricaoItemRespostaService.obter(registroId).then(data => {
            preencherForm(data, idGrid, idDivisao);
            $(".prescricao .portlet-body").find('#salvar-prescricao-item-resposta-divisao-' + atendimentoId + ' i').removeClass('fa-plus').addClass('fa-check');
            $(".prescricao .portlet-body").find(".portlet.prescricao").find('#divisao-id').focus();
        });
    }

    function liberarPrescricaoItem(registroId, descricao) {
        loaderStart();
        abp.message.confirmHtml("",
            app.localize('LiberarPrescricaoItemWarning', descricao),
            function (isConfirmed) {
                if (isConfirmed) {
                    $("button.confirm").buttonBusy(true);
                    _prescricoesService.liberarItemResposta(registroId).then(res => {
                        abp.notify.info(app.localize('ListaAtualizada'));
                        abp.event.trigger('app.CriarOuEditarPrescricaoModalSaved');
                        abp.event.trigger("loadPrescricaoCompleta", $("#id").val());
                        $("button.confirm").buttonBusy(false);
                    });
                }
            }
        );
    }

    function aprovarPrescricaoItem(registroId, descricao) {
        loaderStart();
        abp.message.confirmHtml("",
            app.localize('AprovarPrescricaoItemWarning', descricao),
            function (isConfirmed) {
                if (isConfirmed) {
                    $("button.confirm").buttonBusy(true);
                    _prescricoesService.aprovarItemResposta(registroId).then(res => {
                        abp.notify.info(app.localize('ListaAtualizada'));
                        abp.event.trigger('app.CriarOuEditarPrescricaoModalSaved');
                        abp.event.trigger("loadPrescricaoCompleta", $("#id").val());
                        $("button.confirm").buttonBusy(false);
                    });
                }
            }
        );
    }

    function reativarPrescricaoItem(registroId, descricao) {
        loaderStart();
        abp.message.confirmHtml("",
            app.localize('ReativarPrescricaoItemWarning', descricao),
            function (isConfirmed) {
                if (isConfirmed) {
                    $("button.confirm").buttonBusy(true);
                    _prescricoesService.reAtivarItemResposta(registroId).then(res => {
                        abp.notify.info(app.localize('ListaAtualizada'));
                        abp.event.trigger('app.CriarOuEditarPrescricaoModalSaved');
                        abp.event.trigger("loadPrescricaoCompleta", $("#id").val());
                        $("button.confirm").buttonBusy(false);
                    });
                }
            }
        );
    }

    function suspenderPrescricaoItem(registroId, descricao) {
        loaderStart();
        abp.message.confirmHtml("",
            app.localize('SuspenderPrescricaoItemWarning', descricao),
            function (isConfirmed) {
                if (isConfirmed) {
                    $("button.confirm").buttonBusy(true);
                    _prescricoesService.suspenderItemResposta(registroId, $("#data-agrupamento").val()).then(res => {
                        abp.notify.info(app.localize('ListaAtualizada'));
                        abp.event.trigger('app.CriarOuEditarPrescricaoModalSaved');
                        abp.event.trigger("loadPrescricaoCompleta", $("#id").val());
                        $("button.confirm").buttonBusy(false);
                    });
                }
            }
        );
    }

    function loadButtonsPrescricaoCompleta() {
        $('.btn-edit').on('click', function (e) {
            e.preventDefault();
            editPrescricaoItem($(this).data('registro-id'), $(this).data('descricao'), $(this).data('grid-id'), '0');
        });

        $('.btn-delete').on('click', function (e) {
            e.preventDefault();
            deletePrescricaoCompletaItem($(this).data('grid-id'), $(this).data('descricao'));
        });

        $('.btn-liberarPrescricao').on('click', function (e) {
            e.preventDefault();
            liberarPrescricaoItem($(this).data('grid-id'), $(this).data('descricao'));
        });

        $('.btn-suspender').on('click', function (e) {
            e.preventDefault();
            suspenderPrescricaoItem($(this).data('grid-id'), $(this).data('descricao'));
        });

        $('.btn-aprovarPrescricao').on('click', function (e) {
            e.preventDefault();
            aprovarPrescricaoItem($(this).data('grid-id'), $(this).data('descricao'));
        });

        $('.btn-reativar').on('click', function (e) {
            e.preventDefault();
            reativarPrescricaoItem($(this).data('grid-id'), $(this).data('descricao'));
        });
        
        
        $(".btnLiberar").on('click', onClickBtnLiberar );
        $(".btnAprovar").on('click', onClickBtnAprovar );
        $(".btnImprimirAcrescimosESuspensoes").on('click', onClickBtnImprimirAcrescimosESuspensoes );
        $(".btnImprimirCompleto").on('click', onClickBtnImprimirCompleto );

        abp.event.off("liberarPrescricao", onLiberar);
        abp.event.on("liberarPrescricao", onLiberar);
        
        function onLiberar(prescricaoId) {
            swal({ title: "Liberar Prescrição", 
                    text: "Liberar Prescrição", 
                    type: "info", 
                    showCancelButton: true, 
                    closeOnConfirm: false, 
                    showLoaderOnConfirm: true 
                }, (val) => {
                if (val) {
                    $("button.confirm").buttonBusy(true);
                    swal.close();
                    $.ajax({
                        url: '/api/services/app/prescricaoMedica/liberar?id=' + prescricaoId + '&atendimentoId=' + atendimentoId,
                        method: 'POST',
                        success: (data) => {
                            if (data != null && data != '' && data != undefined) {
                                abp.notify.success(app.localize('SuccessfullyLiberated'));
                                debugger
                                $('.modal-imprimir').modal('toggle');
                                $("#IsAcrescimo").val(data.result.id != 0);
                                $("#prescricao-status-id").val(data.result.prescricaoStatusId != null ? data.result.prescricaoStatusId : 1 )
                            }
                            abp.event.trigger("loadPrescricaoCompleta", prescricaoId);
                            $(".prescricao .portlet-body").find('#prescricao-item-id').focus();
                            $("button.confirm").buttonBusy(false);
                        },
                        error: (request, status, error) => {
                            const req = JSON.parse(request.responseText);
                            swal(app.localize('Error'), req.message, 'error');
                            $("button.confirm").buttonBusy(false);
                        }
                    });
                }
            });
        }

        function onClickBtnLiberar() {
            loaderStart();
            const id = $(".prescricao .portlet-body").find("#id").val();
            return Promise.all([
                _solicitacaoAntimicrobianoService.validaSolicitacaoAntimicrobianoPorPrescricao(atendimentoId,id),
                solicitacaoAutorizacaoAppService.validaSolicitacaoAutorizacaoPorPrescricao(atendimentoId, id)
            ])
                .then(data => {
                    const resAntimicrobiano = data[0];
                    const resAutorizacao = data[1];
                    if(resAntimicrobiano.necessitaSolicitacao || resAutorizacao.necessitaSolicitacao) {
                        return _solicitacoesModal.open({ atendimentoId: atendimentoId, prescricaoId: id});
                    }
                    else {
                        abp.event.trigger("liberarPrescricao", id);
                    }
                })
        }
        
        function onClickBtnAprovar() {
            loaderStart();
            const prescricaoId = $(".prescricao .portlet-body").find("#id").val();
            swal({
                title: "Aprovar Prescrição",
                text: "Aprovar Prescricao",
                type: "info",
                showCancelButton: true,
                closeOnConfirm: false,
                showLoaderOnConfirm: true
            }, function (val) {
                if (val) {
                    $("button.confirm").buttonBusy(true);
                    swal.close();
                    $.ajax({
                        url: '/api/services/app/prescricaoMedica/aprovar?id=' + prescricaoId + '&atendimentoId=' + atendimentoId,
                        method: 'POST',
                        success: function (data) {
                            stopLoader()
                            if (data != null && data != '' && data != undefined) {
                                abp.notify.success(app.localize('SuccessfullyApproved'));
                            }
                            $("button.confirm").buttonBusy(false);
                            abp.event.trigger("loadPrescricaoCompleta", prescricaoId);
                            $(".prescricao .portlet-body").find('#prescricao-item-id').focus();
                        },
                        error: function (request, status, error) {
                            const req = JSON.parse(request.responseText);
                            swal(app.localize('Error'), req.message, 'error');
                            $("button.confirm").buttonBusy(false);
                            
                            abp.event.trigger("loadPrescricaoCompleta", prescricaoId);
                            $(".prescricao .portlet-body").find('#prescricao-item-id').focus();
                        }

                    });
                }
            });
        }
        
        function onClickBtnImprimirAcrescimosESuspensoes() {
            const prescricaoMedicaId = $(".prescricao .portlet-body").find("#id").val();
            _impressaoAcrescimosESuspensoesModal.open({ atendimentoId: atendimentoId, prescricaoMedicaId: prescricaoMedicaId});
        }

        function onClickBtnImprimirCompleto() {
            const prescricaoMedicaId = $(".prescricao .portlet-body").find("#id").val();
            if (prescricaoMedicaId == null || prescricaoMedicaId == undefined) {
                return;
            }
            $.removeCookie("XSRF-TOKEN");
            printJS({
                printable: '/Mpa/AssistenciaisRelatorios/imprimirTudo?prescricaoId=' + prescricaoMedicaId, type: 'pdf'
            })
        }
    }

    function preencherForm(result, idGrid, idDivisao) {
        var data = {result: result};
        if (data.result.prescricaoItemId == null) {
            $(".prescricao .portlet-body").find('#prescricao-item-id').val(null).trigger('change',{onSelecionarSub:false,override:false});
            return;
        }
        
        $(".prescricao .portlet-body").find('#prescricao-item-id')
            .removeAttr('disabled')
            .append($('<option value="' + data.result.prescricaoItemId + '">' + data.result.prescricaoItem.codigo + ' - ' + data.result.prescricaoItem.descricao + '</option>'))
            .val(data.result.prescricaoItemId)
            .trigger('change', {onSelecionarSub:false,override:false});

        $(".prescricao .portlet-body").find('#data-inicial').removeAttr('disabled').val(moment(data.result.dataInicial).format('L'));
        $(".prescricao .portlet-body").find('#observacao').removeAttr('disabled').val(data.result.observacao);
        $(".prescricao .portlet-body").find('#prescricao-item-resposta-id').val(data.result.id);
        $(".prescricao .portlet-body").find('#horarios').val(data.result.horarios)
        $(".prescricao .portlet-body").find('#volumeDiluente').val(data.result.volumeDiluente);
        $(".prescricao .portlet-body").find('#total-dias').val(data.result.totalDias);
        $(".prescricao .portlet-body").find('#dose-unica').prop("checked", data.result.doseUnica);
        $(".prescricao .portlet-body").find('#obsFrequencia').val(data.result.obsFrequencia);

        if (data.result.unidadeOrganizacionalId != null) {
            $select = $(".prescricao .portlet-body").find('#unidade-organizacional-id')
                .removeAttr('disabled')
                .append($('<option value="' + data.result.unidadeOrganizacionalId + '">' + data.result.unidadeOrganizacional.descricao + '</option>'))
                .val(data.result.unidadeOrganizacionalId)
        } else {
            $(".prescricao .portlet-body").find('#unidade-organizacional-id').val(null)
        }

        if (data.result.medicoId != null) {
            $select = $(".prescricao .portlet-body").find('#medico-id')
                .removeAttr('disabled')
                .append($('<option value="' + data.result.medicoId + '">' + data.result.medico.sisPessoa.nomeCompleto + '</option>'))
                .val(data.result.medicoId)
        } else {
            $(".prescricao .portlet-body").find('#medico-id').val(null).change();
        }

        if (data.result.isSeNecessario) {
            $(".prescricao .portlet-body").find('#is-se-necessario').attr('checked', 'checked');
        } else {
            $(".prescricao .portlet-body").find('#is-se-necessario').removeAttr('checked');
        }

        if (data.result.isUrgente) {
            $(".prescricao .portlet-body").find('#is-urgente').attr('checked', 'checked');
        } else {
            $(".prescricao .portlet-body").find('#is-urgente').removeAttr('checked');
        }

        $(".prescricao .portlet-body").find('#divisao-id')
            .removeAttr('disabled')
            .append($('<option value="' + data.result.divisaoId + '">' + data.result.divisao.descricao + '</option>'))
            .val(data.result.divisaoId)
        
        $(".prescricao .portlet-body").find('#quantidade').val(data.result.quantidade);
        campoQuantidade.unmaskedValue = String(data.result.quantidade) ?? "";

        if (data.result.unidade) {
            $(".prescricao .portlet-body").find('#unidade-id')
                .removeAttr('disabled')
                .append($('<option value="' + data.result.unidadeId + '">' + data.result.unidade.descricao + '</option>'))
                .val(data.result.unidadeId)
        } else {
            $(".prescricao .portlet-body").find('#unidade-id').val(null)
        }

        if (data.result.velocidadeInfusao) {
            $(".prescricao .portlet-body").find('#velocidade-infusao-id')
                .removeAttr('disabled')
                .append($('<option value="' + data.result.velocidadeInfusaoId + '">' + data.result.velocidadeInfusao.descricao + '</option>'))
                .val(data.result.velocidadeInfusaoId)
        } else {
            $(".prescricao .portlet-body").find('#velocidade-infusao-id').val(null)
        }

        if (data.result.formaAplicacao) {
            $(".prescricao .portlet-body").find('#forma-aplicacao-id')
                .removeAttr('disabled')
                .append($('<option value="' + data.result.formaAplicacaoId + '">' + data.result.formaAplicacao.descricao + '</option>'))
                .val(data.result.formaAplicacaoId)
        } else {
            $(".prescricao .portlet-body").find('#forma-aplicacao-id').val(null)
        }


        if (data.result.frequencia) {
            if(data.result.frequencia.intervalo != 0) {
                hideHorarios();
            } else {
                showHorarios();
            }
            
            $(".prescricao .portlet-body").find('#hora-inicial').removeAttr('disabled');
            $(".prescricao .portlet-body").find('#frequencia-id')
                .removeAttr('disabled')
                .data("previousFrequenciaId", data.result.frequencia.id)
                .append($('<option value="' + data.result.frequencia.id + '">' + data.result.frequencia.descricao + '</option>'))
                .val(data.result.frequenciaId)
        } else {
            hideHorarios();
            $(".prescricao .portlet-body").find('#hora-inicial').removeAttr('disabled');
            $(".prescricao .portlet-body").find('#frequencia-id').val(null).data("previousFrequenciaId", -1)
        }

        if (data.result.diluenteId) {
            $(".prescricao .portlet-body").find('#diluente-id')
                .removeAttr('disabled')
                .append($('<option value="' + data.result.diluenteId + '">' + data.result.diluente.descricao + '</option>'))
                .val(data.result.diluenteId)
        } else {
            $(".prescricao .portlet-body").find('#diluente-id').val(null)
        }

        setTimeout(() => {
            $(".prescricao .portlet-body").find('#prescricao-item-id').focus();
            abp.event.trigger("aplicarConfiguracaoPrescricaoItem", data.result.prescricaoItem.configuracaoPrescricaoItems);
        }, 0)
    }

    // Fim Prescricao Completa

    // Metodos Form Item

    const frequenciaSelect2Options = {
        ajax: {
            url: '/api/services/app/frequencia/listarDropdown',
            dataType: 'json',
            delay: 250,
            method: 'Post',
            quietMillis: 100,
            data: (params) => {
                params.page = params.page == undefined ? 1 : params.page;
                return {search: params.term, page: params.page, totalPorPagina: 10};
            },
            processResults: (data, params) => {
                params.page = params.page == undefined ? 1 : params.page;

                return {
                    results: data.result.items,
                    pagination: {
                        more: (params.page * 10) < data.result.totalCount
                    }
                };
            },
            cache: true
        },
        minimumInputLength: 0
    };
    
    function loaderStart() {
        const loader = $(".loader-prescricao-completa").clone();
        $(".prescricao .portlet-body").find(`#prescricao-completa-area`).empty().append(loader);
        $(".prescricao .portlet-body").find(`#prescricao-completa-area`).find(".loader-prescricao-completa").show();
    }
    
    const unidadeSelect2Options = {
        ajax: {
            url: '/api/services/app/produtounidade/listarUnidadePorProdutoDropdown',
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
                    filtros: [$(".prescricao .portlet-body").find('#produto-id').val()]
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
        escapeMarkup: function (markup) {
            return markup;
        }, // let our custom formatter work
        minimumInputLength: 0
    };
    
    const prescricaoItemSelect2Options = {
        allowClear: true,
        placeholder: app.localize("SelecioneLista"),
        ajax: {
            url: "/api/services/app/prescricaoitem/listarDropdown",
            dataType: 'json',
            delay: 250,
            method: 'Post',
            data: function (params) {
                if (params.page == undefined) {
                    params.page = '1';
                }
                return {
                    search: params.term,
                    page: params.page,
                    totalPorPagina: 10,
                    id: $(".prescricao .portlet-body").find('#divisao-id').val()
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
        escapeMarkup: function (markup) {
            return markup;
        }, // let our custom formatter work
        minimumInputLength: 0
    };
    
    function loadFormItem() {
        selectSW('.selectModeloPrescricao', "/api/services/app/ModeloPrescricao/ListarDropdown");
        $(".prescricao .portlet-body").find('#is-agora').on('change', changeAgora);

        selectSW('.selectDivisao', "/api/services/app/divisao/ListarDropdown");
        selectSW('.selectVelocidadeInfusao', "/api/services/app/VelocidadeInfusao/ListarDropdown");
        selectSW('.selectFormaAplicacao', "/api/services/app/FormaAplicacao/ListarDropdown");
        selectSW('.selectMedico', "/api/services/app/Medico/ListarDropdown");
        selectSW('.selectUnidadeOrganizacional', "/api/services/app/UnidadeOrganizacional/ListarDropdown");

        selectSW('.selectDiluente', "/api/services/app/PrescricaoItem/ListarDiluenteDropdown", $('#divisao-id'));

        $(".prescricao .portlet-body").find('#divisao-id').on('change', changeDivisao);

        $(".prescricao .portlet-body").find('#frequencia-id').select2(frequenciaSelect2Options).on('select2:selecting',selectingFrequencia).on('change', changeFrequencia);

        $(".prescricao .portlet-body").find('.input-group-addon').on('click', onClickButtonHora);

        $(".prescricao .portlet-body").find("#hora-inicial").on('blur', (e)=> {  $(".prescricao .portlet-body").find('.input-group-addon').trigger("click");});

        $(".prescricao .portlet-body").find('#unidade-id').select2(unidadeSelect2Options);

        $(".prescricao .portlet-body").find('#prescricao-item-id').select2(prescricaoItemSelect2Options).on('change', changePrescricaoItem);

        CamposRequeridos();
        aplicarDateSingle();
        aplicarDateRange();


        campoQuantidade = IMask($(".prescricao .portlet-body").find('#quantidade')[0],numberMaskTemplate);

        $(".prescricao .portlet-body").find('#salvar-prescricao').on("click",salvar);

        $(".prescricao .portlet-body").find('.fa-close').on('click',(e) => { e.preventDefault(); $('#' + sessionStorage["TargetConteudo"]).data('reload', '0'); });

        $(".prescricao .portlet-body").find('.cancelar-divisao').on('click', (e) => { e.preventDefault(); limparPrescricao(); });

        $(".prescricao .portlet-body").find('.voltar-prescricao').on('click', onClickVoltarPrescricao);

        abp.event.off("selecionarSubItemPrescricao", onSelecionarSubItemPrescricao);
        abp.event.on("selecionarSubItemPrescricao", onSelecionarSubItemPrescricao);
    }
    
    function onSelecionarSubItemPrescricao(data) {
        debugger;
        if(data && data.id ) {
            $(".prescricao .portlet-body").find('#prescricao-item-id')
                .removeAttr('disabled')
                .append($('<option value="' + data.id + '">' + data.codigo + ' - ' + data.descricao + '</option>'))
                .val(data.id)
                .trigger('change', {onSelecionarSub:false,override:true});
        }
    }
    
    function onClickVoltarPrescricao(e) {
        if ($('#prescricao-item-id').val() != undefined || $('#prescricao-item-id').val() != null) {
            return abp.message.confirm(
                app.localize('Existe item pendente de inserção ou atualização, deseja proceder mesmo assim?'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        voltarAction();
                    }
                    return Promise.resolve(false);
                });
        } else {
            voltarAction();
        }
    }
    
    function selectingFrequencia(e) {
        previousFrequenciaId = $(this).val();
        $(this).data("previousFrequenciaId", previousFrequenciaId)
    }
    
    function changeFrequencia(event) {
        const el = $(event.currentTarget);
        var previousFrequenciaId = el.data('previousFrequenciaId');
        let id = el.val();
        if (id && id !== null && id !== undefined && id !== '') {
            if (id != previousFrequenciaId || previousFrequenciaId === -1) {
                _frequenciaService.obter(el.val())
                    .done(function (data) {
                        if (data) {
                            if (data.horaInicialMedicacao) {
                                $(".prescricao .portlet-body").find('#hora-inicial').val(data.horaInicialMedicacao)
                            }
                            
                            if(data.intervalo != 0) {
                                hideHorarios();
                                $(".prescricao .portlet-body").find('#horarios').val(definirHorarios(data.intervalo, $(".prescricao .portlet-body").find('#hora-inicial').val()));
                                criarComboHora();
                            } else if(data.intervalo == 0) {
                                showHorarios();
                                $(".prescricao .portlet-body").find("#obsFrequencia").val(data.horarios);
                                $(".prescricao .portlet-body").find('#div-horarios').empty();
                            }
                        }
                    })
            } else {
                criarComboHora();
            }
        } else {
            hideHorarios();
            $(".prescricao .portlet-body").find('#horarios').val('');
            criarComboHora();
        }
    }
    
    function criarComboHora() {
        
        $(".prescricao .portlet-body").find('#div-horarios').empty();
        var horas = $(".prescricao .portlet-body").find('#horarios').val();
        if (horas != '') {
            const horaList = aHorasDia.map(x => moment(x, "HH:mm"));
            const collection = horas.split(' ');
            $('#div-horarios').html('').css('line-height', '30px;').css('padding-bottom', '10px');
            for (var i = 0; i < collection.length; i++) {
                let atual = moment(collection[i], "HH:mm");
                let append = '<div class="col-md-3"><select name="HoraDiaId-' + i + '" class="form-group mapa-horario select2" style="width:100% !important">';
                for (var x = 0; x < horaList.length; x++) {
                    let hora = horaList[x];
                    append += `<option value="${hora.format("HH")}" ${(atual.isSame(hora) ? 'selected' : '')}> ${hora.format("HH:mm")}</option>`;
                }
                append += '</select></div>';
                $(".prescricao .portlet-body").find('#div-horarios').append(append);
            }
            $(".prescricao .portlet-body").find(".mapa-horario.select2").select2();
        }
    }
    
    function onClickButtonHora(e) {
        e.preventDefault();
        var id = $(".prescricao .portlet-body").find('#frequencia-id').val();
        if (id && id != null && id != undefined && id != '') {
            _frequenciaService.obter(id)
                .done(function (data) {
                    if (data.horaInicialMedicacao) {
                        $(".prescricao .portlet-body").find('#hora-inicial').val(data.horaInicialMedicacao)
                    }

                    if(data.intervalo != 0) {
                        hideHorarios();
                        $(".prescricao .portlet-body").find('#horarios').val(definirHorarios(data.intervalo, $(".prescricao .portlet-body").find('#hora-inicial').val()));
                        criarComboHora();
                    }
                    else if(data.intervalo == 0) {
                        showHorarios();
                        $(".prescricao .portlet-body").find("#obsFrequencia").val(data.horarios);
                        $(".prescricao .portlet-body").find('#div-horarios').empty();
                    }
                });
        }
    }
    
    function showHorarios() {
        $(".prescricao .portlet-body").find(".frequencia-horarios").show();
    }
    function hideHorarios() {
        $(".prescricao .portlet-body").find(".frequencia-horarios").hide();
        $(".prescricao .portlet-body").find("#obsFrequencia").val(null)
    }

    function changeAgora(e) {
        if ($(".prescricao .portlet-body").find('#is-agora').is(':checked')) {
            $(".prescricao .portlet-body").find('#frequencia-id').val(null).trigger("change");
            $(".prescricao .portlet-body").find('#frequencia-id').attr('disabled', 'disabled');
            $(".prescricao .portlet-body").find('#frequencia-id').parent('div[class^=col-md-]').hide();
        } else {
            $(".prescricao .portlet-body").find('#frequencia-id').removeAttr('disabled');
            $(".prescricao .portlet-body").find('#frequencia-id').parent('div[class^=col-md-]').show();
        }
    }

    function changeDivisao(e) {
        if (e) {
            e.preventDefault();
        }
        selectSW('.selectDiluente', "/api/services/app/PrescricaoItem/ListarDiluenteDropdown", $('#divisao-id'));
        $(".prescricao .portlet-body").find('#prescricao-item-id').trigger("change", {onSelecionarSub:false});
    }


    function CamposBloqueioDosagem(data) {
        camposBloqueioDosagemPadrao()
        const qtdLabelEl = $(".prescricao .portlet-body").find('#quantidade').parent(".form-group").find("label")
        
        if (data) {
            $(".prescricao .portlet-body").find('#isControleDosagem').val(data.isControleDosagem || false)
            $(".prescricao .portlet-body").find('#minimoBloqueio').val(data.minimoBloqueio || null)
            $(".prescricao .portlet-body").find('#minimoAceitavel').val(data.minimoAceitavel || null)
            $(".prescricao .portlet-body").find('#maximoBloqueio').val(data.maximoBloqueio || null)
            $(".prescricao .portlet-body").find('#maximoAceitavel').val(data.maximoAceitavel || null)
            $(".prescricao .portlet-body").find('#pendenteControleDeDosagem').val(false)
            $(".prescricao .portlet-body").find('#controleDeDosagemJustificativa').val(null)

            const message = `<div style='min-width:400px'>
                    <div class='row'>
                      <div class='col-md-12'> <h5 class='font-weight-bold'>Bloqueio</h5></div>
                      <span style='font-size:14px' class='col-md-6'> Mínimo Bloqueio</span>
                      <span style='font-size:14px' class='col-md-6'> Máximo Bloqueio</span>
                      <span style='font-size:13px;color:#337ab7' class='col-md-6'> ${data.minimoBloqueio || ''}</span>
                      <span style='font-size:13px;color:#337ab7' class='col-md-6'> ${data.maximoBloqueio || ''}</span>
                    </div>
                    <div class='row'>
                      <div class='col-md-12'> <h5 class='font-weight-bold'>Aceitavel</h5></div>
                      <span style='font-size:14px' class='col-md-6'> Mínimo Aceitavel</span>
                      <span style='font-size:14px' class='col-md-6'> Máximo Aceitavel</span>
                      <span style='font-size:13px;color:#337ab7' class='col-md-6'> ${data.minimoAceitavel || ''}</span>
                      <span style='font-size:13px;color:#337ab7' class='col-md-6'> ${data.maximoAceitavel || ''}</span>
                    </div>
                </div>`

            if (data.isControleDosagem) {
                var info = $(`<button
                    type="button"
                    id="info-controle-dosagem"
                    class="btn pull-right fa fa-question-circle"
                    style="margin-bottom: 5px;color: #337ab7;"
                    data-container="body"
                    data-toggle="popover"
                    data-trigger="focus"
                    title="Controle de Dosagem"
                    data-html="true"
                    data-content="${message}"
                    data-template='<div class="popover" style="min-width:450px;border-radius: 6px;" role="tooltip">
                    <div class="arrow"></div>
                    <h3 class="popover-title" style="background-color: #c3c3c3;color: white;font-size: 14px;"></h3>
                    <div class="popover-content"></div></div>'
                    ></button>`)
                qtdLabelEl.after(info)

                $(function () {
                    $('[data-toggle="popover"]').popover()
                })
            }
        }

        function camposBloqueioDosagemPadrao() {
            $(".prescricao .portlet-body").find('#isControleDosagem').val(false)
            $(".prescricao .portlet-body").find('#minimoBloqueio').val(null)
            $(".prescricao .portlet-body").find('#minimoAceitavel').val(null)
            $(".prescricao .portlet-body").find('#maximoBloqueio').val(null)
            $(".prescricao .portlet-body").find('#maximoAceitavel').val(null)
            $(".prescricao .portlet-body").find('#pendenteControleDeDosagem').val(false)
            $(".prescricao .portlet-body").find('#controleDeDosagemJustificativa').val(null)

            $(".prescricao .portlet-body").find("#info-controle-dosagem").remove()

        }
    }

    function validaControleDosagem() {
        debugger
        const resultObject = {
            possuiControle: false,
            bloqueado: false,
            obrigaJustificativa: false,
            possuiJustificativa: !_.isEmpty($(".prescricao .portlet-body").find('#controleDeDosagemJustificativa').val()),
            mensagem: "",
            tituloMensagem:""
        }

        const isControleDosagem = $(".prescricao .portlet-body").find('#isControleDosagem').val() === `true`
        if (!isControleDosagem) {
            $(".prescricao .portlet-body").find('#pendenteControleDeDosagem').val(false)
            return resultObject
        }

        let minimoBloqueio = $(".prescricao .portlet-body").find('#minimoBloqueio').val()
        let minimoAceitavel = $(".prescricao .portlet-body").find('#minimoAceitavel').val()
        let maximoBloqueio = $(".prescricao .portlet-body").find('#maximoBloqueio').val()
        let maximoAceitavel = $(".prescricao .portlet-body").find('#maximoAceitavel').val()


        minimoBloqueio = minimoBloqueio != "" ? parseFloat(minimoBloqueio) : null
        minimoAceitavel = minimoAceitavel != "" ? parseFloat(minimoAceitavel) : null
        maximoBloqueio = maximoBloqueio != "" ? parseFloat(maximoBloqueio) : null
        maximoAceitavel = maximoAceitavel != "" ? parseFloat(maximoAceitavel) : null

        resultObject.possuiControle = true

        const qtd = parseFloat(campoQuantidade.unmaskedValue)

        const horas = $(".prescricao .portlet-body").find('#horarios').val();
        let qtdHoras = 1
        if (horas != '') {
            qtdHoras = horas.split(' ').length
        }

        let totalDia = qtd * qtdHoras


        if (totalDia <= minimoBloqueio && minimoBloqueio != null) {
            resultObject.mensagem = `Item bloqueado devido a não respeitar o mínimo de dosagem diária. Mínimo dia (<b>${minimoBloqueio}</b>)`
            resultObject.tituloMensagem = "Item Bloqueado"
            resultObject.bloqueado = true
            return resultObject
        }
        if (totalDia >= maximoBloqueio && maximoBloqueio != null) {
            resultObject.mensagem = `Item bloqueado devido a não respeitar o máximo de dosagem diária. Máximo dia (<b>${maximoBloqueio}</b>)`
            resultObject.tituloMensagem = "Item Bloqueado"
            resultObject.bloqueado = true
            return resultObject
        }

        if (totalDia < minimoAceitavel && minimoAceitavel != null) {
            resultObject.mensagem = `O item não atendeu o mínimo recomendado e precisa ter uma justificativa para ser liberado. Mínimo dia (<b>${minimoAceitavel}</b>)`
            resultObject.tituloMensagem = "Justificativa Dosagem"
            resultObject.obrigaJustificativa = true
        }
        if (totalDia > maximoAceitavel && maximoAceitavel != null) {
            resultObject.mensagem = `O item excedeu o máximo recomendado e precisa ter uma justificativa para ser liberado. Máximo dia (<b>${maximoAceitavel}</b>)`
            resultObject.tituloMensagem = "Justificativa Dosagem"
            resultObject.obrigaJustificativa = true
        }

        return resultObject
    }
    
    function changePrescricaoItem(e,customEvent) {
        console.log(e,customEvent);
        debugger;
        if ($(this).val() == null) {
            return;
        }
        return _prescricaoItemService.obter($(this).val(), {async: false, cache: false}).done(onDoneRequestPrescricaoItem);
        
        function onDoneRequestPrescricaoItem(data) {
            if(data.hasParent && (!customEvent || customEvent.onSelecionarSub)) {
                abp.ui.setBusy();
                _selecionarSubItemPrescricaoModal.open({prescricaoItemId: data.id});
                return;
            }
            CamposBloqueioDosagem(data)


            $(".prescricao .portlet-body").find('#divisao-id')
                .removeAttr('disabled')
                .append($('<option value="' + data.divisaoId + '">' + data.divisao.descricao + '</option>'))
                .val(data.divisaoId)
            
            if (data.divisao.isQuantidade) {
                $(".prescricao .portlet-body").find('#quantidade').removeAttr('disabled').val(data.quantidade);
                campoQuantidade.unmaskedValue = String(data.quantidade) ?? "";
                $(".prescricao .portlet-body").find('#quantidade').parent('div[class^=col-md-]').show();
                showEl($(".prescricao .portlet-body").find('#quantidade'));
            } else {
                campoQuantidade.unmaskedValue = "";
                hideEl($(".prescricao .portlet-body").find('#quantidade'));
            }
            if (data.divisao.isTotalDias) {
                showEl($(".prescricao .portlet-body").find('#total-dias'));
            } else {
                hideEl($(".prescricao .portlet-body").find('#total-dias'));
            }
            if (data.divisao.isUnidadeMedida) {
                if (data.unidade && data.unidade != null && data.unidade != undefined) {
                    $(".prescricao .portlet-body").find('#unidade-id')
                        .removeAttr('disabled')
                        .append($('<option value="' + data.unidadeId + '">' + data.unidade.descricao + '</option>'))
                        .val(data.unidadeId)
                } else {
                    $(".prescricao .portlet-body").find('#unidade-id').removeAttr('disabled').val(null).change();
                }

                showEl($(".prescricao .portlet-body").find('#unidade-id'));
            } else {
                hideEl($(".prescricao .portlet-body").find('#unidade-id'));
            }
            
            if (data.divisao.isFrequencia) {
                if (data.frequenciaId && data.frequenciaId != null && data.frequenciaId != undefined) {
                    $(".prescricao .portlet-body").find('#frequencia-id')
                        .removeAttr('disabled')
                        .append($('<option value="' + data.frequenciaId + '">' + data.frequencia.descricao + '</option>'))
                        .val(data.frequenciaId)

                    $(".prescricao .portlet-body").find('#hora-inicia').removeAttr('disabled');
                } else {
                    $(".prescricao .portlet-body").find('#frequencia-id').removeAttr('disabled').val(null).change;
                    $(".prescricao .portlet-body").find('#hora-inicial').removeAttr('disabled')
                }
                showEl($(".prescricao .portlet-body").find('#frequencia-id'));
                showEl($(".prescricao .portlet-body").find('#hora-inicial'));
            } else {
                $(".prescricao .portlet-body").find('#frequencia-id').removeAttr('disabled').val(null).change;
                hideEl($(".prescricao .portlet-body").find('#frequencia-id'));
                showEl($(".prescricao .portlet-body").find('#hora-inicial'));
            }

            if (data.divisao.isFormaAplicacao) {
                if (data.formaAplicacaoId && data.formaAplicacaoId != null && data.formaAplicacaoId != undefined) {
                    $(".prescricao .portlet-body").find('#forma-aplicacao-id')
                        .removeAttr('disabled')
                        .append($('<option value="' + data.formaAplicacaoId + '">' + data.formaAplicacao.descricao + '</option>'))
                        .val(data.formaAplicacaoId)
                } else {
                    $(".prescricao .portlet-body").find('#forma-aplicacao-id').removeAttr('disabled').val(null).change;
                }

                showEl($(".prescricao .portlet-body").find('#forma-aplicacao-id'))
            } else {
                hideEl($(".prescricao .portlet-body").find('#forma-aplicacao-id'))
            }

            if (data.divisao.isVelocidadeInfusao) {
                if (data.velocidadeInfusaoId && data.velocidadeInfusaoId != null && data.velocidadeInfusaoId != undefined) {
                    $(".prescricao .portlet-body").find('#velocidade-infusao-id')
                        .removeAttr('disabled')
                        .append($('<option value="' + data.velocidadeInfusaoId + '">' + data.velocidadeInfusao.descricao + '</option>'))
                        .val(data.velocidadeInfusaoId)
                } else {
                    $(".prescricao .portlet-body").find('#velocidade-infusao-id').removeAttr('disabled').val(null).change;
                }
                showEl($(".prescricao .portlet-body").find('#velocidade-infusao-id'))
            } else {
                hideEl($(".prescricao .portlet-body").find('#velocidade-infusao-id'))
            }

            if (data.produtoId && data.produtoId != null && data.produtoId != undefined) {
                $(".prescricao .portlet-body").find('#produto-id').val(data.produtoId);
                selectSWMultiplosFiltros('.selectUnidade', "/api/services/app/ProdutoUnidade/ListarUnidadeConsumoProdutoDropdown", ['produto-id']);
            }

            data.divisao.isDataInicio ? showEl($(".prescricao .portlet-body").find('#data-inicial')) : hideEl($(".prescricao .portlet-body").find('#data-inicial'));

            data.divisao.isDiasAplicacao ? showEl($(".prescricao .portlet-body").find('#total-dias')) : hideEl($(".prescricao .portlet-body").find('#total-dias'));
            data.divisao.isUrgente ? showEl($(".prescricao .portlet-body").find('#is-urgente')) : hideEl($(".prescricao .portlet-body").find('#is-urgente'));

            data.divisao.isAcm ? showEl($(".prescricao .portlet-body").find('#is-acm')) : hideEl($(".prescricao .portlet-body").find('#is-acm'));

            data.divisao.isAgora ? showEl($(".prescricao .portlet-body").find('#is-agora')) : hideEl($(".prescricao .portlet-body").find('#is-agora'));
            data.divisao.isDoseUnica ? showEl($(".prescricao .portlet-body").find('#dose-unica')) : hideEl($(".prescricao .portlet-body").find('#dose-unica'));
            
            const override = (customEvent  && customEvent.hasOwnProperty('override') ) ? customEvent.override : true;
            abp.event.trigger("aplicarConfiguracaoPrescricaoItem", data.configuracaoPrescricaoItems, override);
        }
    }

    function limparPrescricao() {
        $(".prescricao .portlet-body").find('#prescricao-item-resposta-id').val(null).trigger('change');
        $(".prescricao .portlet-body").find('#prescricao-item-id').val(null).trigger('change',{onSelecionarSub:false});
        $(".prescricao .portlet-body").find('#quantidade').val('');
        campoQuantidade.unmaskedValue = "";
        $(".prescricao .portlet-body").find('#unidade-id').val(null).trigger('change');

        $(".prescricao .portlet-body").find('#velocidade-infusao-id').val(null).trigger('change');
        $(".prescricao .portlet-body").find('#forma-aplicacao-id').val(null).trigger('change');
        $(".prescricao .portlet-body").find('#unidade-organizacional-id').val($('#unidadeAtual').val()).trigger('change');
        $(".prescricao .portlet-body").find('#medico-id').val($('#medicoAtual').val()).trigger('change');
        hideHorarios();
        $(".prescricao .portlet-body").find('#frequencia-id').val(null).data("previousFrequenciaId", -1).trigger('change');
        

        $(".prescricao .portlet-body").find('#hora-inicial').val($("#horaInicialAtual").val());
        $(".prescricao .portlet-body").find('#is-agora').removeAttr('checked');
        $(".prescricao .portlet-body").find('#is-acm').removeAttr('checked');
        $(".prescricao .portlet-body").find('#is-urgente').removeAttr('checked');
        $(".prescricao .portlet-body").find('#dose-unica').removeAttr('checked');
        $(".prescricao .portlet-body").find('#total-dias').val('');
        $(".prescricao .portlet-body").find('#observacao').val('');

        $(".prescricao .portlet-body").find('#diluente-id').val(null).change();
        $(".prescricao .portlet-body").find('#volumeDiluente').val('');


    }
    
    function salvar(e, mudarLeito) {
        if (e) {
            e.stopImmediatePropagation();
        }

        $(".prescricao .portlet-body").find('#salvar-prescricao').buttonBusy(true);
        $(".prescricao .portlet-body").find('.voltar-prescricao').buttonBusy(true);
        if (
            $(".prescricao .portlet-body").find('#prescricao-item-id').val() == null ||
            $(".prescricao .portlet-body").find('#prescricao-item-id').val() == '' ||
            $(".prescricao .portlet-body").find('#prescricao-item-id').val() == "" ||
            $(".prescricao .portlet-body").find('#prescricao-item-id').length == 0 ||
            $(".prescricao .portlet-body").find('#prescricao-item-id').val() == undefined
        ) {
            $(".prescricao .portlet-body").find('#salvar-prescricao').buttonBusy(true);
            $(".prescricao .portlet-body").find('.voltar-prescricao').buttonBusy(true);
            abp.notify.info(app.localize('SelecionarPrescricaoItem'));
            return Promise.resolve([]);
        }
        const prescricaoForm = $(".prescricao .portlet-body").find('form[data-atendimento-id="' + atendimentoId + '"]');
        const prescricaoItemForm = $(".prescricao .portlet-body").find('#form-divisao');

        const habilitaAlteracaoLeito = Boolean($(".prescricao .portlet-body").find("#habilita-alteracao-leito").val())
        //const leito = $(".prescricao .portlet-body").find("#leito-id").val()
        //const leitoAtendimento = $(".prescricao .portlet-body").find("#atendimento-leito-id").val()

        let prescricaoFormData = prescricaoForm.serializeFormToObject();
        let prescricaoItemFormData = prescricaoItemForm.serializeFormToObject();
        prescricaoFormData.medicoId = $("#medico-id-prescricao").val();
        if (!prescricaoForm.valid() || !prescricaoItemForm.valid()) {
            abp.notify.warn(app.localize('Preencha os campos obrigatórios'));
            $(".prescricao .portlet-body").find('#salvar-prescricao').buttonBusy(false);
            $(".prescricao .portlet-body").find('.voltar-prescricao').buttonBusy(false);
            return Promise.resolve(false);
        }

        const leitoAtendimento = $(".prescricao .portlet-body").find("#atendimento-leito-id").val();
        const leitoAtual = $(".prescricao .portlet-body").find("#leito-id").val();

        debugger
        if (habilitaAlteracaoLeito && leitoAtendimento != leitoAtual && !mudarLeito) {
            const confirmOptions = {
                cancelButtonText: "Não, salvar sem alterar",
                confirmButtonText: "Sim, alterar e salvar",
                customClass: 'custom-swal-prontuario-eletronico',
            }
            return abp.message.customConfirm("O Leito do atendimento mudou, você quer alterar o leito da prescrição?", "Alteração de leito", (confirm) => {
                if (confirm) {
                    $(".prescricao .portlet-body").find("#leito-id").val($(".prescricao .portlet-body").find("#atendimento-leito-id").val())
                    swal.close();
                    return salvar(null, true)
                }
                return salvar(null, false)
            }, confirmOptions)
        }
            

        const validadorControleDosagem = validaControleDosagem()

        if (validadorControleDosagem.possuiControle) {
            if (validadorControleDosagem.bloqueado) {
                return customConfirmModalHelper.CreateModalAsync({
                    title: validadorControleDosagem.tituloMensagem,
                    message: validadorControleDosagem.mensagem,
                    icon: "fas fa-exclamation-triangle text-info",
                    buttons: [
                        customConfirmModalHelper.CreateButton("Ok", "btn btn-danger ", null, (event, confirmModalInstance) => {
                            const btnOk = $(event.target);
                            btnOk.buttonBusy(false);
                            $(".prescricao .portlet-body").find('#salvar-prescricao').buttonBusy(false);
                            $(".prescricao .portlet-body").find('.voltar-prescricao').buttonBusy(false);
                            return confirmModalInstance.close(false)
                        })
                    ],
                    styles: {
                        "modal-dialog": { 'min-width': '600px' }
                    },
                    confirmModalOptions: {
                        onShowModal(confirmModalInstance) {
                            $("body").get(0).scrollIntoView()
                        }
                    },
                    promiseCallback: (params) => {
                        if (params == true) {
                            return salvar();
                        } else if (params == false) {
                            $(".prescricao .portlet-body").find('#salvar-prescricao').buttonBusy(false);
                            $(".prescricao .portlet-body").find('.voltar-prescricao').buttonBusy(false);
                            return false
                        }
                        return false

                    }
                })
            }
            if (validadorControleDosagem.obrigaJustificativa && !validadorControleDosagem.possuiJustificativa) {
                return customConfirmModalHelper.CreateModalAsync({
                    title: validadorControleDosagem.tituloMensagem,
                    message: validadorControleDosagem.mensagem,
                    icon: "fas fa-exclamation-triangle text-info",
                    buttons: [
                        customConfirmModalHelper.CreateButton("Justificar", "btn btn-primary", null, (event, confirmModalInstance) => {
                            const btnJustificativa = $(event.target);
                            btnJustificativa.buttonBusy(false);
                            var modalJustificativaTexto = confirmModalInstance.instance.find('#modal-justificativa').val()

                            if (!_.isEmpty(modalJustificativaTexto)) {
                                confirmModalInstance.instance.find('.form-group-justificativa').removeClass("has-error")
                                confirmModalInstance.instance.find('.span-justificativa').css("display", "none")
                                $(".prescricao .portlet-body").find('#controleDeDosagemJustificativa').val(modalJustificativaTexto)
                                return confirmModalInstance.close(true)
                            }
                            confirmModalInstance.instance.find('.form-group-justificativa').addClass("has-error")
                            confirmModalInstance.instance.find('.span-justificativa').css("display", "block")
                        })
                    ],
                    customContent:`
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group form-group-justificativa">
                                    <label>
                                        Justificativa
                                        <b class="required-label" style="color:#ff0000"> *</b>
                                    </label>
                                    <textarea name="modal-justificativa" id="modal-justificativa" class="form-control" required="true"></textarea>
                                    <span class="help-block help-block-validation-error span-justificativa" style="display:none">Este campo é requerido.</span>
                                </div>
                            </div>
                        </div>`,
                    styles: {
                        "modal-dialog": { 'min-width': '600px' }
                    },
                    confirmModalOptions: {
                        cancelButton: true,
                        onShowModal(confirmModalInstance) {
                            $("body").get(0).scrollIntoView()
                        }
                    },
                    promiseCallback: (params) => {
                        if (params == true) {
                            return salvar();
                        } else if (params == false) {
                            $(".prescricao .portlet-body").find('#salvar-prescricao').buttonBusy(false);
                            $(".prescricao .portlet-body").find('.voltar-prescricao').buttonBusy(false);
                            return false
                        }
                        return false

                    }
                });
            }
        }

        if(prescricaoFormData.id == 0 || ($("#prescricao-item-resposta-id").val() != "" && $("#prescricao-item-resposta-id").val() != "0"))   {
            return executaSalvar();
        }
        loaderStart();
        return _prescricoesService.validaDuplicidadeItemNaPrescricao(prescricaoFormData.Id,prescricaoItemFormData.PrescricaoItemId)
        .then(function(dataValidaDuplicidade)  {
           if(dataValidaDuplicidade.temMensagem) {
              return abp.message.confirm(dataValidaDuplicidade.mensagem, dataValidaDuplicidade.tituloMensagem, function (confirm) {
                   if (confirm) {
                       return executaSalvar();
                   } else {
                       limpaSalvar();
                       return Promise.resolve(false);
                   }
               });
           }
           else {
               return executaSalvar();
           }
        });
        function executaSalvar() {
            return _prescricoesService.criarOuEditar(prescricaoFormData, false).then(res => {
                $("#id").val(res.id);
                $("#prescricaoId").val(res.id);
                prescricaoItemFormData.PrescricaoMedicaId = res.id;
                prescricaoItemFormData.dataAgrupamento = $("#data-agrupamento").val();
                return salvarPrescricaoItemResposta(prescricaoItemFormData, false).then(res => {
                    abp.notify.success(app.localize('SavedSuccessfully'));
                })
            }).always(() => {
                limpaSalvar()
            })
        }
        function limpaSalvar() {
            $(".prescricao .portlet-body").find('#salvar-prescricao').buttonBusy(false);
            $(".prescricao .portlet-body").find('.voltar-prescricao').buttonBusy(false);
            abp.event.trigger("loadPrescricaoCompleta", $("#id").val());
            $(".prescricao .portlet-body").find('#prescricao-item-id').focus();
            limparPrescricao();
        }
    }

    function salvarPrescricaoItemResposta(prescricaoItemFormData, voltarIndex) {
        if (prescricaoItemFormData.Horarios != null && prescricaoItemFormData.Horarios != '' && prescricaoItemFormData.Horarios != "" && prescricaoItemFormData.Horarios != undefined) {
            const horas = prescricaoItemFormData.Horarios.split(' ')
            let strHorarios = ''
            for (let i = 0; i < horas.length; i++) {
                let horario = $('select[name=HoraDiaId-' + i + ']').val();
                let momentH = moment(horario, "HH");
                strHorarios += momentH.format("HH:mm") + ' ';
            }
            strHorarios = strHorarios.substring(0, strHorarios.length - 1);
            prescricaoItemFormData.Horarios = strHorarios;
        }
        prescricaoItemFormData.quantidade = campoQuantidade.unmaskedValue;
        prescricaoItemFormData.doseUnica = $("#dose-unica").prop("checked");
        prescricaoItemFormData.isAcrescimo = $(".prescricao .portlet-body").find("#is-acrescimo").val();
        return _prescricaoItemRespostaService.criarOuEditar(prescricaoItemFormData, true);
    }
    
    function fecharAction() {
        abp.event.trigger('app.CriarOuEditarPrescricaoModalSaved');
        //fechar();
    }

    $('.modal-imprimir').on('hidden.bs.modal', function () {
        fecharAction();
    });

    function naoImprimir() {
        $('.modal-imprimir').modal('toggle');
    }

    $('.naoImprimir').on("click", naoImprimir);

    function imprimirAcrescimosESuspensosAction() {
        if ($("#id").val()) {
            imprimirAcrescimosESuspensos($("#id").val());
        }
        $('.modal-imprimir').modal('toggle');
    }

    $('.imprimir-acrescimos-suspensos').on("click", imprimirAcrescimosESuspensosAction);

    function imprimirTudoAction() {
        if ($("#id").val()) {
            imprimirTudo($("#id").val());
        }
        $('.modal-imprimir').modal('toggle');
    }

    $('.imprimir-tudo').on("click", imprimirTudoAction);

    function voltarAction() {
        abp.event.trigger('app.CriarOuEditarPrescricaoModalSaved');
        fechar();
    }

    function imprimirAcrescimosESuspensos(id) {
        _impressaoAcrescimosESuspensoesModal.open({ atendimentoId: atendimentoId, prescricaoMedicaId: id});
    }

    function imprimirTudo(id) {
        $.removeCookie("XSRF-TOKEN");
        printJS({printable: '/Mpa/AssistenciaisRelatorios/imprimirTudo?prescricaoId=' + id, type: 'pdf'})
    }

    function validaSolicitacaoAntimicrobiano(resultCallback) {
        if ($(".prescricao .portlet-body").find("#necessita-solicitacao-antimicrobiano").val() !== "1") {
            if ($(".prescricao .portlet-body").find('#respostas-list').val() == '') {
                $(".prescricao .portlet-body").find('#respostas-list').val('[]')
            }

            var prescricaoItems = JSON.parse($(".prescricao .portlet-body").find("#respostas-list").val());
            var prescricaoItemIds = prescricaoItems.map(x => x.PrescricaoItemId);

            return _solicitacaoAntimicrobianoService.validaSolicitacaoAntimicrobiano(atendimentoId, prescricaoItemIds).then(necessitaSolicitacao => {
                $(".prescricao .portlet-body").find("#necessita-solicitacao-antimicrobiano").val(necessitaSolicitacao);
                if (necessitaSolicitacao) {
                    return _solicitacaoAntimicrobianoModal.open({
                        atendimentoId: atendimentoId,
                        prescricaoItemIds: prescricaoItemIds
                    });
                } else {
                    return resultCallback();
                }
            }).fail(() => {
                return resultCallback();
            });
        } else {
            return resultCallback();
        }
    }

    function fechar() {
        unloadCriarOuEditar();
        setTimeout(() => { updateTab(); }, 0);
    }


    $('#modeloPrescricaoCompletaId').on('select2:select', function () {
        copiarPrescricao($(".prescricao .portlet-body").find('#modeloPrescricaoCompletaId').val(), $('#atendimentoId').val());
    });

    function copiarPrescricao(prescricaoId, atendimentoId) {
        swal({
            title: "Modelo de prescrição",
            text: "Incluir prescrição a partir do modelo",
            type: "info",
            showCancelButton: true,
            closeOnConfirm: false,
            showLoaderOnConfirm: true
        }, function () {
            const dataFuturaPrescricao = $("#data-futura-prescricao").val();
            const input = {
                id: prescricaoId,
                prescricaoCorrenteId: ($('#id').val() || 0),
                atendimentoId: ($('#atendimentoId').val() || 0),
                dataFuturaPrescricao: dataFuturaPrescricao != "" ? moment(dataFuturaPrescricao).format("YYYY-MM-DDT HH:mm:ssZ") : null,
                dataAgrupamento : $('#data-agrupamento').val()
            };
            loaderStart();
            _prescricoesService.incluirItemPrescricaoModelo(input).then(function (data) {
                swal.close();
                if (data != null && data != '' && data != undefined && data.errors.length > 0) {
                    _ErrorModal.open({erros: data.errors});
                } else {
                    abp.notify.success(app.localize('Successfully'));
                }
                $("#prescricaoId").val(data.returnObject.prescricaoId);
                $('#id').val(data.returnObject.prescricaoId);
                abp.event.trigger("loadPrescricaoCompleta", $("#id").val());

            }).fail(function (request, status, error) {
                const req = JSON.parse(request.responseText);
                swal(app.localize('Error'), req.message, 'error');
            });
        });
    }
    // Fim Form Item
    
    //Helpers
    function aplicarConfiguracaoPrescricaoItem(configuracoes, override) {
        override = override ?? false;
        const configuracaoPropriedades = {
            QtdPorHorario: {id: 1, domEl: '#quantidade', type: "number"},
            Unidade: {
                id: 2,
                domEl: '#unidade-id',
                type: "select2",
                service: '/api/services/app/prescricaoItem/listarUnidadePorProdutoDropdown',
                depends: "input[name='ProdutoId']"
            },
            ViaDeAplicacao: {
                id: 3,
                domEl: '#velocidade-infusao-id',
                service: "/api/services/app/VelocidadeInfusao/ListarDropdown",
                type: "select2"
            },
            FormaDeAplicacao: {
                id: 4,
                domEl: '#forma-aplicacao-id',
                service: "/api/services/app/FormaAplicacao/ListarDropdown",
                depends: '#velocidade-infusao-id',
                type: "select2"
            },
            Diluente: {
                id: 5,
                domEl: '#diluente-id',
                type: "select2",
                service: "/api/services/app/PrescricaoItem/ListarDiluenteDropdown",
                depends: '#divisao-id'
            },
            Volume: {id: 6, domEl: '#volumeDiluente', type: "number"},
            Medico: {id: 7, domEl: '#medico-id', type: "select2", service: "/api/services/app/Medico/ListarDropdown"},
            Frequencia: {
                id: 8,
                domEl: '#frequencia-id',
                service: '/api/services/app/frequencia/listarDropdown',
                type: "select2"
            },
            HoraInicial: {id: 9, domEl: '#hora-inicial', type: "time"},
            DiaInicial: {id: 10, domEl: '#data-inicial', type: "datetime"},
            DiasProvaveisDeUso: {id: 11, domEl: '#total-dias', type: "number"},
            Observacao: {id: 12, domEl: '#observacao', type: "textarea"},
        }

        _.forEach(configuracaoPropriedades, (item) => {
            let domEl = $(item.domEl);
            domEl.removeAttr("required");
            domEl.removeAttr("disabled").removeAttr("readonly");
        })

        const eachConfig = (item) => {
            let config = _.find(configuracaoPropriedades, (x) => x.id == item.configuracaoPrescricaoItemCampoId);
            if (config) {
                let domEl = $(".prescricao .portlet-body").find(config.domEl);
                if (item.isBlock) {
                    domEl.attr("disabled", "disabled").attr("readonly", "readonly")
                } else {
                    domEl.removeAttr("disabled").removeAttr("readonly")
                }

                if (item.isRequired) {
                    domEl.data("required", "true");
                    domEl.attr("required", "required")
                    domEl.rules("add", "required");
                } else {
                    domEl.data("required", "false");
                    domEl.removeAttr("required")
                }

                if (config.type === "select2") {
                    let value = domEl.select2().val();
                    domEl.data("value", value);
                    if (!value || value == "" || override) {
                        domEl.data("value", item.defaultValue);
                    }
                    domEl.data("options", item.options);
                } else if (config.type === "number") {
                    if(override) {
                        domEl.val(item.defaultValue);
                        if (config.domEl == "#quantidade") {
                            campoQuantidade.unmaskedValue = String(item.defaultValue);
                        }
                    }
                }
                else if(config.type === "datetime") {
                    if(override && item.defaultValue) {
                        domEl.val(item.defaultValue);
                    }
                    else if (item.defaultValue && (domEl.val() == "" || domEl.val() == undefined || domEl.val() == null)) {
                        domEl.val(item.defaultValue);
                    }
                }
                else if(config.type === "time") {
                    if(override && item.defaultValue) {
                        domEl.val(item.defaultValue);
                    }
                    else if (item.defaultValue && (domEl.val() == "" || domEl.val() == undefined || domEl.val() == null)) {
                        domEl.val(item.defaultValue);
                    }
                }  else {
                    if(override) {
                        domEl.val(item.defaultValue);
                    }
                    else if (item.defaultValue && (domEl.val() == "" || domEl.val() == undefined || domEl.val() == null)) {
                        domEl.val(item.defaultValue);
                    }
                }
            }
        }

        const clearFields = () => {
            let validate = divisaoForm.validate();
            _.forEach(configuracaoPropriedades, (config) => {
                let domEl = $(".prescricao .portlet-body").find(config.domEl);
                domEl.removeAttr("disabled").removeAttr("readonly")
                domEl.data("required", "false");
                domEl.removeAttr("required")
                domEl.rules("remove", "required");
                if (validate.settings.hasOwnProperty(domEl.attr('name'))) {
                    delete validate.settings[domEl.attr('name')]
                }
            })
            clearValidation(divisaoForm);
        }

        const applySelect2 = () => {
            _.each(_.filter(configuracaoPropriedades, (x) => x.type == "select2"), (config) => {
                let domEl = $(".prescricao .portlet-body").find(config.domEl);
                let defaultValue = sanitize(domEl.data("value"));
                let options = sanitize(domEl.data("options"));
                let depends = config.depends;
                let service = config.service;
                selectSPrescricao(domEl, service, depends ? $(depends) : undefined, options, defaultValue);
            });
        }

        function clearValidation(formElement) {
            var validator = $(formElement).validate();
            $('[name]', formElement).each(function () {
                validator.successList.push(this);
                validator.showErrors();
            });
            validator.reset();
        }

        setTimeout(() => {
            clearFields();
            clearValidation(divisaoForm);
            _.forEach(configuracoes, eachConfig);
            applySelect2();
            CamposRequeridos();

        }, 0)

    }
    
    function sanitize(value) {
        if (!value || value == "" || value == "undefined" || value == "null" || value == "[]" || _.isUndefined(value) || _.isNull(value)) {
            return undefined
        }
        return value;
    }
    
    function selectSPrescricao(classe, url, elementoFiltro, select2Options, defaultValue) {

        $(".prescricao .portlet-body").find(classe).css('width', '100%');
        $.fn.modal.Constructor.prototype.enforceFocus = function () {
        };

        function retornaJsonFilter() {
            let options = sanitize($(classe).data("options"));

            if (typeof options === "string" && !_.isUndefined(options) && !_.isNull(options) && options != "null" && options != "undefined") {
                if (options.indexOf(",") !== -1 && options.indexOf("[") === -1) {
                    options = JSON.parse("[" + options + "]");
                } else {
                    options = JSON.parse(options);
                }
            }
            return {
                options: options
            }
        }

        function retornaJsonFilterById() {
            let defaultValue = sanitize($(classe).data("value"));
            let options = sanitize($(classe).data("options"));
            if (typeof defaultValue === "string" && !_.isUndefined(defaultValue) && !_.isNull(defaultValue) && defaultValue != "null" && defaultValue != "undefined") {
                if (defaultValue.indexOf(",") !== -1 && defaultValue.indexOf("[") === -1) {
                    defaultValue = JSON.parse("[" + defaultValue + "]");
                } else {
                    defaultValue = JSON.parse(defaultValue);
                }
            }
            if (typeof options === "string" && !_.isUndefined(options) && !_.isNull(options) && options != "null" && options != "undefined") {
                if (options.indexOf(",") !== -1 && options.indexOf("[") === -1) {
                    options = JSON.parse("[" + options + "]");
                } else {
                    options = JSON.parse(options);
                }
            }

            if (!_.isArray(defaultValue) && !_.isUndefined(defaultValue) && !_.isNull(defaultValue)) {
                defaultValue = [defaultValue];
            }

            if (!_.isArray(options) && !_.isUndefined(options) && !_.isNull(options)) {
                options = [options];
            }

            return {
                id: defaultValue,
                options: options
            }
        }

        function filtrar() {
            if (elementoFiltro) {
                var retorno = null;
                if (elementoFiltro.valor != undefined) {
                    retorno = elementoFiltro.valor;
                } else if ((elementoFiltro != undefined) && (elementoFiltro != null) && (elementoFiltro != 0) && (elementoFiltro != '0')) {
                    if (elementoFiltro.val()) {
                        retorno = elementoFiltro.val();
                    } else {
                        retorno = null;// elementoFiltro;
                    }
                } else if (elementoFiltro.val()) {
                    retorno = elementoFiltro.val();
                }
                return retorno;
            } else {
                return null;
            }
        }

        const ajax = {
            url: url,
            dataType: 'json',
            delay: 250,
            method: 'Post',

            data: function (params) {
                //   //console.log('data: ', params, (params.page == undefined));
                if (params.page == undefined)
                    params.page = '1';
                //   //console.log('data: ', params);

                let result = {
                    search: params.term,
                    page: params.page,
                    totalPorPagina: 10,
                    jsonFilter: JSON.stringify(retornaJsonFilter())
                };

                var filtro = filtrar();

                if (filtro && filtro !== "") {
                    result.filtro = filtro;
                    result.filtros = filtro;
                }

                return result;
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
        };
        const defaultSelect2Options = {
            allowClear: true,
            placeholder: app.localize("SelecioneLista"),
            ajax,
            escapeMarkup: function (markup) {
                return markup;
            }, // let our custom formatter work
            minimumInputLength: 0
        };

        if (!_.isUndefined(select2Options) && _.isObject(select2Options)) {
            _.extend(defaultSelect2Options, select2Options);
        }

        const selectByIdFn = (event, id) => {
            let domEl = $(classe);
            domEl.data("value", id);

            $.ajax({
                url: ajax.url,
                dataType: ajax.dataType,
                delay: ajax.delay || 250,
                method: ajax.method,
                data: {page: 1,  totalPorPagina: 50,  jsonFilter: JSON.stringify(retornaJsonFilterById()) }
            }).then((res) => {
                if (res.result && res.result.items) {
                    var item = _.find(res.result.items, (x) => x.id == id);
                    if (item) {
                        let selectedOption = domEl.find(':selected');
                        if (selectedOption.length && selectedOption.val() == id) {
                            selectedOption.attr('selected', 'selected');
                        } else {
                            if (selectedOption.length) {
                                selectedOption.removeAttr('selected', 'selected');
                            }
                            const existEl = domEl.find("option:eq(" + item.id + ")");
                            if (existEl.length) {
                                existEl.attr('selected', 'selected');
                            } else {
                                $(`<option></option`).val(item.id).text(item.text).attr('selected', 'selected').appendTo(domEl);
                            }
                        }

                    }
                }
            }).always(() => {
                setTimeout(() => {
                    domEl.trigger("change");
                }, 0);
            });
        };

        var domEl = $(classe);
        if (domEl.data("select2")) {
            domEl.select2("destroy");
        }
        domEl.select2(defaultSelect2Options);
        domEl.on("select2:selectById", selectByIdFn);
        domEl.data("s2-url", url);
        domEl.trigger("select2:selectById", defaultValue);
    }

    function hideEl($el) {
        $el.attr('disabled', 'disabled').parents('div[class^=col-md-]').first().hide();
    }

    function showEl($el) {
        $el.removeAttr('disabled').parents('div[class^=col-md-]').first().show();
    }

    function updateTab() {
        var conteudo = $('#atendimento-' + sessionStorage['TargetConteudo']);
        if (conteudo.length) {
            var href = $('#atendimento-' + sessionStorage['TargetConteudo']).find("a").attr("href");
            conteudo.tab("show");

            parent.abp.event.trigger('reloadTab', {pagina: sessionStorage['TargetConteudo']});
            $(href).parent().children().removeClass("active");
            $(href).addClass("active");
            $("li[id^='atendimento-Prescrição-']").each((index, item) => {
                $($(item).find("a").attr("href")).remove();
                $(item).remove();
            });

            return true;
        }
        return false;
    }
    
    function createRequestParams() {
        var prms = {};
        _$filterForm.serializeArray().map(function (x) {
            prms[x.name] = x.value;
        });
        return $.extend(prms, _selectedDateRange);
    }
});

