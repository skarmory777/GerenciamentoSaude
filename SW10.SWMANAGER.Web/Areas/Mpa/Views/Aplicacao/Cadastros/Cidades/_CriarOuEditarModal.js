(function ($) {
    app.modals.CriarOuEditarCidadeModal = function () {

        var _cidadesService = abp.services.app.cidade;

        var _modalManager;
        var _$cidadesInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$cidadeInformationForm = _modalManager.getModal().find('form[name=CidadeInformationsForm]');
            _$cidadeInformationForm.validate();
            //$('.modal-dialog').css('width', '1100px');
            //$('select').addClass('form-control edited');
        };

        this.save = function () {
            if (!_$cidadeInformationForm.valid()) {
                return;
            }

            var cidade = _$cidadeInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);

            _cidadesService.criarOuEditar(cidade)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarCidadeModalSaved');
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);