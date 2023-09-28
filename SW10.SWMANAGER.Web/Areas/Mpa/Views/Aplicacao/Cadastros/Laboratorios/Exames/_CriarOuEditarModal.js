(function ($) {
    app.modals.CriarOuEditarExameModal = function () {
        var _examesService = abp.services.app.exame;

        var _modalManager;
        var _$ExamesInformationForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$ExameInformationForm = _modalManager.getModal().find('form[name=ExameInformationsForm]');
            _$ExameInformationForm.validate();
            $('.modal-dialog').css({ 'width': '90%', 'max-width': '1100px' });
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
            $('.select2').css('width', '100%');
        };

        this.save = function () {
            if (!_$ExameInformationForm.valid()) {
                return;
            }
            var Exame = _$ExameInformationForm.serializeFormToObject();
            _modalManager.setBusy(true);
            _examesService.editar(Exame)
                .done(function () {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    _modalManager.close();
                    abp.event.trigger('app.CriarOuEditarExameModalSaved');
                })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };

        selectSW('.selectMaterial', "/api/services/app/material/ListarDropdown");
        selectSW('.selectFormata', "/api/services/app/formata/ListarDropdown");
        selectSW('.lab-exame-select2', "/api/services/app/exame/ListarDropdown");
        selectSW('.lab-unidade-select2', "/api/services/app/laboratoriounidade/ListarDropdown");
        aplicarSelect2Padrao();

        
    };
})(jQuery);