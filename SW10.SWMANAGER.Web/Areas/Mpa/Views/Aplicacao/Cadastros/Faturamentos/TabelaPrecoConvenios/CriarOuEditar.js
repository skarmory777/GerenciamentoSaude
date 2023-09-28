(function () {
    const configConvenioAppService = abp.services.app.faturamentoConfigConvenio
    const configConvenioService = abp.services.app.faturamentoConfigConvenio;
    const planosService = abp.services.app.plano;
    const gridPlanos = $(".grid-planos")
    const gridConfigConvenio = $(".grid-config-convenio")
    const convenioId = $("#convenio-id").val();
    const gridPlanosOptions = criarTabPlano();
    const gridConfigConvenioOptions = criarTabConfigConvenio();
    
    $('body').addClass('page-sidebar-closed');
    $('.page-sidebar-menu').addClass('page-sidebar-menu-closed');

    loadTab('abaPlanos');
    
    $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        const tab = $(e.target).attr("href").replace("#", "");
        loadTab(tab);
    })

    function loadTab(tab) {
        switch (tab) {
            case 'abaPlanos': {
                gridPlanosOptions.render(gridPlanos[0]);
                break;
            }
            case 'abaHistoricoFinanceiro': {
                break;
            }
            case 'abaMoeda': {
                break;
            }
            case 'abaTabelasCobranca': {
                gridConfigConvenioOptions.render(gridConfigConvenio[0]);
                break;
            }
            case 'abaTaxas': {
                break;
            }
            case 'abaConfigCobranca': {
                break;
            }
        }
    }

    function criarTabPlano() {
        const planoPermissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Plano.Create'),
            enableEdit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Plano.Edit'),
            enableDelete: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Plano.Delete')
        };

        const createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Planos/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Planos/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarPlanoModal'
        });

        return AgGridHelper.createAgGrid('grid-faturamento-tabela-preco-convenios-plano', {
            columnDefs: defColumns(),
            rowSelection: 'multiple',
            rowMultiSelectWithClick: true,
            onSelectionChanged: onGridSelectChanged,
            [AgGridHelper.HOOKS.AFTER_CREATED](hookData) {
                setTimeout(() => {
                    hookData.gridOptions.api.sizeColumnsToFit()
                }, 100);
                
                /*gridPlanos.css('width', $('.tab-content').width());*/
            },
            data: {
                callback: planosService.listarPorConvenio,
                autoInitialLoad: true,
                enablePagination: true,
                getData() {
                    const gridPlanosData = baseFilterData();
                    return gridPlanosData;
                }
            },
            editarItem(data) {
                abp.ui.setBusy()
                createOrEditModal.open({ id: data.id, convenioId });
            }
        });

        $('#CreateNewPlanoButton').click(function (e) {
            e.preventDefault();
            abp.ui.setBusy()
            createOrEditModal.open({ id: null, convenioId });
        });

        function onGridSelectChanged() {

        }

        function baseFilterData() {
            return {
                filtro: convenioId
            }
        }
        function defColumns() {
            
            return [
                AgGridHelper.columns.action(planoPermissions, { width: 130, suppressSizeToFit: true}),
                AgGridHelper.columns.base('descricao', app.localize('Descricao'), { suppressSizeToFit: false }),
                AgGridHelper.columns.boolean('isDespesasAcompanhante', app.localize('IsDespesasAcompanhante'), { suppressSizeToFit: false }),
                AgGridHelper.columns.boolean('isValidadeCarteiraIndeterminada', app.localize('IsValidadeCarteiraIndeterminada'), { suppressSizeToFit: false }),
                AgGridHelper.columns.boolean('isPlanoEmpresa', app.localize('IsPlanoEmpresa'), { suppressSizeToFit: false })
            ];
        }
    }

    function criarTabConfigConvenio() {
        const planoPermissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Faturamento.Tabelas.Create'),
            enableEdit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Faturamento.Tabelas.Edit'),
            enableDelete: abp.auth.hasPermission('Pages.Tenant.Cadastros.Faturamento.Tabelas.Delete')
        };
        const gridInstance = AgGridHelper.createAgGrid('grid-faturamento-tabela-preco-convenios-plano', {
            columnDefs: defColumns(),
            rowSelection: 'multiple',
            rowMultiSelectWithClick: true,
            onSelectionChanged: onGridSelectChanged,
            [AgGridHelper.HOOKS.AFTER_CREATED](hookData) {

                hookData.gridOptions.api.sizeColumnsToFit()
                /*gridPlanos.css('width', $('.tab-content').width());*/
            },
            data: {
                callback: configConvenioService.listarPorConvenio2,
                autoInitialLoad: true,
                enablePagination: true,
                getData() {
                    const gridPlanosData = baseFilterData();
                    return gridPlanosData;
                }
            },
            editarItem(data) {
                abp.ui.setBusy()
                //createOrEditModal.open({ id: data.id, convenioId });
            }
        });

        criarForm()

        return gridInstance;

        

        function onGridSelectChanged() {

        }

        function baseFilterData() {
            return {
                filtro: convenioId,
                convenioId: convenioId,
                ativos: $('#filtro-ativos').is(':checked'),
                planoGlobal: $('#filtro-plano-global').is(':checked'),
                planoEspecifico: $('#filtro-plano-especifico').is(':checked'),
                planoTodos: $('#filtro-plano-todos').is(':checked'),
                grupo: $('#filtro-grupo').is(':checked'),
                grupoSubGrupo: $('#filtro-grupo-subgrupo').is(':checked'),
                grupoItem: $('#filtro-grupo-item').is(':checked'),
                grupoTodos: $('#filtro-grupo-todos').is(':checked')
            }
        }
        function defColumns() {
            return [
                AgGridHelper.columns.action(planoPermissions, { width: 130, suppressSizeToFit: true }),
                AgGridHelper.columns.dateTime('dataIncio', app.localize('DataInicio'), { suppressSizeToFit: false }),
                AgGridHelper.columns.dateTime('dataFim', app.localize('DataFim'), { suppressSizeToFit: false }),
                AgGridHelper.columns.base('empresa.nomeFantasia', app.localize('Empresa'), { suppressSizeToFit: false }),
                AgGridHelper.columns.base('grupo.descricao', app.localize('Grupo'), { suppressSizeToFit: false }),
                AgGridHelper.columns.base('subGrupo.descricao', app.localize('SubGrupo'), { suppressSizeToFit: false }),
                AgGridHelper.columns.base('tabela.descricao', app.localize('Tabela'), { suppressSizeToFit: false }),
                AgGridHelper.columns.base('plano.descricao', app.localize('Plano'), { suppressSizeToFit: false }),
                AgGridHelper.columns.base('item.descricao', app.localize('Item'), { suppressSizeToFit: false })
            ];
        }


        function criarForm() {
            selectSWWithDefaultValue('.selectEmpresa', "/api/services/app/empresa/listarDropdownPorUsuario");
            selectSWWithDefaultValue('.selectPlano', "/api/services/app/plano/listarPorConvenioDropdown");
            selectSWWithDefaultValue('.selectTabela', "/api/services/app/faturamentoGlobal/listarDropdown");
            selectSWWithDefaultValue('.selectGrupo', "/api/services/app/faturamentoGrupo/listarDropdown");
            selectSWWithDefaultValue('.selectSubGrupo', "/api/services/app/faturamentoSubGrupo/listarParaGrupoObrigatorio", $('.selectGrupo'));
            selectSWWithDefaultValue('.selectItem', "/api/services/app/faturamentoItem/listarDropdownTodos");
            $("#filtro-ativos").attr("checked", "checked");
            aplicarDateSingle();

            $('#CreateNewPlanoButton').click(function (e) {
                e.preventDefault();
                abp.ui.setBusy()
                //createOrEditModal.open({ id: null, convenioId });
            });

            $("#filtro-ativos").change(() => {
                //const el = $(this);
                //if (el.is(":checked")) {
                //    el.removeAttr("checked")
                //}else {
                //    el.attr("checked","checked")
                //}
                gridInstance.render(gridConfigConvenio[0]);
            })

            $(".filtro-plano").change(function() {
                $(".filtro-plano").not($(this)).removeAttr("checked");
                gridInstance.render(gridConfigConvenio[0]);
            })

            $(".filtro-grupo").change(function () {
                $(".filtro-grupo").not($(this)).removeAttr("checked");
                gridInstance.render(gridConfigConvenio[0]);
            })

            $('#btn-save-tabela-cobranca').on('click', onSaveTabelaCobranca);
            $('#btn-limpar-tabela-cobranca').on('click', onLimparTabelaCobranca);
        }

        function onLimparTabelaCobranca() {
            resetarForm();
        }

        function resetarForm() {
            $('#titulo-config').html('Novo Registro');
            $("input[name='DataIncio']").val(moment(new Date()).format('L'));
            $("input[name='DataFim']").val('');
            $('#empresa-id').trigger("select2:selectById",0)
            $('#plano-id').trigger("select2:selectById", 0)
            $('#tabela-id').trigger("select2:selectById", 0)
            $('#grupo-id').trigger("select2:selectById", 0)
            $('#sub-grupo-id').trigger("select2:selectById", 0)
            $('#item-id').trigger("select2:selectById", 0)
            $('#icone-btn-salvar').removeClass('glyphicon glyphicon-edit');
            $('#icone-btn-salvar').addClass('fa fa-plus');
            $('#btn-apagar-config').fadeOut();

        }

        function deleteTabelas(tabela) {
            abp.message.confirm(
                app.localize('DeleteWarning', tabela.Nome),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _ConfigsService.excluir(tabela)
                            .done(function () {
                                getTabelas(true);
                                abp.notify.success(app.localize('SuccessfullyDeleted'));
                            });
                    }
                }
            );
        }



        function onSaveTabelaCobranca() {
            var inserirTodosGrupos = $('#chk-inserir-todos').is(':checked')
            var configConvenioDto = {
                EmpresaId: $('#empresa-id').val(),
                ConvenioId: $('#convenio-id').val(),
                PlanoId: $('#plano-id').val(),
                GrupoId: $('#grupo-id').val(),
                SubGrupoId: $('#sub-grupo-id').val(),
                TabelaId: $('#tabela-id').val(),
                ItemId: $('#item-id').val(),
                DataIncio: $("input[name='DataIncio']").val(),
                DataFim: $("input[name='DataFim']").val(),
                Id: $('#tabelaCobrancaId').val(),
                Codigo: $('#codigoTabela').val()
            }

            abp.services.app.faturamentoConfigConvenio.verificarDuplicata(configConvenioDto)
                .done(function (result) {
                    if (result > 0) {
                        abp.notify.warn(app.localize('ConfiguracaoDuplicada'));
                        return;
                    } else {
                        if (inserirTodosGrupos == true) {
                            abp.services.app.faturamentoConfigConvenio.criarTodosGrupos(configConvenioDto)
                                .done(function () {
                                    abp.notify.info(app.localize('SavedSuccessfully'));
                                    resetarForm();
                                });
                            getTabelas();
                            return;
                        }

                        abp.services.app.faturamentoConfigConvenio.criarOuEditar(configConvenioDto)
                            .done(function () {
                                abp.notify.info(app.localize('SavedSuccessfully'));
                                gridInstance.render(gridConfigConvenio[0]);
                            });
                    }
                });
        }

    }



})();