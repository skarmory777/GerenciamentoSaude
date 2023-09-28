(function () {
    $(function () {
        var stick = $('.sticky-top');
        var _$AssistencialAtendimentosTable = $('#AssistencialAtendimentosTable-' + localStorage["TipoAtendimento"]);
        var _atendimentosService = abp.services.app.atendimento;
        var _$filterForm = $('#AssistencialAtendimentosFilterForm-' + localStorage["TipoAtendimento"]);
        var _terminalSenhasService = abp.services.app.terminalSenhas;
        var _senhasService = abp.services.app.senha;
        var msg = '';
        var paramts = getParameterByName('id');

        var _selectedDateRange = {
            startDate: moment().add(-1, "days").startOf('day'),
            endDate: moment().endOf('day')
        };

        var pickerOptions = app.createDateRangePickerOptions();
        var todayYesterdayProperty = {};

        function atualizarWidthStick() {
            setTimeout(function () {
                const scroll = window.innerWidth-$(document).width();
                if(scroll > 0) {
                    stick.css("width", $(".portlet.container-content").width() + (window.innerWidth-$(document).width()));
                }else {
                    stick.css("width", $(".portlet.container-content").outerWidth());
                }
            }, 60);
        }

        $(window).resize(function () {
            atualizarWidthStick();
        });

        $('.sidebar-toggler').on('click', function (e) {
            atualizarWidthStick();

        });

        $(".portlet.container-content").resize(function (e) {
            atualizarWidthStick();
        });

        atualizarWidthStick();

        atualizaGrid();


        document.addEventListener('visibilitychange', function () {
            var activeLi = $("#abas-amb").find("li.active");
            getAssistencialAtendimentos();
            setTimeout(() => {
                activeLi.find("a").trigger("click");
            },0)
        });

        $(".AssistencialAtendimentosFilterForm").submit((event) => {
            event.preventDefault();
            getAssistencialAtendimentos();
        });



        todayYesterdayProperty[app.localize('Yesterday/Today')] = [moment().add(-1, "days").startOf('day'), moment().endOf('day')];
        pickerOptions.ranges = Object.assign(todayYesterdayProperty, pickerOptions.ranges);

        $('#date-field-' + localStorage["TipoAtendimento"]).daterangepicker(
            $.extend(true, pickerOptions, _selectedDateRange),
            function (start, end, label) {

                _selectedDateRange.startDate = start.format('YYYY-MM-DDT00:00:00Z');
                _selectedDateRange.endDate = end.format('YYYY-MM-DDT23:59:59.999Z');
                getAssistencialAtendimentos();
            });
        $('#date-field-' + localStorage["TipoAtendimento"]).on('apply.daterangepicker',
            function (ev, picker) {
                var chosenLabel = $(this).data('daterangepicker').chosenLabel;
                var ranges = $(this).data('daterangepicker').ranges;
                var selectedRange = ranges[chosenLabel];
                if (selectedRange) {
                    _selectedDateRange.startDate = selectedRange[0].format('YYYY-MM-DDT00:00:00Z');
                    _selectedDateRange.endDate = selectedRange[1].format('YYYY-MM-DDT23:59:59.999Z');
                }
                getAssistencialAtendimentos();
            });
        $('#date-field-' + localStorage["TipoAtendimento"]).on('cancel.daterangepicker', function (ev, picker) {
            _selectedDateRange.startDate = null;
            _selectedDateRange.endDate = null;
            $(this).val('');
            getAssistencialAtendimentos();
        });

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Assistenciais/CriarOuEditarAssistencialAtendimentoModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Assistenciais/Home/_CriarOuEditar.js',
            modalClass: 'CriarOuEditarAssistencialAtendimentoModal'
        });

        var _atendimentoModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Atendimentos/CriarOuEditarModal',
            //scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Atendimentos/Home/_CriarOuEditar.js',
            modalClass: 'CriarOuEditarAtendimentoModal'
        });

        var _registrosArquivos = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Assistenciais/ListarRegistroArquivos',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Assistenciais/RegistrosArquivos/Index.js',
            modalClass: 'ListarRegistroArquivos'
        });

        var _modalAlta = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/AtendimentoLeitoMov/_AltaModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Atendimentos/Altas/Alta/_CriarOuEditarModal.js',
            modalClass: 'AltaModalViewModel'
        });

        var _pacienteDiagnosticos = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Assistenciais/CadastroPacienteDiagnosticos',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Assistenciais/PacienteDiagnosticos/PacienteDiagnosticosModal.js',
            //scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Atendimentos/Home/_CriarOuEditar.js',
            modalClass: 'PacienteDiagnosticosModal'
        });

        var _pacienteAlergias = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Assistenciais/CadastroPacienteAlergias',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Assistenciais/PacienteAlergias/PacienteAlergiasModal.js',
            //scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Atendimentos/Home/_CriarOuEditar.js',
            modalClass: 'PacienteAlergiasModal'
        });

        var _pacientePesoAltura = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Pacientes/CadastroPacientePeso',
            //scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Atendimentos/Home/_CriarOuEditar.js',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Pacientes/PacientePesosModal.js',
            modalClass: 'PacientePesoModal'
        });

        var _modeloPrescricao = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Assistenciais/SelecionarModelo',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Assistenciais/ModelosPrescricoes/SelecionarModelePrescricaoModal.js',
            //scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Pacientes/PacientePesosModal.js',
            modalClass: 'SelecionarModelePrescricaoModal'
        });


        var _altaFinalizarAtendimento = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/AtendimentoLeitoMov/_AltaModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Atendimentos/Altas/Alta/_CriarOuEditarModal.js',
            modalClass: 'AltaFinalizarAtendimentoModal'
        });

        var _pendenciaFinalizarAtendimento = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Assistenciais/PendenciaFinalizarAtendimento',
            modalClass: 'PendenciaFinalizarAtendimentoModal'
        });



        _$AssistencialAtendimentosTable.jtable({

            title: app.localize('Atendimentos'),
            paging: true,
            sorting: true,
            multiSorting: true,
            selecting: false, //Enable selecting
            multiselect: false, //Allow multiple selecting
            selectingCheckboxes: true, //Show checkboxes on first column

            actions: {
                listAction: {
                    method: localStorage["TipoAtendimento"] === "amb" ? _atendimentosService.listarFiltro : _atendimentosService.listarFiltroInternacao
                }
            },

            //rowInserted: function (event, data) {
            //    if (data) {

            //        if (data.record.corClassificacao) {

            //            data.row[0].cells[3].setAttribute('bgcolor', data.record.corClassificacao);
            //           // data.row[0].cells[2].setAttribute('color', data.record.CorLancamentoLetra);
            //        }
            //    }

            //},


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


                        $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                            .appendTo($span)
                            .click(function () {
                                sessionStorage["id"] = data.record.id;
                                sessionStorage["dataRegistro"] = data.record.dataRegistro;
                                sessionStorage["codigoAtendimento"] = data.record.codigoAtendimento;
                                sessionStorage["paciente"] = data.record.paciente;
                                if (data.record.senha && data.record.senha !== "") {
                                    swal({
                                        title: "Chamar Paciente",
                                        text: "Deseja chamar o paciente?",
                                        type: "info",
                                        showCancelButton: true,
                                        closeOnConfirm: true,
                                        showLoaderOnConfirm: true,
                                        cancelButtonText: app.localize("No"),
                                        cancelButtonColor: "#DD6B55",
                                        confirmButtonColor: "#3598dc",
                                        confirmButtonText: app.localize("Yes"),
                                    }, function (isConfirmed) {

                                        if (isConfirmed) {
                                            if ($('#tipo-local-chamada-id').val() && $('#local-chamada-id').val()) {
                                                chamarPaciente(data.record.senhaAtendimentoId);
                                            } else {
                                                abp.notify.error("É necessário preencher os campos <b>" +
                                                    app.localize("TipoLocalChamada") +
                                                    "</b> e <b>" +
                                                    app.localize("LocalChamada") + "</b> para chamar um paciente!");
                                                $('.jtable-row-selected').removeClass('jtable-row-selected');
                                                $(':checked').removeAttr('checked');
                                                return;
                                            }
                                        }

                                        window.open(window.location.href + "?id=" + data.record.id);
                                        $('.jtable-row-selected').removeClass('jtable-row-selected');
                                        $(':checked').removeAttr('checked');
                                    });
                                } else {
                                    window.open(window.location.href + "?id=" + data.record.id);
                                    $('.jtable-row-selected').removeClass('jtable-row-selected');
                                    $(':checked').removeAttr('checked');
                                }
                            });

                        $('<button class="btn btn-default btn-xs" title="' + "PEP - Prontuário Eletrônico do Paciente" + '"><i class="fa fa-tasks"></i></button>')
                            .appendTo($span)
                            .click(function () {
                                _registrosArquivos.open({ id: data.record.id });

                            });

                        //if (_permissionsInternacao.alta && (!data.record.dataAlta || data.record.dataAlta == null || data.record.dataAlta == undefined || data.record.dataAlta == '') && data.record.atendimentoMotivoCancelamentoId == null)
                        //{
                        $('<button class="btn btn-default btn-xs" title="' + app.localize('Alta') + '"><i class="fa fa-blind fa-3"></i></button>')
                            .appendTo($span)
                            .click(function () {
                                //e.preventDefault();

                                _modalAlta.open({ atendimentoId: data.record.id });
                            });
                        //}


                        return $span;
                    }
                },
                statusDescricao: {
                    title: app.localize('Risco'),
                    width: '1%',
                    display: function (data) {
                        return '<div   style="align:center;" > <span style="display:inline-block; width:20px; height:20px; top:100px; text-align:center; background-color:' + data.record.corClassificacao + '; border-radius: 25px;">  </span> </div>'
                    }
                },
                senha: {
                    title: app.localize('Senha'),
                    width: '1%',
                    display: function (data) {
                        return data.record.senha;
                    }
                },
                status: {
                    title: app.localize('Status'),
                    width: '8%',
                    display: function (data) {
                        var html = '';
                        if (data.record.status) {
                            html += '<span style="display:inline-block; padding:2px 4px;color:' + data.record.corTexto + ' !important;">' + data.record.status + '</span>';
                        }

                        if (data.record.statusAguardando && data.record.statusAguardando !== 0) {
                            switch (data.record.statusAguardando) {
                                case 1:
                                    {
                                        html += '<span style="display:inline-block; margin:2px;color:' + data.record.corTexto + ' !important;">' + app.localize("Recepção") + '</span>';
                                        break;
                                    }
                                case 2:
                                    {
                                        html += '<span style="display:inline-block; margin:2px;color:' + data.record.corTexto + ' !important;">' + app.localize("Médico") + '</span>';
                                        break;
                                    }
                                case 3:
                                    {
                                        html += '<span style="display:inline-block;margin:2px;color:' + data.record.corTexto + ' !important;">' + app.localize("Triagem") + '</span>';
                                        break;
                                    }

                                default:
                            }
                        }

                        if (data.record.statusAtendido && data.record.statusAtendido !== 0) {
                            switch (data.record.statusAtendido) {
                                case 1:
                                    {
                                        html += '<span style="display:inline-block; margin:2px;color:' + data.record.corTexto + ' !important;">' + app.localize("AguardandoInternacao") + '</span>';
                                        break;
                                    }
                                case 2:
                                    {
                                        html += '<span style="display:inline-block; margin:2px;color:' + data.record.corTexto + ' !important;">' + app.localize("Internado") + '</span>';
                                        break;
                                    }
                                case 3:
                                    {
                                        html += '<span style="display:inline-block; margin:2px;color:' + data.record.corTexto + ' !important;">' + app.localize("Alta") + '</span>';
                                        break;
                                    }

                                default:
                            }
                        }

                        if (data.record.isPendenteExames) {
                            html += '<span style="display:inline-block; float:right; margin:2px;cursor:pointer;color:' + data.record.corTexto + ' !important;" title="Pendente de Exames"><i style="font-size: 1.5em;" class="fas fa-stethoscope"></i></span>';
                        }
                        if (data.record.isPendenteMedicacao) {
                            html += '<span style="display:inline-block; float:right; margin:2px;cursor:pointer;color:' + data.record.corTexto + ' !important;" title="Pendente de Medicação"><i style="font-size: 1.5em;" class="fas fa-pills"></i></span>';
                        }

                        if (data.record.isPendenteProcedimento) {
                            html += '<span style="display:inline-block; float:right; margin:2px;cursor:pointer;color:' + data.record.corTexto + ' !important;" title="Pendente de Procedimentos"><i style="font-size: 1.5em;" class="fas fa-procedures"></i></span>';
                        }

                        if (html) {
                            html = '<div style="align:center;color:' + data.record.corTexto + ' !important;" >' + html + '</div>';
                        }

                        return html;
                    }
                },
                QtdImagem: {
                    title: app.localize('Status Img'),
                    tooltip: app.localize('Status Imagens'),
                    width: '3%',
                    sorting: false,
                    display: function (data) {
                        let dataParam = { atendimentoId: data.record.id, tipo: 'Img' };
                        let html = "<div style='width: 100%;text-align:center;cursor:pointer' class='tooltip-result' data-param='" + JSON.stringify(dataParam) + "'> ";
                        html += '<span style="display:inline-block; margin:2px;font-weight: 500;font-size: 14px;">' + (data.record.qtdLauMovimentoItem || 0) + '</span>';
                        html += '<span style="display:inline-block; margin:2px;font-weight: 600;font-size: 14px;">/</span>';
                        html += '<span style="display:inline-block; margin:2px;font-weight: 500;font-size: 14px;">' + (data.record.qtdLauAssSolicitacaoExame || 0) + '</span>';
                        html += "</div>";
                        return html;
                    }
                },
                QtdLaboratorio: {
                    title: app.localize('Status Lab'),
                    tooltip: app.localize('Status Laboratório'),
                    width: '3%',
                    sorting: false,
                    display: function (data) {
                        let dataParam = { atendimentoId: data.record.id, tipo: 'Lab' };
                        let html = "<div style='width: 100%;text-align:center;cursor:pointer' class='tooltip-result' data-param='" + JSON.stringify(dataParam) + "'> ";
                        html += '<span style="display:inline-block; margin:2px;font-weight: 500;font-size: 14px;">' + (data.record.qtdLabResultadoExame || 0) + '</span>';
                        html += '<span style="display:inline-block; margin:2px;font-weight: 600;font-size: 14px;">/</span>';
                        html += '<span style="display:inline-block; margin:2px;font-weight: 500;font-size: 14px;">' + (data.record.qtdLabAssSolicitacaoExame || 0) + '</span>';
                        html += "</div>";
                        return html;
                    }
                },
                "Paciente.NomeCompleto": {
                    title: app.localize('Paciente'),
                    width: '15%',
                    display: function (data) {
                        var nome = ''
                        if (data.record.paciente !== null) {
                            nome = data.record.paciente;
                        }
                        return nome;
                    }
                },
                "pacienteNascimento": {
                    title: app.localize('Idade'),
                    width: '5%',
                    display: function (data) {
                        var idade = '';
                        if (data.record.pacienteIdade !== null) {
                            idade = data.record.pacienteIdade;
                        }
                        return idade;
                    }
                },
                dataRegistro: {
                    title: app.localize('DataAtendimento'),
                    width: '8%',
                    display: function (data) {
                        return data.record.dataRegistro !== '' ? moment(data.record.dataRegistro).format('L LT') : '';
                    }
                },
                codigoAtendimento: {
                    title: app.localize('Codigo'),
                    width: '5%',
                    display: function (data) {
                        var ans = '';
                        if (data.record.codigoAtendimento !== null && data.record.codigoAtendimento !== '' && data.record.codigoAtendimento !== undefined) {
                            ans = zeroEsquerda(data.record.codigoAtendimento, '10');
                        }
                        return ans;
                    }
                },
                medicoId: {
                    title: app.localize('Medico'),
                    width: '15%',
                    display: function (data) {
                        var nome = ''
                        if (data.record.medico !== null) {
                            nome = data.record.medico;
                        }
                        return nome;
                    }
                },
                convenioId: {
                    title: app.localize('Convenio'),
                    width: '10%',
                    display: function (data) {
                        var nome = ''
                        if (data.record.convenio !== null) {
                            nome = data.record.convenio;
                        }
                        return nome;
                    }
                },
                empresaId: {
                    title: app.localize('Empresa'),
                    width: '11%',
                    display: function (data) {
                        var nome = ''
                        if (data.record.empresa !== null) {
                            nome = data.record.empresa;
                        }
                        return nome;
                    }
                },
                protocolo: {
                    title: app.localize('Protocolo'),
                    width: '10%',
                    display: function (data) {
                        var nome = ''
                        if (data.record.protocolo !== null) {
                            nome = data.record.protocolo;
                        }
                        return nome;
                    }
                },
                isControlaTev: {
                    list: true, //$(this).data('record').isControlaTev,
                    title: app.localize('Tev'),
                    width: '3%',
                    display: function (data) {
                        var span;
                        if (data.record.isControlaTev) {
                            abp.services.app.tevMovimento.obterUltimo(data.record.id, { async: false })
                                .done(function (record) {
                                    if (record != null && record != '' && record != undefined) {
                                        var dd = moment()._d;
                                        var strD = dd.getFullYear() + "-" + zeroEsquerda(dd.getMonth() + 1, 2) + "-" + zeroEsquerda(dd.getDate(), 2) + "T00:00:00";
                                        var date1 = new Date(record.data);
                                        var date2 = new Date(strD);
                                        var timeDiff = Math.abs(date2.getTime() - date1.getTime());
                                        var diffDays = Math.ceil(timeDiff / (1000 * 3600 * 24));
                                        if (record.risco.codigo > 4) {
                                            msg += 'O paciente ' + data.record.paciente + ' possui um alto grau de risco segundo o protocolo TEV\n';
                                            span = '<span class="label label-danger blink" title="Atenção, o paciente ' + data.record.paciente + ' possui um alto grau de risco segundo o protocolo TEV">' + app.localize('Yes') + '</span>';
                                        }
                                        else if (diffDays >= 2) {
                                            msg += 'O paciente ' + data.record.paciente + ' está com sua conferência atrasada segundo o protocolo TEV\n';
                                            span = '<span class="label label-danger blink" title="Atenção, o paciente ' + data.record.paciente + ' está com sua conferência atrasada segundo o protocolo TEV">' + app.localize('Yes') + '</span>';
                                        }
                                        else if (diffDays == 1) {
                                            span = '<span class="label label-warning">' + app.localize('Yes') + '</span>';
                                        }
                                        else {
                                            span = '<span class="label label-success">' + app.localize('Yes') + '</span>';
                                        }
                                    }
                                    else {
                                        msg += 'Não há registro de avaliação de risco Tev para o paciente ' + data.record.paciente + '. Favor realizar a avaliação.\n';
                                        span = '<span class="label label-danger blink" title="Atenção, não há registro de avaliação de risco Tev para o paciente ' + data.record.paciente + '. Favor realizar a avaliação.">' + app.localize('Yes') + '</span>';

                                    }
                                });
                            return span;
                        }
                        else {
                            return '<span class="label label-info">' + app.localize('No') + '</span>';
                        }
                    }
                }
            },
            selectionChanged: function () {


                //Get all selected rows
                //var $selectedRows = $('#AssistencialAtendimentosTable-' + localStorage["TipoAtendimento"]).jtable('selectedRows');
                var record = $('#AssistencialAtendimentosTable-' + localStorage["TipoAtendimento"]).jtable('registroSelecionado');
                criarAba(record);
                $('.jtable-row-selected').removeClass('jtable-row-selected');
                $(':checked').removeAttr('checked');
                //getAssistencialAtendimentos();
            },
            rowInserted: function (event, data) {
                if (data.record) {
                    $(data.row[0].cells[3]).css('background-color', data.record.corFundo);
                }
            },
            recordsLoaded: function (event, data) {

                if (msg.length > 0) {
                    //alertSw('Controle TEV', msg, 'warning');
                    alert('Controle TEV', msg, 'warning');
                    msg = '';
                }

                if (getParameterByName('id') == null) {
                    $("#ChamadaPep").addClass('hide');
                }
                else {
                    $("#ChamadaPep").removeClass('hide');
                }

                $('.tooltip-result').tooltipster({
                    content: 'Carregando...',
                    side: 'right',
                    repositionOnScroll: true,
                    trackTooltip: true,
                    theme: 'tooltipster-shadow',
                    updateAnimation: 'fade',
                    trigger: 'custom',
                    contentAsHTML: true,
                    interactive: true,
                    triggerOpen: {
                        click: true,
                        mouseenter: true,
                    },
                    triggerClose: {
                        mouseleave: true,
                    },
                    functionBefore: function (instance, helper) {
                        let $origin = $(helper.origin);
                        let $param = $origin.data('param');
                        $origin.data('loaded', false);
                        // we set a variable so the data is only loaded once via Ajax, not every time the tooltip opens
                        if ($origin.data('loaded') !== true) {
                            $.ajax({
                                url: '/mpa/Assistenciais/DetalhamentoQuantidadeExames?id=' + $param.atendimentoId + '&tipo=' + $param.tipo,
                                contentType: 'application/json; charset=utf-8',
                                success: (data) => {
                                    $("#updateGrid").trigger("click");
                                    // call the 'content' method to update the content of our tooltip with the returned data.
                                    // note: this content update will trigger an update animation (see the updateAnimation option)

                                    instance.content(data);



                                    // to remember that the data has been loaded
                                    $origin.data('loaded', true);

                                    updateEvent();
                                },
                                error: () => {
                                }
                            });
                        }


                        function updateEvent() {
                            setTimeout(() => {
                                let resultadosExamesTableFields = {
                                    id: {
                                        key: true,
                                        list: false,
                                        width: '1%',
                                    },
                                    dataSolicitacao: {
                                        title: app.localize('Data'),
                                        width: '25%',
                                        display: function (data) {
                                            return moment(data.record.dataSolicitacao).format('L LT');
                                        }
                                    },
                                    medicoSolicitante: {
                                        title: app.localize('MedicoSolicitante'),
                                        width: '74%'
                                        //display: function (data) {
                                        //    return data.record.medicoSolicitante.nomeCompleto;
                                        //}
                                    },
                                };

                                let detalharResultadosExameTableFields = {
                                    actions: {
                                        title: app.localize('Actions'),
                                        width: '5%',
                                        sorting: false,
                                        display: function (data) {
                                            var $div = $("<div></div>");
                                            
                                            if (data.record.accessNumber) {
                                                var $span = $('<span style="padding:5px"></span>');
                                                $('<button class="btn btn-default btn-xs" title="' + app.localize('AccessNumber') + '"><i class="fas fa-images"></i></button>')
                                                    .appendTo($span)
                                                    .click(function () {
                                                        window.open(data.record.accessNumber);
                                                    });
                                                $div.appendTo($span);
                                            }

                                            return $div;
                                        }
                                        
                                    },
                                    faturamentoItem: {
                                        title: app.localize('Exame'),
                                        width: '40%',
                                        sorting: false,
                                    },
                                    material: {
                                        title: app.localize('Material'),
                                        width: '30%',
                                        sorting: false,
                                    },
                                    descricao: {
                                        title: app.localize('Status'),
                                        width: '25%',
                                        sorting: false,
                                    }
                                };


                                if ($param.tipo == "Lab") {
                                    resultadosExamesTableFields = {
                                        id: {
                                            width: '1%',
                                            key: true,
                                            list: false
                                        },
                                        actions: {
                                            title: app.localize('Actions'),
                                            width: '5%',
                                            sorting: false,
                                            display: function (data) {
                                                var $div = $("<div></div>");
                                                
                                                    var $span = $('<span style="padding:5px"></span>');
                                                    $('<button class="btn btn-default btn-xs" title="' + app.localize('Resultado') + '"><i class="fas fa-microscope"/></i></button>')
                                                        .appendTo($span)
                                                        .click(function () {
                                                            $.get('/Mpa/Assistenciais/MedicoResultadoExame?id=' + $param.atendimentoId).then((res) => {
                                                                $('.resultadoExame').append($(res));
                                                            });
                                                        });
                                                $span.appendTo($div);

                                                return $div;
                                            }
                                        },
                                        dataSolicitacao: {
                                            title: app.localize('Data'),
                                            width: '25%',
                                            display: function (data) {
                                                return moment(data.record.dataSolicitacao).format('L LT');
                                            }
                                        },
                                        medicoSolicitante: {
                                            title: app.localize('MedicoSolicitante'),
                                            width: '69%'
                                            //display: function (data) {
                                            //    return data.record.medicoSolicitante.nomeCompleto;
                                            //}
                                        },
                                    };

                                    detalharResultadosExameTableFields = {
                                        faturamentoItem: {
                                            title: app.localize('Exame'),
                                            width: '40%',
                                            sorting: false,
                                        },
                                        material: {
                                            title: app.localize('Material'),
                                            width: '30%',
                                            sorting: false,
                                        },
                                        descricao: {
                                            title: app.localize('Status'),
                                            width: '30%',
                                            sorting: false,
                                        }
                                    };

                                    console.log(resultadosExamesTableFields);
                                    console.log(detalharResultadosExameTableFields);
                                }

                                $('.SolicitacoesExamesTable').jtable({

                                    title: app.localize('SolicitacaoExame'),
                                    paging: true,
                                    pageSize: 5,
                                    sorting: true,
                                    multiSorting: true,
                                    selecting: true,
                                    multiselect: false,
                                    selectingCheckboxes: true,

                                    actions: {
                                        listAction: {
                                            method: abp.services.app.atendimento.getDetalhamentoExamesSolicitacao
                                        }
                                    },

                                    fields: {
                                        id: {
                                            key: true,
                                            list: false
                                        },
                                        codigo: {
                                            title: app.localize('Codigo'),
                                            width: '15%',
                                            display: function (data) {
                                                return zeroEsquerda(data.record.codigo, '8');
                                            }
                                        },
                                        dataSolicitacao: {
                                            title: app.localize('Data'),
                                            width: '25%',
                                            display: function (data) {
                                                return moment(data.record.dataSolicitacao).format('L LT');
                                            }
                                        },
                                        medicoSolicitante: {
                                            title: app.localize('MedicoSolicitante'),
                                            width: '30%'
                                        },
                                    },
                                    selectionChanged: function () {
                                        //Get all selected rows
                                        var $selectedRows = $('.SolicitacoesExamesTable').jtable('selectedRows');
                                        if ($selectedRows.length > 0) {
                                            //Show selected rows
                                            $selectedRows.each(function () {
                                                var record = $(this).data('record');
                                                getDetalharSolicitacoesExames(record.id);
                                            });
                                        }
                                    },
                                    recordsLoaded: function (event, data) {
                                        $(".SolicitacoesExamesTable .jtable-main-container tr.jtable-data-row:first input[type=checkbox]").trigger('click');
                                    }
                                });

                                $('.ResultadosExamesTable').jtable({

                                    title: app.localize('SolicitacaoExame'),
                                    paging: true,
                                    pageSize: 5,
                                    sorting: true,
                                    multiSorting: true,
                                    selecting: true,
                                    multiselect: false,
                                    selectingCheckboxes: true,

                                    actions: {
                                        listAction: {
                                            method: abp.services.app.atendimento.getDetalhamentoExamesResultado
                                        }
                                    },

                                    fields: resultadosExamesTableFields,
                                    selectionChanged: function () {
                                        //Get all selected rows
                                        var $selectedRows = $('.ResultadosExamesTable').jtable('selectedRows');
                                        if ($selectedRows.length > 0) {
                                            //Show selected rows
                                            $selectedRows.each(function () {
                                                var record = $(this).data('record');
                                                getDetalharResultadosExames(record.id);
                                            });
                                        }
                                    },
                                    recordsLoaded: function (event, data) {
                                        $(".ResultadosExamesTable .jtable-main-container tr.jtable-data-row:first input[type=checkbox]").trigger('click');
                                        
                                    }
                                });

                                $('.DetalharResultadosExameTable').jtable({
                                    title: app.localize('DetalharSolicitacaoExame'),
                                    paging: true,
                                    pageSize: 5,
                                    sorting: true,
                                    multiSorting: true,
                                    actions: {
                                        listAction: {
                                            method: abp.services.app.atendimento.getDetalhamentoExameItemResultado
                                        }
                                    },
                                    fields: detalharResultadosExameTableFields
                                });

                                $('.DetalharSolicitacoesExameTable').jtable({
                                    title: app.localize('DetalharSolicitacoesExame'),
                                    paging: true,
                                    pageSize: 5,
                                    sorting: true,
                                    multiSorting: true,
                                    actions: {
                                        listAction: {
                                            method: abp.services.app.atendimento.getDetalhamentoExameItemSolicitacao
                                        }
                                    },
                                    fields: detalharResultadosExameTableFields
                                });

                                $('.SolicitacoesExamesTable').jtable('load', { id: $param.atendimentoId, tipo: $param.tipo });
                                $('.ResultadosExamesTable').jtable('load', { id: $param.atendimentoId, tipo: $param.tipo });

                                function getDetalharResultadosExames(id) {
                                    $('.DetalharResultadosExameTable').jtable('load', {
                                        id, tipo: $param.tipo
                                    });
                                }

                                function getDetalharSolicitacoesExames(id) {
                                    $('.DetalharSolicitacoesExameTable').jtable('load', {
                                        id, tipo: $param.tipo
                                    });
                                }
                            }, 250);
                        }
                    },

                    functionAfter: function (instance, helper) {
                        $("#updateGrid").trigger("click");
                    }
                });


            }
        });


        function getParameterByName(name, url) {
            if (!url) url = window.location.href;
            url = url.toLowerCase(); // correcao em caso de case sensitive
            name = name.replace(/[\[\]]/g, "\\$&").toLowerCase();// correcao em caso de case sensitive
            var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
                results = regex.exec(url);
            if (!results) return null;
            if (!results[2]) return '';
            return decodeURIComponent(results[2].replace(/\+/g, " "));
        }



        function criarAba(record) {
            if (record !== null && record !== undefined) {


                localStorage["AtendimentoId"] = record.id;
                localStorage["DataAtendimento"] = moment(record.dataRegistro).format();
                //$('#abas-' + localStorage["TipoAtendimento"] + ' li').removeClass('active');
                //$('#conteudo-abas-' + localStorage["TipoAtendimento"] + ' div.tab-pane').removeClass('active');
                if ($('#atendimento-' + record.id).length === 0) {
                    var ans = record.codigoAtendimento !== null ? zeroEsquerda(record.codigoAtendimento, '10') : '';
                    //$('<li id="atendimento-' + record.id + '" name="Atendimento-' + record.id + '" class="active"><a id="link-atendimento-' + record.id + '" href="#atendimento-' + record.id + '" data-toggle="tab" title="' + record.paciente + '" onclick="lerAtendimentoAmbulatorioEmergencia(' + record.id + ',' + "'assistencial', '" + localStorage["TipoAtendimento"] + "');" + '">' + ans + ' - ' + record.paciente + ' <i class="fa fa-close"></i></a></li>')
                    $('<li id="atendimento-' + record.id + '" name="Atendimento-' + record.id + '"><a id="link-atendimento-' + record.id + '" href="#conteudo-atendimento-' + record.id + '" data-toggle="tab" title="' + record.paciente + '" onclick="atualizarAtendimento(' + "'" + record.id + "'" + ');">' + ans + ' - ' + record.paciente + '<i class="fa fa-close"></i></a></li>')
                        .appendTo('#abas-' + localStorage["TipoAtendimento"]);

                    $('<div class="tab-pane" id="conteudo-atendimento-' + record.id + '" style="padding:5px;">').appendTo('#conteudo-abas-' + localStorage["TipoAtendimento"]).load('/mpa/assistenciais/_leratendimento/' + record.id); //, function () {
                    //});
                    $('#link-atendimento-' + record.id).trigger('click');
                } else {
                    $('#link-atendimento-' + record.id).trigger('click');
                }
            }
        }


        function criarNewAba(id, dataRegistro, codigoAtendimento, paciente) {

            if (id !== null) {

                abp.event.on('loadedLerAtendimento', movimentoAtendimento);

                localStorage["AtendimentoId"] = id;
                localStorage["DataAtendimento"] = moment(dataRegistro).format();
                if ($('#atendimento-' + id).length === 0) {
                    var ans = codigoAtendimento !== null ? zeroEsquerda(codigoAtendimento, '10') : '';

                    var valor = '<li id="atendimento-' + id + '" name="Atendimento-' + id + '"><a id="link-atendimento-' + id + '" href="#conteudo-atendimento-' + id + '" data-toggle="tab" title="' + paciente + '" onclick="atualizarAtendimento(' + "'" + id + "'" + ');">' + ans + ' - ' + paciente + '<i class="fa"></i></a></li>';

                    $(valor)
                        .appendTo('#abas-' + localStorage["TipoAtendimento"]);

                    $('<div class="tab-pane" id="conteudo-atendimento-' + id + '" style="padding:5px;">').appendTo('#conteudo-abas-' + localStorage["TipoAtendimento"]).load('/mpa/assistenciais/_leratendimento/' + id); //, function () {
                    //});
                    $('#link-atendimento-' + id).trigger('click');
                } else {
                    $('#link-atendimento-' + id).trigger('click');
                }



            }
        }

        function criarAbaAte(record) {

            if (record !== null && record !== undefined) {


                localStorage["AtendimentoId"] = record.id;
                localStorage["DataAtendimento"] = moment(record.dataRegistro).format();
                //$('#abas-' + localStorage["TipoAtendimento"] + ' li').removeClass('active');
                //$('#conteudo-abas-' + localStorage["TipoAtendimento"] + ' div.tab-pane').removeClass('active');
                if ($('#atendimento-' + record.id).length === 0) {
                    var ans = record.codigo !== null ? zeroEsquerda(record.codigo, '10') : '';
                    //$('<li id="atendimento-' + record.id + '" name="Atendimento-' + record.id + '" class="active"><a id="link-atendimento-' + record.id + '" href="#atendimento-' + record.id + '" data-toggle="tab" title="' + record.paciente + '" onclick="lerAtendimentoAmbulatorioEmergencia(' + record.id + ',' + "'assistencial', '" + localStorage["TipoAtendimento"] + "');" + '">' + ans + ' - ' + record.paciente + ' <i class="fa fa-close"></i></a></li>')
                    $('<li id="atendimento-' + record.id + '" name="Atendimento-' + record.id + '"><a id="link-atendimento-' + record.id + '" href="#conteudo-atendimento-' + record.id + '" data-toggle="tab" title="' + record.paciente.nomeCompleto + '" onclick="atualizarAtendimento(' + "'" + record.id + "'" + ');">' + ans + ' - ' + record.paciente.nomeCompleto + '<i class="fa fa-close"></i></a></li>')
                        .appendTo('#abas-' + localStorage["TipoAtendimento"]);

                    $('<div class="tab-pane" id="conteudo-atendimento-' + record.id + '" style="padding:5px;">').appendTo('#conteudo-abas-' + localStorage["TipoAtendimento"]).load('/mpa/assistenciais/_leratendimento/' + record.id); //, function () {
                    //});
                    $('#link-atendimento-' + record.id).trigger('click');
                } else {
                    $('#link-atendimento-' + record.id).trigger('click');
                }
            }
        }

        function getAssistencialAtendimentos() {

            if (paramts == null) {
                _$AssistencialAtendimentosTable.jtable('load', createRequestParams());
            }
            else {

                $(document).attr("title", decodeHTML(sessionStorage["paciente"]) + " - Internação :: SWMANAGER");
                $("#link-principal-int").hide();
                $("#link-principal-amb").hide();
                criarNewAba(sessionStorage["id"], sessionStorage["dataRegistro"], sessionStorage["codigoAtendimento"], sessionStorage["paciente"]);
            }
        }

        function decodeHTML(html) {
            var txt = document.createElement('textarea');
            txt.innerHTML = html;
            return txt.value;
        };

        function getAtendimentos() {

            _$AssistencialAtendimentosTable.jtable('load', { Filtro: $('#paciente-id-' + localStorage["TipoAtendimento"]).val() });
        }

        function deleteAssistencialAtendimentos(assistencialAtendimento) {

            abp.message.confirm(
                app.localize('DeleteWarning', assistencialAtendimento.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _atendimentosService.excluir(assistencialAtendimento)
                            .done(function () {
                                getAssistencialAtendimentos();
                                abp.notify.success(app.localize('SuccessfullyDeleted'));
                            });
                    }
                }
            );
        }

        function createRequestParams() {
            var prms = {};
            _$filterForm.serializeArray().map(function (x) {
                if (!_.isUndefined(prms[x.name])) {
                    if (!_.isArray(prms[x.name])) {
                        prms[x.name] = [prms[x.name]];
                    }
                    prms[x.name].push(x.value);
                } else {
                    prms[x.name] = x.value;
                }
            });
            return $.extend(prms, _selectedDateRange);
        }

        $('#ShowAdvancedFiltersSpan').click(function () {
            $('#ShowAdvancedFiltersSpan').hide();
            $('#HideAdvancedFiltersSpan').show();
            $('#AdvacedAssistencialAtendimentosFiltersArea').slideDown();
        });

        $('#HideAdvancedFiltersSpan').click(function () {
            $('#HideAdvancedFiltersSpan').hide();
            $('#ShowAdvancedFiltersSpan').show();
            $('#AdvacedAssistencialAtendimentosFiltersArea').slideUp();
        });

        $('#CreateNewAssistencialAtendimentoButton-' + localStorage["TipoAtendimento"]).click(function () {

            _createOrEditModal.open();
        });

        $('#ExportarAssistencialAtendimentosParaExcelButton').click(function () {
            _atendimentosService
                .listarParaExcel({
                    filtro: $('#AssistencialAtendimentosTableFilter-' + localStorage["TipoAtendimento"]).val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });


        $("#AbrirDiagnostico").click(function () {
            _pacienteDiagnosticos.open({ id: $(this).data("id") });
        });

        $("#AbrirAlergias").click(function () {
            _pacienteAlergias.open({ id: $(this).data("id") });
        });

        $("#AbrirPesoAltura").click(function () {
            _pacientePesoAltura.open({ id: $(this).data("id") });
        });

        $("#AbrirModelos").click(function (e) {
            debugger;
            _modeloPrescricao.open({ id: $(this).data("id") });
        });



        $('#tipo-filtro-data-' + localStorage["TipoAtendimento"]).on('change', function (e) {
            e.preventDefault();

            $('#filtro-data-' + localStorage["TipoAtendimento"]).val($(this).val())
            switch ($(this).val()) {
                case "Atendimento":
                    $('#date-field-area-' + localStorage["TipoAtendimento"]).removeClass('hidden');
                    break;
                case "Internado":
                    $('#date-field-area-' + localStorage["TipoAtendimento"]).addClass('hidden');
                    break;
                default:
                    $('#date-field-area-' + localStorage["TipoAtendimento"]).removeClass('hidden');
            }
        });

        $('#GetAssistencialAtendimentosButton-' + localStorage["TipoAtendimento"]).click(function (e) {
            e.preventDefault();

            getAssistencialAtendimentos();
        });

        $('#RefreshAssistencialAtendimentosButton-' + localStorage["TipoAtendimento"]).click(function (e) {
            e.preventDefault();

            getAssistencialAtendimentos();
        });

        abp.event.on('app.CriarOuEditarAssistencialAtendimentoModalSaved', function () {

            getAssistencialAtendimentos();
        });

        abp.event.on('app.NovoTevMovimentoModalSaved', function () {

            getAssistencialAtendimentos();
        });

        getAssistencialAtendimentos();

        $('#AssistencialAtendimentosTableFilter-' + localStorage["TipoAtendimento"]).focus();

        $('body').addClass('page-sidebar-closed');

        $('.page-sidebar-menu').addClass('page-sidebar-menu-closed');

        if (localStorage["TipoAtendimento"] === "int") {

            $('#tipo-filtro-data-' + localStorage["TipoAtendimento"]).val("Internado").change();
        }

        aplicarSelect2Padrao();

        //$("#AtendimentoStatus-id").val([1, 2, 3]).trigger('change');


        $('.select2').each(function () {
            $(this).on("change",
                function (e) {
                    getAssistencialAtendimentos();
                });
        });

        selectSW('.selectTipoLocalChamada', "/api/services/app/TipoLocalChamada/ListarTipoLocalChamadaDropdown");
        selectSW('.selectLocalChamada', "/api/services/app/LocalChamadas/ListarLocalChamadaPorTipoDropdown", $('#tipo-local-chamada-id'));
        selectSW('.selectSenha', "/api/services/app/Senha/ListarSenhasNaoChamadas");

        $('#tipo-local-chamada-id').on('change', function (e) {
            e.preventDefault();
            $('#local-chamada-id').val('').trigger('change');
            selectSW('.selectLocalChamada', "/api/services/app/LocalChamadas/ListarLocalChamadaPorTipoDropdown", $('#tipo-local-chamada-id'));
            $.cookie('tipoLocalChamada', $(this).val());
        });

        $('#local-chamada-id').on('change', function (e) {
            e.preventDefault();
            selectSW('.selectSenha', "/api/services/app/Senha/ListarSenhasPorlocalChamadaDropdown", $('#local-chamada-id'));
            $('#movimentacao-senha-id').val('').trigger('change');
            $.cookie('localChamada', $('#local-chamada-id').val());
        });

        if ($.cookie('tipoLocalChamada') !== undefined) {
            $("#tipo-local-chamada-id").val($.cookie('tipoLocalChamada')).trigger('change');
        }

        if ($.cookie('localChamada') !== undefined) {
            $("#local-chamada-id").val($.cookie('localChamada')).trigger('change');
        }

        //$('#movimentacao-senha-id').on('change', function (e) {
        //    //e.preventDefault();
        //    //if ($(this).val() !== null && $(this).val() !== '' && $(this).val() !== undefined) {
        //    //    _senhasService.obterMovimento($(this).val())
        //    //        .done(function (data) {
        //    //            _senhasService.obter(data.senhaId)
        //    //                .done(function (senha) {
        //    //                    if (senha && senha.atendimentoId && senha.atendimentoId != 0) {
        //    //                        _atendimentosService.obter(senha.atendimentoId)
        //    //                            .done(function (atendimento) {
        //    //                                criarAbaAte(atendimento);
        //    //                            });
        //    //                    }
        //    //                });
        //    //        });
        //    //}
        //});

        function chamarPaciente(senha) {
            if (!senha) {
                senha = $('#movimentacao-senha-id').val();
            }
            if (!$('#tipo-local-chamada-id').val() ||
                !$('#local-chamada-id').val() ||
                !senha) {
                abp.notify.info(app.localize('Informações para chamar a senha não preenchidas!'));
                return;
            }

            _terminalSenhasService.chamarSenha($('#tipo-local-chamada-id').val(), $('#local-chamada-id').val(), senha).done(function () {
                abp.notify.info("Senha chamada com sucesso.");
            });
            $.cookie('localChamada', $('#local-chamada-id').val());
        }

        $('#senha-btn').on('click', function (e) {
            e.preventDefault();
            chamarPaciente();
            var movSenhaId = $("#movimentacao-senha-id").val();
            if (movSenhaId !== null && movSenhaId !== '' && movSenhaId !== undefined) {
                _senhasService.obterMovimento(movSenhaId)
                    .done(function (data) {
                        _senhasService.obter(data.senhaId)
                            .done(function (senha) {
                                if (senha && senha.atendimentoId && senha.atendimentoId != 0) {
                                    window.open(window.location.href + "?id=" + senha.atendimentoId);
                                }
                            });
                    });
            }
        });

        $('.select2').css('width', '100%');


        $("#updateGrid").on("click",
            function (e) {
                var updateGrid = $("#updateGrid");
                if (updateGrid.data("status") == "play") {
                    updateGrid.data("status", "stop");
                    updateGrid.find(".playTimer").removeClass("hidden");
                    updateGrid.find(".stopTimer").addClass("hidden");
                    clearInterval(updateGrid.data("interval-id"));
                    updateGrid.data("interval-id", null);
                } else if (updateGrid.data("status") == "stop") {
                    updateGrid.data("status", "play");
                    atualizaGrid();
                }
            });

        function movimentoAtendimento() {

            var atendimentoId = getParameterByName('id');
            const atendimentoMovimentoAppService = abp.services.app.atendimentoMovimento;
            if (atendimentoId) {
                atendimentoMovimentoAppService
                    .obter(atendimentoId).then(res => {
                        let text = "Deseja iniciar o atendimento?";
                        if (res.assumirAtendimento) {
                            text = "Deseja assumir e iniciar o atendimento?";
                        }
                        if (res.iniciarAtendimento || res.assumirAtendimento) {
                            swal({
                                title: "",
                                text: text,
                                type: "warning",
                                showCancelButton: true,
                                closeOnConfirm: true,
                                showLoaderOnConfirm: true,
                                cancelButtonText: app.localize("No"),
                                cancelButtonColor: "#DD6B55",
                                confirmButtonColor: "#3598dc",
                                confirmButtonText: app.localize("Yes")
                            }, function (isConfirmed) {
                                if (isConfirmed) {
                                    atendimentoMovimentoAppService.assumirAtendimento(atendimentoId).then(resData => {
                                        atualizarTempo(resData.dataInicio);

                                        $(".tempoChamada").removeClass("hide");
                                        $(".finalizarAtendimento").removeClass("hide").on('click', finalizarAtendimento);
                                        abp.event.on('app.CriarOuEditarAltaModalSaved', finalizarAtendimentoMovimento);
                                        abp.event.on('app.PendenciaModalViewModelSaved', finalizarAtendimentoMovimento);

                                    });
                                }
                            });
                        }
                        else if (res.dataInicio) {
                            atualizarTempo(res.dataInicio);
                            $(".tempoChamada").removeClass("hide");
                            $(".finalizarAtendimento").removeClass("hide").on('click', finalizarAtendimento);
                            abp.event.on('app.CriarOuEditarAltaModalSaved', finalizarAtendimentoMovimento);
                            abp.event.on('app.PendenciaModalViewModelSaved', finalizarAtendimentoMovimento);
                        }
                    });


            }

            function finalizarAtendimento(e) {
                e.stopPropagation();
                return swal({
                    title: "",
                    text: "Deseja dar alta ou por em pendencia?",
                    type: "warning",
                    showCancelButton: true,
                    closeOnConfirm: true,
                    showLoaderOnConfirm: true,
                    cancelButtonText: app.localize("Pendencia"),
                    cancelButtonColor: "#DD6B55",
                    confirmButtonColor: "#3598dc",
                    confirmButtonText: app.localize("Alta")
                }, function (isConfirmed) {
                    if (isConfirmed) {
                        _altaFinalizarAtendimento.open({ atendimentoId, isConsulta: true });
                        return;

                    }
                    _pendenciaFinalizarAtendimento.open({ atendimentoId });
                    return;
                });
            }

            let intervalAtualizarTempo = null;
            function atualizarTempo(date) {
                console.log(date);
                var updateSpan = $(".tempoChamada");
                updateSpan.data("date", date);
                atualizar();
                intervalAtualizarTempo = setInterval(() => {
                    atualizar();
                }, 1000);
                function atualizar() {
                    var dataInicial = moment(updateSpan.data("date"));
                    var dataAtual = moment();
                    var diff = dataAtual.diff(dataInicial);
                    var duration = moment.duration(diff);
                    var tempo = '';


                    if (duration.get('years') > 0) {
                        tempo += `<span style="margin-right:2px"> ${formataNumero(duration.get('years'))} ${(duration.get('years') == 1 ? 'ano' : 'anos')} </span>`;
                    }

                    if (duration.get('months') > 0) {
                        tempo += `<span style="margin-right:2px"> ${formataNumero(duration.get('months'))} ${(duration.get('months') == 1 ? 'mês' : 'meses')} </span>`;
                    }

                    if (duration.get('days') > 0) {
                        tempo += `<span style="margin-right:2px"> ${formataNumero(duration.get('days'))} ${(duration.get('days') == 1 ? 'dia' : 'dias')} </span>`;
                    }
                    tempo += `${formataNumero(duration.get('hours'))}:${formataNumero(duration.get('minutes'))}:${formataNumero(duration.get('seconds'))}`;
                    updateSpan.find(".tempo").html(`<i class="fa fa-clock" style="margin-right:2px"></i><span>${tempo}</span>`);
                }

                function formataNumero(numero) {
                    if (numero < 10) {
                        return '0' + numero;
                    }
                    return numero;
                }
            }


            function finalizarAtendimentoMovimento() {
                atendimentoMovimentoAppService.finalizarAtendimento(atendimentoId).then(resData => {
                    debugger;
                    $(".tempoChamada").addClass("hide");
                    $(".finalizarAtendimento").addClass("hide").off('click', finalizarAtendimento);
                    abp.event.off('app.CriarOuEditarAltaModalSaved', finalizarAtendimentoMovimento);

                    if (intervalAtualizarTempo) {
                        clearInterval(intervalAtualizarTempo);
                    }
                    setTimeout(() => {
                        window.close();
                    }, 0);
                });
            }
        }

        function atualizaGrid() {
            var updateGrid = $("#updateGrid");
            if (paramts === null || paramts === undefined) {
                updateGrid.show();
                if (updateGrid.length) {
                    if (!updateGrid.data("time")) {
                        updateGrid.data("time", 60);
                    }
                    updateGrid.find(".stopTimer").removeClass("hidden");
                    updateGrid.find(".playTimer").addClass("hidden");

                    if (!updateGrid.data("status")) {
                        updateGrid.data("status", "play");
                    }
                    if (!updateGrid.data("interval-id")) {
                        updateGrid.data("interval-id", setInterval(doTimer, 1000))
                    }
                }
            }
            else {
                updateGrid.hide();
            }

            function doTimer() {
                var time = parseInt(updateGrid.data("time"), 10);
                time -= 1;
                updateGrid.data("time", time);

                if (time === 0) {
                    getAssistencialAtendimentos();
                    updateGrid.data("time", 60);
                    doTimer();
                    return;
                }

                updateGrid.find(".timerContent").html(`<i class="fa fa-clock"></i><span>${time} segundos para atualizar</span>`);

            }
        }

    });
})();