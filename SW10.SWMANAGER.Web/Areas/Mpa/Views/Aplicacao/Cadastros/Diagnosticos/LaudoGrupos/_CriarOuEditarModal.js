(function ($) {
    app.modals.CriarOuEditarLaudoGrupoModal = function () {

        var _ModelosLaudoGruposService = abp.services.app.laudoGrupo;

        var _modalManager;
        var _$LaudoGrupoInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$LaudoGrupoInformationForm = _modalManager.getModal().find('form[name=LaudoGrupoInformationsForm]');
            _$LaudoGrupoInformationForm.validate();
            $('.modal-dialog:last').css({ 'width': '70%', 'max-width': '800px' });
         
        };

        this.save = function () {
       
            if (!_$LaudoGrupoInformationForm.valid()) {
                return;
            }

            var laudoGrupo = _$LaudoGrupoInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _ModelosLaudoGruposService.criarOuEditar(laudoGrupo)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarLaudoGrupoModalSaved');
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };

        selectSW('.selectModalidade', "/api/services/app/Modalidade/ListarDropdown");

        


    };
})(jQuery);