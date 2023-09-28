(function ($) {
    app.modals.CriarOuEditarLeitoStatusModal = function () {

        var _leitosStatusService = abp.services.app.leitoStatus;
        var _modalManager;
        var _$leitoStatusInformationForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$leitoStatusInformationForm = _modalManager.getModal().find('form[name=LeitoStatusInformationsForm]');
            _$leitoStatusInformationForm.validate({ ignore: "" });
            //$('.modal-dialog').css('width', '1100px');
            //$('select').addClass('form-control edited');
        };

        this.save = function () {
            if (!_$leitoStatusInformationForm.valid()) {
                return;
            }

            var leitoStatus = _$leitoStatusInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);

            _leitosStatusService.criarOuEditar(leitoStatus)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarLeitoStatusModalSaved');
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };

        // Color Picker
        $('#cor-leito-status').minicolors({
            control: $('#cor-leito-status').attr('data-control') || 'hue',
            defaultValue: $('#cor-leito-status').attr('data-defaultValue') || '',
            format: $('#cor-leito-status').attr('data-format') || 'hex',
            keywords: $('#cor-leito-status').attr('data-keywords') || '',
            inline: $('#cor-leito-status').attr('data-inline') === 'true',
            letterCase: $('#cor-leito-status').attr('data-letterCase') || 'lowercase',
            opacity: $('#cor-leito-status').attr('data-opacity'),
            position: $('#cor-leito-status').attr('data-position') || 'bottom left',
            swatches: $('#cor-leito-status').attr('data-swatches') ? $(this).attr('data-swatches').split('|') : [],
            change: function (value, opacity) {
                if (!value) return;
                if (opacity) value += ', ' + opacity;
                if (typeof console === 'object') {
                    //console.log(value);
                }
                swatches: $('#cor-leito-status').addClass('edited')
            },
            theme: 'bootstrap'
        }).addClass('edited');
    };
})(jQuery);