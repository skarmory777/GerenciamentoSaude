(function ($) {
    app.modals.CriarOuEditarTecnicoModal = function () {

        var _tecnicoesService = abp.services.app.tecnico;

        var _modalManager;
        var _$tecnicoInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$tecnicoInformationForm = _modalManager.getModal().find('form[name=TecnicoInformationsForm]');
            _$tecnicoInformationForm.validate({ ignore: "" });
            //$('.modal-dialog').css('width', '1100px');
            //$('select').addClass('form-control edited');
        };

        this.save = function () {
            if (!_$tecnicoInformationForm.valid()) {
                return;
            }

            var tecnico = _$tecnicoInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);

            _tecnicoesService.criarOuEditar(tecnico)
                .done(function () {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    _modalManager.close();
                    abp.event.trigger('app.CriarOuEditarTecnicoModalSaved');
                })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);