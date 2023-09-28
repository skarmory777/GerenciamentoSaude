(function ($) {
    app.modals.CriarOuEditarTipoControleModal = function () {
        localStorage["FecharModal"] = false;
        var _tipoControleService = abp.services.app.tipoControle;
        var _modalManager;
        var _$formTipoControle = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;
            _$formTipoControle = _modalManager.getModal().find('form[name=TipoControleInformationsForm]');
            _$formTipoControle.validate();
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
            $('.modal-dialog').css({ 'width': '90%', 'max-width': '990px' });
        };

        this.save = function () {
            if (!_$formTipoControle.valid()) {
                return;
            }
            var tipoControle = _$formTipoControle.serializeFormToObject();
            _modalManager.setBusy(true);
            _tipoControleService.criarOuEditar(tipoControle)
                 .done(function (data) {
                     $('#tipo-resposta-id').val(data.id);
                     abp.notify.success(app.localize('SavedSuccessfully'));
                     abp.event.trigger('app.CriarOuEditarTipoControleModalSaved');
                     _modalManager.close();
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };

    };
})(jQuery);