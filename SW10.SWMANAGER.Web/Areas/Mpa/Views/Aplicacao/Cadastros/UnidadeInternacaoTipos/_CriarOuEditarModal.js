(function ($) {
    app.modals.CriarOuEditarUnidadeInternacaoTipoModal = function () {

        var _unidadeInternacaoTiposService = abp.services.app.unidadeInternacaoTipo;
        var _modalManager;
        var _$unidadeInternacaoTipoInformationForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$unidadeInternacaoTipoInformationForm = _modalManager.getModal().find('form[name=UnidadeInternacaoTipoInformationsForm]');
            _$unidadeInternacaoTipoInformationForm.validate({ ignore: "" });
            //$('.modal-dialog').css('width', '1100px');
            //$('select').addClass('form-control edited');
        };

        this.save = function () {
            if (!_$unidadeInternacaoTipoInformationForm.valid()) {
                return;
            }

            var unidadeInternacaoTipo = _$unidadeInternacaoTipoInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);

            _unidadeInternacaoTiposService.criarOuEditar(unidadeInternacaoTipo)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarUnidadeInternacaoTipoModalSaved');
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);