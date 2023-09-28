(function ($) {
    app.modals.CriarOuEditarPacoteModal = function () {
        let modalManager;
        let $modal = null;
        const PacoteContaMedicaId = $("#PacoteContaMedicaId").val()
        const fatItemAppService = abp.services.app.faturamentoItem;
        const contaItemAppService = abp.services.app.faturamentoContaItem;
        this.init = function (_modalManager) {
            abp.ui.clearBusy()
            modalManager = _modalManager;
            $modal = $(_modalManager.getModal());
            $modal.find(".modal-dialog").css({ 'width': '95%'});
            $modal.find(".filtrar-descricao").keyup(onFiltarItems);
            $modal.find(".filtrar-grupo").change(onFiltarItems);
            $modal.find(".remove-item").click(onRemoveItem);
            $modal.find(".undo-item").click(onUndoItem);
            $modal.find(".remove-grupo").click(onRemoveGrupo);
            $modal.find(".add-grupo").click(onAddGrupo);
            atualizaGruposInicial();
            atualizaTotalSelecionados();
            CamposRequeridos();
        }
        
        this.save = function() {
            debugger;
            const validateItems = onValidateItems();
            if(validateItems.valid) {
                abp.message.confirm(`Deseja incluir todos os <b>${$modal.find(".selecionados").text()}</b> items selecionados do Pacote: <b>${$modal.find("#Pacote-descricao").text()}</b> na conta médica?`, "",
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
                // validateItems.anchor.focus();
                // validateItems.anchor.tab("show");
            }
        }
        // por definição o elemento que retorna do erro é o primeiro, mas ele valida todos os seguintes
        function onValidateItems() {
            let result = {
                anchor:undefined,
                valid:true,
                formsData:[]
            };

            const items = $modal.find(".list-fat-items div.fatContaItemAnchor").filter(function () {
                return $(this).data("selected") === true
            });

            result.valid = items.length > 0;

            result.formsData = items.map((index, item) => {
                return {
                    id: $(item).data("item-id")
                }
            }).get();
            return result;
        }
        
        function onSave(formsData) {
            const data = {
                id: $modal.find("#PacoteId").val(),
                faturamentoContaId: PacoteContaMedicaId,
                faturamentoItemId: $modal.find("#PacoteFaturamentoItemId").val(),
                Inicio: $modal.find("#PacoteDataInicio").val(),
                Final: $modal.find("#PacoteDataFim").val(),
                horaIncio: $modal.find("#PacoteHoraIncio").val(),
                horaFim: $modal.find("#PacoteHoraFim").val(),
                quantidade: $modal.find("#PacoteQtde").val(),
                unidadeOrganizacionalId: $modal.find("#PacoteUnidadeOrganizacionalId").val(),
                terceirizadoId: $modal.find("#PacoteTerceirizadoId").val(),
                centroCustoId: $modal.find("#PacoteCentroCustoId").val(),
                turnoId: $modal.find("#PacoteTurnoId").val(),
                tipoLeitoId: $modal.find("#PacoteTipoLeitoId").val(),
                items: formsData
            }

            contaItemAppService.incluirPacote(data).then(res=> {
                abp.notify.success("Pacote incluido com sucesso");
                modalManager.close();
                abp.event.trigger("app.contaMedicaReload");
            });
        }

        
        function onRemoveItem(e) {
            debugger;
            e.stopImmediatePropagation();
            const btn = $(this);
            btn.buttonBusy(true);
            const div = btn.parents("div.fatContaItemAnchor");
            div.data("selected",false);
            div.find(".undo-item").removeClass("hidden");
            btn.addClass("hidden").buttonBusy(false);
            atualizaTotalSelecionados()
            atualizaTotalSelecionadosPorGrupo(div.data("item-grupo-id"));
        }

        function onUndoItem(e) {
            debugger;
            e.stopImmediatePropagation();
            const btn = $(this);
            btn.buttonBusy(true);
            const div = btn.parents("div.fatContaItemAnchor");
            div.data("selected",true);
            div.find(".remove-item").removeClass("hidden");
            btn.addClass("hidden").buttonBusy(false);
            atualizaTotalSelecionados()
            atualizaTotalSelecionadosPorGrupo(div.data("item-grupo-id"));
        }

        function onRemoveGrupo(e) {
            debugger;
            e.stopImmediatePropagation();
            const btn = $(this);
            btn.buttonBusy(true);
            const div = btn.parents("div.aspect-tab");
            div.find("div.fatContaItemAnchor").each(function() {
                const fatContaItemAnchor = $(this);
                fatContaItemAnchor.data("selected",false);
                fatContaItemAnchor.find(".undo-item").removeClass("hidden");
                fatContaItemAnchor.find(".remove-item").addClass("hidden")
            })
            btn.buttonBusy(false);
            
            atualizaTotalSelecionados()
            atualizaTotalSelecionadosPorGrupo(div.data("grupo-id"));
        }

        function onAddGrupo(e) {
            debugger;
            e.stopImmediatePropagation();
            const btn = $(this);
            btn.buttonBusy(true);
            const div = btn.parents("div.aspect-tab");
            div.find("div.fatContaItemAnchor").each(function() {
                const fatContaItemAnchor = $(this);
                fatContaItemAnchor.data("selected",true);
                fatContaItemAnchor.find(".remove-item").removeClass("hidden");
                fatContaItemAnchor.find(".undo-item").addClass("hidden")
            })
            btn.buttonBusy(false);

            atualizaTotalSelecionados()
            atualizaTotalSelecionadosPorGrupo(div.data("grupo-id"));
        }
        
        function atualizaGruposInicial() {
            $modal.find(".aspect-tab").each(function() {
                const grupoId = $(this).data("grupo-id");
                atualizaTotalSelecionadosPorGrupo(grupoId);
            })
        }
        
        function atualizaTotalSelecionados() {
            $modal.find(".selecionados").text($modal.find(".list-fat-items div.fatContaItemAnchor").filter(function() {
                return $(this).data("selected") == true
            }).length);
        }
        
        function atualizaTotalSelecionadosPorGrupo(groupId) {
            const grupoDiv = $modal.find(".aspect-tab").filter(function() {
                return $(this).data("grupo-id") == groupId;
            });
            grupoDiv.find(".selecionados-grupo").text(grupoDiv.find(".list-fat-items div.fatContaItemAnchor").filter(function() {
                return $(this).data("selected") == true
            }).length)
            
        }

        function onFiltarItems() {
            const descricao = $modal.find(".filtrar-descricao").val();
            let grupos =  $(".filtrar-grupo").filter(":checked").map((index,el)=> parseInt($(el).val())).get();
            
            if(!descricao.length && !grupos.length) {
                $modal.find(".list-fat-items div").removeClass("hidden");
            } else {
                $modal.find(".list-fat-items div.fatContaItemAnchor").each(function () {
                    const div = $(this);
                    const grupoId = parseInt(div.data("item-grupo"));
                    const itemDescricao = div.data("item-descricao") ? div.data("item-descricao").toLowerCase() : "";
                    if (!grupos.length) {
                        if (descricao.length === 0) {
                            $modal.find(".list-fat-items div").removeClass("hidden");
                        } else if (itemDescricao.includes(descricao.toLowerCase())) {
                            div.removeClass("hidden");
                        } else {
                            div.addClass("hidden");
                        }
                    } else {
                        if (descricao.length === 0 && _.includes(grupos, grupoId)) {
                            div.removeClass("hidden");
                        } else if (itemDescricao.includes(descricao.toLowerCase()) && _.includes(grupos, grupoId)) {
                            div.removeClass("hidden");
                        } else {
                            div.addClass("hidden");
                        }
                    }
                });
                $modal.find('.total-filtrado').text($modal.find(".list-fat-items div:not(.hidden)").length);
            }
        }
        
    }
})(jQuery);