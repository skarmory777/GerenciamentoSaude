(function ($) {
    app.modals.CriarOuEditarPreMovimentoKitEstoqueItemModal = function () {

        var _estoqueKitItemAppService = abp.services.app.estoqueKitItem;
        var _produtoAppService = abp.services.app.produto;
        var _unidadeService = abp.services.app.unidade;

        var _modalManager;
        var _$PreMovimentoKitEstoqueItemInformationForm = null;

        var lista = [];

        $(document).ready(function () {
            $('#Quantidade').mask('000.000.000,00', { reverse: true });
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$PreMovimentoKitEstoqueItemInformationForm = _modalManager.getModal().find('form[name=PreMovimentoKitEstoqueItemInformationsForm]');
            _$PreMovimentoKitEstoqueItemInformationForm.validate();
            $('ul.ui-autocomplete').css('z-index', '2147483647 !important');
        };

        this.save = function () {
            if (!_$PreMovimentoKitEstoqueItemInformationForm.valid()) {
                return;
            }

            _modalManager.setBusy(true);

            var kitEstoqueItemForm = _$PreMovimentoKitEstoqueItemInformationForm.serializeFormToObject();

            _estoqueKitItemAppService.listarPeloKitEstoqueIdEEstoqueId(kitEstoqueItemForm.KitEstoqueId, $('#EstoqueId').val(), { async: false }).done(function (estoqueKitItens) {
                for (var i = 0; i < estoqueKitItens.length; i++) {
                    var preMovimentoItem = {}
                    preMovimentoItem.Id = $("#PreMovimentoItemId").val();
                    preMovimentoItem.IdGrid = $('#idGrid').val();
                    preMovimentoItem.Quantidade = retirarMascara(kitEstoqueItemForm.Quantidade) * retirarMascara(estoqueKitItens[i].quantidade.toString());
                    preMovimentoItem.EstoqueKitItemId = estoqueKitItens[i].id;

                    _produtoAppService.obterUnidadePorTipo(estoqueKitItens[i].produto.id, 1, { async: false }).done(function (produtoUnidade) {
                        _unidadeService.obterQuantidadeReferencia(produtoUnidade.id, preMovimentoItem.Quantidade, { async: false }).done(function (data) {
                            preMovimentoItem.Quantidade = data;
                            if ($('#itens').val() != '') {
                                lista = JSON.parse($('#itens').val());
                            }

                            if ($('#idGrid').val() != '') {
                                var index = _.findIndex(lista, (x) => x.IdGrid === parseInt($('#idGrid').val()) || x.idGrid === parseInt($('#idGrid').val()));
                                if (index !== -1) {
                                    lista[index]["QuantidadeSolicitada"] = lista[index]["quantidade"] = lista[index]["Quantidade"] = preMovimentoItem.Quantidade;
                                    lista[index]["ProdutoId"] = lista[index]["produtoId"] = estoqueKitItens[i].produto.id;
                                    lista[index].ProdutoUnidadeId = lista[index]["produtoUnidadeId"] = produtoUnidade.id;
                                }
                            }
                            else {
                                if (lista.length == 0) {
                                    preMovimentoItem.IdGrid = preMovimentoItem.idGrid = 1;
                                }
                                else {
                                    const valor = (lista[lista.length - 1].IdGrid ?? lista[lista.length - 1].idGrid) + 1;
                                    preMovimentoItem.IdGrid = preMovimentoItem.idGrid = valor;
                                }

                                preMovimentoItem.Id = 0;
                                preMovimentoItem["ProdutoId"] = preMovimentoItem["ProdutoId"] = estoqueKitItens[i].produto.id;
                                preMovimentoItem["ProdutoUnidadeId"] = preMovimentoItem["ProdutoUnidadeId"] = produtoUnidade.id;
                                lista.push(preMovimentoItem);
                            }
                            if (lista && lista.length) {
                                idGrid = 1;
                                _.forEach(lista, (x) => x.IdGrid = x.idGrid = idGrid++);
                            }
                            $('#itens').val(JSON.stringify(lista));
                        });
                    });
                }

                _modalManager.setBusy(false);
            });

            abp.notify.info(app.localize('SavedSuccessfully'));
            abp.event.trigger('app.CriarOuEditarPreMovimentoItemModalSaved');

            $("#PreMovimentoItemId").val(0);
            $('#produtoId').val(null).trigger("change");
            $('#QuantidadeItemid').val('');
            $('#idGrid').val('');
            $('#ProdutoUnidadeId').val(null).trigger("change");
            $('#produtoId').focus();
            $("#produtoId").removeAttr("required");
            $("#QuantidadeItemid").removeAttr("required");
            $("#ProdutoUnidadeId").removeAttr("required");
            $("#QuantidadeItemid").removeData();
            CamposRequeridos();
            $('form[name=preMovimentoInformationsForm]').validate();
            _modalManager.close();
        };
    };
})(jQuery);