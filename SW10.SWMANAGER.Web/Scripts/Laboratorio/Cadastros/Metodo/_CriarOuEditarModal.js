(function ($) {
    app.modals.CriarOuEditarMetodoModal = function () {

        var _metodoesService = abp.services.app.cadastros;

        var _modalManager;
        var _$metodoInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$metodoInformationForm = _modalManager.getModal().find('form[name=MetodoInformationsForm]');
            _$metodoInformationForm.validate({ ignore: "" });
            //$('.modal-dialog').css('width', '1100px');
            //$('select').addClass('form-control edited');
        };

        this.save = function () {
            if (!_$metodoInformationForm.valid()) {
                return;
            }

            var metodo = _$metodoInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);

            _metodoesService.criarOuEditar(metodo)
                .done(function () {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    _modalManager.close();
                    abp.event.trigger('app.CriarOuEditarMetodoModalSaved');
                })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);