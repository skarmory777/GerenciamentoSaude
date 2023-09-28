(function ($) {
    app.modals.CriarOuEditarPreAtendimentoModal = function () {

        var _modalManager;
        var _$preAtendimentoInformationForm = null;

        var _AtendimentosService = abp.services.app.atendimento;

        //console.log("focus");
        $('#atendimentoTipo').focus();

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$preAtendimentoInformationForm = _modalManager.getModal().find('form[name=PreAtendimentoInformationsForm]');
            $('.modal-dialog').css({ 'width': '800%', 'max-width': '1320px' });
            _$preAtendimentoInformationForm.validate({ ignore: "" });
        };

        this.save = function () {
            debugger;
            if (!_$preAtendimentoInformationForm.valid()) {
                return;
            }

            var preAtendimento = _$preAtendimentoInformationForm.serializeFormToObject();

            preAtendimento.isInternacao = $('#isInternados').is(':checked');
            preAtendimento.isAmbulatorioEmergencia = $('#isAmbulatorioEmergencia').is(':checked');

            _modalManager.setBusy(true);

            _AtendimentosService.criarOuEditar(preAtendimento)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarPreAtendimentoModalSaved');
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };

        selectSW('.selectConvenio', "/api/services/app/Convenio/ListarDropdown");
        selectSW('.selectPlano', "/api/services/app/Plano/ListarDropdown", $('#convenioId'));


        $('#convenioId').change(function (e) {
            selectSW('.selectPlano', "/api/services/app/Plano/ListarDropdown", $('#convenioId'));
        }
        );

        var unidade = $('#isAmbulatorioEmergencia').is(':checked') ? 'ambEmr' : 'inter'

        selectSW('.selectUnidadeOrganizacional', "/api/services/app/UnidadeOrganizacional/ListarDropdownPorUsuario", { valor: unidade });

        selectSWMultiplosFiltros('.select2AtendimentoTipo', "/api/services/app/TipoAtendimento/ListarDropdown", [{ valor: $('#isAmbulatorioEmergencia').is(':checked') }, { valor: $('#isInternados').is(':checked') }]);

        $('.destino').change(function (e)
        {
            e.preventDefault();
            debugger;
            var unidade = $('#isAmbulatorioEmergencia').is(':checked') ? "ambEmr" : "inter"

            selectSW('.selectUnidadeOrganizacional', "/api/services/app/UnidadeOrganizacional/ListarDropdownPorUsuario", { valor: unidade });

            selectSWMultiplosFiltros('.select2AtendimentoTipo', "/api/services/app/TipoAtendimento/ListarDropdown", [{ valor: $('#isAmbulatorioEmergencia').is(':checked') }, { valor: $('#isInternados').is(':checked') }]);


            if ($('#isAmbulatorioEmergencia').is(':checked')) {
                $('#divLeito').hide();

                $('#leitoId').val(null).trigger("change")
                $('#leitoId').attr('required', false);
            }
            else
            {
                $('#divLeito').show();
                $('#leitoId').attr('required', true);
                required

            }
        });
        


        selectSW('.selectTipoLocalChamada', "/api/services/app/TipoLocalChamada/ListarTipoLocalChamadaDropdown");
        selectSW('.selectLocalChamada', "/api/services/app/LocalChamadas/ListarLocalChamadaPorTipoDropdown", $('#tipoLocalChamadaId'));
        selectSWMultiplosFiltros('.selectSenha', "/api/services/app/Senha/ListarSenhasPorlocalChamadaAtendimentoDropdown", [{ valor: $('#localChamadaId').val() }, { valor: $('#id').val() }]);
        selectSW('.selectProximoTipoLocalChamada', "/api/services/app/TipoLocalChamada/ListarTipoLocalChamadaDropdown");

        selectSW('.select2Leito', "/api/services/app/Leito/ListarDropdown");
        

        $('#tipoLocalChamadaId').on('change', function (e) {
            e.preventDefault();
            $('#localChamadaId').val('').trigger('change');
            selectSW('.selectLocalChamada', "/api/services/app/LocalChamadas/ListarLocalChamadaPorTipoDropdown", $('#tipoLocalChamadaId'));
        });

        $('#localChamadaId').change(function (e) {
            e.preventDefault();
            $('#senhaId').val('').trigger('change');


            // selectSW('.selectSenha', "/api/services/app/Senha/ListarSenhasPorlocalChamadaDropdown", $('#@localChamadaId'));
            //selectSWMultiplosFiltros('.selectSenha', "/api/services/app/Senha/ListarSenhasPorlocalChamadaAtendimentoDropdown", values:{ $('@localChamadaId').val(), formData[0].Id});
            selectSWMultiplosFiltros('.selectSenha', "/api/services/app/Senha/ListarSenhasPorlocalChamadaAtendimentoDropdown", [{ valor: $('#localChamadaId').val() }, { valor: $('#id').val() }]);
        });

        var _terminalSenhasService = abp.services.app.terminalSenhas;

        $('#senhaBtn').on('click', function (e) {
            e.preventDefault();
            _terminalSenhasService.chamarSenha($('#tipoLocalChamadaId').val(), $('#localChamadaId').val(), $('#senhaId').val());
            $.cookie('localChamada', $('#localChamadaId').val());
        });




    };
})(jQuery);