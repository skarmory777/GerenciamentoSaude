(function ($) {
    app.modals.CriarOuEditarFaturamentoSisMoedaModal = function () {

        var _sisMoedasService = abp.services.app.sisMoeda;
        var _modalManager;
        var _$sisMoedaInformationForm = null;

        $(document).ready(function () {
            $('#cotacao-valor').mask('000.000,00', { reverse: true });
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$sisMoedaInformationForm = _modalManager.getModal().find('form[name=SisMoedaInformationsForm]');
            _$sisMoedaInformationForm.validate({ ignore: "" });
            $('.modal-dialog').css('width', '800px');
        };

        this.save = function () {
            if (!_$sisMoedaInformationForm.valid()) {
                return;
            }

            var sisMoeda = _$sisMoedaInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);

            _sisMoedasService.criarOuEditar(sisMoeda)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarSisMoedaModalSaved');
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);