(function ($) {
    app.modals.CriarOuEditarMeioPagamentoModal = function () {

        $(document).ready(function () {
            CamposRequeridos();
            $('#taxaAdministracao').mask('000,00', { reverse: true });
        });

        var _meioPagamentoService = abp.services.app.meioPagamento;

        var _modalManager;
        var _$meioPagamentoInformationsForm = null;


        var _ErrorModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Erros/ExibirErros',
        });


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$meioPagamentoInformationsForm = $('form[name=meioPagamentoInformationsForm]');
            _$meioPagamentoInformationsForm.validate();
        };

        this.save = function () {


           
            if (!_$meioPagamentoInformationsForm.valid()) {
                return;
            }

            var meioPagamento = _$meioPagamentoInformationsForm.serializeFormToObject();
            meioPagamento.TaxaAdministracao = retirarMascara(meioPagamento.TaxaAdministracao);


            _modalManager.setBusy(true);
            _meioPagamentoService.criarOuEditar(meioPagamento)
                 .done(function (data) {
                     if (data.errors.length > 0) {
                         _ErrorModal.open({ erros: data.errors });
                     }
                     else {

                         abp.notify.info(app.localize('SavedSuccessfully'));
                         _modalManager.close();
                         abp.event.trigger('app.CriarOuEditarFeriadoModalSaved');
                         //location.reload();//seguindo o projeto pronto
                     }
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };

     
        selectSW('.selectTipoMeioPagamento', "/api/services/app/TipoMeioPagamento/ListarDropdown");
  
        function retirarMascara(_valor) {
            var valor = _valor.toString();
            while (valor.indexOf('.') != -1) valor = valor.replace('.', '');

            valor = valor.replace(',', '.');
            return valor;
        }
    };
})(jQuery);