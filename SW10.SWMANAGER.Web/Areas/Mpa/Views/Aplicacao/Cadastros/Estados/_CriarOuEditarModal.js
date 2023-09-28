(function ($) {
    app.modals.CriarOuEditarEstadoModal = function () {

        var _estadosService = abp.services.app.estado;

        var _modalManager;
        var _$estadosInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$estadoInformationForm = _modalManager.getModal().find('form[name=EstadoInformationsForm]');
            _$estadoInformationForm.validate();
            //$('.modal-dialog').css('width', '1100px');
            //$('select').addClass('form-control edited');
        };

        this.save = function () {
            if (!_$estadoInformationForm.valid()) {
                return;
            }

            var estado = _$estadoInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);

            _estadosService.criarOuEditar(estado)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarEstadoModalSaved');
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
        aplicarDateRange();
        aplicarDateSingle();
        aplicarSelect2Padrao();
        CamposRequeridos();
    };
})(jQuery);