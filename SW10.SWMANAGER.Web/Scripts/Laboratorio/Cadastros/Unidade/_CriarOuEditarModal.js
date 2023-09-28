(function ($) {
    app.modals.CriarOuEditarUnidadeModal = function () {

        var _unidadeesService = abp.services.app.unidadeLaboratorio;

        var _modalManager;
        var _$unidadeInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$unidadeInformationForm = _modalManager.getModal().find('form[name=UnidadeInformationsForm]');
            _$unidadeInformationForm.validate({ ignore: "" });
            //$('.modal-dialog').css('width', '1100px');
            //$('select').addClass('form-control edited');
        };

        this.save = function () {
            if (!_$unidadeInformationForm.valid()) {
                return;
            }

            var unidade = _$unidadeInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);

            _unidadeesService.criarOuEditar(unidade)
                .done(function () {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    _modalManager.close();
                    abp.event.trigger('app.CriarOuEditarUnidadeModalSaved');
                })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);