(function ($) {
    app.modals.CriarOuEditarKitModal = function () {
        let modalManager;
        let $modal = null;
        let liDefinirParaTodos;
        const kitContaMedicaId = $("#KitContaMedicaId").val()
        const fatItemAppService = abp.services.app.faturamentoItem;
        const contaItemAppService = abp.services.app.faturamentoContaItem;
        
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

        
        
        this.init = function (_modalManager) {
            modalManager = _modalManager;
            $modal = $(_modalManager.getModal());
            $modal.find(".modal-dialog").css({ 'width': '95%'});
            $modal.find(".filtrar-descricao").keyup(onFiltarItems);
            $modal.find(".filtrar-grupo").change(onFiltarItems);
            $modal.find(".remove-item").click(onRemoveItem);
            $modal.find(".undo-item").click(onUndoItem);
            $modal.find(".remove-group").click(onRemoveGroup);
            $modal.find(".undo-group").click(onUndoGroup);

            $modal.find("#definir-para-todos").click(onDefinirParaTodosClick);
            liDefinirParaTodos= $modal.find("#definir-para-todos").parents("li");
            onDefinirParaTodosClick();
            $modal.find('a[data-toggle="tab"]').on('show.bs.tab', onTabShow);
            atualizaTotalSelecionados();
            CamposRequeridos();

            abp.ui.clearBusy();
        }
        
        this.save = function() {
            const validateItems = onValidateItems();
            if(validateItems.valid) {
                abp.message.confirm(`Deseja incluir todos os <b>${$modal.find(".selecionados").text()}</b> items selecionados do KIT: <b>${$modal.find("#kit-descricao").text()}</b> na conta médica?`, "",
                    (confirm) => {
                        if (confirm) {
                            abp.ui.block();
                            onSave(validateItems.formsData);
                            abp.ui.unblock();
                        }
                    }, true);
            }
            else {
                //todo fazer scroll
                validateItems.anchor.focus();
                validateItems.anchor.tab("show");
            }
        }
        // por definição o elemento que retorna do erro é o primeiro, mas ele valida todos os seguintes
        function onValidateItems() {
            let result = {
                anchor:undefined,
                valid:true,
                formsData:[]
            };
            
            const selectedLis =  $modal.find("ul.list-fat-items li").filter(function() {
                return $(this).find("a.fatContaItemAnchor").data("selected") === true
            });
            _.forEach(selectedLis, (item) => {
                const currentLi = $(item);
                const currentAnchor = currentLi.find("a.fatContaItemAnchor");
                const formContaItemUniqueId = $modal.find(`${currentAnchor.attr("href")}`).find("form.formContaItemKit");
                const isFormValid = validateForm(formContaItemUniqueId, currentAnchor);
                if(!isFormValid && result.valid) {
                    result.valid = false;
                    result.anchor = currentAnchor;
                }
                if (isFormValid) {
                    const form = formContaItemUniqueId.serializeFormToObject();
                    if (form.data && moment(form.data, "DD/MM/YYYY HH:mm:ss", true).isValid()) {
                        form.data = moment(form.data, "DD/MM/YYYY HH:mm:ss", true).utcOffset(0, true).toISOString()
                    } else if (form.data && moment(form.data, "DD/MM/YYYY HH:mm:ss Z", true).isValid()) {
                        form.data = moment(form.data, "DD/MM/YYYY HH:mm:ss Z", true).utcOffset(0, true).toISOString()
                    }
                    result.formsData.push(form);
                }
            })
            return result;
        }
        
        function onSave(formsData) {
            const data = {
                id:$modal.find("#id").val(),
                faturamentoContaId: kitContaMedicaId,
                faturamentoKitId: $modal.find("#KitId").val(),
                data: $modal.find("#KitData").val(),
                horaIncio: $modal.find("#KitHoraIncio").val(),
                horaFim: $modal.find("#KitHoraFim").val(),
                qtde: $modal.find("#KitQtde").val(),
                unidadeOrganizacionalId: $modal.find("#KitUnidadeOrganizacionalId").val(),
                terceirizadoId: $modal.find("#KitTerceirizadoId").val(),
                centroCustoId: $modal.find("#KitCentroCustoId").val(),
                turnoId: $modal.find("#KitTurnoId").val(),
                tipoLeitoId: $modal.find("#KitTipoLeitoId").val(),
                items: formsData
            }

            contaItemAppService.incluirItemsDoKit(data).then(res=> {
                abp.notify.success("Kit incluido com sucesso");
                modalManager.close();
                abp.event.trigger("app.contaMedicaReload");
            });
        }

        function onTabShow (e) {
            liDefinirParaTodos.removeClass("active");
            const target = $(e.target);
            const currentLi = target.parents("li");
            const formContaItemUniqueId = $(target.attr("href")).find(".formContaItemKit");
            if(!$(e.target).data("loaded")) {
                CriarContaMedicaFormItemPorUniqueId(formContaItemUniqueId);
                $(e.target).data("loaded",true)
            }
            btnProximoVisibilidade(currentLi, formContaItemUniqueId);
            btnRemoverBtnReadicionarVisibilidade(target, formContaItemUniqueId);
        }
        
        function onRemoveItem(e) {
            e.stopImmediatePropagation();
            const btn = $(this);
            btn.buttonBusy(true);
            const anchor = btn.parents("a.fatContaItemAnchor");
            const formContaItemUniqueId = $(anchor.attr("href")).find(".formContaItemKit");
            anchor.data("selected",false);
            anchor.find(".undo-item").removeClass("hidden");
            btn.addClass("hidden").buttonBusy(false);
            atualizaTotalSelecionados()
            btnRemoverBtnReadicionarVisibilidade(anchor,formContaItemUniqueId)
        }

        function onRemoveGroup(e) {
            e.stopImmediatePropagation();
            const btn = $(this);
            btn.buttonBusy(true);
            const anchorBtn = btn.parents(".group-chk-btn");
            anchorBtn.find(".undo-group").removeClass("hidden");
            btn.addClass("hidden").buttonBusy(false);
            $modal.find(`a[data-item-grupo="${btn.data("grupo-id")}"]`).each((index, item) => {
                const anchor = $(item);
                anchor.find(".remove-item").trigger("click");
            });
        }

        function onUndoGroup(e) {
            e.stopImmediatePropagation();
            const btn = $(this);
            btn.buttonBusy(true);
            const anchorBtn = btn.parents(".group-chk-btn");
            anchorBtn.find(".remove-group").removeClass("hidden");
            btn.addClass("hidden").buttonBusy(false);
            $modal.find(`a[data-item-grupo="${btn.data("grupo-id")}"]`).each((index, item) => {
                const anchor = $(item);
                anchor.find(".undo-item").trigger("click");
            });
        }

        function onUndoItem(e) {
            e.stopImmediatePropagation();
            const btn = $(this);
            btn.buttonBusy(true);
            const anchor = btn.parents("a.fatContaItemAnchor");
            const formContaItemUniqueId = $(anchor.attr("href")).find(".formContaItemKit");
            anchor.data("selected",true);
            anchor.find(".remove-item").removeClass("hidden");
            btn.addClass("hidden").buttonBusy(false);
            atualizaTotalSelecionados()
            btnRemoverBtnReadicionarVisibilidade(anchor,formContaItemUniqueId)
        }
        
        function atualizaTotalSelecionados() {
            $modal.find(".selecionados").text($modal.find(".list-fat-items li a.fatContaItemAnchor").filter(function() {
                return $(this).data("selected") == true
            }).length);
        }

        function onDefinirParaTodosClick() {
            const tab = $modal.find("#tabpanel-definir-para-todos");
            const tabContent = tab.parent();
            const formContaItemUniqueId = tab.find(".formContaItemKitDefinirParaTodos");
            tab.addClass("active");
            liDefinirParaTodos.addClass("active");
            tabContent.find(".tab-pane-fat-conta-item").not(tab).removeClass("active");
            $modal.find(".list-fat-items li").removeClass("active");
            CriarContaMedicaFormItemPorUniqueId(formContaItemUniqueId);
            
            formContaItemUniqueId.find(".btnDefinirParaTodos").click(onBtnDefinirParaTodos)
            
            function onBtnDefinirParaTodos(e) {
                const btn = $(this);
                e.stopImmediatePropagation();
                btn.buttonBusy(true);
                if(validateDefinirParaTodosForm(formContaItemUniqueId)){
                    abp.message.confirm("Deseja definir esses parâmetros para todos os itens?","",(confirm)=>{
                        if(confirm) {
                            definirParametrosParaOsItens();
                        }
                        btn.buttonBusy(false);
                    })
                } else {
                    btn.buttonBusy(false);
                }
            }
        }
        function definirParametrosParaOsItens() {
            const definirParaTodosFormData = $modal.find("#formContaItem_DefinirParaTodos").serializeFormToObject()
            const tabPanes = $modal.find(".tab-pane-fat-conta-item").not("#tabpanel-definir-para-todos");
            const principalProperties =  [
                {id:'data',type: 'data'},
                {id:'unidadeOrganizacionalId', type:"select2"},
                {id:'terceirizadoId', type:"select2"},
                {id:'centroCustoId', type:"select2"},
                {id:'turnoId', type:"select2"},
                {id:'tipoLeitoId', type:"select2"},
                {id:'observacao', type:"summernote"}
            ];
            const honorarioProperties = [
                {id:'medicoId', type:"select2"},
                {id:'isMedCredenciado', type:'checkbox'},
                {id:'medicoEspecialidadeId', type:"select2"},
                {id:'auxiliar1Id', type:"select2"},
                {id:'isAux1Credenciado', type:'checkbox'},
                {id:'auxiliar1EspecialidadeId', type:"select2"},
                {id:'auxiliar2Id', type:"select2"},
                {id:'isAux2Credenciado', type:'checkbox'},
                {id:'auxiliar2EspecialidadeId', type:"select2"},
                {id:'auxiliar3Id', type:"select2"},
                {id:'isAux3Credenciado', type:'checkbox'},
                {id:'auxiliar3EspecialidadeId', type:"select2"},
                {id:'InstrumentadorId', type:"select2"},
                {id:'isInstrCredenciado', type:'checkbox'},
                {id:'InstrumentadorEspecialidadeId', type:"select2"},
                {id:'anestesistaId', type:"select2"},
                {id:'isAnestCredenciado', type:'checkbox'},
                {id:'especialidadeAnestesistaId', type:"select2"},
                {id:'procedimentoPrincipalId', type:"select2"},
                {id:'percentual', type:"text"},
            ];
            tabPanes.each(setarParametros);
            
            function setarParametros() {
                const currentTab = $(this);
                const currentForm = currentTab.find("form");
                setarPrincipal();
                
                if(currentForm.find("input[name='grupoId']").val() === "2"){
                    setarHonorarios();
                }
                
                
                function setarHonorarios(){
                    _.forEach(honorarioProperties,(property) => {
                        setProperty(property,currentForm,definirParaTodosFormData);
                    });
                }

                function setarPrincipal(){
                    _.forEach(principalProperties,(property) => {
                        setProperty(property,currentForm,definirParaTodosFormData);
                    });
                }
                
                function setProperty(property,currentForm,definirParaTodosFormData){
                    switch (property.type) {
                        case "select2":{
                            onSetPropertySelect2()
                            break;
                        }
                        case "data":{
                            onSetPropertyData()
                            break;
                        }
                        case "checkbox":{
                            onSetPropertyCheckbox()
                            break;
                        }
                        case "text":{
                            onSetPropertyText()
                            break;
                        }
                        case "summernote":{
                            onSetPropertySummernote()
                            break;
                        }
                        default: {
                            break
                        }
                    }
                    
                    function onSetPropertySelect2() {
                        const dataEl = currentForm.find(`select[name="${property.id}"]`);
                        dataEl.val(definirParaTodosFormData[property.id])
                        dataEl.data("value",definirParaTodosFormData[property.id]);
                        if(dataEl.data("select2")) {
                            dataEl.trigger("select2:selectById", definirParaTodosFormData[property.id]);
                        }
                    }
                    function onSetPropertyData() {
                        const dataEl = currentForm.find(`input[name="${property.id}"]`);
                        dataEl.val(definirParaTodosFormData[property.id]);
                        if(dataEl.data("daterangepicker")) {
                            dataEl.data("daterangepicker").callback(dataEl.val());
                        }
                    }
                    function onSetPropertyCheckbox() {
                        const dataEl = currentForm.find(`input[name="${property.id}"]`);
                        const value = definirParaTodosFormData[property.id];
                        if(value) {
                            dataEl.attr("checked","checked");
                        }
                        else {
                            dataEl.removeAttr("checked");
                        }
                    }
                    function onSetPropertyText() {
                        const dataEl = currentForm.find(`input[name="${property.id}"]`);
                        dataEl.val(definirParaTodosFormData[property.id]);
                    }
                    function onSetPropertySummernote() {
                        const dataEl = currentForm.find(`textarea[name="${property.id}"]`);
                        if(dataEl.data("summernote")) {
                            dataEl.summernote('code',definirParaTodosFormData[property.id]);
                        } else {
                            dataEl.html(definirParaTodosFormData[property.id])
                        }
                    }
                }
            }
        }
        
        function onFiltarItems() {
            const descricao = $modal.find(".filtrar-descricao").val();
            let grupos =  $(".filtrar-grupo").filter(":checked").map((index,el)=> parseInt($(el).val())).get();
            
            if(!descricao.length && !grupos.length) {
                $modal.find(".list-fat-items li").removeClass("hidden");
            } else {
                $modal.find(".list-fat-items li").each(function () {
                    const li = $(this);
                    const btn = li.find("a");
                    const grupoId = parseInt(btn.data("item-grupo"));
                    const itemDescricao = btn.data("item-descricao").toLowerCase()
                    if (!grupos.length) {
                        if (descricao.length === 0) {
                            $modal.find(".list-fat-items li").removeClass("hidden");
                        } else if (itemDescricao.includes(descricao.toLowerCase())) {
                            li.removeClass("hidden");
                        } else {
                            li.addClass("hidden");
                        }
                    } else {
                        if (descricao.length === 0 && _.includes(grupos, grupoId)) {
                            li.removeClass("hidden");
                        } else if (itemDescricao.includes(descricao.toLowerCase()) && _.includes(grupos, grupoId)) {
                            li.removeClass("hidden");
                        } else {
                            li.addClass("hidden");
                        }
                    }
                });
                $modal.find('.total-filtrado').text($modal.find(".list-fat-items li:not(.hidden)").length);
            }
        }
        function getNextFiltered(li) {
            return li.nextAll().filter(function() {
                return $(this).find("a.fatContaItemAnchor").data("selected") === true
            }).first();
        }
        function btnProximoVisibilidade(currentLi, formContaItemUniqueId) {
            if (!getNextFiltered(currentLi).length) {
                formContaItemUniqueId.find(".btnProximo").addClass("hidden");
            }
        }
        
        function btnRemoverBtnReadicionarVisibilidade(anchor, formContaItemUniqueId) {
            if(anchor.data("selected")) {
                formContaItemUniqueId.find(".undo-item-footer").addClass("hidden");
                formContaItemUniqueId.find(".remove-item-footer").removeClass("hidden");
            } else {
                formContaItemUniqueId.find(".remove-item-footer").addClass("hidden");
                formContaItemUniqueId.find(".undo-item-footer").removeClass("hidden");  
            }

            atualizaTotalSelecionados();
        }

        
        function CriarContaMedicaFormItemPorUniqueId(formContaItemUniqueId) {
            const calcularValorTotalItemFaturamento = _.debounce(calcularValorTotalItemFaturamentoMethod, 500);
            const tabpane = formContaItemUniqueId.parents("div.tab-pane-fat-conta-item");
            let currentAnchor = $modal.find(`a[href='#${tabpane.attr("id")}']`);
            const uniqueId = currentAnchor.data("unique-id");
            formContaItemUniqueId.attr("autocomplete","off");
            selectSWWithDefaultValue(formContaItemUniqueId.find(".selectItem"), '/api/services/app/faturamentoItem/listarDropdown');
            selectSWWithDefaultValue(formContaItemUniqueId.find(".selectlocalUtilizacao"), '/api/services/app/UnidadeOrganizacional/ListarDropdown');
            selectSWWithDefaultValue(formContaItemUniqueId.find(".selectTerceirizado"),"/api/services/app/terceirizado/ListarDropdown");
            selectSWWithDefaultValue(formContaItemUniqueId.find(".selectCentroDeCusto"), '/api/services/app/CentroCusto/ListarDropdownCodigoCentroCusto');
            selectSWWithDefaultValue(formContaItemUniqueId.find(".selectTurno"), "/api/services/app/Turno/ListarDropdown");
            selectSWWithDefaultValue(formContaItemUniqueId.find(".selectTipoAcomodacao"),"/api/services/app/TipoAcomodacao/ListarDropdown");
            formContaItemUniqueId.find(".selectEspecialidade").each(function(){
                const select = $(this);
                selectSWWithDefaultValue(formContaItemUniqueId.find(`[name='${select.attr("name")}']`), "/api/services/app/medicoEspecialidade/ListarDropdownPorMedico", select.parents(".row:first").find(".selectMedico"))
            })

            selectSWWithDefaultValue(formContaItemUniqueId.find(".selectMedico"), "/api/services/app/medico/ListarDropdown")

            selectSWWithDefaultValue(formContaItemUniqueId.find(".procedimentoPrincipal"), '/api/services/app/faturamentoContaItem/ListarDropdown')
            formContaItemUniqueId.find(`#${formatUniqueIdField(uniqueId, 'ValorItem')}`).attr("readonly", "readonly");

            formContaItemUniqueId.find(".selectItem").change(onChangeContaItem);
            formContaItemUniqueId.find('.text-editor').summernote(summerNoteOptions);
            formContaItemUniqueId.find(".btnProximo").click(onBtnProximoClick);
            
            formContaItemUniqueId.find(".undo-item-footer").click(onBtnUndoItemFooterClick);
            formContaItemUniqueId.find(".remove-item-footer").click(onBtnRemoveItemFooterClick);
            createSingleDatePicker(formContaItemUniqueId);
            
            formContaItemUniqueId.find(".form-control").change(onChange);
            formContaItemUniqueId.find(`#${formatUniqueIdField(uniqueId, 'UnidadeOrganizacionalId')}`).change(() => { calcularValorTotalItemFaturamento()});
            formContaItemUniqueId.find(`#${formatUniqueIdField(uniqueId, 'TerceirizadoId')}`).change(() => { calcularValorTotalItemFaturamento()});
            formContaItemUniqueId.find(`#${formatUniqueIdField(uniqueId, 'CentroCustoId')}`).change(() => { calcularValorTotalItemFaturamento()});
            formContaItemUniqueId.find(`#${formatUniqueIdField(uniqueId, 'TurnoId')}`).change(() => { calcularValorTotalItemFaturamento()});
            formContaItemUniqueId.find(`#${formatUniqueIdField(uniqueId, 'TipoLeitoId')}`).change(() => { calcularValorTotalItemFaturamento()});
            
            
            formContaItemUniqueId.find(`#${formatUniqueIdField(uniqueId, 'Data')}`).change(() => { calcularValorTotalItemFaturamento()});
            formContaItemUniqueId.find(`#${formatUniqueIdField(uniqueId, 'Qtde')}`).change(() => { calcularValorTotalItemFaturamento()});
            const percentualItem = formContaItemUniqueId.find(`#${formatUniqueIdField(uniqueId,'PercentualItem')}`);
            percentualItem.change(() => { calcularValorTotalItemFaturamento()});
            percentualItem.val(100);

            formContaItemUniqueId.find(".btn-perc-item").removeClass("btn-primary");
            formContaItemUniqueId.find(`#btn-${percentualItem.val()}-perc-item`).addClass("btn-primary");

            formContaItemUniqueId.find(".btn-perc-item").on("click", (event) => {
                const self = $(event.currentTarget);
                formContaItemUniqueId.find(".btn-perc-item").removeClass("btn-primary");
                self.addClass("btn-primary");
                percentualItem.val(self.data("value"));
                formContaItemUniqueId.find(".valor-perc").text((Number(percentualItem.val())/ 100).toLocaleString('pt-br',{style: 'percent'}));
                calcularValorTotalItemFaturamento();
            })

            initForm();
            
            
            function initForm() {
                calcularValorTotalItemFaturamento();
            }
            
            function onChange(e) {
                criarValidate(formContaItemUniqueId);
            }
            
            function onChangeContaItem(event) {
                const val = $(this).val();
                const data = $(this).select2("data");
                if(data.length) {
                    formContaItemUniqueId.find(".itemDescricao").text(data[0].text);
                } else {
                    formContaItemUniqueId.find(".itemDescricao").text('');
                }
                abp.ui.block();
                if(val) {
                    fatItemAppService.obterTipoGrupoId(val).then(resTipoGrupoId => {
                        return obterContaItem(val)
                            .then(() => {
                                atualizaAbas(resTipoGrupoId);
                                abp.ui.unblock();
                        })
                    })
                } else {
                    atualizaAbas("");
                    abp.ui.unblock();
                }
            }

            function obterContaItem(val) {
                return fatItemAppService.obter(val).then(res => {
                    formContaItemUniqueId.find(`#${formatUniqueIdField(uniqueId, 'ValorItem')}`).attr("readonly", "readonly");
                    if (res && res.isPrecoManual) {
                        formContaItemUniqueId.find(`#${formatUniqueIdField(uniqueId, 'ValorItem')}`).removeAttr("readonly");
                        abp.message.info("Este item permite valor manual.");
                    }
                })
            }

            function atualizaAbas(tipoGrupoId) {
                if (tipoGrupoId === 1) {
                    $('#abaItemHonorarios').fadeIn();
                } else if (tipoGrupoId === 2) {
                    $('#abaItemHonorarios').fadeOut();
                    $('#addItemHonorarios').fadeOut();
                    $("#abaItemPrincipal").tab("show");
                    //$('#abaItemObservacao').tab("show");
                } else if (tipoGrupoId === 3) {
                    $('#abaItemHonorarios').fadeOut();
                    $('#addItemHonorarios').fadeOut();
                    $("#abaItemPrincipal").tab("show");
                    //$('#abaItemObservacao').tab("show");
                } else if (tipoGrupoId === 4 ) {
                    $('#abaItemHonorarios').fadeOut();
                    $('#addItemHonorarios').fadeOut();
                    $("#abaItemPrincipal").tab("show");
                    //$('#abaItemObservacao').tab("show");
                } else if(tipoGrupoId === "" || tipoGrupoId == null || tipoGrupoId === 0) {
                    $('#abaItemHonorarios').fadeOut();
                    $('#addItemHonorarios').fadeOut();
                    $("#abaItemPrincipal").tab("show");
                }
            }
            
           function onBtnRemoveItemFooterClick(e) {
               e.stopImmediatePropagation();
               const btn = $(this);
               btn.buttonBusy(true);
               currentAnchor.data("selected",false);
               currentAnchor.find(".undo-item").removeClass("hidden");
               currentAnchor.find(".remove-item").addClass("hidden");
               btn.addClass("hidden").buttonBusy(false);
               btnRemoverBtnReadicionarVisibilidade(currentAnchor,formContaItemUniqueId)
           }
           
           function onBtnUndoItemFooterClick(e) {
               e.stopImmediatePropagation();
               const btn = $(this);
               btn.buttonBusy(true);
               currentAnchor.data("selected",true);
               currentAnchor.find(".remove-item").removeClass("hidden");
               currentAnchor.find(".undo-item").addClass("hidden");
               btn.addClass("hidden").buttonBusy(false);
               btnRemoverBtnReadicionarVisibilidade(currentAnchor,formContaItemUniqueId)
           }
           
            function onBtnProximoClick(e) {
                e.stopImmediatePropagation();
                const btn = $(this);
                btn.buttonBusy(true);
                const nextAnchor = getNextFiltered(currentAnchor.parents("li")).find("a.fatContaItemAnchor");
                if(currentAnchor.data("selected")) {
                    if(validateForm(formContaItemUniqueId,currentAnchor)) {
                        nextAnchor.tab("show")
                        btn.buttonBusy(false);
                    } else {
                        abp.notify.info("Preencha os campos obrigatórios");
                        btn.buttonBusy(false);
                    }
                } else {
                    nextAnchor.tab("show")
                    btn.buttonBusy(false);
                }
            }

            function calcularValorTotalItemFaturamentoMethod(val) {
                const formContaItemFaturamentoItemId = val ?? formContaItemUniqueId.find(`#${formatUniqueIdField(uniqueId,'FaturamentoItemId')}`).val();
                if(!formContaItemFaturamentoItemId) {
                    formContaItemUniqueId.find(`#${formatUniqueIdField(uniqueId,'ValorItem')}`).val(0);
                    formContaItemUniqueId.find(".valorTotal").text(Number(0).toLocaleString('pt-br',{style: 'currency', currency: 'BRL'}));
                    formContaItemUniqueId.find(`#${formatUniqueIdField(uniqueId,'ResumoDetalhamento')}`).val(JSON.stringify(null));
                    return;
                }

                const honorariosDto = {
                    medicoId: formContaItemUniqueId.find(`#${formatUniqueIdField(uniqueId,"MedicoId")}`).val(),
                    isMedicoCredenciado: formContaItemUniqueId.find(`#${formatUniqueIdField(uniqueId, "MedCredenciado")}`).attr("checked"),
                    medicoEspecialidadeId: formContaItemUniqueId.find(`#${formatUniqueIdField(uniqueId, "MedicoEspecialidadeId")}`).val(),
                    auxiliar1Id: formContaItemUniqueId.find(`#${formatUniqueIdField(uniqueId, "Auxiliar1Id")}`).val(),
                    auxiliar1IsCredenciado: formContaItemUniqueId.find(`#${formatUniqueIdField(uniqueId, "IsAux1Credenciado")}`).attr("checked"),
                    auxiliar1EspecialidadeId: formContaItemUniqueId.find(`#${formatUniqueIdField(uniqueId, "Auxiliar1EspecialidadeId")}`).val(),
                    auxiliar2Id: formContaItemUniqueId.find(`#${formatUniqueIdField(uniqueId, "Auxiliar2Id")}`).val(),
                    auxiliar2IsCredenciado: formContaItemUniqueId.find(`#${formatUniqueIdField(uniqueId, "IsAux2Credenciado")}`).attr("checked"),
                    auxiliar2EspecialidadeId: formContaItemUniqueId.find(`#${formatUniqueIdField(uniqueId, "Auxiliar2EspecialidadeId")}`).val(),
                    auxiliar3Id: formContaItemUniqueId.find(`#${formatUniqueIdField(uniqueId, "Auxiliar3Id")}`).val(),
                    auxiliar3IsCredenciado: formContaItemUniqueId.find(`#${formatUniqueIdField(uniqueId, "IsAux3Credenciado")}`).attr("checked"),
                    auxiliar3EspecialidadeId: formContaItemUniqueId.find(`#${formatUniqueIdField(uniqueId, "Auxiliar3EspecialidadeId")}`).val(),
                    instrumentadorId: formContaItemUniqueId.find(`#${formatUniqueIdField(uniqueId, "InstrumentadorId")}`).val(),
                    instrumentadorIsCredenciado: formContaItemUniqueId.find(`#${formatUniqueIdField(uniqueId, "IsInstrCredenciado")}`).attr("checked"),
                    instrumentadorEspecialidadeId: formContaItemUniqueId.find(`#${formatUniqueIdField(uniqueId, "InstrumentadorEspecialidadeId")}`).val(),
                    anestesistaId: formContaItemUniqueId.find(`#${formatUniqueIdField(uniqueId, "AnestesistaId")}`).val(),
                    anestesistaIsCredenciado: formContaItemUniqueId.find(`#${formatUniqueIdField(uniqueId, "IsAnestCredenciado")}`).attr("checked"),
                    anestesistaEspecialidadeId: formContaItemUniqueId.find(`#${formatUniqueIdField(uniqueId, "EspecialidadeAnestesistaId")}`).val(),
                    procedimentoPrincipal: formContaItemUniqueId.find(`#${formatUniqueIdField(uniqueId, "ProcedimentoPrincipalId")}`).val(),
                    Percentual: formContaItemUniqueId.find(`#${ formatUniqueIdField(uniqueId, 'PercentualItem') }`).val() || 100
                }

                const valorTotalItemFaturamentoDto = {
                    contaMedicaId:$("#contaMedicaId").val(),
                    FaturamentoItemId: formContaItemFaturamentoItemId,
                    Data: formContaItemUniqueId.find(`#${formatUniqueIdField(uniqueId,'Data')}`).val(),
                    Qtd: formContaItemUniqueId.find(`#${formatUniqueIdField(uniqueId,'Qtde')}`).val(),
                    Percentual: formContaItemUniqueId.find(`#${formatUniqueIdField(uniqueId, 'PercentualItem')}`).val() || 100,
                    unidadeOrganizacionalId: formContaItemUniqueId.find(`#${formatUniqueIdField(uniqueId, 'UnidadeOrganizacionalId')}`).val(),
                    terceirizadoId: formContaItemUniqueId.find(`#${formatUniqueIdField(uniqueId, 'TerceirizadoId')}`).val(),
                    centroCustoId: formContaItemUniqueId.find(`#${formatUniqueIdField(uniqueId, 'CentroCustoId')}`).val(),
                    turnoId: formContaItemUniqueId.find(`#${formatUniqueIdField(uniqueId, 'TurnoId')}`).val(),
                    tipoLeitoId: formContaItemUniqueId.find(`#${formatUniqueIdField(uniqueId, 'TipoLeitoId')}`).val(),
                    honorariosDto
                };
                contaItemAppService.calcularValorTotalItemFaturamento(valorTotalItemFaturamentoDto).then(atualizarValorTotalItemFaturamento);
            }

            function atualizarValorTotalItemFaturamento(res) {
                if (res.errors.length) {
                    let errors = res.errors.map(mapWarningAndErrors).join("\n");
                    abp.notify.error(errors);
                }

                if (res.warnings.length) {
                    let warnings = res.warnings.map(mapWarningAndErrors).join("\n");
                    abp.notify.warn(warnings);
                }

                if(!res.errors.length) {
                    formContaItemUniqueId.find(`#${formatUniqueIdField(uniqueId,'ValorItem')}`).val(res.returnObject.valor);
                    if (!res.returnObject.valorTotal) {
                        res.returnObject.valorTotal = 0;
                    }
                    if (res.returnObject.resumoDetalhamento) {
                        formContaItemUniqueId.find(`#${formatUniqueIdField(uniqueId,'ResumoDetalhamento')}`).val(JSON.stringify(res.returnObject.resumoDetalhamento))
                        atualizaCardsValor(res.returnObject.resumoDetalhamento);

                    } else {
                        formContaItemUniqueId.find(`#${formatUniqueIdField(uniqueId,'ResumoDetalhamento')}`).val(JSON.stringify(null));
                        atualizaCardsValor(null);
                    }
                    abp.notify.info("Valor Únitario calculado!");
                }

                function mapWarningAndErrors(x) {
                    if (x.codigo) {
                        return `${x.codigo} - ${x.descricao}`
                    }
                    return `${x.descricao}`;
                }
            }

            function atualizaCardsValor(resumoDetalhamento) {
                formContaItemUniqueId.find(".valor-perc").text((Number(formContaItemUniqueId.find(`#${formatUniqueIdField(uniqueId,'PercentualItem')}`).val())/ 100).toLocaleString('pt-br',{style: 'percent'}));
                if(resumoDetalhamento) {
                    formContaItemUniqueId.find(".valorPorte").text(resumoDetalhamento.valorPorte.toLocaleString('pt-br',{style: 'currency', currency: 'BRL'}));
                    formContaItemUniqueId.find(".valorFilme").text(resumoDetalhamento.valorFilme.toLocaleString('pt-br',{style: 'currency', currency: 'BRL'}));
                    formContaItemUniqueId.find(".valorTaxas").text(resumoDetalhamento.valorTaxas.toLocaleString('pt-br',{style: 'currency', currency: 'BRL'}));
                    formContaItemUniqueId.find(".valorItem").text(resumoDetalhamento.valor.toLocaleString('pt-br', { style: 'currency', currency: 'BRL' }));
                    formContaItemUniqueId.find(".valorCOCH").text(resumoDetalhamento.valorCOCH.toLocaleString('pt-br', { style: 'decimal' }));
                    formContaItemUniqueId.find(".valorHMCH").text(resumoDetalhamento.valorHMCH.toLocaleString('pt-br', { style: 'decimal' }));
                    formContaItemUniqueId.find(".valorTotal").text(resumoDetalhamento.valorTotal.toLocaleString('pt-br', { style: 'currency', currency: 'BRL' }));
                    if(resumoDetalhamento.tabela) {
                        formContaItemUniqueId.find(".valorTabela").text(resumoDetalhamento.tabela.descricao);
                    } else {
                        formContaItemUniqueId.find(".valorTabela").text("");
                    }
                } else {
                    const zero = Number(0);
                    formContaItemUniqueId.find(".valorPorte").text(zero.toLocaleString('pt-br',{style: 'currency', currency: 'BRL'}));
                    formContaItemUniqueId.find(".valorFilme").text(zero.toLocaleString('pt-br',{style: 'currency', currency: 'BRL'}));
                    formContaItemUniqueId.find(".valorTaxas").text(zero.toLocaleString('pt-br', { style: 'currency', currency: 'BRL' }));
                    formContaItemUniqueId.find(".valorItem").text(zero.toLocaleString('pt-br', { style: 'currency', currency: 'BRL' }));
                    formContaItemUniqueId.find(".valorCOCH").text(zero.toLocaleString('pt-br', { style: 'decimal' }));
                    formContaItemUniqueId.find(".valorHMCH").text(zero.toLocaleString('pt-br', { style: 'decimal' }));
                    formContaItemUniqueId.find(".valorTotal").text(zero.toLocaleString('pt-br',{style: 'currency', currency: 'BRL'}));
                    formContaItemUniqueId.find(".valorTabela").text("");
                }
            }
            
            function formatUniqueIdField(uniqueId, field) {
                return `formContaItem_${uniqueId}_${field}`;
            }
            
        }

        function validateForm(formContaItemUniqueId,currentAnchor) {
            criarValidate(formContaItemUniqueId);

            if(!formContaItemUniqueId.valid()) {
                currentAnchor.addClass("error");
            } else {
                currentAnchor.removeClass("error");
            }
            return formContaItemUniqueId.valid();
        }
        
        function criarValidate(formContaItemUniqueId) {
            const grupoId = formContaItemUniqueId.find("input[name='grupoId']").val();
            const namesEl = formContaItemUniqueId.find("[name]").not("input[type='hidden']");
            let rules = {};
            _.forEach(namesEl, (el) => {
                let currentRule = {};
                const jQEl = $(el);
                if(jQEl.data("ruleRequired")) {
                    currentRule["required"] = true;
                }

                if(jQEl.data("ruleGreaterThanZero")) {
                    currentRule["greaterThanZero"] = true;
                }
                if(!_.isEmpty(currentRule)) {
                    rules[jQEl.attr("name")] = currentRule;
                }
            });
            
            return formContaItemUniqueId.validate({
                ignore:'input[type="hidden"]',
                rules});
        }

        function validateDefinirParaTodosForm(formContaItem) {
            formContaItem.validate();
            const tabPane = formContaItem.parents(".tab-pane-fat-conta-item");
            const currentAnchor = liDefinirParaTodos.find(`a[href="#${tabPane.attr("id")}"]`)
            if(!formContaItem.valid()) {
                currentAnchor.addClass("error");
            } else {
                currentAnchor.removeClass("error");
            }

            return formContaItem.valid();
        }
    }
})(jQuery);