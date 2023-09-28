(function ($) {
    app.modals.CriarOuEditarTipoPrescricaoModal = function () {

        var _divisoesService = abp.services.app.divisao;

        var _modalManager;
        var _$TipoPrescricaoInformationForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;
            _$TipoPrescricaoInformationForm = _modalManager.getModal().find('form[name=TipoPrescricaoInformationsForm]');
            _$TipoPrescricaoInformationForm.validate();
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
            //$('.modal-dialog').css({ 'width': '90%', 'max-width': '990px' });
        };

        this.save = function () {
            if (!_$TipoPrescricaoInformationForm.valid()) {
                return;
            }
            var tipoPrescricao = _$TipoPrescricaoInformationForm.serializeFormToObject();
            _modalManager.setBusy(true);
            _divisoesService.criarOuEditarTipoPrescricao(tipoPrescricao)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarDivisaoModalSaved');
                     abp.event.trigger('app.CriarOuEditarTipoPrescricaoModalSaved');
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
       
    };
})(jQuery);