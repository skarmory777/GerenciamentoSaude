(function ($) {
    
    app.modals.DetalhamentoExameModal = function () {
        const ocorrenciaAppService = abp.services.app.ocorrencia;
        const resultadoExame = abp.services.app.resultadoExame;
        const gridOcorrencia = $('.grid-ocorrencia');
        let modalManager;
        let $modal;
        this.init = function (_modalManager) {
            modalManager = _modalManager;
            $modal = $(_modalManager.getModal());
            $modal.find(".modal-dialog").css({ 'width': '85%'});

            const gridOcorrenciasOptions = AgGridHelper.createAgGrid('grid-detalhamento-exame-ocorrencia', {
                gridName: 'grid-ocorrencias',
                columnDefs: defColumnsOcorrencias(),
                data: {
                    callback: ocorrenciaAppService.listar,
                    enablePagination: true,
                    autoInitialLoad: true,
                    getData() {
                        return {
                            sourceModel: "resultadoExame",
                            sourceId: $modal.find("#resultado_exame_id").val()
                        }
                    },
                },
                // [AgGridHelper.HOOKS.BEFORE_CREATED](hookData) {
                //     gridOcorrencia.css('width',$modal.find(".modal-body").width());
                // },
            });
            gridOcorrenciasOptions.render(gridOcorrencia[0])
        }

        function defColumnsOcorrencias() {
            const disableFilterAndMenu = {filter: false,  suppressMenu: true};
            return [
                AgGridHelper.columns.dateTime('data',app.localize('Data'), disableFilterAndMenu),
                AgGridHelper.columns.base('tipoOcorrenciaDescricao',app.localize('Tipo'),  disableFilterAndMenu,{width:150,suppressSizeToFit:true}),
                AgGridHelper.columns.base('subTipoOcorrenciaDescricao',app.localize('Sub Tipo'),  disableFilterAndMenu,{width:150,suppressSizeToFit:true}),
                AgGridHelper.columns.base('origem',app.localize('Origem'),  disableFilterAndMenu, {width:150,suppressSizeToFit:true}),
                AgGridHelper.columns.base('texto',app.localize('Ocorrencia'),  disableFilterAndMenu),
                //AgGridHelper.columns.base('usuario',app.localize('Usuario'),  disableFilterAndMenu),
                AgGridHelper.columns.boolean('isSistema',app.localize('Sistema?'),  disableFilterAndMenu),
            ];
        }
        
        this.save = function () {
            const btn = $(this);
            btn.buttonBusy(true);
            resultadoExame.atualizarObservacao($modal.find("#resultado_exame_id").val(), $modal.find(".textarea-observacao").val())
                .then(()=> {
                    abp.notify.success("observação atualizada com sucesso.");
                })
                .always(()=> {
                    modalManager.close();
            });
        }
    }
})(jQuery)