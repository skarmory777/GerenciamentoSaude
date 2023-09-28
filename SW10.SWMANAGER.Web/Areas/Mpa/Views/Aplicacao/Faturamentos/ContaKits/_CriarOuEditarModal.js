(function ($) {
    app.modals.CriarOuEditarContaKitModal = function () {
        var _contakitsService = abp.services.app.faturamentoContaKit;
        var _modalManager;
        var _$contakitsInformationForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$contaKitInformationForm = _modalManager.getModal().find('form[name=ContaKitInformationsForm]');
            _$contaKitInformationForm.validate();
            $('.modal-dialog:last').css('width', '700px');
            $('.modal-dialog:last').css('top', '65px');

            $('#div-aba-honorarios').hide();

            // Botoes de Percentual
            $("#btn-100-perc").on('click', function (e) {
                $('#percentual').val(100);
            });
            $("#btn-70-perc").on('click', function (e) {
                $('#percentual').val(70);
            });
            $("#btn-50-perc").on('click', function (e) {
                $('#percentual').val(50);
            });
        };

        this.save = function () {
            if (!_$contaKitInformationForm.valid()) {
                return;
            }

           
            var contaKit = _$contaKitInformationForm.serializeFormToObject();

            contaKit.faturamentoContaId = $('#conta-id').val();

            contaKit.medicoId = $('#medico').val();
            
            _modalManager.setBusy(true);

            _contakitsService.criarOuEditar(contaKit)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarContaKitModalSaved');
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };



       

        selectSW('.selectMedico', "/api/services/app/medico/ListarDropdown");

    };    
})(jQuery);