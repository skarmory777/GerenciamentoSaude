(function ($) {
    app.modals.CriarOuEditarItemResultadoModal = function () {
        var _itemResultadosService = abp.services.app.itemResultado;

        var _modalManager;
        var _$ItemResultadosInformationForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$ItemResultadoInformationForm = _modalManager.getModal().find('form[name=ItemResultadoInformationsForm]');
            _$ItemResultadoInformationForm.validate();

            $('.modal-dialog').css({ 'width': '90%', 'max-width': '1100px' });
            //Forçando o modal a aceitar sobreposição por causa dos selects do datarangepicker
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
        };

        this.save = function () {
            if (!_$ItemResultadoInformationForm.valid()) {
                return;
            }

           
            var ItemResultado = _$ItemResultadoInformationForm.serializeFormToObject();

            if (ItemResultado.MinimoAceitavelMasculino) {
                ItemResultado.MinimoAceitavelMasculino = ItemResultado.MinimoAceitavelMasculino.replace(',', '.');
            }
            if (ItemResultado.MaximoAceitavelMasculino) {
                ItemResultado.MaximoAceitavelMasculino = ItemResultado.MaximoAceitavelMasculino.replace(',', '.');
            }
            if (ItemResultado.MinimoMasculino) {
                ItemResultado.MinimoMasculino = ItemResultado.MinimoMasculino.replace(',', '.');
            }
            if (ItemResultado.MaximoMasculino) {
                ItemResultado.MaximoMasculino = ItemResultado.MaximoMasculino.replace(',', '.');
            }
            //ItemResultado.NormalMasculino = ItemResultado.NormalMasculino.replace(',', '.');


            if (ItemResultado.MinimoAceitavelFeminino) {
                ItemResultado.MinimoAceitavelFeminino = ItemResultado.MinimoAceitavelFeminino.replace(',', '.');
            }
            if (ItemResultado.MaximoAceitavelFeminino) {
                ItemResultado.MaximoAceitavelFeminino = ItemResultado.MaximoAceitavelFeminino.replace(',', '.');
            }
            if (ItemResultado.MinimoFeminino) {
                ItemResultado.MinimoFeminino = ItemResultado.MinimoFeminino.replace(',', '.');
            }
            if (ItemResultado.MaximoFeminino) {
                ItemResultado.MaximoFeminino = ItemResultado.MaximoFeminino.replace(',', '.');
            }
            //ItemResultado.NormalFeminino = ItemResultado.NormalFeminino.replace(',', '.');



            _modalManager.setBusy(true);

            _itemResultadosService.criarOuEditar(ItemResultado)
                .done(function () {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    _modalManager.close();
                    abp.event.trigger('app.CriarOuEditarItemResultadoModalSaved');
                })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };

        aplicarSelect2Padrao();
        selectSW('.selectUnidade', "/api/services/app/laboratorioUnidade/listarDropdown");
        //selectSW('.selectTipoResultado', "/api/services/app/tiporesultado/listarDropdown");
        selectSW('.selectTabela', "/api/services/app/tabela/listarDropdown");
        
        $('#cbo-tiporesultado').on('change', function () {
            if ($(this).val() == 4) {
                $('#div-tabela-resultado').removeClass('hidden');
            }
            else {
                $('#div-tabela-resultado').addClass('hidden');
            }
        });

        $('#chk-is-TamFixo').on('click', function () {
            if ($(this).is(':checked')) {
                $('#tamanho-fixo').removeAttr('readonly')
            }
            else {
                $('#tamanho-fixo').attr('readonly','readonly')
            }
        })
        $('#chk-is-Interface').on('click', function () {
            if ($(this).is(':checked')) {
                $('#interface').removeAttr('readonly');
                $('#interface-envio').removeAttr('readonly');
                $('#equipamento-id').removeAttr('disabled');
                $('#divide-inter').removeAttr('readonly');

            }
            else {
                $('#interface').attr('readonly', 'readonly');
                $('#interface-envio').attr('readonly', 'readonly');
                $('#equipamento-id').attr('disabled', 'disabled')
                $('#divide-inter').attr('readonly','readonly');
            }
        })
    };
})(jQuery);