(function ($) {
    app.modals.CriarOuEditarModeloAtestadoModal = function () {

        var _ModelosModeloAtestadosService = abp.services.app.modeloAtestado;

        var _modalManager;
        var _$ModeloAtestadoInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$ModeloAtestadoInformationForm = _modalManager.getModal().find('form[name=ModeloAtestadoInformationsForm]');
            _$ModeloAtestadoInformationForm.validate();
            $('.modal-dialog').css({ 'width': '90%', 'max-width': '1100px' });
            $('ul.ui-autocomplete').css('z-index', '2147483647');
            var textarea = $('#conteudo');
            textarea.Editor();
            textarea.Editor('setText', textarea.text());
        };

        this.save = function () {
            var $textarea = $("#conteudo");
            $textarea.text($textarea.Editor("getText"));
            if (!_$ModeloAtestadoInformationForm.valid()) {
                return;
            }

            var modeloAtestado = _$ModeloAtestadoInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _ModelosModeloAtestadosService.criarOuEditar(modeloAtestado)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarModeloAtestadoModalSaved');
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);