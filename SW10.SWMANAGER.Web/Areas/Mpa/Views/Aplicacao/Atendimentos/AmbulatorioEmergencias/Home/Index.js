(function () {
    $(function () {
       
        //==============CARREGADOS JUNTO A PÁGINA================
        $('#AtendimentosTableFilter').focus();
        var _$AtendimentosTable = $('#AtendimentosTable');
        var _AtendimentosService = abp.services.app.atendimento;
        var _$filterForm = $('#AtendimentosFilterForm');
        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Atendimento.AmbulatorioEmergencia.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Atendimento.AmbulatorioEmergencia.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Atendimento.AmbulatorioEmergencia.Delete')
        };
        var _userPermissionsModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Users/PermissionsModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Atendimentos/Atendimentos/_PermissionsModal.js',
            modalClass: 'UserPermissionsModal'
        });

        //Retrair menu principal
        $('body').addClass('page-sidebar-closed');
        $('#menu-lateral').addClass('page-sidebar-menu-closed');

        //Date Range Picker
        var _$filterForm = $('#AtendimentosFilterForm');
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
        //===============FIM CARREGADOS JUNTO A PÁGINA===================


        //==============NOVO ATENDIMENTO======================
        // Novo Atendimento
        var abaNovoAtendimento = $('#aba-novo-atendimento');
        window.atendimentoCounter = 0;

        $('.novo-atendimento').click(function (e) {
            e.preventDefault();
            window.atendimentoCounter++;
            criarAba(0, window.atendimentoCounter);
            return;
        });

        function criarAba(atendimento, atendimentoCounter) {

            var abas = $('#abas');
            var conteudoAbas = $('#conteudo-abas');
            var divNovo = $('<div class="tab-pane aba-comutavel conteudo" id="novo-atendimento-' + atendimentoCounter + '"><div></div></div>');

            var infoExtraAba;
            if (atendimento == 0) {
                infoExtraAba = moment().format('DD-MM HH:mm');
            }
            else {

                infoExtraAba = atendimento.paciente || '..';

                //if (atendimento.paciente) {
                //    infoExtraAba = atendimento.paciente.nomeCompleto
                //    if (atendimento.paciente.sisPessoa) {
                //        infoExtraAba = atendimento.paciente.sisPessoa.nomeCompleto
                //    }
                //} else {
                //    infoExtraAba = '...';
                //}
            }

            var abaAtendimento = $('<li class="borda-aba" onclick="lerPartial(' + atendimentoCounter + ')" id="li-atendimento-' + atendimentoCounter + '"><a href="#novo-atendimento-' + atendimentoCounter + '"class="link-atendimento"  id="aba-atendimento-' + atendimentoCounter + '" data-toggle="tab" aria-expanded="false">Atendimento <i id="icone-aba-' + atendimentoCounter + '" class=""></i><i class="fa fa-close botao-aba" id="aba-' + atendimentoCounter + '" onclick="fecharAba(' + atendimentoCounter + ')" style="float:right;"></i><br/><span id="nome-paciente">' + infoExtraAba + '</span></a></li>');

            divNovo.appendTo(conteudoAbas);
            abaAtendimento.appendTo(abas);
            abaNovoAtendimento.insertAfter(abaAtendimento);
            lerPartial(atendimento, atendimentoCounter);
        }

        function lerPartial(atendimento, atendimentoCounter) {

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
            var metodoNovoAtendimento = '/Mpa/Atendimentos/Index?id=' + atendimentoId + '&abaId=' + atendimentoCounter + '&ambulatorioEmergencia=true';

            $(strId).load(metodoNovoAtendimento, function (data) {
                var abaAtendimento = '#aba-atendimento-' + atendimentoCounter;
                $(abaAtendimento).attr("aria-expanded", true);
                $(this).addClass("active").removeClass('obscurecido');
                $(abaAtendimento).parent().addClass('active').removeClass('obscurecido');
            });
        };


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

                //INSERI AS INFORMAÇÕES DO ELEMENTO SELECIONADO PARA CONSULTAR SE ELE JA ESTA ABERTO POSTERIORMENTE
                edicao[atendimentoCounter] = { id: atendimento.id, aba: atendimentoCounter, nome: atendimento.paciente.nomeCompleto }

            } else {
                //SE Ñ ELE RECUPERA AS INFORMAÇÕES DA ABA ABERTA QUE ESTA NO ARRAY 'indice'
                atendimento.id = selecionado[0].id;
                atendimento.paciente.nomeCompleto = selecionado[0].nome;
                atendimentoCounter = selecionado[0].aba;

                lerPartial(atendimento, atendimentoCounter);
            }
        }

        var _cancelamentoAtendimentoModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Atendimentos/CancelarAtendimentoModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Atendimentos/Internacoes/Home/Cancelamento/CancelarAtendimentoModal.js',
            modalClass: 'CancelarAtendimentoModal'
        });

        //====================EXCLUIR ATENDIMENTO========================
        function deleteAtendimentos(Atendimento) {

            _cancelamentoAtendimentoModal.open({ id: Atendimento.id });

            //abp.message.confirm(
            //    app.localize('CancelarWarning', Atendimento.codigoAtendimento),
            //    function (isConfirmed) {
            //        if (isConfirmed) {
            //            _AtendimentosService.excluir(Atendimento.id)
            //                .done(function () {
            //                    getAtendimentos();
            //                    abp.notify.success(app.localize('SuccessfullyCancelamento'));
            //                });
            //        }
            //    }
            //);
        }


        //=================BUSCAR ATENDIMENTOS===========================
        //metodo chamado pela IndexPartial p/ carrega o grid (pablo)
        window.carregaAtendimento = function () {
            getAtendimentos()
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
            title: app.localize('Atendimentos'),
            paging: true,
            sorting: true,
            multiSorting: true,
            selecting: true,
            selectingCheckboxes: true,
            actions: {
                listAction: {
                    method: abp.services.app.atendimento.listarFiltro
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

                        if (_permissions.edit && data.record.atendimentoMotivoCancelamentoId == null) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    //CHAMADA DO METODO PARA EDIÇÃO
                                    editarAtendimento(data.record)
                                });
                        }

                        if (_permissions.delete && data.record.atendimentoMotivoCancelamentoId == null) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    deleteAtendimentos(data.record);
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

                        return $span;
                    }
                }
                ,
                "unidadeOrganizacional.Descricao": {
                    title: app.localize('Unidade'),
                    width: '10%',
                    display: function (data) {
                        if (data.record.unidade) {
                            //return data.record.unidadeOrganizacional.organizationUnit.displayName;
                            return data.record.unidade;
                        }
                    }
                }
                ,
                "ateAtendimento.Codigo": {
                    title: app.localize('CodigoAtendimento'),
                    width: '10%',
                    display: function (data) {
                        if (data.record.codigoAtendimento) {
                            //return data.record.unidadeOrganizacional.organizationUnit.displayName;
                            return data.record.codigoAtendimento;
                        }
                    }
                },
                "ateAtendimento.DataRegistro": {
                    title: app.localize('DataAtendimento'),
                    width: '10%',
                    display: function (data) {
                        return moment(data.record.dataRegistro).format('L LT');
                    }
                },
                "PessoaPaciente.Codigo": {
                    title: app.localize('CodigoPaciente'),
                    width: '10%',
                    display: function (data) {

                        if (data.record.codigoPaciente) {
                            return zeroEsquerda(data.record.codigoPaciente, 10);
                        }
                    }
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
                //dataAlta: {
                //    title: app.localize('DataAlta'),
                //    width: '6%',
                //    display: function (data) {
                //        if (data.record.dataAlta) {
                //            return moment(data.record.dataAlta).format('L LT');
                //            //return moment(data.record.dataAlta).format("DD/MM/YYYY HH:mm");
                //        }

                //    }
                //},
                "SisPessoaConvenio.NomeFantasia": {
                    title: app.localize('Convenio'),
                    width: '10%'
                    ,
                    display: function (data) {
                        if (data.record.convenio) {
                            return data.record.convenio;
                        }
                    }
                }
                ,
                "PessoaMedico.NomeCompleto": {
                    title: app.localize('Medico'),
                    width: '15%'
                    ,
                    display: function (data) {
                        if (data.record.medico) {
                            return data.record.medico;
                        }
                    }
                },
                //tipoLeitoId: {
                //    title: app.localize('tipoLeito'),
                //    width: '5%',
                //    display: function (data) {
                //        if (data.record.tipoLeito) {
                //            return data.record.tipoLeito;
                //        }
                //    }
                //},
                //leitoId: {
                //    title: app.localize('leito'),
                //    width: '5%',
                //    display: function (data) {
                //        if (data.record.leito) {
                //            return data.record.leito;
                //        }
                //    }
                //},
                "SisEmpresa.NomeFantasia": {
                    title: app.localize('Empresa'),
                    width: '10%'
                    ,
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
                }
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
                        //$('#atendimento-selecionado').val(record.id);
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

        function reativarAtendimentos(Atendimento) {
            abp.message.confirm(
                app.localize('ReativarWarning', Atendimento.codigoAtendimento),
                function (isConfirmed) {
                    if (isConfirmed) {

                        // _cancelamentoAtendimentoModal.open({ id: Atendimento.id });

                        _AtendimentosService.reativar(Atendimento.id)
                            .done(function () {
                                getAtendimentos();
                                abp.notify.success(app.localize('SuccessfullyReativado'));
                            });
                    }
                }
            );
            _reativarAtendimentoModal.open({ id: Atendimento.id });

        }

        //==========CHAMADOS AO CARREGAR PÁGINA============================
        //chamada do grid
        aplicarSelect2Padrao();
        getAtendimentos();


        //=====================CHAMADAS JQUERY===============================
        $('.link-atendimento').on("click", function () {
            $('.borda-aba').addClass('obscurecido');
        });

        $('#ShowAdvancedFiltersSpan').click(function () {
            $('#ShowAdvancedFiltersSpan').hide();
            $('#HideAdvancedFiltersSpan').show();
            $('#AdvacedAtendimentosFiltersArea').slideDown();;
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

        $('#FiltroDataId').on("change", function (e) {

            e.preventDefault();

            var valorRangeAtendimento = $(this).val();
            if (valorRangeAtendimento == "Internado") {
                $("#divDataRange").hide("slow");
            } else {
                $("#divDataRange").show("slow");
            }
        });

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
        //    var caminho = abp.appPath + 'Mpa/' + controller + '/IndexAtendimentoEtiqueta?atendimentoId=' + atendimentoId;  // "@Url.Action("IndexAtendimentoEtiqueta", "AtendimentoRelatorio", new { atendimentoId = atendimentoId })";
        //   
        //    PDFObject.embed(caminho, "#atendimento-etiqueta");
        //});

        $(".select2Empresa").select2({
            allowClear: true,
            placeholder: app.localize("SelecioneLista"),
            ajax: {
                url: "/api/services/app/empresa/ListarDropdownPorUsuario",
                dataType: 'json',
                delay: 250,
                method: 'Post',
                data: function (params) {
                    if (params.page == undefined)
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

        $(".selec2UnidadeOrganizacional").select2({
            allowClear: true,
            placeholder: app.localize("SelecioneLista"),
            ajax: {
                url: "/api/services/app/unidadeorganizacional/ListarDropdown",
                dataType: 'json',
                delay: 250,
                method: 'Post',
                data: function (params) {
                    if (params.page == undefined)
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

        $(".selec2Medico").select2({
            allowClear: true,
            placeholder: app.localize("SelecioneLista"),
            ajax: {
                url: "/api/services/app/medico/ListarDropdown",
                dataType: 'json',
                delay: 250,
                method: 'Post',
                data: function (params) {
                    if (params.page == undefined)
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

        $(".selec2Convenio").select2({
            allowClear: true,
            placeholder: app.localize("SelecioneLista"),
            ajax: {
                url: "/api/services/app/convenio/ListarDropdown",
                dataType: 'json',
                delay: 250,
                method: 'Post',
                data: function (params) {
                    if (params.page == undefined)
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
