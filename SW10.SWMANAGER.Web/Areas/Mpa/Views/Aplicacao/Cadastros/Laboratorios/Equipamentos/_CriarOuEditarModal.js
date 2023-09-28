(function ($) {
    app.modals.CriarOuEditarEquipamentoModal = function () {
        var _equipamentosService = abp.services.app.equipamento;

        var _modalManager;
        var _$EquipamentosInformationForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$EquipamentoInformationForm = _modalManager.getModal().find('form[name=EquipamentoInformationsForm]');
            _$EquipamentoInformationForm.validate();
        };

        this.save = function () {
            if (!_$EquipamentoInformationForm.valid()) {
                return;
            }

            var Equipamento = _$EquipamentoInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);

            _equipamentosService.criarOuEditar(Equipamento)
                .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarEquipamentoModalSaved');
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);