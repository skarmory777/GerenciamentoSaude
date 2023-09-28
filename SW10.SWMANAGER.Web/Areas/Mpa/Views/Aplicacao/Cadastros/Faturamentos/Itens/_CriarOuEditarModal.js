(function ($) {
    app.modals.CriarOuEditarFaturamentoItemModal = function () {

        var _itensService = abp.services.app.faturamentoItem;
        var _modalManager;
        var _$itemInformationForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$itemInformationForm = _modalManager.getModal().find('form[name=ItemInformationsForm]');
            _$itemInformationForm.validate({ ignore: "" });
            $('.modal-dialog').css('width', '1000px');
        };

        this.save = function () {
            if (!_$itemInformationForm.valid()) {
                return;
            }

            var item = _$itemInformationForm.serializeFormToObject();
        //    debugger
            item.Codigo = $('input[name="Codigo"]').val();

            _modalManager.setBusy(true);

            _itensService.criarOuEditar(item)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarItemModalSaved');
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);