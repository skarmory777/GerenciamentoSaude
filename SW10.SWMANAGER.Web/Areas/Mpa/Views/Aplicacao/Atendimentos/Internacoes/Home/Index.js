(function () {
    $(function () {

        $(document).ready(function () {

            if ($('#agendamentoId').val() != null && $('#agendamentoId').val() != '') {

                window.atendimentoCounter++;
                criarAba(0, window.atendimentoCounter, $('#agendamentoId').val());
                return;

            }

        });

        AteSelecionado = [];

        var _relatorioSoliInternacaoModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/FatGuiasSolicitacaoInternacao/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Faturamentos/SolicitacaoInternacao/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarModal'
        });

        var _relatorioResumoInternacaoModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/FatGuiasResumoInternacao/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Faturamentos/ResumoInternacao/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarModal'
        });

        var _relatorioConsultaModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/FatGuiasConsulta/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Faturamentos/Consulta/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarModal'
        });

        //================chama o grid de pre-atendimento=======================
        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Atendimentos/_SelecionarreAtendimentoModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Atendimentos/Internacoes/Home/_SelecionarPreAtendimentoModal.js',
            modalClass: 'SelecionarPreAtendimentoModal'
        });

        var _modalAlta = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/AtendimentoLeitoMov/_AltaModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Atendimentos/Altas/Alta/_CriarOuEditarModal.js',
            modalClass: 'AltaModalViewModel'
        });

        var _permissionsPreAtendimento = {
            create: abp.auth.hasPermission('Pages.Tenant.Atendimento.PreAtendimento.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Atendimento.PreAtendimento.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Atendimento.PreAtendimento.Delete')
        };

        var _userPermissionsModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Users/PermissionsModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Atendimentos/PreAtendimentos/_PermissionsModal.js',
            modalClass: 'UserPermissionsModal'
        });


        var _cancelamentoAtendimentoModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Atendimentos/CancelarAtendimentoModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Atendimentos/Internacoes/Home/Cancelamento/CancelarAtendimentoModal.js',
            modalClass: 'CancelarAtendimentoModal'
        });

        var _reativarAtendimentoModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Atendimentos/ReativarAtendimentoModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Atendimentos/Internacoes/Home/Cancelamento/ReativarAtendimentoModal.js',
            modalClass: 'ReativarAtendimentoModal'
        });



        //=================novo================================================

        $('#CreateNewPreAtendimentoButton').click(function (e) {
            e.preventDefault();
            _createOrEditModal.open();

            $('.modal-dialog').css({ 'width': '100%', 'max-width': '900px' });
            $('.modal-content').css({ 'width': '100%', 'max-width': '900px' });
        });

        $('#CreateNewRelatorioButton').click(function (e) {
            e.preventDefault();
            //window.location.href = "/Mpa/AtendimentoRelatorio/Index";
            window.open("/Mpa/AtendimentoRelatorio/Index");
        });

        //==============CARREGADOS JUNTO A PÁGINA================
        $('#AtendimentosTableFilter').focus();
        var _$AtendimentosTable = $('#AtendimentosTable');
        var _AtendimentosService = abp.services.app.atendimento;
        var _$filterForm = $('#AtendimentosFilterForm');

        var _permissionsInternacao = {
            create: abp.auth.hasPermission('Pages.Tenant.Atendimento.Internacao.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Atendimento.Internacao.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Atendimento.Internacao.Delete'),
            'alta': abp.auth.hasPermission('Pages.Tenant.Atendimento.Internacao.Alta')
        };
        var _userPermissionsModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Users/PermissionsModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Atendimentos/Atendimentos/_PermissionsModal.js',
            modalClass: 'UserPermissionsModal'
        });

        //Retrair menu principal
        $('body').addClass('page-sidebar-closed');
        $('#menu-lateral').addClass('page-sidebar-menu-closed');

        var _$filterForm = $('#AtendimentosFilterForm');

        //Date Range Picker
        var _selectedDateRange = {
            startDate: moment().startOf('day'),
            endDate: moment().endOf('day')
        };

        _$filterForm.find('.date-range-picker').daterangepicker(
            $.extend(true, app.createDateRangePickerOptions(), _selectedDateRange),
            function (start, end, label) {
                debugger;
                _selectedDateRange.startDate = start.format('YYYY-MM-DDT00:00:00Z');
                _selectedDateRange.endDate = end.add(1, 'days').format('YYYY-MM-DDT23:59:59.999Z');
            });

        // $("#divDataRange").hide("slow");
        //===============FIM CARREGADOS JUNTO A PÁGINA===================


        //=====================NOVO ATENDIMENTO==========================
        // Novo Atendimento
        var abaNovoAtendimento = $('#aba-novo-atendimento');
        window.atendimentoCounter = 0;

        $('.novo-atendimento').click(function (e) {
            e.preventDefault();
            window.atendimentoCounter++;
            criarAba(0, window.atendimentoCounter);
            return;
        });

        function criarAba(atendimento, atendimentoCounter, agendamentoId) {
            var abas = $('#abas');
            var conteudoAbas = $('#conteudo-abas');
            var divNovo = $('<div class="tab-pane aba-comutavel conteudo" id="novo-atendimento-' + atendimentoCounter + '"><div></div></div>');

            var infoExtraAba;
            if (atendimento == 0) {
                infoExtraAba = moment().format('DD-MM HH:mm');
            }
            else {

                if (typeof atendimento.paciente == 'string') {
                    infoExtraAba = atendimento.paciente
                } else {
                    infoExtraAba = atendimento.paciente && atendimento.paciente.nomeCompleto || atendimento.paciente.sisPessoa.nomeCompleto;
                }



                //if (atendimento.paciente) {
                //    infoExtraAba = atendimento.paciente.nomeCompleto
                //    if (atendimento.paciente.sisPessoa) {
                //        infoExtraAba = atendimento.paciente.sisPessoa.nomeCompleto
                //    }
                //} else {
                //    infoExtraAba = '...';
                //}
            }

            var abaAtendimento = $('<li class="borda-aba" onclick="lerPartial(' + atendimentoCounter + ')" id="li-atendimento-' + atendimentoCounter + '"><a href="#novo-atendimento-' + atendimentoCounter + '"class="link-atendimento"  id="aba-atendimento-' + atendimentoCounter + '" data-toggle="tab" aria-expanded="false">Atendimento <i id="icone-aba-' + atendimentoCounter + '" class=""></i><i class="fa fa-close" id="aba-' + atendimentoCounter + '"  onclick="fecharAba(' + atendimentoCounter + ')" style="float:right;"></i><br/><span id="nome-paciente">' + infoExtraAba + '</span></a></li>');

            divNovo.appendTo(conteudoAbas);
            abaAtendimento.appendTo(abas);
            abaNovoAtendimento.insertAfter(abaAtendimento);
            lerPartial(atendimento, atendimentoCounter, agendamentoId);
        }

        function lerPartial(atendimento, atendimentoCounter, agendamentoId) {

            var atendimentoId;

            if (atendimento) {
                atendimentoId = atendimento.id;
            }
            else {
                atendimentoId = 0;
            }

            var strId = '#novo-atendimento-' + atendimentoCounter;
            $('.aba-comutavel').removeClass('active');
            $('.borda-aba').addClass('obscurecido');
            $('.link-atendimento').attr("aria-expanded", false);

            var agend = '';
            if (agendamentoId != null && agendamentoId != 0) {
                agend = '&agendamentoId=' + agendamentoId
            }


            var metodoNovoAtendimento = '/Mpa/Atendimentos/Index?id=' + atendimentoId + '&abaId=' + atendimentoCounter + '&internacao=true' + agend;

            $(strId).load(metodoNovoAtendimento, function (data) {
                var abaAtendimento = '#aba-atendimento-' + atendimentoCounter;
                $(abaAtendimento).attr("aria-expanded", true);
                $(this).addClass("active").removeClass('obscurecido');
                $(abaAtendimento).parent().addClass('active').removeClass('obscurecido');
            });
        }


        //==============EDITAR ATENDIMENTO 11/07/2017=================
        window.edicao = [];
        var selecionado;
        function editarAtendimento(atendimento) {

            //PROCURA NO ARRAY 'edicao' SE EXISTE O ID SELECIONANDO PARA A EDIÇÃO
            //EXINTINDO, ATRIBUI PARA O NOVO ARRAY 'edicao'
            selecionado = edicao.filter(function (elemento) {
                return elemento.id == atendimento.id;
            });
            //CASA O ID SELECIONADO Ñ FOR ENCONTRADO NO ARRAY ABRE-SE UMA NOVA ABA
            if (selecionado == "") {
                window.atendimentoCounter++;
                criarAba(atendimento, window.atendimentoCounter);

                //INSERI AS INFORMAÇÕES DO ELEMENTO SELECIONADO, PARA CONSULTAR POSTERIORMENTE
                edicao[atendimentoCounter] = { id: atendimento.id, aba: atendimentoCounter, nome: atendimento.paciente.nomeCompleto }

            } else {
                //CASA O ID SELECIONADO FOR ENCONTRADO ELE RECUPERA AS INFORMAÇÕES DA ABA ABERTA QUE ESTA NO ARRAY 'selecionado'
                atendimento.id = selecionado[0].id;
                atendimento.paciente.nomeCompleto = selecionado[0].nome;
                atendimentoCounter = selecionado[0].aba;

                lerPartial(atendimento, atendimentoCounter);
            }
        }

        //====================EXCLUIR ATENDIMENTO========================
        function deleteAtendimentos(Atendimento) {
            //abp.message.confirm(
            //    app.localize('DeleteWarning', Atendimento.descricao),
            //    function (isConfirmed) {
            //        if (isConfirmed) {

            //           
            _cancelamentoAtendimentoModal.open({ id: Atendimento.id });

            //_AtendimentosService.excluir(Atendimento.id)
            //    .done(function () {
            //        getAtendimentos();
            //        abp.notify.success(app.localize('SuccessfullyDeleted'));
            //    });
            //        }
            //    }
            //);
        }

        function reativarAtendimentos(Atendimento) {

            _reativarAtendimentoModal.open({ id: Atendimento.id });

        }


        function cancelarAlta(Atendimento) {
            abp.message.confirm(
                app.localize('CancelaAltaWarning', Atendimento.paciente),
                function (isConfirmed) {
                    if (isConfirmed) {

                        debugger;

                        _AtendimentosService.cancelarAlta(Atendimento.id)
                            .done(function () {

                                abp.notify.success(app.localize('SuccessfullyDeleted'));
                            });
                    }
                }
            );
        }



        //=================BUSCAR ATENDIMENTOS===========================
        //metodo chamado pela IndexPartial p/ carrega o grid (pablo)
        window.carregaAtendimento = function () {
            getAtendimentos()
        }

        window.editarPreAtendimento = function (preAtendimento) {
            editarAtendimento(preAtendimento)
        }

        function getAtendimentos() {
            _$AtendimentosTable.jtable('load', createRequestParams());
        }

        function createRequestParams() {
            var prms = {};
            _$filterForm.serializeArray().map(function (x) { prms[x.name] = x.value; });
            return $.extend(prms, _selectedDateRange);
        }


        //==============================JTABLE==============================

        // JTABLE
        _$AtendimentosTable.jtable({
            title: app.localize('Internacoes'),
            paging: true,
            sorting: true,
            multiSorting: true,
            selecting: true,
            selectingCheckboxes: true,

            rowInserted: function (event, data) {
                if (data) {

                    if (data.record.corStatusAutorizacao) {
                        data.row[0].cells[15].setAttribute('bgcolor', data.record.corStatusAutorizacao);
                    }
                }

            },



            actions: {
                listAction: {
                    method: abp.services.app.atendimento.listarFiltroInternacao
                }
            },
            fields: {
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

                        if (_permissionsInternacao.edit && data.record.atendimentoMotivoCancelamentoId == null) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                .appendTo($span)
                                .click(function (e) {
                                    e.preventDefault();
                                    editarAtendimento(data.record);
                                });

                        }
                        if (_permissionsInternacao.delete && data.record.atendimentoMotivoCancelamentoId == null) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                .appendTo($span)
                                .click(function (e) {
                                    e.preventDefault();
                                    deleteAtendimentos(data.record);
                                });
                        }
                        if (_permissionsInternacao.alta && (!data.record.dataAlta || data.record.dataAlta == null || data.record.dataAlta == undefined || data.record.dataAlta == '') && data.record.atendimentoMotivoCancelamentoId == null) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Alta') + '"><i class="fa fa-blind fa-3"></i></button>')
                                .appendTo($span)
                                .click(function (e) {
                                    e.preventDefault();
                                    _modalAlta.open({ atendimentoId: data.record.id });
                                });
                        }

                        if (data.record.atendimentoMotivoCancelamentoId != null) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Reativar') + '"><i class="fa fa-recycle"></i></button>')
                                .appendTo($span)
                                .click(function (e) {
                                    e.preventDefault();
                                    reativarAtendimentos(data.record);
                                });
                        }


                        if (data.record.dataAlta != null) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Alterar Alta') + '"><i class="fa fa-blind fa-3"></i></button>')
                                .appendTo($span)
                                .click(function (e) {
                                    e.preventDefault();
                                    _modalAlta.open({ atendimentoId: data.record.id });
                                });

                            $('<button class="btn btn-default btn-xs" title="' + app.localize('CancelarAlta') + '"><i class="fa fa-blind"><i class="fa fa-recycle "></i></button>')
                                .appendTo($span)
                                .click(function (e) {
                                    e.preventDefault();
                                    cancelarAlta(data.record);
                                });
                        }

                        return $span;
                    }
                },
                "unidadeOrganizacional.Descricao": {
                    title: app.localize('Unidade'),
                    width: '5%',
                    sorting: false,
                    display: function (data) {
                        if (data.record.unidade) {
                            return data.record.unidade;
                            //return data.record.unidadeOrganizacional.descricao;
                        }
                    }
                },
                "ateAtendimento.Codigo": {
                    title: app.localize('Codigo'),
                    width: '5%',
                    display: function (data) {
                        return zeroEsquerda(data.record.codigoAtendimento, '10');
                    }
                },
                "PessoaPaciente.Codigo": {
                    title: app.localize('CodigoPaciente'),
                    width: '5%',
                    sorting: false,
                    display: function (data) {
                        var codpac = '';
                        if (data.record.codigoPaciente != null &&
                            data.record.codigoPaciente != '' &&
                            data.record.codigoPaciente != undefined) {
                            codpac = zeroEsquerda(data.record.codigoPaciente, '10');
                        }
                        return codpac;
                    }

                    //if (data.record.paciente) {
                    //    return data.record.codigoPaciente;
                    //}
                },
                "PessoaPaciente.NomeCompleto": {
                    title: app.localize('Paciente'),
                    width: '15%',
                    display: function (data) {
                        if (data.record.paciente) {
                            return data.record.paciente;
                        }
                    }
                },
                "ateAtendimento.DataRegistro": {
                    title: app.localize('Internacao'),
                    width: '5%',
                    display: function (data) {
                        return moment(data.record.dataRegistro).format("DD/MM/YYYY HH:mm");
                    }
                },
                "ateAtendimento.DataAlta": {
                    title: app.localize('Alta'),
                    width: '5%',
                    display: function (data) {
                        if (data.record.dataAlta) {
                            return moment(data.record.dataAlta).format("DD/MM/YYYY HH:mm"); //.format("DD/MM/YYYY HH:mm");
                        }
                    }
                },
                "SisPessoaConvenio.NomeFantasia": {
                    title: app.localize('Convenio'),
                    width: '5%'
                    ,
                    display: function (data) {
                        if (data.record.convenio) {
                            return data.record.convenio;
                        }
                    }
                },
                "PessoaMedico.NomeCompleto": {
                    title: app.localize('Medico'),
                    width: '7%',
                    display: function (data) {
                        if (data.record.medico) {
                            return data.record.medico;
                        }
                    }
                },
                "SisTipoAcomodacao.Descricao": {
                    title: app.localize('tipoLeito'),
                    width: '7%',
                    display: function (data) {
                        if (data.record.tipoLeito) {
                            return data.record.tipoLeito;
                        }
                    }
                },
                "AteLeito.Descricao": {
                    title: app.localize('leito'),
                    width: '5%',
                    display: function (data) {
                        if (data.record.leito) {
                            return data.record.leito;
                        }
                    }
                },
                "SisEmpresa.NomeFantasia": {
                    title: app.localize('Empresa'),
                    width: '10%',
                    display: function (data) {
                        if (data.record.empresa) {
                            return data.record.empresa;
                        }
                    }
                },
                "ateAtendimento.Matricula": {
                    title: app.localize('Matricula'),
                    width: '5%'
                    ,
                    display: function (data) {
                        if (data.record.matricula) {
                            return data.record.matricula;
                        }
                    }
                },
                "AteSenha.Numero": {
                    title: app.localize('Senha'),
                    width: '5%'
                    ,
                    display: function (data) {
                        if (data.record.senha) {
                            return data.record.senha;
                        }
                    }
                },
                "CAST(DATEADD(DAY,QuantidadeAutorizada,ateAtendimento.DataRegistro) AS DATE)": {
                    title: app.localize('Autorizacao'),
                    width: '5%',
                    display: function (data) {
                        if (data.record.dataAutorizada) {
                            return moment(data.record.dataAutorizada).format("L"); //.format("DD/MM/YYYY HH:mm");
                        }
                    }
                },



                //,
                //isControlaTev: {
                //    title: app.localize('Tev'),
                //    width: '5%',
                //    display: function (data) {
                //        if (data.record.isControlaTev) {
                //            return '<span class="label label-danger">' + app.localize('Yes') + '</span>';
                //        } else {
                //            return '<span class="label label-success">' + app.localize('No') + '</span>';
                //        }
                //    }
                //},
            },
            selectionChanged: function () {
                //Get all selected rows
                var $selectedRows = $('#AtendimentosTable').jtable('selectedRows');
                if ($selectedRows.length > 0) {

                    $('#iconesAte').show('slow');

                    //Show selected rows
                    var list = [];
                    var i = 0;
                    $selectedRows.each(function () {
                        var record = $(this).data('record');
                        $('#atendimento-selecionado').val(record.id);

                        //PABLO 05/04/18
                        //ACHO QUE Ñ VAI PRECISAR POIS ESTOU PEGANDO OS DADOS DA LINHA SELECIONADA SEM PRECISAR IR NO SERVIÇO
                        //CASO ACHE NECESSARIO EU SÓ COMENTEI 
                        //$.ajax({
                        //    url: '/mpa/atendimentorelatorio/AtendimentoTempData/' + record.id,
                        //    method: 'post',
                        //    success: function () {
                        //    }
                        //});

                        list[i] = record;
                        i++;
                    })
                    //AteSelecionado = [];

                    AteSelecionado = list;
                } else {
                    $('#iconesAte').hide('slow');
                }
            }
        });

        //$('#fichaAte').click(function (e) {
        //    e.preventDefault();
        //    //RECUPERA O ATENDIMENTO SELECIONADO
        //    var Atendimento = AteSelecionado;
        //    //ATRIBUI PRA "id" E MANDA COMO PARAMENTRO PARA O CONTROLLER
        //    var id = Atendimento[0].id;
        //    _relatorioConsultaModal.open({ id: id });

        //});

        $('#impriGuiaConvenio').click(function (e) {

            e.preventDefault();
            //RECUPERA O ATENDIMENTO SELECIONADO
            var Atendimento = AteSelecionado;
            //ATRIBUI PRA "id" E MANDA COMO PARAMENTRO PARA O CONTROLLER
            var id = Atendimento[0].id;
            _relatorioResumoInternacaoModal.open({ id: id });

        });
        $('#impriGuiaSolicInternacao').click(function (e) {
            e.preventDefault();
            //RECUPERA O ATENDIMENTO SELECIONADO
            var Atendimento = AteSelecionado;
            //ATRIBUI PRA "id" E MANDA COMO PARAMENTRO PARA O CONTROLLER
            var id = Atendimento[0].id;

            _relatorioSoliInternacaoModal.open({ id: id });

        });

        $('#impriConta').click(function (e) {

            e.preventDefault();
            var controller = 'Pacientes';
            var _viewUrl = abp.appPath + 'Mpa/' + controller + '/CriarOuEditarModal';
            var _scriptUrl = abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/' + controller + '/_CriarOuEditarModal.js';
            var _modalClass = 'CriarOuEditarPacienteModal';
            var _CriarOuEditarPacienteModal = new app.ModalManager({
                viewUrl: _viewUrl,
                scriptUrl: _scriptUrl,
                modalClass: _modalClass
            });
            _CriarOuEditarPacienteModal.open();

        });
        //$('#pulseira').click(function (e) {

        //    e.preventDefault();
        //    var controller = 'Pacientes';
        //    var _viewUrl = abp.appPath + 'Mpa/' + controller + '/CriarOuEditarModal';
        //    var _scriptUrl = abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/' + controller + '/_CriarOuEditarModal.js';
        //    var _modalClass = 'CriarOuEditarPacienteModal';
        //    var _CriarOuEditarPacienteModal = new app.ModalManager({
        //        viewUrl: _viewUrl,
        //        scriptUrl: _scriptUrl,
        //        modalClass: _modalClass
        //    });
        //    _CriarOuEditarPacienteModal.open();

        //});
        //$('#impriEtiqueta').click(function (e) {
        //    e.preventDefault();
        //    var controller = 'AtendimentoRelatorio';
        //    //var _viewUrl = abp.appPath + 'Mpa/' + controller + '/IndexAtendimentoEtiqueta';
        //    //var _scriptUrl = abp.appPath + 'Areas/Mpa/Views/Aplicacao/Atendimentos/Relatorios/AtendimentoEtiqueta.js';
        //    //var _modalClass = 'AtendimentoEtiquetaModal';
        //    //var _CriarOuEditarPacienteModal = new app.ModalManager({
        //    //    viewUrl: _viewUrl,
        //    //    scriptUrl: _scriptUrl,
        //    //    modalClass: _modalClass
        //    //});
        //    //_CriarOuEditarPacienteModal.open();
        //    //$('#relat').show();
        //    debugger
        //    var caminho = abp.appPath + 'Mpa/' + controller + '/IndexAtendimentoEtiqueta';  // "@Url.Action("IndexAtendimentoEtiqueta", "AtendimentoRelatorio", new { atendimentoId = atendimentoId })";

        //    PDFObject.embed(caminho, "#atendimento-etiqueta");
        //    $("#atendimento-etiqueta").show();


        //});
        $('#laudos').click(function (e) {

            e.preventDefault();
            var controller = 'Pacientes';
            var _viewUrl = abp.appPath + 'Mpa/' + controller + '/CriarOuEditarModal';
            var _scriptUrl = abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/' + controller + '/_CriarOuEditarModal.js';
            var _modalClass = 'CriarOuEditarPacienteModal';
            var _CriarOuEditarPacienteModal = new app.ModalManager({
                viewUrl: _viewUrl,
                scriptUrl: _scriptUrl,
                modalClass: _modalClass
            });
            _CriarOuEditarPacienteModal.open();

        });
        $('#autorizaPacianete').click(function (e) {
            debugger;
            e.preventDefault();
            var controller = 'Pacientes';
            var _viewUrl = abp.appPath + 'Mpa/' + controller + '/CriarOuEditarModal';
            var _scriptUrl = abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/' + controller + '/_CriarOuEditarModal.js';
            var _modalClass = 'CriarOuEditarPacienteModal';
            var _CriarOuEditarPacienteModal = new app.ModalManager({
                viewUrl: _viewUrl,
                scriptUrl: _scriptUrl,
                modalClass: _modalClass
            });
            _CriarOuEditarPacienteModal.open();

        });
        $('#obsPaciente').click(function (e) {

            e.preventDefault();
            var controller = 'Pacientes';
            var _viewUrl = abp.appPath + 'Mpa/' + controller + '/CriarOuEditarModal';
            var _scriptUrl = abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/' + controller + '/_CriarOuEditarModal.js';
            var _modalClass = 'CriarOuEditarPacienteModal';
            var _CriarOuEditarPacienteModal = new app.ModalManager({
                viewUrl: _viewUrl,
                scriptUrl: _scriptUrl,
                modalClass: _modalClass
            });
            _CriarOuEditarPacienteModal.open();

        });

        //==========CHAMADOS AO CARREGAR PÁGINA============================
        //chamada do grid
        getAtendimentos();


        //=====================CHAMADAS JQUERY===============================
        $('.link-atendimento').on("click", function () {
            $('.borda-aba').addClass('obscurecido');
            // $('.aba-comutavel').addClass('obscurecido'); aba-principal1
            $('#aba-principal1').addClass('obscurecido');
            $('#aba-principal2').addClass('obscurecido');
            $('#aba-principal3').addClass('obscurecido');
            $('#aba-principal4').addClass('obscurecido');
        });

        $('#ShowAdvancedFiltersSpan').click(function () {
            $('#ShowAdvancedFiltersSpan').hide();
            $('#HideAdvancedFiltersSpan').show();
            $('#AdvacedAtendimentosFiltersArea').slideDown();
        });

        $('#HideAdvancedFiltersSpan').click(function () {
            $('#HideAdvancedFiltersSpan').hide();
            $('#ShowAdvancedFiltersSpan').show();
            $('#AdvacedAtendimentosFiltersArea').slideUp();
        });

        $('#ExportarAtendimentosParaExcelButton').click(function () {
            _AtendimentosService
                .listarParaExcel({
                    filtro: $('#AtendimentosTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetAtendimentosButton, #RefreshAtendimentosButton').click(function (e) {
            e.preventDefault();
            getAtendimentos();
        });

        //controla  filtragem  por data
        $('#FiltroDataId').on("change", function (e) {
            e.preventDefault();
            var valorRangeAtendimento = $(this).val();
            if (valorRangeAtendimento == "Internado") {
                //$('#dateRangeInternacao').prop('disabled', true);
                //$('#dateRangeInternacao').val('Desabilitado');
                $("#divDataRange").hide("slow");
            } else {
                //$('#dateRangeInternacao').prop('disabled', false);
                //$('#dateRangeInternacao').val(data + " - " + data);
                $("#divDataRange").show("slow");
            }
        });


        //$(".select2Empresa").select2({
        //    allowClear: true,
        //    placeholder: app.localize("SelecioneLista"),
        //    ajax: {
        //        url: "/api/services/app/empresa/ListarDropdownPorUsuario",
        //        dataType: 'json',
        //        delay: 250,
        //        method: 'Post',
        //        data: function (params) {
        //            if (params.page == undefined)
        //                params.page = '1';
        //            return {
        //                search: params.term,
        //                page: params.page,
        //                totalPorPagina: 10
        //            };
        //        },
        //        processResults: function (data, params) {
        //            params.page = params.page || 1;
        //            return {
        //                results: data.result.items,
        //                pagination: {
        //                    more: (params.page * 10) < data.result.totalCount
        //                }
        //            };
        //        },
        //        cache: true
        //    },
        //    escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
        //    minimumInputLength: 0
        //});


        selectSWWithDefaultValue(".select2Empresa", "/api/services/app/empresa/ListarDropdownPorUsuario");

        selectSWWithDefaultValue(".select2Paciente", "/api/services/app/paciente/ListarDropdown");

        abp.event.on('app.AltaModalViewModel', function () {
            getAtendimentos();
        });

        if (!$.cookie("impressora_etiqueta_visitante") || !$.cookie("impressora_etiqueta_paciente") || !$.cookie("impressora_terminal_de_senha")) {
            var _impressorasModal = new app.ModalManager({
                viewUrl: abp.appPath + 'Mpa/Impressoras/ImpressorasModal',
                scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Impressoras/ImpressorasModal.js',
                modalClass: 'ImpressorasModal'
            });
            _impressorasModal.open();
        }

    });
})();