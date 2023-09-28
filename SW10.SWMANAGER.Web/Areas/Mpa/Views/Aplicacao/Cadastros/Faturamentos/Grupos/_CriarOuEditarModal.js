(function ($) {
    app.modals.CriarOuEditarFaturamentoGrupoModal = function () {

        var _gruposService = abp.services.app.faturamentoGrupo;
        var _modalManager;
        var _$grupoInformationForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$grupoInformationForm = _modalManager.getModal().find('form[name=GrupoInformationsForm]');
            _$grupoInformationForm.validate({ ignore: "" });
            $('.modal-dialog:last').css('width', '800px');
            $('.modal-dialog:last').css('top', '100px');
        };

        this.save = function () {
            if (!_$grupoInformationForm.valid()) {
                return;
            }

            var grupo = _$grupoInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);

            _gruposService.criarOuEditar(grupo)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarGrupoModalSaved');
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);