//(function ($) {
//    app.modals.CriarOuEditarConsultorTabelaCampoModal = function () {
//        var _consultorTabelaCamposService = abp.services.app.consultorTabelaCampo;
//        var _modalManager;
//        var _$consultorTabelaCampoInformationForm = null;

//        this.init = function (modalManager) {
//            _modalManager = modalManager;
//            _$consultorTabelaCampoInformationForm = _modalManager.getModal().find('form[name=ConsultorTabelaCampoInformationsForm]');
//            _$consultorTabelaCampoInformationForm.validate({ ignore: "" });
//        };

//        this.save = function () {
//            if (!_$consultorTabelaCampoInformationForm.valid()) {
//                return;
//            }

//            var consultorTabelaCampo = _$consultorTabelaCampoInformationForm.serializeFormToObject();
//            _modalManager.setBusy(true);

//            _consultorTabelaCamposService.criarOuEditar(consultorTabelaCampo)
//                 .done(function () {
//                     abp.notify.info(app.localize('SavedSuccessfully'));
//                     _modalManager.close();
//                     abp.event.trigger('app.CriarOuEditarConsultorTabelaCampoModalSaved');
//                 })
//                .always(function () {
//                    _modalManager.setBusy(false);
//                });
//        };
//    };
//})(jQuery);