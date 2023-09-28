(function ($) {
    app.modals.CriarOuEditarTabelaPrecoConvenioModal = function () {

        var _kitsService = abp.services.app.tabelaPrecoConvenio;
        var _modalManager;
        var _$kitInformationForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$kitInformationForm = _modalManager.getModal().find('form[name=KitInformationsForm]');
            _$kitInformationForm.validate({ ignore: "" });
            $('.modal-dialog:last').css('width', '95%');
        };

        this.save = function () {
            if (!_$kitInformationForm.valid()) {
                return;
            }

            var kit = _$kitInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);

            _kitsService.criarOuEditar(kit)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarKitModalSaved');
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };

        $('#inserir-item').click(function () {
            _kitsService.inserirItem();
        });
    };
})(jQuery);