(function ($) {
    app.modals.ConferenciaModal = function () {
        var _contasMedicasService = abp.services.app.conta;
        var _modalManager;
        var _$contaMedicaInformationForm = null;
        var _self = this;

        this.init = function (modalManager) {
            _modalManager = modalManager;
            _$contaMedicaInformationForm = _modalManager.getModal().find('form[name=ContaMedicaInformationsForm]');
            _$contaMedicaInformationForm.validate({ ignore: "" });
            $('.modal-dialog:last').css('width', '1250px');
            ModuloFaturamento.gerarForm(_$contaMedicaInformationForm);

            // Entrega de contas

            $('#btn-confirma-conferencia').on('click', function (e) {
                e.preventDefault();
                // StatusId 2 = 'Conferida'
                $('#confirma-conferencia').swSetCampo('2');
                _self.save();
            });

            $('#btn-declara-pendencia').on('click', function (e) {
                e.preventDefault();
                $('#div-motivo-pendencia').show();
                $('#motivo-pendencia').select();
            });

            $('#btn-confirma-pendencia').on('click', function (e) {
                e.preventDefault();
                // StatusId 5 = 'Pendente'
                $('#confirma-conferencia').swSetCampo('5');
                _self.save();
            });

            // Fim - entrega de contas
        };

        this.save = function () {
            if (!_$contaMedicaInformationForm.valid()) {
                return;
            }
            var contaMedica = _$contaMedicaInformationForm.serializeFormToObject();

            contaMedica.MotivoPendencia = $('#motivo-pendencia').val();

            //_contasMedicasService.obterUsuarioLogadoAsync()
            //    .done(function (data) {
            //        debugger
            //        contaMedica.UsuarioConferenciaId = data.id;
            //    })
            

            if (!contaMedica.AtendimentoId || contaMedica.AtendimentoId == 0 || contaMedica.AtendimentoId == '0') {
                abp.notify.warn(app.localize('NenhumAtendimentoSelecionado'));
                return;
            }

            _modalManager.setBusy(true);

            _contasMedicasService.editarComUsuarioConferencia(contaMedica)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarContaMedicaModalSaved');
                     abp.event.trigger('app.ConferenciaModalSaved');
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
        
    };
})(jQuery);