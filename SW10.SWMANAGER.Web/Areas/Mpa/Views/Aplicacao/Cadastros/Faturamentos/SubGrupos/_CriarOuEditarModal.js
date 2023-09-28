(function ($) {
    app.modals.CriarOuEditarFaturamentoSubGrupoModal = function () {

        var _subGruposService = abp.services.app.faturamentoSubGrupo;
        var _modalManager;
        var _$subGrupoInformationForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$subGrupoInformationForm = _modalManager.getModal().find('form[name=SubGrupoInformationsForm]');
            _$subGrupoInformationForm.validate({ ignore: "" });
            $('.modal-dialog:last').css('width', '500px');
            $('.modal-dialog:last').css('top', '150px');
        };

        this.save = function () {
            if (!_$subGrupoInformationForm.valid()) {
                return;
            }

            var subGrupo = _$subGrupoInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);

            _subGruposService.criarOuEditar(subGrupo)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarSubGrupoModalSaved');
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);