(function ($) {
    app.modals.CriarOuEditarFaturamentoBrasPrecoModal = function () {

        var _brasPrecosService = abp.services.app.faturamentoBrasPreco;
        var _modalManager;
        var _$brasPrecoInformationForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$brasPrecoInformationForm = _modalManager.getModal().find('form[name=BrasPrecoInformationsForm]');
            _$brasPrecoInformationForm.validate({ ignore: "" });
            $('.modal-dialog').css('width', '1000px');

            //// Select2 e filtros de combo
            //selectSW(".select2Produto", "/api/services/app/produto/ListarDropdownParaBrasPreco");
            //selectSW(".select2BrasApresentacao", "/api/services/app/faturamentoBrasApresentacao/ListarDropdown");
            //selectSW(".select2BrasLaboratorio", "/api/services/app/faturamentoBrasLaboratorio/ListarDropdown");
            //// Combos filtros
            //// Grupo por tipo
            //$("#combo-produto").on("change", function () {
            //    var produtoId = $(this).val();
            //    selectSW(".select2BrasApresentacao", "/api/services/app/faturamentoBrasApresentacao/ListarDropdown", produtoId);
            //});
            //// SubGrupo por grupo
            //$("#combo-brasApresentacao").on("change", function () {
            //    var apresentacaoId = $(this).val();
            //    selectSW(".select2BrasLaboratorio", "/api/services/app/faturamentoBrasLaboratorio/ListarDropdown", apresentacaoId);
            //});
        };

        this.save = function () {
            if (!_$brasPrecoInformationForm.valid()) {
                return;
            }

            var brasPreco = _$brasPrecoInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);

            brasPreco.Preco = retirarMascara(brasPreco.Preco);

            _brasPrecosService.criarOuEditar(brasPreco)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarBrasPrecoModalSaved');
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);