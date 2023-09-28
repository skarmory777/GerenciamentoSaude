(function ($) {
    app.modals.CriarOuEditarUnidadeInternacaoModal = function () {

        var _unidadesInternacaoService = abp.services.app.unidadeInternacao;
        var _modalManager;
        var _$unidadeInternacaoInformationForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$unidadeInternacaoInformationForm = _modalManager.getModal().find('form[name=UnidadeInternacaoInformationsForm]');
            _$unidadeInternacaoInformationForm.validate({ ignore: "" });
            //$('.modal-dialog').css('width', '1100px');
            //$('select').addClass('form-control edited');
        };

        this.save = function () {

            //alert('entrou no save');

            if (!_$unidadeInternacaoInformationForm.valid()) {
                abp.notify.error('invalid form');
                return;
            }

            var unidadeInternacao = _$unidadeInternacaoInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);

            //  alert('pos modal busy');

            _unidadesInternacaoService.criarOuEditar(unidadeInternacao)
                .done(function () {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    _modalManager.close();
                    abp.event.trigger('app.CriarOuEditarUnidadeInternacaoModalSaved');
                })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);