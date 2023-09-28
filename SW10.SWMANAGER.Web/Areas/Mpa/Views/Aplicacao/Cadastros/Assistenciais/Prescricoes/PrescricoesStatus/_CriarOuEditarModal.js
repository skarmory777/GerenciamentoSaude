(function ($) {
    app.modals.CriarOuEditarPrescricaoStatusModal = function () {

        var _PrescricoesStatusService = abp.services.app.prescricaoStatus;

        var _modalManager;
        var _$PrescricaoStatusInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$PrescricaoStatusInformationForm = _modalManager.getModal().find('form[name=PrescricaoStatusInformationsForm]');
            _$PrescricaoStatusInformationForm.validate();
        };

        this.save = function () {
            if (!_$PrescricaoStatusInformationForm.valid()) {
                return;
            }

            var prescricaoStatus = _$PrescricaoStatusInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _PrescricoesStatusService.criarOuEditar(prescricaoStatus)
                 .done(function () {
                     abp.notify.success(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarPrescricaoStatusModalSaved');
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