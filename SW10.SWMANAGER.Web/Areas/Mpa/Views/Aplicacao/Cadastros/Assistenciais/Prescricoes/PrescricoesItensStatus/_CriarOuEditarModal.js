(function ($) {
    app.modals.CriarOuEditarPrescricaoItemStatusModal = function () {

        var _PrescricoesItensStatusService = abp.services.app.prescricaoItemStatus;

        var _modalManager;
        var _$PrescricaoItemStatusInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$PrescricaoItemStatusInformationForm = _modalManager.getModal().find('form[name=PrescricaoItemStatusInformationsForm]');
            _$PrescricaoItemStatusInformationForm.validate();
        };

        this.save = function () {
            if (!_$PrescricaoItemStatusInformationForm.valid()) {
                return;
            }

            var prescricaoItemStatus = _$PrescricaoItemStatusInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _PrescricoesItensStatusService.criarOuEditar(prescricaoItemStatus)
                 .done(function () {
                     abp.notify.success(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarPrescricaoItemStatusModalSaved');
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };

        $('.minhacor').minicolors({
            defaults: {
                animationSpeed: 50,
                animationEasing: 'swing',
                change: null,
                changeDelay: 0,
                control: 'hue',
                defaultValue: '',
                format: 'hex',
                hide: null,
                hideSpeed: 100,
                inline: false,
                keywords: '',
                letterCase: 'lowercase',
                opacity: false,
                position: 'bottom left',
                show: null,
                showSpeed: 100,
                theme: 'default',
                swatches: []
            }
        });

    };
})(jQuery);