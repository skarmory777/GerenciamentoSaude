(function ($) {
    app.modals.CriarOuEditarOrigemModal = function () {

        var _origensService = abp.services.app.origem;

        var _modalManager;
        var _$origemInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$origemInformationForm = _modalManager.getModal().find('form[name=OrigemInformationsForm]');
            _$origemInformationForm.validate({ ignore: "" });
            //$('.modal-dialog').css('width', '1100px');
            //$('select').addClass('form-control edited');
        };

        this.save = function () {
            if (!_$origemInformationForm.valid()) {
                return;
            }

            var origem = _$origemInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);

            _origensService.criarOuEditar(origem)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarOrigemModalSaved');
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
        aplicarDateRange();
        aplicarDateSingle();
        aplicarSelect2Padrao();
        CamposRequeridos();
    };
})(jQuery);