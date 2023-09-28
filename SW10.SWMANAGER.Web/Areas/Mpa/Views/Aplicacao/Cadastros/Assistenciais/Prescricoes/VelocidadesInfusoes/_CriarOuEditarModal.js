(function ($) {
    app.modals.CriarOuEditarVelocidadeInfusaoModal = function () {

        var _VelocidadesInfusoesService = abp.services.app.velocidadeInfusao;

        var _modalManager;
        var _$VelocidadeInfusaoInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$VelocidadeInfusaoInformationForm = _modalManager.getModal().find('form[name=VelocidadeInfusaoInformationsForm]');
            _$VelocidadeInfusaoInformationForm.validate();


        };

        this.save = function () {
            if (!_$VelocidadeInfusaoInformationForm.valid()) {
                return;
            }

            var velocidadeInfusao = _$VelocidadeInfusaoInformationForm.serializeFormToObject();
            velocidadeInfusao.formaAplicacao = [];

            $(".forma-aplicacao").filter(":checked").each(function () {
                velocidadeInfusao.formaAplicacao.push({
                    id: $(this).data("relationId") ?? 0,
                    formaApplicacaoId: $(this).val(),
                    velocidadeInfusaoId: velocidadeInfusao.Id ?? 0
                });
            })

            debugger;

            _modalManager.setBusy(true);
            _VelocidadesInfusoesService.criarOuEditar(velocidadeInfusao)
                .done(function () {
                    abp.notify.success(app.localize('SavedSuccessfully'));
                    _modalManager.close();
                    abp.event.trigger('app.CriarOuEditarVelocidadeInfusaoModalSaved');
                })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);