(function ($) {
    app.modals.CriarOuEditarFaturamentoBrasItemModal = function () {

        var _brasItensService = abp.services.app.faturamentoBrasItem;
        var _modalManager;
        var _$brasItemInformationForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$brasItemInformationForm = _modalManager.getModal().find('form[name=BrasItemInformationsForm]');
            _$brasItemInformationForm.validate({ ignore: "" });
            $('.modal-dialog').css('width', '600px');
        };

        this.save = function () {
            if (!_$brasItemInformationForm.valid()) {
                return;
            }

            var brasItem = _$brasItemInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);

            _brasItensService.criarOuEditar(brasItem)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarBrasItemModalSaved');
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);