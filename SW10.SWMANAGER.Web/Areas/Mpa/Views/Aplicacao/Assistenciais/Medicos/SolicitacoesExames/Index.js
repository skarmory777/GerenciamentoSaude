(function () {
    $(function () {

        var _$SolicitacoesExamesTable = $('#SolicitacoesExamesTable-' + localStorage["AtendimentoId"]);
        var _$DetalharSolicitacoesExamesTable = $('#DetalharSolicitacaoExameTable-' + localStorage["AtendimentoId"]);
        var _SolicitacoesExamesService = abp.services.app.solicitacaoExame;
        var _SolicitacoesExamesItensService = abp.services.app.solicitacaoExameItem;
        var _$filterForm = $('#SolicitacoesExamesFilterForm-' + localStorage["AtendimentoId"]);
        var _selectedDateRange = {
            startDate: moment(localStorage["DataAtendimento"]).startOf('day'), //moment().startOf('day'),
            endDate: moment().endOf('day')
        };

        console.log(_selectedDateRange);

        $('#date-range-' + localStorage["AtendimentoId"]).daterangepicker(
            $.extend(true, app.createDateRangePickerOptions(), _selectedDateRange),
            function (start, end, label) {
                console.log(_selectedDateRange);
                _selectedDateRange.startDate = start.format('YYYY-MM-DDT00:00:00Z');
                _selectedDateRange.endDate = end.format('YYYY-MM-DDT23:59:59.999Z');
            });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.SolicitacaoExame'),
            edit: abp.auth.hasPermission('Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.SolicitacaoExame'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.SolicitacaoExame')
        };

        var _permissionsItem = {
            create: abp.auth.hasPermission('Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.SolicitacaoExameItem'),
            edit: abp.auth.hasPermission('Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.SolicitacaoExameItem'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.SolicitacaoExameItem')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Assistenciais/CriarOuEditarSolicitacaoExameModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Assistenciais/Medicos/SolicitacoesExames/CriarOuEditarSolicitacaoExame.js',
            modalClass: 'CriarOuEditarSolicitacaoExameModal'
        });

        var _createOrEditItemModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Assistenciais/CriarOuEditarSolicitacaoExameItemModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Assistenciais/Medicos/SolicitacoesExames/CriarOuEditarSolicitacaoExameItem.js',
            modalClass: 'CriarOuEditarSolicitacaoExameItemModal'
        });

        abp.event.off('app.CriarOuEditarSolicitacaoExameModalSaved', onEventCriarOuEditarSolicitacaoExameModalSaved)
        abp.event.on('app.CriarOuEditarSolicitacaoExameModalSaved', onEventCriarOuEditarSolicitacaoExameModalSaved)

        function onEventCriarOuEditarSolicitacaoExameModalSaved(data) {
            debugger;
            if(data && data.id) {
                $.removeCookie("XSRF-TOKEN");
                printJS({
                    printable: '/Mpa/AssistenciaisRelatorios/ImprimirSolicitacaoExames?solicitacaoExameId=' + data.id, type: 'pdf',
                })
            }
        }

        $('#SolicitacoesExamesTable-' + localStorage["AtendimentoId"]).jtable({

            title: app.localize('SolicitacaoExame'),
            paging: true,
            sorting: true,
            multiSorting: true,
            selecting: true,
            multiselect: false,
            selectingCheckboxes: true,

            actions: {
                listAction: {
                    method: _SolicitacoesExamesService.listar
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
                unidadeOrganizacional: {
                    title: app.localize('UnidadeOrganizacional'),
                    width: '30%'
                    //display: function (data) {
                    //    if (data.record.unidadeOrganizacional != null) {
                    //        return data.record.unidadeOrganizacional.descricao;
                    //    }
                    //    else {
                    //        return '';
                    //    }
                    //}
                },
                medicoSolicitante: {
                    title: app.localize('MedicoSolicitante'),
                    width: '30%'
                    //display: function (data) {
                    //    return data.record.medicoSolicitante.nomeCompleto;
                    //}
                },
            },
            selectionChanged: function () {
                //Get all selected rows
                var $selectedRows = $('#SolicitacoesExamesTable-' + localStorage["AtendimentoId"]).jtable('selectedRows');
                if ($selectedRows.length > 0) {
                    //Show selected rows
                    $selectedRows.each(function () {
                        var record = $(this).data('record');
                        localStorage["SolicitacaoExameId"] = record.id;
                        $('#codigo-item-' + localStorage["AtendimentoId"]).html("<span>" + app.localize('Codigo') + ": " + zeroEsquerda(record.codigo, '8') + "</span>");
                        $('#medico-item-' + localStorage["AtendimentoId"]).html("<span>" + app.localize('Medico') + ": " + record.medicoSolicitante + "</span>");
                        $('#content-detalhe-solicitacao-exame-' + localStorage["AtendimentoId"]).removeClass('hidden');
                        getDetalharSolicitacoesExames(record.id);
                        renderizarRelatorio(record.id);
                    });
                }
                else {
                    localStorage.removeItem("SolicitacaoExameId");
                    $('#content-detalhe-solicitacao-exame-' + localStorage["AtendimentoId"]).addClass('hidden');
                }
            },
            recordsLoaded: function (event, data) {

                $("div[id^='SolicitacoesExamesTable'] .jtable-main-container tr.jtable-data-row:first input[type=checkbox]").trigger('click');
            }
        });

        $('#DetalharSolicitacaoExameTable-' + localStorage["AtendimentoId"]).jtable({
            title: app.localize('DetalharSolicitacaoExame'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _SolicitacoesExamesItensService.listarPorSolicitacao
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '30%',
                    sorting: false,
                    display: function (data) {
                        var $span = $('<span></span>');
                        if (_permissionsItem.edit) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    _createOrEditItemModal.open({ solicitacaoId: localStorage["SolicitacaoExameId"], id: data.record.id, atendimentoId: localStorage["AtendimentoId"] });
                                });
                        }
                        if (_permissionsItem.delete) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    deleteSolicitacoesExamesItens(data.record);
                                });
                        }

                        if (data.record.accessNumber) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('AccessNumber') + '"><i class="fas fa-images"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    //_createOrEditModal.open({ id: data.record.id });
                                    window.open(data.record.accessNumber);
                                });
                        }
                        return $span;
                    }
                },
                faturamentoItem: {
                    title: app.localize('Exame'),
                    width: '40%',
                    sorting: false,
                    //display: function (data) {
                    //    if (data.record.faturamentoItem) {
                    //        return data.record.faturamentoItem.descricao;
                    //    }
                    //}
                },
                material: {
                    title: app.localize('Material'),
                    width: '30%',
                    sorting: false,
                    //display: function (data) {
                    //    if (data.record.material) {
                    //        return data.record.material.descricao;
                    //    }
                    //}
                }
            },
        });

        function getDetalharSolicitacoesExames(id) {
            $('#DetalharSolicitacaoExameTable-' + localStorage["AtendimentoId"]).jtable('load', {
                Filtro: id
            });
        }

        function getSolicitacoesExames() {
            $('#SolicitacoesExamesTable-' + localStorage["AtendimentoId"]).jtable('load', createRequestParams());
        }

        function getSolicitacaoExameItens() {
            $('#DetalharSolicitacaoExameTable-' + localStorage["AtendimentoId"]).jtable('load', {
                Filtro: localStorage["SolicitacaoExameId"]
            });
        }

        function deleteSolicitacoesExames(solicitacaoExame) {
            abp.message.confirm(
                app.localize('DeleteWarning', solicitacaoExame.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _SolicitacoesExamesService.excluir(solicitacaoExame)
                            .done(function () {
                                getSolicitacoesExames();
                                abp.notify.success(app.localize('SuccessfullyDeleted'));
                            });
                    }
                }
            );
        }

        function createRequestParams() {
            var prms = {};
            _$filterForm.serializeArray().map(function (x) { prms[x.name] = x.value; });
            return $.extend(prms, _selectedDateRange);
        }

        function deleteSolicitacoesExamesItens(solicitacaoExameItem) {
            abp.message.confirm(
                app.localize('DeleteWarning', solicitacaoExameItem.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _SolicitacoesExamesItensService.excluir(solicitacaoExameItem.id)
                            .done(function () {
                                getSolicitacaoExameItens();
                                abp.notify.success(app.localize('SuccessfullyDeleted'));
                            });
                    }
                }
            );
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

        $('#CreateNewSolicitacaoExameButton-' + localStorage["AtendimentoId"]).click(function (e) {
            e.preventDefault();
            debugger;
            _SolicitacoesExamesService.validarSolicitacaoExame(localStorage["AtendimentoId"]).then(res => {
                console.log(res);
                if (res) {
                    swal({
                        title: "Data solicitação do Exame",
                        text: "Deseja fazer a solicitação do exame para o hoje ou para amanhã?",
                        type: "info",
                        confirmButtonText: "Amanhã",
                        cancelButtonText: "Hoje",
                        showCancelButton: true,
                        closeOnConfirm: true,
                        showLoaderOnConfirm: true
                    }, function (isConfirm) {
                        abreSolicitacaoExames(isConfirm);
                    });
                }
                else {
                    abreSolicitacaoExames(false);
                }
            });

            function abreSolicitacaoExames(isConfirm) {
                _createOrEditModal.open({
                    atendimentoId: localStorage["AtendimentoId"],
                    dataFutura: isConfirm
                });
            }

        });

        $('#ExportarSolicitacoesExamesParaExcelButton-' + localStorage["AtendimentoId"]).click(function () {
            _SolicitacoesExamesService
                .listarParaExcel({
                    filtro: $('#SolicitacoesExamesTableFilter-' + localStorage["AtendimentoId"]).val(),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#RefreshSolicitacaoExameItensListButton-' + localStorage["AtendimentoId"]).click(function (e) {
            e.preventDefault();
            getSolicitacaoExameItens();
        });

        $('#CreateNewSolicitacaoExameItemButton-' + localStorage["AtendimentoId"]).click(function (e) {
            e.preventDefault();
            _createOrEditItemModal.open({
                solicitacaoId: localStorage["SolicitacaoExameId"],
                atendimentoId: localStorage["AtendimentoId"]
            });
        });
        $('#GetSolicitacoesExamesButton-' + localStorage["AtendimentoId"]).on('click', function (e) {
            e.preventDefault();
            getSolicitacoesExames();
        });

        $('#RefreshSolicitacoesExamesListButton-' + localStorage["AtendimentoId"]).on('click', function (e) {
            e.preventDefault();
            getSolicitacoesExames();
        });

        abp.event.on('app.CriarOuEditarSolicitacaoExameModalSaved', function () {
            getSolicitacoesExames();
        });

        getSolicitacoesExames();

        $('#SolicitacoesExamesTableFilter-' + localStorage["AtendimentoId"]).focus();



        function renderizarRelatorio(id) {
            var caminho = `/Mpa/AssistenciaisRelatorios/ImprimirSolicitacaoExames%3FsolicitacaoExameId%3D` + id;
            var urlPath = window.location.href.split(window.location.pathname)[0].split("//")[1];
            $('#fVisualizar').attr('src', "//" + urlPath + "/libs/pdfjs/web/viewer.html?file=" + caminho + "&locale=pt-BR");
            $('#dvVisualizar').show();
        }


    });
})();