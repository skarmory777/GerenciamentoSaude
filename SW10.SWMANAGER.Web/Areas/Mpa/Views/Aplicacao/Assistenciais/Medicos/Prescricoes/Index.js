(function () {
    $(function () {

        app.modals.FullModal = function () {
            $('.modal-dialog').css({ 'width': '90%' });
            //atendimentoId = $('#atendimentoModeloPrescricaoId').val();
        };
        
        const atendimentoId = $("#atendimentoId").val();

        abp.event.on('reloadTab', (event) => {
            $('#RefreshPrescricoesListButton-' + atendimentoId).trigger("click");
        });

        const _$PrescricoesTable = $('#PrescricoesTable-' + atendimentoId);
        const _prescricaoService = abp.services.app.prescricaoMedica;
        const _solicitacaoAntimicrobianoService = abp.services.app.solicitacaoAntimicrobiano;
        const solicitacaoAutorizacaoAppService = abp.services.app.solicitacaoAutorizacao;
        const _$filterForm = $('#PrescricoesFilterForm-' + atendimentoId);
        let _selectedDateRange = {
            startDate: moment(localStorage["DataAtendimento"]).startOf('day'),
            endDate: moment().endOf('day')
        };

        $('#date-range-prescricao-' + atendimentoId).daterangepicker(
            $.extend(true, app.createDateRangePickerOptions(), _selectedDateRange),
            function (start, end, label) {
                _selectedDateRange.startDate = start.format('YYYY-MM-DDT00:00:00Z');
                _selectedDateRange.endDate = end.format('YYYY-MM-DDT23:59:59.999Z');
            });

        const _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.Prescricao.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.Prescricao.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.Prescricao.Delete')
        };

        const _ErrorModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Erros/ExibirErros',
            modalId: 'ModalErro'
        });

        const _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Assistenciais/_CriarOuEditarMedicoPrescricao',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Assistenciais/Medicos/Prescricoes/_CriarOuEditar.js',
            modalClass: 'CriarOuEditarPrescricaoModal'
        });

        const _impressaoAcrescimosESuspensoesModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Assistenciais/impressaoAcrescimosESuspensoes',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Assistenciais/Medicos/Prescricoes/ImpressaoAcrescimosESuspensoesModal.js',
            modalId: 'impressaoAcrescimosESuspensoesModal',
            modalClass:'impressaoAcrescimosESuspensoesModal'
        });

        const _solicitacaoAntimicrobianoModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/SolicitacaoAntimicrobianos/SolicitacaoAntimicrobianoModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Assistenciais/Medicos/SolicitacaoAntimicrobianos/_CriarOuEditarModal.js',
            modalClass: 'solicitacaoAntimicrobianoModal',
            modalId: 'solicitacaoAntimicrobianoModal',
        });

        const _solicitacoesModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Assistenciais/SolicitacoesModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Assistenciais/Medicos/Prescricoes/Solicitacoes/_CriarOuEditarModal.js',
            modalClass: 'solicitacoesModal',
            modalId: 'solicitacoesModal',
        });
        

        $('#PrescricoesTable-' + atendimentoId).jtable({
            title: app.localize('Prescricao'),
            paging: true,
            sorting: true,
            multiSorting: true,
            selecting: true,
            selectingCheckboxes: true,
            actions: {
                listAction: {
                    method: _prescricaoService.listar
                }
            },
            fields: {
                id: {
                    key: true,
                    list: false
                },

                actions: {
                    title: app.localize('Actions'),
                    width: '20%',
                    sorting: false,
                    display: function (data) {
                        var $span = $('<span></span>');
                        if (_permissions.edit && data.record.prescricaoStatusId == 1 || data.record.prescricaoStatusId == 2 || data.record.prescricaoStatusId == 6 || data.record.prescricaoStatusId == 7 || data.record.prescricaoStatusId == 8) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                .appendTo($span)
                                .click(function (e) {
                                    const btn = $(this);
                                    btn.buttonBusy(true);
                                    e.preventDefault();
                                    localStorage.removeItem("RespostasList");
                                    localStorage.removeItem("DivisaoId");
                                    localStorage.removeItem("PrescricaoId");
                                    const target = sessionStorage["TargetConteudo"];
                                    const url = '/Mpa/Assistenciais/_CriarOuEditarMedicoPrescricao?atendimentoid=' + atendimentoId + '&id=' + data.record.id;
                                    
                                    sessionStorage["dataPrescricao"] = data.record.dataPrescricao;
                                    if ($(".portlet.prescricao").length  != 0) {
                                        alert('Existe prescrição em edição.');
                                        btn.buttonBusy(false);
                                    } else {
                                        _prescricaoService.checarMedicoPrescricao(data.record.id).then((res) => {
                                            if (res.hasError) {
                                                alert('Não foi possivel identificar o médico vinculado ao usuário');
                                                return;
                                            }

                                            if (res.isMedico) {
                                                criarNewAba(sessionStorage["id"], sessionStorage["dataRegistro"], sessionStorage["codigoAtendimento"], sessionStorage["paciente"], url, "Prescrição", data.record.id);
                                                return;
                                            }

                                            abp.message.confirm(app.localize('Deseja assumir essa prescrição?'), function (isConfirmed) {
                                                if (isConfirmed) {
                                                    _prescricaoService.alterarMedicoPrescricao(data.record.id).done(function (res) {
                                                        criarNewAba(sessionStorage["id"], sessionStorage["dataRegistro"], sessionStorage["codigoAtendimento"], sessionStorage["paciente"], url, "Prescrição", data.record.id);
                                                    });
                                                }
                                            });
                                        }).always(() => {
                                            setTimeout(() => {
                                                btn.buttonBusy(false);
                                            },1000);
                                        });
                                    }
                                });
                        }
                        if (_permissions.delete && (data.record.prescricaoStatusId == 1)) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                .appendTo($span)
                                .click(function (e) {
                                    e.preventDefault();
                                    deletePrescricoes(data.record);
                                });
                        }
                        //Copiar prescrição
                        if (data.record.prescricaoStatusId == 2 || data.record.prescricaoStatusId == 6 || data.record.prescricaoStatusId == 7 || data.record.prescricaoStatusId == 8) {
                            $('<button class="btn btn-primary btn-xs" title="' + app.localize('CopiarPrescricao') + '"><i class="fa fa-copy"></i></button>')
                                .appendTo($span)
                                .click(function (e) {
                                    e.preventDefault();
                                    copiarPrescricao(data.record.id);
                                });
                        }
                        //Suspender prescrição
                        if (data.record.prescricaoStatusId == 2 || data.record.prescricaoStatusId == 6 || data.record.prescricaoStatusId == 7 || data.record.prescricaoStatusId == 8) {
                            $('<button class="btn btn-danger btn-xs" title="' + app.localize('SuspenderPrescricao') + '"><i class="fa fa-ban"></i></button>')
                                .appendTo($span)
                                .click(function (e) {
                                    e.preventDefault();
                                    suspenderPrescricao(data.record.id);
                                });
                        }
                        //Liberar prescrição - solicitar exames
                        if (data.record.prescricaoStatusId == 1 || data.record.prescricaoStatusId == 7) {
                            $('<button class="btn btn-success btn-xs" title="' + app.localize('LiberarPrescricao') + '"><i class="fa fa-check-square"></i></button>')
                                .appendTo($span)
                                .click(function (e) {
                                    e.preventDefault();
                                    liberarPrescricao(data.record.id);
                                });
                        }

                        //Aprovar prescrição - solicitar estoque
                        if (data.record.prescricaoStatusId == 2 || data.record.prescricaoStatusId == 8) {
                            $('<button class="btn btn-success btn-xs" title="' + app.localize('AprovarPrescricao') + '"><i class="far fa-thumbs-up"></i></button>')
                                .appendTo($span)
                                .click(function (e) {
                                    e.preventDefault();
                                    aprovarPrescricao(data.record.id);
                                });
                        }

                        //Aprovar prescrição - solicitar estoque
                        if (data.record.prescricaoStatusId == 5) {
                            $('<button class="btn btn-success btn-xs" title="' + app.localize('ReAtivar') + '"><i class="far fa-thumbs-up"></i></button>')
                                .appendTo($span)
                                .click(function (e) {
                                    e.preventDefault();
                                    reativarPrescricao(data.record.id);
                                });
                        }

                        if (data.record.prescricaoStatusId !== 7 || data.record.prescricaoStatusId !== 8) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Imprimir Prescrição Completa') + '"><i class="fas fa-file-medical" style="color:#36c6d3"></i></button>')
                                .appendTo($span)
                                .click(function (e) {
                                    e.preventDefault();
                                    imprimirTudo(data.record.id);
                                });
                        }

                        if (data.record.prescricaoStatusId == 7 || data.record.prescricaoStatusId == 8 || data.record.imprimeAcrescimosESuspensoes) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Imprimir acréscimos e suspensões') + '"><i class="fas fa-file-medical" style="color:#36c6d3"></i></button>')
                                .appendTo($span)
                                .click(function (e) {
                                    e.preventDefault();
                                    imprimirAcrescimosESuspensos(data.record.id);
                                });
                        }


                        return $span;
                    }
                },

                prescricaoStatusId: {
                    title: app.localize('Status'),
                    width: '5%',
                    display: function (data) {
                        var $span = $('<div></div>');
                        $('<span class="sw-btn-display" style="background-color:' + data.record.statusCor + ';" title="' + data.record.status + '"></span>')
                            .appendTo($span);
                        return $span;
                    }
                },
                codigo: {
                    title: app.localize('Codigo'),
                    width: '10%',
                    display: function (data) {
                        return data.record.id;
                    }
                },
                "DataPrescricao": {
                    title: app.localize('DataPrescricao'),
                    width: '15%',
                    display: function (data) {
                        return moment(data.record.dataPrescricao).format('L LT');
                    }
                },
                medico: {
                    title: app.localize('Medico'),
                    width: '25%',
                    display: function (data) {
                        return data.record.medico ? data.record.medico : '';
                    }
                },

            },
            selectionChanged: function () {

                atualizaArquivo();
            },

            recordsLoaded: function (event, data) {

                $("div[id^='PrescricoesTable'] .jtable-main-container tr.jtable-data-row:first input[type=checkbox]").trigger('click');
            }


        });
        
        function atualizaArquivo() {
            var $selectedRows = $('#PrescricoesTable-' + atendimentoId).jtable('selectedRows');

            if ($selectedRows.length > 0) {
                $('#criarRegistroButton').enable(true);

                $selectedRows.each(function () {
                    var record = $(this).data('record');
                    exibirRelatorio(record.id);
                });
            }
        }

        function getPrescricoes() {
            $('#PrescricoesTable-' + atendimentoId).jtable('load', createRequestParams());
        }

        function deletePrescricoes(prescricao) {
            abp.message.confirm(
                app.localize('DeleteWarning', prescricao.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _prescricaoService.excluir(prescricao.id)
                            .done(function () {
                                getPrescricoes();
                                abp.notify.success(app.localize('SuccessfullyDeleted'));
                            });
                    }
                }
            );
        }

        function copiarPrescricao(id) {
            swal({
                title: "Copiar Prescrição",
                text: "Copiar Prescricao",
                type: "info",
                showCancelButton: true,
                closeOnConfirm: false,
                showLoaderOnConfirm: true
            }, function (isConfirmCopy) {
                if (isConfirmCopy) {
                    _prescricaoService.validarPrescricaoFutura(atendimentoId).then(res => {
                        if (res) {
                            swal({
                                title: "Copiar Prescrição",
                                text: "Deseja fazer a copia da prescrição para o hoje ou para amanhã?",
                                type: "info",
                                confirmButtonText: "Amanhã",
                                cancelButtonText: "Hoje",
                                showCancelButton: true,
                                closeOnConfirm: true,
                                showLoaderOnConfirm: true
                            }, function (isConfirm) {
                                copiaPrescricao(id, isConfirm);
                            });
                        }
                        else {
                            copiaPrescricao(id, false);
                        }
                    });
                }
                return;

            });
            function copiaPrescricao(id, dataFutura) {
                swal.close();
                startLoader("Copiando");
                $.ajax({
                    url: `/api/services/app/prescricaoMedica/copiar?id=${id}&atendimentoId=${atendimentoId}&dataAgrupamento=&dataFutura=${dataFutura}`,
                    method: 'POST',
                    success: function (data) {
                        stopLoader();
                        if (data != null && data.result != null && data.result != '' && data.result != undefined) {
                            abp.notify.success(app.localize('SuccessfullyCopied'));
                            if (data.result.errors.length > 0) {
                                //swal(app.localize('OperacaoConcluida'), data.result, 'warning');
                                _ErrorModal.open({ erros: data.result.errors });
                                $("#ModalErro .modal-content").css("width", "650px");
                            }
                            atualizaArquivo();
                            getPrescricoes();
                            
                        } 
                        
                    },
                    error: function (request, status, error) {
                        const req = JSON.parse(request.responseText);
                        swal(app.localize('Error'), req.message, 'error');
                        atualizaArquivo();
                        getPrescricoes();
                        stopLoader()
                    }
                });
            }
        }

        function stopLoader() {
            $(".loader").modal('toggle');
            $(".loader .text").html("");
        }
        function startLoader(text) {
            $(".loader .text").html(text);
            $(".loader").modal('toggle');
        }

        function suspenderPrescricao(id) {
            swal({
                title: "Suspender Prescrição",
                text: "Suspender Prescricao",
                type: "info",
                showCancelButton: true,
                closeOnConfirm: false,
                showLoaderOnConfirm: true
            }, function () {
                swal.close();
                startLoader("Suspendendo");
                $.ajax({
                    url: '/api/services/app/prescricaoMedica/suspender?id=' + id + '&atendimentoId=' + atendimentoId,
                    method: 'POST',
                    //data: {
                    //    id: id,
                    //    atendimentoId: atendimentoId
                    //},
                    success: function (data) {
                        stopLoader();
                        if (data != null && data != '' && data != undefined) {
                            abp.notify.success(app.localize('SuccessfullySuspended'));
                        }
                        atualizaArquivo();
                        getPrescricoes();
                    },
                    error: function (request, status, error) {
                        const req = JSON.parse(request.responseText);
                        swal(app.localize('Error'), req.message, 'error');
                        atualizaArquivo();
                        stopLoader();
                    }
                });
            });
        }

        var aberto = false;
        function modalImprimirAbreEFecha() {
            $('.modal-imprimir-presricao').find('.naoImprimir').unbind("click", naoImprimir);
            $('.modal-imprimir-presricao').find('.imprimir-acrescimos-suspensos').unbind("click", imprimirAcrescimosESuspensosAction);
            $('.modal-imprimir-presricao').find('.imprimir-tudo').unbind("click", imprimirTudoAction);
            $('.modal-imprimir-presricao').find('.naoImprimir').on("click", naoImprimir);
            $('.modal-imprimir-presricao').find('.imprimir-acrescimos-suspensos').on("click", imprimirAcrescimosESuspensosAction);
            $('.modal-imprimir-presricao').find('.imprimir-tudo').on("click", imprimirTudoAction);
            
            $('.modal-imprimir-presricao').modal('toggle');
        }


        function naoImprimir(event) {
            event.stopImmediatePropagation();
            modalImprimirAbreEFecha();
        }



        function imprimirAcrescimosESuspensosAction(event) {
            event.stopImmediatePropagation();
            imprimirAcrescimosESuspensos($("#idSelecionado").val());
            modalImprimirAbreEFecha();
        }

        function imprimirTudoAction(event) {
            event.stopImmediatePropagation();
            imprimirTudo($("#idSelecionado").val());
            modalImprimirAbreEFecha();
        }


        abp.event.on("liberarPrescricaoIndex", (prescricaoId) => {
            swal({ title: "Liberar Prescrição", text: "Liberar Prescrição", type: "info", showCancelButton: true, closeOnConfirm: false, showLoaderOnConfirm: true },
                (val) => {
                    if (val) {
                        $("button.confirm").buttonBusy(true);
                        swal.close();
                        startLoader("Liberando");
                        $.ajax({
                            url: '/api/services/app/prescricaoMedica/liberar?id=' + prescricaoId + '&atendimentoId=' + atendimentoId,
                            method: 'POST',
                            success: (data) => {
                                stopLoader()
                                if (data != null && data != '' && data != undefined) {
                                    abp.notify.success(app.localize('SuccessfullyLiberated'));
                                }
                                atualizaArquivo();
                                debugger;
                                getPrescricoes();
                                $("button.confirm").buttonBusy(false);
                                
                                $("#idSelecionado").val(prescricaoId);
                                modalImprimirAbreEFecha();
                                
                            },
                            error: (request, status, error) => {
                                const req = JSON.parse(request.responseText);
                                swal(app.localize('Error'), req.message, 'error');
                                $("button.confirm").buttonBusy(false);
                                atualizaArquivo();
                                stopLoader()
                            }
                        });
                    }
                });
        });

        function liberarPrescricao(id) {
            
            return Promise.all([
                    _solicitacaoAntimicrobianoService.validaSolicitacaoAntimicrobianoPorPrescricao(atendimentoId,id), 
                    solicitacaoAutorizacaoAppService.validaSolicitacaoAutorizacaoPorPrescricao(atendimentoId, id)])
                .then(data => {
                    const resAntimicrobiano = data[0];
                    const resAutorizacao = data[1];
                if(resAntimicrobiano.necessitaSolicitacao || resAutorizacao.necessitaSolicitacao) {
                    return _solicitacoesModal.open({ atendimentoId: atendimentoId, prescricaoId: id});                    
                }
                else {
                    abp.event.trigger("liberarPrescricaoIndex", id);
                }
            })
        }



        function aprovarPrescricao(id) {
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
                    startLoader("Aprovando");
                    $.ajax({
                        url: '/api/services/app/prescricaoMedica/aprovar?id=' + id + '&atendimentoId=' + atendimentoId,
                        method: 'POST',
                        success: function (data) {
                            stopLoader()
                            if (data != null && data != '' && data != undefined) {
                                abp.notify.success(app.localize('SuccessfullyApproved'));
                            }
                            atualizaArquivo();
                            getPrescricoes();
                            $("button.confirm").buttonBusy(false);
                        },
                        error: function (request, status, error) {
                            const req = JSON.parse(request.responseText);
                            swal(app.localize('Error'), req.message, 'error');
                            $("button.confirm").buttonBusy(false);
                            atualizaArquivo();
                           stopLoader()
                        }

                    });
                }
            });
        }


        function imprimirAcrescimosESuspensos(id) {
            _impressaoAcrescimosESuspensoesModal.open({ atendimentoId: atendimentoId, prescricaoMedicaId: id});
        }

        function imprimirTudo(id) {
            if (id == null || id == undefined) {
                return;
            }
            $.removeCookie("XSRF-TOKEN");
            printJS({
                printable: '/Mpa/AssistenciaisRelatorios/imprimirTudo?prescricaoId=' + id, type: 'pdf',
                onPrintDialogClose: () => {
                    modalImprimirAbreEFecha();
                }
            })
            //fetch('/Mpa/AssistenciaisRelatorios/imprimirTudo?prescricaoId=' + id, { method: "GET" })
            //    .then(resp => resp.blob())
            //    .then(blob => {
            //        //const url = window.URL.createObjectURL(blob);
            //        //const a = document.createElement('a');
            //        //a.style.display = 'none';
            //        //a.href = url;
            //        //// the filename you want
            //        //a.download = 'prescricao.pdf';
            //        //document.body.appendChild(a);
            //        //a.click();
            //        //window.URL.revokeObjectURL(url);
            //        printJS({ printable: '/Mpa/AssistenciaisRelatorios/imprimirTudo?prescricaoId=' + id, type: 'pdf' })
            //    });
        }

        function createRequestParams() {
            var prms = {};
            _$filterForm.serializeArray().map(function (x) { prms[x.name] = x.value; });
            return $.extend(prms, _selectedDateRange);
        }

        function reativarPrescricao(id) {
            swal({
                title: "Reativar Prescrição",
                text: "Reativar Prescricao",
                type: "info",
                showCancelButton: true,
                closeOnConfirm: false,
                showLoaderOnConfirm: true
            }, function () {
                $.ajax({
                    url: '/api/services/app/prescricaoMedica/ReAtivar?id=' + id,
                    method: 'POST',
                    //data: {
                    //    id: id,
                    //    atendimentoId: atendimentoId
                    //},
                    success: function (data) {

                        if (data != null && data != '' && data != undefined) {
                            swal(app.localize('OperacaoConcluida'), '', 'success');
                        }
                        else {
                            abp.notify.success(app.localize('SuccessfullySuspended'));
                        }
                        atualizaArquivo();
                        getPrescricoes();
                    },
                    error: function (request, status, error) {
                        var req = JSON.parse(request.responseText);
                        swal(app.localize('Error'), req.message, 'error');
                        atualizaArquivo();
                    }
                });
            });
        }

        $('#ShowAdvancedFiltersSpan').click(function () {
            $('#ShowAdvancedFiltersSpan').hide();
            $('#HideAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideDown();
        });

        $('#HideAdvancedFiltersSpan').click(function () {
            $('#HideAdvancedFiltersSpan').hide();
            $('#ShowAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideUp();
        });

        $('#CreateNewPrescricaoButton-' + atendimentoId).click(function (e) {
            e.preventDefault();

            if ($(".portlet.prescricao").length  == 0) {

                _prescricaoService.validarPrescricaoFutura(atendimentoId).then(res => {
                    console.log(res);
                    if (res) {
                        swal({
                            title: "Data Prescrição",
                            text: "Deseja fazer a prescrição para o hoje ou para amanhã?",
                            type: "info",
                            confirmButtonText: "Amanhã",
                            cancelButtonText: "Hoje",
                            showCancelButton: true,
                            closeOnConfirm: true,
                            showLoaderOnConfirm: true
                        }, function (isConfirm) {
                            abrePrescricao(isConfirm);
                        });
                    }
                    else {
                        abrePrescricao(false);
                    }
                });
            }
            else {
                alert('Existe prescrição em edição.');
            }


            function abrePrescricao(dataFutura) {
                dataFutura = dataFutura ?? false;

                localStorage.removeItem("RespostasList");
                localStorage.removeItem("DivisaoId");
                localStorage.removeItem("PrescricaoId");
                var target = sessionStorage["TargetConteudo"];
                var url = '/Mpa/Assistenciais/_CriarOuEditarMedicoPrescricao?atendimentoid=' + atendimentoId;
                if (dataFutura) {
                    url += '&dataFutura=true';
                    sessionStorage["dataPrescricao"] = moment().add('1', 'days').startOf('day').format();
                    console.log(sessionStorage["dataPrescricao"]);
                }
                else {
                    sessionStorage["dataPrescricao"] = moment().format();
                }
                $('#' + target).data('reload', '0');
                criarNewAba(sessionStorage["id"], sessionStorage["dataRegistro"], sessionStorage["codigoAtendimento"], sessionStorage["paciente"], url, "Prescrição");
            }
        });


        $('#ExportarPrescricoesParaExcelButton-' + atendimentoId).click(function () {
            _prescricaoService
                .listarParaExcel({
                    filtro: $('#PrescricoesTableFilter-' + atendimentoId).val(),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetPrescricoesButton-' + atendimentoId).on('click', function (e) {
            e.preventDefault();
            getPrescricoes();
        });

        $('#RefreshPrescricoesListButton-' + atendimentoId).on('click', function (e) {
            e.preventDefault();
            getPrescricoes();
        });

        abp.event.on('app.CriarOuEditarPrescricaoModalSaved', function () {
            getPrescricoes();
        });

        abp.event.on('app.CriarOuEditarPrescricaoCompletaModalSaved', function () {
            getPrescricoes();
        });

        getPrescricoes();

        $('#PrescricoesTableFilter-' + atendimentoId).focus();

        function exibirRelatorio(prescricaoId) {
            var action = "VisualizarPrescricao?prescricaoId=" + prescricaoId;
            var caminho = "/mpa/AssistenciaisRelatorios/" + action;
            PDFObject.embed(caminho, "#imagemPrescricoes-" + atendimentoId);
        }

    });
})();