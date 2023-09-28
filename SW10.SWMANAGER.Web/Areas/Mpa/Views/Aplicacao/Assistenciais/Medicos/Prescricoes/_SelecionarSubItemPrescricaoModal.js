(function($) {
    app.modals.selecionarSubItemPrescricaoModal = function () {
        const prescricoesItensService = abp.services.app.prescricaoItem;
        const subItensPrescricaoTable = $("#subItensPrescricaoTable");
        let modalManager;
        this.init = function (_modalManager) {
            modalManager = _modalManager;
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
            $(_modalManager.getModal()).find(".modal-dialog").css({ 'width': '80%'});
            getSubPrescricoesItens();
            abp.ui.clearBusy()
        }

        subItensPrescricaoTable.jtable({
            title: app.localize('Sub Itens Prescrição'),
            paging: true,
            sorting: true,
            multiSorting: true,
            actions: {
                listAction: {
                    method: prescricoesItensService.listarSelecionarPorPrescricaoItemId
                }
            },
            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    
                    width: '5%',
                    sorting: false,
                    display: function (data) {
                        let $span = $('<span></span>');
                        $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fas fa-hand-pointer"></i></button>').appendTo($span).on("click", function (e) {
                            selectSubItem(data.record, e);
                        })
                        return $span;
                    },
                },
                descricao: {
                    width: '95%',
                    title: app.localize('Descrição'),
                    sorting: false,
                    display:function(data) {
                        return displayDescricao(data.record);
                    }
                },
            }
        });
        
        function selectSubItem(data, e) {
            abp.event.trigger("selecionarSubItemPrescricao", data);
            modalManager.close();
        }

        function getSubPrescricoesItens() {
            subItensPrescricaoTable.jtable('load', {
                id: modalManager.getArgs().prescricaoItemId
            });
        }
        function displayDescricao(record) {
            let span = $("<div class='col-md-12'></div>")
            const divQtd = record.quantidade ?
                `<div class="col-md-2">
                    <span class="font-weight-bold">Qtd:</span> ${record.quantidade}
                </div>` : '';
            const divObservacao = record.observacao ? 
                ` <div class="row"> 
                    <div class="col-md-12">
                        <span class="font-weight-bold">Observação:</span> ${record.observacao}
                    </div>
                </div>` : '';
            
            const divUnidade = record.unidade ?
                `<div class="col-md-3">
                    <span class="font-weight-bold">Unidade:</span> ${record.unidade}
                </div>`: '';
            const divForma = record.formaAplicacao ?
                `<div class="col-md-3">
                    <span class="font-weight-bold">Forma:</span> ${record.formaAplicacao}
                </div>`: '';
            const divVia = record.viaAplicacao ?
                `<div class="col-md-3">
                    <span class="font-weight-bold">Via:</span> ${record.viaAplicacao}
                </div>`: '';

            const divDiluente = record.diluente ?
                `<div class="col-md-3">
                    <span class="font-weight-bold">Diluente:</span> ${record.diluente}
                </div>`: '';
            
            span.append($(`
                <div class="row"> 
                    <div class="col-md-${(divQtd === ''? 12:10)}">
                        <span class="font-weight-bold">Descrição:</span> ${record.descricao}
                    </div> 
                    ${divQtd}
                </div>
                <div class="row">
                    ${divUnidade}
                    ${divForma}
                    ${divVia}
                    ${divDiluente}
                </div>
                ${divObservacao}`))
            
            return span;
        }
    }
})(jQuery);