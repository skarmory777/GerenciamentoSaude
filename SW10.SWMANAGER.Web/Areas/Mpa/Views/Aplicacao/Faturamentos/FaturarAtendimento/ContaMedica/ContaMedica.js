(function () {
    $(function () {
        const contaAppService = abp.services.app.conta;
        const contaItemAppService = abp.services.app.faturamentoContaItem;
        const fatItemAppService = abp.services.app.faturamentoItem;
        const fatContaKitAppService = abp.services.app.faturamentoContaKit;
        const fatContaPacoteAppService = abp.services.app.faturamentoPacote;
        const ocorrenciaAppService = abp.services.app.ocorrencia;
        const summerNoteOptions = {
            toolbar: [
                ['printSize', ['printSize']],
                ['style', ['bold', 'italic', 'underline']],
                ['fontsize', ['fontsize']],
                ['fontname', ['fontname']],
                ['font', ['font', 'strikethrough', 'superscript', 'subscript']],
                ['color', ['color']],
                ['para', ['ul', 'ol', 'paragraph']],
                ['height', ['height']],
                ['misc', ['codeview', 'fullscreen']],
                ['table', ['table']]
            ],
            width: '100%',
            height: 80,
            padding: 0,
            disableResizeEditor: true
        };
        $.summernote.options.lineHeights = ["0", "0.2", "0.4", "0.6", "0.8", "1.0"];
        const historicoItemModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/FaturarAtendimento/historicoItemModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Faturamentos/FaturarAtendimento/ContaMedica/historicoItemModal.js',
            modalClass: 'historicoItemModal'
        });
        
        const criarOuEditarKitModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/FaturarAtendimento/CriarOuEditarKitModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Faturamentos/FaturarAtendimento/ContaMedica/Kit/CriarOuEditarKitModal.js',
            modalClass: 'CriarOuEditarKitModal'
        });

        const criarOuEditarPacoteModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/FaturarAtendimento/CriarOuEditarPacoteModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Faturamentos/FaturarAtendimento/ContaMedica/Pacote/CriarOuEditarPacoteModal.js',
            modalClass: 'CriarOuEditarPacoteModal'
        });

        jQuery.validator.addMethod("greaterThanZero", function(value, element) {
            return this.optional(element) || (parseFloat(value) > 0);
        }, "O valor precisa ser maior que 0.");
        
        $('.text-editor').summernote(summerNoteOptions);
        numeral.locale('pt-br');
        $('body').addClass('page-sidebar-closed');
        
        $('.page-sidebar-menu').addClass('page-sidebar-menu-closed');
        const formContaItem = $("#formContaItem");
        const formContaKit = $("#formContaKit");
        const formContaPacote = $("#formContaPacote");
        const btnExcluirItem = $('.btnExcluirItem')
        const btnlimparItem = $('.btnlimparItem')
        const btnEditarItem = $('.btnEditarItem')
        const btnAdicionarItem = $('.btnAdicionarItem')
        const btnAdicionarKit = $('.btnAdicionarKit');
        const btnAdicionarPacote = $('.btnAdicionarPacote');
        
        const gridOcorrencia = $('.grid-ocorrencia');
        const gridItems = $('.grid-items');
        const gridKit = $('.grid-kit');
        const gridPacote = $('.grid-pacote');


        const _selectedDateRangeContaItem = {
            startDate: undefined,
            endDate: undefined,
            autoUpdateInput: false,
        };
        
        function getContaMedicaId() {
            return $("#contaMedicaId").val() || 0
        }
        
        const gridItemsOptions = AgGridHelper.createAgGrid('grid-items-conta-medica', {
            columnDefs: defColumnsItems(),
            rowSelection: 'multiple',
            rowMultiSelectWithClick: true,
            onSelectionChanged:onGridItemsSelectChanged,
            onRowDoubleClicked:onGridItemsRowDoubleClicked,
            [AgGridHelper.HOOKS.BEFORE_CREATED](hookData) {
                gridItems.css('width',$('.portlet.light').width());
            },
            
            data: {
                callback:contaAppService.listarItems,
                autoInitialLoad: true,
                enablePagination: true,
                getData() {
                    const gridItemsData = baseFilterItemsData();
                    gridItemsData.contaMedicaId = getContaMedicaId()
                    return gridItemsData;
                }    
            }
        });
        
        gridItemsOptions.render(gridItems[0]);
        
        const gridKitOptions = AgGridHelper.createAgGrid('grid-kit-conta-medica', {
            columnDefs: defColumnsKits(),
            rowSelection: 'single',
            onSelectionChanged: onGridKitSelectChanged,
            data:{
              callback: fatContaKitAppService.listarParaContaMedica,
                getData() {
                    return {contaMedicaId: getContaMedicaId()}
                }  
            },
            [AgGridHelper.HOOKS.BEFORE_CREATED](hookData) {
                gridKit.css('width',$('.portlet.light').width());
            }
        });
        gridKitOptions.render(gridKit[0]);
        
        const gridPacoteOptions = AgGridHelper.createAgGrid('grid-pacote-conta-medica', {
            columnDefs: defColumnsPacote(),
            onSelectionChanged: onGridPacoteSelectedChanged,
            rowSelection: 'single',
            data:{
                callback: fatContaPacoteAppService.listarPacotesPorContaMedica,
                getData() {
                    return {contaMedicaId: getContaMedicaId()}
                }
            },
            [AgGridHelper.HOOKS.BEFORE_CREATED](hookData) {
                gridPacote.css('width',$('.portlet.light').width());
            }
        });
        gridPacoteOptions.render(gridPacote[0]);

        const gridOcorrenciasOptions = AgGridHelper.createAgGrid('grid-ocorrencias-conta-medica', {
            gridName: 'grid-ocorrencias',
            columnDefs: defColumnsOcorrencias(),
            data: {
                callback: ocorrenciaAppService.listar,
                enablePagination: true,
                autoInitialLoad: true,
                getData() {
                    return {
                        sourceModel: "contamedica",
                        sourceId: getContaMedicaId(),
                        relationModel: "contamedica",
                        relationId: getContaMedicaId()
                    }
                },
                [AgGridHelper.HOOKS.BEFORE_CREATED](hookData) {
                    gridOcorrencia.css('width', $('.portlet.light').width());
                },
            }
        });

        gridOcorrenciasOptions.render(gridOcorrencia[0]);
        
        carregaPagina();
        
        function defColumnsItems() {
            const disableFilterAndMenu = {filter: false,  suppressMenu: true};
            return [
                AgGridHelper.columns.checkboxSelection(disableFilterAndMenu),
                //AgGridHelper.columns.action({customAction: [{
                //    title: 'Historico Item',
                //    action: onHistoricoItem,
                //    icon: 'fas fa-history',
                //    class:'btn-info'
                //}]}, {width: 90}),
                AgGridHelper.columns.dateTime('data',app.localize('Data'), disableFilterAndMenu),
                AgGridHelper.columns.base('tipoGrupoDescricao',app.localize('Tipo'), disableFilterAndMenu),
                AgGridHelper.columns.base('grupoCodigo',app.localize('Grupo'), disableFilterAndMenu, {tooltipField:'grupoDescricao' }),
                AgGridHelper.columns.base('itemDescricao',app.localize('Item'), disableFilterAndMenu, {width:200,suppressSizeToFit:true,}),
                AgGridHelper.columns.base('qtde', app.localize('Qtde'), disableFilterAndMenu),
                AgGridHelper.columns.money('valorItem', app.localize('Valor'), disableFilterAndMenu),
                AgGridHelper.columns.base('turnoCodigo',app.localize('Turno'), disableFilterAndMenu, {tooltipField:'turnoDescricao' }),
                AgGridHelper.columns.base('centroCustoCodigo',app.localize('C.C'), disableFilterAndMenu, {tooltipField:'centroCustoDescricao' }),
                AgGridHelper.columns.base('unidadeOrganizacionalDesricao',app.localize('Local'), disableFilterAndMenu),
                AgGridHelper.columns.base('terceirizadoCodigo',app.localize('Terc'), disableFilterAndMenu, {tooltipField:'terceirizadoDescricao' }),
                AgGridHelper.columns.base('leitoDescricao',app.localize('Leito'), disableFilterAndMenu),
                AgGridHelper.columns.base('lote',app.localize('Lote'), disableFilterAndMenu),
                AgGridHelper.columns.base('demostrativoRecebimento',app.localize('Dem. Receb.'), disableFilterAndMenu),
                AgGridHelper.columns.base('repasseRecebimento',app.localize('Rep. Receb.'), disableFilterAndMenu),
                AgGridHelper.columns.base('demostrativoRecuperado', app.localize('Dem. Recup.'), disableFilterAndMenu),
                AgGridHelper.columns.base('faturamentoPacoteItemDescricao', app.localize('Pacote'), disableFilterAndMenu),
                AgGridHelper.columns.base('kitDescricao',app.localize('Kit'), disableFilterAndMenu),
                AgGridHelper.columns.dateTime('dataUltimoPagamento',app.localize('Último Rateio'), disableFilterAndMenu),
                
                
                //AgGridHelper.columns.base('qtdFatura',app.localize('Leito'), disableFilterAndMenu),
            ];
        }
        function defColumnsOcorrencias() {
            const disableFilterAndMenu = {filter: false,  suppressMenu: true};
            return [
                AgGridHelper.columns.action(),
                AgGridHelper.columns.dateTime('data',app.localize('Data'), disableFilterAndMenu),
                AgGridHelper.columns.base('tipoOcorrenciaDescricao',app.localize('Tipo'),  disableFilterAndMenu),
                AgGridHelper.columns.base('subTipoOcorrenciaDescricao',app.localize('Sub Tipo'),  disableFilterAndMenu),
                AgGridHelper.columns.base('origem',app.localize('Origem'),  disableFilterAndMenu),
                AgGridHelper.columns.base('texto',app.localize('Ocorrencia'),  disableFilterAndMenu),
                AgGridHelper.columns.base('usuario',app.localize('Usuario'),  disableFilterAndMenu),
                AgGridHelper.columns.boolean('isSistema',app.localize('Sistema?'),  disableFilterAndMenu),
            ];
        }
        function defColumnsKits() {
            const disableFilterAndMenu = {filter: false,  suppressMenu: true};
            return [
                AgGridHelper.columns.action(),
                AgGridHelper.columns.dateTime('data', app.localize('Data'), disableFilterAndMenu),
                AgGridHelper.columns.base('codigo', app.localize('Codigo'), disableFilterAndMenu),
                AgGridHelper.columns.base('faturamentoKit', app.localize('Kit'), disableFilterAndMenu),
                AgGridHelper.columns.base('qtde', app.localize('Qtde'), disableFilterAndMenu),
                AgGridHelper.columns.base('usuario', app.localize('Usuario'), disableFilterAndMenu),
            ]
        }
        function defColumnsPacote() {
            const disableFilterAndMenu = {filter: false,  suppressMenu: true};
            return [
                AgGridHelper.columns.checkboxSelection(disableFilterAndMenu),
                AgGridHelper.columns.dateTime('dataInicio', app.localize('Data Inicial'), disableFilterAndMenu),
                AgGridHelper.columns.dateTime('dataFinal', app.localize('Data Final'), disableFilterAndMenu),
                AgGridHelper.columns.base('faturamentoItemCodigo', app.localize('Codigo'), disableFilterAndMenu),
                AgGridHelper.columns.base('faturamentoItemDescricao', app.localize('Pacote'), disableFilterAndMenu),
                AgGridHelper.columns.base('qtde', app.localize('Qtde'), disableFilterAndMenu),
                AgGridHelper.columns.integer('totalItensNoPacote', app.localize('Total Itens'), disableFilterAndMenu),
            ]
        }
        
        function onHistoricoItem() {
            historicoItemModal.open();
        }
        
        function updateSmartwizardHeight() {
            $(".tab-content-wizard").css("height","100%");
            $("#smartwizard").smartWizard("fixHeight");
        }

        function onGridKitSelectChanged() {
            debugger;
            if (contaBloqueada()) {
                $(".btn-remove-kit").hide();
                abp.notify.warn("Não é possível alterar uma conta conferida. Favor voltar o status para altera-la");
                return;
            }
            if (gridKitOptions.getSelectedRows().length == 0) {
                $(".btn-remove-kit").hide();
                return;
            }

            $(".btn-remove-kit").show();
            //var row = gridItemsOptions.getSelectedRows()[0];


        }
        
        // TODO: Regra para ver quais itens podem ser agrupados.
        //TODO: melhorar esse codigo.
        function onGridItemsSelectChanged(event) {

            if (contaBloqueada()) {
                $(".btn-add-item-a-pacote").hide();
                $(".btn-remove-item-a-pacote").hide();
                $(".btn-remove-item-kit").hide();
                $(".btn-remove-item").hide();
                abp.notify.warn("Não é possível alterar uma conta conferida. Favor voltar o status para altera-la");
                return;
            }

            if (gridItemsOptions.getSelectedRows().length > 1) {
                if (_.every(gridItemsOptions.getSelectedRows(), (x) => x.faturamentoPacoteId == null)) {
                    $(".btn-add-item-a-itens").show();
                } else {
                    $(".btn-add-item-a-itens").hide();
                }

                if (_.every(gridItemsOptions.getSelectedRows(), (x) => x.faturamentokitId == null)) {
                    $(".btn-remove-item-kit").hide();
                } else {
                    $(".btn-remove-item-kit").show();
                    //$(".btn-add-item-a-pacote").hide();
                    //$(".btn-empacotar-itens").hide();
                }
                $(".btn-remove-item").show();

            } else if(gridItemsOptions.getSelectedRows().length == 1) {
                if (_.every(gridItemsOptions.getSelectedRows(), (x) => x.faturamentoPacoteId == null)) {
                    $(".btn-add-item-a-pacote").show();
                    $(".btn-remove-item-a-pacote").hide();
                } else {
                    $(".btn-add-item-a-pacote").hide();
                    $(".btn-remove-item-a-pacote").show();
                }

                if (_.every(gridItemsOptions.getSelectedRows(), (x) => x.faturamentokitId == null)) {
                    $(".btn-remove-item-kit").hide();
                } else {
                    $(".btn-remove-item-kit").show();
                    //$(".btn-add-item-a-pacote").hide();
                    //$(".btn-empacotar-itens").hide();
                }
                $(".btn-remove-item").show();

            } else {
                /*$(".btn-empacotar-itens").hide();*/
                $(".btn-add-item-a-pacote").hide();
                $(".btn-remove-item-a-pacote").hide();
                $(".btn-remove-item-kit").hide();
                $(".btn-remove-item").hide();
            }
        }

        function onGridPacoteSelectedChanged(event) {
            $(".btn-remove-pacote").hide();
            if (contaBloqueada()) {
                abp.notify.warn("Não é possível alterar uma conta conferida. Favor voltar o status para altera-la");
                return;
            }

            if (gridPacoteOptions.getSelectedRows().length == 1) {
                $(".btn-remove-pacote").show();
            }

        }
        
        function onGridItemsRowDoubleClicked(row) {
            if (contaBloqueada()) {
                abp.notify.warn("Não é possível alterar uma conta conferida. Favor voltar o status para altera-la");
                return;
            }
            if(row.data.id != formContaItem.find("#formContaItemId").val()) {
                editarGridItem(row.data);
                gridItemsOptions.getApi().ensureNodeVisible(row.node,'top');
                gridItemsOptions.getApi().selectNode(row.node,true);
            } else {
                abp.event.trigger("FaturamentoContaMedicaItem::LimparItemParaNovo");
                $("#btnNovoItem").trigger("click");
                
                
            }
        }
        
        function editarGridItem(row) {
            abp.ui.block();
            contaItemAppService.obter(row.id).then(res=> {
                // Aba Principal
                formContaItem.find("#formContaItemId").val(res.id);
                debugger;
                const datePickerData = formContaItem.find("#formContaItemData").data()
                formContaItem.find("#formContaItemData").val(res.data);
                formContaItem.find("#formContaItemData").data("daterangepicker").callback(formContaItem.find("#formContaItemData").val())
                formContaItem.find("#formContaItemData").trigger("change");
                formContaItem.find("#formContaItemTuss").val(res.faturamentoItem.codTuss);
                formContaItem.find("#formContaItemSihSus").val(res.faturamentoItem.sihSus);
                formContaItem.find("#formContaItemCodBarra").val(res.faturamentoItem.codigo);
                formContaItem.find("#formContaItemFaturamentoItemId").val(res.faturamentoItemId).trigger("select2:selectById",res.faturamentoItemId);
                formContaItem.find("#formContaItemUnidadeOrganizacionalId").val(res.unidadeOrganizacionalId).trigger("select2:selectById",res.unidadeOrganizacionalId);
                formContaItem.find("#formContaItemTerceirizadoId").val(res.terceirizadoId).trigger("select2:selectById",res.terceirizadoId);
                formContaItem.find("#formContaItemCentroCustoId").val(res.centroCustoId).trigger("select2:selectById",res.centroCustoId);
                formContaItem.find("#formContaItemTurnoId").val(res.turnoId).trigger("select2:selectById",res.turnoId);
                formContaItem.find("#formContaItemTipoLeitoId").val(res.tipoLeitoId).trigger("select2:selectById",res.tipoLeitoId);
                formContaItem.find("#formContaItemQtde").val(res.qtde);
                formContaItem.find("#formContaItemValorItem").val(res.valorItem);
                //Aba Observacao
                formContaItem.find("#formContaItemObservacao").val(res.observacao);
                
                // Aba Honorarios
                // Medico
                formContaItem.find("#formContaItemMedicoId").val(res.medicoId).trigger("select2:selectById",res.medicoId);
                formContaItem.find("#formContaItemIsMedCredenciado").attr("checked",res.isMedCredenciado)
                formContaItem.find("#formContaItemMedicoEspecialidadeId").val(res.medicoEspecialidadeId).trigger("select2:selectById",res.medicoEspecialidadeId);

                // Auxiliar 1
                formContaItem.find("#formContaItemAuxiliar1Id").val(res.auxiliar1Id).trigger("select2:selectById",res.auxiliar1Id);
                formContaItem.find("#formContaItemIsAux1Credenciado").attr("checked",res.isAux1Credenciado)
                formContaItem.find("#formContaItemAuxiliar1EspecialidadeId").val(res.auxiliar1EspecialidadeId).trigger("select2:selectById",res.auxiliar1EspecialidadeId);

                // Auxiliar 2
                formContaItem.find("#formContaItemAuxiliar2Id").val(res.auxiliar2Id).trigger("select2:selectById",res.auxiliar2Id);
                formContaItem.find("#formContaItemIsAux2Credenciado").attr("checked",res.isAux2Credenciado)
                formContaItem.find("#formContaItemAuxiliar2EspecialidadeId").val(res.auxiliar2EspecialidadeId).trigger("select2:selectById",res.auxiliar2EspecialidadeId);

                // Auxiliar 3
                formContaItem.find("#formContaItemAuxiliar3Id").val(res.auxiliar3Id).trigger("select2:selectById",res.auxiliar3Id);
                formContaItem.find("#formContaItemIsAux3Credenciado").attr("checked",res.isAux3Credenciado)
                formContaItem.find("#formContaItemAuxiliar3EspecialidadeId").val(res.auxiliar3EspecialidadeId).trigger("select2:selectById",res.auxiliar3EspecialidadeId);

                // Instrumentador
                formContaItem.find("#formContaItemInstrumentadorId").val(res.instrumentadorId).trigger("select2:selectById",res.instrumentadorId);
                formContaItem.find("#formContaItemIsInstrCredenciado").attr("checked",res.isInstrCredenciado)
                formContaItem.find("#formContaItemInstrumentadorEspecialidadeId").val(res.instrumentadorEspecialidadeId).trigger("select2:selectById",res.instrumentadorEspecialidadeId);

                // Anestesista
                formContaItem.find("#formContaItemAnestesistaId").val(res.anestesistaId).trigger("select2:selectById",res.anestesistaId);
                formContaItem.find("#formContaItemIsAnestCredenciado").attr("checked",res.isAnestCredenciado)
                formContaItem.find("#formContaItemAuxiliar1EspecialidadeId").val(res.auxiliar1EspecialidadeId).trigger("select2:selectById",res.auxiliar1EspecialidadeId);
                
                //ProcedimentoPrincipal
                formContaItem.find("#formContaItemProcedimentoPrincipalId").val(res.faturamentoPacoteId).trigger("select2:selectById",res.faturamentoPacoteId);
                formContaItem.find("#formContaItemPercentualItem").val(res.percentual ?? 100);

                formContaItem.find(".btn-perc-item").removeClass("active");
                formContaItem.find(".btn-perc-item").find(`.btn-${formContaItem.find("#formContaItemPercentualItem").val()}-perc-item`).addClass("active");

                formContaItem.find("#formContaItemValorItem").attr("readonly", "readonly");
                if (res.faturamentoItem && res.faturamentoItem.isPrecoManual) {
                    formContaItem.find("#formContaItemValorItem").removeAttr("readonly");
                    abp.message.info("Este item permite valor manual.");
                }
                
                $("#btnNovoItem").trigger("click");
                btnEditarItem.show();
                btnAdicionarItem.hide();
                
            }).always(()=> {
                abp.ui.unblock();
            })
        }
        
        function createDatePicker() {
            const defaultProperties = {
                "singleDatePicker": true,
                "showDropdowns": true,
                autoUpdateInput: false,
                //maxDate: new Date(),
                changeYear: true,
                //yearRange: 'c-10:c+10',
                showOn: "both",
                "locale": {
                    "format": moment.locale().toUpperCase() === 'PT-BR' ? "DD/MM/YYYY" : moment.locale().toUpperCase() === 'US' ? "MM/DD/YYYY" : "YYYY-MM-DD",
                    "separator": " - ",
                    "applyLabel": "Apply",
                    "cancelLabel": "Cancel",
                    "fromLabel": "From",
                    "toLabel": "To",
                    "customRangeLabel": "Custom",
                    "daysOfWeek": [
                        app.localize('Dom'),
                        app.localize('Seg'),
                        app.localize('Ter'),
                        app.localize('Qua'),
                        app.localize('Qui'),
                        app.localize('Sex'),
                        app.localize('Sab')
                    ],
                    "monthNames": [
                        app.localize("Jan"),
                        app.localize("Fev"),
                        app.localize("Mar"),
                        app.localize("Abr"),
                        app.localize("Mai"),
                        app.localize("Jun"),
                        app.localize("Jul"),
                        app.localize("Ago"),
                        app.localize("Set"),
                        app.localize("Out"),
                        app.localize("Nov"),
                        app.localize("Dez"),
                    ],
                    "firstDay": 0
                }
            };
            $('.date-picker').each(function (index) {
                let obj = $(this);
                let extendProperties = {};
                if((obj).hasClass("time-picker")) {
                    extendProperties.timePicker = true;
                    extendProperties.locale = {
                        format :moment.locale().toUpperCase() === 'PT-BR' ? "DD/MM/YYYY HH:mm:ss" : moment.locale().toUpperCase() === 'US' ? "MM/DD/YYYY HH:mm:ss" : "YYYY-MM-DD HH:mm:ss"
                    }
                }

                if ((obj).data("defaultValue")) {
                    extendProperties.startDate = moment((obj).data("defaultValue"), extendProperties.locale.format, true);
                }


                obj.daterangepicker(_.extend({},defaultProperties, extendProperties),
                    function (start, end, label) {
                        obj.val(moment(start).format(extendProperties.locale.format));
                        obj.trigger('input');
                        obj.trigger('change');
                    })
                    .on('apply.daterangepicker', function (ev, picker) {
                        obj.val(moment(picker.startDate).format(extendProperties.locale.format));
                        obj.trigger('input');
                        obj.trigger('change');
                    })
                    .on('cancel.daterangepicker', function (ev, picker) {
                        obj.val('');
                    });

                if (extendProperties.startDate) {
                    obj.val(extendProperties.startDate.format(extendProperties.locale.format));
                }
            });
        }
        
        function onShowStep(e, anchorObject, stepIndex, stepDirection) {
            gridOcorrenciasOptions.render(gridOcorrencia[0]);
            
            switch($(anchorObject).attr("href")) {
                case "#nav-items": {
                    gridItemsOptions.render(gridItems[0]);
                    break;
                }
                case "#nav-kits": {
                    gridKitOptions.render(gridKit[0]);
                    break;
                }
                case "#nav-pacotes": {
                    gridPacoteOptions.render(gridPacote[0]);
                    break;
                }
                case "#nav-conta-final-aberta": {
                    debugger;
                    abp.event.trigger("ContaMedica::CarregaContaMedica", { tipo: "Aberta" });
                    break;
                }
                case "#nav-conta-final-fechada": {
                    debugger;
                    abp.event.trigger("ContaMedica::CarregaContaMedica", { tipo: "Fechada" });
                    break;
                }
            }
        }

        function baseFilterItemsData() {
            return  {
                dataInicial: _selectedDateRangeContaItem != null && _selectedDateRangeContaItem.startDate ? _selectedDateRangeContaItem.startDate : null,
                dataFinal: _selectedDateRangeContaItem != null && _selectedDateRangeContaItem.endDate ? _selectedDateRangeContaItem.endDate : null,
                fatKitId: $("#formContaItemFaturamentoFilterKits").val() != 0 ? $("#formContaItemFaturamentoFilterKits").val() : null,
                fatContaKitId: $("#formContaItemFaturamentoFilterKitId").val() != 0 ? $("#formContaItemFaturamentoFilterKitId").val() : null,
                FatPacoteId: $("#formContaItemFaturamentoFilterPacotes").val() != 0 ? $("#formContaItemFaturamentoFilterPacotes").val() : null,
                FatContaPacoteId: $("#formContaItemFaturamentoFilterPacoteId").val() != 0 ? $("#formContaItemFaturamentoFilterPacoteId").val() : null
            }
        }
        
        function criarContaItem() {
            criarContaItemFilter();
            formContaItem.attr("autocomplete","off");
            selectSWWithDefaultValue(formContaItem.find(".selectItem"), '/api/services/app/faturamentoItem/ListarDropdownContaMedica', [$("#convenioId"), formContaItem.find("#formContaItemData"), $("#planoId"), $("#empresaId")]);
            selectSWWithDefaultValue(formContaItem.find(".selectlocalUtilizacao"), '/api/services/app/UnidadeOrganizacional/ListarDropdown');
            selectSWWithDefaultValue(formContaItem.find(".selectTerceirizado"),"/api/services/app/terceirizado/ListarDropdown");
            selectSWWithDefaultValue(formContaItem.find(".selectCentroDeCusto"), '/api/services/app/CentroCusto/ListarDropdownCodigoCentroCusto');
            selectSWWithDefaultValue(formContaItem.find(".selectTurno"), "/api/services/app/Turno/ListarDropdown");
            selectSWWithDefaultValue(formContaItem.find(".selectTipoAcomodacao"), "/api/services/app/TipoAcomodacao/ListarDropdown");

            selectSWWithDefaultValue($("#formContaItemFaturamentoFilterPacotes"), '/api/services/app/faturamentoPacote/listarDropdownPacoteContaMedica', getContaMedicaId());
            selectSWWithDefaultValue($("#formContaItemFaturamentoFilterPacoteId"), '/api/services/app/faturamentoPacote/ListarDropdownPacoteContaMedicaPorPacote', [getContaMedicaId(), $("#formContaItemFaturamentoFilterPacotes")]);

            selectSWWithDefaultValue($("#formContaItemFaturamentoFilterKits"), '/api/services/app/faturamentoKit/listarDropdownKitContaMedica', getContaMedicaId());
            selectSWWithDefaultValue($("#formContaItemFaturamentoFilterKitId"), '/api/services/app/faturamentoKit/ListarDropdownKitContaMedicaPorKit', [getContaMedicaId(), $("#formContaItemFaturamentoFilterKits")]);

            formContaItem.find(".selectEspecialidade").each(function(){
                const select = $(this);
                selectSWWithDefaultValue(formContaItem.find(`[name='${select.attr("name")}']`), "/api/services/app/medicoEspecialidade/ListarDropdownPorMedico", select.parents(".row:first").find(".selectMedico"))
            })

            selectSWWithDefaultValue(formContaItem.find(".selectMedico"), "/api/services/app/medico/ListarDropdown")

            selectSWWithDefaultValue(formContaItem.find(".procedimentoPrincipal"), '/api/services/app/faturamentoContaItem/ListarDropdown')
            $('.collapseAddItem').on('hidden.bs.collapse', updateSmartwizardHeight).on('shown.bs.collapse', updateSmartwizardHeight)
            formContaItem.find(".selectItem").change(onChangeContaItem);
            btnAdicionarItem.on('click', onbtnAdicionarItemClick);
            btnlimparItem.on('click', onBtnlimparItemClick);
            btnEditarItem.on('click', onBtnEditarItemClick);
            //btnExcluirItem.on('click', onBtnExcluirItemClick);
            $(".btn-remove-item").on('click', onBtnExcluirItemsClick);
            $(".btn-add-item-a-pacote").on('click', onBtnAddItensPacoteClick);
            $(".btn-remove-item-a-pacote").on('click', onBtnRemoveItensPacoteClick);
            $(".btn-remove-item-kit").on('click', onBtnRemoveItensKitClick);

            formContaItem.find("#formContaItemData").change(() => {calcularValorTotalItemFaturamento()});
            formContaItem.find("#formContaItemQtde").change(() => { calcularValorTotalItemFaturamento() });
            formContaItem.find("#formContaItemUnidadeOrganizacionalId").change(() => { calcularValorTotalItemFaturamento() });
            formContaItem.find("#formContaItemTerceirizadoId").change(() => { calcularValorTotalItemFaturamento() });
            formContaItem.find("#formContaItemTurnoId").change(() => { calcularValorTotalItemFaturamento() });
            formContaItem.find("#formContaItemTipoLeitoId").change(() => { calcularValorTotalItemFaturamento() });
            formContaItem.find("#formContaItemPercentualItem").change(() => { calcularValorTotalItemFaturamento() });

            formContaItem.find(".busca-codigo").on('keypress', (event) => {
                console.log(event.which);
                if (event.which !== 13) {
                    return;
                }
                abp.ui.setBusy();
                const el = $(event.target);
                const data = {};
                data[el.data("codigo")] = el.val();
                fatItemAppService.obterPorCodigo(data).then(res => {

                    if (res == null || res.length == 0) {
                        formContaItem.find(".selectItem").val(null).trigger('change');
                        abp.notify.error("Não foi possivel encontrar o item");
                    } else if (res.length == 1) {
                        if (res[0] && res[0].id != 0) {
                            formContaItem.find(".selectItem").trigger("select2:selectById", res[0].id);
                            abp.notify.info("Item encontrado e selecionado.");

                        }
                    }
                }).always(() => {
                    abp.ui.clearBusy();
                })
            })


            formContaItem.find("#formContaItemPercentualItem").val(100);

            formContaItem.find(".btn-perc-item").removeClass("btn-primary");
            formContaItem.find(`#btn-${formContaItem.find("#formContaItemPercentualItem").val()}-perc-item`).addClass("btn-primary");

            formContaItem.find(".btn-perc-item").on("click", (event) => {
                const self = $(event.currentTarget);
                formContaItem.find(".btn-perc-item").removeClass("btn-primary");
                self.addClass("btn-primary");
                formContaItem.find("#formContaItemPercentualItem").val(self.data("value"));
                formContaItem.find(".valor-perc").text((Number(formContaItem.find("#formContaItemPercentualItem").val())/ 100).toLocaleString('pt-br',{style: 'percent'}));
                calcularValorTotalItemFaturamento();
            })
            
            abp.event.on(("FaturamentoContaMedicaItem::LimparItemParaNovo"), () => {
                onLimparItemParaNovo();
            })
            
            function criarContaItemFilter() {
                const pickerOptions = app.createDateRangePickerOptions();
                $('#formContaItemFaturamentoFilterData').daterangepicker(
                    $.extend(true, pickerOptions, _selectedDateRangeContaItem),
                    function (start, end, label) {

                        _selectedDateRangeContaItem.startDate = start.format('YYYY-MM-DDT00:00:00Z');
                        _selectedDateRangeContaItem.endDate = end.format('YYYY-MM-DDT23:59:59.999Z');

                        //gridItemsOptions.refresh();
                    })
                    .on('apply.daterangepicker', function (ev, picker) {
                        $(this).val(picker.startDate.format('DD/MM/YYYY') + ' - ' + picker.endDate.format('DD/MM/YYYY'));
                    }).on('cancel.daterangepicker', function (ev, picker) {
                        _selectedDateRangeContaItem.startDate = undefined;
                        _selectedDateRangeContaItem.endDate = undefined;
                        $(this).val('');
                    });

                $(".btn-filtrar-items").on("click", (event) => {
                    const btn = $(this);

                    btn.buttonBusy(true);
                    gridItemsOptions.refresh();
                    setTimeout(() => {
                        btn.buttonBusy(false)
                    }, 300);

                })
            }

            function onbtnAdicionarItemClick(event) {
                const btn = $(this)
                btn.buttonBusy(true);

                formContaItem.validate();
                if(formContaItem.valid()) {
                    const data = formContaItem.serializeFormToObject();
                    data.faturamentoContaId = $("#contaMedicaId").val();
                    if(data.resumoDetalhamento) {
                        data.resumoDetalhamento = JSON.parse(data.resumoDetalhamento);
                    } 
                    
                    const momentData = moment(data.data,"DD/MM/YYYY HH:mm:ss",true);
                    if(momentData.isValid()) {
                        data.data = momentData.format();
                    }
                    contaItemAppService.criarOuEditar(data).then(res => {
                        abp.notify.success("Item da conta adicionado com sucesso");
                        gridItemsOptions.render(gridItems[0])
                        onLimparItemParaNovo();
                    }).always(() => {
                        btn.buttonBusy(false);
                    })
                } else {
                    btn.buttonBusy(false);
                }
            }
            
            function onLimparItemParaNovo() {
                formContaItem.find("#formContaItemId").val('');
                formContaItem.find("#formContaItemCodBarra").val('');
                formContaItem.find("#formContaItemFaturamentoItemId").val("").trigger("change");
                formContaItem.find("#formContaItemQtde").val("1").trigger("change");
                formContaItem.find(".selectMedico").each(function() {
                    const select2 = $(this);
                    select2.val("").trigger("change");
                })
                formContaItem.find(".checkboxCredenciado").removeAttr("checked");
                formContaItem.find(".selectEspecialidade").each(function() {
                    const select2 = $(this);
                    select2.val("").trigger("change");
                })
                formContaItem.find("#formContaItemProcedimentoPrincipalId").val("").trigger("change");
                formContaItem.find('#formContaItemObservacao').summernote('reset');
                formContaItem.find("#formContaItemValorItem").val('');
                
                formContaItem.find("#formContaItemResumoDetalhamento").val(JSON.stringify(null));
                
                formContaItem.find("#formContaItemPercentualItem").val(100);

                formContaItem.find(".btn-perc-item").removeClass("btn-primary");
                formContaItem.find(".btn-perc-item").find(`.btn-100-perc-item`).addClass("btn-primary");

                atualizaCardsValor(null);
            }

            function onBtnlimparItemClick(event) {
                formContaItem.trigger("reset");
                formContaItem.find(".select2").val("").trigger("change");
                formContaItem.find("#formContaItemQtde").val("1").trigger("change");
                btnEditarItem.hide();
                btnAdicionarItem.show();
            }

            function onBtnEditarItemClick(event) {
                const btn = $(this)
                btn.buttonBusy(true);

                formContaItem.validate();
                if(formContaItem.valid()) {
                    const data = formContaItem.serializeFormToObject();
                    data.faturamentoContaId = $("#contaMedicaId").val();
                    if(data.resumoDetalhamento) {
                        data.resumoDetalhamento = JSON.parse(data.resumoDetalhamento);
                    }

                    const momentData = moment(data.data,"DD/MM/YYYY HH:mm:ss",true);
                    if(momentData.isValid()) {
                        data.data = momentData.format();
                    }
                    
                    contaItemAppService.criarOuEditar(data).then(res => {
                        abp.notify.success("Item da conta editado com sucesso");
                        gridItemsOptions.render(gridItems[0]);
                        onBtnlimparItemClick();
                        btn.buttonBusy(false);
                    }).always(() => {
                        btn.buttonBusy(false);
                    })
                }
            }

            function onBtnExcluirItemsClick(event) {
                const btn = $(this)
                btn.buttonBusy(true);
                let mensagem;
                let mensagemSucesso;
                const rows = gridItemsOptions.getSelectedRows();
                if (rows.length == 1) {
                    mensagem = "Deseja excluir o item selecionado da conta?";
                    mensagemSucesso = "Item da conta excluído com sucesso";
                } else if (rows.length > 1) {
                    mensagem = "Deseja excluir os items selecionado da conta?";
                    mensagemSucesso = "Items da conta excluídos com sucesso";
                }

                abp.message.confirm(mensagem, "Exclusão", (confirm)=>{
                    if(confirm) {
                        const data = {
                            contaMedicaId: getContaMedicaId(),
                            items: gridItemsOptions.getSelectedRows().map(x => x.id)
                        };
                        contaItemAppService.excluirItens(data.contaMedicaId, data.items).then(res => {
                            abp.notify.success(mensagemSucesso);
                            gridItemsOptions.refresh();
                            btn.buttonBusy(false);
                        }).always(() => {
                            btn.buttonBusy(false);
                        })
                    } else {
                        btn.buttonBusy(false);
                    }
                })
            }

            function onBtnAddItensPacoteClick(event) {
                const btn = $(this)
                btn.buttonBusy(true);
                let mensagem;
                let mensagemSucesso;
                const rows = gridItemsOptions.getSelectedRows();
                if (rows.length == 1) {
                    mensagem = "Deseja incluir o item selecionado em um pacote já existente ou criar um pacote a partir desse item?";
                    mensagemSucesso = "Item da conta empacotado com sucesso";
                } else if (rows.length > 1) {
                    mensagem = "Deseja incluir os itens selecionados em um pacote já existente ou criar um pacote a partir desses itens?";
                    mensagemSucesso = "Itens da conta empacotados com sucesso";
                }

                customConfirmModalHelper.CreateModal({
                    title: "Empacotar",
                    message: mensagem,
                    icon: "fas fa-exclamation-triangle text-info",
                    buttons: [
                        //customConfirmModalHelper.CreateButton("Cancelar", "btn btn-danger", null, (event, confirmModalInstance) => {
                        //    console.log(confirmModalInstance);
                        //}),
                        customConfirmModalHelper.CreateButton("Pacote Existente", "btn btn-default", null, (event, confirmModalInstance) => {
                            const btnPacote = $(event.target);
                            btnPacote.buttonBusy(true);
                            console.log(confirmModalInstance);
                        }),
                        customConfirmModalHelper.CreateButton("Novo Pacote", "btn btn-default", null, (event, confirmModalInstance) => {
                            const btnPacote = $(event.target);
                            btnPacote.buttonBusy(true);
                            console.log(confirmModalInstance);
                        })
                    ],
                    styles: {
                        "modal-dialog": { 'min-width': '500px' }
                    },
                    confirmModalOptions:{
                        cancelButton: {
                            enable: true,
                            callback: (event, confirmModalInstance) => {
                                btn.buttonBusy(false);
                                confirmModalInstance.close()
                            }
                        },
                        onHideModal: (event, confirmModalInstance) => {
                            btn.buttonBusy(false);
                            confirmModalInstance.removeModal()
                        }
                    }
                });
                return;

                abp.message.confirm(mensagem, "Empacotar", (confirm) => {
                    //if (confirm) {
                    //    const data = {
                    //        contaMedicaId: getContaMedicaId(),
                    //        items = gridItemsOptions.getSelectedRows().map(x => x.id)
                    //    };
                    //    contaItemAppService.excluir(data).then(res => {
                    //        abp.notify.success(mensagemSucesso);
                    //        btn.buttonBusy(false);
                    //    }).always(() => {
                    //        btn.buttonBusy(false);
                    //    })
                    //} else {
                    //    btn.buttonBusy(false);
                    //}
                })
            }

            function onBtnRemoveItensPacoteClick(event) {
                const btn = $(this)
                btn.buttonBusy(true);
                let mensagem;
                let mensagemSucesso;
                const rows = gridItemsOptions.getSelectedRows();
                if (rows.length == 1) {
                    mensagem = "Deseja remover o item selecionado do pacote?";
                    mensagemSucesso = "Item da conta removido do pacote com sucesso";
                } else if (rows.length > 1) {
                    mensagem = "Deseja remover os itens selecionado do pacote?";
                    mensagemSucesso = "Itens da conta removidos do pacote com sucesso";
                }
                abp.message.confirm(mensagem, "Remover items do Pacote", (confirm) => {
                    if (confirm) {
                        const data = {
                            contaMedicaId: getContaMedicaId(),
                            itemIds: gridItemsOptions.getSelectedRows().map(x => x.id),
                            excluirDiversosPacotes:false
                        };
                        fatContaPacoteAppService.excluirItemsPacote(data.contaMedicaId, data.itemIds, data.excluirDiversosPacotes).then(res => {
                            if (res.errors && res.errors.length) {

                                let contentMessage = ''
                                _.forEach(res.errors, (errorItem,index) => {
                                    contentMessage += `
                                        <li class="list-group-item">
                                            <div class="row">
                                                <div class="col-md-2">${errorItem.codigoErro}</div>
                                                <div class="col-md-10"><b>${errorItem.descricao}</b></div>
                                            </div>
                                        </li>`
                                })

                                return customConfirmModalHelper.CreateModalAsync({
                                    title: "Remover items do Pacotes",
                                    message: "Há items selecionados de pacotes diferentes, você deseja remover mesmo assim?",
                                    icon: "fas fa-exclamation-triangle text-info",
                                    buttons: [
                                        customConfirmModalHelper.CreateButton("Excluir mesmo assim", "btn btn-primary", null, (event, confirmModalInstance) => {
                                            const btnExcluir = $(event.target);
                                            btnExcluir.buttonBusy(true);
                                            return confirmModalInstance.close(true)
                                        })
                                    ],
                                    styles: {
                                        "modal-dialog": { 'min-width': '600px' }
                                    },
                                    customContent: `
                                    <div class="row">
                                        <div class="col-md-8 col-md-offset-2">
                                            <ul class="list-group">
                                            ${contentMessage}
                                            </ul>
                                        </div>
                                    </div>`,
                                    confirmModalOptions: {
                                        cancelButton: {
                                            enable: true,
                                            callback: (event, confirmModalInstance) => {
                                                btn.buttonBusy(false);
                                                confirmModalInstance.close()
                                            }
                                        },
                                        onHideModal: (event, confirmModalInstance) => {
                                            btn.buttonBusy(false);
                                            confirmModalInstance.removeModal()
                                        },
                                        onShowModal(confirmModalInstance) {
                                            $("body").get(0).scrollIntoView()
                                        }
                                    },
                                    promiseCallback: (params) => {
                                        if (params == true) {
                                            return fatContaPacoteAppService.excluirItemsPacote(data.contaMedicaId, data.itemIds, true).then(() => {
                                                contaMedicaReload();
                                                abp.notify.success(mensagemSucesso);
                                                btn.buttonBusy(false);
                                            }).always(() => {
                                                btn.buttonBusy(false);
                                            })
                                        } 
                                        return false
                                    }
                                });
                            } else {
                                contaMedicaReload();
                                abp.notify.success(mensagemSucesso);
                                btn.buttonBusy(false);
                            }
                        }).always(() => {
                            btn.buttonBusy(false);
                        })
                    } else {
                        btn.buttonBusy(false);
                    }
                })
            }

            function onBtnRemoveItensKitClick(event) {
                const btn = $(this)
                btn.buttonBusy(true);
                let mensagem;
                let mensagemSucesso;
                const rows = gridItemsOptions.getSelectedRows();
                if (rows.length == 1) {
                    mensagem = "Deseja remover o item selecionado do kit?";
                    mensagemSucesso = "Item da conta removido do kit com sucesso";
                } else if (rows.length > 1) {
                    mensagem = "Deseja remover os itens selecionado do kit?";
                    mensagemSucesso = "Itens da conta removidos do kit com sucesso";
                }

                abp.message.confirm(mensagem, "Remover Kit", (confirm) => {

                    if (confirm) {
                            const data = {
                            contaMedicaId: getContaMedicaId(),
                            items: gridItemsOptions.getSelectedRows().map(x => x.id)
                        };

                        contaAppService.verificaRemoverItensKit(data.contaMedicaId, data.items).then(res => {
                            if (res.errors.length) {
                                errorHandler(res.errors, "Remoção itens Kit");
                                btn.buttonBusy(false);
                                return;
                            }
                            if (res.warnings.length) {
                                abp.message.confirm(res.warnings[0], "Remover Kit", (confirmMultiplos) => {
                                    if (confirmMultiplos) {
                                        removerItensKit(data);
                                    }
                                });
                                btn.buttonBusy(false);
                                return;
                            }

                            removerItensKit(data);

                        })

                        function removerItensKit (data) {
                            contaAppService.removerItensKit(data.contaMedicaId, data.items).then(resRemover => {
                                if (resRemover.errors.length) {
                                    errorHandler(resRemover.errors,"Remoção itens Kit");
                                    return;
                                }
                                contaMedicaReload();
                                abp.notify.info(mensagemSucesso)
                                btn.buttonBusy(false);
                            })
                        }
                    }

                    
                    //if (confirm) {
                    //    const data = {
                    //        contaMedicaId: getContaMedicaId(),
                    //        items = gridItemsOptions.getSelectedRows().map(x => x.id)
                    //    };
                    //    contaItemAppService.excluir(data).then(res => {
                    //        abp.notify.success(mensagemSucesso);
                    //        btn.buttonBusy(false);
                    //    }).always(() => {
                    //        btn.buttonBusy(false);
                    //    })
                    //} else {
                    //    btn.buttonBusy(false);
                    //}
                })
            }
            
            function onChangeContaItem(event) {
                const val = $(this).val();
                const data = $(this).select2("data");
                if(data.length) {
                    $(".itemDescricao").text(data[0].text);
                } else {
                    $(".itemDescricao").text('');
                }
                abp.ui.block();
                if(val) {
                    fatItemAppService.obterTipoGrupoId(val)
                        .then(atualizaAbas)
                        .then(() => {
                            return obterContaItem(val);
                        })
                        .then(() => {
                            calcularValorTotalItemFaturamento(val);
                        })

                    
                    abp.ui.unblock();
                }
                else {
                    calcularValorTotalItemFaturamento(null)
                    atualizaCardsValor(null);
                    atualizaAbas("");
                    abp.ui.unblock();
                }
            }

            function obterContaItem(val) {
                return fatItemAppService.obter(val).then(res => {
                    formContaItem.find("#formContaItemValorItem").attr("readonly", "readonly");
                    if (res && res.isPrecoManual) {
                        formContaItem.find("#formContaItemValorItem").removeAttr("readonly");
                        abp.message.info("Este item permite valor manual.");
                    }
                })
                
            }

            const calcularValorTotalItemFaturamento = _.debounce(calcularValorTotalItemFaturamentoMethod, 500);
            
            function calcularValorTotalItemFaturamentoMethod (val) {
                const  formContaItemFaturamentoItemId = val ?? formContaItem.find("#formContaItemFaturamentoItemId").val();
                if(!formContaItemFaturamentoItemId) {
                    formContaItem.find("#formContaItemValorItem").val(0);
                    formContaItem.find(".valorTotal").text(Number(0).toLocaleString('pt-br',{style: 'currency', currency: 'BRL'}));
                    formContaItem.find("#formContaItemResumoDetalhamento").val(JSON.stringify(null));
                    return;
                }


                const honorariosDto = {
                    medicoId: formContaItem.find("#formContaItemMedicoId").val(),
                    isMedicoCredenciado: formContaItem.find("#formContaItemIsMedCredenciado").attr("checked"),
                    medicoEspecialidadeId: formContaItem.find("#formContaItemMedicoEspecialidadeId").val(),
                    auxiliar1Id: formContaItem.find("#formContaItemAuxiliar1Id").val(),
                    auxiliar1IsCredenciado: formContaItem.find("#formContaItemIsAux1Credenciado").attr("checked"),
                    auxiliar1EspecialidadeId: formContaItem.find("#formContaItemAuxiliar1EspecialidadeId").val(),
                    auxiliar2Id: formContaItem.find("#formContaItemAuxiliar2Id").val(),
                    auxiliar2IsCredenciado: formContaItem.find("#formContaItemIsAux2Credenciado").attr("checked"),
                    auxiliar2EspecialidadeId: formContaItem.find("#formContaItemAuxiliar2EspecialidadeId").val(),
                    auxiliar3Id: formContaItem.find("#formContaItemAuxiliar3Id").val(),
                    auxiliar3IsCredenciado: formContaItem.find("#formContaItemIsAux3Credenciado").attr("checked"),
                    auxiliar3EspecialidadeId: formContaItem.find("#formContaItemAuxiliar3EspecialidadeId").val(),
                    instrumentadorId: formContaItem.find("#formContaItemInstrumentadorId").val(),
                    instrumentadorIsCredenciado: formContaItem.find("#formContaItemIsInstrCredenciado").attr("checked"),
                    instrumentadorEspecialidadeId: formContaItem.find("#formContaItemInstrumentadorEspecialidadeId").val(),
                    anestesistaId: formContaItem.find("#formContaItemAnestesistaId").val(),
                    anestesistaIsCredenciado: formContaItem.find("#formContaItemIsAnestCredenciado").attr("checked"),
                    anestesistaEspecialidadeId: formContaItem.find("#formContaItemEspecialidadeAnestesistaId").val(),
                    procedimentoPrincipal: formContaItem.find("#formContaItemProcedimentoPrincipalId").val(),
                    Percentual: formContaItem.find("#formContaItemPercentualItem").val() || 100
                }

                const valorTotalItemFaturamentoDto = {
                    contaMedicaId:$("#contaMedicaId").val(),
                    faturamentoItemId: formContaItemFaturamentoItemId,
                    data: formContaItem.find("#formContaItemData").val(),
                    qtd: formContaItem.find("#formContaItemQtde").val(),
                    percentual: formContaItem.find("#formContaItemPercentualItem").val() || 100,
                    unidadeOrganizacionalId: formContaItem.find("#formContaItemUnidadeOrganizacionalId").val(),
                    terceirizadoId: formContaItem.find("#formContaItemTerceirizadoId").val(),
                    centroCustoId: formContaItem.find("#formContaItemCentroCustoId").val(),
                    turnoId: formContaItem.find("#formContaItemTurnoId").val(),
                    tipoLeitoId: formContaItem.find("#formContaItemTipoLeitoId").val(),
                    honorarios: honorariosDto,
                    
                };

                

                contaItemAppService.calcularValorTotalItemFaturamento(valorTotalItemFaturamentoDto).then(atualizarValorTotalItemFaturamento);
            }

            
            
            function atualizarValorTotalItemFaturamento(res) {
                if(res.errors.length) {
                    let errors = res.errors.map(mapWarningAndErrors).join("\n");
                    abp.notify.error(errors);
                }

                if(res.warnings.length) {
                    let warnings = res.warnings.map(mapWarningAndErrors).join("\n");
                    abp.notify.warn(warnings);
                }
                
                if(!res.errors.length) {
                    formContaItem.find("#formContaItemValorItem").val(res.returnObject.valor);
                    if (!res.returnObject.valorTotal) {
                        res.returnObject.valorTotal = 0;
                    }
                    if (res.returnObject.resumoDetalhamento) {
                        formContaItem.find("#formContaItemResumoDetalhamento").val(JSON.stringify(res.returnObject.resumoDetalhamento))
                        atualizaCardsValor(res.returnObject.resumoDetalhamento);
                        
                    } else {
                        formContaItem.find("#formContaItemResumoDetalhamento").val(JSON.stringify(null));
                        atualizaCardsValor(null);
                    }
                    abp.notify.info("Valor Únitario calculado!");
                }

                function mapWarningAndErrors(x) {
                    if (x.codigo) {
                        return `${x.codigo} - ${x.descricao}`
                    }
                    return`${x.descricao}`;
                }
            }
            
            function atualizaCardsValor(resumoDetalhamento) {
                formContaItem.find(".valor-perc").text((Number(formContaItem.find("#formContaItemPercentualItem").val())/ 100).toLocaleString('pt-br',{style: 'percent'}));
                if(resumoDetalhamento) {
                    formContaItem.find(".valorPorte").text(resumoDetalhamento.valorPorte.toLocaleString('pt-br',{style: 'currency', currency: 'BRL'}));
                    formContaItem.find(".valorFilme").text(resumoDetalhamento.valorFilme.toLocaleString('pt-br',{style: 'currency', currency: 'BRL'}));
                    formContaItem.find(".valorTaxas").text(resumoDetalhamento.valorTaxas.toLocaleString('pt-br', { style: 'currency', currency: 'BRL' }));
                    formContaItem.find(".valorItem").text(resumoDetalhamento.valor.toLocaleString('pt-br', { style: 'currency', currency: 'BRL' }));
                    formContaItem.find(".valorCOCH").text(resumoDetalhamento.valorCOCH.toLocaleString('pt-br', { style: 'decimal' }));
                    formContaItem.find(".valorHMCH").text(resumoDetalhamento.valorHMCH.toLocaleString('pt-br', { style: 'decimal' }));
                    formContaItem.find(".valorTotal").text(resumoDetalhamento.valorTotal.toLocaleString('pt-br',{style: 'currency', currency: 'BRL'}));
                    if(resumoDetalhamento.tabela) {
                        formContaItem.find(".valorTabela").text(resumoDetalhamento.tabela.descricao);
                    } else {
                        formContaItem.find(".valorTabela").text("");
                    }
                } else {
                    const zero = Number(0);
                    formContaItem.find(".valorPorte").text(zero.toLocaleString('pt-br',{style: 'currency', currency: 'BRL'}));
                    formContaItem.find(".valorFilme").text(zero.toLocaleString('pt-br',{style: 'currency', currency: 'BRL'}));
                    formContaItem.find(".valorTaxas").text(zero.toLocaleString('pt-br',{style: 'currency', currency: 'BRL'}));
                    formContaItem.find(".valorTotal").text(zero.toLocaleString('pt-br', { style: 'currency', currency: 'BRL' }));
                    formContaItem.find(".valorItem").text(zero.toLocaleString('pt-br', { style: 'currency', currency: 'BRL' }));
                    formContaItem.find(".valorCOCH").text(zero.toLocaleString('pt-br', { style: 'decimal' }));
                    formContaItem.find(".valorHMCH").text(zero.toLocaleString('pt-br', { style: 'decimal' }));
                    formContaItem.find(".valorTabela").text("");
                }
            }
            
            function atualizaAbas(tipoGrupoId) {
                if (tipoGrupoId === 1) {
                    $('#abaItemHonorarios').fadeIn();
                } else if (tipoGrupoId === 2) {
                    $('#abaItemHonorarios').fadeOut();
                    //$('#addItemHonorarios').fadeOut();
                    $("#abaItemPrincipal").tab("show");
                    //$('#abaItemObservacao').tab("show");
                } else if (tipoGrupoId === 3) {
                    $('#abaItemHonorarios').fadeOut();
                    //$('#addItemHonorarios').fadeOut();
                    $("#abaItemPrincipal").tab("show");
                    //$('#abaItemObservacao').tab("show");
                } else if (tipoGrupoId === 4 ) {
                    $('#abaItemHonorarios').fadeOut();
                    //$('#addItemHonorarios').fadeOut();
                    $("#abaItemPrincipal").tab("show");
                    //$('#abaItemObservacao').tab("show");
                } else if(tipoGrupoId === "" || tipoGrupoId == null || tipoGrupoId === 0) {
                    $('#abaItemHonorarios').fadeOut();
                    //$('#addItemHonorarios').fadeOut();
                    $("#abaItemPrincipal").tab("show");
                }
            }
        }
        
        function criarContaKit() {
            selectSWWithDefaultValue(formContaKit.find(".select2Kit"), '/api/services/app/faturamentoKit/listarDropdown');
            formContaKit.attr("autocomplete","off");
            selectSWWithDefaultValue(formContaKit.find(".selectItem"), '/api/services/app/faturamentoItem/listarDropdown');
            selectSWWithDefaultValue(formContaKit.find(".selectlocalUtilizacao"), '/api/services/app/UnidadeOrganizacional/ListarDropdown');
            selectSWWithDefaultValue(formContaKit.find(".selectTerceirizado"),"/api/services/app/terceirizado/ListarDropdown");
            selectSWWithDefaultValue(formContaKit.find(".selectCentroDeCusto"), '/api/services/app/CentroCusto/ListarDropdownCodigoCentroCusto');
            selectSWWithDefaultValue(formContaKit.find(".selectTurno"), "/api/services/app/Turno/ListarDropdown");
            selectSWWithDefaultValue(formContaKit.find(".selectTipoAcomodacao"),"/api/services/app/TipoAcomodacao/ListarDropdown");


            btnAdicionarKit.on("click",onBtnAdicionarKitClick);
            $(".btn-remove-kit").on("click", onBtnRemoveKitClick)
            function onBtnAdicionarKitClick(e) {
                const btn = $(this);
                btn.buttonBusy(true);
                formContaKit.validate();
                const formContaKitData = formContaKit.serializeFormToObject();
                formContaKitData.contaMedicaId = $("#contaMedicaId").val()
                formContaKitData.id = formContaKitData.id || 0;
                if (formContaKit.valid()) {
                    abp.ui.setBusy();
                    criarOuEditarKitModal.open(formContaKitData);
                    btn.buttonBusy(false);
                } else {
                    btn.buttonBusy(false);
                }
            }

            function onBtnRemoveKitClick(e) {
                if (!gridKitOptions.getSelectedRows().length) {
                    abp.notify.error("Não é possivel excluir. Não há nenhum kit selecionado");
                    return;
                }
                let kitRow = gridKitOptions.getSelectedRows()[0];
                const btn = $(this);
                btn.buttonBusy(true);

                abp.message.confirm("Todos os itens relacionados a esse kit serão removidos! Voce tem certeza?", "Remover Kit", (confirm) => {
                    if (!confirm) {
                        btn.buttonBusy(false);
                        return;
                    }
                    
                    fatContaKitAppService.removerKit(getContaMedicaId(), kitRow.id).then(res => {
                        if (res.errors && res.errors.length) {
                            //mensagem de erro.
                            errorHandler(res.errors, "Erro ao remover kit.");
                            btn.buttonBusy(false);
                            return;
                        }
                        abp.notify.info("Kit removido com sucesso");
                        contaMedicaReload();

                    }).always(() => {
                        btn.buttonBusy(false);
                    });

                });

            }
        }


        function criarContaPacote() {
            formContaPacote.attr("autocomplete", "off");
            selectSWWithDefaultValue(formContaPacote.find(".select2Pacote"), "/api/services/app/faturamentoItem/ListarDropdownPacote");
            selectSWWithDefaultValue(formContaPacote.find(".SelectUnidadeOrganizacional"), "/api/services/app/unidadeOrganizacional/ListarDropdownLocalUtilizacao");
            selectSWWithDefaultValue(formContaPacote.find(".selectTerceirizado"), "/api/services/app/terceirizado/ListarDropdown");
            selectSWWithDefaultValue(formContaPacote.find(".selectTurno"), "/api/services/app/turno/ListarDropdown");

            btnAdicionarPacote.on("click", onBtnAdicionarPacoteClick);

            $(".btn-remove-pacote").on("click", onBtnRemovePacoteClick )


            function onBtnAdicionarPacoteClick(e) {
                const btnAdd = $(this);
                btnAdd.buttonBusy(true);
                customConfirmModalHelper.CreateModal({
                    title: "Pacote",
                    message: "Deseja adicionar um pacote avulso ou empacotar itens da conta?",
                    icon: "fas fa-exclamation-triangle text-info",
                    buttons: [
                        customConfirmModalHelper.CreateButton("Adicionar Pacote", "btn btn-default", null, (event, confirmModalInstance) => {
                            onAdicionarPacote(event, confirmModalInstance);
                        }),
                        customConfirmModalHelper.CreateButton("Empacotar Itens da conta", "btn btn-default", null, (event, confirmModalInstance) => {
                            onEmpacotarItens(event, confirmModalInstance);
                        })
                    ],
                    styles: {
                        "modal-dialog": { 'min-width': '500px' }
                    },
                    confirmModalOptions: {
                        cancelButton: {
                            enable: true,
                            callback: (event, confirmModalInstance) => {
                                confirmModalInstance.close()
                                btnAdd.buttonBusy(false);
                            }
                        },
                        onHideModal: (event, confirmModalInstance) => {
                            confirmModalInstance.removeModal()
                            btnAdd.buttonBusy(false);
                        }
                    }
                });

            }

            function onAdicionarPacote(e, confirmModalInstance) {
                console.log(confirmModalInstance);
                const btn = $(event.target);
                btn.buttonBusy(true);
                formContaPacote.validate();
                confirmModalInstance.close();
                const formContaPacoteData = formContaPacote.serializeFormToObject();
                //formContaPacoteData.dataInicio = $("#formContaPacoteKitDataInicio").val()
                //formContaPacoteData.dataFim = $("#formContaPacoteKitDataFim").val()
                formContaPacoteData.contaMedicaId = $("#contaMedicaId").val()
                formContaPacoteData.id = formContaPacoteData.id || 0;
                if (formContaPacote.valid()) {
                    abp.ui.setBusy()
                    contaItemAppService.incluirPacoteAvulso(formContaPacoteData).then(res => {
                        if (res.errors && res.errors.length) {
                            //mensagem de erro.
                            errorHandler(res.errors, "Erro ao cadastrar pacote.");
                            abp.ui.clearBusy();
                            btn.buttonBusy(false);
                            btnAdicionarPacote.buttonBusy(false);
                            return;
                        }
                    }).always(() => {
                        abp.ui.clearBusy()
                        btnAdicionarPacote.buttonBusy(false);
                        abp.event.trigger("app.contaMedicaReload")
                    })
                } else {
                    btn.buttonBusy(false);
                    abp.ui.clearBusy();
                    btnAdicionarPacote.buttonBusy(false);
                }
            }

            function onEmpacotarItens(e, confirmModalInstance) {
                console.log(confirmModalInstance);
                const btn = $(event.target);
                btn.buttonBusy(true);
                formContaPacote.validate();
                confirmModalInstance.close();
                const formContaPacoteData = formContaPacote.serializeFormToObject();
                //formContaPacoteData.dataInicio = $("#formContaPacoteKitDataInicio").val()
                //formContaPacoteData.dataFim = $("#formContaPacoteKitDataFim").val()
                formContaPacoteData.contaMedicaId = $("#contaMedicaId").val()
                formContaPacoteData.id = formContaPacoteData.id || 0;
                if (formContaPacote.valid()) {
                    abp.ui.setBusy()
                    contaAppService.verificaPacote(formContaPacoteData).then(res => {
                        if (res.errors && res.errors.length) {
                            //mensagem de erro.
                            errorHandler(res.errors, "Erro ao cadastrar pacote.");
                            btn.buttonBusy(false);
                            btnAdicionarPacote.buttonBusy(false);
                            return;
                        } else {
                            criarOuEditarPacoteModal.open(formContaPacoteData);
                            btn.buttonBusy(false);
                            btnAdicionarPacote.buttonBusy(false);
                        }
                    })
                } else {
                    btn.buttonBusy(false);
                    btnAdicionarPacote.buttonBusy(false);
                }
            }


            function onBtnRemovePacoteClick(e) {
                const btn = $(this);

                btn.buttonBusy(true);
                debugger;
                if (gridPacoteOptions.getSelectedRows().length < 1) {
                    abp.notify.warn("Não há nenhum pacote selecionado");
                    return;
                }
                const pacote = gridPacoteOptions.getSelectedRows()[0];
                if (pacote.totalItensNoPacote <= 0) {
                    return abp.message.confirm("Este pacote não possui nenhum item, você deseja excluir ?", "Exclusão pacote", (confirm) => {
                        if (confirm) {
                            return fatContaPacoteAppService.excluirPacote(pacote.id)
                                .then(() => {
                                    abp.notify.success("Pacote excluído com sucesso.");
                                })
                                .always(() => {
                                    contaMedicaReload();
                                    btn.buttonBusy(false);
                                });
                        }
                        else {
                            btn.buttonBusy(false);
                        }
                    })
                } else if (pacote.totalItensNoPacote > 0) {
                    return abp.message.confirm(
                        `Este pacote possui ${(pacote.TotalItensNoPacote == 1 ? `${pacote.TotalItensNoPacote} item` : `${pacote.TotalItensNoPacote} items`)} atrelado, você deseja excluir ?`,
                        "Exclusão pacote",
                        (confirm) => {
                            if (confirm) {
                                return fatContaPacoteAppService.excluirPacote(pacote.id)
                                    .then(() => {
                                        abp.notify.success("Pacote excluído com sucesso.");
                                    })
                                    .always(() => {
                                        contaMedicaReload();
                                        btn.buttonBusy(false);
                                    });
                            } else {
                                btn.buttonBusy(false);
                            }
                        })
                } else {
                    btn.buttonBusy(false);
                }
            }

        }
        
        function contaMedicaReload() {
            gridItemsOptions.render(gridItems[0]);
            gridKitOptions.render(gridKit[0]);
            gridPacoteOptions.render(gridPacote[0]);
            gridOcorrenciasOptions.render(gridOcorrencia[0]);
        }


        function contaBloqueada() {
            return $("#contaMedicaStatusId").val() == 2
        }

        function bloquearContaConferido() {
            if (contaBloqueada()) {
                bloquearContaItem()
                bloquearContaKit()
                bloquearContaPacote()
                $(".btn-aprovar-conta").hide();
                abp.notify.warn("Não é possível alterar uma conta conferida. Favor voltar o status para altera-la");
            }


            function bloquearContaItem() {
                $("#btnNovoItem").hide();
            }

            function bloquearContaKit() {
                formContaKit.hide();
            }

            function bloquearContaPacote() {
                formContaPacote.hide();
            }
        }
        
        function carregaPagina() {
            $('#smartwizard').smartWizard({
                selected: 0,
                theme: 'arrows',
                enableURLhash: false,
                justified: true,
                autoAdjustHeight: false,
                cycleSteps: false,
                backButtonSupport: false,
                transition: {
                    animation: 'slide-horizontal', // Effect on navigation, none/fade/slide-horizontal/slide-vertical/slide-swing
                    speed: '400', // Transion animation speed
                    easing: '' // Transition animation easing. Not supported without a jQuery easing plugin
                },
                toolbarSettings: {
                    toolbarPosition: 'bottom', // none, top, bottom, both
                    toolbarButtonPosition: 'right', // left, right, center
                    showNextButton: false, // show/hide a Next button
                    showPreviousButton: false, // show/hide a Previous button
                    toolbarExtraButtons: [] // Extra buttons to show on toolbar, array of jQuery input/buttons elements
                },
                anchorSettings: {
                    anchorClickable: true, // Enable/Disable anchor navigation
                    enableAllAnchors: true, // Activates all anchors clickable all times
                    markDoneStep: false, // Add done state on navigation
                    markAllPreviousStepsAsDone: false, // When a step selected by url hash, all previous steps are marked done
                    removeDoneStepOnNavigateBack: false, // While navigate back done step after active step will be cleared
                    enableAnchorOnDoneStep: false // Enable/Disable the done steps navigation
                },
                keyboardSettings: {
                    keyNavigation: false
                }
            }).on('showStep', onShowStep);
            
            
            $(".btn-aprovar-conta").on('click',function (event) {
                const btn = $(this);
                btn.buttonBusy(true);
                // verifica o fluxo para aprovar.
                
                contaAppService.verificaFluxo($("#contaMedicaId").val()).then(res=> {
                    let msg = "";
                    
                    switch(res) {
                        case 1: {
                            msg = "Esta conta será liberada para o fluxo <b> Inicial </b>. Você tem certeza?";
                            break;
                        }
                        case 2: {
                            msg = "Esta conta será liberada para o fluxo <b> Conferido </b>. Você tem certeza?";
                            break;
                        }
                        case 6: {
                            msg = "Esta conta será liberada para o fluxo <b> Auditoria Interna </b>. Você tem certeza?";
                            break;
                        }
                        case 7: {
                            msg = "Esta conta será liberada para o fluxo <b> Auditoria Externa </b>. Você tem certeza?";
                            break;
                        }
                    }

                    abp.message.confirm(msg,"Liberar Conta médica", (confirm) => {
                        if(confirm){
                            contaAppService.alteraStatusConta($("#contaMedicaId").val(), res).then(resAprovarConta => {
                                const basePath = `${window.location.protocol}//${window.location.hostname}${(window.location.port ? `:${window.location.port}`: '') }`;
                                window.location.href = `${basePath}/Mpa/FaturarAtendimento/ContaMedica?atendimentoId=${$("#atendimentoId").val()}&contaMedicaId=${resAprovarConta}`
                            });
                        }
                        else {
                            btn.buttonBusy(false);
                        }
                    },true)
                })
                
            })

            $(".btn-voltar-status-conta").on('click', function (event) {
                const btn = $(this);
                btn.buttonBusy(true);
                contaAppService.verificaFluxoVolta($("#contaMedicaId").val()).then(res => {
                    let msg = "";

                    switch (res) {
                        case 1: {
                            msg = "Esta conta voltará para o fluxo <b> Inicial </b>. Você tem certeza?";
                            break;
                        }
                        case 2: {
                            msg = "Esta conta voltará para o fluxo <b> Conferido </b>. Você tem certeza?";
                            break;
                        }
                        case 6: {
                            msg = "Esta conta voltará para o fluxo <b> Auditoria Interna </b>. Você tem certeza?";
                            break;
                        }
                        case 7: {
                            msg = "Esta conta voltará para o fluxo <b> Auditoria Externa </b>. Você tem certeza?";
                            break;
                        }
                    }

                    abp.message.confirm(msg, "Voltar Status Conta médica", (confirm) => {
                        if (confirm) {
                            contaAppService.alteraStatusConta($("#contaMedicaId").val(), res).then(resAprovarConta => {
                                const basePath = `${window.location.protocol}//${window.location.hostname}${(window.location.port ? `:${window.location.port}` : '')}`;
                                window.location.href = `${basePath}/Mpa/FaturarAtendimento/ContaMedica?atendimentoId=${$("#atendimentoId").val()}&contaMedicaId=${resAprovarConta}`
                            });
                        }
                        else {
                            btn.buttonBusy(false);
                        }
                    }, true)
                })

            })
            
            criarContaItem();
            criarContaKit();
            criarContaPacote();
            createDatePicker();
            CamposRequeridos();

            bloquearContaConferido();
            
            abp.event.on("app.contaMedicaReload",contaMedicaReload);
        }

        
    })
})()