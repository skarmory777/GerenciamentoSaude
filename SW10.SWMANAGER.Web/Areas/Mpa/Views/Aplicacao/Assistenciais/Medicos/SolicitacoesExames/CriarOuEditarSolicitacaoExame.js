(function ($) {
    app.modals.CriarOuEditarSolicitacaoExameModal = function () {




        var _solicitacoesExamesService = abp.services.app.solicitacaoExame;
        var _solicitacaoExameItensService = abp.services.app.solicitacaoExameItem;
        var _$filterForm = $('#SolicitacoesExamesItensFilterForm-' + localStorage["AtendimentoId"]);
        var _$solicitacaoExameForm = null;
        var _$solicitacaoExameItemForm = null;
        var _fatItemService = abp.services.app.faturamentoItem;
        var _materialService = abp.services.app.material;
        var _kitExameItemService = abp.services.app.kitExameItem;


        var $_SolicitacaoExameItensTable = $('#SolicitacaoExameItensTable-' + localStorage["AtendimentoId"]);
        var _modalManager;
        var _$solicitacaoExameInformationForm = null;

        var _createOrEditItemModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Assistenciais/CriarOuEditarSolicitacaoExameItemModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Assistenciais/Medicos/SolicitacoesExames/CriarOuEditarSolicitacaoExameItem.js',
            modalClass: 'CriarOuEditarSolicitacaoExameItemModal'
        });

        var _selecionarKitExameModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/KitsExames/SelecionarKitExameModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/KitsExames/_SelecionarKitExameItemModal.js',
            modalClass: 'SelecionarKitExameItemModal'
        });

        var _ErrorModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Erros/ExibirErros',
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.SolicitacaoExameItem'),
            edit: abp.auth.hasPermission('Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.SolicitacaoExameItem'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.SolicitacaoExameItem')
        };

        this.init = function (modalManager) {
            _modalManager = modalManager;
            $('.modal-dialog').addClass("fullscreen").css("overflow-x","auto");
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };


        };

        this.save = function () {
            _$solicitacaoExameInformationForm = _modalManager.getModal().find('form[name=SolicitacaoExameInformationsForm-' + localStorage["AtendimentoId"] + ']');
            _$solicitacaoExameInformationForm.validate();
            if (!_$solicitacaoExameInformationForm.valid()) {
                return;
            }

            // inserirKit();
            // getKitExameItens();
            // salvarRegistro();

            var solicitacaoExame = _$solicitacaoExameInformationForm.serializeFormToObject();
            //isEdit = $('#id-solicitacao-' + localStorage["AtendimentoId"]).val() > 0;
            _modalManager.setBusy(true);
            _solicitacoesExamesService.criarOuEditar(solicitacaoExame)
                .done(function (data) {
                    if (data.errors.length > 0) {
                        _ErrorModal.open({ erros: data.errors });
                    }
                    else {
                        abp.notify.success(app.localize('SavedSuccessfully'));
                        debugger;
                        abp.event.trigger('app.CriarOuEditarSolicitacaoExameModalSaved', data.returnObject);
                        _modalManager.close();
                    }
                })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };

        $("#medico-solicitante-id-" + localStorage["AtendimentoId"]).select2({
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

        $("#prioridade-" + localStorage["AtendimentoId"]).select2({
            allowClear: true,
            placeholder: app.localize("SelecioneLista"),
            ajax: {
                url: "/api/services/app/solicitacaoExamePrioridade/ListarDropdown",
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

        $("#cbo-kits-" + localStorage["AtendimentoId"]).select2({
            allowClear: true,
            placeholder: app.localize("SelecioneLista"),
            ajax: {
                url: "/api/services/app/kitExame/ListarDropdown",
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
        }).on('select2:select', function (evt) {
            evt.preventDefault();
            getKitExameItens();
        });

        $('#SolicitacaoExameItensTable-' + localStorage["AtendimentoId"]).jtable({
            title: app.localize('SolicitacaoExameItem'),
            edit: false,
            create: false,
            //paging: true,
            sorting: false,
            //multiSorting: true,
            //actions: {
            //    listAction: {
            //        method: _solicitacaoExameItensService.listar
            //    }
            //},
            fields: {
                Id: {
                    key: true,
                    list: false
                },
                IdGrid: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '10%',
                    sorting: false,
                    display: function (data) {
                        var $span = $('<span></span>');
                        if (_permissions.edit) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                .appendTo($span)
                                .click(function (e) {
                                    e.preventDefault();
                                    //_createOrEditItemModal.open({ solicitacaoId: $('#id-solicitacao-' + localStorage["AtendimentoId"]).val(), id: data.record.id });
                                    editarRegistro(data.record.Id, data.record.IdGrid, data.record);
                                });
                        }
                        if (_permissions.delete) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                .appendTo($span)
                                .click(function (e) {
                                    e.preventDefault();
                                    //deleteSolicitacoesExamesItens(data.record.id);
                                    deleteSolicitacaoItem(data.record);
                                });
                        }
                        return $span;
                    }
                },
                FaturamentoItem: {
                    title: app.localize('Exame'),
                    width: '40%',
                    sorting: false,
                    display: function (data) {
                        if (data.record.FaturamentoItem) {
                            return data.record.FaturamentoItem.descricao;
                        }
                    }
                },
                Material: {
                    title: app.localize('Material'),
                    width: '40%',
                    sorting: false,
                    display: function (data) {
                        if (data.record.Material) {
                            return data.record.Material.descricao;
                        }
                    }
                },
                GuiaNumero: {
                    title: app.localize('GuiaNumero'),
                    width: '10%'
                }
            }
        });

        function getSolicitacaoItem() {
            var lista = $('#itens-' + localStorage["AtendimentoId"]).val() != null && $('#itens-' + localStorage["AtendimentoId"]).val() != '' && $('#itens-' + localStorage["AtendimentoId"]).val() != undefined ? JSON.parse($('#itens-' + localStorage["AtendimentoId"]).val()) : [];
            for (var i = 0; i < lista.length; i++) {
                var item = lista[i];
                $('#SolicitacaoExameItensTable-' + localStorage["AtendimentoId"]).jtable('addRecord', {
                    record: item,
                    clientOnly: true
                });
            }
        }

        function getSolicitacaoExameItens() {
            $('#SolicitacaoExameItensTable-' + localStorage["AtendimentoId"]).jtable('load', {
                Filtro: $('#SolicitacaoExameItensTableFilter-' + localStorage["AtendimentoId"]).val(),
                SolicitacaoExameId: $('#id-solicitacao-' + localStorage["AtendimentoId"]).val()
            });
        }

        function createRequestParams() {
            var prms = {};
            _$filterForm.serializeArray().map(function (x) { prms[x.name] = x.value; });
            return $.extend(prms, _selectedDateRange);
        }

        $('#CreateNewSolicitacaoExameItemSubButton-' + localStorage["AtendimentoId"]).on('click', function (e) {
            e.preventDefault();
            //_createOrEditItemModal.open({ solicitacaoId: $('#id-solicitacao-' + localStorage["AtendimentoId"]).val() });
            novoRegistro();
        });

        $('#ExportarSolicitacaoExameItensParaExcelButton-' + localStorage["AtendimentoId"]).on('click', function (e) {
            e.preventDefault();
            _solicitacaoExameItensService
                .listarParaExcel({
                    filtro: $('#SolicitacoesExamesItensTableFilter').val(),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetSolicitacaoExameItensButton-' + localStorage["AtendimentoId"]).click(function (e) {
            e.preventDefault();
            getSolicitacaoItem();
        });

        $('#RefreshSolicitacaoExameItensListButton-' + localStorage["AtendimentoId"]).click(function (e) {
            e.preventDefault();
            getSolicitacaoItem();
        });

        abp.event.on('app.CriarOuEditarSolicitacaoExameItemModalSaved', function () {
            getSolicitacaoItem();
        });

        abp.event.on('app.SelecionarKitExameItemModalSaved', function () {
            getSolicitacaoItem();
        });

        //function deleteSolicitacoesExamesItens(solicitacaoExameItem) {
        //    abp.message.confirm(
        //        app.localize('DeleteWarning', solicitacaoExameItem.descricao),
        //        function (isConfirmed) {
        //            if (isConfirmed) {
        //                _solicitacaoExameItensService.excluir(solicitacaoExameItem.id)
        //                    .done(function () {
        //                        getSolicitacaoExameItens();
        //                        abp.notify.success(app.localize('SuccessfullyDeleted'));
        //                    });
        //            }
        //        }
        //    );
        //}

        $('#salvar-solicitacao-item-' + localStorage["AtendimentoId"]).on('click', function (e) {
            e.preventDefault();
            salvarRegistro();
        });

        $('#cancela-solicitacao-item-' + localStorage["AtendimentoId"]).on('click', function (e) {
            e.preventDefault();
            novoRegistro();
        });

        function deleteSolicitacaoItem(item) {
            abp.message.confirm(
                app.localize('DeleteWarning', item.Descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        lista = JSON.parse($('#itens-' + localStorage["AtendimentoId"]).val());
                        for (var i = 0; i < lista.length; i++) {
                            if (lista[i].IdGrid == item.IdGrid) {
                                lista.splice(i, 1);
                                $('#itens-' + localStorage["AtendimentoId"]).val(JSON.stringify(lista));
                                $('#SolicitacaoExameItensTable-' + localStorage["AtendimentoId"]).jtable('deleteRecord', {
                                    key: item.IdGrid,
                                    clientOnly: true
                                });
                                break;
                            }
                        }
                    }
                }
            );
        }

        function salvarRegistro() {
            debugger;

            _$solicitacaoExameItemInformationForm = _modalManager.getModal().find('form[name=SolicitacaoExameItemInformationsForm-' + localStorage["AtendimentoId"] + ']');
            _$solicitacaoExameItemInformationForm.validate();
            var solicitacaoItem = _$solicitacaoExameItemInformationForm.serializeFormToObject();
            var form1 = solicitacaoItem;

            if (form1.FaturamentoItemId != undefined && form1.FaturamentoItemId != null && form1.FaturamentoItemId != '') {

                var list = $('#itens-' + localStorage["AtendimentoId"]).val();

                if (list != '') {
                    var lista = JSON.parse(list);
                }
                else {
                    var lista = [];
                }

                if (form1.IdGrid !== null && form1.IdGrid !== '' && form1.IdGrid !== undefined) {
                    //var itemProcessado = false;
                    for (var i = 0; i < lista.length; i++) {
                        if (lista[i].IdGrid == form1.IdGrid) {
                            /*
                                    SolicitacaoExameId = item.SolicitacaoExameId,
                                    Codigo = item.Codigo,
                                    DataValidade = item.DataValidade,
                                    Descricao = item.Descricao,
                                    FaturamentoItemId = item.FaturamentoItemId,
                                    GuiaNumero = item.GuiaNumero,
                                    Justificativa = item.Justificativa,
                                    KitExameId = item.KitExameId,
                                    MaterialId = item.MaterialId,
                                    SenhaNumero = item.SenhaNumero,
                                    StatusSolicitacaoExameItemId = item.StatusSolicitacaoExameItemId
                            */
                            var fatItem = _fatItemService.obter(form1.FaturamentoItemId, { async: false }).done(function (data) {
                                    form1.FaturamentoItem = data;
                                });
                            if (form1.MaterialId !== null && form1.MaterialId !== undefined && form1.MaterialId !== '') {
                                var material = _materialService.obter(form1.MaterialId, { async: false }).done(function (data) {
                                    form1.Material = data;
                                    lista[i].MaterialId = form1.MaterialId;
                                    lista[i].Material = form1.Material;
                                });
                            }
                            lista[i].Codigo = form1.Codigo;
                            lista[i].DataValidade = form1.DataValidade;
                            lista[i].Descricao = form1.Descricao;
                            lista[i].FaturamentoItemId = form1.FaturamentoItemId;
                            lista[i].FaturamentoItem = form1.FaturamentoItem;
                            lista[i].Id = parseInt(form1.Id) || 0;
                            lista[i].IdGrid = form1.IdGrid;
                            lista[i].GuiaNumero = form1.GuiaNumero;
                            lista[i].Justificativa = form1.Justificativa;
                            lista[i].KitExameId = form1.KitExameId;
                            lista[i].SenhaNumero = form1.SenhaNumero;
                            lista[i].StatusSolicitacaoExameItemId = form1.StatusSolicitacaoExameItemId;

                            $('#SolicitacaoExameItensTable-' + localStorage["AtendimentoId"]).jtable('updateRecord', {
                                record: lista[i],
                                clientOnly: true
                            });

                            //itemProcessado = true;
                            break;
                        }
                    }
                    //if (!itemProcessado) {
                    //    form1.IdGrid = lista.length == 0 ? 1 : lista[lista.length - 1].IdGrid + 1;
                    //    form1.FormataId = $('#formata-id').val();
                    //    lista.push(form1);
                    //}
                }
                else {
                    var fatItemNew = _fatItemService.obter(form1.FaturamentoItemId, { async: false })
                        .done(function (data) {
                            form1.FaturamentoItem = data;
                        });
                    if (form1.MaterialId !== null && form1.MaterialId !== undefined && form1.MaterialId !== '') {
                        var materialNew = _materialService.obter(form1.MaterialId, { async: false })
                            .done(function (data) {
                                form1.Material = data;
                            });
                    }
                    form1.IdGrid = lista.length == 0 ? 1 : lista[lista.length - 1].IdGrid + 1;
                    form1.SolicitacaoExameId = $('#id-solicitacao-' + localStorage["AtendimentoId"]).val();
                    lista.push(form1);

                    $('#SolicitacaoExameItensTable-' + localStorage["AtendimentoId"]).jtable('addRecord', {
                        record: form1,
                        clientOnly: true
                    });

                }
                $('#itens-' + localStorage["AtendimentoId"]).val(JSON.stringify(lista));
                abp.notify.info(app.localize('ListaAtualizada'));
                
                //_modalManager.close();
                novoRegistro();
            }
        }

        function novoRegistro() {
            $('#id-grid-' + localStorage["AtendimentoId"]).val('');
            $('#solicitacao-exame-item-id-' + localStorage["AtendimentoId"]).val(0);
            $('#solicitacao-exame-id-' + localStorage["AtendimentoId"]).val('');
            $('#valor-item-id-' + localStorage["AtendimentoId"]).val('');
            $('#valor-material-id-' + localStorage["AtendimentoId"]).val('');
            $('#faturamento-item-id-' + localStorage["AtendimentoId"]).val(null).change();
            $('#material-id-' + localStorage["AtendimentoId"]).val(null).change();
            $('#guia-numero-' + localStorage["AtendimentoId"]).val('');
            $('#data-validade-' + localStorage["AtendimentoId"]).val(moment().format("L"));
            $('#senha-numero-' + localStorage["AtendimentoId"]).val('');

            $('#salvar-solicitacao-item-' + localStorage["AtendimentoId"] + ' i').removeClass('fa-check').addClass('fa-plus');

            $('#exibir-sw-div-retratil-solicitao-item-' + localStorage["AtendimentoId"]).trigger('click');
        }

        function editarRegistro(id, idGrid, record) {
            debugger;
            abp.ui.setBusy();
            var list = JSON.parse($('#itens-' + localStorage["AtendimentoId"]).val());
            var data;
            for (var i = 0; i < list.length; i++) {
                if (list[i].IdGrid == idGrid) {
                    data = list[i];
                    break;
                }
            }
            $('#solicitacao-exame-item-id-' + localStorage["AtendimentoId"]).val(data.Id);
            $('#solicitacao-exame-id-' + localStorage["AtendimentoId"]).val(data.SolicitacaoExameId);
            $('#id-grid-' + localStorage["AtendimentoId"]).val(data.IdGrid);
            $('#valor-item-id-' + localStorage["AtendimentoId"]).val('');
            $('#valor-material-id-' + localStorage["AtendimentoId"]).val('');
            if (data.FaturamentoItemId != null && data.FaturamentoItemId > 0) {
                $.ajax({
                    url: '/api/services/app/faturamentoitem/obter?id=' + data.FaturamentoItemId,
                    method: 'POST',
                    async: false,
                    cache: false,
                    success: function (data) {
                        record.FaturamentoItem = data.result;
                    },
                })

                $('#faturamento-item-id-' + localStorage["AtendimentoId"])
                    .append('<option value="' + data.FaturamentoItemId + '">' + record.FaturamentoItem.descricao + '</option>')
                    .val(data.FaturamentoItemId)
                    .trigger('change');
            }
            if (data.MaterialId != null && data.MaterialId > 0) {
                $.ajax({
                    url: '/api/services/app/material/obter?id=' + data.MaterialId,
                    method: 'POST',
                    async: false,
                    cache: false,
                    success: function (data) {
                        record.Material = data.result;
                    },
                })

                $('#material-id-' + localStorage["AtendimentoId"])
                    .append('<option value="' + data.MaterialId + '">' + record.Material.descricao + '</option>')
                    .val(data.MaterialId)
                    .trigger('change');
            }
            $('#guia-numero-' + localStorage["AtendimentoId"]).val(data.GuiaNumero);
            $('#data-validade-' + localStorage["AtendimentoId"]).val(data.DataValidade);
            $('#senha-numero-' + localStorage["AtendimentoId"]).val(data.SenhaNumero);

            $('#exibir-sw-div-retratil-solicitacao-item-' + localStorage["AtendimentoId"]).trigger('click');

            $('#salvar-solicitacao-item-' + localStorage["AtendimentoId"] + ' i').removeClass('fa-plus').addClass('fa-check');

            abp.ui.clearBusy();

        }

        var fatItem = $('#faturamento-item-id-' + localStorage["AtendimentoId"]).select2({
            allowClear: true,
            placeholder: app.localize("SelecioneLista"),
            ajax: {
                url: "/api/services/app/faturamentoItem/ListarDropdownExame",
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

        var labMaterial = $('#material-id-' + localStorage["AtendimentoId"]).select2({
            allowClear: true,
            placeholder: app.localize("SelecioneLista"),
            ajax: {
                url: "/api/services/app/material/ListarDropdown",
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


        $('#kitExameItensTable-' + localStorage["AtendimentoId"]).jtable({
            title: app.localize('KitExameItem'),
            edit: false,
            create: false,
            //paging: true,
            sorting: false,
            multiSorting: false,
            selecting: true,
            multiselect: true,
            selectingCheckboxes: true,

            rowInserted: function (event, data) {
                data.row.click();
            },
            actions: {
                listAction: {
                    method: _kitExameItemService.listarPorKit
                }
            },
            fields: {
                Id: {
                    key: true,
                    list: false
                },
                KitExameId: {
                    key: true,
                    list: false
                },

                FaturamentoItem: {
                    title: app.localize('Exame'),
                    width: '40%',
                    sorting: false,
                    display: function (data) {
                        return data.record.exameDescricao;
                    }
                },
                Material: {
                    title: app.localize('Material'),
                    width: '40%',
                    sorting: false,
                    display: function (data) {
                        return data.record.materialDescricao;
                    }
                },
            }
            , recordsLoaded: function (event, data) {
                $("div[id^='kitExameItensTable'] .jtable-main-container tr.jtable-data-row input[type=checkbox]").trigger('click');
            }
        });

        function getKitExameItens() {
            $('#kitExameItensTable-' + localStorage["AtendimentoId"]).jtable('load', {
                Filtro: $('#cbo-kits-' + localStorage["AtendimentoId"]).val()
            });
        }

        $('#inserirKit-' + localStorage["AtendimentoId"]).click(function (e) {
            e.preventDefault();
            inserirKit();
            getKitExameItens();
        });

        function inserirKit() {
            var itensSelecionados = $('#kitExameItensTable-' + localStorage["AtendimentoId"]).jtable('selectedRows');

            var lista = [];
            var list = $('#itens-' + localStorage["AtendimentoId"]).val();

            if (list != '') {
                lista = JSON.parse(list);
            }


            itensSelecionados.each(function () {
                let registro = $(this).data('record');
                let form1 = {};
                form1.IdGrid = lista.length == 0 ? 1 : lista[lista.length - 1].IdGrid + 1;
                form1.FaturamentoItemId = registro.faturamentoItemId;
                form1.FaturamentoItem = {};
                form1.Material = {};
                form1.FaturamentoItem.descricao = registro.exameDescricao;
                // form1.exameDescricao = registro.exameDescricao;
                form1.MaterialId = registro.materialId;
                form1.Material.descricao = registro.materialDescricao;
                form1.SolicitacaoExameId = $('#id-solicitacao-' + localStorage["AtendimentoId"]).val();
                lista.push(form1);

                $('#SolicitacaoExameItensTable-' + localStorage["AtendimentoId"]).jtable('addRecord', {
                    record: form1,
                    clientOnly: true,
                });
            });

            $('#itens-' + localStorage["AtendimentoId"]).val(JSON.stringify(lista));

            $('#cbo-kits-' + localStorage["AtendimentoId"]).val(null).trigger("change");
        }

        getSolicitacaoItem();
        CamposRequeridos();
        aplicarDateSingle();
        aplicarDateRange();
    };
})(jQuery);