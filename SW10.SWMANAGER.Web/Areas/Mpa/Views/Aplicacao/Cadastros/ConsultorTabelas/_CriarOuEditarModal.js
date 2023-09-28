(function ($) {

    app.modals.CriarOuEditarConsultorTabelaModal = function () {

        var _consultorTabelaService = abp.services.app.consultorTabela;
        var _modalManager;
        var _$ConsultorTabelaInformationForm = null;
        var _$consultorTabelaCamposInformationForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$ConsultorTabelaInformationForm = _modalManager.getModal().find('form[name=ConsultorTabelaInformationsForm]');

            //console.log('pos form');

            _$ConsultorTabelaInformationForm.validate();

            //console.log('pos validate');

            atualizarTabela1();

            //console.log('pos atualizar tabela');

            $('.modal-dialog').css({ 'width': '90%', 'max-width': '1100px' });
            $('.chosen-select').chosen({ no_results_text: app.localize("NotFound"), width: '100%' });
        };

        this.save = function () {
            if (!_$ConsultorTabelaInformationForm.valid()) {
                abp.notify.info(app.localize('ErroSalvar'));
                return;
            }

            var consultorTabela = _$ConsultorTabelaInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);

            _consultorTabelaService.criarOuEditar(consultorTabela)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarConsultorTabelaModalSaved');
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };

        function atualizarTabela1() {
            $('#ConsultorTabelaCamposTable').load('/ConsultorTabelas/_ConsultorTabelaCampos?id=' + $('#id').val());
            $('#consultor-tabela-campo-list').html('').load('/ConsultorTabelas/_CriarOuEditarConsultorTabelaCamposModal?consultorTabelaId=' + $('#id').val());
        }

        $('#btn-novo-consultor-tabela-campo').click(function (e) {
            e.preventDefault()
            $('#tabela-dominio-versao-tiss-parcial').load('/ConsultorTabelas/_CriarOuEditarConsultorTabelaCamposModal?consultorTabelaId=' + $('#id').val());
        });
    };
})(jQuery);
